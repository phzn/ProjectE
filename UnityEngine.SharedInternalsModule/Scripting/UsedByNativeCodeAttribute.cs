using System;
using UnityEngine.Bindings;

namespace UnityEngine.Scripting
{
	// Token: 0x0200002D RID: 45
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface, Inherited = false)]
	[VisibleToOtherModules]
	internal class UsedByNativeCodeAttribute : Attribute
	{
		// Token: 0x0600008D RID: 141 RVA: 0x00002078 File Offset: 0x00000278
		public UsedByNativeCodeAttribute()
		{
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000268B File Offset: 0x0000088B
		public UsedByNativeCodeAttribute(string name)
		{
			this.Name = name;
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600008F RID: 143 RVA: 0x0000269D File Offset: 0x0000089D
		// (set) Token: 0x06000090 RID: 144 RVA: 0x000026A5 File Offset: 0x000008A5
		public string Name { get; set; }
	}
}
