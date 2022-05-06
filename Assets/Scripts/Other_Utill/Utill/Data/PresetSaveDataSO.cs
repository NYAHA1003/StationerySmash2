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
        public List<CardSaveData> _ingameSaveDatas = new List<CardSaveData>();    //�ΰ��ӵ� ī�� ������ ����;
        public PencilCaseData _pencilCaseData = null; //�ΰ��� ���� ������ ����
    }
}

