using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Main.Deck;
using Utill.Tool;
public class ExpComponent : MonoBehaviour, IUserData
{
	[SerializeField]
	private Image _expImage = null;
	[SerializeField]
	private TextMeshProUGUI _expText = null;
	[SerializeField]
	private TextMeshProUGUI _levelText = null;
	[SerializeField]
	private RectTransform _levelTextRect = null;

	private int _currentLevel = 0;
	private int _currentExp = 0;
	private int _previousLevel = 0;
	private int _previousExp = 0;
	private Sequence _textSequence = null;

	public void Awake()
	{
		SaveManager.Instance.SaveData.AddObserver(this);
		_previousLevel = SaveManager.Instance.SaveData.userSaveData._level;
		_previousExp = SaveManager.Instance.SaveData.userSaveData._nowExp;

		if (CheckExpOverLevelExp())
		{
			_previousLevel++;
			_previousExp = 0;
		}



		_textSequence = DOTween.Sequence().SetAutoKill(false).OnStart(() =>
		{
			_levelText.color = Color.white;
			_levelTextRect.localScale = new Vector3(1, 1, 1);
		})
			.Append(_levelTextRect.DOShakeAnchorPos(0.4f, 30))
			.Join(_levelTextRect.DOScale(2, 0.3f).SetEase(Ease.OutExpo)
			.OnComplete(() => _levelTextRect.DOScale(1, 0.3f).SetEase(Ease.InBack)))
			.Join(_levelText.DOColor(Color.red, 0.1f)
			.OnComplete(() => _levelText.DOColor(Color.yellow, 0.1f)
			.OnComplete(() => _levelText.DOColor(Color.blue, 0.1f)
			.OnComplete(() => _levelText.DOColor(Color.magenta, 0.1f)
			.OnComplete(() => _levelText.DOColor(Color.white, 0.1f))))));

		UpdateText();
	}

	public void Notify()
	{
		SetExpBar();
	}

	/// <summary>
	/// EXP바 수정
	/// </summary>
	public void SetExpBar()
	{
		_currentExp = UserSaveManagerSO.UserSaveData._nowExp;
		_currentLevel = UserSaveManagerSO.UserSaveData._level;

		StartCoroutine(UpExpBar());
	}

	/// <summary>
	/// 경험치바 증가
	/// </summary>
	/// <returns></returns>
	private IEnumerator UpExpBar()
	{
		if (_previousLevel < _currentLevel || _previousExp < _currentExp)
		{
			float interval = 0f;
			interval = 0.5f / ((_currentLevel - _previousLevel) * 100 + (_currentExp - _previousExp));
			while (_previousLevel < _currentLevel || _previousExp < _currentExp)
			{
				_previousExp++;
				if (CheckExpOverLevelExp())
				{
					_previousLevel++;
					_previousExp = 0;
					UpdateText();
					AnimationLevelUp();
					yield return new WaitForSeconds(interval * 3);
				}
				else
				{
					UpdateText();
				}

				yield return new WaitForSeconds(interval);
			}
		}
	}

	/// <summary>
	/// 업데이트 텍스트 수정
	/// </summary>
	private void UpdateText()
	{
		_levelText.text = _previousLevel.ToString();
		_expText.text = $"{_previousExp}/{(_previousLevel * 100)}";
		_expImage.fillAmount = (float)_previousExp / (_previousLevel * 100);
	}

	/// <summary>
	/// 과거 EXP가 레벨업할 EXP를 초과했는지 체크
	/// </summary>
	private bool CheckExpOverLevelExp()
	{
		if (_previousExp >= _previousLevel * 100)
		{
			return true;
		}
		return false;
	}

	/// <summary>
	/// 레벨업 텍스트 애니메이션
	/// </summary>
	private void AnimationLevelUp()
	{
		_textSequence.Restart();
	}

}
