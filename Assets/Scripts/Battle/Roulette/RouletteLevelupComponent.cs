using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Tool;

public class RouletteLevelupComponent : MonoBehaviour, IUserData
{
    private static int _previousLevel;
    private static int _previousExp;

    private static int _level;
    private static int _exp;

    [SerializeField]
    private GameObject roulettetObj;
    
    private void Start()
    {
        Debug.Log("·ê·¿ÄÄÆ÷");
        _previousExp = UserSaveManagerSO.UserSaveData._nowExp;
        _previousLevel = UserSaveManagerSO.UserSaveData._level;
        UserSaveManagerSO.AddObserver(this);

    }
    public void Notify()
    {
        CheckExp(); 
    }
    public void CheckExp()
    {
        _level = UserSaveManagerSO.UserSaveData._level;
        _exp = UserSaveManagerSO.UserSaveData._nowExp;
        if (_level > _previousLevel)
        {
            _previousExp = _exp;
            _previousLevel = _level;

            roulettetObj.gameObject.SetActive(true);
            roulettetObj.transform.Find("Roulette").GetComponent<RouletteComponent>().SetRouletteTypeStage(0); 
        }
    }


}
