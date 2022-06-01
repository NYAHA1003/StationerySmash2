using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

namespace Utill.Data
{
    public enum ThemeSkinType
    {
        Normal = 0,

    }

    public enum ThemeUIType
    {
        CostUp = 0,
        PCButton,
        StButton,
        ThrowBar,
        ThemePreview,
    }
    public static class ThemeSkin
    {
        public static Dictionary<string, Sprite> _themeSpriteDictionary = new Dictionary<string, Sprite>();

        /// <summary>
        /// 스킨의 스프라이트를 반환한다.
        /// </summary>
        /// <param name="skinType"></param>
        /// <returns></returns>
        public static Sprite GetSkin(ThemeSkinType themeSkinType, ThemeUIType themeUIType)
        {
            string name = System.Enum.GetName(typeof(ThemeSkinType), themeSkinType) + System.Enum.GetName(typeof(ThemeUIType), themeUIType);
            Sprite sprite = null;
            if (_themeSpriteDictionary.TryGetValue(name, out sprite))
            {
                return _themeSpriteDictionary[name];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 정적으로 테마 스킨의 스프라이트를 등록한다.
        /// </summary>
        /// <param name="skinType"></param>
        public static async Task SetSkinStaticAsync(ThemeSkinType themeSkinType, ThemeUIType themeUIType)
        {
            string name = System.Enum.GetName(typeof(ThemeSkinType), themeSkinType) + System.Enum.GetName(typeof(ThemeUIType), themeUIType);
            if (_themeSpriteDictionary.TryGetValue(name, out var data))
            {
                return;
            }
            else
            {
                var handle = Addressables.LoadAssetAsync<Sprite>(name);
                await handle.Task;
                _themeSpriteDictionary.Add(name, handle.Result);
            }
        }
    }

}