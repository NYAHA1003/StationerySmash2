using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class BattleUnit : BattleCommand
{
    private GameObject unit_Prefeb;
    private Transform unit_PoolManager;
    private Transform unit_Parent;

    //테스트용 팀 설정
    public TeamType eTeam = TeamType.MyTeam;
    private int unitidCount = 0;

    //상태 풀링
    public static Dictionary<string, object> stateDic = new Dictionary<string, object>();


    public BattleUnit(BattleManager battleManager, GameObject unit_Prefeb, Transform unit_PoolManager, Transform unit_Parent) : base(battleManager)
    {
        this.unit_Prefeb = unit_Prefeb;
        this.unit_PoolManager = unit_PoolManager;
        this.unit_Parent = unit_Parent;
    }

    /// <summary>
    /// 유닛 소환
    /// </summary>
    /// <param name="dataBase"></param>
    /// <param name="Pos"></param>
    /// <param name="count"></param>
    public void Summon_Unit(DataBase dataBase, Vector3 Pos, int grade, TeamType eTeam = TeamType.Null)
    {
        Unit unit = null;

        unit = Pool_Unit(Pos);
        if (eTeam == TeamType.Null)
        {
            eTeam = this.eTeam;
        }
        unit.Set_UnitData(dataBase, eTeam, battleManager, unitidCount++, grade);
        

        //유닛 리스트에 추가
        switch (eTeam)
        {
            case TeamType.Null:
                break;
            case TeamType.MyTeam:
                battleManager._myUnitDatasTemp.Add(unit);
                break;
            case TeamType.EnemyTeam:
                battleManager._enemyUnitDatasTemp.Add(unit);
                break;
        }
    }

    /// <summary>
    /// 유닛 풀링
    /// </summary>
    /// <param name="Pos"></param>
    /// <returns></returns>
    private Unit Pool_Unit(Vector3 Pos)
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
        return unit_obj.GetComponent<Unit>();
    }

    /// <summary>
    /// 내 유닛 리스트 추가
    /// </summary>
    /// <param name="unit"></param>
    public void Add_UnitListMy(Unit unit)
    {
        battleManager._myUnitDatasTemp.Add(unit);
    }

    /// <summary>
    /// 적 유닛 리스트 추가
    /// </summary>
    /// <param name="unit"></param>
    public void Add_UnitListEnemy(Unit unit)
    {
        battleManager._enemyUnitDatasTemp.Add(unit);
    }

    /// <summary>
    /// 모든 유닛 삭제
    /// </summary>
    public void Clear_Unit()
    {
        for (int i = 0; battleManager._myUnitDatasTemp.Count > 0;)
        {
            battleManager._myUnitDatasTemp[i].Delete_Unit();
        }
        for (int i = 0; battleManager._enemyUnitDatasTemp.Count > 0;)
        {
            battleManager._enemyUnitDatasTemp[i].Delete_Unit();
        }
    }
    
    /// <summary>
    /// 풀링매니저 생성
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="count"></param>
    public static void CreatePool<T>(Transform myTrm, Transform mySprTrm, Unit myUnit) where T : IStateManager, new()
    {
        Queue<T> q = new Queue<T>();

        T g = new T();
        g.Set_State(myTrm, mySprTrm, myUnit);

        q.Enqueue(g);

        try
        {
            stateDic.Add(typeof(T).Name, q);
        }
        catch (System.ArgumentException e)
        {
            stateDic.Clear();
            stateDic.Add(typeof(T).Name, q);
        }
    }
    /// <summary>
    /// 상태 풀링 매니저 생성
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="myTrm"></param>
    /// <param name="mySprTrm"></param>
    /// <param name="myUnit"></param>
    /// <param name="statusEffect"></param>
    /// <param name="valueList"></param>
    public static void CreatePoolEff<T>(Transform myTrm, Transform mySprTrm, Unit myUnit, AtkType statusEffect, params float[] valueList) where T : Eff_State, new()
    {
        Queue<T> q = new Queue<T>();

        T g = new T();
        g.Set_StateEff(myTrm, mySprTrm, myUnit, statusEffect, valueList);

        q.Enqueue(g);

        try
        {
            stateDic.Add(typeof(T).Name, q);
        }
        catch (System.ArgumentException e)
        {
            stateDic.Clear();
            stateDic.Add(typeof(T).Name, q);
        }
    }

    /// <summary>
    /// 안 쓰는 상태 가져오기
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetItem<T>(Transform myTrm, Transform mySprTrm, Unit myUnit) where T : IStateManager, new()
    {
        T item = default(T);


        if (stateDic.ContainsKey(typeof(T).Name))
        {
            Queue<T> q = (Queue<T>)stateDic[typeof(T).Name];

            if (q.Count == 0)
            {  //안 사용하는 상태가 없으면 새로운 상태를 만든다
                item = new T();
                item.Set_State(myTrm, mySprTrm, myUnit);
            }
            else
            {
                item = q.Dequeue();
                item.Reset_State(myTrm, mySprTrm, myUnit);
            }
        }
        else
        {
            CreatePool<T>(myTrm, mySprTrm, myUnit);
            Queue<T> q = (Queue<T>)stateDic[typeof(T).Name];
            item = q.Dequeue();
            item.Reset_State(myTrm, mySprTrm, myUnit);
        }


        //할당
        return item;
    }

    /// <summary>
    /// 안 쓰는 상태이상 가져오기
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="myTrm"></param>
    /// <param name="mySprTrm"></param>
    /// <param name="myUnit"></param>
    /// <returns></returns>
    public static T GetEff<T>(Transform myTrm, Transform mySprTrm, Unit myUnit, AtkType statusEffect, params float[] valueList) where T : Eff_State, new()
    {
        T item = default(T);


        if (stateDic.ContainsKey(typeof(T).Name))
        {
            Queue<T> q = (Queue<T>)stateDic[typeof(T).Name];

            if (q.Count == 0)
            {  //안 사용하는 상태가 없으면 새로운 상태를 만든다
                item = new T();
                item.Set_StateEff(myTrm, mySprTrm, myUnit, statusEffect, valueList);
            }
            else
            {
                item = q.Dequeue();
                item.Reset_StateEff(myTrm, mySprTrm, myUnit, statusEffect, valueList);
            }
        }
        else
        {
            CreatePoolEff<T>(myTrm, mySprTrm, myUnit, statusEffect, valueList);
            Queue<T> q = (Queue<T>)stateDic[typeof(T).Name];
            item = q.Dequeue();
            item.Reset_StateEff(myTrm, mySprTrm, myUnit, statusEffect, valueList);
        }


        //할당
        return item;
    }

    /// <summary>
    /// 다 쓴 상태 반납
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="state"></param>
    public static void AddItem<T>(T state) where T : IStateManager
    {
        Queue<T> q = (Queue<T>)stateDic[typeof(T).Name];
        q.Enqueue(state);
    }

    /// <summary>
    /// 다 쓴 상태이상 반납
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="state"></param>
    public static void AddEff<T>(T state) where T : Eff_State
    {
        Queue<T> q = (Queue<T>)stateDic[typeof(T).Name];
        q.Enqueue(state);
    }
}
