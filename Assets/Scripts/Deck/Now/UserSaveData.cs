using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

[System.Serializable]
public class UserSaveData 
{
    public List<int> unitLevels = new List<int>();
    public Dictionary<UnitType, bool> haveUnits = new Dictionary<UnitType, bool>();
}