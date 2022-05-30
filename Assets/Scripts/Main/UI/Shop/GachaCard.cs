using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using DG.Tweening; 

public class GachaCard : MonoBehaviour
{
    [SerializeField]
    private float duration;
    [SerializeField]
    private Sprite _backSprite;
    [SerializeField]
    private Sprite _frontSprite;
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private Image CardFrame; 
    [SerializeField]
    private TextMeshProUGUI _itemName;

    private RectTransform _rect;
    private Sequence sequence;

    private void Awake()
    {
        if (_rect == null)
            _rect = itemImage.GetComponent<RectTransform>();
    }
    private void Update()
    {
        if (itemImage.transform.rotation.y >= 90 && itemImage.transform.rotation.y < 270)
        {
            itemImage.sprite = _backSprite;
        }
        else
        {
            itemImage.sprite = _frontSprite;
        }
    }
    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeSprite()); 

        sequence = DOTween.Sequence();
        sequence.Append(itemImage.transform.DORotate(Vector3.up * 360 * 10, duration * 10, RotateMode.FastBeyond360).SetEase(Ease.OutCubic));
        sequence.Join(transform.DOScale(new Vector2(2, 2), duration * 10).SetEase(Ease.InCubic));
        sequence.Append(itemImage.DOFade(0.5f,0.5f));
        sequence.Join(transform.DOScale(new Vector2(1.5f, 1.5f), 0.5f));
        sequence.Append(itemImage.DOFade(1f, 0.3f));
        sequence.Join(transform.DOScale(new Vector2(2f, 2f), 0.3f));

        sequence.AppendCallback(() =>
        {
            // 카드 틀 액티브
        });
        //transform.DORotate(new Vector3(0, 360, 0), duration, RotateMode.FastBeyond360).SetLoops(10,LoopType.Restart);
    }

    IEnumerator ChangeSprite()
    {
        float rotationY; 
        while (gameObject.activeSelf == true)
        {
            rotationY = _rect.eulerAngles.y; 
            Debug.Log("rotationY : " + rotationY) ;
            if (rotationY >= 90 && rotationY < 270)
            {
                itemImage.sprite = _backSprite;
            }
            else
            {
                itemImage.sprite = _frontSprite;
            }

            yield return null;
        }
        
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
        gameObject.SetActive(false); 
    }
}
