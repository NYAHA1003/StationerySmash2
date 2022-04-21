using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

[System.Serializable]
public class UserSaveData //인벤토리, 돈, 캐시 이런걸 저장할 수도 있다
{
    //유닛의 데이터들을 저장
    public List<SaveData> unitSaveDatas;
    public List<SkinData> _haveSkinList = new List<SkinData>();
    public List<MaterialData> _materialDatas = new List<MaterialData>();
    public int _money;
    public int _dalgona;
}

[System.Serializable]
public class SaveData //한 카드의 저장 데이터
{
    public int _level;
    public CardType _cardType;
    public StarategyType _strategicType;
    public UnitType _unitType;
}
