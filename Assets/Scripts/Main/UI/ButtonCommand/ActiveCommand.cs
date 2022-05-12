﻿using UnityEngine;
using Utill.Data;

namespace Main.Buttons
{
    class ActiveCommand : AbstractBtn
    {
        private GameObject panel;
        private ButtonType buttonType;
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
}