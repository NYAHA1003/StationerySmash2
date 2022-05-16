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
		private StageData _stageData = null;
		private GameObject _summonRangeImage = null;
		private RectTransform _summonArrow = null;
		private GameObject _unitAfterImage = null;
		private SpriteRenderer _afterImageSpriteRenderer = null;
		private CardComponent _cardComponent = null;
		private CardSelectComponent _cardSelectComponent = null;
		private CostComponent _costComponent = null;

		//����
		private float _maxsummonRange = 0.0f;
		private float _minsummonRange = 0.0f;
		private float _summonRangeDelay = 30f;

		//�Ӽ�
		public float MaxSummonRange => _maxsummonRange;
		public float MinSummonRange => _minsummonRange;

		/// <summary>
		/// �ʱ�ȭ
		/// </summary>
		public void SetInitialization(CardComponent cardComponent, CardSelectComponent cardSelectComponent, CostComponent costComponent, GameObject summonRangeImage, RectTransform summonArrow,  StageData stageData, GameObject unitAfterImage, SpriteRenderer afterImageSprRenderer)
		{
			//������Ʈ
			_cardComponent = cardComponent;
			_costComponent = costComponent;
			_cardSelectComponent = cardSelectComponent;

			//���� ����
			_summonRangeImage = summonRangeImage;
			_summonArrow = summonArrow;
			_stageData = stageData;
			this._maxsummonRange = -_stageData.max_Range + _stageData.max_Range / 4;
			this._minsummonRange = -_stageData.max_Range;

			//�̸�����
			_unitAfterImage = unitAfterImage;
			_afterImageSpriteRenderer = afterImageSprRenderer;

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
		/// ��ȯ ���� ������
		/// </summary>
		public void DrawSummonRange()
		{
			_summonRangeImage.transform.position = new Vector2(-_stageData.max_Range, -0.1f);
			_summonRangeImage.transform.localScale = new Vector2(Mathf.Abs(_stageData.max_Range + _maxsummonRange), 0.5f);
		}

		/// <summary>
		/// ī�� ��ȯ �̸�����
		/// </summary>
		/// <param name="unitData"></param>
		/// <param name="pos"></param>
		/// <param name="isDelete"></param>
		public void UpdateUnitAfterImage()
		{
			//���콺 ��ġ�� �����´�
			Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
			pos.x = Mathf.Clamp(pos.x, -_stageData.max_Range, _maxsummonRange);

			//��ȯ �̸����Ⱑ �� �� �ִ��� üũ
			if (_cardSelectComponent.SelectedCard == null || pos.y < 0 || _cardSelectComponent.SelectedCard.CardDataValue.unitData.unitType == UnitType.None)
			{
				SetSummonArrowImage(false, pos);
				_unitAfterImage.SetActive(false);
				return;
			}

			//��ȯ �̸����� ����
			_unitAfterImage.SetActive(true);
			_afterImageSpriteRenderer.color = Color.white;

			if (CheckPossibleSummon())
			{
				_afterImageSpriteRenderer.color = Color.red;
			}

			_unitAfterImage.transform.position = new Vector3(pos.x, 0);
			_afterImageSpriteRenderer.sprite = SkinData.GetSkin(_cardSelectComponent.SelectedCard.CardDataValue._skinData._skinType);

			//��ȯ ȭ��ǥ ����
			SetSummonArrowImage(true, pos);
		}

		/// <summary>
		/// ��ȯ ȭ��ǥ ����
		/// </summary>
		private void SetSummonArrowImage(bool isActive, Vector2 pos)
		{
			//��ȯ ȭ��ǥ ����
			_summonArrow.gameObject.SetActive(isActive);
			_summonArrow.transform.position = pos;
			_summonArrow.anchoredPosition = new Vector2(_summonArrow.anchoredPosition.x, Mathf.Clamp(_summonArrow.anchoredPosition.y, 520, 1000));
			_summonArrow.sizeDelta = new Vector2(_summonArrow.sizeDelta.x, _summonArrow.anchoredPosition.y);

			return;
		}

		/// <summary>
		/// ī�带 ���� ���ǿ� ���� ����� �� �ִ��� üũ
		/// </summary>
		private bool CheckPossibleSummon()
		{
			if (_cardSelectComponent.SelectedCard == null)
			{
				return false;
			}
			//�׽�Ʈ�� ��ȯ ���� ����
			if (_cardComponent.IsAlwaysSpawn)
			{
				return true;
			}

			switch (_cardSelectComponent.SelectedCard.CardDataValue.cardType)
			{
				case CardType.Execute:
					break;
				case CardType.SummonUnit:
				case CardType.SummonTrap:
					Vector3 mouse_Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					mouse_Pos.x = Mathf.Clamp(mouse_Pos.x, -_stageData.max_Range, _maxsummonRange);
					if (mouse_Pos.x < -_stageData.max_Range || mouse_Pos.x > _maxsummonRange)
					{
						return false;
					}
					break;
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
