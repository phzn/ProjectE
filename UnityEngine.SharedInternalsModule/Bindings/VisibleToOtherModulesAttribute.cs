using System;

namespace UnityEngine.Bindings
{
	// Token: 0x0200000A RID: 10
	[VisibleToOtherModules]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
	internal class VisibleToOtherModulesAttribute : Attribute
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002078 File Offset: 0x00000278
		public VisibleToOtherModulesAttribute()
		{
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002078 File Offset: 0x00000278
		public VisibleToOtherModulesAttribute(params string[] modules)
		{
		}
	}
}
