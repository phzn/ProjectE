using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200012D RID: 301
	public class MMRadioSignalGenerator : MMRadioSignal
	{
		// Token: 0x060006C8 RID: 1736 RVA: 0x00021D0E File Offset: 0x0001FF0E
		private void Reset()
		{
			this.SignalList = new MMRadioSignalGeneratorItemList
			{
				new MMRadioSignalGeneratorItem()
			};
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00021D28 File Offset: 0x0001FF28
		public virtual float Evaluate(float time)
		{
			float num = 1f;
			if (this.SignalList.Count <= 0)
			{
				return num;
			}
			time = this.ApplyBias(time, this.Bias);
			for (int i = 0; i < this.SignalList.Count; i++)
			{
				if (this.SignalList[i].Active)
				{
					float valueNormalized = MMSignal.GetValueNormalized(time, this.SignalList[i].SignalType, this.SignalList[i].Phase, this.SignalList[i].Amplitude, this.SignalList[i].Frequency, this.SignalList[i].Offset, this.SignalList[i].Invert, this.SignalList[i].Curve, this.SignalList[i].TweenCurve, true, this.Clamps.x, this.Clamps.y, this.BackAndForth, this.BackAndForthMirrorPoint);
					num = ((this.SignalList[i].Mode == MMRadioSignalGeneratorItem.GeneratorItemModes.Multiply) ? (num * valueNormalized) : (num + valueNormalized));
				}
			}
			this.CurrentLevel *= this.GlobalMultiplier;
			this.CurrentLevel = Mathf.Clamp(this.CurrentLevel, this.Clamps.x, this.Clamps.y);
			return num;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00021E98 File Offset: 0x00020098
		protected override void Shake()
		{
			base.Shake();
			if (!this.Playing)
			{
				return;
			}
			if (this.SignalMode == MMRadioSignal.SignalModes.OneTime)
			{
				float num = this.TimescaleTime - this._shakeStartedTimestamp;
				this.CurrentLevel = this.Evaluate(MMMaths.Remap(num, 0f, this.Duration, 0f, 1f));
				return;
			}
			this.CurrentLevel = this.Evaluate(this.DriverTime);
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x00021F04 File Offset: 0x00020104
		protected override void ShakeComplete()
		{
			base.ShakeComplete();
			this.CurrentLevel = this.Evaluate(1f);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00021F1D File Offset: 0x0002011D
		public override float GraphValue(float time)
		{
			time = this.ApplyBias(time, this.Bias);
			return this.Evaluate(time);
		}

		// Token: 0x0400086F RID: 2159
		public bool AnimatedPreview;

		// Token: 0x04000870 RID: 2160
		public bool BackAndForth;

		// Token: 0x04000871 RID: 2161
		[MMCondition("BackAndForth", true)]
		public float BackAndForthMirrorPoint = 0.5f;

		// Token: 0x04000872 RID: 2162
		public MMRadioSignalGeneratorItemList SignalList;

		// Token: 0x04000873 RID: 2163
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 Clamps = new Vector2(0f, 1f);

		// Token: 0x04000874 RID: 2164
		[Range(0f, 1f)]
		public float Bias = 0.5f;
	}
}
