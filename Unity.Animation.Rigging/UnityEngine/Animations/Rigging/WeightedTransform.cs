using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000066 RID: 102
	[Serializable]
	public struct WeightedTransform : ITransformProvider, IWeightProvider, IEquatable<WeightedTransform>
	{
		// Token: 0x060002AB RID: 683 RVA: 0x00009084 File Offset: 0x00007284
		public WeightedTransform(Transform transform, float weight)
		{
			this.transform = transform;
			this.weight = Mathf.Clamp01(weight);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00009099 File Offset: 0x00007299
		public static WeightedTransform Default(float weight)
		{
			return new WeightedTransform(null, weight);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x000090A2 File Offset: 0x000072A2
		public bool Equals(WeightedTransform other)
		{
			return this.transform == other.transform && this.weight == other.weight;
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060002AE RID: 686 RVA: 0x000090C8 File Offset: 0x000072C8
		// (set) Token: 0x060002AF RID: 687 RVA: 0x000090D0 File Offset: 0x000072D0
		Transform ITransformProvider.transform
		{
			get
			{
				return this.transform;
			}
			set
			{
				this.transform = value;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x000090D9 File Offset: 0x000072D9
		// (set) Token: 0x060002B1 RID: 689 RVA: 0x000090E1 File Offset: 0x000072E1
		float IWeightProvider.weight
		{
			get
			{
				return this.weight;
			}
			set
			{
				this.weight = Mathf.Clamp01(value);
			}
		}

		// Token: 0x04000116 RID: 278
		public Transform transform;

		// Token: 0x04000117 RID: 279
		public float weight;
	}
}
