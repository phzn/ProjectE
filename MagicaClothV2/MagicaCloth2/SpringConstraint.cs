using System;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000053 RID: 83
	public class SpringConstraint : IDisposable
	{
		// Token: 0x06000130 RID: 304 RVA: 0x0000706D File Offset: 0x0000526D
		public void Dispose()
		{
		}

		// Token: 0x02000054 RID: 84
		[Serializable]
		public class SerializeData : IDataValidate
		{
			// Token: 0x06000132 RID: 306 RVA: 0x0000D817 File Offset: 0x0000BA17
			public SerializeData()
			{
				this.useSpring = true;
				this.springPower = 0.04f;
				this.limitDistance = 0.1f;
				this.normalLimitRatio = 1f;
				this.springNoise = 0f;
			}

			// Token: 0x06000133 RID: 307 RVA: 0x0000D854 File Offset: 0x0000BA54
			public void DataValidate()
			{
				this.springPower = Math.Clamp(this.springPower, 0.001f, 1f);
				this.limitDistance = Mathf.Max(this.limitDistance, 0f);
				this.normalLimitRatio = Mathf.Clamp01(this.normalLimitRatio);
				this.springNoise = Mathf.Clamp01(this.springNoise);
			}

			// Token: 0x06000134 RID: 308 RVA: 0x0000D8B4 File Offset: 0x0000BAB4
			public SpringConstraint.SerializeData Clone()
			{
				return new SpringConstraint.SerializeData
				{
					useSpring = this.useSpring,
					springPower = this.springPower,
					limitDistance = this.limitDistance,
					normalLimitRatio = this.normalLimitRatio,
					springNoise = this.springNoise
				};
			}

			// Token: 0x0400020C RID: 524
			public bool useSpring;

			// Token: 0x0400020D RID: 525
			[Range(0.001f, 0.2f)]
			public float springPower;

			// Token: 0x0400020E RID: 526
			[Range(0f, 0.5f)]
			public float limitDistance;

			// Token: 0x0400020F RID: 527
			[Range(0f, 1f)]
			public float normalLimitRatio;

			// Token: 0x04000210 RID: 528
			[Range(0f, 1f)]
			public float springNoise;
		}

		// Token: 0x02000055 RID: 85
		public struct SpringConstraintParams
		{
			// Token: 0x06000135 RID: 309 RVA: 0x0000D904 File Offset: 0x0000BB04
			public void Convert(SpringConstraint.SerializeData sdata, ClothProcess.ClothType clothType)
			{
				this.springPower = ((clothType == ClothProcess.ClothType.BoneSpring && sdata.useSpring) ? sdata.springPower : 0f);
				this.limitDistance = sdata.limitDistance;
				this.normalLimitRatio = sdata.normalLimitRatio;
				this.springNoise = sdata.springNoise;
			}

			// Token: 0x04000211 RID: 529
			public float springPower;

			// Token: 0x04000212 RID: 530
			public float limitDistance;

			// Token: 0x04000213 RID: 531
			public float normalLimitRatio;

			// Token: 0x04000214 RID: 532
			public float springNoise;
		}
	}
}
