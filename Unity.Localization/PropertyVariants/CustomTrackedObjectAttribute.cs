using System;

namespace UnityEngine.Localization.PropertyVariants
{
	// Token: 0x020000E8 RID: 232
	[AttributeUsage(AttributeTargets.Class)]
	public class CustomTrackedObjectAttribute : Attribute
	{
		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x00015592 File Offset: 0x00013792
		internal Type ObjectType { get; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x0001559A File Offset: 0x0001379A
		internal bool SupportsInheritedTypes { get; }

		// Token: 0x06000632 RID: 1586 RVA: 0x000155A2 File Offset: 0x000137A2
		public CustomTrackedObjectAttribute(Type type, bool supportsInheritedTypes)
		{
			this.ObjectType = type;
			this.SupportsInheritedTypes = supportsInheritedTypes;
		}
	}
}
