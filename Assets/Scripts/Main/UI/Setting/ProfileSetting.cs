using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;
using Main.Event;

namespace Main.Setting
{
    public enum ProfileImageType
    {
        Pencil,
        Eraser
    }
    [System.Serializable]
    public class ProfileSetting : IEvent
    {
        [Header("����")]
        [SerializeField]
        private User _user;

        [Header("�г��� �Է¶�")]
        //�г��� �Է¶� 
        [SerializeField]
        private Image inputField;
        [SerializeField]
        private TextMesh inputNameText;
        [SerializeField]
        private Button inputNameBtn;

        [SerializeField]
        private Image[] ProfileImages;
        [SerializeField]
        private GameObject ProfilePanel;

        public void ListenEvent()
        {
            EventManager.StartListening(EventsType.ActiveProfileImgPn, OnActiveProfileImgPn);
            EventManager.StartListening(EventsType.ChangeProfileImage, OnChangeProfileImage);
            EventManager.StartListening(EventsType.ChabgeUserName, OnChangeUserName);
            EventManager.StartListening(EventsType.CloaseAllPn, OnProfileDisabled);
        }
        public void OnChangeUserName()
        {
            _user.userName = inputNameText.text;
            //���� 
        }
        public void OnChangeProfileImage(object profileImgType)
        {
            //_user.profileImage = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite;
            _user.profileImage = ReturnProfileImg((ProfileImageType)profileImgType); ;
            ProfilePanel.SetActive(false);
            //����
        }
        private Sprite ReturnProfileImg(ProfileImageType profileImgType)
        {
            return ProfileImages[(int)profileImgType].sprite;
        }
        public void OnActiveProfileImgPn()
        {
            ProfilePanel.SetActive(!ProfilePanel.activeSelf);
        }

        public void OnProfileDisabled()
        {
            ProfilePanel.SetActive(false);
        }
    }
}