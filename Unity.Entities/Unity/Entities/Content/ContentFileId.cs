using System;

namespace Unity.Entities.Content
{
	// Token: 0x0200047E RID: 1150
	[Serializable]
	internal struct ContentFileId : IEquatable<ContentFileId>
	{
		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06001C4E RID: 7246 RVA: 0x000632F7 File Offset: 0x000614F7
		public bool IsValid
		{
			get
			{
				return this.Value.IsValid;
			}
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x00063304 File Offset: 0x00061504
		public bool Equals(ContentFileId other)
		{
			return this.Value.Equals(other.Value);
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x00063317 File Offset: 0x00061517
		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}

		// Token: 0x06001C51 RID: 7249 RVA: 0x0006332A File Offset: 0x0006152A
		public override string ToString()
		{
			return this.Value.ToString();
		}

		// Token: 0x04000A06 RID: 2566
		public Hash128 Value;
	}
}
