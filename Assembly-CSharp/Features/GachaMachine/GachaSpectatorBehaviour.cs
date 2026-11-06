using System;
using System.Collections;
using Features.Detection;
using Features.NPC;
using Features.NPC.StateMachine;
using UnityEngine;

namespace Features.GachaMachine
{
	// Token: 0x020011E0 RID: 4576
	[DisallowMultipleComponent]
	public sealed class GachaSpectatorBehaviour : MonoBehaviour
	{
		// Token: 0x17001690 RID: 5776
		// (get) Token: 0x06008D1C RID: 36124 RVA: 0x00399EB2 File Offset: 0x003980B2
		public bool IsBusy
		{
			get
			{
				return this._busy;
			}
		}

		// Token: 0x17001691 RID: 5777
		// (get) Token: 0x06008D1D RID: 36125 RVA: 0x00399EBA File Offset: 0x003980BA
		// (set) Token: 0x06008D1E RID: 36126 RVA: 0x00399EC2 File Offset: 0x003980C2
		public int SlotIndex { get; private set; } = -1;

		// Token: 0x06008D1F RID: 36127 RVA: 0x00399ECC File Offset: 0x003980CC
		public bool Begin(GachaCrowdAttractor attractor, GachaSpectatorBehaviour.Settings settings)
		{
			if (this._busy || attractor == null)
			{
				return false;
			}
			this._fsm = base.GetComponent<NPCStateMachine>();
			this._wander = base.GetComponent<NPCWanderController>();
			this._controller = base.GetComponent<AStarNPCController>();
			this._senses = base.GetComponent<NPCSenses>();
			this._lookAt = base.GetComponent<NPCSimpleLookAt>() ?? base.GetComponentInChildren<NPCSimpleLookAt>();
			this._animator = base.GetComponentInChildren<Animator>();
			if (this._fsm == null || !this._fsm.IsInitialized || (this._fsm.CurrentStateType != NPCStateType.Idle && this._fsm.CurrentStateType != NPCStateType.Wandering) || this._wander == null || !this._wander.isActiveAndEnabled || this._wander.ExternalControl || this._controller == null || !this._controller.isActiveAndEnabled || this._animator == null)
			{
				return false;
			}
			this._attractor = attractor;
			this._settings = settings;
			this.SlotIndex = settings.SlotIndex;
			this._busy = true;
			this._finishing = false;
			this._hasExpressionParam = GachaSpectatorBehaviour.HasIntParameter(this._animator, GachaSpectatorBehaviour.FaceExpressionHash);
			this._wander.ExternalControl = true;
			this._wander.ForceStop();
			this._routine = base.StartCoroutine(this.RunSequence());
			return true;
		}

		// Token: 0x06008D20 RID: 36128 RVA: 0x0039A032 File Offset: 0x00398232
		public void Release()
		{
			this.Finish();
		}

		// Token: 0x06008D21 RID: 36129 RVA: 0x0039A03A File Offset: 0x0039823A
		private IEnumerator RunSequence()
		{
			float navigationReadyDeadline = Time.time + 2f;
			while (!this._controller.IsNavigationReady && Time.time < navigationReadyDeadline)
			{
				if (!this.CanContinue())
				{
					this.Finish();
					yield break;
				}
				yield return null;
			}
			if (!this.CanContinue())
			{
				this.Finish();
				yield break;
			}
			this._controller.UnlockNavigation(false);
			this._controller.SetDestination(this._settings.SlotPosition);
			float deadline = Time.time + this._settings.ApproachTimeout;
			while (Time.time < deadline)
			{
				if (!this.CanContinue())
				{
					this.Finish();
					yield break;
				}
				Vector3 vector = this._settings.SlotPosition - base.transform.position;
				vector.y = 0f;
				if (vector.magnitude <= 0.7f || this._controller.HasReachedDestination())
				{
					break;
				}
				yield return null;
			}
			if (!this.CanContinue())
			{
				this.Finish();
				yield break;
			}
			Vector3 vector2 = this._settings.SlotPosition - base.transform.position;
			vector2.y = 0f;
			if (vector2.magnitude > 2.5f && this._settings.DebugLogs)
			{
				Debug.Log("[GachaCrowd] " + base.name + " could not reach its slot; watching from afar.");
			}
			this._controller.StopAndLock();
			yield return this.FaceTarget(0.5f);
			if (this._lookAt != null && this._settings.LookTarget != null)
			{
				this._lookAt.StartLookingAt(this._settings.LookTarget);
			}
			this.ApplyExpression(this._settings.WatchingExpressionIndex);
			while (this.CanContinue())
			{
				yield return null;
			}
			this.Finish();
			yield break;
		}

		// Token: 0x06008D22 RID: 36130 RVA: 0x0039A049 File Offset: 0x00398249
		private IEnumerator FaceTarget(float duration)
		{
			Transform lookTarget = this._settings.LookTarget;
			if (lookTarget == null)
			{
				yield break;
			}
			Vector3 vector = lookTarget.position - base.transform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.01f)
			{
				yield break;
			}
			Quaternion desired = Quaternion.LookRotation(vector.normalized, Vector3.up);
			Quaternion initial = base.transform.rotation;
			float elapsed = 0f;
			while (elapsed < duration)
			{
				if (!this.CanContinue())
				{
					yield break;
				}
				elapsed += Time.deltaTime;
				base.transform.rotation = Quaternion.Slerp(initial, desired, elapsed / duration);
				yield return null;
			}
			yield break;
		}

		// Token: 0x06008D23 RID: 36131 RVA: 0x0039A060 File Offset: 0x00398260
		private bool CanContinue()
		{
			return !(this._fsm == null) && this._fsm.IsInitialized && (this._fsm.CurrentStateType == NPCStateType.Idle || this._fsm.CurrentStateType == NPCStateType.Wandering || this._fsm.CurrentStateType == NPCStateType.LookingAt) && (!(this._senses != null) || (!this._senses.IsAlert && !this._senses.IsDetected)) && this._controller != null && this._controller.isActiveAndEnabled;
		}

		// Token: 0x06008D24 RID: 36132 RVA: 0x0039A100 File Offset: 0x00398300
		private void Finish()
		{
			if (!this._busy || this._finishing)
			{
				return;
			}
			this._finishing = true;
			if (this._routine != null)
			{
				base.StopCoroutine(this._routine);
				this._routine = null;
			}
			if (this._lookAt != null)
			{
				this._lookAt.StopLooking();
			}
			this.ApplyExpression(0);
			if (this._wander != null)
			{
				this._wander.ExternalControl = false;
			}
			bool flag = this._fsm == null || !this._fsm.IsInitialized || this._fsm.CurrentStateType == NPCStateType.Idle || this._fsm.CurrentStateType == NPCStateType.Wandering || this._fsm.CurrentStateType == NPCStateType.LookingAt;
			if (flag && this._controller != null)
			{
				this._controller.UnlockNavigation(false);
				this._controller.ResetToDefault();
			}
			if (flag && this._wander != null)
			{
				this._wander.ResumeWandering();
			}
			GachaCrowdAttractor attractor = this._attractor;
			int slotIndex = this.SlotIndex;
			this._attractor = null;
			this.SlotIndex = -1;
			this._busy = false;
			this._finishing = false;
			if (attractor != null)
			{
				attractor.FreeSlot(slotIndex);
			}
		}

		// Token: 0x06008D25 RID: 36133 RVA: 0x0039A242 File Offset: 0x00398442
		private void ApplyExpression(int value)
		{
			if (!this._hasExpressionParam || value < 0 || this._animator == null)
			{
				return;
			}
			this._animator.SetInteger(GachaSpectatorBehaviour.FaceExpressionHash, value);
		}

		// Token: 0x06008D26 RID: 36134 RVA: 0x0039A270 File Offset: 0x00398470
		private static bool HasIntParameter(Animator animator, int hash)
		{
			foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
			{
				if (animatorControllerParameter.type == AnimatorControllerParameterType.Int && animatorControllerParameter.nameHash == hash)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008D27 RID: 36135 RVA: 0x0039A2AB File Offset: 0x003984AB
		private void OnDisable()
		{
			if (this._busy)
			{
				this.Finish();
			}
		}

		// Token: 0x0400900E RID: 36878
		private static readonly int FaceExpressionHash = Animator.StringToHash("FaceExpressionIndex");

		// Token: 0x0400900F RID: 36879
		private GachaCrowdAttractor _attractor;

		// Token: 0x04009010 RID: 36880
		private GachaSpectatorBehaviour.Settings _settings;

		// Token: 0x04009011 RID: 36881
		private NPCStateMachine _fsm;

		// Token: 0x04009012 RID: 36882
		private NPCWanderController _wander;

		// Token: 0x04009013 RID: 36883
		private AStarNPCController _controller;

		// Token: 0x04009014 RID: 36884
		private NPCSenses _senses;

		// Token: 0x04009015 RID: 36885
		private NPCSimpleLookAt _lookAt;

		// Token: 0x04009016 RID: 36886
		private Animator _animator;

		// Token: 0x04009017 RID: 36887
		private Coroutine _routine;

		// Token: 0x04009018 RID: 36888
		private bool _busy;

		// Token: 0x04009019 RID: 36889
		private bool _finishing;

		// Token: 0x0400901A RID: 36890
		private bool _hasExpressionParam;

		// Token: 0x020011E1 RID: 4577
		public struct Settings
		{
			// Token: 0x0400901C RID: 36892
			public Vector3 SlotPosition;

			// Token: 0x0400901D RID: 36893
			public int SlotIndex;

			// Token: 0x0400901E RID: 36894
			public Transform LookTarget;

			// Token: 0x0400901F RID: 36895
			public float ApproachTimeout;

			// Token: 0x04009020 RID: 36896
			public int WatchingExpressionIndex;

			// Token: 0x04009021 RID: 36897
			public bool DebugLogs;
		}
	}
}
