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

		public Stack<AbstractBtn> activeButtonsStack = new Stack<AbstractBtn>(); // Ȱ��ȭ ��Ų ��ư���� �־�� ����
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
		private ActiveCommand chapterButtonCommand;
		private ActiveCommand profileButtonCommand;
		private ActiveCommand cardDescriptionButtonCommand;	

		private int unSetCount = 2; //�� ��ũ��Ʈ�� �ƴ� �ٸ� ��ũ��Ʈ���� ��ư ������ �Լ�������� Ŀ�ǵ��
		private List<ActiveCommand> buttonCommands = new List<ActiveCommand>();
		private void Awake()
		{
			EventManager.Instance.StartListening(EventsType.CloaseAllPn, CloseAllPanels); 
			EventManager.Instance.StartListening(EventsType.ActiveButtonComponent, (x) => OnActiveBtn((ButtonType)x));
			EventManager.Instance.StartListening(EventsType.UndoStack, OnUndoBtn);
			//���� �ִ� ī�� Ŭ���� ī�弳���� ��, DeckSetting���� ī�� ������ָ鼭 AddListener�� EventTrigger�� �������     
			
		}

		/// <summary>
		/// �г� Ȱ��ȭ��ų �� �ߵ��Ǵ� ��ư�Լ� 
		/// </summary>
		/// <param name="buttonType"></param>
		public void OnActiveBtn(ButtonType buttonType)
		{
			activeButtonsStack.Push(new ActiveCommand());
			activeButtonsStack.Peek().Execute(activePanels[(int)buttonType]);
		}

		/// <summary>
		/// �г� ��Ȱ��ȭ��ų �� �ߵ��Ǵ� ��ư�Լ� 
		/// </summary>
		public void OnUndoBtn()
		{
			activeButtonsStack.Pop().Undo();
		}

		/// <summary>
		/// Ȱ��ȭ�Ǿ� �ִ� ��� �г� ��Ȱ��ȭ
		/// </summary>
		public void CloseAllPanels()
		{
			int stackCount = activeButtonsStack.Count; 
			for (int i = 0; i < stackCount; i++)
			{
				activeButtonsStack.Pop().Undo();
			}
		}

	}
}