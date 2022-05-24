using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utill.Tool
{
    public class InitializeContainer : MonoBehaviour
    {
		[SerializeField]
		private List<Object> _iinitializes;
		[SerializeField]
		private ServerDataConnect _serverDataConnect;

		private void Awake()
		{
			int count = _iinitializes.Count;
			for(int i = 0; i < count; i++)
			{
				(_iinitializes[i] as IServerInitialize).ServerInitialize(_serverDataConnect);
			}
		}
	}

}