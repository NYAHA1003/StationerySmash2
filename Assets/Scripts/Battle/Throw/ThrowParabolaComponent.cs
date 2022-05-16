using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;


namespace Battle
{
	public class ThrowParabolaComponent : BattleComponent
	{
		private LineRenderer _parabola;
		private ThrowComponent _throwComponent = null;
		private StageData _stageData;
		private List<Vector2> _lineZeroPos;
		private GameObject _parabolaBackground = null;
		private CameraComponent _cameraCommand = null;


		private float _force;
		private float _pullTime;
		private float _throwGauge = 0f;
		private float _throwGaugeSpeed = 0f;

		public void SetInitialization()
		{
			_lineZeroPos = new List<Vector2>(_parabola.positionCount);
			for (int i = 0; i < _parabola.positionCount; i++)
			{
				_lineZeroPos.Add(Vector2.zero);
			}

		}

		/// <summary>
		/// ���η������� ������ ��ġ �༭ ����
		/// </summary>
		/// <param name="linePos"></param>
		public void SetParabolaPos(int count, float width, float force, float radDir, float time)
		{
			List<Vector2> linePos = ReturnParabolaPos(count, width, force, radDir, time);
			for (int i = 0; i < _parabola.positionCount; i++)
			{
				_parabola.SetPosition(i, linePos[i]);
			}
		}

		/// <summary>
		/// ������ �׸��� ����
		/// </summary>
		public void UnSetParabolaPos()
		{
			for (int i = 0; i < _parabola.positionCount; i++)
			{
				_parabola.SetPosition(i, _lineZeroPos[i]);
			}
		}

		/// <summary>
		/// ������ �׸��� & ������ ��� ����
		/// </summary>
		/// <param name="pos"></param>
		public bool DrawParabola(Vector2 pos)
		{
			Unit throwedUnit = _throwComponent.ThrowedUnit;
			
			if (throwedUnit != null)
			{
				//�ð��� ������ ���
				_pullTime -= Time.deltaTime;
				if (_pullTime < 0)
				{
					throwedUnit.UnitSprite.OrderDraw(throwedUnit.OrderIndex);
					throwedUnit.UnitSticker.OrderDraw(throwedUnit.OrderIndex);
					throwedUnit = null;
					_parabolaBackground.SetActive(false);
					_cameraCommand.SetIsDontMove(false);
					UnDrawParabola();
					return false;
				}

				_cameraCommand.SetIsDontMove(true);

				//������ �ٸ� �ൿ�� ���ϰ� �Ǹ� ���
				if (throwedUnit.Pulling_Unit() == null)
				{
					throwedUnit.UnitSprite.OrderDraw(throwedUnit.OrderIndex);
					throwedUnit.UnitSticker.OrderDraw(throwedUnit.OrderIndex);
					_parabolaBackground.SetActive(false);
					UnDrawParabola();
					throwedUnit = throwedUnit.Pulling_Unit();
					_cameraCommand.SetIsDontMove(false);
					return false;
				}

				//����
				_direction = (Vector2)throwedUnit.transform.position - pos;
				float dir = Mathf.Atan2(_direction.y, _direction.x);
				float dirx = Mathf.Atan2(_direction.y, -_direction.x);

				//������ ���⿡ ���� �������� �� ���̰� ��
				if (dir < 0)
				{
					return false;
				}

				//ȭ��ǥ
				_arrow.transform.position = throwedUnit.transform.position;
				_arrow.transform.eulerAngles = new Vector3(0, 0, dir * Mathf.Rad2Deg);

				//�ʱ� ����
				_force = Mathf.Clamp(Vector2.Distance(throwedUnit.transform.position, pos), 0, 1) * 4 * (100.0f / _throwedUnit.UnitStat.Return_Weight());

				//���� ���� �Ÿ�
				float width = Parabola.Caculated_Width(_force, dirx);
				//���� ���� �ð�
				float time = Parabola.Caculated_Time(_force, dir, 2);

				SetParabolaPos(_parabola.positionCount, width, _force, dir, time);

				return true;
			}
			return false;
		}

		/// <summary>
		/// ������ ��ġ�� ��ȯ
		/// </summary>
		/// <param name="count"></param>
		/// <param name="width"></param>
		/// <param name="force"></param>
		/// <param name="radDir"></param>
		/// <param name="time"></param>
		/// <returns></returns>
		private List<Vector2> ReturnParabolaPos(int count, float width, float force, float radDir, float time)
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

			Unit throwUnit = _throwComponent.ThrowedUnit;
			for (int i = 0; i < count; i++)
			{
				Vector3 pos = Vector3.Lerp((Vector2)throwUnit.transform.position, new Vector2(throwUnit.transform.position.x - width, 0), objLerps[i]);
				pos.y = Parabola.Caculated_TimeToPos(force, radDir, timeLerps[i]);

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
	}

}