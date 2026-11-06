using System;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x0200074D RID: 1869
	public class HideInPlayMode : MonoBehaviour
	{
		// Token: 0x06003074 RID: 12404 RVA: 0x00112329 File Offset: 0x00110529
		private void Start()
		{
			base.gameObject.SetActive(false);
		}
	}
}
