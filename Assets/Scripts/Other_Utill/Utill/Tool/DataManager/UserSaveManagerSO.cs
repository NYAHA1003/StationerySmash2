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

        private static List<IUserData> _userDataObservers = new List<IUserData>();

        //디버그용
        [SerializeField]
        private CardSaveData _cardAddData;

        [SerializeField]
        private UserSaveData _debugSaveData;
        private static bool _isDebugMod = false;

        /// <summary>
        /// 서버 데이터를 받아 초기화
        /// </summary>
        /// <param name="serverDataConnect"></param>
		public void Initialize()
        {
            _isDebugMod = false;
            SetUserID();
        }

        /// <summary>
        /// 디버그용 초기화
        /// </summary>
        public void DebugInitialize()
        {
            _isDebugMod = true;
            _userSaveData = _debugSaveData;
        }

        /// <summary>
        /// 유저 저장 정보를 가져온다
        /// </summary>
        public static void GetUserSaveData()
        {
            if(_isDebugMod)
			{
                return;
			}
            ServerDataConnect.Instance.GetUserSaveData();
        }

        /// <summary>
        /// 유저 저장 정보를 업로드한다
        /// </summary>
        public static void PostUserSaveData()
        {
            if (_isDebugMod)
            {
                return;
            }
            ServerDataConnect.Instance.PostUserSaveData();
        }


        /// <summary>
        /// 유저 세이브 데이터 설정
        /// </summary>
        /// <param name="userSaveData"></param>
        public static void SetUserSaveData(UserSaveData userSaveData)
        {
            _userSaveData = userSaveData;
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
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

                _userSaveData._userID = userIDobj.userID;

                //기본적인 데이터 추가
                _userSaveData._currentProfileType = ProfileType.ProPencil;
                _userSaveData._currentPencilCaseType = PencilCaseType.Normal;
                _userSaveData._haveSkinList.Add(SkinType.PencilNormal);
                _userSaveData._haveSkinList.Add(SkinType.SharpNormal);
                _userSaveData._haveSkinList.Add(SkinType.PencilCaseNormal);
                _userSaveData._haveProfileList.Add(ProfileType.ProPencil);
                _userSaveData._havePencilCaseList.Add(new PencilCaseSaveData
                {
                    _pencilCaseType = PencilCaseType.Normal,
                    _badgeDatas = null
                });
                _userSaveData._haveThemeSkinTypeList.Add(ThemeSkinType.Normal);
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
                _userSaveData._presetPencilCaseType1 = _userSaveData._currentPencilCaseType;
                _userSaveData._presetCardDatas1.Add(CardNamingType.Pencil);
                _userSaveData._presetCardDatas1.Add(CardNamingType.Sharp);
                _userSaveData._presetPencilCaseType2 = _userSaveData._currentPencilCaseType;
                _userSaveData._presetPencilCaseType3 = _userSaveData._currentPencilCaseType;
                _userSaveData._setPrestIndex = 0;

                PostUserSaveData();
            }

        }

        /// <summary>
        /// 관찰자 추가
        /// </summary>
        /// <param name="observer"></param>
        public static void AddObserver(IUserData observer)
        {
            _userDataObservers.Add(observer);
            observer.Notify();
        }

        [ContextMenu("관찰자들에게 유저 데이터 전달")]
        /// <summary>
        /// 관찰자들에게 유저 데이터 전달
        /// </summary>
        public static void DeliverDataToObserver()
        {
            for (int i = 0; i < _userDataObservers.Count; i++)
            {
                _userDataObservers[i].Notify();
            }
        }

        [ContextMenu("유저데이터에 카드를 추가해 포스트")]
        /// <summary>
        /// 유저 데이터에 카드를 추가한다
        /// </summary>
        public void CardAdd()
		{
            _userSaveData._haveCardSaveDatas.Add(_cardAddData);
            PostUserSaveData();
		}

        /// <summary>
        /// 모든 관찰자를 제거
        /// </summary>
        public static void ClearObserver()
        {
            _userDataObservers.Clear();
        }

        /// <summary>
        /// 경험치 추가
        /// </summary>
        /// <param name="exp"></param>
        public static void AddExp(int exp)
		{
            _userSaveData.AddExp(exp);
            DeliverDataToObserver();
            PostUserSaveData();
        }

        /// <summary>
        /// 돈 추가
        /// </summary>
        /// <param name="exp"></param>
        public static void AddMoney(int money)
        {
            _userSaveData.AddMoney(money);
            DeliverDataToObserver();
            PostUserSaveData();
        }

        /// <summary>
        /// 이름 변경
        /// </summary>
        public static void ChangeName(string name)
        {
            _userSaveData._name = name;
            DeliverDataToObserver();
            PostUserSaveData();
        }

        /// <summary>
        /// 인게임 데이터를 프리셋 데이터에 따라 변경한다
        /// </summary>
        public static void ChangeIngameData(int preset)
        {
            _userSaveData.ChangeIngameData(preset);
            if (_isDebugMod)
            {
                return;
            }
            PostUserSaveData();
		}
    }

}