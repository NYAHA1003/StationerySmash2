using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileOptionComponent : MonoBehaviour
{
	//옵션 패널들
	[SerializeField]
	private GameObject _nameChangePanel; //이름 변경 패널
	[SerializeField]
	private GameObject _imageChangePanel; // 프로필 이미지 변경 패널
	[SerializeField]
	private GameObject _themeChangePanel; // 테마 변경 패널

	//옵션을 키는 버튼들
	[SerializeField]
	private Button _nameChangeButton; //이름 변경 버튼
	[SerializeField]
	private Button _imageChangeButton; //프로필 이미지 변경 버튼'
	[SerializeField]
	private Button _themeChangeButton; //테마 변경 버튼

	private void Start()
	{
		OnSetActiveNameChnage();
		_nameChangeButton.onClick.AddListener(() => OnSetActiveNameChnage());
		_imageChangeButton.onClick.AddListener(() => OnSetActiveImageChange());
		_themeChangeButton.onClick.AddListener(() => OnSetActiveThemeChange());
	}


	/// <summary>
	/// 별명 변경 패널 키기
	/// </summary>
	public void OnSetActiveNameChnage()
	{
		_nameChangePanel.SetActive(true);
		_imageChangePanel.SetActive(false);
		_themeChangePanel.SetActive(false);
	}

	/// <summary>
	/// 프로필 이미지 변경 패널 키기
	/// </summary>
	public void OnSetActiveImageChange()
	{
		_nameChangePanel.SetActive(false);
		_imageChangePanel.SetActive(true);
		_themeChangePanel.SetActive(false);
	}

	/// <summary>
	/// 테마 변경 패널 키기
	/// </summary>
	public void OnSetActiveThemeChange()
	{
		_nameChangePanel.SetActive(false);
		_imageChangePanel.SetActive(false);
		_themeChangePanel.SetActive(true);
	}



}
