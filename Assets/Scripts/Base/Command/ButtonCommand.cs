using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ButtonCommand
{
    protected ButtonManager buttonManager;
    public ButtonCommand(ButtonManager buttonManager)
    {
        this.buttonManager = buttonManager; 
    }
}
