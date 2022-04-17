using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class ScratchEffState : EffState
{
    float ScratchDamage = 10; // 예시로 해놓음
    float currntScratchDamage = 0;
    float _scratchTime = 0;
    // Start is called before the first frame update
    public override void Enter()
    {
        SprTrm.GetComponent<SpriteRenderer>().color = Color.green;
        _myUnit.UnitStat.SubtractHP(-(int)currntScratchDamage);

        base.Enter();
    }
    public override void Update()
    {
        InkTimer();
        _myUnit.UnitStat.SubtractHP(-(int)currntScratchDamage); // 초마다 발동으로 바꿔야함
    }

    public override void Exit()
    {
        currntScratchDamage += ScratchDamage;
        SprTrm.GetComponent<SpriteRenderer>().color = Color.red;

        base.Exit();
    }

    public override void SetEffValue(params float[] value)
    {
        if (_scratchTime < value[0])
        {
            _scratchTime = value[0];
            //stunTime = stunTime + (stunTime * (((float)_myUnit.UnitStat.MaxHp / (_myUnit.UnitStat.Hp + 70)) - 1));
        }
    }

    /// <summary>
    /// 잉크 효과 지속시간
    /// </summary>
    private void InkTimer()
    {
        if (_scratchTime > 0)
        {
            _scratchTime -= Time.deltaTime;
            return;
        }
        _curEvent = eEvent.EXIT;
    }
}
