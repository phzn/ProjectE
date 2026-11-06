using System;
using UnityEngine;

namespace Features.CensorBars
{
	// Token: 0x020014BD RID: 5309
	internal readonly struct StreamerCensorMesh
	{
		// Token: 0x0600A34A RID: 41802 RVA: 0x0043D786 File Offset: 0x0043B986
		public StreamerCensorMesh(Mesh mesh, Matrix4x4 matrix)
		{
			this.Mesh = mesh;
			this.Matrix = matrix;
		}

		// Token: 0x0400A715 RID: 42773
		public readonly Mesh Mesh;

		// Token: 0x0400A716 RID: 42774
		public readonly Matrix4x4 Matrix;
	}
}
