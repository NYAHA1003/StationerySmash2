using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utill.Data;
using Utill.Tool;
using Main.Deck;
using Battle.Starategy;

namespace Utill.Tool
{
    [CreateAssetMenu(fileName = "UserSaveManagerSO", menuName = "Scriptable Object/UserSaveManagerSO")]
    public class UserSaveManagerSO : ScriptableObject, Iinitialize
    {
        //����ID �����
        private class UserIDObj
		{
            public string userID;
		}

        //�Ӽ�
        private static UserSaveData _userSaveData = new UserSaveData(); //���� ������
        //������Ƽ
        public static UserSaveData UserSaveData => _userSaveData; //���� ������


        /// <summary>
        /// ���� �����͸� �޾� �ʱ�ȭ
        /// </summary>
        /// <param name="serverDataConnect"></param>
		public void Initialize()
        {
            SetUserID();
        }

        /// <summary>
        /// ���� ���� ������ �����´�
        /// </summary>
        public static void GetUserSaveData()
        {
            ServerDataConnect.Instance.GetUserSaveData();
        }

        /// <summary>
        /// ���� ���� ������ ���ε��Ѵ�
        /// </summary>
        public static void PostUserSaveData()
        {
            ServerDataConnect.Instance.PostUserSaveData();
        }


        /// <summary>
        /// ���� ���̺� ������ ����
        /// </summary>
        /// <param name="userSaveData"></param>
        public static void SetUserSaveData(UserSaveData userSaveData)
        {
            _userSaveData = userSaveData;
            Debug.Log(_userSaveData._name);
		}

        /// <summary>
        /// UserID�� �����Ѵ�
        /// </summary>
        private static void SetUserID()
        {
            //����ID ���� ���
            string path = Application.dataPath + "/" + "UserID.txt";
            
            //���� ��ο� ID�� ������
            if(File.Exists(path))
            {
                //�ش� ID���� �ҷ��´�
                string jsonData = File.ReadAllText(path);
                UserIDObj userIDobj = JsonUtility.FromJson<UserIDObj>(jsonData);
                _userSaveData._userID = userIDobj.userID;
                GetUserSaveData();
            }
            else
            {
                //��ġ�� ���� ID���� �����´�
                var inherenceID = SystemInfo.deviceUniqueIdentifier;
                //���� �ð� ���� �����´�
                var timeID = DateTime.Now.ToString("yyyy-mm-dd-HH-mm");
                
                //��ġ�� ���� ID + ���� �ð����� ���ο� UserID�� �����
                UserIDObj userIDobj = new UserIDObj();
                userIDobj.userID = inherenceID + timeID;
                
                //UserID�� �����Ѵ�
                string jsonData = JsonUtility.ToJson(userIDobj);
                File.WriteAllText(path, jsonData);

                _userSaveData._userID = userIDobj.userID;

                //�⺻���� ������ �߰�
                _userSaveData._currentProfileType = ProfileType.ProPencil;
                _userSaveData._currentPencilCaseType = PencilCaseType.Normal;
                _userSaveData._haveSkinList.Add(SkinType.PencilNormal);
                _userSaveData._haveSkinList.Add(SkinType.SharpNormal);
                _userSaveData._haveSkinList.Add(SkinType.PencilCaseNormal);
                _userSaveData._haveProfileList.Add(ProfileType.ProPencil);
                _userSaveData._havePencilCaseList.Add(PencilCaseType.Normal);
                _userSaveData._haveCardSaveDatas.Add(new CardSaveData()
                {
                    _cardNamingType = CardNamingType.Pencil,
                    _count = 1,
                    _stickerType = StickerType.None,
                    _level = 1,
                    _skinType = SkinType.PencilNormal,
                });
                _userSaveData._haveCardSaveDatas.Add(new CardSaveData()
                {
                    _cardNamingType = CardNamingType.Sharp,
                    _count = 1,
                    _stickerType = StickerType.None,
                    _level = 1,
                    _skinType = SkinType.SharpNormal,
                });


                PostUserSaveData();
            }

        }
	}

}