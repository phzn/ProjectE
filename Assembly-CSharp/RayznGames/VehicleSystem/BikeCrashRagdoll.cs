using System;
using System.Collections;
using Features.Detection;
using Features.GameOver;
using Features.NPC;
using Features.NPC.Audio;
using Features.Police;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x02000771 RID: 1905
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Rigidbody))]
	public sealed class BikeCrashRagdoll : MonoBehaviour
	{
		// Token: 0x0600319B RID: 12699 RVA: 0x00117454 File Offset: 0x00115654
		private void Awake()
		{
			this.vehicleBody = base.GetComponent<Rigidbody>();
			this.vehicleController = base.GetComponent<vVehicleController>();
			this.skateboardController = base.GetComponent<SkateboardInteractable>();
			this.rollerSkatesController = base.GetComponent<SimpleRollerSkatesInteractable>();
			this.vehicleColliders = base.GetComponentsInChildren<Collider>(false);
			if (this.vehicleController == null && this.skateboardController == null && this.rollerSkatesController == null)
			{
				Debug.LogError("[BikeCrashRagdoll] No supported rider controller found. Expected vVehicleController, SkateboardInteractable or SimpleRollerSkatesInteractable.", this);
			}
		}

		// Token: 0x0600319C RID: 12700 RVA: 0x001174D3 File Offset: 0x001156D3
		private void FixedUpdate()
		{
			if (this.vehicleBody == null)
			{
				return;
			}
			this.ApplyQueuedStepAssist();
			this.velocityBeforePhysics = this.vehicleBody.velocity;
			this.angularVelocityBeforePhysics = this.vehicleBody.angularVelocity;
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x0011750C File Offset: 0x0011570C
		private void OnCollisionStay(Collision collision)
		{
			vThirdPersonController vThirdPersonController;
			if (!this.TryGetMountedRider(out vThirdPersonController))
			{
				return;
			}
			this.TryQueueLowObstacleAssist(collision);
		}

		// Token: 0x0600319E RID: 12702 RVA: 0x0011752C File Offset: 0x0011572C
		private void OnCollisionEnter(Collision collision)
		{
			if (BikeCrashRagdoll.IsCrashFlowProtected || Time.time < this.lastCrashTime + this.crashCooldown || collision == null || collision.contactCount == 0)
			{
				return;
			}
			GameObject gameObject = collision.gameObject;
			if (gameObject == null || (this.crashLayers.value & (1 << gameObject.layer)) == 0)
			{
				return;
			}
			vThirdPersonController vThirdPersonController;
			if (!this.TryGetMountedRider(out vThirdPersonController))
			{
				return;
			}
			if (vThirdPersonController == null || gameObject.transform.IsChildOf(vThirdPersonController.transform))
			{
				return;
			}
			if (this.TryQueueLowObstacleAssist(collision))
			{
				return;
			}
			float magnitude = this.velocityBeforePhysics.magnitude;
			if (magnitude < this.minimumCrashSpeed)
			{
				return;
			}
			ContactPoint contactPoint = collision.GetContact(0);
			float num = 0f;
			bool flag = false;
			for (int i = 0; i < collision.contactCount; i++)
			{
				ContactPoint contact = collision.GetContact(i);
				if (contact.normal.y <= this.maximumObstacleNormalY)
				{
					float num2 = Mathf.Max(0f, -Vector3.Dot(this.velocityBeforePhysics, contact.normal));
					float num3 = Mathf.Abs(Vector3.Dot(collision.relativeVelocity, contact.normal));
					float num4 = Mathf.Max(num2, num3);
					if (!flag || num4 > num)
					{
						contactPoint = contact;
						num = num4;
						flag = true;
					}
				}
			}
			if (!flag || num < this.minimumClosingSpeed)
			{
				return;
			}
			this.TriggerCrash(vThirdPersonController, contactPoint, magnitude, num, collision.collider);
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x00117690 File Offset: 0x00115890
		private void TriggerCrash(vThirdPersonController rider, ContactPoint contact, float vehicleSpeed, float closingSpeed, Collider hitCollider)
		{
			if (BikeCrashRagdoll.IsCrashFlowProtected || rider == null)
			{
				return;
			}
			CielRagdollBootstrap component = rider.GetComponent<CielRagdollBootstrap>();
			vRagdoll vRagdoll = ((component != null) ? component.Ragdoll : rider.GetComponent<vRagdoll>());
			if (vRagdoll == null || (component != null && !component.IsReady))
			{
				BicycleRideDiagnostics.Record(this, "crash.rejected.ragdoll_not_ready", rider, "hasRagdoll=" + (vRagdoll != null).ToString() + "; hasBootstrap=" + (component != null).ToString());
				Debug.LogError("[BikeCrashRagdoll] Ciel's ragdoll is not ready. Add CielRagdollBootstrap to the player prefab.", rider);
				return;
			}
			this.lastCrashTime = Time.time;
			BicycleRideDiagnostics.Record(this, "crash.trigger", rider, string.Concat(new string[]
			{
				"other=",
				BicycleRideDiagnostics.Path((contact.otherCollider != null) ? contact.otherCollider.transform : null),
				"; speed=",
				vehicleSpeed.ToString("F3"),
				"; closingSpeed=",
				closingSpeed.ToString("F3"),
				"; velocityBeforePhysics=",
				this.velocityBeforePhysics.ToString("F3"),
				"; point=",
				contact.point.ToString("F3"),
				"; normal=",
				contact.normal.ToString("F3")
			}));
			Vector3 vector = Vector3.ProjectOnPlane(this.velocityBeforePhysics, Vector3.up);
			if (vector.sqrMagnitude < 0.01f)
			{
				vector = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up);
			}
			vector.Normalize();
			Vector3 vector2 = rider.transform.position + vector * this.forwardSpawnOffset + Vector3.up * this.upwardSpawnOffset;
			Vector3 vector3 = this.velocityBeforePhysics * this.forwardVelocityCarry + Vector3.up * this.upwardVelocity;
			vector3 = Vector3.ClampMagnitude(vector3, this.maximumLaunchSpeed);
			this.IgnoreRiderVehicleCollisions(rider);
			this.DismountForCrash();
			vThirdPersonController vThirdPersonController;
			BicycleRideDiagnostics.Record(this, "crash.dismount_returned", rider, string.Concat(new string[]
			{
				"stillMounted=",
				this.TryGetMountedRider(out vThirdPersonController).ToString(),
				"; ejectionOrigin=",
				vector2.ToString("F3"),
				"; launchVelocity=",
				vector3.ToString("F3")
			}));
			rider.transform.position = vector2;
			rider.transform.rotation = Quaternion.LookRotation(vector, Vector3.up);
			Rigidbody component2 = rider.GetComponent<Rigidbody>();
			if (component2 != null)
			{
				component2.isKinematic = false;
				component2.velocity = vector3;
				component2.angularVelocity = Vector3.zero;
			}
			CielRagdollRecovery component3 = rider.GetComponent<CielRagdollRecovery>();
			BicycleRideDiagnostics.Record(this, "crash.ragdoll_activation.begin", rider, "hasRecovery=" + (component3 != null).ToString() + "; minimumRagdollTime=" + this.minimumRagdollTime.ToString());
			if (component3 != null)
			{
				component3.BeginCrashRecovery();
				vRagdoll.ActivateRagdoll(null);
			}
			else
			{
				vRagdoll.ActivateRagdoll(null, this.minimumRagdollTime);
			}
			this.TryRagdollHitNpc(hitCollider, contact.point);
			this.AddForwardRotation(rider, vector);
			this.ReleaseCrashedVehicle(contact, vector);
			BicycleRideDiagnostics.Record(this, "crash.complete", rider, "physicalAftermath=" + this.UsesPhysicalCrashAftermath().ToString() + "; ragdolled=" + rider.ragdolled.ToString());
			if (this.logCrashes)
			{
				Debug.Log(string.Format("[BikeCrashRagdoll] {0} crashed at {1:F1} m/s, ", base.name, vehicleSpeed) + string.Format("closing speed {0:F1} m/s. Launch velocity: {1}.", closingSpeed, vector3), this);
			}
		}

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x060031A0 RID: 12704 RVA: 0x00117A70 File Offset: 0x00115C70
		private static bool IsCrashFlowProtected
		{
			get
			{
				return DetectionTransitionGate.IsBlocked || (ExhibitionEventDirector.Instance != null && ExhibitionEventDirector.Instance.IsEventActive) || (PerpWalkController.Instance != null && PerpWalkController.Instance.IsActive) || (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver);
			}
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x00117AD4 File Offset: 0x00115CD4
		private bool TryRagdollHitNpc(Collider hitCollider, Vector3 impactPoint)
		{
			if (BikeCrashRagdoll.IsCrashFlowProtected)
			{
				return false;
			}
			GameObject gameObject = BikeCrashRagdoll.FindHitNpc(hitCollider);
			if (gameObject == null || !gameObject.activeInHierarchy)
			{
				return false;
			}
			NPCRagdollRecovery npcragdollRecovery = gameObject.GetComponent<NPCRagdollRecovery>();
			if (npcragdollRecovery == null)
			{
				npcragdollRecovery = gameObject.AddComponent<NPCRagdollRecovery>();
			}
			if (npcragdollRecovery.IsRecovering)
			{
				return false;
			}
			Vector3 vector = Vector3.ProjectOnPlane(this.velocityBeforePhysics, Vector3.up);
			Vector3 vector2 = ((vector.sqrMagnitude > 0.01f) ? vector.normalized : base.transform.forward);
			Vector3 vector3 = Vector3.ClampMagnitude(vector * this.npcVelocityCarry + Vector3.up * this.npcUpwardVelocity, this.maximumNpcLaunchSpeed);
			if (!npcragdollRecovery.ActivateRagdoll(vector3, impactPoint, Vector3.Cross(Vector3.up, vector2) * 0.75f))
			{
				return false;
			}
			NPCReactionAudioPlayer component = gameObject.GetComponent<NPCReactionAudioPlayer>();
			if (component != null)
			{
				component.PlayStumbleAudio();
			}
			return true;
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x00117BBC File Offset: 0x00115DBC
		private static GameObject FindHitNpc(Collider hitCollider)
		{
			if (hitCollider == null)
			{
				return null;
			}
			NPCSenses componentInParent = hitCollider.GetComponentInParent<NPCSenses>();
			if (componentInParent != null)
			{
				return componentInParent.gameObject;
			}
			NPCStumbleHandler componentInParent2 = hitCollider.GetComponentInParent<NPCStumbleHandler>();
			if (!(componentInParent2 != null))
			{
				return null;
			}
			return componentInParent2.gameObject;
		}

		// Token: 0x060031A3 RID: 12707 RVA: 0x00117C04 File Offset: 0x00115E04
		private bool TryGetMountedRider(out vThirdPersonController rider)
		{
			if (this.vehicleController != null)
			{
				rider = this.vehicleController.CurrentRider;
				return this.vehicleController.IsBeingRidden && rider != null;
			}
			if (this.skateboardController != null)
			{
				rider = this.skateboardController.CurrentRider;
				return this.skateboardController.IsBeingWorn && rider != null;
			}
			if (this.rollerSkatesController != null)
			{
				rider = this.rollerSkatesController.CurrentRider;
				return this.rollerSkatesController.IsBeingWorn && rider != null;
			}
			rider = null;
			return false;
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x00117CB0 File Offset: 0x00115EB0
		private void DismountForCrash()
		{
			if (this.vehicleController != null)
			{
				this.vehicleController.ForceDismount();
				return;
			}
			if (this.skateboardController != null)
			{
				this.skateboardController.ForceCrashDismount();
				return;
			}
			SimpleRollerSkatesInteractable simpleRollerSkatesInteractable = this.rollerSkatesController;
			if (simpleRollerSkatesInteractable == null)
			{
				return;
			}
			simpleRollerSkatesInteractable.ForceCrashDismount();
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x00117D04 File Offset: 0x00115F04
		private bool TryQueueLowObstacleAssist(Collision collision)
		{
			if (!this.SupportsStepAssist() || collision == null || collision.contactCount == 0 || this.vehicleBody == null || this.vehicleBody.isKinematic)
			{
				return false;
			}
			Vector3 vector = Vector3.ProjectOnPlane(this.velocityBeforePhysics, Vector3.up);
			if (vector.magnitude < this.minimumAssistSpeed)
			{
				vector = Vector3.ProjectOnPlane(this.vehicleBody.velocity, Vector3.up);
			}
			if (vector.magnitude < this.minimumAssistSpeed)
			{
				return false;
			}
			Vector3 normalized = vector.normalized;
			ContactPoint contactPoint = collision.GetContact(0);
			bool flag = false;
			float num = 0f;
			for (int i = 0; i < collision.contactCount; i++)
			{
				ContactPoint contact = collision.GetContact(i);
				if (contact.normal.y <= this.maximumObstacleNormalY)
				{
					float num2 = -Vector3.Dot(normalized, contact.normal);
					if (num2 > 0.15f && (!flag || num2 > num))
					{
						contactPoint = contact;
						num = num2;
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return false;
			}
			float lowestVehiclePoint = this.GetLowestVehiclePoint();
			float num3;
			if (!this.TryFindTraversableStep(contactPoint.point, normalized, lowestVehiclePoint, collision.collider, out num3))
			{
				return false;
			}
			this.queuedStepRise = Mathf.Max(this.queuedStepRise, num3 + 0.02f);
			this.queuedStepDirection = normalized;
			return true;
		}

		// Token: 0x060031A6 RID: 12710 RVA: 0x00117E4C File Offset: 0x0011604C
		private bool TryFindTraversableStep(Vector3 blockingPoint, Vector3 travelDirection, float lowestVehiclePoint, Collider obstacleCollider, out float bestRise)
		{
			bestRise = float.PositiveInfinity;
			if (obstacleCollider == null)
			{
				return false;
			}
			Vector3 vector = blockingPoint + travelDirection * this.stepProbeForwardDistance;
			vector.y = lowestVehiclePoint + this.maximumStepHeight + 0.12f;
			int num = Physics.RaycastNonAlloc(vector, Vector3.down, this.stepProbeHits, this.maximumStepHeight + 0.24f, -1, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = this.stepProbeHits[i];
				Transform transform = raycastHit.transform;
				if (!(transform == null) && !(obstacleCollider == null) && !(raycastHit.collider != obstacleCollider) && !(transform == base.transform) && !transform.IsChildOf(base.transform) && raycastHit.normal.y >= this.minimumStepTopNormalY)
				{
					float num2 = raycastHit.point.y - lowestVehiclePoint;
					if (num2 >= this.minimumStepHeight && num2 <= this.maximumStepHeight && num2 < bestRise)
					{
						bestRise = num2;
					}
				}
			}
			return !float.IsPositiveInfinity(bestRise);
		}

		// Token: 0x060031A7 RID: 12711 RVA: 0x00117F70 File Offset: 0x00116170
		private void ApplyQueuedStepAssist()
		{
			vThirdPersonController vThirdPersonController;
			if (this.queuedStepRise <= 0f || !this.SupportsStepAssist() || !this.TryGetMountedRider(out vThirdPersonController) || this.vehicleBody.isKinematic)
			{
				this.queuedStepRise = 0f;
				return;
			}
			float num = Mathf.Min(this.queuedStepRise, this.stepLiftSpeed * Time.fixedDeltaTime);
			float num2 = this.stepForwardSpeed * Time.fixedDeltaTime;
			this.vehicleBody.MovePosition(this.vehicleBody.position + Vector3.up * num + this.queuedStepDirection * num2);
			if (this.vehicleBody.velocity.y < 0f)
			{
				Vector3 velocity = this.vehicleBody.velocity;
				velocity.y = 0f;
				this.vehicleBody.velocity = velocity;
			}
			this.queuedStepRise = Mathf.Max(0f, this.queuedStepRise - num);
		}

		// Token: 0x060031A8 RID: 12712 RVA: 0x00118064 File Offset: 0x00116264
		private bool SupportsStepAssist()
		{
			return this.assistLowObstacles;
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x0011806C File Offset: 0x0011626C
		private float GetLowestVehiclePoint()
		{
			if (this.vehicleColliders == null)
			{
				this.vehicleColliders = base.GetComponentsInChildren<Collider>(false);
			}
			float num = float.PositiveInfinity;
			for (int i = 0; i < this.vehicleColliders.Length; i++)
			{
				Collider collider = this.vehicleColliders[i];
				if (collider != null && collider.enabled && !collider.isTrigger)
				{
					num = Mathf.Min(num, collider.bounds.min.y);
				}
			}
			if (!float.IsPositiveInfinity(num))
			{
				return num;
			}
			return this.vehicleBody.position.y;
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x00118100 File Offset: 0x00116300
		private void AddForwardRotation(vThirdPersonController rider, Vector3 travelDirection)
		{
			Animator component = rider.GetComponent<Animator>();
			if (component == null || !component.isHuman)
			{
				return;
			}
			Transform boneTransform = component.GetBoneTransform(HumanBodyBones.Chest);
			Rigidbody rigidbody = ((boneTransform != null) ? boneTransform.GetComponent<Rigidbody>() : null);
			if (rigidbody == null || rigidbody.isKinematic)
			{
				return;
			}
			Vector3 normalized = Vector3.Cross(Vector3.up, travelDirection).normalized;
			rigidbody.AddTorque(normalized * this.forwardBodySpin, ForceMode.VelocityChange);
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x0011817C File Offset: 0x0011637C
		private void ReleaseCrashedVehicle(ContactPoint contact, Vector3 travelDirection)
		{
			if (this.vehicleBody == null)
			{
				return;
			}
			if (!this.UsesPhysicalCrashAftermath())
			{
				this.ParkCrashedVehicle(travelDirection);
				return;
			}
			vVehicleController vVehicleController = this.vehicleController;
			if (vVehicleController != null)
			{
				IVehicle vehicle = vVehicleController.Vehicle;
				if (vehicle != null)
				{
					vehicle.ConstrainRotation(false);
				}
			}
			this.vehicleBody.isKinematic = false;
			this.vehicleBody.constraints = RigidbodyConstraints.None;
			this.vehicleBody.velocity = this.velocityBeforePhysics * this.retainedBikeMomentum;
			this.vehicleBody.angularVelocity = this.angularVelocityBeforePhysics;
			float num = Mathf.Sign(Vector3.Dot(contact.point - base.transform.position, base.transform.right));
			if (Mathf.Approximately(num, 0f))
			{
				num = 1f;
			}
			Vector3 normalized = Vector3.ProjectOnPlane(travelDirection, Vector3.up).normalized;
			this.vehicleBody.AddTorque(normalized * num * this.bikeTumble, ForceMode.VelocityChange);
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x0011827C File Offset: 0x0011647C
		private void ParkCrashedVehicle(Vector3 travelDirection)
		{
			Vector3 vector = Vector3.ProjectOnPlane(travelDirection, Vector3.up);
			if (vector.sqrMagnitude < 0.01f)
			{
				vector = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up);
			}
			vector.Normalize();
			Vector3 vector2 = this.vehicleBody.position - vector * this.parkedVehicleRetreat;
			float y = base.transform.eulerAngles.y;
			this.vehicleBody.position = vector2;
			this.vehicleBody.rotation = Quaternion.Euler(0f, y, 0f);
			this.SnapParkedVehicleToGround();
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x0011831C File Offset: 0x0011651C
		private void SnapParkedVehicleToGround()
		{
			Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>(false);
			float num = float.PositiveInfinity;
			foreach (Collider collider in componentsInChildren)
			{
				if (collider != null && !collider.isTrigger)
				{
					num = Mathf.Min(num, collider.bounds.min.y);
				}
			}
			if (float.IsPositiveInfinity(num))
			{
				return;
			}
			RaycastHit[] array = Physics.RaycastAll(this.vehicleBody.position + Vector3.up * 2f, Vector3.down, 5f, -1, QueryTriggerInteraction.Ignore);
			float num2 = float.NegativeInfinity;
			for (int j = 0; j < array.Length; j++)
			{
				Transform transform = array[j].transform;
				if (!(transform == null) && !(transform == base.transform) && !transform.IsChildOf(base.transform) && array[j].normal.y >= 0.45f)
				{
					num2 = Mathf.Max(num2, array[j].point.y);
				}
			}
			if (float.IsNegativeInfinity(num2))
			{
				return;
			}
			Vector3 position = this.vehicleBody.position;
			position.y += num2 - num + 0.015f;
			this.vehicleBody.position = position;
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x00118470 File Offset: 0x00116670
		private bool UsesPhysicalCrashAftermath()
		{
			return !(this.vehicleController == null) && (this.vehicleController.Type == vVehicleController.VehicleType.Bicycle || this.vehicleController.Type == vVehicleController.VehicleType.KickScooter);
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x001184A0 File Offset: 0x001166A0
		private void IgnoreRiderVehicleCollisions(vThirdPersonController rider)
		{
			Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>(true);
			Collider[] componentsInChildren2 = rider.GetComponentsInChildren<Collider>(true);
			base.StartCoroutine(BikeCrashRagdoll.RestoreRiderVehicleCollisions(componentsInChildren, componentsInChildren2, this.vehicleCollisionIgnoreTime));
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x001184D1 File Offset: 0x001166D1
		private static IEnumerator RestoreRiderVehicleCollisions(Collider[] vehicleColliders, Collider[] riderColliders, float delay)
		{
			bool[,] changedPairs = new bool[vehicleColliders.Length, riderColliders.Length];
			for (int i = 0; i < vehicleColliders.Length; i++)
			{
				Collider collider = vehicleColliders[i];
				if (!(collider == null) && Array.IndexOf<Collider>(riderColliders, collider) < 0)
				{
					for (int j = 0; j < riderColliders.Length; j++)
					{
						Collider collider2 = riderColliders[j];
						if (!(collider2 == null) && !Physics.GetIgnoreCollision(collider, collider2))
						{
							Physics.IgnoreCollision(collider, collider2, true);
							changedPairs[i, j] = true;
						}
					}
				}
			}
			yield return new WaitForSeconds(delay);
			for (int k = 0; k < vehicleColliders.Length; k++)
			{
				Collider collider3 = vehicleColliders[k];
				if (!(collider3 == null))
				{
					for (int l = 0; l < riderColliders.Length; l++)
					{
						Collider collider4 = riderColliders[l];
						if (collider4 != null && changedPairs[k, l])
						{
							Physics.IgnoreCollision(collider3, collider4, false);
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x0400321B RID: 12827
		[Header("Crash detection")]
		[Tooltip("Minimum vehicle speed in metres/second (7.5 m/s is about 27 km/h).")]
		[SerializeField]
		[Min(0f)]
		private float minimumCrashSpeed = 7.5f;

		// Token: 0x0400321C RID: 12828
		[Tooltip("Minimum part of the relative velocity directed into the obstacle.")]
		[SerializeField]
		[Min(0f)]
		private float minimumClosingSpeed = 4.5f;

		// Token: 0x0400321D RID: 12829
		[Tooltip("Normals above this value are considered floor/slope contacts.")]
		[SerializeField]
		[Range(-1f, 1f)]
		private float maximumObstacleNormalY = 0.6f;

		// Token: 0x0400321E RID: 12830
		[SerializeField]
		private LayerMask crashLayers = -1;

		// Token: 0x0400321F RID: 12831
		[SerializeField]
		[Min(0f)]
		private float crashCooldown = 1.5f;

		// Token: 0x04003220 RID: 12832
		[Header("Low obstacle traversal")]
		[Tooltip("Lets wheeled vehicles climb curbs and small steps without treating them as crashes.")]
		[SerializeField]
		private bool assistLowObstacles = true;

		// Token: 0x04003221 RID: 12833
		[SerializeField]
		[Min(0.05f)]
		private float maximumStepHeight = 0.45f;

		// Token: 0x04003222 RID: 12834
		[SerializeField]
		[Min(0f)]
		private float minimumStepHeight = 0.04f;

		// Token: 0x04003223 RID: 12835
		[SerializeField]
		[Min(0.05f)]
		private float stepProbeForwardDistance = 0.32f;

		// Token: 0x04003224 RID: 12836
		[SerializeField]
		[Min(0.1f)]
		private float stepLiftSpeed = 3.5f;

		// Token: 0x04003225 RID: 12837
		[SerializeField]
		[Min(0f)]
		private float stepForwardSpeed = 1.25f;

		// Token: 0x04003226 RID: 12838
		[SerializeField]
		[Min(0f)]
		private float minimumAssistSpeed = 0.5f;

		// Token: 0x04003227 RID: 12839
		[SerializeField]
		[Range(0f, 1f)]
		private float minimumStepTopNormalY = 0.65f;

		// Token: 0x04003228 RID: 12840
		[Header("Rider ejection")]
		[SerializeField]
		[Min(0f)]
		private float forwardVelocityCarry = 1.08f;

		// Token: 0x04003229 RID: 12841
		[SerializeField]
		[Min(0f)]
		private float upwardVelocity = 2.8f;

		// Token: 0x0400322A RID: 12842
		[SerializeField]
		[Min(0f)]
		private float maximumLaunchSpeed = 18f;

		// Token: 0x0400322B RID: 12843
		[SerializeField]
		[Min(0f)]
		private float forwardSpawnOffset = 0.35f;

		// Token: 0x0400322C RID: 12844
		[SerializeField]
		[Min(0f)]
		private float upwardSpawnOffset = 0.15f;

		// Token: 0x0400322D RID: 12845
		[SerializeField]
		[Min(0f)]
		private float forwardBodySpin = 2.4f;

		// Token: 0x0400322E RID: 12846
		[SerializeField]
		[Min(0f)]
		private float minimumRagdollTime = 1.25f;

		// Token: 0x0400322F RID: 12847
		[SerializeField]
		[Min(0f)]
		private float vehicleCollisionIgnoreTime = 3f;

		// Token: 0x04003230 RID: 12848
		[Header("NPC impact")]
		[SerializeField]
		[Range(0f, 1f)]
		private float npcVelocityCarry = 0.72f;

		// Token: 0x04003231 RID: 12849
		[SerializeField]
		[Min(0f)]
		private float npcUpwardVelocity = 0.8f;

		// Token: 0x04003232 RID: 12850
		[SerializeField]
		[Min(0f)]
		private float maximumNpcLaunchSpeed = 12f;

		// Token: 0x04003233 RID: 12851
		[Header("Vehicle aftermath")]
		[SerializeField]
		[Range(0f, 1f)]
		private float retainedBikeMomentum = 0.42f;

		// Token: 0x04003234 RID: 12852
		[SerializeField]
		[Min(0f)]
		private float bikeTumble = 2.1f;

		// Token: 0x04003235 RID: 12853
		[Tooltip("Moves parked crash vehicles away from the obstacle before they can be mounted again.")]
		[SerializeField]
		[Min(0f)]
		private float parkedVehicleRetreat = 0.75f;

		// Token: 0x04003236 RID: 12854
		[Header("Debug")]
		[SerializeField]
		private bool logCrashes = true;

		// Token: 0x04003237 RID: 12855
		private Rigidbody vehicleBody;

		// Token: 0x04003238 RID: 12856
		private vVehicleController vehicleController;

		// Token: 0x04003239 RID: 12857
		private SkateboardInteractable skateboardController;

		// Token: 0x0400323A RID: 12858
		private SimpleRollerSkatesInteractable rollerSkatesController;

		// Token: 0x0400323B RID: 12859
		private Vector3 velocityBeforePhysics;

		// Token: 0x0400323C RID: 12860
		private Vector3 angularVelocityBeforePhysics;

		// Token: 0x0400323D RID: 12861
		private float lastCrashTime = float.NegativeInfinity;

		// Token: 0x0400323E RID: 12862
		private float queuedStepRise;

		// Token: 0x0400323F RID: 12863
		private Vector3 queuedStepDirection;

		// Token: 0x04003240 RID: 12864
		private readonly RaycastHit[] stepProbeHits = new RaycastHit[16];

		// Token: 0x04003241 RID: 12865
		private Collider[] vehicleColliders;
	}
}
