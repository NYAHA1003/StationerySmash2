using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill;

public class AbstractDamagedSticker : AbstractSticker
{
	public override void RunStickerAblity()
	{
		//null;
	}

	/// <summary>
	/// 데미지 받음 스티커
	/// </summary>
	/// <param name="atkData"></param>
	public virtual void RunDamagedStickerAblity(ref AtkData atkData)
	{
		//null;
	}
}
