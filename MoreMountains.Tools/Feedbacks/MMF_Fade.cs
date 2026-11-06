using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000299 RID: 665
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you trigger a fade event.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("Camera/Fade")]
	public class MMF_Fade : MMF_Feedback
	{
		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060012A2 RID: 4770 RVA: 0x000502A5 File Offset: 0x0004E4A5
		// (set) Token: 0x060012A3 RID: 4771 RVA: 0x000502B3 File Offset: 0x0004E4B3
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

		// Token: 0x060012A4 RID: 4772 RVA: 0x000502BC File Offset: 0x0004E4BC
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Fade.FeedbackTypeAuthorized)
			{
				return;
			}
			this._position = this.GetPosition(position);
			this._fadeType = this.FadeType;
			if (!this.NormalPlayDirection)
			{
				if (this.FadeType == MMF_Fade.FadeTypes.FadeIn)
				{
					this._fadeType = MMF_Fade.FadeTypes.FadeOut;
				}
				else if (this.FadeType == MMF_Fade.FadeTypes.FadeOut)
				{
					this._fadeType = MMF_Fade.FadeTypes.FadeIn;
				}
			}
			if (this.TargetFader != null)
			{
				switch (this._fadeType)
				{
				case MMF_Fade.FadeTypes.FadeIn:
					this.TargetFader.FadeIn(this.FeedbackDuration, this.Curve, this.IgnoreTimeScale);
					return;
				case MMF_Fade.FadeTypes.FadeOut:
					this.TargetFader.FadeOut(this.FeedbackDuration, this.Curve, this.IgnoreTimeScale);
					return;
				case MMF_Fade.FadeTypes.Custom:
					this.TargetFader.Fade(this.TargetAlpha, this.FeedbackDuration, this.Curve, this.IgnoreTimeScale);
					return;
				default:
					return;
				}
			}
			else
			{
				switch (this._fadeType)
				{
				case MMF_Fade.FadeTypes.FadeIn:
					MMFadeInEvent.Trigger(this.FeedbackDuration, this.Curve, this.ID, this.IgnoreTimeScale, this._position);
					return;
				case MMF_Fade.FadeTypes.FadeOut:
					MMFadeOutEvent.Trigger(this.FeedbackDuration, this.Curve, this.ID, this.IgnoreTimeScale, this._position);
					return;
				case MMF_Fade.FadeTypes.Custom:
					MMFadeEvent.Trigger(this.FeedbackDuration, this.TargetAlpha, this.Curve, this.ID, this.IgnoreTimeScale, this._position);
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x0005042E File Offset: 0x0004E62E
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Fade.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMFadeStopEvent.Trigger(this.ID, false);
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x00050454 File Offset: 0x0004E654
		protected virtual Vector3 GetPosition(Vector3 position)
		{
			switch (this.PositionMode)
			{
			case MMF_Fade.PositionModes.FeedbackPosition:
				return this.Owner.transform.position + this.PositionOffset;
			case MMF_Fade.PositionModes.Transform:
				return this.TargetTransform.position + this.PositionOffset;
			case MMF_Fade.PositionModes.WorldPosition:
				return this.TargetPosition + this.PositionOffset;
			case MMF_Fade.PositionModes.Script:
				return position + this.PositionOffset;
			default:
				return position + this.PositionOffset;
			}
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x000504DE File Offset: 0x0004E6DE
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Fade.FeedbackTypeAuthorized)
			{
				return;
			}
			MMFadeStopEvent.Trigger(this.ID, true);
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x000504FC File Offset: 0x0004E6FC
		public override void AutomaticShakerSetup()
		{
			if (Object.FindObjectOfType<MMFader>() != null)
			{
				return;
			}
			Canvas item = this.Owner.gameObject.MMFindOrCreateObjectOfType<Canvas>("FadeCanvas", null, false).Item1;
			item.renderMode = RenderMode.ScreenSpaceOverlay;
			Image item2 = item.gameObject.MMFindOrCreateObjectOfType<Image>("FadeImage", item.transform, true).Item1;
			item2.raycastTarget = false;
			item2.color = Color.black;
			RectTransform component = item2.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0f, 0f);
			component.anchorMax = new Vector2(1f, 1f);
			component.offsetMin = Vector2.zero;
			component.offsetMax = Vector2.zero;
			item2.gameObject.AddComponent<MMFader>();
			item2.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
			item2.gameObject.GetComponent<CanvasGroup>().interactable = false;
			MMDebug.DebugLogInfo("Added a MMFader to the scene. You're all set.", "", 3, true);
		}

		// Token: 0x040014FB RID: 5371
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040014FC RID: 5372
		[MMFInspectorGroup("Fade", true, 43, false, false)]
		[Tooltip("the type of fade we want to use when this feedback gets played")]
		public MMF_Fade.FadeTypes FadeType;

		// Token: 0x040014FD RID: 5373
		[Tooltip("the ID of the fader(s) to pilot")]
		public int ID;

		// Token: 0x040014FE RID: 5374
		[Tooltip("the duration (in seconds) of the fade")]
		public float Duration = 1f;

		// Token: 0x040014FF RID: 5375
		[Tooltip("the curve to use for this fade")]
		public MMTweenType Curve = new MMTweenType(MMTween.MMTweenCurve.EaseInCubic);

		// Token: 0x04001500 RID: 5376
		[Tooltip("whether or not this fade should ignore timescale")]
		public bool IgnoreTimeScale = true;

		// Token: 0x04001501 RID: 5377
		[Header("Custom")]
		[Tooltip("the target alpha we're aiming for with this fade")]
		public float TargetAlpha;

		// Token: 0x04001502 RID: 5378
		[Header("Position")]
		[Tooltip("the chosen way to position the fade")]
		public MMF_Fade.PositionModes PositionMode;

		// Token: 0x04001503 RID: 5379
		[Tooltip("the transform on which to center the fade")]
		[MMFEnumCondition("PositionMode", new int[] { 1 })]
		public Transform TargetTransform;

		// Token: 0x04001504 RID: 5380
		[Tooltip("the coordinates on which to center the fade")]
		[MMFEnumCondition("PositionMode", new int[] { 2 })]
		public Vector3 TargetPosition;

		// Token: 0x04001505 RID: 5381
		[Tooltip("the position offset to apply when centering the fade")]
		public Vector3 PositionOffset;

		// Token: 0x04001506 RID: 5382
		[Header("Optional Target")]
		[Tooltip("this field lets you bind a specific MMFader to this feedback. If left empty, the feedback will trigger a MMFadeEvent instead, targeting all matching faders. If you fill it, only that specific fader will be targeted.")]
		public MMFader TargetFader;

		// Token: 0x04001507 RID: 5383
		protected Vector3 _position;

		// Token: 0x04001508 RID: 5384
		protected MMF_Fade.FadeTypes _fadeType;

		// Token: 0x0200052D RID: 1325
		public enum FadeTypes
		{
			// Token: 0x040023C6 RID: 9158
			FadeIn,
			// Token: 0x040023C7 RID: 9159
			FadeOut,
			// Token: 0x040023C8 RID: 9160
			Custom
		}

		// Token: 0x0200052E RID: 1326
		public enum PositionModes
		{
			// Token: 0x040023CA RID: 9162
			FeedbackPosition,
			// Token: 0x040023CB RID: 9163
			Transform,
			// Token: 0x040023CC RID: 9164
			WorldPosition,
			// Token: 0x040023CD RID: 9165
			Script
		}
	}
}
