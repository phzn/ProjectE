using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000044 RID: 68
	[Serializable]
	public struct DampedTransformData : IAnimationJobData, IDampedTransformData
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600015F RID: 351 RVA: 0x000068D0 File Offset: 0x00004AD0
		// (set) Token: 0x06000160 RID: 352 RVA: 0x000068D8 File Offset: 0x00004AD8
		public Transform constrainedObject
		{
			get
			{
				return this.m_ConstrainedObject;
			}
			set
			{
				this.m_ConstrainedObject = value;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000161 RID: 353 RVA: 0x000068E1 File Offset: 0x00004AE1
		// (set) Token: 0x06000162 RID: 354 RVA: 0x000068E9 File Offset: 0x00004AE9
		public Transform sourceObject
		{
			get
			{
				return this.m_Source;
			}
			set
			{
				this.m_Source = value;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000163 RID: 355 RVA: 0x000068F2 File Offset: 0x00004AF2
		// (set) Token: 0x06000164 RID: 356 RVA: 0x000068FA File Offset: 0x00004AFA
		public float dampPosition
		{
			get
			{
				return this.m_DampPosition;
			}
			set
			{
				this.m_DampPosition = Mathf.Clamp01(value);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00006908 File Offset: 0x00004B08
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00006910 File Offset: 0x00004B10
		public float dampRotation
		{
			get
			{
				return this.m_DampRotation;
			}
			set
			{
				this.m_DampRotation = Mathf.Clamp01(value);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000167 RID: 359 RVA: 0x0000691E File Offset: 0x00004B1E
		// (set) Token: 0x06000168 RID: 360 RVA: 0x00006926 File Offset: 0x00004B26
		public bool maintainAim
		{
			get
			{
				return this.m_MaintainAim;
			}
			set
			{
				this.m_MaintainAim = value;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000169 RID: 361 RVA: 0x0000692F File Offset: 0x00004B2F
		string IDampedTransformData.dampPositionFloatProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_DampPosition");
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600016A RID: 362 RVA: 0x0000693B File Offset: 0x00004B3B
		string IDampedTransformData.dampRotationFloatProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_DampRotation");
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00006947 File Offset: 0x00004B47
		bool IAnimationJobData.IsValid()
		{
			return !(this.m_ConstrainedObject == null) && !(this.m_Source == null);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00006968 File Offset: 0x00004B68
		void IAnimationJobData.SetDefaultValues()
		{
			this.m_ConstrainedObject = null;
			this.m_Source = null;
			this.m_DampPosition = 0.5f;
			this.m_DampRotation = 0.5f;
			this.m_MaintainAim = true;
		}

		// Token: 0x040000B8 RID: 184
		[SerializeField]
		private Transform m_ConstrainedObject;

		// Token: 0x040000B9 RID: 185
		[SyncSceneToStream]
		[SerializeField]
		private Transform m_Source;

		// Token: 0x040000BA RID: 186
		[SyncSceneToStream]
		[SerializeField]
		[Range(0f, 1f)]
		private float m_DampPosition;

		// Token: 0x040000BB RID: 187
		[SyncSceneToStream]
		[SerializeField]
		[Range(0f, 1f)]
		private float m_DampRotation;

		// Token: 0x040000BC RID: 188
		[NotKeyable]
		[SerializeField]
		private bool m_MaintainAim;
	}
}
