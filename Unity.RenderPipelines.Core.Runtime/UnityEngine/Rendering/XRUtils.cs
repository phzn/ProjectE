using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000E4 RID: 228
	public static class XRUtils
	{
		// Token: 0x06000790 RID: 1936 RVA: 0x00025298 File Offset: 0x00023498
		public static void DrawOcclusionMesh(CommandBuffer cmd, Camera camera, bool stereoEnabled = true)
		{
			if (!XRGraphics.enabled || !camera.stereoEnabled || !stereoEnabled)
			{
				return;
			}
			RectInt rectInt = new RectInt(0, 0, camera.pixelWidth, camera.pixelHeight);
			cmd.DrawOcclusionMesh(rectInt);
		}
	}
}
