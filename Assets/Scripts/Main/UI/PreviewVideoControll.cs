using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;
using Main.Scroll;
using Main.Deck;
using UnityEngine.Video;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class PreviewVideoControll : MonoBehaviour, IScroll
{
    [SerializeField]
    private AgentScroll _scrollObj;

    [SerializeField]
    private VideoPlayer _previewVideo;

    [SerializeField]
    private int _viewIndex = 1; //�� ��° �гο��� ���̰� ����

    private void Start()
    {
        _scrollObj.AddObserver(this);
        _previewVideo.gameObject.SetActive(false);
    }

    /// <summary>
    /// �̸����� ���� ����
    /// </summary>
    /// <param name="cardNamingType"></param>
    public void SetVideo(CardNamingType cardNamingType)
    {
        string name = System.Enum.GetName(typeof(CardNamingType), cardNamingType) + "_Video";
        Addressables.LoadAssetAsync<VideoClip>(name).Completed +=
          (AsyncOperationHandle<VideoClip> obj) =>
          {
              _previewVideo.clip = obj.Result;
          };
    }

    /// <summary>
    /// �̸����� ���� ���
    /// </summary>
    public void PlayVideo()
    {
        _previewVideo.Play();
        _previewVideo.gameObject.SetActive(true);
    }

    /// <summary>
    /// �̸����� ���� ����
    /// </summary>
    public void StopVideo()
    {
        _previewVideo.Stop();
        _previewVideo.gameObject.SetActive(false);
    }

    /// <summary>
    /// ��ũ�ѿ��Լ� ���� �ε����� �޴´�
    /// </summary>
    /// <param name="scrollIndex"></param>
    public void Notify(int scrollIndex)
    {
        if(scrollIndex == _viewIndex)
        {
            PlayVideo();
        }
        else
        {
            StopVideo();
        }
    }
}
