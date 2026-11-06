using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x020004DF RID: 1247
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class vEnumFlagAttribute : PropertyAttribute
	{
		// Token: 0x06001E0E RID: 7694 RVA: 0x00043846 File Offset: 0x00041A46
		public vEnumFlagAttribute()
		{
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x000B710F File Offset: 0x000B530F
		public vEnumFlagAttribute(string name)
		{
			this.enumName = name;
		}

		// Token: 0x04001F10 RID: 7952
		public string enumName;
	}
}
