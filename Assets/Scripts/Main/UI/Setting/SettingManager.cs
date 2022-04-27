using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvent
{
    void ListenEvent();
}
public class SettingManager : MonoBehaviour
{
    [Header("사운드 매니저")]
    [SerializeField]
    private SoundComponent soundManager;
    [Header("프로필")]
    [SerializeField]
    private ProfileSetting profile;

    private List<IEvent> events = new List<IEvent>(); 
    private void Start()
    {
        events.Add(profile);

        ListenAll(); 
    }
    /// <summary>
    /// 이벤트를 사용하는 클래스가 이벤트에 함수를 등록
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
