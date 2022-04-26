using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ư Ŀ�ǵ忡 ���Ǵ� ��ư Enum 
/// </summary>
[System.Serializable]
enum ButtonType
{
    deck,
    cardDescription,
    setting
}

// ButtonType ������� allBtns�� Add���־�� �� 
class ButtonComponent : MonoBehaviour
{

    private Stack<AbstractBtn> activeButtons = new Stack<AbstractBtn>(); // Ȱ��ȭ ��Ų ��ư���� �־�� ����
    private List<AbstractBtn> allBtns = new List<AbstractBtn>(); // �⺻������ �����صδ� ��� ��ưĿ�ǵ�� 

    [Header("Ŭ���� ��ư��")]
    [SerializeField]
    private Button[] clickBtns;
    [Header("ĵ�� ��ư��")]
    [SerializeField]
    private Button[] cancelBtns;
    [Header("Ȱ��ȭ�� �гε�")]
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
    ///  ����� ��ư Ŀ�ǵ�� �ʱ�ȭ 
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
    /// �г� Ȱ��ȭ��ų �� �ߵ��Ǵ� ��ư�Լ� 
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
    /// �г� ��Ȱ��ȭ��ų �� �ߵ��Ǵ� ��ư�Լ� 
    /// </summary>
    public void OnUndoBtn()
    {
        activeButtons.Pop().Undo();
    }

    /// <summary>
    /// Ȱ��ȭ�Ǿ� �ִ� ��� �г� ��Ȱ��ȭ
    /// </summary>
    public void CloseAllPanels()
    {
        for (int i = 0; i < activeButtons.Count; i++)
        {
            activeButtons.Pop().Undo();
        }
    }
}