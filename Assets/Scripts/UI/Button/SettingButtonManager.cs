using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util; 
public class SettingButtonManager : MonoBehaviour
{
    [SerializeField]
    private ProfileSetting profileSetting;

    private void Start()
    {
        profileSetting.ListenEvent(); 
    }

    public void OnActiveProfilePn(ProfileImageType index)
    {
        EventManager.TriggerEvent(EventsType.ActiveProfileImgPn, index);
    }

    public void OnChangeProfileImage()
    {
        EventManager.TriggerEvent(EventsType.ChangeProfileImage);
    }

    public void OnChangeUserName()
    {
        EventManager.TriggerEvent(EventsType.ChabgeUserName);
    }


}

