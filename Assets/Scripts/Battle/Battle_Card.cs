using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utill;

public class Battle_Card : BattleCommand
{
    private int max_Card = 3;
    private int cur_Card = 0;
    private float cardDelay;
    private float summonRange;
    private float summonRangeDelay = 30f;
    private LineRenderer summonRangeLine;
    private DeckData deckData;
    private UnitDataSO unitDataSO;
    private StarategyDataSO starategyDataSO;
    private StageData stageData;
    private GameObject cardMove_Prefeb;
    private Transform card_PoolManager;
    private Transform card_Canvas;
    private RectTransform card_LeftPosition;
    private RectTransform card_RightPosition;
    private RectTransform card_SpawnPosition;

    private GameObject unit_AfterImage;
    private SpriteRenderer unit_AfterImage_Spr;

    private Coroutine coroutine;

    private CardMove selectCard;
    
    public bool isCardDown { get; private set; } //ī�带 Ŭ���� ��������
    public bool isPossibleSummon { get; private set; } //�ش� ī�带 ��ȯ�� �� �ִ���

    private int cardidCount = 0;
    private int unitidCount = 0;

    public Battle_Card(BattleManager battleManager, DeckData deckData, UnitDataSO unitDataSO, StarategyDataSO starategyDataSO, GameObject card_Prefeb, Transform card_PoolManager, Transform card_Canvas, RectTransform card_SpawnPosition, RectTransform card_LeftPosition, RectTransform card_RightPosition, GameObject unit_AfterImage, LineRenderer summonRangeLine)
        : base(battleManager)
    {
        this.deckData = deckData;
        this.unitDataSO = unitDataSO;
        this.starategyDataSO = starategyDataSO;
        this.cardMove_Prefeb = card_Prefeb;
        this.card_PoolManager = card_PoolManager;
        this.card_Canvas = card_Canvas;
        this.card_SpawnPosition = card_SpawnPosition;
        this.card_RightPosition = card_RightPosition;
        this.card_LeftPosition = card_LeftPosition;

        this.stageData = battleManager.currentStageData;
        this.summonRange = -stageData.max_Range + stageData.max_Range / 4;
        this.summonRangeLine = summonRangeLine;
        Set_SummonRangeLinePos();

        this.unit_AfterImage = unit_AfterImage;
        unit_AfterImage_Spr = unit_AfterImage.GetComponent<SpriteRenderer>();

        Set_DeckCard();
    }

    /// <summary>
    /// ���� ī�� �������� �ִ´�(�ӽ�)
    /// </summary>
    public void Set_DeckCard()
    {
        for(int i = 0; i < unitDataSO.unitDatas.Count; i++)
        {
            deckData.Add_CardData(unitDataSO.unitDatas[i]);
        }
        for (int i = 0; i < starategyDataSO.starategyDatas.Count; i++)
        {
            deckData.Add_CardData(starategyDataSO.starategyDatas[i]);
        }
    }

    /// <summary>
    /// �ִ� ������� ī�带 �̴´�
    /// </summary>
    public void Add_AllCard()
    {
        for (; cur_Card < max_Card;)
        {
            Add_OneCard();
        }
    }

    /// <summary>
    /// ī�� ������ �̴´�
    /// </summary>
    public void Add_OneCard()
    {
        int random = Random.Range(0, deckData.cardDatas.Count);
        cur_Card++;

        CardMove cardmove = Pool_Card();
        cardmove.Set_UnitData(deckData.cardDatas[random], cardidCount++);
        battleManager.card_DatasTemp.Add(cardmove);

        Sort_Card();
        Fusion_DelayCard();
    }

    private IEnumerator Delay_Drow()
    {
        yield return new WaitForSeconds(0.2f);
        Fusion_Card();
    }


    /// <summary>
    /// ī�带 Ǯ����
    /// </summary>
    private CardMove Pool_Card()
    {
        GameObject cardmove_obj = null;
        if (card_PoolManager.childCount > 0)
        {
            cardmove_obj = card_PoolManager.GetChild(0).gameObject;
            cardmove_obj.transform.position = card_SpawnPosition.position;
            cardmove_obj.SetActive(true);
        }
        cardmove_obj ??= battleManager.Create_Object(cardMove_Prefeb, card_SpawnPosition.position, Quaternion.identity);
        cardmove_obj.transform.SetParent(card_Canvas);
        return cardmove_obj.GetComponent<CardMove>();
    }

    /// <summary>
    /// ī�� ��ġ�� ������
    /// </summary>
    public void Sort_Card()
    {
        List<PRS> originCardPRS = new List<PRS>();
        originCardPRS = Return_RoundPRS(battleManager.card_DatasTemp.Count, 800, 600);

        for (int i = 0; i < battleManager.card_DatasTemp.Count; i++)
        {
            CardMove targetCard = battleManager.card_DatasTemp[i];
            targetCard.originPRS = originCardPRS[i];
            if (battleManager.card_DatasTemp[i].Equals(selectCard))
                continue;
            if (battleManager.card_DatasTemp[i].isFusion)
                continue;
            targetCard.Set_CardPRS(targetCard.originPRS, 0.5f);
        }
    }

    /// <summary>
    /// ī�� ��ġ�� �������� ��ȯ��
    /// </summary>
    /// <param name="objCount">ī���� ����</param>
    /// <param name="y_Space">ī�庰 y ����</param>
    /// <param name="std_y_Pos">ī��� ��ġ���� �ش� ������ŭ y��ġ�� ��</param>
    /// <returns></returns>
    private List<PRS> Return_RoundPRS(int objCount, float y_Space, float std_y_Pos)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);

        switch (objCount)
        {
            case 1:
                objLerps = new float[] { 0.5f };
                break;
            case 2:
                objLerps = new float[] { 0.27f, 0.77f };
                break;
            default:
                float interbal = 1f / (objCount - 1 > 0 ? objCount - 1 : 1);
                for (int i = 0; i < objCount; i++)
                {
                    objLerps[i] = interbal * i;
                }
                break;
        }


        for (int i = 0; i < objCount; i++)
        {
            Vector3 pos = Vector3.Lerp(card_LeftPosition.anchoredPosition, card_RightPosition.anchoredPosition, objLerps[i]);

            float curve = Mathf.Sqrt(Mathf.Pow(1, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
            pos.y += curve * y_Space - std_y_Pos;
            Quaternion rot = Quaternion.Slerp(card_LeftPosition.rotation, card_RightPosition.rotation, objLerps[i]);
            if (objCount <= 2)
            {
                rot = Quaternion.identity;
            }

            results.Add(new PRS(pos, rot, Vector3.one));
        }

        return results;
    }

    /// <summary>
    /// ī�带 ������
    /// </summary>
    private void Fusion_Card()
    {
        CardMove targetCard1 = null;
        CardMove targetCard2 = null;
        for (int i = 0; i < battleManager.card_DatasTemp.Count - 1; i++)
        {
            targetCard1 = battleManager.card_DatasTemp[i];
            targetCard2 = battleManager.card_DatasTemp[i + 1];
            if (targetCard1.dataBase.cardType != targetCard2.dataBase.cardType)
                continue;

            switch (targetCard1.dataBase.cardType)
            {
                default:
                case CardType.Execute:
                case CardType.SummonTrap:
                case CardType.Installation:
                    if (targetCard1.dataBase.strategyData.starategyType != targetCard2.dataBase.strategyData.starategyType)
                        continue;
                    break;
                case CardType.SummonUnit:
                    if (targetCard1.dataBase.unitData.unitType != targetCard2.dataBase.unitData.unitType)
                        continue;
                    break;
            }

            if (targetCard1.grade != targetCard2.grade)
                continue;

            coroutine = battleManager.StartCoroutine(Fusion_Move(i));
            return;
        }
    }

    /// <summary>
    /// ī�� ���� �ִϸ��̼�
    /// </summary>
    /// <param name="index">�� ��° ī�尡 �����ϴ���</param>
    /// <returns></returns>
    private IEnumerator Fusion_Move(int index)
    {
        battleManager.card_DatasTemp[index].isFusion = true;
        battleManager.card_DatasTemp[index + 1].isFusion = true;

        battleManager.card_DatasTemp[index + 1].Set_CardPRS(battleManager.card_DatasTemp[index].originPRS, 0.3f);
        battleManager.card_DatasTemp[index].Fusion_FadeInEffect();
        battleManager.card_DatasTemp[index + 1].Fusion_FadeInEffect();

        yield return new WaitForSeconds(0.3f);

        battleManager.card_DatasTemp[index].Fusion_FadeOutEffect();
        battleManager.card_DatasTemp[index].Upgrade_UnitGrade();

        battleManager.card_DatasTemp[index].isFusion = false;
        battleManager.card_DatasTemp[index + 1].isFusion = false;

        Subtract_CardAt(index + 1);
        Sort_Card();

        battleManager.StopCoroutine(coroutine);
        coroutine = battleManager.StartCoroutine(Delay_Drow());
    }

    /// <summary>
    /// ī�带 �̰ų� �����ϰ� ���� ���� �������� ����
    /// </summary>
    private void Fusion_DelayCard()
    {
        if (coroutine != null)
        {
            battleManager.StopCoroutine(coroutine);
        }

        coroutine = battleManager.StartCoroutine(Delay_Drow());
    }

    /// <summary>
    /// �ֱ� ���� ī�带 �����
    /// </summary>
    public void Subtract_Card()
    {
        Subtract_CardAt(cur_Card - 1);
    }

    /// <summary>
    /// ������ �ε����� ī�带 �����
    /// </summary>
    public void Subtract_CardAt(int index)
    {
        if (cur_Card.Equals(0))
            return;

        cur_Card--;
        battleManager.card_DatasTemp[index].transform.SetParent(card_PoolManager);
        battleManager.card_DatasTemp[index].gameObject.SetActive(false);
        battleManager.card_DatasTemp.RemoveAt(index);
        Sort_Card();

        Fusion_DelayCard();
    }

    /// <summary>
    /// ��� ī�带 �����
    /// </summary>
    public void Clear_Cards()
    {
        if (coroutine != null)
        {
            battleManager.StopCoroutine(coroutine);
        }

        for (; cur_Card > 0;)
        {
            Subtract_Card();
        }
    }

    /// <summary>
    /// ī�带 ������
    /// </summary>
    /// <param name="card"></param>
    public void Set_SelectCard(CardMove card)
    {
        summonRangeLine.gameObject.SetActive(true);
        if (card.isFusion) return;
        card.transform.DOKill();
        card.Set_CardScale(Vector3.one * 1.3f, 0.3f);
        card.Set_CardRot(Quaternion.identity, 0.3f);
        selectCard = card;
        isCardDown = true;
    }

    /// <summary>
    /// ������ ī�� ��ġ�� ������Ʈ �Ѵ�
    /// </summary>
    public void Update_SelectCardPos()
    {
        if (selectCard == null) 
            return;
        selectCard.transform.position = Input.mousePosition;
    }

    /// <summary>
    /// ī�� ������ �����
    /// </summary>
    /// <param name="card"></param>
    public void Set_UnSelectCard(CardMove card)
    {
        summonRangeLine.gameObject.SetActive(false);
        if (card.isFusion) return;
        card.Set_CardScale(Vector3.one * 1, 0.3f);
        selectCard = null;
        isCardDown = false;
    }

    /// <summary>
    /// ī�带 ����Ѵ�
    /// </summary>
    /// <param name="card"></param>
    public void Set_UseCard(CardMove card)
    {
        selectCard = null;
        summonRangeLine.gameObject.SetActive(false);
        
        Check_PossibleSummon();
        if (!isPossibleSummon)
        {
            card.Run_OriginPRS();
            battleManager.battle_Camera.Set_CameraIsMove(true);
            return; 
        }

        battleManager.battle_Cost.Subtract_Cost(card.card_Cost);
        Subtract_CardAt(battleManager.card_DatasTemp.FindIndex(x => x.id == card.id));
        isCardDown = false;

        //ī�� ���
        Vector3 mouse_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        switch (card.dataBase.cardType)
        {
            case CardType.SummonUnit:
                battleManager.battle_Unit.Summon_Unit(card.dataBase, new Vector3(mouse_Pos.x, 0, 0), unitidCount++);
                break;
            default:
            case CardType.Execute:
            case CardType.SummonTrap:
            case CardType.Installation:
                card.dataBase.strategyData.starategy_State.Run_Card(battleManager);
                break;
        }
        battleManager.ai_Log.Add_Log(card.dataBase, card.grade);

    }

    /// <summary>
    /// ���� ��ȯ �̸�����
    /// </summary>
    /// <param name="unitData"></param>
    /// <param name="pos"></param>
    /// <param name="isDelete"></param>
    public void Set_UnitAfterImage(Sprite unitSprite, Vector3 pos, bool isDelete = false)
    {
        unit_AfterImage_Spr.color = Color.white;
        if (!isPossibleSummon)
        {
            unit_AfterImage_Spr.color = Color.red;
        }
        unit_AfterImage.transform.position = new Vector3(pos.x, 0);
        unit_AfterImage_Spr.sprite = unitSprite;

        if (isDelete)
        {
            unit_AfterImage.SetActive(false);
            return;
        }
        unit_AfterImage.SetActive(true);

        return;
    }

    /// <summary>
    /// ī�带 ���� ���ǿ� ���� ����� �� �ִ��� üũ
    /// </summary>
    public void Check_PossibleSummon()
    {
        if (selectCard == null)
            return;
        //�׽�Ʈ�� ��ȯ ���� ����
        if(battleManager.isAnySummon)
        {
            isPossibleSummon = true;
            return;
        }
        if (battleManager.battle_Unit.eTeam.Equals(TeamType.EnemyTeam))
        {
            isPossibleSummon = true;
            return;
        }

        switch (selectCard.dataBase.cardType)
        {
            case CardType.Execute:
                break;
            case CardType.SummonUnit:
            case CardType.SummonTrap:
                Vector3 mouse_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (mouse_Pos.x < -stageData.max_Range || mouse_Pos.x > summonRange)
                {
                    isPossibleSummon = false;
                    return;
                }
                break;
            case CardType.Installation:
                break;
        }

        if (battleManager.battle_Cost.cur_Cost < selectCard.card_Cost)
        {
            isPossibleSummon = false;
            return;
        }

        isPossibleSummon = true;
    }

    /// <summary>
    /// ��ȯ ���� ������Ʈ �� ����
    /// </summary>
    public void Update_SummonRange()
    {
        if (summonRange >= 0)
            return;

        if(summonRangeDelay > 0)
        {
            summonRangeDelay -= Time.deltaTime;
            return;
        }
        Debug.Log("���� �þ");
        summonRangeDelay = 30f;
        summonRange = summonRange + stageData.max_Range / 4;
        Set_SummonRangeLinePos();
    }

    /// <summary>
    /// ��ȯ ���� �ӽ� ���� ������
    /// </summary>
    private void Set_SummonRangeLinePos()
    {
        summonRangeLine.SetPosition(0, new Vector2(-stageData.max_Range, 0));
        summonRangeLine.SetPosition(1, new Vector2(summonRange, 0));
    }

    /// <summary>
    /// �ڵ� ī�� ��ο� ������Ʈ
    /// </summary>
    public void Update_CardDrow()
    {
        if (cur_Card >= max_Card)
            return;
        if (cardDelay > 0)
        {
            cardDelay -= Time.deltaTime;
            return;
        }
        cardDelay = 0.3f;
        Add_OneCard();
    }

    /// <summary>
    /// �ִ� ī�� ����
    /// </summary>
    /// <param name="max">�ִ� ��</param>
    public void Set_MaxCard(int max)
    {
        max_Card = max;
    }
}
