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
		private StageData _stageData = null;
		private GameObject _summonRangeImage = null;
		private CardComponent _cardComponent = null;
		private CardSelectComponent _cardSelectComponent = null;
		private CostComponent _costComponent = null;

		//변수
		private float _maxsummonRange = 0.0f;
		private float _minsummonRange = 0.0f;
		private float _summonRangeDelay = 30f;

		//속성
		public float MaxSummonRange => _maxsummonRange;
		public float MinSummonRange => _minsummonRange;

		/// <summary>
		/// 초기화
		/// </summary>
		public void SetInitialization(CardComponent cardComponent, CardSelectComponent cardSelectComponent, CostComponent costComponent, GameObject summonRangeImage, RectTransform summonArrow,  StageData stageData)
		{
			//컴포넌트
			_cardComponent = cardComponent;
			_costComponent = costComponent;
			_cardSelectComponent = cardSelectComponent;

			//소한 범위
			_summonRangeImage = summonRangeImage;
			_stageData = stageData;
			this._maxsummonRange = -_stageData.max_Range + _stageData.max_Range / 4;
			this._minsummonRange = -_stageData.max_Range;

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
			if (_maxsummonRange >= 0)
			{
				return;
			}

			if (_summonRangeDelay > 0)
			{
				_summonRangeDelay -= Time.deltaTime;
				return;
			}

			_summonRangeDelay = 30f;
			_maxsummonRange += _stageData.max_Range / 4;
			DrawSummonRange();
		}

		/// <summary>
		/// 소환 범위 렌더링
		/// </summary>
		public void DrawSummonRange()
		{
			_summonRangeImage.transform.position = new Vector2(-_stageData.max_Range, -0.1f);
			_summonRangeImage.transform.localScale = new Vector2(Mathf.Abs(_stageData.max_Range + _maxsummonRange), 0.5f);
		}

		/// <summary>
		/// 카드를 여러 조건에 따라 사용할 수 있는지 체크
		/// </summary>
		private bool CheckPossibleSummon()
		{
			if (_cardSelectComponent.SelectedCard == null)
			{
				return false;
			}
			//테스트용 소환 조건 해제
			if (_cardComponent.IsAlwaysSpawn)
			{
				return true;
			}

			switch (_cardSelectComponent.SelectedCard.CardDataValue._cardType)
			{
				case CardType.Execute:
					break;
				case CardType.SummonUnit:
				case CardType.SummonTrap:
				case CardType.Installation:
					break;
			}

			if (_costComponent.CurrentCost < _cardSelectComponent.SelectedCard.CardCost)
			{
				return false;
			}

			return true;
		}
	}
}
