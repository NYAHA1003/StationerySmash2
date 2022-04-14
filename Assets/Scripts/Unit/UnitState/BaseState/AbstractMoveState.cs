using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public abstract class AbstractMoveState : AbstractUnitState
{
    public override void Enter()
    {
        _curState = eState.MOVE;
        _curEvent = eEvent.ENTER;

        _myUnit.SetIsDontThrow(false);

        //��ƼĿ ���
        _myUnit.UnitSticker.RunStickerAbility(_curState);

        //�̵� �ִϸ��̼� ����
        Animation();

        base.Enter();
    }

    public override void Update()
    {
        //���� ���� �̵� �� �����Ÿ� üũ
        switch (_myUnit.ETeam)
        {
            case TeamType.Null:
                break;
            case TeamType.MyTeam:
                MoveMyTeam();
                CheckRange(_myUnit.BattleManager.CommandUnit._enemyUnitList);
                return;
            case TeamType.EnemyTeam:
                MoveEnemyTeam();
                CheckRange(_myUnit.BattleManager.CommandUnit._playerUnitList);
                return;
        }
    }

    public override void Animation(params float[] value)
    {
        ResetAnimation();
        float rotate = _myUnit.ETeam.Equals(TeamType.MyTeam) ? 30 : -30;
        _mySprTrm.eulerAngles = new Vector3(0, 0, 0);
        _mySprTrm.DORotate(new Vector3(0, 0, rotate), 0.3f).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// �÷��̾� ���� ���� �̵�
    /// </summary>
    protected virtual void MoveMyTeam()
    {
        if (_myTrm.transform.position.x < _stateManager.GetStageData().max_Range - 0.1f)
        {
            _myTrm.Translate(Vector2.right * _myUnit.UnitStat.Return_MoveSpeed() * Time.deltaTime);
        }
    }

    /// <summary>
    /// �� ���� ���� �̵�
    /// </summary>
    protected virtual void MoveEnemyTeam()
    {
        if (_myTrm.transform.position.x > -_stateManager.GetStageData().max_Range + 0.1f)
        {
            _myTrm.Translate(Vector2.left * _myUnit.UnitStat.Return_MoveSpeed() * Time.deltaTime);
        }
    }

    /// <summary>
    /// ��� ������ �����Ÿ��� �ִ��� üũ
    /// </summary>
    /// <param name="list"></param>
    protected virtual void CheckRange(List<Unit> list)
    {
        Unit targetUnit = null;
        int firstNum = 0;
        int lastNum = list.Count - 1;
        int currentIndex = 0;
        
        if(list.Count == 0)
        {
            return;
        }

        if (_myUnit.ETeam == TeamType.MyTeam)
        {
            if (_myTrm.position.x < list[lastNum].transform.position.x)
            {
                targetUnit = list[lastNum];
                currentIndex = lastNum;
            }
        }
        else if (_myUnit.ETeam == TeamType.EnemyTeam)
        {
            if (_myTrm.position.x > list[lastNum].transform.position.x)
            {
                targetUnit = list[lastNum];
                currentIndex = lastNum;
            }
        }

        if (targetUnit == null)
        {
            int loopnum = 0;
            while (true)
            {
                if (list.Count == 0)
                {
                    return;
                }

                int find = (lastNum + firstNum) / 2;

                if (_myTrm.position.x == list[find].transform.position.x)
                {
                    targetUnit = list[find];
                    currentIndex = find;
                    break;
                }

                if (_myUnit.ETeam == TeamType.MyTeam)
                {
                    if (_myTrm.position.x > list[find].transform.position.x)
                    {
                        lastNum = find;
                    }
                    if (_myTrm.position.x < list[find].transform.position.x)
                    {
                        firstNum = find;
                    }
                }
                else if (_myUnit.ETeam == TeamType.EnemyTeam)
                {
                    if (_myTrm.position.x < list[find].transform.position.x)
                    {
                        lastNum = find;
                    }
                    if (_myTrm.position.x > list[find].transform.position.x)
                    {
                        firstNum = find;
                    }
                }

                if (lastNum - firstNum <= 1)
                {
                    targetUnit = list[firstNum];
                    currentIndex = firstNum;
                    break;
                }

                loopnum++;
                if (loopnum > 10000)
                {
                    throw new System.Exception("Infinite Loop");
                }

            }
        }

        while (targetUnit._isInvincibility || targetUnit.transform.position.y > _myTrm.transform.position.y)
        {
            if (list.Count == 0)
            {
                return;
            }

            if (currentIndex - 1 < 0)
            {
                targetUnit = null;
                break;
            }
            targetUnit = list[--currentIndex];
        }

        if (targetUnit != null)
        {
            if (Vector2.Distance(_myTrm.position, targetUnit.transform.position) < _myUnit.UnitStat.Return_Range())
            {
                //�����Ÿ��� ��밡 ������ ����
                CheckTargetUnit(targetUnit);
            }
        }
    }

    /// <summary>
    /// �����Ÿ��ȿ� ���� �ִ�
    /// </summary>
    /// <param name="targetUnit"></param>
    protected virtual void CheckTargetUnit(Unit targetUnit)
    {
        _stateManager.Set_Attack(targetUnit);
    }
}
