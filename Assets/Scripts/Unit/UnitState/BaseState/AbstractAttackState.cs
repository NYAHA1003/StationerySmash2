using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public abstract class AbstractAttackState : AbstractUnitState
{
    protected Unit _targetUnit = null; //공격할 유닛
    protected float _currentdelay = 0; //현재 딜레이
    protected float _maxdelay = 100; //끝 딜레이
    private bool isAttacked; //공격 중인지
    public override void Enter()
    {
        isAttacked = false;
        _curState = eState.ATTACK;
        _curEvent = eEvent.ENTER;

        ResetAllStateAnimation();

        //스티커 사용
        _myUnit.UnitSticker.RunStickerAbility(_curState);

        //공격 딜레이를 유닛의 딜레이로 설정
        _currentdelay = _myUnit.UnitStat.AttackDelay;

        base.Enter();
    }
    public override void Update()
    {
        if(!isAttacked)
        {
            //상대와의 거리 체크
            CheckRangeToTarget();

            //쿨타임 감소
            if (AttackDelay())
            {
                Attack();
            }
        }
    }
    public override void Animation()
    {
        float rotate = _myUnit.ETeam.Equals(TeamType.MyTeam) ? -90 : 90;
        _animationTweener.ChangeEndValue(new Vector3(0, 0, rotate));
        _animationTweener.Restart();
    }
    public override void SetAnimation()
    {
       _animationTweener = _mySprTrm.DORotate(new Vector3(0, 0, 0), 0.2f).SetLoops(2, LoopType.Yoyo).SetAutoKill(false);
       _animationTweener.OnComplete(() => _stateManager.Set_Wait(0.4f)).SetAutoKill(false);
    }

    /// <summary>
    /// 공격할 유닛 설정
    /// </summary>
    /// <param name="targetUnit"></param>
    public void Set_Target(Unit targetUnit)
    {
        this._targetUnit = targetUnit;
    }


    /// <summary>
    /// 공격
    /// </summary>
    protected virtual void Attack()
    {
        isAttacked = true;

        //공격 애니메이션
        Animation();

        //공격 딜레이 초기화
        _currentdelay = 0;
        SetUnitDelayAndUI();


        //공격 명중률에 따라 미스가 뜬다.
        if (Random.Range(0, 100) <= _myUnit.UnitStat.Return_Accuracy())
        {
            AtkData atkData = null;
            SetAttackData(ref atkData);
            _targetUnit.Run_Damaged(atkData);
            _targetUnit = null;
            return;
        }
        else
        {
            Debug.Log("미스");
        }
    }

    /// <summary>
    /// 유닛의 딜레이랑, 딜레이바 UI 수정
    /// </summary>
    protected void SetUnitDelayAndUI()
    {
        _myUnit.UnitSprite.UpdateDelayBar(_currentdelay / _maxdelay);
        _myUnit.UnitStat.SetAttackDelay(_currentdelay);
    }

    /// <summary>
    /// 타겟과의 거리 체크
    /// </summary>
    protected virtual void CheckRangeToTarget()
    {
        if (_targetUnit == null)
        {
            _stateManager.Set_Move();
            return;
        }
        if (Vector2.Distance(_myTrm.position, _targetUnit.transform.position) > _myUnit.UnitStat.Return_Range())
        {
            _stateManager.Set_Move();
            return;
        }
        if (_myUnit.ETeam == TeamType.MyTeam && _myTrm.position.x > _targetUnit.transform.position.x)
        {
            _stateManager.Set_Move();
            return;
        }
        if (_myUnit.ETeam == TeamType.EnemyTeam && _myTrm.position.x < _targetUnit.transform.position.x)
        {
            _stateManager.Set_Move();
            return;
        }
        if (_targetUnit.transform.position.y > _myTrm.position.y)
        {
            _stateManager.Set_Move();
            return;
        }
    }

    /// <summary>
    /// 공격 데이터 설정
    /// </summary>
    /// <param name="atkData"></param>
    protected virtual void SetAttackData(ref AtkData atkData)
    {
        atkData = new AtkData(_myUnit, _myUnit.UnitStat.Return_Attack(), _myUnit.UnitStat.Return_Knockback(), 0, _myUnitData.dir, _myUnit.ETeam == TeamType.MyTeam, 0, AtkType.Normal, _myUnit.SkinData.effectType, originValue);
    }

    /// <summary>
    /// 공격할 수 있을 때 까지 대기한다
    /// </summary>
    /// <returns>True면 공격, 아니면 딜레이</returns>
    private bool AttackDelay()
    {
        if (_maxdelay >= _currentdelay || _targetUnit._isInvincibility)
        {
            _currentdelay += _myUnit.UnitStat.Return_AttackSpeed() * Time.deltaTime;
            SetUnitDelayAndUI();
            return false;
        }
        return true;
    }

}
