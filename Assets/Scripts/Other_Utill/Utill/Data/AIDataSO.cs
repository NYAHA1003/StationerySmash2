using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Load;
using Utill.Tool;
using Main.Deck;

[CreateAssetMenu(fileName = "AIDataSO", menuName = "Scriptable Object/AIDataSO")]
public class AIDataSO : ScriptableObject
{
    //
    public int summonGrade;
    public float throwSpeed;
    public bool isAIOn;
    public List<CardData> cardDataList;
    public List<Vector2> pos;
    public List<float> max_Delay;

    /// <summary>
    /// AIData ¼³Á¤
    /// </summary>
    public void SetAIData(LoadData loadData)
    {
        summonGrade = loadData.summonGrade;
        throwSpeed = loadData.throwSpeed;
        isAIOn = loadData.isAIOn;
        cardDataList.Clear();
        for (int i = 0; i < loadData.cardStageData.Count; i++)
        {
            CardData cardData = DeckDataManagerSO.FindStdCardData(loadData.cardStageData[i]._cardNamingType).DeepCopy();
            cardData._level = loadData.cardStageData[i]._level;
            cardDataList.Add(cardData);
        }
        pos = loadData.pos;
        max_Delay = loadData.max_Delay;
    }

}
