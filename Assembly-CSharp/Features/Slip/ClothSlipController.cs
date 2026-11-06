using System;
using System.Collections;
using System.Collections.Generic;
using Features.DebugTools;
using Features.Flash;
using Features.Inventory;
using Features.Inventory.Placement;
using Features.Rank;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.CoverWall;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Slip
{
	// Token: 0x02000C54 RID: 3156
	public class ClothSlipController : MonoBehaviour
	{
		// Token: 0x1400009B RID: 155
		// (add) Token: 0x06005B37 RID: 23351 RVA: 0x0023B500 File Offset: 0x00239700
		// (remove) Token: 0x06005B38 RID: 23352 RVA: 0x0023B538 File Offset: 0x00239738
		public event Action<string, GameObject> OnSlipVisualSpawned;

		// Token: 0x06005B39 RID: 23353 RVA: 0x0023B56D File Offset: 0x0023976D
		private void OnEnable()
		{
			this.InvalidateWornCandidates();
		}

		// Token: 0x06005B3A RID: 23354 RVA: 0x0023B575 File Offset: 0x00239775
		private void InvalidateWornCandidates()
		{
			this._wornCandidatesAnimator = null;
			this._nextWornCandidateRefresh = 0f;
			this._wornCandidates.Clear();
			this._wornSearchTransforms.Clear();
		}

		// Token: 0x17000EF9 RID: 3833
		// (get) Token: 0x06005B3B RID: 23355 RVA: 0x0023B59F File Offset: 0x0023979F
		public bool IsSlipping
		{
			get
			{
				return this._slipping;
			}
		}

		// Token: 0x06005B3C RID: 23356 RVA: 0x0023B5A8 File Offset: 0x002397A8
		public bool CanSlipLand(string onlyWornName = null)
		{
			if (WallCoverActivityLock.ActiveController != null)
			{
				return false;
			}
			if (!base.isActiveAndEnabled || this._slipping || ClothActionGate.IsBusy || PlayerActivityGate.IsBlocked(PlayerActivity.Undress))
			{
				return false;
			}
			RankRuntimeController rankRuntimeController = RankRuntimeController.Current;
			bool flag = false;
			foreach (ClothSlipController.SlipPiece slipPiece in this.pieces)
			{
				if ((onlyWornName == null || !(slipPiece.wornName != onlyWornName)) && (!(rankRuntimeController != null) || rankRuntimeController.CanSlipPiece(slipPiece.wornName)))
				{
					if (slipPiece.flashGate != null && slipPiece.flashGate.IsEngaged)
					{
						return false;
					}
					this.ResolveWorn(slipPiece);
					if (slipPiece.wornInstance != null && slipPiece.wornInstance.activeInHierarchy && ClothSlipController.LandEnvironmentPrefab(slipPiece) != null)
					{
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x17000EFA RID: 3834
		// (get) Token: 0x06005B3D RID: 23357 RVA: 0x0023B6A8 File Offset: 0x002398A8
		public bool AnySlipped
		{
			get
			{
				foreach (ClothSlipController.SlipPiece slipPiece in this.pieces)
				{
					if (slipPiece.slipped && slipPiece.envInstance != null)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000EFB RID: 3835
		// (get) Token: 0x06005B3E RID: 23358 RVA: 0x0023B714 File Offset: 0x00239914
		public bool HasAnyWornPiece
		{
			get
			{
				foreach (ClothSlipController.SlipPiece slipPiece in this.pieces)
				{
					this.ResolveWorn(slipPiece);
					if (slipPiece.wornInstance != null && slipPiece.wornInstance.activeInHierarchy)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000EFC RID: 3836
		// (get) Token: 0x06005B3F RID: 23359 RVA: 0x0023B78C File Offset: 0x0023998C
		public bool HasWaterSlipPieces
		{
			get
			{
				return this.pieces.Exists((ClothSlipController.SlipPiece p) => p.waterSlipPrefab != null && p.waterEnvPrefab != null);
			}
		}

		// Token: 0x17000EFD RID: 3837
		// (get) Token: 0x06005B40 RID: 23360 RVA: 0x0023B7B8 File Offset: 0x002399B8
		public bool HasAnyWornWaterPiece
		{
			get
			{
				foreach (ClothSlipController.SlipPiece slipPiece in this.pieces)
				{
					if (!(slipPiece.waterSlipPrefab == null) && !(slipPiece.waterEnvPrefab == null))
					{
						this.ResolveWorn(slipPiece);
						if (slipPiece.wornInstance != null && slipPiece.wornInstance.activeInHierarchy)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06005B41 RID: 23361 RVA: 0x0023B84C File Offset: 0x00239A4C
		public GameObject EnvInstanceFor(string wornName)
		{
			ClothSlipController.SlipPiece slipPiece = this.FindPiece(wornName);
			if (slipPiece == null)
			{
				return null;
			}
			return slipPiece.envInstance;
		}

		// Token: 0x06005B42 RID: 23362 RVA: 0x0023B86C File Offset: 0x00239A6C
		public bool HasSlippedPiece(string wornName)
		{
			ClothSlipController.SlipPiece slipPiece = this.FindPiece(wornName);
			return slipPiece != null && slipPiece.slipped && slipPiece.envInstance != null;
		}

		// Token: 0x06005B43 RID: 23363 RVA: 0x0023B89C File Offset: 0x00239A9C
		public bool IsFloatingPiece(string wornName)
		{
			ClothSlipController.SlipPiece slipPiece = this.FindPiece(wornName);
			return slipPiece != null && slipPiece.floating && slipPiece.slipped && slipPiece.envInstance != null;
		}

		// Token: 0x06005B44 RID: 23364 RVA: 0x0023B8D4 File Offset: 0x00239AD4
		public bool WasStolenPiece(string wornName)
		{
			ClothSlipController.SlipPiece slipPiece = this.FindPiece(wornName);
			return slipPiece != null && slipPiece.stolen;
		}

		// Token: 0x06005B45 RID: 23365 RVA: 0x0023B8F4 File Offset: 0x00239AF4
		public bool IsPieceWorn(string wornName)
		{
			ClothSlipController.SlipPiece slipPiece = this.FindPiece(wornName);
			if (slipPiece == null)
			{
				return false;
			}
			this.ResolveWorn(slipPiece);
			return slipPiece.wornInstance != null && slipPiece.wornInstance.activeInHierarchy;
		}

		// Token: 0x06005B46 RID: 23366 RVA: 0x0023B930 File Offset: 0x00239B30
		private void Awake()
		{
			if (this.animator == null)
			{
				this.animator = base.GetComponentInParent<Animator>();
			}
			this.ResolveFlashGates();
			if (this.audioSource == null)
			{
				this.audioSource = base.gameObject.AddComponent<AudioSource>();
				this.audioSource.playOnAwake = false;
				this.audioSource.spatialBlend = 0f;
			}
			if (this.slipSound != null && this.slipSound.loadState == AudioDataLoadState.Unloaded)
			{
				this.slipSound.LoadAudioData();
			}
		}

		// Token: 0x06005B47 RID: 23367 RVA: 0x0023B9C0 File Offset: 0x00239BC0
		private void ResolveFlashGates()
		{
			ArmsFlashController[] array = null;
			foreach (ClothSlipController.SlipPiece slipPiece in this.pieces)
			{
				IFlashGate flashGate = slipPiece.flashGateBehaviour as IFlashGate;
				if (flashGate != null)
				{
					slipPiece.flashGate = flashGate;
				}
				else if (!string.IsNullOrEmpty(slipPiece.flashLayerName))
				{
					if (array == null)
					{
						array = ((this.animator != null) ? this.animator.transform : base.transform).GetComponentsInChildren<ArmsFlashController>(true);
					}
					foreach (ArmsFlashController armsFlashController in array)
					{
						if (armsFlashController.Config != null && armsFlashController.Config.flashLayerName == slipPiece.flashLayerName)
						{
							slipPiece.flashGate = armsFlashController;
							break;
						}
					}
				}
			}
		}

		// Token: 0x06005B48 RID: 23368 RVA: 0x0023BAB0 File Offset: 0x00239CB0
		private ClothSlipController.SlipPiece FindPiece(string wornName)
		{
			foreach (ClothSlipController.SlipPiece slipPiece in this.pieces)
			{
				if (slipPiece.wornName == wornName)
				{
					return slipPiece;
				}
			}
			return null;
		}

		// Token: 0x06005B49 RID: 23369 RVA: 0x0023BB14 File Offset: 0x00239D14
		private void OnDisable()
		{
			this.InvalidateWornCandidates();
			base.StopAllCoroutines();
			foreach (GameObject gameObject in this._activeSlipVisuals)
			{
				if (gameObject != null)
				{
					Object.Destroy(gameObject);
				}
			}
			foreach (ClothSlipController.SlipPiece slipPiece in this._activeSlipPieces)
			{
				if (!slipPiece.slipped && slipPiece.wornInstance != null)
				{
					slipPiece.wornInstance.SetActive(true);
				}
			}
			this._activeSlipVisuals.Clear();
			this._activeSlipPieces.Clear();
			this._slipping = false;
			ClothActionGate.Release(this);
		}

		// Token: 0x06005B4A RID: 23370 RVA: 0x0023BBFC File Offset: 0x00239DFC
		public void ResetForHomeReturn()
		{
			this.OnDisable();
			foreach (ClothSlipController.SlipPiece slipPiece in this.pieces)
			{
				if (slipPiece.envInstance != null)
				{
					Object.Destroy(slipPiece.envInstance);
				}
				slipPiece.envInstance = null;
				if (slipPiece.slipped || slipPiece.stolen)
				{
					IFlashGate flashGate = slipPiece.flashGate;
					if (flashGate != null)
					{
						flashGate.SetRestored();
					}
				}
				slipPiece.slipped = (slipPiece.stolen = (slipPiece.floating = false));
			}
			this._floatEnvironment = false;
		}

		// Token: 0x06005B4B RID: 23371 RVA: 0x0023BCB0 File Offset: 0x00239EB0
		private void Update()
		{
			if (this.allowLandSlipKey && !FlyCamera.IsActive && this.slipInput != null && this.slipInput.GetButtonDown())
			{
				this.TrySlipLandAll();
			}
			if (this._floatEnvironment)
			{
				float num = this.waterlineY + Mathf.Sin(Time.time * this.floatBobSpeed) * this.floatBobAmplitude;
				foreach (ClothSlipController.SlipPiece slipPiece in this.pieces)
				{
					ClothSlipController.BobEnv(slipPiece, num);
				}
			}
		}

		// Token: 0x06005B4C RID: 23372 RVA: 0x0023BD54 File Offset: 0x00239F54
		private static void BobEnv(ClothSlipController.SlipPiece p, float y)
		{
			if (p.envInstance == null)
			{
				return;
			}
			Vector3 position = p.envInstance.transform.position;
			position.y = y;
			p.envInstance.transform.position = position;
		}

		// Token: 0x06005B4D RID: 23373 RVA: 0x0023BD9A File Offset: 0x00239F9A
		public void SuspendFloatBob()
		{
			this._floatEnvironment = false;
		}

		// Token: 0x06005B4E RID: 23374 RVA: 0x0023BDA3 File Offset: 0x00239FA3
		public bool TrySlipLandAll()
		{
			return this.TrySlip(null, false, null);
		}

		// Token: 0x06005B4F RID: 23375 RVA: 0x0023BDB0 File Offset: 0x00239FB0
		public void SetLandDropItem(string wornName, InventoryItemSO item)
		{
			if (item == null)
			{
				return;
			}
			foreach (ClothSlipController.SlipPiece slipPiece in this.pieces)
			{
				if (slipPiece.wornName == wornName)
				{
					slipPiece.inventoryLandItem = item;
					if (item.floorPrefab != null)
					{
						slipPiece.landEnvPrefab = item.floorPrefab;
					}
				}
			}
		}

		// Token: 0x06005B50 RID: 23376 RVA: 0x0023BE38 File Offset: 0x0023A038
		private static GameObject LandEnvironmentPrefab(ClothSlipController.SlipPiece piece)
		{
			if (!(piece.inventoryLandItem != null) || !(piece.inventoryLandItem.floorPrefab != null))
			{
				return piece.landEnvPrefab;
			}
			return piece.inventoryLandItem.floorPrefab;
		}

		// Token: 0x06005B51 RID: 23377 RVA: 0x0023BE70 File Offset: 0x0023A070
		private GameObject SpawnLandEnvironment(ClothSlipController.SlipPiece piece, Vector3 surface, Quaternion legacyRotation, Vector3 legacyScale)
		{
			InventoryDropController instance = InventoryDropController.Instance;
			if (Application.isPlaying && instance != null && instance.isActiveAndEnabled && piece.inventoryLandItem != null)
			{
				PlayerInventoryManager instance2 = PlayerInventoryManager.Instance;
				ItemRarity itemRarity = piece.inventoryLandItem.rarity;
				int num = -1;
				float num2 = 1f;
				if (instance2 != null)
				{
					for (int i = 0; i < instance2.MaxClothSlots; i++)
					{
						if (instance2.GetClothAt(i) == piece.inventoryLandItem)
						{
							itemRarity = instance2.GetClothRarityAt(i);
							num = instance2.GetClothVariantAt(i);
							float clothDurabilityBase = instance2.GetClothDurabilityBase(i);
							num2 = ((clothDurabilityBase > 0f) ? Mathf.Clamp01(instance2.GetClothDurability(i) / clothDurabilityBase) : 1f);
							break;
						}
					}
				}
				GameObject dropped = instance.SpawnDropAt(piece.inventoryLandItem, surface + Vector3.up * 0.15f, itemRarity, num, num2, false, false);
				InventoryPickup inventoryPickup = ((dropped != null) ? dropped.GetComponentInChildren<InventoryPickup>() : null);
				if (inventoryPickup != null)
				{
					inventoryPickup.Collected += delegate
					{
						if (this != null && piece.envInstance == dropped)
						{
							this.DiscardSlipped(piece.wornName);
						}
					};
				}
				return dropped;
			}
			GameObject gameObject = ClothSlipController.LandEnvironmentPrefab(piece);
			if (gameObject == null)
			{
				return null;
			}
			bool flag = piece.inventoryLandItem != null && piece.inventoryLandItem.floorPrefab != null;
			GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, surface, flag ? gameObject.transform.rotation : legacyRotation);
			gameObject2.name = gameObject.name;
			if (!flag)
			{
				gameObject2.transform.localScale = legacyScale;
			}
			DetachedClothVisual.PlaceOnSurface(gameObject2, surface + Vector3.up * 0.01f);
			return gameObject2;
		}

		// Token: 0x06005B52 RID: 23378 RVA: 0x0023C082 File Offset: 0x0023A282
		public bool SlipInWaterAll()
		{
			return this.TrySlip(null, true, null);
		}

		// Token: 0x06005B53 RID: 23379 RVA: 0x0023C08D File Offset: 0x0023A28D
		public bool SlipInWaterAll(PlayerActivityLease diveContext)
		{
			return this.TrySlip(null, true, diveContext);
		}

		// Token: 0x06005B54 RID: 23380 RVA: 0x0023C098 File Offset: 0x0023A298
		public bool TrySlipLandPiece(string wornName)
		{
			return this.TrySlip(wornName, false, null);
		}

		// Token: 0x06005B55 RID: 23381 RVA: 0x0023C0A3 File Offset: 0x0023A2A3
		public bool SlipInWaterPiece(string wornName)
		{
			return this.TrySlip(wornName, true, null);
		}

		// Token: 0x06005B56 RID: 23382 RVA: 0x0023C0B0 File Offset: 0x0023A2B0
		private bool TrySlip(string onlyWornName, bool floatEnv, PlayerActivityLease diveContext = null)
		{
			if (this._slipping)
			{
				return false;
			}
			this._nextWornCandidateRefresh = 0f;
			List<ClothSlipController.SlipPiece> list = new List<ClothSlipController.SlipPiece>();
			foreach (ClothSlipController.SlipPiece slipPiece in this.pieces)
			{
				if ((onlyWornName == null || !(slipPiece.wornName != onlyWornName)) && (!floatEnv || (!(slipPiece.waterSlipPrefab == null) && !(slipPiece.waterEnvPrefab == null))))
				{
					this.ResolveWorn(slipPiece);
					if (slipPiece.wornInstance != null && slipPiece.wornInstance.activeInHierarchy)
					{
						list.Add(slipPiece);
					}
				}
			}
			if (list.Count == 0)
			{
				return false;
			}
			if (!ClothSlipController.AllowsSlipRequest(floatEnv, diveContext))
			{
				BlockedActionFeedback.Show(null);
				return false;
			}
			RankRuntimeController rankRuntimeController = RankRuntimeController.Current;
			for (int i = list.Count - 1; i >= 0; i--)
			{
				ClothSlipController.SlipPiece slipPiece2 = list[i];
				if (rankRuntimeController != null && !rankRuntimeController.CanSlipPiece(slipPiece2.wornName))
				{
					list.RemoveAt(i);
				}
				else if (slipPiece2.flashGate != null && slipPiece2.flashGate.IsEngaged)
				{
					return false;
				}
			}
			if (list.Count == 0)
			{
				return false;
			}
			if (!ClothActionGate.TryBegin(this))
			{
				return false;
			}
			base.StartCoroutine(this.SlipRoutine(list, floatEnv));
			return true;
		}

		// Token: 0x06005B57 RID: 23383 RVA: 0x0023C214 File Offset: 0x0023A414
		private static bool AllowsSlipRequest(bool floatEnv, PlayerActivityLease diveContext)
		{
			if (!floatEnv || diveContext == null || !diveContext.IsActive)
			{
				return PlayerActivityGate.Allows(PlayerActivity.Undress);
			}
			return PlayerActivityGate.CanAcquireAlongside(PlayerActivity.Undress, PlayerContextResource.None, diveContext.Owner, null);
		}

		// Token: 0x06005B58 RID: 23384 RVA: 0x0023C23B File Offset: 0x0023A43B
		private IEnumerator SlipRoutine(List<ClothSlipController.SlipPiece> dropping, bool floatEnv)
		{
			this._slipping = true;
			this._activeSlipPieces.AddRange(dropping);
			if (!floatEnv && PlayerActivityGate.IsResourceOwned(PlayerContextResource.Rider))
			{
				yield return this.MountedSlipRoutine(dropping);
				yield break;
			}
			if (this.slipAnimationDelay > 0f)
			{
				yield return new WaitForSeconds(this.slipAnimationDelay);
			}
			Transform transform = this.ResolveAnchor();
			Vector3 spawnPos = transform.position;
			Quaternion spawnRot = transform.rotation;
			Vector3 spawnScale = transform.lossyScale;
			Dictionary<ClothSlipController.SlipPiece, GameObject> slips = new Dictionary<ClothSlipController.SlipPiece, GameObject>();
			foreach (ClothSlipController.SlipPiece slipPiece in dropping)
			{
				GameObject gameObject = this.SpawnSlip(slipPiece, floatEnv, transform, spawnPos, spawnRot, spawnScale);
				if (gameObject != null)
				{
					slips[slipPiece] = gameObject;
					this._activeSlipVisuals.Add(gameObject);
				}
			}
			foreach (ClothSlipController.SlipPiece slipPiece2 in dropping)
			{
				if (slipPiece2.wornInstance != null)
				{
					slipPiece2.wornInstance.SetActive(false);
				}
			}
			this.PlaySlipSound();
			if (this.jiggleOnTopSlip && dropping.Count > 0 && dropping[0] == this.FirstPiece())
			{
				this.TryShakeJiggle();
			}
			float num = 0f;
			foreach (KeyValuePair<ClothSlipController.SlipPiece, GameObject> keyValuePair in slips)
			{
				string text = (floatEnv ? keyValuePair.Key.waterClipName : keyValuePair.Key.landClipName);
				num = Mathf.Max(num, this.ResolveSlipDuration(keyValuePair.Value, text));
			}
			if (num <= 0f)
			{
				num = this.fallbackSlipDuration;
			}
			yield return new WaitForSeconds(num);
			foreach (ClothSlipController.SlipPiece slipPiece3 in dropping)
			{
				GameObject gameObject2;
				slips.TryGetValue(slipPiece3, out gameObject2);
				this.PlaceEnv(slipPiece3, gameObject2, floatEnv, spawnPos, spawnRot, spawnScale);
				slipPiece3.slipped = slipPiece3.envInstance != null;
				slipPiece3.stolen = false;
				if (!slipPiece3.slipped && slipPiece3.wornInstance != null)
				{
					slipPiece3.wornInstance.SetActive(true);
				}
				if (gameObject2 != null)
				{
					Object.Destroy(gameObject2);
				}
			}
			this.CompleteSlip(dropping, floatEnv);
			yield break;
		}

		// Token: 0x06005B59 RID: 23385 RVA: 0x0023C258 File Offset: 0x0023A458
		private void CompleteSlip(List<ClothSlipController.SlipPiece> dropping, bool floatEnv)
		{
			this._activeSlipVisuals.Clear();
			this._activeSlipPieces.Clear();
			this._slipping = false;
			this._floatEnvironment = floatEnv;
			ClothActionGate.Release(this);
			foreach (ClothSlipController.SlipPiece slipPiece in dropping)
			{
				IFlashGate flashGate = slipPiece.flashGate;
				if (flashGate != null)
				{
					flashGate.SetRemoved();
				}
			}
			UnityEvent onSlipped = this.OnSlipped;
			if (onSlipped == null)
			{
				return;
			}
			onSlipped.Invoke();
		}

		// Token: 0x06005B5A RID: 23386 RVA: 0x0023C2E8 File Offset: 0x0023A4E8
		private IEnumerator MountedSlipRoutine(List<ClothSlipController.SlipPiece> dropping)
		{
			Transform transform = this.ResolveAnchor();
			Transform transform2 = ((this.animator != null) ? this.animator.transform : base.transform);
			Vector3 vector = Vector3.ProjectOnPlane(transform2.forward, Vector3.up).normalized;
			if (vector.sqrMagnitude < 0.001f)
			{
				vector = Vector3.forward;
			}
			Vector3 side = Vector3.Cross(Vector3.up, vector);
			Quaternion groundRotation = Quaternion.LookRotation(vector, Vector3.up);
			Vector3 scale = transform.lossyScale;
			Vector3 vector2 = Vector3.zero;
			foreach (Rigidbody rigidbody in transform2.GetComponentsInParent<Rigidbody>())
			{
				if (!rigidbody.isKinematic)
				{
					vector2 = rigidbody.velocity;
					break;
				}
			}
			vector2 = Vector3.ClampMagnitude(Vector3.ProjectOnPlane(vector2, Vector3.up), 12f);
			List<ClothSlipController.SlipPiece> successful = new List<ClothSlipController.SlipPiece>();
			List<DetachedClothVisual> visuals = new List<DetachedClothVisual>();
			List<Vector3> starts = new List<Vector3>();
			List<Vector3> landings = new List<Vector3>();
			foreach (ClothSlipController.SlipPiece slipPiece in dropping)
			{
				if (!(ClothSlipController.LandEnvironmentPrefab(slipPiece) == null))
				{
					DetachedClothVisual detachedClothVisual = DetachedClothVisual.Capture(slipPiece.wornInstance);
					if (!(detachedClothVisual == null))
					{
						this._activeSlipVisuals.Add(detachedClothVisual.gameObject);
						Action<string, GameObject> onSlipVisualSpawned = this.OnSlipVisualSpawned;
						if (onSlipVisualSpawned != null)
						{
							onSlipVisualSpawned(slipPiece.wornName, detachedClothVisual.gameObject);
						}
						Vector3 position = detachedClothVisual.transform.position;
						Vector3 vector3 = position + side * this.mountedDropDistance - vector * 0.35f + vector2 * 0.2f;
						Vector3 vector4 = this.FindMountedLanding(vector3, transform2);
						successful.Add(slipPiece);
						visuals.Add(detachedClothVisual);
						starts.Add(position);
						landings.Add(vector4);
						slipPiece.wornInstance.SetActive(false);
					}
				}
			}
			if (successful.Count == 0)
			{
				this._activeSlipPieces.Clear();
				this._slipping = false;
				ClothActionGate.Release(this);
				yield break;
			}
			this.PlaySlipSound();
			if (this.jiggleOnTopSlip && successful.Contains(this.FirstPiece()))
			{
				this.TryShakeJiggle();
			}
			float duration = Mathf.Max(0.1f, this.mountedDropDuration);
			for (float elapsed = 0f; elapsed < duration; elapsed += Time.deltaTime)
			{
				float num = Mathf.Clamp01(elapsed / duration);
				for (int j = 0; j < visuals.Count; j++)
				{
					if (!(visuals[j] == null))
					{
						visuals[j].transform.position = Vector3.Lerp(starts[j], landings[j] + Vector3.up * 0.15f, num) + Vector3.up * (4f * this.mountedDropArcHeight * num * (1f - num));
						visuals[j].transform.rotation = Quaternion.AngleAxis(95f * num, side) * Quaternion.AngleAxis(35f * num, Vector3.up);
					}
				}
				yield return null;
			}
			for (int k = 0; k < successful.Count; k++)
			{
				ClothSlipController.SlipPiece slipPiece2 = successful[k];
				slipPiece2.envInstance = this.SpawnLandEnvironment(slipPiece2, landings[k], groundRotation, scale);
				Action<string, GameObject> onSlipVisualSpawned2 = this.OnSlipVisualSpawned;
				if (onSlipVisualSpawned2 != null)
				{
					onSlipVisualSpawned2(slipPiece2.wornName, slipPiece2.envInstance);
				}
				slipPiece2.slipped = true;
				slipPiece2.stolen = false;
				slipPiece2.floating = false;
				if (visuals[k] != null)
				{
					Object.Destroy(visuals[k].gameObject);
				}
			}
			this.CompleteSlip(successful, false);
			yield break;
		}

		// Token: 0x06005B5B RID: 23387 RVA: 0x0023C300 File Offset: 0x0023A500
		private Vector3 FindMountedLanding(Vector3 target, Transform rider)
		{
			Vector3 vector = target + Vector3.up * 2f;
			float num = float.MaxValue;
			Vector3 point = new Vector3(target.x, rider.position.y, target.z);
			foreach (RaycastHit raycastHit in Physics.RaycastAll(vector, Vector3.down, 25f, -5, QueryTriggerInteraction.Ignore))
			{
				if (raycastHit.point.y <= target.y + 0.15f && raycastHit.normal.y >= 0.4f && !raycastHit.transform.IsChildOf(rider))
				{
					Rigidbody rigidbody = raycastHit.rigidbody;
					if ((!(rigidbody != null) || !rider.IsChildOf(rigidbody.transform)) && raycastHit.distance < num)
					{
						num = raycastHit.distance;
						point = raycastHit.point;
					}
				}
			}
			return point;
		}

		// Token: 0x06005B5C RID: 23388 RVA: 0x0023C3EF File Offset: 0x0023A5EF
		private ClothSlipController.SlipPiece FirstPiece()
		{
			if (this.pieces.Count <= 0)
			{
				return null;
			}
			return this.pieces[0];
		}

		// Token: 0x06005B5D RID: 23389 RVA: 0x0023C410 File Offset: 0x0023A610
		private GameObject SpawnSlip(ClothSlipController.SlipPiece p, bool floatEnv, Transform anchor, Vector3 spawnPos, Quaternion spawnRot, Vector3 spawnScale)
		{
			GameObject gameObject = (floatEnv ? p.waterSlipPrefab : p.landSlipPrefab);
			if (gameObject == null)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"[ClothSlipController] ",
					floatEnv ? "Water" : "Land",
					" slip prefab not assigned for '",
					p.wornName,
					"'."
				}), this);
				return null;
			}
			GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject);
			gameObject2.name = gameObject.name;
			if (this.followBodyDuringSlip)
			{
				gameObject2.transform.SetParent(anchor, false);
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localRotation = Quaternion.identity;
				gameObject2.transform.localScale = Vector3.one;
			}
			else
			{
				gameObject2.transform.SetPositionAndRotation(spawnPos, spawnRot);
				gameObject2.transform.localScale = spawnScale;
			}
			if (this.rebindToBodyBones)
			{
				this.RebindToBodyBones(gameObject2, p);
			}
			Action<string, GameObject> onSlipVisualSpawned = this.OnSlipVisualSpawned;
			if (onSlipVisualSpawned != null)
			{
				onSlipVisualSpawned(p.wornName, gameObject2);
			}
			return gameObject2;
		}

		// Token: 0x06005B5E RID: 23390 RVA: 0x0023C520 File Offset: 0x0023A720
		private void PlaceEnv(ClothSlipController.SlipPiece p, GameObject slip, bool floatEnv, Vector3 spawnPos, Quaternion spawnRot, Vector3 spawnScale)
		{
			Vector3 vector = ((this.followBodyDuringSlip && slip != null) ? slip.transform.position : spawnPos);
			Quaternion quaternion = ((this.followBodyDuringSlip && slip != null) ? slip.transform.rotation : spawnRot);
			if (!floatEnv)
			{
				Transform transform = ((this.animator != null) ? this.animator.transform : base.transform);
				p.envInstance = this.SpawnLandEnvironment(p, this.FindMountedLanding(vector, transform), quaternion, spawnScale);
				p.floating = false;
				Action<string, GameObject> onSlipVisualSpawned = this.OnSlipVisualSpawned;
				if (onSlipVisualSpawned == null)
				{
					return;
				}
				onSlipVisualSpawned(p.wornName, p.envInstance);
				return;
			}
			else
			{
				GameObject waterEnvPrefab = p.waterEnvPrefab;
				if (waterEnvPrefab == null)
				{
					return;
				}
				p.envInstance = Object.Instantiate<GameObject>(waterEnvPrefab);
				p.envInstance.name = waterEnvPrefab.name;
				if (floatEnv)
				{
					Vector3 vector2 = vector;
					vector2.y = this.waterlineY;
					p.envInstance.transform.SetPositionAndRotation(vector2, quaternion);
				}
				else
				{
					p.envInstance.transform.SetPositionAndRotation(vector, quaternion);
				}
				p.envInstance.transform.localScale = spawnScale;
				p.floating = floatEnv;
				Action<string, GameObject> onSlipVisualSpawned2 = this.OnSlipVisualSpawned;
				if (onSlipVisualSpawned2 == null)
				{
					return;
				}
				onSlipVisualSpawned2(p.wornName, p.envInstance);
				return;
			}
		}

		// Token: 0x06005B5F RID: 23391 RVA: 0x0023C670 File Offset: 0x0023A870
		public bool NotifyStolen(string wornName = null)
		{
			bool flag = false;
			foreach (ClothSlipController.SlipPiece slipPiece in this.pieces)
			{
				if ((wornName == null || !(slipPiece.wornName != wornName)) && slipPiece.slipped && !(slipPiece.envInstance == null))
				{
					Object.Destroy(slipPiece.envInstance);
					slipPiece.envInstance = null;
					slipPiece.stolen = true;
					slipPiece.floating = false;
					IFlashGate flashGate = slipPiece.flashGate;
					if (flashGate != null)
					{
						flashGate.SetRemoved();
					}
					flag = true;
				}
			}
			if (flag)
			{
				if (!this.AnySlipped)
				{
					this._floatEnvironment = false;
				}
				UnityEvent onStolen = this.OnStolen;
				if (onStolen != null)
				{
					onStolen.Invoke();
				}
			}
			return flag;
		}

		// Token: 0x06005B60 RID: 23392 RVA: 0x0023C73C File Offset: 0x0023A93C
		public bool RestoreWorn(string wornName = null)
		{
			bool flag = false;
			foreach (ClothSlipController.SlipPiece slipPiece in this.pieces)
			{
				if ((wornName == null || !(slipPiece.wornName != wornName)) && slipPiece.slipped)
				{
					if (slipPiece.envInstance != null)
					{
						Object.Destroy(slipPiece.envInstance);
						slipPiece.envInstance = null;
					}
					if (slipPiece.wornInstance != null)
					{
						slipPiece.wornInstance.SetActive(true);
					}
					slipPiece.slipped = false;
					slipPiece.stolen = false;
					slipPiece.floating = false;
					IFlashGate flashGate = slipPiece.flashGate;
					if (flashGate != null)
					{
						flashGate.SetRestored();
					}
					flag = true;
				}
			}
			if (flag)
			{
				if (!this.AnySlipped)
				{
					this._floatEnvironment = false;
				}
				UnityEvent onRestored = this.OnRestored;
				if (onRestored != null)
				{
					onRestored.Invoke();
				}
			}
			return flag;
		}

		// Token: 0x06005B61 RID: 23393 RVA: 0x0023C830 File Offset: 0x0023AA30
		public bool DiscardSlipped(string wornName = null)
		{
			bool flag = false;
			foreach (ClothSlipController.SlipPiece slipPiece in this.pieces)
			{
				if ((wornName == null || !(slipPiece.wornName != wornName)) && slipPiece.slipped)
				{
					if (slipPiece.envInstance != null)
					{
						Object.Destroy(slipPiece.envInstance);
						slipPiece.envInstance = null;
					}
					slipPiece.slipped = false;
					slipPiece.stolen = false;
					slipPiece.floating = false;
					flag = true;
				}
			}
			if (flag)
			{
				if (!this.AnySlipped)
				{
					this._floatEnvironment = false;
				}
				UnityEvent onRestored = this.OnRestored;
				if (onRestored != null)
				{
					onRestored.Invoke();
				}
			}
			return flag;
		}

		// Token: 0x06005B62 RID: 23394 RVA: 0x0023C8F4 File Offset: 0x0023AAF4
		public Vector3? NearestEnvPosition(Vector3 from)
		{
			Vector3? vector = null;
			float num = float.MaxValue;
			foreach (ClothSlipController.SlipPiece slipPiece in this.pieces)
			{
				if (slipPiece.slipped && !(slipPiece.envInstance == null))
				{
					float sqrMagnitude = (slipPiece.envInstance.transform.position - from).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						vector = new Vector3?(slipPiece.envInstance.transform.position);
					}
				}
			}
			return vector;
		}

		// Token: 0x06005B63 RID: 23395 RVA: 0x0023C9A4 File Offset: 0x0023ABA4
		private void PlaySlipSound()
		{
			if (this.slipSound != null && this.audioSource != null)
			{
				this.audioSource.PlayOneShot(this.slipSound, this.slipSoundVolume);
			}
		}

		// Token: 0x06005B64 RID: 23396 RVA: 0x0023C9D9 File Offset: 0x0023ABD9
		private void TryShakeJiggle()
		{
			if (this.breastJiggleBehaviour == null)
			{
				return;
			}
			this.breastJiggleBehaviour.SendMessage("Shake", SendMessageOptions.DontRequireReceiver);
		}

		// Token: 0x06005B65 RID: 23397 RVA: 0x0023C9FC File Offset: 0x0023ABFC
		private Transform ResolveAnchor()
		{
			if (this.spawnAnchor != null)
			{
				return this.spawnAnchor;
			}
			if (this.animator != null && !string.IsNullOrEmpty(this.modelRootNameContains))
			{
				foreach (Transform transform in this.animator.GetComponentsInChildren<Transform>(true))
				{
					if (transform.name.Contains(this.modelRootNameContains))
					{
						return transform;
					}
				}
			}
			if (!(this.animator != null))
			{
				return base.transform;
			}
			return this.animator.transform;
		}

		// Token: 0x06005B66 RID: 23398 RVA: 0x0023CA90 File Offset: 0x0023AC90
		private float ResolveSlipDuration(GameObject slip, string clipName)
		{
			Animator componentInChildren = slip.GetComponentInChildren<Animator>();
			if (componentInChildren != null && componentInChildren.runtimeAnimatorController != null)
			{
				foreach (AnimationClip animationClip in componentInChildren.runtimeAnimatorController.animationClips)
				{
					if (animationClip != null && animationClip.name == clipName)
					{
						return animationClip.length;
					}
				}
				AnimationClip[] animationClips2 = componentInChildren.runtimeAnimatorController.animationClips;
				if (animationClips2 != null && animationClips2.Length != 0 && animationClips2[0] != null)
				{
					return animationClips2[0].length;
				}
			}
			return this.fallbackSlipDuration;
		}

		// Token: 0x06005B67 RID: 23399 RVA: 0x0023CB28 File Offset: 0x0023AD28
		private void ResolveWorn(ClothSlipController.SlipPiece p)
		{
			if (p.wornInstance != null && p.wornInstance.activeInHierarchy)
			{
				return;
			}
			if (this.animator == null || string.IsNullOrEmpty(p.wornName))
			{
				return;
			}
			if (this._wornCandidatesAnimator != this.animator || Time.unscaledTime >= this._nextWornCandidateRefresh || !this._wornCandidates.ContainsKey(p))
			{
				this.RefreshWornCandidates();
			}
			List<GameObject> list;
			if (!this._wornCandidates.TryGetValue(p, out list))
			{
				return;
			}
			p.wornInstance = null;
			for (int i = 0; i < list.Count; i++)
			{
				GameObject gameObject = list[i];
				if (!(gameObject == null))
				{
					if (p.wornInstance == null)
					{
						p.wornInstance = gameObject;
					}
					if (gameObject.activeInHierarchy)
					{
						p.wornInstance = gameObject;
						return;
					}
				}
			}
		}

		// Token: 0x06005B68 RID: 23400 RVA: 0x0023CC00 File Offset: 0x0023AE00
		private void RefreshWornCandidates()
		{
			this._wornCandidatesAnimator = this.animator;
			this._nextWornCandidateRefresh = Time.unscaledTime + 0.5f;
			this.animator.GetComponentsInChildren<Transform>(true, this._wornSearchTransforms);
			foreach (ClothSlipController.SlipPiece slipPiece in this.pieces)
			{
				List<GameObject> list;
				if (!this._wornCandidates.TryGetValue(slipPiece, out list))
				{
					list = new List<GameObject>();
					this._wornCandidates.Add(slipPiece, list);
				}
				list.Clear();
				if (!string.IsNullOrEmpty(slipPiece.wornName))
				{
					for (int i = 0; i < this._wornSearchTransforms.Count; i++)
					{
						Transform transform = this._wornSearchTransforms[i];
						if (!(transform == this.animator.transform) && transform.name.StartsWith(slipPiece.wornName, StringComparison.Ordinal))
						{
							list.Add(transform.gameObject);
						}
					}
				}
			}
		}

		// Token: 0x06005B69 RID: 23401 RVA: 0x0023CD14 File Offset: 0x0023AF14
		private void RebindToBodyBones(GameObject slip, ClothSlipController.SlipPiece p)
		{
			if (slip == null)
			{
				return;
			}
			Dictionary<string, Transform> dictionary = this.BuildLiveBoneMap(p);
			if (dictionary == null || dictionary.Count == 0)
			{
				Debug.LogWarning("[ClothSlipController] Re-bind skipped for '" + p.wornName + "': no live bone source found.", this);
				return;
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in slip.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				Transform[] bones = skinnedMeshRenderer.bones;
				if (bones != null && bones.Length != 0)
				{
					Transform[] array = new Transform[bones.Length];
					int num = 0;
					for (int j = 0; j < bones.Length; j++)
					{
						Transform transform;
						if (bones[j] != null && dictionary.TryGetValue(bones[j].name, out transform))
						{
							array[j] = transform;
							num++;
						}
						else
						{
							array[j] = bones[j];
						}
					}
					skinnedMeshRenderer.bones = array;
					Transform transform2;
					if (skinnedMeshRenderer.rootBone != null && dictionary.TryGetValue(skinnedMeshRenderer.rootBone.name, out transform2))
					{
						skinnedMeshRenderer.rootBone = transform2;
					}
					skinnedMeshRenderer.updateWhenOffscreen = true;
					Debug.Log(string.Format("[ClothSlipController] '{0}' slip re-bind: {1}/{2} bones matched.", p.wornName, num, bones.Length), this);
				}
			}
			DonorSkeleton.Detach(slip, dictionary, this);
		}

		// Token: 0x06005B6A RID: 23402 RVA: 0x0023CE58 File Offset: 0x0023B058
		private Dictionary<string, Transform> BuildLiveBoneMap(ClothSlipController.SlipPiece p)
		{
			Dictionary<string, Transform> dictionary = new Dictionary<string, Transform>();
			if (this.animator == null)
			{
				return dictionary;
			}
			SkinnedMeshRenderer skinnedMeshRenderer = null;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in this.animator.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (!(skinnedMeshRenderer2 == null) && (skinnedMeshRenderer2.name == this.liveBodyMeshName || skinnedMeshRenderer2.name.StartsWith(this.liveBodyMeshName)) && skinnedMeshRenderer2.bones != null && skinnedMeshRenderer2.bones.Length != 0)
				{
					skinnedMeshRenderer = skinnedMeshRenderer2;
					break;
				}
			}
			if (skinnedMeshRenderer != null)
			{
				foreach (Transform transform in skinnedMeshRenderer.bones)
				{
					if (transform != null && !dictionary.ContainsKey(transform.name))
					{
						dictionary[transform.name] = transform;
					}
				}
				if (skinnedMeshRenderer.rootBone != null && !dictionary.ContainsKey(skinnedMeshRenderer.rootBone.name))
				{
					dictionary[skinnedMeshRenderer.rootBone.name] = skinnedMeshRenderer.rootBone;
				}
				return dictionary;
			}
			SkinnedMeshRenderer skinnedMeshRenderer3 = ((p.wornInstance != null) ? (p.wornInstance.GetComponent<SkinnedMeshRenderer>() ?? p.wornInstance.GetComponentInChildren<SkinnedMeshRenderer>(true)) : null);
			if (skinnedMeshRenderer3 != null && skinnedMeshRenderer3.bones != null)
			{
				foreach (Transform transform2 in skinnedMeshRenderer3.bones)
				{
					if (transform2 != null && !dictionary.ContainsKey(transform2.name))
					{
						dictionary[transform2.name] = transform2;
					}
				}
				if (skinnedMeshRenderer3.rootBone != null && !dictionary.ContainsKey(skinnedMeshRenderer3.rootBone.name))
				{
					dictionary[skinnedMeshRenderer3.rootBone.name] = skinnedMeshRenderer3.rootBone;
				}
				Debug.LogWarning(string.Format("[ClothSlipController] Body mesh '{0}' not found; fell back to worn '{1}' SMR bones ({2}).", this.liveBodyMeshName, p.wornName, dictionary.Count), this);
			}
			return dictionary;
		}

		// Token: 0x04005C63 RID: 23651
		[Header("Refs")]
		[SerializeField]
		private Animator animator;

		// Token: 0x04005C64 RID: 23652
		[Tooltip("Optional: shakes a jiggle the moment a TOP piece slips off. Auto-found.")]
		[SerializeField]
		private MonoBehaviour breastJiggleBehaviour;

		// Token: 0x04005C65 RID: 23653
		[Tooltip("Node the slip/environment prefabs align to. If null, the model FBX root (name fragment below) is used.")]
		[SerializeField]
		private Transform spawnAnchor;

		// Token: 0x04005C66 RID: 23654
		[SerializeField]
		private string modelRootNameContains = "_ass";

		// Token: 0x04005C67 RID: 23655
		[Tooltip("Name of Ciel's MAIN body SkinnedMeshRenderer — the authoritative source of LIVE animated bones for the slip re-bind. Matched by prefix; the first body SMR found is used.")]
		[SerializeField]
		private string liveBodyMeshName = "Body";

		// Token: 0x04005C68 RID: 23656
		[Header("Pieces")]
		[SerializeField]
		private List<ClothSlipController.SlipPiece> pieces = new List<ClothSlipController.SlipPiece>();

		// Token: 0x04005C69 RID: 23657
		[Header("Water slip")]
		[Tooltip("World Y of the pool waterline; surfaced pieces float here. POI-Pool = -5.66.")]
		[SerializeField]
		private float waterlineY = -5.66f;

		// Token: 0x04005C6A RID: 23658
		[SerializeField]
		private float floatBobAmplitude = 0.06f;

		// Token: 0x04005C6B RID: 23659
		[SerializeField]
		private float floatBobSpeed = 1.2f;

		// Token: 0x04005C6C RID: 23660
		[Header("Common")]
		[SerializeField]
		private float fallbackSlipDuration = 1.7f;

		// Token: 0x04005C6D RID: 23661
		[Tooltip("Delay (s) before the slip animation starts (prefab spawn + worn-mesh hide + sound). Lets you sync the slip to the right beat of the action. 0 = instant.")]
		[SerializeField]
		private float slipAnimationDelay;

		// Token: 0x04005C6E RID: 23662
		[Tooltip("Parent the slip prefab to the body so it follows the dive arc (avoids the detached/frozen look).")]
		[SerializeField]
		private bool followBodyDuringSlip = true;

		// Token: 0x04005C6F RID: 23663
		[Tooltip("Re-bind the slip mesh to Ciel's LIVE bones so it deforms with her dive pose.")]
		[SerializeField]
		private bool rebindToBodyBones = true;

		// Token: 0x04005C70 RID: 23664
		[Tooltip("Shake the jiggle when the FIRST listed piece (the top) slips off.")]
		[SerializeField]
		private bool jiggleOnTopSlip = true;

		// Token: 0x04005C71 RID: 23665
		[Header("Mounted drop")]
		[SerializeField]
		[Min(0.1f)]
		private float mountedDropDuration = 0.85f;

		// Token: 0x04005C72 RID: 23666
		[SerializeField]
		[Min(0f)]
		private float mountedDropDistance = 1.2f;

		// Token: 0x04005C73 RID: 23667
		[SerializeField]
		[Min(0f)]
		private float mountedDropArcHeight = 0.45f;

		// Token: 0x04005C74 RID: 23668
		[Header("Audio")]
		[SerializeField]
		private AudioClip slipSound;

		// Token: 0x04005C75 RID: 23669
		[SerializeField]
		[Range(0f, 1f)]
		private float slipSoundVolume = 1f;

		// Token: 0x04005C76 RID: 23670
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04005C77 RID: 23671
		[Header("Input")]
		[Tooltip("Land slip key — drops ALL worn pieces at once (matches N).")]
		[SerializeField]
		private GenericInput slipInput = new GenericInput("N", "X", "N");

		// Token: 0x04005C78 RID: 23672
		[SerializeField]
		private bool allowLandSlipKey = true;

		// Token: 0x04005C79 RID: 23673
		[Header("Events")]
		public UnityEvent OnSlipped;

		// Token: 0x04005C7A RID: 23674
		public UnityEvent OnRestored;

		// Token: 0x04005C7B RID: 23675
		public UnityEvent OnStolen;

		// Token: 0x04005C7D RID: 23677
		private bool _slipping;

		// Token: 0x04005C7E RID: 23678
		private bool _floatEnvironment;

		// Token: 0x04005C7F RID: 23679
		private readonly List<GameObject> _activeSlipVisuals = new List<GameObject>();

		// Token: 0x04005C80 RID: 23680
		private readonly List<ClothSlipController.SlipPiece> _activeSlipPieces = new List<ClothSlipController.SlipPiece>();

		// Token: 0x04005C81 RID: 23681
		private readonly Dictionary<ClothSlipController.SlipPiece, List<GameObject>> _wornCandidates = new Dictionary<ClothSlipController.SlipPiece, List<GameObject>>();

		// Token: 0x04005C82 RID: 23682
		private readonly List<Transform> _wornSearchTransforms = new List<Transform>();

		// Token: 0x04005C83 RID: 23683
		private Animator _wornCandidatesAnimator;

		// Token: 0x04005C84 RID: 23684
		private float _nextWornCandidateRefresh;

		// Token: 0x02000C55 RID: 3157
		[Serializable]
		public class SlipPiece
		{
			// Token: 0x04005C85 RID: 23685
			[Tooltip("Worn mesh GameObject name (e.g. School_Swimsuit, EPBikini_Bra). Found by name under the Animator.")]
			public string wornName;

			// Token: 0x04005C86 RID: 23686
			[Tooltip("Animated slip prefab playing the LAND slip clip on enable.")]
			public GameObject landSlipPrefab;

			// Token: 0x04005C87 RID: 23687
			[Tooltip("Animated slip prefab playing the WATER slip clip on enable. Leave null for land-only garments.")]
			public GameObject waterSlipPrefab;

			// Token: 0x04005C88 RID: 23688
			[Tooltip("Static mesh left where the LAND slip ends.")]
			public GameObject landEnvPrefab;

			// Token: 0x04005C89 RID: 23689
			[Tooltip("Inventory item used for the final land drop. Shares the inventory spawn path.")]
			public InventoryItemSO inventoryLandItem;

			// Token: 0x04005C8A RID: 23690
			[Tooltip("Static mesh left floating at the waterline where the WATER slip ends.")]
			public GameObject waterEnvPrefab;

			// Token: 0x04005C8B RID: 23691
			public string landClipName;

			// Token: 0x04005C8C RID: 23692
			public string waterClipName;

			// Token: 0x04005C8D RID: 23693
			[Tooltip("OPTIONAL flash gate for this piece (its ArmsFlashController). Auto-resolved by flashLayerName if left empty. Used to block slip while flashing and to disable/restore flash on slip/pickup.")]
			public MonoBehaviour flashGateBehaviour;

			// Token: 0x04005C8E RID: 23694
			[Tooltip("Layer name used to auto-resolve this piece's flash gate among the ArmsFlashControllers under the player.")]
			public string flashLayerName;

			// Token: 0x04005C8F RID: 23695
			[NonSerialized]
			public GameObject wornInstance;

			// Token: 0x04005C90 RID: 23696
			[NonSerialized]
			public GameObject envInstance;

			// Token: 0x04005C91 RID: 23697
			[NonSerialized]
			public bool slipped;

			// Token: 0x04005C92 RID: 23698
			[NonSerialized]
			public bool stolen;

			// Token: 0x04005C93 RID: 23699
			[NonSerialized]
			public bool floating;

			// Token: 0x04005C94 RID: 23700
			[NonSerialized]
			public IFlashGate flashGate;
		}
	}
}
