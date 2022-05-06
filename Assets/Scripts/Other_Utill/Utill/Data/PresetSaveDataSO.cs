using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;
using Main.Deck;

namespace Utill.Data
{
    [CreateAssetMenu(fileName = "CardSaveDataSO", menuName = "Scriptable Object/CardSaveDataSO")]
    public class PresetSaveDataSO : ScriptableObject
    {
        public List<CardSaveData> _ingameSaveDatas = new List<CardSaveData>();    //인게임덱 카드 데이터 저장;
        public PencilCaseData _pencilCaseData = null; //인게임 필통 데이터 저장
    }
}

