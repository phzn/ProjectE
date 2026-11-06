using System;
using System.Collections.Generic;
using RayznGames.DressRigger;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization
{
	// Token: 0x02000AC3 RID: 2755
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(1100)]
	public sealed class LilyJewelryBodyFit : MonoBehaviour
	{
		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x06004E30 RID: 20016 RVA: 0x001E4D77 File Offset: 0x001E2F77
		public static IReadOnlyList<string> BodyShapeNames { get; } = Array.AsReadOnly<string>(LilyJewelryBodyFit.ShapeNames);

		// Token: 0x06004E31 RID: 20017 RVA: 0x001E4D80 File Offset: 0x001E2F80
		public void ConfigureBody(SkinnedMeshRenderer renderer)
		{
			if (renderer != null && (LilyJewelryBodyFit.ResolveOwner(renderer.transform) != LilyJewelryBodyFit.ResolveOwner(base.transform) || renderer.transform.IsChildOf(base.transform)))
			{
				throw new ArgumentException("The body must belong to the same avatar, outside the jewellery hierarchy.", "renderer");
			}
			this.ResetTargets();
			this.body = null;
			this.bodyMesh = null;
			this.bodyBones = Array.Empty<Transform>();
			this.configuredBody = renderer;
			this.configuredOwner = ((renderer != null) ? LilyJewelryBodyFit.ResolveOwner(base.transform) : null);
			this.targetsDirty = true;
			this.RefreshFit(true);
		}

		// Token: 0x06004E32 RID: 20018 RVA: 0x001E4E27 File Offset: 0x001E3027
		private void OnEnable()
		{
			this.RefreshFit(true);
		}

		// Token: 0x06004E33 RID: 20019 RVA: 0x001E4E30 File Offset: 0x001E3030
		private void OnDisable()
		{
			this.ResetTargets();
			this.body = null;
			this.bodyMesh = null;
			this.bodyBones = Array.Empty<Transform>();
		}

		// Token: 0x06004E34 RID: 20020 RVA: 0x001E4E51 File Offset: 0x001E3051
		private void OnDestroy()
		{
			this.ResetTargets();
		}

		// Token: 0x06004E35 RID: 20021 RVA: 0x001E4E5C File Offset: 0x001E305C
		private void OnTransformParentChanged()
		{
			this.ResetTargets();
			this.body = (this.configuredBody = null);
			this.bodyMesh = null;
			this.bodyBones = Array.Empty<Transform>();
			this.configuredOwner = null;
			this.targetsDirty = true;
			if (base.isActiveAndEnabled)
			{
				this.RefreshFit(true);
			}
		}

		// Token: 0x06004E36 RID: 20022 RVA: 0x001E4EAE File Offset: 0x001E30AE
		private void OnTransformChildrenChanged()
		{
			this.targetsDirty = true;
		}

		// Token: 0x06004E37 RID: 20023 RVA: 0x001E4EB7 File Offset: 0x001E30B7
		private void LateUpdate()
		{
			this.RefreshFit(false);
		}

		// Token: 0x06004E38 RID: 20024 RVA: 0x001E4EC0 File Offset: 0x001E30C0
		public void RefreshFit(bool force = true)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			SkinnedMeshRenderer skinnedMeshRenderer = this.ResolveBody();
			if (skinnedMeshRenderer == null || skinnedMeshRenderer.sharedMesh == null)
			{
				this.ResetTargets();
				this.body = null;
				this.bodyMesh = null;
				this.bodyBones = Array.Empty<Transform>();
				return;
			}
			if (this.body != skinnedMeshRenderer || this.bodyMesh != skinnedMeshRenderer.sharedMesh)
			{
				this.ResetTargets();
				this.body = skinnedMeshRenderer;
				this.bodyMesh = skinnedMeshRenderer.sharedMesh;
				this.bodyBones = skinnedMeshRenderer.bones;
				for (int i = 0; i < LilyJewelryBodyFit.ShapeNames.Length; i++)
				{
					this.sourceIndices[i] = this.bodyMesh.GetBlendShapeIndex(LilyJewelryBodyFit.ShapeNames[i]);
				}
			}
			else if (force)
			{
				this.bodyBones = skinnedMeshRenderer.bones;
			}
			if (force || this.targetsDirty)
			{
				this.RebuildTargets();
			}
			for (int j = 0; j < this.weights.Length; j++)
			{
				float num = ((this.sourceIndices[j] >= 0) ? this.body.GetBlendShapeWeight(this.sourceIndices[j]) : 0f);
				this.weights[j] = ((float.IsNaN(num) || float.IsInfinity(num)) ? 0f : Mathf.Clamp(num, 0f, 100f));
			}
			this.CalculateGridWeights();
			for (int k = 0; k < this.targets.Length; k++)
			{
				LilyJewelryBodyFit.Target target = this.targets[k];
				if (!(target.renderer == null) && !(target.renderer == this.body) && target.renderer.transform.IsChildOf(base.transform))
				{
					if (target.mesh != target.renderer.sharedMesh)
					{
						LilyJewelryBodyFit.IndexTarget(target);
					}
					if (!(target.mesh == null))
					{
						this.BindSkin(target);
						int[] array = (target.usesGrid ? target.gridIndices : target.indices);
						float[] array2 = (target.usesGrid ? this.gridWeights : this.weights);
						for (int l = 0; l < array.Length; l++)
						{
							if (array[l] >= 0 && target.renderer.GetBlendShapeWeight(array[l]) != array2[l])
							{
								target.renderer.SetBlendShapeWeight(array[l], array2[l]);
								target.written = true;
							}
						}
					}
				}
			}
		}

		// Token: 0x06004E39 RID: 20025 RVA: 0x001E5144 File Offset: 0x001E3344
		private void CalculateGridWeights()
		{
			float num = 0f;
			for (int i = 1; i < this.sizeWeights.Length; i++)
			{
				this.sizeWeights[i] = this.weights[i - 1] * 0.01f;
				num += this.sizeWeights[i];
			}
			this.sizeWeights[0] = Mathf.Max(0f, 1f - num);
			if (num > 1f)
			{
				for (int j = 1; j < this.sizeWeights.Length; j++)
				{
					this.sizeWeights[j] /= num;
				}
			}
			float num2 = this.weights[5] * 0.01f;
			float num3 = this.weights[6] * 0.01f;
			for (int k = 0; k < this.sizeWeights.Length; k++)
			{
				for (int l = 0; l < 2; l++)
				{
					for (int m = 0; m < 2; m++)
					{
						this.gridWeights[k * 4 + l * 2 + m] = this.sizeWeights[k] * ((l == 0) ? (1f - num2) : num2) * ((m == 0) ? (1f - num3) : num3) * 100f;
					}
				}
			}
		}

		// Token: 0x06004E3A RID: 20026 RVA: 0x001E526C File Offset: 0x001E346C
		private static string[] MakeGridShapeNames()
		{
			string[] array = new string[24];
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					for (int k = 0; k < 2; k++)
					{
						array[i * 4 + j * 2 + k] = string.Concat(new string[]
						{
							"LilyFit_",
							i.ToString(),
							"_",
							j.ToString(),
							k.ToString()
						});
					}
				}
			}
			return array;
		}

		// Token: 0x06004E3B RID: 20027 RVA: 0x001E52EC File Offset: 0x001E34EC
		private SkinnedMeshRenderer ResolveBody()
		{
			Transform transform = LilyJewelryBodyFit.ResolveOwner(base.transform);
			if (this.configuredBody != null)
			{
				if (transform == this.configuredOwner && LilyJewelryBodyFit.ResolveOwner(this.configuredBody.transform) == transform)
				{
					return this.configuredBody;
				}
				this.configuredBody = null;
				this.configuredOwner = null;
			}
			PlayerDresser componentInParent = base.GetComponentInParent<PlayerDresser>(true);
			if (componentInParent == null)
			{
				return null;
			}
			SkinnedMeshRenderer characterBodyRenderer = componentInParent.CharacterBodyRenderer;
			if (!(characterBodyRenderer != null) || !characterBodyRenderer.transform.IsChildOf(componentInParent.transform) || !(characterBodyRenderer.GetComponentInParent<PlayerDresser>(true) == componentInParent))
			{
				return null;
			}
			return characterBodyRenderer;
		}

		// Token: 0x06004E3C RID: 20028 RVA: 0x001E5398 File Offset: 0x001E3598
		private static Transform ResolveOwner(Transform node)
		{
			PlayerDresser componentInParent = node.GetComponentInParent<PlayerDresser>(true);
			if (componentInParent != null)
			{
				return componentInParent.transform;
			}
			Animator componentInParent2 = node.GetComponentInParent<Animator>(true);
			if (!(componentInParent2 != null))
			{
				return node.root;
			}
			return componentInParent2.transform;
		}

		// Token: 0x06004E3D RID: 20029 RVA: 0x001E53DC File Offset: 0x001E35DC
		private void RebuildTargets()
		{
			this.ResetTargets();
			SkinnedMeshRenderer[] componentsInChildren = base.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			this.targets = new LilyJewelryBodyFit.Target[componentsInChildren.Length];
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				this.targets[i] = new LilyJewelryBodyFit.Target
				{
					renderer = componentsInChildren[i]
				};
				LilyJewelryBodyFit.IndexTarget(this.targets[i]);
			}
			this.targetsDirty = false;
		}

		// Token: 0x06004E3E RID: 20030 RVA: 0x001E5440 File Offset: 0x001E3640
		private static void IndexTarget(LilyJewelryBodyFit.Target target)
		{
			LilyJewelryBodyFit.RestoreSkin(target);
			target.mesh = ((target.renderer != null) ? target.renderer.sharedMesh : null);
			target.bindposeCount = ((target.mesh != null) ? target.mesh.bindposeCount : 0);
			target.written = false;
			for (int i = 0; i < LilyJewelryBodyFit.ShapeNames.Length; i++)
			{
				target.indices[i] = ((target.mesh != null) ? target.mesh.GetBlendShapeIndex(LilyJewelryBodyFit.ShapeNames[i]) : (-1));
			}
			int num = 0;
			for (int j = 0; j < LilyJewelryBodyFit.GridShapeNames.Length; j++)
			{
				target.gridIndices[j] = ((j > 0 && target.mesh != null) ? target.mesh.GetBlendShapeIndex(LilyJewelryBodyFit.GridShapeNames[j]) : (-1));
				if (target.gridIndices[j] >= 0)
				{
					num++;
				}
			}
			target.usesGrid = num == LilyJewelryBodyFit.GridShapeNames.Length - 1;
		}

		// Token: 0x06004E3F RID: 20031 RVA: 0x001E5540 File Offset: 0x001E3740
		private void BindSkin(LilyJewelryBodyFit.Target target)
		{
			if (target.skinBound || target.bindposeCount == 0 || target.bindposeCount != this.bodyBones.Length)
			{
				return;
			}
			target.originalBones = target.renderer.bones;
			target.originalRootBone = target.renderer.rootBone;
			target.boundBones = this.bodyBones;
			target.boundRootBone = base.transform.parent;
			target.renderer.bones = target.boundBones;
			target.renderer.rootBone = target.boundRootBone;
			target.skinBound = true;
		}

		// Token: 0x06004E40 RID: 20032 RVA: 0x001E55D8 File Offset: 0x001E37D8
		private static void RestoreSkin(LilyJewelryBodyFit.Target target)
		{
			if (!target.skinBound)
			{
				return;
			}
			if (target.renderer != null)
			{
				Transform[] bones = target.renderer.bones;
				bool flag = bones.Length == target.boundBones.Length && target.renderer.rootBone == target.boundRootBone;
				int num = 0;
				while (flag && num < bones.Length)
				{
					flag = bones[num] == target.boundBones[num];
					num++;
				}
				if (flag)
				{
					target.renderer.bones = target.originalBones;
					target.renderer.rootBone = target.originalRootBone;
				}
			}
			target.skinBound = false;
			target.originalBones = (target.boundBones = null);
			target.originalRootBone = (target.boundRootBone = null);
		}

		// Token: 0x06004E41 RID: 20033 RVA: 0x001E56A0 File Offset: 0x001E38A0
		private void ResetTargets()
		{
			for (int i = 0; i < this.targets.Length; i++)
			{
				LilyJewelryBodyFit.Target target = this.targets[i];
				LilyJewelryBodyFit.RestoreSkin(target);
				if (target.written && !(target.renderer == null) && !(target.renderer == this.body) && target.renderer.transform.IsChildOf(base.transform))
				{
					if (target.mesh != target.renderer.sharedMesh)
					{
						LilyJewelryBodyFit.IndexTarget(target);
					}
					else
					{
						int[] array = (target.usesGrid ? target.gridIndices : target.indices);
						for (int j = 0; j < array.Length; j++)
						{
							if (array[j] >= 0)
							{
								target.renderer.SetBlendShapeWeight(array[j], 0f);
							}
						}
						target.written = false;
					}
				}
			}
		}

		// Token: 0x04004F7B RID: 20347
		private static readonly string[] ShapeNames = new string[] { "Breast_Size_00_Flat", "Breast_Size_02_Smaller", "Breast_Size_01_Small", "Breast_Size_03_Natural", "Breast_Size_04_Balloon", "Upper Torso Thickness OUT", "Breast Vertical Position" };

		// Token: 0x04004F7D RID: 20349
		private static readonly string[] GridShapeNames = LilyJewelryBodyFit.MakeGridShapeNames();

		// Token: 0x04004F7E RID: 20350
		private readonly int[] sourceIndices = new int[LilyJewelryBodyFit.ShapeNames.Length];

		// Token: 0x04004F7F RID: 20351
		private readonly float[] weights = new float[LilyJewelryBodyFit.ShapeNames.Length];

		// Token: 0x04004F80 RID: 20352
		private readonly float[] sizeWeights = new float[6];

		// Token: 0x04004F81 RID: 20353
		private readonly float[] gridWeights = new float[24];

		// Token: 0x04004F82 RID: 20354
		private LilyJewelryBodyFit.Target[] targets = Array.Empty<LilyJewelryBodyFit.Target>();

		// Token: 0x04004F83 RID: 20355
		private SkinnedMeshRenderer body;

		// Token: 0x04004F84 RID: 20356
		private SkinnedMeshRenderer configuredBody;

		// Token: 0x04004F85 RID: 20357
		private Transform[] bodyBones = Array.Empty<Transform>();

		// Token: 0x04004F86 RID: 20358
		private Transform configuredOwner;

		// Token: 0x04004F87 RID: 20359
		private Mesh bodyMesh;

		// Token: 0x04004F88 RID: 20360
		private bool targetsDirty = true;

		// Token: 0x02000AC4 RID: 2756
		private sealed class Target
		{
			// Token: 0x04004F89 RID: 20361
			public SkinnedMeshRenderer renderer;

			// Token: 0x04004F8A RID: 20362
			public Mesh mesh;

			// Token: 0x04004F8B RID: 20363
			public readonly int[] indices = new int[LilyJewelryBodyFit.ShapeNames.Length];

			// Token: 0x04004F8C RID: 20364
			public readonly int[] gridIndices = new int[24];

			// Token: 0x04004F8D RID: 20365
			public bool usesGrid;

			// Token: 0x04004F8E RID: 20366
			public bool written;

			// Token: 0x04004F8F RID: 20367
			public int bindposeCount;

			// Token: 0x04004F90 RID: 20368
			public bool skinBound;

			// Token: 0x04004F91 RID: 20369
			public Transform[] originalBones;

			// Token: 0x04004F92 RID: 20370
			public Transform[] boundBones;

			// Token: 0x04004F93 RID: 20371
			public Transform originalRootBone;

			// Token: 0x04004F94 RID: 20372
			public Transform boundRootBone;
		}
	}
}
