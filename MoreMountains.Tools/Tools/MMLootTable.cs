using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000199 RID: 409
	public class MMLootTable<T, V> where T : MMLoot<V>
	{
		// Token: 0x060009AE RID: 2478 RVA: 0x0002CA18 File Offset: 0x0002AC18
		public virtual void ComputeWeights()
		{
			if (this.ObjectsToLoot == null)
			{
				return;
			}
			if (this.ObjectsToLoot.Count == 0)
			{
				return;
			}
			this._maximumWeightSoFar = 0f;
			foreach (T t in this.ObjectsToLoot)
			{
				if (t.Weight >= 0f)
				{
					t.RangeFrom = this._maximumWeightSoFar;
					this._maximumWeightSoFar += t.Weight;
					t.RangeTo = this._maximumWeightSoFar;
				}
				else
				{
					t.Weight = 0f;
				}
			}
			this.WeightsTotal = this._maximumWeightSoFar;
			foreach (T t2 in this.ObjectsToLoot)
			{
				t2.ChancePercentage = t2.Weight / this.WeightsTotal * 100f;
			}
			this._weightsComputed = true;
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0002CB58 File Offset: 0x0002AD58
		public virtual T GetLoot()
		{
			if (this.ObjectsToLoot == null)
			{
				T t = default(T);
				return t;
			}
			if (this.ObjectsToLoot.Count == 0)
			{
				T t = default(T);
				return t;
			}
			if (!this._weightsComputed)
			{
				this.ComputeWeights();
			}
			float num = Random.Range(0f, this.WeightsTotal);
			foreach (T t2 in this.ObjectsToLoot)
			{
				if (num > t2.RangeFrom && num < t2.RangeTo)
				{
					return t2;
				}
			}
			return default(T);
		}

		// Token: 0x04000B78 RID: 2936
		[SerializeField]
		public List<T> ObjectsToLoot;

		// Token: 0x04000B79 RID: 2937
		[Header("Debug")]
		[MMReadOnly]
		public float WeightsTotal;

		// Token: 0x04000B7A RID: 2938
		protected float _maximumWeightSoFar;

		// Token: 0x04000B7B RID: 2939
		protected bool _weightsComputed;
	}
}
