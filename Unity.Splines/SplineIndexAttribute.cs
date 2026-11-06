using System;

namespace UnityEngine.Splines
{
	// Token: 0x02000018 RID: 24
	public class SplineIndexAttribute : PropertyAttribute
	{
		// Token: 0x0600009D RID: 157 RVA: 0x00004400 File Offset: 0x00002600
		public SplineIndexAttribute(string splineContainerProperty)
		{
			this.SplineContainerProperty = splineContainerProperty;
		}

		// Token: 0x04000038 RID: 56
		public readonly string SplineContainerProperty;
	}
}
