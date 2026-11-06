using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000068 RID: 104
	public class MMPositionRecorder : MonoBehaviour
	{
		// Token: 0x0600026C RID: 620 RVA: 0x0000B7A0 File Offset: 0x000099A0
		protected virtual void Awake()
		{
			this.Positions = new Vector3[this.NumberOfPositionsToRecord];
			for (int i = 0; i < this.Positions.Length; i++)
			{
				this.Positions[i] = base.transform.position;
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000B7E8 File Offset: 0x000099E8
		protected virtual void Update()
		{
			if (!this.RecordOnTimescaleZero && Time.timeScale == 0f)
			{
				return;
			}
			this.StorePositions();
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000B808 File Offset: 0x00009A08
		protected virtual void StorePositions()
		{
			this.FrameCounter = Time.frameCount;
			if (this.Mode == MMPositionRecorder.Modes.Framecount)
			{
				if (this.FrameCounter - this._frameCountLastRecord < this.FrameInterval)
				{
					return;
				}
				this._frameCountLastRecord = this.FrameCounter;
			}
			else
			{
				if (Time.time - this._timeLastRecord < this.TimeInterval)
				{
					return;
				}
				this._timeLastRecord = Time.time;
			}
			this.Positions[0] = base.transform.position;
			Array.Copy(this.Positions, 0, this.Positions, 1, this.Positions.Length - 1);
		}

		// Token: 0x0400035D RID: 861
		[Header("Recording Settings")]
		public int NumberOfPositionsToRecord = 100;

		// Token: 0x0400035E RID: 862
		public MMPositionRecorder.Modes Mode;

		// Token: 0x0400035F RID: 863
		[MMEnumCondition("Mode", new int[] { 0 })]
		public int FrameInterval;

		// Token: 0x04000360 RID: 864
		[MMEnumCondition("Mode", new int[] { 1 })]
		public float TimeInterval = 0.02f;

		// Token: 0x04000361 RID: 865
		public bool RecordOnTimescaleZero;

		// Token: 0x04000362 RID: 866
		[Header("Debug")]
		public Vector3[] Positions;

		// Token: 0x04000363 RID: 867
		[MMReadOnly]
		public int FrameCounter;

		// Token: 0x04000364 RID: 868
		protected int _frameCountLastRecord;

		// Token: 0x04000365 RID: 869
		protected float _timeLastRecord;

		// Token: 0x020003DF RID: 991
		public enum Modes
		{
			// Token: 0x04001E3F RID: 7743
			Framecount,
			// Token: 0x04001E40 RID: 7744
			Time
		}
	}
}
