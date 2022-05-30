using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
namespace Utill.Load
{

    [System.Serializable]
    public class LoadData
    {
        public BattleStageType battleStageType;
        public PencilCaseData _pencilCaseData;
        public int summonGrade;
        public float throwSpeed;
        public bool isAIOn;
        public List<CardStageData> cardStageData;
        public List<Vector2> pos;
        public List<float> max_Delay;
        public int _awardMoney;
        public int _awardExp;
    }


    [System.Serializable]
    public class CardStageData
	{
        public int _level;
        public CardNamingType _cardNamingType;
	}
}
