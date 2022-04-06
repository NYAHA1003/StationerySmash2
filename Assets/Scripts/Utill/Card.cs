using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill
{
    public enum CardType
    {
        Execute,
        SummonUnit,
        SummonTrap,
        Installation,
    }

    public enum StarategyType
    {
        None,

        //Execute,
        CostUp,

        //SummonTrap,


        //Installation,
        InstallCandy,
        InstallSlowdown,
        InstallRage,
    }
    public enum TeamType
    {
        Null,
        MyTeam,
        EnemyTeam,
    }
    public enum UnitType
    {
        None,
        PencilCase,
        Pencil,
        Eraser,
        Sharp,
        BallPen,
        
        RedCar = 1000,
        YellowCar = 1001,
        GreenCar = 1002,
    }
}
