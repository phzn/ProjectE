using System;
using Features.NPC;
using Features.Police;
using Features.Stadium;
using Invector.vCharacterController;
using ProjectExhibitionist;
using ProjectExhibitionist.Core.SceneManagement;
using RayznGames.VehicleSystem;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001275 RID: 4725
	public static class DetectionRecoveryGate
	{
		// Token: 0x17001765 RID: 5989
		// (get) Token: 0x060092E7 RID: 37607 RVA: 0x003C4E43 File Offset: 0x003C3043
		public static bool MustWaitForPlayer
		{
			get
			{
				return DetectionRecoveryGate.IsPlayerRecovering || ShowerInteraction.IsAnyShowerActionActive;
			}
		}

		// Token: 0x17001766 RID: 5990
		// (get) Token: 0x060092E8 RID: 37608 RVA: 0x003C4E54 File Offset: 0x003C3054
		public static bool IsPlayerRecovering
		{
			get
			{
				if (PlayerStunExecutor.Instance != null && PlayerStunExecutor.Instance.IsStunned)
				{
					return true;
				}
				Transform transform = PlayerRegistry.Player;
				if (transform == null && SensoryManager.Instance != null)
				{
					StealthActor player = SensoryManager.Instance.GetPlayer();
					if (player != null)
					{
						transform = player.transform;
					}
				}
				if (transform != null)
				{
					return DetectionRecoveryGate.IsRecovering(transform);
				}
				CielRagdollRecovery cielRagdollRecovery = Object.FindObjectOfType<CielRagdollRecovery>();
				return cielRagdollRecovery != null && DetectionRecoveryGate.IsRecovering(cielRagdollRecovery.transform);
			}
		}

		// Token: 0x060092E9 RID: 37609 RVA: 0x003C4EE0 File Offset: 0x003C30E0
		public static bool IsRecovering(Transform actor)
		{
			if (actor == null)
			{
				return false;
			}
			StadiumParachuteController componentInParent = actor.GetComponentInParent<StadiumParachuteController>(true);
			if (componentInParent != null && (componentInParent.IsFlying || componentInParent.IsLanding))
			{
				return true;
			}
			CielRagdollRecovery componentInParent2 = actor.GetComponentInParent<CielRagdollRecovery>(true);
			if (componentInParent2 != null && componentInParent2.IsRecovering)
			{
				return true;
			}
			NPCRagdollRecovery componentInParent3 = actor.GetComponentInParent<NPCRagdollRecovery>(true);
			if (componentInParent3 != null && componentInParent3.IsRecovering)
			{
				return true;
			}
			NPCPoolRecovery componentInParent4 = actor.GetComponentInParent<NPCPoolRecovery>(true);
			if (componentInParent4 != null && componentInParent4.IsRecovering)
			{
				return true;
			}
			if (NPCPoolRecovery.IsUnavailableForEncounter(actor))
			{
				return true;
			}
			vThirdPersonController componentInParent5 = actor.GetComponentInParent<vThirdPersonController>(true);
			return componentInParent5 != null && componentInParent5.ragdolled;
		}

		// Token: 0x060092EA RID: 37610 RVA: 0x003C4F94 File Offset: 0x003C3194
		public static bool MustWait(EncounterRequest request)
		{
			if (DetectionRecoveryGate.MustWaitForPlayer || (((request != null) ? request.initiator : null) != null && DetectionRecoveryGate.IsRecovering(request.initiator.transform)))
			{
				return true;
			}
			if (((request != null) ? request.additionalActors : null) != null)
			{
				foreach (NPCSenses npcsenses in request.additionalActors)
				{
					if (npcsenses != null && DetectionRecoveryGate.IsRecovering(npcsenses.transform))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}
	}
}
