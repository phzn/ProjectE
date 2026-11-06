using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001FA RID: 506
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will cause a pause when met, preventing any other feedback lower in the sequence to run until it's complete.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Pause/Pause")]
	public class MMF_Pause : MMF_Feedback
	{
		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x0003A5B1 File Offset: 0x000387B1
		public override IEnumerator Pause
		{
			get
			{
				return this.PauseWait();
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000D59 RID: 3417 RVA: 0x0003A5B9 File Offset: 0x000387B9
		// (set) Token: 0x06000D5A RID: 3418 RVA: 0x0003A5C7 File Offset: 0x000387C7
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.PauseDuration);
			}
			set
			{
				this.PauseDuration = value;
			}
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x0003A5D0 File Offset: 0x000387D0
		protected virtual IEnumerator PauseWait()
		{
			yield return this.WaitFor(this.ApplyTimeMultiplier(this.PauseDuration));
			yield break;
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x0003A5E0 File Offset: 0x000387E0
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			this.ScriptDrivenPause = this.ScriptDriven;
			this.ScriptDrivenPauseAutoResume = (this.AutoResume ? this.AutoResumeAfter : (-1f));
			if (this.RandomizePauseDuration)
			{
				this.PauseDuration = Random.Range(this.MinPauseDuration, this.MaxPauseDuration);
			}
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x0003A63A File Offset: 0x0003883A
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Pause.FeedbackTypeAuthorized)
			{
				return;
			}
			this.ProcessNewPauseDuration();
			this.Owner.StartCoroutine(this.PlayPause());
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x0003A664 File Offset: 0x00038864
		protected virtual void ProcessNewPauseDuration()
		{
			if (this.RandomizePauseDuration && this.RandomizeOnEachPlay)
			{
				this.PauseDuration = Random.Range(this.MinPauseDuration, this.MaxPauseDuration);
			}
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0003A68D File Offset: 0x0003888D
		protected virtual IEnumerator PlayPause()
		{
			yield return this.Pause;
			yield break;
		}

		// Token: 0x04000F4E RID: 3918
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000F4F RID: 3919
		[MMFInspectorGroup("Pause", true, 32, false, false)]
		[Tooltip("the duration of the pause, in seconds")]
		public float PauseDuration = 1f;

		// Token: 0x04000F50 RID: 3920
		public bool RandomizePauseDuration;

		// Token: 0x04000F51 RID: 3921
		[MMFCondition("RandomizePauseDuration", true)]
		public float MinPauseDuration = 1f;

		// Token: 0x04000F52 RID: 3922
		[MMFCondition("RandomizePauseDuration", true)]
		public float MaxPauseDuration = 3f;

		// Token: 0x04000F53 RID: 3923
		[MMFCondition("RandomizePauseDuration", true)]
		public bool RandomizeOnEachPlay = true;

		// Token: 0x04000F54 RID: 3924
		[Tooltip("if this is true, you'll need to call the Resume() method on the host MMFeedbacks for this pause to stop, and the rest of the sequence to play")]
		public bool ScriptDriven;

		// Token: 0x04000F55 RID: 3925
		[Tooltip("if this is true, a script driven pause will resume after its AutoResumeAfter delay, whether it has been manually resumed or not")]
		[MMFCondition("ScriptDriven", true)]
		public bool AutoResume;

		// Token: 0x04000F56 RID: 3926
		[Tooltip("the duration after which to auto resume, regardless of manual resume calls beforehand")]
		[MMFCondition("AutoResume", true)]
		public float AutoResumeAfter = 0.25f;
	}
}
