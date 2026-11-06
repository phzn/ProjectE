using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x0200001A RID: 26
	internal sealed class DependenciesCacheKey : IOperationCacheKey, IEquatable<IOperationCacheKey>
	{
		// Token: 0x060000D4 RID: 212 RVA: 0x00004A1D File Offset: 0x00002C1D
		public DependenciesCacheKey(IList<IResourceLocation> dependencies, int dependenciesHash)
		{
			this.m_Dependencies = dependencies;
			this.m_DependenciesHash = dependenciesHash;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00004A33 File Offset: 0x00002C33
		public override int GetHashCode()
		{
			return this.m_DependenciesHash;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004A3B File Offset: 0x00002C3B
		public override bool Equals(object obj)
		{
			return this.Equals(obj as DependenciesCacheKey);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004A49 File Offset: 0x00002C49
		public bool Equals(IOperationCacheKey other)
		{
			return this.Equals(other as DependenciesCacheKey);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004A57 File Offset: 0x00002C57
		private bool Equals(DependenciesCacheKey other)
		{
			return this == other || (other != null && LocationUtils.DependenciesEqual(this.m_Dependencies, other.m_Dependencies));
		}

		// Token: 0x0400004E RID: 78
		private readonly IList<IResourceLocation> m_Dependencies;

		// Token: 0x0400004F RID: 79
		private readonly int m_DependenciesHash;
	}
}
