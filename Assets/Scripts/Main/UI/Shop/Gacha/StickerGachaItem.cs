using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class StickerGachaItem : GachaCard
{
    public override void ActiveAndAnimate()
    {
        gameObject.SetActive(true);

        sequence = DOTween.Sequence(); 
        sequence.Append(transform.DOScaleX(1.3f, 0.3f));
        sequence.Join(transform.DOScaleY(1.3f, 0.3f));
        sequence.Join(transform.DORotate(Vector3.up * 90, 0.3f));
        sequence.AppendCallback(() => itemImage.sprite = _frontSprite);
        
        sequence.Join(transform.DORotate(Vector3.up * 180, 0.5f));
        sequence.Join(transform.DOScaleX(1f, 0.5f));
        sequence.Join(transform.DOScaleY(1f, 0.5f));
    }

}
