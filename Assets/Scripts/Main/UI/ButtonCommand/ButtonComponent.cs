using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;
using Main.Event;

namespace Main.Buttons
{
	// Ȱ��ȭ ��ư �߰��ҽ� ButtonType�߰�,ActiveCommand�� ���� ������ Init���� �����ϰ� allBtns�� �־��ش�
	// ButtonType ������� allBtns�� Add���־�� �� 
	class ButtonComponent : MonoBehaviour
	{

		private Stack<AbstractBtn> activeButtons = new Stack<AbstractBtn>(); // Ȱ��ȭ ��Ų ��ư���� �־�� ����
		private List<AbstractBtn> allBtns = new List<AbstractBtn>(); // �⺻������ �����صδ� ��� ��ưĿ�ǵ�� 

		[Header("Ŭ���� ��ư��")]
		[SerializeField]
		private Button[] clickBtns;
		[Header("ĵ�� ��ư��")]
		[SerializeField]
		private Button[] cancelBtns;
		[Header("Ȱ��ȭ�� �гε�")]
		[SerializeField]
		private GameObject[] activePanels;

		//� ��ưĿ�ǵ���� ����ϴ��� ���������� �˱����� 
		private ActiveCommand deckButtonCommand;
		private ActiveCommand SettingButtonCommand;
		private ActiveCommand collectionButtonCommand;
		private ActiveCommand skinButtonCommand;
		private ActiveCommand stageButtonCommand;
		private ActiveCommand cardDescriptionButtonCommand;

		private int unSetCount = 1; //�� ��ũ��Ʈ�� �ƴ� �ٸ� ��ũ��Ʈ���� ��ư ������ �Լ�������� Ŀ�ǵ��
		private List<ActiveCommand> buttonCommands = new List<ActiveCommand>();
		private void Awake()
		{
			EventManager.StartListening(EventsType.DeckSetting, (x) => OnActiveBtn((ButtonType)x));
			//���� �ִ� ī�� Ŭ���� ī�弳���� ��, DeckSetting���� ī�� ������ָ鼭 AddListener�� EventTrigger�� �������     
		}
		private void Start()
		{
			Initialized();
		}

		/// <summary>
		///  ����� ��ư Ŀ�ǵ�� �ʱ�ȭ, 
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
		/// Ŭ���� ��ư��, ����гε� onClick�Լ� �־���
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
		/// �г� Ȱ��ȭ��ų �� �ߵ��Ǵ� ��ư�Լ� 
		/// </summary>
		/// <param name="buttonType"></param>
		public void OnActiveBtn(ButtonType buttonType)
		{
			activeButtons.Push(allBtns[(int)buttonType]);
			activeButtons.Peek().Execute(activePanels[(int)buttonType]);
		}

		/// <summary>
		/// �г� ��Ȱ��ȭ��ų �� �ߵ��Ǵ� ��ư�Լ� 
		/// </summary>
		public void OnUndoBtn()
		{
			activeButtons.Pop().Undo();
		}

		/// <summary>
		/// Ȱ��ȭ�Ǿ� �ִ� ��� �г� ��Ȱ��ȭ
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