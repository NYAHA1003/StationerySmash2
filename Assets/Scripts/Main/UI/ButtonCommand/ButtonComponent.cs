using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;
using Main.Event;

namespace Main.Buttons
{
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

		//어떤 버튼커맨드들을 사용하는지 직관적으로 알기위함 
		private ActiveCommand deckButtonCommand;
		private ActiveCommand SettingButtonCommand;
		private ActiveCommand collectionButtonCommand;
		private ActiveCommand skinButtonCommand;
		private ActiveCommand stageButtonCommand;
		private ActiveCommand cardDescriptionButtonCommand;

		private int unSetCount = 1; //이 스크립트가 아닌 다른 스크립트에서 버튼 생성및 함수등록해준 커맨드수
		private List<ActiveCommand> buttonCommands = new List<ActiveCommand>();
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
			for (int i = 0; i < clickBtns.Length + unSetCount; i++)
			{
				buttonCommands.Add(new ActiveCommand());
				allBtns.Add(buttonCommands[i]);
			}
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

			clickBtns[(int)ButtonType.credit].onClick.AddListener(() => EventManager.TriggerEvent(EventsType.MoveCredit));
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
}