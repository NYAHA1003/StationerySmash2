using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;


namespace Utill.Tool
{
    public class StrategyDataPoster : MonoBehaviour
    {
        public StrategyData strategyData;

        [ContextMenu("���� ������ ������Ʈ")]
        /// <summary>
        /// ����Ÿ�Կ� ���� ���� ���������͸� ������Ʈ�Ѵ�
        /// </summary>
        public void UpdateData()
        {
            ServerDataConnect.Instance.PostData(strategyData, ServerDataConnect.DataType.StrategyData);
        }
    }
}