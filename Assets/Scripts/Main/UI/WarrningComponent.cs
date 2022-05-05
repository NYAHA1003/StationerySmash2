using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utill;
using TMPro;

public class WarrningComponent : MonoBehaviour
{
    //인스펙터 변수
    [SerializeField]
    private RectTransform _warrningTransform = null;
    [SerializeField]
    private TextMeshProUGUI _textMeshPro = null;
    //변수
    private Tweener _tweener = null;

    public void Start()
    {
        Initialized();
    }


    /// <summary>
    /// 초기화
    /// </summary>
    public void Initialized()
    {
        _tweener = _warrningTransform.DOAnchorPosY(-2000, 1).SetDelay(1).SetAutoKill(false).
            OnComplete(() => _warrningTransform.gameObject.SetActive(false));
    }

    /// <summary>
    /// 경고 메시지 출력
    /// </summary>
    /// <param name="text"></param>
    public void SetWarrning(string text)
    {
        _warrningTransform.gameObject.SetActive(true);
        _textMeshPro.text = text;
        _tweener.Pause();
        _warrningTransform.anchoredPosition = new Vector2(0, 0);
        _tweener.Restart();
    }

}
