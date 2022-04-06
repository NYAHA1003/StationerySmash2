using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficAbility : AbstractPencilCaseAbility
{
    private enum TrafficType
    {
        Red,
        Yellow,
        Green,
    }

    private TrafficType _trafficType = TrafficType.Red;
    private DataBase _redCarData = null;
    private DataBase _yellowCarData = null;
    private DataBase _greenCarData = null;

    public override void SetState(BattleManager battleManager)
    {
        base.SetState(battleManager);
        SetRedCar();
        SetYellowCar();
        SetGreenCar();
    }

    /// <summary>
    /// 구글 스프레드시트 참고하셈, 빨강 노랑 초록 차 소환
    /// </summary>
    public override void RunPencilCaseAbility()
    {
        switch (_trafficType)
        {
            case TrafficType.Red:
                //빨간차 소환
                _battleManager.CommandUnit.SummonUnit(_redCarData, _battleManager.CommandPencilCase.PlayerPencilCase.transform.position, 1, Utill.TeamType.MyTeam);
                break;
            case TrafficType.Yellow:
                //노란차 소한
                _battleManager.CommandUnit.SummonUnit(_yellowCarData, _battleManager.CommandPencilCase.PlayerPencilCase.transform.position, 1, Utill.TeamType.MyTeam);
                break;
            case TrafficType.Green:
                _battleManager.CommandUnit.SummonUnit(_greenCarData, _battleManager.CommandPencilCase.PlayerPencilCase.transform.position, 1, Utill.TeamType.MyTeam);
                //초록차 소환
                break;
        }

        //다음 신호등으로 넘어감
        _trafficType++;
        if (_trafficType >= TrafficType.Green)
        {
            _trafficType = TrafficType.Red;
        }
    }


    /// <summary>
    /// 빨간차 데이터 설정
    /// </summary>
    private void SetRedCar()
    {
        _redCarData = new DataBase
        {
            cardType = Utill.CardType.SummonUnit,
            card_Cost = 0,
            card_Name = "빨간차",
            //card_Sprite =;
        };
        _redCarData.unitData = new UnitData
        {
            unitType = Utill.UnitType.RedCar,
            accuracy = 100,
            attackSpeed = 1,
            range = 0.1f,
            atkType = Utill.AtkType.Normal,
            damage = 20,
            moveSpeed = 0.5f,
            unit_Hp = 100,
            knockback = 10,
            unit_Weight = 100,
            dir = 45,
        };
        _redCarData.unitData.colideData = new Utill.CollideData()
        {
            originpoints = new Vector2[4]
            {
                new Vector2(-0.01f,0.01f),
                new Vector2(0.01f,0.01f),
                new Vector2(-0.01f,-0.01f),
                new Vector2(0.01f,-0.01f),
            }
        };
    }

    /// <summary>
    /// 노란차 데이터 설정
    /// </summary>
    private void SetYellowCar()
    {
        _yellowCarData = new DataBase
        {
            cardType = Utill.CardType.SummonUnit,
            card_Cost = 0,
            card_Name = "노란차",
            //card_Sprite =;
        };
        _yellowCarData.unitData = new UnitData
        {
            unitType = Utill.UnitType.YellowCar,
            accuracy = 100,
            attackSpeed = 1,
            range = 0.1f,
            atkType = Utill.AtkType.Normal,
            damage = 20,
            moveSpeed = 0.5f,
            unit_Hp = 100,
            knockback = 10,
            unit_Weight = 100,
            dir = 45,
        };
        _yellowCarData.unitData.colideData = new Utill.CollideData()
        {
            originpoints = new Vector2[4]
            {
                new Vector2(-0.01f,0.01f),
                new Vector2(0.01f,0.01f),
                new Vector2(-0.01f,-0.01f),
                new Vector2(0.01f,-0.01f),
            }
        };
    }

    /// <summary>
    /// 초록차 데이터 설정
    /// </summary>
    private void SetGreenCar()
    {
        _greenCarData = new DataBase
        {
            cardType = Utill.CardType.SummonUnit,
            card_Cost = 0,
            card_Name = "노란차",
            //card_Sprite =;
        };
        _greenCarData.unitData = new UnitData
        {
            unitType = Utill.UnitType.YellowCar,
            accuracy = 100,
            attackSpeed = 1,
            range = 0.1f,
            atkType = Utill.AtkType.Normal,
            damage = 20,
            moveSpeed = 0.5f,
            unit_Hp = 100,
            knockback = 10,
            unit_Weight = 100,
            dir = 45,
        };
        _greenCarData.unitData.colideData = new Utill.CollideData()
        {
            originpoints = new Vector2[4]
            {
                new Vector2(-0.01f,0.01f),
                new Vector2(0.01f,0.01f),
                new Vector2(-0.01f,-0.01f),
                new Vector2(0.01f,-0.01f),
            }
        };
    }

}
