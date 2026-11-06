using System;
using UnityEngine;

namespace Invector.vItemManager
{
	// Token: 0x02000566 RID: 1382
	[vClassHeader("Item Collected Display", true, "icon_v2", false, "", helpBoxText = "Use this to display the name of collected items", openClose = false)]
	public class vItemCollectionDisplay : vMonoBehaviour
	{
		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06002109 RID: 8457 RVA: 0x000C15D9 File Offset: 0x000BF7D9
		public static vItemCollectionDisplay Instance
		{
			get
			{
				if (vItemCollectionDisplay.instance == null)
				{
					vItemCollectionDisplay.instance = Object.FindObjectOfType<vItemCollectionDisplay>();
				}
				return vItemCollectionDisplay.instance;
			}
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x000C15F7 File Offset: 0x000BF7F7
		public virtual void FadeText(string message, float timeToStay, float timeToFadeOut)
		{
			vItemCollectionTextHUD vItemCollectionTextHUD = Object.Instantiate<vItemCollectionTextHUD>(this.itemCollectedDiplayPrefab);
			vItemCollectionTextHUD.transform.SetParent(base.transform, false);
			vItemCollectionTextHUD.transform.SetAsFirstSibling();
			vItemCollectionTextHUD.Show(message, timeToStay, timeToFadeOut);
		}

		// Token: 0x040021AD RID: 8621
		private static vItemCollectionDisplay instance;

		// Token: 0x040021AE RID: 8622
		public vItemCollectionTextHUD itemCollectedDiplayPrefab;
	}
}
