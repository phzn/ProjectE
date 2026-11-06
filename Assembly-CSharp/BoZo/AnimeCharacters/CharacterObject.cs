using System;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008B7 RID: 2231
	[CreateAssetMenu(fileName = "BMAC_CharacterObject", menuName = "BMAC_CharacterObject")]
	public class CharacterObject : ScriptableObject
	{
		// Token: 0x06003BDF RID: 15327 RVA: 0x00004F31 File Offset: 0x00003131
		private void OnValidate()
		{
		}

		// Token: 0x06003BE0 RID: 15328 RVA: 0x00004F31 File Offset: 0x00003131
		public void SetIcon()
		{
		}

		// Token: 0x04003C14 RID: 15380
		public Texture2D icon;

		// Token: 0x04003C15 RID: 15381
		public CharacterData data;
	}
}
