using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;

public class PencilCaseStateManager : IStateManager
{
    private PencilCase_Idle_State IdleState;
    private PencilCase_Damaged_State DamagedState;
    private PencilCase_Die_State DieState;
    private UnitState cur_unitState;

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
        IdleState = new PencilCase_Idle_State(myTrm, mySprTrm, myUnit);
        DamagedState = new PencilCase_Damaged_State(myTrm, mySprTrm, myUnit);
        DieState = new PencilCase_Die_State(myTrm, mySprTrm, myUnit);

        Reset_CurrentUnitState(IdleState);

        IdleState.Set_StateChange(this);
        DamagedState.Set_StateChange(this);
        DieState.Set_StateChange(this);
    }
    public void Reset_State(Transform myTrm, Transform mySprTrm, Unit myUnit)
    {
        IdleState.Change_Trm(myTrm, mySprTrm, myUnit);
        DamagedState.Change_Trm(myTrm, mySprTrm, myUnit);
        DieState.Change_Trm(myTrm, mySprTrm, myUnit);

        IdleState.Reset_State();
        DamagedState.Reset_State();
        DieState.Reset_State();

        Reset_CurrentUnitState(IdleState);
    }

    public void Set_Attack(Unit targetUnit)
    {
        throw new System.Exception("필통 에러");
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
        throw new System.Exception("필통 에러");
    }

    public void Set_Throw()
    {
        throw new System.Exception("필통 에러");
    }

    public void Set_Wait(float time)
    {
        throw new System.Exception("필통 에러");
    }


    public void Set_WaitExtraTime(float extraTime)
    {
        throw new System.Exception("필통 에러");
    }

    public void Set_ThrowPos(Vector2 pos)
    {
        throw new System.Exception("필통 에러");
    }
}
public class PencilCase_Idle_State : Stationary_UnitState
{
    public PencilCase_Idle_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }

    public override void Add_StatusEffect(AtkType atkType, params float[] value)
    {

    }
    public override Unit Pull_Unit()
    {
        return null;
    }

    public override Unit Pulling_Unit()
    {
        return null;
    }
    public override void Throw_Unit(Vector2 pos)
    {

    }
}
public class PencilCase_Damaged_State : Stationary_UnitState
{
    private AtkData atkData;

    public PencilCase_Damaged_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }

    public void Set_AtkData(AtkData atkData)
    {
        this.atkData = atkData;
    }

    public override void Enter()
    {
        curState = eState.DAMAGED;
        curEvent = eEvent.ENTER;

        myUnit.Subtract_HP(atkData.damage);
        if (myUnit.hp <= 0)
        {
            stateChange.Set_Die();
            return;
        }
        base.Enter();
    }

    public override void Update()
    {
        stateChange.Set_Idle();
    }

    public override void Animation(params float[] value)
    {
        float rotate = myUnit.eTeam.Equals(TeamType.MyTeam) ? 360 : -360;
        mySprTrm.DORotate(new Vector3(0, 0, rotate), value[0], RotateMode.FastBeyond360);
    }
    public override void Add_StatusEffect(AtkType atkType, params float[] value)
    {

    }
    public override Unit Pull_Unit()
    {
        return null;
    }

    public override Unit Pulling_Unit()
    {
        return null;
    }
    public override void Throw_Unit(Vector2 pos)
    {

    }
}
public class PencilCase_Die_State : Stationary_UnitState
{
    public PencilCase_Die_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
        curState = eState.DIE;
        curEvent = eEvent.ENTER;
    }

    public override void Enter()
    {
        battleManager.battle_Camera.Win_CamEffect(myTrm.position, myUnit.eTeam != TeamType.MyTeam);
        Debug.Log("필통 뒤짐");
        base.Enter();
    }

    private void Reset_SprTrm()
    {
        mySprTrm.localPosition = Vector3.zero;
        mySprTrm.localScale = Vector3.one;
        mySprTrm.eulerAngles = Vector3.zero;
        mySprTrm.DOKill();
    }
    public override void Add_StatusEffect(AtkType atkType, params float[] value)
    {

    }

    public override Unit Pull_Unit()
    {
        return null;
    }

    public override Unit Pulling_Unit()
    {
        return null;
    }
    public override void Throw_Unit(Vector2 pos)
    {
        
    }
}

