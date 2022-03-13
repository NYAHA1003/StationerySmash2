using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DropdownSelection : MonoBehaviour
{
    private string selection;
    void Start()
    {
        Dropdown.OptionData option1 = new Dropdown.OptionData() { text = "Option 1" };
        Dropdown.OptionData option2 = new Dropdown.OptionData() { text = "Option 2" };
        Dropdown.OptionData option3 = new Dropdown.OptionData() { text = "Option 3" };
        GetComponent<Dropdown>().options.Add(option1);
        GetComponent<Dropdown>().options.Add(option2);
        GetComponent<Dropdown>().options.Add(option3);
    }

    public void onChange()
    {
        int index = GetComponent<Dropdown>().value;
        selection = GetComponent<Dropdown>().captionText.text;
    }
}
