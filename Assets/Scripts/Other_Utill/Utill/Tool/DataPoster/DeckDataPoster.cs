using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;

namespace Utill.Tool
{
	public class DeckDataPoster : MonoBehaviour
    {
        public CardData cardData;

        [ContextMenu("ī�� ������ ������Ʈ")]
        /// <summary>
        /// ī��Ÿ�Կ� ���� ���� ī�嵥���͸� ������Ʈ�Ѵ�
        /// </summary>
        public void UpdateData()
        {
            ServerDataConnect.Instance.PostData(cardData, ServerDataConnect.DataType.DeckData);
        }

        [ContextMenu("ī�� Json ������Ʈ")]
        public void Json()
		{
            Debug.Log(JsonUtility.ToJson(cardData));
		}
    }

}
