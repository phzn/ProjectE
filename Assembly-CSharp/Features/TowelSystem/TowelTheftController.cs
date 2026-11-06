using System;
using System.Collections;
using Features.Detection;
using Features.NPC.StateMachine;
using Features.Slip;
using UnityEngine;
using UnityEngine.Events;

namespace Features.TowelSystem
{
	// Token: 0x02000C08 RID: 3080
	public class TowelTheftController : MonoBehaviour
	{
		// Token: 0x06005906 RID: 22790 RVA: 0x0022CF86 File Offset: 0x0022B186
		private void Awake()
		{
			if (this.slipController == null)
			{
				this.slipController = base.GetComponent<ClothSlipController>();
			}
			if (this.slipController == null)
			{
				this.slipController = base.GetComponentInParent<ClothSlipController>();
			}
		}

		// Token: 0x06005907 RID: 22791 RVA: 0x0022CFBC File Offset: 0x0022B1BC
		private void Update()
		{
			if (this._routine != null || this.slipController == null)
			{
				return;
			}
			if (!this.slipController.HasSlippedPiece(this.wornPieceName) || this.slipController.WasStolenPiece(this.wornPieceName))
			{
				return;
			}
			GameObject gameObject = this.slipController.EnvInstanceFor(this.wornPieceName);
			if (gameObject == null)
			{
				return;
			}
			if (Time.time < this._nextCheck)
			{
				return;
			}
			this._nextCheck = Time.time + this.checkInterval;
			NPCStateMachine npcstateMachine = this.FindCandidate(gameObject.transform.position);
			if (npcstateMachine != null)
			{
				this._routine = base.StartCoroutine(this.StealRoutine(npcstateMachine, gameObject));
			}
		}

		// Token: 0x06005908 RID: 22792 RVA: 0x0022D074 File Offset: 0x0022B274
		private NPCStateMachine FindCandidate(Vector3 towelPosition)
		{
			NPCStateMachine npcstateMachine = null;
			float num = float.MaxValue;
			foreach (NPCStateMachine npcstateMachine2 in Object.FindObjectsByType<NPCStateMachine>(FindObjectsSortMode.None))
			{
				if (!(npcstateMachine2 == null) && npcstateMachine2.IsInitialized && npcstateMachine2.IsInNormalState && npcstateMachine2.Context != null && !(npcstateMachine2.Context.Controller == null))
				{
					float num2 = Vector3.Distance(npcstateMachine2.transform.position, towelPosition);
					if (num2 <= this.detectionRadius && num2 < num)
					{
						num = num2;
						npcstateMachine = npcstateMachine2;
					}
				}
			}
			return npcstateMachine;
		}

		// Token: 0x06005909 RID: 22793 RVA: 0x0022D106 File Offset: 0x0022B306
		private IEnumerator StealRoutine(NPCStateMachine fsm, GameObject env)
		{
			yield return new WaitForSeconds(Random.Range(this.noticeDelayRange.x, this.noticeDelayRange.y));
			if (fsm == null || env == null || !fsm.IsInNormalState || this.slipController.EnvInstanceFor(this.wornPieceName) != env)
			{
				this._routine = null;
				yield break;
			}
			NPCFSMContext context = fsm.Context;
			AStarNPCController controller = context.Controller;
			NPCSenses senses = context.Senses;
			bool sensesWereEnabled = senses != null && senses.enabled;
			fsm.ForceState(NPCStateType.Frozen, "TowelTheft");
			if (senses != null)
			{
				senses.enabled = false;
			}
			controller.UnlockNavigation(false);
			controller.SetDestination(env.transform.position);
			float deadline = Time.time + this.walkTimeout;
			while (Time.time < deadline && !(env == null) && !(fsm == null) && Vector3.Distance(fsm.transform.position, env.transform.position) > this.grabDistance && !controller.HasReachedDestination())
			{
				yield return null;
			}
			if (fsm != null && env != null && Vector3.Distance(fsm.transform.position, env.transform.position) <= this.grabDistance + 0.7f)
			{
				controller.Stop();
				float turnTime = 0f;
				while (turnTime < 0.35f && env != null)
				{
					context.RotateToward(env.transform.position, 10f);
					turnTime += Time.deltaTime;
					yield return null;
				}
				Animator animator = context.Animator;
				int num = Animator.StringToHash(this.stealAnimationState);
				bool flag = animator != null && animator.HasState(0, num);
				float duration = this.ResolveStealClipLength(animator);
				if (flag)
				{
					animator.CrossFadeInFixedTime(this.stealAnimationState, 0.2f, 0);
				}
				else
				{
					duration = 1f;
				}
				yield return new WaitForSeconds(duration * Mathf.Clamp01(this.grabAtNormalizedTime));
				if (env != null && this.slipController.NotifyStolen(this.wornPieceName))
				{
					UnityEvent onTowelStolen = this.OnTowelStolen;
					if (onTowelStolen != null)
					{
						onTowelStolen.Invoke();
					}
				}
				yield return new WaitForSeconds(duration * (1f - Mathf.Clamp01(this.grabAtNormalizedTime)));
			}
			if (senses != null)
			{
				senses.enabled = sensesWereEnabled;
			}
			if (fsm != null)
			{
				fsm.ForceState(NPCStateType.Idle, "TowelTheft");
			}
			this._routine = null;
			yield break;
		}

		// Token: 0x0600590A RID: 22794 RVA: 0x0022D124 File Offset: 0x0022B324
		private float ResolveStealClipLength(Animator animator)
		{
			if (animator != null && animator.runtimeAnimatorController != null)
			{
				foreach (AnimationClip animationClip in animator.runtimeAnimatorController.animationClips)
				{
					if (animationClip != null && animationClip.name == this.stealAnimationState)
					{
						return animationClip.length;
					}
				}
			}
			return 2.25f;
		}

		// Token: 0x04005997 RID: 22935
		[Header("Refs")]
		[Tooltip("Generic cloth slip controller (the same used by the swimsuit). Hosts the towel SlipPiece.")]
		[SerializeField]
		private ClothSlipController slipController;

		// Token: 0x04005998 RID: 22936
		[Tooltip("Worn-mesh name della piece towel nel ClothSlipController.")]
		[SerializeField]
		private string wornPieceName = "Bath_Towel";

		// Token: 0x04005999 RID: 22937
		[Header("Detection")]
		[Tooltip("NPCs within this distance from the fallen towel can notice it.")]
		[SerializeField]
		private float detectionRadius = 12f;

		// Token: 0x0400599A RID: 22938
		[Tooltip("How often to look for a candidate thief while the towel is on the ground.")]
		[SerializeField]
		private float checkInterval = 2f;

		// Token: 0x0400599B RID: 22939
		[Tooltip("Random delay before the chosen NPC reacts and starts walking to the towel.")]
		[SerializeField]
		private Vector2 noticeDelayRange = new Vector2(1f, 4f);

		// Token: 0x0400599C RID: 22940
		[Header("Steal")]
		[Tooltip("Distance from the towel at which the NPC stops and grabs it.")]
		[SerializeField]
		private float grabDistance = 1.3f;

		// Token: 0x0400599D RID: 22941
		[Tooltip("Animator state crossfaded on the NPC base layer (added by Tools/Towel/Setup Towel System). Skipped if the NPC's controller doesn't have it.")]
		[SerializeField]
		private string stealAnimationState = "PickUpFloor";

		// Token: 0x0400599E RID: 22942
		[Tooltip("Fraction of the steal animation at which the towel disappears.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float grabAtNormalizedTime = 0.55f;

		// Token: 0x0400599F RID: 22943
		[Tooltip("Max seconds the NPC may spend walking to the towel before giving up.")]
		[SerializeField]
		private float walkTimeout = 20f;

		// Token: 0x040059A0 RID: 22944
		[Header("Events")]
		public UnityEvent OnTowelStolen;

		// Token: 0x040059A1 RID: 22945
		private Coroutine _routine;

		// Token: 0x040059A2 RID: 22946
		private float _nextCheck;
	}
}
