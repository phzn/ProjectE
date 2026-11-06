using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000046 RID: 70
	[AttributeUsage(AttributeTargets.Class)]
	public class CustomStyleAttribute : Attribute
	{
		// Token: 0x060002BC RID: 700 RVA: 0x00009AE0 File Offset: 0x00007CE0
		public CustomStyleAttribute(string ussStyle)
		{
			this.ussStyle = ussStyle;
		}

		// Token: 0x040000F3 RID: 243
		public readonly string ussStyle;
	}
}
