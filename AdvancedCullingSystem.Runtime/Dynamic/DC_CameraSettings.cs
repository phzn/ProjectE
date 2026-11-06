using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x02000034 RID: 52
	public struct DC_CameraSettings
	{
		// Token: 0x06000137 RID: 311 RVA: 0x0000705A File Offset: 0x0000525A
		public DC_CameraSettings(Camera camera)
		{
			this.width = camera.pixelWidth;
			this.height = camera.pixelHeight;
			this.fov = camera.fieldOfView;
			this.farPlane = camera.farClipPlane;
		}

		// Token: 0x040000F1 RID: 241
		public int width;

		// Token: 0x040000F2 RID: 242
		public int height;

		// Token: 0x040000F3 RID: 243
		public float fov;

		// Token: 0x040000F4 RID: 244
		public float farPlane;
	}
}
