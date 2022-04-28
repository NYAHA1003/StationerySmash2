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
    /// 미리보기 영상 설정
    /// </summary>
    /// <param name="cardNamingType"></param>
    public void SetVideo(CardNamingType cardNamingType)
    {
        //어드레서블로 영상 가져오기
    }

    /// <summary>
    /// 미리보기 영상 재생
    /// </summary>
    public void PlayVideo()
    {
        _previewVideo.Play();
    }

    /// <summary>
    /// 미리보기 영상 정지
    /// </summary>
    public void StopVideo()
    {
        _previewVideo.Stop();
    }

    /// <summary>
    /// 스크롤에게서 현재 인덱스를 받는다
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
