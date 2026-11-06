using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000292 RID: 658
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you trigger a blink on an MMBlink object.")]
	[FeedbackPath("Renderer/MMBlink")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	public class MMF_Blink : MMF_Feedback
	{
		// Token: 0x170001EB RID: 491
		// (get) Token: 0x0600127D RID: 4733 RVA: 0x0004FCD0 File Offset: 0x0004DED0
		// (set) Token: 0x0600127E RID: 4734 RVA: 0x0004FCDE File Offset: 0x0004DEDE
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

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x0600127F RID: 4735 RVA: 0x0004FCE7 File Offset: 0x0004DEE7
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x0004FCEA File Offset: 0x0004DEEA
		protected override void AutomateTargetAcquisition()
		{
			this.TargetBlink = this.FindAutomatedTarget<MMBlink>();
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x0004FCF8 File Offset: 0x0004DEF8
		public override void InitializeCustomAttributes()
		{
			this.GrabDurationFromBlinkButton = new MMF_Button("Grab Duration From Blink Component", new MMF_Button.ButtonMethod(this.GrabDurationFromBlink));
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x0004FD18 File Offset: 0x0004DF18
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Blink.FeedbackTypeAuthorized || this.TargetBlink == null)
			{
				return;
			}
			this.HandleBlink(this.TargetBlink);
			foreach (MMBlink mmblink in this.ExtraTargetBlinks)
			{
				this.HandleBlink(mmblink);
			}
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x0004FD98 File Offset: 0x0004DF98
		protected virtual void HandleBlink(MMBlink target)
		{
			target.TimescaleMode = this.ComputedTimescaleMode;
			switch (this.BlinkMode)
			{
			case MMF_Blink.BlinkModes.Toggle:
				target.ToggleBlinking();
				return;
			case MMF_Blink.BlinkModes.Start:
				target.StartBlinking();
				return;
			case MMF_Blink.BlinkModes.Stop:
				target.StopBlinking();
				return;
			default:
				return;
			}
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x0004FDE0 File Offset: 0x0004DFE0
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Blink.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetBlink.StopBlinking();
			foreach (MMBlink mmblink in this.ExtraTargetBlinks)
			{
				mmblink.StopBlinking();
			}
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x0004FE4C File Offset: 0x0004E04C
		public virtual void GrabDurationFromBlink()
		{
			if (this.TargetBlink != null)
			{
				this.Duration = this.TargetBlink.Duration;
			}
		}

		// Token: 0x040014DC RID: 5340
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040014DD RID: 5341
		[MMFInspectorGroup("Blink", true, 61, true, false)]
		[Tooltip("the target object to blink")]
		public MMBlink TargetBlink;

		// Token: 0x040014DE RID: 5342
		[Tooltip("an optional list of extra target objects to blink")]
		public List<MMBlink> ExtraTargetBlinks;

		// Token: 0x040014DF RID: 5343
		[Tooltip("the selected mode for this feedback")]
		public MMF_Blink.BlinkModes BlinkMode;

		// Token: 0x040014E0 RID: 5344
		[Tooltip("the duration of the blink. You can set it manually, or you can press the GrabDurationFromBlink button to automatically compute it. For performance reasons, this isn't updated unless you press the button, make sure you do so if you change the blink's duration.")]
		public float Duration;

		// Token: 0x040014E1 RID: 5345
		public MMF_Button GrabDurationFromBlinkButton;

		// Token: 0x0200052B RID: 1323
		public enum BlinkModes
		{
			// Token: 0x040023BC RID: 9148
			Toggle,
			// Token: 0x040023BD RID: 9149
			Start,
			// Token: 0x040023BE RID: 9150
			Stop
		}
	}
}
