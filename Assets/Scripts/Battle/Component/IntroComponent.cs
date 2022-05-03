using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;
using DG.Tweening;
using TMPro;

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

		//변수
		private bool _isEndIntro = false;

		//참조변수 
		private CameraComponent _cameraComponent = null;

		/// <summary>
		/// 초기화
		/// </summary>
		/// <param name="cameraCommand"></param>
		public void SetInitialization(CameraComponent cameraCommand)
		{
			_cameraComponent = cameraCommand;
		}

		/// <summary>
		/// 인트로 시작
		/// </summary>
		public IEnumerator SetIntro()
		{
			_countText.gameObject.SetActive(true);
			_countText.text = "3";
			_cameraComponent.MovingCamera(Vector2.zero, 1f, 0.3f);
			yield return new WaitForSeconds(0.7f);
			_countText.text = "2";
			_cameraComponent.MovingCamera(_enemyPencilCase.position, 0.5f, 0.2f);
			yield return new WaitForSeconds(0.7f);
			_countText.text = "1";
			_cameraComponent.MovingCamera(_playerPencilCase.position, 0.5f, 0.2f);
			yield return new WaitForSeconds(0.7f);
			_countText.text = "GO!";
			_cameraComponent.MovingCamera(_playerPencilCase.position, 1f, 0.2f);
			yield return new WaitForSeconds(0.7f);
			_countText.gameObject.SetActive(false);
			_isEndIntro = true;
		}
	}
}
