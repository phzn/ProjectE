using System;
using System.Collections.Generic;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000046 RID: 70
	[Serializable]
	public struct MultiAimConstraintData : IAnimationJobData, IMultiAimConstraintData
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600016F RID: 367 RVA: 0x000069DB File Offset: 0x00004BDB
		// (set) Token: 0x06000170 RID: 368 RVA: 0x000069E3 File Offset: 0x00004BE3
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

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000171 RID: 369 RVA: 0x000069EC File Offset: 0x00004BEC
		// (set) Token: 0x06000172 RID: 370 RVA: 0x000069F4 File Offset: 0x00004BF4
		public WeightedTransformArray sourceObjects
		{
			get
			{
				return this.m_SourceObjects;
			}
			set
			{
				this.m_SourceObjects = value;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000173 RID: 371 RVA: 0x000069FD File Offset: 0x00004BFD
		// (set) Token: 0x06000174 RID: 372 RVA: 0x00006A05 File Offset: 0x00004C05
		public bool maintainOffset
		{
			get
			{
				return this.m_MaintainOffset;
			}
			set
			{
				this.m_MaintainOffset = value;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00006A0E File Offset: 0x00004C0E
		// (set) Token: 0x06000176 RID: 374 RVA: 0x00006A16 File Offset: 0x00004C16
		public Vector3 offset
		{
			get
			{
				return this.m_Offset;
			}
			set
			{
				this.m_Offset = value;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00006A1F File Offset: 0x00004C1F
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00006A32 File Offset: 0x00004C32
		public Vector2 limits
		{
			get
			{
				return new Vector2(this.m_MinLimit, this.m_MaxLimit);
			}
			set
			{
				this.m_MinLimit = Mathf.Clamp(value.x, -180f, 180f);
				this.m_MaxLimit = Mathf.Clamp(value.y, -180f, 180f);
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00006A6A File Offset: 0x00004C6A
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00006A72 File Offset: 0x00004C72
		public MultiAimConstraintData.Axis aimAxis
		{
			get
			{
				return this.m_AimAxis;
			}
			set
			{
				this.m_AimAxis = value;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00006A7B File Offset: 0x00004C7B
		// (set) Token: 0x0600017C RID: 380 RVA: 0x00006A83 File Offset: 0x00004C83
		public MultiAimConstraintData.Axis upAxis
		{
			get
			{
				return this.m_UpAxis;
			}
			set
			{
				this.m_UpAxis = value;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00006A8C File Offset: 0x00004C8C
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00006A94 File Offset: 0x00004C94
		public MultiAimConstraintData.WorldUpType worldUpType
		{
			get
			{
				return this.m_WorldUpType;
			}
			set
			{
				this.m_WorldUpType = value;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00006A9D File Offset: 0x00004C9D
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00006AA5 File Offset: 0x00004CA5
		public MultiAimConstraintData.Axis worldUpAxis
		{
			get
			{
				return this.m_WorldUpAxis;
			}
			set
			{
				this.m_WorldUpAxis = value;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00006AAE File Offset: 0x00004CAE
		// (set) Token: 0x06000182 RID: 386 RVA: 0x00006AB6 File Offset: 0x00004CB6
		public Transform worldUpObject
		{
			get
			{
				return this.m_WorldUpObject;
			}
			set
			{
				this.m_WorldUpObject = value;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00006ABF File Offset: 0x00004CBF
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00006ACC File Offset: 0x00004CCC
		public bool constrainedXAxis
		{
			get
			{
				return this.m_ConstrainedAxes.x;
			}
			set
			{
				this.m_ConstrainedAxes.x = value;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00006ADA File Offset: 0x00004CDA
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00006AE7 File Offset: 0x00004CE7
		public bool constrainedYAxis
		{
			get
			{
				return this.m_ConstrainedAxes.y;
			}
			set
			{
				this.m_ConstrainedAxes.y = value;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00006AF5 File Offset: 0x00004CF5
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00006B02 File Offset: 0x00004D02
		public bool constrainedZAxis
		{
			get
			{
				return this.m_ConstrainedAxes.z;
			}
			set
			{
				this.m_ConstrainedAxes.z = value;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00006B10 File Offset: 0x00004D10
		Vector3 IMultiAimConstraintData.aimAxis
		{
			get
			{
				return MultiAimConstraintData.Convert(this.m_AimAxis);
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00006B1D File Offset: 0x00004D1D
		Vector3 IMultiAimConstraintData.upAxis
		{
			get
			{
				return MultiAimConstraintData.Convert(this.m_UpAxis);
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00006B2A File Offset: 0x00004D2A
		int IMultiAimConstraintData.worldUpType
		{
			get
			{
				return (int)this.m_WorldUpType;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00006B32 File Offset: 0x00004D32
		Vector3 IMultiAimConstraintData.worldUpAxis
		{
			get
			{
				return MultiAimConstraintData.Convert(this.m_WorldUpAxis);
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00006B3F File Offset: 0x00004D3F
		string IMultiAimConstraintData.offsetVector3Property
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_Offset");
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00006B4B File Offset: 0x00004D4B
		string IMultiAimConstraintData.minLimitFloatProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_MinLimit");
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00006B57 File Offset: 0x00004D57
		string IMultiAimConstraintData.maxLimitFloatProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_MaxLimit");
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00006B63 File Offset: 0x00004D63
		string IMultiAimConstraintData.sourceObjectsProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_SourceObjects");
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00006B70 File Offset: 0x00004D70
		bool IAnimationJobData.IsValid()
		{
			if (this.m_ConstrainedObject == null || this.m_SourceObjects.Count == 0)
			{
				return false;
			}
			using (IEnumerator<WeightedTransform> enumerator = this.m_SourceObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.transform == null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00006BE8 File Offset: 0x00004DE8
		void IAnimationJobData.SetDefaultValues()
		{
			this.m_ConstrainedObject = null;
			this.m_UpAxis = MultiAimConstraintData.Axis.Y;
			this.m_AimAxis = MultiAimConstraintData.Axis.Z;
			this.m_WorldUpType = MultiAimConstraintData.WorldUpType.None;
			this.m_WorldUpAxis = MultiAimConstraintData.Axis.Y;
			this.m_WorldUpObject = null;
			this.m_SourceObjects.Clear();
			this.m_MaintainOffset = false;
			this.m_Offset = Vector3.zero;
			this.m_ConstrainedAxes = new Vector3Bool(true);
			this.m_MinLimit = -180f;
			this.m_MaxLimit = 180f;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00006C60 File Offset: 0x00004E60
		private static Vector3 Convert(MultiAimConstraintData.Axis axis)
		{
			switch (axis)
			{
			case MultiAimConstraintData.Axis.X:
				return Vector3.right;
			case MultiAimConstraintData.Axis.X_NEG:
				return Vector3.left;
			case MultiAimConstraintData.Axis.Y:
				return Vector3.up;
			case MultiAimConstraintData.Axis.Y_NEG:
				return Vector3.down;
			case MultiAimConstraintData.Axis.Z:
				return Vector3.forward;
			case MultiAimConstraintData.Axis.Z_NEG:
				return Vector3.back;
			default:
				return Vector3.up;
			}
		}

		// Token: 0x040000BD RID: 189
		internal const float k_MinAngularLimit = -180f;

		// Token: 0x040000BE RID: 190
		internal const float k_MaxAngularLimit = 180f;

		// Token: 0x040000BF RID: 191
		[SerializeField]
		private Transform m_ConstrainedObject;

		// Token: 0x040000C0 RID: 192
		[SyncSceneToStream]
		[SerializeField]
		[WeightRange(0f, 1f)]
		private WeightedTransformArray m_SourceObjects;

		// Token: 0x040000C1 RID: 193
		[SyncSceneToStream]
		[SerializeField]
		private Vector3 m_Offset;

		// Token: 0x040000C2 RID: 194
		[SyncSceneToStream]
		[SerializeField]
		[Range(-180f, 180f)]
		private float m_MinLimit;

		// Token: 0x040000C3 RID: 195
		[SyncSceneToStream]
		[SerializeField]
		[Range(-180f, 180f)]
		private float m_MaxLimit;

		// Token: 0x040000C4 RID: 196
		[NotKeyable]
		[SerializeField]
		private MultiAimConstraintData.Axis m_AimAxis;

		// Token: 0x040000C5 RID: 197
		[NotKeyable]
		[SerializeField]
		private MultiAimConstraintData.Axis m_UpAxis;

		// Token: 0x040000C6 RID: 198
		[NotKeyable]
		[SerializeField]
		private MultiAimConstraintData.WorldUpType m_WorldUpType;

		// Token: 0x040000C7 RID: 199
		[SyncSceneToStream]
		[SerializeField]
		private Transform m_WorldUpObject;

		// Token: 0x040000C8 RID: 200
		[NotKeyable]
		[SerializeField]
		private MultiAimConstraintData.Axis m_WorldUpAxis;

		// Token: 0x040000C9 RID: 201
		[NotKeyable]
		[SerializeField]
		private bool m_MaintainOffset;

		// Token: 0x040000CA RID: 202
		[NotKeyable]
		[SerializeField]
		private Vector3Bool m_ConstrainedAxes;

		// Token: 0x0200007F RID: 127
		public enum Axis
		{
			// Token: 0x0400015B RID: 347
			X,
			// Token: 0x0400015C RID: 348
			X_NEG,
			// Token: 0x0400015D RID: 349
			Y,
			// Token: 0x0400015E RID: 350
			Y_NEG,
			// Token: 0x0400015F RID: 351
			Z,
			// Token: 0x04000160 RID: 352
			Z_NEG
		}

		// Token: 0x02000080 RID: 128
		public enum WorldUpType
		{
			// Token: 0x04000162 RID: 354
			None,
			// Token: 0x04000163 RID: 355
			SceneUp,
			// Token: 0x04000164 RID: 356
			ObjectUp,
			// Token: 0x04000165 RID: 357
			ObjectRotationUp,
			// Token: 0x04000166 RID: 358
			Vector
		}
	}
}
