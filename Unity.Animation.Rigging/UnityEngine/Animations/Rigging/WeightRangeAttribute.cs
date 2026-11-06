using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000070 RID: 112
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class WeightRangeAttribute : PropertyAttribute
	{
		// Token: 0x060002DC RID: 732 RVA: 0x000096EF File Offset: 0x000078EF
		public WeightRangeAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x0400012F RID: 303
		public readonly float min;

		// Token: 0x04000130 RID: 304
		public readonly float max = 1f;
	}
}
