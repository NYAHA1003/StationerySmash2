using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

[System.Serializable]
public class UserSaveData //인벤토리, 돈, 캐시 이런걸 저장할 수도 있다
{
    //카드 데이터 저장
    public List<SaveData> _unitSaveDatas = new List<SaveData>();
    //가지고 있는 스킨
    public List<SkinData> _haveSkinList = new List<SkinData>();
    //현재 프로필
    public ProfileType _currentProfileType = ProfileType.None;
    //가지고 있는 프로필
    public List<ProfileType> _haveProfileList = new List<ProfileType>();
    //가지고 있는 재료
    public List<MaterialData> _materialDatas = new List<MaterialData>();
    //가지고 있는 돈
    public int _money = 0;
    //가지고 있는 달고나
    public int _dalgona = 0;
    //이름
    public string _name = "";
}

[System.Serializable]
public class SaveData //한 카드의 저장 데이터
{
    public int _level;
    public CardType _cardType;
    public StarategyType _strategicType;
    public UnitType _unitType;
}
