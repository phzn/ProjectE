using System;
using UnityEngine;

namespace ProjectExhibitionist.BenchSeating
{
	// Token: 0x02000B26 RID: 2854
	[Serializable]
	public sealed class BenchMotionTrack
	{
		// Token: 0x060050FC RID: 20732 RVA: 0x001F5E18 File Offset: 0x001F4018
		public Vector3 Position(float time)
		{
			return new Vector3(this.x.Evaluate(time), 0f, this.z.Evaluate(time));
		}

		// Token: 0x060050FD RID: 20733 RVA: 0x001F5E3C File Offset: 0x001F403C
		public Quaternion Rotation(float time)
		{
			return Quaternion.Euler(0f, this.yaw.Evaluate(time), 0f);
		}

		// Token: 0x060050FE RID: 20734 RVA: 0x001F5E59 File Offset: 0x001F4059
		public float Contact(float time)
		{
			return Mathf.Clamp01(this.contact.Evaluate(time));
		}

		// Token: 0x040051A0 RID: 20896
		public AnimationCurve x = AnimationCurve.Constant(0f, 1f, 0f);

		// Token: 0x040051A1 RID: 20897
		public AnimationCurve z = AnimationCurve.Constant(0f, 1f, 0f);

		// Token: 0x040051A2 RID: 20898
		public AnimationCurve yaw = AnimationCurve.Constant(0f, 1f, 0f);

		// Token: 0x040051A3 RID: 20899
		public AnimationCurve contact = AnimationCurve.Linear(0f, 1f, 1f, 0f);

		// Token: 0x040051A4 RID: 20900
		public float duration = 1f;
	}
}
