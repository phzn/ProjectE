using System;

namespace Unity.Entities.Content
{
	// Token: 0x0200047D RID: 1149
	[Serializable]
	internal struct ContentArchiveId : IEquatable<ContentArchiveId>
	{
		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06001C4A RID: 7242 RVA: 0x000632B1 File Offset: 0x000614B1
		public bool IsValid
		{
			get
			{
				return this.Value.IsValid;
			}
		}

		// Token: 0x06001C4B RID: 7243 RVA: 0x000632BE File Offset: 0x000614BE
		public bool Equals(ContentArchiveId other)
		{
			return this.Value.Equals(other.Value);
		}

		// Token: 0x06001C4C RID: 7244 RVA: 0x000632D1 File Offset: 0x000614D1
		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x000632E4 File Offset: 0x000614E4
		public override string ToString()
		{
			return this.Value.ToString();
		}

		// Token: 0x04000A05 RID: 2565
		public Hash128 Value;
	}
}
