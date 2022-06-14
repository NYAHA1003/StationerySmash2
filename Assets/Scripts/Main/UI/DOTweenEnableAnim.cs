using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DOTweenEnableAnim : MonoBehaviour
{
	private Sequence _sequence;

	private void OnEnable()
	{
		_sequence ??= DOTween.Sequence()
			.SetAutoKill(false)
			.Append(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic));

		transform.localScale = Vector3.zero;
		_sequence.Restart();
	}

}
