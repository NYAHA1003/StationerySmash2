using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Tool
{
    public class UnitDataPoster : MonoBehaviour
    {
        public UnitData unitData;

        [ContextMenu("���� ������ ������Ʈ")]
        /// <summary>
        /// ����Ÿ�Կ� ���� ���� ���ֵ����͸� ������Ʈ�Ѵ�
        /// </summary>
        public void UpdateUnitData()
		{
            ServerDataConnect.Instance.PostData(unitData, ServerDataConnect.DataType.UnitData);
		}
    }

}