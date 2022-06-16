using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using DG.Tweening;
using Utill.Data;

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
    [SerializeField]
    protected TextMeshProUGUI _itemCountText;

    protected RectTransform _rect;
    protected Sequence sequence;

    public Grade _grade; 
    private bool isFront = false; 
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
  
    public void SetGrade(Grade grade)
    {
        _grade = grade; 
    }
    public void SetSprite(Sprite frontSprite , Sprite backSprite , bool isFront, int itemCount = 1)
    {

        _frontSprite = frontSprite;
        _backSprite = backSprite;
        this.isFront = isFront; 
        if (isFront == true)
        {
            itemImage.sprite = _frontSprite;
            return;
        }
        itemImage.sprite = _backSprite;
        _itemCountText.text = string.Format("X{0}",itemCount);
    }
    public virtual void StopCoroutine()
    {
        sequence.Kill();

    }
    public void Reset()
    {
        StopAllCoroutines();
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one; 
        if(isFront == true)
        {
            itemImage.sprite = _frontSprite;
            return; 
        }
        itemImage.sprite = _backSprite;

    }
}
