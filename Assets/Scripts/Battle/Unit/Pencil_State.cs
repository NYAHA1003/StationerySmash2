using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;

public class PencilStateChange : IStateChange
{
    public UnitState Return_Attack(Stationary_UnitState unit, Unit targetUnit)
    {
        unit.Set_Event(eEvent.EXIT);
        return new Pencil_Attack_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData, targetUnit);
    }

    public UnitState Return_Damaged(Stationary_UnitState unit, AtkData atkData)
    {
        unit.Set_Event(eEvent.EXIT);
        return new Pencil_Damaged_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData, atkData);
    }

    public UnitState Return_Die(Stationary_UnitState unit)
    {
        unit.Set_Event(eEvent.EXIT);
        return new Pencil_Die_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData);
    }

    public UnitState Return_Idle(Stationary_UnitState unit)
    {
        unit.Set_Event(eEvent.EXIT);
        return new Pencil_Idle_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData);
    }

    public UnitState Return_Move(Stationary_UnitState unit)
    {
        unit.Set_Event(eEvent.EXIT);
        return new Pencil_Move_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData);
    }

    public UnitState Return_Throw(Stationary_UnitState unit)
    {
        unit.Set_Event(eEvent.EXIT);
        return new Pencil_Throw_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData);
    }

    public UnitState Return_Wait(Stationary_UnitState unit, float time)
    {
        unit.Set_Event(eEvent.EXIT);
        return new Pencil_Wait_State(unit.myTrm, unit.mySprTrm, unit.myUnit, unit.stageData, time);
    }
}

public class Pencil_Idle_State : Stationary_UnitState
{
    public Pencil_Idle_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData) : base(myTrm, mySprTrm, myUnit, stageData)
    {
        stateChange = new PencilStateChange();
        curState = eState.IDLE;
        curEvent = eEvent.ENTER;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        nextState = stateChange.Return_Wait(this, 0.5f);
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class Pencil_Wait_State : Stationary_UnitState
{
    private float waitTime;
    public Pencil_Wait_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData, float waitTime) : base(myTrm, mySprTrm, myUnit, stageData)
    {
        stateChange = new PencilStateChange();
        curState = eState.WAIT;
        curEvent = eEvent.ENTER;
        this.waitTime = waitTime;
    }

    public override void Enter()
    {
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
        nextState = stateChange.Return_Move(this);
    }
}

public class Pencil_Move_State : Stationary_UnitState
{
    public Pencil_Move_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData) : base(myTrm, mySprTrm, myUnit, stageData)
    {
        stateChange = new PencilStateChange();
        curState = eState.MOVE;
        curEvent = eEvent.ENTER;
    }

    public override void Enter()
    {
        Animation();
        base.Enter();
    }

    public override void Update()
    {
        //우리 팀
        if (myUnit.eTeam == TeamType.MyTeam)
        {
            Move_MyTeam();
            Check_Range(myUnit.battleManager.unit_EnemyDatasTemp);
            return;
        }

        //상대 팀
        Move_EnemyTeam();
        Check_Range(myUnit.battleManager.unit_MyDatasTemp);
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
                if (myUnit.eTeam == TeamType.MyTeam && myTrm.position.x > list[i].transform.position.x)
                {
                    continue;
                }
                if (myUnit.eTeam != TeamType.MyTeam && myTrm.position.x < list[i].transform.position.x)
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
                nextState = stateChange.Return_Attack(this, targetUnit);
            }
        }

    }

    public override void Animation(params float[] value)
    {
        mySprTrm.DOKill();
        float rotate = myUnit.eTeam == TeamType.MyTeam ? 30 : -30;
        mySprTrm.eulerAngles = new Vector3(0, 0, 0);
        mySprTrm.DORotate(new Vector3(0, 0, rotate), 0.3f).SetLoops(-1, LoopType.Yoyo);
    }
}

public class Pencil_Attack_State : Stationary_UnitState
{
    private Unit targetUnit;
    private float cur_delay = 0;
    private float max_delay = 100;
    public Pencil_Attack_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData, Unit targetUnit) : base(myTrm, mySprTrm, myUnit, stageData)
    {
        stateChange = new PencilStateChange();
        curState = eState.ATTACK;
        curEvent = eEvent.ENTER;
        this.targetUnit = targetUnit;
    }

    public override void Enter()
    {
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

        nextState = stateChange.Return_Wait(this, 0.4f);
        curEvent = eEvent.EXIT;
        if (Random.Range(0,100) <= myUnit.Return_Accuracy())
        {
            myUnit.battleManager.battle_Effect.Set_Effect(EffectType.Attack, targetUnit.transform.position);
            AtkData atkData = new AtkData(myUnit, myUnit.Return_Damage(), myUnit.Return_Knockback(), 0, myUnitData.dir, myUnit.eTeam == TeamType.MyTeam, 0, originAtkType, originValue);
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
                nextState = stateChange.Return_Move(this);
                return;
            }

            if (myUnit.eTeam == TeamType.MyTeam && myTrm.position.x > targetUnit.transform.position.x)
            {
                nextState = stateChange.Return_Move(this);
                return;
            }
            if (myUnit.eTeam != TeamType.MyTeam && myTrm.position.x < targetUnit.transform.position.x)
            {
                nextState = stateChange.Return_Move(this);
                return;
            }
            if(targetUnit.transform.position.y > myTrm.position.y)
            {
                nextState = stateChange.Return_Move(this);
                return;
            }
        }
    }
    public override void Animation(params float[] value)
    {
        mySprTrm.DOKill();
        float rotate = myUnit.eTeam == TeamType.MyTeam ? -90 : 90;
        mySprTrm.eulerAngles = new Vector3(0, 0, 0);
        mySprTrm.DORotate(new Vector3(0, 0, rotate), 0.2f).SetLoops(2, LoopType.Yoyo);
    }
}

public class Pencil_Damaged_State : Stationary_UnitState
{
    private AtkData atkData;

    public Pencil_Damaged_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData, AtkData atkData) : base(myTrm, mySprTrm, myUnit, stageData)
    {
        stateChange = new PencilStateChange();
        curState = eState.DAMAGED;
        curEvent = eEvent.ENTER;
        this.atkData = atkData;
    }

    public override void Enter()
    {
        myUnit.Set_IsInvincibility(true);
        myUnit.Subtract_HP(atkData.damage);
        nextState = stateChange.Return_Wait(this, 0.5f);
        if (myUnit.hp <= 0)
        {
            nextState = stateChange.Return_Die(this);
            return;
        }
        KnockBack();
        base.Enter();
    }

    private void KnockBack()
    {
        float calculated_knockback = atkData.Caculated_Knockback(myUnit.Return_Weight(), myUnit.hp, myUnit.maxhp, myUnit.eTeam == TeamType.MyTeam);
        float height = atkData.baseKnockback * 0.01f + Utill.Utill.Caculated_Height((atkData.baseKnockback + atkData.extraKnockback) * 0.15f, atkData.direction, 1);
        float time = atkData.baseKnockback * 0.005f +  Mathf.Abs((atkData.baseKnockback * 0.5f + atkData.extraKnockback)  / (Physics2D.gravity.y ));
        
        myTrm.DOKill();
        mySprTrm.DOKill();
        Animation(time);
        myTrm.DOJump(new Vector3(myTrm.position.x - calculated_knockback, 0, myTrm.position.z), height, 1, time).OnComplete(() =>
        {
            curEvent = eEvent.EXIT;
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
        float rotate = myUnit.eTeam == TeamType.MyTeam ? 360 : -360;
        mySprTrm.DORotate(new Vector3(0, 0, rotate), value[0], RotateMode.FastBeyond360);
    }
}

public class Pencil_Die_State : Stationary_UnitState
{
    public Pencil_Die_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData) : base(myTrm, mySprTrm, myUnit, stageData)
    {
        stateChange = new PencilStateChange();
        curState = eState.DIE;
        curEvent = eEvent.ENTER;
    }

    public override void Enter()
    {
        myUnit.NoShow_Canvas();
        
        //뒤짐
        myUnit.Set_IsInvincibility(true);
        myTrm.DOKill();
        mySprTrm.DOKill();

        DieType dietype = Utill.Utill.Return_RandomDieType();
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
        if (myUnit.eTeam == TeamType.MyTeam)
        {
            diePos.x -= Random.Range(0.1f, 0.2f);
        }
        if (myUnit.eTeam == TeamType.EnemyTeam)
        {
            diePos.x += Random.Range(0.1f, 0.2f);
        }
        mySprTrm.DOLocalRotate(new Vector3(0, 0, -360), 0.3f, RotateMode.FastBeyond360).SetLoops(3, LoopType.Incremental);
        myTrm.DOJump(diePos, 2f, 1, 1f);
        mySprTrm.DOScale(10, 0.6f).SetDelay(0.3f).SetEase(Utill.Utill.Return_ScreenKoCurve()).OnComplete(() =>
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
        if (myUnit.eTeam == TeamType.MyTeam)
        {
            diePos.x += Random.Range(-2f, -1f);
        }
        if (myUnit.eTeam == TeamType.EnemyTeam)
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
        if (myUnit.eTeam == TeamType.MyTeam)
        {
            diePos.x -= stageData.max_Range + 1;
        }
        if (myUnit.eTeam == TeamType.EnemyTeam)
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
    public Pencil_Throw_State(Transform myTrm, Transform mySprTrm, Stationary_Unit myUnit, StageData stageData) : base(myTrm, mySprTrm, myUnit, stageData)
    {
        stateChange = new PencilStateChange();
        curState = eState.THROW;
        curEvent = eEvent.ENTER;
    }

    public override void Enter()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        //방향
        Vector2 direction = (Vector2)myTrm.position - mousePos;
        float dir = Mathf.Atan2(direction.y, direction.x);
        float dirx = Mathf.Atan2(direction.y, -direction.x);

        if(dir < 0)
        {
            nextState = stateChange.Return_Wait(this, 0.5f);
            return;
        }

        //초기 벡터
        float force = Mathf.Clamp(Vector2.Distance(myTrm.position, mousePos), 0, 1) * 4 * (100.0f / myUnit.Return_Weight());

        //최고점
        float height = Utill.Utill.Caculated_Height(force, dirx);
        //수평 도달 거리
        float width = Utill.Utill.Caculated_Width(force, dirx);
        //수평 도달 시간
        float time = Utill.Utill.Caculated_Time(force, dir, 3);

        mySprTrm.DOKill();
        myTrm.DOJump(new Vector3(myTrm.position.x - width, 0, myTrm.position.z), height, 1, time).OnComplete(() =>
        {
            nextState = stateChange.Return_Wait(this, 0.5f);
        }).SetEase(Utill.Utill.Return_ParabolaCurve());
        
        base.Enter();
    }

    public override void Update()
    {
        Check_Wall();
        if (myUnit.eTeam == TeamType.MyTeam)
        {
            Check_Collide(myUnit.battleManager.unit_EnemyDatasTemp);
            return;
        }
        Check_Collide(myUnit.battleManager.unit_MyDatasTemp);
    }
    private void Check_Collide(List<Unit> list)
    {
        Unit targetUnit = null;
        for (int i = 0; i < list.Count; i++)
        {
            targetUnit = list[i];
            if (Vector2.Distance(myTrm.position, targetUnit.transform.position) < 0.2f)
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

            atkData.Reset_Kncockback(20, 0, dir, true);
            atkData.Reset_Type(AtkType.Stun);
            atkData.Reset_Value(1);
            atkData.Reset_Damage(0);
            myUnit.Run_Damaged(atkData);
            return;
        }

        //무게가 같을 경우
        if (myUnit.weight == targetUnit.weight)
        {
            atkData.Reset_Kncockback(10, extraKnockBack, dir, false);
            atkData.Reset_Type(AtkType.Stun);
            atkData.Reset_Value(1);
            targetUnit.Run_Damaged(atkData);


            atkData.Reset_Kncockback(20, 0, dir, true);
            atkData.Reset_Type(AtkType.Stun);
            atkData.Reset_Value(1);
            atkData.Reset_Damage(0);
            myUnit.Run_Damaged(atkData);

            return;
        }
    }

}

