using System;
using UnityEngine.Serialization;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000060 RID: 96
	[Serializable]
	public class RigLayer : IRigLayer
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0000849C File Offset: 0x0000669C
		// (set) Token: 0x06000286 RID: 646 RVA: 0x000084A4 File Offset: 0x000066A4
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

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000287 RID: 647 RVA: 0x000084AD File Offset: 0x000066AD
		// (set) Token: 0x06000288 RID: 648 RVA: 0x000084B5 File Offset: 0x000066B5
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

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000289 RID: 649 RVA: 0x000084BE File Offset: 0x000066BE
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

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600028A RID: 650 RVA: 0x000084E4 File Offset: 0x000066E4
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

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600028B RID: 651 RVA: 0x000084F6 File Offset: 0x000066F6
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

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600028C RID: 652 RVA: 0x00008508 File Offset: 0x00006708
		// (set) Token: 0x0600028D RID: 653 RVA: 0x00008510 File Offset: 0x00006710
		public bool isInitialized { get; private set; }

		// Token: 0x0600028E RID: 654 RVA: 0x00008519 File Offset: 0x00006719
		public RigLayer(Rig rig, bool active = true)
		{
			this.rig = rig;
			this.active = active;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00008538 File Offset: 0x00006738
		public bool Initialize(Animator animator)
		{
			if (this.isInitialized)
			{
				return true;
			}
			if (!(this.rig != null))
			{
				return false;
			}
			this.m_Constraints = RigUtils.GetConstraints(this.rig);
			if (this.m_Constraints == null || this.m_Constraints.Length == 0)
			{
				return false;
			}
			this.m_Jobs = RigUtils.CreateAnimationJobs(animator, this.m_Constraints);
			return this.isInitialized = true;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x000085A0 File Offset: 0x000067A0
		public void Update()
		{
			if (!this.isInitialized)
			{
				return;
			}
			int i = 0;
			int num = this.m_Constraints.Length;
			while (i < num)
			{
				this.m_Constraints[i].UpdateJob(this.m_Jobs[i]);
				i++;
			}
		}

		// Token: 0x06000291 RID: 657 RVA: 0x000085E0 File Offset: 0x000067E0
		public void Reset()
		{
			if (!this.isInitialized)
			{
				return;
			}
			RigUtils.DestroyAnimationJobs(this.m_Constraints, this.m_Jobs);
			this.m_Constraints = null;
			this.m_Jobs = null;
			this.isInitialized = false;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00008611 File Offset: 0x00006811
		public bool IsValid()
		{
			return this.rig != null && this.isInitialized;
		}

		// Token: 0x0400010B RID: 267
		[SerializeField]
		[FormerlySerializedAs("rig")]
		private Rig m_Rig;

		// Token: 0x0400010C RID: 268
		[SerializeField]
		[FormerlySerializedAs("active")]
		private bool m_Active = true;

		// Token: 0x0400010D RID: 269
		private IRigConstraint[] m_Constraints;

		// Token: 0x0400010E RID: 270
		private IAnimationJob[] m_Jobs;
	}
}
