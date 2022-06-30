using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using Utill.Data;
using Utill.Load;
using Utill.Tool;

public class AIAndStageData : MonoSingleton<AIAndStageData>
{
    public StageData _currentStageDatas;

    public int summonGrade;
    public float throwSpeed;
    public bool isAIOn;
    public List<CardData> cardDataList;
    public List<Vector2> pos;
    public List<float> max_Delay;
    public bool _isEventMode = false;

    /// <summary>
    /// AIData ¼³Á¤
    /// </summary>
    public void SetAIData(LoadData loadData)
    {
        summonGrade = loadData.summonGrade;
        throwSpeed = loadData.throwSpeed;
        isAIOn = loadData.isAIOn;
        cardDataList = new List<CardData>();
        for (int i = 0; i < loadData.cardStageData.Count; i++)
        {
            CardData cardData = DeckDataManagerSO.FindStdCardData(loadData.cardStageData[i]._cardNamingType).DeepCopy();
            cardData._level = loadData.cardStageData[i]._level;
            cardDataList.Add(cardData);
        }
        pos = loadData.pos;
        max_Delay = loadData.max_Delay;
        _isEventMode = loadData._isEventMode;
    }

}
