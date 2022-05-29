using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;

namespace Utill.Tool
{
    public class StickerDataPoster : MonoBehaviour
    {
        public StickerData stickerData;

        [ContextMenu("��ƼĿ ������ ������Ʈ")]
        /// <summary>
        /// ��ƼĿŸ�Կ� ���� ���� ��ƼĿ�����͸� ������Ʈ�Ѵ�
        /// </summary>
        public void UpdateData()
        {
            ServerDataConnect.Instance.PostData(stickerData, ServerDataConnect.DataType.StickerData);
        }
    }
}