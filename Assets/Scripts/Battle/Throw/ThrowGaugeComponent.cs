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

        //�Ӽ�
        private float _throwGauge = 0f;
        private float _throwGaugeSpeed = 0f;

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        /// <param name="throwComponent"></param>
        /// <param name="unitComponent"></param>
        /// <param name="throwBarFrame"></param>
        /// <param name="throwGaugeBar"></param>
        /// <param name="pencilCaseDataSO"></param>
        public void SetInitialization(ThrowComponent throwComponent, UnitComponent unitComponent, RectTransform throwBarFrame, RectTransform throwGaugeBar , PencilCaseData pencilCaseData)
        {
            this._throwComponent = throwComponent;
            this._unitCommand = unitComponent;
            this._throwBarFrame = throwBarFrame;
            this._throwGaugeBar = throwGaugeBar;
            this._throwGaugeSpeed = pencilCaseData._throwGaugeSpeed;
        }

        /// <summary>
        /// ������ �������� �����´�
        /// </summary>
        /// <returns></returns>
        public float GetThrowGauge()
        {
            return _throwGauge;
        }

        /// <summary>
        /// ������ ������ ����
        /// </summary>
        /// <param name="add"></param>
        public void IncreaseThrowGauge(float add)
        {
            _throwGauge += add;
            if (_throwGauge < 0)
            {
                _throwGauge = 0;
            }
            else if (_throwGauge > 200)
            {
                _throwGauge = 200;
            }
        }

        /// <summary>
        /// ������Ʈ ������
        /// </summary>
        public void UpdateThrowGauge()
        {
            if (_throwGauge <= 200f)
            {
                IncreaseThrowGauge(Time.deltaTime * _throwGaugeSpeed);
                Vector2 rectSize = _throwGaugeBar.sizeDelta;
                rectSize.x = _throwBarFrame.rect.width * (_throwGauge / 200f);
                _throwGaugeBar.sizeDelta = rectSize;
                CheckCanThrow();
            }
        }

        /// <summary>
        /// ������ ������ ���ֵ��� �ð��� ȿ���� �����Ѵ�.
        /// </summary>
        private void CheckCanThrow()
        {
            int count = _unitCommand._playerUnitList.Count;
            for (int i = 1; i < count; i++)
            {
                Unit unit = _unitCommand._playerUnitList[i];
                if(unit.UnitStat.Return_Weight() < _throwGauge && unit.CheckCanThrow())
				{
                    unit.SetThrowImageActive(true);

                }
                else
                {
                    unit.SetThrowImageActive(false);
                }
			}
		}
    }

}