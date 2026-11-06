using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x0200001B RID: 27
	internal sealed class AsyncOpHandlesCacheKey : IOperationCacheKey, IEquatable<IOperationCacheKey>
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x00004A75 File Offset: 0x00002C75
		public AsyncOpHandlesCacheKey(IList<AsyncOperationHandle> handles)
		{
			this.m_Handles = new HashSet<AsyncOperationHandle>(handles);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00004A89 File Offset: 0x00002C89
		public override int GetHashCode()
		{
			return this.m_Handles.GetHashCode();
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004A96 File Offset: 0x00002C96
		public override bool Equals(object obj)
		{
			return this.Equals(obj as AsyncOpHandlesCacheKey);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004AA4 File Offset: 0x00002CA4
		public bool Equals(IOperationCacheKey other)
		{
			return this.Equals(other as AsyncOpHandlesCacheKey);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004AB2 File Offset: 0x00002CB2
		private bool Equals(AsyncOpHandlesCacheKey other)
		{
			return this == other || (other != null && this.m_Handles.SetEquals(other.m_Handles));
		}

		// Token: 0x04000050 RID: 80
		private readonly HashSet<AsyncOperationHandle> m_Handles;
	}
}
