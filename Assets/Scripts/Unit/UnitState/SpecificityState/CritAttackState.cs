using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public class CritAttackState : AbstractAttackState
{
    protected override void Attack()
    {
        //공격 애니메이션
        Animation();

        //공격 딜레이 초기화
        _currentdelay = 0;
        SetUnitDelayAndUI();

        //대기 상태로 돌아감
        _stateManager.Set_Wait(0.4f);
        _curEvent = eEvent.EXIT;


        //공격 명중률에 따라 미스가 뜬다.
        if (Random.Range(0, 100) <= _myUnit.UnitStat.Return_Accuracy())
        {
            _myUnit.BattleManager.CommandEffect.SetEffect(EffectType.Attack, new EffData(_targetUnit.transform.position, 0.2f));
            AtkData atkData = new AtkData(_myUnit, _myUnit.UnitStat.Return_Attack(), _myUnit.UnitStat.Return_Knockback(), 0, _myUnitData.dir, _myUnit.ETeam == TeamType.MyTeam, 0, originAtkType, originValue);
            if (Random.Range(0, 100) <= _myUnit.UnitStat.Return_Accuracy() / 10)
            {
                //크리티컬
                atkData.Reset_Damage(atkData.damage * 2);
            }
            else
            {
                atkData.Reset_Damage(atkData.damage);
            }

            _targetUnit.Run_Damaged(atkData);
            _targetUnit = null;
            return;
        }
        else
        {
            Debug.Log("미스");
        }
    }
}
