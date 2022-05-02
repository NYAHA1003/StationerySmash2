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

    //����
    private string[] _snsLinks =
    {
        "",
        "https://www.naver.com/",
        "https://discord.gg/FZbCfbkXPr",
        "https://www.youtube.com/",
    }; //SNS��ũ�� SNS Type�� �°� ������ ¥���Ѵ�.

    //�ν����� ���� ����
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
	/// SNSŸ�Կ� ���� SNS��ũ�� ����.
	/// </summary>
	/// <param name="snsType"></param>
	private void OnOpenLink(SNSType snsType)
    {
        Application.OpenURL(_snsLinks[(int)snsType]);
    }
}
