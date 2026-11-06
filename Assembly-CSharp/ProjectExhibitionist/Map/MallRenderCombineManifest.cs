using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.Map
{
	// Token: 0x020009CC RID: 2508
	[ExecuteAlways]
	[DisallowMultipleComponent]
	public sealed class MallRenderCombineManifest : MonoBehaviour
	{
		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x06004688 RID: 18056 RVA: 0x001AB039 File Offset: 0x001A9239
		public IReadOnlyList<MeshRenderer> SourceRenderers
		{
			get
			{
				return this.sourceRenderers;
			}
		}

		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x06004689 RID: 18057 RVA: 0x001AB041 File Offset: 0x001A9241
		public int SourceSubmeshes
		{
			get
			{
				return this.sourceSubmeshes;
			}
		}

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x0600468A RID: 18058 RVA: 0x001AB049 File Offset: 0x001A9249
		public int CombinedDraws
		{
			get
			{
				return this.combinedDraws;
			}
		}

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x0600468B RID: 18059 RVA: 0x001AB051 File Offset: 0x001A9251
		public IReadOnlyList<Mesh> GeneratedMeshes
		{
			get
			{
				return this.generatedMeshes;
			}
		}

		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x0600468C RID: 18060 RVA: 0x001AB059 File Offset: 0x001A9259
		public string GeneratedAssetFolder
		{
			get
			{
				return this.generatedAssetFolder;
			}
		}

		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x0600468D RID: 18061 RVA: 0x001AB061 File Offset: 0x001A9261
		public IReadOnlyList<MallRenderCombineManifest.LightingBinding> LightingBindings
		{
			get
			{
				return this.lightingBindings;
			}
		}

		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x0600468E RID: 18062 RVA: 0x001AB069 File Offset: 0x001A9269
		public bool LightingBindingsConfigured
		{
			get
			{
				return this.lightingBindingsConfigured;
			}
		}

		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x0600468F RID: 18063 RVA: 0x001AB074 File Offset: 0x001A9274
		public int ValidLightingBindingCount
		{
			get
			{
				LightmapData[] lightmaps = LightmapSettings.lightmaps;
				int num = 0;
				foreach (MallRenderCombineManifest.LightingBinding lightingBinding in this.lightingBindings)
				{
					if (lightingBinding != null && !(lightingBinding.Renderer == null))
					{
						int num2 = lightingBinding.ResolveIndex(lightmaps);
						if (num2 >= 0 && lightingBinding.Renderer.lightmapIndex == num2 && lightingBinding.Renderer.lightmapScaleOffset == new Vector4(1f, 1f, 0f, 0f))
						{
							num++;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x06004690 RID: 18064 RVA: 0x001AB128 File Offset: 0x001A9328
		public bool OriginalEnabledState(int index)
		{
			return index >= this.originalEnabledStates.Count || this.originalEnabledStates[index];
		}

		// Token: 0x06004691 RID: 18065 RVA: 0x001AB148 File Offset: 0x001A9348
		public void Configure(IEnumerable<MeshRenderer> sources, int sourceDrawCount, int outputDrawCount, string assetFolder = null, IEnumerable<Mesh> meshes = null)
		{
			this.sourceRenderers = new List<MeshRenderer>(sources);
			this.sourceSubmeshes = sourceDrawCount;
			this.combinedDraws = outputDrawCount;
			this.originalEnabledStates = this.sourceRenderers.ConvertAll<bool>((MeshRenderer renderer) => renderer != null && renderer.enabled);
			this.generatedAssetFolder = assetFolder;
			this.generatedMeshes = ((meshes != null) ? new List<Mesh>(meshes) : new List<Mesh>());
		}

		// Token: 0x06004692 RID: 18066 RVA: 0x001AB1BF File Offset: 0x001A93BF
		public void ConfigureLightingBindings(IEnumerable<MallRenderCombineManifest.LightingBinding> bindings)
		{
			this.lightingBindings = new List<MallRenderCombineManifest.LightingBinding>(bindings);
			this.lightingBindingsConfigured = true;
			this.ApplyLightingBindings();
		}

		// Token: 0x06004693 RID: 18067 RVA: 0x001AB1DC File Offset: 0x001A93DC
		public int ApplyLightingBindings()
		{
			LightmapData[] lightmaps = LightmapSettings.lightmaps;
			int num = 0;
			foreach (MallRenderCombineManifest.LightingBinding lightingBinding in this.lightingBindings)
			{
				if (lightingBinding != null && !(lightingBinding.Renderer == null))
				{
					int num2 = lightingBinding.ResolveIndex(lightmaps);
					lightingBinding.Renderer.lightmapIndex = num2;
					lightingBinding.Renderer.lightmapScaleOffset = new Vector4(1f, 1f, 0f, 0f);
					if (num2 >= 0)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06004694 RID: 18068 RVA: 0x001AB288 File Offset: 0x001A9488
		private void OnEnable()
		{
			SceneManager.sceneLoaded += this.OnSceneLoaded;
			SceneManager.sceneUnloaded += this.OnSceneUnloaded;
			this.ScheduleLightingRefresh();
		}

		// Token: 0x06004695 RID: 18069 RVA: 0x001AB2B2 File Offset: 0x001A94B2
		private void Start()
		{
			this.ScheduleLightingRefresh();
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x001AB2BA File Offset: 0x001A94BA
		private void OnDisable()
		{
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
			SceneManager.sceneUnloaded -= this.OnSceneUnloaded;
			if (this.lightingRetry != null)
			{
				base.StopCoroutine(this.lightingRetry);
			}
			this.lightingRetry = null;
		}

		// Token: 0x06004697 RID: 18071 RVA: 0x001AB2B2 File Offset: 0x001A94B2
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			this.ScheduleLightingRefresh();
		}

		// Token: 0x06004698 RID: 18072 RVA: 0x001AB2B2 File Offset: 0x001A94B2
		private void OnSceneUnloaded(Scene scene)
		{
			this.ScheduleLightingRefresh();
		}

		// Token: 0x06004699 RID: 18073 RVA: 0x001AB2FC File Offset: 0x001A94FC
		private void ScheduleLightingRefresh()
		{
			this.ApplyLightingBindings();
			if (Application.IsPlaying(base.gameObject) && base.isActiveAndEnabled)
			{
				if (this.lightingRetry != null)
				{
					base.StopCoroutine(this.lightingRetry);
				}
				this.lightingRetry = base.StartCoroutine(this.RefreshAfterSceneLayout());
			}
		}

		// Token: 0x0600469A RID: 18074 RVA: 0x001AB34B File Offset: 0x001A954B
		private IEnumerator RefreshAfterSceneLayout()
		{
			int num;
			for (int attempt = 0; attempt < 3; attempt = num + 1)
			{
				yield return null;
				this.ApplyLightingBindings();
				num = attempt;
			}
			this.lightingRetry = null;
			yield break;
		}

		// Token: 0x0400462B RID: 17963
		[SerializeField]
		private List<MeshRenderer> sourceRenderers = new List<MeshRenderer>();

		// Token: 0x0400462C RID: 17964
		[SerializeField]
		private int sourceSubmeshes;

		// Token: 0x0400462D RID: 17965
		[SerializeField]
		private int combinedDraws;

		// Token: 0x0400462E RID: 17966
		[SerializeField]
		private List<bool> originalEnabledStates = new List<bool>();

		// Token: 0x0400462F RID: 17967
		[SerializeField]
		private List<Mesh> generatedMeshes = new List<Mesh>();

		// Token: 0x04004630 RID: 17968
		[SerializeField]
		private string generatedAssetFolder;

		// Token: 0x04004631 RID: 17969
		[SerializeField]
		private List<MallRenderCombineManifest.LightingBinding> lightingBindings = new List<MallRenderCombineManifest.LightingBinding>();

		// Token: 0x04004632 RID: 17970
		[SerializeField]
		private bool lightingBindingsConfigured;

		// Token: 0x04004633 RID: 17971
		private Coroutine lightingRetry;

		// Token: 0x020009CD RID: 2509
		[Serializable]
		public sealed class LightingBinding
		{
			// Token: 0x17000C05 RID: 3077
			// (get) Token: 0x0600469C RID: 18076 RVA: 0x001AB38E File Offset: 0x001A958E
			public MeshRenderer Renderer
			{
				get
				{
					return this.renderer;
				}
			}

			// Token: 0x0600469D RID: 18077 RVA: 0x001AB396 File Offset: 0x001A9596
			public LightingBinding(MeshRenderer renderer, MeshRenderer source, Texture2D lightmapColor, Texture2D lightmapDir, Texture2D shadowMask)
			{
				this.renderer = renderer;
				this.source = source;
				this.lightmapColor = lightmapColor;
				this.lightmapDir = lightmapDir;
				this.shadowMask = shadowMask;
			}

			// Token: 0x0600469E RID: 18078 RVA: 0x001AB3C4 File Offset: 0x001A95C4
			internal int ResolveIndex(LightmapData[] maps)
			{
				if (this.lightmapColor == null)
				{
					return -1;
				}
				int num = ((this.source != null) ? this.source.lightmapIndex : (-1));
				if (num >= 0 && num < maps.Length && this.Matches(maps[num]))
				{
					return num;
				}
				for (int i = 0; i < maps.Length; i++)
				{
					if (this.Matches(maps[i]))
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x0600469F RID: 18079 RVA: 0x001AB431 File Offset: 0x001A9631
			private bool Matches(LightmapData map)
			{
				return map != null && map.lightmapColor == this.lightmapColor && map.lightmapDir == this.lightmapDir && map.shadowMask == this.shadowMask;
			}

			// Token: 0x04004634 RID: 17972
			[SerializeField]
			private MeshRenderer renderer;

			// Token: 0x04004635 RID: 17973
			[SerializeField]
			private MeshRenderer source;

			// Token: 0x04004636 RID: 17974
			[SerializeField]
			private Texture2D lightmapColor;

			// Token: 0x04004637 RID: 17975
			[SerializeField]
			private Texture2D lightmapDir;

			// Token: 0x04004638 RID: 17976
			[SerializeField]
			private Texture2D shadowMask;
		}
	}
}
