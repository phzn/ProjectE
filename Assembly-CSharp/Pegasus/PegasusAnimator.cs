using System;
using UnityEngine;

namespace Pegasus
{
	// Token: 0x020001A0 RID: 416
	public class PegasusAnimator : MonoBehaviour
	{
		// Token: 0x06000CCE RID: 3278 RVA: 0x00057940 File Offset: 0x00055B40
		private void Start()
		{
			if (this.m_character == null)
			{
				this.m_character = base.gameObject.transform;
			}
			if (this.m_animator == null)
			{
				this.m_animator = base.GetComponent<Animator>();
			}
			if (this.m_animator != null)
			{
				this.m_lastPosition = base.transform.position;
				if (this.m_idleAnimation != null)
				{
					this.ReplaceClip(this.m_animator, "HumanoidIdle", this.m_idleAnimation);
				}
				if (this.m_walkAnimation != null)
				{
					this.ReplaceClip(this.m_animator, "HumanoidWalk", this.m_walkAnimation);
				}
				if (this.m_runAnimation != null)
				{
					this.ReplaceClip(this.m_animator, "HumanoidRun", this.m_runAnimation);
				}
				this.PlayState(this.m_animationState, true);
			}
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x00057A24 File Offset: 0x00055C24
		private void Update()
		{
			if (this.m_animator == null)
			{
				return;
			}
			float num = Vector3.Distance(base.transform.position, this.m_lastPosition);
			this.m_lastPosition = base.transform.position;
			if (Time.deltaTime > 0f)
			{
				this.m_speed = Mathf.Lerp(this.m_speed, num / Time.deltaTime, Time.deltaTime * (1f / this.m_speedDamping));
				if (this.m_speed > this.m_maxWalkSpeed)
				{
					this.m_animationState = PegasusConstants.PegasusAnimationState.Running;
					this.m_animationMultiplier = this.m_speed / this.m_runSpeed;
				}
				else if (this.m_speed > 0f)
				{
					this.m_animationState = PegasusConstants.PegasusAnimationState.Walking;
					this.m_animationMultiplier = this.m_speed / this.m_walkSpeed;
				}
				else
				{
					this.m_animationState = PegasusConstants.PegasusAnimationState.Idle;
					this.m_animationMultiplier = 1f;
				}
			}
			else
			{
				this.m_speed = 0f;
				this.m_animationState = PegasusConstants.PegasusAnimationState.Idle;
				this.m_animationMultiplier = 1f;
			}
			if (this.m_animator != null && this.m_speed != this.m_lastSpeed)
			{
				this.m_lastSpeed = this.m_speed;
				this.m_animator.SetInteger(this.m_animationStateHash, (int)this.m_animationState);
				this.m_animator.SetFloat(this.m_animationMultiplierHash, this.m_animationMultiplier);
			}
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x00057B7C File Offset: 0x00055D7C
		private void PlayState(PegasusConstants.PegasusAnimationState newState, bool forceStateNow)
		{
			if (forceStateNow)
			{
				this.m_animationState = newState;
				this.m_animator.SetInteger(this.m_animationStateHash, (int)this.m_animationState);
				switch (this.m_animationState)
				{
				case PegasusConstants.PegasusAnimationState.Idle:
					this.m_animator.Play("Base Layer.Idle");
					break;
				case PegasusConstants.PegasusAnimationState.Walking:
					this.m_animator.Play("Base Layer.Walk");
					break;
				case PegasusConstants.PegasusAnimationState.Running:
					this.m_animator.Play("Base Layer.Run");
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			else if (this.m_animationState != newState)
			{
				this.m_animationState = newState;
				this.m_animator.SetInteger(this.m_animationStateHash, (int)this.m_animationState);
			}
			this.m_animator.SetFloat(this.m_animationMultiplierHash, this.m_animationMultiplier);
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x00057C40 File Offset: 0x00055E40
		private void ReplaceClip(Animator animator, string clipName, AnimationClip overrideClip)
		{
			AnimatorOverrideController animatorOverrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
			if (animatorOverrideController == null)
			{
				animatorOverrideController = new AnimatorOverrideController();
				animatorOverrideController.name = "PegasusRuntimeController";
				animatorOverrideController.runtimeAnimatorController = animator.runtimeAnimatorController;
			}
			animatorOverrideController[clipName] = overrideClip;
			if (animator.runtimeAnimatorController != animatorOverrideController)
			{
				animator.runtimeAnimatorController = animatorOverrideController;
			}
		}

		// Token: 0x04000CB0 RID: 3248
		[Header("Character And Animator")]
		[Tooltip("Drop your character here. By default it will select the character this script is attached to.")]
		public Transform m_character;

		// Token: 0x04000CB1 RID: 3249
		[Tooltip("Drop your animator here. By default it will select the animator on the character this script is attached to.")]
		public Animator m_animator;

		// Token: 0x04000CB2 RID: 3250
		[Tooltip("Select your initial animation state. This will be played immediately on start at runtime.")]
		public PegasusConstants.PegasusAnimationState m_animationState;

		// Token: 0x04000CB3 RID: 3251
		[Header("Optional Animation Overrides")]
		[Tooltip("Add your idle animation override here. This is optional, and the default animation will be used instead if not supplied.")]
		public AnimationClip m_idleAnimation;

		// Token: 0x04000CB4 RID: 3252
		[Tooltip("Add your walk animation override here. This is optional, and the default animation will be used instead if not supplied.")]
		public AnimationClip m_walkAnimation;

		// Token: 0x04000CB5 RID: 3253
		[Tooltip("Add your run animation override here. This is optional, and the default animation will be used instead if not supplied.")]
		public AnimationClip m_runAnimation;

		// Token: 0x04000CB6 RID: 3254
		[Header("Walk & Run Speeds (m/sec)")]
		[Tooltip("Walk animations will play when the character movement is greater than the idle speed and less than this speed.")]
		public float m_walkSpeed = 2f;

		// Token: 0x04000CB7 RID: 3255
		[Tooltip("Walk animations will play when the character movement is greater than the idle speed and less than this speed.")]
		public float m_maxWalkSpeed = 3.5f;

		// Token: 0x04000CB8 RID: 3256
		[Tooltip("Run animations will play when the character movement is greater than the walk speed.")]
		public float m_runSpeed = 7f;

		// Token: 0x04000CB9 RID: 3257
		private float m_speedDamping = 0.7f;

		// Token: 0x04000CBA RID: 3258
		private float m_speed;

		// Token: 0x04000CBB RID: 3259
		private float m_lastSpeed = float.MinValue;

		// Token: 0x04000CBC RID: 3260
		private Vector3 m_lastPosition = Vector3.zero;

		// Token: 0x04000CBD RID: 3261
		private float m_animationMultiplier = 1f;

		// Token: 0x04000CBE RID: 3262
		private int m_animationStateHash = Animator.StringToHash("AnimationState");

		// Token: 0x04000CBF RID: 3263
		private int m_animationMultiplierHash = Animator.StringToHash("AnimationMultiplier");
	}
}
