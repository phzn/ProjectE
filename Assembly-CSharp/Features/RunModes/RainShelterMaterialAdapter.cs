using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AzureSky;

namespace Features.RunModes
{
	// Token: 0x02000D4C RID: 3404
	public sealed class RainShelterMaterialAdapter : MonoBehaviour
	{
		// Token: 0x060063F8 RID: 25592 RVA: 0x002813BD File Offset: 0x0027F5BD
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void EnsureRuntime()
		{
			if (RainShelterMaterialAdapter._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[RainShelterMaterialAdapter]");
			Object.DontDestroyOnLoad(gameObject);
			gameObject.AddComponent<RainShelterMaterialAdapter>();
		}

		// Token: 0x060063F9 RID: 25593 RVA: 0x002813E3 File Offset: 0x0027F5E3
		private void Awake()
		{
			RainShelterMaterialAdapter._instance = this;
		}

		// Token: 0x060063FA RID: 25594 RVA: 0x002813EC File Offset: 0x0027F5EC
		private void OnEnable()
		{
			RainShelterRuntime.VisualMaskChanged -= this.Refresh;
			RainShelterRuntime.VisualMaskChanged += this.Refresh;
			AzureParticleFX.InstanceEnabled -= this.OnParticleEnabled;
			AzureParticleFX.InstanceEnabled += this.OnParticleEnabled;
			AzureParticleFX.InstanceDisabled -= this.OnParticleDisabled;
			AzureParticleFX.InstanceDisabled += this.OnParticleDisabled;
			this.Refresh();
		}

		// Token: 0x060063FB RID: 25595 RVA: 0x00281465 File Offset: 0x0027F665
		private void OnParticleEnabled(AzureParticleFX fx)
		{
			this.Refresh();
		}

		// Token: 0x060063FC RID: 25596 RVA: 0x0028146D File Offset: 0x0027F66D
		private void OnParticleDisabled(AzureParticleFX fx)
		{
			this.RestoreMaterials();
			this.Refresh();
		}

		// Token: 0x060063FD RID: 25597 RVA: 0x0028147C File Offset: 0x0027F67C
		private void Refresh()
		{
			if (!RainShelterRuntime.UsesVolumeMask)
			{
				this.RestoreMaterials();
				return;
			}
			if (this._shelterShader == null)
			{
				this._shelterShader = Resources.Load<Shader>("RainShelterParticles");
			}
			if (this._shelterShader == null)
			{
				return;
			}
			IReadOnlyList<AzureParticleFX> activeInstances = AzureParticleFX.activeInstances;
			for (int i = 0; i < activeInstances.Count; i++)
			{
				this.BindParticle(activeInstances[i]);
			}
			if (this._snapshots.Count > 0 && !this._watchingOverrides)
			{
				AzureNotificationCenter.OnAfterOverrideUpdate += this.OnAfterOverrideUpdate;
				this._watchingOverrides = true;
			}
		}

		// Token: 0x060063FE RID: 25598 RVA: 0x00281518 File Offset: 0x0027F718
		private void BindParticle(AzureParticleFX fx)
		{
			if (fx == null || !this._boundParticles.Add(fx))
			{
				return;
			}
			foreach (ParticleSystemRenderer particleSystemRenderer in fx.GetComponentsInChildren<ParticleSystemRenderer>(true))
			{
				if (!this._renderers.Contains(particleSystemRenderer))
				{
					Material[] sharedMaterials = particleSystemRenderer.sharedMaterials;
					bool flag = false;
					for (int j = 0; j < sharedMaterials.Length; j++)
					{
						Material material = this.CloneMaterial(sharedMaterials[j]);
						if (!(material == null))
						{
							sharedMaterials[j] = material;
							flag = true;
						}
					}
					if (flag)
					{
						particleSystemRenderer.sharedMaterials = sharedMaterials;
						this._renderers.Add(particleSystemRenderer);
					}
				}
			}
		}

		// Token: 0x060063FF RID: 25599 RVA: 0x002815B8 File Offset: 0x0027F7B8
		private Material CloneMaterial(Material authored)
		{
			if (authored == null || authored.shader == null || authored.shader.name != "Azure[Sky] Dynamic Skybox/Particles Additive")
			{
				return null;
			}
			for (int i = 0; i < this._snapshots.Count; i++)
			{
				if (this._snapshots[i].authored == authored)
				{
					return this._snapshots[i].runtime;
				}
			}
			Material material = new Material(authored)
			{
				name = authored.name + " (Runtime rain shelter)",
				hideFlags = HideFlags.DontSave,
				shader = this._shelterShader
			};
			this._snapshots.Add(new RainShelterMaterialAdapter.MaterialSnapshot
			{
				authored = authored,
				runtime = material
			});
			return material;
		}

		// Token: 0x06006400 RID: 25600 RVA: 0x0028168C File Offset: 0x0027F88C
		private void OnAfterOverrideUpdate(AzureWeatherSystem weatherSystem)
		{
			for (int i = 0; i < this._snapshots.Count; i++)
			{
				RainShelterMaterialAdapter.MaterialSnapshot materialSnapshot = this._snapshots[i];
				if (!(materialSnapshot.authored == null) && !(materialSnapshot.runtime == null))
				{
					Color color = materialSnapshot.authored.GetColor(RainShelterMaterialAdapter.TintColorId);
					if (materialSnapshot.runtime.GetColor(RainShelterMaterialAdapter.TintColorId) != color)
					{
						materialSnapshot.runtime.SetColor(RainShelterMaterialAdapter.TintColorId, color);
					}
				}
			}
		}

		// Token: 0x06006401 RID: 25601 RVA: 0x00281714 File Offset: 0x0027F914
		private void RestoreMaterials()
		{
			if (this._watchingOverrides)
			{
				AzureNotificationCenter.OnAfterOverrideUpdate -= this.OnAfterOverrideUpdate;
				this._watchingOverrides = false;
			}
			foreach (ParticleSystemRenderer particleSystemRenderer in this._renderers)
			{
				if (!(particleSystemRenderer == null))
				{
					Material[] sharedMaterials = particleSystemRenderer.sharedMaterials;
					bool flag = false;
					for (int i = 0; i < sharedMaterials.Length; i++)
					{
						for (int j = 0; j < this._snapshots.Count; j++)
						{
							if (sharedMaterials[i] == this._snapshots[j].runtime)
							{
								sharedMaterials[i] = this._snapshots[j].authored;
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						particleSystemRenderer.sharedMaterials = sharedMaterials;
					}
				}
			}
			this._renderers.Clear();
			this._boundParticles.Clear();
			for (int k = 0; k < this._snapshots.Count; k++)
			{
				if (this._snapshots[k].runtime != null)
				{
					if (Application.isPlaying)
					{
						Object.Destroy(this._snapshots[k].runtime);
					}
					else
					{
						Object.DestroyImmediate(this._snapshots[k].runtime);
					}
				}
			}
			this._snapshots.Clear();
		}

		// Token: 0x06006402 RID: 25602 RVA: 0x00281894 File Offset: 0x0027FA94
		private void OnDisable()
		{
			RainShelterRuntime.VisualMaskChanged -= this.Refresh;
			AzureParticleFX.InstanceEnabled -= this.OnParticleEnabled;
			AzureParticleFX.InstanceDisabled -= this.OnParticleDisabled;
			this.RestoreMaterials();
		}

		// Token: 0x06006403 RID: 25603 RVA: 0x002818CF File Offset: 0x0027FACF
		private void OnDestroy()
		{
			this.OnDisable();
			if (RainShelterMaterialAdapter._instance == this)
			{
				RainShelterMaterialAdapter._instance = null;
			}
		}

		// Token: 0x04006700 RID: 26368
		private static RainShelterMaterialAdapter _instance;

		// Token: 0x04006701 RID: 26369
		private static readonly int TintColorId = Shader.PropertyToID("_TintColor");

		// Token: 0x04006702 RID: 26370
		private readonly List<RainShelterMaterialAdapter.MaterialSnapshot> _snapshots = new List<RainShelterMaterialAdapter.MaterialSnapshot>(4);

		// Token: 0x04006703 RID: 26371
		private readonly List<ParticleSystemRenderer> _renderers = new List<ParticleSystemRenderer>(5);

		// Token: 0x04006704 RID: 26372
		private readonly HashSet<AzureParticleFX> _boundParticles = new HashSet<AzureParticleFX>();

		// Token: 0x04006705 RID: 26373
		private Shader _shelterShader;

		// Token: 0x04006706 RID: 26374
		private bool _watchingOverrides;

		// Token: 0x02000D4D RID: 3405
		private struct MaterialSnapshot
		{
			// Token: 0x04006707 RID: 26375
			public Material authored;

			// Token: 0x04006708 RID: 26376
			public Material runtime;
		}
	}
}
