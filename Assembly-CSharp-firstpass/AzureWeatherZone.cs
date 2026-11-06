using System;

namespace UnityEngine.AzureSky
{
	// Token: 0x02000028 RID: 40
	[AddComponentMenu("Azure[Sky] Dynamic Skybox/Azure Weather Zone")]
	public sealed class AzureWeatherZone : MonoBehaviour
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600025F RID: 607 RVA: 0x00009F9F File Offset: 0x0000819F
		// (set) Token: 0x06000260 RID: 608 RVA: 0x00009FA7 File Offset: 0x000081A7
		public AzureWeatherPreset weatherPreset
		{
			get
			{
				return this.m_weatherPreset;
			}
			set
			{
				this.m_weatherPreset = value;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000261 RID: 609 RVA: 0x00009FB0 File Offset: 0x000081B0
		// (set) Token: 0x06000262 RID: 610 RVA: 0x00009FB8 File Offset: 0x000081B8
		public Collider colliderVolume
		{
			get
			{
				return this.m_colliderVolume;
			}
			set
			{
				this.m_colliderVolume = value;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000263 RID: 611 RVA: 0x00009FC1 File Offset: 0x000081C1
		// (set) Token: 0x06000264 RID: 612 RVA: 0x00009FC9 File Offset: 0x000081C9
		public float blendDistance
		{
			get
			{
				return this.m_blendDistance;
			}
			set
			{
				this.m_blendDistance = value;
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00009FD2 File Offset: 0x000081D2
		private void Awake()
		{
			if (!this.m_colliderVolume)
			{
				this.m_colliderVolume = base.GetComponent<Collider>();
			}
		}

		// Token: 0x040001A8 RID: 424
		[SerializeField]
		private AzureWeatherPreset m_weatherPreset;

		// Token: 0x040001A9 RID: 425
		[SerializeField]
		private Collider m_colliderVolume;

		// Token: 0x040001AA RID: 426
		[SerializeField]
		private float m_blendDistance;
	}
}
