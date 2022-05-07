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
        PencilCaseNormal = 400,
    }
    [System.Serializable]
    public class SkinData
    {
        public static Dictionary<SkinType, Sprite> _spriteDictionary = new Dictionary<SkinType, Sprite>();
        public static Dictionary<CardNamingType, List<SkinData>> _skinDictionary = new Dictionary<CardNamingType, List<SkinData>>();
        
        public SkinType _skinType = SkinType.SpriteNone;
        public EffectType _effectType = EffectType.Attack;
        
        /// <summary>
        /// ��Ų �����͸� ��ųʸ��� �߰�
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
        /// �ش� Ÿ�� ī���� ��Ų����Ʈ ��������
        /// </summary>
        /// <param name="cardNamingType"></param>
        /// <returns></returns>
        public static List<SkinData> GetSkinDataList(CardNamingType cardNamingType)
        {
            _skinDictionary.TryGetValue(cardNamingType, out var skinList);
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
        /// �������� ��Ų�� ��������Ʈ�� ����Ѵ�.
        /// </summary>
        /// <param name="skinType"></param>
        public static async Task SetSkinStaticAsync(SkinType skinType)
        {
            if (_spriteDictionary.TryGetValue(skinType, out var data))
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
