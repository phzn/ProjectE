using System;
using System.Collections;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002BB RID: 699
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you update a TMP text value over time, with a value going from A to B over time, on a curve")]
	[FeedbackPath("TextMesh Pro/TMP Count To")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.TextMeshPro", null)]
	public class MMF_TMPCountTo : MMF_Feedback
	{
		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060013BA RID: 5050 RVA: 0x000554F6 File Offset: 0x000536F6
		// (set) Token: 0x060013BB RID: 5051 RVA: 0x00055504 File Offset: 0x00053704
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

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060013BC RID: 5052 RVA: 0x0005550D File Offset: 0x0005370D
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x00055510 File Offset: 0x00053710
		protected override void AutomateTargetAcquisition()
		{
			this.TargetTMPText = this.FindAutomatedTarget<TMP_Text>();
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x00055520 File Offset: 0x00053720
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TMPCountTo.FeedbackTypeAuthorized)
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

		// Token: 0x060013BF RID: 5055 RVA: 0x00055574 File Offset: 0x00053774
		protected virtual IEnumerator CountCo()
		{
			this._lastRefreshAt = float.MinValue;
			float num = this.CountFrom;
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

		// Token: 0x060013C0 RID: 5056 RVA: 0x00055584 File Offset: 0x00053784
		protected virtual void UpdateText(float currentValue)
		{
			if (this.FloorValues)
			{
				this._newText = Mathf.Floor(currentValue).ToString(this.Format);
			}
			else
			{
				this._newText = currentValue.ToString(this.Format);
			}
			this.TargetTMPText.text = this._newText;
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x000555D9 File Offset: 0x000537D9
		protected virtual float ProcessCount()
		{
			return MMTween.Tween(this.FeedbackTime - this._startTime, 0f, this.Duration, this.CountFrom, this.CountTo, this.CountingCurve);
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x0005560A File Offset: 0x0005380A
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TMPCountTo.FeedbackTypeAuthorized || this._coroutine == null)
			{
				return;
			}
			this.IsPlaying = false;
			this.Owner.StopCoroutine(this._coroutine);
			this._coroutine = null;
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x00055643 File Offset: 0x00053843
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_TMPCountTo.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetTMPText.text = this._initialText;
		}

		// Token: 0x040016C4 RID: 5828
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040016C5 RID: 5829
		[MMFInspectorGroup("TextMeshPro Target Text", true, 12, true, false)]
		[Tooltip("the target TMP_Text component we want to change the text on")]
		public TMP_Text TargetTMPText;

		// Token: 0x040016C6 RID: 5830
		[MMFInspectorGroup("Count Settings", true, 13, false, false)]
		[Tooltip("the value from which to count from")]
		public float CountFrom;

		// Token: 0x040016C7 RID: 5831
		[Tooltip("the value to count towards")]
		public float CountTo = 10f;

		// Token: 0x040016C8 RID: 5832
		[Tooltip("the curve on which to animate the count")]
		public MMTweenType CountingCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x040016C9 RID: 5833
		[Tooltip("the duration of the count, in seconds")]
		public float Duration = 5f;

		// Token: 0x040016CA RID: 5834
		[Tooltip("the format with which to display the count")]
		public string Format = "00.00";

		// Token: 0x040016CB RID: 5835
		[Tooltip("whether or not value should be floored")]
		public bool FloorValues = true;

		// Token: 0x040016CC RID: 5836
		[Tooltip("the minimum frequency (in seconds) at which to refresh the text field")]
		public float MinRefreshFrequency;

		// Token: 0x040016CD RID: 5837
		protected string _newText;

		// Token: 0x040016CE RID: 5838
		protected float _startTime;

		// Token: 0x040016CF RID: 5839
		protected float _lastRefreshAt;

		// Token: 0x040016D0 RID: 5840
		protected string _initialText;

		// Token: 0x040016D1 RID: 5841
		protected Coroutine _coroutine;
	}
}
