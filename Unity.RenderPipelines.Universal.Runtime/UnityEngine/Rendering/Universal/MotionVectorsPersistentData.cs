using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200008B RID: 139
	internal sealed class MotionVectorsPersistentData
	{
		// Token: 0x06000556 RID: 1366 RVA: 0x0001F235 File Offset: 0x0001D435
		internal MotionVectorsPersistentData()
		{
			this.Reset();
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x0001F273 File Offset: 0x0001D473
		internal int lastFrameIndex
		{
			get
			{
				return this.m_LastFrameIndex[0];
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x0001F27D File Offset: 0x0001D47D
		internal Matrix4x4 viewProjection
		{
			get
			{
				return this.m_ViewProjection[0];
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x0001F28B File Offset: 0x0001D48B
		internal Matrix4x4 previousViewProjection
		{
			get
			{
				return this.m_PreviousViewProjection[0];
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x0001F299 File Offset: 0x0001D499
		internal Matrix4x4[] viewProjectionStereo
		{
			get
			{
				return this.m_ViewProjection;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x0001F2A1 File Offset: 0x0001D4A1
		internal Matrix4x4[] previousViewProjectionStereo
		{
			get
			{
				return this.m_PreviousViewProjection;
			}
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001F2AC File Offset: 0x0001D4AC
		public void Reset()
		{
			for (int i = 0; i < this.m_ViewProjection.Length; i++)
			{
				this.m_ViewProjection[i] = Matrix4x4.identity;
				this.m_PreviousViewProjection[i] = Matrix4x4.identity;
				this.m_LastFrameIndex[i] = -1;
				this.m_PrevAspectRatio[i] = -1f;
			}
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001F304 File Offset: 0x0001D504
		internal int GetXRMultiPassId(ref CameraData cameraData)
		{
			if (!cameraData.xr.enabled)
			{
				return 0;
			}
			return cameraData.xr.multipassId;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0001F320 File Offset: 0x0001D520
		public void Update(ref CameraData cameraData)
		{
			int xrmultiPassId = this.GetXRMultiPassId(ref cameraData);
			bool flag = this.m_PrevAspectRatio[xrmultiPassId] != cameraData.aspectRatio;
			if (this.m_LastFrameIndex[xrmultiPassId] != Time.frameCount || flag)
			{
				if (cameraData.xr.enabled && cameraData.xr.singlePassEnabled)
				{
					Matrix4x4 matrix4x = GL.GetGPUProjectionMatrix(cameraData.GetProjectionMatrixNoJitter(0), true) * cameraData.GetViewMatrix(0);
					Matrix4x4 matrix4x2 = GL.GetGPUProjectionMatrix(cameraData.GetProjectionMatrixNoJitter(1), true) * cameraData.GetViewMatrix(1);
					this.m_PreviousViewProjection[0] = (flag ? matrix4x : this.m_ViewProjection[0]);
					this.m_PreviousViewProjection[1] = (flag ? matrix4x2 : this.m_ViewProjection[1]);
					this.m_ViewProjection[0] = matrix4x;
					this.m_ViewProjection[1] = matrix4x2;
				}
				else
				{
					Matrix4x4 matrix4x3 = GL.GetGPUProjectionMatrix(cameraData.GetProjectionMatrixNoJitter(0), true) * cameraData.GetViewMatrix(0);
					this.m_PreviousViewProjection[xrmultiPassId] = (flag ? matrix4x3 : this.m_ViewProjection[xrmultiPassId]);
					this.m_ViewProjection[xrmultiPassId] = matrix4x3;
				}
				this.m_LastFrameIndex[xrmultiPassId] = Time.frameCount;
				this.m_PrevAspectRatio[xrmultiPassId] = cameraData.aspectRatio;
			}
		}

		// Token: 0x040003CD RID: 973
		private const int k_EyeCount = 2;

		// Token: 0x040003CE RID: 974
		private readonly Matrix4x4[] m_ViewProjection = new Matrix4x4[2];

		// Token: 0x040003CF RID: 975
		private readonly Matrix4x4[] m_PreviousViewProjection = new Matrix4x4[2];

		// Token: 0x040003D0 RID: 976
		private readonly int[] m_LastFrameIndex = new int[2];

		// Token: 0x040003D1 RID: 977
		private readonly float[] m_PrevAspectRatio = new float[2];
	}
}
