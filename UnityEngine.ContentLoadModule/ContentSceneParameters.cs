using System;
using UnityEngine.Bindings;
using UnityEngine.SceneManagement;

namespace Unity.Loading
{
	// Token: 0x02000007 RID: 7
	public struct ContentSceneParameters
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000C RID: 12 RVA: 0x0000220C File Offset: 0x0000040C
		// (set) Token: 0x0600000D RID: 13 RVA: 0x00002224 File Offset: 0x00000424
		public LoadSceneMode loadSceneMode
		{
			get
			{
				return this.m_LoadSceneMode;
			}
			set
			{
				this.m_LoadSceneMode = value;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002230 File Offset: 0x00000430
		// (set) Token: 0x0600000F RID: 15 RVA: 0x00002248 File Offset: 0x00000448
		public LocalPhysicsMode localPhysicsMode
		{
			get
			{
				return this.m_LocalPhysicsMode;
			}
			set
			{
				this.m_LocalPhysicsMode = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002254 File Offset: 0x00000454
		// (set) Token: 0x06000011 RID: 17 RVA: 0x0000226C File Offset: 0x0000046C
		public bool autoIntegrate
		{
			get
			{
				return this.m_AutoIntegrate;
			}
			set
			{
				this.m_AutoIntegrate = value;
			}
		}

		// Token: 0x04000010 RID: 16
		[NativeName("LoadSceneMode")]
		internal LoadSceneMode m_LoadSceneMode;

		// Token: 0x04000011 RID: 17
		[NativeName("LocalPhysicsMode")]
		internal LocalPhysicsMode m_LocalPhysicsMode;

		// Token: 0x04000012 RID: 18
		[NativeName("AutoIntegrate")]
		internal bool m_AutoIntegrate;
	}
}
