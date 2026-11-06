using System;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.Operations
{
	// Token: 0x020000C6 RID: 198
	internal abstract class WaitForCurrentOperationAsyncOperationBase<TObject> : AsyncOperationBase<TObject>
	{
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x000146B2 File Offset: 0x000128B2
		// (set) Token: 0x060005AA RID: 1450 RVA: 0x000146BA File Offset: 0x000128BA
		protected AsyncOperationHandle CurrentOperation { get; set; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x000146C3 File Offset: 0x000128C3
		// (set) Token: 0x060005AC RID: 1452 RVA: 0x000146CB File Offset: 0x000128CB
		public AsyncOperationHandle Dependency { get; set; }

		// Token: 0x060005AD RID: 1453 RVA: 0x000146D4 File Offset: 0x000128D4
		protected override bool InvokeWaitForCompletion()
		{
			if (!base.IsRunning || this.m_Waiting)
			{
				return true;
			}
			bool flag;
			try
			{
				this.m_Waiting = true;
				if (!this.HasExecuted && this.Dependency.IsValid())
				{
					this.Dependency.WaitForCompletion();
					if (this.Dependency.IsValid() && this.Dependency.Status == AsyncOperationStatus.Failed)
					{
						base.Complete(default(TObject), false, "Dependency `" + base.Handle.DebugName + "` failed to complete.");
						flag = true;
					}
					else
					{
						flag = false;
					}
				}
				else if (!this.CurrentOperation.IsValid())
				{
					base.Complete(default(TObject), false, "Expected to have a current operation to wait on. Can not complete " + this.ToString() + ".");
					flag = true;
				}
				else
				{
					this.CurrentOperation.WaitForCompletion();
					flag = false;
				}
			}
			finally
			{
				this.m_Waiting = false;
			}
			return flag;
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x000147E0 File Offset: 0x000129E0
		protected override void Destroy()
		{
			base.Destroy();
			this.Dependency = default(AsyncOperationHandle);
			this.CurrentOperation = default(AsyncOperationHandle);
			this.HasExecuted = false;
		}

		// Token: 0x040001F0 RID: 496
		private bool m_Waiting;
	}
}
