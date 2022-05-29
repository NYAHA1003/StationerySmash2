using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;

namespace Utill.Tool
{
    public class BadgeDataPoster : MonoBehaviour
    {
        public BadgeData badgeData;

        [ContextMenu("뱃지 데이터 업데이트")]
        /// <summary>
        /// 뱃지타입에 따라 현재 뱃지데이터를 업데이트한다
        /// </summary>
        public void UpdateData()
        {
            ServerDataConnect.Instance.PostData(badgeData, ServerDataConnect.DataType.BadgeData);
        }
    }
}