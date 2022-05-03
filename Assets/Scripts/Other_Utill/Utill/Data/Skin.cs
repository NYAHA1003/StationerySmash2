using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

namespace Utill.Data
{
    public enum SkinType
    {
        SpriteNone = 0,
        PencilNormal = 100,
        SharpNormal = 200,
        EraserNormal = 300,
    }
    [System.Serializable]
    public class SkinData
    {
        public static Dictionary<SkinType, Sprite> _spriteDictionary = new Dictionary<SkinType, Sprite>();
        public static Dictionary<CardNamingType, List<SkinData>> skinDictionary = new Dictionary<CardNamingType, List<SkinData>>();
        public SkinType _skinType = SkinType.SpriteNone;
        public EffectType _effectType = EffectType.Attack;
        public CardNamingType _cardNamingType = CardNamingType.None;

        /// <summary>
        /// ��Ų Ÿ���� ��Ų�׸��� �߰�
        /// </summary>
        public static void AddSkinTypeInSkinTheme()
		{

		}
        
        /// <summary>
        /// ��Ų �����͸� ��ųʸ��� �߰�
        /// </summary>
        public void AddSkinData()
        {
            if(!skinDictionary.TryGetValue(_cardNamingType, out var name))
            {
                skinDictionary.Add(_cardNamingType, new List<SkinData>());
            }
            skinDictionary[_cardNamingType].Add(this);
        }

        /// <summary>
        /// �ش� Ÿ�� ī���� ��Ų����Ʈ ��������
        /// </summary>
        /// <param name="cardNamingType"></param>
        /// <returns></returns>
        public static List<SkinData> GetSkinDataList(CardNamingType cardNamingType)
        {
            skinDictionary.TryGetValue(cardNamingType, out var skinList);
            return skinList;
        }

        /// <summary>
        /// ��Ų�� ��������Ʈ�� ��ȯ�Ѵ�.
        /// </summary>
        /// <param name="skinType"></param>
        /// <returns></returns>
        public static Sprite GetSkin(SkinType skinType)
        {
            Sprite sprite = null;
            if(_spriteDictionary.TryGetValue(skinType, out sprite))
			{
                return _spriteDictionary[skinType];
			}
            else
			{
                return null;
			}
        }

        /// <summary>
        /// ��Ų�� ��������Ʈ�� ����Ѵ�.
        /// </summary>
        /// <param name="skinType"></param>
        public async Task SetSkin(SkinType skinType)
        {
            if(_spriteDictionary.TryGetValue(skinType, out var data))
            {
                return;
            }
            else
            {
                string name = System.Enum.GetName(typeof(SkinType), skinType);
                var handle = Addressables.LoadAssetAsync<Sprite>(name);
                await handle.Task;
                _spriteDictionary.Add(skinType, handle.Result);
            }
        }
    }
}
