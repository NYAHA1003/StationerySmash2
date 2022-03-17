using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;

public class PencilStateManager : IStateManager
{
    private Pencil_Idle_State IdleState;
    private Pencil_Attack_State AttackState;
    private Pencil_Damaged_State DamagedState;
    private Pencil_Throw_State ThrowState;
    private Pencil_Die_State DieState;
    private Pencil_Move_State MoveState;
    private Pencil_Wait_State WaitState;
    private UnitState cur_unitState;

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
        IdleState = new Pencil_Idle_State(myTrm, mySprTrm, myUnit);
        WaitState = new Pencil_Wait_State(myTrm, mySprTrm, myUnit);
        MoveState = new Pencil_Move_State(myTrm, mySprTrm, myUnit);
        AttackState = new Pencil_Attack_State(myTrm, mySprTrm, myUnit);
        DamagedState = new Pencil_Damaged_State(myTrm, mySprTrm, myUnit);
        DieState = new Pencil_Die_State(myTrm, mySprTrm, myUnit);
        ThrowState = new Pencil_Throw_State(myTrm, mySprTrm, myUnit);

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

        Set_WaitExtraTime(0);
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
}

public class Pencil_Idle_State : Stationary_UnitState
{
    public Pencil_Idle_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }

    public override void Enter()
    {
        curState = eState.IDLE;
        curEvent = eEvent.ENTER;

        stateChange.Set_Wait(0.5f);
    }
}

public class Pencil_Wait_State : Stationary_UnitState
{
    private float waitTime;
    private float extraWaitTime;
    public Pencil_Wait_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }

    public void Set_Time(float waitTime)
    {
        this.waitTime = waitTime;
    }
    public void Set_ExtraTime(float extraWaitTime)
    {
        this.extraWaitTime = extraWaitTime;
        this.waitTime = waitTime + extraWaitTime;
    }

    public override void Enter()
    {
        curState = eState.WAIT;
        curEvent = eEvent.ENTER;
        mySprTrm.DOKill();
        base.Enter();
    }

    public override void Update()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            return;
        }
        stateChange.Set_Move();
    }
}

public class Pencil_Move_State : Stationary_UnitState
{
    public Pencil_Move_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }

    public override void Enter()
    {
        myUnit.Set_IsDontThrow(false);
        curState = eState.MOVE;
        curEvent = eEvent.ENTER;
        Animation();
        base.Enter();
    }

    public override void Update()
    {
        //우리 팀
        switch (myUnit.eTeam)
        {
            case TeamType.Null:
                break;
            case TeamType.MyTeam:
                Move_MyTeam();
                Check_Range(myUnit.battleManager.unit_EnemyDatasTemp);
                return;
            case TeamType.EnemyTeam:
                Move_EnemyTeam();
                Check_Range(myUnit.battleManager.unit_MyDatasTemp);
                return;
        }
    }

    private void Move_MyTeam()
    {
        if(myTrm.transform.position.x < stageData.max_Range - 0.1f)
        {
            myTrm.Translate(Vector2.right * myUnit.Return_MoveSpeed() * Time.deltaTime);
        }
    }

    private void Move_EnemyTeam()
    {
        if (myTrm.transform.position.x > -stageData.max_Range + 0.1f)
        {
            myTrm.Translate(Vector2.left * myUnit.Return_MoveSpeed() * Time.deltaTime);
        }
    }

    private void Check_Range(List<Unit> list)
    {
        float targetRange = float.MaxValue;
        Unit targetUnit = null;
        for (int i = 0; i < list.Count; i++)
        {
            if (Vector2.Distance(myTrm.position, list[i].transform.position) < targetRange)
            {
                if (myUnit.eTeam.Equals(TeamType.MyTeam) && myTrm.position.x > list[i].transform.position.x)
                {
                    continue;
                }
                if (!myUnit.eTeam.Equals(TeamType.MyTeam) && myTrm.position.x < list[i].transform.position.x)
                {
                    continue;
                }
                if(list[i].transform.position.y > myTrm.transform.position.y)
                {
                    continue;
                }
                if(list[i].isInvincibility)
                {
                    continue;
                }

                targetUnit = list[i];
                targetRange = Vector2.Distance(myTrm.position, targetUnit.transform.position);
            }
        }
        
        if(targetUnit != null)
        {
            if (Vector2.Distance(myTrm.position, targetUnit.transform.position) < myUnit.Return_Range())
            {
                stateChange.Set_Attack(targetUnit);
            }
        }

    }

    public override void Animation(params float[] value)
    {
        mySprTrm.DOKill();
        float rotate = myUnit.eTeam.Equals(TeamType.MyTeam) ? 30 : -30;
        mySprTrm.eulerAngles = new Vector3(0, 0, 0);
        mySprTrm.DORotate(new Vector3(0, 0, rotate), 0.3f).SetLoops(-1, LoopType.Yoyo);
    }
}

public class Pencil_Attack_State : Stationary_UnitState
{
    private Unit targetUnit;
    private float cur_delay = 0;
    private float max_delay = 100;
    public Pencil_Attack_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
    }

    public void Set_Target(Unit targetUnit)
    {
        this.targetUnit = targetUnit;
    }

    public override void Enter()
    {
        curState = eState.ATTACK;
        curEvent = eEvent.ENTER;
        cur_delay = myUnit.attack_Cur_Delay;
        base.Enter();
    }
    public override void Update()
    {
        //상대와의 거리 체크
        Check_Range();

        //쿨타임 감소
        if(max_delay >= cur_delay || targetUnit.isInvincibility)
        {
            cur_delay += myUnit.Return_AttackSpeed() * Time.deltaTime;
            Set_Delay();
            return;
        }

        Attack();
    }

    private void Attack()
    {
        Animation();

        cur_delay = 0;
        Set_Delay();

        stateChange.Set_Wait(0.4f);
        curEvent = eEvent.EXIT;
        if (Random.Range(0,100) <= myUnit.Return_Accuracy())
        {
            myUnit.battleManager.battle_Effect.Set_Effect(EffectType.Attack, new EffData(targetUnit.transform.position, 0.2f));
            AtkData atkData = new AtkData(myUnit, myUnit.Return_Damage(), myUnit.Return_Knockback(), 0, myUnitData.dir, myUnit.eTeam.Equals(TeamType.MyTeam), 0, originAtkType, originValue);
            targetUnit.Run_Damaged(atkData);
            targetUnit = null;
            return;
        }
        Debug.Log("미스");
    }

    private void Set_Delay()
    {
        myUnit.Update_DelayBar(cur_delay / max_delay);
        myUnit.Set_AttackDelay(cur_delay);
    }

    private void Check_Range()
    {
        if (targetUnit != null)
        {
            if (Vector2.Distance(myTrm.position, targetUnit.transform.position) > myUnit.Return_Range())
            {
                stateChange.Set_Move();
                return;
            }

            if (myUnit.eTeam.Equals(TeamType.MyTeam) && myTrm.position.x > targetUnit.transform.position.x)
            {
                stateChange.Set_Move();
                return;
            }
            if (myUnit.eTeam.Equals(TeamType.EnemyTeam) && myTrm.position.x < targetUnit.transform.position.x)
            {
                stateChange.Set_Move();
                return;
            }
            if(targetUnit.transform.position.y > myTrm.position.y)
            {
                stateChange.Set_Move();
                return;
            }
        }
    }
    public override void Animation(params float[] value)
    {
        mySprTrm.DOKill();
        float rotate = myUnit.eTeam.Equals(TeamType.MyTeam) ? -90 : 90;
        mySprTrm.eulerAngles = new Vector3(0, 0, 0);
        mySprTrm.DORotate(new Vector3(0, 0, rotate), 0.2f).SetLoops(2, LoopType.Yoyo);
    }
}

public class Pencil_Damaged_State : Stationary_UnitState
{
    private AtkData atkData;

    public Pencil_Damaged_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
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

        myUnit.Set_IsDontThrow(true);
        myUnit.Set_IsInvincibility(true);
        myUnit.Subtract_HP(atkData.damage);
        if (myUnit.hp <= 0)
        {
            stateChange.Set_Die();
            return;
        }
        KnockBack();
        base.Enter();
    }

    private void KnockBack()
    {
        float calculated_knockback = atkData.Caculated_Knockback(myUnit.Return_Weight(), myUnit.hp, myUnit.maxhp, myUnit.eTeam.Equals(TeamType.MyTeam));
        float height = atkData.baseKnockback * 0.01f + Utill.Parabola.Caculated_Height((atkData.baseKnockback + atkData.extraKnockback) * 0.15f, atkData.direction, 1);
        float time = atkData.baseKnockback * 0.005f +  Mathf.Abs((atkData.baseKnockback * 0.5f + atkData.extraKnockback)  / (Physics2D.gravity.y ));
        
        myTrm.DOKill();
        mySprTrm.DOKill();
        Animation(time);
        myTrm.DOJump(new Vector3(myTrm.position.x - calculated_knockback, 0, myTrm.position.z), height, 1, time).OnComplete(() =>
        {
            stateChange.Set_Wait(0.4f);
        });
    }

    public override void Update()
    {
        Check_Wall();
    }

    public override void Exit()
    {
        if (atkData.atkType != AtkType.Normal)
        {
            myUnit.Add_StatusEffect(atkData.atkType, atkData.value);
        }
        myUnit.Set_IsInvincibility(false);
        base.Exit();
    }
    public override void Animation(params float[] value)
    {
        float rotate = myUnit.eTeam.Equals(TeamType.MyTeam) ? 360 : -360;
        mySprTrm.DORotate(new Vector3(0, 0, rotate), value[0], RotateMode.FastBeyond360);
    }
}

public class Pencil_Die_State : Stationary_UnitState
{
    public Pencil_Die_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
        curState = eState.DIE;
        curEvent = eEvent.ENTER;
    }

    public override void Enter()
    {
        myUnit.Delete_EffStetes();
        myUnit.Set_IsDontThrow(true);
        myUnit.Show_Canvas(false);
        
        //뒤짐
        myUnit.Set_IsInvincibility(true);
        myTrm.DOKill();
        mySprTrm.DOKill();

        DieType dietype = Utill.Die.Return_RandomDieType();
        switch (dietype)
        {
            case DieType.StarKo:
                Animation_StarKO();
                break;
            case DieType.ScreenKo:
                Animation_ScreenKO();
                break;
            case DieType.OutKo:
                Animation_OutKO();
                break;
        }

        base.Enter();
    }

    private void Reset_SprTrm()
    {
        mySprTrm.localPosition = Vector3.zero;
        mySprTrm.localScale = Vector3.one;
        mySprTrm.eulerAngles = Vector3.zero;
        mySprTrm.DOKill();
    }

    private void Animation_ScreenKO()
    {
        Vector3 diePos = new Vector3(myTrm.position.x, myTrm.position.y + 0.4f, 0);
        if (myUnit.eTeam.Equals(TeamType.MyTeam))
        {
            diePos.x -= Random.Range(0.1f, 0.2f);
        }
        if (myUnit.eTeam.Equals(TeamType.EnemyTeam))
        {
            diePos.x += Random.Range(0.1f, 0.2f);
        }
        mySprTrm.DOLocalRotate(new Vector3(0, 0, -360), 0.3f, RotateMode.FastBeyond360).SetLoops(3, LoopType.Incremental);
        myTrm.DOJump(diePos, 2f, 1, 1f);
        mySprTrm.DOScale(10, 0.6f).SetDelay(0.3f).SetEase(Utill.Parabola.Return_ScreenKoCurve()).OnComplete(() =>
        {
            mySprTrm.eulerAngles = new Vector3(0, 0, Random.Range(mySprTrm.eulerAngles.z - 10, mySprTrm.eulerAngles.z + 10));
            mySprTrm.DOShakePosition(0.6f, 0.1f, 30).OnComplete(() =>
            {
                mySprTrm.DOMoveY(-3, 1).OnComplete(() =>
                {
                    Reset_SprTrm();
                    curEvent = eEvent.EXIT;
                    myUnit.Delete_Unit();
                });
            });

        });
    }

    private void Animation_StarKO()
    {
        Vector3 diePos = new Vector3(myTrm.position.x, 1, 0);
        if (myUnit.eTeam.Equals(TeamType.MyTeam))
        {
            diePos.x += Random.Range(-2f, -1f);
        }
        if (myUnit.eTeam.Equals(TeamType.EnemyTeam))
        {
            diePos.x += Random.Range(1f, 2f);
        }

        mySprTrm.DOLocalRotate(new Vector3(0, 0, -360), 0.5f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
        mySprTrm.DOScale(0.1f, 1f).SetDelay(1);
        myTrm.DOJump(diePos, 3, 1, 2f).OnComplete(() =>
        {
            Reset_SprTrm();
            curEvent = eEvent.EXIT;
            myUnit.Delete_Unit();
        });
    }

    private void Animation_OutKO()
    {
        Vector3 diePos = new Vector3(myTrm.position.x, myTrm.position.y -2, 0);
        if (myUnit.eTeam.Equals(TeamType.MyTeam))
        {
            diePos.x -= stageData.max_Range + 1;
        }
        if (myUnit.eTeam.Equals(TeamType.EnemyTeam))
        {
            diePos.x += stageData.max_Range + 1;
        }

        float time = Mathf.Abs(myTrm.position.x - diePos.x) / 2;
        mySprTrm.DOScale(3f, time);
        mySprTrm.DOLocalRotate(new Vector3(0, 0, -360), time, RotateMode.FastBeyond360);
        myTrm.DOJump(diePos, Random.Range(3, 5), 1, time).OnComplete(() =>
        {
            Reset_SprTrm();
            curEvent = eEvent.EXIT;
            myUnit.Delete_Unit();
        });
    }

}

public class Pencil_Throw_State : Stationary_UnitState
{
    Vector2 mousePos = Vector2.zero;
    public Pencil_Throw_State(Transform myTrm, Transform mySprTrm, Unit myUnit) : base(myTrm, mySprTrm, myUnit)
    {
        curState = eState.THROW;
        curEvent = eEvent.ENTER;
    }

    public void Set_ThrowPos(Vector2 pos)
    {
        mousePos = pos;
    }

    public override void Enter()
    {
        myUnit.Set_IsDontThrow(true);
        //방향
        Vector2 direction = (Vector2)myTrm.position - mousePos;
        float dir = Mathf.Atan2(direction.y, direction.x);
        float dirx = Mathf.Atan2(direction.y, -direction.x);

        if(dir < 0)
        {
            stateChange.Set_Wait(0.5f);
            return;
        }

        //초기 벡터
        float force = Mathf.Clamp(Vector2.Distance(myTrm.position, mousePos), 0, 1) * 4 * (100.0f / myUnit.Return_Weight());

        //최고점
        float height = Parabola.Caculated_Height(force, dirx);
        //수평 도달 거리
        float width = Parabola.Caculated_Width(force, dirx);
        //수평 도달 시간
        float time = Parabola.Caculated_Time(force, dir, 3);

        mySprTrm.DOKill();
        myTrm.DOJump(new Vector3(myTrm.position.x - width, 0, myTrm.position.z), height, 1, time).OnComplete(() =>
        {
            stateChange.Set_Wait(0.5f);
        }).SetEase(Utill.Parabola.Return_ParabolaCurve());
        
        base.Enter();
    }

    public override void Update()
    {
        Check_Wall();
        if (myUnit.eTeam.Equals(TeamType.MyTeam))
        {
            Check_Collide(myUnit.battleManager.unit_EnemyDatasTemp);
            return;
        }
        if (myUnit.eTeam.Equals(TeamType.EnemyTeam))
        {
            Check_Collide(myUnit.battleManager.unit_MyDatasTemp);
            return;
        }
    }
    private void Check_Collide(List<Unit> list)
    {
        Unit targetUnit = null;
        for (int i = 0; i < list.Count; i++)
        {
            targetUnit = list[i];
            float distance = Utill.Collider.FindDistanceBetweenSegments(myUnit.collideData.Set_Pos(myTrm.position), targetUnit.collideData.Set_Pos(targetUnit.transform.position));
            if (distance < 0.2f)
            {
                Run_ThrowAttack(targetUnit);
            }
        }
    }

    protected virtual void Run_ThrowAttack(Unit targetUnit)
    {
        float dir = Vector2.Angle((Vector2)myTrm.position, (Vector2)targetUnit.transform.position);
        float extraKnockBack = (targetUnit.weight - myUnit.Return_Weight() * (float)targetUnit.hp / targetUnit.maxhp) * 0.025f;
        AtkData atkData = new AtkData(myUnit, 0, 0, 0, 0, true, 0, AtkType.Normal);
        AtkData atkDataMy = new AtkData(myUnit, 0, 0, 0, 0, true, 0, AtkType.Normal);
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
            atkData.Reset_Value(null);
            targetUnit.Run_Damaged(atkData);

            atkDataMy.Reset_Kncockback(20, 0, dir, true);
            atkDataMy.Reset_Type(AtkType.Stun);
            atkDataMy.Reset_Value(1);
            atkDataMy.Reset_Damage(0);
            myUnit.Run_Damaged(atkDataMy);
            return;
        }

        //무게가 같을 경우
        if (myUnit.weight.Equals(targetUnit.weight))
        {
            atkData.Reset_Kncockback(10, extraKnockBack, dir, false);
            atkData.Reset_Type(AtkType.Stun);
            atkData.Reset_Value(1);
            targetUnit.Run_Damaged(atkData);


            atkDataMy.Reset_Kncockback(20, 0, dir, true);
            atkDataMy.Reset_Type(AtkType.Normal);
            atkDataMy.Reset_Value(1);
            atkDataMy.Reset_Damage(0);
            myUnit.Run_Damaged(atkDataMy);

            return;
        }
    }

}

