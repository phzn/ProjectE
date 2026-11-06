using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000AC RID: 172
	internal class CapturePass : ScriptableRenderPass
	{
		// Token: 0x06000598 RID: 1432 RVA: 0x00020371 File Offset: 0x0001E571
		public CapturePass(RenderPassEvent evt)
		{
			base.profilingSampler = new ProfilingSampler("CapturePass");
			base.renderPassEvent = evt;
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00020390 File Offset: 0x0001E590
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			this.m_CameraColorHandle = renderingData.cameraData.renderer.GetCameraColorBackBuffer(commandBuffer);
			using (new ProfilingScope(commandBuffer, CapturePass.m_ProfilingSampler))
			{
				RenderTargetIdentifier nameID = this.m_CameraColorHandle.nameID;
				IEnumerator<Action<RenderTargetIdentifier, CommandBuffer>> captureActions = renderingData.cameraData.captureActions;
				captureActions.Reset();
				while (captureActions.MoveNext())
				{
					Action<RenderTargetIdentifier, CommandBuffer> action = captureActions.Current;
					action(nameID, renderingData.commandBuffer);
				}
			}
		}

		// Token: 0x04000447 RID: 1095
		private RTHandle m_CameraColorHandle;

		// Token: 0x04000448 RID: 1096
		private const string m_ProfilerTag = "Capture Pass";

		// Token: 0x04000449 RID: 1097
		private static readonly ProfilingSampler m_ProfilingSampler = new ProfilingSampler("Capture Pass");
	}
}
