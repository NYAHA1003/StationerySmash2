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
        //속성
        private static UserSaveData _userSaveData; //유저 데이터

        //프로퍼티
        public static UserSaveData UserSaveData => _userSaveData; //유저 데이터

        /// <summary>
        /// 서버 데이터를 받아 초기화
        /// </summary>
        /// <param name="serverDataConnect"></param>
		public void ServerInitialize(ServerDataConnect serverDataConnect)
		{
            serverDataConnect.GetStandardUserSaveData(SetUserSaveData);
		}

        /// <summary>
        /// 유저 세이브 데이터 설정
        /// </summary>
        /// <param name="userSaveData"></param>
        public static void SetUserSaveData(UserSaveData userSaveData)
		{
            _userSaveData = userSaveData;
		}
	}

}