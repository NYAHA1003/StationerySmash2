using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Tool
{
    public class InitializeContainer : MonoBehaviour
    {
		[SerializeField]
		private List<Object> _Iinitializes;
		[SerializeField]
		private bool _isDebugMode;

		private void Awake()
		{
			int count = _Iinitializes.Count;
			for(int i = 0; i < count; i++)
			{
				if(_isDebugMode)
				{
					(_Iinitializes[i] as Iinitialize).Initialize();
				}
				else
				{
					(_Iinitializes[i] as Iinitialize).DebugInitialize();
				}
			}
		}
	}

}