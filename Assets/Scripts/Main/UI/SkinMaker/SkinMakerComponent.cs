using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utill.Data;
using Utill.Tool;
using Main.Deck;

namespace Main.Skin
{
	/// <summary>
	/// ��Ų ���� ���� ������Ʈ
	/// </summary>
	public class SkinMakerComponent : MonoBehaviour
	{
		//�ν����� ����
		[SerializeField]
		private SkinMakeDataSO _skinMakeDataSO;
		[SerializeField]
		private Transform _skinParent = null; // ��Ų ���� ��ư ���� �θ�
		[SerializeField]
		private GameObject _skinButtonPrefeb = null; //��Ų ���� ��ư ������
		[SerializeField]
		private Image _currentMakeSkinImage = null; //���� ������ ��Ų �̹��� 
		[SerializeField]
		private Transform _materialParent = null; //�ʿ� ��� �ڽ� ���� �θ�
		[SerializeField]
		private Button _skinMakeButton = null; // ��Ų ���� ��ư

		//����
		public List<SkinMakeData> _skinMakeDatas = new List<SkinMakeData>();
		private bool _isCanCrate = false;

		//���� ���� 
		private SkinMakeData _selectSkinData = null;

		private void Start()
		{
			SetSkinMakeDatas(0);

			//��Ų ���۹�ư�� �Լ� �߰�
			_skinMakeButton.onClick.AddListener(() => OnCreate());

		}

		/// <summary>
		/// �ε����� ���� ��Ų ���� ����Ʈ���� �����Ѵ�
		/// </summary>
		/// <param name="index"></param>
		public void SetSkinMakeDatas(int index)
		{
			_skinMakeDatas = null;
			_skinMakeDatas = _skinMakeDataSO.skinMakeDataLists[index].skinMakeDatas;
			PullSkinButtons();
		}

		/// <summary>
		/// ��Ų ���� �ݹ� �Լ�
		/// </summary>
		public void OnCreate()
		{
			//��Ų�� ������ �� �ִٸ� �����Ѵ�
			if (_isCanCrate)
			{
				//��� �Ҹ�
				for (int i = 0; i < _selectSkinData._needMaterial.Count; i++)
				{
					MaterialData materialData = _selectSkinData._needMaterial[i];
					UserSaveManagerSO.UserSaveData._materialDatas.Find(x => x._materialType == materialData._materialType)._count -= materialData._count;
				}
				UserSaveManagerSO.UserSaveData._haveSkinList.Add(_selectSkinData.skinType);
				SetMaterialBoxs(_selectSkinData);
				PullSkinButtons();
			}
			else
			{
				Debug.Log("��ᰡ �����ϰų� �̹� ���۵�");
				//��ᰡ �����ϰų� �̹� �����߽��ϴ�
			}
		}

		/// <summary>
		/// ��Ų �����͸� ������ �װſ� �°� ��Ų ���� ��ư�� �ʿ��� ������ ������
		/// </summary>
		/// <param name="skinMakeData"></param>
		public void SetSkinMakeButtonAndBoxs(SkinMakeData skinMakeData, bool isAlreadyHave)
		{
			//���� ��Ų �����͸� �ٲٰ� ��������Ʈ�� �������ش�
			_selectSkinData = skinMakeData;
			_currentMakeSkinImage.sprite = skinMakeData.sprite;

			//��� ����
			SetMaterialBoxs(skinMakeData);

			//������ �� �ִ��� ���� üũ
			if (!isAlreadyHave && CheckCanCreate(skinMakeData))
			{
				_isCanCrate = true;
			}
			else
			{
				_isCanCrate = false;
			}
		}

		/// <summary>
		/// �ʿ� �����۵� ��Ÿ����.
		/// </summary>
		/// <param name="skinMakeData"></param>
		public void SetMaterialBoxs(SkinMakeData skinMakeData)
		{
			//�ʿ� ��� ��ŭ ��� �ڽ����� ���ش�.
			for (int i = 0; i < _materialParent.childCount; i++)
			{
				if (i < skinMakeData._needMaterial.Count)
				{
					//��� �ڽ� ���ֱ�
					_materialParent.GetChild(i).gameObject.SetActive(true);

					//��� �����Ͱ� �ִ��� Ž��
					MaterialData materialData = UserSaveManagerSO.UserSaveData._materialDatas.Find(x => x._materialType == skinMakeData._needMaterial[i]._materialType);
					int haveMaterialCount = 0;

					//�κ��丮�� ��ᰡ �ִٸ� �װ��� ������ �ٲ۴�
					if (materialData != null)
					{
						haveMaterialCount = materialData._count;
					}

					//������ �ڽ��� ���� ���� �������� ������ ������.
					_materialParent.GetChild(i).GetComponent<MaterialBox>().SetMaterial(skinMakeData._needMaterial[i], haveMaterialCount);
				}
				else
				{
					//��� ������ �ʿ��� ��� �������� ������ ���ش�.
					_materialParent.GetChild(i).gameObject.SetActive(false);
				}
			}
		}

		/// <summary>
		/// ��Ḧ �� ��Ҵ��� üũ�Ѵ�
		/// </summary>
		private bool CheckCanCreate(SkinMakeData skinMakeData)
		{
			for (int i = 0; i < skinMakeData._needMaterial.Count; i++)
			{
				MaterialData haveMaterialData = UserSaveManagerSO.UserSaveData._materialDatas.Find(x => x._materialType == skinMakeData._needMaterial[i]._materialType);
				if (haveMaterialData != null)
				{
					if (haveMaterialData._count < skinMakeData._needMaterial[i]._count)
					{
						//�ʿ��� �������� �ʿ� ������ŭ ������ False�� ��ȯ
						return false;
					}
				}
				else
				{
					//�ʿ� �������� ������ Flase�� ��ȯ
					return false;
				}
			}
			//���� ������ ������ True�� ��ȯ
			return true;
		}



		/// <summary>
		/// ��Ų ��ư�� Ǯ��
		/// </summary>
		private void PullSkinButtons()
		{
			for (int i = 0; i < _skinParent.childCount; i++)
			{
				_skinParent.GetChild(i).gameObject.SetActive(false);
			}

			//��Ų �����͵��� �ҷ��� ��Ų ��ư���� ����.
			for (int i = 0; i < _skinMakeDatas.Count; i++)
			{
				SkinDataButton skinButton = null;
				if (_skinParent.childCount > i)
				{
					skinButton = _skinParent.GetChild(i).GetComponent<SkinDataButton>();
				}
				else
				{
					skinButton = Instantiate(_skinButtonPrefeb, _skinParent).GetComponent<SkinDataButton>();
				}
				skinButton.SetSkinData(_skinMakeDatas[i], UserSaveManagerSO.UserSaveData._haveSkinList.Contains(_skinMakeDatas[i].skinType), this);
				skinButton.gameObject.SetActive(true);

			}
		}
	}
}