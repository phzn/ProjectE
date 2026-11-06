using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace UnityEngine.Rendering
{
	// Token: 0x020000E7 RID: 231
	[ExecuteAlways]
	[AddComponentMenu("Miscellaneous/Volume")]
	public class Volume : MonoBehaviour, IVolume
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x0002589A File Offset: 0x00023A9A
		// (set) Token: 0x0600079D RID: 1949 RVA: 0x000258A2 File Offset: 0x00023AA2
		[Tooltip("When enabled, the Volume is applied to the entire Scene.")]
		public bool isGlobal
		{
			get
			{
				return this.m_IsGlobal;
			}
			set
			{
				this.m_IsGlobal = value;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x000258AC File Offset: 0x00023AAC
		// (set) Token: 0x0600079F RID: 1951 RVA: 0x00025958 File Offset: 0x00023B58
		public VolumeProfile profile
		{
			get
			{
				if (this.m_InternalProfile == null)
				{
					this.m_InternalProfile = ScriptableObject.CreateInstance<VolumeProfile>();
					if (this.sharedProfile != null)
					{
						this.m_InternalProfile.name = this.sharedProfile.name;
						foreach (VolumeComponent volumeComponent in this.sharedProfile.components)
						{
							VolumeComponent volumeComponent2 = Object.Instantiate<VolumeComponent>(volumeComponent);
							this.m_InternalProfile.components.Add(volumeComponent2);
						}
					}
				}
				return this.m_InternalProfile;
			}
			set
			{
				this.m_InternalProfile = value;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x00025961 File Offset: 0x00023B61
		public List<Collider> colliders
		{
			get
			{
				return this.m_Colliders;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x00025969 File Offset: 0x00023B69
		internal VolumeProfile profileRef
		{
			get
			{
				if (!(this.m_InternalProfile == null))
				{
					return this.m_InternalProfile;
				}
				return this.sharedProfile;
			}
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x00025986 File Offset: 0x00023B86
		public bool HasInstantiatedProfile()
		{
			return this.m_InternalProfile != null;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x00025994 File Offset: 0x00023B94
		private void OnEnable()
		{
			this.m_PreviousLayer = base.gameObject.layer;
			VolumeManager.instance.Register(this, this.m_PreviousLayer);
			base.GetComponents<Collider>(this.m_Colliders);
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x000259C4 File Offset: 0x00023BC4
		private void OnDisable()
		{
			VolumeManager.instance.Unregister(this, base.gameObject.layer);
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x000259DC File Offset: 0x00023BDC
		private void Update()
		{
			this.UpdateLayer();
			if (this.priority != this.m_PreviousPriority)
			{
				VolumeManager.instance.SetLayerDirty(base.gameObject.layer);
				this.m_PreviousPriority = this.priority;
			}
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00025A14 File Offset: 0x00023C14
		internal void UpdateLayer()
		{
			int layer = base.gameObject.layer;
			if (layer != this.m_PreviousLayer)
			{
				VolumeManager.instance.UpdateVolumeLayer(this, this.m_PreviousLayer, layer);
				this.m_PreviousLayer = layer;
			}
		}

		// Token: 0x040004B8 RID: 1208
		[SerializeField]
		[FormerlySerializedAs("isGlobal")]
		private bool m_IsGlobal = true;

		// Token: 0x040004B9 RID: 1209
		[Tooltip("A value which determines which Volume is being used when Volumes have an equal amount of influence on the Scene. Volumes with a higher priority will override lower ones.")]
		[Delayed]
		public float priority;

		// Token: 0x040004BA RID: 1210
		[Tooltip("Sets the outer distance to start blending from. A value of 0 means no blending and Unity applies the Volume overrides immediately upon entry.")]
		public float blendDistance;

		// Token: 0x040004BB RID: 1211
		[Range(0f, 1f)]
		[Tooltip("Sets the total weight of this Volume in the Scene. 0 means no effect and 1 means full effect.")]
		public float weight = 1f;

		// Token: 0x040004BC RID: 1212
		public VolumeProfile sharedProfile;

		// Token: 0x040004BD RID: 1213
		internal List<Collider> m_Colliders = new List<Collider>();

		// Token: 0x040004BE RID: 1214
		private int m_PreviousLayer;

		// Token: 0x040004BF RID: 1215
		private float m_PreviousPriority;

		// Token: 0x040004C0 RID: 1216
		private VolumeProfile m_InternalProfile;
	}
}
