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
		private GameObject _parabolaBackground = null;
		private CameraComponent _cameraCommand = null;
		private Transform _arrow;

		private List<Vector2> _lineZeroPos;

		/// <summary>
		/// 초기화
		/// </summary>
		/// <param name="parabola"></param>
		/// <param name="throwComponent"></param>
		/// <param name="stageData"></param>
		/// <param name="parabolaBackground"></param>
		/// <param name="cameraCommand"></param>
		/// <param name="arrow"></param>
		public void SetInitialization(LineRenderer parabola, ThrowComponent throwComponent, StageData stageData, GameObject parabolaBackground, CameraComponent cameraCommand, Transform arrow)
		{
			_parabola = parabola;
			_throwComponent = throwComponent;
			_stageData = stageData;
			_parabolaBackground = parabolaBackground;
			_cameraCommand = cameraCommand;
			_arrow = arrow;

			_lineZeroPos = new List<Vector2>(_parabola.positionCount);
			for (int i = 0; i < _parabola.positionCount; i++)
			{
				_lineZeroPos.Add(Vector2.zero);
			}

		}

		/// <summary>
		/// 라인렌더러의 포물선 위치 줘서 설정
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
		/// 포물선 그리기 해제
		/// </summary>
		public void UnDrawParabola()
		{
			for (int i = 0; i < _parabola.positionCount; i++)
			{
				_parabola.SetPosition(i, _lineZeroPos[i]);
			}
		}

		/// <summary>
		/// 포물선 위치를 반환
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