using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public abstract class AbstractDamagedState : AbstractUnitState
{
    protected AtkData _atkData; //���ݹ��� ������
    private float _animationTime = 0f; // �ִϸ��̼ǿ� ����� �ð�
    private Sequence _animationJumpTweener; // ���� �ִϸ��̼� Ʈ����

    public override void Enter()
    {
        _curState = eState.DAMAGED;
        _curEvent = eEvent.ENTER;

        //����ȿ�� �Ӽ��̸� ȿ�� ����
        if (_atkData.atkType > EffAttackType.Inherence)
        {
            _myUnit.AddInherence(_atkData);
        }

        //��������, ������ ����
        _myUnit.SetIsDontThrow(true);
        _myUnit.SetIsInvincibility(true);
        _myUnit.BattleManager.CommandEffect.SetEffect(_atkData._effectType, new EffData(_myTrm.transform.position, 0.2f));
        _myUnit.SubtractHP(_atkData.damage * (_myUnit.UnitStat.DamagedPercent / 100) - _myUnit.UnitStat.DamageDecrese); //����

        //��ƼĿ ���
        _myUnit.UnitSticker.RunDamagedStickerAbility(_curState, ref _atkData);

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
        if (_atkData.atkType != EffAttackType.Normal && _atkData.atkType <= EffAttackType.Inherence && _myUnit.UnitStat.Hp > 0)
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
    public override void Animation()
    {
        float rotate = _myUnit.ETeam.Equals(TeamType.MyTeam) ? -360 : 360;
        _animationTweener.ChangeEndValue(new Vector3(0, 0, rotate), _animationTime);
        _animationTweener.Restart();
    }
    public override void SetAnimation()
    {
        _animationTweener = _mySprTrm.DORotate(new Vector3(0, 0, 0), 1, RotateMode.FastBeyond360).SetAutoKill(false);
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
        _animationTime = time;
        //ȸ�� �ִϸ��̼�
        ResetAllStateAnimation();
        Animation();

        SetKnockBack(_myTrm.DOJump(new Vector3(_myTrm.position.x - calculated_knockback, 0, _myTrm.position.z), height, 1, time).OnComplete(() =>
        {
            _stateManager.Set_Wait(0.4f);
        }));
        
    }
}
