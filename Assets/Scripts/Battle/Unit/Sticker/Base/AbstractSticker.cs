using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

[System.Serializable]
public abstract class AbstractSticker
{
    protected Unit _myUnit = null;

    public virtual void SetSticker(Unit unit)
    {
        _myUnit = unit;
    }
}
