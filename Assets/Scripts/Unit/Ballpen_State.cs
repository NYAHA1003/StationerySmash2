using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;

public class BallpenStateManager : IStateManager
{
    private Ballpen_Idle_State IdleState;
    private Ballpen_Attack_State AttackState;
    private Ballpen_Damaged_State DamagedState;
    private Ballpen_Throw_State ThrowState;
    private Ballpen_Die_State DieState;
    private Ballpen_Move_State MoveState;
    private Ballpen_Wait_State WaitState;
    private UnitState cur_unitState;
    public StageData _stageData { get; private set; }

    private float Wait_extraTime = 0;

    public void Reset_CurrentUnitState(UnitState unitState)
    {
        cur_unitState = unitState;
    }
    public UnitState Return_CurrentUnitState()
    {
        return cur_unitState;
    }

    public void Set_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
    {
        //스테이트들을 설정한다
        IdleState = new    Ballpen_Idle_State(myTrm, mySprTrm, myUnit);
        WaitState = new    Ballpen_Wait_State(myTrm, mySprTrm, myUnit);
        MoveState = new    Ballpen_Move_State(myTrm, mySprTrm, myUnit);
        AttackState = new  Ballpen_Attack_State(myTrm, mySprTrm, myUnit);
        DamagedState = new Ballpen_Damaged_State(myTrm, mySprTrm, myUnit);
        DieState = new     Ballpen_Die_State(myTrm, mySprTrm, myUnit);
        ThrowState = new Ballpen_Throw_State(myTrm, mySprTrm, myUnit);

        Reset_CurrentUnitState(IdleState);

        IdleState.Set_StateChange(this);
        WaitState.Set_StateChange(this);
        MoveState.Set_StateChange(this);
        AttackState.Set_StateChange(this);
        DamagedState.Set_StateChange(this);
        DieState.Set_StateChange(this);
        ThrowState.Set_StateChange(this);
    }
    public void Reset_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
    {
        IdleState.Change_Trm(myTrm, mySprTrm, myUnit);
        WaitState.Change_Trm(myTrm, mySprTrm, myUnit);
        MoveState.Change_Trm(myTrm, mySprTrm, myUnit);
        AttackState.Change_Trm(myTrm, mySprTrm, myUnit);
        DamagedState.Change_Trm(myTrm, mySprTrm, myUnit);
        DieState.Change_Trm(myTrm, mySprTrm, myUnit);
        ThrowState.Change_Trm(myTrm, mySprTrm, myUnit);

        IdleState.Reset_State();
        WaitState.Reset_State();
        MoveState.Reset_State();
        AttackState.Reset_State();
        DamagedState.Reset_State();
        DieState.Reset_State();
        ThrowState.Reset_State();

        Reset_CurrentUnitState(IdleState);
    }

    public void Set_Attack(Unit targetUnit)
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        AttackState.Set_Target(targetUnit);
        cur_unitState.nextState = AttackState;
        AttackState.Reset_State();
        Reset_CurrentUnitState(AttackState);
    }

    public void Set_Damaged(AtkData atkData)
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        DamagedState.Set_AtkData(atkData);
        cur_unitState.nextState = DamagedState;
        DamagedState.Reset_State();
        Reset_CurrentUnitState(DamagedState);
    }

    public void Set_Die()
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        cur_unitState.nextState = DieState;
        DieState.Reset_State();
        Reset_CurrentUnitState(DieState);
    }

    public void Set_Idle()
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        cur_unitState.nextState = IdleState;
        IdleState.Reset_State();
        Reset_CurrentUnitState(IdleState);
    }

    public void Set_Move()
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        cur_unitState.nextState = MoveState;
        MoveState.Reset_State();
        Reset_CurrentUnitState(MoveState);
    }

    public void Set_Throw()
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        cur_unitState.nextState = ThrowState;
        ThrowState.Reset_State();
        Reset_CurrentUnitState(ThrowState);
    }

    public void Set_Wait(float time)
    {
        cur_unitState.Set_Event(eEvent.EXIT);
        WaitState.Set_Time(time);
        WaitState.Set_ExtraTime(Wait_extraTime);
        cur_unitState.nextState = WaitState;
        WaitState.Reset_State();
        Reset_CurrentUnitState(WaitState);
    }


    public void Set_WaitExtraTime(float extraTime)
    {
        this.Wait_extraTime = extraTime;
    }
    public void Set_ThrowPos(Vector2 pos)
    {
        this.ThrowState.Set_ThrowPos(pos);
    }

    public void SetStageData(StageData stageData)
    {
        _stageData = stageData;
    }

    public StageData GetStageData()
    {
        return _stageData;
    }
}

public class Ballpen_Idle_State : Pencil_Idle_State
{
    public Ballpen_Idle_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }
}

public class Ballpen_Wait_State : Pencil_Wait_State
{
    public Ballpen_Wait_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {

    }
}

public class Ballpen_Move_State : Pencil_Move_State
{
    public Ballpen_Move_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }
}

public class Ballpen_Attack_State : Pencil_Attack_State
{
    public Ballpen_Attack_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }
}

public class Ballpen_Damaged_State : Pencil_Damaged_State
{
    public Ballpen_Damaged_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }
}

public class Ballpen_Die_State : Pencil_Die_State
{
    public Ballpen_Die_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }

}

public class Ballpen_Throw_State : Pencil_Throw_State
{
    public Ballpen_Throw_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }
    protected override void Run_ThrowAttack(Unit targetUnit)
    {
        float dir = Vector2.Angle((Vector2)myTrm.position, (Vector2)targetUnit.transform.position);
        float extraKnockBack = (targetUnit.weight - myUnit.Return_Weight() * (float)targetUnit.hp / targetUnit.maxhp) * 0.025f;
        AtkData atkData = new AtkData(myUnit, 0, 0, 0, 0, true, 0, AtkType.Normal, originValue);

        if (myUnit.eTeam == TeamType.MyTeam)
        {
            IntAttack(myUnit._battleManager.CommandUnit._enemyUnitList);
        }
        else if (myUnit.eTeam == TeamType.EnemyTeam)
        {
            IntAttack(myUnit._battleManager.CommandUnit._playerUnitList);
        }

        atkData.Reset_Damage(100 + (myUnit.weight > targetUnit.weight ? (Mathf.RoundToInt((float)myUnit.weight - targetUnit.weight) / 2) : Mathf.RoundToInt((float)(targetUnit.weight - myUnit.weight) / 5)));


        //무게가 더 클 경우
        if (myUnit.weight > targetUnit.weight)
        {
            atkData.Reset_Kncockback(10, extraKnockBack, dir, false);
            atkData.Reset_Type(AtkType.Stun);
            atkData.Reset_Value(1);
            targetUnit.Run_Damaged(atkData);
            return;
        }

        //무게가 더 작을 경우
        if (myUnit.weight < targetUnit.weight)
        {
            atkData.Reset_Kncockback(0, 0, 0, false);
            atkData.Reset_Type(AtkType.Normal);
            atkData.Reset_Value(0);
            targetUnit.Run_Damaged(atkData);

            atkData.Reset_Kncockback(20, 0, dir, true);
            atkData.Reset_Type(AtkType.Stun);
            atkData.Reset_Value(1);
            atkData.Reset_Damage(0);
            myUnit.Run_Damaged(atkData);
            return;
        }

        //무게가 같을 경우
        if (myUnit.weight.Equals(targetUnit.weight))
        {
            atkData.Reset_Kncockback(10, extraKnockBack, dir, false);
            atkData.Reset_Type(AtkType.Stun);
            atkData.Reset_Value(1);
            targetUnit.Run_Damaged(atkData);


            atkData.Reset_Kncockback(20, 0, dir, true);
            atkData.Reset_Type(AtkType.Normal);
            atkData.Reset_Value(1);
            atkData.Reset_Damage(0);
            myUnit.Run_Damaged(atkData);

            return;
        }
    }

    private void IntAttack(List<Unit> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            //originValue[3]은 거리 값
            if (Vector2.Distance(myTrm.position, list[i].transform.position) < originValue[3])
            {
                list[i].Add_StatusEffect(AtkType.Ink, originValue);
            }
        }
    }

}

