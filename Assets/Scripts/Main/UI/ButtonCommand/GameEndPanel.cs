using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameEndPanel : MonoBehaviour
{
	[SerializeField]
	private Button _endButton;
	[SerializeField]
	private Button _playButton;
	[SerializeField]
	private GameObject _endPanel;

	public void Start()
	{
		_endButton.onClick.AddListener(() => EndGame());
		_playButton.onClick.AddListener(() => NoneSetting());
	}
	
	/// <summary>
	/// ���� ����â�� Ų��
	/// </summary>
	public void Setting()
	{
		if(!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
			_endPanel.SetActive(true);
		}
		else
		{
			gameObject.SetActive(false);
			_endPanel.SetActive(false);
		}
	}

	/// <summary>
	/// ���� ����â�� �ݴ´�
	/// </summary>
	public void NoneSetting()
	{
		gameObject.SetActive(false);
		_endPanel.SetActive(false);
	}

	/// <summary>
	/// ������ �����Ų��
	/// </summary>
	public void EndGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
	Application.Quit();
#endif
	}
}
