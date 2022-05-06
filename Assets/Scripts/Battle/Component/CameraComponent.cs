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

        private Vector3 _clickPos = Vector3.zero;
        private Vector3 _curPos = Vector3.zero;
        private Vector3 _mousePos;

        private bool _isCameraMove = false;
        private bool _isEffect = false;
        public float _perspectiveZoomSpeed = 0.5f;       // perspective mode.
        public float _orthoZoomSpeed = 0.5f;        //  orthographic mode.
        public float _moveSpeed = 1f;

        //참조 변수
        private StageData _stageData = null;
        private WinLoseComponent _commandWinLose = null;
        private CardComponent _commandCard = null;

        //인스펙터 참조 변수
        [SerializeField]
        private UnityEngine.Camera _camera = null;
        [SerializeField]
        private Transform _myPencilCase = null;
        [SerializeField]
        private Transform _enemyPencilCase = null;
        [SerializeField]
        private EventTrigger eventTrigger;

        /// <summary>
        /// 초기화
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

            //관찰자를 등록한다
            _commandWinLose.AddObservers(this);
        }

        /// <summary>
        /// 카메라가 움직일 수 있는 상태인지
        /// </summary>
        /// <param name="isCameraMove">True면 움직일 수 있음</param>
        public void SetCameraIsMove(bool isCameraMove)
        {
            this._isCameraMove = isCameraMove;

        }

        /// <summary>
        /// 카메라 크기 조정
        /// </summary>
        public void UpdateCameraScale()
        {
            if (_isEffect)
                return;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (_stageData.max_Range - 1 < _camera.orthographicSize)
                    return;
                _camera.orthographicSize += Time.deltaTime * 10;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if (1 > _camera.orthographicSize)
                    return;
                _camera.orthographicSize -= Time.deltaTime * 10;
            }

            //아래는 모바일로 테스트

            // 손가락으로 줌인/아웃의 경우 무조건 2손가락이 터치가 되어야 하기 때문에 Count = 2일 경우만 동작
            if (Input.touchCount == 2)
            {
                // Store both touches.
                Touch touchZero = Input.GetTouch(0); //첫번째 손가락 좌표
                Touch touchOne = Input.GetTouch(1); //두번째 손가락 좌표

                // deltaposition은 deltatime과 동일하게 delta만큼 시간동안 움직인 거리를 말한다.

                // 현재 position에서 이전 delta값을 빼주면 움직이기 전의 손가락 위치가 된다.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // 현재와 과거값의 움직임의 크기를 구한다.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // 두 값의 차이는 즉 확대/축소할때 얼만큼 많이 확대/축소가 진행되어야 하는지를 결정한다.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                // If the camera is orthographic...
                if (_camera.orthographic)
                {
                    // ... change the orthographic size based on the change in distance between the touches.
                    _camera.orthographicSize += deltaMagnitudeDiff * _orthoZoomSpeed;

                    // Make sure the orthographic size never drops below zero.
                    _camera.orthographicSize = Mathf.Max(_camera.orthographicSize, 0.1f);
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
        /// 방향키로 좌우 이동이 가능함
        /// </summary>
        public void UpdateInputMove()
		{
            if(Input.GetKey(KeyCode.RightArrow))
			{
                OnRightMove();
			}
            if(Input.GetKey(KeyCode.LeftArrow))
			{
                OnLeftMove();
			}

        }

        /// <summary>
        /// 카메라를 지정한 곳으로 이동하게 함
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
        /// 왼쪽으로 카메라 이동
        /// </summary>
        public void OnLeftMove()
        {
            if (_isEffect)
            {
                return;
            }
            if (_commandCard.IsSelectCard)
            {
                return;
            }
            if (-_stageData.max_Range - 1f > _camera.transform.position.x)
            {
                return;
            }
            _camera.transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 오른쪽으로 카메라 이동
        /// </summary>
        public void OnRightMove()
        {
            if (_isEffect)
            {
                return;
            }
            if (_commandCard.IsSelectCard)
            {
                return;
            }

            if (_stageData.max_Range + 1f < _camera.transform.position.x)
            {
                return;
            }
                _camera.transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 승리 카메라 이펙트
        /// </summary>
        public void WinCamEffect(Vector2 pos, bool isWin)
        {
            if (_isEffect)
            {
                return;
            }
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
        /// 승리에 따라 필통이 파괴되는 연출을 사용한다.
        /// </summary>
        /// <param name="isWin"></param>
		public void Notify(bool isWin)
		{
            if(isWin)
			{
                WinCamEffect(_enemyPencilCase.position, isWin);
			}
            else
			{
                WinCamEffect(_myPencilCase.position, isWin);
			}
        }
	}

}