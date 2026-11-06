using System;

namespace DA_Assets.FCU.Attributes
{
	// Token: 0x02000052 RID: 82
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class PaintPriorityAttribute : Attribute
	{
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x00006F39 File Offset: 0x00005139
		public int Priority { get; }

		// Token: 0x060001FA RID: 506 RVA: 0x00006F41 File Offset: 0x00005141
		public PaintPriorityAttribute(int priority)
		{
			this.Priority = priority;
		}
	}
}
