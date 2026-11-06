using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000265 RID: 613
	[Serializable]
	public class MMSpringClampSettings
	{
		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06001151 RID: 4433 RVA: 0x0004C966 File Offset: 0x0004AB66
		public bool ClampNeeded
		{
			get
			{
				return this.ClampMin || this.ClampMax || this.ClampMinBounce || this.ClampMaxBounce;
			}
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x0004C988 File Offset: 0x0004AB88
		public virtual float GetTargetValue(float value, float initialValue)
		{
			float num = value;
			float num2 = (this.ClampMinInitial ? initialValue : this.ClampMinValue);
			if (this.ClampMin && value < num2)
			{
				num = num2;
			}
			float num3 = (this.ClampMaxInitial ? initialValue : this.ClampMaxValue);
			if (this.ClampMax && value > num3)
			{
				num = num3;
			}
			return num;
		}

		// Token: 0x0400144E RID: 5198
		[Header("Min")]
		[Tooltip("whether or not to clamp the min value of this spring, preventing it from going below a certain value")]
		public bool ClampMin;

		// Token: 0x0400144F RID: 5199
		[Tooltip("the value below which this spring can't go")]
		[MMCondition("ClampMin", true)]
		public float ClampMinValue;

		// Token: 0x04001450 RID: 5200
		[Tooltip("if ClampMin is true, whether or not to use the initial value as the min value")]
		[MMCondition("ClampMin", true)]
		public bool ClampMinInitial;

		// Token: 0x04001451 RID: 5201
		[Tooltip("whether or not the spring should bounce off the min value or not")]
		[MMCondition("ClampMin", true)]
		public bool ClampMinBounce;

		// Token: 0x04001452 RID: 5202
		[Header("Max")]
		[Tooltip("whether or not to clamp the max value of this spring, preventing it from going above a certain value")]
		public bool ClampMax;

		// Token: 0x04001453 RID: 5203
		[Tooltip("the value above which this spring can't go")]
		[MMCondition("ClampMax", true)]
		public float ClampMaxValue = 10f;

		// Token: 0x04001454 RID: 5204
		[Tooltip("if ClampMax is true, whether or not to use the initial value as the max value")]
		[MMCondition("ClampMax", true)]
		public bool ClampMaxInitial;

		// Token: 0x04001455 RID: 5205
		[Tooltip("whether or not the spring should bounce off the max value or not")]
		[MMCondition("ClampMax", true)]
		public bool ClampMaxBounce;
	}
}
