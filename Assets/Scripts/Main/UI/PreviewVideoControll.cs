using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;

public class PreviewVideoControll : MonoBehaviour, IScroll
{
    [SerializeField]
    private AgentScroll _scrollObj;

    [SerializeField]
    private VideoPlayer _previewVideo;

    private void Start()
    {
        _scrollObj.AddObserver(this);
    }

    /// <summary>
    /// �̸����� ���� ����
    /// </summary>
    /// <param name="cardNamingType"></param>
    public void SetVideo(CardNamingType cardNamingType)
    {
        //��巹����� ���� ��������
    }

    /// <summary>
    /// �̸����� ���� ���
    /// </summary>
    public void PlayVideo()
    {
        _previewVideo.Play();
    }

    /// <summary>
    /// �̸����� ���� ����
    /// </summary>
    public void StopVideo()
    {
        _previewVideo.Stop();
    }

    /// <summary>
    /// ��ũ�ѿ��Լ� ���� �ε����� �޴´�
    /// </summary>
    /// <param name="scrollIndex"></param>
    public void Notify(int scrollIndex)
    {
        if(scrollIndex == 1)
        {
            PlayVideo();
        }
        else
        {
            StopVideo();
        }
    }
}
