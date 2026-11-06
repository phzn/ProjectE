using System;

namespace UnityEngine.Rendering.Universal.Internal
{
	// Token: 0x0200011B RID: 283
	public static class NormalReconstruction
	{
		// Token: 0x0600094D RID: 2381 RVA: 0x0003DC94 File Offset: 0x0003BE94
		public static void SetupProperties(CommandBuffer cmd, in CameraData cameraData)
		{
			int num = ((cameraData.xr.enabled && cameraData.xr.singlePassEnabled) ? 2 : 1);
			for (int i = 0; i < num; i++)
			{
				CameraData cameraData2 = cameraData;
				Matrix4x4 viewMatrix = cameraData2.GetViewMatrix(i);
				cameraData2 = cameraData;
				Matrix4x4 projectionMatrix = cameraData2.GetProjectionMatrix(i);
				NormalReconstruction.s_NormalReconstructionMatrix[i] = projectionMatrix * viewMatrix;
				Matrix4x4 matrix4x = viewMatrix;
				matrix4x.SetColumn(3, new Vector4(0f, 0f, 0f, 1f));
				Matrix4x4 inverse = (projectionMatrix * matrix4x).inverse;
				NormalReconstruction.s_NormalReconstructionMatrix[i] = inverse;
			}
			cmd.SetGlobalMatrixArray(NormalReconstruction.s_NormalReconstructionMatrixID, NormalReconstruction.s_NormalReconstructionMatrix);
		}

		// Token: 0x0400086C RID: 2156
		private static readonly int s_NormalReconstructionMatrixID = Shader.PropertyToID("_NormalReconstructionMatrix");

		// Token: 0x0400086D RID: 2157
		private static Matrix4x4[] s_NormalReconstructionMatrix = new Matrix4x4[2];
	}
}
