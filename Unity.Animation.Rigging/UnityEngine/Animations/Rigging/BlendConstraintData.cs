using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000040 RID: 64
	[Serializable]
	public struct BlendConstraintData : IAnimationJobData, IBlendConstraintData
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600012D RID: 301 RVA: 0x000064F5 File Offset: 0x000046F5
		// (set) Token: 0x0600012E RID: 302 RVA: 0x000064FD File Offset: 0x000046FD
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

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00006506 File Offset: 0x00004706
		// (set) Token: 0x06000130 RID: 304 RVA: 0x0000650E File Offset: 0x0000470E
		public Transform sourceObjectA
		{
			get
			{
				return this.m_SourceA;
			}
			set
			{
				this.m_SourceA = value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00006517 File Offset: 0x00004717
		// (set) Token: 0x06000132 RID: 306 RVA: 0x0000651F File Offset: 0x0000471F
		public Transform sourceObjectB
		{
			get
			{
				return this.m_SourceB;
			}
			set
			{
				this.m_SourceB = value;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00006528 File Offset: 0x00004728
		// (set) Token: 0x06000134 RID: 308 RVA: 0x00006530 File Offset: 0x00004730
		public bool blendPosition
		{
			get
			{
				return this.m_BlendPosition;
			}
			set
			{
				this.m_BlendPosition = value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00006539 File Offset: 0x00004739
		// (set) Token: 0x06000136 RID: 310 RVA: 0x00006541 File Offset: 0x00004741
		public bool blendRotation
		{
			get
			{
				return this.m_BlendRotation;
			}
			set
			{
				this.m_BlendRotation = value;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000137 RID: 311 RVA: 0x0000654A File Offset: 0x0000474A
		// (set) Token: 0x06000138 RID: 312 RVA: 0x00006552 File Offset: 0x00004752
		public float positionWeight
		{
			get
			{
				return this.m_PositionWeight;
			}
			set
			{
				this.m_PositionWeight = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00006560 File Offset: 0x00004760
		// (set) Token: 0x0600013A RID: 314 RVA: 0x00006568 File Offset: 0x00004768
		public float rotationWeight
		{
			get
			{
				return this.m_RotationWeight;
			}
			set
			{
				this.m_RotationWeight = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00006576 File Offset: 0x00004776
		// (set) Token: 0x0600013C RID: 316 RVA: 0x0000657E File Offset: 0x0000477E
		public bool maintainPositionOffsets
		{
			get
			{
				return this.m_MaintainPositionOffsets;
			}
			set
			{
				this.m_MaintainPositionOffsets = value;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00006587 File Offset: 0x00004787
		// (set) Token: 0x0600013E RID: 318 RVA: 0x0000658F File Offset: 0x0000478F
		public bool maintainRotationOffsets
		{
			get
			{
				return this.m_MaintainRotationOffsets;
			}
			set
			{
				this.m_MaintainRotationOffsets = value;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00006598 File Offset: 0x00004798
		string IBlendConstraintData.blendPositionBoolProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_BlendPosition");
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000140 RID: 320 RVA: 0x000065A4 File Offset: 0x000047A4
		string IBlendConstraintData.blendRotationBoolProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_BlendRotation");
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000141 RID: 321 RVA: 0x000065B0 File Offset: 0x000047B0
		string IBlendConstraintData.positionWeightFloatProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_PositionWeight");
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000142 RID: 322 RVA: 0x000065BC File Offset: 0x000047BC
		string IBlendConstraintData.rotationWeightFloatProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_RotationWeight");
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000065C8 File Offset: 0x000047C8
		bool IAnimationJobData.IsValid()
		{
			return !(this.m_ConstrainedObject == null) && !(this.m_SourceA == null) && !(this.m_SourceB == null);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000065F8 File Offset: 0x000047F8
		void IAnimationJobData.SetDefaultValues()
		{
			this.m_ConstrainedObject = null;
			this.m_SourceA = null;
			this.m_SourceB = null;
			this.m_BlendPosition = true;
			this.m_BlendRotation = true;
			this.m_PositionWeight = 0.5f;
			this.m_RotationWeight = 0.5f;
			this.m_MaintainPositionOffsets = false;
			this.m_MaintainRotationOffsets = false;
		}

		// Token: 0x040000A2 RID: 162
		[SerializeField]
		private Transform m_ConstrainedObject;

		// Token: 0x040000A3 RID: 163
		[SyncSceneToStream]
		[SerializeField]
		private Transform m_SourceA;

		// Token: 0x040000A4 RID: 164
		[SyncSceneToStream]
		[SerializeField]
		private Transform m_SourceB;

		// Token: 0x040000A5 RID: 165
		[SyncSceneToStream]
		[SerializeField]
		private bool m_BlendPosition;

		// Token: 0x040000A6 RID: 166
		[SyncSceneToStream]
		[SerializeField]
		private bool m_BlendRotation;

		// Token: 0x040000A7 RID: 167
		[SyncSceneToStream]
		[SerializeField]
		[Range(0f, 1f)]
		private float m_PositionWeight;

		// Token: 0x040000A8 RID: 168
		[SyncSceneToStream]
		[SerializeField]
		[Range(0f, 1f)]
		private float m_RotationWeight;

		// Token: 0x040000A9 RID: 169
		[NotKeyable]
		[SerializeField]
		private bool m_MaintainPositionOffsets;

		// Token: 0x040000AA RID: 170
		[NotKeyable]
		[SerializeField]
		private bool m_MaintainRotationOffsets;
	}
}
