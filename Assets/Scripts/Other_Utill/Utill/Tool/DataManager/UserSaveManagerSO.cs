using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utill.Data;
using Utill.Load;
using Utill.Tool;
using Main.Deck;
using Battle.Starategy;

namespace Utill.Tool
{
    [CreateAssetMenu(fileName = "UserSaveManagerSO", menuName = "Scriptable Object/UserSaveManagerSO")]
    public class UserSaveManagerSO : ScriptableObject, Iinitialize
    {
        //�Ӽ�
        private static UserSaveData _userSaveData = new UserSaveData(); //���� ������
        //������Ƽ
        public static UserSaveData UserSaveData => _userSaveData; //���� ������

        private static List<IUserData> _userDataObservers = new List<IUserData>();

        //����׿�
        [SerializeField]
        private CardSaveData _cardAddData;

        [SerializeField]
        private UserSaveData _debugSaveData;
        private static bool _isDebugMod = false;

        /// <summary>
        /// ���� �����͸� �޾� �ʱ�ȭ
        /// </summary>
        /// <param name="serverDataConnect"></param>
		public void Initialize()
        {
            _isDebugMod = false;
            SetUserID();
        }

        /// <summary>
        /// ����׿� �ʱ�ȭ
        /// </summary>
        public void DebugInitialize()
        {
            _isDebugMod = true;
            _userSaveData = _debugSaveData;
            SetUserID();
            _debugSaveData = _userSaveData;
        }

        /// <summary>
        /// ���� ���� ������ �����´�
        /// </summary>
        public static void GetUserSaveData()
        {
            if(_isDebugMod)
			{
             
                return;
			}
            else
            {
                ServerDataConnect.Instance.GetUserSaveData();
            }
        }

        /// <summary>
        /// ���� ���� ������ ���ε��Ѵ�
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
        /// ������ ����
        /// </summary>
        /// <param name="profileType"></param>
        public static void ChangeProfileType(ProfileType profileType)
		{
            UserSaveData._currentProfileType = profileType;
            PostUserSaveData();
            DeliverDataToObserver();
        }

        /// <summary>
        /// ���� ���̺� ������ ����
        /// </summary>
        /// <param name="userSaveData"></param>
        public static void SetUserSaveData(UserSaveData userSaveData)
        {
            _userSaveData = userSaveData;
		}

        /// <summary>
        /// UserID�� �����Ѵ�
        /// </summary>
        private static void SetUserID()
        {
            //����ID ���� ���
            string path = Application.persistentDataPath + "/" + "UserID.txt";
            
            //���� ��ο� ID�� ������
            if(File.Exists(path))
            {
                //�ش� ID���� �ҷ��´�
                string jsonData = File.ReadAllText(path);
                UserSaveData saverData = JsonUtility.FromJson<UserSaveData>(jsonData);
                _userSaveData = saverData;
                GetUserSaveData();
            }
            else
            {
                //��ġ�� ���� ID���� �����´�
                var inherenceID = SystemInfo.deviceUniqueIdentifier;
                //���� �ð� ���� �����´�
                var timeID = DateTime.Now.ToString("yyyy-mm-dd-HH-mm");
                //��ġ�� ���� ID + ���� �ð����� ���ο� UserID�� �����


#if UNITY_EDITOR
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
#endif
                _userSaveData._userID = inherenceID + timeID;

                //�⺻���� ������ �߰�
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

                //UserID�� �����Ѵ�
                string jsonData = JsonUtility.ToJson(_userSaveData);
                File.WriteAllText(path, jsonData);

                PostUserSaveData();
            }

        }

        /// <summary>
        /// ������ �߰�
        /// </summary>
        /// <param name="observer"></param>
        public static void AddObserver(IUserData observer)
        {
            _userDataObservers.Add(observer);
            observer.Notify();
        }

        [ContextMenu("�����ڵ鿡�� ���� ������ ����")]
        /// <summary>
        /// �����ڵ鿡�� ���� ������ ����
        /// </summary>
        public static void DeliverDataToObserver()
        {
            for (int i = 0; i < _userDataObservers.Count; i++)
            {
                _userDataObservers[i].Notify();
            }
        }

        [ContextMenu("���������Ϳ� ī�带 �߰��� ����Ʈ")]
        /// <summary>
        /// ����׿� ���� �����Ϳ� ī�带 �߰��Ѵ�
        /// </summary>
        public void DebugCardAdd()
		{
            _userSaveData._haveCardSaveDatas.Add(_cardAddData);
            PostUserSaveData();
		}

        /// <summary>
        /// ī�带 ���� �����Ϳ� �߰��Ѵ�
        /// </summary>
        /// <param name="cardData"></param>
        public static void AddCardData(CardData cardData, int count)
        {
            CardSaveData cardSaveData = _userSaveData._haveCardSaveDatas.Find(x => x._cardNamingType == cardData._cardNamingType);
            if (cardSaveData != null)
			{
                cardSaveData._count += count;
                DeckDataManagerSO.FindHaveCardData(cardData._cardNamingType)._count += count;
                DeckDataManagerSO.SetHaveDataList();
            }
			else
            {
                cardSaveData = new CardSaveData()
                {
                    _cardNamingType = cardData._cardNamingType,
                    _skinType = cardData._skinData._skinType,
                    _stickerType = StickerType.None,
                    _level = 1,
                    _count = 1,
                };
                _userSaveData._haveSkinList.Add(cardSaveData._skinType);
                _userSaveData._haveCardSaveDatas.Add(cardSaveData);
                if(cardData._unitType != UnitType.None)
				{
                    UnitDataManagerSO.AddHaveData(cardData._unitType);
				}

                DeckDataManagerSO.SetHaveDataList();
            }
            PostUserSaveData();
        }

        /// <summary>
        /// ��� �����ڸ� ����
        /// </summary>
        public static void ClearObserver()
        {
            _userDataObservers.Clear();
        }

        /// <summary>
        /// ����ġ �߰�
        /// </summary>
        /// <param name="exp"></param>
        public static void AddExp(int exp)
		{
            _userSaveData.AddExp(exp);
            DeliverDataToObserver();
            PostUserSaveData();
        }

        /// <summary>
        /// �� �߰�
        /// </summary>
        /// <param name="exp"></param>
        public static void AddMoney(int money)
        {
            _userSaveData.AddMoney(money);
            DeliverDataToObserver();
            PostUserSaveData();
        }

        /// <summary>
        /// �ް� �߰�
        /// </summary>
        /// <param name="dalgona"></param>
        public static void AddDalgona(int dalgona)
        {
            _userSaveData.AddDalgona(dalgona);
            DeliverDataToObserver();
            PostUserSaveData();
        }

        /// <summary>
        /// �̸� ����
        /// </summary>
        public static void ChangeName(string name)
        {
            _userSaveData._name = name;
            DeliverDataToObserver();
            PostUserSaveData();
        }

        /// <summary>
        /// �ΰ��� �����͸� ������ �����Ϳ� ���� �����Ѵ�
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
    
        /// <summary>
        /// ���������� Ŭ������ �������� ���
        /// </summary>
        public static void SetLastClearStage(BattleStageType battleStageType)
		{
            if (battleStageType > UserSaveData._lastPlayStage)
			{
                UserSaveData._lastPlayStage = battleStageType;
            }
		}
    }

}