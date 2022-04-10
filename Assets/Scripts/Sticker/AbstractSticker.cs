using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

[System.Serializable]
public abstract class AbstractSticker
{
    protected eState _matchState = eState.IDLE;
    protected Unit _myUnit = null;

    public virtual void SetSticker(Unit unit)
    {
        _myUnit = unit;
    }

    public abstract void RunStickerAblity(eState eState);

}
