using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileOptionComponent : MonoBehaviour
{
	//�ɼ� �гε�
	[SerializeField]
	private GameObject _nameChangePanel; //�̸� ���� �г�
	[SerializeField]
	private GameObject _imageChangePanel; // ������ �̹��� ���� �г�
	[SerializeField]
	private GameObject _themeChangePanel; // �׸� ���� �г�

	//�ɼ��� Ű�� ��ư��
	[SerializeField]
	private Button _nameChangeButton; //�̸� ���� ��ư
	[SerializeField]
	private Button _imageChangeButton; //������ �̹��� ���� ��ư'
	[SerializeField]
	private Button _themeChangeButton; //�׸� ���� ��ư

	private void Start()
	{
		OnSetActiveNameChnage();
		_nameChangeButton.onClick.AddListener(() => OnSetActiveNameChnage());
		_imageChangeButton.onClick.AddListener(() => OnSetActiveImageChange());
		_themeChangeButton.onClick.AddListener(() => OnSetActiveThemeChange());
	}


	/// <summary>
	/// ���� ���� �г� Ű��
	/// </summary>
	public void OnSetActiveNameChnage()
	{
		_nameChangePanel.SetActive(true);
		_imageChangePanel.SetActive(false);
		_themeChangePanel.SetActive(false);
	}

	/// <summary>
	/// ������ �̹��� ���� �г� Ű��
	/// </summary>
	public void OnSetActiveImageChange()
	{
		_nameChangePanel.SetActive(false);
		_imageChangePanel.SetActive(true);
		_themeChangePanel.SetActive(false);
	}

	/// <summary>
	/// �׸� ���� �г� Ű��
	/// </summary>
	public void OnSetActiveThemeChange()
	{
		_nameChangePanel.SetActive(false);
		_imageChangePanel.SetActive(false);
		_themeChangePanel.SetActive(true);
	}



}
