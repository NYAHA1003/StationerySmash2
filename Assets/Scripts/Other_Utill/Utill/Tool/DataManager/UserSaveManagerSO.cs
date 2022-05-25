using System;
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
            serverDataConnect.GetStandardUserSaveData(SetUserSaveData);
		}

        /// <summary>
        /// ���� ���̺� ������ ����
        /// </summary>
        /// <param name="userSaveData"></param>
        public static void SetUserSaveData(UserSaveData userSaveData)
		{
            _userSaveData = userSaveData;
		}
	}

}