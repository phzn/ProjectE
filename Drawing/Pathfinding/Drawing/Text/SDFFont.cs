using System;
using UnityEngine;

namespace Pathfinding.Drawing.Text
{
	// Token: 0x0200005C RID: 92
	internal struct SDFFont
	{
		// Token: 0x0400017E RID: 382
		public string name;

		// Token: 0x0400017F RID: 383
		public int size;

		// Token: 0x04000180 RID: 384
		public int width;

		// Token: 0x04000181 RID: 385
		public int height;

		// Token: 0x04000182 RID: 386
		public bool bold;

		// Token: 0x04000183 RID: 387
		public bool italic;

		// Token: 0x04000184 RID: 388
		public SDFCharacter[] characters;

		// Token: 0x04000185 RID: 389
		public Material material;
	}
}
