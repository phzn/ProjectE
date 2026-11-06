using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000400 RID: 1024
	[AttributeUsage(AttributeTargets.Class)]
	internal class EventCategoryAttribute : Attribute
	{
		// Token: 0x060020E2 RID: 8418 RVA: 0x0007C78B File Offset: 0x0007A98B
		public EventCategoryAttribute(EventCategory category)
		{
			this.category = category;
		}

		// Token: 0x04000DEA RID: 3562
		internal EventCategory category;
	}
}
