using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x02000007 RID: 7
	public class XRLayout
	{
		// Token: 0x06000020 RID: 32 RVA: 0x00002A9C File Offset: 0x00000C9C
		public void AddCamera(Camera camera, bool enableXR)
		{
			if (camera == null)
			{
				return;
			}
			bool flag = (camera.cameraType == CameraType.Game || camera.cameraType == CameraType.VR) && camera.targetTexture == null && enableXR;
			if (XRSystem.displayActive && flag)
			{
				XRSystem.SetDisplayZRange(camera.nearClipPlane, camera.farClipPlane);
				XRSystem.CreateDefaultLayout(camera);
				return;
			}
			this.AddPass(camera, XRSystem.emptyPass);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002B09 File Offset: 0x00000D09
		public void ReconfigurePass(XRPass xrPass, Camera camera)
		{
			if (xrPass.enabled)
			{
				XRSystem.ReconfigurePass(xrPass, camera);
				xrPass.UpdateCombinedOcclusionMesh();
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002B20 File Offset: 0x00000D20
		public List<ValueTuple<Camera, XRPass>> GetActivePasses()
		{
			return this.m_ActivePasses;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002B28 File Offset: 0x00000D28
		internal void AddPass(Camera camera, XRPass xrPass)
		{
			xrPass.UpdateCombinedOcclusionMesh();
			this.m_ActivePasses.Add(new ValueTuple<Camera, XRPass>(camera, xrPass));
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002B44 File Offset: 0x00000D44
		internal void Clear()
		{
			for (int i = 0; i < this.m_ActivePasses.Count; i++)
			{
				XRPass item = this.m_ActivePasses[this.m_ActivePasses.Count - i - 1].Item2;
				if (item != XRSystem.emptyPass)
				{
					item.Release();
				}
			}
			this.m_ActivePasses.Clear();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002BA0 File Offset: 0x00000DA0
		internal void LogDebugInfo()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("XRSystem setup for frame {0}, active: {1}", Time.frameCount, XRSystem.displayActive);
			stringBuilder.AppendLine();
			for (int i = 0; i < this.m_ActivePasses.Count; i++)
			{
				XRPass item = this.m_ActivePasses[i].Item2;
				for (int j = 0; j < item.viewCount; j++)
				{
					Rect viewport = item.GetViewport(j);
					stringBuilder.AppendFormat("XR Pass {0} Cull {1} View {2} Slice {3} : {4} x {5}", new object[]
					{
						item.multipassId,
						item.cullingPassId,
						j,
						item.GetTextureArraySlice(j),
						viewport.width,
						viewport.height
					});
					stringBuilder.AppendLine();
				}
			}
			Debug.Log(stringBuilder);
		}

		// Token: 0x0400001D RID: 29
		private readonly List<ValueTuple<Camera, XRPass>> m_ActivePasses = new List<ValueTuple<Camera, XRPass>>();
	}
}
