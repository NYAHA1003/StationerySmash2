using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


namespace Main.Coupon
{
    [System.Serializable]
    public class GoogleData
    {
        public string order, result, value, truth;
    }
    public class Coupon : MonoBehaviour
    {
        const string URL = "https://script.google.com/macros/s/AKfycbzac2CtBEkGImHpju0A0fZmZKiFhutRQSmfeMI9h12gBsNUqfmW8mSH-4c3Zq698AF6/exec";
        public GoogleData GD;
        public InputField CouponInput;
        string truth;
        string coupon;
        string value;
        string[] UseCouponList = new string[10]; //구글저장 해줘야함
        string[] UseCouponKind = new string[10];
        int UseCoupon = 0; //구글 저장 해줘야함

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// 입력한 쿠폰이 Null이 아닌지 체크
        /// </summary>
        /// <returns></returns>
        bool SetCoupon()
        {
            coupon = CouponInput.text.Trim();
            if (coupon == "") return false;
            else return true;
        }
        bool OverlapCheck()
        {
            coupon = CouponInput.text.Trim();
            for (int i = 0; i != UseCoupon; i++)
            {
                if (UseCouponList[i] == coupon)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 쿠폰이 없는지 체크하고 시트에 전달함
        /// </summary>
        public void EmptyCheck()
        {
            //입력한 쿠폰 번호가 Null인지 체크
            if (!SetCoupon())
            {
                print("쿠폰번호 빔");
                return;
            }
            if (!OverlapCheck())//체크 안됨 문제 있음
            {
                print("쿠폰번호 중복");
                return;
            }
            //입력한 쿠폰을 시트에 전달
            WWWForm form = new WWWForm();
            form.AddField("order", "coupon");
            form.AddField("coupon", coupon);
            StartCoroutine(Post(form));
        }

        /// <summary>
        /// 쿠폰 전달
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        IEnumerator Post(WWWForm form)
        {
            using (UnityWebRequest link = UnityWebRequest.Post(URL, form))
            {
                yield return link.SendWebRequest();

                //전달해서 받은게 있는지 체크
                if (link.isDone) Response(link.downloadHandler.text);
                else print("응답이 없습니다.");
            }
        }
        void Response(string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            GD = JsonUtility.FromJson<GoogleData>(json);
            if (GD.result == "쿠폰있음")
            {
                truth = GD.truth;
                if (truth != "사용된코드")
                {
                    UseCouponList[UseCoupon] = coupon;
                    UseCouponKind[UseCoupon] = GD.value;
                    UseCoupon++;
                    print("보상 지급");
                }
                else
                {
                    print("사용된 쿠폰코드");
                }
                for (int i = 0; i != UseCoupon; i++)
                {
                    print(UseCouponList[i]);
                    print(UseCouponKind[i]);
                }
                return;
            }
            else
            {
                print("쿠폰번호틀림");
                return;
            }

        }
    }
}
