using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020003FF RID: 1023
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class EventInterestAttribute : Attribute
	{
		// Token: 0x060020DF RID: 8415 RVA: 0x0007C75B File Offset: 0x0007A95B
		public EventInterestAttribute(params Type[] eventTypes)
		{
			this.eventTypes = eventTypes;
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x0007C773 File Offset: 0x0007A973
		public EventInterestAttribute(EventInterestOptions interests)
		{
			this.categoryFlags = (EventCategoryFlags)interests;
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x0007C773 File Offset: 0x0007A973
		internal EventInterestAttribute(EventInterestOptionsInternal interests)
		{
			this.categoryFlags = (EventCategoryFlags)interests;
		}

		// Token: 0x04000DE8 RID: 3560
		internal Type[] eventTypes;

		// Token: 0x04000DE9 RID: 3561
		internal EventCategoryFlags categoryFlags = EventCategoryFlags.None;
	}
}
