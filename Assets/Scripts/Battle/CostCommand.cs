using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;
using TMPro;

namespace Battle
{
    [System.Serializable]
    public class CostCommand : BattleCommand
    {
        public int CurrentCost { get; private set; } = 0;
        public int MaxCost { get; private set; } = 2;
        public int MaxGrade { get; private set; } = 5;
        public int CurrentGrade { get; private set; } = 1;

        public float _costSpeed = 200;
        public float _costDelay;

        [SerializeField]
        private TextMeshProUGUI _costText;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="cost_CostText"></param>
        public void SetInitialization(BattleManager battleManager, PencilCaseData pencilCasePlayerData)
        {
            this._battleManager = battleManager;
            battleManager.AddUpdateAction(UpdateCost);
            SetCostSpeed(pencilCasePlayerData.costSpeed);
        }


        /// <summary>
        /// 현재 코스트 증가
        /// </summary>
        /// <param name="addCost"></param>
        public void AddCost(int addCost)
        {
            CurrentCost += addCost;
            UpdateCostText();
        }

        /// <summary>
        /// 현재 코스트 감소
        /// </summary>
        /// <param name="subtractCost"></param>
        public void SubtractCost(int subtractCost)
        {
            CurrentCost -= subtractCost;
            UpdateCostText();
        }

        /// <summary>
        /// 현재 코스트 설정
        /// </summary>
        /// <param name="setCost"></param>
        public void SetCost(int setCost)
        {
            CurrentCost = setCost;
            UpdateCostText();
        }

        /// <summary>
        /// 코스트 증가 속도 설정
        /// </summary>
        /// <param name="speed"></param>
        public void SetCostSpeed(float speed)
        {
            _costSpeed = speed;
        }

        /// <summary>
        /// 코스트 증가 속도를 증가
        /// </summary>
        /// <param name="speed"></param>
        public void AddCostSpeed(float speed)
        {
            _costSpeed += speed;
        }

        /// <summary>
        /// 코스트 장가 속도를 곱해줌
        /// </summary>
        /// <param name="multiplespeed"></param>
        public void MultipleCostSpeed(float multiplespeed)
        {
            _costSpeed *= multiplespeed;
        }

        /// <summary>
        /// 코스트 업데이트
        /// </summary>
        public void UpdateCost()
        {
            if (CurrentCost >= MaxCost)
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

        /// <summary>
        /// 코스트 텍스트 업데이트
        /// </summary>
        public void UpdateCostText()
        {
            _costText.text = string.Format("{0} / {1}", CurrentCost.ToString(), MaxCost.ToString());
        }

        /// <summary>
        /// 코스트 단계를 업그레이드
        /// </summary>
        public void UpgradeCostGrade()
        {
            if (CurrentGrade >= MaxGrade)
            {
                return;
            }
            if (CurrentCost < MaxCost)
            {
                return;
            }

            CurrentGrade++;
            SubtractCost(MaxCost);
            AddCostSpeed(0.25f);
            MaxCost += 2;
            UpdateCostText();


        }

    }
}
