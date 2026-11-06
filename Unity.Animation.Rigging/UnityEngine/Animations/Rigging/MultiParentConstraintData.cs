using System;
using System.Collections.Generic;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000048 RID: 72
	[Serializable]
	public struct MultiParentConstraintData : IAnimationJobData, IMultiParentConstraintData
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00006D4C File Offset: 0x00004F4C
		// (set) Token: 0x06000197 RID: 407 RVA: 0x00006D54 File Offset: 0x00004F54
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

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00006D5D File Offset: 0x00004F5D
		// (set) Token: 0x06000199 RID: 409 RVA: 0x00006D65 File Offset: 0x00004F65
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

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00006D6E File Offset: 0x00004F6E
		// (set) Token: 0x0600019B RID: 411 RVA: 0x00006D76 File Offset: 0x00004F76
		public bool maintainPositionOffset
		{
			get
			{
				return this.m_MaintainPositionOffset;
			}
			set
			{
				this.m_MaintainPositionOffset = value;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00006D7F File Offset: 0x00004F7F
		// (set) Token: 0x0600019D RID: 413 RVA: 0x00006D87 File Offset: 0x00004F87
		public bool maintainRotationOffset
		{
			get
			{
				return this.m_MaintainRotationOffset;
			}
			set
			{
				this.m_MaintainRotationOffset = value;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00006D90 File Offset: 0x00004F90
		// (set) Token: 0x0600019F RID: 415 RVA: 0x00006D9D File Offset: 0x00004F9D
		public bool constrainedPositionXAxis
		{
			get
			{
				return this.m_ConstrainedPositionAxes.x;
			}
			set
			{
				this.m_ConstrainedPositionAxes.x = value;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00006DAB File Offset: 0x00004FAB
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x00006DB8 File Offset: 0x00004FB8
		public bool constrainedPositionYAxis
		{
			get
			{
				return this.m_ConstrainedPositionAxes.y;
			}
			set
			{
				this.m_ConstrainedPositionAxes.y = value;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00006DC6 File Offset: 0x00004FC6
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x00006DD3 File Offset: 0x00004FD3
		public bool constrainedPositionZAxis
		{
			get
			{
				return this.m_ConstrainedPositionAxes.z;
			}
			set
			{
				this.m_ConstrainedPositionAxes.z = value;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00006DE1 File Offset: 0x00004FE1
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x00006DEE File Offset: 0x00004FEE
		public bool constrainedRotationXAxis
		{
			get
			{
				return this.m_ConstrainedRotationAxes.x;
			}
			set
			{
				this.m_ConstrainedRotationAxes.x = value;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00006DFC File Offset: 0x00004FFC
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00006E09 File Offset: 0x00005009
		public bool constrainedRotationYAxis
		{
			get
			{
				return this.m_ConstrainedRotationAxes.y;
			}
			set
			{
				this.m_ConstrainedRotationAxes.y = value;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00006E17 File Offset: 0x00005017
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00006E24 File Offset: 0x00005024
		public bool constrainedRotationZAxis
		{
			get
			{
				return this.m_ConstrainedRotationAxes.z;
			}
			set
			{
				this.m_ConstrainedRotationAxes.z = value;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00006E32 File Offset: 0x00005032
		string IMultiParentConstraintData.sourceObjectsProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_SourceObjects");
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00006E40 File Offset: 0x00005040
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

		// Token: 0x060001AC RID: 428 RVA: 0x00006EB8 File Offset: 0x000050B8
		void IAnimationJobData.SetDefaultValues()
		{
			this.m_ConstrainedObject = null;
			this.m_ConstrainedPositionAxes = new Vector3Bool(true);
			this.m_ConstrainedRotationAxes = new Vector3Bool(true);
			this.m_SourceObjects.Clear();
			this.m_MaintainPositionOffset = false;
			this.m_MaintainRotationOffset = false;
		}

		// Token: 0x040000CB RID: 203
		[SerializeField]
		private Transform m_ConstrainedObject;

		// Token: 0x040000CC RID: 204
		[SerializeField]
		[SyncSceneToStream]
		[WeightRange(0f, 1f)]
		private WeightedTransformArray m_SourceObjects;

		// Token: 0x040000CD RID: 205
		[NotKeyable]
		[SerializeField]
		private Vector3Bool m_ConstrainedPositionAxes;

		// Token: 0x040000CE RID: 206
		[NotKeyable]
		[SerializeField]
		private Vector3Bool m_ConstrainedRotationAxes;

		// Token: 0x040000CF RID: 207
		[NotKeyable]
		[SerializeField]
		private bool m_MaintainPositionOffset;

		// Token: 0x040000D0 RID: 208
		[NotKeyable]
		[SerializeField]
		private bool m_MaintainRotationOffset;
	}
}
