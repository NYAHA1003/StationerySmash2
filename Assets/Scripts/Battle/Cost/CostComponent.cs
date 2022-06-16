using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Utill.Data;
using Utill.Tool;
using TMPro;
using Main.Event;
using DG.Tweening;

namespace Battle
{
    [System.Serializable]
    public class CostComponent : BattleComponent
    {
        //������Ƽ
        public int MaxCost { get; private set; } = 10; //�ִ� �ڽ�Ʈ
        public float AddCostSpeedValue { get; private set; } = 35f; //�ڽ�Ʈ ���׷��̵�� �����ϴ� �ڽ�Ʈ ���ǵ尪
        public int CurrentCost { get; private set; } = 0; //���� �ڽ�Ʈ
        public int MaxGrade { get; private set; } = 5; //�ִ�ܰ�
        public int CurrentGrade { get; private set; } = 1; //���� �ܰ�

        //����
        public float _costSpeed = 200;
        public float _costDelay;
        private bool _isActiveButton; //��ư�� ������ ��

        //�ν����� ���� ����
        [SerializeField]
        private TextMeshProUGUI _costText = null;
        [SerializeField]
        private Image _costImage = null;
        [SerializeField]
        private RectTransform _costUpButtonRect = null;
        [SerializeField]
        private Image _disableImage = null;
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
            EventManager.Instance.StartListening(EventsType.CostUp, OnUpgradeCostGrade);
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
            _textAnimationComponent.SetText($"+{addCost}", animationPos, 0.5f, TextAnimationComponent.AnimationDirType.Right, TextAnimationComponent.AnimationType.BigToSmall);
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
            _textAnimationComponent.SetText($"-{subtractCost}", animationPos, 0.5f, TextAnimationComponent.AnimationDirType.Right, TextAnimationComponent.AnimationType.BigToSmall);
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
                SetDisableImage(false);
                if(!_isActiveButton)
				{
                    _isActiveButton = true;
                    _costUpButtonRect.DOShakeAnchorPos(0.3f);
				}
                return;
			}
            else
            {
                _isActiveButton = false;
                SetDisableImage(true);
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
        /// �ڽ�Ʈ ���� ��ư�� ��Ȱ��ȭ �̹����� ų�� ����
        /// </summary>
        private void SetDisableImage(bool setActive)
        {
            _disableImage.gameObject.SetActive(setActive);
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

            SubtractCost(MaxCost);
            MaxCost += CurrentGrade * 5; //�ִ� �ڽ�Ʈ ����
            CurrentGrade++;
            AddCostSpeed(AddCostSpeedValue); //���׷��̵�� �������� �ӵ���
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
