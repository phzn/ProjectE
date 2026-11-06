using System;
using UnityEngine;

namespace Features.EggRotor
{
	// Token: 0x02001218 RID: 4632
	public struct EggRotorTrembleWeights
	{
		// Token: 0x170016E1 RID: 5857
		// (get) Token: 0x06008F35 RID: 36661 RVA: 0x003A91DC File Offset: 0x003A73DC
		public bool Any
		{
			get
			{
				return this.Legs > 0f || this.Pelvis > 0f || this.Torso > 0f || this.Head > 0f || this.Arms > 0f;
			}
		}

		// Token: 0x06008F36 RID: 36662 RVA: 0x003A922C File Offset: 0x003A742C
		public static EggRotorTrembleWeights Split(float lower, float upper)
		{
			return new EggRotorTrembleWeights
			{
				Legs = lower,
				Pelvis = lower,
				Torso = upper,
				Head = upper,
				Arms = upper
			};
		}

		// Token: 0x06008F37 RID: 36663 RVA: 0x003A926C File Offset: 0x003A746C
		public EggRotorTrembleWeights Clamped()
		{
			return new EggRotorTrembleWeights
			{
				Legs = EggRotorTrembleWeights.Clamp(this.Legs),
				Pelvis = EggRotorTrembleWeights.Clamp(this.Pelvis),
				Torso = EggRotorTrembleWeights.Clamp(this.Torso),
				Head = EggRotorTrembleWeights.Clamp(this.Head),
				Arms = EggRotorTrembleWeights.Clamp(this.Arms)
			};
		}

		// Token: 0x06008F38 RID: 36664 RVA: 0x003A92DC File Offset: 0x003A74DC
		public void ReleaseLowerBody(float step)
		{
			this.Legs = Mathf.MoveTowards(this.Legs, 0f, step);
			this.Pelvis = Mathf.MoveTowards(this.Pelvis, 0f, step);
		}

		// Token: 0x06008F39 RID: 36665 RVA: 0x003A930C File Offset: 0x003A750C
		private static float Clamp(float value)
		{
			if (!float.IsNaN(value))
			{
				return Mathf.Clamp01(value);
			}
			return 0f;
		}

		// Token: 0x040092DD RID: 37597
		public float Legs;

		// Token: 0x040092DE RID: 37598
		public float Pelvis;

		// Token: 0x040092DF RID: 37599
		public float Torso;

		// Token: 0x040092E0 RID: 37600
		public float Head;

		// Token: 0x040092E1 RID: 37601
		public float Arms;
	}
}
