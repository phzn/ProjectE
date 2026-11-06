using System;
using System.Collections.Generic;
using MagicaCloth2;
using UnityEngine;

namespace Features.Inventory.Wearable
{
	// Token: 0x0200111A RID: 4378
	internal static class CollarIdPortraitWardrobeScope
	{
		// Token: 0x060084CD RID: 33997 RVA: 0x00362AA8 File Offset: 0x00360CA8
		public static IDisposable Apply(Transform player, IEnumerable<Renderer> subjects)
		{
			return new CollarIdPortraitWardrobeScope.Scope(player, subjects);
		}

		// Token: 0x0200111B RID: 4379
		private sealed class Scope : IDisposable
		{
			// Token: 0x060084CE RID: 33998 RVA: 0x00362AB4 File Offset: 0x00360CB4
			public Scope(Transform player, IEnumerable<Renderer> subjects)
			{
				if (player == null || subjects == null)
				{
					return;
				}
				HashSet<Renderer> hashSet = new HashSet<Renderer>(subjects);
				HashSet<SkinnedMeshRenderer> hashSet2 = new HashSet<SkinnedMeshRenderer>();
				try
				{
					foreach (MagicaCloth magicaCloth in player.GetComponentsInChildren<MagicaCloth>(true))
					{
						if (!(magicaCloth == null) && magicaCloth.SerializeData.clothType == ClothProcess.ClothType.MeshCloth && magicaCloth.SerializeData.sourceRenderers != null)
						{
							foreach (Renderer renderer in magicaCloth.SerializeData.sourceRenderers)
							{
								SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
								if (!(skinnedMeshRenderer == null) && hashSet.Contains(skinnedMeshRenderer) && !hashSet2.Contains(skinnedMeshRenderer) && skinnedMeshRenderer.transform.IsChildOf(player))
								{
									Mesh customMesh = magicaCloth.GetCustomMesh(skinnedMeshRenderer);
									if (!(customMesh == null) && !(skinnedMeshRenderer.sharedMesh != customMesh))
									{
										Mesh originalMesh = magicaCloth.GetOriginalMesh(skinnedMeshRenderer);
										if (originalMesh == null || originalMesh == customMesh)
										{
											throw new InvalidOperationException("The portrait garment has no usable original mesh: " + skinnedMeshRenderer.name);
										}
										int num = originalMesh.bindposes.Length;
										List<Transform> customBones = magicaCloth.GetCustomBones(skinnedMeshRenderer);
										if (customBones == null || customBones.Count < num)
										{
											throw new InvalidOperationException("The portrait garment has an incomplete original bone list: " + skinnedMeshRenderer.name);
										}
										Transform[] array = new Transform[num];
										for (int j = 0; j < num; j++)
										{
											array[j] = customBones[j];
										}
										CollarIdPortraitWardrobeScope.RendererSnapshot rendererSnapshot = new CollarIdPortraitWardrobeScope.RendererSnapshot(skinnedMeshRenderer);
										this._snapshots.Add(rendererSnapshot);
										hashSet2.Add(skinnedMeshRenderer);
										rendererSnapshot.UseOriginal(originalMesh, array);
									}
								}
							}
						}
					}
				}
				catch
				{
					this.Dispose();
					throw;
				}
			}

			// Token: 0x060084CF RID: 33999 RVA: 0x00362CE8 File Offset: 0x00360EE8
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
					Debug.LogWarning("[Collar ID] Could not restore a garment after the portrait: " + ex.Message);
				}
			}

			// Token: 0x0400877C RID: 34684
			private readonly List<CollarIdPortraitWardrobeScope.RendererSnapshot> _snapshots = new List<CollarIdPortraitWardrobeScope.RendererSnapshot>();

			// Token: 0x0400877D RID: 34685
			private bool _disposed;
		}

		// Token: 0x0200111C RID: 4380
		private sealed class RendererSnapshot
		{
			// Token: 0x060084D0 RID: 34000 RVA: 0x00362D70 File Offset: 0x00360F70
			public RendererSnapshot(SkinnedMeshRenderer renderer)
			{
				this._renderer = renderer;
				this._mesh = renderer.sharedMesh;
				this._bones = renderer.bones;
				this._rootBone = renderer.rootBone;
				this._localBounds = renderer.localBounds;
				this._blendShapeWeights = new float[(this._mesh != null) ? this._mesh.blendShapeCount : 0];
				for (int i = 0; i < this._blendShapeWeights.Length; i++)
				{
					this._blendShapeWeights[i] = renderer.GetBlendShapeWeight(i);
				}
			}

			// Token: 0x060084D1 RID: 34001 RVA: 0x00362E04 File Offset: 0x00361004
			public void UseOriginal(Mesh original, Transform[] originalBones)
			{
				this._renderer.sharedMesh = original;
				this._renderer.bones = originalBones;
				for (int i = 0; i < original.blendShapeCount; i++)
				{
					int blendShapeIndex = this._mesh.GetBlendShapeIndex(original.GetBlendShapeName(i));
					if (blendShapeIndex >= 0 && blendShapeIndex < this._blendShapeWeights.Length)
					{
						this._renderer.SetBlendShapeWeight(i, this._blendShapeWeights[blendShapeIndex]);
					}
				}
			}

			// Token: 0x060084D2 RID: 34002 RVA: 0x00362E70 File Offset: 0x00361070
			public void Restore()
			{
				if (this._renderer == null)
				{
					return;
				}
				this._renderer.sharedMesh = this._mesh;
				this._renderer.bones = this._bones;
				this._renderer.rootBone = this._rootBone;
				this._renderer.localBounds = this._localBounds;
				if (this._mesh == null)
				{
					return;
				}
				for (int i = 0; i < this._blendShapeWeights.Length; i++)
				{
					this._renderer.SetBlendShapeWeight(i, this._blendShapeWeights[i]);
				}
			}

			// Token: 0x0400877E RID: 34686
			private readonly SkinnedMeshRenderer _renderer;

			// Token: 0x0400877F RID: 34687
			private readonly Mesh _mesh;

			// Token: 0x04008780 RID: 34688
			private readonly Transform[] _bones;

			// Token: 0x04008781 RID: 34689
			private readonly Transform _rootBone;

			// Token: 0x04008782 RID: 34690
			private readonly Bounds _localBounds;

			// Token: 0x04008783 RID: 34691
			private readonly float[] _blendShapeWeights;
		}
	}
}
