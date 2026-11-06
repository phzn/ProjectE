using System;
using UnityEngine;

namespace Features.DumpsterHide
{
	// Token: 0x0200123C RID: 4668
	public sealed class DumpsterRootTrajectory : ScriptableObject
	{
		// Token: 0x17001732 RID: 5938
		// (get) Token: 0x0600912B RID: 37163 RVA: 0x003B6F89 File Offset: 0x003B5189
		public AnimationClip SourceClip
		{
			get
			{
				return this.sourceClip;
			}
		}

		// Token: 0x17001733 RID: 5939
		// (get) Token: 0x0600912C RID: 37164 RVA: 0x003B6F94 File Offset: 0x003B5194
		public bool IsValid
		{
			get
			{
				return this.sourceClip != null && this.entryPositions != null && this.entryPositions.Length > 1 && this.entryPositions[this.entryPositions.Length - 1].sqrMagnitude > 0.0001f;
			}
		}

		// Token: 0x0600912D RID: 37165 RVA: 0x003B6FE5 File Offset: 0x003B51E5
		public void SetSamples(AnimationClip clip, Vector3[] positions)
		{
			this.sourceClip = clip;
			this.entryPositions = positions;
		}

		// Token: 0x0600912E RID: 37166 RVA: 0x003B6FF8 File Offset: 0x003B51F8
		public Vector3 EvaluateEntry(float phase, Vector3 start, Vector3 target)
		{
			phase = Mathf.Clamp01(phase);
			if (phase <= 0f)
			{
				return start;
			}
			if (phase >= 1f)
			{
				return target;
			}
			float num = phase * (float)(this.entryPositions.Length - 1);
			int num2 = Mathf.FloorToInt(num);
			Vector3 vector = Vector3.Lerp(this.entryPositions[num2], this.entryPositions[num2 + 1], num - (float)num2);
			Vector3 vector2 = this.entryPositions[this.entryPositions.Length - 1];
			Vector3 vector3 = target - start;
			vector2.y = (vector3.y = (vector.y = 0f));
			Quaternion quaternion = ((vector3.sqrMagnitude > 0.0001f) ? Quaternion.AngleAxis(Vector3.SignedAngle(vector2, vector3, Vector3.up), Vector3.up) : Quaternion.identity);
			Vector3 vector4 = start + quaternion * vector * (vector3.magnitude / vector2.magnitude);
			vector4.y = Mathf.Lerp(start.y, target.y, phase);
			return vector4;
		}

		// Token: 0x04009552 RID: 38226
		public const string ResourceName = "DumpsterRootTrajectory";

		// Token: 0x04009553 RID: 38227
		[SerializeField]
		private AnimationClip sourceClip;

		// Token: 0x04009554 RID: 38228
		[SerializeField]
		private Vector3[] entryPositions;
	}
}
