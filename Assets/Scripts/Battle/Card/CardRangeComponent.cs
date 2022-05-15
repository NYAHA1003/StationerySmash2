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
		//��������
		private GameObject _summonRangeImage = null;
		private StageData _stageData = null;

		//����
		private float _summonRange = 0.0f;
		private float _summonRangeDelay = 30f;

		/// <summary>
		/// �ʱ�ȭ
		/// </summary>
		public void SetInitialization(GameObject summonRangeImage, StageData stageData)
		{
			_summonRangeImage = summonRangeImage;
			_stageData = stageData;
			this._summonRange = -_stageData.max_Range + _stageData.max_Range / 4;
			DrawSummonRange();
		}

		/// <summary>
		/// ��ȯ ���� �׸��⸦ Ű�ų� ����
		/// </summary>
		/// <param name="isActive"></param>
		public void SetSummonRangeLine(bool isActive)
		{
			_summonRangeImage.gameObject.SetActive(isActive);
		}

		/// <summary>
		/// ��ȯ ���� ������Ʈ �� ����
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
		/// ��ȯ ���� ������
		/// </summary>
		public void DrawSummonRange()
		{
			_summonRangeImage.transform.position = new Vector2(-_stageData.max_Range, -0.1f);
			_summonRangeImage.transform.localScale = new Vector2(Mathf.Abs(_stageData.max_Range + _summonRange), 0.5f);
		}

	}
}
