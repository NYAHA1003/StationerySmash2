using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public abstract class AbstractDamagedState : AbstractUnitState
{
    protected AtkData _atkData; //���ݹ��� ������
    private float _animationRotate = 0f; //�ִϸ��̼ǿ� ����� ����
    private float _animationTime = 0f; // �ִϸ��̼ǿ� ����� �ð�
    private Tweener _animationTweener = null;

    public override void Enter()
    {
        _curState = eState.DAMAGED;
        _curEvent = eEvent.ENTER;

        //����ȿ�� �Ӽ��̸� ȿ�� ����
        if (_atkData.atkType > AtkType.Inherence)
        {
            _myUnit.AddInherence(_atkData);
        }

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
        if (_atkData.atkType != AtkType.Normal && _atkData.atkType <= AtkType.Inherence && _myUnit.UnitStat.Hp > 0)
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
        ResetAnimation();
        _animationSequence.Restart();
    }
    public override void SetAnimation()
    {
        _animationSequence.Append(_mySprTrm.DORotate(new Vector3(0, 0, _animationRotate), _animationTime, RotateMode.FastBeyond360));
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
        _myTrm.DOKill();
        Animation();

        //����
        _myTrm.DOJump(new Vector3(_myTrm.position.x - calculated_knockback, 0, _myTrm.position.z), height, 1, time).OnComplete(() =>
        {
            _stateManager.Set_Wait(0.4f);
        });
    }
}
