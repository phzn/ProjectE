using System;

namespace Unity.Entities.Content
{
	// Token: 0x0200047F RID: 1151
	[Serializable]
	internal struct ContentSceneId : IEquatable<ContentSceneId>
	{
		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06001C52 RID: 7250 RVA: 0x0006333D File Offset: 0x0006153D
		public bool IsValid
		{
			get
			{
				return this.Value.IsValid;
			}
		}

		// Token: 0x06001C53 RID: 7251 RVA: 0x0006334A File Offset: 0x0006154A
		public bool Equals(ContentSceneId other)
		{
			return this.Value.Equals(other.Value);
		}

		// Token: 0x06001C54 RID: 7252 RVA: 0x0006335D File Offset: 0x0006155D
		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x00063370 File Offset: 0x00061570
		public override string ToString()
		{
			return this.Value.ToString();
		}

		// Token: 0x04000A07 RID: 2567
		public Hash128 Value;
	}
}
