using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001A6 RID: 422
	[AddComponentMenu("")]
	public class MMFeedbacks : MonoBehaviour
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000A0C RID: 2572 RVA: 0x0002E67D File Offset: 0x0002C87D
		// (set) Token: 0x06000A0D RID: 2573 RVA: 0x0002E685 File Offset: 0x0002C885
		public bool IsPlaying { get; protected set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000A0E RID: 2574 RVA: 0x0002E68E File Offset: 0x0002C88E
		public virtual float ElapsedTime
		{
			get
			{
				if (!this.IsPlaying)
				{
					return 0f;
				}
				return this.GetTime() - this._lastStartAt;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000A0F RID: 2575 RVA: 0x0002E6AB File Offset: 0x0002C8AB
		// (set) Token: 0x06000A10 RID: 2576 RVA: 0x0002E6B3 File Offset: 0x0002C8B3
		public int TimesPlayed { get; protected set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000A11 RID: 2577 RVA: 0x0002E6BC File Offset: 0x0002C8BC
		// (set) Token: 0x06000A12 RID: 2578 RVA: 0x0002E6C4 File Offset: 0x0002C8C4
		public bool InScriptDrivenPause { get; set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000A13 RID: 2579 RVA: 0x0002E6CD File Offset: 0x0002C8CD
		// (set) Token: 0x06000A14 RID: 2580 RVA: 0x0002E6D5 File Offset: 0x0002C8D5
		public bool ContainsLoop { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000A15 RID: 2581 RVA: 0x0002E6DE File Offset: 0x0002C8DE
		// (set) Token: 0x06000A16 RID: 2582 RVA: 0x0002E6E6 File Offset: 0x0002C8E6
		public bool ShouldRevertOnNextPlay { get; set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x0002E6EF File Offset: 0x0002C8EF
		public bool ForcingUnscaledTimescaleMode
		{
			get
			{
				return this.ForceTimescaleMode && this.ForcedTimescaleMode == TimescaleModes.Unscaled;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000A18 RID: 2584 RVA: 0x0002E704 File Offset: 0x0002C904
		public virtual float TotalDuration
		{
			get
			{
				float num = 0f;
				foreach (MMFeedback mmfeedback in this.Feedbacks)
				{
					if (mmfeedback != null && mmfeedback.Active && num < mmfeedback.TotalDuration)
					{
						num = mmfeedback.TotalDuration;
					}
				}
				return this.ComputedInitialDelay + num;
			}
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0002E780 File Offset: 0x0002C980
		public virtual float GetTime()
		{
			if (this.PlayerTimescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledTime;
			}
			return Time.time;
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0002E795 File Offset: 0x0002C995
		public virtual float GetDeltaTime()
		{
			if (this.PlayerTimescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledDeltaTime;
			}
			return Time.deltaTime;
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000A1B RID: 2587 RVA: 0x0002E7AA File Offset: 0x0002C9AA
		public virtual float ComputedInitialDelay
		{
			get
			{
				return this.ApplyTimeMultiplier(this.InitialDelay);
			}
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0002E7B8 File Offset: 0x0002C9B8
		protected virtual void Awake()
		{
			if (this.AutoPlayOnEnable)
			{
				MMFeedbacksEnabler mmfeedbacksEnabler = base.GetComponent<MMFeedbacksEnabler>();
				if (mmfeedbacksEnabler == null)
				{
					mmfeedbacksEnabler = base.gameObject.AddComponent<MMFeedbacksEnabler>();
				}
				mmfeedbacksEnabler.TargetMMFeedbacks = this;
			}
			if (this.InitializationMode == MMFeedbacks.InitializationModes.Awake && Application.isPlaying)
			{
				this.Initialization(base.gameObject);
			}
			this.CheckForLoops();
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x0002E812 File Offset: 0x0002CA12
		protected virtual void Start()
		{
			if (this.InitializationMode == MMFeedbacks.InitializationModes.Start && Application.isPlaying)
			{
				this.Initialization(base.gameObject);
			}
			if (this.AutoPlayOnStart && Application.isPlaying)
			{
				this.PlayFeedbacks();
			}
			this.CheckForLoops();
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0002E84B File Offset: 0x0002CA4B
		protected virtual void OnEnable()
		{
			if (this.AutoPlayOnEnable && Application.isPlaying)
			{
				this.PlayFeedbacks();
			}
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0002E862 File Offset: 0x0002CA62
		public virtual void Initialization(bool forceInitIfPlaying = false)
		{
			this.Initialization(base.gameObject);
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0002E870 File Offset: 0x0002CA70
		public virtual void Initialization(GameObject owner)
		{
			if (this.SafeMode == MMFeedbacks.SafeModes.RuntimeOnly || this.SafeMode == MMFeedbacks.SafeModes.Full)
			{
				this.AutoRepair();
			}
			this.IsPlaying = false;
			this.TimesPlayed = 0;
			this._lastStartAt = float.MinValue;
			for (int i = 0; i < this.Feedbacks.Count; i++)
			{
				if (this.Feedbacks[i] != null)
				{
					this.Feedbacks[i].Initialization(owner);
				}
			}
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0002E8EA File Offset: 0x0002CAEA
		public virtual void PlayFeedbacks()
		{
			this.PlayFeedbacksInternal(base.transform.position, this.FeedbacksIntensity, false);
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0002E904 File Offset: 0x0002CB04
		public virtual async Task PlayFeedbacksTask(Vector3 position, float feedbacksIntensity = 1f, bool forceRevert = false)
		{
			this.PlayFeedbacks(position, feedbacksIntensity, forceRevert);
			while (this.IsPlaying)
			{
				await Task.Yield();
			}
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0002E960 File Offset: 0x0002CB60
		public virtual async Task PlayFeedbacksTask()
		{
			this.PlayFeedbacks();
			while (this.IsPlaying)
			{
				await Task.Yield();
			}
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0002E9A3 File Offset: 0x0002CBA3
		public virtual void PlayFeedbacks(Vector3 position, float feedbacksIntensity = 1f, bool forceRevert = false)
		{
			this.PlayFeedbacksInternal(position, feedbacksIntensity, forceRevert);
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0002E9AE File Offset: 0x0002CBAE
		public virtual void PlayFeedbacksInReverse()
		{
			this.PlayFeedbacksInternal(base.transform.position, this.FeedbacksIntensity, true);
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0002E9C8 File Offset: 0x0002CBC8
		public virtual void PlayFeedbacksInReverse(Vector3 position, float feedbacksIntensity = 1f, bool forceRevert = false)
		{
			this.PlayFeedbacksInternal(position, feedbacksIntensity, forceRevert);
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0002E9D3 File Offset: 0x0002CBD3
		public virtual void PlayFeedbacksOnlyIfReversed()
		{
			if ((this.Direction == MMFeedbacks.Directions.BottomToTop && !this.ShouldRevertOnNextPlay) || (this.Direction == MMFeedbacks.Directions.TopToBottom && this.ShouldRevertOnNextPlay))
			{
				this.PlayFeedbacks();
			}
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0002E9FC File Offset: 0x0002CBFC
		public virtual void PlayFeedbacksOnlyIfReversed(Vector3 position, float feedbacksIntensity = 1f, bool forceRevert = false)
		{
			if ((this.Direction == MMFeedbacks.Directions.BottomToTop && !this.ShouldRevertOnNextPlay) || (this.Direction == MMFeedbacks.Directions.TopToBottom && this.ShouldRevertOnNextPlay))
			{
				this.PlayFeedbacks(position, feedbacksIntensity, forceRevert);
			}
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0002EA28 File Offset: 0x0002CC28
		public virtual void PlayFeedbacksOnlyIfNormalDirection()
		{
			if (this.Direction == MMFeedbacks.Directions.TopToBottom)
			{
				this.PlayFeedbacks();
			}
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0002EA38 File Offset: 0x0002CC38
		public virtual void PlayFeedbacksOnlyIfNormalDirection(Vector3 position, float feedbacksIntensity = 1f, bool forceRevert = false)
		{
			if (this.Direction == MMFeedbacks.Directions.TopToBottom)
			{
				this.PlayFeedbacks(position, feedbacksIntensity, forceRevert);
			}
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0002EA4B File Offset: 0x0002CC4B
		public virtual IEnumerator PlayFeedbacksCoroutine(Vector3 position, float feedbacksIntensity = 1f, bool forceRevert = false)
		{
			this.PlayFeedbacks(position, feedbacksIntensity, forceRevert);
			while (this.IsPlaying)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x0002EA70 File Offset: 0x0002CC70
		protected virtual void PlayFeedbacksInternal(Vector3 position, float feedbacksIntensity, bool forceRevert = false)
		{
			if (!this.CanPlay)
			{
				return;
			}
			if (this.IsPlaying && !this.CanPlayWhileAlreadyPlaying)
			{
				return;
			}
			if (!this.EvaluateChance())
			{
				return;
			}
			if (this.CooldownDuration > 0f && this.GetTime() - this._lastStartAt < this.CooldownDuration)
			{
				return;
			}
			if (!MMFeedbacks.GlobalMMFeedbacksActive)
			{
				return;
			}
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (this.ShouldRevertOnNextPlay)
			{
				this.Revert();
				this.ShouldRevertOnNextPlay = false;
			}
			if (forceRevert)
			{
				this.Direction = ((this.Direction == MMFeedbacks.Directions.BottomToTop) ? MMFeedbacks.Directions.TopToBottom : MMFeedbacks.Directions.BottomToTop);
			}
			this.ResetFeedbacks();
			base.enabled = true;
			int timesPlayed = this.TimesPlayed;
			this.TimesPlayed = timesPlayed + 1;
			this.IsPlaying = true;
			this._startTime = this.GetTime();
			this._lastStartAt = this._startTime;
			this._totalDuration = this.TotalDuration;
			this.CheckForPauses();
			if (this.ComputedInitialDelay > 0f)
			{
				base.StartCoroutine(this.HandleInitialDelayCo(position, feedbacksIntensity, forceRevert));
				return;
			}
			this.PreparePlay(position, feedbacksIntensity, forceRevert);
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0002EB79 File Offset: 0x0002CD79
		protected virtual void PreparePlay(Vector3 position, float feedbacksIntensity, bool forceRevert = false)
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

		// Token: 0x06000A2E RID: 2606 RVA: 0x0002EBBC File Offset: 0x0002CDBC
		protected virtual void CheckForPauses()
		{
			this._pauseFound = false;
			for (int i = 0; i < this.Feedbacks.Count; i++)
			{
				if (this.Feedbacks[i] != null)
				{
					if (this.Feedbacks[i].Pause != null && this.Feedbacks[i].Active && this.Feedbacks[i].ShouldPlayInThisSequenceDirection)
					{
						this._pauseFound = true;
					}
					if (this.Feedbacks[i].HoldingPause && this.Feedbacks[i].Active && this.Feedbacks[i].ShouldPlayInThisSequenceDirection)
					{
						this._pauseFound = true;
					}
				}
			}
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0002EC84 File Offset: 0x0002CE84
		protected virtual void PlayAllFeedbacks(Vector3 position, float feedbacksIntensity, bool forceRevert = false)
		{
			for (int i = 0; i < this.Feedbacks.Count; i++)
			{
				if (this.FeedbackCanPlay(this.Feedbacks[i]))
				{
					this.Feedbacks[i].Play(position, feedbacksIntensity);
				}
			}
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0002ECCE File Offset: 0x0002CECE
		protected virtual IEnumerator HandleInitialDelayCo(Vector3 position, float feedbacksIntensity, bool forceRevert = false)
		{
			this.IsPlaying = true;
			yield return MMFeedbacksCoroutine.WaitFor(this.ComputedInitialDelay);
			this.PreparePlay(position, feedbacksIntensity, forceRevert);
			yield break;
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0002ECF4 File Offset: 0x0002CEF4
		protected virtual void Update()
		{
			if (this._shouldStop)
			{
				if (this.HasFeedbackStillPlaying())
				{
					return;
				}
				this.IsPlaying = false;
				this.Events.TriggerOnComplete(this);
				this.ApplyAutoRevert();
				base.enabled = false;
				this._shouldStop = false;
			}
			if (this.IsPlaying)
			{
				if (!this._pauseFound && this.GetTime() - this._startTime > this._totalDuration)
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

		// Token: 0x06000A32 RID: 2610 RVA: 0x0002ED70 File Offset: 0x0002CF70
		public virtual bool HasFeedbackStillPlaying()
		{
			int count = this.Feedbacks.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.Feedbacks[i] != null && this.Feedbacks[i].IsPlaying)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0002EDBF File Offset: 0x0002CFBF
		protected virtual IEnumerator PausedFeedbacksCo(Vector3 position, float feedbacksIntensity)
		{
			yield return null;
			yield break;
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x0002EDC7 File Offset: 0x0002CFC7
		public virtual void StopFeedbacks()
		{
			this.StopFeedbacks(true);
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0002EDD0 File Offset: 0x0002CFD0
		public virtual void StopFeedbacks(bool stopAllFeedbacks = true)
		{
			this.StopFeedbacks(base.transform.position, 1f, stopAllFeedbacks);
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x0002EDEC File Offset: 0x0002CFEC
		public virtual void StopFeedbacks(Vector3 position, float feedbacksIntensity = 1f, bool stopAllFeedbacks = true)
		{
			if (stopAllFeedbacks)
			{
				for (int i = 0; i < this.Feedbacks.Count; i++)
				{
					if (this.Feedbacks[i] != null)
					{
						this.Feedbacks[i].Stop(position, feedbacksIntensity);
					}
				}
			}
			this.IsPlaying = false;
			base.StopAllCoroutines();
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x0002EE48 File Offset: 0x0002D048
		public virtual void ResetFeedbacks()
		{
			for (int i = 0; i < this.Feedbacks.Count; i++)
			{
				if (this.Feedbacks[i] != null && this.Feedbacks[i].Active)
				{
					this.Feedbacks[i].ResetFeedback();
				}
			}
			this.IsPlaying = false;
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x0002EEAA File Offset: 0x0002D0AA
		public virtual void Revert()
		{
			this.Events.TriggerOnRevert(this);
			this.Direction = ((this.Direction == MMFeedbacks.Directions.BottomToTop) ? MMFeedbacks.Directions.TopToBottom : MMFeedbacks.Directions.BottomToTop);
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x0002EECB File Offset: 0x0002D0CB
		public virtual void SetCanPlay(bool newState)
		{
			this.CanPlay = newState;
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0002EED4 File Offset: 0x0002D0D4
		public virtual void PauseFeedbacks()
		{
			this.Events.TriggerOnPause(this);
			this.InScriptDrivenPause = true;
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0002EEE9 File Offset: 0x0002D0E9
		public virtual void ResumeFeedbacks()
		{
			this.Events.TriggerOnResume(this);
			this.InScriptDrivenPause = false;
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x0002EEFE File Offset: 0x0002D0FE
		public virtual MMFeedback AddFeedback(Type feedbackType, bool add = true)
		{
			MMFeedback mmfeedback = base.gameObject.AddComponent(feedbackType) as MMFeedback;
			mmfeedback.hideFlags = HideFlags.HideInInspector;
			mmfeedback.Label = FeedbackPathAttribute.GetFeedbackDefaultName(feedbackType);
			this.AutoRepair();
			return mmfeedback;
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x0002EF2A File Offset: 0x0002D12A
		public virtual void RemoveFeedback(int id)
		{
			Object.DestroyImmediate(this.Feedbacks[id]);
			this.Feedbacks.RemoveAt(id);
			this.AutoRepair();
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x0002EF4F File Offset: 0x0002D14F
		protected virtual bool EvaluateChance()
		{
			return this.ChanceToPlay != 0f && (this.ChanceToPlay == 100f || Random.Range(0f, 100f) <= this.ChanceToPlay);
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x0002EF88 File Offset: 0x0002D188
		protected virtual void CheckForLoops()
		{
			this.ContainsLoop = false;
			for (int i = 0; i < this.Feedbacks.Count; i++)
			{
				if (this.Feedbacks[i] != null && this.Feedbacks[i].LooperPause && this.Feedbacks[i].Active)
				{
					this.ContainsLoop = true;
					return;
				}
			}
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0002EFF4 File Offset: 0x0002D1F4
		protected bool FeedbackCanPlay(MMFeedback feedback)
		{
			return !(feedback == null) && feedback.Timing != null && (feedback.Timing.MMFeedbacksDirectionCondition == MMFeedbackTiming.MMFeedbacksDirectionConditions.Always || ((this.Direction == MMFeedbacks.Directions.TopToBottom && feedback.Timing.MMFeedbacksDirectionCondition == MMFeedbackTiming.MMFeedbacksDirectionConditions.OnlyWhenForwards) || (this.Direction == MMFeedbacks.Directions.BottomToTop && feedback.Timing.MMFeedbacksDirectionCondition == MMFeedbackTiming.MMFeedbacksDirectionConditions.OnlyWhenBackwards)));
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0002F055 File Offset: 0x0002D255
		protected virtual void ApplyAutoRevert()
		{
			if (this.AutoChangeDirectionOnEnd)
			{
				this.ShouldRevertOnNextPlay = true;
			}
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0002F066 File Offset: 0x0002D266
		public virtual float ApplyTimeMultiplier(float duration)
		{
			return duration * Mathf.Clamp(this.DurationMultiplier, 0.001f, float.MaxValue);
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0002F080 File Offset: 0x0002D280
		public virtual void AutoRepair()
		{
			new List<Component>();
			foreach (Component component in base.gameObject.GetComponents<Component>().ToList<Component>())
			{
				if (component is MMFeedback)
				{
					bool flag = false;
					for (int i = 0; i < this.Feedbacks.Count; i++)
					{
						if (this.Feedbacks[i] == (MMFeedback)component)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.Feedbacks.Add((MMFeedback)component);
					}
				}
			}
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x0002F130 File Offset: 0x0002D330
		protected virtual void OnDisable()
		{
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0002F132 File Offset: 0x0002D332
		protected virtual void OnValidate()
		{
			this.DurationMultiplier = Mathf.Clamp(this.DurationMultiplier, 0.001f, float.MaxValue);
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0002F14F File Offset: 0x0002D34F
		protected virtual void OnDestroy()
		{
			this.IsPlaying = false;
		}

		// Token: 0x04000C38 RID: 3128
		public List<MMFeedback> Feedbacks = new List<MMFeedback>();

		// Token: 0x04000C39 RID: 3129
		[Tooltip("the chosen initialization modes. If you use Script, you'll have to initialize manually by calling the Initialization method and passing it an owner. Otherwise, you can have this component initialize itself at Awake or Start, and in this case the owner will be the MMFeedbacks itself")]
		public MMFeedbacks.InitializationModes InitializationMode = MMFeedbacks.InitializationModes.Start;

		// Token: 0x04000C3A RID: 3130
		[Tooltip("if you set this to true, the system will make changes to ensure that initialization always happens before play")]
		public bool AutoInitialization = true;

		// Token: 0x04000C3B RID: 3131
		[Tooltip("the selected safe mode")]
		public MMFeedbacks.SafeModes SafeMode = MMFeedbacks.SafeModes.Full;

		// Token: 0x04000C3C RID: 3132
		[Tooltip("the selected direction these feedbacks should play in")]
		public MMFeedbacks.Directions Direction;

		// Token: 0x04000C3D RID: 3133
		[Tooltip("whether or not this MMFeedbacks should invert its direction when all feedbacks have played")]
		public bool AutoChangeDirectionOnEnd;

		// Token: 0x04000C3E RID: 3134
		[Tooltip("whether or not to play this feedbacks automatically on Start")]
		public bool AutoPlayOnStart;

		// Token: 0x04000C3F RID: 3135
		[Tooltip("whether or not to play this feedbacks automatically on Enable")]
		public bool AutoPlayOnEnable;

		// Token: 0x04000C40 RID: 3136
		[Tooltip("if this is true, all feedbacks within that player will work on the specified ForcedTimescaleMode, regardless of their individual settings")]
		public bool ForceTimescaleMode;

		// Token: 0x04000C41 RID: 3137
		[Tooltip("the time scale mode all feedbacks on this player should work on, if ForceTimescaleMode is true")]
		[MMFCondition("ForceTimescaleMode", true)]
		public TimescaleModes ForcedTimescaleMode = TimescaleModes.Unscaled;

		// Token: 0x04000C42 RID: 3138
		[Tooltip("a time multiplier that will be applied to all feedback durations (initial delay, duration, delay between repeats...)")]
		public float DurationMultiplier = 1f;

		// Token: 0x04000C43 RID: 3139
		[Tooltip("a multiplier to apply to all timescale operations (1: normal, less than 1: slower operations, higher than 1: faster operations)")]
		public float TimescaleMultiplier = 1f;

		// Token: 0x04000C44 RID: 3140
		[Tooltip("if this is true, will expose a RandomDurationMultiplier. The final duration of each feedback will be : their base duration * DurationMultiplier * a random value between RandomDurationMultiplier.x and RandomDurationMultiplier.y")]
		public bool RandomizeDuration;

		// Token: 0x04000C45 RID: 3141
		[Tooltip("if RandomizeDuration is true, the min (x) and max (y) values for the random duration multiplier")]
		[MMCondition("RandomizeDuration", true)]
		public Vector2 RandomDurationMultiplier = new Vector2(0.5f, 1.5f);

		// Token: 0x04000C46 RID: 3142
		[Tooltip("if this is true, more editor-only, detailed info will be displayed per feedback in the duration slot")]
		public bool DisplayFullDurationDetails;

		// Token: 0x04000C47 RID: 3143
		[Tooltip("the timescale at which the player itself will operate. This notably impacts sequencing and pauses duration evaluation.")]
		public TimescaleModes PlayerTimescaleMode = TimescaleModes.Unscaled;

		// Token: 0x04000C48 RID: 3144
		[Tooltip("if this is true, this feedback will only play if its distance to RangeCenter is lower or equal to RangeDistance")]
		public bool OnlyPlayIfWithinRange;

		// Token: 0x04000C49 RID: 3145
		[Tooltip("when in OnlyPlayIfWithinRange mode, the transform to consider as the center of the range")]
		public Transform RangeCenter;

		// Token: 0x04000C4A RID: 3146
		[Tooltip("when in OnlyPlayIfWithinRange mode, the distance to the center within which the feedback will play")]
		public float RangeDistance = 5f;

		// Token: 0x04000C4B RID: 3147
		[Tooltip("when in OnlyPlayIfWithinRange mode, whether or not to modify the intensity of feedbacks based on the RangeFallOff curve")]
		public bool UseRangeFalloff;

		// Token: 0x04000C4C RID: 3148
		[Tooltip("the animation curve to use to define falloff (on the x 0 represents the range center, 1 represents the max distance to it)")]
		[MMFCondition("UseRangeFalloff", true)]
		public AnimationCurve RangeFalloff = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000C4D RID: 3149
		[Tooltip("the values to remap the falloff curve's y axis' 0 and 1")]
		[MMFVector(new string[] { "Zero", "One" })]
		public Vector2 RemapRangeFalloff = new Vector2(0f, 1f);

		// Token: 0x04000C4E RID: 3150
		[Tooltip("whether or not to ignore MMSetFeedbackRangeCenterEvent, used to set the RangeCenter from anywhere")]
		public bool IgnoreRangeEvents;

		// Token: 0x04000C4F RID: 3151
		[Tooltip("a duration, in seconds, during which triggering a new play of this MMFeedbacks after it's been played once will be impossible")]
		public float CooldownDuration;

		// Token: 0x04000C50 RID: 3152
		[Tooltip("a duration, in seconds, to delay the start of this MMFeedbacks' contents play")]
		public float InitialDelay;

		// Token: 0x04000C51 RID: 3153
		[Tooltip("whether this player can be played or not, useful to temporarily prevent play from another class, for example")]
		public bool CanPlay = true;

		// Token: 0x04000C52 RID: 3154
		[Tooltip("if this is true, you'll be able to trigger a new Play while this feedback is already playing, otherwise you won't be able to")]
		public bool CanPlayWhileAlreadyPlaying = true;

		// Token: 0x04000C53 RID: 3155
		[Tooltip("the chance of this sequence happening (in percent : 100 : happens all the time, 0 : never happens, 50 : happens once every two calls, etc)")]
		[Range(0f, 100f)]
		public float ChanceToPlay = 100f;

		// Token: 0x04000C54 RID: 3156
		[Tooltip("the intensity at which to play this feedback. That value will be used by most feedbacks to tune their amplitude. 1 is normal, 0.5 is half power, 0 is no effect.Note that what this value controls depends from feedback to feedback, don't hesitate to check the code to see what it does exactly.")]
		public float FeedbacksIntensity = 1f;

		// Token: 0x04000C55 RID: 3157
		[Tooltip("a number of UnityEvents that can be triggered at the various stages of this MMFeedbacks")]
		public MMFeedbacksEvents Events;

		// Token: 0x04000C56 RID: 3158
		[Tooltip("a global switch used to turn all feedbacks on or off globally")]
		public static bool GlobalMMFeedbacksActive = true;

		// Token: 0x04000C57 RID: 3159
		[HideInInspector]
		public bool DebugActive;

		// Token: 0x04000C5D RID: 3165
		protected float _startTime;

		// Token: 0x04000C5E RID: 3166
		protected float _holdingMax;

		// Token: 0x04000C5F RID: 3167
		protected float _lastStartAt = float.MinValue;

		// Token: 0x04000C60 RID: 3168
		protected int _lastStartFrame = -1;

		// Token: 0x04000C61 RID: 3169
		protected bool _pauseFound;

		// Token: 0x04000C62 RID: 3170
		protected float _totalDuration;

		// Token: 0x04000C63 RID: 3171
		protected bool _shouldStop;

		// Token: 0x04000C64 RID: 3172
		protected const float _smallValue = 0.001f;

		// Token: 0x04000C65 RID: 3173
		protected float _randomDurationMultiplier = 1f;

		// Token: 0x04000C66 RID: 3174
		protected float _lastOnEnableFrame = -1f;

		// Token: 0x02000480 RID: 1152
		public enum Directions
		{
			// Token: 0x04002104 RID: 8452
			TopToBottom,
			// Token: 0x04002105 RID: 8453
			BottomToTop
		}

		// Token: 0x02000481 RID: 1153
		public enum SafeModes
		{
			// Token: 0x04002107 RID: 8455
			Nope,
			// Token: 0x04002108 RID: 8456
			EditorOnly,
			// Token: 0x04002109 RID: 8457
			RuntimeOnly,
			// Token: 0x0400210A RID: 8458
			Full
		}

		// Token: 0x02000482 RID: 1154
		public enum InitializationModes
		{
			// Token: 0x0400210C RID: 8460
			Script,
			// Token: 0x0400210D RID: 8461
			Awake,
			// Token: 0x0400210E RID: 8462
			Start
		}
	}
}
