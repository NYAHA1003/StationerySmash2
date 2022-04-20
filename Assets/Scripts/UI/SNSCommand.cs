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

    //����
    private string[] _snsLinks =
    {
        "",
        "https://www.naver.com/",
        "",
    }; //SNS��ũ�� SNS Type�� �°� ������ ¥���Ѵ�.

    /// <summary>
    /// SNSŸ�Կ� ���� SNS��ũ�� ����.
    /// </summary>
    /// <param name="snsType"></param>
    public void OpenLink(SNSType snsType)
    {
        Application.OpenURL(_snsLinks[(int)snsType]);
    }
}
