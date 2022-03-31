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
    public int CardCost { get; private set; }
    
    [SerializeField]
    private Image _background;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private TextMeshProUGUI _costText;
    [SerializeField]
    private Image _gradeImage;
    [SerializeField]
    private TextMeshProUGUI _gradeText;
    [SerializeField]
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private Image _fusionEffect;

    public bool _isFusion;
    public bool _isDontMove;

    public DataBase _dataBase;

    public int _grade = 1;
    public int _id;

    private RectTransform _rectTransform;

    private BattleManager _battleManager;


    private bool _isDrag; // �巡�� ���� �����ΰ�
    
    public PRS _originPRS;

    /// <summary>
    /// ī�忡 �����͸� ������
    /// </summary>
    /// <param name="dataBase">���� ������</param>
    /// <param name="id">ī�� ���� ���̵�</param>
    public void Set_UnitData(DataBase dataBase, int id)
    {
        _battleManager??= FindObjectOfType<BattleManager>();
        _rectTransform??= GetComponent<RectTransform>();

        //�⺻���� �ʱ�ȭ
        _isDrag = false;
        this._id = id;
        this._dataBase = dataBase;
        _nameText.text = dataBase.card_Name;
        _costText.text = dataBase.card_Cost.ToString();
        CardCost = dataBase.card_Cost;
        _image.sprite = dataBase.card_Sprite;
        _grade = 1;
        SetUnitGrade();
        _isFusion = false;
        _fusionEffect.color = new Color(1, 1, 1, 1);
        _fusionEffect.DOFade(0, 0.8f);

        //���ֺ� �ʱ�ȭ
        switch (dataBase.cardType)
        {
            default:
            case CardType.Execute:
            case CardType.SummonTrap:
            case CardType.Installation:
                break;
            case CardType.SummonUnit:
                break;
        }

        
    }

    public void ShowCard(bool isboolean)
    {
        gameObject.SetActive(isboolean);
    }

    /// <summary>
    /// ī�� �ִϸ��̼� ��Ʈ��
    /// </summary>
    /// <param name="prs">��ġ ���� ������</param>
    /// <param name="duration">�ð�</param>
    /// <param name="isDotween">True�� ��Ʈ�� ���</param>
    public void SetCardPRS(PRS prs, float duration, bool isDotween = true)
    {
        if (_isDontMove)
            return;
        if (isDotween)
        {
            _rectTransform.DOAnchorPos(prs.pos, duration);
            _rectTransform.DORotateQuaternion(prs.rot, duration);
            _rectTransform.DOScale(prs.scale, duration);
            return;
        }
        _rectTransform.anchoredPosition = prs.pos;
        _rectTransform.rotation = prs.rot;
        _rectTransform.localScale = prs.scale;
    }
    /// <summary>
    /// ��ġ�� �ű�
    /// </summary>
    /// <param name="pos">��ġ</param>
    /// <param name="duration">�ð�</param>
    /// <param name="isDotween">True�� ��Ʈ�� ���</param>
    public void SetCardPos(Vector3 pos, float duration, bool isDotween = true)
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
    public void SetCardScale(Vector3 scale, float duration, bool isDotween = true)
    {
        if (isDotween)
        {
            _rectTransform.DOScale(scale, duration);
            return;
        }
        _rectTransform.localScale = scale;
    }
    /// <summary>
    /// ������ �ٲ�
    /// </summary>
    /// <param name="rot">����</param>
    /// <param name="duration">�ð�</param>
    /// <param name="isDotween">True�� ��Ʈ�� ���</param>
    public void SetCardRot(Quaternion rot, float duration, bool isDotween = true)
    {
        if (isDotween)
        {
            _rectTransform.DORotateQuaternion(rot, duration);
            return;
        }
        _rectTransform.rotation = rot;
    }


    /// <summary>
    /// ���� �ܰ� �̹��� ����
    /// </summary>
    public void SetUnitGrade()
    {
        _gradeText.text = _grade.ToString();
        switch (_grade)
        {
            default:
            case 0:
            case 1:
                _gradeImage.color = new Color(0, 0, 0);
                break;
            case 2:
                _gradeImage.color = new Color(1, 1, 0);
                break;
            case 3:
                _gradeImage.color = new Color(1, 1, 1);
                break;
        }
    }

    /// <summary>
    /// ���� ���׷��̵� 
    /// </summary>
    public void UpgradeUnitGrade()
    {
        _grade++;
        SetUnitGrade();
    }

    /// <summary>
    /// ���ս� �Ͼ���
    /// </summary>
    public void FusionFadeInEffect(Color color)
    {
        _fusionEffect.color = color;
        _fusionEffect.DOFade(1, 0.2f);
    }
    /// <summary>
    /// ������ ���� �� ���ƿ�
    /// </summary>
    public void FusionFadeOutEffect()
    {
        _fusionEffect.DOFade(0, 0.5f);
    }

    /// <summary>
    /// ī�� ����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isFusion) return;
        if (_isDrag) return;

        _isDrag = true;
        _battleManager.CommandCard.SelectCard(this);
    }

    /// <summary>
    /// ī�� ���� ����
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if(_isDrag)
        {
            _isDrag = false;
            
            if (_rectTransform.anchoredPosition.y > 0)
            {
                _battleManager.CommandCard.SetUseCard(this);
                return;
            }

            SetCardPRS(_originPRS, 0.3f);
            _battleManager.CommandCard.SetUnSelectCard(this);
        }
    }

    /// <summary>
    /// ���� ��ġ�� ���ư�
    /// </summary>
    public void RunOriginPRS()
    {
        SetCardPRS(_originPRS, 0.3f);
    }
}
