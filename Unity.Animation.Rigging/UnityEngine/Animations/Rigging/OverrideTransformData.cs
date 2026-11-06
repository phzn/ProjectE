using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000050 RID: 80
	[Serializable]
	public struct OverrideTransformData : IAnimationJobData, IOverrideTransformData
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x000073C4 File Offset: 0x000055C4
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x000073CC File Offset: 0x000055CC
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

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x000073D5 File Offset: 0x000055D5
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x000073DD File Offset: 0x000055DD
		public Transform sourceObject
		{
			get
			{
				return this.m_OverrideSource;
			}
			set
			{
				this.m_OverrideSource = value;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x000073E6 File Offset: 0x000055E6
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x000073EE File Offset: 0x000055EE
		public OverrideTransformData.Space space
		{
			get
			{
				return this.m_Space;
			}
			set
			{
				this.m_Space = value;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x000073F7 File Offset: 0x000055F7
		// (set) Token: 0x060001EA RID: 490 RVA: 0x000073FF File Offset: 0x000055FF
		public Vector3 position
		{
			get
			{
				return this.m_OverridePosition;
			}
			set
			{
				this.m_OverridePosition = value;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060001EB RID: 491 RVA: 0x00007408 File Offset: 0x00005608
		// (set) Token: 0x060001EC RID: 492 RVA: 0x00007410 File Offset: 0x00005610
		public Vector3 rotation
		{
			get
			{
				return this.m_OverrideRotation;
			}
			set
			{
				this.m_OverrideRotation = value;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00007419 File Offset: 0x00005619
		// (set) Token: 0x060001EE RID: 494 RVA: 0x00007421 File Offset: 0x00005621
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

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0000742F File Offset: 0x0000562F
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x00007437 File Offset: 0x00005637
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

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x00007445 File Offset: 0x00005645
		int IOverrideTransformData.space
		{
			get
			{
				return (int)this.m_Space;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000744D File Offset: 0x0000564D
		string IOverrideTransformData.positionWeightFloatProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_PositionWeight");
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x00007459 File Offset: 0x00005659
		string IOverrideTransformData.rotationWeightFloatProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_RotationWeight");
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x00007465 File Offset: 0x00005665
		string IOverrideTransformData.positionVector3Property
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_OverridePosition");
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x00007471 File Offset: 0x00005671
		string IOverrideTransformData.rotationVector3Property
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_OverrideRotation");
			}
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000747D File Offset: 0x0000567D
		bool IAnimationJobData.IsValid()
		{
			return this.m_ConstrainedObject != null;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000748C File Offset: 0x0000568C
		void IAnimationJobData.SetDefaultValues()
		{
			this.m_ConstrainedObject = null;
			this.m_OverrideSource = null;
			this.m_OverridePosition = Vector3.zero;
			this.m_OverrideRotation = Vector3.zero;
			this.m_Space = OverrideTransformData.Space.Pivot;
			this.m_PositionWeight = 1f;
			this.m_RotationWeight = 1f;
		}

		// Token: 0x040000DD RID: 221
		[SerializeField]
		private Transform m_ConstrainedObject;

		// Token: 0x040000DE RID: 222
		[SyncSceneToStream]
		[SerializeField]
		private Transform m_OverrideSource;

		// Token: 0x040000DF RID: 223
		[SyncSceneToStream]
		[SerializeField]
		private Vector3 m_OverridePosition;

		// Token: 0x040000E0 RID: 224
		[SyncSceneToStream]
		[SerializeField]
		private Vector3 m_OverrideRotation;

		// Token: 0x040000E1 RID: 225
		[SyncSceneToStream]
		[SerializeField]
		[Range(0f, 1f)]
		private float m_PositionWeight;

		// Token: 0x040000E2 RID: 226
		[SyncSceneToStream]
		[SerializeField]
		[Range(0f, 1f)]
		private float m_RotationWeight;

		// Token: 0x040000E3 RID: 227
		[NotKeyable]
		[SerializeField]
		private OverrideTransformData.Space m_Space;

		// Token: 0x02000081 RID: 129
		[Serializable]
		public enum Space
		{
			// Token: 0x04000168 RID: 360
			World,
			// Token: 0x04000169 RID: 361
			Local,
			// Token: 0x0400016A RID: 362
			Pivot
		}
	}
}
