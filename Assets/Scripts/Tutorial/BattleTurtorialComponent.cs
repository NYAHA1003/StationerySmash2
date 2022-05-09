using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System;
using Main.Event;
using Utill.Data;
using Battle.Tutorial;
using Utill.Load;
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
    private Image blackBackground; // ���� ����̹���,���� 
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
    private TutorialTextSO tutorialTextSO; // ���� �ؽ�Ʈ����

    public TutorialTextSO TutorialTextSO => tutorialTextSO;
    public TextMeshProUGUI SpeechBubbleText => speechBubbleText;
    public Image BlackBackground => blackBackground; 
    [SerializeField]
    private One_ZeroStageTutorial one_ZeroStageTutorial; // 1-0�������� Ʃ�丮�� 

    private AbstractStageTutorial currentStageTutorial; // ���� Ʃ�丮�� 
    private BattleStageType currentBattleStageType; 
    //private bool isPause = false;
    private void Start()
    {
        speechBubbleText.text = tutorialTextSO._textDatas[(int)currentBattleStageType]._tutorialText[0];
        checkButton.onClick.AddListener(() => NextExplain()); 
        // EventManager.StartListening(EventsType.NextExplain, NextExplain);
        // EventManager.StartListening(EventsType.SetTutorial, (x) => SetTutorial((BattleStageType)x));    
    }

    /// <summary>
    /// ���� ���������� ���� Ʃ�丮�� ���� 
    /// </summary>
    /// <param name="tutorialType"></param>
    /// SceneLoadButtonManager�� SetBattleLoadButton���� �̺�Ʈ�� �������ٰ���
     [ContextMenu("�̸�")]
    public void SetTutorial(BattleStageType battleStageType)
    {
        currentBattleStageType = battleStageType;
        switch (battleStageType)
        {
            case BattleStageType.S1_1:
                currentStageTutorial = one_ZeroStageTutorial;
                break;
            case BattleStageType.S1_2:
                break;
            case BattleStageType.S1_3:
                break;
            case BattleStageType.S1_4:
                break;
        }
        SetTimeScale(); 
        ActiveTutorialCanvas(); 
        currentStageTutorial.SetQueue();
    }

    /// <summary>
    /// Ʃ�丮�� ĵ���� Ȱ��ȭ,��Ȱ��ȭ 
    /// </summary>
    public void ActiveTutorialCanvas()
    {
        tutorialCanvas.gameObject.SetActive(!tutorialCanvas.gameObject.activeSelf); 
    }
    /// <summary>
    /// ���� ���� 
    /// </summary>
    public void NextExplain()
    {
        Debug.Log("���� ����");
        if (tutorialEventQueue.Count == 0)
        {
            ActiveTutorialCanvas();
            SetTimeScale(); 
            return; 
        }
        tutorialEventQueue.Dequeue().Invoke();

        // ���� ���������� �ؽ�Ʈ ������Ʈ���ִ� �Լ� 
        // ��ǳ�� �ؽ�Ʈ �ٸ� �ɷ� �ٲ� 
    }


    private bool isPause = false; 
    ///// <summary>
    ///// Ÿ�ӽ����� ���� ( timeScale�� 0 �̸� 1 / 1 �̸� 0 ���� ) 
    ///// </summary>
    public void SetTimeScale()
    {
        if (isPause == true)
        {
            Time.timeScale = 1;
            isPause = false;
            return;
        }
        Time.timeScale = 0;
        isPause = true;
    }
}
