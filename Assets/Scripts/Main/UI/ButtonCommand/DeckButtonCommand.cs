using UnityEngine;  
public class DeckButtonCommand : AbstractBtn
{
    private GameObject deckPanel; 
    protected override void Execute()
    {
        deckPanel.SetActive(true); 
    }
    protected override void Undo()
    {
        deckPanel.SetActive(false);
    }
}
