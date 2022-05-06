using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Data
{
    [CreateAssetMenu(fileName = "PencilCaseDataListSO", menuName = "Scriptable Object/PencilCaseDataListSO")]
    public class PencilCaseDataListSO : ScriptableObject
    {
        public List<PencilCaseData> _pencilCaseDataList;
    }

}