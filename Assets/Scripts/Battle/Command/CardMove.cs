using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using Utill;
using Battle;


public class CardMove : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //변수
    public int CardCost { get; private set; }
    public int OriginCardCost => _originCardCost;
    public bool _isFusion;
    public bool _isDontMove;
    public CardData _dataBase;
    public int _grade = 1;
    public int _id;
    private bool _isDrag; // 드래그 중인 상태인가

    //참조 변수
    private BattleManager _battleManager;

    //인스펙터 참조 변수
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
    [SerializeField]
    private RectTransform _rectTransform;



    
    public PRS _originPRS;
    private int _originCardCost = 0; 

    /// <summary>
    /// 카드에 데이터를 전달함
    /// </summary>
    /// <param name="dataBase">유닛 데이터</param>
    /// <param name="id">카드 고유 아이디</param>
    public void Set_UnitData(CardData dataBase, int id)
    {
        _rectTransform??= GetComponent<RectTransform>();
        _battleManager ??= FindObjectOfType<BattleManager>();

        //기본적인 초기화
        _isDrag = false;
        this._id = id;
        this._dataBase = dataBase;
        _nameText.text = dataBase.card_Name;
        _costText.text = dataBase.card_Cost.ToString();
        _originCardCost = dataBase.card_Cost;
        CardCost = dataBase.card_Cost;
        _image.sprite = dataBase.card_Sprite;
        _grade = 1;
        SetUnitGrade();
        _isFusion = false;
        _fusionEffect.color = new Color(1, 1, 1, 1);
        _fusionEffect.DOFade(0, 0.8f);

        //유닛별 초기화
        switch (dataBase.cardType)
        {
            default:
            case CardType.Execute:
            case CardType.SummonTrap:
            case CardType.Installation:
                _dataBase.strategyData.starategy_State.SetBattleManager(_battleManager);
                _dataBase.strategyData.starategy_State.SetCard(this);
                break;
            case CardType.SummonUnit:
                break;
        }
    }

    /// <summary>
    /// 코스트 설정
    /// </summary>
    /// <param name="cost"></param>
    public void SetCost(int cost)
    {
        CardCost = cost;
    }
    public void ShowCard(bool isboolean)
    {
        gameObject.SetActive(isboolean);
    }

    /// <summary>
    /// 카드 애니메이션 닷트윈
    /// </summary>
    /// <param name="prs">위치 정보 데이터</param>
    /// <param name="duration">시간</param>
    /// <param name="isDotween">True면 닷트윈 사용</param>
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
    /// 위치만 옮김
    /// </summary>
    /// <param name="pos">위치</param>
    /// <param name="duration">시간</param>
    /// <param name="isDotween">True면 닷트윈 사용</param>
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
    /// 크기만 바꿈
    /// </summary>
    /// <param name="scale">크기</param>
    /// <param name="duration">시간</param>
    /// <param name="isDotween">True면 닷트윈 사용</param>
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
    /// 각도만 바꿈
    /// </summary>
    /// <param name="rot">각도</param>
    /// <param name="duration">시간</param>
    /// <param name="isDotween">True면 닷트윈 사용</param>
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
    /// 유닛 단계 이미지 설정
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
    /// 유닛 업그레이드 
    /// </summary>
    public void UpgradeUnitGrade()
    {
        _grade++;
        SetUnitGrade();
    }

    /// <summary>
    /// 융합시 하얘짐
    /// </summary>
    public void FusionFadeInEffect(Color color)
    {
        _fusionEffect.color = color;
        _fusionEffect.DOFade(1, 0.2f);
    }
    /// <summary>
    /// 융합이 끝난 후 돌아옴
    /// </summary>
    public void FusionFadeOutEffect()
    {
        _fusionEffect.DOFade(0, 0.5f);
    }

    /// <summary>
    /// 카드 선택
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
    /// 카드 선택 해제
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
    /// 원래 위치로 돌아감
    /// </summary>
    public void RunOriginPRS()
    {
        SetCardPRS(_originPRS, 0.3f);
    }
}
