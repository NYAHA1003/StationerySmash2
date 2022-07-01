using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using DG.Tweening;
using Main.Event;
using Utill.Data;
using Utill.Tool;
using System.Linq;
using TMPro;
using Battle; 

public enum RouletteType
{
    Daily, // ���Ϸ귿
    Stage, // �������� �귿
}


public class RouletteComponent : MonoBehaviour
{
    // 1 , 1.5 , 2 , 3
    // 50% 25% 18% 7%
    // �⺻(�ּ�) ȹ�� ���� 
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
    [SerializeField]
    private TextMeshProUGUI _rouletteText; // � �귿���� ���� �ؽ�Ʈ  

    private int _selectionIndex = 0; // �귿���� ���õ� ������ 

    private int _accumulatedWeight; // ����ġ ����� ���� ���� 

    [SerializeField]
    private RouletteType _rouletteType;
    [SerializeField]
    private int _standardMoney; // ���������� ���� ��� �ּ� ��  

    private Vector3 _originPigPos;

    [SerializeField]
    private RouletteDataSO _rouletteDataSO;  // �귿�� �� ��, �ް� ������ ��������Ʈ ���� 

    private List<CardData> allCardList;
    private List<RouletteItem> roulletItemList = new List<RouletteItem>(); 

    void Awake()
    {
        _itemAngle = 360 / _rouletteDataSO._rouletteItemDataList.Count;
        _lineAngle = _itemAngle * 0.5f;
        _originPigPos = _pig.anchoredPosition;
        //  _standardMoney = AIAndStageData.Instance._currentStageDatas._rewardMoney;

        EventManager.Instance.StartListening(EventsType.SpinRoulette, SpinRoulette);
        EventManager.Instance.StartListening(EventsType.ActiveRouletteCanvas, ActiveRoulleteCanvas); 
    }

    [ContextMenu("�׽�Ʈ")]
    public void Test()
    {
        ResetValues(); 
        StartCoroutine(AssignItemsAndLines());
    }
    private void Start()
    {
        SetWeight();
        SetCanRouletteItem();
        CreateItemsAndLines(); 
    }

    private void OnEnable()
    {
        ResetValues();
        SetRouletteTypeStage(1); 
        StartCoroutine(AssignItemsAndLines());
    }

    /// <summary>
    /// 0 : Daily(LevelUp) 1 : Stage 
    /// </summary>
    /// <param name="rouletteType"></param>
    public void SetRouletteTypeStage(int rouletteType)
    {
        _rouletteType = (RouletteType)rouletteType;

        if(_rouletteType == RouletteType.Daily)
        {
            _rouletteText.text = string.Format("���� ��!");
        }
        else if (_rouletteType == RouletteType.Stage)
        {
            _rouletteText.text = string.Format("�������� Ŭ����!");
        }

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

    private int _notCoinOrDalgona = 2; // ���ΰ� �ް��� �ƴ� ������ ����
    #region ������ ���� 
    /// <summary>
    /// ���������� ���� �޶����� ��� ���� �� ���� (�ϵ� �귿)  
    /// </summary>
    private void SetItemCountEasy()
    {
        _standardMoney = AIAndStageData.Instance._currentStageDatas._rewardMoney; 
        int count = _rouletteDataSO._rouletteItemDataList.Count;
        for (int i = 0; i < count - _notCoinOrDalgona; i++)
        {
            _rouletteDataSO._rouletteItemDataList[i]._itemCount = (int)(_standardMoney + (i * _standardMoney * 0.5f));
            _rouletteDataSO._rouletteItemDataList[i]._itemImage = _rouletteDataSO.itemSprites[(int)RouletteItemType.Coin];

        }
        _rouletteDataSO._rouletteItemDataList[count - 1]._itemCount = _standardMoney * 3;
        _rouletteDataSO._rouletteItemDataList[count - 1]._itemImage = _rouletteDataSO.itemSprites[(int)RouletteItemType.Coin];

    }


    /// <summary>
    /// 65% : ��      35% : �ް� ( ���� �귿 ) 
    /// </summary>
    private void SetItemCountHard()
    {
        int count = _rouletteDataSO._rouletteItemDataList.Count;
        for (int i = 0; i < count - _notCoinOrDalgona; i++)
        {
            if(Random.Range(0, 100) < 65) // ������ ��� 
            {
                _rouletteDataSO._rouletteItemDataList[i]._itemCount = _rouletteDataSO.coinCounts[i];
                _rouletteDataSO._rouletteItemDataList[i].rulletItemType = RouletteItemType.Coin;
                _rouletteDataSO._rouletteItemDataList[i]._itemImage = _rouletteDataSO.itemSprites[(int)RouletteItemType.Coin];   
            }
            else // �ް��� ��� 
            {
                _rouletteDataSO._rouletteItemDataList[i]._itemCount = _rouletteDataSO.dalgonaCounts[i];
                _rouletteDataSO._rouletteItemDataList[i].rulletItemType = RouletteItemType.Dalgona;
                _rouletteDataSO._rouletteItemDataList[i]._itemImage = _rouletteDataSO.itemSprites[(int)RouletteItemType.Dalgona];

            }
        }
    }

    /// <summary>
    ///  �귿���� ���� �� �ִ� �нǹ� ���� 
    /// </summary>
    private void SetCanRouletteItem()
    {
        allCardList = DeckDataManagerSO.StdDeckDataList.ToList();

        foreach (var type in System.Enum.GetValues(typeof(CardNamingType)))
        {
            if ((int)type > 1000)
            {
                allCardList.Remove(DeckDataManagerSO.FindStdCardData((CardNamingType)type));
            }
        }
    }
    CardData card;
    ///// <summary>
    ///// Ư�� ������ ����  ( �нǹ�, ���� ) 
    ///// </summary>
    private void SetISpecialtem(int idx,RouletteItemType rouletteItemType)
    {
        int cardCount = allCardList.Count;
        int index = Random.Range(0, cardCount);
        int count = Random.Range(5, 21);

        switch (rouletteItemType)
        {
            case RouletteItemType.Card:
                card = allCardList[index];

                _rouletteDataSO._rouletteItemDataList[idx]._itemImage = SkinData.GetSkin(card._skinData._skinType);
                _rouletteDataSO._rouletteItemDataList[idx].rulletItemType = RouletteItemType.Card;
                _rouletteDataSO._rouletteItemDataList[idx]._itemCount = count;
                _rouletteDataSO._rouletteItemDataList[idx]._chance = 5;
                break;
            case RouletteItemType.Coupon:
                _rouletteDataSO._rouletteItemDataList[idx]._itemImage = _rouletteDataSO.itemSprites[(int)RouletteItemType.Coupon];
                _rouletteDataSO._rouletteItemDataList[idx].rulletItemType = RouletteItemType.Coupon;
                _rouletteDataSO._rouletteItemDataList[idx]._itemCount = 1;
                _rouletteDataSO._rouletteItemDataList[idx]._chance = 1000;
                break;
        }

;
    }


    /// <summary>
    /// ��, ������ ���� 
    /// </summary>
    private void CreateItemsAndLines()
    {
        int count = _rouletteDataSO._rouletteItemDataList.Count;
        roulletItemList.Clear(); 
        for (int i = 0; i < count; i++)
        {
            RouletteItem rouletteItem = Instantiate(_rouletteItem, _itemParent.position, Quaternion.identity, _itemParent);
            roulletItemList.Add(rouletteItem);
            if (i == count - 1)
            {
                roulletItemList[i].transform.GetChild(0).localScale = new Vector2(2, 2);
            }
            roulletItemList[i].transform.RotateAround(_itemParent.position, Vector3.back, _itemAngle * i);

            Transform line = Instantiate(_linePrefab, _lineParent.position, Quaternion.identity, _lineParent);
            line.transform.RotateAround(_lineParent.position, Vector3.back, (_itemAngle * i) + _lineAngle);
        }
    }
    /// <summary>
    /// ������ �� �Ҵ� 
    /// </summary>
    private IEnumerator AssignItemsAndLines()
    {
        while(allCardList == null)
        {
            yield return null; 
        }
        int count = _rouletteDataSO._rouletteItemDataList.Count;
        if (BattleManager.IsHardMode == false)
        {
           // _rouletteText.text = string.Format("Level Up! �귿");
            SetItemCountEasy();
        }
        else if (BattleManager.IsHardMode == true)
        {
            // _rouletteText.text = string.Format("Stage Clear! �귿");
            SetItemCountHard(); 
        }
        SetISpecialtem(count - 2, RouletteItemType.Coupon);
        SetISpecialtem(count - 1, RouletteItemType.Card);

        for (int i = 0; i < count; i++)
        {
            //RouletteItem rouletteItem = Instantiate(_rouletteItem, _itemParent.position, Quaternion.identity, _itemParent);
            //if(i == count -1)
            //{
            //    roulletItemList[i].transform.GetChild(0).localScale = new Vector2(3, 3); 
            //}
            roulletItemList[i].SetUp(_rouletteDataSO._rouletteItemDataList[i]);

            //roulletItemList[i].transform.RotateAround(_itemParent.position, Vector3.back, _itemAngle * i);

            //Transform line = Instantiate(_linePrefab, _lineParent.position, Quaternion.identity, _lineParent);
            //line.transform.RotateAround(_lineParent.position, Vector3.back, (_itemAngle * i) + _lineAngle);
        }
        //SetItem(); 
    }

    /// <summary>
    /// ����ġ ���� 
    /// </summary>
    private void SetWeight()
    {
        int count = _rouletteDataSO._rouletteItemDataList.Count;
        Debug.Log("D");
        for (int i = 0; i < count; i++)
        {
            Debug.Log("S");
            _rouletteDataSO._rouletteItemDataList[i].index = i;

            _accumulatedWeight += _rouletteDataSO._rouletteItemDataList[i]._chance;
            _rouletteDataSO._rouletteItemDataList[i].weght = _accumulatedWeight; 
        }
    }
    #endregion
    /// <summary>
    /// ������ ȹ�� 
    /// </summary>
    private int GetItem()
    {
        int randWeight = Random.Range(0, _accumulatedWeight);
        int count = _rouletteDataSO._rouletteItemDataList.Count;

        for (int i = 0; i < count; i++)
        {
            if (randWeight < _rouletteDataSO._rouletteItemDataList[i].weght)
            {
                return i;
            }
        }
        return 0; 
    }

    /// <summary>
    /// ���� ������ ������ �����ϱ� 
    /// </summary>
    private void SaveItemData(RouletteItemType roulletItemType, int amount)
    {
        if(roulletItemType == RouletteItemType.Coin)
        {
            UserSaveManagerSO.AddMoney(amount);
        }
        else if (roulletItemType == RouletteItemType.Dalgona)
        {
            UserSaveManagerSO.AddDalgona(amount);
        }
        else if(roulletItemType == RouletteItemType.Card)
        {
            UserSaveManagerSO.AddCardData(card, amount);
        }
        else if(roulletItemType == RouletteItemType.Coupon)
        {
            UserSaveManagerSO.AddCoupon(amount); 
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
        _gainedItemPanel.GetComponent< RouletteItem>().SetUp(_rouletteDataSO._rouletteItemDataList[_selectionIndex]);

        int getItemCount = _rouletteDataSO._rouletteItemDataList[_selectionIndex]._itemCount; 
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
        seq.AppendCallback(() => SaveItemData(_rouletteDataSO._rouletteItemDataList[_selectionIndex].rulletItemType, getItemCount ));
    }

}
