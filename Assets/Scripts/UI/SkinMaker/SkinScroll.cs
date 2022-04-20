using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkinScroll : AgentScroll
{
    [SerializeField]
    private GameObject stressIconParent;
    [SerializeField]
    private int currentSize = 3; 
    private List<Image> stressIcons = new List<Image>();

    private Vector3 orginIconScale = Vector3.one;

    protected override void ChildAwake()
    {
        SetSize();
    }
    protected override void ChildStart()
    {
        GetStressIcon();
        StressIcon();
    }
    private void SetSize()
    {
        Size = currentSize; 
    }
    private void GetStressIcon()
    {
        for (int i = 0; i < Size; i++)
        {
            stressIcons.Add(stressIconParent.transform.GetChild(i).GetComponent<Image>());
        }
    }

    private void StressIcon()
    {
        for(int i = 0; i < stressIcons.Count; i++)
        {
            if(targetIndex  == i)
            {
                stressIcons[i].transform.localScale = orginIconScale * 1.5f;
                stressIcons[i].color = Color.yellow;
            }
            else
            {
                stressIcons[i].transform.localScale = orginIconScale;
                stressIcons[i].color = Color.white;
            }
        }
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (curPos == targetPos)
        {
            DeltaSlide(eventData.delta.y);
        }
        StressIcon();
    }
}
