using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SNSComponent : MonoBehaviour
{
    private enum SNSType
    {
        None = 0,
        Naver = 1,
        Discord = 2,
        Youtube = 2,

    }

    //변수
    private string[] _snsLinks =
    {
        "",
        "https://www.naver.com/",
        "https://discord.gg/FZbCfbkXPr",
        "https://www.youtube.com/",
    }; //SNS링크들 SNS Type에 맞게 순서를 짜야한다.

    //인스펙터 참조 변수
    [SerializeField]
    private Button _naverButton = null;
    [SerializeField]
    private Button _discordButton = null;
    [SerializeField]
    private Button _youtubeButton = null;
    private void Start()
	{
        _naverButton.onClick.AddListener(() => OnOpenLink(SNSType.Naver));
        _discordButton.onClick.AddListener(() => OnOpenLink(SNSType.Discord));
        _youtubeButton.onClick.AddListener(() => OnOpenLink(SNSType.Youtube));

    }

	/// <summary>
	/// SNS타입에 따라 SNS링크를 연다.
	/// </summary>
	/// <param name="snsType"></param>
	private void OnOpenLink(SNSType snsType)
    {
        Application.OpenURL(_snsLinks[(int)snsType]);
    }
}
