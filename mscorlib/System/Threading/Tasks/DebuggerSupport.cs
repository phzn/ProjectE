using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Internal.Runtime.Augments;

namespace System.Threading.Tasks
{
	// Token: 0x0200033E RID: 830
	internal static class DebuggerSupport
	{
		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x060022B1 RID: 8881 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public static bool LoggingOn
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public static void TraceOperationCreation(CausalityTraceLevel traceLevel, Task task, string operationName, ulong relatedContext)
		{
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public static void TraceOperationCompletion(CausalityTraceLevel traceLevel, Task task, AsyncStatus status)
		{
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public static void TraceOperationRelation(CausalityTraceLevel traceLevel, Task task, CausalityRelation relation)
		{
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public static void TraceSynchronousWorkStart(CausalityTraceLevel traceLevel, Task task, CausalitySynchronousWork work)
		{
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public static void TraceSynchronousWorkCompletion(CausalityTraceLevel traceLevel, CausalitySynchronousWork work)
		{
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x0007CCD1 File Offset: 0x0007AED1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AddToActiveTasks(Task task)
		{
			if (Task.s_asyncDebuggingEnabled)
			{
				DebuggerSupport.AddToActiveTasksNonInlined(task);
			}
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x0007CCE0 File Offset: 0x0007AEE0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void AddToActiveTasksNonInlined(Task task)
		{
			int id = task.Id;
			object obj = DebuggerSupport.s_activeTasksLock;
			lock (obj)
			{
				DebuggerSupport.s_activeTasks[id] = task;
			}
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x0007CD2C File Offset: 0x0007AF2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void RemoveFromActiveTasks(Task task)
		{
			if (Task.s_asyncDebuggingEnabled)
			{
				DebuggerSupport.RemoveFromActiveTasksNonInlined(task);
			}
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x0007CD3C File Offset: 0x0007AF3C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void RemoveFromActiveTasksNonInlined(Task task)
		{
			int id = task.Id;
			object obj = DebuggerSupport.s_activeTasksLock;
			lock (obj)
			{
				DebuggerSupport.s_activeTasks.Remove(id);
			}
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x0007CD88 File Offset: 0x0007AF88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Task GetActiveTaskFromId(int taskId)
		{
			Task task = null;
			DebuggerSupport.s_activeTasks.TryGetValue(taskId, out task);
			return task;
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x0007CDA6 File Offset: 0x0007AFA6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Task GetTaskIfDebuggingEnabled(this AsyncVoidMethodBuilder builder)
		{
			if (DebuggerSupport.LoggingOn || Task.s_asyncDebuggingEnabled)
			{
				return builder.Task;
			}
			return null;
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x0007CDBF File Offset: 0x0007AFBF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Task GetTaskIfDebuggingEnabled(this AsyncTaskMethodBuilder builder)
		{
			if (DebuggerSupport.LoggingOn || Task.s_asyncDebuggingEnabled)
			{
				return builder.Task;
			}
			return null;
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x0007CDD8 File Offset: 0x0007AFD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Task GetTaskIfDebuggingEnabled<TResult>(this AsyncTaskMethodBuilder<TResult> builder)
		{
			if (DebuggerSupport.LoggingOn || Task.s_asyncDebuggingEnabled)
			{
				return builder.Task;
			}
			return null;
		}

		// Token: 0x04001C81 RID: 7297
		private static readonly LowLevelDictionary<int, Task> s_activeTasks = new LowLevelDictionary<int, Task>();

		// Token: 0x04001C82 RID: 7298
		private static readonly object s_activeTasksLock = new object();
	}
}
