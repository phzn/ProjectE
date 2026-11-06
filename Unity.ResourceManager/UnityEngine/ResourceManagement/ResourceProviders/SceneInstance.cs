using System;
using UnityEngine.SceneManagement;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
	// Token: 0x0200003C RID: 60
	public struct SceneInstance
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00006AE5 File Offset: 0x00004CE5
		// (set) Token: 0x06000190 RID: 400 RVA: 0x00006AED File Offset: 0x00004CED
		public Scene Scene
		{
			get
			{
				return this.m_Scene;
			}
			internal set
			{
				this.m_Scene = value;
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00006AF6 File Offset: 0x00004CF6
		[Obsolete("Activate() has been deprecated.  Please use ActivateAsync().")]
		public void Activate()
		{
			this.m_Operation.allowSceneActivation = true;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00006B04 File Offset: 0x00004D04
		public AsyncOperation ActivateAsync()
		{
			this.m_Operation.allowSceneActivation = true;
			return this.m_Operation;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00006B18 File Offset: 0x00004D18
		public override int GetHashCode()
		{
			return this.Scene.GetHashCode();
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00006B3C File Offset: 0x00004D3C
		public override bool Equals(object obj)
		{
			return obj is SceneInstance && this.Scene.Equals(((SceneInstance)obj).Scene);
		}

		// Token: 0x0400009D RID: 157
		private Scene m_Scene;

		// Token: 0x0400009E RID: 158
		internal AsyncOperation m_Operation;
	}
}
