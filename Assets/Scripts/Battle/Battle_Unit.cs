using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class Battle_Unit : BattleCommand
{
    private GameObject unit_Prefeb;
    private Transform unit_PoolManager;
    private Transform unit_Parent;
    public TeamType eTeam = TeamType.MyTeam;

    public Battle_Unit(BattleManager battleManager, GameObject unit_Prefeb, Transform unit_PoolManager, Transform unit_Parent) : base(battleManager)
    {
        this.unit_Prefeb = unit_Prefeb;
        this.unit_PoolManager = unit_PoolManager;
        this.unit_Parent = unit_Parent;
    }

    public void Summon_Unit(DataBase dataBase, Vector3 Pos, int count)
    {
        Unit unit = null;

        switch (dataBase.cardType)
        {
            case CardType.Execute:
            case CardType.SummonTrap:
            case CardType.Installation:
                unit = Pool_Strategy_Unit(Pos);
                break;
            case CardType.SummonUnit:
                unit = Pool_Stationary_Unit(Pos);
                break;
        }
        unit.Set_UnitData(dataBase, eTeam, battleManager, count);


        switch (eTeam)
        {
            case TeamType.Null:
                break;
            case TeamType.MyTeam:
                battleManager.unit_MyDatasTemp.Add(unit);
                break;
            case TeamType.EnemyTeam:
                battleManager.unit_EnemyDatasTemp.Add(unit);
                break;
        }
    }

    private Stationary_Unit Pool_Stationary_Unit(Vector3 Pos)
    {
        GameObject unit_obj = null;
        if (unit_PoolManager.childCount > 0)
        {
            unit_obj = unit_PoolManager.GetChild(0).gameObject;
            unit_obj.transform.position = Pos;
            unit_obj.SetActive(true);
        }
        unit_obj ??= battleManager.Create_Object(unit_Prefeb, Pos, Quaternion.identity);
        unit_obj.transform.SetParent(unit_Parent);
        return unit_obj.GetComponent<Stationary_Unit>();
    }
    private Strategy_Unit Pool_Strategy_Unit(Vector3 Pos)
    {
        GameObject unit_obj = null;
        if (unit_PoolManager.childCount > 0)
        {
            unit_obj = unit_PoolManager.GetChild(0).gameObject;
            unit_obj.transform.position = Pos;
            unit_obj.SetActive(true);
        }
        unit_obj ??= battleManager.Create_Object(unit_Prefeb, Pos, Quaternion.identity);
        unit_obj.transform.SetParent(unit_Parent);
        return unit_obj.GetComponent<Strategy_Unit>();
    }

    public void Add_UnitListMy(Unit unit)
    {
        battleManager.unit_MyDatasTemp.Add(unit);
    }

    public void Add_UnitListEnemy(Unit unit)
    {
        battleManager.unit_EnemyDatasTemp.Add(unit);
    }

    public void Clear_Unit()
    {
        for (int i = 1; battleManager.unit_MyDatasTemp.Count > 1;)
        {
            battleManager.unit_MyDatasTemp[i].Delete_Unit();
        }
        for (int i = 1; battleManager.unit_EnemyDatasTemp.Count > 1;)
        {
            battleManager.unit_EnemyDatasTemp[i].Delete_Unit();
        }
    }
}
