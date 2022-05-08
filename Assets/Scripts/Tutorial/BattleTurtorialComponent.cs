using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System;
using Main.Event;
using Utill.Data; 

/// <summary>
/// �������� ���� �ؽ�ƮŸ��
/// </summary>
public enum TutorialType
{
    One_Zero, // 1-0
    One_One, // 1-1
    One_Two, // 1-2
    One_Three, // 1-3
    One_Four, // 1-4
    One_Boss // 1-Boss
}

public class BattleTurtorialComponent : MonoBehaviour
{
    public static Queue<Action> tutorialEventQueue = new Queue<Action>();

    [SerializeField]  
    private Image blackBackground; // ���� ����̹��� 
    [SerializeField]
    private Image explainer; // �����ϴ� ���� 

    [SerializeField]
    private Button checkButton; // OK ��ư, ���� �������� �Ѿ
    [SerializeField]

    private TextMeshProUGUI expaineText; // ��ܿ� �� ���� �ؽ�Ʈ    
    [SerializeField]
    private TextMeshProUGUI speechBubbleText; // ��ǳ���� �� ���� �ؽ�Ʈ  
    [SerializeField]
    private Image speechBubble; // ��ǳ�� 
    [SerializeField]
    private Canvas tutorialCanvas; // Ʃ�丮�� ĵ���� 

    [SerializeField]
    private One_ZeroStageTutorial one_ZeroStageTutorial;



    private AbstractStageTutorial currentStageTutorial; // ���� Ʃ�丮�� 
    //private bool isPause = false;
    private bool isCheckInput = false;
    private void Start()
    {
        EventManager.StartListening(EventsType.NextExplain, NextExplain);

    }

    /// <summary>
    /// ���� ���������� ���� Ʃ�丮�� ���� 
    /// </summary>
    /// <param name="tutorialType"></param>
    public void SetTutorial(TutorialType tutorialType)
    {
        switch (tutorialType)
        {
            case TutorialType.One_Zero:
                currentStageTutorial = one_ZeroStageTutorial;
                break;
            case TutorialType.One_One:
                break;
            case TutorialType.One_Two:
                break;
            case TutorialType.One_Three:
                break;
            case TutorialType.One_Four:
                break;
            case TutorialType.One_Boss:
                break;
        }
        ShowTutorialCanvas(); 
        currentStageTutorial.SetQueue(); 
    }

    /// <summary>
    /// Ʃ�丮�� ĵ���� Ȱ��ȭ,��Ȱ��ȭ 
    /// </summary>
    public void ShowTutorialCanvas()
    {
        tutorialCanvas.gameObject.SetActive(!tutorialCanvas.gameObject.activeSelf); 
    }
    /// <summary>
    /// ���� ���� 
    /// </summary>
    public void NextExplain()
    {
        Debug.Log("���� ����");
        currentStageTutorial.NextExplain(); 
        // ���� ���������� �ؽ�Ʈ ������Ʈ���ִ� �Լ� 
        // ��ǳ�� �ؽ�Ʈ �ٸ� �ɷ� �ٲ� 
    }
    /// <summary>
    /// ���� ��ȯ�� ���� ���� 
    /// </summary>
    public void ExplainSummon()
    {
        
        Debug.Log("���� ���� ����");
    }

    ///// <summary>
    ///// Ÿ�ӽ����� ���� ( timeScale�� 0 �̸� 1 / 1 �̸� 0 ���� ) 
    ///// </summary>
    //public void SetTimeScale()
    //{
    //    if(isPause == true)
    //    {
    //        Time.timeScale = 1;
    //        isPause = false; 
    //        return; 
    //    }
    //    Time.timeScale = 0;
    //    isPause = true; 
    //}
}
