using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public class UnitMoveState : UnitState
{
    public override void Enter()
    {
        _myUnit.Set_IsDontThrow(false);
        _curState = eState.MOVE;
        _curEvent = eEvent.ENTER;
        Animation();
        base.Enter();
    }

    public override void Update()
    {
        //¿ì¸® ÆÀ
        switch (_myUnit.ETeam)
        {
            case TeamType.Null:
                break;
            case TeamType.MyTeam:
                Move_MyTeam();
                Check_Range(_myUnit.BattleManager.CommandUnit._enemyUnitList);
                return;
            case TeamType.EnemyTeam:
                Move_EnemyTeam();
                Check_Range(_myUnit.BattleManager.CommandUnit._playerUnitList);
                return;
        }
    }

    private void Move_MyTeam()
    {
        if (_myTrm.transform.position.x < _stateManager.GetStageData().max_Range - 0.1f)
        {
            _myTrm.Translate(Vector2.right * _myUnit.UnitStat.Return_MoveSpeed() * Time.deltaTime);
        }
    }

    private void Move_EnemyTeam()
    {
        if (_myTrm.transform.position.x > -_stateManager.GetStageData().max_Range + 0.1f)
        {
            _myTrm.Translate(Vector2.left * _myUnit.UnitStat.Return_MoveSpeed() * Time.deltaTime);
        }
    }

    private void Check_Range(List<Unit> list)
    {
        float targetRange = float.MaxValue;
        Unit targetUnit = null;
        for (int i = 0; i < list.Count; i++)
        {
            if (Vector2.Distance(_myTrm.position, list[i].transform.position) < targetRange)
            {
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
        }

        if (targetUnit != null)
        {
            if (Vector2.Distance(_myTrm.position, targetUnit.transform.position) < _myUnit.UnitStat.Return_Range())
            {
                _stateManager.Set_Attack(targetUnit);
            }
        }

    }

    public override void Animation(params float[] value)
    {
        _mySprTrm.DOKill();
        float rotate = _myUnit.ETeam.Equals(TeamType.MyTeam) ? 30 : -30;
        _mySprTrm.eulerAngles = new Vector3(0, 0, 0);
        _mySprTrm.DORotate(new Vector3(0, 0, rotate), 0.3f).SetLoops(-1, LoopType.Yoyo);
    }
}
