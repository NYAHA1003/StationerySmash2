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
    /// ���� ���������Ʈ �����ϼ�, ���� ��� �ʷ� �� ��ȯ
    /// </summary>
    public override void RunPencilCaseAbility()
    {
        switch (_trafficType)
        {
            case TrafficType.Red:
                //������ ��ȯ
                _battleManager.CommandUnit.SummonUnit(_redCarData, _battleManager.CommandPencilCase.PlayerPencilCase.transform.position, 1, Utill.TeamType.MyTeam);
                break;
            case TrafficType.Yellow:
                //����� ����
                _battleManager.CommandUnit.SummonUnit(_yellowCarData, _battleManager.CommandPencilCase.PlayerPencilCase.transform.position, 1, Utill.TeamType.MyTeam);
                break;
            case TrafficType.Green:
                _battleManager.CommandUnit.SummonUnit(_greenCarData, _battleManager.CommandPencilCase.PlayerPencilCase.transform.position, 1, Utill.TeamType.MyTeam);
                //�ʷ��� ��ȯ
                break;
        }

        //���� ��ȣ������ �Ѿ
        _trafficType++;
        if (_trafficType >= TrafficType.Green)
        {
            _trafficType = TrafficType.Red;
        }
    }


    /// <summary>
    /// ������ ������ ����
    /// </summary>
    private void SetRedCar()
    {
        _redCarData = new DataBase
        {
            cardType = Utill.CardType.SummonUnit,
            card_Cost = 0,
            card_Name = "������",
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
    /// ����� ������ ����
    /// </summary>
    private void SetYellowCar()
    {
        _yellowCarData = new DataBase
        {
            cardType = Utill.CardType.SummonUnit,
            card_Cost = 0,
            card_Name = "�����",
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
    /// �ʷ��� ������ ����
    /// </summary>
    private void SetGreenCar()
    {
        _greenCarData = new DataBase
        {
            cardType = Utill.CardType.SummonUnit,
            card_Cost = 0,
            card_Name = "�����",
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
