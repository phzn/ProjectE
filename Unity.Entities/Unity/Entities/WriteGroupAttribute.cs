using System;

namespace Unity.Entities
{
	// Token: 0x02000320 RID: 800
	[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
	public class WriteGroupAttribute : Attribute
	{
		// Token: 0x0600155D RID: 5469 RVA: 0x00050EBF File Offset: 0x0004F0BF
		public WriteGroupAttribute(Type targetType)
		{
			this.TargetType = targetType;
		}

		// Token: 0x040007CB RID: 1995
		public Type TargetType;
	}
}
