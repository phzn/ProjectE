using System;
using UnityEngine;

namespace UltimateRadialMenuExample.CharacterInventory2D
{
	// Token: 0x0200017F RID: 383
	public class WorldItem : MonoBehaviour
	{
		// Token: 0x06000BF1 RID: 3057 RVA: 0x00052ED4 File Offset: 0x000510D4
		public void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.name == "Player")
			{
				this.myManager.PickupItem(this.myInformation);
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x04000BFD RID: 3069
		public CharacterInventoryGameManager myManager;

		// Token: 0x04000BFE RID: 3070
		public CharacterInventoryGameManager.ItemInformation myInformation;
	}
}
