using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SNSCommand : MonoBehaviour
{
    public enum SNSType
    {
        None = 0,
        Naver = 1,
        Discord = 2,

    }

    //변수
    private string[] _snsLinks =
    {
        "",
        "https://www.naver.com/",
        "",
    }; //SNS링크들 SNS Type에 맞게 순서를 짜야한다.

    /// <summary>
    /// SNS타입에 따라 SNS링크를 연다.
    /// </summary>
    /// <param name="snsType"></param>
    public void OpenLink(SNSType snsType)
    {
        Application.OpenURL(_snsLinks[(int)snsType]);
    }
}
