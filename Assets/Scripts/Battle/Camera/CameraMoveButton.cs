using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Battle;
using DG.Tweening;

namespace Battle.Cameras
{
	public class CameraMoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public enum CameraButtonType
		{
			LeftMove = 0,
			RightMove = 1,
		}
		[SerializeField]
		private BattleManager _battleManager = null;
		[SerializeField]
		private Image _image = null;

		private CameraComponent _cameraComponent = null;
		public CameraButtonType _cameraButtonType = CameraButtonType.LeftMove;
		private bool isMove = false;

		void Start()
		{
			_cameraComponent = _battleManager.CameraComponent;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			isMove = true;
			_image.DOFade(0.5f, 0.5f);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			isMove = false;
			_image.DOFade(0.3f, 0.5f);
		}

		public void Update()
		{
			if(isMove)
			{
				switch (_cameraButtonType)
				{
					case CameraButtonType.LeftMove:
						_cameraComponent.OnLeftMove();
						break;
					case CameraButtonType.RightMove:
						_cameraComponent.OnRightMove();
						break;
				}
			}
		}
	}

}