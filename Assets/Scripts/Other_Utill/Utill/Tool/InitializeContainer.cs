using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Tool
{
    public class InitializeContainer : MonoBehaviour
    {
		[SerializeField]
		private List<Object> _iinitializes;

		private void Awake()
		{
			int count = _iinitializes.Count;
			for(int i = 0; i < count; i++)
			{
				(_iinitializes[i] as Iinitialize).Initialize();
			}
		}
	}

}