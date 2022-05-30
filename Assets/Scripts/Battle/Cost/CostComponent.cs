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
        //������Ƽ
        public int CurrentCost { get; private set; } = 0;
        public int MaxCost { get; private set; } = 2;
        public int MaxGrade { get; private set; } = 5;
        public int CurrentGrade { get; private set; } = 1;

        //����
        public float _costSpeed = 200;
        public float _costDelay;

        //�ν����� ���� ����
        [SerializeField]
        private TextMeshProUGUI _costText = null;
        [SerializeField]
        private Image _costImage = null;
        [SerializeField]
        private TextAnimationComponent _textAnimationComponent;

        /// <summary>
        /// �ʱ�ȭ
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
        /// ���� �ڽ�Ʈ ����
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
        /// ���� �ڽ�Ʈ ����
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
        /// ���� �ڽ�Ʈ ����
        /// </summary>
        /// <param name="setCost"></param>
        public void SetCost(int setCost)
        {
            CurrentCost = setCost;
            UpdateCostText();
        }

        /// <summary>
        /// �ڽ�Ʈ ���� �ӵ� ����
        /// </summary>
        /// <param name="speed"></param>
        public void SetCostSpeed(float speed)
        {
            _costSpeed = speed;
        }

        /// <summary>
        /// �ڽ�Ʈ ���� �ӵ��� ����
        /// </summary>
        /// <param name="speed"></param>
        public void AddCostSpeed(float speed)
        {
            _costSpeed += speed;
        }

        /// <summary>
        /// �ڽ�Ʈ ���� �ӵ��� ������
        /// </summary>
        /// <param name="multiplespeed"></param>
        public void MultipleCostSpeed(float multiplespeed)
        {
            _costSpeed *= multiplespeed;
        }

        /// <summary>
        /// �ڽ�Ʈ ������Ʈ
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
        /// �ڽ�Ʈ �ؽ�Ʈ ������Ʈ
        /// </summary>
        private void UpdateCostText()
        {
            _costText.text = string.Format("{0} / {1}", CurrentCost.ToString(), MaxCost.ToString());
        }

        /// <summary>
        /// �ڽ�Ʈ �ܰ踦 ���׷��̵�
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
        /// Ŭ���ϸ� �ڽ�Ʈ �ܰ� ����
        /// </summary>
        private void OnUpgradeCostGrade()
        {
            UpgradeCostGrade();
        }

    }
}
