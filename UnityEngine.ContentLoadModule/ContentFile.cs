using System;
using UnityEngine;

namespace Unity.Loading
{
	// Token: 0x02000005 RID: 5
	public struct ContentFile
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002090 File Offset: 0x00000290
		public ContentFileUnloadHandle UnloadAsync()
		{
			this.ThrowIfInvalidHandle();
			ContentLoadInterface.ContentFile_UnloadAsync(this);
			return new ContentFileUnloadHandle
			{
				Id = this
			};
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020CC File Offset: 0x000002CC
		public Object[] GetObjects()
		{
			this.ThrowIfNotComplete();
			return ContentLoadInterface.ContentFile_GetObjects(this);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020F0 File Offset: 0x000002F0
		public Object GetObject(ulong localIdentifierInFile)
		{
			this.ThrowIfNotComplete();
			return ContentLoadInterface.ContentFile_GetObject(this, localIdentifierInFile);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002118 File Offset: 0x00000318
		private void ThrowIfInvalidHandle()
		{
			bool flag = !this.IsValid;
			if (flag)
			{
				throw new Exception("The ContentFile operation cannot be performed because the handle is invalid. Did you already unload it?");
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002140 File Offset: 0x00000340
		private void ThrowIfNotComplete()
		{
			LoadingStatus loadingStatus = this.LoadingStatus;
			bool flag = loadingStatus == LoadingStatus.Failed;
			if (flag)
			{
				throw new Exception("Cannot use a failed ContentFile operation.");
			}
			bool flag2 = loadingStatus == LoadingStatus.InProgress;
			if (flag2)
			{
				throw new Exception("This ContentFile functionality is not supported while loading is in progress");
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000008 RID: 8 RVA: 0x0000217C File Offset: 0x0000037C
		public bool IsValid
		{
			get
			{
				return ContentLoadInterface.ContentFile_IsHandleValid(this);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000009 RID: 9 RVA: 0x0000219C File Offset: 0x0000039C
		public LoadingStatus LoadingStatus
		{
			get
			{
				this.ThrowIfInvalidHandle();
				return ContentLoadInterface.ContentFile_GetLoadingStatus(this);
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021C0 File Offset: 0x000003C0
		public bool WaitForCompletion(int timeoutMs)
		{
			this.ThrowIfInvalidHandle();
			return ContentLoadInterface.WaitForLoadCompletion(this, timeoutMs);
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000021E8 File Offset: 0x000003E8
		public static ContentFile GlobalTableDependency
		{
			get
			{
				return new ContentFile
				{
					Id = 1UL
				};
			}
		}

		// Token: 0x04000009 RID: 9
		internal ulong Id;
	}
}
