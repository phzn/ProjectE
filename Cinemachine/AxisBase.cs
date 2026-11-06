using System;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000037 RID: 55
	[Serializable]
	public struct AxisBase
	{
		// Token: 0x060002A4 RID: 676 RVA: 0x00012534 File Offset: 0x00010734
		public void Validate()
		{
			this.m_MaxValue = Mathf.Clamp(this.m_MaxValue, this.m_MinValue, this.m_MaxValue);
		}

		// Token: 0x040001E1 RID: 481
		[NoSaveDuringPlay]
		[Tooltip("The current value of the axis.")]
		public float m_Value;

		// Token: 0x040001E2 RID: 482
		[Tooltip("The minimum value for the axis")]
		public float m_MinValue;

		// Token: 0x040001E3 RID: 483
		[Tooltip("The maximum value for the axis")]
		public float m_MaxValue;

		// Token: 0x040001E4 RID: 484
		[Tooltip("If checked, then the axis will wrap around at the min/max values, forming a loop")]
		public bool m_Wrap;
	}
}
