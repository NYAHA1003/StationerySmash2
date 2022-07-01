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
    Daily, // 일일룰렛
    Stage, // 스테이지 룰렛
}


public class RouletteComponent : MonoBehaviour
{
    // 1 , 1.5 , 2 , 3
    // 50% 25% 18% 7%
    // 기본(최소) 획득 코인 
    [SerializeField]
    private RouletteItem _rouletteItem; // 룰렛 아이템 프리팹
    [SerializeField]
    private Transform _linePrefab; // 선 프리팹 
    [SerializeField]
    private Transform _itemParent; // 룰렛 아이템 부모 
    [SerializeField]
    private Transform _lineParent; // 룰렛 선 부모 

    private float _itemAngle; // 룰렛 아이템 각도 ( 개수에 따라 달라짐 ) 
    private float _lineAngle; // 선 각도 ( 개수에 따라 달라짐 )

    [SerializeField]
    private int _spinDuration; // 회전 시간
    [SerializeField]
    private Transform _spinningRoulette; // 회전하는 회전판 
    
    [SerializeField]
    private Button _spinButton; // 돌리기 버튼 

    [SerializeField]
    private RectTransform _gainedItemPanel; // 얻은 아이템 띄우는 패널 
    [SerializeField]
    private RectTransform _pig; // 돼지 저금통
    [SerializeField]
    private GameObject _rouletteCanvas;
    [SerializeField]
    private GameObject _blackImage; // 뽑기 후 나타나는 검은 배경 이미지 
    [SerializeField]
    private TextMeshProUGUI _rouletteText; // 어떤 룰렛인지 설명 텍스트  

    private int _selectionIndex = 0; // 룰렛에서 선택된 아이템 

    private int _accumulatedWeight; // 가중치 계산을 위한 변수 

    [SerializeField]
    private RouletteType _rouletteType;
    [SerializeField]
    private int _standardMoney; // 스테이지에 따라 얻는 최소 돈  

    private Vector3 _originPigPos;

    [SerializeField]
    private RouletteDataSO _rouletteDataSO;  // 룰렛에 뜰 돈, 달고나 개수와 스프라이트 정보 

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

    [ContextMenu("테스트")]
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
            _rouletteText.text = string.Format("레벨 업!");
        }
        else if (_rouletteType == RouletteType.Stage)
        {
            _rouletteText.text = string.Format("스테이지 클리어!");
        }

    }
    /// <summary>
    ///  룰렛 캔버스 활성화 비활성화 , 비활성화 -> 활성화시 값들 재설정 
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
    /// 값 재설정
    /// </summary>
    private void ResetValues()
    {
        _spinButton.gameObject.SetActive(true);
        _blackImage.SetActive(false);
        _gainedItemPanel.gameObject.SetActive(false);
        _gainedItemPanel.anchoredPosition = Vector2.zero;
        _pig.anchoredPosition = _originPigPos;
    }

    private int _notCoinOrDalgona = 2; // 코인과 달고나가 아닌 아이템 개수
    #region 데이터 설정 
    /// <summary>
    /// 스테이지에 따라 달라지는 얻는 돈의 양 설정 (하드 룰렛)  
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
    /// 65% : 돈      35% : 달고나 ( 이지 룰렛 ) 
    /// </summary>
    private void SetItemCountHard()
    {
        int count = _rouletteDataSO._rouletteItemDataList.Count;
        for (int i = 0; i < count - _notCoinOrDalgona; i++)
        {
            if(Random.Range(0, 100) < 65) // 코인일 경우 
            {
                _rouletteDataSO._rouletteItemDataList[i]._itemCount = _rouletteDataSO.coinCounts[i];
                _rouletteDataSO._rouletteItemDataList[i].rulletItemType = RouletteItemType.Coin;
                _rouletteDataSO._rouletteItemDataList[i]._itemImage = _rouletteDataSO.itemSprites[(int)RouletteItemType.Coin];   
            }
            else // 달고나일 경우 
            {
                _rouletteDataSO._rouletteItemDataList[i]._itemCount = _rouletteDataSO.dalgonaCounts[i];
                _rouletteDataSO._rouletteItemDataList[i].rulletItemType = RouletteItemType.Dalgona;
                _rouletteDataSO._rouletteItemDataList[i]._itemImage = _rouletteDataSO.itemSprites[(int)RouletteItemType.Dalgona];

            }
        }
    }

    /// <summary>
    ///  룰렛으로 뽑을 수 있는 분실물 설정 
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
    ///// 특별 아이템 설정  ( 분실물, 쿠폰 ) 
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
    /// 선, 아이템 생성 
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
    /// 아이템 값 할당 
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
           // _rouletteText.text = string.Format("Level Up! 룰렛");
            SetItemCountEasy();
        }
        else if (BattleManager.IsHardMode == true)
        {
            // _rouletteText.text = string.Format("Stage Clear! 룰렛");
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
    /// 가중치 설정 
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
    /// 아이템 획득 
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
    /// 뽑은 아이템 데이터 저장하기 
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
    // 클릭시 버튼 없애고 
    // 아이템 뽑고 이에 따라 회전값 i * _itemAngle ( 이게 멈출 곳 ) 
    // 총 회전 값 int angle = 360 * angleSpeed * angleDuration 
    // 돌리는 게 끝나면 아이템 패널 뜨고 
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
