using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utill.Data;
using Utill.Tool;
using Main.Deck;

namespace Battle
{
	public class CardRangeComponent : BattleComponent
	{
		//참조변수
		private GameObject _summonRangeImage = null;
		private StageData _stageData = null;

		//변수
		private float _summonRange = 0.0f;
		private float _summonRangeDelay = 30f;

		/// <summary>
		/// 초기화
		/// </summary>
		public void SetInitialization(GameObject summonRangeImage, StageData stageData)
		{
			_summonRangeImage = summonRangeImage;
			_stageData = stageData;
			this._summonRange = -_stageData.max_Range + _stageData.max_Range / 4;
			DrawSummonRange();
		}

		/// <summary>
		/// 소환 범위 그리기를 키거나 끄기
		/// </summary>
		/// <param name="isActive"></param>
		public void SetSummonRangeLine(bool isActive)
		{
			_summonRangeImage.gameObject.SetActive(isActive);
		}

		/// <summary>
		/// 소환 범위 업데이트 및 증가
		/// </summary>
		public void UpdateSummonRange()
		{
			if (_summonRange >= 0)
			{
				return;
			}

			if (_summonRangeDelay > 0)
			{
				_summonRangeDelay -= Time.deltaTime;
				return;
			}

			_summonRangeDelay = 30f;
			_summonRange += _stageData.max_Range / 4;
			DrawSummonRange();
		}

		/// <summary>
		/// 소환 범위 렌더링
		/// </summary>
		public void DrawSummonRange()
		{
			_summonRangeImage.transform.position = new Vector2(-_stageData.max_Range, -0.1f);
			_summonRangeImage.transform.localScale = new Vector2(Mathf.Abs(_stageData.max_Range + _summonRange), 0.5f);
		}

	}
}
