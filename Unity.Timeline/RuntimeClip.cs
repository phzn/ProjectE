using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000021 RID: 33
	internal class RuntimeClip : RuntimeClipBase
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00007E0C File Offset: 0x0000600C
		public override double start
		{
			get
			{
				return this.m_Clip.extrapolatedStart;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00007E19 File Offset: 0x00006019
		public override double duration
		{
			get
			{
				return this.m_Clip.extrapolatedDuration;
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00007E26 File Offset: 0x00006026
		public RuntimeClip(TimelineClip clip, Playable clipPlayable, Playable parentMixer)
		{
			this.Create(clip, clipPlayable, parentMixer);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00007E37 File Offset: 0x00006037
		private void Create(TimelineClip clip, Playable clipPlayable, Playable parentMixer)
		{
			this.m_Clip = clip;
			this.m_Playable = clipPlayable;
			this.m_ParentMixer = parentMixer;
			clipPlayable.Pause<Playable>();
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000205 RID: 517 RVA: 0x00007E54 File Offset: 0x00006054
		public TimelineClip clip
		{
			get
			{
				return this.m_Clip;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00007E5C File Offset: 0x0000605C
		public Playable mixer
		{
			get
			{
				return this.m_ParentMixer;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00007E64 File Offset: 0x00006064
		public Playable playable
		{
			get
			{
				return this.m_Playable;
			}
		}

		// Token: 0x17000096 RID: 150
		// (set) Token: 0x06000208 RID: 520 RVA: 0x00007E6C File Offset: 0x0000606C
		public override bool enable
		{
			set
			{
				if (value && this.m_Playable.GetPlayState<Playable>() != PlayState.Playing)
				{
					this.m_Playable.Play<Playable>();
					this.SetTime(this.m_Clip.clipIn);
					return;
				}
				if (!value && this.m_Playable.GetPlayState<Playable>() != PlayState.Paused)
				{
					this.m_Playable.Pause<Playable>();
					if (this.m_ParentMixer.IsValid<Playable>())
					{
						this.m_ParentMixer.SetInputWeight(this.m_Playable, 0f);
					}
				}
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00007EE5 File Offset: 0x000060E5
		public void SetTime(double time)
		{
			this.m_Playable.SetTime(time);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00007EF3 File Offset: 0x000060F3
		public void SetDuration(double duration)
		{
			this.m_Playable.SetDuration(duration);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00007F04 File Offset: 0x00006104
		public override void EvaluateAt(double localTime, FrameData frameData)
		{
			this.enable = true;
			if (frameData.timeLooped)
			{
				this.SetTime(this.clip.clipIn);
				this.SetTime(this.clip.clipIn);
			}
			float num;
			if (this.clip.IsPreExtrapolatedTime(localTime))
			{
				num = this.clip.EvaluateMixIn((double)((float)this.clip.start));
			}
			else if (this.clip.IsPostExtrapolatedTime(localTime))
			{
				num = this.clip.EvaluateMixOut((double)((float)this.clip.end));
			}
			else
			{
				num = this.clip.EvaluateMixIn(localTime) * this.clip.EvaluateMixOut(localTime);
			}
			if (this.mixer.IsValid<Playable>())
			{
				this.mixer.SetInputWeight(this.playable, num);
			}
			double num2 = this.clip.ToLocalTime(localTime);
			if (num2 >= -DiscreteTime.tickValue / 2.0)
			{
				this.SetTime(num2);
			}
			this.SetDuration(this.clip.extrapolatedDuration);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000800C File Offset: 0x0000620C
		public override void DisableAt(double localTime, double rootDuration, FrameData frameData)
		{
			double num = Math.Min(localTime, (double)DiscreteTime.FromTicks(this.intervalEnd));
			if (frameData.timeLooped)
			{
				num = Math.Min(num, rootDuration);
			}
			double num2 = this.clip.ToLocalTime(num);
			if (num2 > -DiscreteTime.tickValue / 2.0)
			{
				this.SetTime(num2);
			}
			this.enable = false;
		}

		// Token: 0x040000BC RID: 188
		private TimelineClip m_Clip;

		// Token: 0x040000BD RID: 189
		private Playable m_Playable;

		// Token: 0x040000BE RID: 190
		private Playable m_ParentMixer;
	}
}
