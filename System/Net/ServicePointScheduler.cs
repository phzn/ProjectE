using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
	// Token: 0x020006B0 RID: 1712
	internal class ServicePointScheduler
	{
		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x0600370A RID: 14090 RVA: 0x000C0BEE File Offset: 0x000BEDEE
		// (set) Token: 0x0600370B RID: 14091 RVA: 0x000C0BF6 File Offset: 0x000BEDF6
		private ServicePoint ServicePoint { get; set; }

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x0600370C RID: 14092 RVA: 0x000C0BFF File Offset: 0x000BEDFF
		// (set) Token: 0x0600370D RID: 14093 RVA: 0x000C0C07 File Offset: 0x000BEE07
		public int MaxIdleTime
		{
			get
			{
				return this.maxIdleTime;
			}
			set
			{
				if (value < -1 || value > 2147483647)
				{
					throw new ArgumentOutOfRangeException();
				}
				if (value == this.maxIdleTime)
				{
					return;
				}
				this.maxIdleTime = value;
				this.Run();
			}
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x0600370E RID: 14094 RVA: 0x000C0C32 File Offset: 0x000BEE32
		// (set) Token: 0x0600370F RID: 14095 RVA: 0x000C0C3A File Offset: 0x000BEE3A
		public int ConnectionLimit
		{
			get
			{
				return this.connectionLimit;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException();
				}
				if (value == this.connectionLimit)
				{
					return;
				}
				this.connectionLimit = value;
				this.Run();
			}
		}

		// Token: 0x06003710 RID: 14096 RVA: 0x000C0C60 File Offset: 0x000BEE60
		public ServicePointScheduler(ServicePoint servicePoint, int connectionLimit, int maxIdleTime)
		{
			this.ServicePoint = servicePoint;
			this.connectionLimit = connectionLimit;
			this.maxIdleTime = maxIdleTime;
			this.schedulerEvent = new ServicePointScheduler.AsyncManualResetEvent(false);
			this.defaultGroup = new ServicePointScheduler.ConnectionGroup(this, string.Empty);
			this.operations = new LinkedList<ValueTuple<ServicePointScheduler.ConnectionGroup, WebOperation>>();
			this.idleConnections = new LinkedList<ValueTuple<ServicePointScheduler.ConnectionGroup, WebConnection, Task>>();
			this.idleSince = DateTime.UtcNow;
		}

		// Token: 0x06003711 RID: 14097 RVA: 0x00003917 File Offset: 0x00001B17
		[Conditional("MONO_WEB_DEBUG")]
		private void Debug(string message)
		{
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06003712 RID: 14098 RVA: 0x000C0CE4 File Offset: 0x000BEEE4
		public int CurrentConnections
		{
			get
			{
				return this.currentConnections;
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x06003713 RID: 14099 RVA: 0x000C0CEC File Offset: 0x000BEEEC
		public DateTime IdleSince
		{
			get
			{
				return this.idleSince;
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x06003714 RID: 14100 RVA: 0x000C0CF4 File Offset: 0x000BEEF4
		internal string ME { get; }

		// Token: 0x06003715 RID: 14101 RVA: 0x000C0CFC File Offset: 0x000BEEFC
		public void Run()
		{
			if (Interlocked.CompareExchange(ref this.running, 1, 0) == 0)
			{
				Task.Run(() => this.RunScheduler());
			}
			this.schedulerEvent.Set();
		}

		// Token: 0x06003716 RID: 14102 RVA: 0x000C0D2C File Offset: 0x000BEF2C
		private async Task RunScheduler()
		{
			this.idleSince = DateTime.UtcNow + TimeSpan.FromDays(3650.0);
			for (;;)
			{
				List<Task> taskList = new List<Task>();
				bool finalCleanup = false;
				ServicePoint servicePoint = this.ServicePoint;
				ValueTuple<ServicePointScheduler.ConnectionGroup, WebOperation>[] operationArray;
				ValueTuple<ServicePointScheduler.ConnectionGroup, WebConnection, Task>[] idleArray;
				Task<bool> schedulerTask;
				lock (servicePoint)
				{
					this.Cleanup();
					operationArray = new ValueTuple<ServicePointScheduler.ConnectionGroup, WebOperation>[this.operations.Count];
					this.operations.CopyTo(operationArray, 0);
					idleArray = new ValueTuple<ServicePointScheduler.ConnectionGroup, WebConnection, Task>[this.idleConnections.Count];
					this.idleConnections.CopyTo(idleArray, 0);
					schedulerTask = this.schedulerEvent.WaitAsync(this.maxIdleTime);
					taskList.Add(schedulerTask);
					if (this.groups == null && this.defaultGroup.IsEmpty() && this.operations.Count == 0 && this.idleConnections.Count == 0)
					{
						this.idleSince = DateTime.UtcNow;
						finalCleanup = true;
					}
					else
					{
						foreach (ValueTuple<ServicePointScheduler.ConnectionGroup, WebOperation> valueTuple in operationArray)
						{
							taskList.Add(valueTuple.Item2.Finished.Task);
						}
						foreach (ValueTuple<ServicePointScheduler.ConnectionGroup, WebConnection, Task> valueTuple2 in idleArray)
						{
							taskList.Add(valueTuple2.Item3);
						}
					}
				}
				Task task = await Task.WhenAny(taskList).ConfigureAwait(false);
				servicePoint = this.ServicePoint;
				lock (servicePoint)
				{
					bool flag2 = false;
					if (finalCleanup)
					{
						if (!schedulerTask.Result)
						{
							this.FinalCleanup();
							break;
						}
						flag2 = true;
					}
					else if (task == taskList[0])
					{
						flag2 = true;
					}
					foreach (ValueTuple<ServicePointScheduler.ConnectionGroup, WebOperation> valueTuple3 in operationArray)
					{
						if (valueTuple3.Item2.Finished.CurrentResult != null)
						{
							this.operations.Remove(valueTuple3);
							flag2 |= this.OperationCompleted(valueTuple3.Item1, valueTuple3.Item2);
						}
					}
					if (flag2)
					{
						this.RunSchedulerIteration();
					}
					int num = -1;
					for (int k = 0; k < idleArray.Length; k++)
					{
						if (task == taskList[k + 1 + operationArray.Length])
						{
							num = k;
							break;
						}
					}
					if (num >= 0)
					{
						ValueTuple<ServicePointScheduler.ConnectionGroup, WebConnection, Task> valueTuple4 = idleArray[num];
						this.idleConnections.Remove(valueTuple4);
						this.CloseIdleConnection(valueTuple4.Item1, valueTuple4.Item2);
					}
				}
				operationArray = null;
				idleArray = null;
				taskList = null;
				schedulerTask = null;
			}
		}

		// Token: 0x06003717 RID: 14103 RVA: 0x000C0D70 File Offset: 0x000BEF70
		private void Cleanup()
		{
			if (this.groups != null)
			{
				string[] array = new string[this.groups.Count];
				this.groups.Keys.CopyTo(array, 0);
				foreach (string text in array)
				{
					if (this.groups.ContainsKey(text) && this.groups[text].IsEmpty())
					{
						this.groups.Remove(text);
					}
				}
				if (this.groups.Count == 0)
				{
					this.groups = null;
				}
			}
		}

		// Token: 0x06003718 RID: 14104 RVA: 0x000C0E00 File Offset: 0x000BF000
		private void RunSchedulerIteration()
		{
			this.schedulerEvent.Reset();
			bool flag;
			do
			{
				flag = this.SchedulerIteration(this.defaultGroup);
				if (this.groups != null)
				{
					foreach (KeyValuePair<string, ServicePointScheduler.ConnectionGroup> keyValuePair in this.groups)
					{
						flag |= this.SchedulerIteration(keyValuePair.Value);
					}
				}
			}
			while (flag);
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x000C0E80 File Offset: 0x000BF080
		private bool OperationCompleted(ServicePointScheduler.ConnectionGroup group, WebOperation operation)
		{
			WebCompletionSource<ValueTuple<bool, WebOperation>>.Result currentResult = operation.Finished.CurrentResult;
			bool flag;
			WebOperation webOperation;
			if (!currentResult.Success)
			{
				flag = false;
				webOperation = null;
			}
			else
			{
				ValueTuple<bool, WebOperation> argument = currentResult.Argument;
				flag = argument.Item1;
				webOperation = argument.Item2;
			}
			if (!flag || !operation.Connection.Continue(webOperation))
			{
				group.RemoveConnection(operation.Connection);
				if (webOperation == null)
				{
					return true;
				}
				flag = false;
			}
			if (webOperation == null)
			{
				if (flag)
				{
					Task task = Task.Delay(this.MaxIdleTime);
					this.idleConnections.AddLast(new ValueTuple<ServicePointScheduler.ConnectionGroup, WebConnection, Task>(group, operation.Connection, task));
				}
				return true;
			}
			this.operations.AddLast(new ValueTuple<ServicePointScheduler.ConnectionGroup, WebOperation>(group, webOperation));
			if (flag)
			{
				this.RemoveIdleConnection(operation.Connection);
				return false;
			}
			group.Cleanup();
			group.CreateOrReuseConnection(webOperation, true);
			return false;
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x000C0F43 File Offset: 0x000BF143
		private void CloseIdleConnection(ServicePointScheduler.ConnectionGroup group, WebConnection connection)
		{
			group.RemoveConnection(connection);
			this.RemoveIdleConnection(connection);
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x000C0F54 File Offset: 0x000BF154
		private bool SchedulerIteration(ServicePointScheduler.ConnectionGroup group)
		{
			group.Cleanup();
			WebOperation nextOperation = group.GetNextOperation();
			if (nextOperation == null)
			{
				return false;
			}
			WebConnection item = group.CreateOrReuseConnection(nextOperation, false).Item1;
			if (item == null)
			{
				return false;
			}
			this.operations.AddLast(new ValueTuple<ServicePointScheduler.ConnectionGroup, WebOperation>(group, nextOperation));
			this.RemoveIdleConnection(item);
			return true;
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x000C0FA4 File Offset: 0x000BF1A4
		private void RemoveOperation(WebOperation operation)
		{
			LinkedListNode<ValueTuple<ServicePointScheduler.ConnectionGroup, WebOperation>> linkedListNode = this.operations.First;
			while (linkedListNode != null)
			{
				LinkedListNode<ValueTuple<ServicePointScheduler.ConnectionGroup, WebOperation>> linkedListNode2 = linkedListNode;
				linkedListNode = linkedListNode.Next;
				if (linkedListNode2.Value.Item2 == operation)
				{
					this.operations.Remove(linkedListNode2);
				}
			}
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x000C0FE8 File Offset: 0x000BF1E8
		private void RemoveIdleConnection(WebConnection connection)
		{
			LinkedListNode<ValueTuple<ServicePointScheduler.ConnectionGroup, WebConnection, Task>> linkedListNode = this.idleConnections.First;
			while (linkedListNode != null)
			{
				LinkedListNode<ValueTuple<ServicePointScheduler.ConnectionGroup, WebConnection, Task>> linkedListNode2 = linkedListNode;
				linkedListNode = linkedListNode.Next;
				if (linkedListNode2.Value.Item2 == connection)
				{
					this.idleConnections.Remove(linkedListNode2);
				}
			}
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x000C1029 File Offset: 0x000BF229
		private void FinalCleanup()
		{
			this.groups = null;
			this.operations = null;
			this.idleConnections = null;
			this.defaultGroup = null;
			this.ServicePoint.FreeServicePoint();
			ServicePointManager.RemoveServicePoint(this.ServicePoint);
			this.ServicePoint = null;
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x000C1064 File Offset: 0x000BF264
		public void SendRequest(WebOperation operation, string groupName)
		{
			ServicePoint servicePoint = this.ServicePoint;
			lock (servicePoint)
			{
				this.GetConnectionGroup(groupName).EnqueueOperation(operation);
				this.Run();
			}
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x000C10B4 File Offset: 0x000BF2B4
		public bool CloseConnectionGroup(string groupName)
		{
			ServicePointScheduler.ConnectionGroup connectionGroup;
			if (string.IsNullOrEmpty(groupName))
			{
				connectionGroup = this.defaultGroup;
			}
			else if (this.groups == null || !this.groups.TryGetValue(groupName, out connectionGroup))
			{
				return false;
			}
			if (connectionGroup != this.defaultGroup)
			{
				this.groups.Remove(groupName);
				if (this.groups.Count == 0)
				{
					this.groups = null;
				}
			}
			connectionGroup.Close();
			this.Run();
			return true;
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x000C1124 File Offset: 0x000BF324
		private ServicePointScheduler.ConnectionGroup GetConnectionGroup(string name)
		{
			ServicePoint servicePoint = this.ServicePoint;
			ServicePointScheduler.ConnectionGroup connectionGroup;
			lock (servicePoint)
			{
				if (string.IsNullOrEmpty(name))
				{
					connectionGroup = this.defaultGroup;
				}
				else
				{
					if (this.groups == null)
					{
						this.groups = new Dictionary<string, ServicePointScheduler.ConnectionGroup>();
					}
					ServicePointScheduler.ConnectionGroup connectionGroup2;
					if (this.groups.TryGetValue(name, out connectionGroup2))
					{
						connectionGroup = connectionGroup2;
					}
					else
					{
						connectionGroup2 = new ServicePointScheduler.ConnectionGroup(this, name);
						this.groups.Add(name, connectionGroup2);
						connectionGroup = connectionGroup2;
					}
				}
			}
			return connectionGroup;
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x000C11B0 File Offset: 0x000BF3B0
		private void OnConnectionCreated(WebConnection connection)
		{
			Interlocked.Increment(ref this.currentConnections);
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x000C11BE File Offset: 0x000BF3BE
		private void OnConnectionClosed(WebConnection connection)
		{
			this.RemoveIdleConnection(connection);
			Interlocked.Decrement(ref this.currentConnections);
		}

		// Token: 0x06003724 RID: 14116 RVA: 0x000C11D4 File Offset: 0x000BF3D4
		public static async Task<bool> WaitAsync(Task workerTask, int millisecondTimeout)
		{
			CancellationTokenSource cts = new CancellationTokenSource();
			bool flag;
			try
			{
				Task timeoutTask = Task.Delay(millisecondTimeout, cts.Token);
				ConfiguredTaskAwaitable<Task>.ConfiguredTaskAwaiter configuredTaskAwaiter = Task.WhenAny(new Task[] { workerTask, timeoutTask }).ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<Task>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<Task>.ConfiguredTaskAwaiter);
				}
				flag = configuredTaskAwaiter.GetResult() != timeoutTask;
			}
			finally
			{
				cts.Cancel();
				cts.Dispose();
			}
			return flag;
		}

		// Token: 0x0400200F RID: 8207
		private int running;

		// Token: 0x04002010 RID: 8208
		private int maxIdleTime = 100000;

		// Token: 0x04002011 RID: 8209
		private ServicePointScheduler.AsyncManualResetEvent schedulerEvent;

		// Token: 0x04002012 RID: 8210
		private ServicePointScheduler.ConnectionGroup defaultGroup;

		// Token: 0x04002013 RID: 8211
		private Dictionary<string, ServicePointScheduler.ConnectionGroup> groups;

		// Token: 0x04002014 RID: 8212
		private LinkedList<ValueTuple<ServicePointScheduler.ConnectionGroup, WebOperation>> operations;

		// Token: 0x04002015 RID: 8213
		private LinkedList<ValueTuple<ServicePointScheduler.ConnectionGroup, WebConnection, Task>> idleConnections;

		// Token: 0x04002016 RID: 8214
		private int currentConnections;

		// Token: 0x04002017 RID: 8215
		private int connectionLimit;

		// Token: 0x04002018 RID: 8216
		private DateTime idleSince;

		// Token: 0x04002019 RID: 8217
		private static int nextId;

		// Token: 0x0400201A RID: 8218
		public readonly int ID = ++ServicePointScheduler.nextId;

		// Token: 0x020006B1 RID: 1713
		private class ConnectionGroup
		{
			// Token: 0x17000B82 RID: 2946
			// (get) Token: 0x06003726 RID: 14118 RVA: 0x000C1227 File Offset: 0x000BF427
			public ServicePointScheduler Scheduler { get; }

			// Token: 0x17000B83 RID: 2947
			// (get) Token: 0x06003727 RID: 14119 RVA: 0x000C122F File Offset: 0x000BF42F
			public string Name { get; }

			// Token: 0x17000B84 RID: 2948
			// (get) Token: 0x06003728 RID: 14120 RVA: 0x000C1237 File Offset: 0x000BF437
			public bool IsDefault
			{
				get
				{
					return string.IsNullOrEmpty(this.Name);
				}
			}

			// Token: 0x06003729 RID: 14121 RVA: 0x000C1244 File Offset: 0x000BF444
			public ConnectionGroup(ServicePointScheduler scheduler, string name)
			{
				this.Scheduler = scheduler;
				this.Name = name;
				this.connections = new LinkedList<WebConnection>();
				this.queue = new LinkedList<WebOperation>();
			}

			// Token: 0x0600372A RID: 14122 RVA: 0x000C1283 File Offset: 0x000BF483
			public bool IsEmpty()
			{
				return this.connections.Count == 0 && this.queue.Count == 0;
			}

			// Token: 0x0600372B RID: 14123 RVA: 0x000C12A2 File Offset: 0x000BF4A2
			public void RemoveConnection(WebConnection connection)
			{
				this.connections.Remove(connection);
				connection.Dispose();
				this.Scheduler.OnConnectionClosed(connection);
			}

			// Token: 0x0600372C RID: 14124 RVA: 0x000C12C4 File Offset: 0x000BF4C4
			public void Cleanup()
			{
				LinkedListNode<WebConnection> linkedListNode = this.connections.First;
				while (linkedListNode != null)
				{
					WebConnection value = linkedListNode.Value;
					LinkedListNode<WebConnection> linkedListNode2 = linkedListNode;
					linkedListNode = linkedListNode.Next;
					if (value.Closed)
					{
						this.connections.Remove(linkedListNode2);
						this.Scheduler.OnConnectionClosed(value);
					}
				}
			}

			// Token: 0x0600372D RID: 14125 RVA: 0x000C1314 File Offset: 0x000BF514
			public void Close()
			{
				foreach (WebOperation webOperation in this.queue)
				{
					webOperation.Abort();
					this.Scheduler.RemoveOperation(webOperation);
				}
				this.queue.Clear();
				foreach (WebConnection webConnection in this.connections)
				{
					webConnection.Dispose();
					this.Scheduler.OnConnectionClosed(webConnection);
				}
				this.connections.Clear();
			}

			// Token: 0x0600372E RID: 14126 RVA: 0x000C13D8 File Offset: 0x000BF5D8
			public void EnqueueOperation(WebOperation operation)
			{
				this.queue.AddLast(operation);
			}

			// Token: 0x0600372F RID: 14127 RVA: 0x000C13E8 File Offset: 0x000BF5E8
			public WebOperation GetNextOperation()
			{
				LinkedListNode<WebOperation> linkedListNode = this.queue.First;
				while (linkedListNode != null)
				{
					WebOperation value = linkedListNode.Value;
					LinkedListNode<WebOperation> linkedListNode2 = linkedListNode;
					linkedListNode = linkedListNode.Next;
					if (!value.Aborted)
					{
						return value;
					}
					this.queue.Remove(linkedListNode2);
					this.Scheduler.RemoveOperation(value);
				}
				return null;
			}

			// Token: 0x06003730 RID: 14128 RVA: 0x000C143C File Offset: 0x000BF63C
			public WebConnection FindIdleConnection(WebOperation operation)
			{
				WebConnection webConnection = null;
				foreach (WebConnection webConnection2 in this.connections)
				{
					if (webConnection2.CanReuseConnection(operation) && (webConnection == null || webConnection2.IdleSince > webConnection.IdleSince))
					{
						webConnection = webConnection2;
					}
				}
				if (webConnection != null && webConnection.StartOperation(operation, true))
				{
					this.queue.Remove(operation);
					return webConnection;
				}
				foreach (WebConnection webConnection3 in this.connections)
				{
					if (webConnection3.StartOperation(operation, true))
					{
						this.queue.Remove(operation);
						return webConnection3;
					}
				}
				return null;
			}

			// Token: 0x06003731 RID: 14129 RVA: 0x000C1524 File Offset: 0x000BF724
			[return: TupleElementNames(new string[] { "connection", "created" })]
			public ValueTuple<WebConnection, bool> CreateOrReuseConnection(WebOperation operation, bool force)
			{
				WebConnection webConnection = this.FindIdleConnection(operation);
				if (webConnection != null)
				{
					return new ValueTuple<WebConnection, bool>(webConnection, false);
				}
				if (force || this.Scheduler.ServicePoint.ConnectionLimit > this.connections.Count || this.connections.Count == 0)
				{
					webConnection = new WebConnection(this.Scheduler.ServicePoint);
					webConnection.StartOperation(operation, false);
					this.connections.AddFirst(webConnection);
					this.Scheduler.OnConnectionCreated(webConnection);
					this.queue.Remove(operation);
					return new ValueTuple<WebConnection, bool>(webConnection, true);
				}
				return new ValueTuple<WebConnection, bool>(null, false);
			}

			// Token: 0x0400201E RID: 8222
			private static int nextId;

			// Token: 0x0400201F RID: 8223
			public readonly int ID = ++ServicePointScheduler.ConnectionGroup.nextId;

			// Token: 0x04002020 RID: 8224
			private LinkedList<WebConnection> connections;

			// Token: 0x04002021 RID: 8225
			private LinkedList<WebOperation> queue;
		}

		// Token: 0x020006B2 RID: 1714
		private class AsyncManualResetEvent
		{
			// Token: 0x06003732 RID: 14130 RVA: 0x000C15C0 File Offset: 0x000BF7C0
			public Task WaitAsync()
			{
				return this.m_tcs.Task;
			}

			// Token: 0x06003733 RID: 14131 RVA: 0x000C15CF File Offset: 0x000BF7CF
			public bool WaitOne(int millisecondTimeout)
			{
				return this.m_tcs.Task.Wait(millisecondTimeout);
			}

			// Token: 0x06003734 RID: 14132 RVA: 0x000C15E4 File Offset: 0x000BF7E4
			public Task<bool> WaitAsync(int millisecondTimeout)
			{
				return ServicePointScheduler.WaitAsync(this.m_tcs.Task, millisecondTimeout);
			}

			// Token: 0x06003735 RID: 14133 RVA: 0x000C15FC File Offset: 0x000BF7FC
			public void Set()
			{
				TaskCompletionSource<bool> tcs = this.m_tcs;
				Task.Factory.StartNew<bool>((object s) => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs, CancellationToken.None, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
				tcs.Task.Wait();
			}

			// Token: 0x06003736 RID: 14134 RVA: 0x000C1654 File Offset: 0x000BF854
			public void Reset()
			{
				TaskCompletionSource<bool> tcs;
				do
				{
					tcs = this.m_tcs;
				}
				while (tcs.Task.IsCompleted && Interlocked.CompareExchange<TaskCompletionSource<bool>>(ref this.m_tcs, new TaskCompletionSource<bool>(), tcs) != tcs);
			}

			// Token: 0x06003737 RID: 14135 RVA: 0x000C168B File Offset: 0x000BF88B
			public AsyncManualResetEvent(bool state)
			{
				if (state)
				{
					this.Set();
				}
			}

			// Token: 0x04002022 RID: 8226
			private volatile TaskCompletionSource<bool> m_tcs = new TaskCompletionSource<bool>();
		}
	}
}
