using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MessageComponent : MonoBehaviour
{
	[SerializeField]
	private PencilCaseUnit _playerPencilCaseUnit;
	[SerializeField]
	private PencilCaseUnit _enemyPencilCaseUnit;
	[SerializeField]
	private TextMeshPro _textMeshPro = null;
	[SerializeField]
	private Transform _messageBoxTrm = null;
	[SerializeField]
	private GameObject _messagePanel = null;
	[SerializeField]
	private Button _messageSetButton = null; 
	[SerializeField]
	private Button _messageButton1 = null;
	[SerializeField]
	private Button _messageButton2 = null;
	[SerializeField]
	private Button _messageButton3 = null;
	[SerializeField]
	private Button _messageButton4 = null;

	public void Start()
	{
		_messageSetButton.onClick.AddListener(() => OnSetMessagePanel());
		_messageButton1.onClick.AddListener(() => OnMessage(MessageType.Hey));
		_messageButton2.onClick.AddListener(() => OnMessage(MessageType.Hi));
		_messageButton3.onClick.AddListener(() => OnMessage(MessageType.Winner));
		_messageButton4.onClick.AddListener(() => OnMessage(MessageType.Loser));

		Vector3 pos = _playerPencilCaseUnit.transform.position;
		pos.x += 0.8f;
		pos.y += 0.5f;
		_messageBoxTrm.position = pos;
	}

	/// <summary>
	/// 패널 설정
	/// </summary>
	public void OnSetMessagePanel()
	{
		_messagePanel.SetActive(true);
	}

	/// <summary>
	/// 메시지 타입에 따른 메시지박스를 나타낸다
	/// </summary>
	/// <param name="messageType"></param>
	public void OnMessage(MessageType messageType)
	{
		_textMeshPro.text = System.Enum.GetName(typeof(MessageType), messageType);
		_messageBoxTrm.DOKill();
		_messageBoxTrm.DOScaleY(1, 0.3f)
			.OnComplete(() =>
			{
				_messageBoxTrm.DOScaleY(0, 0.6f).SetDelay(2);
			});

		_messagePanel.SetActive(false);
	}

}
