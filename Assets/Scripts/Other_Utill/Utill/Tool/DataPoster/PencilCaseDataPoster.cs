using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;

namespace Utill.Tool
{
	public class PencilCaseDataPoster : MonoBehaviour
    {
        public PencilCaseData pencilCaseData;

        [ContextMenu("필통 데이터 업데이트")]
        /// <summary>
        /// 필통타입에 따라 현재 필통데이터를 업데이트한다
        /// </summary>
        public void UpdateData()
        {
            ServerDataConnect.Instance.PostData(pencilCaseData, ServerDataConnect.DataType.PencilCaseData);
        }
    }
}
