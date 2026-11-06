using System;

namespace UnityEngine.Rendering.LookDev
{
	// Token: 0x0200011C RID: 284
	public class StageRuntimeInterface
	{
		// Token: 0x0600087D RID: 2173 RVA: 0x00027DEB File Offset: 0x00025FEB
		public StageRuntimeInterface(Func<bool, GameObject> AddGameObject, Func<Camera> GetCamera, Func<Light> GetSunLight)
		{
			this.m_AddGameObject = AddGameObject;
			this.m_GetCamera = GetCamera;
			this.m_GetSunLight = GetSunLight;
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00027E08 File Offset: 0x00026008
		public GameObject AddGameObject(bool persistent = false)
		{
			Func<bool, GameObject> addGameObject = this.m_AddGameObject;
			if (addGameObject == null)
			{
				return null;
			}
			return addGameObject(persistent);
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x00027E1C File Offset: 0x0002601C
		public Camera camera
		{
			get
			{
				Func<Camera> getCamera = this.m_GetCamera;
				if (getCamera == null)
				{
					return null;
				}
				return getCamera();
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x00027E2F File Offset: 0x0002602F
		public Light sunLight
		{
			get
			{
				Func<Light> getSunLight = this.m_GetSunLight;
				if (getSunLight == null)
				{
					return null;
				}
				return getSunLight();
			}
		}

		// Token: 0x040004FC RID: 1276
		private Func<bool, GameObject> m_AddGameObject;

		// Token: 0x040004FD RID: 1277
		private Func<Camera> m_GetCamera;

		// Token: 0x040004FE RID: 1278
		private Func<Light> m_GetSunLight;

		// Token: 0x040004FF RID: 1279
		public object SRPData;
	}
}
