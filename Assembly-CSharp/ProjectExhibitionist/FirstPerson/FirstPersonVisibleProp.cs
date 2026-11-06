using System;
using UnityEngine;

namespace ProjectExhibitionist.FirstPerson
{
	// Token: 0x020009F0 RID: 2544
	[DisallowMultipleComponent]
	[AddComponentMenu("")]
	public sealed class FirstPersonVisibleProp : MonoBehaviour
	{
		// Token: 0x0600484C RID: 18508 RVA: 0x001B8F0C File Offset: 0x001B710C
		public static void Mark(GameObject prop)
		{
			if (prop != null && prop.GetComponent<FirstPersonVisibleProp>() == null)
			{
				prop.AddComponent<FirstPersonVisibleProp>();
			}
		}
	}
}
