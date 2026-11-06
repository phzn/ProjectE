using System;
using UnityEngine.Bindings;

namespace UnityEngine.Scripting
{
	// Token: 0x0200002E RID: 46
	[VisibleToOtherModules]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface, Inherited = false)]
	internal class RequiredByNativeCodeAttribute : Attribute
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00002078 File Offset: 0x00000278
		public RequiredByNativeCodeAttribute()
		{
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000026AE File Offset: 0x000008AE
		public RequiredByNativeCodeAttribute(string name)
		{
			this.Name = name;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000026C0 File Offset: 0x000008C0
		public RequiredByNativeCodeAttribute(bool optional)
		{
			this.Optional = optional;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000026D2 File Offset: 0x000008D2
		public RequiredByNativeCodeAttribute(string name, bool optional)
		{
			this.Name = name;
			this.Optional = optional;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000026EC File Offset: 0x000008EC
		// (set) Token: 0x06000096 RID: 150 RVA: 0x000026F4 File Offset: 0x000008F4
		public string Name { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000026FD File Offset: 0x000008FD
		// (set) Token: 0x06000098 RID: 152 RVA: 0x00002705 File Offset: 0x00000905
		public bool Optional { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000099 RID: 153 RVA: 0x0000270E File Offset: 0x0000090E
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00002716 File Offset: 0x00000916
		public bool GenerateProxy { get; set; }
	}
}
