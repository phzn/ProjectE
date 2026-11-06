using System;

namespace UnityEngine.Timeline
{
	// Token: 0x0200003D RID: 61
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class TrackClipTypeAttribute : Attribute
	{
		// Token: 0x060002B1 RID: 689 RVA: 0x00009A3F File Offset: 0x00007C3F
		public TrackClipTypeAttribute(Type clipClass)
		{
			this.inspectedType = clipClass;
			this.allowAutoCreate = true;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00009A55 File Offset: 0x00007C55
		public TrackClipTypeAttribute(Type clipClass, bool allowAutoCreate)
		{
			this.inspectedType = clipClass;
		}

		// Token: 0x040000E8 RID: 232
		public readonly Type inspectedType;

		// Token: 0x040000E9 RID: 233
		public readonly bool allowAutoCreate;
	}
}
