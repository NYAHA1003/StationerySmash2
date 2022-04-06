using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public abstract class AbstractThrowState : AbstractUnitState
{
    Vector2 mousePos = Vector2.zero;

    public void Set_ThrowPos(Vector2 pos)
    {
        mousePos = pos;
    }

    public override void Enter()
    {
        _myUnit.Set_IsDontThrow(true);
        //방향
        Vector2 direction = (Vector2)_myTrm.position - mousePos;
        float dir = Mathf.Atan2(direction.y, direction.x);
        float dirx = Mathf.Atan2(direction.y, -direction.x);

        if (dir < 0)
        {
            _stateManager.Set_Wait(0.5f);
            return;
        }

        //초기 벡터
        float force = Mathf.Clamp(Vector2.Distance(_myTrm.position, mousePos), 0, 1) * 4 * (100.0f / _myUnit.UnitStat.Return_Weight());

        //최고점
        float height = Parabola.Caculated_Height(force, dirx);
        //수평 도달 거리
        float width = Parabola.Caculated_Width(force, dirx);
        //수평 도달 시간
        float time = Parabola.Caculated_Time(force, dir, 3);

        _mySprTrm.DOKill();
        _myTrm.DOJump(new Vector3(_myTrm.position.x - width, 0, _myTrm.position.z), height, 1, time).OnComplete(() =>
        {
            _stateManager.Set_Wait(0.5f);
        }).SetEase(Utill.Parabola.Return_ParabolaCurve());

        base.Enter();
    }

    public override void Update()
    {
        Check_Wall();
        if (_myUnit.ETeam.Equals(TeamType.MyTeam))
        {
            Check_Collide(_myUnit.BattleManager.CommandUnit._enemyUnitList);
            return;
        }
        if (_myUnit.ETeam.Equals(TeamType.EnemyTeam))
        {
            Check_Collide(_myUnit.BattleManager.CommandUnit._playerUnitList);
            return;
        }
    }
    private void Check_Collide(List<Unit> list)
    {
        Unit targetUnit = null;
        for (int i = 0; i < list.Count; i++)
        {
            targetUnit = list[i];
            if (targetUnit._isInvincibility)
            {
                continue;
            }
            float distance = Utill.Collider.FindDistanceBetweenSegments(_myUnit.CollideData.SetPos(_myTrm.position), targetUnit.CollideData.SetPos(targetUnit.transform.position));
            if (distance < 0.2f)
            {
                Run_ThrowAttack(targetUnit);
            }
        }
    }

    protected virtual void Run_ThrowAttack(Unit targetUnit)
    {
        float dir = Vector2.Angle((Vector2)_myTrm.position, (Vector2)targetUnit.transform.position);
        float extraKnockBack = (targetUnit.UnitStat.Weight - _myUnit.UnitStat.Return_Weight() * (float)targetUnit.UnitStat.Hp / targetUnit.UnitStat.MaxHp) * 0.025f;
        AtkData atkData = new AtkData(_myUnit, 0, 0, 0, 0, true, 0, AtkType.Normal);
        AtkData atkDataMy = new AtkData(_myUnit, 0, 0, 0, 0, true, 0, AtkType.Normal);
        atkData.Reset_Damage(100 + (_myUnit.UnitStat.Weight > targetUnit.UnitStat.Weight ? (Mathf.RoundToInt((float)_myUnit.UnitStat.Weight - targetUnit.UnitStat.Weight) / 2) : Mathf.RoundToInt((float)(targetUnit.UnitStat.Weight - _myUnit.UnitStat.Weight) / 5)));

        //무게가 더 클 경우
        if (_myUnit.UnitStat.Weight > targetUnit.UnitStat.Weight)
        {
            atkData.Reset_Kncockback(10, extraKnockBack, dir, false);
            atkData.Reset_Type(AtkType.Stun);
            atkData.Reset_Value(1);
            targetUnit.Run_Damaged(atkData);
            return;
        }

        //무게가 더 작을 경우
        if (_myUnit.UnitStat.Weight < targetUnit.UnitStat.Weight)
        {
            atkData.Reset_Kncockback(0, 0, 0, false);
            atkData.Reset_Type(AtkType.Normal);
            atkData.Reset_Value(null);
            targetUnit.Run_Damaged(atkData);

            atkDataMy.Reset_Kncockback(20, 0, dir, true);
            atkDataMy.Reset_Type(AtkType.Stun);
            atkDataMy.Reset_Value(1);
            atkDataMy.Reset_Damage(0);
            _myUnit.Run_Damaged(atkDataMy);
            return;
        }

        //무게가 같을 경우
        if (_myUnit.UnitStat.Weight.Equals(targetUnit.UnitStat.Weight))
        {
            atkData.Reset_Kncockback(10, extraKnockBack, dir, false);
            atkData.Reset_Type(AtkType.Stun);
            atkData.Reset_Value(1);
            targetUnit.Run_Damaged(atkData);


            atkDataMy.Reset_Kncockback(20, 0, dir, true);
            atkDataMy.Reset_Type(AtkType.Normal);
            atkDataMy.Reset_Value(1);
            atkDataMy.Reset_Damage(0);
            _myUnit.Run_Damaged(atkDataMy);

            return;
        }
    }
}
