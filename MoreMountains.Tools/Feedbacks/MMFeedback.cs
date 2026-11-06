using System;
using System.Collections;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001A5 RID: 421
	[AddComponentMenu("")]
	[ExecuteAlways]
	[Serializable]
	public abstract class MMFeedback : MonoBehaviour
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060009E0 RID: 2528 RVA: 0x0002DFEE File Offset: 0x0002C1EE
		// (set) Token: 0x060009E1 RID: 2529 RVA: 0x0002DFF6 File Offset: 0x0002C1F6
		public GameObject Owner { get; set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060009E2 RID: 2530 RVA: 0x0002DFFF File Offset: 0x0002C1FF
		public virtual IEnumerator Pause
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x0002E002 File Offset: 0x0002C202
		public virtual bool HoldingPause
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060009E4 RID: 2532 RVA: 0x0002E005 File Offset: 0x0002C205
		public virtual bool LooperPause
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x0002E008 File Offset: 0x0002C208
		// (set) Token: 0x060009E6 RID: 2534 RVA: 0x0002E010 File Offset: 0x0002C210
		public virtual bool ScriptDrivenPause { get; set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x0002E019 File Offset: 0x0002C219
		// (set) Token: 0x060009E8 RID: 2536 RVA: 0x0002E021 File Offset: 0x0002C221
		public virtual float ScriptDrivenPauseAutoResume { get; set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x0002E02A File Offset: 0x0002C22A
		public virtual bool LooperStart
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060009EA RID: 2538 RVA: 0x0002E02D File Offset: 0x0002C22D
		public virtual bool InCooldown
		{
			get
			{
				return this.Timing.CooldownDuration > 0f && this.FeedbackTime - this._lastPlayTimestamp < this.Timing.CooldownDuration;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060009EB RID: 2539 RVA: 0x0002E05D File Offset: 0x0002C25D
		// (set) Token: 0x060009EC RID: 2540 RVA: 0x0002E065 File Offset: 0x0002C265
		public virtual bool IsPlaying { get; set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060009ED RID: 2541 RVA: 0x0002E06E File Offset: 0x0002C26E
		public float FeedbackTime
		{
			get
			{
				if (this.Timing.TimescaleMode == TimescaleModes.Scaled)
				{
					return Time.time;
				}
				return Time.unscaledTime;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060009EE RID: 2542 RVA: 0x0002E088 File Offset: 0x0002C288
		public float FeedbackDeltaTime
		{
			get
			{
				if (this.Timing.TimescaleMode == TimescaleModes.Scaled)
				{
					return Time.deltaTime;
				}
				return Time.unscaledDeltaTime;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060009EF RID: 2543 RVA: 0x0002E0A4 File Offset: 0x0002C2A4
		public float TotalDuration
		{
			get
			{
				if (this.Timing != null && !this.Timing.ContributeToTotalDuration)
				{
					return 0f;
				}
				float num = 0f;
				if (this.Timing == null)
				{
					return 0f;
				}
				if (this.Timing.InitialDelay != 0f)
				{
					num += this.ApplyTimeMultiplier(this.Timing.InitialDelay);
				}
				num += this.FeedbackDuration;
				if (this.Timing.NumberOfRepeats > 0)
				{
					float num2 = this.ApplyTimeMultiplier(this.Timing.DelayBetweenRepeats);
					num += (float)this.Timing.NumberOfRepeats * this.FeedbackDuration + (float)this.Timing.NumberOfRepeats * num2;
				}
				return num;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060009F0 RID: 2544 RVA: 0x0002E155 File Offset: 0x0002C355
		public virtual float FeedbackStartedAt
		{
			get
			{
				return this._lastPlayTimestamp;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060009F1 RID: 2545 RVA: 0x0002E15D File Offset: 0x0002C35D
		// (set) Token: 0x060009F2 RID: 2546 RVA: 0x0002E164 File Offset: 0x0002C364
		public virtual float FeedbackDuration
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060009F3 RID: 2547 RVA: 0x0002E166 File Offset: 0x0002C366
		public virtual bool FeedbackPlaying
		{
			get
			{
				return this.FeedbackStartedAt > 0f && Time.time - this.FeedbackStartedAt < this.FeedbackDuration;
			}
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0002E18B File Offset: 0x0002C38B
		public virtual MMChannelData ChannelData(int channel)
		{
			return this._channelData.Set(MMChannelModes.Int, channel, null);
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0002E19B File Offset: 0x0002C39B
		protected virtual void OnEnable()
		{
			this._hostMMFeedbacks = base.gameObject.GetComponent<MMFeedbacks>();
			this._isHostMMFeedbacksNotNull = this._hostMMFeedbacks != null;
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x0002E1C0 File Offset: 0x0002C3C0
		public virtual void Initialization(GameObject owner)
		{
			this._initialized = true;
			this.Owner = owner;
			this._playsLeft = this.Timing.NumberOfRepeats + 1;
			this._hostMMFeedbacks = base.gameObject.GetComponent<MMFeedbacks>();
			this._channelData = new MMChannelData(MMChannelModes.Int, 0, null);
			this.SetInitialDelay(this.Timing.InitialDelay);
			this.SetDelayBetweenRepeats(this.Timing.DelayBetweenRepeats);
			this.SetSequence(this.Timing.Sequence);
			this.CustomInitialization(owner);
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0002E248 File Offset: 0x0002C448
		public virtual void Play(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active)
			{
				return;
			}
			if (!this._initialized)
			{
				Debug.LogWarning("The " + ((this != null) ? this.ToString() : null) + " feedback is being played without having been initialized. Call Initialization() first.");
			}
			if (this.InCooldown)
			{
				return;
			}
			if (this.Timing.InitialDelay > 0f)
			{
				this._playCoroutine = base.StartCoroutine(this.PlayCoroutine(position, feedbacksIntensity));
				return;
			}
			this._lastPlayTimestamp = this.FeedbackTime;
			this.RegularPlay(position, feedbacksIntensity);
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0002E2CB File Offset: 0x0002C4CB
		protected virtual IEnumerator PlayCoroutine(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.Timing.TimescaleMode == TimescaleModes.Scaled)
			{
				yield return MMFeedbacksCoroutine.WaitFor(this.Timing.InitialDelay);
			}
			else
			{
				yield return MMFeedbacksCoroutine.WaitForUnscaled(this.Timing.InitialDelay);
			}
			this._lastPlayTimestamp = this.FeedbackTime;
			this.RegularPlay(position, feedbacksIntensity);
			yield break;
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0002E2E8 File Offset: 0x0002C4E8
		protected virtual void RegularPlay(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.Chance == 0f)
			{
				return;
			}
			if (this.Chance != 100f && Random.Range(0f, 100f) > this.Chance)
			{
				return;
			}
			if (this.Timing.UseIntensityInterval && (feedbacksIntensity < this.Timing.IntensityIntervalMin || feedbacksIntensity >= this.Timing.IntensityIntervalMax))
			{
				return;
			}
			if (this.Timing.RepeatForever)
			{
				this._infinitePlayCoroutine = base.StartCoroutine(this.InfinitePlay(position, feedbacksIntensity));
				return;
			}
			if (this.Timing.NumberOfRepeats > 0)
			{
				this._repeatedPlayCoroutine = base.StartCoroutine(this.RepeatedPlay(position, feedbacksIntensity));
				return;
			}
			if (this.Timing.Sequence == null)
			{
				this.CustomPlayFeedback(position, feedbacksIntensity);
				return;
			}
			this._sequenceCoroutine = base.StartCoroutine(this.SequenceCoroutine(position, feedbacksIntensity));
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0002E3C7 File Offset: 0x0002C5C7
		protected virtual IEnumerator InfinitePlay(Vector3 position, float feedbacksIntensity = 1f)
		{
			for (;;)
			{
				this._lastPlayTimestamp = this.FeedbackTime;
				if (this.Timing.Sequence == null)
				{
					this.CustomPlayFeedback(position, feedbacksIntensity);
					if (this.Timing.TimescaleMode == TimescaleModes.Scaled)
					{
						yield return MMFeedbacksCoroutine.WaitFor(this.Timing.DelayBetweenRepeats);
					}
					else
					{
						yield return MMFeedbacksCoroutine.WaitForUnscaled(this.Timing.DelayBetweenRepeats);
					}
				}
				else
				{
					this._sequenceCoroutine = base.StartCoroutine(this.SequenceCoroutine(position, feedbacksIntensity));
					float num = this.ApplyTimeMultiplier(this.Timing.DelayBetweenRepeats) + this.Timing.Sequence.Length;
					if (this.Timing.TimescaleMode == TimescaleModes.Scaled)
					{
						yield return MMFeedbacksCoroutine.WaitFor(num);
					}
					else
					{
						yield return MMFeedbacksCoroutine.WaitForUnscaled(num);
					}
				}
			}
			yield break;
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0002E3E4 File Offset: 0x0002C5E4
		protected virtual IEnumerator RepeatedPlay(Vector3 position, float feedbacksIntensity = 1f)
		{
			while (this._playsLeft > 0)
			{
				this._lastPlayTimestamp = this.FeedbackTime;
				this._playsLeft--;
				if (this.Timing.Sequence == null)
				{
					this.CustomPlayFeedback(position, feedbacksIntensity);
					if (this.Timing.TimescaleMode == TimescaleModes.Scaled)
					{
						yield return MMFeedbacksCoroutine.WaitFor(this.Timing.DelayBetweenRepeats);
					}
					else
					{
						yield return MMFeedbacksCoroutine.WaitForUnscaled(this.Timing.DelayBetweenRepeats);
					}
				}
				else
				{
					this._sequenceCoroutine = base.StartCoroutine(this.SequenceCoroutine(position, feedbacksIntensity));
					float num = this.ApplyTimeMultiplier(this.Timing.DelayBetweenRepeats) + this.Timing.Sequence.Length;
					if (this.Timing.TimescaleMode == TimescaleModes.Scaled)
					{
						yield return MMFeedbacksCoroutine.WaitFor(num);
					}
					else
					{
						yield return MMFeedbacksCoroutine.WaitForUnscaled(num);
					}
				}
			}
			this._playsLeft = this.Timing.NumberOfRepeats + 1;
			yield break;
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0002E401 File Offset: 0x0002C601
		protected virtual IEnumerator SequenceCoroutine(Vector3 position, float feedbacksIntensity = 1f)
		{
			yield return null;
			float timeStartedAt = this.FeedbackTime;
			float lastFrame = this.FeedbackTime;
			this.BeatThisFrame = false;
			this.LastBeatIndex = 0;
			this.CurrentSequenceIndex = 0;
			this.LastBeatTimestamp = 0f;
			if (this.Timing.Quantized)
			{
				while (this.CurrentSequenceIndex < this.Timing.Sequence.QuantizedSequence[0].Line.Count)
				{
					this._beatInterval = 60f / (float)this.Timing.TargetBPM;
					if (this.FeedbackTime - this.LastBeatTimestamp >= this._beatInterval || this.LastBeatTimestamp == 0f)
					{
						this.BeatThisFrame = true;
						this.LastBeatIndex = this.CurrentSequenceIndex;
						this.LastBeatTimestamp = this.FeedbackTime;
						for (int i = 0; i < this.Timing.Sequence.SequenceTracks.Count; i++)
						{
							if (this.Timing.Sequence.QuantizedSequence[i].Line[this.CurrentSequenceIndex].ID == this.Timing.TrackID)
							{
								this.CustomPlayFeedback(position, feedbacksIntensity);
							}
						}
						this.CurrentSequenceIndex++;
					}
					yield return null;
				}
			}
			else
			{
				while (this.FeedbackTime - timeStartedAt < this.Timing.Sequence.Length)
				{
					foreach (MMSequenceNote mmsequenceNote in this.Timing.Sequence.OriginalSequence.Line)
					{
						if (mmsequenceNote.ID == this.Timing.TrackID && mmsequenceNote.Timestamp >= lastFrame && mmsequenceNote.Timestamp <= this.FeedbackTime - timeStartedAt)
						{
							this.CustomPlayFeedback(position, feedbacksIntensity);
						}
					}
					lastFrame = this.FeedbackTime - timeStartedAt;
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0002E420 File Offset: 0x0002C620
		public virtual void Stop(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this._playCoroutine != null)
			{
				base.StopCoroutine(this._playCoroutine);
			}
			if (this._infinitePlayCoroutine != null)
			{
				base.StopCoroutine(this._infinitePlayCoroutine);
			}
			if (this._repeatedPlayCoroutine != null)
			{
				base.StopCoroutine(this._repeatedPlayCoroutine);
			}
			if (this._sequenceCoroutine != null)
			{
				base.StopCoroutine(this._sequenceCoroutine);
			}
			this._lastPlayTimestamp = 0f;
			this._playsLeft = this.Timing.NumberOfRepeats + 1;
			if (this.Timing.InterruptsOnStop)
			{
				this.CustomStopFeedback(position, feedbacksIntensity);
			}
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x0002E4B0 File Offset: 0x0002C6B0
		public virtual void ResetFeedback()
		{
			this._playsLeft = this.Timing.NumberOfRepeats + 1;
			this.CustomReset();
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0002E4CC File Offset: 0x0002C6CC
		public virtual void SetSequence(MMSequence newSequence)
		{
			this.Timing.Sequence = newSequence;
			if (this.Timing.Sequence != null)
			{
				for (int i = 0; i < this.Timing.Sequence.SequenceTracks.Count; i++)
				{
					if (this.Timing.Sequence.SequenceTracks[i].ID == this.Timing.TrackID)
					{
						this._sequenceTrackID = i;
					}
				}
			}
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0002E547 File Offset: 0x0002C747
		public virtual void SetDelayBetweenRepeats(float delay)
		{
			this.Timing.DelayBetweenRepeats = delay;
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0002E555 File Offset: 0x0002C755
		public virtual void SetInitialDelay(float delay)
		{
			this.Timing.InitialDelay = delay;
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0002E563 File Offset: 0x0002C763
		protected virtual float ApplyDirection(float normalizedTime)
		{
			if (!this.NormalPlayDirection)
			{
				return 1f - normalizedTime;
			}
			return normalizedTime;
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000A03 RID: 2563 RVA: 0x0002E578 File Offset: 0x0002C778
		public virtual bool NormalPlayDirection
		{
			get
			{
				switch (this.Timing.PlayDirection)
				{
				case MMFeedbackTiming.PlayDirections.FollowMMFeedbacksDirection:
					return this._hostMMFeedbacks.Direction == MMFeedbacks.Directions.TopToBottom;
				case MMFeedbackTiming.PlayDirections.OppositeMMFeedbacksDirection:
					return this._hostMMFeedbacks.Direction > MMFeedbacks.Directions.TopToBottom;
				case MMFeedbackTiming.PlayDirections.AlwaysNormal:
					return true;
				case MMFeedbackTiming.PlayDirections.AlwaysRewind:
					return false;
				default:
					return true;
				}
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x0002E5CC File Offset: 0x0002C7CC
		public virtual bool ShouldPlayInThisSequenceDirection
		{
			get
			{
				switch (this.Timing.MMFeedbacksDirectionCondition)
				{
				case MMFeedbackTiming.MMFeedbacksDirectionConditions.Always:
					return true;
				case MMFeedbackTiming.MMFeedbacksDirectionConditions.OnlyWhenForwards:
					return this._hostMMFeedbacks.Direction == MMFeedbacks.Directions.TopToBottom;
				case MMFeedbackTiming.MMFeedbacksDirectionConditions.OnlyWhenBackwards:
					return this._hostMMFeedbacks.Direction == MMFeedbacks.Directions.BottomToTop;
				default:
					return true;
				}
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x0002E61A File Offset: 0x0002C81A
		protected virtual float FinalNormalizedTime
		{
			get
			{
				if (!this.NormalPlayDirection)
				{
					return 0f;
				}
				return 1f;
			}
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x0002E62F File Offset: 0x0002C82F
		protected virtual float ApplyTimeMultiplier(float duration)
		{
			if (this._isHostMMFeedbacksNotNull)
			{
				return this._hostMMFeedbacks.ApplyTimeMultiplier(duration);
			}
			return duration;
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0002E647 File Offset: 0x0002C847
		protected virtual void CustomInitialization(GameObject owner)
		{
		}

		// Token: 0x06000A08 RID: 2568
		protected abstract void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f);

		// Token: 0x06000A09 RID: 2569 RVA: 0x0002E649 File Offset: 0x0002C849
		protected virtual void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0002E64B File Offset: 0x0002C84B
		protected virtual void CustomReset()
		{
		}

		// Token: 0x04000C1F RID: 3103
		[Tooltip("whether or not this feedback is active")]
		public bool Active = true;

		// Token: 0x04000C20 RID: 3104
		[Tooltip("the name of this feedback to display in the inspector")]
		public string Label = "MMFeedback";

		// Token: 0x04000C21 RID: 3105
		[Tooltip("the chance of this feedback happening (in percent : 100 : happens all the time, 0 : never happens, 50 : happens once every two calls, etc)")]
		[Range(0f, 100f)]
		public float Chance = 100f;

		// Token: 0x04000C22 RID: 3106
		[Tooltip("a number of timing-related values (delay, repeat, etc)")]
		public MMFeedbackTiming Timing;

		// Token: 0x04000C24 RID: 3108
		[HideInInspector]
		public bool DebugActive;

		// Token: 0x04000C28 RID: 3112
		protected float _lastPlayTimestamp = -1f;

		// Token: 0x04000C29 RID: 3113
		protected int _playsLeft;

		// Token: 0x04000C2A RID: 3114
		protected bool _initialized;

		// Token: 0x04000C2B RID: 3115
		protected Coroutine _playCoroutine;

		// Token: 0x04000C2C RID: 3116
		protected Coroutine _infinitePlayCoroutine;

		// Token: 0x04000C2D RID: 3117
		protected Coroutine _sequenceCoroutine;

		// Token: 0x04000C2E RID: 3118
		protected Coroutine _repeatedPlayCoroutine;

		// Token: 0x04000C2F RID: 3119
		protected int _sequenceTrackID;

		// Token: 0x04000C30 RID: 3120
		protected MMFeedbacks _hostMMFeedbacks;

		// Token: 0x04000C31 RID: 3121
		protected float _beatInterval;

		// Token: 0x04000C32 RID: 3122
		protected bool BeatThisFrame;

		// Token: 0x04000C33 RID: 3123
		protected int LastBeatIndex;

		// Token: 0x04000C34 RID: 3124
		protected int CurrentSequenceIndex;

		// Token: 0x04000C35 RID: 3125
		protected float LastBeatTimestamp;

		// Token: 0x04000C36 RID: 3126
		protected bool _isHostMMFeedbacksNotNull;

		// Token: 0x04000C37 RID: 3127
		protected MMChannelData _channelData;
	}
}
