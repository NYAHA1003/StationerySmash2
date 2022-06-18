using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Setting;
using UnityEngine.UI;

public class ButtonFx : MonoBehaviour
{
    private Button button;
    [SerializeField]
    private int idx;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(()=>PlaySound(idx));
    }
    private void PlaySound(int x)
    {
        Sound.PlayEff(x);
    }
}
