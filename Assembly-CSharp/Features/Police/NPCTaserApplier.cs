using System;
using System.Collections;
using Features.Detection;
using Features.NPC;
using Features.NPC.StateMachine;
using Pathfinding;
using UnityEngine;

namespace Features.Police
{
	// Token: 0x02000DE8 RID: 3560
	public static class NPCTaserApplier
	{
		// Token: 0x0600697F RID: 27007 RVA: 0x002A3D2C File Offset: 0x002A1F2C
		public static bool TryTaser(Collider collider)
		{
			if (collider == null || TaserOfficer.MustAbortAttack || NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				return false;
			}
			NPCStateMachine componentInParent = collider.GetComponentInParent<NPCStateMachine>();
			if (componentInParent != null)
			{
				NPCAmbientSuspension component = componentInParent.GetComponent<NPCAmbientSuspension>();
				if (component != null)
				{
					component.SetSuspended(false);
				}
				componentInParent.ForceState(NPCStateType.Tasered, "TaserBolt");
				return true;
			}
			NPCSenses componentInParent2 = collider.GetComponentInParent<NPCSenses>();
			if (componentInParent2 != null)
			{
				NPCTaserApplier.ApplyFallback(componentInParent2);
				return true;
			}
			return false;
		}

		// Token: 0x06006980 RID: 27008 RVA: 0x002A3DA8 File Offset: 0x002A1FA8
		private static void ApplyFallback(NPCSenses senses)
		{
			if (senses.GetComponent<NPCTaserApplier.TaseredFallbackMarker>() != null)
			{
				return;
			}
			NPCAmbientSuspension component = senses.GetComponent<NPCAmbientSuspension>();
			if (component != null)
			{
				component.SetSuspended(false);
			}
			senses.gameObject.AddComponent<NPCTaserApplier.TaseredFallbackMarker>().Begin(senses);
		}

		// Token: 0x06006981 RID: 27009 RVA: 0x002A3DDC File Offset: 0x002A1FDC
		internal static bool HasFallbackStun(NPCSenses senses)
		{
			return senses != null && senses.GetComponent<NPCTaserApplier.TaseredFallbackMarker>() != null;
		}

		// Token: 0x04006C52 RID: 27730
		private static readonly int TaseredTagHash = Animator.StringToHash("Tasered");

		// Token: 0x04006C53 RID: 27731
		private const float SafetyTimeout = 10f;

		// Token: 0x04006C54 RID: 27732
		private const float MinTimeBeforeExitCheck = 0.3f;

		// Token: 0x04006C55 RID: 27733
		private const int VariantCount = 2;

		// Token: 0x02000DE9 RID: 3561
		private class TaseredFallbackMarker : MonoBehaviour
		{
			// Token: 0x06006983 RID: 27011 RVA: 0x002A3E08 File Offset: 0x002A2008
			public void Begin(NPCSenses senses)
			{
				this._senses = senses;
				this._follower = senses.GetComponent<FollowerEntity>();
				this._animator = senses.GetComponentInChildren<Animator>();
				this._sensesWasEnabled = senses.enabled;
				senses.enabled = false;
				if (this._follower != null)
				{
					this._followerWasStopped = this._follower.isStopped;
					this._follower.isStopped = true;
				}
				if (this._animator != null)
				{
					this._reactionsLayer = this._animator.GetLayerIndex("Reactions");
					if (this._reactionsLayer >= 0)
					{
						this._animator.SetLayerWeight(this._reactionsLayer, 1f);
					}
					int num = Random.Range(0, 2);
					this._animator.SetFloat("TaseredVariant", (float)num);
					this._animator.SetTrigger("TaseredTrigger");
				}
				TaserSfx.PlayShockOn(senses.gameObject);
				base.StartCoroutine(this.RunUntilFinished());
			}

			// Token: 0x06006984 RID: 27012 RVA: 0x002A3EF7 File Offset: 0x002A20F7
			private IEnumerator RunUntilFinished()
			{
				float elapsed = 0f;
				bool enteredTaseredOnce = false;
				while (elapsed < 10f)
				{
					yield return null;
					if (!NPCSimulationPause.IsPaused && NPCSimulationPause.SimulationScale > 0.0001f)
					{
						elapsed += NPCSimulationPause.SimulationDeltaTime;
						if (this._animator == null || this._reactionsLayer < 0)
						{
							if (elapsed >= 0.3f)
							{
								break;
							}
						}
						else if (elapsed >= 0.3f)
						{
							AnimatorStateInfo currentAnimatorStateInfo = this._animator.GetCurrentAnimatorStateInfo(this._reactionsLayer);
							if (currentAnimatorStateInfo.tagHash == NPCTaserApplier.TaseredTagHash)
							{
								enteredTaseredOnce = true;
							}
							if (enteredTaseredOnce && currentAnimatorStateInfo.tagHash != NPCTaserApplier.TaseredTagHash && !this._animator.IsInTransition(this._reactionsLayer))
							{
								break;
							}
						}
					}
				}
				this.Restore();
				yield break;
			}

			// Token: 0x06006985 RID: 27013 RVA: 0x002A3F08 File Offset: 0x002A2108
			private void Restore()
			{
				if (this._senses != null)
				{
					this._senses.enabled = this._sensesWasEnabled;
				}
				if (this._follower != null)
				{
					this._follower.isStopped = this._followerWasStopped;
				}
				Object.Destroy(this);
			}

			// Token: 0x04006C56 RID: 27734
			private NPCSenses _senses;

			// Token: 0x04006C57 RID: 27735
			private FollowerEntity _follower;

			// Token: 0x04006C58 RID: 27736
			private Animator _animator;

			// Token: 0x04006C59 RID: 27737
			private int _reactionsLayer = -1;

			// Token: 0x04006C5A RID: 27738
			private bool _sensesWasEnabled;

			// Token: 0x04006C5B RID: 27739
			private bool _followerWasStopped;
		}
	}
}
