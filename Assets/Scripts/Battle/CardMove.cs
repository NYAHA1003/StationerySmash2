using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using Utill;



public class CardMove : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Image card_Background;
    [SerializeField]
    private Image card_UnitImage;
    [SerializeField]
    private TextMeshProUGUI card_UnitCost;
    [SerializeField]
    private Image card_Grade;
    [SerializeField]
    private TextMeshProUGUI card_GradeText;
    [SerializeField]
    private TextMeshProUGUI card_Name;
    [SerializeField]
    private Image fusion_Effect;

    public int card_Cost { get; private set; }
    public bool isFusion;

    public UnitData unitData;

    public int grade = 1;
    public int id;
    private float scale;

    private RectTransform rectTransform;

    private BattleManager battleManager;

    private Canvas cardCanvas;

    private bool isDrag; // 드래그 중인 상태인가
    private void Awake()
    {
        battleManager = FindObjectOfType<BattleManager>();
        rectTransform = GetComponent<RectTransform>();
    }

    public PRS originPRS;

    /// <summary>
    /// 카드에 유닛 데이터를 전달함
    /// </summary>
    /// <param name="unitData">유닛 데이터</param>
    /// <param name="id">카드 고유 아이디</param>
    public void Set_UnitData(UnitData unitData, int id)
    {
        cardCanvas ??= transform.parent.GetComponent<Canvas>();
        
        isDrag = false;

        this.id = id;
        this.unitData = unitData;
        card_Name.text = unitData.unitName;
        card_UnitCost.text = unitData.cost.ToString();
        card_UnitImage.sprite = unitData.sprite;
        card_Cost = unitData.cost;
        grade = 0;
        Set_UnitGrade();
        
        fusion_Effect.color = new Color(1, 1, 1, 1);
        fusion_Effect.DOFade(0, 0.8f);
    }

    /// <summary>
    /// 카드 애니메이션 닷트윈
    /// </summary>
    /// <param name="prs">위치 정보 데이터</param>
    /// <param name="duration">시간</param>
    /// <param name="isDotween">True면 닷트윈 사용</param>
    public void Set_CardPRS(PRS prs, float duration, bool isDotween = true)
    {
        if (isDotween)
        {
            rectTransform.DOAnchorPos(prs.pos, duration);
            rectTransform.DORotateQuaternion(prs.rot, duration);
            rectTransform.DOScale(prs.scale, duration);
            return;
        }
        rectTransform.anchoredPosition = prs.pos;
        rectTransform.rotation = prs.rot;
        rectTransform.localScale = prs.scale;
    }
    /// <summary>
    /// 위치만 옮김
    /// </summary>
    /// <param name="pos">위치</param>
    /// <param name="duration">시간</param>
    /// <param name="isDotween">True면 닷트윈 사용</param>
    public void Set_CardPos(Vector3 pos, float duration, bool isDotween = true)
    {
        if(isDotween)
        {
            transform.DOMove(pos, duration);
            return;
        }
        transform.position = pos;
    }
    /// <summary>
    /// 크기만 바꿈
    /// </summary>
    /// <param name="scale">크기</param>
    /// <param name="duration">시간</param>
    /// <param name="isDotween">True면 닷트윈 사용</param>
    public void Set_CardScale(Vector3 scale, float duration, bool isDotween = true)
    {
        if (isDotween)
        {
            rectTransform.DOScale(scale, duration);
            return;
        }
        rectTransform.localScale = scale;
    }
    /// <summary>
    /// 각도만 바꿈
    /// </summary>
    /// <param name="rot">각도</param>
    /// <param name="duration">시간</param>
    /// <param name="isDotween">True면 닷트윈 사용</param>
    public void Set_CardRot(Quaternion rot, float duration, bool isDotween = true)
    {
        if (isDotween)
        {
            rectTransform.DORotateQuaternion(rot, duration);
            return;
        }
        rectTransform.rotation = rot;
    }


    /// <summary>
    /// 유닛 단계 이미지 설정
    /// </summary>
    public void Set_UnitGrade()
    {
        card_GradeText.text = grade.ToString();
        switch (grade)
        {
            default:
            case 0:
            case 1:
                card_Grade.color = new Color(0, 0, 0);
                break;
            case 2:
                card_Grade.color = new Color(1, 1, 0);
                break;
            case 3:
                card_Grade.color = new Color(1, 1, 1);
                break;
        }
    }

    /// <summary>
    /// 유닛 업그레이드 
    /// </summary>
    public void Upgrade_UnitGrade()
    {
        grade++;
        Set_UnitGrade();
    }

    /// <summary>
    /// 융합시 하얘짐
    /// </summary>
    public void Fusion_FadeInEffect()
    {
        fusion_Effect.DOFade(1, 0.3f);
    }
    /// <summary>
    /// 융합이 끝난 후 돌아옴
    /// </summary>
    public void Fusion_FadeOutEffect()
    {
        fusion_Effect.DOFade(0, 0.3f);
    }

    /// <summary>
    /// 드래그 중일 때
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        //if(rectTransform.anchoredPosition.y > 0)
        //{
        //    Vector3 mouse_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    battleManager.battle_Card.Set_UnitAfterImage(unitData, mouse_Pos);
        //    return;
        //}
        //battleManager.battle_Card.Set_UnitAfterImage(unitData, Vector3.zero, true);
    }

    /// <summary>
    /// 카드 선택
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isFusion) return;
        if (isDrag) return;

        isDrag = true;
        battleManager.battle_Card.Set_SelectCard(this);
    }

    /// <summary>
    /// 카드 선택 해제
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if(isDrag)
        {
            isDrag = false;
            battleManager.battle_Card.Set_UnitAfterImage(unitData, Vector3.zero, true);

            if (rectTransform.anchoredPosition.y > 0)
            {
                battleManager.battle_Card.Set_UseCard(this);
                return;
            }

            Set_CardPRS(originPRS, 0.3f);
            battleManager.battle_Card.Set_UnSelectCard(this);
        }
    }

    public void Run_OriginPRS()
    {
        Set_CardPRS(originPRS, 0.3f);
    }
}
