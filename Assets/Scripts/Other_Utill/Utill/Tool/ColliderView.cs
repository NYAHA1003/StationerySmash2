using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;



namespace Utill.Tool
{
	public class ColliderView : MonoBehaviour
	{
		public CollideData collideData;

		private void OnDrawGizmos()
		{
			if (collideData.originpoints.Length == 4)
			{
				collideData.SetMultiple(1);
				var vec = collideData.GetPoint(transform.position);
				DrawLine(vec[0], vec[1]);
				DrawLine(vec[0], vec[2]);
				DrawLine(vec[1], vec[3]);
				DrawLine(vec[2], vec[3]);
			}
		}

		private void DrawLine(Vector2 point1, Vector2 point2)
		{
			Vector2 fromPosition = point1;
			Vector2 toPosition = point2;
			Vector2 direction = toPosition - fromPosition;
			Debug.DrawRay(point1, direction, Color.red);
		}
	}

}