using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Main.Deck
{
    [CreateAssetMenu(fileName = "SaveData", menuName = "Scriptable Object/SaveDataSO")]
    public class SaveDataSO : ScriptableObject
    {
        //���嵥����(����, �������ִ���) ����
        public UserSaveData userSaveData = new UserSaveData();

        private List<IUserData> _userDataObservers = new List<IUserData>();

        /// <summary>
        /// ������ �߰�
        /// </summary>
        /// <param name="observer"></param>
        public void AddObserver(IUserData observer)
        {
            _userDataObservers.Add(observer);
        }

        /// <summary>
        /// �����ڵ鿡�� ���� ������ ����
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