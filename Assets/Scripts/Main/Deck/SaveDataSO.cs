using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Main.Deck
{
    [CreateAssetMenu(fileName = "SaveData", menuName = "Scriptable Object/SaveDataSO")]
    public class SaveDataSO : ScriptableObject
    {
        //저장데이터(레벨, 가지고있는지) 저장
        public UserSaveData userSaveData = new UserSaveData();

        private List<IUserData> _userDataObservers = new List<IUserData>();

        /// <summary>
        /// 관찰자 추가
        /// </summary>
        /// <param name="observer"></param>
        public void AddObserver(IUserData observer)
        {
            _userDataObservers.Add(observer);
        }

        /// <summary>
        /// 관찰자들에게 유저 데이터 전달
        /// </summary>
        public void DeliverDataToObserver()
        {
            for (int i = 0; i < _userDataObservers.Count; i++)
            {
                _userDataObservers[i].Notify(ref userSaveData);
            }
        }


    }
}