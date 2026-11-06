using System;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering
{
	// Token: 0x02000119 RID: 281
	public static class XRGraphicsAutomatedTests
	{
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x00027C4D File Offset: 0x00025E4D
		private static bool activatedFromCommandLine
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x00027C50 File Offset: 0x00025E50
		public static bool enabled { get; } = XRGraphicsAutomatedTests.activatedFromCommandLine;

		// Token: 0x06000873 RID: 2163 RVA: 0x00027C58 File Offset: 0x00025E58
		internal static void OverrideLayout(XRLayout layout, Camera camera)
		{
			if (XRGraphicsAutomatedTests.enabled && XRGraphicsAutomatedTests.running)
			{
				Matrix4x4 projectionMatrix = camera.projectionMatrix;
				Matrix4x4 worldToCameraMatrix = camera.worldToCameraMatrix;
				ScriptableCullingParameters scriptableCullingParameters;
				if (camera.TryGetCullingParameters(false, out scriptableCullingParameters))
				{
					scriptableCullingParameters.stereoProjectionMatrix = projectionMatrix;
					scriptableCullingParameters.stereoViewMatrix = worldToCameraMatrix;
					scriptableCullingParameters.stereoSeparationDistance = 0f;
					List<ValueTuple<Camera, XRPass>> activePasses = layout.GetActivePasses();
					for (int i = 0; i < activePasses.Count; i++)
					{
						XRPass item = activePasses[i].Item2;
						item.AssignCullingParams(item.cullingPassId, scriptableCullingParameters);
						for (int j = 0; j < item.viewCount; j++)
						{
							Matrix4x4 matrix4x = projectionMatrix;
							Matrix4x4 matrix4x2 = worldToCameraMatrix;
							bool flag = activePasses.Count == 2 && i == 0;
							bool flag2 = activePasses.Count == 1 && j == 0;
							if (flag || flag2)
							{
								FrustumPlanes decomposeProjection = matrix4x.decomposeProjection;
								decomposeProjection.left *= 0.44f;
								decomposeProjection.right *= 0.88f;
								decomposeProjection.top *= 0.11f;
								decomposeProjection.bottom *= 0.33f;
								matrix4x = Matrix4x4.Frustum(decomposeProjection);
								matrix4x2 *= Matrix4x4.Translate(new Vector3(0.34f, 0.25f, -0.08f));
							}
							XRView xrview = new XRView(matrix4x, matrix4x2, item.GetViewport(j), null, item.GetTextureArraySlice(j));
							item.AssignView(j, xrview);
						}
					}
				}
			}
		}

		// Token: 0x040004F8 RID: 1272
		public static bool running = false;
	}
}
