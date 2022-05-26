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
    public class UserSaveManagerSO : ScriptableObject, IServerInitialize
    {
        //����ID �����
        private class UserIDObj
		{
            public string userID;
		}

        //�Ӽ�
        private static UserSaveData _userSaveData; //���� ������
        //������Ƽ
        public static UserSaveData UserSaveData => _userSaveData; //���� ������

        /// <summary>
        /// ���� �����͸� �޾� �ʱ�ȭ
        /// </summary>
        /// <param name="serverDataConnect"></param>
		public void ServerInitialize(ServerDataConnect serverDataConnect)
        {
            SetUserID();
            //serverDataConnect.GetStandardUserSaveData(SetUserSaveData);
		}

        /// <summary>
        /// ���� ���̺� ������ ����
        /// </summary>
        /// <param name="userSaveData"></param>
        private static void SetUserSaveData(UserSaveData userSaveData)
		{
            _userSaveData = userSaveData;
		}

        /// <summary>
        /// UserID�� �����Ѵ�
        /// </summary>
        private static void SetUserID()
        {
            //����ID ���� ���
            string path = Application.persistentDataPath + "/" + "UserID";
            
            //���� ��ο� ID�� ������
            if(File.Exists(path))
            {
                //�ش� ID���� �ҷ��´�
                string jsonData = File.ReadAllText(path);
                UserIDObj userIDobj = JsonUtility.FromJson<UserIDObj>(jsonData);
                _userSaveData._userID = userIDobj.userID;
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
            }

        }
	}

}