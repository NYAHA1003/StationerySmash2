using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public class SeFiceAttackState : AbstractAttackState
{
    private int _seFiceDamaged = 1;

    /// <summary>
    /// 공격
    /// </summary>
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
            AtkData atkData = new AtkData(_myUnit, _myUnit.UnitStat.Return_Attack(), _myUnit.UnitStat.Return_Knockback(), 0, _myUnitData.dir, _myUnit.ETeam == TeamType.MyTeam, 0, originAtkType, originValue);
            _myUnit.SubtractHP(_seFiceDamaged);
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
