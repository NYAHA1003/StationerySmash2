using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill;

/// <summary>
/// 버튼 커맨드에 사용되는 버튼 Enum 
/// </summary>
/// 
namespace Utill
{
	[System.Serializable]
	enum ButtonType
	{
		deck,
		setting,
		cardDescription,
	}

}

// 활성화 버튼 추가할시 ButtonType추가,ActiveCommand로 변수 만든후 Init에서 생성하고 allBtns에 넣어준다
// ButtonType 순서대로 allBtns에 Add해주어야 함 
class ButtonComponent : MonoBehaviour
{

	private Stack<AbstractBtn> activeButtons = new Stack<AbstractBtn>(); // 활성화 시킨 버튼들을 넣어둘 스택
	private List<AbstractBtn> allBtns = new List<AbstractBtn>(); // 기본적으로 세팅해두는 모든 버튼커맨드들 

	[Header("클릭할 버튼들")]
	[SerializeField]
	private Button[] clickBtns;
	[Header("캔슬 버튼들")]
	[SerializeField]
	private Button[] cancelBtns;
	[Header("활성화할 패널들")]
	[SerializeField]
	private GameObject[] activePanels;


	private ActiveCommand deckButtonCommand;
	private ActiveCommand cardDescriptionButtonCommand;
	private ActiveCommand SettingButtonCommand;

	private void Awake()
	{
		EventManager.StartListening(EventsType.DeckSetting, (x) => OnActiveBtn((ButtonType)x)); 
		//덱에 있는 카드 클릭시 카드설명이 뜸, DeckSetting에서 카드 만들어주면서 AddListener로 EventTrigger로 등록해줌     
	}
	private void Start()
	{
		Initialized();
	}

	/// <summary>
	///  사용할 버튼 커맨드들 초기화, 
	/// </summary>
	private void Initialized()
	{
		deckButtonCommand = new ActiveCommand();
		SettingButtonCommand = new ActiveCommand();
		cardDescriptionButtonCommand = new ActiveCommand();

		allBtns.Add(deckButtonCommand);
		allBtns.Add(SettingButtonCommand);
		allBtns.Add(cardDescriptionButtonCommand );
		AddListner();

	}

	/// <summary>
	/// 클릭할 버튼들, 취소패널들 onClick함수 넣어줌
	/// </summary>
	private void AddListner()
	{
		for (int i = 0; i < clickBtns.Length; i++)
		{
			int index = i;
			clickBtns[i].onClick.AddListener(() => OnActiveBtn((ButtonType)index));
		}

		for (int i = 0; i < cancelBtns.Length; i++)
		{
			cancelBtns[i].onClick.AddListener(() => OnUndoBtn());
		}
	}
	/// <summary>
	/// 패널 활성화시킬 때 발동되는 버튼함수 
	/// </summary>
	/// <param name="buttonType"></param>
	public void OnActiveBtn(ButtonType buttonType)
	{
		activeButtons.Push(allBtns[(int)buttonType]);
		activeButtons.Peek().Execute(activePanels[(int)buttonType]);
	}

	/// <summary>
	/// 패널 비활성화시킬 때 발동되는 버튼함수 
	/// </summary>
	public void OnUndoBtn()
	{
		activeButtons.Pop().Undo();
	}

	/// <summary>
	/// 활성화되어 있는 모든 패널 비활성화
	/// </summary>
	public void CloseAllPanels()
	{
		for (int i = 0; i < activeButtons.Count; i++)
		{
			activeButtons.Pop().Undo();
		}
	}
}