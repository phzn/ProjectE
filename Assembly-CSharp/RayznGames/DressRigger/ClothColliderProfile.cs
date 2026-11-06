using System;
using System.Collections.Generic;
using UnityEngine;

namespace RayznGames.DressRigger
{
	// Token: 0x0200077F RID: 1919
	[AddComponentMenu("Dress Rigger/Cloth Collider Profile")]
	public class ClothColliderProfile : MonoBehaviour
	{
		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x06003260 RID: 12896 RVA: 0x0011D415 File Offset: 0x0011B615
		public ClothColliderProfile.SelectionMode Mode
		{
			get
			{
				return this.mode;
			}
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06003261 RID: 12897 RVA: 0x0011D41D File Offset: 0x0011B61D
		public IReadOnlyList<string> ColliderNames
		{
			get
			{
				return this.colliderNames;
			}
		}

		// Token: 0x06003262 RID: 12898 RVA: 0x0011D428 File Offset: 0x0011B628
		public bool ContainsName(string colliderName)
		{
			if (string.IsNullOrEmpty(colliderName))
			{
				return false;
			}
			for (int i = 0; i < this.colliderNames.Count; i++)
			{
				if (string.Equals(this.colliderNames[i], colliderName, StringComparison.Ordinal))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400331E RID: 13086
		[Tooltip("Come scegliere i collider del personaggio per i MagicaCloth di questo capo.")]
		[SerializeField]
		private ClothColliderProfile.SelectionMode mode = ClothColliderProfile.SelectionMode.NamedOnly;

		// Token: 0x0400331F RID: 13087
		[Tooltip("Nomi dei GameObject dei Magica collider sul rig del personaggio (usati con mode = NamedOnly). Vedi Tools/Dress Rigger/Dump Player Magica Colliders.")]
		[SerializeField]
		private List<string> colliderNames = new List<string>();

		// Token: 0x02000780 RID: 1920
		public enum SelectionMode
		{
			// Token: 0x04003321 RID: 13089
			AllPlayerColliders,
			// Token: 0x04003322 RID: 13090
			NamedOnly,
			// Token: 0x04003323 RID: 13091
			None
		}
	}
}
