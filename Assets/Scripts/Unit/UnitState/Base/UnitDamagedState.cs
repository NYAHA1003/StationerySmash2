using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public class UnitDamagedState : UnitState
{
    private AtkData atkData;

    public void Set_AtkData(AtkData atkData)
    {
        this.atkData = atkData;
    }

    public override void Enter()
    {
        _curState = eState.DAMAGED;
        _curEvent = eEvent.ENTER;

        _myUnit.Set_IsDontThrow(true);
        _myUnit.Set_IsInvincibility(true);
        _myUnit.SubtractHP(atkData.damage);
        if (_myUnit.UnitStat.Hp <= 0)
        {
            _stateManager.Set_Die();
            return;
        }
        KnockBack();
        base.Enter();
    }

    private void KnockBack()
    {
        float calculated_knockback = atkData.Caculated_Knockback(_myUnit.UnitStat.Return_Weight(), _myUnit.UnitStat.Hp, _myUnit.UnitStat.MaxHp, _myUnit.ETeam == TeamType.MyTeam);
        float height = atkData.baseKnockback * 0.01f + Utill.Parabola.Caculated_Height((atkData.baseKnockback + atkData.extraKnockback) * 0.15f, atkData.direction, 1);
        float time = atkData.baseKnockback * 0.005f + Mathf.Abs((atkData.baseKnockback * 0.5f + atkData.extraKnockback) / (Physics2D.gravity.y));

        _myTrm.DOKill();
        _mySprTrm.DOKill();
        Animation(time);
        _myTrm.DOJump(new Vector3(_myTrm.position.x - calculated_knockback, 0, _myTrm.position.z), height, 1, time).OnComplete(() =>
        {
            _stateManager.Set_Wait(0.4f);
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
            _myUnit.AddStatusEffect(atkData.atkType, atkData.value);
        }
        _myUnit.Set_IsInvincibility(false);
        base.Exit();
    }
    public override void Animation(params float[] value)
    {
        float rotate = _myUnit.ETeam.Equals(TeamType.MyTeam) ? 360 : -360;
        _mySprTrm.DORotate(new Vector3(0, 0, rotate), value[0], RotateMode.FastBeyond360);
    }
}
