using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;

namespace Utill.Tool
{
	public class PencilCaseDataPoster : MonoBehaviour
    {
        public PencilCaseData pencilCaseData;

        [ContextMenu("���� ������ ������Ʈ")]
        /// <summary>
        /// ����Ÿ�Կ� ���� ���� ���뵥���͸� ������Ʈ�Ѵ�
        /// </summary>
        public void UpdateData()
        {
            ServerDataConnect.Instance.PostData(pencilCaseData, ServerDataConnect.DataType.PencilCaseData);
        }
    }
}
