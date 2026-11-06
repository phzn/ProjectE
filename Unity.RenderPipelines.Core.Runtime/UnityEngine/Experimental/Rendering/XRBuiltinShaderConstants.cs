using System;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x02000006 RID: 6
	public static class XRBuiltinShaderConstants
	{
		// Token: 0x0600001C RID: 28 RVA: 0x0000264C File Offset: 0x0000084C
		public static void UpdateBuiltinShaderConstants(Matrix4x4 viewMatrix, Matrix4x4 projMatrix, bool renderIntoTexture, int viewIndex)
		{
			XRBuiltinShaderConstants.s_cameraProjMatrix[viewIndex] = projMatrix;
			XRBuiltinShaderConstants.s_viewMatrix[viewIndex] = viewMatrix;
			XRBuiltinShaderConstants.s_projMatrix[viewIndex] = GL.GetGPUProjectionMatrix(XRBuiltinShaderConstants.s_cameraProjMatrix[viewIndex], renderIntoTexture);
			XRBuiltinShaderConstants.s_viewProjMatrix[viewIndex] = XRBuiltinShaderConstants.s_projMatrix[viewIndex] * XRBuiltinShaderConstants.s_viewMatrix[viewIndex];
			XRBuiltinShaderConstants.s_invCameraProjMatrix[viewIndex] = Matrix4x4.Inverse(XRBuiltinShaderConstants.s_cameraProjMatrix[viewIndex]);
			XRBuiltinShaderConstants.s_invViewMatrix[viewIndex] = Matrix4x4.Inverse(XRBuiltinShaderConstants.s_viewMatrix[viewIndex]);
			XRBuiltinShaderConstants.s_invProjMatrix[viewIndex] = Matrix4x4.Inverse(XRBuiltinShaderConstants.s_projMatrix[viewIndex]);
			XRBuiltinShaderConstants.s_invViewProjMatrix[viewIndex] = Matrix4x4.Inverse(XRBuiltinShaderConstants.s_viewProjMatrix[viewIndex]);
			XRBuiltinShaderConstants.s_worldSpaceCameraPos[viewIndex] = XRBuiltinShaderConstants.s_invViewMatrix[viewIndex].GetColumn(3);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000273C File Offset: 0x0000093C
		public static void SetBuiltinShaderConstants(CommandBuffer cmd)
		{
			cmd.SetGlobalMatrixArray(XRBuiltinShaderConstants.unity_StereoCameraProjection, XRBuiltinShaderConstants.s_cameraProjMatrix);
			cmd.SetGlobalMatrixArray(XRBuiltinShaderConstants.unity_StereoCameraInvProjection, XRBuiltinShaderConstants.s_invCameraProjMatrix);
			cmd.SetGlobalMatrixArray(XRBuiltinShaderConstants.unity_StereoMatrixV, XRBuiltinShaderConstants.s_viewMatrix);
			cmd.SetGlobalMatrixArray(XRBuiltinShaderConstants.unity_StereoMatrixInvV, XRBuiltinShaderConstants.s_invViewMatrix);
			cmd.SetGlobalMatrixArray(XRBuiltinShaderConstants.unity_StereoMatrixP, XRBuiltinShaderConstants.s_projMatrix);
			cmd.SetGlobalMatrixArray(XRBuiltinShaderConstants.unity_StereoMatrixInvP, XRBuiltinShaderConstants.s_invProjMatrix);
			cmd.SetGlobalMatrixArray(XRBuiltinShaderConstants.unity_StereoMatrixVP, XRBuiltinShaderConstants.s_viewProjMatrix);
			cmd.SetGlobalMatrixArray(XRBuiltinShaderConstants.unity_StereoMatrixInvVP, XRBuiltinShaderConstants.s_invViewProjMatrix);
			cmd.SetGlobalVectorArray(XRBuiltinShaderConstants.unity_StereoWorldSpaceCameraPos, XRBuiltinShaderConstants.s_worldSpaceCameraPos);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000027DC File Offset: 0x000009DC
		public static void Update(XRPass xrPass, CommandBuffer cmd, bool renderIntoTexture)
		{
			if (xrPass.enabled)
			{
				cmd.SetViewProjectionMatrices(xrPass.GetViewMatrix(0), xrPass.GetProjMatrix(0));
				if (xrPass.singlePassEnabled)
				{
					for (int i = 0; i < 2; i++)
					{
						XRBuiltinShaderConstants.s_cameraProjMatrix[i] = xrPass.GetProjMatrix(i);
						XRBuiltinShaderConstants.s_viewMatrix[i] = xrPass.GetViewMatrix(i);
						XRBuiltinShaderConstants.s_projMatrix[i] = GL.GetGPUProjectionMatrix(XRBuiltinShaderConstants.s_cameraProjMatrix[i], renderIntoTexture);
						XRBuiltinShaderConstants.s_viewProjMatrix[i] = XRBuiltinShaderConstants.s_projMatrix[i] * XRBuiltinShaderConstants.s_viewMatrix[i];
						XRBuiltinShaderConstants.s_invCameraProjMatrix[i] = Matrix4x4.Inverse(XRBuiltinShaderConstants.s_cameraProjMatrix[i]);
						XRBuiltinShaderConstants.s_invViewMatrix[i] = Matrix4x4.Inverse(XRBuiltinShaderConstants.s_viewMatrix[i]);
						XRBuiltinShaderConstants.s_invProjMatrix[i] = Matrix4x4.Inverse(XRBuiltinShaderConstants.s_projMatrix[i]);
						XRBuiltinShaderConstants.s_invViewProjMatrix[i] = Matrix4x4.Inverse(XRBuiltinShaderConstants.s_viewProjMatrix[i]);
						XRBuiltinShaderConstants.s_worldSpaceCameraPos[i] = XRBuiltinShaderConstants.s_invViewMatrix[i].GetColumn(3);
					}
					cmd.SetGlobalMatrixArray(XRBuiltinShaderConstants.unity_StereoCameraProjection, XRBuiltinShaderConstants.s_cameraProjMatrix);
					cmd.SetGlobalMatrixArray(XRBuiltinShaderConstants.unity_StereoCameraInvProjection, XRBuiltinShaderConstants.s_invCameraProjMatrix);
					cmd.SetGlobalMatrixArray(XRBuiltinShaderConstants.unity_StereoMatrixV, XRBuiltinShaderConstants.s_viewMatrix);
					cmd.SetGlobalMatrixArray(XRBuiltinShaderConstants.unity_StereoMatrixInvV, XRBuiltinShaderConstants.s_invViewMatrix);
					cmd.SetGlobalMatrixArray(XRBuiltinShaderConstants.unity_StereoMatrixP, XRBuiltinShaderConstants.s_projMatrix);
					cmd.SetGlobalMatrixArray(XRBuiltinShaderConstants.unity_StereoMatrixInvP, XRBuiltinShaderConstants.s_invProjMatrix);
					cmd.SetGlobalMatrixArray(XRBuiltinShaderConstants.unity_StereoMatrixVP, XRBuiltinShaderConstants.s_viewProjMatrix);
					cmd.SetGlobalMatrixArray(XRBuiltinShaderConstants.unity_StereoMatrixInvVP, XRBuiltinShaderConstants.s_invViewProjMatrix);
					cmd.SetGlobalVectorArray(XRBuiltinShaderConstants.unity_StereoWorldSpaceCameraPos, XRBuiltinShaderConstants.s_worldSpaceCameraPos);
				}
			}
		}

		// Token: 0x0400000B RID: 11
		public static readonly int unity_StereoCameraProjection = Shader.PropertyToID("unity_StereoCameraProjection");

		// Token: 0x0400000C RID: 12
		public static readonly int unity_StereoCameraInvProjection = Shader.PropertyToID("unity_StereoCameraInvProjection");

		// Token: 0x0400000D RID: 13
		public static readonly int unity_StereoMatrixV = Shader.PropertyToID("unity_StereoMatrixV");

		// Token: 0x0400000E RID: 14
		public static readonly int unity_StereoMatrixInvV = Shader.PropertyToID("unity_StereoMatrixInvV");

		// Token: 0x0400000F RID: 15
		public static readonly int unity_StereoMatrixP = Shader.PropertyToID("unity_StereoMatrixP");

		// Token: 0x04000010 RID: 16
		public static readonly int unity_StereoMatrixInvP = Shader.PropertyToID("unity_StereoMatrixInvP");

		// Token: 0x04000011 RID: 17
		public static readonly int unity_StereoMatrixVP = Shader.PropertyToID("unity_StereoMatrixVP");

		// Token: 0x04000012 RID: 18
		public static readonly int unity_StereoMatrixInvVP = Shader.PropertyToID("unity_StereoMatrixInvVP");

		// Token: 0x04000013 RID: 19
		public static readonly int unity_StereoWorldSpaceCameraPos = Shader.PropertyToID("unity_StereoWorldSpaceCameraPos");

		// Token: 0x04000014 RID: 20
		private static Matrix4x4[] s_cameraProjMatrix = new Matrix4x4[2];

		// Token: 0x04000015 RID: 21
		private static Matrix4x4[] s_invCameraProjMatrix = new Matrix4x4[2];

		// Token: 0x04000016 RID: 22
		private static Matrix4x4[] s_viewMatrix = new Matrix4x4[2];

		// Token: 0x04000017 RID: 23
		private static Matrix4x4[] s_invViewMatrix = new Matrix4x4[2];

		// Token: 0x04000018 RID: 24
		private static Matrix4x4[] s_projMatrix = new Matrix4x4[2];

		// Token: 0x04000019 RID: 25
		private static Matrix4x4[] s_invProjMatrix = new Matrix4x4[2];

		// Token: 0x0400001A RID: 26
		private static Matrix4x4[] s_viewProjMatrix = new Matrix4x4[2];

		// Token: 0x0400001B RID: 27
		private static Matrix4x4[] s_invViewProjMatrix = new Matrix4x4[2];

		// Token: 0x0400001C RID: 28
		private static Vector4[] s_worldSpaceCameraPos = new Vector4[2];
	}
}
