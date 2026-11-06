using System;

namespace System.Threading.Tasks
{
	// Token: 0x02000342 RID: 834
	internal class SystemThreadingTasks_FutureDebugView<TResult>
	{
		// Token: 0x060022F7 RID: 8951 RVA: 0x0007D548 File Offset: 0x0007B748
		public SystemThreadingTasks_FutureDebugView(Task<TResult> task)
		{
			this.m_task = task;
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x060022F8 RID: 8952 RVA: 0x0007D558 File Offset: 0x0007B758
		public TResult Result
		{
			get
			{
				if (this.m_task.Status != TaskStatus.RanToCompletion)
				{
					return default(TResult);
				}
				return this.m_task.Result;
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x060022F9 RID: 8953 RVA: 0x0007D588 File Offset: 0x0007B788
		public object AsyncState
		{
			get
			{
				return this.m_task.AsyncState;
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x060022FA RID: 8954 RVA: 0x0007D595 File Offset: 0x0007B795
		public TaskCreationOptions CreationOptions
		{
			get
			{
				return this.m_task.CreationOptions;
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x060022FB RID: 8955 RVA: 0x0007D5A2 File Offset: 0x0007B7A2
		public Exception Exception
		{
			get
			{
				return this.m_task.Exception;
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x060022FC RID: 8956 RVA: 0x0007D5AF File Offset: 0x0007B7AF
		public int Id
		{
			get
			{
				return this.m_task.Id;
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x060022FD RID: 8957 RVA: 0x0007D5BC File Offset: 0x0007B7BC
		public bool CancellationPending
		{
			get
			{
				return this.m_task.Status == TaskStatus.WaitingToRun && this.m_task.CancellationToken.IsCancellationRequested;
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x060022FE RID: 8958 RVA: 0x0007D5EC File Offset: 0x0007B7EC
		public TaskStatus Status
		{
			get
			{
				return this.m_task.Status;
			}
		}

		// Token: 0x04001C87 RID: 7303
		private Task<TResult> m_task;
	}
}
