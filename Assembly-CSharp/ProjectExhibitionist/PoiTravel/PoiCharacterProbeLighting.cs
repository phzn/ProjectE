using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.PoiTravel
{
	// Token: 0x02000959 RID: 2393
	[DisallowMultipleComponent]
	public sealed class PoiCharacterProbeLighting : MonoBehaviour
	{
		// Token: 0x0600413B RID: 16699 RVA: 0x00182F38 File Offset: 0x00181138
		private static Vector3[] CreateDiffuseDirections()
		{
			Vector3[] array = new Vector3[26];
			int num = 0;
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					for (int k = -1; k <= 1; k++)
					{
						if (i != 0 || j != 0 || k != 0)
						{
							array[num++] = new Vector3((float)i, (float)j, (float)k).normalized;
						}
					}
				}
			}
			return array;
		}

		// Token: 0x0600413C RID: 16700 RVA: 0x00182FA2 File Offset: 0x001811A2
		private void OnEnable()
		{
			SceneManager.activeSceneChanged += this.OnActiveSceneChanged;
			SceneBootLoader.OnCoreReady += this.Refresh;
			GameSystemsBootstrapper.OnGameplayReady += this.Refresh;
			this.Refresh();
		}

		// Token: 0x0600413D RID: 16701 RVA: 0x00182FDD File Offset: 0x001811DD
		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			SceneBootLoader.OnCoreReady -= this.Refresh;
			GameSystemsBootstrapper.OnGameplayReady -= this.Refresh;
			this.Restore();
		}

		// Token: 0x0600413E RID: 16702 RVA: 0x00183018 File Offset: 0x00181218
		private void OnActiveSceneChanged(Scene previous, Scene next)
		{
			if (next != base.gameObject.scene)
			{
				this.Restore();
				return;
			}
			this.Refresh();
		}

		// Token: 0x0600413F RID: 16703 RVA: 0x0018303C File Offset: 0x0018123C
		private void LateUpdate()
		{
			if (this._player == null || SceneManager.GetActiveScene() != base.gameObject.scene)
			{
				if (this._originals.Count > 0)
				{
					this.Restore();
				}
				return;
			}
			if (this.minimumDiffuseLuminance <= 0f)
			{
				if (this._fillActive)
				{
					foreach (KeyValuePair<Renderer, PoiCharacterProbeLighting.Original> keyValuePair in this._originals)
					{
						if (!(keyValuePair.Key == null))
						{
							PoiCharacterProbeLighting.RestorePropertyBlocks(keyValuePair.Key, keyValuePair.Value);
							keyValuePair.Key.lightProbeUsage = LightProbeUsage.BlendProbes;
						}
					}
					this._fillActive = false;
				}
				return;
			}
			if (!this._fillActive)
			{
				this.ApplyToPlayer(this._player);
				return;
			}
			if (this._anchor == null)
			{
				this.Refresh();
				return;
			}
			this.UpdateDiffuseFill();
		}

		// Token: 0x06004140 RID: 16704 RVA: 0x00183140 File Offset: 0x00181340
		private void Refresh()
		{
			if (!base.isActiveAndEnabled || SceneManager.GetActiveScene() != base.gameObject.scene)
			{
				this.Restore();
				return;
			}
			Transform transform = ((PlayerProximityProvider.Instance != null) ? PlayerProximityProvider.Instance.Player : null);
			if (transform == null)
			{
				this.Restore();
				return;
			}
			this.ApplyToPlayer(transform);
		}

		// Token: 0x06004141 RID: 16705 RVA: 0x001831A8 File Offset: 0x001813A8
		internal void ApplyToPlayer(Transform player)
		{
			if (player == null)
			{
				this.Restore();
				return;
			}
			if (player != this._player)
			{
				this.Restore();
				this._player = player;
				if (this._player == null)
				{
					return;
				}
				this._style = this._player.GetComponentInChildren<MCStyleCustomizationService>(true);
				if (this._style != null)
				{
					this._style.OnActiveStateChanged += this.Refresh;
				}
			}
			if (this._anchor == null)
			{
				Animator componentInChildren = this._player.GetComponentInChildren<Animator>(true);
				if (componentInChildren != null && componentInChildren.avatar != null && componentInChildren.isHuman)
				{
					this._anchor = componentInChildren.GetBoneTransform(HumanBodyBones.Chest) ?? componentInChildren.GetBoneTransform(HumanBodyBones.Spine);
				}
				if (this._anchor == null)
				{
					this._anchor = this._player;
				}
			}
			foreach (Renderer renderer in this._player.GetComponentsInChildren<Renderer>(true))
			{
				if (renderer is SkinnedMeshRenderer || renderer is MeshRenderer)
				{
					PoiCharacterProbeLighting.Original original;
					if (!this._originals.TryGetValue(renderer, out original))
					{
						original = new PoiCharacterProbeLighting.Original
						{
							usage = renderer.lightProbeUsage,
							anchor = renderer.probeAnchor
						};
						this._originals.Add(renderer, original);
					}
					renderer.probeAnchor = this._anchor;
					if (this.minimumDiffuseLuminance > 0f)
					{
						this.CapturePropertyBlocks(renderer, original);
						renderer.lightProbeUsage = LightProbeUsage.CustomProvided;
					}
					else
					{
						PoiCharacterProbeLighting.RestorePropertyBlocks(renderer, original);
						renderer.lightProbeUsage = LightProbeUsage.BlendProbes;
					}
				}
			}
			if (this.minimumDiffuseLuminance > 0f)
			{
				this.UpdateDiffuseFill();
				return;
			}
			this._fillActive = false;
		}

		// Token: 0x06004142 RID: 16706 RVA: 0x0018335C File Offset: 0x0018155C
		private void CapturePropertyBlocks(Renderer renderer, PoiCharacterProbeLighting.Original original)
		{
			if (!original.blocksCaptured)
			{
				original.globalBlock = new MaterialPropertyBlock();
				renderer.GetPropertyBlock(original.globalBlock);
				original.blocksCaptured = true;
			}
			renderer.GetSharedMaterials(this._materials);
			original.materialCount = this._materials.Count;
			while (original.materialBlocks.Count < original.materialCount)
			{
				MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
				renderer.GetPropertyBlock(materialPropertyBlock, original.materialBlocks.Count);
				original.materialBlocks.Add(materialPropertyBlock);
			}
		}

		// Token: 0x06004143 RID: 16707 RVA: 0x001833E8 File Offset: 0x001815E8
		internal static SphericalHarmonicsL2 AddNeutralDiffuseFloor(SphericalHarmonicsL2 lighting, float minimum)
		{
			if (minimum <= 0f)
			{
				return lighting;
			}
			lighting.Evaluate(PoiCharacterProbeLighting.DiffuseDirections, PoiCharacterProbeLighting.DiffuseSamples);
			float num = float.PositiveInfinity;
			for (int i = 0; i < PoiCharacterProbeLighting.DiffuseSamples.Length; i++)
			{
				Color color = PoiCharacterProbeLighting.DiffuseSamples[i];
				num = Mathf.Min(num, color.r * 0.2126f + color.g * 0.7152f + color.b * 0.0722f);
			}
			float num2 = Mathf.Max(0f, minimum - num);
			if (num2 > 0f)
			{
				lighting.AddAmbientLight(new Color(num2, num2, num2, 1f));
			}
			return lighting;
		}

		// Token: 0x06004144 RID: 16708 RVA: 0x0018348C File Offset: 0x0018168C
		private void UpdateDiffuseFill()
		{
			if (this._anchor == null || this.minimumDiffuseLuminance <= 0f)
			{
				return;
			}
			this._samplePositions[0] = this._anchor.position;
			LightProbes.CalculateInterpolatedLightAndOcclusionProbes(this._samplePositions, this._sampleLighting, this._sampleOcclusion);
			this._sampleLighting[0] = PoiCharacterProbeLighting.AddNeutralDiffuseFloor(this._sampleLighting[0], this.minimumDiffuseLuminance);
			this._fillActive = true;
			this.ApplySampledLighting();
		}

		// Token: 0x06004145 RID: 16709 RVA: 0x00183514 File Offset: 0x00181714
		private void ApplySampledLighting()
		{
			if (this._workingBlock == null)
			{
				this._workingBlock = new MaterialPropertyBlock();
			}
			foreach (KeyValuePair<Renderer, PoiCharacterProbeLighting.Original> keyValuePair in this._originals)
			{
				Renderer key = keyValuePair.Key;
				if (!(key == null) && keyValuePair.Value.blocksCaptured)
				{
					key.GetPropertyBlock(this._workingBlock);
					this.CopySampleToBlock();
					key.SetPropertyBlock(this._workingBlock);
					for (int i = 0; i < keyValuePair.Value.materialCount; i++)
					{
						key.GetPropertyBlock(this._workingBlock, i);
						if (!this._workingBlock.isEmpty)
						{
							this.CopySampleToBlock();
							key.SetPropertyBlock(this._workingBlock, i);
						}
					}
				}
			}
		}

		// Token: 0x06004146 RID: 16710 RVA: 0x001835FC File Offset: 0x001817FC
		private void CopySampleToBlock()
		{
			this._workingBlock.CopySHCoefficientArraysFrom(this._sampleLighting);
			this._workingBlock.CopyProbeOcclusionArrayFrom(this._sampleOcclusion);
		}

		// Token: 0x06004147 RID: 16711 RVA: 0x00183620 File Offset: 0x00181820
		private static void RestorePropertyBlocks(Renderer renderer, PoiCharacterProbeLighting.Original original)
		{
			if (!original.blocksCaptured)
			{
				return;
			}
			renderer.SetPropertyBlock(original.globalBlock.isEmpty ? null : original.globalBlock);
			for (int i = 0; i < original.materialCount; i++)
			{
				MaterialPropertyBlock materialPropertyBlock = original.materialBlocks[i];
				renderer.SetPropertyBlock(materialPropertyBlock.isEmpty ? null : materialPropertyBlock, i);
			}
			original.globalBlock = null;
			original.materialBlocks.Clear();
			original.blocksCaptured = false;
		}

		// Token: 0x06004148 RID: 16712 RVA: 0x0018369C File Offset: 0x0018189C
		internal void Restore()
		{
			if (this._style != null)
			{
				this._style.OnActiveStateChanged -= this.Refresh;
			}
			foreach (KeyValuePair<Renderer, PoiCharacterProbeLighting.Original> keyValuePair in this._originals)
			{
				if (!(keyValuePair.Key == null))
				{
					PoiCharacterProbeLighting.RestorePropertyBlocks(keyValuePair.Key, keyValuePair.Value);
					keyValuePair.Key.lightProbeUsage = keyValuePair.Value.usage;
					keyValuePair.Key.probeAnchor = keyValuePair.Value.anchor;
				}
			}
			this._originals.Clear();
			this._materials.Clear();
			this._fillActive = false;
			this._style = null;
			this._player = null;
			this._anchor = null;
		}

		// Token: 0x04004121 RID: 16673
		[Tooltip("Optional minimum neutral diffuse light for this room's player, in linear light units. Zero keeps the authored probes unchanged.")]
		[SerializeField]
		[Min(0f)]
		private float minimumDiffuseLuminance;

		// Token: 0x04004122 RID: 16674
		private readonly Dictionary<Renderer, PoiCharacterProbeLighting.Original> _originals = new Dictionary<Renderer, PoiCharacterProbeLighting.Original>();

		// Token: 0x04004123 RID: 16675
		private readonly List<Material> _materials = new List<Material>();

		// Token: 0x04004124 RID: 16676
		private readonly Vector3[] _samplePositions = new Vector3[1];

		// Token: 0x04004125 RID: 16677
		private readonly SphericalHarmonicsL2[] _sampleLighting = new SphericalHarmonicsL2[1];

		// Token: 0x04004126 RID: 16678
		private readonly Vector4[] _sampleOcclusion = new Vector4[1];

		// Token: 0x04004127 RID: 16679
		private MaterialPropertyBlock _workingBlock;

		// Token: 0x04004128 RID: 16680
		private bool _fillActive;

		// Token: 0x04004129 RID: 16681
		private Transform _player;

		// Token: 0x0400412A RID: 16682
		private Transform _anchor;

		// Token: 0x0400412B RID: 16683
		private MCStyleCustomizationService _style;

		// Token: 0x0400412C RID: 16684
		private static readonly Vector3[] DiffuseDirections = PoiCharacterProbeLighting.CreateDiffuseDirections();

		// Token: 0x0400412D RID: 16685
		private static readonly Color[] DiffuseSamples = new Color[26];

		// Token: 0x0200095A RID: 2394
		private sealed class Original
		{
			// Token: 0x0400412E RID: 16686
			public LightProbeUsage usage;

			// Token: 0x0400412F RID: 16687
			public Transform anchor;

			// Token: 0x04004130 RID: 16688
			public MaterialPropertyBlock globalBlock;

			// Token: 0x04004131 RID: 16689
			public readonly List<MaterialPropertyBlock> materialBlocks = new List<MaterialPropertyBlock>();

			// Token: 0x04004132 RID: 16690
			public bool blocksCaptured;

			// Token: 0x04004133 RID: 16691
			public int materialCount;
		}
	}
}
