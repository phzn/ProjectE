using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using MagicaCloth2;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectExhibitionist.Futanari
{
	// Token: 0x020009DF RID: 2527
	[ExecuteAlways]
	[DisallowMultipleComponent]
	public class FutanariModeController : MonoBehaviour
	{
		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x0600475D RID: 18269 RVA: 0x001B0890 File Offset: 0x001AEA90
		public bool FutanariMode
		{
			get
			{
				return this.futanariMode;
			}
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x0600475E RID: 18270 RVA: 0x001B0898 File Offset: 0x001AEA98
		private bool EffectiveVisible
		{
			get
			{
				return base.enabled && this.futanariMode && !this._externallyOccluded && !this._clothingOccluded;
			}
		}

		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x0600475F RID: 18271 RVA: 0x001B08BD File Offset: 0x001AEABD
		public bool IsClothSimulationRequested
		{
			get
			{
				return this._clothSimulationRequested && this._clothPauseOwners.Count == 0;
			}
		}

		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x06004760 RID: 18272 RVA: 0x001B08D7 File Offset: 0x001AEAD7
		public bool IsEffectivelyVisible
		{
			get
			{
				return this.EffectiveVisible;
			}
		}

		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x06004761 RID: 18273 RVA: 0x001B08DF File Offset: 0x001AEADF
		public IReadOnlyList<ColliderComponent> ActiveFutanariColliders
		{
			get
			{
				return this.futanariColliders;
			}
		}

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x06004762 RID: 18274 RVA: 0x001B08E7 File Offset: 0x001AEAE7
		public IReadOnlyCollection<ColliderComponent> GarmentCollisionColliders
		{
			get
			{
				return this._garmentCollisionColliders;
			}
		}

		// Token: 0x06004763 RID: 18275 RVA: 0x001B08EF File Offset: 0x001AEAEF
		private void Awake()
		{
			this.InitializeGarmentColliderSet();
			this.ResolveStyleService();
			this.RefreshClothingOcclusion();
			this.ApplyState(this.futanariMode, false);
			this._lastAppliedState = this.futanariMode;
		}

		// Token: 0x06004764 RID: 18276 RVA: 0x001B091C File Offset: 0x001AEB1C
		private void OnEnable()
		{
			this.InitializeGarmentColliderSet();
			this.ResolveStyleService();
			this.AlignRenderersLayer();
			this.SubscribeStyle();
			this._clothPauseOwners.RemoveWhere((Object owner) => owner == null);
			this.RefreshClothingOcclusion();
			this.ApplyState(this.futanariMode, false);
			if (Application.isPlaying)
			{
				this.InitializeShaftColliderFit();
				MagicaManager.OnPreSimulation = (Action)Delegate.Combine(MagicaManager.OnPreSimulation, new Action(this.PrepareShaftCollider));
				MagicaManager.OnPostSimulation = (Action)Delegate.Combine(MagicaManager.OnPostSimulation, new Action(this.CaptureShaftCollider));
			}
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x001B09CC File Offset: 0x001AEBCC
		private void OnDisable()
		{
			this.UnsubscribeStyle();
			MagicaManager.OnPreSimulation = (Action)Delegate.Remove(MagicaManager.OnPreSimulation, new Action(this.PrepareShaftCollider));
			MagicaManager.OnPostSimulation = (Action)Delegate.Remove(MagicaManager.OnPostSimulation, new Action(this.CaptureShaftCollider));
			this.ApplyState(false, false);
			FutanariShaftColliderFit shaftColliderFit = this._shaftColliderFit;
			if (shaftColliderFit != null)
			{
				shaftColliderFit.Dispose();
			}
			this._shaftColliderFit = null;
			this.RestoreInternalShaftCollider();
			FutanariGarmentContact garmentContact = this._garmentContact;
			if (garmentContact != null)
			{
				garmentContact.Dispose();
			}
			this._garmentContact = null;
			this._shaftCloth = null;
			this._erectionController = null;
			this._stableGarmentSupport = false;
			this._supportedPoseFrame = -1;
		}

		// Token: 0x06004766 RID: 18278 RVA: 0x001B0A7C File Offset: 0x001AEC7C
		private void InitializeShaftColliderFit()
		{
			if (this._shaftColliderFit != null)
			{
				return;
			}
			foreach (ColliderComponent colliderComponent in this.futanariColliders)
			{
				MagicaCapsuleCollider magicaCapsuleCollider = colliderComponent as MagicaCapsuleCollider;
				if (magicaCapsuleCollider != null)
				{
					foreach (MagicaCloth magicaCloth in this.futanariCloths)
					{
						if (!(magicaCloth == null) && magicaCloth.SerializeData.rootBones.Contains(magicaCapsuleCollider.transform.parent))
						{
							foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.futanariRenderers)
							{
								this._shaftColliderFit = FutanariShaftColliderFit.TryCreate(magicaCapsuleCollider, skinnedMeshRenderer);
								if (this._shaftColliderFit != null)
								{
									this.SeparateInternalShaftCollider(magicaCapsuleCollider);
									this._garmentContact = new FutanariGarmentContact(magicaCapsuleCollider.transform.parent, skinnedMeshRenderer);
									this._shaftCloth = magicaCloth;
									this._erectionController = base.GetComponent<PenisErectionController>();
									this._shaftColliderFit.CapturePose();
									return;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06004767 RID: 18279 RVA: 0x001B0C08 File Offset: 0x001AEE08
		private void SeparateInternalShaftCollider(MagicaCapsuleCollider capsule)
		{
			this._internalShaftCloths.Clear();
			foreach (MagicaCloth magicaCloth in this.futanariCloths)
			{
				List<ColliderComponent> list;
				if (!(magicaCloth != null))
				{
					list = null;
				}
				else
				{
					ClothSerializeData serializeData = magicaCloth.SerializeData;
					if (serializeData == null)
					{
						list = null;
					}
					else
					{
						ColliderCollisionConstraint.SerializeData colliderCollisionConstraint = serializeData.colliderCollisionConstraint;
						list = ((colliderCollisionConstraint != null) ? colliderCollisionConstraint.colliderList : null);
					}
				}
				List<ColliderComponent> list2 = list;
				if (list2 != null && list2.Contains(capsule))
				{
					this._internalShaftCloths.Add(magicaCloth);
				}
			}
			if (this._internalShaftCloths.Count == 0)
			{
				return;
			}
			GameObject gameObject = new GameObject("Magica Capsule Collider (Internal Futa Shaft)");
			gameObject.SetActive(false);
			gameObject.hideFlags = HideFlags.DontSave;
			gameObject.layer = capsule.gameObject.layer;
			gameObject.transform.SetParent(capsule.transform.parent, false);
			gameObject.transform.SetLocalPositionAndRotation(capsule.transform.localPosition, capsule.transform.localRotation);
			gameObject.transform.localScale = capsule.transform.localScale;
			this._internalShaftCollider = gameObject.AddComponent<MagicaCapsuleCollider>();
			this._internalShaftCollider.center = capsule.center;
			this._internalShaftCollider.SetSize(capsule.GetSize());
			this._internalShaftCollider.direction = capsule.direction;
			this._internalShaftCollider.reverseDirection = capsule.reverseDirection;
			this._internalShaftCollider.radiusSeparation = capsule.radiusSeparation;
			this._internalShaftCollider.alignedOnCenter = capsule.alignedOnCenter;
			this._internalShaftCollider.symmetryMode = capsule.symmetryMode;
			this._internalShaftCollider.symmetryTarget = capsule.symmetryTarget;
			this._internalShaftCollider.enabled = capsule.enabled;
			this._fittedShaftCollider = capsule;
			this.futanariColliders.Add(this._internalShaftCollider);
			this.garmentColliderExclusions.Add(this._internalShaftCollider);
			gameObject.SetActive(capsule.gameObject.activeSelf);
			foreach (MagicaCloth magicaCloth2 in this._internalShaftCloths)
			{
				FutanariModeController.ReplaceCollider(magicaCloth2, capsule, this._internalShaftCollider);
			}
		}

		// Token: 0x06004768 RID: 18280 RVA: 0x001B0E50 File Offset: 0x001AF050
		private void RestoreInternalShaftCollider()
		{
			if (this._internalShaftCollider != null)
			{
				foreach (MagicaCloth magicaCloth in this._internalShaftCloths)
				{
					FutanariModeController.ReplaceCollider(magicaCloth, this._internalShaftCollider, this._fittedShaftCollider);
				}
				this.futanariColliders.Remove(this._internalShaftCollider);
				this.garmentColliderExclusions.Remove(this._internalShaftCollider);
				this._garmentCollisionColliders.Remove(this._internalShaftCollider);
				this._internalShaftCollider.gameObject.SetActive(false);
				if (Application.isPlaying)
				{
					Object.Destroy(this._internalShaftCollider.gameObject);
				}
				else
				{
					Object.DestroyImmediate(this._internalShaftCollider.gameObject);
				}
			}
			this._internalShaftCloths.Clear();
			this._internalShaftCollider = null;
			this._fittedShaftCollider = null;
		}

		// Token: 0x06004769 RID: 18281 RVA: 0x001B0F48 File Offset: 0x001AF148
		private static void ReplaceCollider(MagicaCloth cloth, ColliderComponent previous, ColliderComponent replacement)
		{
			List<ColliderComponent> list;
			if (!(cloth != null))
			{
				list = null;
			}
			else
			{
				ClothSerializeData serializeData = cloth.SerializeData;
				if (serializeData == null)
				{
					list = null;
				}
				else
				{
					ColliderCollisionConstraint.SerializeData colliderCollisionConstraint = serializeData.colliderCollisionConstraint;
					list = ((colliderCollisionConstraint != null) ? colliderCollisionConstraint.colliderList : null);
				}
			}
			List<ColliderComponent> list2 = list;
			if (list2 == null)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < list2.Count; i++)
			{
				if (!(list2[i] != previous))
				{
					list2[i] = replacement;
					flag = true;
				}
			}
			if (flag)
			{
				cloth.SetParameterChange();
			}
		}

		// Token: 0x0600476A RID: 18282 RVA: 0x001B0FBC File Offset: 0x001AF1BC
		private void PrepareShaftCollider()
		{
			this.RefreshGarmentSupport();
			if (this._stableGarmentSupport && this.EffectiveVisible && this.IsClothSimulationRequested)
			{
				if (this._supportedPoseFrame == Time.frameCount)
				{
					return;
				}
				this._supportedPoseFrame = Time.frameCount;
				FutanariGarmentContact garmentContact = this._garmentContact;
				if (garmentContact != null)
				{
					garmentContact.PrepareSimulation();
				}
				FutanariGarmentContact garmentContact2 = this._garmentContact;
				if (garmentContact2 != null)
				{
					garmentContact2.ApplySupported((this._erectionController != null) ? this._erectionController.ErectionProgress : 0f);
				}
				FutanariShaftColliderFit shaftColliderFit = this._shaftColliderFit;
				if (shaftColliderFit == null)
				{
					return;
				}
				shaftColliderFit.CapturePose();
				return;
			}
			else
			{
				this._supportedPoseFrame = -1;
				if (this.EffectiveVisible && this.IsClothSimulationRequested && this._shaftCloth != null && this._shaftCloth.isActiveAndEnabled)
				{
					FutanariGarmentContact garmentContact3 = this._garmentContact;
					if (garmentContact3 != null)
					{
						garmentContact3.PrepareSimulation();
					}
				}
				else
				{
					FutanariGarmentContact garmentContact4 = this._garmentContact;
					if (garmentContact4 != null)
					{
						garmentContact4.Prepare();
					}
				}
				if (!this.EffectiveVisible)
				{
					return;
				}
				if (this._shaftCloth == null || !this._shaftCloth.isActiveAndEnabled)
				{
					FutanariShaftColliderFit shaftColliderFit2 = this._shaftColliderFit;
					if (shaftColliderFit2 == null)
					{
						return;
					}
					shaftColliderFit2.CapturePose();
					return;
				}
				else
				{
					FutanariShaftColliderFit shaftColliderFit3 = this._shaftColliderFit;
					if (shaftColliderFit3 == null)
					{
						return;
					}
					shaftColliderFit3.ApplyPose();
					return;
				}
			}
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x001B10F0 File Offset: 0x001AF2F0
		private void CaptureShaftCollider()
		{
			if (!this._stableGarmentSupport || !this.EffectiveVisible || !this.IsClothSimulationRequested)
			{
				FutanariGarmentContact garmentContact = this._garmentContact;
				if (garmentContact != null)
				{
					garmentContact.Apply(this.EffectiveVisible && this.IsClothSimulationRequested);
				}
			}
			if (this.EffectiveVisible)
			{
				FutanariShaftColliderFit shaftColliderFit = this._shaftColliderFit;
				if (shaftColliderFit == null)
				{
					return;
				}
				shaftColliderFit.CapturePose();
			}
		}

		// Token: 0x0600476C RID: 18284 RVA: 0x001B1150 File Offset: 0x001AF350
		private void LateUpdate()
		{
			if (Application.isPlaying)
			{
				this.RefreshClothingOcclusion();
				if (this._clothPauseOwners.RemoveWhere((Object owner) => owner == null) > 0)
				{
					this.ApplyClothState();
				}
			}
			if (Application.isPlaying && !this._stableGarmentSupport && (this._shaftCloth == null || !this._shaftCloth.isActiveAndEnabled))
			{
				this.PrepareShaftCollider();
			}
		}

		// Token: 0x0600476D RID: 18285 RVA: 0x001B11D0 File Offset: 0x001AF3D0
		private void AlignRenderersLayer()
		{
			int num = ((this.styleService != null) ? this.styleService.gameObject.layer : base.gameObject.layer);
			for (int i = 0; i < this.futanariRenderers.Count; i++)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = this.futanariRenderers[i];
				if (skinnedMeshRenderer != null && skinnedMeshRenderer.gameObject.layer != num)
				{
					skinnedMeshRenderer.gameObject.layer = num;
				}
			}
		}

		// Token: 0x0600476E RID: 18286 RVA: 0x001B124F File Offset: 0x001AF44F
		private void OnValidate()
		{
			this._garmentColliderSetInitialized = false;
			this.InitializeGarmentColliderSet();
			if (this.futanariMode != this._lastAppliedState)
			{
				this.ApplyState(this.futanariMode, Application.isPlaying);
				this._lastAppliedState = this.futanariMode;
			}
		}

		// Token: 0x0600476F RID: 18287 RVA: 0x001B128C File Offset: 0x001AF48C
		private void ResolveStyleService()
		{
			if (this.styleService != null)
			{
				return;
			}
			this.styleService = base.GetComponentInParent<MCStyleCustomizationService>();
			if (this.styleService == null && base.transform.root != null)
			{
				this.styleService = base.transform.root.GetComponentInChildren<MCStyleCustomizationService>(true);
			}
		}

		// Token: 0x06004770 RID: 18288 RVA: 0x001B12EC File Offset: 0x001AF4EC
		private void SubscribeStyle()
		{
			if (this.styleService != null && !this._styleSubscribed)
			{
				this.styleService.OnActiveStateChanged += this.HandleWardrobeChanged;
				this.styleService.OnSlotVisibilityChanged += this.HandleWardrobeChanged;
				this._styleSubscribed = true;
			}
		}

		// Token: 0x06004771 RID: 18289 RVA: 0x001B1344 File Offset: 0x001AF544
		private void UnsubscribeStyle()
		{
			if (this.styleService != null && this._styleSubscribed)
			{
				this.styleService.OnActiveStateChanged -= this.HandleWardrobeChanged;
				this.styleService.OnSlotVisibilityChanged -= this.HandleWardrobeChanged;
				this._styleSubscribed = false;
			}
		}

		// Token: 0x06004772 RID: 18290 RVA: 0x001B139C File Offset: 0x001AF59C
		private void HandleWardrobeChanged()
		{
			this._garmentRenderers.Clear();
			this._registeredGarmentRoots.Clear();
			this.RefreshClothingOcclusion();
		}

		// Token: 0x06004773 RID: 18291 RVA: 0x001B13BC File Offset: 0x001AF5BC
		public void RefreshClothingOcclusion()
		{
			FutanariGarmentContact garmentContact = this._garmentContact;
			if (garmentContact != null)
			{
				garmentContact.BeginWardrobe();
			}
			if (this.styleService == null)
			{
				this.ResolveStyleService();
				this.SubscribeStyle();
			}
			bool flag = false;
			this._compatibleGarmentCloths.RemoveWhere((MagicaCloth cloth) => cloth == null);
			if (this.styleService != null)
			{
				IReadOnlyList<MCStylePartDefinitionSO> partDefinitions = this.styleService.PartDefinitions;
				if (partDefinitions == null)
				{
					this.SetClothingOccluded(false);
					return;
				}
				for (int i = 0; i < partDefinitions.Count; i++)
				{
					MCStylePartDefinitionSO mcstylePartDefinitionSO = partDefinitions[i];
					if (!(mcstylePartDefinitionSO == null) && !string.IsNullOrWhiteSpace(mcstylePartDefinitionSO.partId))
					{
						string partId = mcstylePartDefinitionSO.partId;
						string text;
						if (this.styleService.TryGetEquippedItem(partId, out text))
						{
							GameObject spawnedRoot = this.styleService.GetSpawnedRoot(partId);
							if (this.HasVisibleGarmentRenderer(spawnedRoot))
							{
								MCStylePresetSO activePreset = this.styleService.GetActivePreset(partId);
								MCFutanariClothingMode mcfutanariClothingMode = ((activePreset != null) ? activePreset.futanariClothingMode : MCFutanariClothingMode.Automatic);
								if (mcfutanariClothingMode == MCFutanariClothingMode.KeepVisibleAndCollide)
								{
									if (activePreset != null && activePreset.bendFutanariOnContact)
									{
										FutanariGarmentContact garmentContact2 = this._garmentContact;
										if (garmentContact2 != null)
										{
											garmentContact2.AddGarment(spawnedRoot);
										}
									}
									this.RegisterCollidersOnGarment(spawnedRoot);
								}
								else if (this.IsGarmentOcclusionReleased(spawnedRoot) || this.IsGarmentOpen(spawnedRoot, (activePreset != null) ? activePreset.futanariOpeningShapes : null))
								{
									this.RegisterCollidersOnGarment(spawnedRoot);
								}
								else if (mcfutanariClothingMode == MCFutanariClothingMode.Hide || (mcfutanariClothingMode == MCFutanariClothingMode.Automatic && this.IsAutomaticOccluderSlot(partId)))
								{
									flag = true;
								}
							}
						}
					}
				}
			}
			this.SetClothingOccluded(flag);
			this.RefreshGarmentSupport();
		}

		// Token: 0x06004774 RID: 18292 RVA: 0x001B156C File Offset: 0x001AF76C
		private void RefreshGarmentSupport()
		{
			bool flag = this._garmentContact != null && this._garmentContact.HasStableSupport;
			if (this._stableGarmentSupport != flag)
			{
				if (!flag)
				{
					FutanariGarmentContact garmentContact = this._garmentContact;
					if (garmentContact != null)
					{
						garmentContact.Release();
					}
				}
				this._stableGarmentSupport = flag;
				this._supportedPoseFrame = -1;
				this.ApplyClothState();
				if (!flag && this._shaftCloth != null && this._shaftCloth.isActiveAndEnabled)
				{
					this._shaftCloth.AddForce(Vector3.zero, 0f, ClothForceMode.VelocityAdd);
					this._shaftCloth.ResetCloth(false);
				}
			}
		}

		// Token: 0x06004775 RID: 18293 RVA: 0x001B1604 File Offset: 0x001AF804
		private bool IsGarmentOpen(GameObject root, string[] openingShapes)
		{
			Renderer[] array;
			if (root == null || openingShapes == null || openingShapes.Length == 0 || !this._garmentRenderers.TryGetValue(root, out array))
			{
				return false;
			}
			Renderer[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = array2[i] as SkinnedMeshRenderer;
				if (skinnedMeshRenderer != null && skinnedMeshRenderer.enabled && skinnedMeshRenderer.gameObject.activeInHierarchy && !(skinnedMeshRenderer.sharedMesh == null))
				{
					foreach (string text in openingShapes)
					{
						if (!string.IsNullOrEmpty(text))
						{
							int blendShapeIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(text);
							if (blendShapeIndex >= 0 && skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex) > 0.01f)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06004776 RID: 18294 RVA: 0x001B16C1 File Offset: 0x001AF8C1
		private void SetClothingOccluded(bool occluded)
		{
			if (this._clothingOccluded == occluded)
			{
				return;
			}
			this._clothingOccluded = occluded;
			this.ApplyState(this.futanariMode, false);
		}

		// Token: 0x06004777 RID: 18295 RVA: 0x001B16E1 File Offset: 0x001AF8E1
		public void SetGarmentOcclusionReleased(Object owner, GameObject garment, bool released)
		{
			if (owner == null)
			{
				return;
			}
			if (released && owner != null && garment != null)
			{
				this._garmentExposureOwners[owner] = garment;
			}
			else
			{
				this._garmentExposureOwners.Remove(owner);
			}
			this.RefreshClothingOcclusion();
		}

		// Token: 0x06004778 RID: 18296 RVA: 0x001B1720 File Offset: 0x001AF920
		public bool IsGarmentOcclusionReleased(GameObject garment)
		{
			if (garment == null)
			{
				return false;
			}
			bool flag = false;
			this._expiredExposureOwners.Clear();
			foreach (KeyValuePair<Object, GameObject> keyValuePair in this._garmentExposureOwners)
			{
				if (keyValuePair.Key == null || keyValuePair.Value == null)
				{
					this._expiredExposureOwners.Add(keyValuePair.Key);
				}
				else if (keyValuePair.Value == garment)
				{
					flag = true;
				}
			}
			foreach (Object @object in this._expiredExposureOwners)
			{
				this._garmentExposureOwners.Remove(@object);
			}
			return flag;
		}

		// Token: 0x06004779 RID: 18297 RVA: 0x001B1814 File Offset: 0x001AFA14
		private bool IsAutomaticOccluderSlot(string slotId)
		{
			if (this.occluderSlotIds == null || string.IsNullOrWhiteSpace(slotId))
			{
				return false;
			}
			for (int i = 0; i < this.occluderSlotIds.Count; i++)
			{
				if (string.Equals(this.occluderSlotIds[i], slotId, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600477A RID: 18298 RVA: 0x001B1864 File Offset: 0x001AFA64
		private bool HasVisibleGarmentRenderer(GameObject root)
		{
			if (root == null || !root.activeInHierarchy)
			{
				return false;
			}
			Renderer[] componentsInChildren;
			if (!this._garmentRenderers.TryGetValue(root, out componentsInChildren))
			{
				componentsInChildren = root.GetComponentsInChildren<Renderer>(true);
				this._garmentRenderers[root] = componentsInChildren;
			}
			foreach (Renderer renderer in componentsInChildren)
			{
				if (renderer != null && renderer.enabled && renderer.gameObject.activeInHierarchy)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600477B RID: 18299 RVA: 0x001B18DC File Offset: 0x001AFADC
		private void InitializeGarmentColliderSet()
		{
			if (this._garmentColliderSetInitialized)
			{
				return;
			}
			this._garmentCollisionColliders.Clear();
			if (this.futanariColliders != null)
			{
				for (int i = 0; i < this.futanariColliders.Count; i++)
				{
					ColliderComponent colliderComponent = this.futanariColliders[i];
					if (colliderComponent != null && (this.garmentColliderExclusions == null || !this.garmentColliderExclusions.Contains(colliderComponent)))
					{
						this._garmentCollisionColliders.Add(colliderComponent);
					}
				}
			}
			this._garmentColliderSetInitialized = true;
		}

		// Token: 0x0600477C RID: 18300 RVA: 0x001B195C File Offset: 0x001AFB5C
		private void RegisterCollidersOnGarment(GameObject garmentRoot)
		{
			if (garmentRoot == null)
			{
				return;
			}
			if (!this._registeredGarmentRoots.Add(garmentRoot))
			{
				return;
			}
			this.InitializeGarmentColliderSet();
			foreach (MagicaCloth magicaCloth in garmentRoot.GetComponentsInChildren<MagicaCloth>(true))
			{
				if (!(magicaCloth == null) && magicaCloth.gameObject.activeInHierarchy && magicaCloth.SerializeData != null && magicaCloth.SerializeData.colliderCollisionConstraint != null)
				{
					List<ColliderComponent> list = magicaCloth.SerializeData.colliderCollisionConstraint.colliderList;
					if (list == null)
					{
						list = new List<ColliderComponent>();
						magicaCloth.SerializeData.colliderCollisionConstraint.colliderList = list;
					}
					bool flag = magicaCloth.SerializeData.colliderCollisionConstraint.mode != ColliderCollisionConstraint.Mode.Edge;
					if (flag)
					{
						magicaCloth.SerializeData.colliderCollisionConstraint.mode = ColliderCollisionConstraint.Mode.Edge;
					}
					if (this.garmentColliderExclusions != null)
					{
						for (int j = 0; j < this.garmentColliderExclusions.Count; j++)
						{
							ColliderComponent colliderComponent = this.garmentColliderExclusions[j];
							if (colliderComponent != null)
							{
								while (list.Remove(colliderComponent))
								{
									flag = true;
								}
							}
						}
					}
					foreach (ColliderComponent colliderComponent2 in this._garmentCollisionColliders)
					{
						if (colliderComponent2 != null && !list.Contains(colliderComponent2))
						{
							list.Add(colliderComponent2);
							flag = true;
						}
					}
					this._compatibleGarmentCloths.Add(magicaCloth);
					if (flag)
					{
						magicaCloth.SetParameterChange();
					}
				}
			}
		}

		// Token: 0x0600477D RID: 18301 RVA: 0x001B1AF8 File Offset: 0x001AFCF8
		public void SetFutanariMode(bool value)
		{
			if (this.futanariMode == value)
			{
				this.RefreshClothingOcclusion();
				this.ApplyState(value, false);
				return;
			}
			this.futanariMode = value;
			this.RefreshClothingOcclusion();
			this.ApplyState(value, true);
			this._lastAppliedState = value;
		}

		// Token: 0x0600477E RID: 18302 RVA: 0x001B1B2E File Offset: 0x001AFD2E
		public void Toggle()
		{
			this.SetFutanariMode(!this.futanariMode);
		}

		// Token: 0x0600477F RID: 18303 RVA: 0x001B1B40 File Offset: 0x001AFD40
		public void SetExternallyOccluded(bool occluded)
		{
			if (this._externallyOccluded == occluded)
			{
				return;
			}
			this._externallyOccluded = occluded;
			this.ApplyState(this.futanariMode, false);
			if (this.debugLog)
			{
				Debug.Log(string.Format("[FutanariMode] Occlusione esterna = {0} (Futa={1}, visibile={2}).", occluded, this.futanariMode, this.EffectiveVisible));
			}
		}

		// Token: 0x06004780 RID: 18304 RVA: 0x001B1BA0 File Offset: 0x001AFDA0
		public void SetFlashExposureRequested(bool requested)
		{
			if (this._flashExposureRequested == requested)
			{
				return;
			}
			this._flashExposureRequested = requested;
			this.ApplyState(this.futanariMode, false);
			if (this.debugLog)
			{
				Debug.Log(string.Format("[FutanariMode] Esposizione flash = {0} ", requested) + string.Format("(occluso={0}, visibile={1}).", this._externallyOccluded, this.EffectiveVisible));
			}
		}

		// Token: 0x06004781 RID: 18305 RVA: 0x001B1C0D File Offset: 0x001AFE0D
		public void SetClothActive(bool active)
		{
			this._clothSimulationRequested = active;
			this.ApplyClothState();
			if (this.debugLog)
			{
				Debug.Log(string.Format("[FutanariMode] Cloth pause toggle = {0} (renderers invariati).", active));
			}
		}

		// Token: 0x06004782 RID: 18306 RVA: 0x001B1C39 File Offset: 0x001AFE39
		public void SetClothSuspended(Object owner, bool suspended)
		{
			if (owner == null)
			{
				return;
			}
			if (suspended && owner != null)
			{
				this._clothPauseOwners.Add(owner);
			}
			else
			{
				this._clothPauseOwners.Remove(owner);
			}
			this.ApplyClothState();
		}

		// Token: 0x06004783 RID: 18307 RVA: 0x001B1C70 File Offset: 0x001AFE70
		private void ApplyClothState()
		{
			if (!this.IsClothSimulationRequested || !this.EffectiveVisible)
			{
				FutanariGarmentContact garmentContact = this._garmentContact;
				if (garmentContact != null)
				{
					garmentContact.Release();
				}
				this._supportedPoseFrame = -1;
			}
			for (int i = 0; i < this.futanariCloths.Count; i++)
			{
				if (this.futanariCloths[i] != null)
				{
					this.futanariCloths[i].enabled = this.IsClothSimulationRequested && this.EffectiveVisible && (!this._stableGarmentSupport || !(this.futanariCloths[i] == this._shaftCloth));
				}
			}
		}

		// Token: 0x06004784 RID: 18308 RVA: 0x001B1D18 File Offset: 0x001AFF18
		private void ApplyState(bool active, bool fireEvent)
		{
			bool flag = base.enabled && active && !this._externallyOccluded && !this._clothingOccluded;
			if (!flag)
			{
				FutanariGarmentContact garmentContact = this._garmentContact;
				if (garmentContact != null)
				{
					garmentContact.Release();
				}
				this._supportedPoseFrame = -1;
			}
			if (flag)
			{
				for (int i = 0; i < this.futanariRoots.Count; i++)
				{
					if (this.futanariRoots[i] != null && !this.futanariRoots[i].activeSelf)
					{
						this.futanariRoots[i].SetActive(true);
					}
				}
			}
			for (int j = 0; j < this.futanariColliders.Count; j++)
			{
				if (this.futanariColliders[j] != null)
				{
					this.futanariColliders[j].enabled = flag;
				}
			}
			for (int k = 0; k < this.futanariRenderers.Count; k++)
			{
				if (this.futanariRenderers[k] != null)
				{
					this.futanariRenderers[k].enabled = flag;
				}
			}
			for (int l = 0; l < this.futanariCloths.Count; l++)
			{
				if (this.futanariCloths[l] != null)
				{
					this.futanariCloths[l].enabled = flag && this.IsClothSimulationRequested && (!this._stableGarmentSupport || !(this.futanariCloths[l] == this._shaftCloth));
				}
			}
			if (flag && !this._lastEffectiveVisible && Application.isPlaying)
			{
				foreach (MagicaCloth magicaCloth in this._compatibleGarmentCloths)
				{
					if (magicaCloth != null && magicaCloth.isActiveAndEnabled)
					{
						magicaCloth.SetParameterChange();
						magicaCloth.ResetCloth(false);
					}
				}
			}
			if (!flag)
			{
				for (int m = 0; m < this.futanariRoots.Count; m++)
				{
					if (this.futanariRoots[m] != null && this.futanariRoots[m].activeSelf)
					{
						this.futanariRoots[m].SetActive(false);
					}
				}
			}
			this._lastEffectiveVisible = flag;
			if (this.debugLog)
			{
				Debug.Log(string.Concat(new string[]
				{
					string.Format("[FutanariMode] Logico={0}, occluso={1}, ", active, this._externallyOccluded),
					string.Format("flash={0}, visibile={1}, ", this._flashExposureRequested, flag),
					string.Format("roots={0}, renderers={1}, ", this.futanariRoots.Count, this.futanariRenderers.Count),
					string.Format("cloths={0}, colliders={1}, ", this.futanariCloths.Count, this.futanariColliders.Count),
					string.Format("capiCompatibili={0}.", this._compatibleGarmentCloths.Count)
				}));
			}
			if (fireEvent)
			{
				UnityEvent<bool> onFutanariModeChanged = this.OnFutanariModeChanged;
				if (onFutanariModeChanged == null)
				{
					return;
				}
				onFutanariModeChanged.Invoke(active);
			}
		}

		// Token: 0x06004785 RID: 18309 RVA: 0x001B2058 File Offset: 0x001B0258
		[ContextMenu("Toggle Futanari Mode")]
		private void ContextToggle()
		{
			this.Toggle();
		}

		// Token: 0x04004706 RID: 18182
		[Header("Renderers")]
		[Tooltip("Root visivi/fisici della modalita' (nel prefab gameplay 'Penis' parte inattivo).")]
		[SerializeField]
		private List<GameObject> futanariRoots = new List<GameObject>();

		// Token: 0x04004707 RID: 18183
		[SerializeField]
		private List<SkinnedMeshRenderer> futanariRenderers = new List<SkinnedMeshRenderer>();

		// Token: 0x04004708 RID: 18184
		[Header("Physics")]
		[Tooltip("MagicaCloth da spegnere/accendere insieme alla modalita' (Sao_Physics, Tama_Physics).")]
		[SerializeField]
		private List<MagicaCloth> futanariCloths = new List<MagicaCloth>();

		// Token: 0x04004709 RID: 18185
		[Tooltip("Collider Magica che descrivono penis e testicoli. Restano disabilitati quando la modalita' e' spenta o il capo indossato li occulta.")]
		[SerializeField]
		private List<ColliderComponent> futanariColliders = new List<ColliderComponent>();

		// Token: 0x0400470A RID: 18186
		[Tooltip("Collider necessari alla fisica genitale interna ma da NON registrare sui vestiti. Le sfere dei testicoli, sommate al raggio particella del garment, creerebbero un rigonfiamento artificiale molto piu' grande del mesh.")]
		[SerializeField]
		private List<ColliderComponent> garmentColliderExclusions = new List<ColliderComponent>();

		// Token: 0x0400470B RID: 18187
		[Header("Clothing Occlusion")]
		[Tooltip("Gli item in modalita' Automatic dentro uno di questi slot nascondono il penis quando sono realmente visibili. Il singolo MCStylePreset puo' forzare Hide oppure KeepVisibleAndCollide.")]
		[SerializeField]
		private List<string> occluderSlotIds = new List<string> { "Outfit.Bottom", "Outfit.Underwear", "Outfit.Swimsuit" };

		// Token: 0x0400470C RID: 18188
		[Tooltip("Service degli stili MC da cui leggere lo stato indumenti. Se nullo, risolto sul modello.")]
		[SerializeField]
		private MCStyleCustomizationService styleService;

		// Token: 0x0400470D RID: 18189
		[Header("State")]
		[SerializeField]
		private bool futanariMode;

		// Token: 0x0400470E RID: 18190
		[Header("Events")]
		public UnityEvent<bool> OnFutanariModeChanged;

		// Token: 0x0400470F RID: 18191
		[Header("Debug")]
		[SerializeField]
		private bool debugLog;

		// Token: 0x04004710 RID: 18192
		private bool _lastAppliedState;

		// Token: 0x04004711 RID: 18193
		private bool _lastEffectiveVisible;

		// Token: 0x04004712 RID: 18194
		private bool _clothSimulationRequested = true;

		// Token: 0x04004713 RID: 18195
		private readonly HashSet<Object> _clothPauseOwners = new HashSet<Object>();

		// Token: 0x04004714 RID: 18196
		private readonly Dictionary<Object, GameObject> _garmentExposureOwners = new Dictionary<Object, GameObject>();

		// Token: 0x04004715 RID: 18197
		private readonly List<Object> _expiredExposureOwners = new List<Object>();

		// Token: 0x04004716 RID: 18198
		private readonly Dictionary<GameObject, Renderer[]> _garmentRenderers = new Dictionary<GameObject, Renderer[]>();

		// Token: 0x04004717 RID: 18199
		private readonly HashSet<GameObject> _registeredGarmentRoots = new HashSet<GameObject>();

		// Token: 0x04004718 RID: 18200
		private readonly HashSet<MagicaCloth> _compatibleGarmentCloths = new HashSet<MagicaCloth>();

		// Token: 0x04004719 RID: 18201
		private readonly HashSet<ColliderComponent> _garmentCollisionColliders = new HashSet<ColliderComponent>();

		// Token: 0x0400471A RID: 18202
		private bool _garmentColliderSetInitialized;

		// Token: 0x0400471B RID: 18203
		private FutanariShaftColliderFit _shaftColliderFit;

		// Token: 0x0400471C RID: 18204
		private MagicaCapsuleCollider _fittedShaftCollider;

		// Token: 0x0400471D RID: 18205
		private MagicaCapsuleCollider _internalShaftCollider;

		// Token: 0x0400471E RID: 18206
		private readonly List<MagicaCloth> _internalShaftCloths = new List<MagicaCloth>();

		// Token: 0x0400471F RID: 18207
		private MagicaCloth _shaftCloth;

		// Token: 0x04004720 RID: 18208
		private FutanariGarmentContact _garmentContact;

		// Token: 0x04004721 RID: 18209
		private PenisErectionController _erectionController;

		// Token: 0x04004722 RID: 18210
		private bool _stableGarmentSupport;

		// Token: 0x04004723 RID: 18211
		private int _supportedPoseFrame = -1;

		// Token: 0x04004724 RID: 18212
		private bool _externallyOccluded;

		// Token: 0x04004725 RID: 18213
		private bool _clothingOccluded;

		// Token: 0x04004726 RID: 18214
		private bool _flashExposureRequested;

		// Token: 0x04004727 RID: 18215
		private bool _styleSubscribed;
	}
}
