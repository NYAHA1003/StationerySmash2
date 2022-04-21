using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Utill;

public class CardInfoPanel : MonoBehaviour
{
    //카드 스탯 텍스트들
    //유닛일 때만 사용
    [SerializeField]
    private GameObject _unitStatTexts = null;
    [SerializeField]
    private TextMeshProUGUI _hpText = null;
    [SerializeField]
    private TextMeshProUGUI _attackText = null;
    [SerializeField]
    private TextMeshProUGUI _attackSpeedText = null;
    [SerializeField]
    private TextMeshProUGUI _moveSpeedText = null;
    [SerializeField]
    private TextMeshProUGUI _weightText = null;

    //카드 설명창 
    [SerializeField]
    private TextMeshProUGUI _nameText = null;
    [SerializeField]
    private TextMeshProUGUI _descriptionText = null;

    //카드 이미지
    [SerializeField]
    private Image _cardImage;

    //스킨  
    [SerializeField]
    private SkinTestInventory _skinTestInventory = null;
    [SerializeField]
    private GameObject _skinButtonPrefeb = null;
    [SerializeField]
    private Transform _buttonParent = null;

    //스티커 착용창
    [SerializeField]
    private GameObject _stickerPanel = null;
    //유닛일 때만 사용

    //미리보기창

    //스크롤 조절창
    [SerializeField]
    private SkinScroll _skinScroll = null;

    private CardData _selectCardData;

    // 유저 데이터
    [SerializeField]
    private UserDeckData userDeckData; 
    private void Start()
    {
        EventManager.StartListening(Util.EventsType.ActiveCardDescription, SetCardsDatas);
    }
    /// <summary>
    /// 카드데이터 설정
    /// </summary>
    public void SetCardInfoPanel(CardData cardData)
    {
        _selectCardData = cardData;

        //카드 타입에 따라 설명창 설정
        switch (_selectCardData.cardType)
        {
            case CardType.Execute:
                SetCardExecute(cardData);
                break;
            case CardType.SummonUnit:
                SetCardSummonUnit(cardData);
                break;
            case CardType.SummonTrap:
                SetCardSummonTrap(cardData);
                break;
            case CardType.Installation:
                SetCardInstallation(cardData);
                break;
        }
    }

    /// <summary>
    /// 카드 데이터 설정
    /// </summary>
    public void SetCardsDatas()
    {
        for (int i = 0; i < userDeckData.deckList.cardDatas.Count; i++)
        {
            SetCardInfoPanel(userDeckData.deckList.cardDatas[i]);
        }
    }
    /// <summary>
    /// 발동형 카드의 UI 설정
    /// </summary>
    /// <param name="cardData"></param>
    public void SetCardExecute(CardData cardData)
    {
        _unitStatTexts.SetActive(false);
        _stickerPanel.SetActive(false);

        //이름, 이미지, 설명 설정
        _nameText.text = cardData.card_Name;
        _cardImage.sprite = cardData.skinData._cardSprite;
        _descriptionText.text = cardData.card_Description;
    }
    /// <summary>
    /// 유닛 소환형 카드의 UI 설정
    /// </summary>
    /// <param name="cardData"></param>
    public void SetCardSummonUnit(CardData cardData)
    {
        _unitStatTexts.SetActive(true);
        _stickerPanel.SetActive(true);

        //이름, 이미지, 설명 설정
        _nameText.text = cardData.card_Name;
        _cardImage.sprite = cardData.skinData._cardSprite;
        _descriptionText.text = cardData.card_Description;

        //스탯 텍스트 설정
        _hpText.text = cardData.unitData.unit_Hp.ToString();
        _attackText.text = cardData.unitData.damage.ToString();
        _attackSpeedText.text = cardData.unitData.attackSpeed.ToString();
        _moveSpeedText.text = cardData.unitData.moveSpeed.ToString();
        _weightText.text = cardData.unitData.unit_Weight.ToString();
    }
    /// <summary>
    /// 함정 소환형의 UI 설정
    /// </summary>
    /// <param name="cardData"></param>
    public void SetCardSummonTrap(CardData cardData)
    {
        _unitStatTexts.SetActive(false);
        _stickerPanel.SetActive(false);

        //이름, 이미지, 설명 설정
        _nameText.text = cardData.card_Name;
        _cardImage.sprite = cardData.skinData._cardSprite;
        _descriptionText.text = cardData.card_Description;
    }
    /// <summary>
    /// 설치형의 UI 설정
    /// </summary>
    /// <param name="cardData"></param>
    public void SetCardInstallation(CardData cardData)
    {
        _unitStatTexts.SetActive(false);
        _stickerPanel.SetActive(false);

        //이름, 이미지, 설명 설정
        _nameText.text = cardData.card_Name;
        _cardImage.sprite = cardData.skinData._cardSprite;
        _descriptionText.text = cardData.card_Description;
    }

    /// <summary>
    /// 스킨을 가지고 있다면 적용
    /// </summary>
    /// <param name="skinData"></param>
    public void OnSetSkin(SkinData skinData)
    {
        //인벤토리에 해당 스킨을 가지고 오기
        SkinData getSkinData = SkinData.GetSkinDataList(skinData._cardNamingType)?.Find(x => x._skinId == skinData._skinId);
        
        //스킨데이터가 있다면 유닛 데이터의 스킨데이터를 변경
        if(getSkinData != null)
        {
            _selectCardData.skinData = getSkinData;
        }

    }

    /// <summary>
    /// 카드가 가진 스킨리스트 쫙 생성
    /// </summary>
    public void SetSkinList(CardData cardData)
    {
        _selectCardData = cardData;

        //선택한 유닛의 스킨 리스트 가져오기
        List<SkinData> skinList = SkinData.GetSkinDataList(_selectCardData.skinData._cardNamingType);
        
        //스킨 버튼들 생성
        for(int i = 0; i < skinList.Count; i++)
        {
            Button skinButton = null;
            if(_buttonParent.GetChild(i) != null)
            {
                skinButton = _buttonParent.GetChild(i).GetComponent<Button>();
            }
            else
            {
                skinButton = Instantiate(_skinButtonPrefeb, _buttonParent).GetComponent<Button>();
            }

            skinButton.onClick.RemoveAllListeners();
            skinButton.GetComponent<CardChangeSkinButton>().SetButtonImages(skinList[i]);

            //스킨 함수들을 넣어준다
            skinButton.onClick.AddListener(() => OnSetSkin(skinList[i]));
        }
    }
}
