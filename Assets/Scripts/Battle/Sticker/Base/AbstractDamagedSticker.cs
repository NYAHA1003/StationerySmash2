using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utill.Data;
using Utill.Tool;

namespace Battle.Sticker
{

	public class AbstractDamagedSticker : AbstractSticker
	{
		/// <summary>
		/// ������ ���� ��ƼĿ
		/// </summary>
		/// <param name="atkData"></param>
		public virtual void RunDamagedStickerAblity(ref AtkData atkData)
		{
			//null;
		}
	}

}