using System;

namespace Unity.Collections
{
	// Token: 0x02000081 RID: 129
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property)]
	public class ExcludeFromBurstCompatTestingAttribute : Attribute
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x00016BC6 File Offset: 0x00014DC6
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x00016BCE File Offset: 0x00014DCE
		public string Reason { get; set; }

		// Token: 0x060006D9 RID: 1753 RVA: 0x00016BD7 File Offset: 0x00014DD7
		public ExcludeFromBurstCompatTestingAttribute(string _reason)
		{
			this.Reason = _reason;
		}
	}
}
