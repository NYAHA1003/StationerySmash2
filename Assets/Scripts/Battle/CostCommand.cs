using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using TMPro;

namespace Battle
{
    public class CostCommand : BattleCommand
    {
        public int CurCost { get; private set; } = 0;
        public int MaxCost { get; private set; } = 2;

        public float _costSpeed = 200;
        public float _costDelay;

        public int Grade { get; private set; } = 1;
        public int MaxGrade { get; private set; } = 5;

        private TextMeshProUGUI _costText;

        public void SetInitialization(BattleManager battleManager, TextMeshProUGUI cost_CostText)
        {
            SetBattleManager(battleManager);
            this._costText = cost_CostText;
            battleManager.AddAction(UpdateCost);
        }


        public void AddCost(int addCost)
        {
            CurCost += addCost;
            UpdateCostText();
        }

        public void SubtractCost(int subtractCost)
        {
            CurCost -= subtractCost;
            UpdateCostText();
        }

        public void SetCost(int setCost)
        {
            CurCost = setCost;
            UpdateCostText();
        }

        public void SetCostSpeed(float speed)
        {
            _costSpeed = speed;
        }
        public void AddCostSpeed(float speed)
        {
            _costSpeed += speed;
        }
        public void MultipleCostSpeed(float multiplespeed)
        {
            _costSpeed *= multiplespeed;
        }

        public void UpdateCost()
        {
            if (CurCost >= MaxCost)
                return;
            if (_costDelay > 0)
            {
                _costDelay -= _costSpeed * Time.deltaTime;
                return;
            }
            AddCost(1);
            UpdateCostText();
            _costDelay = 100;
        }

        public void UpdateCostText()
        {
            _costText.text = string.Format("{0} / {1}", CurCost.ToString(), MaxCost.ToString());
        }

        public void RunUpgradeCostGrade()
        {
            if (Grade >= MaxGrade)
                return;
            if (CurCost < MaxCost)
                return;

            Grade++;
            SubtractCost(MaxCost);
            AddCostSpeed(0.25f);
            MaxCost += 2;
            UpdateCostText();


        }

    }
}
