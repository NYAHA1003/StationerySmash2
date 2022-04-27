using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvent
{
    void ListenEvent();
}
public class SettingManager : MonoBehaviour
{
    [Header("���� �Ŵ���")]
    [SerializeField]
    private SoundComponent soundManager;
    [Header("������")]
    [SerializeField]
    private ProfileSetting profile;

    private List<IEvent> events = new List<IEvent>(); 
    private void Start()
    {
        events.Add(profile);

        ListenAll(); 
    }
    /// <summary>
    /// �̺�Ʈ�� ����ϴ� Ŭ������ �̺�Ʈ�� �Լ��� ���
    /// </summary>
    private void ListenAll()
    {
        foreach (IEvent ievent in events)
            ievent.ListenEvent(); 
    }
    public void OnYoutube()
    {

    }
    public void OnDiscord()
    {

    }
    public void OnNaverCafe()
    {

    }
    public void OnChangeProfileImage()
    {

    }
    public void OnChangeNickname()
    {

    }
    public void OnEffectSound()
    {

    }
    public void OnBgm()
    {

    }
    public void OnInputCoupon()
    {

    }
    public void OnTutorial()
    {

    }
    public void OnCredit()
    {

    }
    public void OnLanguage()
    {

    }
}
