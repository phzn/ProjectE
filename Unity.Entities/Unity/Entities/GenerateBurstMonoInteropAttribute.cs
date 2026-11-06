using System;

namespace Unity.Entities
{
	// Token: 0x02000194 RID: 404
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
	public sealed class GenerateBurstMonoInteropAttribute : Attribute
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000A0C RID: 2572 RVA: 0x00032139 File Offset: 0x00030339
		// (set) Token: 0x06000A0D RID: 2573 RVA: 0x00032141 File Offset: 0x00030341
		public string AssetName { get; private set; }

		// Token: 0x06000A0E RID: 2574 RVA: 0x0003214A File Offset: 0x0003034A
		public GenerateBurstMonoInteropAttribute(string assetName)
		{
			this.AssetName = assetName;
		}
	}
}
