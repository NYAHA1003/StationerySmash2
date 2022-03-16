using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    [SerializeField]
    private GameObject selectArrow;
    [SerializeField]
    private GameObject IndexPanel_GameObj;
    [SerializeField]
    private GameObject StoryPanel_GameObj;

    private bool isCheckChapter = false;
    private bool isCheckStoryMode = false;
    private bool isCheckEventMode = false;

    public void StoryMode_Select()
    {
        isCheckStoryMode = !isCheckStoryMode;
        StoryPanel_GameObj.SetActive(isCheckStoryMode);
    }
    public void EventMode_Select()
    {
        isCheckEventMode = !isCheckEventMode;
        IndexPanel_GameObj.SetActive(isCheckChapter);
    }
    public void Chapter_Select()
    {
        isCheckChapter = !isCheckChapter;
        IndexPanel_GameObj.SetActive(isCheckChapter);
    }
}