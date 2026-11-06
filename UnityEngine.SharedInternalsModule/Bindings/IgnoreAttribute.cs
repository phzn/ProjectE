using System;

namespace UnityEngine.Bindings
{
	// Token: 0x02000025 RID: 37
	[AttributeUsage(AttributeTargets.Field)]
	[VisibleToOtherModules]
	internal class IgnoreAttribute : Attribute, IBindingsAttribute
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000074 RID: 116 RVA: 0x000025DA File Offset: 0x000007DA
		// (set) Token: 0x06000075 RID: 117 RVA: 0x000025E2 File Offset: 0x000007E2
		public bool DoesNotContributeToSize { get; set; }
	}
}
