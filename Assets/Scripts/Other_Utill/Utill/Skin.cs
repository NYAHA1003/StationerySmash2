using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

namespace Utill
{
    public enum SkinType
    {
        None = 0,
        PencilNormal = 100,
        SharpNormal = 200,
        EraserNormal = 300,
    }

    [System.Serializable]
    public class SkinData
    {
        public static Dictionary<SkinType, Sprite> _spriteDictionary = new Dictionary<SkinType, Sprite>();
        public static Dictionary<CardNamingType, List<SkinData>> skinDictionary = new Dictionary<CardNamingType, List<SkinData>>();
        public SkinType _skinType = SkinType.None;
        public EffectType _effectType = EffectType.Attack;
        public CardNamingType _cardNamingType = CardNamingType.None;

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
        public static async Task<Sprite> GetSkin(SkinType skinType)
        {
            if(_spriteDictionary.TryGetValue(skinType, out var sprite))
            {
                return sprite;
            }
            else
            {
                await AwitLoadAssetAsync(skinType);
                return _spriteDictionary[skinType];
            }
        }
        /// <summary>
        /// ��Ų Ÿ���� ��������Ʈ�� ����Ѵ�.
        /// </summary>
        /// <param name="skinType"></param>
        /// <returns></returns>
        public static async Task AwitLoadAssetAsync(SkinType skinType)
        {
            AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(skinType);
            await handle.Task;
            _spriteDictionary[skinType] = handle.Result;
        }
    }
}
