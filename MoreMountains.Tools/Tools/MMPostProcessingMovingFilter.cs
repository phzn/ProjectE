using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000004 RID: 4
	[AddComponentMenu("More Mountains/Tools/Camera/MMPostProcessingMovingFilter")]
	public class MMPostProcessingMovingFilter : MonoBehaviour
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002162 File Offset: 0x00000362
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000216C File Offset: 0x0000036C
		protected virtual void Initialization()
		{
			this._lastMovementStartedAt = 0f;
			this._positionToRestore = base.transform.localPosition;
			if (this.AddToInitialPosition)
			{
				this._initialPosition = base.transform.localPosition;
			}
			else
			{
				this._initialPosition = Vector3.zero;
			}
			this._newPosition = this._initialPosition;
			this._newPosition.y = (this.Active ? (this._initialPosition.y + this.FilterOffset.x) : (this._initialPosition.y + this.FilterOffset.y));
			base.transform.localPosition = this._newPosition;
			this._lastReachedState = this.Active;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002227 File Offset: 0x00000427
		protected virtual void Update()
		{
			if (this._lastReachedState == this.Active)
			{
				return;
			}
			this.MoveTowardsCurrentTarget();
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002240 File Offset: 0x00000440
		protected virtual void MoveTowardsCurrentTarget()
		{
			if (this._newPosition != base.transform.localPosition)
			{
				base.transform.localPosition = this._newPosition;
			}
			float num = (this.Active ? (this._initialPosition.y + this.FilterOffset.y) : (this._initialPosition.y + this.FilterOffset.x));
			float num2 = (this.Active ? (this._initialPosition.y + this.FilterOffset.x) : (this._initialPosition.y + this.FilterOffset.y));
			float num3 = ((this.TimeScale == MMPostProcessingMovingFilter.TimeScales.Unscaled) ? Time.unscaledTime : Time.time);
			this._newPosition = base.transform.localPosition;
			this._newPosition.y = MMTween.Tween(num3 - this._lastMovementStartedAt, 0f, this._duration, num, num2, this.Curve);
			if (num3 - this._lastMovementStartedAt > this._duration)
			{
				this._newPosition.y = num2;
				base.transform.localPosition = this._newPosition;
				this._lastReachedState = this.Active;
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002372 File Offset: 0x00000572
		public virtual void RestoreInitialPosition()
		{
			base.transform.localPosition = this._positionToRestore;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002388 File Offset: 0x00000588
		public virtual void OnMMPostProcessingMovingFilterEvent(MMTweenType curve, bool active, bool toggle, float duration, int channel = 0, bool stop = false, bool restore = false)
		{
			if (channel != this.Channel && channel != -1 && this.Channel != -1)
			{
				return;
			}
			if (stop)
			{
				this._lastReachedState = this.Active;
				return;
			}
			if (restore)
			{
				this.RestoreInitialPosition();
				return;
			}
			this.Curve = curve;
			this._duration = duration;
			if (toggle)
			{
				this.Active = !this.Active;
			}
			else
			{
				this.Active = active;
			}
			float num = ((this.TimeScale == MMPostProcessingMovingFilter.TimeScales.Unscaled) ? Time.unscaledTime : Time.time);
			this._lastMovementStartedAt = num;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002410 File Offset: 0x00000610
		protected virtual void OnEnable()
		{
			MMPostProcessingMovingFilterEvent.Register(new MMPostProcessingMovingFilterEvent.Delegate(this.OnMMPostProcessingMovingFilterEvent));
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002424 File Offset: 0x00000624
		protected virtual void OnDisable()
		{
			MMPostProcessingMovingFilterEvent.Unregister(new MMPostProcessingMovingFilterEvent.Delegate(this.OnMMPostProcessingMovingFilterEvent));
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002438 File Offset: 0x00000638
		protected virtual void PostProcessingToggle()
		{
			MMPostProcessingMovingFilterEvent.Trigger(new MMTweenType(MMTween.MMTweenCurve.EaseInOutCubic), false, true, this.TestDuration, 0, false, false);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002450 File Offset: 0x00000650
		protected virtual void PostProcessingTriggerOff()
		{
			MMPostProcessingMovingFilterEvent.Trigger(new MMTweenType(MMTween.MMTweenCurve.EaseInOutCubic), false, false, this.TestDuration, 0, false, false);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002468 File Offset: 0x00000668
		protected virtual void PostProcessingTriggerOn()
		{
			MMPostProcessingMovingFilterEvent.Trigger(new MMTweenType(MMTween.MMTweenCurve.EaseInOutCubic), true, false, this.TestDuration, 0, false, false);
		}

		// Token: 0x04000002 RID: 2
		[Header("Settings")]
		public int Channel;

		// Token: 0x04000003 RID: 3
		public MMPostProcessingMovingFilter.TimeScales TimeScale;

		// Token: 0x04000004 RID: 4
		public MMTweenType Curve = new MMTweenType(MMTween.MMTweenCurve.EaseInCubic);

		// Token: 0x04000005 RID: 5
		public bool Active;

		// Token: 0x04000006 RID: 6
		[MMVector(new string[] { "On", "Off" })]
		public Vector2 FilterOffset = new Vector2(0f, 5f);

		// Token: 0x04000007 RID: 7
		public bool AddToInitialPosition = true;

		// Token: 0x04000008 RID: 8
		[Header("Tests")]
		public float TestDuration = 0.5f;

		// Token: 0x04000009 RID: 9
		[MMInspectorButton("PostProcessingToggle")]
		public bool PostProcessingToggleButton;

		// Token: 0x0400000A RID: 10
		[MMInspectorButton("PostProcessingTriggerOff")]
		public bool PostProcessingTriggerOffButton;

		// Token: 0x0400000B RID: 11
		[MMInspectorButton("PostProcessingTriggerOn")]
		public bool PostProcessingTriggerOnButton;

		// Token: 0x0400000C RID: 12
		protected bool _lastReachedState;

		// Token: 0x0400000D RID: 13
		protected float _duration = 2f;

		// Token: 0x0400000E RID: 14
		protected float _lastMovementStartedAt;

		// Token: 0x0400000F RID: 15
		protected Vector3 _initialPosition;

		// Token: 0x04000010 RID: 16
		protected Vector3 _positionToRestore;

		// Token: 0x04000011 RID: 17
		protected Vector3 _newPosition;

		// Token: 0x02000396 RID: 918
		public enum TimeScales
		{
			// Token: 0x04001D2F RID: 7471
			Unscaled,
			// Token: 0x04001D30 RID: 7472
			Scaled
		}
	}
}
