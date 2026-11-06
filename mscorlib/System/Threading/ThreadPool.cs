using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Threading
{
	/// <summary>Provides a pool of threads that can be used to execute tasks, post work items, process asynchronous I/O, wait on behalf of other threads, and process timers.</summary>
	// Token: 0x020002E6 RID: 742
	public static class ThreadPool
	{
		/// <summary>Sets the number of requests to the thread pool that can be active concurrently. All requests above that number remain queued until thread pool threads become available.</summary>
		/// <param name="workerThreads">The maximum number of worker threads in the thread pool.</param>
		/// <param name="completionPortThreads">The maximum number of asynchronous I/O threads in the thread pool.</param>
		/// <returns>
		///   <see langword="true" /> if the change is successful; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002035 RID: 8245 RVA: 0x000757B8 File Offset: 0x000739B8
		[SecuritySafeCritical]
		public static bool SetMaxThreads(int workerThreads, int completionPortThreads)
		{
			return ThreadPool.SetMaxThreadsNative(workerThreads, completionPortThreads);
		}

		/// <summary>Retrieves the number of requests to the thread pool that can be active concurrently. All requests above that number remain queued until thread pool threads become available.</summary>
		/// <param name="workerThreads">The maximum number of worker threads in the thread pool.</param>
		/// <param name="completionPortThreads">The maximum number of asynchronous I/O threads in the thread pool.</param>
		// Token: 0x06002036 RID: 8246 RVA: 0x000757C1 File Offset: 0x000739C1
		[SecuritySafeCritical]
		public static void GetMaxThreads(out int workerThreads, out int completionPortThreads)
		{
			ThreadPool.GetMaxThreadsNative(out workerThreads, out completionPortThreads);
		}

		/// <summary>Sets the minimum number of threads the thread pool creates on demand, as new requests are made, before switching to an algorithm for managing thread creation and destruction.</summary>
		/// <param name="workerThreads">The minimum number of worker threads that the thread pool creates on demand.</param>
		/// <param name="completionPortThreads">The minimum number of asynchronous I/O threads that the thread pool creates on demand.</param>
		/// <returns>
		///   <see langword="true" /> if the change is successful; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002037 RID: 8247 RVA: 0x000757CA File Offset: 0x000739CA
		[SecuritySafeCritical]
		public static bool SetMinThreads(int workerThreads, int completionPortThreads)
		{
			return ThreadPool.SetMinThreadsNative(workerThreads, completionPortThreads);
		}

		/// <summary>Retrieves the minimum number of threads the thread pool creates on demand, as new requests are made, before switching to an algorithm for managing thread creation and destruction.</summary>
		/// <param name="workerThreads">When this method returns, contains the minimum number of worker threads that the thread pool creates on demand.</param>
		/// <param name="completionPortThreads">When this method returns, contains the minimum number of asynchronous I/O threads that the thread pool creates on demand.</param>
		// Token: 0x06002038 RID: 8248 RVA: 0x000757D3 File Offset: 0x000739D3
		[SecuritySafeCritical]
		public static void GetMinThreads(out int workerThreads, out int completionPortThreads)
		{
			ThreadPool.GetMinThreadsNative(out workerThreads, out completionPortThreads);
		}

		/// <summary>Retrieves the difference between the maximum number of thread pool threads returned by the <see cref="M:System.Threading.ThreadPool.GetMaxThreads(System.Int32@,System.Int32@)" /> method, and the number currently active.</summary>
		/// <param name="workerThreads">The number of available worker threads.</param>
		/// <param name="completionPortThreads">The number of available asynchronous I/O threads.</param>
		// Token: 0x06002039 RID: 8249 RVA: 0x000757DC File Offset: 0x000739DC
		[SecuritySafeCritical]
		public static void GetAvailableThreads(out int workerThreads, out int completionPortThreads)
		{
			ThreadPool.GetAvailableThreadsNative(out workerThreads, out completionPortThreads);
		}

		/// <summary>Registers a delegate to wait for a <see cref="T:System.Threading.WaitHandle" />, specifying a 32-bit unsigned integer for the time-out in milliseconds.</summary>
		/// <param name="waitObject">The <see cref="T:System.Threading.WaitHandle" /> to register. Use a <see cref="T:System.Threading.WaitHandle" /> other than <see cref="T:System.Threading.Mutex" />.</param>
		/// <param name="callBack">The <see cref="T:System.Threading.WaitOrTimerCallback" /> delegate to call when the <paramref name="waitObject" /> parameter is signaled.</param>
		/// <param name="state">The object passed to the delegate.</param>
		/// <param name="millisecondsTimeOutInterval">The time-out in milliseconds. If the <paramref name="millisecondsTimeOutInterval" /> parameter is 0 (zero), the function tests the object's state and returns immediately. If <paramref name="millisecondsTimeOutInterval" /> is -1, the function's time-out interval never elapses.</param>
		/// <param name="executeOnlyOnce">
		///   <see langword="true" /> to indicate that the thread will no longer wait on the <paramref name="waitObject" /> parameter after the delegate has been called; <see langword="false" /> to indicate that the timer is reset every time the wait operation completes until the wait is unregistered.</param>
		/// <returns>The <see cref="T:System.Threading.RegisteredWaitHandle" /> that can be used to cancel the registered wait operation.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="millisecondsTimeOutInterval" /> parameter is less than -1.</exception>
		// Token: 0x0600203A RID: 8250 RVA: 0x000757E8 File Offset: 0x000739E8
		[SecuritySafeCritical]
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static RegisteredWaitHandle RegisterWaitForSingleObject(WaitHandle waitObject, WaitOrTimerCallback callBack, object state, uint millisecondsTimeOutInterval, bool executeOnlyOnce)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return ThreadPool.RegisterWaitForSingleObject(waitObject, callBack, state, millisecondsTimeOutInterval, executeOnlyOnce, ref stackCrawlMark, true);
		}

		/// <summary>Registers a delegate to wait for a <see cref="T:System.Threading.WaitHandle" />, specifying a 32-bit unsigned integer for the time-out in milliseconds. This method does not propagate the calling stack to the worker thread.</summary>
		/// <param name="waitObject">The <see cref="T:System.Threading.WaitHandle" /> to register. Use a <see cref="T:System.Threading.WaitHandle" /> other than <see cref="T:System.Threading.Mutex" />.</param>
		/// <param name="callBack">The delegate to call when the <paramref name="waitObject" /> parameter is signaled.</param>
		/// <param name="state">The object that is passed to the delegate.</param>
		/// <param name="millisecondsTimeOutInterval">The time-out in milliseconds. If the <paramref name="millisecondsTimeOutInterval" /> parameter is 0 (zero), the function tests the object's state and returns immediately. If <paramref name="millisecondsTimeOutInterval" /> is -1, the function's time-out interval never elapses.</param>
		/// <param name="executeOnlyOnce">
		///   <see langword="true" /> to indicate that the thread will no longer wait on the <paramref name="waitObject" /> parameter after the delegate has been called; <see langword="false" /> to indicate that the timer is reset every time the wait operation completes until the wait is unregistered.</param>
		/// <returns>The <see cref="T:System.Threading.RegisteredWaitHandle" /> object that can be used to cancel the registered wait operation.</returns>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x0600203B RID: 8251 RVA: 0x00075808 File Offset: 0x00073A08
		[SecurityCritical]
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static RegisteredWaitHandle UnsafeRegisterWaitForSingleObject(WaitHandle waitObject, WaitOrTimerCallback callBack, object state, uint millisecondsTimeOutInterval, bool executeOnlyOnce)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return ThreadPool.RegisterWaitForSingleObject(waitObject, callBack, state, millisecondsTimeOutInterval, executeOnlyOnce, ref stackCrawlMark, false);
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x00075828 File Offset: 0x00073A28
		[SecurityCritical]
		private static RegisteredWaitHandle RegisterWaitForSingleObject(WaitHandle waitObject, WaitOrTimerCallback callBack, object state, uint millisecondsTimeOutInterval, bool executeOnlyOnce, ref StackCrawlMark stackMark, bool compressStack)
		{
			if (waitObject == null)
			{
				throw new ArgumentNullException("waitObject");
			}
			if (callBack == null)
			{
				throw new ArgumentNullException("callBack");
			}
			if (millisecondsTimeOutInterval != 4294967295U && millisecondsTimeOutInterval > 2147483647U)
			{
				throw new NotSupportedException("Timeout is too big. Maximum is Int32.MaxValue");
			}
			RegisteredWaitHandle registeredWaitHandle = new RegisteredWaitHandle(waitObject, callBack, state, new TimeSpan(0, 0, 0, 0, (int)millisecondsTimeOutInterval), executeOnlyOnce);
			if (compressStack)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(registeredWaitHandle.Wait), null);
			}
			else
			{
				ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(registeredWaitHandle.Wait), null);
			}
			return registeredWaitHandle;
		}

		/// <summary>Registers a delegate to wait for a <see cref="T:System.Threading.WaitHandle" />, specifying a 32-bit signed integer for the time-out in milliseconds.</summary>
		/// <param name="waitObject">The <see cref="T:System.Threading.WaitHandle" /> to register. Use a <see cref="T:System.Threading.WaitHandle" /> other than <see cref="T:System.Threading.Mutex" />.</param>
		/// <param name="callBack">The <see cref="T:System.Threading.WaitOrTimerCallback" /> delegate to call when the <paramref name="waitObject" /> parameter is signaled.</param>
		/// <param name="state">The object that is passed to the delegate.</param>
		/// <param name="millisecondsTimeOutInterval">The time-out in milliseconds. If the <paramref name="millisecondsTimeOutInterval" /> parameter is 0 (zero), the function tests the object's state and returns immediately. If <paramref name="millisecondsTimeOutInterval" /> is -1, the function's time-out interval never elapses.</param>
		/// <param name="executeOnlyOnce">
		///   <see langword="true" /> to indicate that the thread will no longer wait on the <paramref name="waitObject" /> parameter after the delegate has been called; <see langword="false" /> to indicate that the timer is reset every time the wait operation completes until the wait is unregistered.</param>
		/// <returns>The <see cref="T:System.Threading.RegisteredWaitHandle" /> that encapsulates the native handle.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="millisecondsTimeOutInterval" /> parameter is less than -1.</exception>
		// Token: 0x0600203D RID: 8253 RVA: 0x000758AC File Offset: 0x00073AAC
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static RegisteredWaitHandle RegisterWaitForSingleObject(WaitHandle waitObject, WaitOrTimerCallback callBack, object state, int millisecondsTimeOutInterval, bool executeOnlyOnce)
		{
			if (millisecondsTimeOutInterval < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeOutInterval", Environment.GetResourceString("Number must be either non-negative and less than or equal to Int32.MaxValue or -1."));
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return ThreadPool.RegisterWaitForSingleObject(waitObject, callBack, state, (uint)((millisecondsTimeOutInterval == -1) ? (-1) : millisecondsTimeOutInterval), executeOnlyOnce, ref stackCrawlMark, true);
		}

		/// <summary>Registers a delegate to wait for a <see cref="T:System.Threading.WaitHandle" />, using a 32-bit signed integer for the time-out in milliseconds. This method does not propagate the calling stack to the worker thread.</summary>
		/// <param name="waitObject">The <see cref="T:System.Threading.WaitHandle" /> to register. Use a <see cref="T:System.Threading.WaitHandle" /> other than <see cref="T:System.Threading.Mutex" />.</param>
		/// <param name="callBack">The delegate to call when the <paramref name="waitObject" /> parameter is signaled.</param>
		/// <param name="state">The object that is passed to the delegate.</param>
		/// <param name="millisecondsTimeOutInterval">The time-out in milliseconds. If the <paramref name="millisecondsTimeOutInterval" /> parameter is 0 (zero), the function tests the object's state and returns immediately. If <paramref name="millisecondsTimeOutInterval" /> is -1, the function's time-out interval never elapses.</param>
		/// <param name="executeOnlyOnce">
		///   <see langword="true" /> to indicate that the thread will no longer wait on the <paramref name="waitObject" /> parameter after the delegate has been called; <see langword="false" /> to indicate that the timer is reset every time the wait operation completes until the wait is unregistered.</param>
		/// <returns>The <see cref="T:System.Threading.RegisteredWaitHandle" /> object that can be used to cancel the registered wait operation.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="millisecondsTimeOutInterval" /> parameter is less than -1.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x0600203E RID: 8254 RVA: 0x000758EC File Offset: 0x00073AEC
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static RegisteredWaitHandle UnsafeRegisterWaitForSingleObject(WaitHandle waitObject, WaitOrTimerCallback callBack, object state, int millisecondsTimeOutInterval, bool executeOnlyOnce)
		{
			if (millisecondsTimeOutInterval < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeOutInterval", Environment.GetResourceString("Number must be either non-negative and less than or equal to Int32.MaxValue or -1."));
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return ThreadPool.RegisterWaitForSingleObject(waitObject, callBack, state, (uint)((millisecondsTimeOutInterval == -1) ? (-1) : millisecondsTimeOutInterval), executeOnlyOnce, ref stackCrawlMark, false);
		}

		/// <summary>Registers a delegate to wait for a <see cref="T:System.Threading.WaitHandle" />, specifying a 64-bit signed integer for the time-out in milliseconds.</summary>
		/// <param name="waitObject">The <see cref="T:System.Threading.WaitHandle" /> to register. Use a <see cref="T:System.Threading.WaitHandle" /> other than <see cref="T:System.Threading.Mutex" />.</param>
		/// <param name="callBack">The <see cref="T:System.Threading.WaitOrTimerCallback" /> delegate to call when the <paramref name="waitObject" /> parameter is signaled.</param>
		/// <param name="state">The object passed to the delegate.</param>
		/// <param name="millisecondsTimeOutInterval">The time-out in milliseconds. If the <paramref name="millisecondsTimeOutInterval" /> parameter is 0 (zero), the function tests the object's state and returns immediately. If <paramref name="millisecondsTimeOutInterval" /> is -1, the function's time-out interval never elapses.</param>
		/// <param name="executeOnlyOnce">
		///   <see langword="true" /> to indicate that the thread will no longer wait on the <paramref name="waitObject" /> parameter after the delegate has been called; <see langword="false" /> to indicate that the timer is reset every time the wait operation completes until the wait is unregistered.</param>
		/// <returns>The <see cref="T:System.Threading.RegisteredWaitHandle" /> that encapsulates the native handle.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="millisecondsTimeOutInterval" /> parameter is less than -1.</exception>
		// Token: 0x0600203F RID: 8255 RVA: 0x0007592C File Offset: 0x00073B2C
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static RegisteredWaitHandle RegisterWaitForSingleObject(WaitHandle waitObject, WaitOrTimerCallback callBack, object state, long millisecondsTimeOutInterval, bool executeOnlyOnce)
		{
			if (millisecondsTimeOutInterval < -1L)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeOutInterval", Environment.GetResourceString("Number must be either non-negative and less than or equal to Int32.MaxValue or -1."));
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return ThreadPool.RegisterWaitForSingleObject(waitObject, callBack, state, (millisecondsTimeOutInterval == -1L) ? uint.MaxValue : ((uint)millisecondsTimeOutInterval), executeOnlyOnce, ref stackCrawlMark, true);
		}

		/// <summary>Registers a delegate to wait for a <see cref="T:System.Threading.WaitHandle" />, specifying a 64-bit signed integer for the time-out in milliseconds. This method does not propagate the calling stack to the worker thread.</summary>
		/// <param name="waitObject">The <see cref="T:System.Threading.WaitHandle" /> to register. Use a <see cref="T:System.Threading.WaitHandle" /> other than <see cref="T:System.Threading.Mutex" />.</param>
		/// <param name="callBack">The delegate to call when the <paramref name="waitObject" /> parameter is signaled.</param>
		/// <param name="state">The object that is passed to the delegate.</param>
		/// <param name="millisecondsTimeOutInterval">The time-out in milliseconds. If the <paramref name="millisecondsTimeOutInterval" /> parameter is 0 (zero), the function tests the object's state and returns immediately. If <paramref name="millisecondsTimeOutInterval" /> is -1, the function's time-out interval never elapses.</param>
		/// <param name="executeOnlyOnce">
		///   <see langword="true" /> to indicate that the thread will no longer wait on the <paramref name="waitObject" /> parameter after the delegate has been called; <see langword="false" /> to indicate that the timer is reset every time the wait operation completes until the wait is unregistered.</param>
		/// <returns>The <see cref="T:System.Threading.RegisteredWaitHandle" /> object that can be used to cancel the registered wait operation.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="millisecondsTimeOutInterval" /> parameter is less than -1.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06002040 RID: 8256 RVA: 0x0007596C File Offset: 0x00073B6C
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static RegisteredWaitHandle UnsafeRegisterWaitForSingleObject(WaitHandle waitObject, WaitOrTimerCallback callBack, object state, long millisecondsTimeOutInterval, bool executeOnlyOnce)
		{
			if (millisecondsTimeOutInterval < -1L)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeOutInterval", Environment.GetResourceString("Number must be either non-negative and less than or equal to Int32.MaxValue or -1."));
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return ThreadPool.RegisterWaitForSingleObject(waitObject, callBack, state, (millisecondsTimeOutInterval == -1L) ? uint.MaxValue : ((uint)millisecondsTimeOutInterval), executeOnlyOnce, ref stackCrawlMark, false);
		}

		/// <summary>Registers a delegate to wait for a <see cref="T:System.Threading.WaitHandle" />, specifying a <see cref="T:System.TimeSpan" /> value for the time-out.</summary>
		/// <param name="waitObject">The <see cref="T:System.Threading.WaitHandle" /> to register. Use a <see cref="T:System.Threading.WaitHandle" /> other than <see cref="T:System.Threading.Mutex" />.</param>
		/// <param name="callBack">The <see cref="T:System.Threading.WaitOrTimerCallback" /> delegate to call when the <paramref name="waitObject" /> parameter is signaled.</param>
		/// <param name="state">The object passed to the delegate.</param>
		/// <param name="timeout">The time-out represented by a <see cref="T:System.TimeSpan" />. If <paramref name="timeout" /> is 0 (zero), the function tests the object's state and returns immediately. If <paramref name="timeout" /> is -1, the function's time-out interval never elapses.</param>
		/// <param name="executeOnlyOnce">
		///   <see langword="true" /> to indicate that the thread will no longer wait on the <paramref name="waitObject" /> parameter after the delegate has been called; <see langword="false" /> to indicate that the timer is reset every time the wait operation completes until the wait is unregistered.</param>
		/// <returns>The <see cref="T:System.Threading.RegisteredWaitHandle" /> that encapsulates the native handle.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="timeout" /> parameter is less than -1.</exception>
		/// <exception cref="T:System.NotSupportedException">The <paramref name="timeout" /> parameter is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		// Token: 0x06002041 RID: 8257 RVA: 0x000759AC File Offset: 0x00073BAC
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static RegisteredWaitHandle RegisterWaitForSingleObject(WaitHandle waitObject, WaitOrTimerCallback callBack, object state, TimeSpan timeout, bool executeOnlyOnce)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("Number must be either non-negative and less than or equal to Int32.MaxValue or -1."));
			}
			if (num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("Argument must be less than or equal to 2^31 - 1 milliseconds."));
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return ThreadPool.RegisterWaitForSingleObject(waitObject, callBack, state, (uint)num, executeOnlyOnce, ref stackCrawlMark, true);
		}

		/// <summary>Registers a delegate to wait for a <see cref="T:System.Threading.WaitHandle" />, specifying a <see cref="T:System.TimeSpan" /> value for the time-out. This method does not propagate the calling stack to the worker thread.</summary>
		/// <param name="waitObject">The <see cref="T:System.Threading.WaitHandle" /> to register. Use a <see cref="T:System.Threading.WaitHandle" /> other than <see cref="T:System.Threading.Mutex" />.</param>
		/// <param name="callBack">The delegate to call when the <paramref name="waitObject" /> parameter is signaled.</param>
		/// <param name="state">The object that is passed to the delegate.</param>
		/// <param name="timeout">The time-out represented by a <see cref="T:System.TimeSpan" />. If <paramref name="timeout" /> is 0 (zero), the function tests the object's state and returns immediately. If <paramref name="timeout" /> is -1, the function's time-out interval never elapses.</param>
		/// <param name="executeOnlyOnce">
		///   <see langword="true" /> to indicate that the thread will no longer wait on the <paramref name="waitObject" /> parameter after the delegate has been called; <see langword="false" /> to indicate that the timer is reset every time the wait operation completes until the wait is unregistered.</param>
		/// <returns>The <see cref="T:System.Threading.RegisteredWaitHandle" /> object that can be used to cancel the registered wait operation.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="timeout" /> parameter is less than -1.</exception>
		/// <exception cref="T:System.NotSupportedException">The <paramref name="timeout" /> parameter is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06002042 RID: 8258 RVA: 0x00075A0C File Offset: 0x00073C0C
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static RegisteredWaitHandle UnsafeRegisterWaitForSingleObject(WaitHandle waitObject, WaitOrTimerCallback callBack, object state, TimeSpan timeout, bool executeOnlyOnce)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("Number must be either non-negative and less than or equal to Int32.MaxValue or -1."));
			}
			if (num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout", Environment.GetResourceString("Argument must be less than or equal to 2^31 - 1 milliseconds."));
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return ThreadPool.RegisterWaitForSingleObject(waitObject, callBack, state, (uint)num, executeOnlyOnce, ref stackCrawlMark, false);
		}

		/// <summary>Queues a method for execution, and specifies an object containing data to be used by the method. The method executes when a thread pool thread becomes available.</summary>
		/// <param name="callBack">A <see cref="T:System.Threading.WaitCallback" /> representing the method to execute.</param>
		/// <param name="state">An object containing data to be used by the method.</param>
		/// <returns>
		///   <see langword="true" /> if the method is successfully queued; <see cref="T:System.NotSupportedException" /> is thrown if the work item could not be queued.</returns>
		/// <exception cref="T:System.NotSupportedException">The common language runtime (CLR) is hosted, and the host does not support this action.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="callBack" /> is <see langword="null" />.</exception>
		// Token: 0x06002043 RID: 8259 RVA: 0x00075A6C File Offset: 0x00073C6C
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static bool QueueUserWorkItem(WaitCallback callBack, object state)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return ThreadPool.QueueUserWorkItemHelper(callBack, state, ref stackCrawlMark, true, true);
		}

		/// <summary>Queues a method for execution. The method executes when a thread pool thread becomes available.</summary>
		/// <param name="callBack">A <see cref="T:System.Threading.WaitCallback" /> that represents the method to be executed.</param>
		/// <returns>
		///   <see langword="true" /> if the method is successfully queued; <see cref="T:System.NotSupportedException" /> is thrown if the work item could not be queued.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="callBack" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.NotSupportedException">The common language runtime (CLR) is hosted, and the host does not support this action.</exception>
		// Token: 0x06002044 RID: 8260 RVA: 0x00075A88 File Offset: 0x00073C88
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static bool QueueUserWorkItem(WaitCallback callBack)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return ThreadPool.QueueUserWorkItemHelper(callBack, null, ref stackCrawlMark, true, true);
		}

		/// <summary>Queues the specified delegate to the thread pool, but does not propagate the calling stack to the worker thread.</summary>
		/// <param name="callBack">A <see cref="T:System.Threading.WaitCallback" /> that represents the delegate to invoke when a thread in the thread pool picks up the work item.</param>
		/// <param name="state">The object that is passed to the delegate when serviced from the thread pool.</param>
		/// <returns>
		///   <see langword="true" /> if the method succeeds; <see cref="T:System.OutOfMemoryException" /> is thrown if the work item could not be queued.</returns>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.ApplicationException">An out-of-memory condition was encountered.</exception>
		/// <exception cref="T:System.OutOfMemoryException">The work item could not be queued.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="callBack" /> is <see langword="null" />.</exception>
		// Token: 0x06002045 RID: 8261 RVA: 0x00075AA4 File Offset: 0x00073CA4
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static bool UnsafeQueueUserWorkItem(WaitCallback callBack, object state)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return ThreadPool.QueueUserWorkItemHelper(callBack, state, ref stackCrawlMark, false, true);
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x00075AC0 File Offset: 0x00073CC0
		public static bool QueueUserWorkItem<TState>(Action<TState> callBack, TState state, bool preferLocal)
		{
			if (callBack == null)
			{
				throw new ArgumentNullException("callBack");
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return ThreadPool.QueueUserWorkItemHelper(delegate(object x)
			{
				callBack((TState)((object)x));
			}, state, ref stackCrawlMark, true, !preferLocal);
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x00075B0C File Offset: 0x00073D0C
		public static bool UnsafeQueueUserWorkItem<TState>(Action<TState> callBack, TState state, bool preferLocal)
		{
			if (callBack == null)
			{
				throw new ArgumentNullException("callBack");
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return ThreadPool.QueueUserWorkItemHelper(delegate(object x)
			{
				callBack((TState)((object)x));
			}, state, ref stackCrawlMark, false, !preferLocal);
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x00075B58 File Offset: 0x00073D58
		[SecurityCritical]
		private static bool QueueUserWorkItemHelper(WaitCallback callBack, object state, ref StackCrawlMark stackMark, bool compressStack, bool forceGlobal = true)
		{
			bool flag = true;
			if (callBack != null)
			{
				ThreadPool.EnsureVMInitialized();
				try
				{
					return flag;
				}
				finally
				{
					QueueUserWorkItemCallback queueUserWorkItemCallback = new QueueUserWorkItemCallback(callBack, state, compressStack, ref stackMark);
					ThreadPoolGlobals.workQueue.Enqueue(queueUserWorkItemCallback, forceGlobal);
					flag = true;
				}
			}
			throw new ArgumentNullException("WaitCallback");
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x00075BA8 File Offset: 0x00073DA8
		[SecurityCritical]
		internal static void UnsafeQueueCustomWorkItem(IThreadPoolWorkItem workItem, bool forceGlobal)
		{
			ThreadPool.EnsureVMInitialized();
			try
			{
			}
			finally
			{
				ThreadPoolGlobals.workQueue.Enqueue(workItem, forceGlobal);
			}
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x00075BDC File Offset: 0x00073DDC
		[SecurityCritical]
		internal static bool TryPopCustomWorkItem(IThreadPoolWorkItem workItem)
		{
			return ThreadPoolGlobals.vmTpInitialized && ThreadPoolGlobals.workQueue.LocalFindAndPop(workItem);
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x00075BF4 File Offset: 0x00073DF4
		[SecurityCritical]
		internal static IEnumerable<IThreadPoolWorkItem> GetQueuedWorkItems()
		{
			return ThreadPool.EnumerateQueuedWorkItems(ThreadPoolWorkQueue.allThreadQueues.Current, ThreadPoolGlobals.workQueue.queueTail);
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x00075C11 File Offset: 0x00073E11
		internal static IEnumerable<IThreadPoolWorkItem> EnumerateQueuedWorkItems(ThreadPoolWorkQueue.WorkStealingQueue[] wsQueues, ThreadPoolWorkQueue.QueueSegment globalQueueTail)
		{
			if (wsQueues != null)
			{
				foreach (ThreadPoolWorkQueue.WorkStealingQueue workStealingQueue in wsQueues)
				{
					if (workStealingQueue != null && workStealingQueue.m_array != null)
					{
						IThreadPoolWorkItem[] items = workStealingQueue.m_array;
						int num;
						for (int i = 0; i < items.Length; i = num + 1)
						{
							IThreadPoolWorkItem threadPoolWorkItem = items[i];
							if (threadPoolWorkItem != null)
							{
								yield return threadPoolWorkItem;
							}
							num = i;
						}
						items = null;
					}
				}
				ThreadPoolWorkQueue.WorkStealingQueue[] array = null;
			}
			if (globalQueueTail != null)
			{
				ThreadPoolWorkQueue.QueueSegment segment;
				for (segment = globalQueueTail; segment != null; segment = segment.Next)
				{
					IThreadPoolWorkItem[] items = segment.nodes;
					int num;
					for (int j = 0; j < items.Length; j = num + 1)
					{
						IThreadPoolWorkItem threadPoolWorkItem2 = items[j];
						if (threadPoolWorkItem2 != null)
						{
							yield return threadPoolWorkItem2;
						}
						num = j;
					}
					items = null;
				}
				segment = null;
			}
			yield break;
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x00075C28 File Offset: 0x00073E28
		[SecurityCritical]
		internal static IEnumerable<IThreadPoolWorkItem> GetLocallyQueuedWorkItems()
		{
			return ThreadPool.EnumerateQueuedWorkItems(new ThreadPoolWorkQueue.WorkStealingQueue[] { ThreadPoolWorkQueueThreadLocals.threadLocals.workStealingQueue }, null);
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x00075C43 File Offset: 0x00073E43
		[SecurityCritical]
		internal static IEnumerable<IThreadPoolWorkItem> GetGloballyQueuedWorkItems()
		{
			return ThreadPool.EnumerateQueuedWorkItems(null, ThreadPoolGlobals.workQueue.queueTail);
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x00075C58 File Offset: 0x00073E58
		private static object[] ToObjectArray(IEnumerable<IThreadPoolWorkItem> workitems)
		{
			int num = 0;
			foreach (IThreadPoolWorkItem threadPoolWorkItem in workitems)
			{
				num++;
			}
			object[] array = new object[num];
			num = 0;
			foreach (IThreadPoolWorkItem threadPoolWorkItem2 in workitems)
			{
				if (num < array.Length)
				{
					array[num] = threadPoolWorkItem2;
				}
				num++;
			}
			return array;
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x00075CE8 File Offset: 0x00073EE8
		[SecurityCritical]
		internal static object[] GetQueuedWorkItemsForDebugger()
		{
			return ThreadPool.ToObjectArray(ThreadPool.GetQueuedWorkItems());
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x00075CF4 File Offset: 0x00073EF4
		[SecurityCritical]
		internal static object[] GetGloballyQueuedWorkItemsForDebugger()
		{
			return ThreadPool.ToObjectArray(ThreadPool.GetGloballyQueuedWorkItems());
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x00075D00 File Offset: 0x00073F00
		[SecurityCritical]
		internal static object[] GetLocallyQueuedWorkItemsForDebugger()
		{
			return ThreadPool.ToObjectArray(ThreadPool.GetLocallyQueuedWorkItems());
		}

		// Token: 0x06002053 RID: 8275
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool RequestWorkerThread();

		// Token: 0x06002054 RID: 8276
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool PostQueuedCompletionStatus(NativeOverlapped* overlapped);

		/// <summary>Queues an overlapped I/O operation for execution.</summary>
		/// <param name="overlapped">The <see cref="T:System.Threading.NativeOverlapped" /> structure to queue.</param>
		/// <returns>
		///   <see langword="true" /> if the operation was successfully queued to an I/O completion port; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002055 RID: 8277 RVA: 0x00075D0C File Offset: 0x00073F0C
		[SecurityCritical]
		[CLSCompliant(false)]
		public unsafe static bool UnsafeQueueNativeOverlapped(NativeOverlapped* overlapped)
		{
			throw new NotImplementedException("");
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x00075D18 File Offset: 0x00073F18
		[SecurityCritical]
		private static void EnsureVMInitialized()
		{
			if (!ThreadPoolGlobals.vmTpInitialized)
			{
				ThreadPool.InitializeVMTp(ref ThreadPoolGlobals.enableWorkerTracking);
				ThreadPoolGlobals.vmTpInitialized = true;
			}
		}

		// Token: 0x06002057 RID: 8279
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SetMinThreadsNative(int workerThreads, int completionPortThreads);

		// Token: 0x06002058 RID: 8280
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SetMaxThreadsNative(int workerThreads, int completionPortThreads);

		// Token: 0x06002059 RID: 8281
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetMinThreadsNative(out int workerThreads, out int completionPortThreads);

		// Token: 0x0600205A RID: 8282
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetMaxThreadsNative(out int workerThreads, out int completionPortThreads);

		// Token: 0x0600205B RID: 8283
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetAvailableThreadsNative(out int workerThreads, out int completionPortThreads);

		// Token: 0x0600205C RID: 8284
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool NotifyWorkItemComplete();

		// Token: 0x0600205D RID: 8285
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ReportThreadStatus(bool isWorking);

		// Token: 0x0600205E RID: 8286 RVA: 0x00075D35 File Offset: 0x00073F35
		[SecuritySafeCritical]
		internal static void NotifyWorkItemProgress()
		{
			ThreadPool.EnsureVMInitialized();
			ThreadPool.NotifyWorkItemProgressNative();
		}

		// Token: 0x0600205F RID: 8287
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void NotifyWorkItemProgressNative();

		// Token: 0x06002060 RID: 8288
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void NotifyWorkItemQueued();

		// Token: 0x06002061 RID: 8289 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		[SecurityCritical]
		internal static bool IsThreadPoolHosted()
		{
			return false;
		}

		// Token: 0x06002062 RID: 8290
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InitializeVMTp(ref bool enableWorkerTracking);

		/// <summary>Binds an operating system handle to the <see cref="T:System.Threading.ThreadPool" />.</summary>
		/// <param name="osHandle">An <see cref="T:System.IntPtr" /> that holds the handle. The handle must have been opened for overlapped I/O on the unmanaged side.</param>
		/// <returns>
		///   <see langword="true" /> if the handle is bound; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06002063 RID: 8291 RVA: 0x00075D41 File Offset: 0x00073F41
		[Obsolete("ThreadPool.BindHandle(IntPtr) has been deprecated.  Please use ThreadPool.BindHandle(SafeHandle) instead.", false)]
		[SecuritySafeCritical]
		public static bool BindHandle(IntPtr osHandle)
		{
			return ThreadPool.BindIOCompletionCallbackNative(osHandle);
		}

		/// <summary>Binds an operating system handle to the <see cref="T:System.Threading.ThreadPool" />.</summary>
		/// <param name="osHandle">A <see cref="T:System.Runtime.InteropServices.SafeHandle" /> that holds the operating system handle. The handle must have been opened for overlapped I/O on the unmanaged side.</param>
		/// <returns>
		///   <see langword="true" /> if the handle is bound; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="osHandle" /> is <see langword="null" />.</exception>
		// Token: 0x06002064 RID: 8292 RVA: 0x00075D4C File Offset: 0x00073F4C
		[SecuritySafeCritical]
		public static bool BindHandle(SafeHandle osHandle)
		{
			if (osHandle == null)
			{
				throw new ArgumentNullException("osHandle");
			}
			bool flag = false;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				osHandle.DangerousAddRef(ref flag2);
				flag = ThreadPool.BindIOCompletionCallbackNative(osHandle.DangerousGetHandle());
			}
			finally
			{
				if (flag2)
				{
					osHandle.DangerousRelease();
				}
			}
			return flag;
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x000040F7 File Offset: 0x000022F7
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityCritical]
		private static bool BindIOCompletionCallbackNative(IntPtr fileHandle)
		{
			return true;
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06002066 RID: 8294 RVA: 0x00075DA4 File Offset: 0x00073FA4
		internal static bool IsThreadPoolThread
		{
			get
			{
				return Thread.CurrentThread.IsThreadPoolThread;
			}
		}
	}
}
