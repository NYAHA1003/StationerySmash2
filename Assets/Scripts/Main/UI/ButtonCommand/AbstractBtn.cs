using UnityEngine;

namespace Main.Buttons
{
    public abstract class AbstractBtn
    {
        public abstract void Execute(GameObject panel);
        public abstract void Undo();
    }
}