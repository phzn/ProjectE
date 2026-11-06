using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001E6 RID: 486
	[AddComponentMenu("")]
	[FeedbackHelp("On play, this feedback will broadcast a MMFlashEvent. If you create a UI image with a MMFlash component on it (see example in the Demo scene), it will intercept that event, and flash (usually you'll want it to take the full size of your screen, but that's not mandatory). In the feedback's inspector, you can define the color of the flash, its duration, alpha, and a FlashID. That FlashID needs to be the same on your feedback and MMFlash for them to work together. This allows you to have multiple MMFlashs in your scene, and flash them separately.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Camera/Flash")]
	public class MMF_Flash : MMF_Feedback
	{
		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000C73 RID: 3187 RVA: 0x0003609F File Offset: 0x0003429F
		// (set) Token: 0x06000C74 RID: 3188 RVA: 0x000360AD File Offset: 0x000342AD
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.FlashDuration);
			}
			set
			{
				this.FlashDuration = value;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x000360B6 File Offset: 0x000342B6
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x000360B9 File Offset: 0x000342B9
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x000360BC File Offset: 0x000342BC
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Flash.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			if (this.TargetFlash != null)
			{
				this.TargetFlash.Flash(this.FlashColor, this.FlashDuration * num, this.FlashAlpha, this.ComputedTimescaleMode);
				return;
			}
			MMFlashEvent.Trigger(this.FlashColor, this.FeedbackDuration * num, this.FlashAlpha, this.FlashID, this.ChannelData, this.ComputedTimescaleMode, false);
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x00036144 File Offset: 0x00034344
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Flash.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMFlashEvent.Trigger(this.FlashColor, this.FeedbackDuration, this.FlashAlpha, this.FlashID, this.ChannelData, this.ComputedTimescaleMode, true);
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x00036193 File Offset: 0x00034393
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Flash.FeedbackTypeAuthorized)
			{
				return;
			}
			MMFlashEvent.Trigger(this.FlashColor, this.FeedbackDuration, this.FlashAlpha, this.FlashID, this.ChannelData, this.ComputedTimescaleMode, true);
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x000361D0 File Offset: 0x000343D0
		public override void AutomaticShakerSetup()
		{
			if (Object.FindObjectOfType<MMFlash>() != null)
			{
				return;
			}
			Canvas item = this.Owner.gameObject.MMFindOrCreateObjectOfType<Canvas>("FlashCanvas", null, false).Item1;
			item.renderMode = RenderMode.ScreenSpaceOverlay;
			Image item2 = item.gameObject.MMFindOrCreateObjectOfType<Image>("FlashImage", item.transform, true).Item1;
			item2.raycastTarget = false;
			item2.color = Color.white;
			RectTransform component = item2.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0f, 0f);
			component.anchorMax = new Vector2(1f, 1f);
			component.offsetMin = Vector2.zero;
			component.offsetMax = Vector2.zero;
			item2.gameObject.AddComponent<MMFlash>();
			item2.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
			item2.gameObject.GetComponent<CanvasGroup>().interactable = false;
			MMDebug.DebugLogInfo("Added a MMFlash to the scene. You're all set.", "", 3, true);
		}

		// Token: 0x04000E32 RID: 3634
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000E33 RID: 3635
		[MMFInspectorGroup("Flash", true, 37, false, false)]
		[Tooltip("the color of the flash")]
		public Color FlashColor = Color.white;

		// Token: 0x04000E34 RID: 3636
		[Tooltip("the flash duration (in seconds)")]
		public float FlashDuration = 0.2f;

		// Token: 0x04000E35 RID: 3637
		[Tooltip("the alpha of the flash")]
		public float FlashAlpha = 1f;

		// Token: 0x04000E36 RID: 3638
		[Tooltip("the ID of the flash (usually 0). You can specify on each MMFlash object an ID, allowing you to have different flash images in one scene and call them separately (one for damage, one for health pickups, etc)")]
		public int FlashID;

		// Token: 0x04000E37 RID: 3639
		[Header("Optional Target")]
		[Tooltip("this field lets you bind a specific MMFlash to this feedback. If left empty, the feedback will trigger a MMFlashEvent instead, targeting all matching flashes. If you fill it, only that specific MMFlash will be targeted.")]
		public MMFlash TargetFlash;
	}
}
