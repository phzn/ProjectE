using System;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000070 RID: 112
	[Serializable]
	public class WindSettings : IValid, IDataValidate
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x000105A8 File Offset: 0x0000E7A8
		public bool IsValid()
		{
			return this.influence > 1E-08f;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x000105B8 File Offset: 0x0000E7B8
		public void DataValidate()
		{
			this.influence = Mathf.Clamp(this.influence, 0f, 2f);
			this.frequency = Mathf.Clamp(this.frequency, 0f, 2f);
			this.turbulence = Mathf.Clamp(this.turbulence, 0f, 2f);
			this.blend = Mathf.Clamp01(this.blend);
			this.synchronization = Mathf.Clamp01(this.synchronization);
			this.depthWeight = Mathf.Clamp01(this.depthWeight);
			this.movingWind = Mathf.Clamp(this.movingWind, 0f, 10f);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00010664 File Offset: 0x0000E864
		public WindSettings Clone()
		{
			return new WindSettings
			{
				influence = this.influence,
				frequency = this.frequency,
				turbulence = this.turbulence,
				blend = this.blend,
				synchronization = this.synchronization,
				depthWeight = this.depthWeight,
				movingWind = this.movingWind
			};
		}

		// Token: 0x04000289 RID: 649
		[Range(0f, 2f)]
		public float influence = 1f;

		// Token: 0x0400028A RID: 650
		[Range(0f, 2f)]
		public float frequency = 1f;

		// Token: 0x0400028B RID: 651
		[Range(0f, 2f)]
		public float turbulence = 1f;

		// Token: 0x0400028C RID: 652
		[Range(0f, 1f)]
		public float blend = 0.7f;

		// Token: 0x0400028D RID: 653
		[Range(0f, 1f)]
		public float synchronization = 0.7f;

		// Token: 0x0400028E RID: 654
		[Range(0f, 1f)]
		public float depthWeight;

		// Token: 0x0400028F RID: 655
		[Range(0f, 10f)]
		public float movingWind;
	}
}
