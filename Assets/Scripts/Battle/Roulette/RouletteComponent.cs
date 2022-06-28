using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using DG.Tweening;
using Main.Event;
using Utill.Data;
using Utill.Tool;
using System.Linq; 

public class RouletteComponent : MonoBehaviour
{
    // 1 , 1.5 , 2 , 3
    // 50% 25% 18% 7%
    // �⺻(�ּ�) ȹ�� ���� 
 
    [SerializeField]
    private List<RouletteItemData> _rouletteItemDataList = new List<RouletteItemData>();
    [SerializeField]
    private RouletteItem _rouletteItem; // �귿 ������ ������
    [SerializeField]
    private Transform _linePrefab; // �� ������ 
    [SerializeField]
    private Transform _itemParent; // �귿 ������ �θ� 
    [SerializeField]
    private Transform _lineParent; // �귿 �� �θ� 

    private float _itemAngle; // �귿 ������ ���� ( ������ ���� �޶��� ) 
    private float _lineAngle; // �� ���� ( ������ ���� �޶��� )

    [SerializeField]
    private int _spinDuration; // ȸ�� �ð�
    [SerializeField]
    private Transform _spinningRoulette; // ȸ���ϴ� ȸ���� 
    
    [SerializeField]
    private Button _spinButton; // ������ ��ư 

    [SerializeField]
    private RectTransform _gainedItemPanel; // ���� ������ ���� �г� 
    [SerializeField]
    private RectTransform _pig; // ���� ������
    [SerializeField]
    private GameObject _rouletteCanvas;
    [SerializeField]
    private GameObject _blackImage; // �̱� �� ��Ÿ���� ���� ��� �̹��� 

    private int _selectionIndex = 0; // �귿���� ���õ� ������ 

    private int _accumulatedWeight; // ����ġ ����� ���� ���� 
    

    [SerializeField]
    private int _standardMoney; // ���������� ���� ��� �ּ� ��  

    private Vector3 _originPigPos; 
    
    void Awake()
    {
        _itemAngle = 360 / _rouletteItemDataList.Count;
        _lineAngle = _itemAngle * 0.5f;
        _originPigPos = _pig.anchoredPosition;
        //  _standardMoney = AIAndStageData.Instance._currentStageDatas._rewardMoney;

        EventManager.Instance.StartListening(EventsType.SpinRoulette, SpinRoulette);
        EventManager.Instance.StartListening(EventsType.ActiveRouletteCanvas, ActiveRoulleteCanvas); 
    }

    private void Start()
    {
        SetWeight();
        CreateItemsAndLines();


    }

    /// <summary>
    ///  �귿 ĵ���� Ȱ��ȭ ��Ȱ��ȭ , ��Ȱ��ȭ -> Ȱ��ȭ�� ���� �缳�� 
    /// </summary>
    private void ActiveRoulleteCanvas()
    {
        if (_rouletteCanvas.activeSelf == false)
        {
            ResetValues();
        }
        _rouletteCanvas.SetActive(!_rouletteCanvas.activeSelf);
    }
    /// <summary>
    /// �� �缳��
    /// </summary>
    private void ResetValues()
    {
        _spinButton.gameObject.SetActive(true);
        _blackImage.SetActive(false);
        _gainedItemPanel.gameObject.SetActive(false);
        _gainedItemPanel.anchoredPosition = Vector2.zero;
        _pig.anchoredPosition = _originPigPos;
    }

    #region ������ ���� 
    /// <summary>
    /// ���������� ���� �޶����� ��� ���� �� ���� (�������� �귿)  
    /// </summary>
    private void SetItemCount()
    {
        int count = _rouletteItemDataList.Count;
        for (int i = 0; i < count - 1; i++)
        {
            _rouletteItemDataList[i]._itemCount = (int)(_standardMoney + (i * _standardMoney * 0.5f));
        }
        _rouletteItemDataList[count - 1]._itemCount = _standardMoney * 3;
    }

    [SerializeField]
    private int[] dalgonaCounts;
    [SerializeField]
    private int[] coinCounts;
    [SerializeField]
    private Sprite[] itemSprites; 
    /// <summary>
    /// 65% : ��      35% : �ް� 
    /// </summary>
    private void SetItemCounts()
    {
        bool isCoin = false; 
        int count = _rouletteItemDataList.Count;
        for (int i = 0; i < count ; i++)
        {
            if(Random.Range(0, 100) < 65)
            {
                isCoin = true;
                _rouletteItemDataList[i]._itemCount = coinCounts[i];
                _rouletteItemDataList[i].rulletItemType = RoulletItemType.Coin;
                _rouletteItemDataList[i]._itemImage = itemSprites[(int)RoulletItemType.Coin];   
            }
            else
            {
                _rouletteItemDataList[i]._itemCount = dalgonaCounts[i];
                _rouletteItemDataList[i].rulletItemType = RoulletItemType.Dalgona;
                _rouletteItemDataList[i]._itemImage = itemSprites[(int)RoulletItemType.Dalgona];

            }
        }
    }

    CardData card;
    ///// <summary>
    ///// �нǹ� ����  
    ///// </summary>
    private void SetItem(int idx)
    {
        List<CardData> cardDatas = DeckDataManagerSO.StdDeckDataList.ToList(); 
        int cardCount = cardDatas.Count;
        int index = Random.Range(0, cardCount);

        card = cardDatas[index];

        _rouletteItemDataList[idx]._itemImage = SkinData.GetSkin(card._skinData._skinType);
        _rouletteItemDataList[idx].rulletItemType = RoulletItemType.Card;
        _rouletteItemDataList[idx]._itemCount = 1;
        // ��� �нǹ��� �������� 
        // ����Ʈ �����ͼ� �������� n�� ������ 
        // ��������Ʈ ��������, 
        // ���� ���� 
    }
    /// <summary>
    /// �귿�� ���� �����۰� �� ���� 
    /// </summary>
    private void CreateItemsAndLines()
    {
        SetItemCounts();
        SetItem(0);
        int count = _rouletteItemDataList.Count;
        for (int i = 0; i < count; i++)
        {
            RouletteItem rouletteItem = Instantiate(_rouletteItem, _itemParent.position, Quaternion.identity, _itemParent);
            rouletteItem.SetUp(_rouletteItemDataList[i]);

            rouletteItem.transform.RotateAround(_itemParent.position, Vector3.back, _itemAngle * i);

            Transform line = Instantiate(_linePrefab, _lineParent.position, Quaternion.identity, _lineParent);
            line.transform.RotateAround(_lineParent.position, Vector3.back, (_itemAngle * i) + _lineAngle);
        }
        //SetItem(); 
    }

    /// <summary>
    /// ����ġ ���� 
    /// </summary>
    private void SetWeight()
    {
        int count = _rouletteItemDataList.Count;
        Debug.Log("D");
        for (int i = 0; i < count; i++)
        {
            Debug.Log("S");
            _rouletteItemDataList[i].index = i;

            _accumulatedWeight += _rouletteItemDataList[i]._chance;
            _rouletteItemDataList[i].weght = _accumulatedWeight; 
        }
    }
    #endregion
    /// <summary>
    /// ������ ȹ�� 
    /// </summary>
    private int GetItem()
    {
        int randWeight = Random.Range(0, _accumulatedWeight);
        int count = _rouletteItemDataList.Count;

        for (int i = 0; i < count; i++)
        {
            if (randWeight < _rouletteItemDataList[i].weght)
            {
                return i;
            }
        }
        return 0; 
    }

    /// <summary>
    /// ���� ������ ������ �����ϱ� 
    /// </summary>
    private void SaveItemData(RoulletItemType roulletItemType, int amount)
    {
        if(roulletItemType == RoulletItemType.Coin)
        {
            UserSaveManagerSO.AddMoney(amount);
        }
        else if (roulletItemType == RoulletItemType.Dalgona)
        {
            UserSaveManagerSO.AddCardData(card, amount);
        }
        else if(roulletItemType == RoulletItemType.Card)
        {
            UserSaveManagerSO.AddCardData(card, amount);
        }
    }
    // Ŭ���� ��ư ���ְ� 
    // ������ �̰� �̿� ���� ȸ���� i * _itemAngle ( �̰� ���� �� ) 
    // �� ȸ�� �� int angle = 360 * angleSpeed * angleDuration 
    // ������ �� ������ ������ �г� �߰� 
    private void SpinRoulette()
    {
        _spinButton.gameObject.SetActive(false);

        _selectionIndex = GetItem();
        _gainedItemPanel.GetComponent< RouletteItem>().SetUp(_rouletteItemDataList[_selectionIndex]);

        int getItemCount = _rouletteItemDataList[_selectionIndex]._itemCount; 
        float angle = _itemAngle * _selectionIndex;

        float targetAngle = angle +  360 * _spinDuration;

        Sequence seq = DOTween.Sequence();
        seq.Append(
        _spinningRoulette.DORotate(new Vector3(0, 0, targetAngle), 7, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuart)
        );
        seq.Join(_pig.DOAnchorPosY(-300, 0.5f));

        seq.AppendCallback(() =>
        {
            _gainedItemPanel.gameObject.SetActive(true);
            _blackImage.SetActive(true);
        });
        
        seq.Append(_gainedItemPanel.transform.DOScale(0.8f, 0.5f));
        seq.Append(_gainedItemPanel.transform.DOScale(1.2f, 0.5f));
        seq.Append(_gainedItemPanel.transform.DOScale(1f, 0.5f));
        seq.AppendCallback(() => SaveItemData(_rouletteItemDataList[_selectionIndex].rulletItemType, getItemCount ));
    }

}
