using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill; 
namespace Utill
{
    public enum LanguageType
    {
        Ko,
        En,
        Ja,
        zh_CN,
        zh_TW,
        ru,
        fr,
        de,
        it,
        id,
        vi
    }
}

[System.Serializable]
public class User 
{
    public string userName;
    public Sprite profileImage;
    public bool isFirstPlay;
    public LanguageType language;
}
