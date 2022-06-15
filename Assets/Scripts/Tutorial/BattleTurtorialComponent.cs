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
    //������Ƽ
    public GameObject TutorialCanvas
    {
        get
        {
            tutorialCanvas ??= GameObject.Find("TutorialCanvas");

            return tutorialCanvas;
        }
    }

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
    private GameObject tutorialCanvas; // Ʃ�丮�� ĵ���� 

    [SerializeField]
    private TutorialTextSO tutorialTextSO; // ���� �ؽ�Ʈ����
    [SerializeField]
    private LoadingBattleDataSO _loadingBattleDataSO;

    public TutorialTextSO TutorialTextSO => tutorialTextSO;
    public TextMeshProUGUI SpeechBubbleText => speechBubbleText;
    public Image BlackBackground => blackBackground;

    [SerializeField]
    private ST_MakeStageTutorial ST_MakeStageTutorial; // ����׿� �������� Ʃ�丮�� 
    [SerializeField]
    private One_ZeroStageTutorial one_ZeroStageTutorial; // 1-0�������� Ʃ�丮�� 


    private AbstractStageTutorial currentStageTutorial; // ���� Ʃ�丮�� 
    [SerializeField]
    private BattleStageType currentBattleStageType;

    private bool _isPause = false;
    public static bool _isTutorial = false; // Ʃ�丮�� �ߴ��� 
    private void Start()
    {
        checkButton.onClick.AddListener(() => NextExplain());
        SetTutorial(); 
        // EventManager.StartListening(EventsType.NextExplain, NextExplain);
    }

    /// <summary>
    /// ť�� �� �� ����ִ��� 
    /// </summary>
    public static bool CheckQueue()
    {
        if (tutorialEventQueue.Count == 0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// ���� ���������� ���� Ʃ�丮�� ���� 
    /// </summary>
    /// <param name="tutorialType"></param>
    /// SceneLoadButtonManager�� SetBattleLoadButton���� �̺�Ʈ�� �������ٰ���
    public void SetTutorial()
    {
        currentBattleStageType = _loadingBattleDataSO.CurrentStageData.battleStageType; // ���� �� ������������ �޾ƿ�
        switch (currentBattleStageType)
        {
            case BattleStageType.ST_MAKE: 
                currentStageTutorial = ST_MakeStageTutorial;
                break; 
            case BattleStageType.S1_1:
                currentStageTutorial = one_ZeroStageTutorial;
                break;
            case BattleStageType.S1_2:
                break;
            case BattleStageType.S1_3:
                break;
            case BattleStageType.S1_4:
                break;
            default: // Ʃ�丮���� �ִ� ���������� �ƴϸ� ���� 
                return; 
        }
        tutorialEventQueue.Enqueue(StartTutorial);
        currentStageTutorial.SetQueue();
    }

    /// <summary>
    /// Ʃ�丮�� ����
    /// </summary>
    [ContextMenu("Ʃ�丮�� �׽�Ʈ")]
    public void StartTutorial()
    {
        _isTutorial = true; 
        SetTimeScale();
        ActiveTutorialCanvas();
        NextExplain();
    }

    /// <summary>
    /// Ʃ�丮�� ĵ���� Ȱ��ȭ,��Ȱ��ȭ 
    /// </summary>
    public void ActiveTutorialCanvas()
    {
        TutorialCanvas.gameObject.SetActive(!TutorialCanvas.gameObject.activeSelf); 
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


    ///// <summary>
    ///// Ÿ�ӽ����� ���� ( timeScale�� 0 �̸� 1 / 1 �̸� 0 ���� ) 
    ///// </summary>
    public void SetTimeScale()
    {
        if (_isPause == true)
        {
            Time.timeScale = 1;
            _isPause = false;
            return;
        }
        Time.timeScale = 0;
        _isPause = true;
    }

}
