using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001FC RID: 508
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to control one or more target MMF Players")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Feedbacks/MMF Player Control")]
	public class MMF_PlayerControl : MMF_Feedback
	{
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000D6A RID: 3434 RVA: 0x0003A930 File Offset: 0x00038B30
		public override bool HasChannel
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000D6B RID: 3435 RVA: 0x0003A934 File Offset: 0x00038B34
		public override float FeedbackDuration
		{
			get
			{
				if (this.TargetPlayers == null)
				{
					return 0f;
				}
				if (!this.WaitForTargetPlayersToFinish)
				{
					return 0f;
				}
				if (this.Mode == MMF_PlayerControl.Modes.PlayFeedbacks && this.TargetPlayers.Count > 0)
				{
					float num = 0f;
					foreach (MMF_Player mmf_Player in this.TargetPlayers)
					{
						if (mmf_Player != null && num < mmf_Player.TotalDuration)
						{
							num = mmf_Player.TotalDuration;
						}
					}
					return num;
				}
				return 0f;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000D6C RID: 3436 RVA: 0x0003A9D8 File Offset: 0x00038BD8
		public override bool IsPlaying
		{
			get
			{
				if (this.WaitForTargetPlayersToFinish)
				{
					using (List<MMF_Player>.Enumerator enumerator = this.TargetPlayers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.IsPlaying)
							{
								return true;
							}
						}
					}
					return false;
				}
				return false;
			}
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x0003AA3C File Offset: 0x00038C3C
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x0003AA48 File Offset: 0x00038C48
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.TargetPlayers.Count == 0)
			{
				return;
			}
			if (!this.Active || !MMF_PlayerControl.FeedbackTypeAuthorized)
			{
				return;
			}
			switch (this.Mode)
			{
			case MMF_PlayerControl.Modes.PlayFeedbacks:
			{
				using (List<MMF_Player>.Enumerator enumerator = this.TargetPlayers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MMF_Player mmf_Player = enumerator.Current;
						mmf_Player.PlayFeedbacks(position, feedbacksIntensity, false);
					}
					return;
				}
				break;
			}
			case MMF_PlayerControl.Modes.StopFeedbacks:
				break;
			case MMF_PlayerControl.Modes.PauseFeedbacks:
				goto IL_00DB;
			case MMF_PlayerControl.Modes.ResumeFeedbacks:
				goto IL_0111;
			case MMF_PlayerControl.Modes.Initialization:
				goto IL_0147;
			case MMF_PlayerControl.Modes.PlayFeedbacksInReverse:
				goto IL_017E;
			case MMF_PlayerControl.Modes.PlayFeedbacksOnlyIfReversed:
				goto IL_01B7;
			case MMF_PlayerControl.Modes.PlayFeedbacksOnlyIfNormalDirection:
				goto IL_01F0;
			case MMF_PlayerControl.Modes.ResetFeedbacks:
				goto IL_0229;
			case MMF_PlayerControl.Modes.Revert:
				goto IL_025F;
			case MMF_PlayerControl.Modes.SetDirectionTopToBottom:
				goto IL_0295;
			case MMF_PlayerControl.Modes.SetDirectionBottomToTop:
				goto IL_02CB;
			case MMF_PlayerControl.Modes.RestoreInitialValues:
				goto IL_0301;
			case MMF_PlayerControl.Modes.SkipToTheEnd:
				goto IL_0334;
			case MMF_PlayerControl.Modes.RefreshCache:
				goto IL_0367;
			default:
				goto IL_039A;
			}
			using (List<MMF_Player>.Enumerator enumerator = this.TargetPlayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MMF_Player mmf_Player2 = enumerator.Current;
					mmf_Player2.StopFeedbacks();
				}
				return;
			}
			IL_00DB:
			using (List<MMF_Player>.Enumerator enumerator = this.TargetPlayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MMF_Player mmf_Player3 = enumerator.Current;
					mmf_Player3.PauseFeedbacks();
				}
				return;
			}
			IL_0111:
			using (List<MMF_Player>.Enumerator enumerator = this.TargetPlayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MMF_Player mmf_Player4 = enumerator.Current;
					mmf_Player4.ResumeFeedbacks();
				}
				return;
			}
			IL_0147:
			using (List<MMF_Player>.Enumerator enumerator = this.TargetPlayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MMF_Player mmf_Player5 = enumerator.Current;
					mmf_Player5.Initialization(false);
				}
				return;
			}
			IL_017E:
			using (List<MMF_Player>.Enumerator enumerator = this.TargetPlayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MMF_Player mmf_Player6 = enumerator.Current;
					mmf_Player6.PlayFeedbacksInReverse(position, feedbacksIntensity, false);
				}
				return;
			}
			IL_01B7:
			using (List<MMF_Player>.Enumerator enumerator = this.TargetPlayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MMF_Player mmf_Player7 = enumerator.Current;
					mmf_Player7.PlayFeedbacksOnlyIfReversed(position, feedbacksIntensity, false);
				}
				return;
			}
			IL_01F0:
			using (List<MMF_Player>.Enumerator enumerator = this.TargetPlayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MMF_Player mmf_Player8 = enumerator.Current;
					mmf_Player8.PlayFeedbacksOnlyIfNormalDirection(position, feedbacksIntensity, false);
				}
				return;
			}
			IL_0229:
			using (List<MMF_Player>.Enumerator enumerator = this.TargetPlayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MMF_Player mmf_Player9 = enumerator.Current;
					mmf_Player9.ResetFeedbacks();
				}
				return;
			}
			IL_025F:
			using (List<MMF_Player>.Enumerator enumerator = this.TargetPlayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MMF_Player mmf_Player10 = enumerator.Current;
					mmf_Player10.Revert();
				}
				return;
			}
			IL_0295:
			using (List<MMF_Player>.Enumerator enumerator = this.TargetPlayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MMF_Player mmf_Player11 = enumerator.Current;
					mmf_Player11.SetDirectionTopToBottom();
				}
				return;
			}
			IL_02CB:
			using (List<MMF_Player>.Enumerator enumerator = this.TargetPlayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MMF_Player mmf_Player12 = enumerator.Current;
					mmf_Player12.SetDirectionBottomToTop();
				}
				return;
			}
			IL_0301:
			using (List<MMF_Player>.Enumerator enumerator = this.TargetPlayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MMF_Player mmf_Player13 = enumerator.Current;
					mmf_Player13.RestoreInitialValues();
				}
				return;
			}
			IL_0334:
			using (List<MMF_Player>.Enumerator enumerator = this.TargetPlayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MMF_Player mmf_Player14 = enumerator.Current;
					mmf_Player14.SkipToTheEnd();
				}
				return;
			}
			IL_0367:
			using (List<MMF_Player>.Enumerator enumerator = this.TargetPlayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MMF_Player mmf_Player15 = enumerator.Current;
					mmf_Player15.RefreshCache();
				}
				return;
			}
			IL_039A:
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x04000F59 RID: 3929
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000F5A RID: 3930
		[MMFInspectorGroup("MMF Player", true, 79, false, false)]
		[Tooltip("a specific MMFeedbacks / MMF_Player to play")]
		public List<MMF_Player> TargetPlayers;

		// Token: 0x04000F5B RID: 3931
		[Tooltip("if this is true, this feedback will be considered as Playing while any of the target players are still Playing")]
		public bool WaitForTargetPlayersToFinish = true;

		// Token: 0x04000F5C RID: 3932
		public MMF_PlayerControl.Modes Mode;

		// Token: 0x020004D3 RID: 1235
		public enum Modes
		{
			// Token: 0x04002272 RID: 8818
			PlayFeedbacks,
			// Token: 0x04002273 RID: 8819
			StopFeedbacks,
			// Token: 0x04002274 RID: 8820
			PauseFeedbacks,
			// Token: 0x04002275 RID: 8821
			ResumeFeedbacks,
			// Token: 0x04002276 RID: 8822
			Initialization,
			// Token: 0x04002277 RID: 8823
			PlayFeedbacksInReverse,
			// Token: 0x04002278 RID: 8824
			PlayFeedbacksOnlyIfReversed,
			// Token: 0x04002279 RID: 8825
			PlayFeedbacksOnlyIfNormalDirection,
			// Token: 0x0400227A RID: 8826
			ResetFeedbacks,
			// Token: 0x0400227B RID: 8827
			Revert,
			// Token: 0x0400227C RID: 8828
			SetDirectionTopToBottom,
			// Token: 0x0400227D RID: 8829
			SetDirectionBottomToTop,
			// Token: 0x0400227E RID: 8830
			RestoreInitialValues,
			// Token: 0x0400227F RID: 8831
			SkipToTheEnd,
			// Token: 0x04002280 RID: 8832
			RefreshCache
		}
	}
}
