using System;
using System.Collections;
using Features.Detection;
using Features.PhoneCompanion;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.FirstPerson;
using UnityEngine;

namespace Features.Inventory.Placement
{
	// Token: 0x0200114D RID: 4429
	[DefaultExecutionOrder(-250)]
	public class VehicleItemPlacementSystem : MonoBehaviour
	{
		// Token: 0x17001556 RID: 5462
		// (get) Token: 0x06008697 RID: 34455 RVA: 0x0036FCC9 File Offset: 0x0036DEC9
		public bool IsPlacing
		{
			get
			{
				return this._isPlacing;
			}
		}

		// Token: 0x17001557 RID: 5463
		// (get) Token: 0x06008698 RID: 34456 RVA: 0x0036FCD1 File Offset: 0x0036DED1
		public static bool AnyPlacing
		{
			get
			{
				return VehicleItemPlacementSystem._activePlacement != null && VehicleItemPlacementSystem._activePlacement._isPlacing;
			}
		}

		// Token: 0x17001558 RID: 5464
		// (get) Token: 0x06008699 RID: 34457 RVA: 0x0036FCEC File Offset: 0x0036DEEC
		public static bool OwnsPlacementInput
		{
			get
			{
				bool placementHeld = InventoryControllerInput.PlacementHeld;
				if (!placementHeld)
				{
					VehicleItemPlacementSystem._placementPressClaimed = false;
				}
				PlayerInventoryManager instance = PlayerInventoryManager.Instance;
				InventoryItemSO inventoryItemSO = ((instance != null) ? instance.GetActiveItem() : null);
				return VehicleItemPlacementSystem.AnyPlacing || (placementHeld && (VehicleItemPlacementSystem._placementPressClaimed || (inventoryItemSO != null && inventoryItemSO.isVehicleItem)));
			}
		}

		// Token: 0x0600869A RID: 34458 RVA: 0x0036FD49 File Offset: 0x0036DF49
		private void OnDisable()
		{
			this.ExitPlacement();
		}

		// Token: 0x0600869B RID: 34459 RVA: 0x0036FD54 File Offset: 0x0036DF54
		private void Start()
		{
			if (this.playerInput == null)
			{
				this.playerInput = base.GetComponentInParent<vThirdPersonInput>();
			}
			if (this.playerRoot == null && this.playerInput != null)
			{
				this.playerRoot = this.playerInput.transform.root;
			}
			if (this.fpController == null && this.playerInput != null)
			{
				this.fpController = this.playerInput.GetComponent<FirstPersonController>();
			}
		}

		// Token: 0x0600869C RID: 34460 RVA: 0x0036FDDC File Offset: 0x0036DFDC
		private void Update()
		{
			if (GraphicsSettingsRuntimeUI.AnyOpen || GameplayInputGate.BlocksGameplayInput || TextInputFocus.BlocksGameplayInput || Time.timeScale <= 0f)
			{
				return;
			}
			if (this.inventoryManager == null)
			{
				this.ExitPlacement();
				return;
			}
			if (this._isPlacing && (this._placementLease == null || !this._placementLease.IsActive || (this._usesTripodCamera && !SmartphoneController.IsStableTripodCameraOwner)))
			{
				this.ExitPlacement();
				return;
			}
			if (this._isPlacing && this.inventoryManager.GetActiveItem() != this._activeItem)
			{
				this.ExitPlacement();
				return;
			}
			InventoryItemSO activeItem = this.inventoryManager.GetActiveItem();
			if (activeItem != null && activeItem.isVehicleItem && InventoryControllerInput.PlacementDown && !this._isPlacing)
			{
				this.EnterPlacement(activeItem);
			}
			if (this._isPlacing)
			{
				if (InventoryControllerInput.TryCancelPlacement(this) || !InventoryControllerInput.PlacementHeld)
				{
					this.ExitPlacement();
					return;
				}
				this.UpdatePlacement();
				if (InventoryControllerInput.TryConfirmPlacement())
				{
					if (this._lastResult.IsValid)
					{
						this.ConfirmPlacement();
						return;
					}
					this.PlayFailSFX();
				}
			}
		}

		// Token: 0x0600869D RID: 34461 RVA: 0x0036FEF4 File Offset: 0x0036E0F4
		private void EnterPlacement(InventoryItemSO item)
		{
			if (this._isPlacing || item == null || !item.isVehicleItem || !PlayerActivityGate.CanStartWorldInteraction || !PlayerActivityGate.Allows(PlayerActivity.Inventory))
			{
				return;
			}
			bool isStableTripodCameraOwner = SmartphoneController.IsStableTripodCameraOwner;
			PlayerContextResource playerContextResource = PlayerContextResource.WorldInteraction | PlayerContextResource.HeldItem;
			if (!isStableTripodCameraOwner)
			{
				playerContextResource |= PlayerContextResource.Camera;
			}
			if (!PlayerActivityGate.TryAcquire("Vehicle placement", this, PlayerActivity.Interact | PlayerActivity.Inventory, playerContextResource, PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.Inventory | PlayerActivity.Phone | PlayerActivity.DebugFly, out this._placementLease, false, null))
			{
				return;
			}
			this._usesTripodCamera = isStableTripodCameraOwner;
			this._isPlacing = true;
			VehicleItemPlacementSystem._activePlacement = this;
			VehicleItemPlacementSystem._placementPressClaimed = true;
			this._activeItem = item;
			this._lastResult = PlacementResult.Invalid;
			if (!this._usesTripodCamera && this.fpController != null)
			{
				this.fpController.EnterFirstPerson();
			}
			if (this.playerInput != null)
			{
				vMeleeCombatInput vMeleeCombatInput = this.playerInput as vMeleeCombatInput;
				this._savedMeleeLock = vMeleeCombatInput != null && vMeleeCombatInput.lockMeleeInput;
				if (!this._usesTripodCamera)
				{
					this.playerInput.SetLockAllInput(false);
				}
				if (vMeleeCombatInput != null)
				{
					vMeleeCombatInput.SetLockMeleeInput(true);
				}
			}
			if (!this._usesTripodCamera)
			{
				base.StartCoroutine(this.EnsureInputUnlocked());
			}
			this._placementCover = ((this.playerRoot != null) ? this.playerRoot.GetComponent<CoverPoseController>() : null);
			this._savedCoverEnabled = this._placementCover != null && this._placementCover.enabled;
			if (this._placementCover != null)
			{
				this._placementCover.enabled = false;
			}
			this.CacheFloorAnchor();
			this.SpawnGhost();
			if (this.inventoryManager != null)
			{
				this.inventoryManager.ToggleHandPrefabVisibility(false);
			}
		}

		// Token: 0x0600869E RID: 34462 RVA: 0x0037009C File Offset: 0x0036E29C
		private void ExitPlacement()
		{
			if (!this._isPlacing)
			{
				return;
			}
			this._isPlacing = false;
			if (VehicleItemPlacementSystem._activePlacement == this)
			{
				VehicleItemPlacementSystem._activePlacement = null;
			}
			if (!this._usesTripodCamera && this.fpController != null)
			{
				this.fpController.ExitFirstPerson();
			}
			vMeleeCombatInput vMeleeCombatInput = this.playerInput as vMeleeCombatInput;
			if (vMeleeCombatInput != null)
			{
				vMeleeCombatInput.SetLockMeleeInput(this._savedMeleeLock);
			}
			if (this._placementCover != null)
			{
				this._placementCover.enabled = this._savedCoverEnabled;
			}
			this._placementCover = null;
			if (this.ghost != null)
			{
				this.ghost.Destroy();
			}
			if (this.inventoryManager != null)
			{
				this.inventoryManager.ToggleHandPrefabVisibility(true);
			}
			this._activeItem = null;
			PlayerActivityLease placementLease = this._placementLease;
			if (placementLease != null)
			{
				placementLease.Dispose();
			}
			this._placementLease = null;
			this._usesTripodCamera = false;
		}

		// Token: 0x0600869F RID: 34463 RVA: 0x00370190 File Offset: 0x0036E390
		private void UpdatePlacement()
		{
			Camera main = Camera.main;
			if (main == null || this.surfaceDetector == null || this.playerRoot == null)
			{
				this._lastResult = PlacementResult.Invalid;
				return;
			}
			Vector3 vector = (this._usesTripodCamera ? (this.playerRoot.position + Vector3.up * 1.6f) : main.transform.position);
			Vector3? vector2 = (this._usesTripodCamera ? new Vector3?(this.playerRoot.forward + Vector3.down) : null);
			this._lastResult = this.surfaceDetector.DetectSurface(main, this.placementLayerMask, this.maxPlacementDistance, this.playerRoot, new Vector3?(vector), vector2);
			if (this._lastResult.SurfaceType != PlacementSurfaceType.Floor || !MovingPlacementSupport.Allows(this._activeItem, this._lastResult.SurfaceTransform))
			{
				this._lastResult.IsValid = false;
			}
			this.ApplyAnchorToResult(ref this._lastResult);
			if (this.ghost != null)
			{
				this.ghost.UpdateTarget(this._lastResult.Position, this._lastResult.Rotation, this._lastResult.IsValid, null);
				this.ghost.SetVisible(this._lastResult.IsValid);
			}
		}

		// Token: 0x060086A0 RID: 34464 RVA: 0x003702F4 File Offset: 0x0036E4F4
		private void ConfirmPlacement()
		{
			if (this._activeItem == null || !this._lastResult.IsValid || !MovingPlacementSupport.Allows(this._activeItem, this._lastResult.SurfaceTransform))
			{
				return;
			}
			GameObject floorPrefab = this._activeItem.floorPrefab;
			if (floorPrefab == null)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[VehicleItemPlacementSystem] '" + this._activeItem.itemName + "' has no floorPrefab assigned.");
				}
				return;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(floorPrefab, this._lastResult.Position, this._lastResult.Rotation, this.placedObjectsContainer);
			gameObject.name = "Placed_" + this._activeItem.itemName;
			vVehicleInteractable componentInChildren = gameObject.GetComponentInChildren<vVehicleInteractable>(true);
			if (componentInChildren != null)
			{
				componentInChildren.SetSourceItem(this._activeItem);
			}
			else if (this.debugLogs)
			{
				Debug.LogWarning("[VehicleItemPlacementSystem] '" + this._activeItem.itemName + "' placed but prefab has no vVehicleInteractable; pickup-back will not work.");
			}
			if (this.placementSuccessSFX != null && this.audioSource != null)
			{
				this.audioSource.PlayOneShot(this.placementSuccessSFX);
			}
			this.inventoryManager.ConsumeActiveItem();
			this.ExitPlacement();
		}

		// Token: 0x060086A1 RID: 34465 RVA: 0x00370433 File Offset: 0x0036E633
		private IEnumerator EnsureInputUnlocked()
		{
			int i = 0;
			while (i < 5 && this._isPlacing && !this._usesTripodCamera && !(this.playerInput == null))
			{
				if (!InventoryControllerInput.CanRead)
				{
					yield return null;
				}
				else
				{
					this.playerInput.lockInput = false;
					this.playerInput.lockCameraInput = false;
					this.playerInput.lockMoveInput = false;
					if (this.playerInput.cc != null)
					{
						this.playerInput.cc.lockMovement = false;
						this.playerInput.cc.lockRotation = false;
					}
					yield return null;
				}
				int num = i;
				i = num + 1;
			}
			yield break;
		}

		// Token: 0x060086A2 RID: 34466 RVA: 0x00370444 File Offset: 0x0036E644
		private void PlayFailSFX()
		{
			if (Time.time - this._failSFXLastPlayedTime < this.failSFXCooldown)
			{
				return;
			}
			if (this.placementFailSFX != null && this.audioSource != null)
			{
				this.audioSource.PlayOneShot(this.placementFailSFX);
			}
			this._failSFXLastPlayedTime = Time.time;
		}

		// Token: 0x060086A3 RID: 34467 RVA: 0x003704A0 File Offset: 0x0036E6A0
		private void SpawnGhost()
		{
			if (this._activeItem == null || this._activeItem.floorPrefab == null || this.ghost == null)
			{
				return;
			}
			this.ghost.Spawn(this._activeItem.floorPrefab, this.ghostMaterial, null, false);
		}

		// Token: 0x060086A4 RID: 34468 RVA: 0x00370504 File Offset: 0x0036E704
		private void CacheFloorAnchor()
		{
			this._hasFloorAnchor = false;
			if (this._activeItem == null || this._activeItem.floorPrefab == null)
			{
				return;
			}
			PlacementAnchor componentInChildren = this._activeItem.floorPrefab.GetComponentInChildren<PlacementAnchor>(true);
			if (componentInChildren != null)
			{
				this._floorAnchorLocalPos = componentInChildren.LocalPosition;
				this._floorAnchorLocalRot = componentInChildren.LocalRotation;
				this._hasFloorAnchor = true;
				return;
			}
			float num;
			if (VehicleItemPlacementSystem.TryComputePrefabBottomYLocal(this._activeItem.floorPrefab, out num))
			{
				this._floorAnchorLocalPos = new Vector3(0f, num, 0f);
				this._floorAnchorLocalRot = Quaternion.identity;
				this._hasFloorAnchor = true;
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[VehicleItemPlacementSystem] No PlacementAnchor on '{0}', derived bottom Y = {1:F3} from renderer bounds.", this._activeItem.itemName, num));
				}
			}
		}

		// Token: 0x060086A5 RID: 34469 RVA: 0x003705DC File Offset: 0x0036E7DC
		private static bool TryComputePrefabBottomYLocal(GameObject prefab, out float bottomYLocal)
		{
			bottomYLocal = 0f;
			if (prefab == null)
			{
				return false;
			}
			Transform transform = prefab.transform;
			bool flag = false;
			Bounds bounds = default(Bounds);
			foreach (MeshFilter meshFilter in prefab.GetComponentsInChildren<MeshFilter>(true))
			{
				if (!(meshFilter.sharedMesh == null))
				{
					VehicleItemPlacementSystem.EncapsulateLocal(transform, meshFilter.transform, meshFilter.sharedMesh.bounds, ref bounds, ref flag);
				}
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in prefab.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (!(skinnedMeshRenderer.sharedMesh == null))
				{
					VehicleItemPlacementSystem.EncapsulateLocal(transform, skinnedMeshRenderer.transform, skinnedMeshRenderer.localBounds, ref bounds, ref flag);
				}
			}
			if (!flag)
			{
				return false;
			}
			bottomYLocal = bounds.min.y;
			return true;
		}

		// Token: 0x060086A6 RID: 34470 RVA: 0x003706B8 File Offset: 0x0036E8B8
		private static void EncapsulateLocal(Transform root, Transform renderer, Bounds localBounds, ref Bounds combined, ref bool hasAny)
		{
			Matrix4x4 matrix4x = root.worldToLocalMatrix * renderer.localToWorldMatrix;
			Vector3 center = localBounds.center;
			Vector3 extents = localBounds.extents;
			for (int i = 0; i < 8; i++)
			{
				Vector3 vector = new Vector3(center.x + (((i & 1) == 0) ? (-extents.x) : extents.x), center.y + (((i & 2) == 0) ? (-extents.y) : extents.y), center.z + (((i & 4) == 0) ? (-extents.z) : extents.z));
				Vector3 vector2 = matrix4x.MultiplyPoint3x4(vector);
				if (!hasAny)
				{
					combined = new Bounds(vector2, Vector3.zero);
					hasAny = true;
				}
				else
				{
					combined.Encapsulate(vector2);
				}
			}
		}

		// Token: 0x060086A7 RID: 34471 RVA: 0x00370784 File Offset: 0x0036E984
		private void ApplyAnchorToResult(ref PlacementResult result)
		{
			if (!this._hasFloorAnchor)
			{
				return;
			}
			Quaternion quaternion = result.Rotation * Quaternion.Inverse(this._floorAnchorLocalRot);
			result.Position -= quaternion * this._floorAnchorLocalPos;
			result.Rotation = quaternion;
		}

		// Token: 0x04008948 RID: 35144
		[Header("Internal References")]
		[SerializeField]
		private PlacementSurfaceDetector surfaceDetector;

		// Token: 0x04008949 RID: 35145
		[SerializeField]
		private PlacementGhost ghost;

		// Token: 0x0400894A RID: 35146
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x0400894B RID: 35147
		[Header("External References (Explicit)")]
		[SerializeField]
		private Transform playerRoot;

		// Token: 0x0400894C RID: 35148
		[SerializeField]
		private vThirdPersonInput playerInput;

		// Token: 0x0400894D RID: 35149
		[SerializeField]
		private FirstPersonController fpController;

		// Token: 0x0400894E RID: 35150
		[SerializeField]
		private PlayerInventoryManager inventoryManager;

		// Token: 0x0400894F RID: 35151
		[Header("Settings")]
		[SerializeField]
		private LayerMask placementLayerMask;

		// Token: 0x04008950 RID: 35152
		[SerializeField]
		private float maxPlacementDistance = 5f;

		// Token: 0x04008951 RID: 35153
		[SerializeField]
		private Material ghostMaterial;

		// Token: 0x04008952 RID: 35154
		[SerializeField]
		private Transform placedObjectsContainer;

		// Token: 0x04008953 RID: 35155
		[Header("Audio")]
		[SerializeField]
		private AudioClip placementSuccessSFX;

		// Token: 0x04008954 RID: 35156
		[SerializeField]
		private AudioClip placementFailSFX;

		// Token: 0x04008955 RID: 35157
		[SerializeField]
		private float failSFXCooldown = 0.5f;

		// Token: 0x04008956 RID: 35158
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04008957 RID: 35159
		private bool _isPlacing;

		// Token: 0x04008958 RID: 35160
		private PlayerActivityLease _placementLease;

		// Token: 0x04008959 RID: 35161
		private bool _usesTripodCamera;

		// Token: 0x0400895A RID: 35162
		private bool _savedMeleeLock;

		// Token: 0x0400895B RID: 35163
		private bool _savedCoverEnabled;

		// Token: 0x0400895C RID: 35164
		private CoverPoseController _placementCover;

		// Token: 0x0400895D RID: 35165
		private InventoryItemSO _activeItem;

		// Token: 0x0400895E RID: 35166
		private PlacementResult _lastResult;

		// Token: 0x0400895F RID: 35167
		private float _failSFXLastPlayedTime = -999f;

		// Token: 0x04008960 RID: 35168
		private bool _hasFloorAnchor;

		// Token: 0x04008961 RID: 35169
		private Vector3 _floorAnchorLocalPos;

		// Token: 0x04008962 RID: 35170
		private Quaternion _floorAnchorLocalRot;

		// Token: 0x04008963 RID: 35171
		private static VehicleItemPlacementSystem _activePlacement;

		// Token: 0x04008964 RID: 35172
		private static bool _placementPressClaimed;
	}
}
