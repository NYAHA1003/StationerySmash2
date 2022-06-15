using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Gacha/GachaSO")]
public class GachaSO : ScriptableObject
{

    [Header("����")]
    public float rarePercent; // ���� Ȯ��
    public float epicPercent; // ���� Ȯ��

    [Header("��ƼĿ")]
    public float rareCount; // ���� ����
    public float epicCount; // ���� ����

    [Space(10)]
    public int maxAmount; // ���� �� ���� �̴� Ƚ��(���� ������ ����) 
}