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

    private bool isDrag; // �巡�� ���� �����ΰ�
    private void Awake()
    {
        battleManager = FindObjectOfType<BattleManager>();
        rectTransform = GetComponent<RectTransform>();
    }

    public PRS originPRS;

    /// <summary>
    /// ī�忡 ���� �����͸� ������
    /// </summary>
    /// <param name="unitData">���� ������</param>
    /// <param name="id">ī�� ���� ���̵�</param>
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
    /// ī�� �ִϸ��̼� ��Ʈ��
    /// </summary>
    /// <param name="prs">��ġ ���� ������</param>
    /// <param name="duration">�ð�</param>
    /// <param name="isDotween">True�� ��Ʈ�� ���</param>
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
    /// ��ġ�� �ű�
    /// </summary>
    /// <param name="pos">��ġ</param>
    /// <param name="duration">�ð�</param>
    /// <param name="isDotween">True�� ��Ʈ�� ���</param>
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
    /// ũ�⸸ �ٲ�
    /// </summary>
    /// <param name="scale">ũ��</param>
    /// <param name="duration">�ð�</param>
    /// <param name="isDotween">True�� ��Ʈ�� ���</param>
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
    /// ������ �ٲ�
    /// </summary>
    /// <param name="rot">����</param>
    /// <param name="duration">�ð�</param>
    /// <param name="isDotween">True�� ��Ʈ�� ���</param>
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
    /// ���� �ܰ� �̹��� ����
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
    /// ���� ���׷��̵� 
    /// </summary>
    public void Upgrade_UnitGrade()
    {
        grade++;
        Set_UnitGrade();
    }

    /// <summary>
    /// ���ս� �Ͼ���
    /// </summary>
    public void Fusion_FadeInEffect()
    {
        fusion_Effect.DOFade(1, 0.3f);
    }
    /// <summary>
    /// ������ ���� �� ���ƿ�
    /// </summary>
    public void Fusion_FadeOutEffect()
    {
        fusion_Effect.DOFade(0, 0.3f);
    }

    /// <summary>
    /// �巡�� ���� ��
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
    /// ī�� ����
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
    /// ī�� ���� ����
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
