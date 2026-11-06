using System;
using UnityEngine;

namespace SplineMesh
{
	// Token: 0x02000190 RID: 400
	[Serializable]
	public class TransformedMesh
	{
		// Token: 0x06000C66 RID: 3174 RVA: 0x00055731 File Offset: 0x00053931
		public TransformedMesh()
		{
			this.scale = Vector3.one;
		}

		// Token: 0x04000C59 RID: 3161
		public Mesh mesh;

		// Token: 0x04000C5A RID: 3162
		public Material material;

		// Token: 0x04000C5B RID: 3163
		public PhysicMaterial physicMaterial;

		// Token: 0x04000C5C RID: 3164
		public Vector3 translation;

		// Token: 0x04000C5D RID: 3165
		public Vector3 rotation;

		// Token: 0x04000C5E RID: 3166
		public Vector3 scale = Vector3.one;
	}
}
