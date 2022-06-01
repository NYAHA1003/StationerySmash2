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
        Eraser = 300,
        Scissors = 400,
        Glue = 500,
        Ruler = 600,
        CutterKnife = 700,
        PostIt = 800,
        MechaPencilLead = 900,
        Pen = 1000,
        EraserPiece = 1100,
        PostItPiece = 1200,
        PencilCase = 10000,
        SharpSim = 11000,
    }

    public enum StrategyType
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
