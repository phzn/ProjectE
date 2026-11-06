using System;
using UnityEngine;

namespace Features.NPCs.Runtime
{
	// Token: 0x02000EBC RID: 3772
	[RequireComponent(typeof(Animator))]
	public class StadiumFanReactionDriver : MonoBehaviour
	{
		// Token: 0x06007107 RID: 28935 RVA: 0x002DE80B File Offset: 0x002DCA0B
		private void Awake()
		{
			this.animator = base.GetComponent<Animator>();
			this.trackedSmr = base.GetComponentInChildren<SkinnedMeshRenderer>(true);
			if (this.deterministic)
			{
				Random.InitState(base.GetInstanceID());
			}
			this.ScheduleNextPick();
		}

		// Token: 0x06007108 RID: 28936 RVA: 0x002DE83F File Offset: 0x002DCA3F
		private void OnEnable()
		{
			this.ScheduleNextPick();
		}

		// Token: 0x06007109 RID: 28937 RVA: 0x002DE848 File Offset: 0x002DCA48
		private void Update()
		{
			if (this.animator == null || !this.animator.isActiveAndEnabled || this.animator.runtimeAnimatorController == null)
			{
				return;
			}
			if (this.trackedSmr != null && !this.trackedSmr.isVisible)
			{
				return;
			}
			bool flag = this.animator.GetCurrentAnimatorStateInfo(0).shortNameHash == StadiumFanReactionDriver.IdleStateHash;
			if (flag)
			{
				if (!this.wasInIdle)
				{
					this.animator.SetInteger(StadiumFanReactionDriver.ReactionIndexHash, -1);
					this.ScheduleNextPick();
				}
				if (Time.time >= this.nextPickTime)
				{
					this.animator.SetInteger(StadiumFanReactionDriver.ReactionIndexHash, Random.Range(0, 3));
				}
			}
			else if (this.wasInIdle)
			{
				this.animator.SetInteger(StadiumFanReactionDriver.ReactionIndexHash, -1);
			}
			this.wasInIdle = flag;
		}

		// Token: 0x0600710A RID: 28938 RVA: 0x002DE924 File Offset: 0x002DCB24
		private void ScheduleNextPick()
		{
			float num = Mathf.Max(0f, this.minIdleDelay);
			float num2 = Mathf.Max(num, this.maxIdleDelay);
			this.nextPickTime = Time.time + Random.Range(num, num2);
		}

		// Token: 0x040073C3 RID: 29635
		private const int ReactionCount = 3;

		// Token: 0x040073C4 RID: 29636
		private const int ResetValue = -1;

		// Token: 0x040073C5 RID: 29637
		[Tooltip("Min seconds to wait in Idle before picking the next reaction.")]
		public float minIdleDelay = 0.5f;

		// Token: 0x040073C6 RID: 29638
		[Tooltip("Max seconds to wait in Idle before picking the next reaction.")]
		public float maxIdleDelay = 3f;

		// Token: 0x040073C7 RID: 29639
		[Tooltip("If true, randomization is seeded per-instance for repeatable variation across reloads.")]
		public bool deterministic = true;

		// Token: 0x040073C8 RID: 29640
		private static readonly int ReactionIndexHash = Animator.StringToHash("ReactionIndex");

		// Token: 0x040073C9 RID: 29641
		private static readonly int IdleStateHash = Animator.StringToHash("Idle");

		// Token: 0x040073CA RID: 29642
		private Animator animator;

		// Token: 0x040073CB RID: 29643
		private SkinnedMeshRenderer trackedSmr;

		// Token: 0x040073CC RID: 29644
		private float nextPickTime;

		// Token: 0x040073CD RID: 29645
		private bool wasInIdle;
	}
}
