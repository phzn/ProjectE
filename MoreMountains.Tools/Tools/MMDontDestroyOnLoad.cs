using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200000B RID: 11
	public class MMDontDestroyOnLoad : MonoBehaviour
	{
		// Token: 0x06000029 RID: 41 RVA: 0x00002905 File Offset: 0x00000B05
		protected void Awake()
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
	}
}
