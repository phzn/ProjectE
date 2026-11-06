using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000445 RID: 1093
	internal class JobManager : IDisposable
	{
		// Token: 0x06002269 RID: 8809 RVA: 0x000848F3 File Offset: 0x00082AF3
		public void Add(ref NudgeJobData job)
		{
			this.m_NudgeJobs.Add(ref job);
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x00084903 File Offset: 0x00082B03
		public void Add(ref ConvertMeshJobData job)
		{
			this.m_ConvertMeshJobs.Add(ref job);
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x00084913 File Offset: 0x00082B13
		public void Add(ref CopyClosingMeshJobData job)
		{
			this.m_CopyClosingMeshJobs.Add(ref job);
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x00084924 File Offset: 0x00082B24
		public void CompleteNudgeJobs()
		{
			foreach (NativeSlice<NudgeJobData> nativeSlice in this.m_NudgeJobs.GetPages())
			{
				this.m_JobMerger.Add(JobProcessor.ScheduleNudgeJobs((IntPtr)nativeSlice.GetUnsafePtr<NudgeJobData>(), nativeSlice.Length));
			}
			this.m_JobMerger.MergeAndReset().Complete();
			this.m_NudgeJobs.Reset();
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x000849BC File Offset: 0x00082BBC
		public void CompleteConvertMeshJobs()
		{
			foreach (NativeSlice<ConvertMeshJobData> nativeSlice in this.m_ConvertMeshJobs.GetPages())
			{
				this.m_JobMerger.Add(JobProcessor.ScheduleConvertMeshJobs((IntPtr)nativeSlice.GetUnsafePtr<ConvertMeshJobData>(), nativeSlice.Length));
			}
			this.m_JobMerger.MergeAndReset().Complete();
			this.m_ConvertMeshJobs.Reset();
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x00084A54 File Offset: 0x00082C54
		public void CompleteClosingMeshJobs()
		{
			foreach (NativeSlice<CopyClosingMeshJobData> nativeSlice in this.m_CopyClosingMeshJobs.GetPages())
			{
				this.m_JobMerger.Add(JobProcessor.ScheduleCopyClosingMeshJobs((IntPtr)nativeSlice.GetUnsafePtr<CopyClosingMeshJobData>(), nativeSlice.Length));
			}
			this.m_JobMerger.MergeAndReset().Complete();
			this.m_CopyClosingMeshJobs.Reset();
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x0600226F RID: 8815 RVA: 0x00084AEC File Offset: 0x00082CEC
		// (set) Token: 0x06002270 RID: 8816 RVA: 0x00084AF4 File Offset: 0x00082CF4
		private protected bool disposed { protected get; private set; }

		// Token: 0x06002271 RID: 8817 RVA: 0x00084AFD File Offset: 0x00082CFD
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x00084B10 File Offset: 0x00082D10
		protected void Dispose(bool disposing)
		{
			bool disposed = this.disposed;
			if (!disposed)
			{
				if (disposing)
				{
					this.m_NudgeJobs.Dispose();
					this.m_ConvertMeshJobs.Dispose();
					this.m_CopyClosingMeshJobs.Dispose();
					this.m_JobMerger.Dispose();
				}
				this.disposed = true;
			}
		}

		// Token: 0x04000F3B RID: 3899
		private NativePagedList<NudgeJobData> m_NudgeJobs = new NativePagedList<NudgeJobData>(64);

		// Token: 0x04000F3C RID: 3900
		private NativePagedList<ConvertMeshJobData> m_ConvertMeshJobs = new NativePagedList<ConvertMeshJobData>(64);

		// Token: 0x04000F3D RID: 3901
		private NativePagedList<CopyClosingMeshJobData> m_CopyClosingMeshJobs = new NativePagedList<CopyClosingMeshJobData>(64);

		// Token: 0x04000F3E RID: 3902
		private JobMerger m_JobMerger = new JobMerger(128);
	}
}
