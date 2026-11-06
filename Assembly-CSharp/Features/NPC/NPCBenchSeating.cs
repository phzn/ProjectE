using System;
using ProjectExhibitionist.BenchSeating;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000ED5 RID: 3797
	internal sealed class NPCBenchSeating
	{
		// Token: 0x1700128C RID: 4748
		// (get) Token: 0x060071AA RID: 29098 RVA: 0x002E310B File Offset: 0x002E130B
		public bool Active
		{
			get
			{
				return this.ownsRootMotionSuppression;
			}
		}

		// Token: 0x1700128D RID: 4749
		// (get) Token: 0x060071AB RID: 29099 RVA: 0x002E3113 File Offset: 0x002E1313
		public bool HasPresentation
		{
			get
			{
				return this.animation != null;
			}
		}

		// Token: 0x1700128E RID: 4750
		// (get) Token: 0x060071AC RID: 29100 RVA: 0x002E311E File Offset: 0x002E131E
		// (set) Token: 0x060071AD RID: 29101 RVA: 0x002E3126 File Offset: 0x002E1326
		public bool OwnsBodyPose { get; private set; }

		// Token: 0x1700128F RID: 4751
		// (get) Token: 0x060071AE RID: 29102 RVA: 0x002E312F File Offset: 0x002E132F
		public Vector3 EntryPosition
		{
			get
			{
				if (this.animation == null)
				{
					return this.owner.transform.position;
				}
				return this.animation.EntryPosition;
			}
		}

		// Token: 0x17001290 RID: 4752
		// (get) Token: 0x060071AF RID: 29103 RVA: 0x002E3155 File Offset: 0x002E1355
		public Quaternion EntryRotation
		{
			get
			{
				if (this.animation == null)
				{
					return this.owner.transform.rotation;
				}
				return this.animation.EntryRotation;
			}
		}

		// Token: 0x17001291 RID: 4753
		// (get) Token: 0x060071B0 RID: 29104 RVA: 0x002E317B File Offset: 0x002E137B
		public Vector3 ExitPosition
		{
			get
			{
				if (this.animation == null)
				{
					return this.owner.transform.position;
				}
				return this.animation.ExitPosition;
			}
		}

		// Token: 0x17001292 RID: 4754
		// (get) Token: 0x060071B1 RID: 29105 RVA: 0x002E31A1 File Offset: 0x002E13A1
		public bool HasFinishedStanding
		{
			get
			{
				return this.standing && this.animation != null && this.animation.IsComplete;
			}
		}

		// Token: 0x17001293 RID: 4755
		// (get) Token: 0x060071B2 RID: 29106 RVA: 0x002E31C0 File Offset: 0x002E13C0
		public bool HasFinishedRelease
		{
			get
			{
				return this.releasing && this.animation != null && this.animation.IsComplete;
			}
		}

		// Token: 0x060071B3 RID: 29107 RVA: 0x002E31DF File Offset: 0x002E13DF
		public NPCBenchSeating(NPCWanderController owner, AStarNPCController controller)
		{
			this.owner = owner;
			this.controller = controller;
		}

		// Token: 0x060071B4 RID: 29108 RVA: 0x002E31F8 File Offset: 0x002E13F8
		public bool Begin(BenchSeat target)
		{
			this.End(false);
			if (target == null || this.controller == null || this.controller.animator == null || !target.isActiveAndEnabled || !target.IsReservedBy(this.owner.gameObject.GetInstanceID()))
			{
				return false;
			}
			NPCAnimationStance component = this.owner.GetComponent<NPCAnimationStance>();
			bool flag = component != null && component.Stance >= 0.5f;
			this.animation = new BenchAnimationPlayer(this.controller.animator, target, flag, 0.29f, true);
			if (!this.animation.IsValid)
			{
				this.animation.Dispose();
				this.animation = null;
				return false;
			}
			this.seat = target;
			this.originalRootMotionSuppression = this.controller.SuppressRootMotionPosition;
			this.ownsRootMotionSuppression = true;
			this.controller.SuppressRootMotionPosition = true;
			return true;
		}

		// Token: 0x060071B5 RID: 29109 RVA: 0x002E32EC File Offset: 0x002E14EC
		public bool BeginSitting(out float duration)
		{
			duration = 0f;
			if (this.seat == null || this.animation == null || !this.animation.IsValid || !this.seat.Occupy(this.owner.gameObject.GetInstanceID()))
			{
				return false;
			}
			if (Vector3.Distance(this.owner.transform.position, this.EntryPosition) > 0.035f || Quaternion.Angle(this.owner.transform.rotation, this.EntryRotation) > 2f)
			{
				return false;
			}
			if (!this.animation.PlaySitDown())
			{
				this.animation.Dispose();
				this.animation = null;
				return false;
			}
			this.controller.SetAction(NPCInterestPoint.ActionType.None);
			this.controller.SetSpeedOverride(0f);
			this.controller.SetPosture(NPCInterestPoint.PostureType.Sitting);
			this.OwnsBodyPose = true;
			duration = Mathf.Max(0.05f, this.animation.SitDownDuration);
			this.waitingForIdle = true;
			this.standing = false;
			this.releasing = false;
			return true;
		}

		// Token: 0x060071B6 RID: 29110 RVA: 0x002E3408 File Offset: 0x002E1608
		public bool BeginStanding(out float duration)
		{
			duration = 0f;
			if (this.animation == null)
			{
				return false;
			}
			this.waitingForIdle = false;
			this.controller.SetPosture(NPCInterestPoint.PostureType.Standing);
			if (!this.animation.PlayStandUp())
			{
				return false;
			}
			duration = Mathf.Max(0.05f, this.animation.StandUpDuration);
			this.standing = true;
			return true;
		}

		// Token: 0x060071B7 RID: 29111 RVA: 0x002E3468 File Offset: 0x002E1668
		public void Tick(float delta)
		{
			if (this.animation == null)
			{
				return;
			}
			if (NPCSimulationPause.IsPaused)
			{
				delta = 0f;
			}
			this.animation.Tick(delta);
			if (NPCSimulationPause.IsPaused)
			{
				return;
			}
			if (!this.waitingForIdle)
			{
				return;
			}
			if (!this.animation.IsComplete)
			{
				return;
			}
			this.waitingForIdle = false;
			this.standing = false;
			this.animation.PlayIdle();
		}

		// Token: 0x060071B8 RID: 29112 RVA: 0x002E34D0 File Offset: 0x002E16D0
		public void BeginRelease()
		{
			if (this.animation == null)
			{
				return;
			}
			this.waitingForIdle = false;
			this.standing = false;
			this.releasing = true;
			this.ResumeNativeLocomotion();
			this.animation.BeginRelease();
		}

		// Token: 0x060071B9 RID: 29113 RVA: 0x002E3501 File Offset: 0x002E1701
		public void CompleteStanding(bool resumeLocomotion = false)
		{
			BenchAnimationPlayer benchAnimationPlayer = this.animation;
			if (benchAnimationPlayer != null)
			{
				benchAnimationPlayer.Dispose();
			}
			this.animation = null;
			this.OwnsBodyPose = false;
			this.waitingForIdle = false;
			this.standing = false;
			this.releasing = false;
			if (resumeLocomotion)
			{
				this.ResumeNativeLocomotion();
			}
		}

		// Token: 0x060071BA RID: 29114 RVA: 0x002E3540 File Offset: 0x002E1740
		private void ResumeNativeLocomotion()
		{
			if (this.controller == null || this.controller.animator == null || this.controller.animator.runtimeAnimatorController == null)
			{
				return;
			}
			Animator animator = this.controller.animator;
			int num = Animator.StringToHash("Base Layer.Standing.Normal Movement");
			if (!animator.HasState(0, num))
			{
				num = Animator.StringToHash("Base Layer.Normal Movement");
			}
			if (animator.HasState(0, num))
			{
				animator.Play(num, 0, 0f);
			}
		}

		// Token: 0x060071BB RID: 29115 RVA: 0x002E35CC File Offset: 0x002E17CC
		public void End(bool relocateToExit)
		{
			this.CompleteStanding(false);
			if (!this.ownsRootMotionSuppression)
			{
				return;
			}
			Vector3 vector;
			Quaternion quaternion;
			if (this.seat != null && relocateToExit && this.owner != null && this.owner.gameObject.activeInHierarchy && this.controller != null && this.controller.IsManualMode && this.seat.TryGetExitPose(this.owner.transform, out vector, out quaternion, false))
			{
				this.owner.transform.SetPositionAndRotation(vector, quaternion);
			}
			if (this.controller != null)
			{
				this.controller.SuppressRootMotionPosition = this.originalRootMotionSuppression;
			}
			this.ownsRootMotionSuppression = false;
			this.seat = null;
		}

		// Token: 0x0400749D RID: 29853
		private readonly NPCWanderController owner;

		// Token: 0x0400749E RID: 29854
		private readonly AStarNPCController controller;

		// Token: 0x0400749F RID: 29855
		private BenchSeat seat;

		// Token: 0x040074A0 RID: 29856
		private BenchAnimationPlayer animation;

		// Token: 0x040074A1 RID: 29857
		private bool originalRootMotionSuppression;

		// Token: 0x040074A2 RID: 29858
		private bool ownsRootMotionSuppression;

		// Token: 0x040074A3 RID: 29859
		private bool waitingForIdle;

		// Token: 0x040074A4 RID: 29860
		private bool standing;

		// Token: 0x040074A5 RID: 29861
		private bool releasing;
	}
}
