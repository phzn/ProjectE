using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ExhibitionistDemo.UI;
using Features.CarHide;
using Features.DebugTools;
using Features.Detection;
using Features.DumpsterHide;
using Features.Flash;
using Features.FlexiPenis;
using Features.GachaMachine;
using Features.GameOver;
using Features.Inventory.Actions;
using Features.Inventory.Placement;
using Features.LockerHide;
using Features.PhoneCompanion;
using Features.PlayerEmotes;
using Features.Police;
using Features.PoolDive;
using Features.RenderingPrecision;
using Features.Stadium;
using Features.TrashcanLoot;
using Features.Vehicles.Traffic;
using Invector.vCamera;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using Invector.vCover;
using MagicaCloth2;
using PoolWaterTools;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Subway;
using RayznGames.VehicleSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using VehicleSpawnPlugin;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A6C RID: 2668
	[DisallowMultipleComponent]
	public sealed class PlayerUnstuckService : MonoBehaviour
	{
		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x06004BEB RID: 19435 RVA: 0x001D7BC5 File Offset: 0x001D5DC5
		public bool RecoveryPending
		{
			get
			{
				return this._request != null;
			}
		}

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x06004BEC RID: 19436 RVA: 0x001D7BD0 File Offset: 0x001D5DD0
		public static bool IsRecoveryPending
		{
			get
			{
				return PlayerUnstuckService._runtime != null && PlayerUnstuckService._runtime.RecoveryPending;
			}
		}

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x06004BED RID: 19437 RVA: 0x001D7BEB File Offset: 0x001D5DEB
		public static bool IsRecoveryAllowed
		{
			get
			{
				return !GameplayInputGate.IsLoading && !PlayerUnstuckService.IsRestrictedRecoveryScene(SceneManager.GetActiveScene()) && (SceneContext.Current == null || !PlayerUnstuckService.IsRestrictedRecoveryScene(SceneContext.Current.gameObject.scene));
			}
		}

		// Token: 0x06004BEE RID: 19438 RVA: 0x001D7C28 File Offset: 0x001D5E28
		private static bool IsRestrictedRecoveryScene(Scene scene)
		{
			return scene.name == "CielHome" || scene.name == "GameOver - Prison";
		}

		// Token: 0x06004BEF RID: 19439 RVA: 0x001D7C50 File Offset: 0x001D5E50
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetRuntime()
		{
			PlayerUnstuckService._runtime = null;
		}

		// Token: 0x06004BF0 RID: 19440 RVA: 0x001D7C58 File Offset: 0x001D5E58
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Install()
		{
			if (PlayerUnstuckService._runtime != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[PlayerUnstuck]");
			Object.DontDestroyOnLoad(gameObject);
			PlayerUnstuckService._runtime = gameObject.AddComponent<PlayerUnstuckService>();
		}

		// Token: 0x06004BF1 RID: 19441 RVA: 0x001D7C84 File Offset: 0x001D5E84
		public static PlayerUnstuckService EnsureForPlayer(Transform player)
		{
			if (player == null)
			{
				return null;
			}
			PlayerUnstuckService playerUnstuckService;
			if (Application.isPlaying)
			{
				PlayerUnstuckService.Install();
				playerUnstuckService = PlayerUnstuckService._runtime;
			}
			else
			{
				playerUnstuckService = player.GetComponent<PlayerUnstuckService>() ?? player.gameObject.AddComponent<PlayerUnstuckService>();
			}
			playerUnstuckService.Bind(player);
			return playerUnstuckService;
		}

		// Token: 0x06004BF2 RID: 19442 RVA: 0x001D7CD0 File Offset: 0x001D5ED0
		public static bool TryRecoverPlayer(out string failureReason)
		{
			Transform player = PlayerRegistry.Player;
			if (player == null)
			{
				failureReason = "The gameplay player is not available.";
				return false;
			}
			return PlayerUnstuckService.EnsureForPlayer(player).TryRecover(out failureReason);
		}

		// Token: 0x06004BF3 RID: 19443 RVA: 0x001D7D04 File Offset: 0x001D5F04
		public static void RequestPlayerRecovery(Action<bool, string> completed = null)
		{
			Transform player = PlayerRegistry.Player;
			if (player == null)
			{
				if (completed != null)
				{
					completed(false, "The gameplay player is not available.");
				}
				return;
			}
			PlayerUnstuckService.EnsureForPlayer(player).RequestRecovery(completed);
		}

		// Token: 0x06004BF4 RID: 19444 RVA: 0x001D7D3C File Offset: 0x001D5F3C
		private void OnEnable()
		{
			WorldOrigin.Shifted += this.ApplyWorldOriginShift;
		}

		// Token: 0x06004BF5 RID: 19445 RVA: 0x001D7D4F File Offset: 0x001D5F4F
		private void OnDisable()
		{
			WorldOrigin.Shifted -= this.ApplyWorldOriginShift;
			this.CancelRecovery();
		}

		// Token: 0x06004BF6 RID: 19446 RVA: 0x001D7D68 File Offset: 0x001D5F68
		private void Update()
		{
			if (this == PlayerUnstuckService._runtime)
			{
				this.Bind(PlayerRegistry.Player);
			}
			if (this._player == null || Time.unscaledTime < this._nextSample)
			{
				return;
			}
			this._nextSample = Time.unscaledTime + 0.5f;
			this.CheckScene();
			if (PlayerUnstuckService.IsRecoveryAllowed)
			{
				this.RememberSafePosition();
			}
		}

		// Token: 0x06004BF7 RID: 19447 RVA: 0x001D7DD0 File Offset: 0x001D5FD0
		private void Bind(Transform player)
		{
			if (this._player == player)
			{
				return;
			}
			this.CancelRecovery();
			this._player = player;
			this._history.Clear();
			this._context = SceneContext.Current;
			this._sceneHandle = PlayerUnstuckService.CurrentSceneHandle();
			this.CaptureEntryPosition();
			this._nextSample = 0f;
		}

		// Token: 0x06004BF8 RID: 19448 RVA: 0x001D7E2C File Offset: 0x001D602C
		private static int CurrentSceneHandle()
		{
			if (!(SceneContext.Current != null))
			{
				return SceneManager.GetActiveScene().handle;
			}
			return SceneContext.Current.gameObject.scene.handle;
		}

		// Token: 0x06004BF9 RID: 19449 RVA: 0x001D7E6C File Offset: 0x001D606C
		private void CheckScene()
		{
			int num = PlayerUnstuckService.CurrentSceneHandle();
			if (this._context == SceneContext.Current && this._sceneHandle == num)
			{
				return;
			}
			this._history.Clear();
			this._context = SceneContext.Current;
			this._sceneHandle = num;
			this.CaptureEntryPosition();
		}

		// Token: 0x06004BFA RID: 19450 RVA: 0x001D7EC0 File Offset: 0x001D60C0
		private void CaptureEntryPosition()
		{
			Quaternion quaternion;
			this._hasEntryPosition = this._context != null && this._context.TryGetPlayerSpawn(out this._entryPosition, out quaternion);
			if (!this._hasEntryPosition && this._player != null)
			{
				this._entryPosition = this._player.position;
				this._hasEntryPosition = true;
			}
		}

		// Token: 0x06004BFB RID: 19451 RVA: 0x001D7F28 File Offset: 0x001D6128
		private void ApplyWorldOriginShift(Vector3 delta)
		{
			if (this._hasEntryPosition)
			{
				this._entryPosition += delta;
			}
			if (this._request != null)
			{
				this._request.OriginalPosition += delta;
				this._request.Focus += delta;
			}
			for (int i = 0; i < this._history.Count; i++)
			{
				PlayerUnstuckService.SafeSample safeSample = this._history[i];
				safeSample.Position += delta;
				this._history[i] = safeSample;
			}
		}

		// Token: 0x06004BFC RID: 19452 RVA: 0x001D7FD0 File Offset: 0x001D61D0
		private void RememberSafePosition()
		{
			if (this._player == null || !this._player.gameObject.activeInHierarchy)
			{
				return;
			}
			Rigidbody component = this._player.GetComponent<Rigidbody>();
			if (component != null && (component.isKinematic || Mathf.Abs(component.velocity.y) > 0.4f))
			{
				return;
			}
			vThirdPersonController component2 = this._player.GetComponent<vThirdPersonController>();
			if (component2 != null && (!component2.isGrounded || component2.customAction || component2.ragdolled))
			{
				return;
			}
			PlayerUnstuckService.StandingCapsule standingCapsule = PlayerUnstuckService.GetStandingCapsule(this._player);
			Vector3 vector;
			Collider collider;
			if (!PlayerUnstuckService.TryGround(this._player, this._player.position, standingCapsule, this._sceneHandle, 0.12f, 0.2f, out vector, out collider))
			{
				return;
			}
			if (this._history.Count > 0 && (this._history[this._history.Count - 1].Position - vector).sqrMagnitude < 1f)
			{
				return;
			}
			if (this._history.Count == 24)
			{
				this._history.RemoveAt(0);
			}
			this._history.Add(new PlayerUnstuckService.SafeSample
			{
				Position = vector,
				Ground = collider,
				SceneHandle = this._sceneHandle
			});
		}

		// Token: 0x06004BFD RID: 19453 RVA: 0x001D812C File Offset: 0x001D632C
		public bool TryGetRecoveryPosition(out Vector3 position)
		{
			position = ((this._player != null) ? this._player.position : Vector3.zero);
			if (this._player == null || !PlayerUnstuckService.IsRecoveryAllowed)
			{
				return false;
			}
			this.CheckScene();
			Physics.SyncTransforms();
			PlayerUnstuckService.StandingCapsule standingCapsule = PlayerUnstuckService.GetStandingCapsule(this._player);
			Vector3 position2 = this._player.position;
			if (PlayerUnstuckService.TryNearby(this._player, position2, standingCapsule, this._sceneHandle, out position))
			{
				return true;
			}
			for (int i = this._history.Count - 1; i >= 0; i--)
			{
				PlayerUnstuckService.SafeSample safeSample = this._history[i];
				if (safeSample.SceneHandle == this._sceneHandle && PlayerUnstuckService.IsStructuralGround(safeSample.Ground, this._player, this._sceneHandle) && PlayerUnstuckService.TryNearby(this._player, safeSample.Position, standingCapsule, this._sceneHandle, out position))
				{
					return true;
				}
			}
			return this.TryLoadedFloors(this._player, position2, standingCapsule, this._sceneHandle, out position) || (this._hasEntryPosition && PlayerUnstuckService.TryNearby(this._player, this._entryPosition, standingCapsule, this._sceneHandle, out position));
		}

		// Token: 0x06004BFE RID: 19454 RVA: 0x001D825C File Offset: 0x001D645C
		public bool TryRecover(out string failureReason)
		{
			failureReason = null;
			if (!PlayerUnstuckService.IsRecoveryAllowed)
			{
				failureReason = "Recovery is unavailable in the current scene.";
				return false;
			}
			if (this._player == null)
			{
				failureReason = "The gameplay player is not available.";
				return false;
			}
			Vector3 position = this._player.position;
			PlayerUnstuckService.ReleaseInteractions(this._player);
			Vector3 vector;
			if (!this.TryGetRecoveryPosition(out vector))
			{
				DistanceTracker component = this._player.GetComponent<DistanceTracker>();
				if (component != null)
				{
					component.ExcludeRecoveryDisplacement(this._player.position - position);
				}
				failureReason = "No loaded, supported standing position was found.";
				return false;
			}
			this.CompleteRecovery(vector, position);
			return true;
		}

		// Token: 0x06004BFF RID: 19455 RVA: 0x001D82F0 File Offset: 0x001D64F0
		public void RequestRecovery(Action<bool, string> completed = null)
		{
			if (!PlayerUnstuckService.IsRecoveryAllowed)
			{
				this.CancelRecovery();
				if (completed != null)
				{
					completed(false, "Recovery is unavailable in the current scene.");
				}
				return;
			}
			if (this._request != null)
			{
				this._recoveryCompleted = (Action<bool, string>)Delegate.Combine(this._recoveryCompleted, completed);
				return;
			}
			string text;
			if (this.TryRecover(out text))
			{
				if (completed != null)
				{
					completed(true, null);
				}
				return;
			}
			if (this._player == null)
			{
				if (completed != null)
				{
					completed(false, text);
				}
				return;
			}
			this._recoveryCompleted = completed;
			this._request = new PlayerUnstuckService.RecoveryRequest
			{
				Player = this._player,
				Context = SceneContext.Current,
				Scene = this._sceneHandle,
				OriginalPosition = this._player.position,
				Manager = PlayerUnstuckService.CurrentWorldManager(),
				Focus = ((this._history.Count > 0) ? this._history[this._history.Count - 1].Position : new Vector3(this._player.position.x, this._hasEntryPosition ? this._entryPosition.y : this._player.position.y, this._player.position.z))
			};
			this._recoveryRoutine = base.StartCoroutine(this.RecoverAtCheckpoint(this._request));
		}

		// Token: 0x06004C00 RID: 19456 RVA: 0x001D8450 File Offset: 0x001D6650
		public void CancelRecovery()
		{
			if (this._request == null)
			{
				return;
			}
			Coroutine recoveryRoutine = this._recoveryRoutine;
			this.FinishRequest(false, "Recovery cancelled because the gameplay context changed.");
			if (recoveryRoutine != null)
			{
				base.StopCoroutine(recoveryRoutine);
			}
		}

		// Token: 0x06004C01 RID: 19457 RVA: 0x001D8483 File Offset: 0x001D6683
		private IEnumerator RecoverAtCheckpoint(PlayerUnstuckService.RecoveryRequest request)
		{
			yield return null;
			int num;
			for (int phase = 0; phase < 2; phase = num + 1)
			{
				if (phase == 1 && this._hasEntryPosition)
				{
					request.Focus = this._entryPosition;
				}
				float deadline = Time.realtimeSinceStartup + ((phase == 0) ? 3f : 9f);
				while (Time.realtimeSinceStartup < deadline)
				{
					if (!this.RequestContextIsValid(request))
					{
						this.FinishRequest(false, "Recovery cancelled because the gameplay context changed.");
						yield break;
					}
					if (request.Manager != null && request.Manager.SetPreparationFocus(this, request.Focus))
					{
						request.Manager.TryWakeRecoveryArea(this, request.Focus);
					}
					Physics.SyncTransforms();
					PlayerUnstuckService.StandingCapsule standingCapsule = PlayerUnstuckService.GetStandingCapsule(request.Player);
					Vector3 vector;
					Collider collider;
					if (PlayerUnstuckService.TryGround(request.Player, request.Player.position, standingCapsule, request.Scene, 2f, 3f, out vector, out collider) || PlayerUnstuckService.TryNearby(request.Player, request.Focus, standingCapsule, request.Scene, out vector))
					{
						this.CompleteRequest(request, vector);
						yield break;
					}
					Rigidbody component = request.Player.GetComponent<Rigidbody>();
					if (component != null && !component.isKinematic && component.velocity.y < 0f)
					{
						Vector3 velocity = component.velocity;
						velocity.y = 0f;
						component.velocity = velocity;
					}
					vThirdPersonController component2 = request.Player.GetComponent<vThirdPersonController>();
					if (component2 != null)
					{
						component2.verticalVelocity = 0f;
						component2.heightReached = request.Player.position.y;
					}
					yield return new WaitForSecondsRealtime(0.2f);
				}
				num = phase;
			}
			Vector3 vector2;
			if (this.RequestContextIsValid(request) && this.TryGetRecoveryPosition(out vector2))
			{
				this.CompleteRequest(request, vector2);
			}
			else
			{
				this.FinishRequest(false, "The recovery checkpoint has no usable loaded collision geometry.");
			}
			yield break;
		}

		// Token: 0x06004C02 RID: 19458 RVA: 0x001D849C File Offset: 0x001D669C
		private bool RequestContextIsValid(PlayerUnstuckService.RecoveryRequest request)
		{
			return this._request == request && request.Player != null && request.Player == this._player && request.Context == SceneContext.Current && request.Scene == PlayerUnstuckService.CurrentSceneHandle() && PlayerUnstuckService.IsRecoveryAllowed && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.PlayerControl | PlayerContextResource.Rider | PlayerContextResource.WaterMovement) && (ExhibitionEventDirector.Instance == null || !ExhibitionEventDirector.Instance.IsEventActive) && (GameOverManager.Instance == null || (!GameOverManager.Instance.IsGameOver && !GameOverManager.Instance.HasPendingCapture)) && (PerpWalkController.Instance == null || !PerpWalkController.Instance.IsActive);
		}

		// Token: 0x06004C03 RID: 19459 RVA: 0x001D856A File Offset: 0x001D676A
		private void CompleteRequest(PlayerUnstuckService.RecoveryRequest request, Vector3 target)
		{
			this.CompleteRecovery(target, request.Player.position);
			this.FinishRequest(true, null);
		}

		// Token: 0x06004C04 RID: 19460 RVA: 0x001D8588 File Offset: 0x001D6788
		private void FinishRequest(bool succeeded, string reason)
		{
			PlayerUnstuckService.RecoveryRequest request = this._request;
			this._request = null;
			this._recoveryRoutine = null;
			if (request != null)
			{
				ChunkManager manager = request.Manager;
				if (manager != null)
				{
					manager.ReleasePreparationFocus(this);
				}
			}
			Action<bool, string> recoveryCompleted = this._recoveryCompleted;
			this._recoveryCompleted = null;
			if (recoveryCompleted == null)
			{
				return;
			}
			recoveryCompleted(succeeded, reason);
		}

		// Token: 0x06004C05 RID: 19461 RVA: 0x001D85DC File Offset: 0x001D67DC
		private void CompleteRecovery(Vector3 destination, Vector3 originalPosition)
		{
			vThirdPersonController component = this._player.GetComponent<vThirdPersonController>();
			Rigidbody component2 = this._player.GetComponent<Rigidbody>();
			CapsuleCollider component3 = this._player.GetComponent<CapsuleCollider>();
			global::CharacterController component4 = this._player.GetComponent<global::CharacterController>();
			bool flag = component4 != null && component4.enabled;
			bool flag2 = component != null && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.PlayerControl | PlayerContextResource.Rider | PlayerContextResource.WaterMovement);
			bool flag3 = component3 != null && (component3.enabled || flag2);
			if (component != null)
			{
				component.isCrouching = false;
				component.customAction = false;
				component.isJumping = false;
				component.verticalVelocity = 0f;
				component.heightReached = destination.y;
				if (component2 != null && component.animator != null)
				{
					component.StopCharacter();
				}
				else
				{
					component.input = Vector3.zero;
					component.moveDirection = Vector3.zero;
				}
			}
			Quaternion quaternion = Quaternion.Euler(0f, this._player.eulerAngles.y, 0f);
			if (flag)
			{
				component4.enabled = false;
			}
			if (component3 != null)
			{
				component3.enabled = false;
			}
			try
			{
				this._player.SetPositionAndRotation(destination, quaternion);
				if (component2 != null)
				{
					component2.position = destination;
					component2.rotation = quaternion;
					if (flag2)
					{
						component2.isKinematic = false;
						component2.useGravity = true;
						component2.constraints = RigidbodyConstraints.FreezeRotation;
					}
					if (!component2.isKinematic)
					{
						component2.velocity = Vector3.zero;
						component2.angularVelocity = Vector3.zero;
					}
				}
				DistanceTracker component5 = this._player.GetComponent<DistanceTracker>();
				if (component5 != null)
				{
					component5.ExcludeRecoveryDisplacement(destination - originalPosition);
				}
				if (flag2)
				{
					PlayerUnstuckService.RestoreLocomotion(this._player, component, component3);
				}
				Physics.SyncTransforms();
			}
			finally
			{
				if (component3 != null)
				{
					component3.enabled = flag3;
				}
				if (flag)
				{
					component4.enabled = true;
				}
			}
			foreach (MagicaCloth magicaCloth in this._player.GetComponentsInChildren<MagicaCloth>(true))
			{
				if (magicaCloth.isActiveAndEnabled)
				{
					magicaCloth.ResetCloth(false);
				}
			}
			if (!PlayerActivityGate.IsResourceOwned(PlayerContextResource.Camera))
			{
				vThirdPersonInput component6 = this._player.GetComponent<vThirdPersonInput>();
				if (component6 != null)
				{
					component6.ignoreTpCamera = false;
				}
				vThirdPersonCamera vThirdPersonCamera = ((component6 != null && component6.tpCamera != null) ? component6.tpCamera : Object.FindObjectOfType<vThirdPersonCamera>());
				if (vThirdPersonCamera != null && vThirdPersonCamera.isInit && vThirdPersonCamera.currentTarget != null)
				{
					vThirdPersonCamera.UnFreezeCamera();
					vThirdPersonCamera.ResetAngleWithoutSmooth();
					vThirdPersonCamera.SnapToCurrentStateWithoutSmooth(false);
				}
			}
			this.RememberSafePosition();
		}

		// Token: 0x06004C06 RID: 19462 RVA: 0x001D8894 File Offset: 0x001D6A94
		private static void RestoreLocomotion(Transform player, vThirdPersonController motor, CapsuleCollider capsule)
		{
			motor.enabled = true;
			motor.lockMovement = (motor.lockRotation = (motor.lockAnimMovement = (motor.lockAnimRotation = false)));
			motor.lockSetMoveSpeed = false;
			motor.ragdolled = false;
			if (capsule != null)
			{
				capsule.isTrigger = false;
				if (motor.colliderHeightDefault > 0f)
				{
					capsule.height = motor.colliderHeightDefault;
					capsule.radius = motor.colliderRadiusDefault;
					capsule.center = motor.colliderCenterDefault;
				}
			}
			vThirdPersonInput component = player.GetComponent<vThirdPersonInput>();
			if (component != null)
			{
				component.enabled = true;
				component.SetLockAllInput(false);
				component.lockMoveInput = false;
				component.lockUpdateMoveDirection = false;
				if (!PlayerActivityGate.IsResourceOwned(PlayerContextResource.Camera))
				{
					component.lockCameraInput = false;
				}
			}
			Animator animator = motor.animator;
			if (animator == null || animator.runtimeAnimatorController == null)
			{
				return;
			}
			animator.enabled = true;
			string[] array = new string[] { "Base Layer.Locomotion.Free Movement", "Base Layer.Free Movement", "Free Movement", "Locomotion", "Idle", "Grounded" };
			for (int i = 0; i < array.Length; i++)
			{
				int num = Animator.StringToHash(array[i]);
				if (animator.HasState(0, num))
				{
					animator.Play(num, 0, 0f);
					animator.Update(0f);
					return;
				}
			}
		}

		// Token: 0x06004C07 RID: 19463 RVA: 0x001D89F8 File Offset: 0x001D6BF8
		private static PlayerUnstuckService.StandingCapsule GetStandingCapsule(Transform player)
		{
			CapsuleCollider component = player.GetComponent<CapsuleCollider>();
			vThirdPersonController component2 = player.GetComponent<vThirdPersonController>();
			Vector3 lossyScale = player.lossyScale;
			float num = ((component != null) ? (component.radius * Mathf.Max(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.z))) : 0.3f);
			float num2 = ((component != null) ? (component.height * Mathf.Abs(lossyScale.y)) : 1.8f);
			Vector3 vector = ((component != null) ? Vector3.Scale(component.center, lossyScale) : (Vector3.up * 0.9f));
			if (component2 != null && component2.colliderHeightDefault > 0f)
			{
				num2 = component2.colliderHeightDefault * Mathf.Abs(lossyScale.y);
				num = component2.colliderRadiusDefault * Mathf.Max(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.z));
				vector = Vector3.Scale(component2.colliderCenterDefault, lossyScale);
			}
			num = Mathf.Max(0.02f, num);
			float num3 = vector.y - num2 * 0.5f;
			num2 = Mathf.Max(num2, 2f * num);
			vector.y = num3 + num2 * 0.5f;
			return new PlayerUnstuckService.StandingCapsule
			{
				Radius = num,
				Height = num2,
				BottomOffset = num3,
				CenterOffset = vector
			};
		}

		// Token: 0x06004C08 RID: 19464 RVA: 0x001D8B64 File Offset: 0x001D6D64
		private static bool TryNearby(Transform player, Vector3 origin, PlayerUnstuckService.StandingCapsule capsule, int scene, out Vector3 position)
		{
			Collider collider;
			if (PlayerUnstuckService.TryGround(player, origin, capsule, scene, 2f, 3f, out position, out collider))
			{
				return true;
			}
			foreach (float num in new float[] { 0.75f, 1.5f, 2.5f, 4f, 6f, 9f, 14f, 20f, 30f, 45f })
			{
				for (int j = 0; j < 16; j++)
				{
					float num2 = (float)j * 3.1415927f / 8f;
					Vector3 vector = origin + new Vector3(Mathf.Cos(num2), 0f, Mathf.Sin(num2)) * num;
					if (PlayerUnstuckService.TryGround(player, vector, capsule, scene, 2f, 3f, out position, out collider))
					{
						return true;
					}
				}
			}
			position = origin;
			return false;
		}

		// Token: 0x06004C09 RID: 19465 RVA: 0x001D8C20 File Offset: 0x001D6E20
		private static bool TryGround(Transform player, Vector3 candidate, PlayerUnstuckService.StandingCapsule capsule, int scene, float up, float down, out Vector3 position, out Collider ground)
		{
			position = candidate;
			ground = null;
			Vector3 feet = candidate + Vector3.up * capsule.BottomOffset;
			RaycastHit[] array = Physics.RaycastAll(feet + Vector3.up * up, Vector3.down, up + down, -1, QueryTriggerInteraction.Ignore);
			Array.Sort<RaycastHit>(array, (RaycastHit a, RaycastHit b) => Mathf.Abs(a.point.y - feet.y).CompareTo(Mathf.Abs(b.point.y - feet.y)));
			RaycastHit[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				RaycastHit raycastHit = array2[i];
				if (!PlayerUnstuckService.IsPlayer(raycastHit.collider, player) && PlayerUnstuckService.IsStructuralGround(raycastHit.collider, player, scene) && raycastHit.normal.y >= 0.7f)
				{
					float num = capsule.Radius * (1f / Mathf.Max(0.7f, raycastHit.normal.y) - 1f);
					Vector3 vector = new Vector3(candidate.x, raycastHit.point.y - capsule.BottomOffset + 0.04f + num, candidate.z);
					Collider collider;
					if (PoolWaterRecovery.TryFindWater(raycastHit.point + Vector3.up * 0.04f, out collider, 0.35f, true))
					{
						return false;
					}
					if (!PlayerUnstuckService.HasSupport(player, raycastHit.point, raycastHit.normal, capsule.Radius, scene) || !PlayerUnstuckService.CapsuleClear(player, vector, capsule))
					{
						return false;
					}
					position = vector;
					ground = raycastHit.collider;
					return true;
				}
				else
				{
					i++;
				}
			}
			return false;
		}

		// Token: 0x06004C0A RID: 19466 RVA: 0x001D8DB4 File Offset: 0x001D6FB4
		private static bool HasSupport(Transform player, Vector3 center, Vector3 normal, float radius, int scene)
		{
			for (int i = 0; i < 4; i++)
			{
				float num = (float)i * 3.1415927f * 0.5f;
				Vector3 vector = center + new Vector3(Mathf.Cos(num), 0f, Mathf.Sin(num)) * radius * 0.85f;
				vector.y = center.y - (normal.x * (vector.x - center.x) + normal.z * (vector.z - center.z)) / normal.y;
				RaycastHit[] array = Physics.RaycastAll(vector + Vector3.up * 0.32f, Vector3.down, 0.64f, -1, QueryTriggerInteraction.Ignore);
				bool flag = false;
				foreach (RaycastHit raycastHit in array)
				{
					if (PlayerUnstuckService.IsStructuralGround(raycastHit.collider, player, scene) && raycastHit.normal.y >= 0.7f && Mathf.Abs(raycastHit.point.y - vector.y) <= 0.3f)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004C0B RID: 19467 RVA: 0x001D8EE4 File Offset: 0x001D70E4
		private static bool CapsuleClear(Transform player, Vector3 position, PlayerUnstuckService.StandingCapsule capsule)
		{
			Quaternion quaternion = Quaternion.Euler(0f, player.eulerAngles.y, 0f);
			Vector3 vector = position + quaternion * capsule.CenterOffset;
			float num = capsule.Height * 0.5f - capsule.Radius;
			Collider[] array = Physics.OverlapCapsule(vector + Vector3.up * num, vector - Vector3.up * num, capsule.Radius, -1, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < array.Length; i++)
			{
				if (!PlayerUnstuckService.IsPlayer(array[i], player))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004C0C RID: 19468 RVA: 0x001D8F84 File Offset: 0x001D7184
		private static bool IsPlayer(Collider collider, Transform player)
		{
			return collider == null || (player != null && (collider.transform == player || collider.transform.IsChildOf(player)));
		}

		// Token: 0x06004C0D RID: 19469 RVA: 0x001D8FB8 File Offset: 0x001D71B8
		private static bool IsStructuralGround(Collider collider, Transform player, int scene)
		{
			return collider != null && collider.enabled && !collider.isTrigger && collider.gameObject.activeInHierarchy && PlayerUnstuckService.IsInCurrentWorld(collider.transform, scene) && !PlayerUnstuckService.IsPlayer(collider, player) && collider.attachedRigidbody == null && collider.GetComponentInParent<Animator>() == null && collider.GetComponentInParent<NPCSenses>() == null && collider.GetComponentInParent<VehicleAI>() == null && collider.GetComponentInParent<vVehicleController>() == null && collider.GetComponentInParent<vBikeController>() == null;
		}

		// Token: 0x06004C0E RID: 19470 RVA: 0x001D905C File Offset: 0x001D725C
		private static ChunkManager CurrentWorldManager()
		{
			ChunkManager instance = ChunkManager.Instance;
			if (instance == null || !instance.isActiveAndEnabled || instance.IsPoiTravelSuspended || instance.IsTearingDown)
			{
				return null;
			}
			if (!((GameSystemsBootstrapper.Instance != null) ? GameSystemsBootstrapper.Instance.GetFeatureEnabled(GameSystemsBootstrapper.RuntimeFeature.ProceduralMap) : (SceneContext.Current != null && SceneContext.Current.Profile != null && SceneContext.Current.Profile.proceduralMap)))
			{
				return null;
			}
			return instance;
		}

		// Token: 0x06004C0F RID: 19471 RVA: 0x001D90E4 File Offset: 0x001D72E4
		private static bool IsInCurrentWorld(Transform target, int scene)
		{
			if (target.gameObject.scene.handle == scene)
			{
				return true;
			}
			ChunkManager chunkManager = PlayerUnstuckService.CurrentWorldManager();
			if (chunkManager == null)
			{
				return false;
			}
			ChunkInstance chunkContaining = chunkManager.GetChunkContaining(target);
			return ((chunkContaining != null) ? chunkContaining.chunkRoot : null) != null && chunkContaining.chunkRoot.activeInHierarchy && chunkManager.IsChunkPresentationVisible(chunkContaining);
		}

		// Token: 0x06004C10 RID: 19472 RVA: 0x001D914C File Offset: 0x001D734C
		private bool TryLoadedFloors(Transform player, Vector3 origin, PlayerUnstuckService.StandingCapsule capsule, int scene, out Vector3 position)
		{
			PlayerUnstuckService.<>c__DisplayClass58_0 CS$<>8__locals1;
			CS$<>8__locals1.hints = new List<PlayerUnstuckService.FloorHint>();
			CS$<>8__locals1.unique = new HashSet<Vector3Int>();
			foreach (Collider collider in Object.FindObjectsOfType<Collider>())
			{
				Bounds bounds = collider.bounds;
				if (bounds.size.x >= capsule.Radius * 2f && bounds.size.z >= capsule.Radius * 2f)
				{
					Vector3 vector = new Vector3(Mathf.Clamp(origin.x, bounds.min.x + capsule.Radius, bounds.max.x - capsule.Radius), bounds.min.y, Mathf.Clamp(origin.z, bounds.min.z + capsule.Radius, bounds.max.z - capsule.Radius));
					Vector3 vector2 = vector - origin;
					vector2.y = 0f;
					if (vector2.sqrMagnitude <= 65536f && PlayerUnstuckService.IsStructuralGround(collider, player, scene))
					{
						if (origin.y >= bounds.min.y - 3f && origin.y <= bounds.max.y + 3f)
						{
							vector.y = origin.y;
							PlayerUnstuckService.<TryLoadedFloors>g__AddHint|58_0(vector, vector2.sqrMagnitude, true, ref CS$<>8__locals1);
						}
						if (this._hasEntryPosition && this._entryPosition.y >= bounds.min.y - 3f && this._entryPosition.y <= bounds.max.y + 3f)
						{
							vector.y = this._entryPosition.y;
							PlayerUnstuckService.<TryLoadedFloors>g__AddHint|58_0(vector, vector2.sqrMagnitude, true, ref CS$<>8__locals1);
						}
						vector.y = bounds.min.y - capsule.BottomOffset;
						PlayerUnstuckService.<TryLoadedFloors>g__AddHint|58_0(vector, vector2.sqrMagnitude + 0.01f, false, ref CS$<>8__locals1);
						if (bounds.size.y <= 0.8f || collider is BoxCollider || collider is TerrainCollider)
						{
							vector.y = bounds.max.y - capsule.BottomOffset;
							PlayerUnstuckService.<TryLoadedFloors>g__AddHint|58_0(vector, vector2.sqrMagnitude + 0.02f, false, ref CS$<>8__locals1);
						}
					}
				}
			}
			CS$<>8__locals1.hints.Sort(delegate(PlayerUnstuckService.FloorHint a, PlayerUnstuckService.FloorHint b)
			{
				int num = a.Distance.CompareTo(b.Distance);
				if (num == 0)
				{
					return a.Position.y.CompareTo(b.Position.y);
				}
				return num;
			});
			for (int j = 0; j < Mathf.Min(CS$<>8__locals1.hints.Count, 256); j++)
			{
				Vector3 vector3 = CS$<>8__locals1.hints[j].Position + Vector3.up * (capsule.BottomOffset - 0.12f);
				bool flag = false;
				if (!CS$<>8__locals1.hints[j].KnownElevation)
				{
					foreach (RaycastHit raycastHit in Physics.RaycastAll(vector3, Vector3.down, 40f, -1, QueryTriggerInteraction.Ignore))
					{
						if (PlayerUnstuckService.IsStructuralGround(raycastHit.collider, player, scene) && raycastHit.normal.y >= 0.7f)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag && PlayerUnstuckService.TryNearby(player, CS$<>8__locals1.hints[j].Position, capsule, scene, out position))
				{
					return true;
				}
			}
			position = origin;
			return false;
		}

		// Token: 0x06004C11 RID: 19473 RVA: 0x001D94DC File Offset: 0x001D76DC
		private static void ReleaseInteractions(Transform player)
		{
			FlyCamera.StopForContextChange();
			DetectionPlayerNormalizer.InterruptGameplayOverlays();
			SmartphoneController.InterruptForDetection();
			DildoPlacementSystem[] array = Object.FindObjectsOfType<DildoPlacementSystem>(true);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].InterruptForEncounter();
			}
			FlexiWallHeldActions[] array2 = Object.FindObjectsOfType<FlexiWallHeldActions>(true);
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].InterruptForEncounter();
			}
			DildoActionExecutor[] array3 = Object.FindObjectsOfType<DildoActionExecutor>(true);
			for (int i = 0; i < array3.Length; i++)
			{
				array3[i].CancelAction();
			}
			FlashGateRegistry.InterruptForEncounter(player);
			vBasicCoverController[] componentsInChildren = player.GetComponentsInChildren<vBasicCoverController>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].InterruptForEncounter();
			}
			TrashcanLootInteractable[] array4 = Object.FindObjectsOfType<TrashcanLootInteractable>(true);
			for (int i = 0; i < array4.Length; i++)
			{
				array4[i].InterruptForEncounter();
			}
			LockerHideInteractable[] array5 = Object.FindObjectsOfType<LockerHideInteractable>(true);
			for (int i = 0; i < array5.Length; i++)
			{
				array5[i].InterruptForEncounter();
			}
			DumpsterHideInteractable[] array6 = Object.FindObjectsOfType<DumpsterHideInteractable>(true);
			for (int i = 0; i < array6.Length; i++)
			{
				array6[i].InterruptForEncounter();
			}
			WasteCanHideInteractable[] array7 = Object.FindObjectsOfType<WasteCanHideInteractable>(true);
			for (int i = 0; i < array7.Length; i++)
			{
				array7[i].InterruptForEncounter();
			}
			GachaMachineController[] array8 = Object.FindObjectsOfType<GachaMachineController>(true);
			for (int i = 0; i < array8.Length; i++)
			{
				array8[i].InterruptForEncounter();
			}
			CarClimbBridge[] array9 = Object.FindObjectsOfType<CarClimbBridge>(true);
			for (int i = 0; i < array9.Length; i++)
			{
				array9[i].InterruptForEncounter();
			}
			foreach (GameplayEmoteController gameplayEmoteController in player.GetComponentsInChildren<GameplayEmoteController>(true))
			{
				if (gameplayEmoteController.CurrentState != GameplayEmoteController.EmoteState.Idle)
				{
					gameplayEmoteController.ForceEndEmoteImmediate();
				}
			}
			PlayerActionAnimator[] componentsInChildren3 = player.GetComponentsInChildren<PlayerActionAnimator>(true);
			for (int i = 0; i < componentsInChildren3.Length; i++)
			{
				componentsInChildren3[i].CancelCurrentAction();
			}
			if (CoverPoseController.Instance != null && CoverPoseController.Instance.IsCovering)
			{
				CoverPoseController.Instance.DeactivateCoverPose();
			}
			foreach (CarHideUnderInteractable carHideUnderInteractable in Object.FindObjectsOfType<CarHideUnderInteractable>(true))
			{
				if (carHideUnderInteractable.IsBusy)
				{
					carHideUnderInteractable.ForceExitImmediate();
				}
			}
			StadiumParachuteController[] componentsInChildren4 = player.GetComponentsInChildren<StadiumParachuteController>(true);
			for (int i = 0; i < componentsInChildren4.Length; i++)
			{
				componentsInChildren4[i].EndFlight(false);
			}
			SubwayRideDirector[] array11 = Object.FindObjectsOfType<SubwayRideDirector>(true);
			for (int i = 0; i < array11.Length; i++)
			{
				array11[i].InterruptForUnstuck();
			}
			foreach (CarRoofCarrier carRoofCarrier in Object.FindObjectsOfType<CarRoofCarrier>(true))
			{
				if (carRoofCarrier.HasRider)
				{
					carRoofCarrier.Dismount();
				}
			}
			DetectionPlayerNormalizer.ForceDismount(player);
			PoolDiveController[] componentsInChildren5 = player.GetComponentsInChildren<PoolDiveController>(true);
			for (int i = 0; i < componentsInChildren5.Length; i++)
			{
				componentsInChildren5[i].InterruptForEncounter();
			}
			vSwimming[] componentsInChildren6 = player.GetComponentsInChildren<vSwimming>(true);
			for (int i = 0; i < componentsInChildren6.Length; i++)
			{
				componentsInChildren6[i].ForceExitSwimming();
			}
			PoolEntryStateGuard[] componentsInChildren7 = player.GetComponentsInChildren<PoolEntryStateGuard>(true);
			for (int i = 0; i < componentsInChildren7.Length; i++)
			{
				componentsInChildren7[i].RestoreForEncounter();
			}
			SwimmingLayerSuppressor[] componentsInChildren8 = player.GetComponentsInChildren<SwimmingLayerSuppressor>(true);
			for (int i = 0; i < componentsInChildren8.Length; i++)
			{
				componentsInChildren8[i].RestoreForEncounter();
			}
			CielRagdollRecovery[] componentsInChildren9 = player.GetComponentsInChildren<CielRagdollRecovery>(true);
			for (int i = 0; i < componentsInChildren9.Length; i++)
			{
				componentsInChildren9[i].InterruptForUnstuck();
			}
			FirstPersonController[] componentsInChildren10 = player.GetComponentsInChildren<FirstPersonController>(true);
			for (int i = 0; i < componentsInChildren10.Length; i++)
			{
				componentsInChildren10[i].ResetForSceneTransition();
			}
		}

		// Token: 0x06004C13 RID: 19475 RVA: 0x001D9830 File Offset: 0x001D7A30
		[CompilerGenerated]
		internal static void <TryLoadedFloors>g__AddHint|58_0(Vector3 point, float distance, bool knownElevation = false, ref PlayerUnstuckService.<>c__DisplayClass58_0 A_3)
		{
			Vector3Int vector3Int = new Vector3Int(Mathf.RoundToInt(point.x * 2f), Mathf.RoundToInt(point.y * 2f), Mathf.RoundToInt(point.z * 2f));
			if (A_3.unique.Add(vector3Int))
			{
				A_3.hints.Add(new PlayerUnstuckService.FloorHint
				{
					Position = point,
					Distance = distance,
					KnownElevation = knownElevation
				});
			}
		}

		// Token: 0x04004D89 RID: 19849
		private const float FootClearance = 0.04f;

		// Token: 0x04004D8A RID: 19850
		private const float MinimumNormalY = 0.7f;

		// Token: 0x04004D8B RID: 19851
		private const int HistoryCapacity = 24;

		// Token: 0x04004D8C RID: 19852
		private static PlayerUnstuckService _runtime;

		// Token: 0x04004D8D RID: 19853
		private Transform _player;

		// Token: 0x04004D8E RID: 19854
		private SceneContext _context;

		// Token: 0x04004D8F RID: 19855
		private int _sceneHandle;

		// Token: 0x04004D90 RID: 19856
		private float _nextSample;

		// Token: 0x04004D91 RID: 19857
		private Vector3 _entryPosition;

		// Token: 0x04004D92 RID: 19858
		private bool _hasEntryPosition;

		// Token: 0x04004D93 RID: 19859
		private Coroutine _recoveryRoutine;

		// Token: 0x04004D94 RID: 19860
		private Action<bool, string> _recoveryCompleted;

		// Token: 0x04004D95 RID: 19861
		private PlayerUnstuckService.RecoveryRequest _request;

		// Token: 0x04004D96 RID: 19862
		private readonly List<PlayerUnstuckService.SafeSample> _history = new List<PlayerUnstuckService.SafeSample>(24);

		// Token: 0x02000A6D RID: 2669
		private struct SafeSample
		{
			// Token: 0x04004D97 RID: 19863
			public Vector3 Position;

			// Token: 0x04004D98 RID: 19864
			public Collider Ground;

			// Token: 0x04004D99 RID: 19865
			public int SceneHandle;
		}

		// Token: 0x02000A6E RID: 2670
		private struct StandingCapsule
		{
			// Token: 0x04004D9A RID: 19866
			public float Radius;

			// Token: 0x04004D9B RID: 19867
			public float Height;

			// Token: 0x04004D9C RID: 19868
			public float BottomOffset;

			// Token: 0x04004D9D RID: 19869
			public Vector3 CenterOffset;
		}

		// Token: 0x02000A6F RID: 2671
		private struct FloorHint
		{
			// Token: 0x04004D9E RID: 19870
			public Vector3 Position;

			// Token: 0x04004D9F RID: 19871
			public float Distance;

			// Token: 0x04004DA0 RID: 19872
			public bool KnownElevation;
		}

		// Token: 0x02000A70 RID: 2672
		private sealed class RecoveryRequest
		{
			// Token: 0x04004DA1 RID: 19873
			public Transform Player;

			// Token: 0x04004DA2 RID: 19874
			public SceneContext Context;

			// Token: 0x04004DA3 RID: 19875
			public int Scene;

			// Token: 0x04004DA4 RID: 19876
			public Vector3 OriginalPosition;

			// Token: 0x04004DA5 RID: 19877
			public Vector3 Focus;

			// Token: 0x04004DA6 RID: 19878
			public ChunkManager Manager;
		}
	}
}
