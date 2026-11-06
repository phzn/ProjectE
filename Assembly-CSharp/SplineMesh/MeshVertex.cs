using System;
using UnityEngine;

namespace SplineMesh
{
	// Token: 0x02000198 RID: 408
	[Serializable]
	public class MeshVertex
	{
		// Token: 0x06000C9F RID: 3231 RVA: 0x00056BB2 File Offset: 0x00054DB2
		public MeshVertex(Vector3 position, Vector3 normal, Vector2 uv)
		{
			this.position = position;
			this.normal = normal;
			this.uv = uv;
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x00056BCF File Offset: 0x00054DCF
		public MeshVertex(Vector3 position, Vector3 normal)
			: this(position, normal, Vector2.zero)
		{
		}

		// Token: 0x04000C8F RID: 3215
		public Vector3 position;

		// Token: 0x04000C90 RID: 3216
		public Vector3 normal;

		// Token: 0x04000C91 RID: 3217
		public Vector2 uv;
	}
}
