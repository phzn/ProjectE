using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000040 RID: 64
	[AttributeUsage(AttributeTargets.Class)]
	public class TrackBindingTypeAttribute : Attribute
	{
		// Token: 0x060002B4 RID: 692 RVA: 0x00009A6F File Offset: 0x00007C6F
		public TrackBindingTypeAttribute(Type type)
		{
			this.type = type;
			this.flags = TrackBindingFlags.AllowCreateComponent;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00009A85 File Offset: 0x00007C85
		public TrackBindingTypeAttribute(Type type, TrackBindingFlags flags)
		{
			this.type = type;
			this.flags = flags;
		}

		// Token: 0x040000EE RID: 238
		public readonly Type type;

		// Token: 0x040000EF RID: 239
		public readonly TrackBindingFlags flags;
	}
}
