using System;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000062 RID: 98
	[Serializable]
	public class CurveSerializeData
	{
		// Token: 0x06000154 RID: 340 RVA: 0x0000EDC4 File Offset: 0x0000CFC4
		public CurveSerializeData()
		{
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000EDEC File Offset: 0x0000CFEC
		public CurveSerializeData(float value)
		{
			this.value = value;
			this.useCurve = false;
			this.curve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000EE4C File Offset: 0x0000D04C
		public CurveSerializeData(float value, float curveStart, float curveEnd, bool useCurve = true)
		{
			this.value = value;
			this.useCurve = useCurve;
			this.curve = AnimationCurve.Linear(0f, Mathf.Clamp01(curveStart), 1f, Mathf.Clamp01(curveEnd));
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000EEAE File Offset: 0x0000D0AE
		public CurveSerializeData(float value, AnimationCurve curve)
		{
			this.value = value;
			this.useCurve = true;
			this.curve = curve;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000EEEA File Offset: 0x0000D0EA
		public void SetValue(float value)
		{
			this.value = value;
			this.useCurve = false;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000EEFA File Offset: 0x0000D0FA
		public void SetValue(float value, float curveStart, float curveEnd, bool useCurve = true)
		{
			this.value = value;
			this.useCurve = useCurve;
			this.curve = AnimationCurve.Linear(0f, Mathf.Clamp01(curveStart), 1f, Mathf.Clamp01(curveEnd));
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000EF2C File Offset: 0x0000D12C
		public void SetValue(float value, AnimationCurve curve)
		{
			this.value = value;
			this.useCurve = true;
			this.curve = curve;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000EF43 File Offset: 0x0000D143
		public void DataValidate(float min, float max)
		{
			this.value = Mathf.Clamp(this.value, min, max);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000EF58 File Offset: 0x0000D158
		public float Evaluate(float time)
		{
			if (this.useCurve)
			{
				return this.curve.Evaluate(time) * this.value;
			}
			return this.value;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000EF7C File Offset: 0x0000D17C
		public float4x4 ConvertFloatArray()
		{
			if (this.useCurve)
			{
				return DataUtility.ConvertAnimationCurve(this.curve) * this.value;
			}
			return this.value;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000EFA8 File Offset: 0x0000D1A8
		public CurveSerializeData Clone()
		{
			return new CurveSerializeData
			{
				value = this.value,
				useCurve = this.useCurve,
				curve = new AnimationCurve(this.curve.keys)
				{
					preWrapMode = this.curve.preWrapMode,
					postWrapMode = this.curve.postWrapMode
				}
			};
		}

		// Token: 0x0400023C RID: 572
		public float value;

		// Token: 0x0400023D RID: 573
		public bool useCurve;

		// Token: 0x0400023E RID: 574
		public AnimationCurve curve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
	}
}
