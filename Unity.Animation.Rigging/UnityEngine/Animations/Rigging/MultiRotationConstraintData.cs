using System;
using System.Collections.Generic;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200004E RID: 78
	[Serializable]
	public struct MultiRotationConstraintData : IAnimationJobData, IMultiRotationConstraintData
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00007227 File Offset: 0x00005427
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x0000722F File Offset: 0x0000542F
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

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00007238 File Offset: 0x00005438
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x00007240 File Offset: 0x00005440
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

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00007249 File Offset: 0x00005449
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x00007251 File Offset: 0x00005451
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

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000725A File Offset: 0x0000545A
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x00007262 File Offset: 0x00005462
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

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000726B File Offset: 0x0000546B
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x00007278 File Offset: 0x00005478
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

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00007286 File Offset: 0x00005486
		// (set) Token: 0x060001DA RID: 474 RVA: 0x00007293 File Offset: 0x00005493
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

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060001DB RID: 475 RVA: 0x000072A1 File Offset: 0x000054A1
		// (set) Token: 0x060001DC RID: 476 RVA: 0x000072AE File Offset: 0x000054AE
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

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060001DD RID: 477 RVA: 0x000072BC File Offset: 0x000054BC
		string IMultiRotationConstraintData.offsetVector3Property
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_Offset");
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060001DE RID: 478 RVA: 0x000072C8 File Offset: 0x000054C8
		string IMultiRotationConstraintData.sourceObjectsProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_SourceObjects");
			}
		}

		// Token: 0x060001DF RID: 479 RVA: 0x000072D4 File Offset: 0x000054D4
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

		// Token: 0x060001E0 RID: 480 RVA: 0x0000734C File Offset: 0x0000554C
		void IAnimationJobData.SetDefaultValues()
		{
			this.m_ConstrainedObject = null;
			this.m_ConstrainedAxes = new Vector3Bool(true);
			this.m_SourceObjects.Clear();
			this.m_MaintainOffset = false;
			this.m_Offset = Vector3.zero;
		}

		// Token: 0x040000D8 RID: 216
		[SerializeField]
		private Transform m_ConstrainedObject;

		// Token: 0x040000D9 RID: 217
		[SyncSceneToStream]
		[SerializeField]
		[WeightRange(0f, 1f)]
		private WeightedTransformArray m_SourceObjects;

		// Token: 0x040000DA RID: 218
		[SyncSceneToStream]
		[SerializeField]
		private Vector3 m_Offset;

		// Token: 0x040000DB RID: 219
		[NotKeyable]
		[SerializeField]
		private Vector3Bool m_ConstrainedAxes;

		// Token: 0x040000DC RID: 220
		[NotKeyable]
		[SerializeField]
		private bool m_MaintainOffset;
	}
}
