using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Main.Event;
using Utill.Data; 
public class BadgeGachaItem : GachaCard
{
    [SerializeField]
    protected float duration;

    private bool _isDotween; // ��Ʈ�� �������ΰ� 
    /// <summary>
    /// Ȱ��ȭ �� �ִϸ��̼� ���� 
    /// </summary>
    public override void ActiveAndAnimate()
    {
        gameObject.SetActive(true);
        _isDotween = true; 

        if(this.gameObject.activeInHierarchy)
        {
            StartCoroutine(ChangeSprite());
        }

        sequence = DOTween.Sequence();
        sequence.Append(itemImage.transform.DORotate(Vector3.up * 360 * 10, duration * 10, RotateMode.FastBeyond360).SetEase(Ease.OutCubic));
        sequence.Join(transform.DOScale(new Vector2(2, 2), duration * 10).SetEase(Ease.InCubic));
        sequence.Append(itemImage.DOFade(0.5f, 0.5f));
        sequence.Join(transform.DOScale(new Vector2(1.5f, 1.5f), 0.5f));
        sequence.Append(itemImage.DOFade(1f, 0.3f));
        sequence.Join(transform.DOScale(new Vector2(2f, 2f), 0.3f));
        sequence.AppendCallback(() =>
        {
            _isDotween = false; 
            EventManager.Instance.TriggerEvent(EventsType.ActiveNextBtn);
            // next��ư Ȱ��ȭ 
        });

        sequence.AppendCallback(() =>
        {
            // ī�� Ʋ ��Ƽ��
        });
    }

    /// <summary>
    /// ����������, ���ᰪ �ٷ� ����ǵ��� 
    /// </summary>
    public override void StopCoroutine()
    {
        base.StopCoroutine();
        itemImage.transform.rotation = Quaternion.identity;
        transform.localScale = new Vector2(2, 2);
        itemImage.sprite = _frontSprite;
    }

    /// <summary>
    /// ȸ�������� ���� �ո� �̹��� �޸��̹��� �� �ٲٱ� 
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeSprite()
    {
        float rotationY;
        while (_isDotween == true)
        {
            rotationY = _rect.eulerAngles.y;
            //Debug.Log("rotationY : " + rotationY);
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

}
