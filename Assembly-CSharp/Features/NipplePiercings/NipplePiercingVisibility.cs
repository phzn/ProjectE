using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Inventory.Wearable;
using UnityEngine;

namespace Features.NipplePiercings
{
	// Token: 0x02000F70 RID: 3952
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(31000)]
	public sealed class NipplePiercingVisibility : MonoBehaviour
	{
		// Token: 0x1700137B RID: 4987
		// (get) Token: 0x060076F0 RID: 30448 RVA: 0x00305CCA File Offset: 0x00303ECA
		// (set) Token: 0x060076F1 RID: 30449 RVA: 0x00305CD2 File Offset: 0x00303ED2
		public bool IsCovered { get; private set; }

		// Token: 0x060076F2 RID: 30450 RVA: 0x00305CDB File Offset: 0x00303EDB
		public void Configure(Renderer[] renderers)
		{
			this.RestoreRenderers();
			this.piercingRenderers = renderers ?? Array.Empty<Renderer>();
			this.RefreshVisibility();
		}

		// Token: 0x060076F3 RID: 30451 RVA: 0x00305CF9 File Offset: 0x00303EF9
		private void OnEnable()
		{
			this.RefreshVisibility();
		}

		// Token: 0x060076F4 RID: 30452 RVA: 0x00305CF9 File Offset: 0x00303EF9
		private void LateUpdate()
		{
			this.RefreshVisibility();
		}

		// Token: 0x060076F5 RID: 30453 RVA: 0x00305D01 File Offset: 0x00303F01
		private void OnDisable()
		{
			this.RestoreRenderers();
			this.UnbindStyle();
		}

		// Token: 0x060076F6 RID: 30454 RVA: 0x00305D0F File Offset: 0x00303F0F
		private void OnTransformParentChanged()
		{
			this.UnbindStyle();
			this.RefreshVisibility();
		}

		// Token: 0x060076F7 RID: 30455 RVA: 0x00305D20 File Offset: 0x00303F20
		public void RefreshVisibility()
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this._style == null || !base.transform.IsChildOf(this._style.transform))
			{
				this.BindStyle();
			}
			bool flag = this.IsChestCovered();
			this.IsCovered = flag;
			if (!flag)
			{
				this.RestoreRenderers();
				return;
			}
			if (this.piercingRenderers == null || this.piercingRenderers.Length == 0)
			{
				this.piercingRenderers = base.GetComponentsInChildren<Renderer>(true);
			}
			for (int i = 0; i < this.piercingRenderers.Length; i++)
			{
				Renderer renderer = this.piercingRenderers[i];
				if (!(renderer == null))
				{
					if (!this._previousRenderingOff.ContainsKey(renderer))
					{
						this._previousRenderingOff.Add(renderer, renderer.forceRenderingOff);
					}
					renderer.forceRenderingOff = true;
				}
			}
		}

		// Token: 0x060076F8 RID: 30456 RVA: 0x00305DE4 File Offset: 0x00303FE4
		private void BindStyle()
		{
			this.UnbindStyle();
			this._style = base.GetComponentInParent<MCStyleCustomizationService>(true);
			if (this._style == null)
			{
				return;
			}
			this._style.OnActiveStateChanged += this.InvalidateGarments;
			this._style.OnPreviewClothingVisibilityChanged += this.RefreshVisibility;
			this._style.OnPhysicalSwapCompleted += this.OnPhysicalSwap;
			this._style.OnPhysicalRemoved += this.OnPhysicalSwap;
		}

		// Token: 0x060076F9 RID: 30457 RVA: 0x00305E70 File Offset: 0x00304070
		private void UnbindStyle()
		{
			if (this._style != null)
			{
				this._style.OnActiveStateChanged -= this.InvalidateGarments;
				this._style.OnPreviewClothingVisibilityChanged -= this.RefreshVisibility;
				this._style.OnPhysicalSwapCompleted -= this.OnPhysicalSwap;
				this._style.OnPhysicalRemoved -= this.OnPhysicalSwap;
			}
			this._style = null;
			this._garments.Clear();
		}

		// Token: 0x060076FA RID: 30458 RVA: 0x00305EF9 File Offset: 0x003040F9
		private void InvalidateGarments()
		{
			this._garments.Clear();
		}

		// Token: 0x060076FB RID: 30459 RVA: 0x00305F06 File Offset: 0x00304106
		private void OnPhysicalSwap(MCStylePartDefinitionSO part, GameObject root)
		{
			this.InvalidateGarments();
		}

		// Token: 0x060076FC RID: 30460 RVA: 0x00305F10 File Offset: 0x00304110
		private bool IsChestCovered()
		{
			if (this._style == null)
			{
				return false;
			}
			IReadOnlyList<MCStylePartDefinitionSO> partDefinitions = this._style.PartDefinitions;
			for (int i = 0; i < partDefinitions.Count; i++)
			{
				MCStylePartDefinitionSO mcstylePartDefinitionSO = partDefinitions[i];
				if (!(mcstylePartDefinitionSO == null) && !string.IsNullOrEmpty(mcstylePartDefinitionSO.partId))
				{
					MCStylePresetSO activePreset = this._style.GetActivePreset(mcstylePartDefinitionSO.partId);
					if (!(activePreset == null))
					{
						WearableItemSO mappedWearable = CustomizationWardrobeDiscoveryPolicy.GetMappedWearable(activePreset);
						if (!(mappedWearable == null) && mappedWearable.physicallyCoversTop)
						{
							GameObject spawnedRoot = this._style.GetSpawnedRoot(mcstylePartDefinitionSO.partId);
							if (!(spawnedRoot == null) && spawnedRoot.activeInHierarchy && !this._style.IsPartHiddenForPreview(mcstylePartDefinitionSO.partId))
							{
								NipplePiercingVisibility.Garment garment;
								if (!this._garments.TryGetValue(mcstylePartDefinitionSO.partId, out garment) || garment.root != spawnedRoot || garment.preset != activePreset)
								{
									garment = new NipplePiercingVisibility.Garment(spawnedRoot, activePreset);
									this._garments[mcstylePartDefinitionSO.partId] = garment;
								}
								if (garment.CoversChest())
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060076FD RID: 30461 RVA: 0x0030604C File Offset: 0x0030424C
		private void RestoreRenderers()
		{
			foreach (KeyValuePair<Renderer, bool> keyValuePair in this._previousRenderingOff)
			{
				if (keyValuePair.Key != null)
				{
					keyValuePair.Key.forceRenderingOff = keyValuePair.Value;
				}
			}
			this._previousRenderingOff.Clear();
			this.IsCovered = false;
		}

		// Token: 0x04007A3E RID: 31294
		[SerializeField]
		private Renderer[] piercingRenderers = Array.Empty<Renderer>();

		// Token: 0x04007A3F RID: 31295
		private MCStyleCustomizationService _style;

		// Token: 0x04007A40 RID: 31296
		private readonly Dictionary<string, NipplePiercingVisibility.Garment> _garments = new Dictionary<string, NipplePiercingVisibility.Garment>();

		// Token: 0x04007A41 RID: 31297
		private readonly Dictionary<Renderer, bool> _previousRenderingOff = new Dictionary<Renderer, bool>();

		// Token: 0x02000F71 RID: 3953
		private sealed class Garment
		{
			// Token: 0x060076FF RID: 30463 RVA: 0x003060F5 File Offset: 0x003042F5
			public Garment(GameObject root, MCStylePresetSO preset)
			{
				this.root = root;
				this.preset = preset;
				this._renderers = root.GetComponentsInChildren<Renderer>(true);
				this._skinned = root.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			}

			// Token: 0x06007700 RID: 30464 RVA: 0x00306130 File Offset: 0x00304330
			public bool CoversChest()
			{
				bool flag = false;
				for (int i = 0; i < this._renderers.Length; i++)
				{
					if (NipplePiercingVisibility.Garment.IsVisible(this._renderers[i]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
				string presetId = this.preset.presetId;
				if (presetId == "expose_bra")
				{
					return !this.IsOpen("Open");
				}
				if (presetId == "bikini_bra")
				{
					return !this.IsOpen("BraLift") && (!this.IsOpen("NippleSlip01_Left") || !this.IsOpen("NippleSlip01_Right"));
				}
				if (presetId == "swimsuit_school")
				{
					return !this.IsOpen("Bra_PullDown02");
				}
				if (!(presetId == "raincoat"))
				{
					return !(presetId == "towel_bath") || !this.IsOpen("Towel_R1") || !this.IsOpen("Towel_R3") || !this.IsOpen("Towel_L2");
				}
				return !this.IsOpen("RainCoat_Flash");
			}

			// Token: 0x06007701 RID: 30465 RVA: 0x00306244 File Offset: 0x00304444
			private bool IsOpen(string shape)
			{
				for (int i = 0; i < this._skinned.Length; i++)
				{
					SkinnedMeshRenderer skinnedMeshRenderer = this._skinned[i];
					if (NipplePiercingVisibility.Garment.IsVisible(skinnedMeshRenderer) && !(skinnedMeshRenderer.sharedMesh == null))
					{
						Mesh sharedMesh = skinnedMeshRenderer.sharedMesh;
						Dictionary<string, int> dictionary;
						if (!this._shapeIndices.TryGetValue(sharedMesh, out dictionary))
						{
							dictionary = new Dictionary<string, int>();
							this._shapeIndices.Add(sharedMesh, dictionary);
						}
						int blendShapeIndex;
						if (!dictionary.TryGetValue(shape, out blendShapeIndex))
						{
							blendShapeIndex = sharedMesh.GetBlendShapeIndex(shape);
							dictionary.Add(shape, blendShapeIndex);
						}
						if (blendShapeIndex >= 0 && skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex) >= 99.5f)
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x06007702 RID: 30466 RVA: 0x003062E7 File Offset: 0x003044E7
			private static bool IsVisible(Renderer renderer)
			{
				return renderer != null && renderer.enabled && !renderer.forceRenderingOff && renderer.gameObject.activeInHierarchy;
			}

			// Token: 0x04007A43 RID: 31299
			public readonly GameObject root;

			// Token: 0x04007A44 RID: 31300
			public readonly MCStylePresetSO preset;

			// Token: 0x04007A45 RID: 31301
			private readonly Renderer[] _renderers;

			// Token: 0x04007A46 RID: 31302
			private readonly SkinnedMeshRenderer[] _skinned;

			// Token: 0x04007A47 RID: 31303
			private readonly Dictionary<Mesh, Dictionary<string, int>> _shapeIndices = new Dictionary<Mesh, Dictionary<string, int>>();
		}
	}
}
