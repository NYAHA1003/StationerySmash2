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
        /// 스킨 데이터를 딕셔너리에 추가
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
        /// 해당 타입 카드의 스킨리스트 가져오기
        /// </summary>
        /// <param name="cardNamingType"></param>
        /// <returns></returns>
        public static List<SkinData> GetSkinDataList(CardNamingType cardNamingType)
        {
            skinDictionary.TryGetValue(cardNamingType, out var skinList);
            return skinList;
        }

        /// <summary>
        /// 스킨의 스프라이트를 반환한다.
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
        /// 스킨 타입의 스프라이트를 등록한다.
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
