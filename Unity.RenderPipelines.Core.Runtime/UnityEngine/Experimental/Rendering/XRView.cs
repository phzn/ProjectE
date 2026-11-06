using System;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x0200000D RID: 13
	internal readonly struct XRView
	{
		// Token: 0x06000076 RID: 118 RVA: 0x000041A7 File Offset: 0x000023A7
		internal XRView(Matrix4x4 projMatrix, Matrix4x4 viewMatrix, Rect viewport, Mesh occlusionMesh, int textureArraySlice)
		{
			this.projMatrix = projMatrix;
			this.viewMatrix = viewMatrix;
			this.viewport = viewport;
			this.occlusionMesh = occlusionMesh;
			this.textureArraySlice = textureArraySlice;
			this.eyeCenterUV = XRView.ComputeEyeCenterUV(projMatrix);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000041DC File Offset: 0x000023DC
		private static Vector2 ComputeEyeCenterUV(Matrix4x4 proj)
		{
			FrustumPlanes decomposeProjection = proj.decomposeProjection;
			float num = Math.Abs(decomposeProjection.left);
			float num2 = Math.Abs(decomposeProjection.right);
			float num3 = Math.Abs(decomposeProjection.top);
			float num4 = Math.Abs(decomposeProjection.bottom);
			return new Vector2(num / (num2 + num), num3 / (num3 + num4));
		}

		// Token: 0x0400004F RID: 79
		internal readonly Matrix4x4 projMatrix;

		// Token: 0x04000050 RID: 80
		internal readonly Matrix4x4 viewMatrix;

		// Token: 0x04000051 RID: 81
		internal readonly Rect viewport;

		// Token: 0x04000052 RID: 82
		internal readonly Mesh occlusionMesh;

		// Token: 0x04000053 RID: 83
		internal readonly int textureArraySlice;

		// Token: 0x04000054 RID: 84
		internal readonly Vector2 eyeCenterUV;
	}
}
