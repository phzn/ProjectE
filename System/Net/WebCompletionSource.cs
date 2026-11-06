using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
	// Token: 0x020006B8 RID: 1720
	internal class WebCompletionSource<T>
	{
		// Token: 0x0600375B RID: 14171 RVA: 0x000C2628 File Offset: 0x000C0828
		public WebCompletionSource(bool runAsync = true)
		{
			this.completion = new TaskCompletionSource<WebCompletionSource<T>.Result>(runAsync ? TaskCreationOptions.RunContinuationsAsynchronously : TaskCreationOptions.None);
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x0600375C RID: 14172 RVA: 0x000C2643 File Offset: 0x000C0843
		internal WebCompletionSource<T>.Result CurrentResult
		{
			get
			{
				return this.currentResult;
			}
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x0600375D RID: 14173 RVA: 0x000C264B File Offset: 0x000C084B
		internal WebCompletionSource<T>.Status CurrentStatus
		{
			get
			{
				WebCompletionSource<T>.Result result = this.currentResult;
				if (result == null)
				{
					return WebCompletionSource<T>.Status.Running;
				}
				return result.Status;
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x0600375E RID: 14174 RVA: 0x000C265E File Offset: 0x000C085E
		internal Task Task
		{
			get
			{
				return this.completion.Task;
			}
		}

		// Token: 0x0600375F RID: 14175 RVA: 0x000C266C File Offset: 0x000C086C
		public bool TrySetCompleted(T argument)
		{
			WebCompletionSource<T>.Result result = new WebCompletionSource<T>.Result(argument);
			return Interlocked.CompareExchange<WebCompletionSource<T>.Result>(ref this.currentResult, result, null) == null && this.completion.TrySetResult(result);
		}

		// Token: 0x06003760 RID: 14176 RVA: 0x000C26A0 File Offset: 0x000C08A0
		public bool TrySetCompleted()
		{
			WebCompletionSource<T>.Result result = new WebCompletionSource<T>.Result(WebCompletionSource<T>.Status.Completed, null);
			return Interlocked.CompareExchange<WebCompletionSource<T>.Result>(ref this.currentResult, result, null) == null && this.completion.TrySetResult(result);
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x000C26D2 File Offset: 0x000C08D2
		public bool TrySetCanceled()
		{
			return this.TrySetCanceled(new OperationCanceledException());
		}

		// Token: 0x06003762 RID: 14178 RVA: 0x000C26E0 File Offset: 0x000C08E0
		public bool TrySetCanceled(OperationCanceledException error)
		{
			WebCompletionSource<T>.Result result = new WebCompletionSource<T>.Result(WebCompletionSource<T>.Status.Canceled, ExceptionDispatchInfo.Capture(error));
			return Interlocked.CompareExchange<WebCompletionSource<T>.Result>(ref this.currentResult, result, null) == null && this.completion.TrySetResult(result);
		}

		// Token: 0x06003763 RID: 14179 RVA: 0x000C2718 File Offset: 0x000C0918
		public bool TrySetException(Exception error)
		{
			WebCompletionSource<T>.Result result = new WebCompletionSource<T>.Result(WebCompletionSource<T>.Status.Faulted, ExceptionDispatchInfo.Capture(error));
			return Interlocked.CompareExchange<WebCompletionSource<T>.Result>(ref this.currentResult, result, null) == null && this.completion.TrySetResult(result);
		}

		// Token: 0x06003764 RID: 14180 RVA: 0x000C274F File Offset: 0x000C094F
		public void ThrowOnError()
		{
			if (!this.completion.Task.IsCompleted)
			{
				return;
			}
			ExceptionDispatchInfo error = this.completion.Task.Result.Error;
			if (error == null)
			{
				return;
			}
			error.Throw();
		}

		// Token: 0x06003765 RID: 14181 RVA: 0x000C2784 File Offset: 0x000C0984
		public async Task<T> WaitForCompletion()
		{
			WebCompletionSource<T>.Result result = await this.completion.Task.ConfigureAwait(false);
			if (result.Status == WebCompletionSource<T>.Status.Completed)
			{
				return result.Argument;
			}
			result.Error.Throw();
			throw new InvalidOperationException("Should never happen.");
		}

		// Token: 0x0400203D RID: 8253
		private TaskCompletionSource<WebCompletionSource<T>.Result> completion;

		// Token: 0x0400203E RID: 8254
		private WebCompletionSource<T>.Result currentResult;

		// Token: 0x020006B9 RID: 1721
		internal enum Status
		{
			// Token: 0x04002040 RID: 8256
			Running,
			// Token: 0x04002041 RID: 8257
			Completed,
			// Token: 0x04002042 RID: 8258
			Canceled,
			// Token: 0x04002043 RID: 8259
			Faulted
		}

		// Token: 0x020006BA RID: 1722
		internal class Result
		{
			// Token: 0x17000B8E RID: 2958
			// (get) Token: 0x06003766 RID: 14182 RVA: 0x000C27C7 File Offset: 0x000C09C7
			public WebCompletionSource<T>.Status Status { get; }

			// Token: 0x17000B8F RID: 2959
			// (get) Token: 0x06003767 RID: 14183 RVA: 0x000C27CF File Offset: 0x000C09CF
			public bool Success
			{
				get
				{
					return this.Status == WebCompletionSource<T>.Status.Completed;
				}
			}

			// Token: 0x17000B90 RID: 2960
			// (get) Token: 0x06003768 RID: 14184 RVA: 0x000C27DA File Offset: 0x000C09DA
			public ExceptionDispatchInfo Error { get; }

			// Token: 0x17000B91 RID: 2961
			// (get) Token: 0x06003769 RID: 14185 RVA: 0x000C27E2 File Offset: 0x000C09E2
			public T Argument { get; }

			// Token: 0x0600376A RID: 14186 RVA: 0x000C27EA File Offset: 0x000C09EA
			public Result(T argument)
			{
				this.Status = WebCompletionSource<T>.Status.Completed;
				this.Argument = argument;
			}

			// Token: 0x0600376B RID: 14187 RVA: 0x000C2800 File Offset: 0x000C0A00
			public Result(WebCompletionSource<T>.Status state, ExceptionDispatchInfo error)
			{
				this.Status = state;
				this.Error = error;
			}
		}
	}
}
