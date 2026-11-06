using System;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008C7 RID: 2247
	[CreateAssetMenu(fileName = "NewOutfitType", menuName = "BoZo/BMAC_OutfitType")]
	public class OutfitType : ScriptableObject
	{
		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06003C8A RID: 15498 RVA: 0x00168506 File Offset: 0x00166706
		public string TypeName
		{
			get
			{
				if (string.IsNullOrEmpty(this.typeName))
				{
					return base.name;
				}
				return this.typeName;
			}
		}

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06003C8B RID: 15499 RVA: 0x00168522 File Offset: 0x00166722
		public string Description
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x0016852A File Offset: 0x0016672A
		private void OnValidate()
		{
			if (string.IsNullOrEmpty(this.typeName))
			{
				this.typeName = base.name;
			}
		}

		// Token: 0x04003C7E RID: 15486
		[Header("Outfit Type Settings")]
		[SerializeField]
		private string typeName;

		// Token: 0x04003C7F RID: 15487
		[SerializeField]
		private string description;
	}
}
