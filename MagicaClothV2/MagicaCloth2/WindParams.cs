using System;

namespace MagicaCloth2
{
	// Token: 0x0200006F RID: 111
	public struct WindParams : IValid
	{
		// Token: 0x060001B2 RID: 434 RVA: 0x00010538 File Offset: 0x0000E738
		public void Convert(WindSettings sdata, ClothProcess.ClothType clothType)
		{
			this.influence = sdata.influence;
			this.frequency = sdata.frequency;
			this.turbulence = sdata.turbulence;
			this.blend = sdata.blend;
			this.synchronization = sdata.synchronization;
			this.depthWeight = sdata.depthWeight;
			this.movingWind = sdata.movingWind;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00010599 File Offset: 0x0000E799
		public bool IsValid()
		{
			return this.influence > 1E-08f;
		}

		// Token: 0x04000282 RID: 642
		public float influence;

		// Token: 0x04000283 RID: 643
		public float frequency;

		// Token: 0x04000284 RID: 644
		public float turbulence;

		// Token: 0x04000285 RID: 645
		public float blend;

		// Token: 0x04000286 RID: 646
		public float synchronization;

		// Token: 0x04000287 RID: 647
		public float depthWeight;

		// Token: 0x04000288 RID: 648
		public float movingWind;
	}
}
