using UnityEngine; 
class ActiveCommand : AbstractBtn
{
    public override void Execute(GameObject panel)
    {
        this.panel = panel;
        panel.SetActive(true);
    }

    public override void Undo()
    {
        panel.SetActive(false);
    }
}
