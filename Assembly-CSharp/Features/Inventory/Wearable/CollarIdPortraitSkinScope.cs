using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Inventory.Wearable
{
	// Token: 0x02001117 RID: 4375
	internal static class CollarIdPortraitSkinScope
	{
		// Token: 0x060084C6 RID: 33990 RVA: 0x00362641 File Offset: 0x00360841
		public static IDisposable Apply(IEnumerable<Renderer> subjects)
		{
			return new CollarIdPortraitSkinScope.Scope(subjects);
		}

		// Token: 0x02001118 RID: 4376
		private sealed class Scope : IDisposable
		{
			// Token: 0x060084C7 RID: 33991 RVA: 0x0036264C File Offset: 0x0036084C
			public Scope(IEnumerable<Renderer> subjects)
			{
				if (subjects == null)
				{
					return;
				}
				HashSet<SkinnedMeshRenderer> hashSet = new HashSet<SkinnedMeshRenderer>();
				try
				{
					foreach (Renderer renderer in subjects)
					{
						SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
						if (!(skinnedMeshRenderer == null) && skinnedMeshRenderer.enabled && skinnedMeshRenderer.gameObject.activeInHierarchy && !skinnedMeshRenderer.forceRenderingOff && !(skinnedMeshRenderer.sharedMesh == null) && hashSet.Add(skinnedMeshRenderer))
						{
							CollarIdPortraitSkinScope.SkinSnapshot skinSnapshot = new CollarIdPortraitSkinScope.SkinSnapshot(skinnedMeshRenderer);
							this._snapshots.Add(skinSnapshot);
							skinSnapshot.CreatePortraitSkin();
						}
					}
				}
				catch
				{
					this.Dispose();
					throw;
				}
			}

			// Token: 0x060084C8 RID: 33992 RVA: 0x0036271C File Offset: 0x0036091C
			public void Dispose()
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
				Exception ex = null;
				for (int i = this._snapshots.Count - 1; i >= 0; i--)
				{
					try
					{
						this._snapshots[i].Restore();
					}
					catch (Exception ex2)
					{
						if (ex == null)
						{
							ex = ex2;
						}
					}
				}
				this._snapshots.Clear();
				if (ex != null)
				{
					Debug.LogWarning("[Collar ID] Could not fully restore a portrait skin: " + ex.Message);
				}
			}

			// Token: 0x04008776 RID: 34678
			private readonly List<CollarIdPortraitSkinScope.SkinSnapshot> _snapshots = new List<CollarIdPortraitSkinScope.SkinSnapshot>();

			// Token: 0x04008777 RID: 34679
			private bool _disposed;
		}

		// Token: 0x02001119 RID: 4377
		private sealed class SkinSnapshot
		{
			// Token: 0x060084C9 RID: 33993 RVA: 0x003627A4 File Offset: 0x003609A4
			public SkinSnapshot(SkinnedMeshRenderer source)
			{
				this._source = source;
				this._forceRenderingOff = source.forceRenderingOff;
			}

			// Token: 0x060084CA RID: 33994 RVA: 0x003627C0 File Offset: 0x003609C0
			public void CreatePortraitSkin()
			{
				this._mesh = new Mesh
				{
					name = this._source.name + " (ID portrait skin)",
					hideFlags = HideFlags.HideAndDontSave
				};
				this._source.BakeMesh(this._mesh, true);
				this._visual = new GameObject(this._source.name + " (ID portrait skin)")
				{
					hideFlags = HideFlags.HideAndDontSave,
					layer = this._source.gameObject.layer
				};
				this._visual.SetActive(false);
				this._visual.transform.SetParent(this._source.transform, false);
				this._visual.AddComponent<MeshFilter>().sharedMesh = this._mesh;
				MeshRenderer meshRenderer = this._visual.AddComponent<MeshRenderer>();
				Material[] sharedMaterials = this._source.sharedMaterials;
				meshRenderer.sharedMaterials = sharedMaterials;
				meshRenderer.shadowCastingMode = this._source.shadowCastingMode;
				meshRenderer.receiveShadows = this._source.receiveShadows;
				meshRenderer.lightProbeUsage = this._source.lightProbeUsage;
				meshRenderer.reflectionProbeUsage = this._source.reflectionProbeUsage;
				meshRenderer.probeAnchor = this._source.probeAnchor;
				meshRenderer.lightProbeProxyVolumeOverride = this._source.lightProbeProxyVolumeOverride;
				meshRenderer.renderingLayerMask = this._source.renderingLayerMask;
				meshRenderer.sortingLayerID = this._source.sortingLayerID;
				meshRenderer.sortingOrder = this._source.sortingOrder;
				meshRenderer.allowOcclusionWhenDynamic = this._source.allowOcclusionWhenDynamic;
				meshRenderer.motionVectorGenerationMode = this._source.motionVectorGenerationMode;
				meshRenderer.lightmapIndex = this._source.lightmapIndex;
				meshRenderer.lightmapScaleOffset = this._source.lightmapScaleOffset;
				meshRenderer.realtimeLightmapIndex = this._source.realtimeLightmapIndex;
				meshRenderer.realtimeLightmapScaleOffset = this._source.realtimeLightmapScaleOffset;
				MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
				this._source.GetPropertyBlock(materialPropertyBlock);
				meshRenderer.SetPropertyBlock(materialPropertyBlock.isEmpty ? null : materialPropertyBlock);
				for (int i = 0; i < sharedMaterials.Length; i++)
				{
					materialPropertyBlock.Clear();
					this._source.GetPropertyBlock(materialPropertyBlock, i);
					meshRenderer.SetPropertyBlock(materialPropertyBlock.isEmpty ? null : materialPropertyBlock, i);
				}
				this._source.forceRenderingOff = true;
				this._visual.SetActive(true);
			}

			// Token: 0x060084CB RID: 33995 RVA: 0x00362A18 File Offset: 0x00360C18
			public void Restore()
			{
				try
				{
					if (this._visual != null)
					{
						this._visual.SetActive(false);
					}
				}
				finally
				{
					try
					{
						if (this._source != null)
						{
							this._source.forceRenderingOff = this._forceRenderingOff;
						}
					}
					finally
					{
						try
						{
							CollarIdPortraitSkinScope.SkinSnapshot.DestroyOwned(this._visual);
						}
						finally
						{
							CollarIdPortraitSkinScope.SkinSnapshot.DestroyOwned(this._mesh);
						}
					}
				}
			}

			// Token: 0x060084CC RID: 33996 RVA: 0x000A4FF8 File Offset: 0x000A31F8
			private static void DestroyOwned(Object value)
			{
				if (value == null)
				{
					return;
				}
				if (Application.isPlaying)
				{
					Object.Destroy(value);
					return;
				}
				Object.DestroyImmediate(value);
			}

			// Token: 0x04008778 RID: 34680
			private readonly SkinnedMeshRenderer _source;

			// Token: 0x04008779 RID: 34681
			private readonly bool _forceRenderingOff;

			// Token: 0x0400877A RID: 34682
			private Mesh _mesh;

			// Token: 0x0400877B RID: 34683
			private GameObject _visual;
		}
	}
}
