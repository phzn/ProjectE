using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.BodyPhysics;
using Features.Detection;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.MainMenu;
using Features.NipplePiercings;
using Features.NPC;
using Features.NPC.StateMachine;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.PoiTravel;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.NippleRotor
{
	// Token: 0x02000F55 RID: 3925
	[DisallowMultipleComponent]
	public sealed class NippleRotorController : MonoBehaviour
	{
		// Token: 0x140000C9 RID: 201
		// (add) Token: 0x060075AB RID: 30123 RVA: 0x002FA8B0 File Offset: 0x002F8AB0
		// (remove) Token: 0x060075AC RID: 30124 RVA: 0x002FA8E4 File Offset: 0x002F8AE4
		public static event Action<int> LevelChanged;

		// Token: 0x17001352 RID: 4946
		// (get) Token: 0x060075AD RID: 30125 RVA: 0x002FA918 File Offset: 0x002F8B18
		public static NippleRotorController Current
		{
			get
			{
				PlayerInventoryManager instance = PlayerInventoryManager.Instance;
				if (instance == null)
				{
					return null;
				}
				Transform transform = ((instance.PlayerAnimator != null) ? instance.PlayerAnimator.transform : null);
				for (int i = NippleRotorController.EnabledControllers.Count - 1; i >= 0; i--)
				{
					NippleRotorController nippleRotorController = NippleRotorController.EnabledControllers[i];
					if (nippleRotorController == null)
					{
						NippleRotorController.EnabledControllers.RemoveAt(i);
					}
					else if (nippleRotorController._inventory == instance || (transform != null && nippleRotorController._playerRoot == transform))
					{
						return nippleRotorController;
					}
				}
				return null;
			}
		}

		// Token: 0x17001353 RID: 4947
		// (get) Token: 0x060075AE RID: 30126 RVA: 0x002FA9B4 File Offset: 0x002F8BB4
		public NippleRotorConfigSO Config
		{
			get
			{
				return this._config;
			}
		}

		// Token: 0x17001354 RID: 4948
		// (get) Token: 0x060075AF RID: 30127 RVA: 0x002FA9BC File Offset: 0x002F8BBC
		public Transform PlayerRoot
		{
			get
			{
				return this._playerRoot;
			}
		}

		// Token: 0x17001355 RID: 4949
		// (get) Token: 0x060075B0 RID: 30128 RVA: 0x002FA9C4 File Offset: 0x002F8BC4
		public Animator PlayerAnimator
		{
			get
			{
				return this._animator;
			}
		}

		// Token: 0x17001356 RID: 4950
		// (get) Token: 0x060075B1 RID: 30129 RVA: 0x002FA9CC File Offset: 0x002F8BCC
		public bool IsWorn
		{
			get
			{
				return this._worn;
			}
		}

		// Token: 0x17001357 RID: 4951
		// (get) Token: 0x060075B2 RID: 30130 RVA: 0x002FA9D4 File Offset: 0x002F8BD4
		public int Level
		{
			get
			{
				return this._level;
			}
		}

		// Token: 0x17001358 RID: 4952
		// (get) Token: 0x060075B3 RID: 30131 RVA: 0x002FA9DC File Offset: 0x002F8BDC
		public bool IsSuspended
		{
			get
			{
				return Time.timeScale <= 0f || this.IsInterrupted;
			}
		}

		// Token: 0x17001359 RID: 4953
		// (get) Token: 0x060075B4 RID: 30132 RVA: 0x002FA9F4 File Offset: 0x002F8BF4
		private bool IsInterrupted
		{
			get
			{
				if (DetectionInteractionGate.IsGameOver)
				{
					return true;
				}
				ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
				return (instance != null && instance.isActiveAndEnabled && instance.IsEventActive) || GameplayInputGate.IsLoading;
			}
		}

		// Token: 0x1700135A RID: 4954
		// (get) Token: 0x060075B5 RID: 30133 RVA: 0x002FAA30 File Offset: 0x002F8C30
		public bool IsVibrating
		{
			get
			{
				return this._worn && this._level > 0 && !this.IsSuspended;
			}
		}

		// Token: 0x1700135B RID: 4955
		// (get) Token: 0x060075B6 RID: 30134 RVA: 0x002FAA4E File Offset: 0x002F8C4E
		public bool CupsCovered
		{
			get
			{
				return this._visibility != null && this._visibility.IsCovered;
			}
		}

		// Token: 0x060075B7 RID: 30135 RVA: 0x002FAA6B File Offset: 0x002F8C6B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			NippleRotorController.EnabledControllers.Clear();
			NippleRotorController.LevelChanged = null;
		}

		// Token: 0x060075B8 RID: 30136 RVA: 0x002FAA80 File Offset: 0x002F8C80
		public void Configure(NippleRotorConfigSO config, PlayerInventoryManager inventory)
		{
			Animator animator = ((inventory != null) ? inventory.PlayerAnimator : null);
			if (config == this._config && inventory == this._inventory && animator == this._animator && this._playerRoot != null)
			{
				return;
			}
			this.SetWorn(false);
			this.BindWearController(null);
			this.ClearNoise();
			this.RestorePiercings();
			this._config = config;
			this._inventory = inventory;
			this._animator = animator;
			this._playerRoot = ((animator != null) ? animator.transform : null);
			this._actor = null;
			this._style = null;
			this._faceConfiguredFor = null;
			this._warnedMissingAssets = false;
			this.ResolveActor();
			this.ResolveFace();
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this.BindWearController(this.ResolveWearController());
			this.ReconcileWorn();
		}

		// Token: 0x060075B9 RID: 30137 RVA: 0x002FAB63 File Offset: 0x002F8D63
		public void SetLevel(int level, bool playSwitch = true)
		{
			this.SetLevelInternal(level, playSwitch);
		}

		// Token: 0x060075BA RID: 30138 RVA: 0x002FAB6D File Offset: 0x002F8D6D
		public void CycleLevel()
		{
			if (!this._worn)
			{
				return;
			}
			this.SetLevel(NippleRotorIds.NextLevel(this._level), true);
		}

		// Token: 0x060075BB RID: 30139 RVA: 0x002FAB8A File Offset: 0x002F8D8A
		private void Awake()
		{
			this.ResolveFace();
		}

		// Token: 0x060075BC RID: 30140 RVA: 0x002FAB94 File Offset: 0x002F8D94
		private void OnEnable()
		{
			if (!NippleRotorController.EnabledControllers.Contains(this))
			{
				NippleRotorController.EnabledControllers.Add(this);
			}
			this._housekeepingTimer = 0f;
			if (this._config == null || this._playerRoot == null)
			{
				return;
			}
			this.BindWearController(this.ResolveWearController());
			this.ReconcileWorn();
		}

		// Token: 0x060075BD RID: 30141 RVA: 0x002FABF3 File Offset: 0x002F8DF3
		private void OnDisable()
		{
			NippleRotorController.EnabledControllers.Remove(this);
			this.BindWearController(null);
			this.SetWorn(false);
			this.ClearNoise();
			if (this._face != null)
			{
				this._face.SetWanted(false);
			}
		}

		// Token: 0x060075BE RID: 30142 RVA: 0x002FAC2F File Offset: 0x002F8E2F
		private void OnDestroy()
		{
			NippleRotorController.EnabledControllers.Remove(this);
			this.BindWearController(null);
			this.ClearNoise();
			this.RestorePiercings();
			this.DespawnRig();
		}

		// Token: 0x060075BF RID: 30143 RVA: 0x002FAC58 File Offset: 0x002F8E58
		private void Update()
		{
			if (this._config == null || this._playerRoot == null)
			{
				return;
			}
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			this._housekeepingTimer -= unscaledDeltaTime;
			if (this._housekeepingTimer <= 0f)
			{
				this._housekeepingTimer = 0.5f;
				this.Housekeeping();
			}
			if (this._level != 0 && DetectionInteractionGate.IsGameOver)
			{
				this.SetLevelInternal(0, false);
			}
			bool isVibrating = this.IsVibrating;
			this.UpdateNoise(isVibrating);
			this.UpdateNpcTargets(isVibrating);
			this.UpdateFace(this._worn && this._level > 0 && !this.IsInterrupted);
			this.UpdateBuzz(isVibrating);
			if (this._cord != null)
			{
				this._cord.SetHidden(this.CupsCovered);
			}
			this.UpdateAudio(isVibrating, unscaledDeltaTime);
		}

		// Token: 0x060075C0 RID: 30144 RVA: 0x002FAD31 File Offset: 0x002F8F31
		private WearableUseController ResolveWearController()
		{
			if (!(this._playerRoot != null))
			{
				return null;
			}
			return this._playerRoot.GetComponentInChildren<WearableUseController>(true);
		}

		// Token: 0x060075C1 RID: 30145 RVA: 0x002FAD50 File Offset: 0x002F8F50
		private void BindWearController(WearableUseController wear)
		{
			if (this._wear != null)
			{
				this._wear.OnItemWorn -= this.HandleItemWorn;
				this._wear.OnItemUnworn -= this.HandleItemUnworn;
			}
			this._wear = wear;
			if (this._wear == null || !base.isActiveAndEnabled)
			{
				return;
			}
			this._wear.OnItemWorn += this.HandleItemWorn;
			this._wear.OnItemUnworn += this.HandleItemUnworn;
		}

		// Token: 0x060075C2 RID: 30146 RVA: 0x002FADE5 File Offset: 0x002F8FE5
		private static bool IsRotorItem(WearableItemSO item)
		{
			return item != null && string.Equals(item.itemId, "nipple_rotor", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060075C3 RID: 30147 RVA: 0x002FAE03 File Offset: 0x002F9003
		private bool IsRotorWorn()
		{
			return this._wear != null && this._wear.GetWorn("nipple_rotor", false) != null;
		}

		// Token: 0x060075C4 RID: 30148 RVA: 0x002FAE2C File Offset: 0x002F902C
		private void HandleItemWorn(WearableItemSO item)
		{
			if (NippleRotorController.IsRotorItem(item))
			{
				this.SetWorn(true);
			}
		}

		// Token: 0x060075C5 RID: 30149 RVA: 0x002FAE3D File Offset: 0x002F903D
		private void HandleItemUnworn(WearableItemSO item)
		{
			if (NippleRotorController.IsRotorItem(item))
			{
				this.SetWorn(this.IsRotorWorn());
			}
		}

		// Token: 0x060075C6 RID: 30150 RVA: 0x002FAE53 File Offset: 0x002F9053
		private void ReconcileWorn()
		{
			this.SetWorn(this._config != null && this.IsRotorWorn());
		}

		// Token: 0x060075C7 RID: 30151 RVA: 0x002FAE74 File Offset: 0x002F9074
		private void SetWorn(bool worn)
		{
			if (this._worn == worn)
			{
				return;
			}
			this._worn = worn;
			if (worn)
			{
				this.SpawnRig();
				this.RefreshPiercingHiding();
				return;
			}
			this.SetLevelInternal(0, false);
			this.DespawnRig();
			this.RestorePiercings();
			this.ClearNoise();
			if (this._face != null)
			{
				this._face.SetWanted(false);
			}
			this.StopAudioImmediate();
		}

		// Token: 0x060075C8 RID: 30152 RVA: 0x002FAEDC File Offset: 0x002F90DC
		private void SetLevelInternal(int level, bool playSwitch)
		{
			level = NippleRotorIds.ClampLevel(level);
			if (!this._worn || DetectionInteractionGate.IsGameOver)
			{
				level = 0;
			}
			if (level == this._level)
			{
				return;
			}
			int level2 = this._level;
			this._level = level;
			if (playSwitch)
			{
				this.PlaySwitch();
			}
			if (level > level2 && !this.IsSuspended)
			{
				this.Shake();
			}
			Action<int> levelChanged = NippleRotorController.LevelChanged;
			if (levelChanged == null)
			{
				return;
			}
			levelChanged(level);
		}

		// Token: 0x060075C9 RID: 30153 RVA: 0x002FAF48 File Offset: 0x002F9148
		private void Housekeeping()
		{
			if (this._wear == null)
			{
				this.BindWearController(this.ResolveWearController());
			}
			if (this._actor == null)
			{
				this.ResolveActor();
			}
			if (this._face == null)
			{
				this.ResolveFace();
			}
			this.ReconcileWorn();
			if (!this._worn)
			{
				return;
			}
			if ((this._cupsExpected && this._cups == null) || (this._remoteExpected && this._remote == null))
			{
				this.SpawnRig();
			}
			this.RefreshPiercingHiding();
			if (this._cordObject != null)
			{
				NippleRotorController.SetLayerRecursive(this._cordObject.transform, this._rigLayer);
			}
			if (this._cord != null && this._attachment != null && this._attachment.Body != null && this._breastSurface.Body != this._attachment.Body)
			{
				this.BindBreastSurface();
			}
		}

		// Token: 0x060075CA RID: 30154 RVA: 0x002FB054 File Offset: 0x002F9254
		private void SpawnRig()
		{
			this.DespawnRig();
			if (this._config == null || this._playerRoot == null)
			{
				return;
			}
			if (this._config.cupsPrefab != null)
			{
				this.SpawnCups();
			}
			this._rigLayer = this.ResolveRigLayer();
			if (this._cups != null)
			{
				NippleRotorController.SetLayerRecursive(this._cups.transform, this._rigLayer);
			}
			if (this._config.remotePrefab != null)
			{
				this.SpawnRemote();
			}
			if (this._cups != null && this._remote != null)
			{
				this.SpawnCord();
			}
			this.RefreshPoiProbeLighting();
			if ((this._config.cupsPrefab == null || this._config.remotePrefab == null) && !this._warnedMissingAssets)
			{
				this._warnedMissingAssets = true;
				Debug.LogWarning("[NippleRotor] The config is missing the cups or remote prefab; run Tools/Nipple Rotor/Build Assets.", this._config);
			}
		}

		// Token: 0x060075CB RID: 30155 RVA: 0x002FB154 File Offset: 0x002F9354
		private void RefreshPoiProbeLighting()
		{
			Transform transform = ((PlayerProximityProvider.Instance != null) ? PlayerProximityProvider.Instance.Player : null);
			if (transform == null)
			{
				return;
			}
			Scene activeScene = SceneManager.GetActiveScene();
			foreach (PoiCharacterProbeLighting poiCharacterProbeLighting in Object.FindObjectsByType<PoiCharacterProbeLighting>(FindObjectsSortMode.None))
			{
				if (poiCharacterProbeLighting.isActiveAndEnabled && poiCharacterProbeLighting.gameObject.scene == activeScene)
				{
					poiCharacterProbeLighting.ApplyToPlayer(transform);
				}
			}
		}

		// Token: 0x060075CC RID: 30156 RVA: 0x002FB1CC File Offset: 0x002F93CC
		private void SpawnCups()
		{
			this._chestBone = this.ResolveChestBone();
			this._cups = Object.Instantiate<GameObject>(this._config.cupsPrefab, this._chestBone, false);
			this._cups.name = this._config.cupsPrefab.name;
			this._cupsExpected = true;
			this._attachment = this._cups.GetComponent<NipplePiercingAttachment>();
			this._visibility = this._cups.GetComponent<NipplePiercingVisibility>();
			if (this._attachment != null)
			{
				this._attachment.UpdatePose();
			}
			Transform transform = ((this._attachment != null && this._attachment.LeftAnchor != null) ? this._attachment.LeftAnchor : NippleRotorController.FindDeep(this._cups.transform, "LeftAnchor"));
			Transform transform2 = ((this._attachment != null && this._attachment.RightAnchor != null) ? this._attachment.RightAnchor : NippleRotorController.FindDeep(this._cups.transform, "RightAnchor"));
			this._leftBuzz = NippleRotorController.FindBuzz(transform);
			this._rightBuzz = NippleRotorController.FindBuzz(transform2);
			this._leftSocket = NippleRotorController.FindDeep(transform, "CordSocket");
			this._rightSocket = NippleRotorController.FindDeep(transform2, "CordSocket");
		}

		// Token: 0x060075CD RID: 30157 RVA: 0x002FB324 File Offset: 0x002F9524
		private void SpawnRemote()
		{
			this._remote = Object.Instantiate<GameObject>(this._config.remotePrefab, base.transform, false);
			this._remote.name = this._config.remotePrefab.name;
			this._remoteExpected = true;
			FirstPersonVisibleProp.Mark(this._remote);
			NippleRotorController.SetLayerRecursive(this._remote.transform, this._rigLayer);
			this._remoteBuzz = NippleRotorController.FindBuzz(this._remote.transform);
			this._grip = this._remote.GetComponent<NippleRotorRemoteGrip>();
			if (this._grip == null)
			{
				this._grip = this._remote.AddComponent<NippleRotorRemoteGrip>();
			}
			this._grip.Configure(this._animator, this._config.grip ?? new NippleRotorGripSettings());
		}

		// Token: 0x060075CE RID: 30158 RVA: 0x002FB3FC File Offset: 0x002F95FC
		private void SpawnCord()
		{
			Transform transform = ((this._grip != null && this._grip.CordSocket != null) ? this._grip.CordSocket : NippleRotorController.FindDeep(this._remote.transform, "CordSocket"));
			if (this._leftSocket == null || this._rightSocket == null || transform == null)
			{
				Debug.LogWarning("[NippleRotor] Cord sockets not found on the cups or the remote; the cord is skipped.", this);
				return;
			}
			this._cordObject = new GameObject("NippleRotorCord");
			this._cordObject.layer = this._rigLayer;
			this._cordObject.transform.SetParent(base.transform, false);
			this._cord = this._cordObject.AddComponent<NippleRotorCord>();
			this._cord.Configure(this._config.cord ?? new NippleRotorCordSettings(), this._config.cordMaterial, this._leftSocket, this._rightSocket, transform, this._playerRoot, this._attachment, this._animator);
			this.BindBreastSurface();
			NippleRotorController.SetLayerRecursive(this._cordObject.transform, this._rigLayer);
			this._cord.SetHidden(this.CupsCovered);
		}

		// Token: 0x060075CF RID: 30159 RVA: 0x002FB540 File Offset: 0x002F9740
		private void BindBreastSurface()
		{
			if (this._cord == null)
			{
				return;
			}
			SkinnedMeshRenderer skinnedMeshRenderer = ((this._attachment != null) ? this._attachment.Body : null);
			bool flag = this._config != null && this._config.breastSurface != null && skinnedMeshRenderer != null && this._breastSurface.Bind(this._config.breastSurface, skinnedMeshRenderer);
			if (!flag)
			{
				this._breastSurface.Unbind();
			}
			this._cord.SkinSurface = (flag ? this._breastSurface : null);
			if (flag || this._config == null || this._config.breastSurface == null || skinnedMeshRenderer == null || this._warnedBreastSurface)
			{
				return;
			}
			this._warnedBreastSurface = true;
			Debug.LogWarning("[NippleRotor] The breast skin samples do not match the body \"" + skinnedMeshRenderer.name + "\"; the cable uses the approximate breast spheres.", this);
		}

		// Token: 0x060075D0 RID: 30160 RVA: 0x002FB63C File Offset: 0x002F983C
		private void DespawnRig()
		{
			if (this._cordObject != null)
			{
				NippleRotorController.DestroyRuntime(this._cordObject);
			}
			if (this._grip != null)
			{
				this._grip.Release();
			}
			if (this._remote != null)
			{
				NippleRotorController.DestroyRuntime(this._remote);
			}
			if (this._cups != null)
			{
				NippleRotorController.DestroyRuntime(this._cups);
			}
			this._cordObject = null;
			this._cord = null;
			this._breastSurface.Unbind();
			this._grip = null;
			this._remote = null;
			this._remoteBuzz = null;
			this._cups = null;
			this._attachment = null;
			this._visibility = null;
			this._chestBone = null;
			this._leftBuzz = (this._rightBuzz = null);
			this._leftSocket = (this._rightSocket = null);
			this._cupsExpected = (this._remoteExpected = false);
			this._buzzApplied = false;
		}

		// Token: 0x060075D1 RID: 30161 RVA: 0x002FB730 File Offset: 0x002F9930
		private Transform ResolveChestBone()
		{
			bool flag = this._animator != null && this._animator.isHuman;
			Transform transform = (flag ? this._animator.GetBoneTransform(HumanBodyBones.Hips) : null);
			Transform transform2 = ((transform != null) ? NippleRotorController.FindDeep(transform, "spine_02.x") : null);
			if (transform2 == null)
			{
				transform2 = NippleRotorController.FindDeep(this._playerRoot, "spine_02.x");
			}
			if (transform2 == null && flag)
			{
				transform2 = this._animator.GetBoneTransform(HumanBodyBones.Chest);
			}
			if (!(transform2 != null))
			{
				return this._playerRoot;
			}
			return transform2;
		}

		// Token: 0x060075D2 RID: 30162 RVA: 0x002FB7C8 File Offset: 0x002F99C8
		private int ResolveRigLayer()
		{
			SkinnedMeshRenderer skinnedMeshRenderer = ((this._attachment != null) ? this._attachment.Body : null);
			if (skinnedMeshRenderer == null)
			{
				skinnedMeshRenderer = this.FindBodyRenderer();
			}
			if (skinnedMeshRenderer != null)
			{
				return skinnedMeshRenderer.gameObject.layer;
			}
			int num = LayerMask.NameToLayer("PlayerClothing");
			if (num >= 0)
			{
				return num;
			}
			if (!(this._playerRoot != null))
			{
				return base.gameObject.layer;
			}
			return this._playerRoot.gameObject.layer;
		}

		// Token: 0x060075D3 RID: 30163 RVA: 0x002FB854 File Offset: 0x002F9A54
		private SkinnedMeshRenderer FindBodyRenderer()
		{
			if (this._playerRoot == null)
			{
				return null;
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this._playerRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (skinnedMeshRenderer != null && skinnedMeshRenderer.name == "Body_sotai_NSFW" && skinnedMeshRenderer.gameObject.activeSelf)
				{
					return skinnedMeshRenderer;
				}
			}
			return null;
		}

		// Token: 0x060075D4 RID: 30164 RVA: 0x002FB8BC File Offset: 0x002F9ABC
		private static NippleRotorBuzz FindBuzz(Transform root)
		{
			if (root == null)
			{
				return null;
			}
			Transform transform = NippleRotorController.FindDeep(root, "Buzz");
			NippleRotorBuzz nippleRotorBuzz = ((transform != null) ? transform.GetComponent<NippleRotorBuzz>() : null);
			if (!(nippleRotorBuzz != null))
			{
				return root.GetComponentInChildren<NippleRotorBuzz>(true);
			}
			return nippleRotorBuzz;
		}

		// Token: 0x060075D5 RID: 30165 RVA: 0x002FB908 File Offset: 0x002F9B08
		private static Transform FindDeep(Transform root, string objectName)
		{
			if (root == null)
			{
				return null;
			}
			if (root.name == objectName)
			{
				return root;
			}
			for (int i = 0; i < root.childCount; i++)
			{
				Transform transform = NippleRotorController.FindDeep(root.GetChild(i), objectName);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x060075D6 RID: 30166 RVA: 0x002FB95C File Offset: 0x002F9B5C
		private static void SetLayerRecursive(Transform root, int layer)
		{
			if (root == null)
			{
				return;
			}
			root.gameObject.layer = layer;
			for (int i = 0; i < root.childCount; i++)
			{
				NippleRotorController.SetLayerRecursive(root.GetChild(i), layer);
			}
		}

		// Token: 0x060075D7 RID: 30167 RVA: 0x000A4FF8 File Offset: 0x000A31F8
		private static void DestroyRuntime(Object target)
		{
			if (target == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(target);
				return;
			}
			Object.DestroyImmediate(target);
		}

		// Token: 0x060075D8 RID: 30168 RVA: 0x002FB9A0 File Offset: 0x002F9BA0
		private void UpdateBuzz(bool vibrating)
		{
			NippleRotorLevel nippleRotorLevel = (vibrating ? this._config.GetLevel(this._level) : default(NippleRotorLevel));
			float num = (vibrating ? Mathf.Max(0f, nippleRotorLevel.jitterAmplitude) : 0f);
			float num2 = (vibrating ? nippleRotorLevel.jitterHz : 0f);
			float num3 = (vibrating ? nippleRotorLevel.wobbleDegrees : 0f);
			if (this._buzzApplied && num == this._appliedAmplitude && num2 == this._appliedHz && num3 == this._appliedWobble)
			{
				return;
			}
			this._buzzApplied = true;
			this._appliedAmplitude = num;
			this._appliedHz = num2;
			this._appliedWobble = num3;
			if (this._leftBuzz != null)
			{
				this._leftBuzz.SetIntensity(num, num2, num3);
			}
			if (this._rightBuzz != null)
			{
				this._rightBuzz.SetIntensity(num, num2, num3);
			}
			float num4 = Mathf.Max(0f, this._config.remoteBuzzScale);
			if (this._remoteBuzz != null)
			{
				this._remoteBuzz.SetIntensity(num * num4, num2, num3 * num4);
			}
		}

		// Token: 0x060075D9 RID: 30169 RVA: 0x002FBABC File Offset: 0x002F9CBC
		private void Shake()
		{
			if (this._config == null || this._config.levelChangeShake <= 0f || this._playerRoot == null)
			{
				return;
			}
			AnimeBodyPhysics.TryShake(this._playerRoot, Vector3.up, false, this._config.levelChangeShake);
		}

		// Token: 0x060075DA RID: 30170 RVA: 0x002FBB18 File Offset: 0x002F9D18
		private void ResolveFace()
		{
			NippleRotorFaceDriver component = base.GetComponent<NippleRotorFaceDriver>();
			if (component != this._face)
			{
				this._faceConfiguredFor = null;
			}
			this._face = component;
		}

		// Token: 0x060075DB RID: 30171 RVA: 0x002FBB48 File Offset: 0x002F9D48
		private void UpdateFace(bool wanted)
		{
			if (this._face == null)
			{
				return;
			}
			if (this._animator != null && this._faceConfiguredFor != this._animator)
			{
				this._face.Configure(this._animator);
				this._faceConfiguredFor = this._animator;
			}
			this._face.SetWanted(wanted);
		}

		// Token: 0x060075DC RID: 30172 RVA: 0x002FBBB0 File Offset: 0x002F9DB0
		private void ResolveActor()
		{
			StealthActor stealthActor = ((this._playerRoot != null) ? this._playerRoot.GetComponentInChildren<StealthActor>(true) : null);
			if (stealthActor == null && SensoryManager.Instance != null)
			{
				stealthActor = SensoryManager.Instance.GetPlayer();
			}
			if (stealthActor == this._actor)
			{
				return;
			}
			if (this._actor != null)
			{
				this._actor.SetExternalNoise(this, 0f);
			}
			this._actor = stealthActor;
		}

		// Token: 0x060075DD RID: 30173 RVA: 0x002FBC31 File Offset: 0x002F9E31
		private void UpdateNoise(bool vibrating)
		{
			if (this._actor == null)
			{
				return;
			}
			this._actor.SetExternalNoise(this, vibrating ? NippleRotorConfigSO.NoiseRadiusFor(this._config, this._level) : 0f);
		}

		// Token: 0x060075DE RID: 30174 RVA: 0x002FBC69 File Offset: 0x002F9E69
		private void ClearNoise()
		{
			if (this._actor != null)
			{
				this._actor.SetExternalNoise(this, 0f);
			}
		}

		// Token: 0x060075DF RID: 30175 RVA: 0x002FBC8C File Offset: 0x002F9E8C
		private void UpdateNpcTargets(bool vibrating)
		{
			if (!vibrating || this._config.npcRefreshHz <= 0f)
			{
				this._npcRefreshTimer = 0f;
				return;
			}
			this._npcRefreshTimer -= Time.deltaTime;
			if (this._npcRefreshTimer > 0f)
			{
				return;
			}
			this._npcRefreshTimer = 1f / this._config.npcRefreshHz;
			this.RefreshNpcTargets();
		}

		// Token: 0x060075E0 RID: 30176 RVA: 0x002FBCF8 File Offset: 0x002F9EF8
		private void RefreshNpcTargets()
		{
			SensoryManager instance = SensoryManager.Instance;
			if (instance == null || this._actor == null || instance.GetPlayer() != this._actor)
			{
				return;
			}
			if (DetectionInteractionGate.IsGameOver || DetectionTransitionGate.IsBlocked || instance.IsPaused || NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f || !instance.IsRunModeDetectionEnabled)
			{
				return;
			}
			IReadOnlyList<NPCSenses> registeredNPCs = instance.GetRegisteredNPCs();
			if (registeredNPCs == null)
			{
				return;
			}
			Vector3 position = this._actor.Position;
			Vector3 position2 = this._actor.transform.position;
			for (int i = 0; i < registeredNPCs.Count; i++)
			{
				NPCSenses npcsenses = registeredNPCs[i];
				if (!(npcsenses == null) && !npcsenses.IsInCooldown && npcsenses.HasAudioContact)
				{
					npcsenses.LastKnownPlayerPosition = position;
					NPCStateMachine npcstateMachine;
					if (npcsenses.IsSearching && npcsenses.TryGetComponent<NPCStateMachine>(out npcstateMachine))
					{
						NPCFSMContext context = npcstateMachine.Context;
						if (context != null)
						{
							context.LastSeenPlayerPosition = position2;
						}
					}
				}
			}
		}

		// Token: 0x060075E1 RID: 30177 RVA: 0x002FBDFC File Offset: 0x002F9FFC
		private void RefreshPiercingHiding()
		{
			if (!this._worn)
			{
				this.RestorePiercings();
				return;
			}
			if (this._style == null && this._playerRoot != null)
			{
				this._style = this._playerRoot.GetComponentInChildren<MCStyleCustomizationService>(true);
			}
			GameObject gameObject = ((this._style != null) ? this._style.GetSpawnedRoot("Accessory.NipplePiercing") : null);
			if (gameObject != this._piercingRoot)
			{
				this.RestorePiercings();
				this._piercingRoot = gameObject;
			}
			if (gameObject == null)
			{
				return;
			}
			gameObject.GetComponentsInChildren<Renderer>(true, this._rendererScratch);
			for (int i = 0; i < this._rendererScratch.Count; i++)
			{
				Renderer renderer = this._rendererScratch[i];
				if (!(renderer == null) && renderer.enabled)
				{
					renderer.enabled = false;
					if (!this._hiddenPiercings.Contains(renderer))
					{
						this._hiddenPiercings.Add(renderer);
					}
				}
			}
			this._rendererScratch.Clear();
		}

		// Token: 0x060075E2 RID: 30178 RVA: 0x002FBEFC File Offset: 0x002FA0FC
		private void RestorePiercings()
		{
			for (int i = 0; i < this._hiddenPiercings.Count; i++)
			{
				if (this._hiddenPiercings[i] != null)
				{
					this._hiddenPiercings[i].enabled = true;
				}
			}
			this._hiddenPiercings.Clear();
			this._piercingRoot = null;
		}

		// Token: 0x060075E3 RID: 30179 RVA: 0x002FBF58 File Offset: 0x002FA158
		private void UpdateAudio(bool vibrating, float unscaledDeltaTime)
		{
			AudioClip audioClip = (vibrating ? this.ResolveLoopClip(this._level, this.CupsCovered) : null);
			if (audioClip == null && !this._audioActive)
			{
				return;
			}
			this.EnsureAudio();
			for (int i = 0; i < this._voices.Length; i++)
			{
				if (this._voices[i].source == null)
				{
					return;
				}
			}
			if (audioClip != null)
			{
				if (this._voices[this._activeVoice].source.clip != audioClip)
				{
					this.StartVoice(audioClip);
				}
				else
				{
					this.ResumeVoice(this._activeVoice);
				}
				NippleRotorLevel level = this._config.GetLevel(this._level);
				float num = (this.CupsCovered ? Mathf.Clamp01(this._config.coveredVolumeScale) : 1f);
				this._voices[this._activeVoice].volume = Mathf.Clamp01(level.volume) * num;
				this._voices[this._activeVoice].pitch = ((level.pitch > 0f) ? level.pitch : 1f);
			}
			float audioFadeSeconds = this._config.audioFadeSeconds;
			float num2 = ((audioFadeSeconds > 0.0001f) ? (unscaledDeltaTime / audioFadeSeconds) : 1f);
			bool flag = this._worn && this._level > 0 && this.IsSuspended;
			bool flag2 = false;
			for (int j = 0; j < this._voices.Length; j++)
			{
				AudioSource source = this._voices[j].source;
				float num3 = ((audioClip != null && j == this._activeVoice) ? 1f : 0f);
				this._voices[j].gain = Mathf.MoveTowards(this._voices[j].gain, num3, num2);
				source.volume = this._voices[j].gain * this._voices[j].volume;
				source.pitch = this._voices[j].pitch;
				if (this._voices[j].gain > 0f || num3 > 0f)
				{
					flag2 = true;
				}
				else if (flag && j == this._activeVoice)
				{
					if (source.isPlaying)
					{
						source.Pause();
						this._voices[j].paused = true;
					}
					flag2 |= this._voices[j].paused;
				}
				else if (source.isPlaying || this._voices[j].paused)
				{
					source.Stop();
					this._voices[j].paused = false;
				}
			}
			this._audioActive = flag2;
			this.PlaceAudio();
		}

		// Token: 0x060075E4 RID: 30180 RVA: 0x002FC250 File Offset: 0x002FA450
		private AudioClip ResolveLoopClip(int level, bool covered)
		{
			if (level <= 0 || this._config == null)
			{
				return null;
			}
			int num = level - 1;
			AudioClip audioClip = NippleRotorController.PickClip(covered ? this._config.insideLoops : this._config.outsideLoops, num);
			if (!(audioClip != null))
			{
				return NippleRotorController.PickClip(covered ? this._config.outsideLoops : this._config.insideLoops, num);
			}
			return audioClip;
		}

		// Token: 0x060075E5 RID: 30181 RVA: 0x002FC2C3 File Offset: 0x002FA4C3
		private static AudioClip PickClip(AudioClip[] clips, int index)
		{
			if (clips == null || index < 0 || index >= clips.Length)
			{
				return null;
			}
			return clips[index];
		}

		// Token: 0x060075E6 RID: 30182 RVA: 0x002FC2D8 File Offset: 0x002FA4D8
		private void StartVoice(AudioClip clip)
		{
			int num = -1;
			int num2 = 0;
			while (num2 < this._voices.Length && num < 0)
			{
				if (num2 != this._activeVoice && this._voices[num2].source.clip == clip && (this._voices[num2].source.isPlaying || this._voices[num2].paused))
				{
					num = num2;
				}
				num2++;
			}
			bool flag = num >= 0;
			if (!flag)
			{
				float num3 = float.MaxValue;
				for (int i = 0; i < this._voices.Length; i++)
				{
					if (i != this._activeVoice && this._voices[i].gain < num3)
					{
						num3 = this._voices[i].gain;
						num = i;
					}
				}
			}
			ref NippleRotorController.LoopVoice ptr = ref this._voices[this._activeVoice];
			ref NippleRotorController.LoopVoice ptr2 = ref this._voices[num];
			if (flag)
			{
				if (ptr2.paused)
				{
					ptr2.source.UnPause();
				}
				ptr2.paused = false;
			}
			else
			{
				AudioSource source = ptr.source;
				float num4 = ((source.clip != null && (source.isPlaying || ptr.paused)) ? source.time : 0f);
				ptr2.source.Stop();
				ptr2.paused = false;
				ptr2.gain = 0f;
				ptr2.source.clip = clip;
				ptr2.source.loop = true;
				this.ApplySpatial(ptr2.source);
				if (clip.length > 0f && clip.loadState == AudioDataLoadState.Loaded)
				{
					ptr2.source.time = Mathf.Repeat(num4, clip.length);
				}
				GameAudioRouter.Route(ptr2.source, GameAudioRouter.Channel.Sfx);
				ptr2.source.Play();
			}
			this._activeVoice = num;
		}

		// Token: 0x060075E7 RID: 30183 RVA: 0x002FC4C4 File Offset: 0x002FA6C4
		private void ResumeVoice(int index)
		{
			AudioSource source = this._voices[index].source;
			if (this._voices[index].paused)
			{
				source.UnPause();
				this._voices[index].paused = false;
			}
			if (!source.isPlaying)
			{
				this.ApplySpatial(source);
				source.Play();
			}
		}

		// Token: 0x060075E8 RID: 30184 RVA: 0x002FC524 File Offset: 0x002FA724
		private void StopAudioImmediate()
		{
			for (int i = 0; i < this._voices.Length; i++)
			{
				AudioSource source = this._voices[i].source;
				if (source != null)
				{
					source.Stop();
					source.volume = 0f;
				}
				this._voices[i].gain = 0f;
				this._voices[i].paused = false;
			}
			this._audioActive = false;
		}

		// Token: 0x060075E9 RID: 30185 RVA: 0x002FC5A0 File Offset: 0x002FA7A0
		private void PlaySwitch()
		{
			if (this._config == null || this._config.switchClip == null || !base.isActiveAndEnabled)
			{
				return;
			}
			this.EnsureAudio();
			if (this._switchSource == null)
			{
				return;
			}
			this.ApplySpatial(this._switchSource);
			this.PlaceAudio();
			this._switchSource.PlayOneShot(this._config.switchClip, this._config.switchVolume);
		}

		// Token: 0x060075EA RID: 30186 RVA: 0x002FC620 File Offset: 0x002FA820
		private void EnsureAudio()
		{
			if (this._voices[0].source == null)
			{
				this._voices[0] = new NippleRotorController.LoopVoice
				{
					source = this.CreateSource("NippleRotorBuzzAudio", true),
					pitch = 1f
				};
			}
			if (this._voices[1].source == null)
			{
				this._voices[1] = new NippleRotorController.LoopVoice
				{
					source = this.CreateSource("NippleRotorBuzzAudio2", true),
					pitch = 1f
				};
			}
			if (this._voices[2].source == null)
			{
				this._voices[2] = new NippleRotorController.LoopVoice
				{
					source = this.CreateSource("NippleRotorBuzzAudio3", true),
					pitch = 1f
				};
			}
			if (this._switchSource == null)
			{
				this._switchSource = this.CreateSource("NippleRotorSwitchAudio", false);
			}
		}

		// Token: 0x060075EB RID: 30187 RVA: 0x002FC734 File Offset: 0x002FA934
		private AudioSource CreateSource(string objectName, bool loop)
		{
			Transform transform = base.transform.Find(objectName);
			GameObject gameObject = ((transform != null) ? transform.gameObject : new GameObject(objectName));
			if (transform == null)
			{
				gameObject.transform.SetParent(base.transform, false);
			}
			AudioSource audioSource = gameObject.GetComponent<AudioSource>();
			if (audioSource == null)
			{
				audioSource = gameObject.AddComponent<AudioSource>();
			}
			audioSource.playOnAwake = false;
			audioSource.loop = loop;
			audioSource.dopplerLevel = 0f;
			audioSource.volume = (loop ? 0f : 1f);
			this.ApplySpatial(audioSource);
			GameAudioRouter.Route(audioSource, GameAudioRouter.Channel.Sfx);
			return audioSource;
		}

		// Token: 0x060075EC RID: 30188 RVA: 0x002FC7D8 File Offset: 0x002FA9D8
		private void ApplySpatial(AudioSource source)
		{
			if (source == null || this._config == null)
			{
				return;
			}
			float num = Mathf.Max(0.01f, this._config.audioMinDistance);
			source.spatialBlend = Mathf.Clamp01(this._config.audioSpatialBlend);
			source.rolloffMode = AudioRolloffMode.Linear;
			source.minDistance = num;
			source.maxDistance = Mathf.Max(num + 0.01f, this._config.audioMaxDistance);
			source.dopplerLevel = 0f;
		}

		// Token: 0x060075ED RID: 30189 RVA: 0x002FC860 File Offset: 0x002FAA60
		private void PlaceAudio()
		{
			Vector3 vector;
			if (this._attachment != null && this._attachment.LeftAnchor != null && this._attachment.RightAnchor != null)
			{
				vector = (this._attachment.LeftAnchor.position + this._attachment.RightAnchor.position) * 0.5f;
			}
			else if (this._chestBone != null)
			{
				vector = this._chestBone.position;
			}
			else
			{
				vector = base.transform.position;
			}
			for (int i = 0; i < this._voices.Length; i++)
			{
				if (this._voices[i].source != null)
				{
					this._voices[i].source.transform.position = vector;
				}
			}
			if (this._switchSource != null)
			{
				this._switchSource.transform.position = ((this._remote != null) ? this._remote.transform.position : vector);
			}
		}

		// Token: 0x04007898 RID: 30872
		public const string BuzzAudioObjectName = "NippleRotorBuzzAudio";

		// Token: 0x04007899 RID: 30873
		public const string BuzzCrossfadeAudioObjectName = "NippleRotorBuzzAudio2";

		// Token: 0x0400789A RID: 30874
		public const string BuzzThirdAudioObjectName = "NippleRotorBuzzAudio3";

		// Token: 0x0400789B RID: 30875
		public const string SwitchAudioObjectName = "NippleRotorSwitchAudio";

		// Token: 0x0400789C RID: 30876
		public const string CordObjectName = "NippleRotorCord";

		// Token: 0x0400789D RID: 30877
		private const string ChestBoneName = "spine_02.x";

		// Token: 0x0400789E RID: 30878
		private const string BodyRendererName = "Body_sotai_NSFW";

		// Token: 0x0400789F RID: 30879
		private const string ClothingLayerName = "PlayerClothing";

		// Token: 0x040078A0 RID: 30880
		private const string NipplePiercingPartId = "Accessory.NipplePiercing";

		// Token: 0x040078A1 RID: 30881
		private const float HousekeepingInterval = 0.5f;

		// Token: 0x040078A2 RID: 30882
		private static readonly List<NippleRotorController> EnabledControllers = new List<NippleRotorController>(2);

		// Token: 0x040078A4 RID: 30884
		private NippleRotorConfigSO _config;

		// Token: 0x040078A5 RID: 30885
		private PlayerInventoryManager _inventory;

		// Token: 0x040078A6 RID: 30886
		private Transform _playerRoot;

		// Token: 0x040078A7 RID: 30887
		private Animator _animator;

		// Token: 0x040078A8 RID: 30888
		private WearableUseController _wear;

		// Token: 0x040078A9 RID: 30889
		private StealthActor _actor;

		// Token: 0x040078AA RID: 30890
		private MCStyleCustomizationService _style;

		// Token: 0x040078AB RID: 30891
		private NippleRotorFaceDriver _face;

		// Token: 0x040078AC RID: 30892
		private Animator _faceConfiguredFor;

		// Token: 0x040078AD RID: 30893
		private bool _worn;

		// Token: 0x040078AE RID: 30894
		private int _level;

		// Token: 0x040078AF RID: 30895
		private GameObject _cups;

		// Token: 0x040078B0 RID: 30896
		private NipplePiercingAttachment _attachment;

		// Token: 0x040078B1 RID: 30897
		private NipplePiercingVisibility _visibility;

		// Token: 0x040078B2 RID: 30898
		private Transform _chestBone;

		// Token: 0x040078B3 RID: 30899
		private NippleRotorBuzz _leftBuzz;

		// Token: 0x040078B4 RID: 30900
		private NippleRotorBuzz _rightBuzz;

		// Token: 0x040078B5 RID: 30901
		private NippleRotorBuzz _remoteBuzz;

		// Token: 0x040078B6 RID: 30902
		private Transform _leftSocket;

		// Token: 0x040078B7 RID: 30903
		private Transform _rightSocket;

		// Token: 0x040078B8 RID: 30904
		private GameObject _remote;

		// Token: 0x040078B9 RID: 30905
		private NippleRotorRemoteGrip _grip;

		// Token: 0x040078BA RID: 30906
		private GameObject _cordObject;

		// Token: 0x040078BB RID: 30907
		private NippleRotorCord _cord;

		// Token: 0x040078BC RID: 30908
		private readonly NippleRotorBreastSurface _breastSurface = new NippleRotorBreastSurface();

		// Token: 0x040078BD RID: 30909
		private bool _warnedBreastSurface;

		// Token: 0x040078BE RID: 30910
		private int _rigLayer;

		// Token: 0x040078BF RID: 30911
		private bool _cupsExpected;

		// Token: 0x040078C0 RID: 30912
		private bool _remoteExpected;

		// Token: 0x040078C1 RID: 30913
		private bool _warnedMissingAssets;

		// Token: 0x040078C2 RID: 30914
		private bool _buzzApplied;

		// Token: 0x040078C3 RID: 30915
		private float _appliedAmplitude;

		// Token: 0x040078C4 RID: 30916
		private float _appliedHz;

		// Token: 0x040078C5 RID: 30917
		private float _appliedWobble;

		// Token: 0x040078C6 RID: 30918
		private readonly NippleRotorController.LoopVoice[] _voices = new NippleRotorController.LoopVoice[3];

		// Token: 0x040078C7 RID: 30919
		private int _activeVoice;

		// Token: 0x040078C8 RID: 30920
		private bool _audioActive;

		// Token: 0x040078C9 RID: 30921
		private AudioSource _switchSource;

		// Token: 0x040078CA RID: 30922
		private GameObject _piercingRoot;

		// Token: 0x040078CB RID: 30923
		private readonly List<Renderer> _hiddenPiercings = new List<Renderer>(8);

		// Token: 0x040078CC RID: 30924
		private readonly List<Renderer> _rendererScratch = new List<Renderer>(8);

		// Token: 0x040078CD RID: 30925
		private float _housekeepingTimer;

		// Token: 0x040078CE RID: 30926
		private float _npcRefreshTimer;

		// Token: 0x02000F56 RID: 3926
		private struct LoopVoice
		{
			// Token: 0x040078CF RID: 30927
			public AudioSource source;

			// Token: 0x040078D0 RID: 30928
			public float gain;

			// Token: 0x040078D1 RID: 30929
			public float volume;

			// Token: 0x040078D2 RID: 30930
			public float pitch;

			// Token: 0x040078D3 RID: 30931
			public bool paused;
		}
	}
}
