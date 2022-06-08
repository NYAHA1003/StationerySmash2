using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using DG.Tweening; 

public abstract class GachaCard : MonoBehaviour
{

    [SerializeField]
    protected Sprite _backSprite;
    [SerializeField]
    protected Sprite _frontSprite;
    [SerializeField]
    protected Image itemImage;
    [SerializeField]
    protected Image CardFrame; 
    [SerializeField]
    protected TextMeshProUGUI _itemName;

    protected RectTransform _rect;


    private void Awake()
    {
        if (_rect == null)
            _rect = itemImage.GetComponent<RectTransform>();
    }

    public abstract void ActiveAndAnimate();

    /// <summary>
    /// 하나의 아이템 확대 강조 
    /// </summary>
    public void StressOneItem()
    {
        gameObject.SetActive(true);
        gameObject.transform.DOScale(3, 0.3f); 
    }
    
    private void OnEnable()
    {
        Reset(); 
    }

  
    public void SetSprite(Sprite frontSprite, Sprite backSprite)
    {
        _frontSprite = frontSprite;
        _backSprite = backSprite;

        itemImage.sprite = _frontSprite; 
    }
    public void Reset()
    {
        StopAllCoroutines();
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one; 
    }
}
