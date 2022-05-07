using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Data
{
    public enum CardType
    {
        Execute,
        SummonUnit,
        SummonTrap,
        Installation,
    }

    public enum CardNamingType
    {
        None = 0,
        Pencil = 100,
        Sharp = 200,
        PencilCase = 10000,
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
        MechaPencil,
        Eraser,
        Scissors,
        Glue,
        Ruller,
        Cutterknife,
        Postit,
        MechaPencilLead,
        Pen,



        RedCar = 1000,
        YellowCar = 1001,
        GreenCar = 1002,
        EraserPiece,
        SharpSim,
        Apostit,
    }
}
