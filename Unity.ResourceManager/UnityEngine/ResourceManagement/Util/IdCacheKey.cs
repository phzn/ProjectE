using System;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x02000018 RID: 24
	internal sealed class IdCacheKey : IOperationCacheKey, IEquatable<IOperationCacheKey>
	{
		// Token: 0x060000CA RID: 202 RVA: 0x000048FC File Offset: 0x00002AFC
		public IdCacheKey(Type locType, string id)
		{
			this.ID = id;
			this.locationType = locType;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004912 File Offset: 0x00002B12
		private bool Equals(IdCacheKey other)
		{
			return this == other || (other != null && other.ID == this.ID && this.locationType == other.locationType);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004945 File Offset: 0x00002B45
		public override int GetHashCode()
		{
			return (527 + this.ID.GetHashCode()) * 31 + this.locationType.GetHashCode();
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004967 File Offset: 0x00002B67
		public override bool Equals(object obj)
		{
			return this.Equals(obj as IdCacheKey);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004975 File Offset: 0x00002B75
		public bool Equals(IOperationCacheKey other)
		{
			return this.Equals(other as IdCacheKey);
		}

		// Token: 0x0400004A RID: 74
		public string ID;

		// Token: 0x0400004B RID: 75
		public Type locationType;
	}
}
