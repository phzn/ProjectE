using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using UnityEngine;

namespace BoZo.CharacterCreator.MC
{
	// Token: 0x020007B8 RID: 1976
	[DefaultExecutionOrder(20001)]
	public class ClothingBlendShapeSyncController : MonoBehaviour
	{
		// Token: 0x06003456 RID: 13398 RVA: 0x0012AA58 File Offset: 0x00128C58
		public static void BeginExternalGarmentDrive()
		{
			ClothingBlendShapeSyncController._activeExternalDrivers++;
		}

		// Token: 0x06003457 RID: 13399 RVA: 0x0012AA66 File Offset: 0x00128C66
		public static void EndExternalGarmentDrive()
		{
			if (ClothingBlendShapeSyncController._activeExternalDrivers > 0)
			{
				ClothingBlendShapeSyncController._activeExternalDrivers--;
			}
		}

		// Token: 0x06003458 RID: 13400 RVA: 0x0012AA7C File Offset: 0x00128C7C
		private void Start()
		{
			this.RebuildSyncMaps();
			this.TrySubscribe();
			this.TrySubscribeService();
		}

		// Token: 0x06003459 RID: 13401 RVA: 0x0012AA90 File Offset: 0x00128C90
		private void OnEnable()
		{
			this.TrySubscribe();
			this.TrySubscribeService();
		}

		// Token: 0x0600345A RID: 13402 RVA: 0x0012AA9E File Offset: 0x00128C9E
		private void OnDisable()
		{
			this.Unsubscribe();
			this.UnsubscribeService();
		}

		// Token: 0x0600345B RID: 13403 RVA: 0x0012AAAC File Offset: 0x00128CAC
		private void LateUpdate()
		{
			if (this._pendingVisibilityResync)
			{
				this._pendingVisibilityResync = false;
				if (this.isDirty)
				{
					this.RebuildSyncMaps();
				}
				this.SyncAll();
			}
			bool flag = ClothingBlendShapeSyncController._activeExternalDrivers > 0;
			if (flag || this._drovLastFrame)
			{
				this._drovLastFrame = flag;
				if (this.isDirty)
				{
					this.RebuildSyncMaps();
				}
				this.SyncAll();
				return;
			}
			if (this.eventDriven && this._subscribed)
			{
				return;
			}
			if (!this.autoSync)
			{
				return;
			}
			if (this.isDirty)
			{
				this.RebuildSyncMaps();
			}
			this.SyncAll();
		}

		// Token: 0x0600345C RID: 13404 RVA: 0x0012AB3C File Offset: 0x00128D3C
		private void TrySubscribe()
		{
			if (!this.eventDriven || this._subscribed)
			{
				return;
			}
			if (this._manager == null)
			{
				this._manager = BlendShapeManager.ResolveForRig(this);
			}
			if (this._manager != null)
			{
				this._manager.OnBlendShapesApplied += this.HandleBlendShapesApplied;
				this._subscribed = true;
			}
		}

		// Token: 0x0600345D RID: 13405 RVA: 0x0012ABA0 File Offset: 0x00128DA0
		private void Unsubscribe()
		{
			if (this._manager != null && this._subscribed)
			{
				this._manager.OnBlendShapesApplied -= this.HandleBlendShapesApplied;
			}
			this._subscribed = false;
		}

		// Token: 0x0600345E RID: 13406 RVA: 0x0012ABD6 File Offset: 0x00128DD6
		private void HandleBlendShapesApplied()
		{
			if (this.isDirty)
			{
				this.RebuildSyncMaps();
			}
			this.SyncAll();
		}

		// Token: 0x0600345F RID: 13407 RVA: 0x0012ABEC File Offset: 0x00128DEC
		private void TrySubscribeService()
		{
			if (this._serviceSubscribed)
			{
				return;
			}
			if (this.styleService == null)
			{
				this.styleService = base.GetComponentInParent<MCStyleCustomizationService>(true);
			}
			if (this.styleService != null)
			{
				this.styleService.OnPhysicalSwapCompleted += this.HandlePhysicalSwapCompleted;
				this.styleService.OnPhysicalRemoved += this.HandlePhysicalRemoved;
				this.styleService.OnSlotVisibilityChanged += this.HandleSlotVisibilityChanged;
				this._serviceSubscribed = true;
				this.RegisterExistingSpawns();
			}
		}

		// Token: 0x06003460 RID: 13408 RVA: 0x0012AC80 File Offset: 0x00128E80
		private void UnsubscribeService()
		{
			if (this.styleService != null && this._serviceSubscribed)
			{
				this.styleService.OnPhysicalSwapCompleted -= this.HandlePhysicalSwapCompleted;
				this.styleService.OnPhysicalRemoved -= this.HandlePhysicalRemoved;
				this.styleService.OnSlotVisibilityChanged -= this.HandleSlotVisibilityChanged;
			}
			this._serviceSubscribed = false;
		}

		// Token: 0x06003461 RID: 13409 RVA: 0x0012ACEF File Offset: 0x00128EEF
		private void HandleSlotVisibilityChanged()
		{
			this._pendingVisibilityResync = true;
		}

		// Token: 0x06003462 RID: 13410 RVA: 0x0012ACEF File Offset: 0x00128EEF
		public void RequestResync()
		{
			this._pendingVisibilityResync = true;
		}

		// Token: 0x06003463 RID: 13411 RVA: 0x0012ACF8 File Offset: 0x00128EF8
		private void HandlePhysicalSwapCompleted(MCStylePartDefinitionSO definition, GameObject go)
		{
			this.RegisterClothingHierarchy(go);
		}

		// Token: 0x06003464 RID: 13412 RVA: 0x0012AD01 File Offset: 0x00128F01
		private void HandlePhysicalRemoved(MCStylePartDefinitionSO definition, GameObject go)
		{
			this.UnregisterClothingHierarchy(go);
		}

		// Token: 0x06003465 RID: 13413 RVA: 0x0012AD0C File Offset: 0x00128F0C
		private void RegisterExistingSpawns()
		{
			if (this.styleService == null)
			{
				return;
			}
			IReadOnlyList<MCStylePartDefinitionSO> partDefinitions = this.styleService.PartDefinitions;
			if (partDefinitions == null)
			{
				return;
			}
			for (int i = 0; i < partDefinitions.Count; i++)
			{
				if (!(partDefinitions[i] == null) && !string.IsNullOrWhiteSpace(partDefinitions[i].partId))
				{
					GameObject spawnedRoot = this.styleService.GetSpawnedRoot(partDefinitions[i].partId);
					if (spawnedRoot != null)
					{
						this.RegisterClothingHierarchy(spawnedRoot);
					}
				}
			}
		}

		// Token: 0x06003466 RID: 13414 RVA: 0x0012AD93 File Offset: 0x00128F93
		public void RegisterClothingRenderer(SkinnedMeshRenderer renderer)
		{
			if (renderer == null || this.clothingRenderers.Contains(renderer))
			{
				return;
			}
			this.clothingRenderers.Add(renderer);
			this.isDirty = true;
			this.RebuildSyncMaps();
			this.SyncAll();
		}

		// Token: 0x06003467 RID: 13415 RVA: 0x0012ADCC File Offset: 0x00128FCC
		public void RegisterClothingHierarchy(GameObject outfitRoot)
		{
			if (outfitRoot == null)
			{
				return;
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in outfitRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (skinnedMeshRenderer != null && !this.clothingRenderers.Contains(skinnedMeshRenderer))
				{
					this.clothingRenderers.Add(skinnedMeshRenderer);
				}
			}
			this.isDirty = true;
			this.RebuildSyncMaps();
			this.SyncAll();
		}

		// Token: 0x06003468 RID: 13416 RVA: 0x0012AE33 File Offset: 0x00129033
		public void UnregisterClothingRenderer(SkinnedMeshRenderer renderer)
		{
			if (renderer == null)
			{
				return;
			}
			this.clothingRenderers.Remove(renderer);
			this.headSyncMap.Remove(renderer);
			this.bodySyncMap.Remove(renderer);
		}

		// Token: 0x06003469 RID: 13417 RVA: 0x0012AE68 File Offset: 0x00129068
		public void UnregisterClothingHierarchy(GameObject outfitRoot)
		{
			if (outfitRoot == null)
			{
				return;
			}
			SkinnedMeshRenderer[] componentsInChildren = outfitRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				this.UnregisterClothingRenderer(componentsInChildren[i]);
			}
		}

		// Token: 0x0600346A RID: 13418 RVA: 0x0012AEA0 File Offset: 0x001290A0
		public void RebuildSyncMaps()
		{
			this.headSyncMap.Clear();
			this.bodySyncMap.Clear();
			for (int i = this.clothingRenderers.Count - 1; i >= 0; i--)
			{
				if (this.clothingRenderers[i] == null)
				{
					this.clothingRenderers.RemoveAt(i);
				}
				else
				{
					this.BuildMapForRenderer(this.headRenderer, this.clothingRenderers[i], this.headSyncMap);
					this.BuildMapForRenderer(this.bodyRenderer, this.clothingRenderers[i], this.bodySyncMap);
				}
			}
			this.isDirty = false;
		}

		// Token: 0x0600346B RID: 13419 RVA: 0x0012AF40 File Offset: 0x00129140
		private void BuildMapForRenderer(SkinnedMeshRenderer source, SkinnedMeshRenderer target, Dictionary<SkinnedMeshRenderer, Dictionary<int, int>> mapDict)
		{
			if (source == null || source.sharedMesh == null)
			{
				return;
			}
			if (target == null || target.sharedMesh == null)
			{
				return;
			}
			Mesh sharedMesh = source.sharedMesh;
			Mesh sharedMesh2 = target.sharedMesh;
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (int i = 0; i < sharedMesh.blendShapeCount; i++)
			{
				string blendShapeName = sharedMesh.GetBlendShapeName(i);
				int blendShapeIndex = sharedMesh2.GetBlendShapeIndex(blendShapeName);
				if (blendShapeIndex >= 0)
				{
					dictionary[i] = blendShapeIndex;
				}
			}
			if (dictionary.Count > 0)
			{
				mapDict[target] = dictionary;
			}
		}

		// Token: 0x0600346C RID: 13420 RVA: 0x0012AFD4 File Offset: 0x001291D4
		public void SyncAll()
		{
			this.SyncFromSource(this.headRenderer, this.headSyncMap);
			this.SyncFromSource(this.bodyRenderer, this.bodySyncMap);
		}

		// Token: 0x0600346D RID: 13421 RVA: 0x0012ABD6 File Offset: 0x00128DD6
		public void SyncNow()
		{
			if (this.isDirty)
			{
				this.RebuildSyncMaps();
			}
			this.SyncAll();
		}

		// Token: 0x0600346E RID: 13422 RVA: 0x0012AFFC File Offset: 0x001291FC
		private void SyncFromSource(SkinnedMeshRenderer source, Dictionary<SkinnedMeshRenderer, Dictionary<int, int>> mapDict)
		{
			if (source == null)
			{
				return;
			}
			foreach (KeyValuePair<SkinnedMeshRenderer, Dictionary<int, int>> keyValuePair in mapDict)
			{
				SkinnedMeshRenderer key = keyValuePair.Key;
				if (key == null)
				{
					this.isDirty = true;
				}
				else
				{
					foreach (KeyValuePair<int, int> keyValuePair2 in keyValuePair.Value)
					{
						float blendShapeWeight = source.GetBlendShapeWeight(keyValuePair2.Key);
						key.SetBlendShapeWeight(keyValuePair2.Value, blendShapeWeight);
					}
				}
			}
		}

		// Token: 0x0600346F RID: 13423 RVA: 0x0012B0C4 File Offset: 0x001292C4
		public void ClearAll()
		{
			this.clothingRenderers.Clear();
			this.headSyncMap.Clear();
			this.bodySyncMap.Clear();
			this.isDirty = false;
		}

		// Token: 0x040034BC RID: 13500
		[Header("Source Renderers (MC Body)")]
		[Tooltip("The head SkinnedMeshRenderer whose blend shapes are the source of truth.")]
		[SerializeField]
		private SkinnedMeshRenderer headRenderer;

		// Token: 0x040034BD RID: 13501
		[Tooltip("The body SkinnedMeshRenderer whose blend shapes are the source of truth.")]
		[SerializeField]
		private SkinnedMeshRenderer bodyRenderer;

		// Token: 0x040034BE RID: 13502
		[Header("Clothing Targets")]
		[Tooltip("All clothing SkinnedMeshRenderers to sync blend shapes to. Update this list when clothing items are equipped/unequipped.")]
		[SerializeField]
		private List<SkinnedMeshRenderer> clothingRenderers = new List<SkinnedMeshRenderer>();

		// Token: 0x040034BF RID: 13503
		[Header("Settings")]
		[Tooltip("Event-driven (consigliato): sincronizza gli outfit SOLO quando i blend shape del corpo cambiano davvero (sottoscrizione a BlendShapeManager.OnBlendShapesApplied), niente polling per-frame. Se il manager non è risolvibile, fa fallback al polling.")]
		[SerializeField]
		private bool eventDriven = true;

		// Token: 0x040034C0 RID: 13504
		[Tooltip("If true (e event-driven off, o nessun manager), sync runs every LateUpdate.")]
		[SerializeField]
		private bool autoSync = true;

		// Token: 0x040034C1 RID: 13505
		private Dictionary<SkinnedMeshRenderer, Dictionary<int, int>> headSyncMap = new Dictionary<SkinnedMeshRenderer, Dictionary<int, int>>();

		// Token: 0x040034C2 RID: 13506
		private Dictionary<SkinnedMeshRenderer, Dictionary<int, int>> bodySyncMap = new Dictionary<SkinnedMeshRenderer, Dictionary<int, int>>();

		// Token: 0x040034C3 RID: 13507
		private bool isDirty = true;

		// Token: 0x040034C4 RID: 13508
		private bool _pendingVisibilityResync;

		// Token: 0x040034C5 RID: 13509
		private static int _activeExternalDrivers;

		// Token: 0x040034C6 RID: 13510
		[Header("Outfit auto-registration")]
		[Tooltip("Se valorizzato (o risolvibile dal rig), il controller registra/deregistra automaticamente gli outfit fisici spawnati dal service via i suoi eventi.")]
		[SerializeField]
		private MCStyleCustomizationService styleService;

		// Token: 0x040034C7 RID: 13511
		private BlendShapeManager _manager;

		// Token: 0x040034C8 RID: 13512
		private bool _subscribed;

		// Token: 0x040034C9 RID: 13513
		private bool _serviceSubscribed;

		// Token: 0x040034CA RID: 13514
		private bool _drovLastFrame;
	}
}
