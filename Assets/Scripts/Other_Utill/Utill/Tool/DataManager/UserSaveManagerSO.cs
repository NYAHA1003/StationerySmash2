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
        //유저ID 저장용
        private class UserIDObj
		{
            public string userID;
		}

        //속성
        private static UserSaveData _userSaveData = new UserSaveData(); //유저 데이터
        //프로퍼티
        public static UserSaveData UserSaveData => _userSaveData; //유저 데이터


        /// <summary>
        /// 서버 데이터를 받아 초기화
        /// </summary>
        /// <param name="serverDataConnect"></param>
		public void Initialize()
        {
            SetUserID();
        }

        /// <summary>
        /// 유저 저장 정보를 가져온다
        /// </summary>
        public static void GetUserSaveData()
        {
            ServerDataConnect.Instance.GetUserSaveData();
        }

        /// <summary>
        /// 유저 저장 정보를 업로드한다
        /// </summary>
        public static void PostUserSaveData()
        {
            ServerDataConnect.Instance.PostUserSaveData();
        }


        /// <summary>
        /// 유저 세이브 데이터 설정
        /// </summary>
        /// <param name="userSaveData"></param>
        public static void SetUserSaveData(UserSaveData userSaveData)
        {
            _userSaveData = userSaveData;
            Debug.Log(_userSaveData._name);
		}

        /// <summary>
        /// UserID를 설정한다
        /// </summary>
        private static void SetUserID()
        {
            //유저ID 저장 경로
            string path = Application.dataPath + "/" + "UserID.txt";
            
            //저장 경로에 ID가 있을시
            if(File.Exists(path))
            {
                //해당 ID값을 불러온다
                string jsonData = File.ReadAllText(path);
                UserIDObj userIDobj = JsonUtility.FromJson<UserIDObj>(jsonData);
                _userSaveData._userID = userIDobj.userID;
                GetUserSaveData();
            }
            else
            {
                //장치의 고유 ID값을 가져온다
                var inherenceID = SystemInfo.deviceUniqueIdentifier;
                //현재 시간 값을 가져온다
                var timeID = DateTime.Now.ToString("yyyy-mm-dd-HH-mm");
                
                //장치의 고유 ID + 현재 시간으로 새로운 UserID를 만든다
                UserIDObj userIDobj = new UserIDObj();
                userIDobj.userID = inherenceID + timeID;
                
                //UserID를 저장한다
                string jsonData = JsonUtility.ToJson(userIDobj);
                File.WriteAllText(path, jsonData);

                _userSaveData._userID = userIDobj.userID;

                //기본적인 데이터 추가
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