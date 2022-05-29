using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;

namespace Utill.Tool
{
    public class StickerDataPoster : MonoBehaviour
    {
        public StickerData stickerData;

        [ContextMenu("스티커 데이터 업데이트")]
        /// <summary>
        /// 스티커타입에 따라 현재 스티커데이터를 업데이트한다
        /// </summary>
        public void UpdateData()
        {
            ServerDataConnect.Instance.PostData(stickerData, ServerDataConnect.DataType.StickerData);
        }
    }
}