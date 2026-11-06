using System;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.AI
{
	// Token: 0x02000011 RID: 17
	[UsedByNativeCode]
	[MovedFrom("UnityEngine")]
	public struct NavMeshTriangulation
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00002C0B File Offset: 0x00000E0B
		[Obsolete("Use areas instead.")]
		public int[] layers
		{
			get
			{
				return this.areas;
			}
		}

		// Token: 0x0400002D RID: 45
		public Vector3[] vertices;

		// Token: 0x0400002E RID: 46
		public int[] indices;

		// Token: 0x0400002F RID: 47
		public int[] areas;
	}
}
