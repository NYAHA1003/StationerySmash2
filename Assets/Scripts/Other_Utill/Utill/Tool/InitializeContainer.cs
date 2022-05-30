using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Tool
{
    public class InitializeContainer : MonoBehaviour
    {
		[SerializeField]
		private List<Object> _Iinitializes;

		private void Awake()
		{
			int count = _Iinitializes.Count;
			for(int i = 0; i < count; i++)
			{
				(_Iinitializes[i] as Iinitialize).Initialize();
			}
		}
	}

}