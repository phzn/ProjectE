using System;
using System.Collections.Generic;
using Features.Flash;
using Features.HideFlash;
using Features.NPC;
using Features.NPC.Audio;
using Invector.vCharacterController;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x02000773 RID: 1907
	[DefaultExecutionOrder(300)]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(vThirdPersonController), typeof(Rigidbody))]
	public sealed class CielOnFootNpcCrashRagdoll : MonoBehaviour
	{
		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x060031B8 RID: 12728 RVA: 0x001187B4 File Offset: 0x001169B4
		public float CurrentPlanarSpeed
		{
			get
			{
				return Vector3.ProjectOnPlane(this.velocityBeforePhysics, Vector3.up).magnitude;
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x060031B9 RID: 12729 RVA: 0x001187D9 File Offset: 0x001169D9
		public float EffectiveMinimumRunSpeed
		{
			get
			{
				return this.GetEffectiveMinimumRunSpeed();
			}
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x060031BA RID: 12730 RVA: 0x001187E4 File Offset: 0x001169E4
		public bool CanCrashNow
		{
			get
			{
				return this.character != null && this.bootstrap != null && this.bootstrap.IsReady && this.recovery != null && !this.recovery.IsRecovering && !this.character.ragdolled && this.character.isGrounded && !this.character.customAction && !HideFlashController.HasHeldCoverItem && !FlashGateRegistry.AnyFlashEngaged && (this.character.isRunning || this.character.isSprinting) && this.CurrentPlanarSpeed >= this.EffectiveMinimumRunSpeed && (this.rigController == null || !this.rigController.IsVehicleRigActive) && Time.time >= this.lastCrashTime + this.collisionCooldown;
			}
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x001188D8 File Offset: 0x00116AD8
		private void Awake()
		{
			this.character = base.GetComponent<vThirdPersonController>();
			this.bootstrap = base.GetComponent<CielRagdollBootstrap>();
			this.recovery = base.GetComponent<CielRagdollRecovery>();
			this.rigController = base.GetComponent<CharacterRigController>();
			this.rootBody = base.GetComponent<Rigidbody>();
			this.animator = base.GetComponent<Animator>();
		}

		// Token: 0x060031BC RID: 12732 RVA: 0x0011892D File Offset: 0x00116B2D
		private void FixedUpdate()
		{
			if (this.rootBody != null && !this.character.ragdolled)
			{
				this.velocityBeforePhysics = this.rootBody.velocity;
			}
			this.CleanupSeparatedEncounters();
		}

		// Token: 0x060031BD RID: 12733 RVA: 0x00118964 File Offset: 0x00116B64
		private void OnCollisionEnter(Collision collision)
		{
			if (collision == null || collision.collider == null)
			{
				return;
			}
			NPCStumbleHandler componentInParent = collision.collider.GetComponentInParent<NPCStumbleHandler>();
			if (componentInParent != null)
			{
				this.TryTriggerNpcCollision(componentInParent, collision);
			}
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x001189A4 File Offset: 0x00116BA4
		private void OnCollisionStay(Collision collision)
		{
			if (collision == null || collision.collider == null)
			{
				return;
			}
			NPCStumbleHandler componentInParent = collision.collider.GetComponentInParent<NPCStumbleHandler>();
			if (componentInParent != null)
			{
				this.TouchEncounter(componentInParent);
			}
		}

		// Token: 0x060031BF RID: 12735 RVA: 0x001189E0 File Offset: 0x00116BE0
		public bool TryTriggerNpcCollision(NPCStumbleHandler npc, Collision collision)
		{
			if (npc == null || !npc.isActiveAndEnabled)
			{
				return false;
			}
			NPCRagdollRecovery npcragdollRecovery = npc.GetComponent<NPCRagdollRecovery>();
			CielOnFootNpcCrashRagdoll.NpcCollisionEncounter npcCollisionEncounter = this.TouchEncounter(npc);
			if (npcCollisionEncounter.HasDecision)
			{
				return npcCollisionEncounter.UseRagdoll;
			}
			if (npcragdollRecovery != null && npcragdollRecovery.IsRecovering)
			{
				return true;
			}
			if (!this.CanCrashNow)
			{
				return this.recovery != null && this.recovery.IsRecovering;
			}
			if (npcragdollRecovery == null)
			{
				npcragdollRecovery = npc.gameObject.AddComponent<NPCRagdollRecovery>();
			}
			if (!npcragdollRecovery.IsReady && !npcragdollRecovery.ReinitializeAfterRigChange())
			{
				return false;
			}
			Vector3 vector = Vector3.ProjectOnPlane(this.velocityBeforePhysics, Vector3.up);
			float magnitude = vector.magnitude;
			float effectiveMinimumRunSpeed = this.EffectiveMinimumRunSpeed;
			if (magnitude < effectiveMinimumRunSpeed)
			{
				return false;
			}
			bool flag = Random.value < this.ragdollChance;
			CielOnFootNpcCrashRagdoll.SetEncounterDecision(npcCollisionEncounter, flag);
			if (!flag)
			{
				if (this.logCrashes)
				{
					Debug.Log("[CielOnFootNpcCrashRagdoll] Original stumble selected for " + string.Format("'{0}' at {1:F2} m/s.", npc.name, magnitude), this);
				}
				return false;
			}
			Vector3 normalized = vector.normalized;
			Vector3 vector2 = Vector3.ProjectOnPlane(npc.transform.position - base.transform.position, Vector3.up);
			if (vector2.sqrMagnitude < 0.01f)
			{
				vector2 = normalized;
			}
			vector2.Normalize();
			float num = Mathf.Sign(Vector3.Dot(npc.transform.position - base.transform.position, base.transform.right));
			if (Mathf.Approximately(num, 0f))
			{
				num = 1f;
			}
			Vector3 vector3 = vector2 * (magnitude * this.npcMomentum) + base.transform.right * (num * this.npcSideSeparation) + Vector3.up * this.npcUpwardVelocity;
			Vector3 vector4 = Vector3.Cross(Vector3.up, vector2).normalized * this.npcAngularVelocity;
			Vector3 vector5 = ((collision != null && collision.contactCount > 0) ? collision.GetContact(0).point : (Vector3.Lerp(base.transform.position, npc.transform.position, 0.5f) + Vector3.up));
			if (!npcragdollRecovery.ActivateRagdoll(vector3, vector5, vector4))
			{
				CielOnFootNpcCrashRagdoll.SetEncounterDecision(npcCollisionEncounter, false);
				return false;
			}
			NPCReactionAudioPlayer component = npc.GetComponent<NPCReactionAudioPlayer>();
			if (component != null)
			{
				component.PlayStumbleAudio();
			}
			this.lastCrashTime = Time.time;
			Vector3 vector6 = normalized * (magnitude * this.retainedPlayerMomentum) + Vector3.up * this.playerUpwardVelocity;
			this.rootBody.isKinematic = false;
			this.rootBody.velocity = vector6;
			this.rootBody.angularVelocity = Vector3.zero;
			this.recovery.BeginCrashRecovery();
			this.bootstrap.Ragdoll.ActivateRagdoll(null);
			this.AddPlayerForwardRotation(normalized);
			if (this.logCrashes)
			{
				Debug.Log("[CielOnFootNpcCrashRagdoll] Paired crash with '" + npc.name + "' " + string.Format("at {0:F2} m/s (threshold {1:F2}).", magnitude, effectiveMinimumRunSpeed), this);
			}
			return true;
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x00118D18 File Offset: 0x00116F18
		private CielOnFootNpcCrashRagdoll.NpcCollisionEncounter TouchEncounter(NPCStumbleHandler npc)
		{
			int instanceID = npc.GetInstanceID();
			CielOnFootNpcCrashRagdoll.NpcCollisionEncounter npcCollisionEncounter;
			if (!this.npcEncounters.TryGetValue(instanceID, out npcCollisionEncounter))
			{
				npcCollisionEncounter = new CielOnFootNpcCrashRagdoll.NpcCollisionEncounter
				{
					Npc = npc
				};
				this.npcEncounters.Add(instanceID, npcCollisionEncounter);
			}
			npcCollisionEncounter.Npc = npc;
			npcCollisionEncounter.LastContactTime = Time.time;
			return npcCollisionEncounter;
		}

		// Token: 0x060031C1 RID: 12737 RVA: 0x00118D69 File Offset: 0x00116F69
		private static void SetEncounterDecision(CielOnFootNpcCrashRagdoll.NpcCollisionEncounter encounter, bool useRagdoll)
		{
			encounter.HasDecision = true;
			encounter.UseRagdoll = useRagdoll;
		}

		// Token: 0x060031C2 RID: 12738 RVA: 0x00118D7C File Offset: 0x00116F7C
		private void CleanupSeparatedEncounters()
		{
			if (this.npcEncounters.Count == 0)
			{
				return;
			}
			this.encountersToRemove.Clear();
			foreach (KeyValuePair<int, CielOnFootNpcCrashRagdoll.NpcCollisionEncounter> keyValuePair in this.npcEncounters)
			{
				CielOnFootNpcCrashRagdoll.NpcCollisionEncounter value = keyValuePair.Value;
				NPCStumbleHandler npc = value.Npc;
				if (npc == null || !npc.isActiveAndEnabled)
				{
					this.encountersToRemove.Add(keyValuePair.Key);
				}
				else if (Time.time - value.LastContactTime > this.encounterResetDelay && Vector3.ProjectOnPlane(npc.transform.position - base.transform.position, Vector3.up).magnitude >= this.encounterResetDistance)
				{
					this.encountersToRemove.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < this.encountersToRemove.Count; i++)
			{
				this.npcEncounters.Remove(this.encountersToRemove[i]);
			}
		}

		// Token: 0x060031C3 RID: 12739 RVA: 0x00118EAC File Offset: 0x001170AC
		private float GetEffectiveMinimumRunSpeed()
		{
			if (this.character == null)
			{
				return this.minimumRunSpeed;
			}
			vThirdPersonMotor.vMovementSpeed vMovementSpeed = (this.character.isStrafing ? this.character.strafeSpeed : this.character.freeSpeed);
			if (vMovementSpeed == null)
			{
				return this.minimumRunSpeed;
			}
			float num = Mathf.Max(0f, this.character.speedMultiplier);
			float num2 = vMovementSpeed.walkSpeed * num;
			float num3 = vMovementSpeed.runningSpeed * num;
			if (this.character.isSprinting && vMovementSpeed.sprintSpeed > vMovementSpeed.walkSpeed + 0.1f)
			{
				num3 = vMovementSpeed.sprintSpeed * num;
			}
			if (num3 <= 0.01f)
			{
				return this.minimumRunSpeed;
			}
			return Mathf.Min(Mathf.Max(new float[]
			{
				this.minimumRunSpeed,
				num2 + 0.25f,
				num3 * this.configuredRunSpeedFraction
			}), num3 * 0.9f);
		}

		// Token: 0x060031C4 RID: 12740 RVA: 0x00118F94 File Offset: 0x00117194
		private void AddPlayerForwardRotation(Vector3 travelDirection)
		{
			if (this.animator == null || !this.animator.isHuman)
			{
				return;
			}
			Transform boneTransform = this.animator.GetBoneTransform(HumanBodyBones.Chest);
			Rigidbody rigidbody = ((boneTransform != null) ? boneTransform.GetComponent<Rigidbody>() : null);
			if (rigidbody == null || rigidbody.isKinematic)
			{
				return;
			}
			Vector3 normalized = Vector3.Cross(Vector3.up, travelDirection).normalized;
			rigidbody.AddTorque(normalized * this.playerForwardSpin, ForceMode.VelocityChange);
		}

		// Token: 0x04003248 RID: 12872
		[Header("Run collision")]
		[Tooltip("Absolute safety floor. The actual threshold also adapts to the active locomotion settings.")]
		[SerializeField]
		[Min(0f)]
		private float minimumRunSpeed = 1.5f;

		// Token: 0x04003249 RID: 12873
		[Tooltip("Fraction of Ciel's configured run speed required before an impact can trigger ragdoll.")]
		[SerializeField]
		[Range(0.1f, 0.95f)]
		private float configuredRunSpeedFraction = 0.65f;

		// Token: 0x0400324A RID: 12874
		[Tooltip("Chance that an otherwise valid running collision uses the paired ragdoll. A miss keeps the original NPC stumble reaction.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float ragdollChance = 0.5f;

		// Token: 0x0400324B RID: 12875
		[SerializeField]
		[Min(0f)]
		private float collisionCooldown = 1.5f;

		// Token: 0x0400324C RID: 12876
		[Tooltip("A 50/50 result is retained until Ciel has genuinely moved away from that NPC.")]
		[SerializeField]
		[Min(0.1f)]
		private float encounterResetDistance = 2f;

		// Token: 0x0400324D RID: 12877
		[SerializeField]
		[Min(0f)]
		private float encounterResetDelay = 0.25f;

		// Token: 0x0400324E RID: 12878
		[Header("Player fall")]
		[SerializeField]
		[Range(0f, 2f)]
		private float retainedPlayerMomentum = 0.86f;

		// Token: 0x0400324F RID: 12879
		[SerializeField]
		[Min(0f)]
		private float playerUpwardVelocity = 1.15f;

		// Token: 0x04003250 RID: 12880
		[SerializeField]
		[Min(0f)]
		private float playerForwardSpin = 1.65f;

		// Token: 0x04003251 RID: 12881
		[Header("NPC fall")]
		[SerializeField]
		[Range(0f, 2f)]
		private float npcMomentum = 0.72f;

		// Token: 0x04003252 RID: 12882
		[SerializeField]
		[Min(0f)]
		private float npcUpwardVelocity = 0.8f;

		// Token: 0x04003253 RID: 12883
		[SerializeField]
		[Min(0f)]
		private float npcSideSeparation = 0.45f;

		// Token: 0x04003254 RID: 12884
		[SerializeField]
		[Min(0f)]
		private float npcAngularVelocity = 0.75f;

		// Token: 0x04003255 RID: 12885
		[Header("Debug")]
		[SerializeField]
		private bool logCrashes;

		// Token: 0x04003256 RID: 12886
		private vThirdPersonController character;

		// Token: 0x04003257 RID: 12887
		private CielRagdollBootstrap bootstrap;

		// Token: 0x04003258 RID: 12888
		private CielRagdollRecovery recovery;

		// Token: 0x04003259 RID: 12889
		private CharacterRigController rigController;

		// Token: 0x0400325A RID: 12890
		private Rigidbody rootBody;

		// Token: 0x0400325B RID: 12891
		private Animator animator;

		// Token: 0x0400325C RID: 12892
		private Vector3 velocityBeforePhysics;

		// Token: 0x0400325D RID: 12893
		private float lastCrashTime = float.NegativeInfinity;

		// Token: 0x0400325E RID: 12894
		private readonly Dictionary<int, CielOnFootNpcCrashRagdoll.NpcCollisionEncounter> npcEncounters = new Dictionary<int, CielOnFootNpcCrashRagdoll.NpcCollisionEncounter>();

		// Token: 0x0400325F RID: 12895
		private readonly List<int> encountersToRemove = new List<int>();

		// Token: 0x02000774 RID: 1908
		private sealed class NpcCollisionEncounter
		{
			// Token: 0x04003260 RID: 12896
			public NPCStumbleHandler Npc;

			// Token: 0x04003261 RID: 12897
			public float LastContactTime;

			// Token: 0x04003262 RID: 12898
			public bool HasDecision;

			// Token: 0x04003263 RID: 12899
			public bool UseRagdoll;
		}
	}
}
