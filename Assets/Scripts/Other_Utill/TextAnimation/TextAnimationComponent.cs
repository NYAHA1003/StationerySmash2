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

	[SerializeField]
	private GameObject _textPrefeb;

	[SerializeField]
	private Transform _textParent;

	/// <summary>
	/// �ؽ�Ʈ �ִϸ��̼��� �����Ѵ�
	/// </summary>
	/// <param name="text"></param>
	/// <param name="animationDirType"></param>
	public void SetText(string text, Vector2 startPos, float duration, AnimationDirType animationDirType)
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
		MoveAnimation(textobj, endPos, duration);
	}

	/// <summary>
	/// �ؽ�Ʈ ������Ʈ�� Ǯ���ؼ� �����´�
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
	/// �ؽ�Ʈ ������ �ִϸ��̼�
	/// </summary>
	/// <param name="textobj"></param>
	/// <param name="vector"></param>
	/// <param name="duration"></param>
	private void MoveAnimation(GameObject textobj, Vector2 vector, float duration)
	{
		RectTransform rect = textobj.GetComponent<RectTransform>();
		rect.DOAnchorPos(vector, duration)
			.OnComplete(() =>
			{
				textobj.SetActive(false);
				textobj.transform.SetParent(_textParent);
			});
	}
}
