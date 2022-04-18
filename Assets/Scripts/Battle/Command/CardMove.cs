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
    //프로퍼티
    public int CardCost => _cardCost; //카드 코스트
    public int OriginCardCost => _originCardCost; //원래 카드 코스트
    public int Id => _id; //아이디
    public CardData DataBase => _dataBase; //카드 데이터
    public int Grade => _grade; // 카드 등급
    public bool IsFusion => _isFusion; //융합 중인지
    public bool IsFusionFrom => _isFusionFrom; //융합할 때 이 카드가 이동하는지
    public PRS OriginPRS => _originPRS; //카드 위치

    //변수
    private int _cardCost = 0;   
    private int _originCardCost = 0; 
    private bool _isFusion = false;
    private bool _isFusionFrom = false;
    private int _grade = 1;
    private int _id = 0;
    private bool _isDrag; // 드래그 중인 상태인가
    private PRS _originPRS = default;

    //참조 변수
    private CardData _dataBase = null;
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


    /// <summary>
    /// 카드에 데이터를 전달함
    /// </summary>
    /// <param name="dataBase">유닛 데이터</param>
    /// <param name="id">카드 고유 아이디</param>
    public void Set_UnitData(CardData dataBase, int id)
    {
        _rectTransform ??= GetComponent<RectTransform>();
        _battleManager ??= FindObjectOfType<BattleManager>();

        //기본적인 초기화
        _isDrag = false;
        this._id = id;
        this._dataBase = dataBase;
        _nameText.text = dataBase.card_Name;
        _costText.text = dataBase.card_Cost.ToString();
        _originCardCost = dataBase.card_Cost;
        _cardCost = dataBase.card_Cost;
        _image.sprite = dataBase.skinData.cardSprite;
        _grade = 1;
        SetUnitGrade();
        _isFusion = false;
        _fusionEffect.color = new Color(1, 1, 1, 1);
        _fusionEffect.DOFade(0, 0.8f);

        //카드 종류별 초기화
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
    /// 카드 위치 설정
    /// </summary>
    /// <param name="prs"></param>
    public void SetOriginPRS(PRS prs)
    {
        _originPRS = prs;
    }

    /// <summary>
    /// 융합 중인지 설정
    /// </summary>
    /// <param name="isfusion"></param>
    public void SetIsFusion(bool isfusion)
    {
        _isFusion = isfusion;
    }

    /// <summary>
    /// 융합하러 이동하는 애인지 설정
    /// </summary>
    /// <param name="isfrom"></param>
    public void SetIsFusionFrom(bool isfrom)
    {
        _isFusionFrom = isfrom;
    }

    /// <summary>
    /// 아이디 설정
    /// </summary>
    public void SetID(int id)
    {
        _id = id;
    }

    /// <summary>
    /// 코스트 설정
    /// </summary>
    /// <param name="cost"></param>
    public void SetCost(int cost)
    {
        _cardCost = cost;
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
        if (_isFusionFrom)
        {
            return;
        }
        if (_isDrag)
        {
            return;
        }
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
