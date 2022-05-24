using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using Utill.Data;
using Utill.Tool;
using Battle;


public class CardObj : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //프로퍼티
    public int CardCost => _cardCost; //카드 코스트
    public int OriginCardCost => _originCardCost; //원래 카드 코스트
    public int Id => _id; //아이디
    public CardData CardDataValue => _cardData; //카드 데이터
    public int Grade => _grade; // 카드 등급
    public bool IsFusion => _isFusion; //융합 중인지
    public bool IsFusionFrom => _isFusionFrom; //융합할 때 이 카드가 이동하는지
    public PRS OriginPRS => _originPRS; //카드 위치
    public bool IsSelected => _isSelected;

    //변수
    private int _cardCost = 0;   
    private int _originCardCost = 0; 
    private bool _isFusion = false;
    private bool _isFusionFrom = false;
    private bool _isDrag; // 드래그 중인 상태인가
    private bool _isUnderCost; // 코스트가 부족할 때
    private int _grade = 1;
    private int _id = 0;
    private PRS _originPRS = default;
    private bool _isSelected = false;

    //참조 변수
    private CardData _cardData = null;
    private BattleManager _battleManager;

    //인스펙터 참조 변수
    [SerializeField]
    private Image _cardimage; //카드의 유닛, 혹은 전략카드 이미지
    [SerializeField]
    private Image _dontUseimage; //카드를 사용할 수 없을 때 명도 표시 이미지
    [SerializeField]
    private TextMeshProUGUI _costText; //카드의 코스트를 나타내는 텍스트
    [SerializeField]
    private Image _gradeFrame; // 카드의 등급 테두리
    [SerializeField]
    private Image _outLineFrame; // 카드 외곽획
    [SerializeField]
    private TextMeshProUGUI _nameText; //카드의 이름
    [SerializeField]
    private Image _fusionEffect; // 융합시 카드 색깔이 바뀔 때 사용하는 이미지
    [SerializeField]
    private RectTransform _rectTransform; // 카드의 렉트
    [SerializeField]
    private List<Sprite> _gradeFrameSprites; // 카드 테두리 스프라이트들
    [SerializeField, Header("유닛용")]
    private Image _stickerImage; //스티커 이미지
    [SerializeField]
    private RectTransform _stickerRect; //스티커 렉트트랜스폼

    /// <summary>
    /// 카드에 데이터를 전달함
    /// </summary>
    /// <param name="dataBase">유닛 데이터</param>
    /// <param name="id">카드 고유 아이디</param>
    public void Set_UnitData(CardData dataBase, int id)
    {
        _battleManager ??= FindObjectOfType<BattleManager>();

        //기본적인 초기화
        _isDrag = false;
        this._id = id;
        this._cardData = dataBase;
        _nameText.text = dataBase.card_Name;
        _costText.text = dataBase.card_Cost.ToString();
        _originCardCost = dataBase.card_Cost;
        _cardCost = dataBase.card_Cost;
        _cardimage.sprite = SkinData.GetSkin(dataBase._skinData._skinType);
        _grade = 1;
        SetUnitGrade();
        _isFusion = false;
        _fusionEffect.color = new Color(1, 1, 1, 1);
        _fusionEffect.DOFade(0, 0.8f);
        //스티커 초기화
        SetSticker();

        //카드 타입별 초기화
        switch (dataBase.cardType)
        {
            default:
            case CardType.Execute:
            case CardType.SummonTrap:
            case CardType.Installation:
                _cardData.strategyData.starategy_State.SetBattleManager(_battleManager);
                _cardData.strategyData.starategy_State.SetCard(this);
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
        switch (_grade)
        {
            default:
            case 0:
            case 1:
                _gradeFrame.sprite = _gradeFrameSprites[0];
                _outLineFrame.sprite = _gradeFrameSprites[0];
                _outLineFrame.color = Color.black;
                break;
            case 2:
                _gradeFrame.sprite = _gradeFrameSprites[1];
                _outLineFrame.sprite = _gradeFrameSprites[1];
                _outLineFrame.color = Color.black;
                break;
            case 3:
                _gradeFrame.sprite = _gradeFrameSprites[2];
                _outLineFrame.sprite = _gradeFrameSprites[2];
                _outLineFrame.color = Color.white;
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
        Color useColor = color;
        if(_isUnderCost)
		{
            useColor.r = useColor.r - 0.3f;
            useColor.g = useColor.g - 0.3f;
            useColor.b = useColor.b - 0.3f;
        }
        _fusionEffect.color = useColor;


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
        if(_isUnderCost)
		{
            return;
		}
        if (_isFusionFrom)
        {
            return;
        }
        if (_isDrag)
        {
            return;
        }
        _isDrag = true;
        _battleManager.CardComponent.SelectCard(this);
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
                _battleManager.CardComponent.SetUseCard(this);
                return;
            }

            SetCardPRS(_originPRS, 0.3f);
            _battleManager.CardComponent.SetUnSelectCard(this);
        }
    }

    /// <summary>
    /// 원래 위치로 돌아감
    /// </summary>
    public void RunOriginPRS()
    {
        SetCardPRS(_originPRS, 0.3f);
    }

    /// <summary>
    /// 선택중인 카드라면 강제로 사용하지 않게 한다
    /// </summary>
    public void DontUseCard()
	{
        //원래 위치로 돌아간다
        RunOriginPRS();

    }

    /// <summary>
    /// 현재 코스트와 이 카드의 코스트를 비교해 사용할 수 있는지 체크
    /// </summary>
    /// <param name="curCost"></param>
    public void CheckCost(int curCost)
	{
        if(curCost < _cardCost)
		{
            _isUnderCost = true;
            _dontUseimage.gameObject.SetActive(true);
		}
        else
        {
            _isUnderCost = false;
            _dontUseimage.gameObject.SetActive(false);
		}
	}

    /// <summary>
    /// 스티커 설정
    /// </summary>
    private void SetSticker()
    {
        if(StickerData.CheckCanSticker(_cardData))
        {
            _stickerImage.sprite = SkinData.GetSkin(_cardData.unitData.stickerData._skinType);
            _stickerRect.anchoredPosition = StickerData.ReturnStickerPos(_cardData.unitData.unitType);
            _stickerRect.gameObject.SetActive(true);
        }
        else
        {
            _stickerRect.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 선택되었는지를 설정한다
    /// </summary>
    public void SetIsSelected(bool boolean)
	{
        _isSelected = boolean;
    }
}
