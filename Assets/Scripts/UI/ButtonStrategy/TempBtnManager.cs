using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBtnManager : MonoBehaviour
{
    private AbstractButton currentButton;
    private Stack<AbstractButton> abstractButtons;

    public DeckActive deckActive;
    
    public void OnActiveBtn()
    {
        currentButton.PerformActive(currentButton); 
    }
    public void OnUndoBtn()
    {
        currentButton.PerformUndo(currentButton);
        abstractButtons.Pop(); 
    }
    public void SetCurrentButton()
    {
        currentButton = currentButton.SetCurrentBtn();
        abstractButtons.Push(currentButton); 
    }

    // 스택 초기화, 활성화된 모든 패널 클리어 
}
