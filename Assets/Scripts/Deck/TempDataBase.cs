using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill; 
public class TempDataBase : MonoBehaviour
{
    public static TempDataBase Instance;


    [SerializeField]
    private CardData pecilCard;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public CardData ReturnData()
    {
        return pecilCard; 
    }
    public UnitType ReturnDataType()
    {
        return UnitType.Pencil;
    }
}