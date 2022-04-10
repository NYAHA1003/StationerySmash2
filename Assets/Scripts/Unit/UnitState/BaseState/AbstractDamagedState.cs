using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public abstract class AbstractDamagedState : AbstractUnitState
{
    protected AtkData _atkData; //���ݹ��� ������
    public override void Enter()
    {
        _curState = eState.DAMAGED;
        _curEvent = eEvent.ENTER;

        //��������, ������ ����
        _myUnit.SetIsDontThrow(true);
        _myUnit.SetIsInvincibility(true);
        _myUnit.BattleManager.CommandEffect.SetEffect(_atkData._effectType, new EffData(_myTrm.transform.position, 0.2f));
        _myUnit.SubtractHP(_atkData.damage * (_myUnit.UnitStat.DamagedPercent / 100));

        //��ƼĿ ���
        _myUnit.UnitSticker.RunStickerAbility(_curState);

        //ü���� 0 ���ϸ� ���� ���·� ��ȯ
        if (_myUnit.UnitStat.Hp <= 0)
        {
            _stateManager.Set_Die();
            return;
        }

        //�˹� ����
        KnockBack();

        base.Enter();
    }
    public override void Update()
    {
        //�˹��߿� �������� ���� ��Ҵ��� üũ
        CheckWall();
    }
    public override void Exit()
    {
        //����� ������ �ƴϸ� �����̻� ����
        if (_atkData.atkType != AtkType.Normal)
        {
            _myUnit.AddStatusEffect(_atkData.atkType, _atkData.value);
        }

        //���� Ǯ��
        _myUnit.SetIsInvincibility(false);
        
        base.Exit();
    }
    
    /// <summary>
    /// ���� ������ �ޱ�
    /// </summary>
    /// <param name="atkData"></param>
    public void Set_AtkData(AtkData atkData)
    {
        _atkData = atkData;
    }
    public override void Animation(params float[] value)
    {
        ResetAnimation();
        float rotate = _myUnit.ETeam.Equals(TeamType.MyTeam) ? 360 : -360;
        _mySprTrm.DORotate(new Vector3(0, 0, rotate), value[0], RotateMode.FastBeyond360);
    }

    /// <summary>
    /// �˹� ����
    /// </summary>
    private void KnockBack()
    {
        //�˹� ���
        float calculated_knockback = _atkData.Caculated_Knockback(_myUnit.UnitStat.Return_Weight(), _myUnit.UnitStat.Hp, _myUnit.UnitStat.MaxHp, _myUnit.ETeam == TeamType.MyTeam);
        float height = _atkData.baseKnockback * 0.01f + Utill.Parabola.Caculated_Height((_atkData.baseKnockback + _atkData.extraKnockback) * 0.15f, _atkData.direction, 1);
        float time = _atkData.baseKnockback * 0.005f + Mathf.Abs((_atkData.baseKnockback * 0.5f + _atkData.extraKnockback) / (Physics2D.gravity.y));

        //ȸ�� �ִϸ��̼�
        _myTrm.DOKill();
        Animation(time);

        //����
        _myTrm.DOJump(new Vector3(_myTrm.position.x - calculated_knockback, 0, _myTrm.position.z), height, 1, time).OnComplete(() =>
        {
            _stateManager.Set_Wait(0.4f);
        });
    }
}
