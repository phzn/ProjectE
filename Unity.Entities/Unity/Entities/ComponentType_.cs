using System;

namespace Unity.Entities
{
	// Token: 0x0200004C RID: 76
	internal struct ComponentType_
	{
		// Token: 0x06000221 RID: 545 RVA: 0x0000AC24 File Offset: 0x00008E24
		public override string ToString()
		{
			if (this.IsEnableable)
			{
				return string.Format("{0} {1} ({2} Disabled)", this.Type, this.Version, this.NumDisabledEntitiesInChunk);
			}
			return string.Format("{0} {1}", this.Type, this.Version);
		}

		// Token: 0x0400008E RID: 142
		public ComponentType Type;

		// Token: 0x0400008F RID: 143
		public uint Version;

		// Token: 0x04000090 RID: 144
		public bool IsEnableable;

		// Token: 0x04000091 RID: 145
		public int NumDisabledEntitiesInChunk;
	}
}
