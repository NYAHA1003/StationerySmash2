using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;


[System.Serializable]
public class PRS
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;

    public PRS(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
    }
}

public class CardMove : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
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

    public void Set_UnitData(UnitData unitData, int id)
    {
        cardCanvas ??= transform.parent.GetComponent<Canvas>();
        
        isDrag = false;

        this.id = id;
        this.unitData = unitData;
        card_Name.text = unitData.unitName;
        card_UnitCost.text = unitData.cost.ToString();
        card_UnitImage.sprite = unitData.sprite;
        grade = 0;
        Set_UnitGrade();
        
        fusion_Effect.color = new Color(1, 1, 1, 1);
        fusion_Effect.DOFade(0, 0.8f);
    }

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
    public void Set_CardPos(Vector3 pos, float duration, bool isDotween = true)
    {
        if(isDotween)
        {
            transform.DOMove(pos, duration);
            return;
        }
        transform.position = pos;
    }
    public void Set_CardScale(Vector3 scale, float duration, bool isDotween = true)
    {
        if (isDotween)
        {
            rectTransform.DOScale(scale, duration);
            return;
        }
        rectTransform.localScale = scale;
    }
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

    public void Fusion_FadeInEffect()
    {
        fusion_Effect.DOFade(1, 0.3f);
    }
    public void Fusion_FadeOutEffect()
    {
        fusion_Effect.DOFade(0, 0.3f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        /*
         *캔버스가 카메라옵션일 때
         *Vector2 localPos;
         *RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mouseScrollDelta, cardCanvas.worldCamera, out localPos);
         *Set_CardPosition(new PRS(cardCanvas.transform.TransformPoint(localPos), Quaternion.identity, Vector3.one), 0, false);
        */

        if (battleManager.battle_Card.isFusion) return;
        if (battleManager.battle_Card.isDrow) return;

        isDrag = true;
        transform.position = Input.mousePosition;
        Set_CardRot(Quaternion.identity, 0.3f);

        if(rectTransform.anchoredPosition.y > 0)
        {
            Vector3 mouse_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            battleManager.battle_Unit.Set_UnitAfterImage(unitData, mouse_Pos);
            return;
        }
        battleManager.battle_Unit.Set_UnitAfterImage(unitData, Vector3.zero, true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        battleManager.battle_Unit.Set_UnitAfterImage(unitData, Vector3.zero, true);

        if (rectTransform.anchoredPosition.y > 0)
        {
            battleManager.battle_Card.Check_MouseUp(this);
            return;
        }

        Set_CardPRS(originPRS, 0.3f);
        battleManager.battle_Card.Check_MouseExit(this);
        isDrag = false;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        battleManager.battle_Card.Check_MouseOver(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(!isDrag)
        {
            battleManager.battle_Card.Check_MouseExit(this);
        }
    }
}
