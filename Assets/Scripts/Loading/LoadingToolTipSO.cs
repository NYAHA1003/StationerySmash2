using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LoadingToolTipSO", menuName = "Scriptable Object/Loading/LoadingToolTipSO")]
public class LoadingToolTipSO : ScriptableObject
{
    public List<string> toolTips = new List<string>();
}
