using System;
using System.Collections;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002BC RID: 700
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you update a TMP text value over time, with a long value going from A to B over time, on a curve")]
	[FeedbackPath("TextMesh Pro/TMP Count To Long")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.TextMeshPro", null)]
	public class MMF_TMPCountToLong : MMF_Feedback
	{
		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060013C6 RID: 5062 RVA: 0x00055703 File Offset: 0x00053903
		// (set) Token: 0x060013C7 RID: 5063 RVA: 0x00055711 File Offset: 0x00053911
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.Duration);
			}
			set
			{
				this.Duration = value;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060013C8 RID: 5064 RVA: 0x0005571A File Offset: 0x0005391A
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x0005571D File Offset: 0x0005391D
		protected override void AutomateTargetAcquisition()
		{
			this.TargetTMPText = this.FindAutomatedTarget<TMP_Text>();
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x0005572C File Offset: 0x0005392C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TMPCountToLong.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetTMPText == null)
			{
				return;
			}
			this._initialText = this.TargetTMPText.text;
			this._coroutine = this.Owner.StartCoroutine(this.CountCo());
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x00055780 File Offset: 0x00053980
		protected virtual IEnumerator CountCo()
		{
			this._lastRefreshAt = float.MinValue;
			long num = this.CountFrom;
			this._startTime = this.FeedbackTime;
			while (this.FeedbackTime - this._startTime <= this.Duration)
			{
				if (this.FeedbackTime - this._lastRefreshAt >= this.MinRefreshFrequency)
				{
					num = this.ProcessCount();
					this.UpdateText(num);
					this._lastRefreshAt = this.FeedbackTime;
				}
				yield return null;
			}
			this.UpdateText(this.CountTo);
			yield break;
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x0005578F File Offset: 0x0005398F
		protected virtual void UpdateText(long currentValue)
		{
			this._newText = currentValue.ToString(this.Format);
			this.TargetTMPText.text = this._newText;
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x000557B5 File Offset: 0x000539B5
		protected virtual long ProcessCount()
		{
			return MMTween.Tween(this.FeedbackTime - this._startTime, 0f, this.Duration, this.CountFrom, this.CountTo, this.CountingCurve);
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x000557E6 File Offset: 0x000539E6
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TMPCountToLong.FeedbackTypeAuthorized || this._coroutine == null)
			{
				return;
			}
			this.IsPlaying = false;
			this.Owner.StopCoroutine(this._coroutine);
			this._coroutine = null;
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x0005581F File Offset: 0x00053A1F
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_TMPCountToLong.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetTMPText.text = this._initialText;
		}

		// Token: 0x040016D2 RID: 5842
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040016D3 RID: 5843
		[MMFInspectorGroup("TextMeshPro Target Text", true, 12, true, false)]
		[Tooltip("the target TMP_Text component we want to change the text on")]
		public TMP_Text TargetTMPText;

		// Token: 0x040016D4 RID: 5844
		[MMFInspectorGroup("Count Settings", true, 13, false, false)]
		[Tooltip("the value from which to count from")]
		public long CountFrom;

		// Token: 0x040016D5 RID: 5845
		[Tooltip("the value to count towards")]
		public long CountTo = 100000001L;

		// Token: 0x040016D6 RID: 5846
		[Tooltip("the curve on which to animate the count")]
		public MMTweenType CountingCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x040016D7 RID: 5847
		[Tooltip("the duration of the count, in seconds")]
		public float Duration = 5f;

		// Token: 0x040016D8 RID: 5848
		[Tooltip("the format with which to display the count")]
		public string Format = "00.00";

		// Token: 0x040016D9 RID: 5849
		[Tooltip("the minimum frequency (in seconds) at which to refresh the text field")]
		public float MinRefreshFrequency;

		// Token: 0x040016DA RID: 5850
		protected string _newText;

		// Token: 0x040016DB RID: 5851
		protected float _startTime;

		// Token: 0x040016DC RID: 5852
		protected float _lastRefreshAt;

		// Token: 0x040016DD RID: 5853
		protected string _initialText;

		// Token: 0x040016DE RID: 5854
		protected Coroutine _coroutine;
	}
}
