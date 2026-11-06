using System;
using System.Collections.Generic;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200004A RID: 74
	[Serializable]
	public struct MultiPositionConstraintData : IAnimationJobData, IMultiPositionConstraintData
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00006F38 File Offset: 0x00005138
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x00006F40 File Offset: 0x00005140
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

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00006F49 File Offset: 0x00005149
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x00006F51 File Offset: 0x00005151
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

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00006F5A File Offset: 0x0000515A
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x00006F62 File Offset: 0x00005162
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

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00006F6B File Offset: 0x0000516B
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x00006F73 File Offset: 0x00005173
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

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00006F7C File Offset: 0x0000517C
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x00006F89 File Offset: 0x00005189
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

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00006F97 File Offset: 0x00005197
		// (set) Token: 0x060001BA RID: 442 RVA: 0x00006FA4 File Offset: 0x000051A4
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

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00006FB2 File Offset: 0x000051B2
		// (set) Token: 0x060001BC RID: 444 RVA: 0x00006FBF File Offset: 0x000051BF
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

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00006FCD File Offset: 0x000051CD
		string IMultiPositionConstraintData.offsetVector3Property
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_Offset");
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00006FD9 File Offset: 0x000051D9
		string IMultiPositionConstraintData.sourceObjectsProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_SourceObjects");
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00006FE8 File Offset: 0x000051E8
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

		// Token: 0x060001C0 RID: 448 RVA: 0x00007060 File Offset: 0x00005260
		void IAnimationJobData.SetDefaultValues()
		{
			this.m_ConstrainedObject = null;
			this.m_ConstrainedAxes = new Vector3Bool(true);
			this.m_SourceObjects.Clear();
			this.m_MaintainOffset = false;
			this.m_Offset = Vector3.zero;
		}

		// Token: 0x040000D1 RID: 209
		[SerializeField]
		private Transform m_ConstrainedObject;

		// Token: 0x040000D2 RID: 210
		[SyncSceneToStream]
		[SerializeField]
		[WeightRange(0f, 1f)]
		private WeightedTransformArray m_SourceObjects;

		// Token: 0x040000D3 RID: 211
		[SyncSceneToStream]
		[SerializeField]
		private Vector3 m_Offset;

		// Token: 0x040000D4 RID: 212
		[NotKeyable]
		[SerializeField]
		private Vector3Bool m_ConstrainedAxes;

		// Token: 0x040000D5 RID: 213
		[NotKeyable]
		[SerializeField]
		private bool m_MaintainOffset;
	}
}
