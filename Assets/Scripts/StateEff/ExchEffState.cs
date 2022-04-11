using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using DG.Tweening;

public class ExchEffState : EffState
{
    private bool isAddEff = false;
    private float _exchOneTime = 0.0f; // 교환 첫번째 시간
    private float _exchTwoTime = 0.0f; // 교환 두번쨰 시간
    private float attackSubtractPercent = 0; //공격력 퍼센트가 얼마나 커지고 줄어들것인가
    private float moveSpeedSubtractPercent = 0;//이동속도 퍼센트가 얼마나 커지고 줄어들것인가

    public override void Enter()
    {
        SprTrm.GetComponent<SpriteRenderer>().color = Color.green;
        _myUnit.UnitStat.IncreaseAttackPercent((int)attackSubtractPercent);
        _myUnit.UnitStat.IncreaseMoveSpeedPercent((int)moveSpeedSubtractPercent);

        //이펙트 오브젝트 가져오기
        //_effectObj = _battleManager.CommandEffect.SetEffect(EffectType.Stun, new EffData(new Vector2(Trm.position.x, Trm.position.y + 0.1f), stunTime, Trm));

        base.Enter();
    }

    public override void Update()
    {
        if(!ExchOneTimer())
        {
            return;
        }
        if(!isAddEff)
        {
            _myUnit.UnitStat.IncreaseAttackPercent(-(int)attackSubtractPercent * 2);
            _myUnit.UnitStat.IncreaseMoveSpeedPercent(-(int)moveSpeedSubtractPercent * 2);
            isAddEff = true;
        }
        if (!ExchTwoTimer())
        {
            return;
        }
        _curEvent = eEvent.EXIT;
    }

    public override void Exit()
    {
        _myUnit.UnitStat.IncreaseAttackPercent((int)attackSubtractPercent);
        _myUnit.UnitStat.IncreaseMoveSpeedPercent((int)moveSpeedSubtractPercent);
        DeleteEffectObject();
        base.Exit();
    }

    public override void SetEffValue(params float[] value)
    {
        _exchOneTime = value[0];
        _exchTwoTime = value[0];
        attackSubtractPercent = value[1];
        moveSpeedSubtractPercent = value[2];
    }


    /// <summary>
    /// 교환 1단계 유지 시간
    /// </summary>
    private bool ExchOneTimer()
    {
        if (_exchOneTime > 0)
        {
            _exchOneTime -= Time.deltaTime;
            return false;
        }
        return true;
    }

    /// <summary>
    /// 교환 2단계 유지 시간
    /// </summary>
    private bool ExchTwoTimer()
    {
        if (_exchTwoTime > 0)
        {
            _exchTwoTime -= Time.deltaTime;
            return false;
        }
        return true;
    }

}
