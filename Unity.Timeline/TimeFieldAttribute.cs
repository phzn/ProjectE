using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000043 RID: 67
	internal class TimeFieldAttribute : PropertyAttribute
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x00009AB9 File Offset: 0x00007CB9
		public TimeFieldAttribute.UseEditMode useEditMode { get; }

		// Token: 0x060002B9 RID: 697 RVA: 0x00009AC1 File Offset: 0x00007CC1
		public TimeFieldAttribute(TimeFieldAttribute.UseEditMode useEditMode = TimeFieldAttribute.UseEditMode.ApplyEditMode)
		{
			this.useEditMode = useEditMode;
		}

		// Token: 0x02000078 RID: 120
		public enum UseEditMode
		{
			// Token: 0x0400017E RID: 382
			None,
			// Token: 0x0400017F RID: 383
			ApplyEditMode
		}
	}
}
