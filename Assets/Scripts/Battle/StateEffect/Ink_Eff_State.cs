using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class Ink_Eff_State : Eff_State
{
    private float inkTime = 0;
    private float damageSubtractPercent = 0;
    private float accuracySubtractPercent = 0;

    public Ink_Eff_State() : base()
    {
    }
    public override void Enter()
    {
        mySprTrm.GetComponent<SpriteRenderer>().color = Color.green;
        myUnit.damagePercent -= (int)damageSubtractPercent;
        myUnit.accuracyPercent -= (int)accuracySubtractPercent;

        base.Enter();
    }
    public override void Update()
    {
        if (inkTime > 0)
        {
            inkTime -= Time.deltaTime;
            return;
        }

        curEvent = eEvent.EXIT;
    }

    public override void Exit()
    {
        myUnit.damagePercent += (int)damageSubtractPercent;
        myUnit.accuracyPercent += (int)accuracySubtractPercent;
        mySprTrm.GetComponent<SpriteRenderer>().color = Color.red;

        base.Exit();
    }

    public override void Set_EffValue(params float[] value)
    {
        inkTime = value[0];
        damageSubtractPercent = value[1];
        accuracySubtractPercent = value[2];
    }
}