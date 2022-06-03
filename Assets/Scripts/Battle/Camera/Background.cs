using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
	[SerializeField]
	private Transform _cameraTrm = null;
	[SerializeField]
	private float _lerp = 0.1f;

	private Vector2 _originPos = Vector2.zero;

	private void Start()
	{
		_originPos = transform.position;
	}

	private void LateUpdate()
	{
		Vector2 moveVector = transform.position;
		moveVector = Vector2.Lerp(_originPos, _cameraTrm.position, _lerp);
		moveVector.y = transform.position.y;
		transform.position = moveVector;
	}
}
