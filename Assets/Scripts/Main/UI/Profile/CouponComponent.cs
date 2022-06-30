using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Main.Deck;
using Utill.Tool;
using Utill.Data;
public class CouponComponent : MonoBehaviour, IUserData
{
    [SerializeField]
    private TextMeshProUGUI _couponText = null;

    private int _currentCoupon = 0;
    private int _previousCoupon = 0;


    public void Awake()
    {
        UserSaveManagerSO.AddObserver(this);
        _previousCoupon = UserSaveManagerSO.UserSaveData._coupon;
        SetCouponText();
    }

    public void Notify()
    {
        _previousCoupon = UserSaveManagerSO.UserSaveData._coupon;

        SetCouponText();
        StartCoroutine(UpCountingCoupon());
    }

    /// <summary>
    /// 쿠폰 텍스트 값 수정
    /// </summary>
    public void SetCouponText()
    {
        _couponText.text = _previousCoupon.ToString();
    }

    /// <summary>
    /// 쿠폰 업데이트
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpCountingCoupon()
    {
        float interval = 0.05f;
        while (_previousCoupon < _currentCoupon)
        {
            _previousCoupon++;
            SetCouponText();

            yield return new WaitForSeconds(interval);
        }
    }

}
