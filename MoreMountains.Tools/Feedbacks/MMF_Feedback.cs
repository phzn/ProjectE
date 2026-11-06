using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001C3 RID: 451
	[Serializable]
	public abstract class MMF_Feedback
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000AAB RID: 2731 RVA: 0x00030155 File Offset: 0x0002E355
		public virtual IEnumerator Pause
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000AAC RID: 2732 RVA: 0x00030158 File Offset: 0x0002E358
		public virtual bool HoldingPause
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000AAD RID: 2733 RVA: 0x0003015B File Offset: 0x0002E35B
		public virtual bool LooperPause
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x0003015E File Offset: 0x0002E35E
		// (set) Token: 0x06000AAF RID: 2735 RVA: 0x00030166 File Offset: 0x0002E366
		public virtual bool ScriptDrivenPause { get; set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x0003016F File Offset: 0x0002E36F
		// (set) Token: 0x06000AB1 RID: 2737 RVA: 0x00030177 File Offset: 0x0002E377
		public virtual float ScriptDrivenPauseAutoResume { get; set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x00030180 File Offset: 0x0002E380
		public virtual bool LooperStart
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x00030183 File Offset: 0x0002E383
		public virtual bool HasChannel
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x00030186 File Offset: 0x0002E386
		public virtual bool HasAutomaticShakerSetup
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x00030189 File Offset: 0x0002E389
		public virtual bool HasRandomness
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x0003018C File Offset: 0x0002E38C
		public virtual bool CanForceInitialValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x0003018F File Offset: 0x0002E38F
		public virtual bool ForceInitialValueDelayed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x00030192 File Offset: 0x0002E392
		public virtual bool HasAutomatedTargetAcquisition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x00030195 File Offset: 0x0002E395
		// (set) Token: 0x06000ABA RID: 2746 RVA: 0x0003019D File Offset: 0x0002E39D
		public virtual MMF_ReferenceHolder ForcedReferenceHolder { get; set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x000301A6 File Offset: 0x0002E3A6
		public virtual bool HasRange
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000ABC RID: 2748 RVA: 0x000301A9 File Offset: 0x0002E3A9
		public virtual int PlaysLeft
		{
			get
			{
				return this._playsLeft;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000ABD RID: 2749 RVA: 0x000301B1 File Offset: 0x0002E3B1
		public virtual bool HasCustomInspectors
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x000301B4 File Offset: 0x0002E3B4
		public virtual bool InCooldown
		{
			get
			{
				return this.Timing.CooldownDuration > 0f && this.FeedbackTime - this._lastPlayTimestamp < this.Timing.CooldownDuration;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x000301E4 File Offset: 0x0002E3E4
		// (set) Token: 0x06000AC0 RID: 2752 RVA: 0x000301EC File Offset: 0x0002E3EC
		public virtual bool IsPlaying { get; set; }

		// Token: 0x06000AC1 RID: 2753 RVA: 0x000301F5 File Offset: 0x0002E3F5
		public virtual float ComputeIntensity(float intensity, Vector3 position)
		{
			return (this.Timing.ConstantIntensity ? 1f : intensity) * this.ComputedRandomMultiplier * this.Owner.ComputeRangeIntensityMultiplier(position);
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x00030220 File Offset: 0x0002E420
		public virtual float ComputedRandomMultiplier
		{
			get
			{
				if (!this.RandomizeOutput)
				{
					return 1f;
				}
				return Random.Range(this.RandomMultiplier.x, this.RandomMultiplier.y);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x0003024B File Offset: 0x0002E44B
		public virtual TimescaleModes ComputedTimescaleMode
		{
			get
			{
				if (this.Owner.ForceTimescaleMode)
				{
					return this.Owner.ForcedTimescaleMode;
				}
				return this.Timing.TimescaleMode;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x00030271 File Offset: 0x0002E471
		public virtual bool InScaledTimescaleMode
		{
			get
			{
				if (this.Owner.ForceTimescaleMode)
				{
					return this.Owner.ForcedTimescaleMode == TimescaleModes.Scaled;
				}
				return this.Timing.TimescaleMode == TimescaleModes.Scaled;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x000302A0 File Offset: 0x0002E4A0
		public virtual float FeedbackTime
		{
			get
			{
				float timescaleMultiplier = this.Owner.TimescaleMultiplier;
				if (this.Timing.UseScriptDrivenTimescale)
				{
					return this.Timing.ScriptDrivenTime * timescaleMultiplier;
				}
				if (this.Owner.ForceTimescaleMode)
				{
					if (this.Owner.ForcedTimescaleMode == TimescaleModes.Scaled)
					{
						return Time.time * timescaleMultiplier;
					}
					return Time.unscaledTime * timescaleMultiplier;
				}
				else
				{
					if (this.Timing.TimescaleMode == TimescaleModes.Scaled)
					{
						return Time.time * timescaleMultiplier;
					}
					return Time.unscaledTime * timescaleMultiplier;
				}
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x0003031C File Offset: 0x0002E51C
		public virtual float FeedbackDeltaTime
		{
			get
			{
				float timescaleMultiplier = this.Owner.TimescaleMultiplier;
				if (this.Timing.UseScriptDrivenTimescale)
				{
					return this.Timing.ScriptDrivenDeltaTime * timescaleMultiplier;
				}
				if (this.Owner.ForceTimescaleMode)
				{
					if (this.Owner.ForcedTimescaleMode == TimescaleModes.Scaled)
					{
						return Time.deltaTime * timescaleMultiplier;
					}
					return Time.unscaledDeltaTime * timescaleMultiplier;
				}
				else
				{
					if (this.Owner.SkippingToTheEnd)
					{
						return float.MaxValue;
					}
					if (this.Timing.TimescaleMode == TimescaleModes.Scaled)
					{
						return Time.deltaTime * timescaleMultiplier;
					}
					return Time.unscaledDeltaTime * timescaleMultiplier;
				}
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x000303A9 File Offset: 0x0002E5A9
		public virtual float TotalDuration
		{
			get
			{
				return this._totalDuration;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x000303B1 File Offset: 0x0002E5B1
		// (set) Token: 0x06000AC9 RID: 2761 RVA: 0x000303B9 File Offset: 0x0002E5B9
		public virtual bool IsExpanded { get; set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000ACA RID: 2762 RVA: 0x000303C2 File Offset: 0x0002E5C2
		public virtual bool RequiresSetup
		{
			get
			{
				return this._requiresSetup;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000ACB RID: 2763 RVA: 0x000303CA File Offset: 0x0002E5CA
		public virtual string RequiredTarget
		{
			get
			{
				return this._requiredTarget;
			}
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x000303D2 File Offset: 0x0002E5D2
		public virtual void CacheRequiresSetup()
		{
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000ACD RID: 2765 RVA: 0x000303D4 File Offset: 0x0002E5D4
		public virtual bool DrawGroupInspectors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000ACE RID: 2766 RVA: 0x000303D7 File Offset: 0x0002E5D7
		public virtual bool DisplayFullHeaderColor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x000303DA File Offset: 0x0002E5DA
		public virtual string RequiresSetupText
		{
			get
			{
				return "This feedback requires some additional setup.";
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x000303E1 File Offset: 0x0002E5E1
		public virtual string RequiredTargetText
		{
			get
			{
				return "";
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x000303E8 File Offset: 0x0002E5E8
		public virtual string RequiredTargetTextExtra
		{
			get
			{
				return "";
			}
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x000303EF File Offset: 0x0002E5EF
		public virtual bool EvaluateRequiresSetup()
		{
			return false;
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x000303F2 File Offset: 0x0002E5F2
		public virtual string RequiredChannelText
		{
			get
			{
				if (this.ChannelMode != MMChannelModes.MMChannel)
				{
					return "Channel " + this.Channel.ToString();
				}
				if (this.MMChannelDefinition == null)
				{
					return "None";
				}
				return this.MMChannelDefinition.name;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x00030432 File Offset: 0x0002E632
		public virtual float FeedbackStartedAt
		{
			get
			{
				if (!Application.isPlaying)
				{
					return -1f;
				}
				return this._lastPlayTimestamp;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x00030447 File Offset: 0x0002E647
		// (set) Token: 0x06000AD6 RID: 2774 RVA: 0x0003044E File Offset: 0x0002E64E
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

		// Token: 0x06000AD7 RID: 2775 RVA: 0x00030450 File Offset: 0x0002E650
		public virtual void SetFeedbackDuration(float newDuration)
		{
			this.FeedbackDuration = newDuration;
			this.Owner.ComputeCachedTotalDuration();
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x00030464 File Offset: 0x0002E664
		public virtual bool FeedbackPlaying
		{
			get
			{
				return this.FeedbackStartedAt > 0f && Time.time - this.FeedbackStartedAt < this.FeedbackDuration;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x00030489 File Offset: 0x0002E689
		public virtual MMChannelData ChannelData
		{
			get
			{
				return this._channelData.Set(this.ChannelMode, this.Channel, this.MMChannelDefinition);
			}
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x000304A8 File Offset: 0x0002E6A8
		public virtual void PreInitialization(MMF_Player owner, int index)
		{
			this._channelData = new MMChannelData(this.ChannelMode, this.Channel, this.MMChannelDefinition);
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x000304C8 File Offset: 0x0002E6C8
		public virtual void Initialization(MMF_Player owner, int index)
		{
			if (this.Timing == null)
			{
				this.Timing = new MMFeedbackTiming();
			}
			this.SetIndexInFeedbacksList(index);
			this._lastPlayTimestamp = -1f;
			this.Timing.PlayCount = 0;
			this._initialized = true;
			this.Owner = owner;
			this._playsLeft = this.Timing.NumberOfRepeats + 1;
			this._repeatOffset = 0f;
			this._channelData = new MMChannelData(this.ChannelMode, this.Channel, this.MMChannelDefinition);
			this.AutomateTargetAcquisitionInternal();
			this.SetInitialDelay(this.Timing.InitialDelay);
			this.SetDelayBetweenRepeats(this.Timing.DelayBetweenRepeats);
			this.SetSequence(this.Timing.Sequence);
			this.CustomInitialization(owner);
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0003058F File Offset: 0x0002E78F
		public virtual void SetIndexInFeedbacksList(int index)
		{
			this._indexInOwnerFeedbackList = index;
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x00030598 File Offset: 0x0002E798
		public virtual void AutomaticShakerSetup()
		{
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0003059A File Offset: 0x0002E79A
		protected virtual void AutomateTargetAcquisitionInternal()
		{
			if (!this.HasAutomatedTargetAcquisition)
			{
				return;
			}
			if (this.AutomatedTargetAcquisition == null)
			{
				this.AutomatedTargetAcquisition = new MMFeedbackTargetAcquisition();
			}
			if (this.AutomatedTargetAcquisition.Mode == MMFeedbackTargetAcquisition.Modes.None)
			{
				return;
			}
			this.AutomateTargetAcquisition();
			this.CacheRequiresSetup();
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x000305D2 File Offset: 0x0002E7D2
		public virtual void ForceAutomateTargetAcquisition()
		{
			this.AutomateTargetAcquisition();
			this.CacheRequiresSetup();
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x000305E0 File Offset: 0x0002E7E0
		protected virtual void AutomateTargetAcquisition()
		{
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x000305E2 File Offset: 0x0002E7E2
		protected virtual GameObject FindAutomatedTargetGameObject()
		{
			return MMFeedbackTargetAcquisition.FindAutomatedTargetGameObject(this.AutomatedTargetAcquisition, this.Owner, this._indexInOwnerFeedbackList);
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x000305FB File Offset: 0x0002E7FB
		protected virtual T FindAutomatedTarget<T>()
		{
			return MMFeedbackTargetAcquisition.FindAutomatedTarget<T>(this.AutomatedTargetAcquisition, this.Owner, this._indexInOwnerFeedbackList);
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00030614 File Offset: 0x0002E814
		public virtual void Play(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active)
			{
				return;
			}
			if (!this._initialized)
			{
				string text = this.ToString().Replace("MoreMountains.Feedbacks.", "");
				Debug.LogWarning(string.Concat(new string[]
				{
					"The ",
					text,
					" feedback on ",
					this.Owner.gameObject.name,
					" is being played without having been initialized. Always call the Initialization() method first. This can be done manually, or on Start or Awake (automatically on Start is the default). If you're auto playing your feedback on Start or on Enable, initialize on Awake (which runs before Start and Enable). You can change that setting on your MMF Player, unfold the Settings foldout at the top, and change the Initialization Mode."
				}), this.Owner.gameObject);
			}
			if (this.InCooldown)
			{
				return;
			}
			if (this.Timing.InitialDelay > 0f)
			{
				this._playCoroutine = this.Owner.StartCoroutine(this.PlayCoroutine(position, feedbacksIntensity));
				return;
			}
			this.RegularPlay(position, feedbacksIntensity);
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x000306CF File Offset: 0x0002E8CF
		protected virtual IEnumerator PlayCoroutine(Vector3 position, float feedbacksIntensity = 1f)
		{
			yield return this.WaitFor(this.ApplyTimeMultiplier(this.Timing.InitialDelay));
			this.RegularPlay(position, feedbacksIntensity);
			yield break;
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x000306EC File Offset: 0x0002E8EC
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
			if (this.Timing.LimitPlayCount && this.Timing.PlayCount >= this.Timing.MaxPlayCount)
			{
				return;
			}
			if (this.Timing.UseIntensityInterval && (feedbacksIntensity < this.Timing.IntensityIntervalMin || feedbacksIntensity >= this.Timing.IntensityIntervalMax))
			{
				return;
			}
			this._repeatOffset = 0f;
			if (this.Timing.RepeatForever)
			{
				this._infinitePlayCoroutine = this.Owner.StartCoroutine(this.InfinitePlay(position, feedbacksIntensity));
				return;
			}
			if (this.Timing.NumberOfRepeats > 0)
			{
				this._repeatedPlayCoroutine = this.Owner.StartCoroutine(this.RepeatedPlay(position, feedbacksIntensity));
				return;
			}
			if (this.Timing.Sequence == null)
			{
				this.TriggerCustomPlay(position, feedbacksIntensity);
				return;
			}
			this._sequenceCoroutine = this.Owner.StartCoroutine(this.SequenceCoroutine(position, feedbacksIntensity));
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0003080B File Offset: 0x0002EA0B
		protected virtual void TriggerCustomPlay(Vector3 position, float intensity)
		{
			this.Timing.PlayCount++;
			this._lastPlayTimestamp = this.FeedbackTime;
			this.CustomPlayFeedback(position, intensity);
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00030834 File Offset: 0x0002EA34
		protected virtual IEnumerator InfinitePlay(Vector3 position, float feedbacksIntensity = 1f)
		{
			for (;;)
			{
				yield return this.TriggerRepeatedPlay(position, feedbacksIntensity);
			}
			yield break;
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00030851 File Offset: 0x0002EA51
		protected virtual IEnumerator RepeatedPlay(Vector3 position, float feedbacksIntensity = 1f)
		{
			while (this._playsLeft > 0)
			{
				this._playsLeft--;
				yield return this.TriggerRepeatedPlay(position, feedbacksIntensity);
			}
			this._playsLeft = this.Timing.NumberOfRepeats + 1;
			yield break;
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0003086E File Offset: 0x0002EA6E
		protected virtual IEnumerator TriggerRepeatedPlay(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.Timing.Sequence == null)
			{
				this.TriggerCustomPlay(position, feedbacksIntensity);
				float repeatStartTime = Time.time;
				float repeatDuration = this.Timing.DelayBetweenRepeats + this.FeedbackDuration;
				if (this._repeatOffset <= this.Timing.DelayBetweenRepeats)
				{
					repeatDuration = this.Timing.DelayBetweenRepeats + this.FeedbackDuration - this._repeatOffset;
				}
				yield return this.WaitFor(repeatDuration);
				yield return null;
				this._repeatOffset = Time.time - repeatStartTime - repeatDuration;
			}
			else
			{
				this._sequenceCoroutine = this.Owner.StartCoroutine(this.SequenceCoroutine(position, feedbacksIntensity));
				float num = this.ApplyTimeMultiplier(this.Timing.DelayBetweenRepeats) + this.Timing.Sequence.Length;
				yield return this.WaitFor(num);
			}
			yield break;
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x0003088B File Offset: 0x0002EA8B
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
								this.TriggerCustomPlay(position, feedbacksIntensity);
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
							this.TriggerCustomPlay(position, feedbacksIntensity);
						}
					}
					lastFrame = this.FeedbackTime - timeStartedAt;
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x000308A8 File Offset: 0x0002EAA8
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

		// Token: 0x06000AEC RID: 2796 RVA: 0x00030924 File Offset: 0x0002EB24
		public virtual void Stop(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this._playCoroutine != null)
			{
				this.Owner.StopCoroutine(this._playCoroutine);
			}
			if (this._infinitePlayCoroutine != null)
			{
				this.Owner.StopCoroutine(this._infinitePlayCoroutine);
			}
			if (this._repeatedPlayCoroutine != null)
			{
				this.Owner.StopCoroutine(this._repeatedPlayCoroutine);
			}
			if (this._sequenceCoroutine != null)
			{
				this.Owner.StopCoroutine(this._sequenceCoroutine);
			}
			this._lastPlayTimestamp = -1f;
			this._playsLeft = this.Timing.NumberOfRepeats + 1;
			if (this.Timing.InterruptsOnStop)
			{
				this.CustomStopFeedback(position, feedbacksIntensity);
			}
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x000309C8 File Offset: 0x0002EBC8
		public virtual void SkipToTheEnd(Vector3 position, float feedbacksIntensity = 1f)
		{
			this.CustomSkipToTheEnd(position, feedbacksIntensity);
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x000309D2 File Offset: 0x0002EBD2
		public virtual void ForceInitialValue(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.CanForceInitialValue)
			{
				return;
			}
			if (this.ForceInitialValueDelayed)
			{
				this.Owner.StartCoroutine(this.ForceInitialValueDelayedCo(position, feedbacksIntensity));
				return;
			}
			this.Play(position, feedbacksIntensity);
			this.Stop(position, feedbacksIntensity);
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x00030A0A File Offset: 0x0002EC0A
		protected virtual IEnumerator ForceInitialValueDelayedCo(Vector3 position, float feedbacksIntensity = 1f)
		{
			this.Play(position, feedbacksIntensity);
			yield return new WaitForEndOfFrame();
			this.Stop(position, feedbacksIntensity);
			yield break;
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x00030A27 File Offset: 0x0002EC27
		public virtual void RestoreInitialValues()
		{
			this.CustomRestoreInitialValues();
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x00030A2F File Offset: 0x0002EC2F
		public virtual void ResetFeedback()
		{
			this._playsLeft = this.Timing.NumberOfRepeats + 1;
			if (this.Timing.SetPlayCountToZeroOnReset)
			{
				this.ResetPlayCount();
			}
			this.CustomReset();
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x00030A5D File Offset: 0x0002EC5D
		public virtual void PlayerComplete()
		{
			this.CustomPlayerComplete();
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x00030A65 File Offset: 0x0002EC65
		public virtual void SetDelayBetweenRepeats(float delay)
		{
			this.Timing.DelayBetweenRepeats = delay;
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x00030A73 File Offset: 0x0002EC73
		public virtual void SetInitialDelay(float delay)
		{
			this.Timing.InitialDelay = delay;
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x00030A81 File Offset: 0x0002EC81
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

		// Token: 0x06000AF6 RID: 2806 RVA: 0x00030A96 File Offset: 0x0002EC96
		public virtual void ComputeNewRandomDurationMultiplier()
		{
			this._randomDurationMultiplier = Random.Range(this.RandomDurationMultiplier.x, this.RandomDurationMultiplier.y);
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x00030AB9 File Offset: 0x0002ECB9
		public virtual void ResetPlayCount()
		{
			this.Timing.PlayCount = 0;
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x00030AC7 File Offset: 0x0002ECC7
		protected virtual float ApplyTimeMultiplier(float duration)
		{
			if (this.Owner == null)
			{
				return 0f;
			}
			if (this.RandomizeDuration)
			{
				duration *= this._randomDurationMultiplier;
			}
			return this.Owner.ApplyTimeMultiplier(duration);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x00030AFB File Offset: 0x0002ECFB
		protected virtual IEnumerator WaitFor(float delay)
		{
			if (this.InScaledTimescaleMode)
			{
				yield return MMFeedbacksCoroutine.WaitFor(delay);
			}
			else
			{
				yield return MMFeedbacksCoroutine.WaitForUnscaled(delay);
			}
			yield break;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00030B14 File Offset: 0x0002ED14
		public virtual void ComputeTotalDuration()
		{
			if (this.Timing != null && !this.Timing.ContributeToTotalDuration)
			{
				this._totalDuration = 0f;
				return;
			}
			float num = 0f;
			if (this.Timing == null)
			{
				this._totalDuration = 0f;
				return;
			}
			if (this.Timing.InitialDelay != 0f)
			{
				num += this.ApplyTimeMultiplier(this.Timing.InitialDelay);
			}
			num += this.FeedbackDuration;
			if (this.Timing.NumberOfRepeats != 0)
			{
				float num2 = this.ApplyTimeMultiplier(this.Timing.DelayBetweenRepeats);
				num += (float)this.Timing.NumberOfRepeats * num2;
			}
			this._totalDuration = num;
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x00030BC2 File Offset: 0x0002EDC2
		protected virtual float ApplyDirection(float normalizedTime)
		{
			if (!this.NormalPlayDirection)
			{
				return 1f - normalizedTime;
			}
			return normalizedTime;
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000AFC RID: 2812 RVA: 0x00030BD8 File Offset: 0x0002EDD8
		public virtual bool NormalPlayDirection
		{
			get
			{
				switch (this.Timing.PlayDirection)
				{
				case MMFeedbackTiming.PlayDirections.FollowMMFeedbacksDirection:
					return this.Owner.Direction == MMFeedbacks.Directions.TopToBottom;
				case MMFeedbackTiming.PlayDirections.OppositeMMFeedbacksDirection:
					return this.Owner.Direction > MMFeedbacks.Directions.TopToBottom;
				case MMFeedbackTiming.PlayDirections.AlwaysNormal:
					return true;
				case MMFeedbackTiming.PlayDirections.AlwaysRewind:
					return false;
				default:
					return true;
				}
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000AFD RID: 2813 RVA: 0x00030C2C File Offset: 0x0002EE2C
		public virtual bool ShouldPlayInThisSequenceDirection
		{
			get
			{
				if (this.Timing == null)
				{
					return true;
				}
				switch (this.Timing.MMFeedbacksDirectionCondition)
				{
				case MMFeedbackTiming.MMFeedbacksDirectionConditions.Always:
					return true;
				case MMFeedbackTiming.MMFeedbacksDirectionConditions.OnlyWhenForwards:
					return this.Owner.Direction == MMFeedbacks.Directions.TopToBottom;
				case MMFeedbackTiming.MMFeedbacksDirectionConditions.OnlyWhenBackwards:
					return this.Owner.Direction == MMFeedbacks.Directions.BottomToTop;
				default:
					return true;
				}
			}
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x00030C84 File Offset: 0x0002EE84
		protected virtual void CustomInitialization(MMF_Player owner)
		{
		}

		// Token: 0x06000AFF RID: 2815
		protected abstract void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f);

		// Token: 0x06000B00 RID: 2816 RVA: 0x00030C86 File Offset: 0x0002EE86
		protected virtual void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x00030C88 File Offset: 0x0002EE88
		protected virtual void CustomSkipToTheEnd(Vector3 position, float feedbacksIntensity = 1f)
		{
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x00030C8A File Offset: 0x0002EE8A
		protected virtual void CustomRestoreInitialValues()
		{
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x00030C8C File Offset: 0x0002EE8C
		protected virtual void CustomPlayerComplete()
		{
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x00030C8E File Offset: 0x0002EE8E
		protected virtual void CustomReset()
		{
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x00030C90 File Offset: 0x0002EE90
		public virtual void InitializeCustomAttributes()
		{
			if (this.HasAutomaticShakerSetup)
			{
				this.AutomaticShakerSetupButton = new MMF_Button("Automatic Shaker Setup", new MMF_Button.ButtonMethod(this.AutomaticShakerSetup));
			}
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x00030CB7 File Offset: 0x0002EEB7
		public virtual void OnValidate()
		{
			this.InitializeCustomAttributes();
			this.ComputeTotalDuration();
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x00030CC5 File Offset: 0x0002EEC5
		public virtual void OnAddFeedback()
		{
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x00030CC7 File Offset: 0x0002EEC7
		public virtual void OnDestroy()
		{
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x00030CC9 File Offset: 0x0002EEC9
		public virtual void OnDisable()
		{
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x00030CCB File Offset: 0x0002EECB
		public virtual void OnDrawGizmosSelectedHandler()
		{
		}

		// Token: 0x04000CD0 RID: 3280
		public const string _randomnessGroupName = "Feedback Randomness";

		// Token: 0x04000CD1 RID: 3281
		public const string _rangeGroupName = "Feedback Range";

		// Token: 0x04000CD2 RID: 3282
		public const string _automaticSetupGroupName = "Automatic Setup";

		// Token: 0x04000CD3 RID: 3283
		[MMFInspectorGroup("Feedback Settings", true, 0, false, true)]
		[Tooltip("whether or not this feedback is active")]
		public bool Active = true;

		// Token: 0x04000CD4 RID: 3284
		[HideInInspector]
		public int UniqueID;

		// Token: 0x04000CD5 RID: 3285
		[Tooltip("the name of this feedback to display in the inspector")]
		public string Label = "MMFeedback";

		// Token: 0x04000CD6 RID: 3286
		[Tooltip("whether to broadcast this feedback's message using an int or a scriptable object. Ints are simple to setup but can get messy and make it harder to remember what int corresponds to what. MMChannel scriptable objects require you to create them in advance, but come with a readable name and are more scalable")]
		public MMChannelModes ChannelMode;

		// Token: 0x04000CD7 RID: 3287
		[Tooltip("the ID of the channel on which this feedback will communicate")]
		[MMEnumCondition("ChannelMode", new int[] { 0 })]
		public int Channel;

		// Token: 0x04000CD8 RID: 3288
		[Tooltip("the MMChannel definition asset to use to broadcast this feedback. The shaker will have to reference that same MMChannel definition to receive events - to create a MMChannel, right click anywhere in your project (usually in a Data folder) and go MoreMountains > MMChannel, then name it with some unique name")]
		[MMEnumCondition("ChannelMode", new int[] { 1 })]
		public MMChannel MMChannelDefinition;

		// Token: 0x04000CD9 RID: 3289
		[Tooltip("the chance of this feedback happening (in percent : 100 : happens all the time, 0 : never happens, 50 : happens once every two calls, etc)")]
		[Range(0f, 100f)]
		public float Chance = 100f;

		// Token: 0x04000CDA RID: 3290
		[Tooltip("use this color to customize the background color of the feedback in the MMF_Player's list")]
		public Color DisplayColor = Color.black;

		// Token: 0x04000CDB RID: 3291
		[Tooltip("a number of timing-related values (delay, repeat, etc)")]
		public MMFeedbackTiming Timing;

		// Token: 0x04000CDC RID: 3292
		[Tooltip("a set of settings letting you define automated target acquisition for this feedback, to (for example) automatically grab the target on this game object, or a parent, a child, or on a reference holder")]
		public MMFeedbackTargetAcquisition AutomatedTargetAcquisition;

		// Token: 0x04000CDD RID: 3293
		[MMFInspectorGroup("Feedback Randomness", true, 58, false, true)]
		[Tooltip("if this is true, intensity will be multiplied by a random value on play, picked between RandomMultiplier.x and RandomMultiplier.y")]
		public bool RandomizeOutput;

		// Token: 0x04000CDE RID: 3294
		[Tooltip("a random value (randomized between its x and y) by which to multiply the output of this feedback, if RandomizeOutput is true")]
		[MMFCondition("RandomizeOutput", true)]
		[MMFVector(new string[] { "Min", "Max" })]
		public Vector2 RandomMultiplier = new Vector2(0.8f, 1f);

		// Token: 0x04000CDF RID: 3295
		[Tooltip("if this is true, this feedback's duration will be multiplied by a random value on play, picked between RandomDurationMultiplier.x and RandomDurationMultiplier.y")]
		public bool RandomizeDuration;

		// Token: 0x04000CE0 RID: 3296
		[Tooltip("a random value (randomized between its x and y) by which to multiply the duration of this feedback, if RandomizeDuration is true")]
		[MMFCondition("RandomizeDuration", true)]
		[MMFVector(new string[] { "Min", "Max" })]
		public Vector2 RandomDurationMultiplier = new Vector2(0.5f, 2f);

		// Token: 0x04000CE1 RID: 3297
		[MMFInspectorGroup("Feedback Range", true, 47, false, false)]
		[Tooltip("if this is true, only shakers within the specified range will respond to this feedback")]
		public bool UseRange;

		// Token: 0x04000CE2 RID: 3298
		[Tooltip("when in UseRange mode, only shakers within that distance will respond to this feedback")]
		public float RangeDistance = 5f;

		// Token: 0x04000CE3 RID: 3299
		[Tooltip("when in UseRange mode, whether or not to modify the shake intensity based on the RangeFallOff curve")]
		public bool UseRangeFalloff;

		// Token: 0x04000CE4 RID: 3300
		[Tooltip("the animation curve to use to define falloff (on the x 0 represents the range center, 1 represents the max distance to it)")]
		public AnimationCurve RangeFalloff = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000CE5 RID: 3301
		[Tooltip("the values to remap the falloff curve's y axis' 0 and 1")]
		[MMFVector(new string[] { "Zero", "One" })]
		public Vector2 RemapRangeFalloff = new Vector2(0f, 1f);

		// Token: 0x04000CE6 RID: 3302
		[MMFInspectorGroup("Automatic Setup", true, 49, false, true)]
		[Tooltip("a button used to attempt an auto shaker setup for this feedback, adding whatever shaker it requires to function to the scene")]
		public MMF_Button AutomaticShakerSetupButton;

		// Token: 0x04000CE7 RID: 3303
		[HideInInspector]
		public MMF_Player Owner;

		// Token: 0x04000CE8 RID: 3304
		[HideInInspector]
		public bool DebugActive;

		// Token: 0x04000CEE RID: 3310
		protected float _lastPlayTimestamp = -1f;

		// Token: 0x04000CEF RID: 3311
		protected int _playsLeft;

		// Token: 0x04000CF0 RID: 3312
		protected bool _initialized;

		// Token: 0x04000CF1 RID: 3313
		protected Coroutine _playCoroutine;

		// Token: 0x04000CF2 RID: 3314
		protected Coroutine _infinitePlayCoroutine;

		// Token: 0x04000CF3 RID: 3315
		protected Coroutine _sequenceCoroutine;

		// Token: 0x04000CF4 RID: 3316
		protected Coroutine _repeatedPlayCoroutine;

		// Token: 0x04000CF5 RID: 3317
		protected bool _requiresSetup;

		// Token: 0x04000CF6 RID: 3318
		protected string _requiredTarget = "";

		// Token: 0x04000CF7 RID: 3319
		protected float _randomDurationMultiplier = 1f;

		// Token: 0x04000CF8 RID: 3320
		protected int _sequenceTrackID;

		// Token: 0x04000CF9 RID: 3321
		protected float _beatInterval;

		// Token: 0x04000CFA RID: 3322
		protected bool BeatThisFrame;

		// Token: 0x04000CFB RID: 3323
		protected int LastBeatIndex;

		// Token: 0x04000CFC RID: 3324
		protected int CurrentSequenceIndex;

		// Token: 0x04000CFD RID: 3325
		protected float LastBeatTimestamp;

		// Token: 0x04000CFE RID: 3326
		protected MMChannelData _channelData;

		// Token: 0x04000CFF RID: 3327
		protected float _totalDuration;

		// Token: 0x04000D00 RID: 3328
		protected int _indexInOwnerFeedbackList;

		// Token: 0x04000D01 RID: 3329
		protected string _requiredTargetTextCached = ".";

		// Token: 0x04000D02 RID: 3330
		protected string _requiredTargetTextCachedExtra = "";

		// Token: 0x04000D03 RID: 3331
		protected float _repeatOffset;
	}
}
