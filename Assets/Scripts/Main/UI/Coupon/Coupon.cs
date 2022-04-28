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
        string[] UseCouponList = new string[10]; //�������� �������
        string[] UseCouponKind = new string[10];
        int UseCoupon = 0; //���� ���� �������

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// �Է��� ������ Null�� �ƴ��� üũ
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
        /// ������ ������ üũ�ϰ� ��Ʈ�� ������
        /// </summary>
        public void EmptyCheck()
        {
            //�Է��� ���� ��ȣ�� Null���� üũ
            if (!SetCoupon())
            {
                print("������ȣ ��");
                return;
            }
            if (!OverlapCheck())//üũ �ȵ� ���� ����
            {
                print("������ȣ �ߺ�");
                return;
            }
            //�Է��� ������ ��Ʈ�� ����
            WWWForm form = new WWWForm();
            form.AddField("order", "coupon");
            form.AddField("coupon", coupon);
            StartCoroutine(Post(form));
        }

        /// <summary>
        /// ���� ����
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        IEnumerator Post(WWWForm form)
        {
            using (UnityWebRequest link = UnityWebRequest.Post(URL, form))
            {
                yield return link.SendWebRequest();

                //�����ؼ� ������ �ִ��� üũ
                if (link.isDone) Response(link.downloadHandler.text);
                else print("������ �����ϴ�.");
            }
        }
        void Response(string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            GD = JsonUtility.FromJson<GoogleData>(json);
            if (GD.result == "��������")
            {
                truth = GD.truth;
                if (truth != "�����ڵ�")
                {
                    UseCouponList[UseCoupon] = coupon;
                    UseCouponKind[UseCoupon] = GD.value;
                    UseCoupon++;
                    print("���� ����");
                }
                else
                {
                    print("���� �����ڵ�");
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
                print("������ȣƲ��");
                return;
            }

        }
    }
}
