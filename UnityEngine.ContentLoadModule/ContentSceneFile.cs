using System;
using UnityEngine.SceneManagement;

namespace Unity.Loading
{
	// Token: 0x02000008 RID: 8
	public struct ContentSceneFile
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002278 File Offset: 0x00000478
		public Scene Scene
		{
			get
			{
				this.ThrowIfInvalidHandle();
				return ContentLoadInterface.ContentSceneFile_GetScene(this);
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000229C File Offset: 0x0000049C
		public void IntegrateAtEndOfFrame()
		{
			this.ThrowIfInvalidHandle();
			ContentLoadInterface.ContentSceneFile_IntegrateAtEndOfFrame(this);
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000022B4 File Offset: 0x000004B4
		public SceneLoadingStatus Status
		{
			get
			{
				this.ThrowIfInvalidHandle();
				return ContentLoadInterface.ContentSceneFile_GetStatus(this);
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000022D8 File Offset: 0x000004D8
		public bool UnloadAtEndOfFrame()
		{
			this.ThrowIfInvalidHandle();
			return ContentLoadInterface.ContentSceneFile_UnloadAtEndOfFrame(this);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000022FC File Offset: 0x000004FC
		public bool WaitForLoadCompletion(int timeoutMs)
		{
			this.ThrowIfInvalidHandle();
			return ContentLoadInterface.ContentSceneFile_WaitForCompletion(this, timeoutMs);
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002324 File Offset: 0x00000524
		public bool IsValid
		{
			get
			{
				return ContentLoadInterface.ContentSceneFile_IsHandleValid(this);
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002344 File Offset: 0x00000544
		private void ThrowIfInvalidHandle()
		{
			bool flag = !this.IsValid;
			if (flag)
			{
				throw new Exception("The ContentSceneFile operation cannot be performed because the handle is invalid. Did you already unload it?");
			}
		}

		// Token: 0x04000013 RID: 19
		internal ulong Id;
	}
}
