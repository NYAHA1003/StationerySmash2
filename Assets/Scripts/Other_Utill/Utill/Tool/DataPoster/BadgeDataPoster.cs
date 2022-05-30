using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;

namespace Utill.Tool
{
    public class BadgeDataPoster : MonoBehaviour
    {
        public BadgeData badgeData;

        [ContextMenu("���� ������ ������Ʈ")]
        /// <summary>
        /// ����Ÿ�Կ� ���� ���� ���������͸� ������Ʈ�Ѵ�
        /// </summary>
        public void UpdateData()
        {
            ServerDataConnect.Instance.PostData(badgeData, ServerDataConnect.DataType.BadgeData);
        }
    }
}