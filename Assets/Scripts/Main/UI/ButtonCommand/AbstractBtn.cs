using UnityEngine;
public abstract class AbstractBtn
{
    protected GameObject panel;
    public abstract void Execute(GameObject panel);
    public abstract void Undo();
}

