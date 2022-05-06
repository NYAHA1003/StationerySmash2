using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Load;
using DG.Tweening;
using TMPro;
using Main.Deck;

namespace Battle
{


	[System.Serializable]
	public class IntroComponent
	{
		//프로퍼티
		public bool isEndIntro => _isEndIntro;

		//인스펙터 참조 변수
		[SerializeField]
		private TextMeshProUGUI _countText;
		[SerializeField]
		private Transform _playerPencilCase;
		[SerializeField]
		private Transform _enemyPencilCase;
		[SerializeField]
		private GameObject _introCanvas = null;
		[SerializeField]
		private PencilCaseDataSO _playerPencilCaseDataSO = null;
		[SerializeField]
		private PencilCaseDataSO _enemyPencilCaseDataSO = null;
		[SerializeField]
		private AIDataSO _enemyAIDataSO = null;
		[SerializeField]
		private CardDeckSO _inGameCardDataSO = null;
		[SerializeField]
		private PencilCaseCard _pencilCaseCard = null;
		[SerializeField]
		private List<DeckCard> _deckCards = null;

		//변수
		private bool _isEndIntro = false;
		private List<CardData> _playerCardDatas = null;
		private List<CardData> _enemyCardDatas = null;

		//참조변수 
		private CameraComponent _cameraComponent = null;

		/// <summary>
		/// 초기화
		/// </summary>
		/// <param name="cameraCommand"></param>
		public void SetInitialization(CameraComponent cameraCommand)
		{
			_cameraComponent = cameraCommand;
			_playerCardDatas = _inGameCardDataSO.cardDatas;
			_enemyCardDatas = _enemyAIDataSO.cardDataList;
		}

		/// <summary>
		/// 덱에 대한 정보를 변경한다
		/// </summary>
		public void ShowDeckInfo(TeamType teamType)
		{
			int deckCardCount = _deckCards.Count;
			for (int i = 0; i < deckCardCount; i++)
			{
				_deckCards[i].gameObject.SetActive(false);
			}

			if (teamType == TeamType.MyTeam)
			{
				_pencilCaseCard.SetPencilCaseData(_playerPencilCaseDataSO._pencilCaseData);
				int cardDataCount = _playerCardDatas.Count;
				for (int i = 0; i < cardDataCount; i++)
				{
					_deckCards[i].SetCard(_playerCardDatas[i]);
					_deckCards[i].gameObject.SetActive(true);
				}
			}
			else if(teamType == TeamType.EnemyTeam)
			{
				_pencilCaseCard.SetPencilCaseData(_enemyPencilCaseDataSO._pencilCaseData);
				int cardDataCount = _enemyCardDatas.Count;
				for (int i = 0; i < cardDataCount; i++)
				{
					_deckCards[i].SetCard(_enemyCardDatas[i]);
					_deckCards[i].gameObject.SetActive(true);
				}
			}
		}

		/// <summary>
		/// 인트로 시작
		/// </summary>
		public IEnumerator SetIntro()
		{
			_countText.gameObject.SetActive(true);

			//맵 중앙을 보여준다
			_countText.text = "3";
			_cameraComponent.MovingCamera(Vector2.zero, 1f, 0.3f);
			yield return new WaitForSeconds(1f);

			//내 필통을 보여준다
			_countText.text = "2";
			_cameraComponent.MovingCamera(_playerPencilCase.position, 0.5f, 0.2f, 0.85f);
			ShowDeckInfo(TeamType.MyTeam);
			yield return new WaitForSeconds(0.2f);
			_introCanvas.SetActive(true);
			yield return new WaitForSeconds(1f);
			_introCanvas.SetActive(false);

			//상대 필통을 보여준다
			_countText.text = "1";
			_cameraComponent.MovingCamera(_enemyPencilCase.position, 0.5f, 0.2f, 0.85f);
			ShowDeckInfo(TeamType.EnemyTeam);
			yield return new WaitForSeconds(0.2f);
			_introCanvas.SetActive(true);
			yield return new WaitForSeconds(1f);
			_introCanvas.SetActive(false);

			//다시 내 필통쪽으로 클로즈업한다
			_countText.text = "GO!";
			_cameraComponent.MovingCamera(_playerPencilCase.position, 1f, 0.2f);
			yield return new WaitForSeconds(1f);
			_countText.gameObject.SetActive(false);
			_isEndIntro = true;
		}
	}
}
