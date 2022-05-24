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
    public StrategyType starategyType;
    public AbstractStarategy starategy_State;
    public float[] starategyablityData = new float[0];

    public void Set_State(params float[] values)
    {

        switch (starategyType)
        {
            case StrategyType.None:
                break;
            case StrategyType.CostUp:
                starategy_State = new Starategy_CostUp();
                break;
            case StrategyType.InstallCandy:
                starategy_State = new Starategy_Candy();
                break;
            case StrategyType.InstallSlowdown:
                starategy_State = new Starategy_Slowdown();
                break;
            case StrategyType.InstallRage:
                starategy_State = new Starategy_Rage();
                break;
        }
        //논이 아닌 경우 셋 밸류를 해준다.
        if (starategyType != StrategyType.None)
        {
            starategy_State.SetValuse(starategyablityData);
        }
    }
}
