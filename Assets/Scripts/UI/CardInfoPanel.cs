using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Utill;

public class CardInfoPanel : MonoBehaviour
{
    //ī�� ���� �ؽ�Ʈ��
    //������ ���� ���
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

    //ī�� ����â 
    [SerializeField]
    private TextMeshProUGUI _nameText = null;
    [SerializeField]
    private TextMeshProUGUI _descriptionText = null;

    //ī�� �̹���
    [SerializeField]
    private Image _cardImage;

    //��Ų  
    [SerializeField]
    private SkinTestInventory _skinTestInventory = null;
    [SerializeField]
    private GameObject _skinButtonPrefeb = null;
    [SerializeField]
    private Transform _buttonParent = null;

    //��ƼĿ ����â
    [SerializeField]
    private GameObject _stickerPanel = null;
    //������ ���� ���

    //�̸�����â

    //��ũ�� ����â
    [SerializeField]
    private SkinScroll _skinScroll = null;

    private CardData _selectCardData;

    // ���� ������
    [SerializeField]
    private UserDeckData userDeckData; 
    private void Start()
    {
        EventManager.StartListening(Util.EventsType.ActiveCardDescription, SetCardsDatas);
    }
    /// <summary>
    /// ī�嵥���� ����
    /// </summary>
    public void SetCardInfoPanel(CardData cardData)
    {
        _selectCardData = cardData;

        //ī�� Ÿ�Կ� ���� ����â ����
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
    /// ī�� ������ ����
    /// </summary>
    public void SetCardsDatas()
    {
        for (int i = 0; i < userDeckData.deckList.cardDatas.Count; i++)
        {
            SetCardInfoPanel(userDeckData.deckList.cardDatas[i]);
        }
    }
    /// <summary>
    /// �ߵ��� ī���� UI ����
    /// </summary>
    /// <param name="cardData"></param>
    public void SetCardExecute(CardData cardData)
    {
        _unitStatTexts.SetActive(false);
        _stickerPanel.SetActive(false);

        //�̸�, �̹���, ���� ����
        _nameText.text = cardData.card_Name;
        _cardImage.sprite = cardData.skinData._cardSprite;
        _descriptionText.text = cardData.card_Description;
    }
    /// <summary>
    /// ���� ��ȯ�� ī���� UI ����
    /// </summary>
    /// <param name="cardData"></param>
    public void SetCardSummonUnit(CardData cardData)
    {
        _unitStatTexts.SetActive(true);
        _stickerPanel.SetActive(true);

        //�̸�, �̹���, ���� ����
        _nameText.text = cardData.card_Name;
        _cardImage.sprite = cardData.skinData._cardSprite;
        _descriptionText.text = cardData.card_Description;

        //���� �ؽ�Ʈ ����
        _hpText.text = cardData.unitData.unit_Hp.ToString();
        _attackText.text = cardData.unitData.damage.ToString();
        _attackSpeedText.text = cardData.unitData.attackSpeed.ToString();
        _moveSpeedText.text = cardData.unitData.moveSpeed.ToString();
        _weightText.text = cardData.unitData.unit_Weight.ToString();
    }
    /// <summary>
    /// ���� ��ȯ���� UI ����
    /// </summary>
    /// <param name="cardData"></param>
    public void SetCardSummonTrap(CardData cardData)
    {
        _unitStatTexts.SetActive(false);
        _stickerPanel.SetActive(false);

        //�̸�, �̹���, ���� ����
        _nameText.text = cardData.card_Name;
        _cardImage.sprite = cardData.skinData._cardSprite;
        _descriptionText.text = cardData.card_Description;
    }
    /// <summary>
    /// ��ġ���� UI ����
    /// </summary>
    /// <param name="cardData"></param>
    public void SetCardInstallation(CardData cardData)
    {
        _unitStatTexts.SetActive(false);
        _stickerPanel.SetActive(false);

        //�̸�, �̹���, ���� ����
        _nameText.text = cardData.card_Name;
        _cardImage.sprite = cardData.skinData._cardSprite;
        _descriptionText.text = cardData.card_Description;
    }

    /// <summary>
    /// ��Ų�� ������ �ִٸ� ����
    /// </summary>
    /// <param name="skinData"></param>
    public void OnSetSkin(SkinData skinData)
    {
        //�κ��丮�� �ش� ��Ų�� ������ ����
        SkinData getSkinData = SkinData.GetSkinDataList(skinData._cardNamingType)?.Find(x => x._skinId == skinData._skinId);
        
        //��Ų�����Ͱ� �ִٸ� ���� �������� ��Ų�����͸� ����
        if(getSkinData != null)
        {
            _selectCardData.skinData = getSkinData;
        }

    }

    /// <summary>
    /// ī�尡 ���� ��Ų����Ʈ �� ����
    /// </summary>
    public void SetSkinList(CardData cardData)
    {
        _selectCardData = cardData;

        //������ ������ ��Ų ����Ʈ ��������
        List<SkinData> skinList = SkinData.GetSkinDataList(_selectCardData.skinData._cardNamingType);
        
        //��Ų ��ư�� ����
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

            //��Ų �Լ����� �־��ش�
            skinButton.onClick.AddListener(() => OnSetSkin(skinList[i]));
        }
    }
}
