using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001FB RID: 507
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to chain any number of target MMF Players and play them in sequence, with optional delays before and after")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Feedbacks/MMF Player Chain")]
	public class MMF_PlayerChain : MMF_Feedback
	{
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000D62 RID: 3426 RVA: 0x0003A6E0 File Offset: 0x000388E0
		public override float FeedbackDuration
		{
			get
			{
				if (this.Players == null || this.Players.Count == 0)
				{
					return 0f;
				}
				float num = 0f;
				foreach (MMF_PlayerChain.PlayerChainItem playerChainItem in this.Players)
				{
					if (playerChainItem != null && !(playerChainItem.TargetPlayer == null) && !playerChainItem.Inactive)
					{
						num += playerChainItem.Delay.x;
						num += playerChainItem.TargetPlayer.TotalDuration;
						num += playerChainItem.Delay.y;
					}
				}
				return num;
			}
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x0003A794 File Offset: 0x00038994
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.Players == null || this.Players.Count == 0)
			{
				return;
			}
			if (!this.Active || !MMF_PlayerChain.FeedbackTypeAuthorized)
			{
				return;
			}
			this.Owner.StartCoroutine(this.PlayChain());
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x0003A7CE File Offset: 0x000389CE
		protected virtual IEnumerator PlayChain()
		{
			this.IsPlaying = true;
			foreach (MMF_PlayerChain.PlayerChainItem item in this.Players)
			{
				if (item != null && !(item.TargetPlayer == null) && !item.Inactive)
				{
					if (item.Delay.x > 0f)
					{
						yield return this.WaitFor(item.Delay.x);
					}
					if (item.WaitUntilComplete)
					{
						item.TargetPlayer.PlayFeedbacks();
						yield return this.WaitFor(item.TargetPlayer.TotalDuration);
					}
					else
					{
						item.TargetPlayer.PlayFeedbacks();
					}
					if (item.Delay.y > 0f)
					{
						yield return this.WaitFor(item.Delay.y);
					}
					item = null;
				}
			}
			List<MMF_PlayerChain.PlayerChainItem>.Enumerator enumerator = default(List<MMF_PlayerChain.PlayerChainItem>.Enumerator);
			while (this.FeedbacksStillPlaying())
			{
				yield return null;
			}
			this.IsPlaying = false;
			yield break;
			yield break;
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0003A7E0 File Offset: 0x000389E0
		protected virtual bool FeedbacksStillPlaying()
		{
			bool flag = false;
			using (List<MMF_PlayerChain.PlayerChainItem>.Enumerator enumerator = this.Players.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.TargetPlayer.IsPlaying)
					{
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0003A83C File Offset: 0x00038A3C
		protected override void CustomSkipToTheEnd(Vector3 position, float feedbacksIntensity = 1f)
		{
			foreach (MMF_PlayerChain.PlayerChainItem playerChainItem in this.Players)
			{
				if (playerChainItem != null && !(playerChainItem.TargetPlayer == null) && !playerChainItem.Inactive)
				{
					playerChainItem.TargetPlayer.PlayFeedbacks();
					playerChainItem.TargetPlayer.SkipToTheEnd();
				}
			}
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0003A8B8 File Offset: 0x00038AB8
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_PlayerChain.FeedbackTypeAuthorized)
			{
				return;
			}
			foreach (MMF_PlayerChain.PlayerChainItem playerChainItem in this.Players)
			{
				playerChainItem.TargetPlayer.RestoreInitialValues();
			}
		}

		// Token: 0x04000F57 RID: 3927
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000F58 RID: 3928
		[MMFInspectorGroup("Feedbacks", true, 79, false, false)]
		[Tooltip("the list of MMF Player that make up the chain. The chain's items will be played from index 0 to the last in the list")]
		public List<MMF_PlayerChain.PlayerChainItem> Players;

		// Token: 0x020004D1 RID: 1233
		[Serializable]
		public class PlayerChainItem
		{
			// Token: 0x04002268 RID: 8808
			[Tooltip("the target MMF Player")]
			public MMF_Player TargetPlayer;

			// Token: 0x04002269 RID: 8809
			[Tooltip("a delay in seconds to wait for before playing this MMF Player (x) and after (y)")]
			[MMVector(new string[] { "Before", "After" })]
			public Vector2 Delay;

			// Token: 0x0400226A RID: 8810
			[Tooltip("whether this player is active in the list or not. Inactive players will be skipped when playing the chain of players")]
			public bool Inactive;

			// Token: 0x0400226B RID: 8811
			[Tooltip("if this is true, the sequence will be blocked until this player has completed playing")]
			public bool WaitUntilComplete = true;
		}
	}
}
