using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Runtime
{
	// Token: 0x0200002E RID: 46
	internal class ThreadNeutralSemaphore
	{
		// Token: 0x0600015F RID: 351 RVA: 0x000060B3 File Offset: 0x000042B3
		public ThreadNeutralSemaphore(int maxCount)
			: this(maxCount, null)
		{
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000060BD File Offset: 0x000042BD
		public ThreadNeutralSemaphore(int maxCount, Func<Exception> abortedExceptionGenerator)
		{
			this.maxCount = maxCount;
			this.abortedExceptionGenerator = abortedExceptionGenerator;
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000161 RID: 353 RVA: 0x000060DE File Offset: 0x000042DE
		private static Action<object, TimeoutException> EnteredAsyncCallback
		{
			get
			{
				if (ThreadNeutralSemaphore.enteredAsyncCallback == null)
				{
					ThreadNeutralSemaphore.enteredAsyncCallback = new Action<object, TimeoutException>(ThreadNeutralSemaphore.OnEnteredAsync);
				}
				return ThreadNeutralSemaphore.enteredAsyncCallback;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000162 RID: 354 RVA: 0x000060FD File Offset: 0x000042FD
		private Queue<AsyncWaitHandle> Waiters
		{
			get
			{
				if (this.waiters == null)
				{
					this.waiters = new Queue<AsyncWaitHandle>();
				}
				return this.waiters;
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00006118 File Offset: 0x00004318
		public bool EnterAsync(TimeSpan timeout, FastAsyncCallback callback, object state)
		{
			AsyncWaitHandle asyncWaitHandle = null;
			object thisLock = this.ThisLock;
			lock (thisLock)
			{
				if (this.aborted)
				{
					throw Fx.Exception.AsError(this.CreateObjectAbortedException());
				}
				if (this.count < this.maxCount)
				{
					this.count++;
					return true;
				}
				asyncWaitHandle = new AsyncWaitHandle();
				this.Waiters.Enqueue(asyncWaitHandle);
			}
			return asyncWaitHandle.WaitAsync(ThreadNeutralSemaphore.EnteredAsyncCallback, new ThreadNeutralSemaphore.EnterAsyncData(this, asyncWaitHandle, callback, state), timeout);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000061B8 File Offset: 0x000043B8
		private static void OnEnteredAsync(object state, TimeoutException exception)
		{
			ThreadNeutralSemaphore.EnterAsyncData enterAsyncData = (ThreadNeutralSemaphore.EnterAsyncData)state;
			ThreadNeutralSemaphore semaphore = enterAsyncData.Semaphore;
			Exception ex = exception;
			if (exception != null && !semaphore.RemoveWaiter(enterAsyncData.Waiter))
			{
				ex = null;
			}
			if (semaphore.aborted)
			{
				ex = semaphore.CreateObjectAbortedException();
			}
			enterAsyncData.Callback(enterAsyncData.State, ex);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000620C File Offset: 0x0000440C
		public bool TryEnter()
		{
			object thisLock = this.ThisLock;
			bool flag2;
			lock (thisLock)
			{
				if (this.count < this.maxCount)
				{
					this.count++;
					flag2 = true;
				}
				else
				{
					flag2 = false;
				}
			}
			return flag2;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000626C File Offset: 0x0000446C
		public void Enter(TimeSpan timeout)
		{
			if (!this.TryEnter(timeout))
			{
				throw Fx.Exception.AsError(ThreadNeutralSemaphore.CreateEnterTimedOutException(timeout));
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00006288 File Offset: 0x00004488
		public bool TryEnter(TimeSpan timeout)
		{
			AsyncWaitHandle asyncWaitHandle = this.EnterCore();
			if (asyncWaitHandle == null)
			{
				return true;
			}
			bool flag = !asyncWaitHandle.Wait(timeout);
			if (this.aborted)
			{
				throw Fx.Exception.AsError(this.CreateObjectAbortedException());
			}
			if (flag && !this.RemoveWaiter(asyncWaitHandle))
			{
				flag = false;
			}
			return !flag;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x000062D7 File Offset: 0x000044D7
		internal static TimeoutException CreateEnterTimedOutException(TimeSpan timeout)
		{
			return new TimeoutException(InternalSR.LockTimeoutExceptionMessage(timeout));
		}

		// Token: 0x06000169 RID: 361 RVA: 0x000062E9 File Offset: 0x000044E9
		private Exception CreateObjectAbortedException()
		{
			if (this.abortedExceptionGenerator != null)
			{
				return this.abortedExceptionGenerator();
			}
			return new OperationCanceledException("Thread Neutral Semaphore Aborted");
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000630C File Offset: 0x0000450C
		private bool RemoveWaiter(AsyncWaitHandle waiter)
		{
			bool flag = false;
			object thisLock = this.ThisLock;
			lock (thisLock)
			{
				for (int i = this.Waiters.Count; i > 0; i--)
				{
					AsyncWaitHandle asyncWaitHandle = this.Waiters.Dequeue();
					if (asyncWaitHandle == waiter)
					{
						flag = true;
					}
					else
					{
						this.Waiters.Enqueue(asyncWaitHandle);
					}
				}
			}
			return flag;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00006384 File Offset: 0x00004584
		private AsyncWaitHandle EnterCore()
		{
			object thisLock = this.ThisLock;
			AsyncWaitHandle asyncWaitHandle;
			lock (thisLock)
			{
				if (this.aborted)
				{
					throw Fx.Exception.AsError(this.CreateObjectAbortedException());
				}
				if (this.count < this.maxCount)
				{
					this.count++;
					return null;
				}
				asyncWaitHandle = new AsyncWaitHandle();
				this.Waiters.Enqueue(asyncWaitHandle);
			}
			return asyncWaitHandle;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000640C File Offset: 0x0000460C
		public int Exit()
		{
			int num = -1;
			object thisLock = this.ThisLock;
			AsyncWaitHandle asyncWaitHandle;
			lock (thisLock)
			{
				if (this.aborted)
				{
					return num;
				}
				if (this.count == 0)
				{
					string text = "Invalid Semaphore Exit";
					throw Fx.Exception.AsError(new SynchronizationLockException(text));
				}
				if (this.waiters == null || this.waiters.Count == 0)
				{
					this.count--;
					return this.count;
				}
				asyncWaitHandle = this.waiters.Dequeue();
				num = this.count;
			}
			asyncWaitHandle.Set();
			return num;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000064C0 File Offset: 0x000046C0
		public void Abort()
		{
			object thisLock = this.ThisLock;
			lock (thisLock)
			{
				if (!this.aborted)
				{
					this.aborted = true;
					if (this.waiters != null)
					{
						while (this.waiters.Count > 0)
						{
							this.waiters.Dequeue().Set();
						}
					}
				}
			}
		}

		// Token: 0x040000D5 RID: 213
		private static Action<object, TimeoutException> enteredAsyncCallback;

		// Token: 0x040000D6 RID: 214
		private bool aborted;

		// Token: 0x040000D7 RID: 215
		private Func<Exception> abortedExceptionGenerator;

		// Token: 0x040000D8 RID: 216
		private int count;

		// Token: 0x040000D9 RID: 217
		private int maxCount;

		// Token: 0x040000DA RID: 218
		private object ThisLock = new object();

		// Token: 0x040000DB RID: 219
		private Queue<AsyncWaitHandle> waiters;

		// Token: 0x02000088 RID: 136
		private class EnterAsyncData
		{
			// Token: 0x060003EF RID: 1007 RVA: 0x00012A1E File Offset: 0x00010C1E
			public EnterAsyncData(ThreadNeutralSemaphore semaphore, AsyncWaitHandle waiter, FastAsyncCallback callback, object state)
			{
				this.Waiter = waiter;
				this.Semaphore = semaphore;
				this.Callback = callback;
				this.State = state;
			}

			// Token: 0x1700009A RID: 154
			// (get) Token: 0x060003F0 RID: 1008 RVA: 0x00012A43 File Offset: 0x00010C43
			// (set) Token: 0x060003F1 RID: 1009 RVA: 0x00012A4B File Offset: 0x00010C4B
			public ThreadNeutralSemaphore Semaphore { get; set; }

			// Token: 0x1700009B RID: 155
			// (get) Token: 0x060003F2 RID: 1010 RVA: 0x00012A54 File Offset: 0x00010C54
			// (set) Token: 0x060003F3 RID: 1011 RVA: 0x00012A5C File Offset: 0x00010C5C
			public AsyncWaitHandle Waiter { get; set; }

			// Token: 0x1700009C RID: 156
			// (get) Token: 0x060003F4 RID: 1012 RVA: 0x00012A65 File Offset: 0x00010C65
			// (set) Token: 0x060003F5 RID: 1013 RVA: 0x00012A6D File Offset: 0x00010C6D
			public FastAsyncCallback Callback { get; set; }

			// Token: 0x1700009D RID: 157
			// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00012A76 File Offset: 0x00010C76
			// (set) Token: 0x060003F7 RID: 1015 RVA: 0x00012A7E File Offset: 0x00010C7E
			public object State { get; set; }
		}
	}
}
