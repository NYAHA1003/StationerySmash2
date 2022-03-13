using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class Strategy_Unit : Unit
{
    public Starategy_State starategy_State;


    public override void Add_StatusEffect(AtkType atkType, params float[] value)
    {
        throw new System.NotImplementedException();
    }

    public override void Run_Damaged(AtkData atkData)
    {
        throw new System.NotImplementedException();
    }


}
