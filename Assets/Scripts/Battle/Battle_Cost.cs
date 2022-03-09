using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using TMPro;

public class Battle_Cost : BattleCommand
{
    public int cur_Cost { get; private set; }
    public int max_Cost { get; private set; } = 2;

    public float cost_Speed;
    public float cost_Delay;

    public int grade { get; private set; } = 1;
    public int max_grade { get; private set; } = 5;

    private TextMeshProUGUI cost_CostText;

    public Battle_Cost(BattleManager battleManager, TextMeshProUGUI cost_CostText) : base(battleManager)
    {
        this.cost_CostText = cost_CostText;
        cost_Speed = battleManager.pencilCaseDataSO.costSpeed;
    }

    public void Add_Cost(int addCost)
    {
        cur_Cost += addCost;
        Update_CostText();
    }

    public void Subtract_Cost(int SubtractCost)
    {
        cur_Cost -= SubtractCost;
        Update_CostText();
    }

    public void Set_Cost(int setCost)
    {
        cur_Cost = setCost;
        Update_CostText();
    }

    public void Set_CostSpeed(float speed)
    {
        cost_Speed = speed;
    }
    public void Add_CostSpeed(float speed)
    {
        cost_Speed += speed;
    }
    public void Multiple_CostSpeed(float multiplespeed)
    {
        cost_Speed *= multiplespeed;
    }

    public void Update_Cost()
    {
        if (cur_Cost >= max_Cost)
            return;
        if (cost_Delay > 0)
        {
            cost_Delay -= cost_Speed * Time.deltaTime;
            return;
        }
        Add_Cost(1);
        Update_CostText();
        cost_Delay = 100;
    }

    public void Update_CostText()
    {
        cost_CostText.text = cur_Cost.ToString();
    }

    public void Run_UpgradeCostGrade()
    {
        if (grade >= max_grade)
            return;
        if (cur_Cost < max_Cost)
            return;

        grade++;
        Subtract_Cost(max_Cost);
        Add_CostSpeed(0.25f);
        max_Cost += 2;
    
    }

}
