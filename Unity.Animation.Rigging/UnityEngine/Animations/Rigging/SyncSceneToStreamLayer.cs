using System;
using System.Collections.Generic;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000063 RID: 99
	internal class SyncSceneToStreamLayer
	{
		// Token: 0x060002A0 RID: 672 RVA: 0x00008F08 File Offset: 0x00007108
		public bool Initialize(Animator animator, IList<IRigLayer> layers)
		{
			if (this.isInitialized)
			{
				return true;
			}
			this.m_RigIndices = new List<int>(layers.Count);
			for (int i = 0; i < layers.Count; i++)
			{
				if (layers[i].IsValid())
				{
					this.m_RigIndices.Add(i);
				}
			}
			this.m_Data = RigUtils.CreateSyncSceneToStreamData(animator, layers);
			if (!this.m_Data.IsValid())
			{
				return false;
			}
			this.job = RigUtils.syncSceneToStreamBinder.Create(animator, this.m_Data, null);
			return this.isInitialized = true;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00008F9C File Offset: 0x0000719C
		public void Update(IList<IRigLayer> layers)
		{
			if (!this.isInitialized || !this.m_Data.IsValid())
			{
				return;
			}
			IRigSyncSceneToStreamData rigSyncSceneToStreamData = (IRigSyncSceneToStreamData)this.m_Data;
			int i = 0;
			int count = this.m_RigIndices.Count;
			while (i < count)
			{
				rigSyncSceneToStreamData.rigStates[i] = layers[this.m_RigIndices[i]].active;
				i++;
			}
			RigUtils.syncSceneToStreamBinder.Update(this.job, this.m_Data);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00009018 File Offset: 0x00007218
		public void Reset()
		{
			if (!this.isInitialized)
			{
				return;
			}
			if (this.m_Data != null && this.m_Data.IsValid())
			{
				RigUtils.syncSceneToStreamBinder.Destroy(this.job);
				this.m_Data = null;
			}
			this.isInitialized = false;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00009056 File Offset: 0x00007256
		public bool IsValid()
		{
			return this.job != null && this.m_Data != null;
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000906B File Offset: 0x0000726B
		// (set) Token: 0x060002A5 RID: 677 RVA: 0x00009073 File Offset: 0x00007273
		public bool isInitialized { get; private set; }

		// Token: 0x04000113 RID: 275
		public IAnimationJob job;

		// Token: 0x04000114 RID: 276
		private IAnimationJobData m_Data;

		// Token: 0x04000115 RID: 277
		private List<int> m_RigIndices;
	}
}
