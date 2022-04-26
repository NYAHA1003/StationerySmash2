using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 버튼 커맨드에 사용되는 버튼 Enum 
/// </summary>
[System.Serializable]
enum ButtonType
{
    deck,
    cardDescription,
    setting
}

// ButtonType 순서대로 allBtns에 Add해주어야 함 
class ButtonComponent : MonoBehaviour
{

    private Stack<AbstractBtn> activeButtons = new Stack<AbstractBtn>(); // 활성화 시킨 버튼들을 넣어둘 스택
    private List<AbstractBtn> allBtns = new List<AbstractBtn>(); // 기본적으로 세팅해두는 모든 버튼커맨드들 

    [Header("클릭할 버튼들")]
    [SerializeField]
    private Button[] clickBtns;
    [Header("캔슬 버튼들")]
    [SerializeField]
    private Button[] cancelBtns;
    [Header("활성화할 패널들")]
    [SerializeField]
    private GameObject[] activePanels;


    private ActiveCommand deckButtonCommand;
    private ActiveCommand cardDescriptionButtonCommand;
    private ActiveCommand SettingButtonCommand;
    private void Start()
    {
        Initialized();
    }

    /// <summary>
    ///  사용할 버튼 커맨드들 초기화 
    /// </summary>
    private void Initialized()
    {
        deckButtonCommand = new ActiveCommand();
        cardDescriptionButtonCommand = new ActiveCommand();
        SettingButtonCommand = new ActiveCommand();

        allBtns.Add(deckButtonCommand);
        allBtns.Add(cardDescriptionButtonCommand);
        allBtns.Add(SettingButtonCommand);
        AddListner();

    }

    private void AddListner()
    {
        for (int i = 0; i < clickBtns.Length; i++)
        {
            int index = i;
            clickBtns[i].onClick.AddListener(() => OnActiveBtn((ButtonType)index));
        }

        for (int i = 0; i < cancelBtns.Length; i++)
        {
            cancelBtns[i].onClick.AddListener(() => OnUndoBtn());
        }
    }
    /// <summary>
    /// 패널 활성화시킬 때 발동되는 버튼함수 
    /// </summary>
    /// <param name="buttonType"></param>
    public void OnActiveBtn(ButtonType buttonType)
    {
        Debug.Log(buttonType);
        activeButtons.Push(allBtns[(int)buttonType]);
        try
        {
            activeButtons.Peek().Execute(activePanels[(int)buttonType]);
        }
        catch(NullReferenceException e)
        {
            Debug.Log(activeButtons.Count); 
            for(int i= 0; i < activeButtons.Count;i++)
            {
                Debug.Log(i);
                Debug.Log(activePanels[i].name);
                Debug.Log(activeButtons.Peek().GetType().Name);
                activeButtons.Peek().Execute(activePanels[(int)buttonType]);
            }
        }
    }
    /// <summary>
    /// 패널 비활성화시킬 때 발동되는 버튼함수 
    /// </summary>
    public void OnUndoBtn()
    {
        activeButtons.Pop().Undo();
    }

    /// <summary>
    /// 활성화되어 있는 모든 패널 비활성화
    /// </summary>
    public void CloseAllPanels()
    {
        for (int i = 0; i < activeButtons.Count; i++)
        {
            activeButtons.Pop().Undo();
        }
    }
}