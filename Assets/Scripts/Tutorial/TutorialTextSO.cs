using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Battle.Tutorial
{
    [CreateAssetMenu(menuName = "Scriptable Object/TutorialText")]
    public class TutorialTextSO : ScriptableObject
    {
        public List<TextData> _textDatas;
    }

    [System.Serializable]
    public class TextData
    {
        public List<string> _tutorialText;
    }
}
