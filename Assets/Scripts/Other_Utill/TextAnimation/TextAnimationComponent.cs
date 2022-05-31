using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TextAnimationComponent : MonoBehaviour
{
	public enum AnimationDirType
	{
		Up = 0,
		Down,
		Left,
		Right
	}

	public enum AnimationType
	{
		Normal = 0,
		BigToSmall,
	}

	[SerializeField]
	private GameObject _textPrefeb;

	[SerializeField]
	private Transform _textParent;

	/// <summary>
	/// 텍스트 애니메이션을 설정한다
	/// </summary>
	/// <param name="text"></param>
	/// <param name="animationDirType"></param>
	public void SetText(string text, Vector2 startPos, float duration, AnimationDirType animationDirType, AnimationType animationType)
	{
		GameObject textobj = PoolTextObj();
		textobj.GetComponent<RectTransform>().anchoredPosition = startPos;
		TextMeshProUGUI tmpro = textobj.GetComponent<TextMeshProUGUI>();
		Vector2 endPos = startPos;
		tmpro.text = text;

		switch (animationDirType)
		{
			case AnimationDirType.Up:
				endPos.y += 50;
				break;
			case AnimationDirType.Down:
				endPos.y -= 50;
				break;
			case AnimationDirType.Left:
				endPos.x -= 50;
				break;
			case AnimationDirType.Right:
				endPos.x += 50;
				break;
		}
		MoveAnimation(textobj, animationType, endPos, duration);
	}

	/// <summary>
	/// 텍스트 오브젝트를 풀링해서 가져온다
	/// </summary>
	/// <returns></returns>
	private GameObject PoolTextObj()
	{
		GameObject returnObj = null;
		if (_textParent.childCount > 0)
		{
			returnObj = _textParent.GetChild(0).gameObject;
			returnObj.SetActive(true);
			returnObj.transform.SetParent(gameObject.transform);
			return returnObj;
		}
		else
		{
			returnObj = Instantiate(_textPrefeb, _textParent);
			return returnObj;
		}
	}

	/// <summary>
	/// 텍스트 움직임 애니메이션
	/// </summary>
	/// <param name="textobj"></param>
	/// <param name="vector"></param>
	/// <param name="duration"></param>
	private void MoveAnimation(GameObject textobj, AnimationType animationType, Vector2 vector, float duration)
	{
		RectTransform rect = textobj.GetComponent<RectTransform>();

		switch(animationType)
		{
			case AnimationType.Normal:
				break;
			case AnimationType.BigToSmall:
				BigToSmallAnimation(rect, duration);
				break;
		}

		rect.DOAnchorPos(vector, duration)
			.OnComplete(() =>
			{
				textobj.SetActive(false);
				textobj.transform.SetParent(_textParent);
			});
	}

	private void BigToSmallAnimation(RectTransform rect, float duration)
	{
		rect.localScale = Vector2.zero;
		rect.DOScale(1f, duration - 0.2f)
			.OnComplete(() =>
			{
				rect.DOScale(0f, 0.2f).SetEase(Ease.InExpo);
			}).SetEase(Ease.OutExpo);
	}
}
