using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public abstract class AbstractDieState : AbstractUnitState
{
    public override void Enter()
    {
        _myUnit.UnitStateEff.DeleteEffStetes();
        _myUnit.Set_IsDontThrow(true);
        _myUnit.UnitSprite.ShowCanvas(false);

        //µÚÁü
        _myUnit.Set_IsInvincibility(true);
        _myTrm.DOKill();
        _mySprTrm.DOKill();

        DieType dietype = Utill.Die.Return_RandomDieType();
        switch (dietype)
        {
            case DieType.StarKo:
                Animation_StarKO();
                break;
            case DieType.ScreenKo:
                Animation_ScreenKO();
                break;
            case DieType.OutKo:
                Animation_OutKO();
                break;
        }

        base.Enter();
    }

    private void Reset_SprTrm()
    {
        _mySprTrm.localPosition = Vector3.zero;
        _mySprTrm.localScale = Vector3.one;
        _mySprTrm.eulerAngles = Vector3.zero;
        _mySprTrm.DOKill();
    }

    private void Animation_ScreenKO()
    {
        Vector3 diePos = new Vector3(_myTrm.position.x, _myTrm.position.y + 0.4f, 0);
        if (_myUnit.ETeam.Equals(TeamType.MyTeam))
        {
            diePos.x -= Random.Range(0.1f, 0.2f);
        }
        if (_myUnit.ETeam.Equals(TeamType.EnemyTeam))
        {
            diePos.x += Random.Range(0.1f, 0.2f);
        }
        _mySprTrm.DOLocalRotate(new Vector3(0, 0, -360), 0.3f, RotateMode.FastBeyond360).SetLoops(3, LoopType.Incremental);
        _myTrm.DOJump(diePos, 2f, 1, 1f);
        _mySprTrm.DOScale(10, 0.6f).SetDelay(0.3f).SetEase(Utill.Parabola.Return_ScreenKoCurve()).OnComplete(() =>
        {
            _mySprTrm.eulerAngles = new Vector3(0, 0, Random.Range(_mySprTrm.eulerAngles.z - 10, _mySprTrm.eulerAngles.z + 10));
            _mySprTrm.DOShakePosition(0.6f, 0.1f, 30).OnComplete(() =>
            {
                _mySprTrm.DOMoveY(-3, 1).OnComplete(() =>
                {
                    Reset_SprTrm();
                    _curEvent = eEvent.EXIT;
                    _myUnit.Delete_Unit();
                });
            });

        });
    }

    private void Animation_StarKO()
    {
        Vector3 diePos = new Vector3(_myTrm.position.x, 1, 0);
        if (_myUnit.ETeam.Equals(TeamType.MyTeam))
        {
            diePos.x += Random.Range(-2f, -1f);
        }
        if (_myUnit.ETeam.Equals(TeamType.EnemyTeam))
        {
            diePos.x += Random.Range(1f, 2f);
        }

        _mySprTrm.DOLocalRotate(new Vector3(0, 0, -360), 0.5f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
        _mySprTrm.DOScale(0.1f, 1f).SetDelay(1);
        _myTrm.DOJump(diePos, 3, 1, 2f).OnComplete(() =>
        {
            Reset_SprTrm();
            _curEvent = eEvent.EXIT;
            _myUnit.Delete_Unit();
        });
    }

    private void Animation_OutKO()
    {
        Vector3 diePos = new Vector3(_myTrm.position.x, _myTrm.position.y - 2, 0);
        if (_myUnit.ETeam == TeamType.MyTeam)
        {
            diePos.x -= _stateManager.GetStageData().max_Range + 1;
        }
        if (_myUnit.ETeam == TeamType.EnemyTeam)
        {
            diePos.x += _stateManager.GetStageData().max_Range + 1;
        }

        float time = Mathf.Abs(_myTrm.position.x - diePos.x) / 2;
        _mySprTrm.DOScale(3f, time);
        _mySprTrm.DOLocalRotate(new Vector3(0, 0, -360), time, RotateMode.FastBeyond360);
        _myTrm.DOJump(diePos, Random.Range(3, 5), 1, time).OnComplete(() =>
        {
            Reset_SprTrm();
            _curEvent = eEvent.EXIT;
            _myUnit.Delete_Unit();
        });
    }

}
