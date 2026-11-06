using System;
using System.Collections;
using Features.Detection;
using Features.NPC;
using Features.NPC.StateMachine;
using UnityEngine;

namespace Features.MallHack
{
	// Token: 0x02001005 RID: 4101
	[DisallowMultipleComponent]
	public sealed class HackMeNpcObserver : MonoBehaviour
	{
		// Token: 0x17001418 RID: 5144
		// (get) Token: 0x06007C78 RID: 31864 RVA: 0x0032DAC9 File Offset: 0x0032BCC9
		public bool IsBusy
		{
			get
			{
				return this._busy;
			}
		}

		// Token: 0x06007C79 RID: 31865 RVA: 0x0032DAD4 File Offset: 0x0032BCD4
		public bool Begin(HackMeStoreZone store, HackMeObservationTarget target, Vector3 navigationPoint, bool recordVideo, HackMeNpcDirector director, HackMeNpcObserver.SequenceSettings settings)
		{
			if (this._busy || store == null || !store.CanProvokeNpcReactions || !target.IsValid || director == null)
			{
				return false;
			}
			if (!this.CacheAndValidateNpc())
			{
				return false;
			}
			this._store = store;
			this._target = target;
			this._navigationPoint = navigationPoint;
			this._recordVideo = recordVideo;
			this._director = director;
			this._settings = settings;
			this._busy = true;
			this._finishing = false;
			this._wander.ExternalControl = true;
			this._wander.ForceStop();
			this._routine = base.StartCoroutine(this.RunSequence());
			return true;
		}

		// Token: 0x06007C7A RID: 31866 RVA: 0x0032DB80 File Offset: 0x0032BD80
		private bool CacheAndValidateNpc()
		{
			this._fsm = base.GetComponent<NPCStateMachine>();
			this._wander = base.GetComponent<NPCWanderController>();
			this._controller = base.GetComponent<AStarNPCController>();
			this._senses = base.GetComponent<NPCSenses>();
			this._equipment = base.GetComponent<NPCEquipmentManager>();
			this._animator = base.GetComponentInChildren<Animator>();
			return this._fsm != null && this._fsm.IsInitialized && (this._fsm.CurrentStateType == NPCStateType.Idle || this._fsm.CurrentStateType == NPCStateType.Wandering) && this._wander != null && this._wander.isActiveAndEnabled && !this._wander.ExternalControl && this._controller != null && this._controller.isActiveAndEnabled && !this._controller.SuppressPOIBehavior && this._equipment != null && this._animator != null && this._animator.isHuman && this.HasAnimatorParameter(HackMeNpcObserver.ReactionTypeHash, AnimatorControllerParameterType.Int) && this.HasAnimatorParameter(HackMeNpcObserver.ReactionTriggerHash, AnimatorControllerParameterType.Trigger) && (this._senses == null || (!this._senses.IsAlert && !this._senses.IsDetected && !this._senses.IsPassiveReacting));
		}

		// Token: 0x06007C7B RID: 31867 RVA: 0x0032DCFB File Offset: 0x0032BEFB
		private IEnumerator RunSequence()
		{
			float readyElapsed = 0f;
			while (!this._controller.IsNavigationReady && readyElapsed < 2f)
			{
				if (!this.CanContinue())
				{
					this.Finish();
					yield break;
				}
				readyElapsed += HackMeNpcObserver.SimulationDeltaTime();
				yield return null;
			}
			if (!this.CanContinue() || !this._controller.IsNavigationReady)
			{
				this.Finish();
				yield break;
			}
			this._controller.UnlockNavigation(false);
			this._controller.SetDestination(this._navigationPoint);
			yield return null;
			float approachElapsed = 0f;
			while (approachElapsed < this._settings.ApproachTimeout)
			{
				if (!this.CanContinue())
				{
					this.Finish();
					yield break;
				}
				Vector3 vector = this._navigationPoint - base.transform.position;
				vector.y = 0f;
				if (vector.magnitude <= this._settings.ApproachDistance || this._controller.HasReachedDestination())
				{
					break;
				}
				approachElapsed += HackMeNpcObserver.SimulationDeltaTime();
				yield return null;
			}
			if (!this.CanContinue())
			{
				this.Finish();
				yield break;
			}
			Vector3 vector2 = this._navigationPoint - base.transform.position;
			vector2.y = 0f;
			if (vector2.magnitude > this._settings.ApproachDistance * 2.25f)
			{
				this.Log("Approach timed out before reaching the hacked display.");
				this.Finish();
				yield break;
			}
			this._controller.StopAndLock();
			yield return this.RotateTowardDisplay(0.5f);
			if (!this.CanContinue())
			{
				this.Finish();
				yield break;
			}
			yield return this.PlayPhoneReaction();
			if (!this.CanContinue())
			{
				this.Finish();
				yield break;
			}
			float linger = Random.Range(this._settings.LingerAfterCapture.x, this._settings.LingerAfterCapture.y);
			float lingerElapsed = 0f;
			while (lingerElapsed < linger)
			{
				if (!this.CanContinue())
				{
					this.Finish();
					yield break;
				}
				this.FaceDisplay(360f);
				lingerElapsed += HackMeNpcObserver.SimulationDeltaTime();
				yield return null;
			}
			this.Finish();
			yield break;
		}

		// Token: 0x06007C7C RID: 31868 RVA: 0x0032DD0A File Offset: 0x0032BF0A
		private IEnumerator RotateTowardDisplay(float duration)
		{
			float elapsed = 0f;
			while (elapsed < duration)
			{
				if (!this.CanContinue())
				{
					yield break;
				}
				this.FaceDisplay(540f);
				elapsed += HackMeNpcObserver.SimulationDeltaTime();
				yield return null;
			}
			this.FaceDisplay(float.PositiveInfinity);
			yield break;
		}

		// Token: 0x06007C7D RID: 31869 RVA: 0x0032DD20 File Offset: 0x0032BF20
		private IEnumerator PlayPhoneReaction()
		{
			NPCReactionEntry npcreactionEntry = this.FindReactionEntry(this._recordVideo);
			int num = (this._recordVideo ? 2 : 1);
			float num2 = ((npcreactionEntry != null && npcreactionEntry.ReactionDuration > 0f) ? npcreactionEntry.ReactionDuration : (this._recordVideo ? 3.5f : 2.5f));
			float duration = Mathf.Max(this._recordVideo ? 3.25f : 2.1f, num2);
			string text = ((npcreactionEntry != null && !string.IsNullOrEmpty(npcreactionEntry.AnimationTrigger)) ? npcreactionEntry.AnimationTrigger : "ReactionTrigger");
			this._activePhoneProp = this.ResolvePhonePropName(npcreactionEntry);
			if (!string.IsNullOrEmpty(this._activePhoneProp))
			{
				this._equipment.ActivateAndLockProp(this._activePhoneProp);
			}
			this._reactionsLayerIndex = this._animator.GetLayerIndex("Reactions");
			if (this._reactionsLayerIndex >= 0)
			{
				this._previousReactionsLayerWeight = this._animator.GetLayerWeight(this._reactionsLayerIndex);
				this._animator.SetLayerWeight(this._reactionsLayerIndex, 1f);
			}
			this._animator.SetInteger(HackMeNpcObserver.ReactionTypeHash, num);
			int num3 = Animator.StringToHash(text);
			if (!this.HasAnimatorParameter(num3, AnimatorControllerParameterType.Trigger))
			{
				num3 = HackMeNpcObserver.ReactionTriggerHash;
			}
			this._animator.ResetTrigger(num3);
			this._animator.SetTrigger(num3);
			this.Log(string.Concat(new string[]
			{
				"Started ",
				this._recordVideo ? "video" : "photo",
				" reaction at ",
				this._target.IsLargeDisplay ? "TV/monitor" : "phone/tablet",
				"."
			}));
			float elapsed = 0f;
			while (elapsed < duration)
			{
				if (!this.CanContinue())
				{
					yield break;
				}
				this.FaceDisplay(360f);
				elapsed += HackMeNpcObserver.SimulationDeltaTime();
				yield return null;
			}
			this.EndPhoneReaction();
			yield break;
		}

		// Token: 0x06007C7E RID: 31870 RVA: 0x0032DD30 File Offset: 0x0032BF30
		private NPCReactionEntry FindReactionEntry(bool video)
		{
			NPCSenses senses = this._senses;
			bool flag;
			if (senses == null)
			{
				flag = null != null;
			}
			else
			{
				NPCReactionProfileSO reactionProfile = senses.ReactionProfile;
				flag = ((reactionProfile != null) ? reactionProfile.Reactions : null) != null;
			}
			if (!flag)
			{
				return null;
			}
			ReactionType reactionType = (video ? ReactionType.TakeVideo : ReactionType.TakePhoto);
			foreach (NPCReactionEntry npcreactionEntry in this._senses.ReactionProfile.Reactions)
			{
				if (npcreactionEntry != null && npcreactionEntry.Type == reactionType)
				{
					int num = (video ? 2 : 1);
					if (npcreactionEntry.AnimatorReactionIndex == 0 || npcreactionEntry.AnimatorReactionIndex == num)
					{
						return npcreactionEntry;
					}
				}
			}
			return null;
		}

		// Token: 0x06007C7F RID: 31871 RVA: 0x0032DDE0 File Offset: 0x0032BFE0
		private string ResolvePhonePropName(NPCReactionEntry entry)
		{
			if (((entry != null) ? entry.PropsToActivate : null) != null)
			{
				foreach (string text in entry.PropsToActivate)
				{
					if (!string.IsNullOrWhiteSpace(text) && text.IndexOf("phone", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						return text;
					}
				}
			}
			Transform[] componentsInChildren = base.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (string.Equals(componentsInChildren[i].name, "Smartphone", StringComparison.OrdinalIgnoreCase))
				{
					return componentsInChildren[i].name;
				}
			}
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				if (componentsInChildren[j].name.IndexOf("phone", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return componentsInChildren[j].name;
				}
			}
			return "Smartphone";
		}

		// Token: 0x06007C80 RID: 31872 RVA: 0x0032DECC File Offset: 0x0032C0CC
		private bool CanContinue()
		{
			if (!this._busy || !base.isActiveAndEnabled || this._store == null || !this._store.CanProvokeNpcReactions || !this._target.IsValid || this._fsm == null || !this._fsm.IsInitialized || this._controller == null || !this._controller.isActiveAndEnabled || this._wander == null || !this._wander.isActiveAndEnabled)
			{
				return false;
			}
			NPCStateType currentStateType = this._fsm.CurrentStateType;
			return (currentStateType == NPCStateType.Idle || currentStateType == NPCStateType.Wandering || currentStateType == NPCStateType.LookingAt) && (this._senses == null || (!this._senses.IsAlert && !this._senses.IsDetected && !this._senses.IsPassiveReacting));
		}

		// Token: 0x06007C81 RID: 31873 RVA: 0x0032DFC4 File Offset: 0x0032C1C4
		private void FaceDisplay(float degreesPerSecond)
		{
			if (!this._target.IsValid)
			{
				return;
			}
			Vector3 vector = this._target.LookPoint - base.transform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.001f)
			{
				return;
			}
			Quaternion quaternion = Quaternion.LookRotation(vector.normalized, Vector3.up);
			if (float.IsPositiveInfinity(degreesPerSecond))
			{
				base.transform.rotation = quaternion;
				return;
			}
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, degreesPerSecond * HackMeNpcObserver.SimulationDeltaTime());
		}

		// Token: 0x06007C82 RID: 31874 RVA: 0x0032E060 File Offset: 0x0032C260
		private void EndPhoneReaction()
		{
			if (this._animator != null)
			{
				if (this.HasAnimatorParameter(HackMeNpcObserver.ReactionTypeHash, AnimatorControllerParameterType.Int))
				{
					this._animator.SetInteger(HackMeNpcObserver.ReactionTypeHash, 0);
				}
				if (this.HasAnimatorParameter(HackMeNpcObserver.ReactionTriggerHash, AnimatorControllerParameterType.Trigger))
				{
					this._animator.ResetTrigger(HackMeNpcObserver.ReactionTriggerHash);
				}
			}
			if (this._equipment != null && !string.IsNullOrEmpty(this._activePhoneProp))
			{
				this._equipment.DeactivateProp(this._activePhoneProp);
			}
			this._activePhoneProp = null;
		}

		// Token: 0x06007C83 RID: 31875 RVA: 0x0032E0EC File Offset: 0x0032C2EC
		private bool HasAnimatorParameter(int hash, AnimatorControllerParameterType type)
		{
			if (this._animator == null)
			{
				return false;
			}
			foreach (AnimatorControllerParameter animatorControllerParameter in this._animator.parameters)
			{
				if (animatorControllerParameter.nameHash == hash && animatorControllerParameter.type == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007C84 RID: 31876 RVA: 0x0032E13C File Offset: 0x0032C33C
		private static float SimulationDeltaTime()
		{
			if (!NPCSimulationPause.IsPaused)
			{
				return Time.deltaTime;
			}
			return 0f;
		}

		// Token: 0x06007C85 RID: 31877 RVA: 0x0032E150 File Offset: 0x0032C350
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
			this.EndPhoneReaction();
			if (this._animator != null && this._reactionsLayerIndex >= 0)
			{
				this._animator.SetLayerWeight(this._reactionsLayerIndex, this._previousReactionsLayerWeight);
			}
			this._reactionsLayerIndex = -1;
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
			if (flag && this._wander != null && this._wander.isActiveAndEnabled)
			{
				this._wander.ResumeWandering();
			}
			HackMeNpcDirector director = this._director;
			HackMeStoreZone store = this._store;
			this._director = null;
			this._store = null;
			this._target = default(HackMeObservationTarget);
			this._busy = false;
			this._finishing = false;
			if (director == null)
			{
				return;
			}
			director.NotifySequenceFinished(this, store);
		}

		// Token: 0x06007C86 RID: 31878 RVA: 0x0032E2C1 File Offset: 0x0032C4C1
		private void OnDisable()
		{
			if (this._busy)
			{
				this.Finish();
			}
		}

		// Token: 0x06007C87 RID: 31879 RVA: 0x0032E2C1 File Offset: 0x0032C4C1
		private void OnDestroy()
		{
			if (this._busy)
			{
				this.Finish();
			}
		}

		// Token: 0x06007C88 RID: 31880 RVA: 0x0032E2D1 File Offset: 0x0032C4D1
		private void Log(string message)
		{
			if (this._settings.DebugLogs)
			{
				Debug.Log("[HackMeNPC] " + base.name + ": " + message, this);
			}
		}

		// Token: 0x04008046 RID: 32838
		private static readonly int ReactionTypeHash = Animator.StringToHash("ReactionType");

		// Token: 0x04008047 RID: 32839
		private static readonly int ReactionTriggerHash = Animator.StringToHash("ReactionTrigger");

		// Token: 0x04008048 RID: 32840
		private NPCStateMachine _fsm;

		// Token: 0x04008049 RID: 32841
		private NPCWanderController _wander;

		// Token: 0x0400804A RID: 32842
		private AStarNPCController _controller;

		// Token: 0x0400804B RID: 32843
		private NPCSenses _senses;

		// Token: 0x0400804C RID: 32844
		private NPCEquipmentManager _equipment;

		// Token: 0x0400804D RID: 32845
		private Animator _animator;

		// Token: 0x0400804E RID: 32846
		private HackMeStoreZone _store;

		// Token: 0x0400804F RID: 32847
		private HackMeObservationTarget _target;

		// Token: 0x04008050 RID: 32848
		private HackMeNpcDirector _director;

		// Token: 0x04008051 RID: 32849
		private HackMeNpcObserver.SequenceSettings _settings;

		// Token: 0x04008052 RID: 32850
		private Vector3 _navigationPoint;

		// Token: 0x04008053 RID: 32851
		private Coroutine _routine;

		// Token: 0x04008054 RID: 32852
		private string _activePhoneProp;

		// Token: 0x04008055 RID: 32853
		private bool _recordVideo;

		// Token: 0x04008056 RID: 32854
		private bool _busy;

		// Token: 0x04008057 RID: 32855
		private bool _finishing;

		// Token: 0x04008058 RID: 32856
		private int _reactionsLayerIndex = -1;

		// Token: 0x04008059 RID: 32857
		private float _previousReactionsLayerWeight;

		// Token: 0x02001006 RID: 4102
		public struct SequenceSettings
		{
			// Token: 0x0400805A RID: 32858
			public float ApproachDistance;

			// Token: 0x0400805B RID: 32859
			public float ApproachTimeout;

			// Token: 0x0400805C RID: 32860
			public Vector2 LingerAfterCapture;

			// Token: 0x0400805D RID: 32861
			public bool DebugLogs;
		}
	}
}
