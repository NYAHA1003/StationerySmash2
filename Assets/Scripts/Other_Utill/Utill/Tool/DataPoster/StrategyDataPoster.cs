using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;


namespace Utill.Tool
{
    public class StrategyDataPoster : MonoBehaviour
    {
        public StrategyData strategyData;

        [ContextMenu("전략 데이터 업데이트")]
        /// <summary>
        /// 전략타입에 따라 현재 전략데이터를 업데이트한다
        /// </summary>
        public void UpdateData()
        {
            ServerDataConnect.Instance.PostData(strategyData, ServerDataConnect.DataType.StrategyData);
        }
    }
}