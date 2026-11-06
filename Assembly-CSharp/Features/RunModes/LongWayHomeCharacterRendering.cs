using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Features.RunModes
{
	// Token: 0x02000CAF RID: 3247
	[DisallowMultipleComponent]
	public sealed class LongWayHomeCharacterRendering : MonoBehaviour
	{
		// Token: 0x06005E49 RID: 24137 RVA: 0x002546C0 File Offset: 0x002528C0
		private void OnEnable()
		{
			this.LoadShader("lilToon", "CielStableOpaque");
			this.LoadShader("Hidden/lilToonTransparent", "CielStableTransparent");
			this.LoadShader("Hidden/lilToonCutout", "CielStableCutout");
			RenderPipelineManager.beginCameraRendering += this.BeginCamera;
			RenderPipelineManager.endCameraRendering += this.EndCamera;
		}

		// Token: 0x06005E4A RID: 24138 RVA: 0x00254720 File Offset: 0x00252920
		private void LoadShader(string original, string resource)
		{
			Material material = Resources.Load<Material>("RenderingPrecision/" + resource);
			if (material != null && material.shader != null)
			{
				this._shaders[original] = material.shader;
			}
		}

		// Token: 0x06005E4B RID: 24139 RVA: 0x00254768 File Offset: 0x00252968
		private void BeginCamera(ScriptableRenderContext context, Camera camera)
		{
			if (this._renderDepth != 0 && this._renderFrame != Time.frameCount)
			{
				this.RestoreMaterials();
				this._renderDepth = 0;
			}
			this._renderFrame = Time.frameCount;
			int num = this._renderDepth;
			this._renderDepth = num + 1;
			if (num != 0)
			{
				return;
			}
			base.GetComponentsInChildren<Renderer>(true, this._renderers);
			this._bindingCount = 0;
			this._used.Clear();
			foreach (Renderer renderer in this._renderers)
			{
				if (!(renderer == null) && renderer.enabled && renderer.gameObject.activeInHierarchy)
				{
					renderer.GetSharedMaterials(this._current);
					bool flag = false;
					foreach (Material material in this._current)
					{
						if (material != null && material.shader != null && this._shaders.ContainsKey(material.shader.name))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						if (this._bindingCount == this._bindings.Count)
						{
							this._bindings.Add(new LongWayHomeCharacterRendering.Binding());
						}
						List<LongWayHomeCharacterRendering.Binding> bindings = this._bindings;
						num = this._bindingCount;
						this._bindingCount = num + 1;
						LongWayHomeCharacterRendering.Binding binding = bindings[num];
						binding.Renderer = renderer;
						binding.Original.Clear();
						binding.Original.AddRange(this._current);
						binding.Applied.Clear();
						foreach (Material material2 in this._current)
						{
							Material material3 = material2;
							Shader shader;
							if (material2 != null && material2.shader != null && this._shaders.TryGetValue(material2.shader.name, out shader))
							{
								if (!this._copies.TryGetValue(material2, out material3) || material3 == null)
								{
									material3 = new Material(material2)
									{
										name = material2.name + " (Long Way Home projection)",
										hideFlags = HideFlags.HideAndDontSave
									};
									this._copies[material2] = material3;
								}
								if (this._used.Add(material2))
								{
									int num2 = material2.ComputeCRC();
									int num3;
									if (material3.shader != shader || !this._sourceVersions.TryGetValue(material2, out num3) || num3 != num2)
									{
										if (material3.shader != shader)
										{
											material3.shader = shader;
										}
										material3.CopyPropertiesFromMaterial(material2);
										material3.renderQueue = material2.renderQueue;
										this._sourceVersions[material2] = num2;
									}
								}
							}
							binding.Applied.Add(material3);
						}
						renderer.SetSharedMaterials(binding.Applied);
					}
				}
			}
			this._expired.Clear();
			foreach (KeyValuePair<Material, Material> keyValuePair in this._copies)
			{
				if (!this._used.Contains(keyValuePair.Key))
				{
					this._expired.Add(keyValuePair.Key);
				}
			}
			foreach (Material material4 in this._expired)
			{
				LongWayHomeCharacterRendering.DestroyCopy(this._copies[material4]);
				this._copies.Remove(material4);
				this._sourceVersions.Remove(material4);
			}
		}

		// Token: 0x06005E4C RID: 24140 RVA: 0x00254BC0 File Offset: 0x00252DC0
		private void EndCamera(ScriptableRenderContext context, Camera camera)
		{
			if (this._renderDepth != 0)
			{
				int num = this._renderDepth - 1;
				this._renderDepth = num;
				if (num == 0)
				{
					this.RestoreMaterials();
					return;
				}
			}
		}

		// Token: 0x06005E4D RID: 24141 RVA: 0x00254BF0 File Offset: 0x00252DF0
		private void RestoreMaterials()
		{
			for (int i = 0; i < this._bindingCount; i++)
			{
				LongWayHomeCharacterRendering.Binding binding = this._bindings[i];
				if (!(binding.Renderer == null))
				{
					binding.Renderer.GetSharedMaterials(this._current);
					bool flag = false;
					int num = 0;
					while (num < this._current.Count && num < binding.Applied.Count)
					{
						if (this._current[num] == binding.Applied[num] && this._current[num] != binding.Original[num])
						{
							this._current[num] = binding.Original[num];
							flag = true;
						}
						num++;
					}
					if (flag)
					{
						binding.Renderer.SetSharedMaterials(this._current);
					}
				}
			}
			this._bindingCount = 0;
		}

		// Token: 0x06005E4E RID: 24142 RVA: 0x00254CDC File Offset: 0x00252EDC
		private void OnDisable()
		{
			RenderPipelineManager.beginCameraRendering -= this.BeginCamera;
			RenderPipelineManager.endCameraRendering -= this.EndCamera;
			this.RestoreMaterials();
			this._renderDepth = 0;
			foreach (Material material in this._copies.Values)
			{
				LongWayHomeCharacterRendering.DestroyCopy(material);
			}
			this._copies.Clear();
			this._sourceVersions.Clear();
		}

		// Token: 0x06005E4F RID: 24143 RVA: 0x000A4FF8 File Offset: 0x000A31F8
		private static void DestroyCopy(Material material)
		{
			if (material == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(material);
				return;
			}
			Object.DestroyImmediate(material);
		}

		// Token: 0x04005F4A RID: 24394
		private readonly List<Renderer> _renderers = new List<Renderer>();

		// Token: 0x04005F4B RID: 24395
		private readonly List<LongWayHomeCharacterRendering.Binding> _bindings = new List<LongWayHomeCharacterRendering.Binding>();

		// Token: 0x04005F4C RID: 24396
		private readonly List<Material> _current = new List<Material>();

		// Token: 0x04005F4D RID: 24397
		private readonly Dictionary<string, Shader> _shaders = new Dictionary<string, Shader>();

		// Token: 0x04005F4E RID: 24398
		private readonly Dictionary<Material, Material> _copies = new Dictionary<Material, Material>();

		// Token: 0x04005F4F RID: 24399
		private readonly Dictionary<Material, int> _sourceVersions = new Dictionary<Material, int>();

		// Token: 0x04005F50 RID: 24400
		private readonly HashSet<Material> _used = new HashSet<Material>();

		// Token: 0x04005F51 RID: 24401
		private readonly List<Material> _expired = new List<Material>();

		// Token: 0x04005F52 RID: 24402
		private int _renderDepth;

		// Token: 0x04005F53 RID: 24403
		private int _renderFrame = -1;

		// Token: 0x04005F54 RID: 24404
		private int _bindingCount;

		// Token: 0x02000CB0 RID: 3248
		private sealed class Binding
		{
			// Token: 0x04005F55 RID: 24405
			public Renderer Renderer;

			// Token: 0x04005F56 RID: 24406
			public readonly List<Material> Original = new List<Material>();

			// Token: 0x04005F57 RID: 24407
			public readonly List<Material> Applied = new List<Material>();
		}
	}
}
