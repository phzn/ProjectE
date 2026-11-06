using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001C5 RID: 453
	[AddComponentMenu("More Mountains/Feedbacks/MMF Player")]
	[DisallowMultipleComponent]
	public class MMF_Player : MMFeedbacks
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000B0D RID: 2829 RVA: 0x00030DDF File Offset: 0x0002EFDF
		public override float TotalDuration
		{
			get
			{
				return this._cachedTotalDuration;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000B0E RID: 2830 RVA: 0x00030DE7 File Offset: 0x0002EFE7
		// (set) Token: 0x06000B0F RID: 2831 RVA: 0x00030DEF File Offset: 0x0002EFEF
		public virtual bool SkippingToTheEnd { get; protected set; }

		// Token: 0x06000B10 RID: 2832 RVA: 0x00030DF8 File Offset: 0x0002EFF8
		protected override void Awake()
		{
			if (this.AutoInitialization && (this.AutoPlayOnEnable || this.AutoPlayOnStart))
			{
				this.InitializationMode = MMFeedbacks.InitializationModes.Awake;
			}
			if (this.AutoPlayOnEnable)
			{
				MMF_PlayerEnabler mmf_PlayerEnabler = base.GetComponent<MMF_PlayerEnabler>();
				if (mmf_PlayerEnabler == null)
				{
					mmf_PlayerEnabler = base.gameObject.AddComponent<MMF_PlayerEnabler>();
				}
				mmf_PlayerEnabler.TargetMmfPlayer = this;
			}
			if (this.InitializationMode == MMFeedbacks.InitializationModes.Awake && Application.isPlaying)
			{
				this.Initialization(false);
			}
			this.InitializeFeedbackList();
			this.ExtraInitializationChecks();
			this.CheckForLoops();
			this.ComputeCachedTotalDuration();
			this.PreInitialization();
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x00030E84 File Offset: 0x0002F084
		protected override void Start()
		{
			if (this.InitializationMode == MMFeedbacks.InitializationModes.Start && Application.isPlaying)
			{
				this.Initialization(false);
			}
			if (this.AutoPlayOnStart && Application.isPlaying)
			{
				this.PlayFeedbacks();
			}
			this.CheckForLoops();
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x00030EB8 File Offset: 0x0002F0B8
		protected virtual void InitializeFeedbackList()
		{
			if (this.FeedbacksList == null)
			{
				this.FeedbacksList = new List<MMF_Feedback>();
			}
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x00030ECD File Offset: 0x0002F0CD
		protected virtual void ExtraInitializationChecks()
		{
			if (this.Events == null)
			{
				this.Events = new MMFeedbacksEvents();
				this.Events.Initialization();
			}
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x00030EF0 File Offset: 0x0002F0F0
		protected override void OnEnable()
		{
			this.Events.TriggerOnEnable(this);
			if (this.OnlyPlayIfWithinRange)
			{
				MMSetFeedbackRangeCenterEvent.Register(new MMSetFeedbackRangeCenterEvent.Delegate(this.OnMMSetFeedbackRangeCenterEvent));
			}
			foreach (MMF_Feedback mmf_Feedback in this.FeedbacksList)
			{
				mmf_Feedback.CacheRequiresSetup();
			}
			if (this.AutoPlayOnEnable && Application.isPlaying)
			{
				if (this._lastOnEnableFrame == (float)Time.frameCount)
				{
					return;
				}
				if (Time.frameCount < 2)
				{
					this._lastOnEnableFrame = 2f;
					base.StartCoroutine(this.PlayFeedbacksAfterFrames(2));
					return;
				}
				this.PlayFeedbacks();
			}
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x00030FAC File Offset: 0x0002F1AC
		public virtual IEnumerator PlayFeedbacksAfterFrames(int framesAmount)
		{
			yield return MMFeedbacksCoroutine.WaitForFrames(framesAmount);
			this.PlayFeedbacks();
			yield break;
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x00030FC4 File Offset: 0x0002F1C4
		public virtual void PreInitialization()
		{
			int count = this.FeedbacksList.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.FeedbacksList[i] != null)
				{
					this.FeedbacksList[i].PreInitialization(this, i);
				}
			}
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x0003100C File Offset: 0x0002F20C
		public override void Initialization(bool forceInitIfPlaying = false)
		{
			if (base.IsPlaying && !forceInitIfPlaying)
			{
				return;
			}
			this.SkippingToTheEnd = false;
			base.IsPlaying = false;
			this._lastStartAt = float.MinValue;
			int count = this.FeedbacksList.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.FeedbacksList[i] != null)
				{
					this.FeedbacksList[i].Initialization(this, i);
				}
			}
			this.Events.TriggerOnInitializationComplete(this);
			this._initialized = true;
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x0003108A File Offset: 0x0002F28A
		public override void Initialization(GameObject owner)
		{
			this.Initialization(false);
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00031093 File Offset: 0x0002F293
		public override void PlayFeedbacks()
		{
			this.PlayFeedbacksInternal(base.transform.position, this.FeedbacksIntensity, false);
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x000310AD File Offset: 0x0002F2AD
		public override void PlayFeedbacks(Vector3 position, float feedbacksIntensity = 1f, bool forceRevert = false)
		{
			this.PlayFeedbacksInternal(position, feedbacksIntensity, forceRevert);
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x000310B8 File Offset: 0x0002F2B8
		public override void PlayFeedbacksInReverse()
		{
			this.PlayFeedbacksInternal(base.transform.position, this.FeedbacksIntensity, true);
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x000310D2 File Offset: 0x0002F2D2
		public override void PlayFeedbacksInReverse(Vector3 position, float feedbacksIntensity = 1f, bool forceRevert = false)
		{
			this.PlayFeedbacksInternal(position, feedbacksIntensity, forceRevert);
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x000310DD File Offset: 0x0002F2DD
		public override void PlayFeedbacksOnlyIfReversed()
		{
			if ((this.Direction == MMFeedbacks.Directions.BottomToTop && !base.ShouldRevertOnNextPlay) || (this.Direction == MMFeedbacks.Directions.TopToBottom && base.ShouldRevertOnNextPlay))
			{
				this.PlayFeedbacks();
			}
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x00031106 File Offset: 0x0002F306
		public override void PlayFeedbacksOnlyIfReversed(Vector3 position, float feedbacksIntensity = 1f, bool forceRevert = false)
		{
			if ((this.Direction == MMFeedbacks.Directions.BottomToTop && !base.ShouldRevertOnNextPlay) || (this.Direction == MMFeedbacks.Directions.TopToBottom && base.ShouldRevertOnNextPlay))
			{
				this.PlayFeedbacks(position, feedbacksIntensity, forceRevert);
			}
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x00031132 File Offset: 0x0002F332
		public override void PlayFeedbacksOnlyIfNormalDirection()
		{
			if (this.Direction == MMFeedbacks.Directions.TopToBottom)
			{
				this.PlayFeedbacks();
			}
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x00031142 File Offset: 0x0002F342
		public override void PlayFeedbacksOnlyIfNormalDirection(Vector3 position, float feedbacksIntensity = 1f, bool forceRevert = false)
		{
			if (this.Direction == MMFeedbacks.Directions.TopToBottom)
			{
				this.PlayFeedbacks(position, feedbacksIntensity, forceRevert);
			}
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x00031155 File Offset: 0x0002F355
		public override IEnumerator PlayFeedbacksCoroutine(Vector3 position, float feedbacksIntensity = 1f, bool forceRevert = false)
		{
			this.PlayFeedbacks(position, feedbacksIntensity, forceRevert);
			while (base.IsPlaying)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0003117C File Offset: 0x0002F37C
		protected override void PlayFeedbacksInternal(Vector3 position, float feedbacksIntensity, bool forceRevert = false)
		{
			if (this.AutoInitialization && !this._initialized)
			{
				this.Initialization(false);
			}
			if (!this.IsAllowedToPlay(position))
			{
				return;
			}
			this.SkippingToTheEnd = false;
			if (base.ShouldRevertOnNextPlay)
			{
				this.Revert();
				base.ShouldRevertOnNextPlay = false;
			}
			if (forceRevert)
			{
				this.Direction = ((this.Direction == MMFeedbacks.Directions.BottomToTop) ? MMFeedbacks.Directions.TopToBottom : MMFeedbacks.Directions.BottomToTop);
			}
			this.ResetFeedbacks();
			this._lastStartFrame = Time.frameCount;
			this._startTime = this.GetTime();
			this._lastStartAt = this._startTime;
			base.enabled = true;
			base.IsPlaying = true;
			this.PlayCount++;
			this.ComputeNewRandomDurationMultipliers();
			this.CheckForPauses();
			if (Time.frameCount < 2)
			{
				base.enabled = false;
				base.StartCoroutine(this.FrameOnePlayCo(position, feedbacksIntensity, forceRevert));
				return;
			}
			if (this.InitialDelay > 0f)
			{
				base.StartCoroutine(this.HandleInitialDelayCo(position, feedbacksIntensity, forceRevert));
				return;
			}
			this.PreparePlay(position, feedbacksIntensity, forceRevert);
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00031274 File Offset: 0x0002F474
		public virtual bool IsAllowedToPlay(Vector3 position)
		{
			if (!this.CanPlay)
			{
				return false;
			}
			if (base.IsPlaying && !this.CanPlayWhileAlreadyPlaying)
			{
				return false;
			}
			if (this.AutoPlayOnEnable && this._lastStartFrame == Time.frameCount)
			{
				return false;
			}
			if (!this.EvaluateChance())
			{
				return false;
			}
			if (this.CooldownDuration > 0f && this.GetTime() - this._lastStartAt < this.CooldownDuration)
			{
				return false;
			}
			if (!MMFeedbacks.GlobalMMFeedbacksActive)
			{
				return false;
			}
			if (!base.gameObject.activeInHierarchy)
			{
				return false;
			}
			if (this.OnlyPlayIfWithinRange)
			{
				if (this.RangeCenter == null)
				{
					return false;
				}
				if (Vector3.Distance(position, this.RangeCenter.position) > this.RangeDistance)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x0003132E File Offset: 0x0002F52E
		protected virtual IEnumerator FrameOnePlayCo(Vector3 position, float feedbacksIntensity, bool forceRevert = false)
		{
			yield return null;
			base.enabled = true;
			this._startTime = this.GetTime();
			this._lastStartAt = this._startTime;
			base.IsPlaying = true;
			yield return MMFeedbacksCoroutine.WaitForUnscaled(this.ComputedInitialDelay);
			this.PreparePlay(position, feedbacksIntensity, forceRevert);
			yield break;
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00031352 File Offset: 0x0002F552
		protected override void PreparePlay(Vector3 position, float feedbacksIntensity, bool forceRevert = false)
		{
			this.Events.TriggerOnPlay(this);
			this._holdingMax = 0f;
			this.CheckForPauses();
			if (!this._pauseFound)
			{
				this.PlayAllFeedbacks(position, feedbacksIntensity, forceRevert);
				return;
			}
			base.StartCoroutine(this.PausedFeedbacksCo(position, feedbacksIntensity));
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00031394 File Offset: 0x0002F594
		protected override void CheckForPauses()
		{
			this._pauseFound = false;
			int count = this.FeedbacksList.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.FeedbacksList[i] != null)
				{
					if (this.FeedbacksList[i].Pause != null && this.FeedbacksList[i].Active && this.FeedbacksList[i].ShouldPlayInThisSequenceDirection)
					{
						this._pauseFound = true;
					}
					if (this.FeedbacksList[i].HoldingPause && this.FeedbacksList[i].Active && this.FeedbacksList[i].ShouldPlayInThisSequenceDirection)
					{
						this._pauseFound = true;
					}
				}
			}
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x00031458 File Offset: 0x0002F658
		protected override void PlayAllFeedbacks(Vector3 position, float feedbacksIntensity, bool forceRevert = false)
		{
			int count = this.FeedbacksList.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.FeedbackCanPlay(this.FeedbacksList[i]))
				{
					this.FeedbacksList[i].Play(position, feedbacksIntensity);
				}
			}
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x000314A4 File Offset: 0x0002F6A4
		protected override IEnumerator HandleInitialDelayCo(Vector3 position, float feedbacksIntensity, bool forceRevert = false)
		{
			base.IsPlaying = true;
			if (this.PlayerTimescaleMode == TimescaleModes.Scaled)
			{
				yield return MMFeedbacksCoroutine.WaitFor(this.ComputedInitialDelay);
			}
			else
			{
				yield return MMFeedbacksCoroutine.WaitForUnscaled(this.ComputedInitialDelay);
			}
			this.PreparePlay(position, feedbacksIntensity, forceRevert);
			yield break;
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x000314C8 File Offset: 0x0002F6C8
		protected override void Update()
		{
			if (this._shouldStop)
			{
				if (this.HasFeedbackStillPlaying())
				{
					return;
				}
				base.IsPlaying = false;
				this.ApplyAutoRevert();
				base.enabled = false;
				this._shouldStop = false;
				this.PlayerCompleteFeedbacks();
				this.Events.TriggerOnComplete(this);
			}
			if (base.IsPlaying)
			{
				if (!this._pauseFound && this.GetTime() - this._startTime > this.TotalDuration)
				{
					this._shouldStop = true;
					return;
				}
			}
			else
			{
				base.enabled = false;
			}
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x00031547 File Offset: 0x0002F747
		protected override IEnumerator PausedFeedbacksCo(Vector3 position, float feedbacksIntensity)
		{
			base.IsPlaying = true;
			int i = ((this.Direction == MMFeedbacks.Directions.TopToBottom) ? 0 : (this.FeedbacksList.Count - 1));
			int count = this.FeedbacksList.Count;
			while (i >= 0 && i < count)
			{
				if (!base.IsPlaying)
				{
					yield break;
				}
				if (this.FeedbacksList[i] == null)
				{
					yield break;
				}
				if ((this.FeedbacksList[i].Active && this.FeedbacksList[i].ScriptDrivenPause) || base.InScriptDrivenPause)
				{
					base.InScriptDrivenPause = true;
					bool inAutoResume = this.FeedbacksList[i].ScriptDrivenPauseAutoResume > 0f;
					float scriptDrivenPauseStartedAt = this.GetTime();
					float autoResumeDuration = this.FeedbacksList[i].ScriptDrivenPauseAutoResume;
					while (base.InScriptDrivenPause)
					{
						if (inAutoResume && this.GetTime() - scriptDrivenPauseStartedAt > autoResumeDuration)
						{
							this.ResumeFeedbacks();
						}
						yield return null;
					}
				}
				if (this.FeedbacksList[i].Active && (this.FeedbacksList[i].HoldingPause || this.FeedbacksList[i].LooperPause) && this.FeedbacksList[i].ShouldPlayInThisSequenceDirection)
				{
					this.Events.TriggerOnPause(this);
					while (this.GetTime() - this._lastStartAt < this._holdingMax / this.TimescaleMultiplier && !this.SkippingToTheEnd)
					{
						yield return null;
					}
					this._holdingMax = 0f;
					this._lastStartAt = this.GetTime();
				}
				if (this.FeedbackCanPlay(this.FeedbacksList[i]))
				{
					this.FeedbacksList[i].Play(position, feedbacksIntensity);
				}
				if (this.FeedbacksList[i].Pause != null && this.FeedbacksList[i].Active && this.FeedbacksList[i].ShouldPlayInThisSequenceDirection && !this.SkippingToTheEnd)
				{
					bool flag = true;
					if (this.FeedbacksList[i].Chance < 100f && Random.Range(0f, 100f) > this.FeedbacksList[i].Chance)
					{
						flag = false;
					}
					if (flag)
					{
						yield return this.FeedbacksList[i].Pause;
						this.Events.TriggerOnResume(this);
						this._lastStartAt = this.GetTime();
						this._holdingMax = 0f;
					}
				}
				if (this.FeedbacksList[i].Active && this.FeedbacksList[i].Pause == null && this.FeedbacksList[i].ShouldPlayInThisSequenceDirection && !this.FeedbacksList[i].Timing.ExcludeFromHoldingPauses)
				{
					float totalDuration = this.FeedbacksList[i].TotalDuration;
					this._holdingMax = Mathf.Max(totalDuration, this._holdingMax);
				}
				if (this.FeedbacksList[i].LooperPause && this.FeedbacksList[i].Active && this.FeedbacksList[i].ShouldPlayInThisSequenceDirection)
				{
					if ((this.FeedbacksList[i] as MMF_Looper).NumberOfLoopsLeft <= 0)
					{
						if (!(this.FeedbacksList[i] as MMF_Looper).InInfiniteLoop)
						{
							goto IL_05E4;
						}
					}
					while (this.HasFeedbackStillPlaying() && !this.SkippingToTheEnd)
					{
						yield return null;
					}
					bool loopAtLastPause = (this.FeedbacksList[i] as MMF_Looper).LoopAtLastPause;
					bool loopAtLastLoopStart = (this.FeedbacksList[i] as MMF_Looper).LoopAtLastLoopStart;
					int num = 0;
					int num2 = ((this.Direction == MMFeedbacks.Directions.TopToBottom) ? (i - 1) : (i + 1));
					int count2 = this.FeedbacksList.Count;
					while (num2 >= 0 && num2 <= count2)
					{
						if (num2 == 0)
						{
							num = num2 - 1;
							break;
						}
						if (num2 == count2)
						{
							num = num2;
							break;
						}
						if (this.FeedbacksList[num2].Pause != null && !this.SkippingToTheEnd && this.FeedbacksList[num2].FeedbackDuration > 0f && loopAtLastPause && this.FeedbacksList[num2].Active)
						{
							num = num2;
							break;
						}
						if (this.FeedbacksList[num2].LooperStart && !this.SkippingToTheEnd && loopAtLastLoopStart && this.FeedbacksList[num2].Active)
						{
							num = num2;
							break;
						}
						num2 += ((this.Direction == MMFeedbacks.Directions.TopToBottom) ? (-1) : 1);
					}
					i = num;
				}
				IL_05E4:
				i += ((this.Direction == MMFeedbacks.Directions.TopToBottom) ? 1 : (-1));
			}
			float unscaledTimeAtEnd = this.GetTime();
			while (this.GetTime() - unscaledTimeAtEnd < this._holdingMax)
			{
				if (this.SkippingToTheEnd)
				{
					break;
				}
				yield return null;
			}
			while (this.HasFeedbackStillPlaying() && !this.SkippingToTheEnd)
			{
				yield return null;
			}
			base.IsPlaying = false;
			this.PlayerCompleteFeedbacks();
			this.Events.TriggerOnComplete(this);
			this.ApplyAutoRevert();
			yield break;
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x00031564 File Offset: 0x0002F764
		protected virtual IEnumerator SkipToTheEndCo()
		{
			if (this._startTime == this.GetTime())
			{
				yield return null;
			}
			this.SkippingToTheEnd = true;
			this.Events.TriggerOnSkipToTheEnd(this);
			int count = this.FeedbacksList.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.FeedbacksList[i] != null && this.FeedbacksList[i].Active)
				{
					this.FeedbacksList[i].SkipToTheEnd(base.transform.position, 1f);
				}
			}
			yield return null;
			yield return null;
			this.SkippingToTheEnd = false;
			this.StopFeedbacks();
			yield break;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00031573 File Offset: 0x0002F773
		public override void StopFeedbacks()
		{
			this.StopFeedbacks(true);
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0003157C File Offset: 0x0002F77C
		public override void StopFeedbacks(bool stopAllFeedbacks = true)
		{
			this.StopFeedbacks(base.transform.position, 1f, stopAllFeedbacks);
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x00031598 File Offset: 0x0002F798
		public override void StopFeedbacks(Vector3 position, float feedbacksIntensity = 1f, bool stopAllFeedbacks = true)
		{
			if (stopAllFeedbacks)
			{
				int count = this.FeedbacksList.Count;
				for (int i = 0; i < count; i++)
				{
					this.FeedbacksList[i].Stop(position, feedbacksIntensity);
				}
			}
			base.IsPlaying = false;
			base.StopAllCoroutines();
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x000315E0 File Offset: 0x0002F7E0
		public override void ResetFeedbacks()
		{
			int count = this.FeedbacksList.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.FeedbacksList[i] != null && this.FeedbacksList[i].Active)
				{
					this.FeedbacksList[i].ResetFeedback();
				}
			}
			base.IsPlaying = false;
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x0003163E File Offset: 0x0002F83E
		public override void Revert()
		{
			this.Events.TriggerOnRevert(this);
			this.Direction = ((this.Direction == MMFeedbacks.Directions.BottomToTop) ? MMFeedbacks.Directions.TopToBottom : MMFeedbacks.Directions.BottomToTop);
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0003165F File Offset: 0x0002F85F
		public virtual void SetDirection(MMFeedbacks.Directions newDirection)
		{
			this.Direction = newDirection;
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x00031668 File Offset: 0x0002F868
		public void SetDirectionTopToBottom()
		{
			this.Direction = MMFeedbacks.Directions.TopToBottom;
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x00031671 File Offset: 0x0002F871
		public void SetDirectionBottomToTop()
		{
			this.Direction = MMFeedbacks.Directions.BottomToTop;
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0003167C File Offset: 0x0002F87C
		public virtual void PlayerCompleteFeedbacks()
		{
			int count = this.FeedbacksList.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.FeedbacksList[i] != null && this.FeedbacksList[i].Active)
				{
					this.FeedbacksList[i].PlayerComplete();
				}
			}
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x000316D3 File Offset: 0x0002F8D3
		public override void PauseFeedbacks()
		{
			this.Events.TriggerOnPause(this);
			base.InScriptDrivenPause = true;
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x000316E8 File Offset: 0x0002F8E8
		public virtual void RestoreInitialValues()
		{
			if (this.PlayCount <= 0)
			{
				return;
			}
			for (int i = this.FeedbacksList.Count - 1; i >= 0; i--)
			{
				if (this.FeedbacksList[i] != null && this.FeedbacksList[i].Active)
				{
					this.FeedbacksList[i].RestoreInitialValues();
				}
			}
			this.Events.TriggerOnRestoreInitialValues(this);
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00031758 File Offset: 0x0002F958
		public virtual void ForceInitialValues()
		{
			for (int i = this.FeedbacksList.Count - 1; i >= 0; i--)
			{
				if (this.FeedbacksList[i] != null && this.FeedbacksList[i].Active)
				{
					this.FeedbacksList[i].ForceInitialValue(base.transform.position, this.FeedbacksIntensity);
				}
			}
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x000317C0 File Offset: 0x0002F9C0
		public virtual void SkipToTheEnd()
		{
			base.StartCoroutine(this.SkipToTheEndCo());
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x000317CF File Offset: 0x0002F9CF
		public override void ResumeFeedbacks()
		{
			this.Events.TriggerOnResume(this);
			base.InScriptDrivenPause = false;
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x000317E4 File Offset: 0x0002F9E4
		public virtual void AddFeedback(MMF_Feedback newFeedback)
		{
			this.InitializeFeedbackList();
			newFeedback.Owner = this;
			newFeedback.UniqueID = Guid.NewGuid().GetHashCode();
			this.FeedbacksList.Add(newFeedback);
			newFeedback.OnAddFeedback();
			newFeedback.CacheRequiresSetup();
			newFeedback.InitializeCustomAttributes();
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00031838 File Offset: 0x0002FA38
		public new MMF_Feedback AddFeedback(Type feedbackType, bool add = true)
		{
			this.InitializeFeedbackList();
			MMF_Feedback mmf_Feedback = (MMF_Feedback)Activator.CreateInstance(feedbackType);
			mmf_Feedback.Label = FeedbackPathAttribute.GetFeedbackDefaultName(feedbackType);
			mmf_Feedback.Owner = this;
			mmf_Feedback.Timing = new MMFeedbackTiming();
			mmf_Feedback.UniqueID = Guid.NewGuid().GetHashCode();
			if (add)
			{
				this.FeedbacksList.Add(mmf_Feedback);
			}
			mmf_Feedback.OnAddFeedback();
			mmf_Feedback.InitializeCustomAttributes();
			mmf_Feedback.CacheRequiresSetup();
			return mmf_Feedback;
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x000318B0 File Offset: 0x0002FAB0
		public override void RemoveFeedback(int id)
		{
			if (this.FeedbacksList.Count < id)
			{
				return;
			}
			this.FeedbacksList.RemoveAt(id);
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x000318CD File Offset: 0x0002FACD
		public virtual void CopyPlayerFrom(MMF_Player source)
		{
			JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(source), this);
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x000318DB File Offset: 0x0002FADB
		public virtual void CopyFeedbackListFrom(MMF_Player source)
		{
			this.FeedbacksList = MMF_Player.MMF_FeedbackListCopy.CopyFrom(source);
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x000318EC File Offset: 0x0002FAEC
		public virtual void AddFeedbackListFrom(MMF_Player source)
		{
			List<MMF_Feedback> list = new List<MMF_Feedback>();
			List<MMF_Feedback> list2 = new List<MMF_Feedback>();
			list = MMF_Player.MMF_FeedbackListCopy.CopyFrom(this);
			list2 = MMF_Player.MMF_FeedbackListCopy.CopyFrom(source);
			list.AddRange(list2);
			this.FeedbacksList = list;
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000B40 RID: 2880 RVA: 0x00031924 File Offset: 0x0002FB24
		public virtual bool HasAutomaticShakerSetup
		{
			get
			{
				if (this.FeedbacksList == null)
				{
					return false;
				}
				int count = this.FeedbacksList.Count;
				for (int i = 0; i < count; i++)
				{
					if (this.FeedbacksList[i] != null && this.FeedbacksList[i].HasAutomaticShakerSetup)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x00031978 File Offset: 0x0002FB78
		public virtual void AutomaticShakerSetup()
		{
			int count = this.FeedbacksList.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.FeedbacksList[i] != null)
				{
					this.FeedbacksList[i].AutomaticShakerSetup();
				}
			}
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x000319BC File Offset: 0x0002FBBC
		public override bool HasFeedbackStillPlaying()
		{
			int count = this.FeedbacksList.Count;
			for (int i = 0; i < count; i++)
			{
				if ((this.FeedbacksList[i].IsPlaying && !this.FeedbacksList[i].Timing.ExcludeFromHoldingPauses) || this.FeedbacksList[i].Timing.RepeatForever || (this.FeedbacksList[i].Timing.NumberOfRepeats > 0 && this.FeedbacksList[i].PlaysLeft > 0))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x00031A54 File Offset: 0x0002FC54
		protected override void CheckForLoops()
		{
			base.ContainsLoop = false;
			int count = this.FeedbacksList.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.FeedbacksList[i] != null && this.FeedbacksList[i].LooperPause && this.FeedbacksList[i].Active)
				{
					base.ContainsLoop = true;
					return;
				}
			}
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00031ABC File Offset: 0x0002FCBC
		protected virtual void ComputeNewRandomDurationMultipliers()
		{
			if (this.RandomizeDuration)
			{
				this._randomDurationMultiplier = Random.Range(this.RandomDurationMultiplier.x, this.RandomDurationMultiplier.y);
			}
			int count = this.FeedbacksList.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.FeedbacksList[i] != null && this.FeedbacksList[i].RandomizeDuration)
				{
					this.FeedbacksList[i].ComputeNewRandomDurationMultiplier();
				}
			}
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00031B3C File Offset: 0x0002FD3C
		public virtual float ComputeRangeIntensityMultiplier(Vector3 position)
		{
			if (!this.OnlyPlayIfWithinRange)
			{
				return 1f;
			}
			if (this.RangeCenter == null)
			{
				return 0f;
			}
			float num = Vector3.Distance(position, this.RangeCenter.position);
			if (num > this.RangeDistance)
			{
				return 0f;
			}
			if (!this.UseRangeFalloff)
			{
				return 1f;
			}
			float num2 = MMFeedbacksHelpers.Remap(num, 0f, this.RangeDistance, 0f, 1f);
			return MMFeedbacksHelpers.Remap(this.RangeFalloff.Evaluate(num2), 0f, 1f, this.RemapRangeFalloff.x, this.RemapRangeFalloff.y);
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00031BE8 File Offset: 0x0002FDE8
		protected bool FeedbackCanPlay(MMF_Feedback feedback)
		{
			return feedback.Timing.MMFeedbacksDirectionCondition == MMFeedbackTiming.MMFeedbacksDirectionConditions.Always || ((this.Direction == MMFeedbacks.Directions.TopToBottom && feedback.Timing.MMFeedbacksDirectionCondition == MMFeedbackTiming.MMFeedbacksDirectionConditions.OnlyWhenForwards) || (this.Direction == MMFeedbacks.Directions.BottomToTop && feedback.Timing.MMFeedbacksDirectionCondition == MMFeedbackTiming.MMFeedbacksDirectionConditions.OnlyWhenBackwards));
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00031C34 File Offset: 0x0002FE34
		protected override void ApplyAutoRevert()
		{
			if (this.AutoChangeDirectionOnEnd)
			{
				base.ShouldRevertOnNextPlay = true;
			}
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00031C45 File Offset: 0x0002FE45
		public override float ApplyTimeMultiplier(float duration)
		{
			return duration * Mathf.Clamp(this.DurationMultiplier, 0.001f, float.MaxValue) * this._randomDurationMultiplier / this.TimescaleMultiplier;
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00031C6C File Offset: 0x0002FE6C
		public virtual void ProxyDestroy(GameObject gameObjectToDestroy)
		{
			Object.Destroy(gameObjectToDestroy);
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00031C74 File Offset: 0x0002FE74
		public virtual void ProxyDestroy(GameObject gameObjectToDestroy, float delay)
		{
			Object.Destroy(gameObjectToDestroy, delay);
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x00031C7D File Offset: 0x0002FE7D
		public virtual void ProxyDestroyImmediate(GameObject gameObjectToDestroy)
		{
			Object.DestroyImmediate(gameObjectToDestroy);
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00031C88 File Offset: 0x0002FE88
		public virtual T GetFeedbackOfType<T>(MMF_Player.AccessMethods method, int referenceIndex) where T : MMF_Feedback
		{
			this._t = typeof(T);
			referenceIndex = Mathf.Clamp(referenceIndex, 0, this.FeedbacksList.Count);
			switch (method)
			{
			case MMF_Player.AccessMethods.First:
			{
				for (int i = 0; i < this.FeedbacksList.Count; i++)
				{
					if (this.<GetFeedbackOfType>g__Check|77_0<T>(i))
					{
						return (T)((object)this.FeedbacksList[i]);
					}
				}
				break;
			}
			case MMF_Player.AccessMethods.Previous:
			{
				for (int j = referenceIndex; j >= 0; j--)
				{
					if (this.<GetFeedbackOfType>g__Check|77_0<T>(j))
					{
						return (T)((object)this.FeedbacksList[j]);
					}
				}
				break;
			}
			case MMF_Player.AccessMethods.Closest:
			{
				int num = referenceIndex;
				int num2 = referenceIndex;
				for (int k = referenceIndex; k >= 0; k--)
				{
					if (this.<GetFeedbackOfType>g__Check|77_0<T>(k))
					{
						num = k;
						break;
					}
				}
				for (int l = referenceIndex; l < this.FeedbacksList.Count; l++)
				{
					if (this.<GetFeedbackOfType>g__Check|77_0<T>(l))
					{
						num2 = l;
						break;
					}
				}
				if (num != referenceIndex || num2 != referenceIndex)
				{
					int num3;
					if (num == referenceIndex)
					{
						num3 = num2;
					}
					else if (num2 == referenceIndex)
					{
						num3 = num;
					}
					else
					{
						int num4 = Mathf.Abs(referenceIndex - num);
						int num5 = Mathf.Abs(referenceIndex - num2);
						num3 = ((num4 > num5) ? num2 : num);
					}
					return (T)((object)this.FeedbacksList[num3]);
				}
				return default(T);
			}
			case MMF_Player.AccessMethods.Next:
			{
				for (int m = referenceIndex; m < this.FeedbacksList.Count; m++)
				{
					if (this.<GetFeedbackOfType>g__Check|77_0<T>(m))
					{
						return (T)((object)this.FeedbacksList[m]);
					}
				}
				break;
			}
			case MMF_Player.AccessMethods.Last:
			{
				for (int n = this.FeedbacksList.Count - 1; n >= 0; n--)
				{
					if (this.<GetFeedbackOfType>g__Check|77_0<T>(n))
					{
						return (T)((object)this.FeedbacksList[n]);
					}
				}
				break;
			}
			}
			return default(T);
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x00031E58 File Offset: 0x00030058
		public virtual T GetFeedbackOfType<T>() where T : MMF_Feedback
		{
			this._t = typeof(T);
			foreach (MMF_Feedback mmf_Feedback in this.FeedbacksList)
			{
				if (mmf_Feedback.GetType() == this._t)
				{
					return (T)((object)mmf_Feedback);
				}
			}
			return default(T);
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x00031EDC File Offset: 0x000300DC
		public virtual List<T> GetFeedbacksOfType<T>() where T : MMF_Feedback
		{
			this._t = typeof(T);
			List<T> list = new List<T>();
			foreach (MMF_Feedback mmf_Feedback in this.FeedbacksList)
			{
				if (mmf_Feedback.GetType() == this._t)
				{
					list.Add((T)((object)mmf_Feedback));
				}
			}
			return list;
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x00031F60 File Offset: 0x00030160
		public virtual T GetFeedbackOfType<T>(string searchedLabel) where T : MMF_Feedback
		{
			this._t = typeof(T);
			foreach (MMF_Feedback mmf_Feedback in this.FeedbacksList)
			{
				if (mmf_Feedback.GetType() == this._t && mmf_Feedback.Label == searchedLabel)
				{
					return (T)((object)mmf_Feedback);
				}
			}
			return default(T);
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x00031FF4 File Offset: 0x000301F4
		public virtual List<T> GetFeedbacksOfType<T>(string searchedLabel) where T : MMF_Feedback
		{
			this._t = typeof(T);
			List<T> list = new List<T>();
			foreach (MMF_Feedback mmf_Feedback in this.FeedbacksList)
			{
				if (mmf_Feedback.GetType() == this._t && mmf_Feedback.Label == searchedLabel)
				{
					list.Add((T)((object)mmf_Feedback));
				}
			}
			return list;
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x00032084 File Offset: 0x00030284
		protected virtual void OnMMSetFeedbackRangeCenterEvent(Transform newTransform)
		{
			if (this.IgnoreRangeEvents)
			{
				return;
			}
			this.RangeCenter = newTransform;
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x00032098 File Offset: 0x00030298
		protected override void OnDisable()
		{
			if (this.OnlyPlayIfWithinRange)
			{
				MMSetFeedbackRangeCenterEvent.Unregister(new MMSetFeedbackRangeCenterEvent.Delegate(this.OnMMSetFeedbackRangeCenterEvent));
			}
			if (base.IsPlaying)
			{
				if (this.StopFeedbacksOnDisable)
				{
					this.StopFeedbacks();
				}
				base.StopAllCoroutines();
				for (int i = this.FeedbacksList.Count - 1; i >= 0; i--)
				{
					this.FeedbacksList[i].OnDisable();
				}
			}
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x00032104 File Offset: 0x00030304
		protected override void OnValidate()
		{
			this.RefreshCache();
			if (this.FeedbacksList != null && this.FeedbacksList.Count > 0)
			{
				for (int i = this.FeedbacksList.Count - 1; i >= 0; i--)
				{
					this.FeedbacksList[i].OnValidate();
				}
			}
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x00032158 File Offset: 0x00030358
		public virtual void RefreshCache()
		{
			if (this.FeedbacksList == null)
			{
				return;
			}
			this.DurationMultiplier = Mathf.Clamp(this.DurationMultiplier, 0.001f, float.MaxValue);
			for (int i = this.FeedbacksList.Count - 1; i >= 0; i--)
			{
				if (this.FeedbacksList[i] == null)
				{
					this.FeedbacksList.RemoveAt(i);
				}
				else
				{
					this.FeedbacksList[i].Owner = this;
					this.FeedbacksList[i].CacheRequiresSetup();
				}
			}
			this.ComputeCachedTotalDuration();
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x000321E8 File Offset: 0x000303E8
		public virtual void ComputeCachedTotalDuration()
		{
			float num = 0f;
			if (this.FeedbacksList == null)
			{
				this._cachedTotalDuration = this.ComputedInitialDelay;
				return;
			}
			this.CheckForPauses();
			if (!this._pauseFound)
			{
				using (List<MMF_Feedback>.Enumerator enumerator = this.FeedbacksList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MMF_Feedback mmf_Feedback = enumerator.Current;
						mmf_Feedback.ComputeTotalDuration();
						if (mmf_Feedback != null && mmf_Feedback.Active && mmf_Feedback.ShouldPlayInThisSequenceDirection && num < mmf_Feedback.TotalDuration)
						{
							num = mmf_Feedback.TotalDuration;
						}
					}
					goto IL_0368;
				}
			}
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 1000;
			float num8 = 0f;
			int num9 = ((this.Direction == MMFeedbacks.Directions.TopToBottom) ? 0 : (this.Feedbacks.Count - 1));
			float num10 = 0f;
			while (num9 >= 0 && num9 < this.FeedbacksList.Count && num6 < num7)
			{
				num6++;
				if (this.FeedbacksList[num9] != null && this.FeedbacksList[num9].Active && this.FeedbacksList[num9].ShouldPlayInThisSequenceDirection)
				{
					this.FeedbacksList[num9].ComputeTotalDuration();
					if (this.FeedbacksList[num9].Pause != null)
					{
						if (this.FeedbacksList[num9].Timing != null && !this.FeedbacksList[num9].Timing.ContributeToTotalDuration)
						{
							continue;
						}
						if (this.FeedbacksList[num9].HoldingPause)
						{
							num10 += this.ApplyTimeMultiplier((this.FeedbacksList[num9] as MMF_Pause).PauseDuration);
							num += num10;
							num10 = 0f;
						}
						else
						{
							num8 += this.ApplyTimeMultiplier((this.FeedbacksList[num9] as MMF_Pause).PauseDuration);
						}
						if (this.FeedbacksList[num9].LooperStart)
						{
							num2 = num9;
						}
						if (!this.FeedbacksList[num9].LooperPause)
						{
							num4 = num9;
						}
						if (this.FeedbacksList[num9].LooperPause && (this.FeedbacksList[num9] as MMF_Looper).NumberOfLoops > 0)
						{
							if (num9 == num3)
							{
								num5--;
								if (num5 <= 0)
								{
									num9 += ((this.Direction == MMFeedbacks.Directions.TopToBottom) ? 1 : (-1));
									continue;
								}
							}
							else
							{
								num3 = num9;
								num5 = (this.FeedbacksList[num9] as MMF_Looper).NumberOfLoops - 1;
							}
							if ((this.FeedbacksList[num9] as MMF_Looper).InfiniteLoop)
							{
								this._cachedTotalDuration = 999f;
								return;
							}
							if ((this.FeedbacksList[num9] as MMF_Looper).LoopAtLastPause)
							{
								num9 = num4;
								num += num10;
								num10 = 0f;
								num8 = 0f;
								continue;
							}
							if ((this.FeedbacksList[num9] as MMF_Looper).LoopAtLastLoopStart)
							{
								num9 = num2;
								num += num10;
								num10 = 0f;
								num8 = 0f;
								continue;
							}
							num9 = 0;
							num += num10;
							num10 = 0f;
							num8 = 0f;
							continue;
						}
					}
					else
					{
						float num11 = this.FeedbacksList[num9].TotalDuration + num8;
						if (num10 < num11)
						{
							num10 = num11;
						}
					}
				}
				num9 += ((this.Direction == MMFeedbacks.Directions.TopToBottom) ? 1 : (-1));
			}
			num += num10;
			IL_0368:
			this._cachedTotalDuration = this.ComputedInitialDelay + num;
			this._cachedTotalDuration /= this.TimescaleMultiplier;
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x00032590 File Offset: 0x00030790
		protected override void OnDestroy()
		{
			base.IsPlaying = false;
			foreach (MMF_Feedback mmf_Feedback in this.FeedbacksList)
			{
				mmf_Feedback.OnDestroy();
			}
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x000325E8 File Offset: 0x000307E8
		protected void OnDrawGizmosSelected()
		{
			if (this.FeedbacksList == null)
			{
				return;
			}
			for (int i = this.FeedbacksList.Count - 1; i >= 0; i--)
			{
				this.FeedbacksList[i].OnDrawGizmosSelectedHandler();
			}
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x0003262F File Offset: 0x0003082F
		[CompilerGenerated]
		private bool <GetFeedbackOfType>g__Check|77_0<T>(int i) where T : MMF_Feedback
		{
			return this.FeedbacksList[i].GetType() == this._t;
		}

		// Token: 0x04000D06 RID: 3334
		[SerializeReference]
		public List<MMF_Feedback> FeedbacksList;

		// Token: 0x04000D07 RID: 3335
		public bool KeepPlayModeChanges;

		// Token: 0x04000D08 RID: 3336
		[Tooltip("if this is true, the inspector won't refresh while the feedback plays, this saves on performance but feedback inspectors' progress bars for example won't look as smooth")]
		public bool PerformanceMode;

		// Token: 0x04000D09 RID: 3337
		[Tooltip("if this is true, StopFeedbacks will be called on all feedbacks on Disable")]
		public bool StopFeedbacksOnDisable;

		// Token: 0x04000D0A RID: 3338
		[Tooltip("how many times this player has started playing")]
		[MMReadOnly]
		public int PlayCount;

		// Token: 0x04000D0C RID: 3340
		protected Type _t;

		// Token: 0x04000D0D RID: 3341
		protected float _cachedTotalDuration;

		// Token: 0x04000D0E RID: 3342
		protected bool _initialized;

		// Token: 0x0200049C RID: 1180
		[Serializable]
		private class MMF_FeedbackListCopy
		{
			// Token: 0x06001B44 RID: 6980 RVA: 0x0006F990 File Offset: 0x0006DB90
			public static List<MMF_Feedback> CopyFrom(MMF_Player source)
			{
				MMF_Player.MMF_FeedbackListCopy mmf_FeedbackListCopy = new MMF_Player.MMF_FeedbackListCopy();
				mmf_FeedbackListCopy.FeedbackList = source.FeedbacksList;
				string text = JsonUtility.ToJson(mmf_FeedbackListCopy);
				mmf_FeedbackListCopy.FeedbackList = null;
				JsonUtility.FromJsonOverwrite(text, mmf_FeedbackListCopy);
				return mmf_FeedbackListCopy.FeedbackList;
			}

			// Token: 0x04002180 RID: 8576
			[SerializeReference]
			public List<MMF_Feedback> FeedbackList;
		}

		// Token: 0x0200049D RID: 1181
		public enum AccessMethods
		{
			// Token: 0x04002182 RID: 8578
			First,
			// Token: 0x04002183 RID: 8579
			Previous,
			// Token: 0x04002184 RID: 8580
			Closest,
			// Token: 0x04002185 RID: 8581
			Next,
			// Token: 0x04002186 RID: 8582
			Last
		}
	}
}
