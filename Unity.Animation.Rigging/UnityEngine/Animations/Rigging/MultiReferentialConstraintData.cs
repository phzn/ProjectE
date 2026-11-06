using System;
using System.Collections.Generic;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200004C RID: 76
	[Serializable]
	public struct MultiReferentialConstraintData : IAnimationJobData, IMultiReferentialConstraintData
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x000070D8 File Offset: 0x000052D8
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x000070E0 File Offset: 0x000052E0
		public int driver
		{
			get
			{
				return this.m_Driver;
			}
			set
			{
				this.m_Driver = Mathf.Clamp(value, 0, this.m_SourceObjects.Count - 1);
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x000070FC File Offset: 0x000052FC
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00007117 File Offset: 0x00005317
		public List<Transform> sourceObjects
		{
			get
			{
				if (this.m_SourceObjects == null)
				{
					this.m_SourceObjects = new List<Transform>();
				}
				return this.m_SourceObjects;
			}
			set
			{
				this.m_SourceObjects = value;
				this.m_Driver = Mathf.Clamp(this.m_Driver, 0, this.m_SourceObjects.Count - 1);
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x0000713F File Offset: 0x0000533F
		Transform[] IMultiReferentialConstraintData.sourceObjects
		{
			get
			{
				return this.m_SourceObjects.ToArray();
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000714C File Offset: 0x0000534C
		int IMultiReferentialConstraintData.driverValue
		{
			get
			{
				return this.m_Driver;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00007154 File Offset: 0x00005354
		string IMultiReferentialConstraintData.driverIntProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_Driver");
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00007160 File Offset: 0x00005360
		bool IAnimationJobData.IsValid()
		{
			if (this.m_SourceObjects.Count < 2)
			{
				return false;
			}
			using (List<Transform>.Enumerator enumerator = this.m_SourceObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000071CC File Offset: 0x000053CC
		void IAnimationJobData.SetDefaultValues()
		{
			this.m_Driver = 0;
			this.m_SourceObjects = new List<Transform>();
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000071E0 File Offset: 0x000053E0
		public void UpdateDriver()
		{
			this.m_Driver = Mathf.Clamp(this.m_Driver, 0, (this.m_SourceObjects != null) ? (this.m_SourceObjects.Count - 1) : 0);
		}

		// Token: 0x040000D6 RID: 214
		[SyncSceneToStream]
		[SerializeField]
		private int m_Driver;

		// Token: 0x040000D7 RID: 215
		[SyncSceneToStream]
		[SerializeField]
		private List<Transform> m_SourceObjects;
	}
}
