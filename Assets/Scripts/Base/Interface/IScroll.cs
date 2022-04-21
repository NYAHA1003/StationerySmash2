using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScroll 
{
    /// <summary>
    /// 스크롤 인덱스를 전달함
    /// </summary>
    /// <param name="scrollIndex"></param>
    public void Notify(int scrollIndex);
}
