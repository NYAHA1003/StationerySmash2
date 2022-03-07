using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Battle_Card : BattleCommand
{
    private int max_Card = 3;
    private int cur_Card = 0;
    private UnitDataSO unitDataSO;
    private GameObject cardMove_Prefeb;
    private Transform card_PoolManager;
    private Transform card_Canvas;
    private RectTransform card_LeftPosition;
    private RectTransform card_RightPosition;
    private RectTransform card_SpawnPosition;

    private Coroutine coroutine;
    public bool isDrow {get; private set;} //ī�带 �̰ų� �����ϴ� ������ 
    public bool isFusion { get; private set; } //ī�尡 �������� ��������
    public bool isCardDown { get; private set; } //ī�带 Ŭ���� ��������

    private int cardidCount;
    private int unitidCount;

    public Battle_Card(BattleManager battleManager, UnitDataSO unitDataSO, GameObject card_Prefeb, Transform card_PoolManager, Transform card_Canvas, RectTransform card_SpawnPosition, RectTransform card_LeftPosition, RectTransform card_RightPosition)
        : base(battleManager)
    {
        this.unitDataSO = unitDataSO;
        this.cardMove_Prefeb = card_Prefeb;
        this.card_PoolManager = card_PoolManager;
        this.card_Canvas = card_Canvas;
        this.card_SpawnPosition = card_SpawnPosition;
        this.card_RightPosition = card_RightPosition;
        this.card_LeftPosition = card_LeftPosition;
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
        int random = Random.Range(0, unitDataSO.unitDatas.Count);
        cur_Card++;

        CardMove cardmove = Pool_Card();
        cardmove.Set_UnitData(unitDataSO.unitDatas[random], cardidCount++);
        battleManager.card_DatasTemp.Add(cardmove);

        Sort_Card();
        Fusion_DelayCard();
    }

    private IEnumerator Delay_Drow()
    {
        yield return new WaitForSeconds(0.5f);
        isDrow = false;
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
        if (isDrow) return;
        for (int i = 0; i < battleManager.card_DatasTemp.Count - 1; i++)
        {
            if (battleManager.card_DatasTemp[i].unitData.unitType == battleManager.card_DatasTemp[i + 1].unitData.unitType)
            {
                if (battleManager.card_DatasTemp[i].grade == battleManager.card_DatasTemp[i + 1].grade)
                {
                    coroutine = battleManager.StartCoroutine(Fusion_Move(i));
                    isDrow = true;
                    isFusion = true;
                    return;
                }
            }
        }
        isFusion = false;
    }

    /// <summary>
    /// ī�� ���� �ִϸ��̼�
    /// </summary>
    /// <param name="index">�� ��° ī�尡 �����ϴ���</param>
    /// <returns></returns>
    private IEnumerator Fusion_Move(int index)
    {
        battleManager.card_DatasTemp[index + 1].Set_CardPRS(battleManager.card_DatasTemp[index].originPRS, 0.3f);
        battleManager.card_DatasTemp[index].Fusion_FadeInEffect();
        battleManager.card_DatasTemp[index + 1].Fusion_FadeInEffect();

        yield return new WaitForSeconds(0.3f);
        
        battleManager.card_DatasTemp[index].Fusion_FadeOutEffect();
        battleManager.card_DatasTemp[index].Upgrade_UnitGrade();
        
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

        isDrow = true;
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
        if (cur_Card == 0)
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
    public void Check_MouseOver(CardMove card)
    {
        if (isFusion) return;
        if (isDrow) return;
        card.Set_CardScale(Vector3.one * 1.3f, 0.3f);
        isCardDown = true;
    }

    /// <summary>
    /// ī�� ������ �����
    /// </summary>
    /// <param name="card"></param>
    public void Check_MouseExit(CardMove card)
    {
        if (isFusion) return;
        if (isDrow) return;
        card.Set_CardScale(Vector3.one * 1, 0.3f);
        isCardDown = false;
    }

    /// <summary>
    /// ī�带 ����Ѵ�
    /// </summary>
    /// <param name="card"></param>
    public void Check_MouseUp(CardMove card)
    {
        if (isFusion) return;
        if (isDrow) return;
        Subtract_CardAt(battleManager.card_DatasTemp.FindIndex(x => x.id == card.id));
        isCardDown = false;

        //���� ��ȯ

        Vector3 mouse_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        battleManager.battle_Unit.Summon_Unit(card.unitData, new Vector3(mouse_Pos.x,0,0), unitidCount);
    }
}
