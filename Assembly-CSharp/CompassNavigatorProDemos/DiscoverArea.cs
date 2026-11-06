using System;
using CompassNavigatorPro;
using UnityEngine;

namespace CompassNavigatorProDemos
{
	// Token: 0x020002AF RID: 687
	public class DiscoverArea : MonoBehaviour
	{
		// Token: 0x06001373 RID: 4979 RVA: 0x0007E1E1 File Offset: 0x0007C3E1
		private void OnTriggerEnter(Collider col)
		{
			if (col.tag == "MainCamera")
			{
				base.gameObject.SetActive(false);
				CompassPro.instance.ShowAnimatedText("Area Discovered!");
			}
		}
	}
}
