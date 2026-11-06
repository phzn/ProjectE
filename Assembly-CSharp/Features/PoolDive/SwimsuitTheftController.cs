using System;
using System.Collections;
using Features.Detection;
using Features.NPC.StateMachine;
using Features.Slip;
using UnityEngine;
using UnityEngine.Events;

namespace Features.PoolDive
{
	// Token: 0x02000DE5 RID: 3557
	public class SwimsuitTheftController : MonoBehaviour
	{
		// Token: 0x06006970 RID: 26992 RVA: 0x002A354C File Offset: 0x002A174C
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
			if (this.floatToEdge == null)
			{
				this.floatToEdge = base.GetComponent<SwimsuitFloatToEdge>();
			}
			if (this.floatToEdge == null)
			{
				this.floatToEdge = base.GetComponentInParent<SwimsuitFloatToEdge>();
			}
		}

		// Token: 0x06006971 RID: 26993 RVA: 0x002A35C4 File Offset: 0x002A17C4
		private void Update()
		{
			if (this._routine != null || this.slipController == null)
			{
				return;
			}
			if (!this.slipController.HasSlippedPiece(this.swimsuitPieceName) || this.slipController.WasStolenPiece(this.swimsuitPieceName))
			{
				return;
			}
			if (this.floatToEdge == null || !this.floatToEdge.IsAnchored)
			{
				return;
			}
			GameObject gameObject = this.slipController.EnvInstanceFor(this.swimsuitPieceName);
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

		// Token: 0x06006972 RID: 26994 RVA: 0x002A3698 File Offset: 0x002A1898
		private NPCStateMachine FindCandidate(Vector3 suitPosition)
		{
			NPCStateMachine npcstateMachine = null;
			float num = float.MaxValue;
			foreach (NPCStateMachine npcstateMachine2 in Object.FindObjectsByType<NPCStateMachine>(FindObjectsSortMode.None))
			{
				if (!(npcstateMachine2 == null) && npcstateMachine2.IsInitialized && npcstateMachine2.IsInNormalState && npcstateMachine2.Context != null && !(npcstateMachine2.Context.Controller == null))
				{
					float num2 = Vector3.Distance(npcstateMachine2.transform.position, suitPosition);
					if (num2 <= this.detectionRadius && num2 < num)
					{
						num = num2;
						npcstateMachine = npcstateMachine2;
					}
				}
			}
			return npcstateMachine;
		}

		// Token: 0x06006973 RID: 26995 RVA: 0x002A372A File Offset: 0x002A192A
		private IEnumerator StealRoutine(NPCStateMachine fsm, GameObject env)
		{
			yield return new WaitForSeconds(Random.Range(this.noticeDelayRange.x, this.noticeDelayRange.y));
			if (fsm == null || env == null || !fsm.IsInNormalState || this.slipController.EnvInstanceFor(this.swimsuitPieceName) != env || this.floatToEdge == null || !this.floatToEdge.IsAnchored)
			{
				this._routine = null;
				yield break;
			}
			NPCFSMContext context = fsm.Context;
			AStarNPCController controller = context.Controller;
			NPCSenses senses = context.Senses;
			bool sensesWereEnabled = senses != null && senses.enabled;
			fsm.ForceState(NPCStateType.Frozen, "SwimsuitTheft");
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
				int num = Animator.StringToHash(this.grabAnimationState);
				bool flag = animator != null && animator.HasState(0, num);
				float duration = this.ResolveGrabClipLength(animator);
				if (flag)
				{
					animator.CrossFadeInFixedTime(this.grabAnimationState, 0.2f, 0);
				}
				else
				{
					duration = 1f;
				}
				yield return new WaitForSeconds(duration * Mathf.Clamp01(this.grabAtNormalizedTime));
				if (env != null && this.slipController.NotifyStolen(this.swimsuitPieceName))
				{
					UnityEvent onSwimsuitStolen = this.OnSwimsuitStolen;
					if (onSwimsuitStolen != null)
					{
						onSwimsuitStolen.Invoke();
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
				fsm.ForceState(NPCStateType.Idle, "SwimsuitTheft");
			}
			this._routine = null;
			yield break;
		}

		// Token: 0x06006974 RID: 26996 RVA: 0x002A3748 File Offset: 0x002A1948
		private float ResolveGrabClipLength(Animator animator)
		{
			if (animator != null && animator.runtimeAnimatorController != null)
			{
				foreach (AnimationClip animationClip in animator.runtimeAnimatorController.animationClips)
				{
					if (animationClip != null && animationClip.name == this.grabAnimationState)
					{
						return animationClip.length;
					}
				}
			}
			return 2.25f;
		}

		// Token: 0x04006C35 RID: 27701
		[Header("Refs")]
		[SerializeField]
		private ClothSlipController slipController;

		// Token: 0x04006C36 RID: 27702
		[Tooltip("Worn name of the swimsuit piece inside the generic slip controller.")]
		[SerializeField]
		private string swimsuitPieceName = "School_Swimsuit";

		// Token: 0x04006C37 RID: 27703
		[Tooltip("Gate: NPCs may only grab the suit once this reports IsAnchored (suit at the edge).")]
		[SerializeField]
		private SwimsuitFloatToEdge floatToEdge;

		// Token: 0x04006C38 RID: 27704
		[Header("Detection")]
		[Tooltip("NPCs within this distance from the anchored swimsuit can notice it.")]
		[SerializeField]
		private float detectionRadius = 12f;

		// Token: 0x04006C39 RID: 27705
		[Tooltip("How often to look for a candidate while the suit is anchored at the edge.")]
		[SerializeField]
		private float checkInterval = 2f;

		// Token: 0x04006C3A RID: 27706
		[Tooltip("Random delay before the chosen NPC reacts and starts walking to the suit.")]
		[SerializeField]
		private Vector2 noticeDelayRange = new Vector2(1f, 4f);

		// Token: 0x04006C3B RID: 27707
		[Header("Grab")]
		[Tooltip("Distance from the suit at which the NPC stops and grabs it.")]
		[SerializeField]
		private float grabDistance = 1.3f;

		// Token: 0x04006C3C RID: 27708
		[Tooltip("Animator state crossfaded on the NPC base layer. Skipped if the NPC's controller lacks it.")]
		[SerializeField]
		private string grabAnimationState = "PickUpFloor";

		// Token: 0x04006C3D RID: 27709
		[Tooltip("Fraction of the grab animation at which the suit disappears.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float grabAtNormalizedTime = 0.55f;

		// Token: 0x04006C3E RID: 27710
		[Tooltip("Max seconds the NPC may spend walking to the suit before giving up.")]
		[SerializeField]
		private float walkTimeout = 20f;

		// Token: 0x04006C3F RID: 27711
		[Header("Events")]
		public UnityEvent OnSwimsuitStolen;

		// Token: 0x04006C40 RID: 27712
		private Coroutine _routine;

		// Token: 0x04006C41 RID: 27713
		private float _nextCheck;
	}
}
