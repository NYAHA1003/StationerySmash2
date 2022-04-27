using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public abstract class AbstractDamagedState : AbstractUnitState
{
    protected AtkData _atkData; //공격받은 데이터
    private float _animationTime = 0f; // 애니메이션에 사용할 시간
    private Sequence _animationJumpTweener; // 점프 애니메이션 트위너

    public override void Enter()
    {
        _curState = eState.DAMAGED;
        _curEvent = eEvent.ENTER;

        //고유효과 속성이면 효과 적용
        if (_atkData.atkType > EffAttackType.Inherence)
        {
            _myUnit.AddInherence(_atkData);
        }

        //무적여부, 데미지 적용
        _myUnit.SetIsDontThrow(true);
        _myUnit.SetIsInvincibility(true);
        _myUnit.BattleManager.CommandEffect.SetEffect(_atkData._effectType, new EffData(_myTrm.transform.position, 0.2f));
        _myUnit.SubtractHP(_atkData.damage * (_myUnit.UnitStat.DamagedPercent / 100) - _myUnit.UnitStat.DamageDecrese); //여기

        //스티커 사용
        _myUnit.UnitSticker.RunDamagedStickerAbility(_curState, ref _atkData);

        //체력이 0 이하면 죽음 상태로 전환
        if (_myUnit.UnitStat.Hp <= 0)
        {
            _stateManager.Set_Die();
            return;
        }

        //넉백 적용
        KnockBack();

        base.Enter();
    }
    public override void Update()
    {
        //넉백중에 스테이지 끝에 닿았는지 체크
        CheckWall();
    }
    public override void Exit()
    {
        //평범한 공격이 아니면 상태이상 적용
        if (_atkData.atkType != EffAttackType.Normal && _atkData.atkType <= EffAttackType.Inherence && _myUnit.UnitStat.Hp > 0)
        {
            _myUnit.AddStatusEffect(_atkData.atkType, _atkData.value);
        }

        //무적 풀기
        _myUnit.SetIsInvincibility(false);

        base.Exit();
    }

    /// <summary>
    /// 공격 데이터 받기
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
    /// 넉백 적용
    /// </summary>
    private void KnockBack()
    {
        //넉백 계산
        float calculated_knockback = _atkData.Caculated_Knockback(_myUnit.UnitStat.Return_Weight(), _myUnit.UnitStat.Hp, _myUnit.UnitStat.MaxHp, _myUnit.ETeam == TeamType.MyTeam);
        float height = _atkData.baseKnockback * 0.01f + Utill.Parabola.Caculated_Height((_atkData.baseKnockback + _atkData.extraKnockback) * 0.15f, _atkData.direction, 1);
        float time = _atkData.baseKnockback * 0.005f + Mathf.Abs((_atkData.baseKnockback * 0.5f + _atkData.extraKnockback) / (Physics2D.gravity.y));
        _animationTime = time;
        //회전 애니메이션
        ResetAllStateAnimation();
        Animation();

        SetKnockBack(_myTrm.DOJump(new Vector3(_myTrm.position.x - calculated_knockback, 0, _myTrm.position.z), height, 1, time).OnComplete(() =>
        {
            _stateManager.Set_Wait(0.4f);
        }));
        
    }
}
