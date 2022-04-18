using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class ScratchEffState : EffState
{
    float ScratchDamage = 10; // ���� ���ô� ������
    float currntScratchDamage = 0; // ���� ���õ�����
    float _scratchTime = 1;
    float timer = 0;
    bool isScratch = false;
    // Start is called before the first frame update
    public override void Enter()
    {
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
        currntScratchDamage += ScratchDamage;
        SprTrm.GetComponent<SpriteRenderer>().color = Color.red;
        isScratch = false;
        currntScratchDamage = 0;
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
                _myUnit.UnitStat.SubtractHP(-(int)currntScratchDamage);
                timer = 0;
            }
        }
    }
    /// <summary>
    /// ���� ȿ�� ���ӽð�
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
