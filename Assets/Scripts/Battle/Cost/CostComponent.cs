using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Utill.Data;
using Utill.Tool;
using TMPro;
using Main.Event;

namespace Battle
{
    [System.Serializable]
    public class CostComponent : BattleComponent
    {
        //프로퍼티
        public int CurrentCost { get; private set; } = 0;
        public int MaxCost { get; private set; } = 2;
        public int MaxGrade { get; private set; } = 5;
        public int CurrentGrade { get; private set; } = 1;

        //변수
        public float _costSpeed = 200;
        public float _costDelay;

        //인스펙터 참조 변수
        [SerializeField]
        private TextMeshProUGUI _costText = null;
        [SerializeField]
        private Image _costImage = null;
        [SerializeField]
        private TextAnimationComponent _textAnimationComponent;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="cost_CostText"></param>
        public void SetInitialization(ref System.Action updateAction, PencilCaseData pencilCasePlayerData)
        {
            updateAction += UpdateCost;
            SetCostSpeed(pencilCasePlayerData._costSpeed);
            EventManager.StartListening(EventsType.CostUp, OnUpgradeCostGrade);
        }

        /// <summary>
        /// 현재 코스트 증가
        /// </summary>
        /// <param name="addCost"></param>
        public void AddCost(int addCost)
        {
            CurrentCost += addCost;
            Vector2 animationPos = _costText.rectTransform.localPosition;
            animationPos.x += 250;
            _textAnimationComponent.SetText($"+{addCost}", animationPos, 0.3f, TextAnimationComponent.AnimationDirType.Right);
            UpdateCostText();
        }

        /// <summary>
        /// 현재 코스트 감소
        /// </summary>
        /// <param name="subtractCost"></param>
        public void SubtractCost(int subtractCost)
        {
            CurrentCost -= subtractCost;
            Vector2 animationPos = _costText.rectTransform.localPosition;
            animationPos.x += 250;
            _textAnimationComponent.SetText($"-{subtractCost}", animationPos, 0.3f, TextAnimationComponent.AnimationDirType.Right);
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
        /// 코스트 증가 속도를 곱해줌
        /// </summary>
        /// <param name="multiplespeed"></param>
        public void MultipleCostSpeed(float multiplespeed)
        {
            _costSpeed *= multiplespeed;
        }

        /// <summary>
        /// 코스트 업데이트
        /// </summary>
        private void UpdateCost()
        {
            if (CurrentCost >= MaxCost)
			{
                return;
			}
            if (_costDelay > 0)
            {
                _costDelay -= _costSpeed * Time.deltaTime;
                _costImage.fillAmount = (100 - _costDelay) / 100;
                return;
            }
            AddCost(1);
            UpdateCostText();
            _costDelay = 100;
            _costImage.fillAmount = (100 - _costDelay) / 100;
        }

        /// <summary>
        /// 코스트 텍스트 업데이트
        /// </summary>
        private void UpdateCostText()
        {
            _costText.text = string.Format("{0} / {1}", CurrentCost.ToString(), MaxCost.ToString());
        }

        /// <summary>
        /// 코스트 단계를 업그레이드
        /// </summary>
        private void UpgradeCostGrade()
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

        /// <summary>
        /// 클릭하면 코스트 단계 증가
        /// </summary>
        private void OnUpgradeCostGrade()
        {
            UpgradeCostGrade();
        }

    }
}
