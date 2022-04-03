using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

namespace Battle
{
    [System.Serializable]
    public class ThrowCommand : BattleCommand
    {
        //�ν����� ���� ����
        [SerializeField]
        private LineRenderer _parabola;
        [SerializeField]
        private Transform _arrow;

        //���� ����
        private Unit _throwUnit = null;

        private StageData _stageData;

        private List<Vector2> _lineZeroPos;
        private Vector2 _direction;
        private float _force;
        private float _pullTime;

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        /// <param name="battleManager"></param>
        /// <param name="parabola"></param>
        /// <param name="arrow"></param>
        /// <param name="stageData"></param>
        public void SetInitialization(BattleManager battleManager, StageData stageData)
        {
            this._battleManager = battleManager;
            this._stageData = stageData;
            _lineZeroPos = new List<Vector2>(_parabola.positionCount);
            for (int i = 0; i < _parabola.positionCount; i++)
            {
                _lineZeroPos.Add(Vector2.zero);
            }
        }

        /// <summary>
        /// ���� ���� ����
        /// </summary>
        /// <param name="pos"></param>
        public void PullUnit(Vector2 pos)
        {
            float targetRange = float.MaxValue;
            for (int i = 1; i < _battleManager._myUnitDatasTemp.Count; i++)
            {
                if (Vector2.Distance(pos, _battleManager._myUnitDatasTemp[i].transform.position) < targetRange)
                {
                    _throwUnit = _battleManager._myUnitDatasTemp[i];
                    targetRange = Vector2.Distance(pos, _throwUnit.transform.position);
                }
            }

            if (_throwUnit != null)
            {
                if (Vector2.Distance(pos, _throwUnit.transform.position) < 0.1f)
                {
                    _throwUnit = _throwUnit.Pull_Unit();
                    if (_throwUnit == null)
                    {
                        _battleManager.CommandCamera.SetCameraIsMove(false);
                    }
                    _pullTime = 2f;
                    return;
                }
                _throwUnit = null;
            }
        }

        /// <summary>
        /// ������ �׸���
        /// </summary>
        /// <param name="pos"></param>
        public void DrawParabola(Vector2 pos)
        {
            UnDrawParabola();
            if (_throwUnit != null)
            {
                //�ð��� ������ ���
                _pullTime -= Time.deltaTime;
                if (_pullTime < 0)
                {
                    _throwUnit = null;
                    UnDrawParabola();
                    return;
                }

                //������ �ٸ� �ൿ�� ���ϰ� �Ǹ� ���
                _throwUnit = _throwUnit.Pulling_Unit();
                _battleManager.CommandCamera.SetCameraIsMove(false);

                if (_throwUnit == null)
                {
                    UnDrawParabola();
                    return;
                }

                //����
                _direction = (Vector2)_throwUnit.transform.position - pos;
                float dir = Mathf.Atan2(_direction.y, _direction.x);
                float dirx = Mathf.Atan2(_direction.y, -_direction.x);

                //������ ���⿡ ���� �������� �� ���̰� ��
                if (dir < 0)
                {
                    UnDrawParabola();
                    return;
                }

                //ȭ��ǥ
                _arrow.transform.position = _throwUnit.transform.position;
                _arrow.transform.eulerAngles = new Vector3(0, 0, dir * Mathf.Rad2Deg);

                //�ʱ� ����
                _force = Mathf.Clamp(Vector2.Distance(_throwUnit.transform.position, pos), 0, 1) * 4 * (100.0f / _throwUnit.weight);

                //�ְ���
                float height = Utill.Parabola.Caculated_Height(_force, dirx);
                //���� ���� �Ÿ�
                float width = Utill.Parabola.Caculated_Width(_force, dirx);
                //���� ���� �ð�
                float time = Utill.Parabola.Caculated_Time(_force, dir, 2);

                List<Vector2> linePos = SetParabolaPos(_parabola.positionCount, width, _force, dir, time);

                SetParabolaPos(linePos);

                return;
            }
        }

        /// <summary>
        /// ������ �׸��� ����
        /// </summary>
        public void UnDrawParabola()
        {
            SetParabolaPos(_lineZeroPos);
        }

        /// <summary>
        /// ���η������� ������ ��ġ �༭ ����
        /// </summary>
        /// <param name="linePos"></param>
        private void SetParabolaPos(List<Vector2> linePos)
        {
            for (int i = 0; i < _parabola.positionCount; i++)
            {
                _parabola.SetPosition(i, linePos[i]);
            }
        }

        /// <summary>
        /// ������ ��ġ�� ��ȯ
        /// </summary>
        /// <param name="count"></param>
        /// <param name="width"></param>
        /// <param name="force"></param>
        /// <param name="dir_rad"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private List<Vector2> SetParabolaPos(int count, float width, float force, float dir_rad, float time)
        {
            List<Vector2> results = new List<Vector2>(count);
            float[] objLerps = new float[count];
            float[] timeLerps = new float[count];
            float interbal = 1f / (count - 1 > 0 ? count - 1 : 1);
            float timeInterbal = time / (count - 1 > 0 ? count - 1 : 1);
            for (int i = 0; i < count; i++)
            {
                objLerps[i] = interbal * i;
                timeLerps[i] = timeInterbal * i;
            }

            for (int i = 0; i < count; i++)
            {
                Vector3 pos = Vector3.Lerp((Vector2)_throwUnit.transform.position, new Vector2(_throwUnit.transform.position.x - width, 0), objLerps[i]);
                pos.y = Utill.Parabola.Caculated_TimeToPos(force, dir_rad, timeLerps[i]);

                if (i > 0)
                {
                    if (pos.x >= _stageData.max_Range || pos.x <= -_stageData.max_Range)
                    {
                        pos = results[i - 1];
                    }
                }

                results.Add(pos);
            }

            return results;
        }

        /// <summary>
        /// ���� ������
        /// </summary>
        public void ThrowUnit()
        {
            if (_throwUnit != null)
            {
                _throwUnit.Throw_Unit(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                _throwUnit = null;
                UnDrawParabola();
            }
        }
    }

}
