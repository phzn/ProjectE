using System;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x020000D5 RID: 213
	[Serializable]
	public class ReductionSettings : IDataValidate
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x0000706A File Offset: 0x0000526A
		public bool IsEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00028079 File Offset: 0x00026279
		public float GetMaxConnectionDistance()
		{
			return math.max(math.max(0.001f, this.simpleDistance), this.shapeDistance);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00028096 File Offset: 0x00026296
		public ReductionSettings Clone()
		{
			return new ReductionSettings
			{
				simpleDistance = this.simpleDistance,
				shapeDistance = this.shapeDistance
			};
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x000280B5 File Offset: 0x000262B5
		public void DataValidate()
		{
			this.simpleDistance = Mathf.Clamp(this.simpleDistance, 0f, 0.2f);
			this.shapeDistance = Mathf.Clamp(this.shapeDistance, 0f, 0.2f);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x000280ED File Offset: 0x000262ED
		public override int GetHashCode()
		{
			return 0 + this.simpleDistance.GetHashCode() + this.shapeDistance.GetHashCode();
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00028108 File Offset: 0x00026308
		public override string ToString()
		{
			return string.Format("ReductionSettings. sameDist:{0}, simpleDist:{1}, shapeDist:{2} maxStep:{3}", new object[] { 0.001f, this.simpleDistance, this.shapeDistance, 100 });
		}

		// Token: 0x040007DA RID: 2010
		[Range(0f, 0.2f)]
		public float simpleDistance;

		// Token: 0x040007DB RID: 2011
		[Range(0f, 0.2f)]
		public float shapeDistance;
	}
}
