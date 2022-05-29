using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utill.Data;
using Utill.Tool;
using Battle.Starategy;


[System.Serializable]
public class StrategyData
{
    public StrategyType _starategyType;
    public float[] _starategyablityData = new float[0];

    private AbstractStarategy _starategyState;

    /// <summary>
    /// 전략 클래스를 반환
    /// </summary>
    /// <returns></returns>
    public AbstractStarategy ReturnState()
	{
        return _starategyState;

    }

    public void Set_State(params float[] values)
    {

        switch (_starategyType)
        {
            case StrategyType.None:
                break;
            case StrategyType.CostUp:
                _starategyState = new Starategy_CostUp();
                break;
            case StrategyType.InstallCandy:
                _starategyState = new Starategy_Candy();
                break;
            case StrategyType.InstallSlowdown:
                _starategyState = new Starategy_Slowdown();
                break;
            case StrategyType.InstallRage:
                _starategyState = new Starategy_Rage();
                break;
        }
        //논이 아닌 경우 셋 밸류를 해준다.
        if (_starategyType != StrategyType.None)
        {
            _starategyState.SetValuse(_starategyablityData);
        }
    }
}
