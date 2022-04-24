using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill
{
    [System.Serializable]
    public class SkinData
    {
        public static Dictionary<CardNamingType, List<SkinData>> skinDictionary = new Dictionary<CardNamingType, List<SkinData>>();
        public int _skinId = 0;
        public Sprite _cardSprite;
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
    }
}
