using System;
using ProjectExhibitionist;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x0200073B RID: 1851
	public sealed class GarbageBodyPaintDefaults : ScriptableObject
	{
		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06002FD9 RID: 12249 RVA: 0x0010EC04 File Offset: 0x0010CE04
		public SkinDecal Decal
		{
			get
			{
				return this.decal;
			}
		}

		// Token: 0x04003059 RID: 12377
		[SerializeField]
		private SkinDecal decal;
	}
}
