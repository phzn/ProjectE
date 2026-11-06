using System;
using Unity.Jobs;

namespace Pathfinding.Sync
{
	// Token: 0x0200023D RID: 573
	public class RWLock
	{
		// Token: 0x06000E66 RID: 3686 RVA: 0x00003788 File Offset: 0x00001988
		private void AddPendingSync()
		{
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x00003788 File Offset: 0x00001988
		private void RemovePendingSync()
		{
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x00003788 File Offset: 0x00001988
		private void AddPendingAsync()
		{
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x00003788 File Offset: 0x00001988
		private void RemovePendingAsync()
		{
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x00059099 File Offset: 0x00057299
		public RWLock.LockSync ReadSync()
		{
			this.AddPendingSync();
			this.lastWrite.Complete();
			this.lastWrite = default(JobHandle);
			return new RWLock.LockSync(this);
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x000590BE File Offset: 0x000572BE
		public RWLock.ReadLockAsync Read()
		{
			this.AddPendingAsync();
			return new RWLock.ReadLockAsync(this, this.lastWrite);
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x000590D2 File Offset: 0x000572D2
		public RWLock.LockSync WriteSync()
		{
			this.AddPendingSync();
			this.lastWrite.Complete();
			this.lastWrite = default(JobHandle);
			this.lastRead.Complete();
			return new RWLock.LockSync(this);
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00059102 File Offset: 0x00057302
		public RWLock.WriteLockAsync Write()
		{
			this.AddPendingAsync();
			return new RWLock.WriteLockAsync(this, JobHandle.CombineDependencies(this.lastRead, this.lastWrite));
		}

		// Token: 0x04000AD4 RID: 2772
		private JobHandle lastWrite;

		// Token: 0x04000AD5 RID: 2773
		private JobHandle lastRead;

		// Token: 0x0200023E RID: 574
		public readonly struct CombinedReadLockAsync
		{
			// Token: 0x06000E6F RID: 3695 RVA: 0x00059121 File Offset: 0x00057321
			public CombinedReadLockAsync(RWLock.ReadLockAsync lock1, RWLock.ReadLockAsync lock2)
			{
				this.lock1 = lock1.inner;
				this.lock2 = lock2.inner;
				this.dependency = JobHandle.CombineDependencies(lock1.dependency, lock2.dependency);
			}

			// Token: 0x06000E70 RID: 3696 RVA: 0x00059154 File Offset: 0x00057354
			public void UnlockAfter(JobHandle handle)
			{
				if (this.lock1 != null)
				{
					this.lock1.RemovePendingAsync();
					this.lock1.lastRead = JobHandle.CombineDependencies(this.lock1.lastRead, handle);
				}
				if (this.lock2 != null)
				{
					this.lock2.RemovePendingAsync();
					this.lock2.lastRead = JobHandle.CombineDependencies(this.lock2.lastRead, handle);
				}
			}

			// Token: 0x06000E71 RID: 3697 RVA: 0x000591BF File Offset: 0x000573BF
			public void Unlock()
			{
				if (this.lock1 != null)
				{
					this.lock1.RemovePendingAsync();
				}
				if (this.lock2 != null)
				{
					this.lock2.RemovePendingAsync();
				}
			}

			// Token: 0x04000AD6 RID: 2774
			private readonly RWLock lock1;

			// Token: 0x04000AD7 RID: 2775
			private readonly RWLock lock2;

			// Token: 0x04000AD8 RID: 2776
			public readonly JobHandle dependency;
		}

		// Token: 0x0200023F RID: 575
		public readonly struct ReadLockAsync
		{
			// Token: 0x06000E72 RID: 3698 RVA: 0x000591E7 File Offset: 0x000573E7
			public ReadLockAsync(RWLock inner, JobHandle dependency)
			{
				this.inner = inner;
				this.dependency = dependency;
			}

			// Token: 0x06000E73 RID: 3699 RVA: 0x000591F7 File Offset: 0x000573F7
			public void UnlockAfter(JobHandle handle)
			{
				if (this.inner != null)
				{
					this.inner.RemovePendingAsync();
					this.inner.lastRead = JobHandle.CombineDependencies(this.inner.lastRead, handle);
				}
			}

			// Token: 0x04000AD9 RID: 2777
			internal readonly RWLock inner;

			// Token: 0x04000ADA RID: 2778
			public readonly JobHandle dependency;
		}

		// Token: 0x02000240 RID: 576
		public readonly struct WriteLockAsync
		{
			// Token: 0x06000E74 RID: 3700 RVA: 0x00059228 File Offset: 0x00057428
			public WriteLockAsync(RWLock inner, JobHandle dependency)
			{
				this.inner = inner;
				this.dependency = dependency;
			}

			// Token: 0x06000E75 RID: 3701 RVA: 0x00059238 File Offset: 0x00057438
			public void UnlockAfter(JobHandle handle)
			{
				if (this.inner != null)
				{
					this.inner.RemovePendingAsync();
					this.inner.lastWrite = handle;
				}
			}

			// Token: 0x04000ADB RID: 2779
			private readonly RWLock inner;

			// Token: 0x04000ADC RID: 2780
			public readonly JobHandle dependency;
		}

		// Token: 0x02000241 RID: 577
		public readonly struct LockSync : IDisposable
		{
			// Token: 0x06000E76 RID: 3702 RVA: 0x00059259 File Offset: 0x00057459
			public LockSync(RWLock inner)
			{
				this.inner = inner;
			}

			// Token: 0x06000E77 RID: 3703 RVA: 0x00059262 File Offset: 0x00057462
			public void Unlock()
			{
				if (this.inner != null)
				{
					this.inner.RemovePendingSync();
				}
			}

			// Token: 0x06000E78 RID: 3704 RVA: 0x00059277 File Offset: 0x00057477
			void IDisposable.Dispose()
			{
				this.Unlock();
			}

			// Token: 0x04000ADD RID: 2781
			private readonly RWLock inner;
		}
	}
}
