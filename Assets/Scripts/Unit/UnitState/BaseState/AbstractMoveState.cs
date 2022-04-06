using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public abstract class AbstractMoveState : AbstractUnitState
{
    public override void Enter()
    {
        _myUnit.SetIsDontThrow(false);
        _curState = eState.MOVE;
        _curEvent = eEvent.ENTER;
        
        //이동 애니메이션 시작
        Animation();

        base.Enter();
    }

    public override void Update()
    {
        //팀에 따른 이동 및 사정거리 체크
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
    /// 플레이어 팀의 유닛 이동
    /// </summary>
    protected virtual void MoveMyTeam()
    {
        if (_myTrm.transform.position.x < _stateManager.GetStageData().max_Range - 0.1f)
        {
            _myTrm.Translate(Vector2.right * _myUnit.UnitStat.Return_MoveSpeed() * Time.deltaTime);
        }
    }

    /// <summary>
    /// 적 팀의 유닛 이동
    /// </summary>
    protected virtual void MoveEnemyTeam()
    {
        if (_myTrm.transform.position.x > -_stateManager.GetStageData().max_Range + 0.1f)
        {
            _myTrm.Translate(Vector2.left * _myUnit.UnitStat.Return_MoveSpeed() * Time.deltaTime);
        }
    }

    /// <summary>
    /// 상대 유닛이 사정거리에 있는지 체크
    /// </summary>
    /// <param name="list"></param>
    protected virtual void CheckRange(List<Unit> list)
    {
        float targetRange = float.MaxValue;
        Unit targetUnit = null;
        for (int i = 0; i < list.Count; i++)
        {
            if (Vector2.Distance(_myTrm.position, list[i].transform.position) >= targetRange)
            {
                continue;
            }
            if (_myUnit.ETeam.Equals(TeamType.MyTeam) && _myTrm.position.x > list[i].transform.position.x)
            {
                continue;
            }
            if (!_myUnit.ETeam.Equals(TeamType.MyTeam) && _myTrm.position.x < list[i].transform.position.x)
            {
                continue;
            }
            if (list[i].transform.position.y > _myTrm.transform.position.y)
            {
                continue;
            }
            if (list[i]._isInvincibility)
            {
                continue;
            }

            targetUnit = list[i];
            targetRange = Vector2.Distance(_myTrm.position, targetUnit.transform.position);
        }

        if (targetUnit != null)
        {
            if (Vector2.Distance(_myTrm.position, targetUnit.transform.position) < _myUnit.UnitStat.Return_Range())
            {
                //사정거리에 상대가 있으면 공격
                _stateManager.Set_Attack(targetUnit);
            }
        }

    }
}
