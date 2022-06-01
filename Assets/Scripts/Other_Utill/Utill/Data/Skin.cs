using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Utill.Tool;

namespace Utill.Data
{
    public enum SkinType
    {
        SpriteNone = 0,
        PencilNormal = 100,
        SharpNormal = 200,
        EraserNormal = 300,
        PencilCaseNormal = 400,

        PencilHotSummer = 1100,
        SharpHotSummer = 1200,
        EraserHotSummer = 1300,
        GlueHotSummer = 1400,
        RulerHotSummer = 1500,

        PencilSewing = 2100,
        ScissorsSewing = 2200,

        SharpChristmas = 3100,
        GlueChristmas = 3200,
        PostItChristmas = 3300,
        MechaPencilLeadChristmas = 3400,

        EraserSweetDessert = 4100,
        RulerSweetDessert = 4200,
        CutterKnifeSweetDessert = 4300,
        PostItSweetDessert = 4400,
        MechaPencilLeadSweetDessert = 4500,
    }

    [System.Serializable]
    public class SkinData
    {
        public static Dictionary<SkinType, Sprite> _spriteDictionary = new Dictionary<SkinType, Sprite>();
        public static Dictionary<CardNamingType, List<SkinData>> _skinDictionary = new Dictionary<CardNamingType, List<SkinData>>();
        
        public SkinType _skinType = SkinType.SpriteNone;
        public EffectType _effectType = EffectType.Attack;
        
        /// <summary>
        /// 스킨 데이터를 딕셔너리에 추가
        /// </summary>
        public void AddSkinDataIntCardDictionary(CardNamingType cardNamingType)
        {
            if(!_skinDictionary.TryGetValue(cardNamingType, out var name))
            {
                _skinDictionary.Add(cardNamingType, new List<SkinData>());
            }
            _skinDictionary[cardNamingType].Add(this);
        }

        /// <summary>
        /// 해당 타입 카드의 스킨리스트 가져오기
        /// </summary>
        /// <param name="cardNamingType"></param>
        /// <returns></returns>
        public static List<SkinData> GetSkinDataList(CardNamingType cardNamingType)
        {
            _skinDictionary.TryGetValue(cardNamingType, out var skinList);
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
                return sprite;

            }
            else
			{
                return null;
			}
        }

        /// <summary>
        /// 정적으로 스킨의 스프라이트를 등록한다.
        /// </summary>
        /// <param name="skinType"></param>
        public static void SetSkin(SkinType skinType)
        {
            if (_spriteDictionary.TryGetValue(skinType, out var data))
            {
                return;
            }
            else
            {
                string name = System.Enum.GetName(typeof(SkinType), skinType);
                AddressableTool.GetAddressableAssetDicAsync<SkinType, Sprite>(AddSpriteDictionary, skinType, name);
            }
        }

        /// <summary>
        /// 딕셔너리에 키와 스프라이트를 추가한다
        /// </summary>
        /// <param name="skinType"></param>
        /// <param name="sprite"></param>
        private static void AddSpriteDictionary(SkinType skinType, Sprite sprite)
        {
            if (_spriteDictionary.TryGetValue(skinType, out var data))
            {
                return;
            }
            else
			{
                _spriteDictionary.Add(skinType, sprite);
			}
        }
    }
}
