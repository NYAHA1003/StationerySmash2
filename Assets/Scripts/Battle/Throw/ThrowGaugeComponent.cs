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
        //참조 변수
        private RectTransform _throwBarFrame;
        private RectTransform _throwGaugeBar;
        private ThrowComponent _throwComponent;
        private UnitComponent _unitCommand = null;
        private Material _defaultShader;
        private Material _throwShader;

        //속성
        private float _throwGauge = 0f;
        private float _throwGaugeSpeed = 0f;

        /// <summary>
        /// 초기화
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
            AddressableTool.GetAddressableAssetAsync<Material>(SetDefaultShader, "defaultShader");
            AddressableTool.GetAddressableAssetAsync<Material>(SetThrowShader, "throwShader");
        }

        /// <summary>
        /// 던지기 게이지를 가져온다
        /// </summary>
        /// <returns></returns>
        public float GetThrowGauge()
        {
            return _throwGauge;
        }

        /// <summary>
        /// 던지기 게이지 증감
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
        /// 업데이트 딜레이
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
        /// 던지기 가능한 유닛들의 시각적 효과를 설정한다.
        /// </summary>
        private void CheckCanThrow()
        {
            int count = _unitCommand._playerUnitList.Count;
            for (int i = 1; i < count; i++)
            {
                Unit unit = _unitCommand._playerUnitList[i];
                if(unit.UnitStat.Return_Weight() < _throwGauge && unit.CheckCanThrow())
				{
                    unit.ChangeMaterial(_throwShader);
				}
                else
                {
                    unit.ChangeMaterial(_defaultShader);
                }
			}
		}

		/// <summary>
		/// 기본 쉐이더를 설정
		/// </summary>
		/// <param name="material"></param>
		private void SetDefaultShader(Material material)
		{
			_defaultShader = material;
		}

		/// <summary>
		/// 던지기 쉐이더를 설정
		/// </summary>
		/// <param name="material"></param>
		private void SetThrowShader(Material material)
        {
            _throwShader = material;
        }
    }

}