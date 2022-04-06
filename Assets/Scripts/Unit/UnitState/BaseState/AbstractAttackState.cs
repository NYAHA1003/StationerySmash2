using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public abstract class AbstractAttackState : AbstractUnitState
{
    private Unit targetUnit;
    private float cur_delay = 0;
    private float max_delay = 100;
    public void Set_Target(Unit targetUnit)
    {
        this.targetUnit = targetUnit;
    }

    public override void Enter()
    {
        _curState = eState.ATTACK;
        _curEvent = eEvent.ENTER;
        cur_delay = _myUnit.UnitStat.AttackDelay;
        base.Enter();
    }
    public override void Update()
    {
        //상대와의 거리 체크
        Check_Range();

        //쿨타임 감소
        if (max_delay >= cur_delay || targetUnit._isInvincibility)
        {
            cur_delay += _myUnit.UnitStat.Return_AttackSpeed() * Time.deltaTime;
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

        _stateManager.Set_Wait(0.4f);
        _curEvent = eEvent.EXIT;
        if (Random.Range(0, 100) <= _myUnit.UnitStat.Return_Accuracy())
        {
            _myUnit.BattleManager.CommandEffect.SetEffect(EffectType.Attack, new EffData(targetUnit.transform.position, 0.2f));
            AtkData atkData = new AtkData(_myUnit, _myUnit.UnitStat.Return_Damage(), _myUnit.UnitStat.Return_Knockback(), 0, _myUnitData.dir, _myUnit.ETeam == TeamType.MyTeam, 0, originAtkType, originValue);
            targetUnit.Run_Damaged(atkData);
            targetUnit = null;
            return;
        }
        Debug.Log("미스");
    }

    private void Set_Delay()
    {
        _myUnit.UnitSprite.UpdateDelayBar(cur_delay / max_delay);
        _myUnit.UnitStat.SetAttackDelay(cur_delay);
    }

    private void Check_Range()
    {
        if (targetUnit != null)
        {
            if (Vector2.Distance(_myTrm.position, targetUnit.transform.position) > _myUnit.UnitStat.Return_Range())
            {
                _stateManager.Set_Move();
                return;
            }

            if (_myUnit.ETeam == TeamType.MyTeam & _myTrm.position.x > targetUnit.transform.position.x)
            {
                _stateManager.Set_Move();
                return;
            }
            if (_myUnit.ETeam == TeamType.EnemyTeam && _myTrm.position.x < targetUnit.transform.position.x)
            {
                _stateManager.Set_Move();
                return;
            }
            if (targetUnit.transform.position.y > _myTrm.position.y)
            {
                _stateManager.Set_Move();
                return;
            }
        }
    }
    public override void Animation(params float[] value)
    {
        _mySprTrm.DOKill();
        float rotate = _myUnit.ETeam.Equals(TeamType.MyTeam) ? -90 : 90;
        _mySprTrm.eulerAngles = new Vector3(0, 0, 0);
        _mySprTrm.DORotate(new Vector3(0, 0, rotate), 0.2f).SetLoops(2, LoopType.Yoyo);
    }
}
