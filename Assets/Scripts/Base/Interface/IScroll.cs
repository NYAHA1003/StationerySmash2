using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScroll 
{
    /// <summary>
    /// ��ũ�� �ε����� ������
    /// </summary>
    /// <param name="scrollIndex"></param>
    public void Notify(int scrollIndex);
}
