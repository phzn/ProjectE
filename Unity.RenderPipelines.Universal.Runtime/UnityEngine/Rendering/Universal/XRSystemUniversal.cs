using System;
using UnityEngine.Experimental.Rendering;
using UnityEngine.XR;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000107 RID: 263
	internal static class XRSystemUniversal
	{
		// Token: 0x06000846 RID: 2118 RVA: 0x000344A8 File Offset: 0x000326A8
		internal static void BeginLateLatching(Camera camera, XRPassUniversal xrPass)
		{
			XRDisplaySubsystem activeDisplay = XRSystem.GetActiveDisplay();
			if (activeDisplay != null && xrPass.viewCount == 2)
			{
				activeDisplay.BeginRecordingIfLateLatched(camera);
				xrPass.isLateLatchEnabled = true;
			}
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x000344D8 File Offset: 0x000326D8
		internal static void EndLateLatching(Camera camera, XRPassUniversal xrPass)
		{
			XRDisplaySubsystem activeDisplay = XRSystem.GetActiveDisplay();
			if (activeDisplay != null && xrPass.isLateLatchEnabled)
			{
				activeDisplay.EndRecordingIfLateLatched(camera);
				xrPass.isLateLatchEnabled = false;
			}
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x00034504 File Offset: 0x00032704
		internal static void UnmarkShaderProperties(CommandBuffer cmd, XRPassUniversal xrPass)
		{
			if (xrPass.isLateLatchEnabled && xrPass.hasMarkedLateLatch)
			{
				cmd.UnmarkLateLatchMatrix(CameraLateLatchMatrixType.View);
				cmd.UnmarkLateLatchMatrix(CameraLateLatchMatrixType.InverseView);
				cmd.UnmarkLateLatchMatrix(CameraLateLatchMatrixType.ViewProjection);
				cmd.UnmarkLateLatchMatrix(CameraLateLatchMatrixType.InverseViewProjection);
				xrPass.hasMarkedLateLatch = false;
			}
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0003453C File Offset: 0x0003273C
		internal static void MarkShaderProperties(CommandBuffer cmd, XRPassUniversal xrPass, bool renderIntoTexture)
		{
			if (xrPass.isLateLatchEnabled && xrPass.canMarkLateLatch)
			{
				cmd.MarkLateLatchMatrixShaderPropertyID(CameraLateLatchMatrixType.View, XRBuiltinShaderConstants.unity_StereoMatrixV);
				cmd.MarkLateLatchMatrixShaderPropertyID(CameraLateLatchMatrixType.InverseView, XRBuiltinShaderConstants.unity_StereoMatrixInvV);
				cmd.MarkLateLatchMatrixShaderPropertyID(CameraLateLatchMatrixType.ViewProjection, XRBuiltinShaderConstants.unity_StereoMatrixVP);
				cmd.MarkLateLatchMatrixShaderPropertyID(CameraLateLatchMatrixType.InverseViewProjection, XRBuiltinShaderConstants.unity_StereoMatrixInvVP);
				for (int i = 0; i < 2; i++)
				{
					XRSystemUniversal.s_projMatrix[i] = GL.GetGPUProjectionMatrix(xrPass.GetProjMatrix(i), renderIntoTexture);
				}
				cmd.SetLateLatchProjectionMatrices(XRSystemUniversal.s_projMatrix);
				xrPass.hasMarkedLateLatch = true;
			}
		}

		// Token: 0x040007B3 RID: 1971
		private static Matrix4x4[] s_projMatrix = new Matrix4x4[2];
	}
}
