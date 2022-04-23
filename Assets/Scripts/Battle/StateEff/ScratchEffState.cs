using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class ScratchEffState : EffState
{
    float ScratchDamage = 10; // 흡집 스택당 데미지
    float ScratchStack = 0; // 현재 스택
    float _scratchTime = 5;
    float timer = 0;
    bool isScratch = false;
    // Start is called before the first frame update
    public override void Enter()
    {
        ScratchStack++;
        SprTrm.GetComponent<SpriteRenderer>().color = Color.green;
        isScratch = true;
        ScratchHit();
        base.Enter();
    }
    public override void Update()
    {
        InkTimer();
    }

    public override void Exit()
    {
        SprTrm.GetComponent<SpriteRenderer>().color = Color.red;
        isScratch = false;
        ScratchStack = 0;
        base.Exit();
    }

    public override void SetEffValue(params float[] value)
    {
        if (_scratchTime < value[0])
        {
            _scratchTime = value[0];
        }
    }
    
    public void ScratchHit()
    {
        while(true)
        {
            if(!isScratch)
            {
                break;
            }
            timer += Time.deltaTime;
            if (timer >= _scratchTime)
            {
                _myUnit.UnitStat.SubtractHP(-1 * (int)ScratchDamage * (int)ScratchStack);
                timer = 0;
            }
        }
    }
    /// <summary>
    /// 흠집 효과 지속시간
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
