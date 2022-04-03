using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utill;

namespace Battle
{
    [System.Serializable]
    public class CameraCommand : BattleCommand
    {

        private Vector3 _clickPos = Vector3.zero;
        private Vector3 _curPos = Vector3.zero;
        private Vector3 _mousePos;

        private bool _isCameraMove = false;
        private bool _isEffect = false;

        [SerializeField]
        private Camera _camera = null;
        public float _perspectiveZoomSpeed = 0.5f;       // perspective mode.
        public float _orthoZoomSpeed = 0.5f;        //  orthographic mode.

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="camera"></param>
        public void SetInitialization(BattleManager battleManager)
        {
            this._battleManager = battleManager;
            battleManager.AddUpdateAction(UpdateCameraPos);
            battleManager.AddUpdateAction(UpdateCameraScale);
        }

        /// <summary>
        /// ī�޶� ������ �� �ִ� ��������
        /// </summary>
        /// <param name="isCameraMove">True�� ������ �� ����</param>
        public void SetCameraIsMove(bool isCameraMove)
        {
            this._isCameraMove = isCameraMove;

        }

        /// <summary>
        /// ī�޶� ũ�� ����
        /// </summary>
        public void UpdateCameraScale()
        {
            if (_isEffect)
                return;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (_battleManager.CurrentStageData.max_Range - 1 < _camera.orthographicSize)
                    return;
                _camera.orthographicSize += Time.deltaTime * 10;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if (1 > _camera.orthographicSize)
                    return;
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
        /// ī�޶� ��ġ ����
        /// </summary>
        public void UpdateCameraPos()
        {
            if (_isEffect)
                return;

            //ī�带 Ŭ���� ���¶��
            if (_battleManager.CommandCard.IsSelectCard)
            {
                _isCameraMove = false;
                return;
            }

            _mousePos = Input.mousePosition * 0.005f;

            if (Input.GetMouseButtonDown(0) && Input.mousePosition.y > _camera.pixelHeight * 0.3f)
            {
                _clickPos = _mousePos;
                _curPos = _camera.transform.position;
                _isCameraMove = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _isCameraMove = false;
            }

            if (_isCameraMove)
            {
                _camera.transform.position = new Vector3(_curPos.x + (_clickPos.x + -_mousePos.x), 0, -10);
                if (_battleManager.CurrentStageData.max_Range + 1f < _camera.transform.position.x)
                {
                    _camera.transform.DOMoveX(_battleManager.CurrentStageData.max_Range, 0.1f);
                }
                if (-_battleManager.CurrentStageData.max_Range - 1f > _camera.transform.position.x)
                {
                    _camera.transform.DOMoveX(-_battleManager.CurrentStageData.max_Range, 0.1f);
                }
            }
        }

        /// <summary>
        /// �¸� ī�޶� ����Ʈ
        /// </summary>
        public void WinCamEffect(Vector2 pos, bool isWin)
        {
            if (_isEffect)
                return;
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
                        _battleManager.CommandWinLose.SetWinLosePanel(isWin);
                    });
                });
            });
        }
    }

}