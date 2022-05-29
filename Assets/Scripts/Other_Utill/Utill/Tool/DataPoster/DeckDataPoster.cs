using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;

namespace Utill.Tool
{
	public class DeckDataPoster : MonoBehaviour
    {
        public CardData cardData;

        [ContextMenu("카드 데이터 업데이트")]
        /// <summary>
        /// 카드타입에 따라 현재 카드데이터를 업데이트한다
        /// </summary>
        public void UpdateData()
        {
            ServerDataConnect.Instance.PostData(cardData, ServerDataConnect.DataType.DeckData);
        }

        [ContextMenu("카드 Json 업데이트")]
        public void Json()
		{
            Debug.Log(JsonUtility.ToJson(cardData));
		}
    }

}
