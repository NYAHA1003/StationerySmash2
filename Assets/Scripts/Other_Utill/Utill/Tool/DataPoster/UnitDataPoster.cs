using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Tool
{
    public class UnitDataPoster : MonoBehaviour
    {
        public UnitData unitData;

        [ContextMenu("유닛 데이터 업데이트")]
        /// <summary>
        /// 유닛타입에 따라 현재 유닛데이터를 업데이트한다
        /// </summary>
        public void UpdateUnitData()
		{
            ServerDataConnect.Instance.PostData(unitData, ServerDataConnect.DataType.UnitData);
		}
    }

}