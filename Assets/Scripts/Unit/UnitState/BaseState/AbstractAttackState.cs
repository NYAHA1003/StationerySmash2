using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public abstract class AbstractAttackState : AbstractUnitState
{
    protected Unit _targetUnit = null; //������ ����
    protected float _currentdelay = 0; //���� ������
    protected float _maxdelay = 100; //�� ������
    private bool isAttacked; //���� ������
    public override void Enter()
    {
        isAttacked = false;
        _curState = eState.ATTACK;
        _curEvent = eEvent.ENTER;

        ResetAllStateAnimation();

        //��ƼĿ ���
        _myUnit.UnitSticker.RunStickerAbility(_curState);

        //���� �����̸� ������ �����̷� ����
        _currentdelay = _myUnit.UnitStat.AttackDelay;

        base.Enter();
    }
    public override void Update()
    {
        if(!isAttacked)
        {
            //������ �Ÿ� üũ
            CheckRangeToTarget();

            //��Ÿ�� ����
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
    /// ������ ���� ����
    /// </summary>
    /// <param name="targetUnit"></param>
    public void Set_Target(Unit targetUnit)
    {
        this._targetUnit = targetUnit;
    }


    /// <summary>
    /// ����
    /// </summary>
    protected virtual void Attack()
    {
        isAttacked = true;

        //���� �ִϸ��̼�
        Animation();

        //���� ������ �ʱ�ȭ
        _currentdelay = 0;
        SetUnitDelayAndUI();


        //���� ���߷��� ���� �̽��� ���.
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
            Debug.Log("�̽�");
        }
    }

    /// <summary>
    /// ������ �����̶�, �����̹� UI ����
    /// </summary>
    protected void SetUnitDelayAndUI()
    {
        _myUnit.UnitSprite.UpdateDelayBar(_currentdelay / _maxdelay);
        _myUnit.UnitStat.SetAttackDelay(_currentdelay);
    }

    /// <summary>
    /// Ÿ�ٰ��� �Ÿ� üũ
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
    /// ���� ������ ����
    /// </summary>
    /// <param name="atkData"></param>
    protected virtual void SetAttackData(ref AtkData atkData)
    {
        atkData = new AtkData(_myUnit, _myUnit.UnitStat.Return_Attack(), _myUnit.UnitStat.Return_Knockback(), 0, _myUnitData.dir, _myUnit.ETeam == TeamType.MyTeam, 0, AtkType.Normal, _myUnit.SkinData.effectType, originValue);
    }

    /// <summary>
    /// ������ �� ���� �� ���� ����Ѵ�
    /// </summary>
    /// <returns>True�� ����, �ƴϸ� ������</returns>
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
