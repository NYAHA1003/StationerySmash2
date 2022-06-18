using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;
using Utill.Load;
using DG.Tweening;
using TMPro;
using Main.Deck;
using System.Linq;

namespace Battle
{


	[System.Serializable]
	public class IntroComponent
	{
		//������Ƽ
		public bool isEndIntro => _isEndIntro;

		//�ν����� ���� ����
		[SerializeField]
		private TextMeshProUGUI _countText;
		[SerializeField]
		private RectTransform _countTextRect;
		[SerializeField]
		private Transform _playerPencilCase;
		[SerializeField]
		private Transform _enemyPencilCase;
		[SerializeField]
		private GameObject _introCanvas = null;
		[SerializeField]
		private GameObject _cardDatas = null;
		[SerializeField]
		private AIDataSO _enemyAIDataSO = null;
		[SerializeField]
		private CardDeckSO _inGameCardDataSO = null;
		[SerializeField]
		private PencilCaseCard _pencilCaseCard = null;
		[SerializeField]
		private RectTransform _pencilCaseCardRectTrm;
		[SerializeField]
		private List<DeckCard> _deckCards = null;
		[SerializeField]
		private Canvas _uiCanvas = null;
		[SerializeField]
		private Image _fadeUp = null;
		[SerializeField]
		private Image _fadeDown = null;

		//����
		private bool _isEndIntro = false;
		private List<CardData> _playerCardDatas = null;
		private List<CardData> _enemyCardDatas = null;

		//�������� 
		private CameraComponent _cameraComponent = null;
		private MonoBehaviour _managerBase = null;

		/// <summary>
		/// �ʱ�ȭ
		/// </summary>
		/// <param name="cameraCommand"></param>
		public void SetInitialization(CameraComponent cameraCommand, MonoBehaviour managerBase)
		{
			_cameraComponent = cameraCommand;
			_playerCardDatas = _inGameCardDataSO.cardDatas;

			_enemyCardDatas = _enemyAIDataSO.cardDataList.Distinct(new CardDataComparer()).ToList();
			_managerBase = managerBase;
		}

		/// <summary>
		/// ��Ʈ�� ����
		/// </summary>
		public void StartIntro()
		{
			_managerBase.StartCoroutine(SetIntro());
		}

		/// <summary>
		/// ���� ���� ������ �����Ѵ�
		/// </summary>
		private void ShowDeckInfo(TeamType teamType)
		{
			int deckCardCount = _deckCards.Count;
			for (int i = 0; i < deckCardCount; i++)
			{
				_deckCards[i].gameObject.SetActive(false);
			}

			if (teamType == TeamType.MyTeam)
			{
				_pencilCaseCard.SetPencilCaseData(PencilCaseDataManagerSO.InGamePencilCaseData);
				int cardDataCount = _playerCardDatas.Count;
				for (int i = 0; i < cardDataCount; i++)
				{
					_deckCards[i].SetCard(_playerCardDatas[i]);
					_deckCards[i].gameObject.SetActive(true);
				}
			}
			else if (teamType == TeamType.EnemyTeam)
			{
				_pencilCaseCard.SetPencilCaseData(PencilCaseDataManagerSO.EnemyGamePencilCaseData);
				int cardDataCount = _enemyCardDatas.Count;
				for (int i = 0; i < cardDataCount; i++)
				{
					_deckCards[i].SetCard(_enemyCardDatas[i]);
					_deckCards[i].gameObject.SetActive(true);
				}
			}
		}

		/// <summary>
		/// ��Ʈ�� ����
		/// </summary>
		private IEnumerator SetIntro()
		{
			_uiCanvas.gameObject.SetActive(false);
			_introCanvas.gameObject.SetActive(true);
			_countText.gameObject.SetActive(true);

			//�� �߾��� �����ش�
			_fadeUp.rectTransform.sizeDelta = new Vector2(5000, 300);
			_fadeDown.rectTransform.sizeDelta = new Vector2(5000, 300);
			_fadeUp.DOFade(1, 0.3f);
			_fadeDown.DOFade(1, 0.3f);
			_countText.text = "3";
			_countTextRect.localScale = new Vector2(0.1f, 0.1f);
			_countTextRect.DOScale(2, 0.3f);
			_cameraComponent.MovingCamera(Vector2.zero, 1f, 0.3f);
			yield return new WaitForSeconds(1f);

			//�� ������ �����ش�
			_countText.text = "2";
			_pencilCaseCardRectTrm.anchoredPosition = new Vector2(-1450, 0);
			_countTextRect.localScale = new Vector2(0.1f, 0.1f);
			_countTextRect.DOScale(2, 0.3f);
			_cameraComponent.MovingCamera(_playerPencilCase.position, 0.5f, 0.2f, 0.85f);
			ShowDeckInfo(TeamType.MyTeam);
			yield return new WaitForSeconds(0.2f);
			_cardDatas.SetActive(true);
			yield return new WaitForSeconds(1f);
			_cardDatas.SetActive(false);

			//��� ������ �����ش�
			_countText.text = "1";
			_pencilCaseCardRectTrm.anchoredPosition = new Vector2(1450, 0);
			_countTextRect.localScale = new Vector2(0.1f, 0.1f);
			_countTextRect.DOScale(2, 0.3f);
			_cameraComponent.MovingCamera(_enemyPencilCase.position, 0.5f, 0.2f, 0.85f);
			ShowDeckInfo(TeamType.EnemyTeam);
			yield return new WaitForSeconds(0.2f);
			_cardDatas.SetActive(true);
			yield return new WaitForSeconds(1f);
			_cardDatas.SetActive(false);

			//�������� �߾����� ī�޶� �ű��
			_fadeUp.DOFade(0, 0.3f);
			_fadeDown.DOFade(0, 0.3f);
			_uiCanvas.gameObject.SetActive(true);
			_countText.text = "GO!";
			_countTextRect.localScale = new Vector2(0.1f, 0.1f);
			_countTextRect.DOScale(2.5f, 0.3f);
			_cameraComponent.MovingCameraMoverOrigin(0.2f);
			yield return new WaitForSeconds(0.3f);
			_introCanvas.gameObject.SetActive(false);
			yield return new WaitForSeconds(0.7f);
			_countText.gameObject.SetActive(false);
			_isEndIntro = true;

		}
	}
}
