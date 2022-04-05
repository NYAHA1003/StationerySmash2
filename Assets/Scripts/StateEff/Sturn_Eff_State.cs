using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class Sturn_Eff_State : EffState
{
    private float stunTime = 0.0f; // 스턴 시간

    public override void Enter()
    {
        stunTime = stunTime + (stunTime * (((float)_myUnit.UnitStat.MaxHp / (_myUnit.UnitStat.Hp + 70)) - 1));
        _myUnit.Set_IsDontThrow(true);
        _stateManager.Set_Wait(stunTime);
        _stateManager.Set_WaitExtraTime(stunTime);

        //이펙트 오브젝트 가져오기
        _effectObj = _battleManager.CommandEffect.SetEffect(EffectType.Stun, new EffData(new Vector2(Trm.position.x, Trm.position.y + 0.1f), stunTime, Trm));

        base.Enter();
    }

    public override void Update()
    {
        StunTimer();
    }

    public override void Exit()
    {
        DeleteEffectObject();
        base.Exit();
    }

    public override void SetEffValue(params float[] value)
    {
        if (stunTime < value[0])
        {
            stunTime = value[0];
            stunTime = stunTime + (stunTime * (((float)_myUnit.UnitStat.MaxHp / (_myUnit.UnitStat.Hp + 70)) - 1));
        }
    }

    /// <summary>
    /// 스턴타임 유지 시간
    /// </summary>
    private void StunTimer()
    {
        if (stunTime > 0)
        {
            stunTime -= Time.deltaTime;
            _stateManager.Set_WaitExtraTime(stunTime);
            return;
        }
        _myUnit.Set_IsDontThrow(false);
        _curEvent = eEvent.EXIT;
    }
}