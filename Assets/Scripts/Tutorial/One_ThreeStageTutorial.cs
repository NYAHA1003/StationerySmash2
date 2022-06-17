using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class One_ThreeStageTutorial : AbstractStageTutorial
{
    [SerializeField]
    private GameObject _eraser; // 지우개 
    [SerializeField]
    private GameObject _eraserPiece; // 지우개 조각
    
    public override void SetQueue()
    {
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainEraser);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainEraserPiece);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainSpecialAbility);
        BattleTurtorialComponent.tutorialEventQueue.Enqueue(ExplainAdvice);

    }
    public override void EndTutorial()
    {
    }

    /// <summary>
    /// 지우개 설명  
    /// </summary>
    private void ExplainEraser()
    {
        SetSpeechText();
        _eraser.SetActive(true); 
    }
    /// <summary>
    /// 지우개 조각 설명 
    /// </summary>
    private void ExplainEraserPiece()
    {
        SetSpeechText();
        _eraserPiece.SetActive(true); 
    }
    /// <summary>
    /// 카드에는 특별한 능력이 있다 설명
    /// </summary>
    private void ExplainSpecialAbility()
    {
        SetSpeechText();
    }
    /// <summary>
    ///  조언 
    /// </summary>
    private void ExplainAdvice()
    {
        SetSpeechText();
    }
}

