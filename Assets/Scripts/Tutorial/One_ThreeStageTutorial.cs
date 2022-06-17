using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class One_ThreeStageTutorial : AbstractStageTutorial
{
    [SerializeField]
    private GameObject _eraser; // ���찳 
    [SerializeField]
    private GameObject _eraserPiece; // ���찳 ����
    
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
    /// ���찳 ����  
    /// </summary>
    private void ExplainEraser()
    {
        SetSpeechText();
        _eraser.SetActive(true); 
    }
    /// <summary>
    /// ���찳 ���� ���� 
    /// </summary>
    private void ExplainEraserPiece()
    {
        SetSpeechText();
        _eraserPiece.SetActive(true); 
    }
    /// <summary>
    /// ī�忡�� Ư���� �ɷ��� �ִ� ����
    /// </summary>
    private void ExplainSpecialAbility()
    {
        SetSpeechText();
    }
    /// <summary>
    ///  ���� 
    /// </summary>
    private void ExplainAdvice()
    {
        SetSpeechText();
    }
}

