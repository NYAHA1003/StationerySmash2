using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public class SeFiceAttackState : AbstractAttackState
{
    private int _seFiceDamaged = 1;

    /// <summary>
    /// ����
    /// </summary>
    protected override void Attack()
    {
        //���� �ִϸ��̼�
        Animation();

        //���� ������ �ʱ�ȭ
        _currentdelay = 0;
        SetUnitDelayAndUI();

        //��� ���·� ���ư�
        _stateManager.Set_Wait(0.4f);
        _curEvent = eEvent.EXIT;


        //���� ���߷��� ���� �̽��� ���.
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
            Debug.Log("�̽�");
        }
    }
}
