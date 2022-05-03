using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Load
{

    [System.Serializable]
    public class LoadData
    {
        public BattleStageType battleStageType;
        public PencilCaseData PencilCasedataBase;
        public int summonGrade;
        public float throwSpeed;
        public bool isAIOn;
        public List<CardData> cardDataList;
        public List<Vector2> pos;
        public List<float> max_Delay;
    }
}
