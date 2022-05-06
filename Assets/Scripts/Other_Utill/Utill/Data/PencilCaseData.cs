using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using Battle.PCAbility;

namespace Utill.Data
{
    [System.Serializable]
    public class PencilCaseData
    {
        public int _maxCard;
        public float _costSpeed;
        public float _throwGaugeSpeed;
        public PencilCaseType _pencilCaseType;
        public AbstractPencilCaseAbility _pencilState;
        public CardData _pencilCaseData;
        public List<BadgeData> _badgeDatas;
    }

}
