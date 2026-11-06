using System;
using UnityEngine;

namespace Unity.Entities.Serialization
{
	// Token: 0x02000387 RID: 903
	internal struct SerializedKeyFrame
	{
		// Token: 0x06001807 RID: 6151 RVA: 0x00059F00 File Offset: 0x00058100
		public SerializedKeyFrame(Keyframe kf)
		{
			this.Time = kf.time;
			this.Value = kf.value;
			this.InTangent = kf.inTangent;
			this.OutTangent = kf.outTangent;
			this.InWeight = kf.inWeight;
			this.OutWeight = kf.outWeight;
			this.WeightedMode = (int)kf.weightedMode;
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x00059F68 File Offset: 0x00058168
		public static implicit operator Keyframe(SerializedKeyFrame kf)
		{
			return new Keyframe(kf.Time, kf.Value, kf.InTangent, kf.OutTangent, kf.InWeight, kf.OutWeight)
			{
				weightedMode = (WeightedMode)kf.WeightedMode
			};
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x00059FAE File Offset: 0x000581AE
		public static implicit operator SerializedKeyFrame(Keyframe kf)
		{
			return new SerializedKeyFrame(kf);
		}

		// Token: 0x04000927 RID: 2343
		public float Time;

		// Token: 0x04000928 RID: 2344
		public float Value;

		// Token: 0x04000929 RID: 2345
		public float InTangent;

		// Token: 0x0400092A RID: 2346
		public float OutTangent;

		// Token: 0x0400092B RID: 2347
		public float InWeight;

		// Token: 0x0400092C RID: 2348
		public float OutWeight;

		// Token: 0x0400092D RID: 2349
		public int WeightedMode;
	}
}
