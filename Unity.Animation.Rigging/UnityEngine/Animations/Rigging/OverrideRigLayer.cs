using System;
using UnityEngine.Serialization;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x0200005B RID: 91
	[Serializable]
	public class OverrideRigLayer : IRigLayer
	{
		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600024D RID: 589 RVA: 0x00007A90 File Offset: 0x00005C90
		// (set) Token: 0x0600024E RID: 590 RVA: 0x00007A98 File Offset: 0x00005C98
		public Rig rig
		{
			get
			{
				return this.m_Rig;
			}
			private set
			{
				this.m_Rig = value;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600024F RID: 591 RVA: 0x00007AA1 File Offset: 0x00005CA1
		// (set) Token: 0x06000250 RID: 592 RVA: 0x00007AA9 File Offset: 0x00005CA9
		public bool active
		{
			get
			{
				return this.m_Active;
			}
			set
			{
				this.m_Active = value;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000251 RID: 593 RVA: 0x00007AB2 File Offset: 0x00005CB2
		public string name
		{
			get
			{
				if (!(this.rig != null))
				{
					return "no-name";
				}
				return this.rig.gameObject.name;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000252 RID: 594 RVA: 0x00007AD8 File Offset: 0x00005CD8
		public IRigConstraint[] constraints
		{
			get
			{
				if (!this.isInitialized)
				{
					return null;
				}
				return this.m_Constraints;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000253 RID: 595 RVA: 0x00007AEA File Offset: 0x00005CEA
		public IAnimationJob[] jobs
		{
			get
			{
				if (!this.isInitialized)
				{
					return null;
				}
				return this.m_Jobs;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000254 RID: 596 RVA: 0x00007AFC File Offset: 0x00005CFC
		// (set) Token: 0x06000255 RID: 597 RVA: 0x00007B04 File Offset: 0x00005D04
		public bool isInitialized { get; private set; }

		// Token: 0x06000256 RID: 598 RVA: 0x00007B0D File Offset: 0x00005D0D
		public OverrideRigLayer(Rig rig, IRigConstraint[] constraints, bool active = true)
		{
			this.rig = rig;
			this.active = active;
			this.m_Constraints = constraints;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00007B34 File Offset: 0x00005D34
		public bool Initialize(Animator animator)
		{
			if (this.isInitialized)
			{
				return true;
			}
			if (this.rig == null)
			{
				return false;
			}
			if (this.m_Constraints == null || this.m_Constraints.Length == 0)
			{
				return false;
			}
			this.m_Jobs = new IAnimationJob[this.m_Constraints.Length];
			for (int i = 0; i < this.m_Constraints.Length; i++)
			{
				this.m_Jobs[i] = this.m_Constraints[i].CreateJob(animator);
			}
			return this.isInitialized = true;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00007BB4 File Offset: 0x00005DB4
		public void Update()
		{
			if (!this.isInitialized)
			{
				return;
			}
			for (int i = 0; i < this.m_Constraints.Length; i++)
			{
				this.m_Constraints[i].UpdateJob(this.m_Jobs[i]);
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00007BF4 File Offset: 0x00005DF4
		public void Reset()
		{
			if (!this.isInitialized)
			{
				return;
			}
			int i = 0;
			int num = this.m_Constraints.Length;
			while (i < num)
			{
				this.m_Constraints[i].DestroyJob(this.m_Jobs[i]);
				i++;
			}
			this.m_Constraints = null;
			this.m_Jobs = null;
			this.isInitialized = false;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00007C49 File Offset: 0x00005E49
		public bool IsValid()
		{
			return this.rig != null && this.isInitialized;
		}

		// Token: 0x040000F8 RID: 248
		[SerializeField]
		[FormerlySerializedAs("rig")]
		private Rig m_Rig;

		// Token: 0x040000F9 RID: 249
		[SerializeField]
		[FormerlySerializedAs("active")]
		private bool m_Active = true;

		// Token: 0x040000FA RID: 250
		private IRigConstraint[] m_Constraints;

		// Token: 0x040000FB RID: 251
		private IAnimationJob[] m_Jobs;
	}
}
