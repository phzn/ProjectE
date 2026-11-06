using System;
using System.Collections.Generic;

namespace UnityEngine.AzureSky
{
	// Token: 0x02000018 RID: 24
	[RequireComponent(typeof(ParticleSystem))]
	[AddComponentMenu("Azure[Sky] Dynamic Skybox/Azure Particle FX")]
	public sealed class AzureParticleFX : MonoBehaviour
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600008D RID: 141 RVA: 0x0000354F File Offset: 0x0000174F
		public static IReadOnlyList<AzureParticleFX> activeInstances
		{
			get
			{
				return AzureParticleFX.s_activeInstances;
			}
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x0600008E RID: 142 RVA: 0x00003558 File Offset: 0x00001758
		// (remove) Token: 0x0600008F RID: 143 RVA: 0x0000358C File Offset: 0x0000178C
		public static event Action<AzureParticleFX> InstanceEnabled;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000090 RID: 144 RVA: 0x000035C0 File Offset: 0x000017C0
		// (remove) Token: 0x06000091 RID: 145 RVA: 0x000035F4 File Offset: 0x000017F4
		public static event Action<AzureParticleFX> InstanceDisabled;

		// Token: 0x06000092 RID: 146 RVA: 0x00003627 File Offset: 0x00001827
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetInstances()
		{
			AzureParticleFX.s_activeInstances.Clear();
			AzureParticleFX.InstanceEnabled = (AzureParticleFX.InstanceDisabled = null);
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000093 RID: 147 RVA: 0x0000363F File Offset: 0x0000183F
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00003647 File Offset: 0x00001847
		public float intensity
		{
			get
			{
				return this.m_intensity;
			}
			set
			{
				this.m_intensity = value;
				this.ApplyIntensity();
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003656 File Offset: 0x00001856
		private void Awake()
		{
			this.m_particleSystem = base.GetComponent<ParticleSystem>();
			if (this.m_particleSystem)
			{
				this.m_particleEmission = this.m_particleSystem.emission;
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003684 File Offset: 0x00001884
		private void OnEnable()
		{
			if (!this.m_particleSystem)
			{
				this.m_particleSystem = base.GetComponent<ParticleSystem>();
			}
			if (this.m_particleSystem)
			{
				this.m_particleEmission = this.m_particleSystem.emission;
			}
			if (!AzureParticleFX.s_activeInstances.Contains(this))
			{
				AzureParticleFX.s_activeInstances.Add(this);
			}
			Action<AzureParticleFX> instanceEnabled = AzureParticleFX.InstanceEnabled;
			if (instanceEnabled != null)
			{
				instanceEnabled(this);
			}
			if (this.m_emissionSuppressed && this.m_particleSystem)
			{
				this.m_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
			this.ApplyIntensity();
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003719 File Offset: 0x00001919
		private void OnDisable()
		{
			AzureParticleFX.s_activeInstances.Remove(this);
			Action<AzureParticleFX> instanceDisabled = AzureParticleFX.InstanceDisabled;
			if (instanceDisabled == null)
			{
				return;
			}
			instanceDisabled(this);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003737 File Offset: 0x00001937
		public void SetEmissionSuppressed(bool suppressed)
		{
			if (this.m_emissionSuppressed == suppressed)
			{
				return;
			}
			this.m_emissionSuppressed = suppressed;
			if (suppressed && this.m_particleSystem)
			{
				this.m_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
			this.ApplyIntensity();
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003770 File Offset: 0x00001970
		private void ApplyIntensity()
		{
			if (!this.m_particleSystem)
			{
				return;
			}
			this.m_particleEmission.rateOverTimeMultiplier = (this.m_emissionSuppressed ? 0f : this.m_intensity);
			if (this.m_emissionSuppressed || !base.isActiveAndEnabled)
			{
				return;
			}
			if (this.m_intensity > 0f)
			{
				if (!this.m_particleSystem.isPlaying)
				{
					this.m_particleSystem.Play();
					return;
				}
			}
			else if (this.m_particleSystem.isPlaying)
			{
				this.m_particleSystem.Stop();
			}
		}

		// Token: 0x0400007C RID: 124
		private static readonly List<AzureParticleFX> s_activeInstances = new List<AzureParticleFX>();

		// Token: 0x0400007F RID: 127
		private bool m_emissionSuppressed;

		// Token: 0x04000080 RID: 128
		private ParticleSystem m_particleSystem;

		// Token: 0x04000081 RID: 129
		private ParticleSystem.EmissionModule m_particleEmission;

		// Token: 0x04000082 RID: 130
		[SerializeField]
		private float m_intensity;
	}
}
