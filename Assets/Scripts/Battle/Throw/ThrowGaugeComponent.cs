using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;

namespace Battle
{
    public class ThrowGaugeComponent : BattleComponent
    {
        //���� ����
        private RectTransform _throwBarFrame;
        private RectTransform _throwGaugeBar;
        private ThrowComponent _throwComponent;
        private UnitComponent _unitCommand = null;
        private GameObject _throwStackPanel;
        private GameObject _throwStackObj;
        private List<Transform> _throwStackObjList = new List<Transform>();

        //�Ӽ�
        private float _throwGauge = 0f;
        private int _throwStack = 0;
        private int _throwMaxStack = 0;
        private float _throwGaugeSpeed = 0f;

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        /// <param name="throwComponent"></param>
        /// <param name="unitComponent"></param>
        /// <param name="throwBarFrame"></param>
        /// <param name="throwGaugeBar"></param>
        /// <param name="pencilCaseDataSO"></param>
        public void SetInitialization(ThrowComponent throwComponent, UnitComponent unitComponent, RectTransform throwBarFrame, RectTransform throwGaugeBar , GameObject throwStackPanel, GameObject throwStackObj, PencilCaseData pencilCaseData)
        {
            this._throwComponent = throwComponent;
            this._unitCommand = unitComponent;
            this._throwBarFrame = throwBarFrame;
            this._throwGaugeBar = throwGaugeBar;
            this._throwGaugeSpeed = pencilCaseData._throwGaugeSpeed;
            this._throwStackPanel = throwStackPanel;
            this._throwStackObj = throwStackObj;
            if(BattleManager.IsHardMode)
			{
                this._throwMaxStack = pencilCaseData._maxThrowStack - 1;
            }
            else
			{
                this._throwMaxStack = pencilCaseData._maxThrowStack;
			}

            StackInitialize();
            StackSetting();
        }

        /// <summary>
        /// ���� ������ ������ �����´�
        /// </summary>
        /// <returns></returns>
        public int GetThrowStack()
		{
            return _throwStack;
		}

        /// <summary>
        /// ������ ������ ����
        /// </summary>
        /// <param name="add"></param>
        public void IncreaseThrowStack(int stack)
        {
            _throwStack += stack;
            StackSetting();
        }

        /// <summary>
        /// ������Ʈ ������
        /// </summary>
        public void UpdateThrowGauge()
        {
            if (_throwStack == _throwMaxStack)
			{
                return;
			}

            if (_throwGauge <= 200f)
            {
                IncreaseThrowGauge(Time.deltaTime * _throwGaugeSpeed);
                Vector2 rectSize = _throwGaugeBar.sizeDelta;
                rectSize.x = _throwBarFrame.rect.width * (_throwGauge / 200f);
                _throwGaugeBar.sizeDelta = rectSize;
            }
            else
			{
                _throwGauge = 0;
                _throwStack++;
                StackSetting();

            }
        }

        /// <summary>
        /// ������ ����
        /// </summary>
        /// <param name="add"></param>
        private void IncreaseThrowGauge(float add)
		{
            _throwGauge += add;

        }

        /// <summary>
        /// ���û���
        /// </summary>
        private void StackInitialize()
		{
            for(int i = 0; i < _throwMaxStack; ++i)
			{
                Transform obj = GameObject.Instantiate(_throwStackObj, _throwStackPanel.transform).transform;
                _throwStackObjList.Add(obj);
            }
		}

        /// <summary>
        /// ���� ����
        /// </summary>
        private void StackSetting()
		{
            for(int i = 0; i < _throwMaxStack; ++i)
			{
                if(i < _throwStack)
				{
                    _throwStackObjList[i].GetChild(0).gameObject.SetActive(true);
				}
                else
				{
                    _throwStackObjList[i].GetChild(0).gameObject.SetActive(false);
				}
            }
		}


    }

}