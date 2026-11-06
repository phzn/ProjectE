using System;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000CB8 RID: 3256
	public sealed class LongWayHomeFloorRoutine
	{
		// Token: 0x17000F3D RID: 3901
		// (get) Token: 0x06005E75 RID: 24181 RVA: 0x00255D7C File Offset: 0x00253F7C
		// (set) Token: 0x06005E76 RID: 24182 RVA: 0x00255D84 File Offset: 0x00253F84
		public LongWayHomeFloorRoutine.Segment Current { get; private set; }

		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x06005E77 RID: 24183 RVA: 0x00255D8D File Offset: 0x00253F8D
		public bool IsLooping
		{
			get
			{
				return this.Current == LongWayHomeFloorRoutine.Segment.Belly || this.Current == LongWayHomeFloorRoutine.Segment.Back;
			}
		}

		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x06005E78 RID: 24184 RVA: 0x00255DA3 File Offset: 0x00253FA3
		public bool IsOnBack
		{
			get
			{
				return this.Current == LongWayHomeFloorRoutine.Segment.Back || this.Current == LongWayHomeFloorRoutine.Segment.Settle || this.Current == LongWayHomeFloorRoutine.Segment.Asleep;
			}
		}

		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x06005E79 RID: 24185 RVA: 0x00255DC2 File Offset: 0x00253FC2
		public bool CanSwitch
		{
			get
			{
				return this.IsLooping && !this._switchRequested && !this._sleepRequested;
			}
		}

		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x06005E7A RID: 24186 RVA: 0x00255DDF File Offset: 0x00253FDF
		public bool CanSleep
		{
			get
			{
				return this.IsLooping && !this._sleepRequested;
			}
		}

		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x06005E7B RID: 24187 RVA: 0x00255DF4 File Offset: 0x00253FF4
		public bool IsAsleep
		{
			get
			{
				return this.Current == LongWayHomeFloorRoutine.Segment.Asleep;
			}
		}

		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x06005E7C RID: 24188 RVA: 0x00255DFF File Offset: 0x00253FFF
		public bool HasStood
		{
			get
			{
				return this.Current == LongWayHomeFloorRoutine.Segment.Standing;
			}
		}

		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x06005E7D RID: 24189 RVA: 0x00255E0B File Offset: 0x0025400B
		public double TimeA
		{
			get
			{
				return this._time[0];
			}
		}

		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x06005E7E RID: 24190 RVA: 0x00255E15 File Offset: 0x00254015
		public double TimeB
		{
			get
			{
				return this._time[1];
			}
		}

		// Token: 0x17000F46 RID: 3910
		// (get) Token: 0x06005E7F RID: 24191 RVA: 0x00255E1F File Offset: 0x0025401F
		public float WeightB
		{
			get
			{
				if (this._active != 1)
				{
					return 1f - Mathf.SmoothStep(0f, 1f, this._blend);
				}
				return Mathf.SmoothStep(0f, 1f, this._blend);
			}
		}

		// Token: 0x17000F47 RID: 3911
		// (get) Token: 0x06005E80 RID: 24192 RVA: 0x00255E5B File Offset: 0x0025405B
		public double ActiveTime
		{
			get
			{
				return this._time[this._active];
			}
		}

		// Token: 0x06005E81 RID: 24193 RVA: 0x00255E6A File Offset: 0x0025406A
		public LongWayHomeFloorRoutine(LongWayHomeUniversityConfigSO config)
		{
			this._config = config;
		}

		// Token: 0x06005E82 RID: 24194 RVA: 0x00255EA8 File Offset: 0x002540A8
		public void StartFall()
		{
			this._active = 0;
			this._time[0] = (this._time[1] = 0.0);
			this._speed[0] = (this._speed[1] = 0f);
			this._blend = 1f;
			this._switchRequested = (this._sleepRequested = false);
			this.Current = LongWayHomeFloorRoutine.Segment.Fall;
		}

		// Token: 0x06005E83 RID: 24195 RVA: 0x00255F14 File Offset: 0x00254114
		public void StartAsleep()
		{
			this._active = 0;
			this._time[0] = (this._time[1] = (double)this._config.sleepPose);
			this._speed[0] = (this._speed[1] = 0f);
			this._blend = 1f;
			this._switchRequested = (this._sleepRequested = false);
			this.Current = LongWayHomeFloorRoutine.Segment.Asleep;
		}

		// Token: 0x06005E84 RID: 24196 RVA: 0x00255F81 File Offset: 0x00254181
		public bool RequestSwitch()
		{
			if (!this.CanSwitch)
			{
				return false;
			}
			this._switchRequested = true;
			return true;
		}

		// Token: 0x06005E85 RID: 24197 RVA: 0x00255F95 File Offset: 0x00254195
		public bool RequestSleep()
		{
			if (!this.CanSleep)
			{
				return false;
			}
			this._sleepRequested = true;
			this._switchRequested = false;
			return true;
		}

		// Token: 0x06005E86 RID: 24198 RVA: 0x00255FB0 File Offset: 0x002541B0
		public bool StartGetUp()
		{
			if (this.Current != LongWayHomeFloorRoutine.Segment.Asleep)
			{
				return false;
			}
			this.CrossFadeTo(this._config.getUp.x, 0.6f);
			this.Current = LongWayHomeFloorRoutine.Segment.GetUp;
			return true;
		}

		// Token: 0x06005E87 RID: 24199 RVA: 0x00255FE0 File Offset: 0x002541E0
		public void Tick(float deltaTime)
		{
			if (deltaTime <= 0f)
			{
				return;
			}
			int num = 1 - this._active;
			if (this._blend < 1f)
			{
				this._blend = Mathf.Min(1f, this._blend + deltaTime / this._blendSeconds);
				this._time[num] += (double)(deltaTime * this._speed[num]);
			}
			float num2 = this.SegmentSpeed();
			this._speed[this._active] = num2;
			this._time[this._active] += (double)(deltaTime * num2);
			double num3 = this._time[this._active];
			switch (this.Current)
			{
			case LongWayHomeFloorRoutine.Segment.Fall:
				if (num3 >= (double)this._config.bellyLoop.x)
				{
					this.Current = LongWayHomeFloorRoutine.Segment.Belly;
					return;
				}
				break;
			case LongWayHomeFloorRoutine.Segment.Belly:
				if (this._sleepRequested || this._switchRequested)
				{
					this.CrossFadeTo(this._config.rollToBackStart, this._config.switchBlendSeconds);
					this._switchRequested = false;
					this.Current = LongWayHomeFloorRoutine.Segment.ToBack;
					return;
				}
				this.WrapLoop(this._config.bellyLoop);
				return;
			case LongWayHomeFloorRoutine.Segment.ToBack:
				if (num3 >= (double)this._config.backLoop.x)
				{
					this.Current = (this._sleepRequested ? this.BeginSettle() : LongWayHomeFloorRoutine.Segment.Back);
					return;
				}
				break;
			case LongWayHomeFloorRoutine.Segment.Back:
				if (this._sleepRequested)
				{
					this.Current = this.BeginSettle();
					return;
				}
				if (this._switchRequested)
				{
					this.CrossFadeTo(this._config.rollToBellyStart, this._config.switchBlendSeconds);
					this._switchRequested = false;
					this.Current = LongWayHomeFloorRoutine.Segment.ToBelly;
					return;
				}
				this.WrapLoop(this._config.backLoop);
				return;
			case LongWayHomeFloorRoutine.Segment.ToBelly:
				if (num3 >= (double)this._config.rollToBellyEnd)
				{
					this.CrossFadeTo(this._config.riseToElbowsStart, this._config.reentryBlendSeconds);
					this.Current = LongWayHomeFloorRoutine.Segment.Rise;
					return;
				}
				break;
			case LongWayHomeFloorRoutine.Segment.Rise:
				if (num3 >= (double)this._config.bellyLoop.x)
				{
					this.Current = LongWayHomeFloorRoutine.Segment.Belly;
					return;
				}
				break;
			case LongWayHomeFloorRoutine.Segment.Settle:
				if (this._blend >= 1f)
				{
					this.Current = LongWayHomeFloorRoutine.Segment.Asleep;
					return;
				}
				break;
			case LongWayHomeFloorRoutine.Segment.Asleep:
				break;
			case LongWayHomeFloorRoutine.Segment.GetUp:
				if (num3 >= (double)this._config.getUp.y)
				{
					this._time[this._active] = (double)this._config.getUp.y;
					this.Current = LongWayHomeFloorRoutine.Segment.Standing;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06005E88 RID: 24200 RVA: 0x0025624A File Offset: 0x0025444A
		private LongWayHomeFloorRoutine.Segment BeginSettle()
		{
			this._sleepRequested = false;
			this.CrossFadeTo(this._config.sleepPose, Mathf.Max(0.2f, this._config.sleepSettleSeconds));
			return LongWayHomeFloorRoutine.Segment.Settle;
		}

		// Token: 0x06005E89 RID: 24201 RVA: 0x0025627C File Offset: 0x0025447C
		private float SegmentSpeed()
		{
			switch (this.Current)
			{
			case LongWayHomeFloorRoutine.Segment.Fall:
				return Mathf.Max(0.05f, (this._config.fallPlaybackSpeed != null && this._config.fallPlaybackSpeed.length > 0) ? this._config.fallPlaybackSpeed.Evaluate((float)this._time[this._active]) : 1f);
			case LongWayHomeFloorRoutine.Segment.ToBack:
			case LongWayHomeFloorRoutine.Segment.ToBelly:
			case LongWayHomeFloorRoutine.Segment.Rise:
				return this._config.rollPlaybackSpeed;
			case LongWayHomeFloorRoutine.Segment.Settle:
			case LongWayHomeFloorRoutine.Segment.Asleep:
			case LongWayHomeFloorRoutine.Segment.Standing:
				return 0f;
			case LongWayHomeFloorRoutine.Segment.GetUp:
				return this._config.getUpPlaybackSpeed;
			}
			return 1f;
		}

		// Token: 0x06005E8A RID: 24202 RVA: 0x00256334 File Offset: 0x00254534
		private void WrapLoop(Vector2 loop)
		{
			if (this._blend < 1f)
			{
				return;
			}
			float num = Mathf.Min(this._config.loopBlendSeconds, (loop.y - loop.x) * 0.4f);
			if (this._time[this._active] >= (double)(loop.y - num))
			{
				this.CrossFadeTo(loop.x, num);
			}
		}

		// Token: 0x06005E8B RID: 24203 RVA: 0x00256398 File Offset: 0x00254598
		private void CrossFadeTo(float clipSeconds, float seconds)
		{
			int num = ((this._blend < 0.5f) ? (1 - this._active) : this._active);
			int num2 = 1 - num;
			this._time[num2] = (double)clipSeconds;
			this._speed[num2] = 0f;
			this._active = num2;
			this._blendSeconds = Mathf.Max(0.01f, seconds);
			this._blend = 0f;
		}

		// Token: 0x04005F93 RID: 24467
		private readonly LongWayHomeUniversityConfigSO _config;

		// Token: 0x04005F94 RID: 24468
		private readonly double[] _time = new double[2];

		// Token: 0x04005F95 RID: 24469
		private readonly float[] _speed = new float[2];

		// Token: 0x04005F96 RID: 24470
		private int _active;

		// Token: 0x04005F97 RID: 24471
		private float _blend = 1f;

		// Token: 0x04005F98 RID: 24472
		private float _blendSeconds = 0.01f;

		// Token: 0x04005F99 RID: 24473
		private bool _switchRequested;

		// Token: 0x04005F9A RID: 24474
		private bool _sleepRequested;

		// Token: 0x02000CB9 RID: 3257
		public enum Segment
		{
			// Token: 0x04005F9D RID: 24477
			Fall,
			// Token: 0x04005F9E RID: 24478
			Belly,
			// Token: 0x04005F9F RID: 24479
			ToBack,
			// Token: 0x04005FA0 RID: 24480
			Back,
			// Token: 0x04005FA1 RID: 24481
			ToBelly,
			// Token: 0x04005FA2 RID: 24482
			Rise,
			// Token: 0x04005FA3 RID: 24483
			Settle,
			// Token: 0x04005FA4 RID: 24484
			Asleep,
			// Token: 0x04005FA5 RID: 24485
			GetUp,
			// Token: 0x04005FA6 RID: 24486
			Standing
		}
	}
}
