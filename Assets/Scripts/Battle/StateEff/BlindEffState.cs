using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class BlindEffState : EffState
{
    private float _blindTime = 0; //����ε� ���ӽð�
    private float _rangeSubtractPercent = 50; //�����Ÿ� �ۼ�Ʈ�� �󸶳� �پ����ΰ�

    public override void Enter()
    {
        SprTrm.GetComponent<SpriteRenderer>().color = Color.green;
        _myUnit.UnitStat.IncreaseRangePercent(-(int)_rangeSubtractPercent);

        base.Enter();
    }
    public override void Update()
    {
        InkTimer();
    }

    public override void Exit()
    {
        _myUnit.UnitStat.IncreaseRangePercent((int)_rangeSubtractPercent);
        SprTrm.GetComponent<SpriteRenderer>().color = Color.red;

        base.Exit();
    }

    public override void SetEffValue(params float[] value)
    {
        _blindTime = value[0];
    }

    /// <summary>
    /// ��ũ ȿ�� ���ӽð�
    /// </summary>
    private void InkTimer()
    {
        if (_blindTime > 0)
        {
            _blindTime -= Time.deltaTime;
            return;
        }
        _curEvent = eEvent.EXIT;
    }
}
