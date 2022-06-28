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
    // 기본(최소) 획득 코인 
 
    [SerializeField]
    private List<RouletteItemData> _rouletteItemDataList = new List<RouletteItemData>();
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

    private int _selectionIndex = 0; // 룰렛에서 선택된 아이템 

    private int _accumulatedWeight; // 가중치 계산을 위한 변수 
    

    [SerializeField]
    private int _standardMoney; // 스테이지에 따라 얻는 최소 돈  

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

    #region 데이터 설정 
    /// <summary>
    /// 스테이지에 따라 달라지는 얻는 돈의 양 설정 (스테이지 룰렛)  
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
    /// 65% : 돈      35% : 달고나 
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
    ///// 분실물 설정  
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
        // 모든 분실물들 가져오고 
        // 리스트 가져와서 랜덤으로 n개 가져와 
        // 스프라이트 가져오고, 
        // 개수 설정 
    }
    /// <summary>
    /// 룰렛에 나올 아이템과 선 생성 
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
    /// 가중치 설정 
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
    /// 아이템 획득 
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
    /// 뽑은 아이템 데이터 저장하기 
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
    // 클릭시 버튼 없애고 
    // 아이템 뽑고 이에 따라 회전값 i * _itemAngle ( 이게 멈출 곳 ) 
    // 총 회전 값 int angle = 360 * angleSpeed * angleDuration 
    // 돌리는 게 끝나면 아이템 패널 뜨고 
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
