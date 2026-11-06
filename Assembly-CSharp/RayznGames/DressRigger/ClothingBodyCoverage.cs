using System;
using System.Collections.Generic;
using UnityEngine;

namespace RayznGames.DressRigger
{
	// Token: 0x02000781 RID: 1921
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(22000)]
	public sealed class ClothingBodyCoverage : MonoBehaviour
	{
		// Token: 0x06003264 RID: 12900 RVA: 0x0011D487 File Offset: 0x0011B687
		public void Configure(Mesh source, Mesh covered, Renderer garmentRenderer)
		{
			this.ReleaseCoverage();
			this.sourceBodyMesh = source;
			this.coveredBodyMesh = covered;
			this.coverageRenderer = garmentRenderer;
			this._checkedConfiguration = false;
			this.RefreshCoverage();
		}

		// Token: 0x06003265 RID: 12901 RVA: 0x0011D4B4 File Offset: 0x0011B6B4
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetRegistry()
		{
			foreach (ClothingBodyCoverage.BodyCoverage bodyCoverage in ClothingBodyCoverage.Bodies.Values)
			{
				bodyCoverage.Restore();
			}
			ClothingBodyCoverage.Bodies.Clear();
		}

		// Token: 0x06003266 RID: 12902 RVA: 0x0011D514 File Offset: 0x0011B714
		private void OnEnable()
		{
			this.RefreshCoverage();
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x0011D514 File Offset: 0x0011B714
		private void LateUpdate()
		{
			this.RefreshCoverage();
		}

		// Token: 0x06003268 RID: 12904 RVA: 0x0011D51C File Offset: 0x0011B71C
		private void OnDisable()
		{
			this.ReleaseCoverage();
		}

		// Token: 0x06003269 RID: 12905 RVA: 0x0011D51C File Offset: 0x0011B71C
		private void OnDestroy()
		{
			this.ReleaseCoverage();
		}

		// Token: 0x0600326A RID: 12906 RVA: 0x0011D524 File Offset: 0x0011B724
		private void OnTransformParentChanged()
		{
			this.ReleaseCoverage();
			this._dresser = null;
			this.RefreshCoverage();
		}

		// Token: 0x0600326B RID: 12907 RVA: 0x0011D53C File Offset: 0x0011B73C
		public void RefreshCoverage()
		{
			if (!this._checkedConfiguration)
			{
				this._checkedConfiguration = true;
				this._validConfiguration = ClothingBodyCoverage.MeshesAreCompatible(this.sourceBodyMesh, this.coveredBodyMesh);
			}
			if (!this.IsCoverageVisible || !this._validConfiguration)
			{
				this.ReleaseCoverage();
				return;
			}
			if (this._dresser == null || !base.transform.IsChildOf(this._dresser.transform))
			{
				this._dresser = base.GetComponentInParent<PlayerDresser>(true);
			}
			if (this._dresser == null)
			{
				this.ReleaseCoverage();
				return;
			}
			SkinnedMeshRenderer characterBodyRenderer = this._dresser.CharacterBodyRenderer;
			if (this._coverage != null && (this._coverage.Body != characterBodyRenderer || !this._coverage.AcceptsCurrentBody()))
			{
				this.ReleaseCoverage();
			}
			if (this._coverage == null)
			{
				if (characterBodyRenderer == null || !ClothingBodyCoverage.SharesSkeleton(this.coverageRenderer as SkinnedMeshRenderer, characterBodyRenderer))
				{
					return;
				}
				ClothingBodyCoverage.BodyCoverage bodyCoverage;
				if (!ClothingBodyCoverage.Bodies.TryGetValue(characterBodyRenderer, out bodyCoverage))
				{
					if (characterBodyRenderer.sharedMesh != this.sourceBodyMesh)
					{
						return;
					}
					bodyCoverage = new ClothingBodyCoverage.BodyCoverage(characterBodyRenderer, this._dresser.transform, this.sourceBodyMesh);
					ClothingBodyCoverage.Bodies.Add(characterBodyRenderer, bodyCoverage);
				}
				if (bodyCoverage.Source != this.sourceBodyMesh || !bodyCoverage.AcceptsCurrentBody())
				{
					return;
				}
				this._coverage = bodyCoverage;
				bodyCoverage.Add(this);
			}
			this._coverage.Refresh();
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x0600326C RID: 12908 RVA: 0x0011D6AC File Offset: 0x0011B8AC
		private bool IsCoverageVisible
		{
			get
			{
				return base.isActiveAndEnabled && this.coverageRenderer != null && this.coverageRenderer.enabled && !this.coverageRenderer.forceRenderingOff && this.coverageRenderer.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x0600326D RID: 12909 RVA: 0x0011D6FC File Offset: 0x0011B8FC
		private void ReleaseCoverage()
		{
			ClothingBodyCoverage.BodyCoverage coverage = this._coverage;
			this._coverage = null;
			if (coverage == null)
			{
				return;
			}
			coverage.Remove(this);
			if (!coverage.HasOwners)
			{
				ClothingBodyCoverage.Bodies.Remove(coverage.Body);
			}
		}

		// Token: 0x0600326E RID: 12910 RVA: 0x0011D73C File Offset: 0x0011B93C
		private static bool MeshesAreCompatible(Mesh source, Mesh covered)
		{
			if (source == null || covered == null || source == covered || source.vertexCount != covered.vertexCount || source.subMeshCount != covered.subMeshCount || source.blendShapeCount != covered.blendShapeCount)
			{
				return false;
			}
			for (int i = 0; i < source.blendShapeCount; i++)
			{
				if (source.GetBlendShapeName(i) != covered.GetBlendShapeName(i) || source.GetBlendShapeFrameCount(i) != covered.GetBlendShapeFrameCount(i))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600326F RID: 12911 RVA: 0x0011D7CC File Offset: 0x0011B9CC
		private static bool SharesSkeleton(SkinnedMeshRenderer garment, SkinnedMeshRenderer body)
		{
			if (garment == null || body == null || garment == body)
			{
				return false;
			}
			if (garment.rootBone != null && garment.rootBone == body.rootBone)
			{
				return true;
			}
			Transform[] bones = body.bones;
			foreach (Transform transform in garment.bones)
			{
				if (transform != null)
				{
					for (int j = 0; j < bones.Length; j++)
					{
						if (transform == bones[j])
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x04003324 RID: 13092
		[SerializeField]
		private Mesh sourceBodyMesh;

		// Token: 0x04003325 RID: 13093
		[SerializeField]
		private Mesh coveredBodyMesh;

		// Token: 0x04003326 RID: 13094
		[SerializeField]
		private Renderer coverageRenderer;

		// Token: 0x04003327 RID: 13095
		private static readonly Dictionary<SkinnedMeshRenderer, ClothingBodyCoverage.BodyCoverage> Bodies = new Dictionary<SkinnedMeshRenderer, ClothingBodyCoverage.BodyCoverage>();

		// Token: 0x04003328 RID: 13096
		private ClothingBodyCoverage.BodyCoverage _coverage;

		// Token: 0x04003329 RID: 13097
		private PlayerDresser _dresser;

		// Token: 0x0400332A RID: 13098
		private bool _checkedConfiguration;

		// Token: 0x0400332B RID: 13099
		private bool _validConfiguration;

		// Token: 0x02000782 RID: 1922
		private sealed class BodyCoverage
		{
			// Token: 0x17000995 RID: 2453
			// (get) Token: 0x06003272 RID: 12914 RVA: 0x0011D86F File Offset: 0x0011BA6F
			public bool HasOwners
			{
				get
				{
					return this._owners.Count != 0;
				}
			}

			// Token: 0x06003273 RID: 12915 RVA: 0x0011D880 File Offset: 0x0011BA80
			public BodyCoverage(SkinnedMeshRenderer body, Transform rig, Mesh source)
			{
				this.Body = body;
				this._rig = rig;
				this.Source = source;
			}

			// Token: 0x06003274 RID: 12916 RVA: 0x0011D8D8 File Offset: 0x0011BAD8
			public bool AcceptsCurrentBody()
			{
				return this.Body != null && (this.Body.sharedMesh == this.Source || this._coveredMeshes.Contains(this.Body.sharedMesh));
			}

			// Token: 0x06003275 RID: 12917 RVA: 0x0011D925 File Offset: 0x0011BB25
			public void Add(ClothingBodyCoverage owner)
			{
				if (!this._owners.Contains(owner))
				{
					this._owners.Add(owner);
				}
				this._coveredMeshes.Add(owner.coveredBodyMesh);
			}

			// Token: 0x06003276 RID: 12918 RVA: 0x0011D953 File Offset: 0x0011BB53
			public void Remove(ClothingBodyCoverage owner)
			{
				this._owners.Remove(owner);
				this.Refresh();
			}

			// Token: 0x06003277 RID: 12919 RVA: 0x0011D968 File Offset: 0x0011BB68
			public void Refresh()
			{
				Mesh mesh = null;
				for (int i = this._owners.Count - 1; i >= 0; i--)
				{
					ClothingBodyCoverage clothingBodyCoverage = this._owners[i];
					if (clothingBodyCoverage != null && clothingBodyCoverage.IsCoverageVisible)
					{
						mesh = clothingBodyCoverage.coveredBodyMesh;
						break;
					}
				}
				if (mesh == null || !this.AcceptsCurrentBody())
				{
					this.Restore();
					return;
				}
				if (this._rig == null)
				{
					return;
				}
				this._rig.GetComponentsInChildren<SkinnedMeshRenderer>(true, this._renderers);
				for (int j = 0; j < this._renderers.Count; j++)
				{
					SkinnedMeshRenderer skinnedMeshRenderer = this._renderers[j];
					if (!(skinnedMeshRenderer == null))
					{
						Mesh sharedMesh = skinnedMeshRenderer.sharedMesh;
						if ((!(sharedMesh != this.Source) || this._coveredMeshes.Contains(sharedMesh)) && this.BelongsToBody(skinnedMeshRenderer))
						{
							this._modified.Add(skinnedMeshRenderer);
							ClothingBodyCoverage.BodyCoverage.SetMeshPreservingPose(skinnedMeshRenderer, mesh);
						}
					}
				}
			}

			// Token: 0x06003278 RID: 12920 RVA: 0x0011DA64 File Offset: 0x0011BC64
			public void Restore()
			{
				if (this._rig != null)
				{
					this._rig.GetComponentsInChildren<SkinnedMeshRenderer>(true, this._renderers);
					for (int i = 0; i < this._renderers.Count; i++)
					{
						SkinnedMeshRenderer skinnedMeshRenderer = this._renderers[i];
						if (skinnedMeshRenderer != null && this._coveredMeshes.Contains(skinnedMeshRenderer.sharedMesh) && this.BelongsToBody(skinnedMeshRenderer))
						{
							this._modified.Add(skinnedMeshRenderer);
						}
					}
				}
				foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in this._modified)
				{
					if (skinnedMeshRenderer2 != null && this._coveredMeshes.Contains(skinnedMeshRenderer2.sharedMesh))
					{
						ClothingBodyCoverage.BodyCoverage.SetMeshPreservingPose(skinnedMeshRenderer2, this.Source);
					}
				}
				this._modified.Clear();
			}

			// Token: 0x06003279 RID: 12921 RVA: 0x0011DB58 File Offset: 0x0011BD58
			private bool BelongsToBody(SkinnedMeshRenderer renderer)
			{
				if (!(renderer == this.Body))
				{
					PlayerDresser componentInParent = renderer.GetComponentInParent<PlayerDresser>(true);
					return ((componentInParent != null) ? componentInParent.CharacterBodyRenderer : null) == this.Body && ClothingBodyCoverage.SharesSkeleton(renderer, this.Body);
				}
				return true;
			}

			// Token: 0x0600327A RID: 12922 RVA: 0x0011DB98 File Offset: 0x0011BD98
			private static void SetMeshPreservingPose(SkinnedMeshRenderer renderer, Mesh mesh)
			{
				if (renderer.sharedMesh == mesh)
				{
					return;
				}
				int num = ((renderer.sharedMesh != null) ? renderer.sharedMesh.blendShapeCount : 0);
				float[] array = new float[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = renderer.GetBlendShapeWeight(i);
				}
				Bounds localBounds = renderer.localBounds;
				renderer.sharedMesh = mesh;
				renderer.localBounds = localBounds;
				int num2 = 0;
				while (num2 < num && num2 < mesh.blendShapeCount)
				{
					renderer.SetBlendShapeWeight(num2, array[num2]);
					num2++;
				}
			}

			// Token: 0x0400332C RID: 13100
			public readonly SkinnedMeshRenderer Body;

			// Token: 0x0400332D RID: 13101
			public readonly Mesh Source;

			// Token: 0x0400332E RID: 13102
			private readonly Transform _rig;

			// Token: 0x0400332F RID: 13103
			private readonly List<ClothingBodyCoverage> _owners = new List<ClothingBodyCoverage>(2);

			// Token: 0x04003330 RID: 13104
			private readonly HashSet<Mesh> _coveredMeshes = new HashSet<Mesh>();

			// Token: 0x04003331 RID: 13105
			private readonly HashSet<SkinnedMeshRenderer> _modified = new HashSet<SkinnedMeshRenderer>();

			// Token: 0x04003332 RID: 13106
			private readonly List<SkinnedMeshRenderer> _renderers = new List<SkinnedMeshRenderer>();
		}
	}
}
