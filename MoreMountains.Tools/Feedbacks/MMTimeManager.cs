using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002DD RID: 733
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Various/MMTimeManager")]
	public class MMTimeManager : MMSingleton<MMTimeManager>
	{
		// Token: 0x06001481 RID: 5249 RVA: 0x00057CD9 File Offset: 0x00055ED9
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		protected static void InitializeStatics()
		{
			MMSingleton<MMTimeManager>._instance = null;
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x00057CE4 File Offset: 0x00055EE4
		protected virtual void TestButtonToSlowDownTime()
		{
			MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, 0.5f, 3f, true, 1f, false, MMTimeScaleLerpModes.Speed, null, 0.2f, false, null, 0.2f);
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x00057D16 File Offset: 0x00055F16
		protected override void Awake()
		{
			base.Awake();
			this.PreInitialization();
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x00057D24 File Offset: 0x00055F24
		public virtual void PreInitialization()
		{
			this._timeScaleProperties = new Stack<TimeScaleProperties>();
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x00057D31 File Offset: 0x00055F31
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x00057D39 File Offset: 0x00055F39
		public virtual void Initialization()
		{
			this.TargetTimeScale = this.NormalTimeScale;
			this._initialFixedDeltaTime = Time.fixedDeltaTime;
			this._initialMaximumDeltaTime = Time.maximumDeltaTime;
			this.ApplyTimeScale(this.NormalTimeScale);
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x00057D6C File Offset: 0x00055F6C
		protected virtual void Update()
		{
			while (this._timeScaleProperties.Count > 0)
			{
				this._currentProperty = this._timeScaleProperties.Peek();
				this.TargetTimeScale = this._currentProperty.TimeScale;
				this._currentProperty.Duration = this._currentProperty.Duration - Time.unscaledDeltaTime;
				this._timeScaleProperties.Pop();
				this._timeScaleProperties.Push(this._currentProperty);
				if (this._currentProperty.Duration > 0f || this._currentProperty.Infinite)
				{
					break;
				}
				this.Unfreeze();
			}
			if (this._timeScaleProperties.Count == 0)
			{
				this.TargetTimeScale = this.NormalTimeScale;
			}
			if (this._currentProperty.TimeScaleLerp)
			{
				if (this._currentProperty.TimeScaleLerpMode == MMTimeScaleLerpModes.Speed)
				{
					if (this._currentProperty.LerpSpeed <= 0f)
					{
						this._currentProperty.LerpSpeed = 1f;
					}
					this.ApplyTimeScale(Mathf.Lerp(Time.timeScale, this.TargetTimeScale, Time.unscaledDeltaTime * this._currentProperty.LerpSpeed));
					return;
				}
				if (this._currentProperty.TimeScaleLerpMode == MMTimeScaleLerpModes.Duration)
				{
					float num = Time.unscaledTime - this._startedAt;
					float num2 = MMMaths.Remap(num, 0f, this._currentProperty.TimeScaleLerpDuration, 0f, 1f);
					float num3 = this._currentProperty.TimeScaleLerpCurve.Evaluate(num2);
					this.ApplyTimeScale(Mathf.Lerp(Time.timeScale, this.TargetTimeScale, num3));
					if (num > this._currentProperty.TimeScaleLerpDuration)
					{
						this.ApplyTimeScale(this.TargetTimeScale);
						if (this._lerpingBackToNormal)
						{
							this._lerpingBackToNormal = false;
							this._timeScaleProperties.Pop();
							return;
						}
					}
				}
			}
			else
			{
				this.ApplyTimeScale(this.TargetTimeScale);
			}
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x00057F2C File Offset: 0x0005612C
		protected virtual void ApplyTimeScale(float newValue)
		{
			if (newValue == this._timeScaleLastTime)
			{
				return;
			}
			if (this.UpdateTimescale)
			{
				Time.timeScale = newValue;
			}
			if (this.UpdateFixedDeltaTime && newValue != 0f)
			{
				Time.fixedDeltaTime = this._initialFixedDeltaTime * newValue;
			}
			if (this.UpdateMaximumDeltaTime)
			{
				Time.maximumDeltaTime = this._initialMaximumDeltaTime * newValue;
			}
			this.CurrentTimeScale = Time.timeScale;
			this._timeScaleLastTime = this.CurrentTimeScale;
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x00057F9A File Offset: 0x0005619A
		protected virtual void SetTimeScale(float newTimeScale)
		{
			this._timeScaleProperties.Clear();
			this.ApplyTimeScale(newTimeScale);
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x00057FB0 File Offset: 0x000561B0
		protected virtual void SetTimeScale(TimeScaleProperties timeScaleProperties)
		{
			if (timeScaleProperties.TimeScaleLerp && timeScaleProperties.TimeScaleLerpMode == MMTimeScaleLerpModes.Duration)
			{
				timeScaleProperties.Duration = Mathf.Max(timeScaleProperties.Duration, timeScaleProperties.TimeScaleLerpDuration);
				timeScaleProperties.Duration = Mathf.Max(timeScaleProperties.Duration, timeScaleProperties.TimeScaleLerpDurationOnReset);
			}
			this._startedAt = Time.unscaledTime;
			this._timeScaleProperties.Push(timeScaleProperties);
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x00058015 File Offset: 0x00056215
		protected virtual void ResetTimeScale()
		{
			this.SetTimeScale(this.NormalTimeScale);
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x00058024 File Offset: 0x00056224
		protected virtual void Unfreeze()
		{
			if (this._timeScaleProperties.Count > 0)
			{
				this._resetProperty = this._timeScaleProperties.Peek();
				this._timeScaleProperties.Pop();
			}
			if (this._timeScaleProperties.Count == 0)
			{
				if (this._resetProperty.TimeScaleLerp && this._resetProperty.TimeScaleLerpMode == MMTimeScaleLerpModes.Duration && this._resetProperty.TimeScaleLerpOnReset)
				{
					this._lerpingBackToNormal = true;
					MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, this.NormalTimeScale, this._resetProperty.TimeScaleLerpDuration, this._resetProperty.TimeScaleLerp, this._resetProperty.LerpSpeed, true, MMTimeScaleLerpModes.Duration, this._resetProperty.TimeScaleLerpCurveOnReset, this._resetProperty.TimeScaleLerpDurationOnReset, false, null, 0.2f);
					return;
				}
				this.ResetTimeScale();
			}
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x000580F0 File Offset: 0x000562F0
		public virtual void SetTimeScaleTo(float newNormalTimeScale)
		{
			MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, newNormalTimeScale, 0f, false, 0f, true, MMTimeScaleLerpModes.Speed, null, 0.2f, false, null, 0.2f);
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x00058120 File Offset: 0x00056320
		public virtual void OnTimeScaleEvent(MMTimeScaleMethods timeScaleMethod, float timeScale, float duration, bool lerp, float lerpSpeed, bool infinite, MMTimeScaleLerpModes timeScaleLerpMode = MMTimeScaleLerpModes.Speed, MMTweenType timeScaleLerpCurve = null, float timeScaleLerpDuration = 0.2f, bool timeScaleLerpOnReset = false, MMTweenType timeScaleLerpCurveOnReset = null, float timeScaleLerpDurationOnReset = 0.2f)
		{
			TimeScaleProperties timeScaleProperties = default(TimeScaleProperties);
			timeScaleProperties.TimeScale = timeScale;
			timeScaleProperties.Duration = duration;
			timeScaleProperties.TimeScaleLerp = lerp;
			timeScaleProperties.LerpSpeed = lerpSpeed;
			timeScaleProperties.Infinite = infinite;
			timeScaleProperties.TimeScaleLerpOnReset = timeScaleLerpOnReset;
			timeScaleProperties.TimeScaleLerpCurveOnReset = timeScaleLerpCurveOnReset;
			timeScaleProperties.TimeScaleLerpDurationOnReset = timeScaleLerpDurationOnReset;
			timeScaleProperties.TimeScaleLerpMode = timeScaleLerpMode;
			timeScaleProperties.TimeScaleLerpCurve = timeScaleLerpCurve;
			timeScaleProperties.TimeScaleLerpDuration = timeScaleLerpDuration;
			switch (timeScaleMethod)
			{
			case MMTimeScaleMethods.For:
				this.SetTimeScale(timeScaleProperties);
				return;
			case MMTimeScaleMethods.Reset:
				this.ResetTimeScale();
				return;
			case MMTimeScaleMethods.Unfreeze:
				this.Unfreeze();
				return;
			default:
				return;
			}
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x000581C0 File Offset: 0x000563C0
		public virtual void OnMMFreezeFrameEvent(float duration)
		{
			this.SetTimeScale(new TimeScaleProperties
			{
				Duration = duration,
				TimeScaleLerp = false,
				LerpSpeed = 0f,
				TimeScale = 0f
			});
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x00058204 File Offset: 0x00056404
		private void OnEnable()
		{
			MMFreezeFrameEvent.Register(new MMFreezeFrameEvent.Delegate(this.OnMMFreezeFrameEvent));
			MMTimeScaleEvent.Register(new MMTimeScaleEvent.Delegate(this.OnTimeScaleEvent));
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x0005822A File Offset: 0x0005642A
		private void OnDisable()
		{
			MMFreezeFrameEvent.Unregister(new MMFreezeFrameEvent.Delegate(this.OnMMFreezeFrameEvent));
			MMTimeScaleEvent.Unregister(new MMTimeScaleEvent.Delegate(this.OnTimeScaleEvent));
		}

		// Token: 0x04001777 RID: 6007
		[Header("Default Values")]
		[Tooltip("The reference time scale, to which the system will go back to after all time is changed")]
		public float NormalTimeScale = 1f;

		// Token: 0x04001778 RID: 6008
		[Header("Impacted Values")]
		[Tooltip("whether or not to update Time.timeScale when changing time scale")]
		public bool UpdateTimescale = true;

		// Token: 0x04001779 RID: 6009
		[Tooltip("whether or not to update Time.fixedDeltaTime when changing time scale")]
		public bool UpdateFixedDeltaTime = true;

		// Token: 0x0400177A RID: 6010
		[Tooltip("whether or not to update Time.maximumDeltaTime when changing time scale")]
		public bool UpdateMaximumDeltaTime = true;

		// Token: 0x0400177B RID: 6011
		[Header("Debug")]
		[Tooltip("the current, real time, time scale")]
		[MMReadOnly]
		public float CurrentTimeScale = 1f;

		// Token: 0x0400177C RID: 6012
		[Tooltip("the time scale the system is lerping towards")]
		[MMReadOnly]
		public float TargetTimeScale = 1f;

		// Token: 0x0400177D RID: 6013
		[MMInspectorButton("TestButtonToSlowDownTime")]
		public bool TestButton;

		// Token: 0x0400177E RID: 6014
		protected Stack<TimeScaleProperties> _timeScaleProperties;

		// Token: 0x0400177F RID: 6015
		protected TimeScaleProperties _currentProperty;

		// Token: 0x04001780 RID: 6016
		protected TimeScaleProperties _resetProperty;

		// Token: 0x04001781 RID: 6017
		protected float _initialFixedDeltaTime;

		// Token: 0x04001782 RID: 6018
		protected float _initialMaximumDeltaTime;

		// Token: 0x04001783 RID: 6019
		protected float _startedAt;

		// Token: 0x04001784 RID: 6020
		protected bool _lerpingBackToNormal;

		// Token: 0x04001785 RID: 6021
		protected float _timeScaleLastTime = float.NegativeInfinity;
	}
}
