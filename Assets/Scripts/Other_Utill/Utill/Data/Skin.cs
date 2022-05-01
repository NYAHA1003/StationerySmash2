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
        /// 스킨의 스프라이트를 등록한다.
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
