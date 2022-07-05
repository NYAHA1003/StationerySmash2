using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Utill;

namespace Battle
{
	[System.Serializable]
	public class CameraComponent : BattleComponent, IWinLose
	{
		private bool _isEffect = false;
		private bool _isDontMove = false;
		public float _perspectiveZoomSpeed = 0.5f;       // perspective mode.
		public float _orthoZoomSpeed = 0.5f;        //  orthographic mode.
		public float _moveSpeed = 1f;

		//���� ����
		private StageData _stageData = null;
		private WinLoseComponent _commandWinLose = null;
		private CardComponent _commandCard = null;

		//�ν����� ���� ����
		[SerializeField]
		private UnityEngine.Camera _camera = null;
		[SerializeField]
		private Transform _cameraMover = null;
		[SerializeField]
		private Transform _myPencilCase = null;
		[SerializeField]
		private Transform _enemyPencilCase = null;
		[SerializeField]
		private EventTrigger eventTrigger;
		[SerializeField]
		private GameObject _leftButton;
		[SerializeField]
		private GameObject _rightButton;

		private Vector3 _originVector = Vector3.zero;

		/// <summary>
		/// �ʱ�ȭ
		/// </summary>
		/// <param name="battleManager"></param>
		/// <param name="camera"></param>
		public void SetInitialization(CardComponent cardCommand, WinLoseComponent commandWInLose, ref System.Action updateAction, StageData stageData)
		{
			_stageData = stageData;
			_commandWinLose = commandWInLose;
			_commandCard = cardCommand;

			updateAction += UpdateCameraScale;
			updateAction += UpdateInputMove;

			Vector3 pos = _camera.transform.localPosition;
			pos.y = (0.9f + (_stageData.max_Range - 1) * -0.3f);
			_camera.transform.localPosition = pos;
			_originVector = _camera.transform.localPosition;

			//�����ڸ� ����Ѵ�
			_commandWinLose.AddObservers(this);
		}

		/// <summary>
		/// ī�޶� ������ ������ �̵��ϰ� ��
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="size"></param>
		/// <param name="duration"></param>
		public void MovingCamera(Vector3 pos, float size, float duration, float lerp = 1)
		{
			pos.x = Mathf.Lerp(_camera.transform.position.x, pos.x, lerp);
			pos.y += 0.1f;
			pos.z = -10;
			_camera.transform.DOMove(pos, duration).SetEase(Ease.OutExpo);
			_camera.transform.DOScale(size, duration).SetEase(Ease.OutExpo);
			DOTween.To(() => _camera.orthographicSize, x => _camera.orthographicSize = x, size, duration);
		}
		/// <summary>
		/// ����� ���������ʰ� ī�޶� �̵���Ų��
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="duration"></param>
		/// <param name="lerp"></param>
		public void MovingCameraMoverNoneSize(Vector3 pos, float duration, float lerp = 1)
		{
			pos.x = Mathf.Lerp(_cameraMover.transform.position.x, pos.x, lerp);
			pos.y += 0.1f;
			pos.z = -10;
			_cameraMover.transform.DOMove(pos, duration).SetEase(Ease.OutExpo);
		}

		/// <summary>
		/// ī�޶��� Y��ġ�� ���� ��ġY�� �̵��ϰ� ��
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="size"></param>
		/// <param name="duration"></param>
		public void MovingCameraMoverOrigin(float duration)
		{
			_camera.transform.DOMove(_originVector, duration).SetEase(Ease.OutExpo);
			_camera.transform.DOScale(Vector3.one, duration).SetEase(Ease.OutExpo);
			DOTween.To(() => _camera.orthographicSize, x => _camera.orthographicSize = x, 1 + (_stageData.max_Range - 1) * 0.5f, duration);
		}

		/// <summary>
		/// �������� ī�޶� �̵�
		/// </summary>
		public void OnLeftMove()
		{
			if (_isEffect)
			{
				return;
			}
			if (_commandCard.ReturnIsSelected())
			{
				return;
			}
			if (_isDontMove)
			{
				return;
			}
			if (-_stageData.max_Range - 1f > _cameraMover.transform.position.x)
			{
				return;
			}
			_cameraMover.Translate(Vector3.left * _moveSpeed * Time.deltaTime);
		}

		/// <summary>
		/// ���������� ī�޶� �̵�
		/// </summary>
		public void OnRightMove()
		{
			if (_isEffect)
			{
				return;
			}
			if (_commandCard.ReturnIsSelected())
			{
				return;
			}
			if (_isDontMove)
			{
				return;
			}

			if (_stageData.max_Range + 1f < _cameraMover.transform.position.x)
			{
				return;
			}
			_cameraMover.Translate(Vector3.right * _moveSpeed * Time.deltaTime);
		}

		/// <summary>
		/// �¸��� ���� ������ �ı��Ǵ� ������ ����Ѵ�.
		/// </summary>
		/// <param name="isWin"></param>
		public void Notify(bool isWin)
		{
			if (isWin)
			{
				WinCamEffect(_enemyPencilCase.position, isWin);
			}
			else
			{
				WinCamEffect(_myPencilCase.position, isWin);
			}
		}

		/// <summary>
		/// ī�޶� ������ �� ���� ������
		/// </summary>
		/// <param name="boolean"></param>
		public void SetIsDontMove(bool boolean)
		{
			_isDontMove = boolean;
		}

		/// <summary>
		/// ī�޶� �ȿ� ������ ��鸲
		/// </summary>
		public void CameraInShake(Transform transform, float power, float time)
		{
			if (IsTargetVisible(transform))
			{
				ShakeCamera(power, time);
			}
		}

		/// <summary>
		/// ī�޶� ����
		/// </summary>
		/// <param name="power"></param>
		/// <param name="time"></param>
		public void ShakeCamera(float power, float time)
		{
			_camera.DOShakePosition(time, new Vector3(0, power))
				.OnComplete(() =>
				{
					_camera.transform.localPosition = _originVector;

				});
		}

		/// <summary>
		/// ī�޶� �ȿ� Ÿ���� ���Դ��� üũ
		/// </summary>
		/// <param name="_camera"></param>
		/// <param name="_transform"></param>
		/// <returns></returns>
		public bool IsTargetVisible(Transform transform)
		{
			var planes = GeometryUtility.CalculateFrustumPlanes(_camera);
			var point = transform.position;
			foreach (var plane in planes)
			{
				if (plane.GetDistanceToPoint(point) < 0)
					return false;
			}
			return true;
		}

		/// <summary>
		/// ī�޶� ũ�� ����
		/// </summary>
		private void UpdateCameraScale()
		{
			if (_isEffect)
			{
				return;
			}
			if (_isDontMove)
			{
				return;
			}

			//ī�޶� ũ�Ⱑ Ŭ ��
			if ((1 + (_stageData.max_Range - 1) * 0.5f) - _camera.orthographicSize <= 0.2f)
			{
				SetFalseCameraMoveButton();
				MovingCameraMoverNoneSize(Vector2.zero, 0.2f, 0.1f);
			}
			//ī�޶� ũ�Ⱑ ���� ��
			else
			{
				SetActiveCameraMoveButton();
			}

			if (Input.GetKey(KeyCode.UpArrow))
			{
				if ((1 + (_stageData.max_Range - 1) * 0.5f < _camera.orthographicSize))
				{
					return;
				}
				_camera.orthographicSize += Time.deltaTime * 10;
			}
			else if (Input.GetKey(KeyCode.DownArrow))
			{
				if (1 > _camera.orthographicSize)
				{
					return;
				}
				_camera.orthographicSize -= Time.deltaTime * 10;
			}

			//�Ʒ��� ����Ϸ� �׽�Ʈ

			// �հ������� ����/�ƿ��� ��� ������ 2�հ����� ��ġ�� �Ǿ�� �ϱ� ������ Count = 2�� ��츸 ����
			if (Input.touchCount == 2)
			{
				// Store both touches.
				Touch touchZero = Input.GetTouch(0); //ù��° �հ��� ��ǥ
				Touch touchOne = Input.GetTouch(1); //�ι�° �հ��� ��ǥ

				// deltaposition�� deltatime�� �����ϰ� delta��ŭ �ð����� ������ �Ÿ��� ���Ѵ�.

				// ���� position���� ���� delta���� ���ָ� �����̱� ���� �հ��� ��ġ�� �ȴ�.
				Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
				Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

				// ����� ���Ű��� �������� ũ�⸦ ���Ѵ�.
				float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
				float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

				// �� ���� ���̴� �� Ȯ��/����Ҷ� ��ŭ ���� Ȯ��/��Ұ� ����Ǿ�� �ϴ����� �����Ѵ�.
				float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

				// If the camera is orthographic...
				if (_camera.orthographic)
				{
					// ... change the orthographic size based on the change in distance between the touches.
					_camera.orthographicSize += deltaMagnitudeDiff * _orthoZoomSpeed * Time.deltaTime;

					// Make sure the orthographic size never drops below zero.
					_camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, 1, _stageData.max_Range - 1);
				}
				else
				{
					// Otherwise change the field of view based on the change in distance between the touches.
					_camera.fieldOfView += deltaMagnitudeDiff * _perspectiveZoomSpeed;

					// Clamp the field of view to make sure it's between 0 and 180.
					_camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView, 0.1f, 179.9f);
				}
			}
		}

		/// <summary>
		/// �¸� ī�޶� ����Ʈ
		/// </summary>
		private void WinCamEffect(Vector2 pos, bool isWin)
		{
			if (_isEffect)
			{
				return;
			}
			_isEffect = true;
			float time = Vector2.Distance(_camera.transform.position, pos) / 5;
			_camera.transform.DOMove(new Vector3(pos.x, pos.y, -10), time);

			DOTween.To(() => _camera.orthographicSize, x => _camera.orthographicSize = x, 1f, time);
			_camera.transform.DORotate(new Vector3(0, 0, Random.Range(-30f, -10f)), 0.07f).SetDelay(0.2f).OnComplete(() =>
			{
				DOTween.To(() => _camera.orthographicSize, x => _camera.orthographicSize = x, 0.8f, 0.05f).SetDelay(0.2f);
				_camera.transform.DORotate(new Vector3(0, 0, Random.Range(10f, 30f)), 0.07f).SetDelay(0.2f).OnComplete(() =>
				{
					DOTween.To(() => _camera.orthographicSize, x => _camera.orthographicSize = x, 0.6f, 0.05f).SetDelay(0.2f); ;
					_camera.transform.DORotate(new Vector3(0, 0, Random.Range(-30f, -10f)), 0.07f).SetDelay(0.2f).OnComplete(() =>
					{
						_commandWinLose.SetWinLosePanel(isWin);
					});
				});
			});
		}

		/// <summary>
		/// ����Ű�� �¿� �̵��� ������
		/// </summary>
		private void UpdateInputMove()
		{
			if (Input.GetKey(KeyCode.RightArrow))
			{
				OnRightMove();
			}
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				OnLeftMove();
			}

		}


		/// <summary>
		/// ī�޶� �̵� ��ư Ȱ��ȭ
		/// </summary>
		private void SetActiveCameraMoveButton()
		{
			_leftButton.SetActive(true);
			_rightButton.SetActive(true);
		}

		/// <summary>
		/// ī�޶� �̵� ��ư ��Ȱ��ȭ
		/// </summary>
		private void SetFalseCameraMoveButton()
		{
			_leftButton.SetActive(false);
			_rightButton.SetActive(false);
		}
	}

}