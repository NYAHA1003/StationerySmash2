using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TempBtnManager : MonoBehaviour
{
    private AbstractButton currentButton;
    private Stack<AbstractButton> abstractButtons;

    public DeckActive deckActive;

    public void OnActiveBtn()
    {
        if (abstractButtons == null)
            return;
        currentButton.PerformActive(currentButton);
    }
    public void OnUndoBtn()
    {
        if (abstractButtons == null)
            return;
        currentButton.PerformUndo(currentButton);
        abstractButtons.Pop();
    }

    public void SetCurrentButton()
    {
        currentButton = EventSystem.current.currentSelectedGameObject.GetComponent<AbstractButton>().SetCurrentBtn();
        abstractButtons.Push(currentButton);
    }

    public void Reset()
    {
        int count = abstractButtons.Count;
        for (int i = 0; i < count; i++)
        {
            abstractButtons.Pop().gameObject.SetActive(false);
        }
        abstractButtons.Clear();

    }
    // 스택 초기화, 활성화된 모든 패널 클리어 
}
