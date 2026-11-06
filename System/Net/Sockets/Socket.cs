using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Configuration;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Mono;

namespace System.Net.Sockets
{
	/// <summary>Implements the Berkeley sockets interface.</summary>
	// Token: 0x02000793 RID: 1939
	public class Socket : IDisposable
	{
		// Token: 0x06003D1D RID: 15645 RVA: 0x000D0288 File Offset: 0x000CE488
		internal Task<Socket> AcceptAsync(Socket acceptSocket)
		{
			Socket.TaskSocketAsyncEventArgs<Socket> taskSocketAsyncEventArgs = Interlocked.Exchange<Socket.TaskSocketAsyncEventArgs<Socket>>(ref LazyInitializer.EnsureInitialized<Socket.CachedEventArgs>(ref this._cachedTaskEventArgs, () => new Socket.CachedEventArgs()).TaskAccept, Socket.s_rentedSocketSentinel);
			if (taskSocketAsyncEventArgs == Socket.s_rentedSocketSentinel)
			{
				return this.AcceptAsyncApm(acceptSocket);
			}
			if (taskSocketAsyncEventArgs == null)
			{
				taskSocketAsyncEventArgs = new Socket.TaskSocketAsyncEventArgs<Socket>();
				taskSocketAsyncEventArgs.Completed += Socket.AcceptCompletedHandler;
			}
			taskSocketAsyncEventArgs.AcceptSocket = acceptSocket;
			Task<Socket> task;
			if (this.AcceptAsync(taskSocketAsyncEventArgs))
			{
				bool flag;
				task = taskSocketAsyncEventArgs.GetCompletionResponsibility(out flag).Task;
				if (flag)
				{
					this.ReturnSocketAsyncEventArgs(taskSocketAsyncEventArgs);
				}
			}
			else
			{
				task = ((taskSocketAsyncEventArgs.SocketError == SocketError.Success) ? Task.FromResult<Socket>(taskSocketAsyncEventArgs.AcceptSocket) : Task.FromException<Socket>(Socket.GetException(taskSocketAsyncEventArgs.SocketError, false)));
				this.ReturnSocketAsyncEventArgs(taskSocketAsyncEventArgs);
			}
			return task;
		}

		// Token: 0x06003D1E RID: 15646 RVA: 0x000D0350 File Offset: 0x000CE550
		private Task<Socket> AcceptAsyncApm(Socket acceptSocket)
		{
			TaskCompletionSource<Socket> taskCompletionSource = new TaskCompletionSource<Socket>(this);
			this.BeginAccept(acceptSocket, 0, delegate(IAsyncResult iar)
			{
				TaskCompletionSource<Socket> taskCompletionSource2 = (TaskCompletionSource<Socket>)iar.AsyncState;
				try
				{
					taskCompletionSource2.TrySetResult(((Socket)taskCompletionSource2.Task.AsyncState).EndAccept(iar));
				}
				catch (Exception ex)
				{
					taskCompletionSource2.TrySetException(ex);
				}
			}, taskCompletionSource);
			return taskCompletionSource.Task;
		}

		// Token: 0x06003D1F RID: 15647 RVA: 0x000D0394 File Offset: 0x000CE594
		internal Task ConnectAsync(EndPoint remoteEP)
		{
			TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>(this);
			this.BeginConnect(remoteEP, delegate(IAsyncResult iar)
			{
				TaskCompletionSource<bool> taskCompletionSource2 = (TaskCompletionSource<bool>)iar.AsyncState;
				try
				{
					((Socket)taskCompletionSource2.Task.AsyncState).EndConnect(iar);
					taskCompletionSource2.TrySetResult(true);
				}
				catch (Exception ex)
				{
					taskCompletionSource2.TrySetException(ex);
				}
			}, taskCompletionSource);
			return taskCompletionSource.Task;
		}

		// Token: 0x06003D20 RID: 15648 RVA: 0x000D03D8 File Offset: 0x000CE5D8
		internal Task ConnectAsync(IPAddress address, int port)
		{
			TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>(this);
			this.BeginConnect(address, port, delegate(IAsyncResult iar)
			{
				TaskCompletionSource<bool> taskCompletionSource2 = (TaskCompletionSource<bool>)iar.AsyncState;
				try
				{
					((Socket)taskCompletionSource2.Task.AsyncState).EndConnect(iar);
					taskCompletionSource2.TrySetResult(true);
				}
				catch (Exception ex)
				{
					taskCompletionSource2.TrySetException(ex);
				}
			}, taskCompletionSource);
			return taskCompletionSource.Task;
		}

		// Token: 0x06003D21 RID: 15649 RVA: 0x000D041C File Offset: 0x000CE61C
		internal Task ConnectAsync(IPAddress[] addresses, int port)
		{
			TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>(this);
			this.BeginConnect(addresses, port, delegate(IAsyncResult iar)
			{
				TaskCompletionSource<bool> taskCompletionSource2 = (TaskCompletionSource<bool>)iar.AsyncState;
				try
				{
					((Socket)taskCompletionSource2.Task.AsyncState).EndConnect(iar);
					taskCompletionSource2.TrySetResult(true);
				}
				catch (Exception ex)
				{
					taskCompletionSource2.TrySetException(ex);
				}
			}, taskCompletionSource);
			return taskCompletionSource.Task;
		}

		// Token: 0x06003D22 RID: 15650 RVA: 0x000D0460 File Offset: 0x000CE660
		internal Task ConnectAsync(string host, int port)
		{
			TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>(this);
			this.BeginConnect(host, port, delegate(IAsyncResult iar)
			{
				TaskCompletionSource<bool> taskCompletionSource2 = (TaskCompletionSource<bool>)iar.AsyncState;
				try
				{
					((Socket)taskCompletionSource2.Task.AsyncState).EndConnect(iar);
					taskCompletionSource2.TrySetResult(true);
				}
				catch (Exception ex)
				{
					taskCompletionSource2.TrySetException(ex);
				}
			}, taskCompletionSource);
			return taskCompletionSource.Task;
		}

		// Token: 0x06003D23 RID: 15651 RVA: 0x000D04A4 File Offset: 0x000CE6A4
		internal Task<int> ReceiveAsync(ArraySegment<byte> buffer, SocketFlags socketFlags, bool fromNetworkStream)
		{
			Socket.ValidateBuffer(buffer);
			return this.ReceiveAsync(buffer, socketFlags, fromNetworkStream, default(CancellationToken)).AsTask();
		}

		// Token: 0x06003D24 RID: 15652 RVA: 0x000D04D8 File Offset: 0x000CE6D8
		internal ValueTask<int> ReceiveAsync(Memory<byte> buffer, SocketFlags socketFlags, bool fromNetworkStream, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return new ValueTask<int>(Task.FromCanceled<int>(cancellationToken));
			}
			Socket.AwaitableSocketAsyncEventArgs e = LazyInitializer.EnsureInitialized<Socket.AwaitableSocketAsyncEventArgs>(ref LazyInitializer.EnsureInitialized<Socket.CachedEventArgs>(ref this._cachedTaskEventArgs, () => new Socket.CachedEventArgs()).ValueTaskReceive, () => new Socket.AwaitableSocketAsyncEventArgs());
			if (e.Reserve())
			{
				e.SetBuffer(buffer);
				e.SocketFlags = socketFlags;
				e.WrapExceptionsInIOExceptions = fromNetworkStream;
				return e.ReceiveAsync(this);
			}
			return new ValueTask<int>(this.ReceiveAsyncApm(buffer, socketFlags));
		}

		// Token: 0x06003D25 RID: 15653 RVA: 0x000D0584 File Offset: 0x000CE784
		private Task<int> ReceiveAsyncApm(Memory<byte> buffer, SocketFlags socketFlags)
		{
			ArraySegment<byte> arraySegment;
			if (MemoryMarshal.TryGetArray<byte>(buffer, out arraySegment))
			{
				TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>(this);
				this.BeginReceive(arraySegment.Array, arraySegment.Offset, arraySegment.Count, socketFlags, delegate(IAsyncResult iar)
				{
					TaskCompletionSource<int> taskCompletionSource3 = (TaskCompletionSource<int>)iar.AsyncState;
					try
					{
						taskCompletionSource3.TrySetResult(((Socket)taskCompletionSource3.Task.AsyncState).EndReceive(iar));
					}
					catch (Exception ex)
					{
						taskCompletionSource3.TrySetException(ex);
					}
				}, taskCompletionSource);
				return taskCompletionSource.Task;
			}
			byte[] array = ArrayPool<byte>.Shared.Rent(buffer.Length);
			TaskCompletionSource<int> taskCompletionSource2 = new TaskCompletionSource<int>(this);
			this.BeginReceive(array, 0, buffer.Length, socketFlags, delegate(IAsyncResult iar)
			{
				Tuple<TaskCompletionSource<int>, Memory<byte>, byte[]> tuple = (Tuple<TaskCompletionSource<int>, Memory<byte>, byte[]>)iar.AsyncState;
				try
				{
					int num = ((Socket)tuple.Item1.Task.AsyncState).EndReceive(iar);
					new ReadOnlyMemory<byte>(tuple.Item3, 0, num).Span.CopyTo(tuple.Item2.Span);
					tuple.Item1.TrySetResult(num);
				}
				catch (Exception ex2)
				{
					tuple.Item1.TrySetException(ex2);
				}
				finally
				{
					ArrayPool<byte>.Shared.Return(tuple.Item3, false);
				}
			}, Tuple.Create<TaskCompletionSource<int>, Memory<byte>, byte[]>(taskCompletionSource2, buffer, array));
			return taskCompletionSource2.Task;
		}

		// Token: 0x06003D26 RID: 15654 RVA: 0x000D0644 File Offset: 0x000CE844
		internal Task<int> ReceiveAsync(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
		{
			Socket.ValidateBuffersList(buffers);
			Socket.Int32TaskSocketAsyncEventArgs e = this.RentSocketAsyncEventArgs(true);
			if (e != null)
			{
				Socket.ConfigureBufferList(e, buffers, socketFlags);
				return this.GetTaskForSendReceive(this.ReceiveAsync(e), e, false, true);
			}
			return this.ReceiveAsyncApm(buffers, socketFlags);
		}

		// Token: 0x06003D27 RID: 15655 RVA: 0x000D0684 File Offset: 0x000CE884
		private Task<int> ReceiveAsyncApm(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
		{
			TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>(this);
			this.BeginReceive(buffers, socketFlags, delegate(IAsyncResult iar)
			{
				TaskCompletionSource<int> taskCompletionSource2 = (TaskCompletionSource<int>)iar.AsyncState;
				try
				{
					taskCompletionSource2.TrySetResult(((Socket)taskCompletionSource2.Task.AsyncState).EndReceive(iar));
				}
				catch (Exception ex)
				{
					taskCompletionSource2.TrySetException(ex);
				}
			}, taskCompletionSource);
			return taskCompletionSource.Task;
		}

		// Token: 0x06003D28 RID: 15656 RVA: 0x000D06C8 File Offset: 0x000CE8C8
		internal Task<SocketReceiveFromResult> ReceiveFromAsync(ArraySegment<byte> buffer, SocketFlags socketFlags, EndPoint remoteEndPoint)
		{
			Socket.StateTaskCompletionSource<EndPoint, SocketReceiveFromResult> stateTaskCompletionSource = new Socket.StateTaskCompletionSource<EndPoint, SocketReceiveFromResult>(this)
			{
				_field1 = remoteEndPoint
			};
			this.BeginReceiveFrom(buffer.Array, buffer.Offset, buffer.Count, socketFlags, ref stateTaskCompletionSource._field1, delegate(IAsyncResult iar)
			{
				Socket.StateTaskCompletionSource<EndPoint, SocketReceiveFromResult> stateTaskCompletionSource2 = (Socket.StateTaskCompletionSource<EndPoint, SocketReceiveFromResult>)iar.AsyncState;
				try
				{
					int num = ((Socket)stateTaskCompletionSource2.Task.AsyncState).EndReceiveFrom(iar, ref stateTaskCompletionSource2._field1);
					stateTaskCompletionSource2.TrySetResult(new SocketReceiveFromResult
					{
						ReceivedBytes = num,
						RemoteEndPoint = stateTaskCompletionSource2._field1
					});
				}
				catch (Exception ex)
				{
					stateTaskCompletionSource2.TrySetException(ex);
				}
			}, stateTaskCompletionSource);
			return stateTaskCompletionSource.Task;
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x000D072C File Offset: 0x000CE92C
		internal Task<SocketReceiveMessageFromResult> ReceiveMessageFromAsync(ArraySegment<byte> buffer, SocketFlags socketFlags, EndPoint remoteEndPoint)
		{
			Socket.StateTaskCompletionSource<SocketFlags, EndPoint, SocketReceiveMessageFromResult> stateTaskCompletionSource = new Socket.StateTaskCompletionSource<SocketFlags, EndPoint, SocketReceiveMessageFromResult>(this)
			{
				_field1 = socketFlags,
				_field2 = remoteEndPoint
			};
			this.BeginReceiveMessageFrom(buffer.Array, buffer.Offset, buffer.Count, socketFlags, ref stateTaskCompletionSource._field2, delegate(IAsyncResult iar)
			{
				Socket.StateTaskCompletionSource<SocketFlags, EndPoint, SocketReceiveMessageFromResult> stateTaskCompletionSource2 = (Socket.StateTaskCompletionSource<SocketFlags, EndPoint, SocketReceiveMessageFromResult>)iar.AsyncState;
				try
				{
					IPPacketInformation ippacketInformation;
					int num = ((Socket)stateTaskCompletionSource2.Task.AsyncState).EndReceiveMessageFrom(iar, ref stateTaskCompletionSource2._field1, ref stateTaskCompletionSource2._field2, out ippacketInformation);
					stateTaskCompletionSource2.TrySetResult(new SocketReceiveMessageFromResult
					{
						ReceivedBytes = num,
						RemoteEndPoint = stateTaskCompletionSource2._field2,
						SocketFlags = stateTaskCompletionSource2._field1,
						PacketInformation = ippacketInformation
					});
				}
				catch (Exception ex)
				{
					stateTaskCompletionSource2.TrySetException(ex);
				}
			}, stateTaskCompletionSource);
			return stateTaskCompletionSource.Task;
		}

		// Token: 0x06003D2A RID: 15658 RVA: 0x000D0798 File Offset: 0x000CE998
		internal Task<int> SendAsync(ArraySegment<byte> buffer, SocketFlags socketFlags)
		{
			Socket.ValidateBuffer(buffer);
			return this.SendAsync(buffer, socketFlags, default(CancellationToken)).AsTask();
		}

		// Token: 0x06003D2B RID: 15659 RVA: 0x000D07CC File Offset: 0x000CE9CC
		internal ValueTask<int> SendAsync(ReadOnlyMemory<byte> buffer, SocketFlags socketFlags, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return new ValueTask<int>(Task.FromCanceled<int>(cancellationToken));
			}
			Socket.AwaitableSocketAsyncEventArgs e = LazyInitializer.EnsureInitialized<Socket.AwaitableSocketAsyncEventArgs>(ref LazyInitializer.EnsureInitialized<Socket.CachedEventArgs>(ref this._cachedTaskEventArgs, () => new Socket.CachedEventArgs()).ValueTaskSend, () => new Socket.AwaitableSocketAsyncEventArgs());
			if (e.Reserve())
			{
				e.SetBuffer(MemoryMarshal.AsMemory<byte>(buffer));
				e.SocketFlags = socketFlags;
				e.WrapExceptionsInIOExceptions = false;
				return e.SendAsync(this);
			}
			return new ValueTask<int>(this.SendAsyncApm(buffer, socketFlags));
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x000D087C File Offset: 0x000CEA7C
		internal ValueTask SendAsyncForNetworkStream(ReadOnlyMemory<byte> buffer, SocketFlags socketFlags, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return new ValueTask(Task.FromCanceled(cancellationToken));
			}
			Socket.AwaitableSocketAsyncEventArgs e = LazyInitializer.EnsureInitialized<Socket.AwaitableSocketAsyncEventArgs>(ref LazyInitializer.EnsureInitialized<Socket.CachedEventArgs>(ref this._cachedTaskEventArgs, () => new Socket.CachedEventArgs()).ValueTaskSend, () => new Socket.AwaitableSocketAsyncEventArgs());
			if (e.Reserve())
			{
				e.SetBuffer(MemoryMarshal.AsMemory<byte>(buffer));
				e.SocketFlags = socketFlags;
				e.WrapExceptionsInIOExceptions = true;
				return e.SendAsyncForNetworkStream(this);
			}
			return new ValueTask(this.SendAsyncApm(buffer, socketFlags));
		}

		// Token: 0x06003D2D RID: 15661 RVA: 0x000D092C File Offset: 0x000CEB2C
		private Task<int> SendAsyncApm(ReadOnlyMemory<byte> buffer, SocketFlags socketFlags)
		{
			ArraySegment<byte> arraySegment;
			if (MemoryMarshal.TryGetArray<byte>(buffer, out arraySegment))
			{
				TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>(this);
				this.BeginSend(arraySegment.Array, arraySegment.Offset, arraySegment.Count, socketFlags, delegate(IAsyncResult iar)
				{
					TaskCompletionSource<int> taskCompletionSource3 = (TaskCompletionSource<int>)iar.AsyncState;
					try
					{
						taskCompletionSource3.TrySetResult(((Socket)taskCompletionSource3.Task.AsyncState).EndSend(iar));
					}
					catch (Exception ex)
					{
						taskCompletionSource3.TrySetException(ex);
					}
				}, taskCompletionSource);
				return taskCompletionSource.Task;
			}
			byte[] array = ArrayPool<byte>.Shared.Rent(buffer.Length);
			buffer.Span.CopyTo(array);
			TaskCompletionSource<int> taskCompletionSource2 = new TaskCompletionSource<int>(this);
			this.BeginSend(array, 0, buffer.Length, socketFlags, delegate(IAsyncResult iar)
			{
				Tuple<TaskCompletionSource<int>, byte[]> tuple = (Tuple<TaskCompletionSource<int>, byte[]>)iar.AsyncState;
				try
				{
					tuple.Item1.TrySetResult(((Socket)tuple.Item1.Task.AsyncState).EndSend(iar));
				}
				catch (Exception ex2)
				{
					tuple.Item1.TrySetException(ex2);
				}
				finally
				{
					ArrayPool<byte>.Shared.Return(tuple.Item2, false);
				}
			}, Tuple.Create<TaskCompletionSource<int>, byte[]>(taskCompletionSource2, array));
			return taskCompletionSource2.Task;
		}

		// Token: 0x06003D2E RID: 15662 RVA: 0x000D09FC File Offset: 0x000CEBFC
		internal Task<int> SendAsync(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
		{
			Socket.ValidateBuffersList(buffers);
			Socket.Int32TaskSocketAsyncEventArgs e = this.RentSocketAsyncEventArgs(false);
			if (e != null)
			{
				Socket.ConfigureBufferList(e, buffers, socketFlags);
				return this.GetTaskForSendReceive(this.SendAsync(e), e, false, false);
			}
			return this.SendAsyncApm(buffers, socketFlags);
		}

		// Token: 0x06003D2F RID: 15663 RVA: 0x000D0A3C File Offset: 0x000CEC3C
		private Task<int> SendAsyncApm(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
		{
			TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>(this);
			this.BeginSend(buffers, socketFlags, delegate(IAsyncResult iar)
			{
				TaskCompletionSource<int> taskCompletionSource2 = (TaskCompletionSource<int>)iar.AsyncState;
				try
				{
					taskCompletionSource2.TrySetResult(((Socket)taskCompletionSource2.Task.AsyncState).EndSend(iar));
				}
				catch (Exception ex)
				{
					taskCompletionSource2.TrySetException(ex);
				}
			}, taskCompletionSource);
			return taskCompletionSource.Task;
		}

		// Token: 0x06003D30 RID: 15664 RVA: 0x000D0A80 File Offset: 0x000CEC80
		internal Task<int> SendToAsync(ArraySegment<byte> buffer, SocketFlags socketFlags, EndPoint remoteEP)
		{
			TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>(this);
			this.BeginSendTo(buffer.Array, buffer.Offset, buffer.Count, socketFlags, remoteEP, delegate(IAsyncResult iar)
			{
				TaskCompletionSource<int> taskCompletionSource2 = (TaskCompletionSource<int>)iar.AsyncState;
				try
				{
					taskCompletionSource2.TrySetResult(((Socket)taskCompletionSource2.Task.AsyncState).EndSendTo(iar));
				}
				catch (Exception ex)
				{
					taskCompletionSource2.TrySetException(ex);
				}
			}, taskCompletionSource);
			return taskCompletionSource.Task;
		}

		// Token: 0x06003D31 RID: 15665 RVA: 0x000D0AD8 File Offset: 0x000CECD8
		private static void ValidateBuffer(ArraySegment<byte> buffer)
		{
			if (buffer.Array == null)
			{
				throw new ArgumentNullException("Array");
			}
			if (buffer.Offset < 0 || buffer.Offset > buffer.Array.Length)
			{
				throw new ArgumentOutOfRangeException("Offset");
			}
			if (buffer.Count < 0 || buffer.Count > buffer.Array.Length - buffer.Offset)
			{
				throw new ArgumentOutOfRangeException("Count");
			}
		}

		// Token: 0x06003D32 RID: 15666 RVA: 0x000D0B4F File Offset: 0x000CED4F
		private static void ValidateBuffersList(IList<ArraySegment<byte>> buffers)
		{
			if (buffers == null)
			{
				throw new ArgumentNullException("buffers");
			}
			if (buffers.Count == 0)
			{
				throw new ArgumentException(SR.Format("The parameter {0} must contain one or more elements.", "buffers"), "buffers");
			}
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x000D0B84 File Offset: 0x000CED84
		private static void ConfigureBufferList(Socket.Int32TaskSocketAsyncEventArgs saea, IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
		{
			if (!saea.MemoryBuffer.Equals(default(Memory<byte>)))
			{
				saea.SetBuffer(default(Memory<byte>));
			}
			saea.BufferList = buffers;
			saea.SocketFlags = socketFlags;
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x000D0BC8 File Offset: 0x000CEDC8
		private Task<int> GetTaskForSendReceive(bool pending, Socket.Int32TaskSocketAsyncEventArgs saea, bool fromNetworkStream, bool isReceive)
		{
			Task<int> task;
			if (pending)
			{
				bool flag;
				task = saea.GetCompletionResponsibility(out flag).Task;
				if (flag)
				{
					this.ReturnSocketAsyncEventArgs(saea, isReceive);
				}
			}
			else
			{
				if (saea.SocketError == SocketError.Success)
				{
					int bytesTransferred = saea.BytesTransferred;
					if (bytesTransferred == 0 || (fromNetworkStream & !isReceive))
					{
						task = Socket.s_zeroTask;
					}
					else
					{
						task = Task.FromResult<int>(bytesTransferred);
					}
				}
				else
				{
					task = Task.FromException<int>(Socket.GetException(saea.SocketError, fromNetworkStream));
				}
				this.ReturnSocketAsyncEventArgs(saea, isReceive);
			}
			return task;
		}

		// Token: 0x06003D35 RID: 15669 RVA: 0x000D0C40 File Offset: 0x000CEE40
		private static void CompleteAccept(Socket s, Socket.TaskSocketAsyncEventArgs<Socket> saea)
		{
			SocketError socketError = saea.SocketError;
			Socket acceptSocket = saea.AcceptSocket;
			bool flag;
			AsyncTaskMethodBuilder<Socket> completionResponsibility = saea.GetCompletionResponsibility(out flag);
			if (flag)
			{
				s.ReturnSocketAsyncEventArgs(saea);
			}
			if (socketError == SocketError.Success)
			{
				completionResponsibility.SetResult(acceptSocket);
				return;
			}
			completionResponsibility.SetException(Socket.GetException(socketError, false));
		}

		// Token: 0x06003D36 RID: 15670 RVA: 0x000D0C88 File Offset: 0x000CEE88
		private static void CompleteSendReceive(Socket s, Socket.Int32TaskSocketAsyncEventArgs saea, bool isReceive)
		{
			SocketError socketError = saea.SocketError;
			int bytesTransferred = saea.BytesTransferred;
			bool wrapExceptionsInIOExceptions = saea._wrapExceptionsInIOExceptions;
			bool flag;
			AsyncTaskMethodBuilder<int> completionResponsibility = saea.GetCompletionResponsibility(out flag);
			if (flag)
			{
				s.ReturnSocketAsyncEventArgs(saea, isReceive);
			}
			if (socketError == SocketError.Success)
			{
				completionResponsibility.SetResult(bytesTransferred);
				return;
			}
			completionResponsibility.SetException(Socket.GetException(socketError, wrapExceptionsInIOExceptions));
		}

		// Token: 0x06003D37 RID: 15671 RVA: 0x000D0CDC File Offset: 0x000CEEDC
		private static Exception GetException(SocketError error, bool wrapExceptionsInIOExceptions = false)
		{
			Exception ex = new SocketException((int)error);
			if (!wrapExceptionsInIOExceptions)
			{
				return ex;
			}
			return new IOException(SR.Format("Unable to transfer data on the transport connection: {0}.", ex.Message), ex);
		}

		// Token: 0x06003D38 RID: 15672 RVA: 0x000D0D0C File Offset: 0x000CEF0C
		private Socket.Int32TaskSocketAsyncEventArgs RentSocketAsyncEventArgs(bool isReceive)
		{
			Socket.CachedEventArgs e = LazyInitializer.EnsureInitialized<Socket.CachedEventArgs>(ref this._cachedTaskEventArgs, () => new Socket.CachedEventArgs());
			Socket.Int32TaskSocketAsyncEventArgs e2 = (isReceive ? Interlocked.Exchange<Socket.Int32TaskSocketAsyncEventArgs>(ref e.TaskReceive, Socket.s_rentedInt32Sentinel) : Interlocked.Exchange<Socket.Int32TaskSocketAsyncEventArgs>(ref e.TaskSend, Socket.s_rentedInt32Sentinel));
			if (e2 == Socket.s_rentedInt32Sentinel)
			{
				return null;
			}
			if (e2 == null)
			{
				e2 = new Socket.Int32TaskSocketAsyncEventArgs();
				e2.Completed += (isReceive ? Socket.ReceiveCompletedHandler : Socket.SendCompletedHandler);
			}
			return e2;
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x000D0D94 File Offset: 0x000CEF94
		private void ReturnSocketAsyncEventArgs(Socket.Int32TaskSocketAsyncEventArgs saea, bool isReceive)
		{
			saea._accessed = false;
			saea._builder = default(AsyncTaskMethodBuilder<int>);
			saea._wrapExceptionsInIOExceptions = false;
			if (isReceive)
			{
				Volatile.Write<Socket.Int32TaskSocketAsyncEventArgs>(ref this._cachedTaskEventArgs.TaskReceive, saea);
				return;
			}
			Volatile.Write<Socket.Int32TaskSocketAsyncEventArgs>(ref this._cachedTaskEventArgs.TaskSend, saea);
		}

		// Token: 0x06003D3A RID: 15674 RVA: 0x000D0DE1 File Offset: 0x000CEFE1
		private void ReturnSocketAsyncEventArgs(Socket.TaskSocketAsyncEventArgs<Socket> saea)
		{
			saea.AcceptSocket = null;
			saea._accessed = false;
			saea._builder = default(AsyncTaskMethodBuilder<Socket>);
			Volatile.Write<Socket.TaskSocketAsyncEventArgs<Socket>>(ref this._cachedTaskEventArgs.TaskAccept, saea);
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x000D0E10 File Offset: 0x000CF010
		private void DisposeCachedTaskSocketAsyncEventArgs()
		{
			Socket.CachedEventArgs cachedTaskEventArgs = this._cachedTaskEventArgs;
			if (cachedTaskEventArgs != null)
			{
				Socket.TaskSocketAsyncEventArgs<Socket> taskSocketAsyncEventArgs = Interlocked.Exchange<Socket.TaskSocketAsyncEventArgs<Socket>>(ref cachedTaskEventArgs.TaskAccept, Socket.s_rentedSocketSentinel);
				if (taskSocketAsyncEventArgs != null)
				{
					taskSocketAsyncEventArgs.Dispose();
				}
				Socket.Int32TaskSocketAsyncEventArgs e = Interlocked.Exchange<Socket.Int32TaskSocketAsyncEventArgs>(ref cachedTaskEventArgs.TaskReceive, Socket.s_rentedInt32Sentinel);
				if (e != null)
				{
					e.Dispose();
				}
				Socket.Int32TaskSocketAsyncEventArgs e2 = Interlocked.Exchange<Socket.Int32TaskSocketAsyncEventArgs>(ref cachedTaskEventArgs.TaskSend, Socket.s_rentedInt32Sentinel);
				if (e2 != null)
				{
					e2.Dispose();
				}
				Socket.AwaitableSocketAsyncEventArgs e3 = Interlocked.Exchange<Socket.AwaitableSocketAsyncEventArgs>(ref cachedTaskEventArgs.ValueTaskReceive, Socket.AwaitableSocketAsyncEventArgs.Reserved);
				if (e3 != null)
				{
					e3.Dispose();
				}
				Socket.AwaitableSocketAsyncEventArgs e4 = Interlocked.Exchange<Socket.AwaitableSocketAsyncEventArgs>(ref cachedTaskEventArgs.ValueTaskSend, Socket.AwaitableSocketAsyncEventArgs.Reserved);
				if (e4 == null)
				{
					return;
				}
				e4.Dispose();
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Sockets.Socket" /> class using the specified socket type and protocol.</summary>
		/// <param name="socketType">One of the <see cref="T:System.Net.Sockets.SocketType" /> values.</param>
		/// <param name="protocolType">One of the <see cref="T:System.Net.Sockets.ProtocolType" /> values.</param>
		/// <exception cref="T:System.Net.Sockets.SocketException">The combination of  <paramref name="socketType" /> and <paramref name="protocolType" /> results in an invalid socket.</exception>
		// Token: 0x06003D3C RID: 15676 RVA: 0x000D0EB0 File Offset: 0x000CF0B0
		public Socket(SocketType socketType, ProtocolType protocolType)
			: this(AddressFamily.InterNetworkV6, socketType, protocolType)
		{
			this.DualMode = true;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Sockets.Socket" /> class using the specified address family, socket type and protocol.</summary>
		/// <param name="addressFamily">One of the <see cref="T:System.Net.Sockets.AddressFamily" /> values.</param>
		/// <param name="socketType">One of the <see cref="T:System.Net.Sockets.SocketType" /> values.</param>
		/// <param name="protocolType">One of the <see cref="T:System.Net.Sockets.ProtocolType" /> values.</param>
		/// <exception cref="T:System.Net.Sockets.SocketException">The combination of <paramref name="addressFamily" />, <paramref name="socketType" />, and <paramref name="protocolType" /> results in an invalid socket.</exception>
		// Token: 0x06003D3D RID: 15677 RVA: 0x000D0EC4 File Offset: 0x000CF0C4
		public Socket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
		{
			this.ReadSem = new SemaphoreSlim(1, 1);
			this.WriteSem = new SemaphoreSlim(1, 1);
			this.is_blocking = true;
			base..ctor();
			Socket.s_LoggingEnabled = Logging.On;
			bool flag = Socket.s_LoggingEnabled;
			Socket.InitializeSockets();
			int num;
			this.m_Handle = new SafeSocketHandle(Socket.Socket_icall(addressFamily, socketType, protocolType, out num), true);
			if (this.m_Handle.IsInvalid)
			{
				throw new SocketException();
			}
			this.addressFamily = addressFamily;
			this.socketType = socketType;
			this.protocolType = protocolType;
			IPProtectionLevel ipprotectionLevel = SettingsSectionInternal.Section.IPProtectionLevel;
			if (ipprotectionLevel != IPProtectionLevel.Unspecified)
			{
				this.SetIPProtectionLevel(ipprotectionLevel);
			}
			this.SocketDefaults();
			bool flag2 = Socket.s_LoggingEnabled;
		}

		/// <summary>Gets a value indicating whether IPv4 support is available and enabled on the current host.</summary>
		/// <returns>
		///   <see langword="true" /> if the current host supports the IPv4 protocol; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x06003D3E RID: 15678 RVA: 0x000D0F73 File Offset: 0x000CF173
		[Obsolete("SupportsIPv4 is obsoleted for this type, please use OSSupportsIPv4 instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public static bool SupportsIPv4
		{
			get
			{
				Socket.InitializeSockets();
				return Socket.s_SupportsIPv4;
			}
		}

		/// <summary>Indicates whether the underlying operating system and network adaptors support Internet Protocol version 4 (IPv4).</summary>
		/// <returns>
		///   <see langword="true" /> if the operating system and network adaptors support the IPv4 protocol; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x06003D3F RID: 15679 RVA: 0x000D0F73 File Offset: 0x000CF173
		public static bool OSSupportsIPv4
		{
			get
			{
				Socket.InitializeSockets();
				return Socket.s_SupportsIPv4;
			}
		}

		/// <summary>Gets a value that indicates whether the Framework supports IPv6 for certain obsolete <see cref="T:System.Net.Dns" /> members.</summary>
		/// <returns>
		///   <see langword="true" /> if the Framework supports IPv6 for certain obsolete <see cref="T:System.Net.Dns" /> methods; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x06003D40 RID: 15680 RVA: 0x000D0F81 File Offset: 0x000CF181
		[Obsolete("SupportsIPv6 is obsoleted for this type, please use OSSupportsIPv6 instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public static bool SupportsIPv6
		{
			get
			{
				Socket.InitializeSockets();
				return Socket.s_SupportsIPv6;
			}
		}

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06003D41 RID: 15681 RVA: 0x000D0F81 File Offset: 0x000CF181
		internal static bool LegacySupportsIPv6
		{
			get
			{
				Socket.InitializeSockets();
				return Socket.s_SupportsIPv6;
			}
		}

		/// <summary>Indicates whether the underlying operating system and network adaptors support Internet Protocol version 6 (IPv6).</summary>
		/// <returns>
		///   <see langword="true" /> if the operating system and network adaptors support the IPv6 protocol; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06003D42 RID: 15682 RVA: 0x000D0F8F File Offset: 0x000CF18F
		public static bool OSSupportsIPv6
		{
			get
			{
				Socket.InitializeSockets();
				return Socket.s_OSSupportsIPv6;
			}
		}

		/// <summary>Gets the operating system handle for the <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>An <see cref="T:System.IntPtr" /> that represents the operating system handle for the <see cref="T:System.Net.Sockets.Socket" />.</returns>
		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x06003D43 RID: 15683 RVA: 0x000D0F9D File Offset: 0x000CF19D
		public IntPtr Handle
		{
			get
			{
				return this.m_Handle.DangerousGetHandle();
			}
		}

		/// <summary>Specifies whether the socket should only use Overlapped I/O mode.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Net.Sockets.Socket" /> uses only overlapped I/O; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The socket has been bound to a completion port.</exception>
		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x06003D44 RID: 15684 RVA: 0x000D0FAA File Offset: 0x000CF1AA
		// (set) Token: 0x06003D45 RID: 15685 RVA: 0x000D0FB2 File Offset: 0x000CF1B2
		public bool UseOnlyOverlappedIO
		{
			get
			{
				return this.useOverlappedIO;
			}
			set
			{
				this.useOverlappedIO = value;
			}
		}

		/// <summary>Gets the address family of the <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>One of the <see cref="T:System.Net.Sockets.AddressFamily" /> values.</returns>
		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x06003D46 RID: 15686 RVA: 0x000D0FBB File Offset: 0x000CF1BB
		public AddressFamily AddressFamily
		{
			get
			{
				return this.addressFamily;
			}
		}

		/// <summary>Gets the type of the <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>One of the <see cref="T:System.Net.Sockets.SocketType" /> values.</returns>
		// Token: 0x17000E0C RID: 3596
		// (get) Token: 0x06003D47 RID: 15687 RVA: 0x000D0FC3 File Offset: 0x000CF1C3
		public SocketType SocketType
		{
			get
			{
				return this.socketType;
			}
		}

		/// <summary>Gets the protocol type of the <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>One of the <see cref="T:System.Net.Sockets.ProtocolType" /> values.</returns>
		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x06003D48 RID: 15688 RVA: 0x000D0FCB File Offset: 0x000CF1CB
		public ProtocolType ProtocolType
		{
			get
			{
				return this.protocolType;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that specifies whether the <see cref="T:System.Net.Sockets.Socket" /> allows only one process to bind to a port.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Net.Sockets.Socket" /> allows only one socket to bind to a specific port; otherwise, <see langword="false" />. The default is <see langword="true" /> for Windows Server 2003 and Windows XP Service Pack 2, and <see langword="false" /> for all other versions.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.Bind(System.Net.EndPoint)" /> has been called for this <see cref="T:System.Net.Sockets.Socket" />.</exception>
		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x06003D49 RID: 15689 RVA: 0x000D0FD3 File Offset: 0x000CF1D3
		// (set) Token: 0x06003D4A RID: 15690 RVA: 0x000D0FEC File Offset: 0x000CF1EC
		public bool ExclusiveAddressUse
		{
			get
			{
				return (int)this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse) != 0;
			}
			set
			{
				if (this.IsBound)
				{
					throw new InvalidOperationException(SR.GetString("The socket must not be bound or connected."));
				}
				this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, value ? 1 : 0);
			}
		}

		/// <summary>Gets or sets a value that specifies the size of the receive buffer of the <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that contains the size, in bytes, of the receive buffer. The default is 8192.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation is less than 0.</exception>
		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x06003D4B RID: 15691 RVA: 0x000D101A File Offset: 0x000CF21A
		// (set) Token: 0x06003D4C RID: 15692 RVA: 0x000D1031 File Offset: 0x000CF231
		public int ReceiveBufferSize
		{
			get
			{
				return (int)this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer);
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the size of the send buffer of the <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that contains the size, in bytes, of the send buffer. The default is 8192.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation is less than 0.</exception>
		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x06003D4D RID: 15693 RVA: 0x000D1053 File Offset: 0x000CF253
		// (set) Token: 0x06003D4E RID: 15694 RVA: 0x000D106A File Offset: 0x000CF26A
		public int SendBufferSize
		{
			get
			{
				return (int)this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer);
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the amount of time after which a synchronous <see cref="Overload:System.Net.Sockets.Socket.Receive" /> call will time out.</summary>
		/// <returns>The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation is less than -1.</exception>
		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x06003D4F RID: 15695 RVA: 0x000D108C File Offset: 0x000CF28C
		// (set) Token: 0x06003D50 RID: 15696 RVA: 0x000D10A3 File Offset: 0x000CF2A3
		public int ReceiveTimeout
		{
			get
			{
				return (int)this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout);
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value == -1)
				{
					value = 0;
				}
				this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the amount of time after which a synchronous <see cref="Overload:System.Net.Sockets.Socket.Send" /> call will time out.</summary>
		/// <returns>The time-out value, in milliseconds. If you set the property with a value between 1 and 499, the value will be changed to 500. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation is less than -1.</exception>
		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x06003D51 RID: 15697 RVA: 0x000D10CC File Offset: 0x000CF2CC
		// (set) Token: 0x06003D52 RID: 15698 RVA: 0x000D10E3 File Offset: 0x000CF2E3
		public int SendTimeout
		{
			get
			{
				return (int)this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout);
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value == -1)
				{
					value = 0;
				}
				this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, value);
			}
		}

		/// <summary>Gets or sets a value that specifies whether the <see cref="T:System.Net.Sockets.Socket" /> will delay closing a socket in an attempt to send all pending data.</summary>
		/// <returns>A <see cref="T:System.Net.Sockets.LingerOption" /> that specifies how to linger while closing a socket.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x06003D53 RID: 15699 RVA: 0x000D110C File Offset: 0x000CF30C
		// (set) Token: 0x06003D54 RID: 15700 RVA: 0x000D1123 File Offset: 0x000CF323
		public LingerOption LingerState
		{
			get
			{
				return (LingerOption)this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger);
			}
			set
			{
				this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the Time To Live (TTL) value of Internet Protocol (IP) packets sent by the <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>The TTL value.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The TTL value can't be set to a negative number.</exception>
		/// <exception cref="T:System.NotSupportedException">This property can be set only for sockets in the <see cref="F:System.Net.Sockets.AddressFamily.InterNetwork" /> or <see cref="F:System.Net.Sockets.AddressFamily.InterNetworkV6" /> families.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. This error is also returned when an attempt was made to set TTL to a value higher than 255.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x06003D55 RID: 15701 RVA: 0x000D1138 File Offset: 0x000CF338
		// (set) Token: 0x06003D56 RID: 15702 RVA: 0x000D1188 File Offset: 0x000CF388
		public short Ttl
		{
			get
			{
				if (this.addressFamily == AddressFamily.InterNetwork)
				{
					return (short)((int)this.GetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress));
				}
				if (this.addressFamily == AddressFamily.InterNetworkV6)
				{
					return (short)((int)this.GetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.ReuseAddress));
				}
				throw new NotSupportedException(SR.GetString("This protocol version is not supported."));
			}
			set
			{
				if (value < 0 || value > 255)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (this.addressFamily == AddressFamily.InterNetwork)
				{
					this.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, (int)value);
					return;
				}
				if (this.addressFamily == AddressFamily.InterNetworkV6)
				{
					this.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.ReuseAddress, (int)value);
					return;
				}
				throw new NotSupportedException(SR.GetString("This protocol version is not supported."));
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that specifies whether the <see cref="T:System.Net.Sockets.Socket" /> allows Internet Protocol (IP) datagrams to be fragmented.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Net.Sockets.Socket" /> allows datagram fragmentation; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		/// <exception cref="T:System.NotSupportedException">This property can be set only for sockets in the <see cref="F:System.Net.Sockets.AddressFamily.InterNetwork" /> or <see cref="F:System.Net.Sockets.AddressFamily.InterNetworkV6" /> families.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x06003D57 RID: 15703 RVA: 0x000D11E3 File Offset: 0x000CF3E3
		// (set) Token: 0x06003D58 RID: 15704 RVA: 0x000D1211 File Offset: 0x000CF411
		public bool DontFragment
		{
			get
			{
				if (this.addressFamily == AddressFamily.InterNetwork)
				{
					return (int)this.GetSocketOption(SocketOptionLevel.IP, SocketOptionName.DontFragment) != 0;
				}
				throw new NotSupportedException(SR.GetString("This protocol version is not supported."));
			}
			set
			{
				if (this.addressFamily == AddressFamily.InterNetwork)
				{
					this.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DontFragment, value ? 1 : 0);
					return;
				}
				throw new NotSupportedException(SR.GetString("This protocol version is not supported."));
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that specifies whether the <see cref="T:System.Net.Sockets.Socket" /> is a dual-mode socket used for both IPv4 and IPv6.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Net.Sockets.Socket" /> is a  dual-mode socket; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x06003D59 RID: 15705 RVA: 0x000D123C File Offset: 0x000CF43C
		// (set) Token: 0x06003D5A RID: 15706 RVA: 0x000D126A File Offset: 0x000CF46A
		public bool DualMode
		{
			get
			{
				if (this.AddressFamily != AddressFamily.InterNetworkV6)
				{
					throw new NotSupportedException(SR.GetString("This protocol version is not supported."));
				}
				return (int)this.GetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only) == 0;
			}
			set
			{
				if (this.AddressFamily != AddressFamily.InterNetworkV6)
				{
					throw new NotSupportedException(SR.GetString("This protocol version is not supported."));
				}
				this.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, value ? 0 : 1);
			}
		}

		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x06003D5B RID: 15707 RVA: 0x000D1297 File Offset: 0x000CF497
		private bool IsDualMode
		{
			get
			{
				return this.AddressFamily == AddressFamily.InterNetworkV6 && this.DualMode;
			}
		}

		// Token: 0x06003D5C RID: 15708 RVA: 0x000D12AB File Offset: 0x000CF4AB
		internal bool CanTryAddressFamily(AddressFamily family)
		{
			return family == this.addressFamily || (family == AddressFamily.InterNetwork && this.IsDualMode);
		}

		/// <summary>Establishes a connection to a remote host. The host is specified by an array of IP addresses and a port number.</summary>
		/// <param name="addresses">The IP addresses of the remote host.</param>
		/// <param name="port">The port number of the remote host.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="addresses" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The port number is not valid.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.NotSupportedException">This method is valid for sockets in the <see cref="F:System.Net.Sockets.AddressFamily.InterNetwork" /> or <see cref="F:System.Net.Sockets.AddressFamily.InterNetworkV6" /> families.</exception>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="address" /> is zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> has been placed in a listening state by calling <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" />.</exception>
		// Token: 0x06003D5D RID: 15709 RVA: 0x000D12C4 File Offset: 0x000CF4C4
		public void Connect(IPAddress[] addresses, int port)
		{
			bool flag = Socket.s_LoggingEnabled;
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (addresses == null)
			{
				throw new ArgumentNullException("addresses");
			}
			if (addresses.Length == 0)
			{
				throw new ArgumentException(SR.GetString("The number of specified IP addresses has to be greater than 0."), "addresses");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			if (this.addressFamily != AddressFamily.InterNetwork && this.addressFamily != AddressFamily.InterNetworkV6)
			{
				throw new NotSupportedException(SR.GetString("This protocol version is not supported."));
			}
			Exception ex = null;
			foreach (IPAddress ipaddress in addresses)
			{
				if (this.CanTryAddressFamily(ipaddress.AddressFamily))
				{
					try
					{
						this.Connect(new IPEndPoint(ipaddress, port));
						ex = null;
						break;
					}
					catch (Exception ex2)
					{
						if (NclUtilities.IsFatal(ex2))
						{
							throw;
						}
						ex = ex2;
					}
				}
			}
			if (ex != null)
			{
				throw ex;
			}
			if (!this.Connected)
			{
				throw new ArgumentException(SR.GetString("None of the discovered or specified addresses match the socket address family."), "addresses");
			}
			bool flag2 = Socket.s_LoggingEnabled;
		}

		/// <summary>Sends the specified number of bytes of data to a connected <see cref="T:System.Net.Sockets.Socket" />, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent.</param>
		/// <param name="size">The number of bytes to send.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <returns>The number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="size" /> is less than 0 or exceeds the size of the buffer.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="socketFlags" /> is not a valid combination of values.  
		/// -or-  
		/// An operating system error occurs while accessing the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003D5E RID: 15710 RVA: 0x000D13CC File Offset: 0x000CF5CC
		public int Send(byte[] buffer, int size, SocketFlags socketFlags)
		{
			return this.Send(buffer, 0, size, socketFlags);
		}

		/// <summary>Sends data to a connected <see cref="T:System.Net.Sockets.Socket" /> using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <returns>The number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003D5F RID: 15711 RVA: 0x000D13D8 File Offset: 0x000CF5D8
		public int Send(byte[] buffer, SocketFlags socketFlags)
		{
			return this.Send(buffer, 0, (buffer != null) ? buffer.Length : 0, socketFlags);
		}

		/// <summary>Sends data to a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent.</param>
		/// <returns>The number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003D60 RID: 15712 RVA: 0x000D13EC File Offset: 0x000CF5EC
		public int Send(byte[] buffer)
		{
			return this.Send(buffer, 0, (buffer != null) ? buffer.Length : 0, SocketFlags.None);
		}

		/// <summary>Sends the set of buffers in the list to a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <param name="buffers">A list of <see cref="T:System.ArraySegment`1" />s of type <see cref="T:System.Byte" /> that contains the data to be sent.</param>
		/// <returns>The number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffers" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="buffers" /> is empty.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See remarks section below.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003D61 RID: 15713 RVA: 0x000D1400 File Offset: 0x000CF600
		public int Send(IList<ArraySegment<byte>> buffers)
		{
			return this.Send(buffers, SocketFlags.None);
		}

		/// <summary>Sends the set of buffers in the list to a connected <see cref="T:System.Net.Sockets.Socket" />, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <param name="buffers">A list of <see cref="T:System.ArraySegment`1" />s of type <see cref="T:System.Byte" /> that contains the data to be sent.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <returns>The number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffers" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="buffers" /> is empty.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003D62 RID: 15714 RVA: 0x000D140C File Offset: 0x000CF60C
		public int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
		{
			SocketError socketError;
			int num = this.Send(buffers, socketFlags, out socketError);
			if (socketError != SocketError.Success)
			{
				throw new SocketException(socketError);
			}
			return num;
		}

		/// <summary>Sends the file <paramref name="fileName" /> to a connected <see cref="T:System.Net.Sockets.Socket" /> object with the <see cref="F:System.Net.Sockets.TransmitFileOptions.UseDefaultWorkerThread" /> transmit flag.</summary>
		/// <param name="fileName">A <see cref="T:System.String" /> that contains the path and name of the file to be sent. This parameter can be <see langword="null" />.</param>
		/// <exception cref="T:System.NotSupportedException">The socket is not connected to a remote host.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> object is not in blocking mode and cannot accept this synchronous call.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file <paramref name="fileName" /> was not found.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		// Token: 0x06003D63 RID: 15715 RVA: 0x000D142D File Offset: 0x000CF62D
		public void SendFile(string fileName)
		{
			this.SendFile(fileName, null, null, TransmitFileOptions.UseDefaultWorkerThread);
		}

		/// <summary>Sends the specified number of bytes of data to a connected <see cref="T:System.Net.Sockets.Socket" />, starting at the specified offset, and using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent.</param>
		/// <param name="offset">The position in the data buffer at which to begin sending data.</param>
		/// <param name="size">The number of bytes to send.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <returns>The number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.  
		/// -or-  
		/// <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.  
		/// -or-  
		/// <paramref name="size" /> is less than 0.  
		/// -or-  
		/// <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="socketFlags" /> is not a valid combination of values.  
		/// -or-  
		/// An operating system error occurs while accessing the <see cref="T:System.Net.Sockets.Socket" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003D64 RID: 15716 RVA: 0x000D143C File Offset: 0x000CF63C
		public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags)
		{
			SocketError socketError;
			int num = this.Send(buffer, offset, size, socketFlags, out socketError);
			if (socketError != SocketError.Success)
			{
				throw new SocketException(socketError);
			}
			return num;
		}

		/// <summary>Sends the specified number of bytes of data to the specified endpoint using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent.</param>
		/// <param name="size">The number of bytes to send.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="remoteEP">The <see cref="T:System.Net.EndPoint" /> that represents the destination location for the data.</param>
		/// <returns>The number of bytes sent.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="remoteEP" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified <paramref name="size" /> exceeds the size of <paramref name="buffer" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003D65 RID: 15717 RVA: 0x000D1460 File Offset: 0x000CF660
		public int SendTo(byte[] buffer, int size, SocketFlags socketFlags, EndPoint remoteEP)
		{
			return this.SendTo(buffer, 0, size, socketFlags, remoteEP);
		}

		/// <summary>Sends data to a specific endpoint using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="remoteEP">The <see cref="T:System.Net.EndPoint" /> that represents the destination location for the data.</param>
		/// <returns>The number of bytes sent.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="remoteEP" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003D66 RID: 15718 RVA: 0x000D146E File Offset: 0x000CF66E
		public int SendTo(byte[] buffer, SocketFlags socketFlags, EndPoint remoteEP)
		{
			return this.SendTo(buffer, 0, (buffer != null) ? buffer.Length : 0, socketFlags, remoteEP);
		}

		/// <summary>Sends data to the specified endpoint.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent.</param>
		/// <param name="remoteEP">The <see cref="T:System.Net.EndPoint" /> that represents the destination for the data.</param>
		/// <returns>The number of bytes sent.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="remoteEP" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003D67 RID: 15719 RVA: 0x000D1483 File Offset: 0x000CF683
		public int SendTo(byte[] buffer, EndPoint remoteEP)
		{
			return this.SendTo(buffer, 0, (buffer != null) ? buffer.Length : 0, SocketFlags.None, remoteEP);
		}

		/// <summary>Receives the specified number of bytes of data from a bound <see cref="T:System.Net.Sockets.Socket" /> into a receive buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data.</param>
		/// <param name="size">The number of bytes to receive.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <returns>The number of bytes received.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="size" /> exceeds the size of <paramref name="buffer" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call stack does not have the required permissions.</exception>
		// Token: 0x06003D68 RID: 15720 RVA: 0x000D1498 File Offset: 0x000CF698
		public int Receive(byte[] buffer, int size, SocketFlags socketFlags)
		{
			return this.Receive(buffer, 0, size, socketFlags);
		}

		/// <summary>Receives data from a bound <see cref="T:System.Net.Sockets.Socket" /> into a receive buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <returns>The number of bytes received.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call stack does not have the required permissions.</exception>
		// Token: 0x06003D69 RID: 15721 RVA: 0x000D14A4 File Offset: 0x000CF6A4
		public int Receive(byte[] buffer, SocketFlags socketFlags)
		{
			return this.Receive(buffer, 0, (buffer != null) ? buffer.Length : 0, socketFlags);
		}

		/// <summary>Receives data from a bound <see cref="T:System.Net.Sockets.Socket" /> into a receive buffer.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data.</param>
		/// <returns>The number of bytes received.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call stack does not have the required permissions.</exception>
		// Token: 0x06003D6A RID: 15722 RVA: 0x000D14B8 File Offset: 0x000CF6B8
		public int Receive(byte[] buffer)
		{
			return this.Receive(buffer, 0, (buffer != null) ? buffer.Length : 0, SocketFlags.None);
		}

		/// <summary>Receives the specified number of bytes from a bound <see cref="T:System.Net.Sockets.Socket" /> into the specified offset position of the receive buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for received data.</param>
		/// <param name="offset">The location in <paramref name="buffer" /> to store the received data.</param>
		/// <param name="size">The number of bytes to receive.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <returns>The number of bytes received.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.  
		/// -or-  
		/// <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.  
		/// -or-  
		/// <paramref name="size" /> is less than 0.  
		/// -or-  
		/// <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="socketFlags" /> is not a valid combination of values.  
		/// -or-  
		/// The <see cref="P:System.Net.Sockets.Socket.LocalEndPoint" /> property was not set.  
		/// -or-  
		/// An operating system error occurs while accessing the <see cref="T:System.Net.Sockets.Socket" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call stack does not have the required permissions.</exception>
		// Token: 0x06003D6B RID: 15723 RVA: 0x000D14CC File Offset: 0x000CF6CC
		public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags)
		{
			SocketError socketError;
			int num = this.Receive(buffer, offset, size, socketFlags, out socketError);
			if (socketError != SocketError.Success)
			{
				throw new SocketException(socketError);
			}
			return num;
		}

		/// <summary>Receives data from a bound <see cref="T:System.Net.Sockets.Socket" /> into the list of receive buffers.</summary>
		/// <param name="buffers">A list of <see cref="T:System.ArraySegment`1" />s of type <see cref="T:System.Byte" /> that contains the received data.</param>
		/// <returns>The number of bytes received.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="buffer" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred while attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003D6C RID: 15724 RVA: 0x000D14F0 File Offset: 0x000CF6F0
		public int Receive(IList<ArraySegment<byte>> buffers)
		{
			return this.Receive(buffers, SocketFlags.None);
		}

		/// <summary>Receives data from a bound <see cref="T:System.Net.Sockets.Socket" /> into the list of receive buffers, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <param name="buffers">A list of <see cref="T:System.ArraySegment`1" />s of type <see cref="T:System.Byte" /> that contains the received data.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <returns>The number of bytes received.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffers" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="buffers" />.Count is zero.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred while attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003D6D RID: 15725 RVA: 0x000D14FC File Offset: 0x000CF6FC
		public int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
		{
			SocketError socketError;
			int num = this.Receive(buffers, socketFlags, out socketError);
			if (socketError != SocketError.Success)
			{
				throw new SocketException(socketError);
			}
			return num;
		}

		/// <summary>Receives the specified number of bytes into the data buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />, and stores the endpoint.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for received data.</param>
		/// <param name="size">The number of bytes to receive.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" />, passed by reference, that represents the remote server.</param>
		/// <returns>The number of bytes received.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="remoteEP" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="size" /> is less than 0.  
		/// -or-  
		/// <paramref name="size" /> is greater than the length of <paramref name="buffer" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="socketFlags" /> is not a valid combination of values.  
		/// -or-  
		/// The <see cref="P:System.Net.Sockets.Socket.LocalEndPoint" /> property was not set.  
		/// -or-  
		/// An operating system error occurs while accessing the <see cref="T:System.Net.Sockets.Socket" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call stack does not have the required permissions.</exception>
		// Token: 0x06003D6E RID: 15726 RVA: 0x000D151D File Offset: 0x000CF71D
		public int ReceiveFrom(byte[] buffer, int size, SocketFlags socketFlags, ref EndPoint remoteEP)
		{
			return this.ReceiveFrom(buffer, 0, size, socketFlags, ref remoteEP);
		}

		/// <summary>Receives a datagram into the data buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />, and stores the endpoint.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" />, passed by reference, that represents the remote server.</param>
		/// <returns>The number of bytes received.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="remoteEP" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call stack does not have the required permissions.</exception>
		// Token: 0x06003D6F RID: 15727 RVA: 0x000D152B File Offset: 0x000CF72B
		public int ReceiveFrom(byte[] buffer, SocketFlags socketFlags, ref EndPoint remoteEP)
		{
			return this.ReceiveFrom(buffer, 0, (buffer != null) ? buffer.Length : 0, socketFlags, ref remoteEP);
		}

		/// <summary>Receives a datagram into the data buffer and stores the endpoint.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for received data.</param>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" />, passed by reference, that represents the remote server.</param>
		/// <returns>The number of bytes received.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="remoteEP" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call stack does not have the required permissions.</exception>
		// Token: 0x06003D70 RID: 15728 RVA: 0x000D1540 File Offset: 0x000CF740
		public int ReceiveFrom(byte[] buffer, ref EndPoint remoteEP)
		{
			return this.ReceiveFrom(buffer, 0, (buffer != null) ? buffer.Length : 0, SocketFlags.None, ref remoteEP);
		}

		/// <summary>Sets low-level operating modes for the <see cref="T:System.Net.Sockets.Socket" /> using the <see cref="T:System.Net.Sockets.IOControlCode" /> enumeration to specify control codes.</summary>
		/// <param name="ioControlCode">A <see cref="T:System.Net.Sockets.IOControlCode" /> value that specifies the control code of the operation to perform.</param>
		/// <param name="optionInValue">An array of type <see cref="T:System.Byte" /> that contains the input data required by the operation.</param>
		/// <param name="optionOutValue">An array of type <see cref="T:System.Byte" /> that contains the output data returned by the operation.</param>
		/// <returns>The number of bytes in the <paramref name="optionOutValue" /> parameter.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to change the blocking mode without using the <see cref="P:System.Net.Sockets.Socket.Blocking" /> property.</exception>
		// Token: 0x06003D71 RID: 15729 RVA: 0x000D1555 File Offset: 0x000CF755
		public int IOControl(IOControlCode ioControlCode, byte[] optionInValue, byte[] optionOutValue)
		{
			return this.IOControl((int)ioControlCode, optionInValue, optionOutValue);
		}

		/// <summary>Set the IP protection level on a socket.</summary>
		/// <param name="level">The IP protection level to set on this socket.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="level" /> parameter cannot be <see cref="F:System.Net.Sockets.IPProtectionLevel.Unspecified" />. The IP protection level cannot be set to unspecified.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Net.Sockets.AddressFamily" /> of the socket must be either <see cref="F:System.Net.Sockets.AddressFamily.InterNetworkV6" /> or <see cref="F:System.Net.Sockets.AddressFamily.InterNetwork" />.</exception>
		// Token: 0x06003D72 RID: 15730 RVA: 0x000D1564 File Offset: 0x000CF764
		public void SetIPProtectionLevel(IPProtectionLevel level)
		{
			if (level == IPProtectionLevel.Unspecified)
			{
				throw new ArgumentException(SR.GetString("The specified value is not valid."), "level");
			}
			if (this.addressFamily == AddressFamily.InterNetworkV6)
			{
				this.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPProtectionLevel, (int)level);
				return;
			}
			if (this.addressFamily == AddressFamily.InterNetwork)
			{
				this.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.IPProtectionLevel, (int)level);
				return;
			}
			throw new NotSupportedException(SR.GetString("This protocol version is not supported."));
		}

		/// <summary>Sends the file <paramref name="fileName" /> to a connected <see cref="T:System.Net.Sockets.Socket" /> object using the <see cref="F:System.Net.Sockets.TransmitFileOptions.UseDefaultWorkerThread" /> flag.</summary>
		/// <param name="fileName">A string that contains the path and name of the file to send. This parameter can be <see langword="null" />.</param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
		/// <param name="state">An object that contains state information for this request.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that represents the asynchronous send.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed.</exception>
		/// <exception cref="T:System.NotSupportedException">The socket is not connected to a remote host.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file <paramref name="fileName" /> was not found.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See remarks section below.</exception>
		// Token: 0x06003D73 RID: 15731 RVA: 0x000D15C3 File Offset: 0x000CF7C3
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginSendFile(string fileName, AsyncCallback callback, object state)
		{
			return this.BeginSendFile(fileName, null, null, TransmitFileOptions.UseDefaultWorkerThread, callback, state);
		}

		/// <summary>Begins an asynchronous request for a remote host connection. The host is specified by an <see cref="T:System.Net.IPAddress" /> and a port number.</summary>
		/// <param name="address">The <see cref="T:System.Net.IPAddress" /> of the remote host.</param>
		/// <param name="port">The port number of the remote host.</param>
		/// <param name="requestCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the connect operation is complete.</param>
		/// <param name="state">A user-defined object that contains information about the connect operation. This object is passed to the <paramref name="requestCallback" /> delegate when the operation is complete.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous connection.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Net.Sockets.Socket" /> is not in the socket family.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The port number is not valid.</exception>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="address" /> is zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> has been placed in a listening state by calling <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" />, or an asynchronous operation is already in progress.</exception>
		// Token: 0x06003D74 RID: 15732 RVA: 0x000D15D4 File Offset: 0x000CF7D4
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object state)
		{
			bool flag = Socket.s_LoggingEnabled;
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (!ValidationHelper.ValidateTcpPort(port))
			{
				throw new ArgumentOutOfRangeException("port");
			}
			if (!this.CanTryAddressFamily(address.AddressFamily))
			{
				throw new NotSupportedException(SR.GetString("This protocol version is not supported."));
			}
			IAsyncResult asyncResult = this.BeginConnect(new IPEndPoint(address, port), requestCallback, state);
			bool flag2 = Socket.s_LoggingEnabled;
			return asyncResult;
		}

		/// <summary>Sends data asynchronously to a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to send.</param>
		/// <param name="offset">The zero-based position in the <paramref name="buffer" /> parameter at which to begin sending data.</param>
		/// <param name="size">The number of bytes to send.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
		/// <param name="state">An object that contains state information for this request.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous send.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See remarks section below.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.  
		/// -or-  
		/// <paramref name="offset" /> is less than the length of <paramref name="buffer" />.  
		/// -or-  
		/// <paramref name="size" /> is less than 0.  
		/// -or-  
		/// <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003D75 RID: 15733 RVA: 0x000D165C File Offset: 0x000CF85C
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			SocketError socketError;
			IAsyncResult asyncResult = this.BeginSend(buffer, offset, size, socketFlags, out socketError, callback, state);
			if (socketError != SocketError.Success && socketError != SocketError.IOPending)
			{
				throw new SocketException(socketError);
			}
			return asyncResult;
		}

		/// <summary>Sends data asynchronously to a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <param name="buffers">An array of type <see cref="T:System.Byte" /> that contains the data to send.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
		/// <param name="state">An object that contains state information for this request.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous send.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffers" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="buffers" /> is empty.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See remarks section below.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003D76 RID: 15734 RVA: 0x000D168C File Offset: 0x000CF88C
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			SocketError socketError;
			IAsyncResult asyncResult = this.BeginSend(buffers, socketFlags, out socketError, callback, state);
			if (socketError != SocketError.Success && socketError != SocketError.IOPending)
			{
				throw new SocketException(socketError);
			}
			return asyncResult;
		}

		/// <summary>Ends a pending asynchronous send.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information for this asynchronous operation.</param>
		/// <returns>If successful, the number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />; otherwise, an invalid <see cref="T:System.Net.Sockets.Socket" /> error.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by a call to the <see cref="M:System.Net.Sockets.Socket.BeginSend(System.Byte[],System.Int32,System.Int32,System.Net.Sockets.SocketFlags,System.AsyncCallback,System.Object)" /> method.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndSend(System.IAsyncResult)" /> was previously called for the asynchronous send.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003D77 RID: 15735 RVA: 0x000D16B8 File Offset: 0x000CF8B8
		public int EndSend(IAsyncResult asyncResult)
		{
			SocketError socketError;
			int num = this.EndSend(asyncResult, out socketError);
			if (socketError != SocketError.Success)
			{
				throw new SocketException(socketError);
			}
			return num;
		}

		/// <summary>Begins to asynchronously receive data from a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data.</param>
		/// <param name="offset">The zero-based position in the <paramref name="buffer" /> parameter at which to store the received data.</param>
		/// <param name="size">The number of bytes to receive.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete.</param>
		/// <param name="state">A user-defined object that contains information about the receive operation. This object is passed to the <see cref="M:System.Net.Sockets.Socket.EndReceive(System.IAsyncResult)" /> delegate when the operation is complete.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous read.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///   <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.  
		/// -or-  
		/// <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.  
		/// -or-  
		/// <paramref name="size" /> is less than 0.  
		/// -or-  
		/// <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter.</exception>
		// Token: 0x06003D78 RID: 15736 RVA: 0x000D16D8 File Offset: 0x000CF8D8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			SocketError socketError;
			IAsyncResult asyncResult = this.BeginReceive(buffer, offset, size, socketFlags, out socketError, callback, state);
			if (socketError != SocketError.Success && socketError != SocketError.IOPending)
			{
				throw new SocketException(socketError);
			}
			return asyncResult;
		}

		/// <summary>Begins to asynchronously receive data from a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <param name="buffers">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete.</param>
		/// <param name="state">A user-defined object that contains information about the receive operation. This object is passed to the <see cref="M:System.Net.Sockets.Socket.EndReceive(System.IAsyncResult)" /> delegate when the operation is complete.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous read.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///   <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003D79 RID: 15737 RVA: 0x000D1708 File Offset: 0x000CF908
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			SocketError socketError;
			IAsyncResult asyncResult = this.BeginReceive(buffers, socketFlags, out socketError, callback, state);
			if (socketError != SocketError.Success && socketError != SocketError.IOPending)
			{
				throw new SocketException(socketError);
			}
			return asyncResult;
		}

		/// <summary>Ends a pending asynchronous read.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information and any user defined data for this asynchronous operation.</param>
		/// <returns>The number of bytes received.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by a call to the <see cref="M:System.Net.Sockets.Socket.BeginReceive(System.Byte[],System.Int32,System.Int32,System.Net.Sockets.SocketFlags,System.AsyncCallback,System.Object)" /> method.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndReceive(System.IAsyncResult)" /> was previously called for the asynchronous read.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003D7A RID: 15738 RVA: 0x000D1734 File Offset: 0x000CF934
		public int EndReceive(IAsyncResult asyncResult)
		{
			SocketError socketError;
			int num = this.EndReceive(asyncResult, out socketError);
			if (socketError != SocketError.Success)
			{
				throw new SocketException(socketError);
			}
			return num;
		}

		/// <summary>Begins an asynchronous operation to accept an incoming connection attempt and receives the first block of data sent by the client application.</summary>
		/// <param name="receiveSize">The number of bytes to accept from the sender.</param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
		/// <param name="state">An object that contains state information for this request.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous <see cref="T:System.Net.Sockets.Socket" /> creation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows NT is required for this method.</exception>
		/// <exception cref="T:System.InvalidOperationException">The accepting socket is not listening for connections. You must call <see cref="M:System.Net.Sockets.Socket.Bind(System.Net.EndPoint)" /> and <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" /> before calling <see cref="M:System.Net.Sockets.Socket.BeginAccept(System.AsyncCallback,System.Object)" />.  
		///  -or-  
		///  The accepted socket is bound.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="receiveSize" /> is less than 0.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		// Token: 0x06003D7B RID: 15739 RVA: 0x000D1754 File Offset: 0x000CF954
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginAccept(int receiveSize, AsyncCallback callback, object state)
		{
			return this.BeginAccept(null, receiveSize, callback, state);
		}

		/// <summary>Asynchronously accepts an incoming connection attempt and creates a new <see cref="T:System.Net.Sockets.Socket" /> object to handle remote host communication. This method returns a buffer that contains the initial data transferred.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the bytes transferred.</param>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> object that stores state information for this asynchronous operation as well as any user defined data.</param>
		/// <returns>A <see cref="T:System.Net.Sockets.Socket" /> object to handle communication with the remote host.</returns>
		/// <exception cref="T:System.NotSupportedException">Windows NT is required for this method.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is empty.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not created by a call to <see cref="M:System.Net.Sockets.Socket.BeginAccept(System.AsyncCallback,System.Object)" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndAccept(System.IAsyncResult)" /> method was previously called.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the <see cref="T:System.Net.Sockets.Socket" /></exception>
		// Token: 0x06003D7C RID: 15740 RVA: 0x000D1760 File Offset: 0x000CF960
		public Socket EndAccept(out byte[] buffer, IAsyncResult asyncResult)
		{
			byte[] array;
			int num;
			Socket socket = this.EndAccept(out array, out num, asyncResult);
			buffer = new byte[num];
			Array.Copy(array, buffer, num);
			return socket;
		}

		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x06003D7D RID: 15741 RVA: 0x000D178C File Offset: 0x000CF98C
		private static object InternalSyncObject
		{
			get
			{
				if (Socket.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref Socket.s_InternalSyncObject, obj, null);
				}
				return Socket.s_InternalSyncObject;
			}
		}

		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x06003D7E RID: 15742 RVA: 0x000D17B8 File Offset: 0x000CF9B8
		internal bool CleanedUp
		{
			get
			{
				return this.m_IntCleanedUp == 1;
			}
		}

		// Token: 0x06003D7F RID: 15743 RVA: 0x000D17C4 File Offset: 0x000CF9C4
		internal static void InitializeSockets()
		{
			if (!Socket.s_Initialized)
			{
				object internalSyncObject = Socket.InternalSyncObject;
				lock (internalSyncObject)
				{
					if (!Socket.s_Initialized)
					{
						bool flag2 = Socket.IsProtocolSupported(NetworkInterfaceComponent.IPv4);
						bool flag3 = Socket.IsProtocolSupported(NetworkInterfaceComponent.IPv6);
						if (flag3)
						{
							Socket.s_OSSupportsIPv6 = true;
							flag3 = SettingsSectionInternal.Section.Ipv6Enabled;
						}
						Socket.s_SupportsIPv4 = flag2;
						Socket.s_SupportsIPv6 = flag3;
						Socket.s_Initialized = true;
					}
				}
			}
		}

		/// <summary>Releases all resources used by the current instance of the <see cref="T:System.Net.Sockets.Socket" /> class.</summary>
		// Token: 0x06003D80 RID: 15744 RVA: 0x000D184C File Offset: 0x000CFA4C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003D81 RID: 15745 RVA: 0x000D185C File Offset: 0x000CFA5C
		~Socket()
		{
			this.Dispose(false);
		}

		/// <summary>Begins an asynchronous request for a connection to a remote host.</summary>
		/// <param name="socketType">One of the <see cref="T:System.Net.Sockets.SocketType" /> values.</param>
		/// <param name="protocolType">One of the <see cref="T:System.Net.Sockets.ProtocolType" /> values.</param>
		/// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
		/// <returns>
		///   <see langword="true" /> if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation.  
		/// <see langword="false" /> if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
		/// <exception cref="T:System.ArgumentException">An argument is not valid. This exception occurs if multiple buffers are specified, the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.BufferList" /> property is not null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="e" /> parameter cannot be null and the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.RemoteEndPoint" /> cannot be null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> is listening or a socket operation was already in progress using the <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object specified in the <paramref name="e" /> parameter.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows XP or later is required for this method. This exception also occurs if the local endpoint and the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.RemoteEndPoint" /> are not the same address family.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Security.SecurityException">A caller higher in the call stack does not have permission for the requested operation.</exception>
		// Token: 0x06003D82 RID: 15746 RVA: 0x000D188C File Offset: 0x000CFA8C
		public static bool ConnectAsync(SocketType socketType, ProtocolType protocolType, SocketAsyncEventArgs e)
		{
			bool flag = Socket.s_LoggingEnabled;
			if (e.BufferList != null)
			{
				throw new ArgumentException(SR.GetString("Multiple buffers cannot be used with this method."), "BufferList");
			}
			if (e.RemoteEndPoint == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			EndPoint remoteEndPoint = e.RemoteEndPoint;
			DnsEndPoint dnsEndPoint = remoteEndPoint as DnsEndPoint;
			bool flag2;
			if (dnsEndPoint != null)
			{
				Socket socket = null;
				MultipleConnectAsync multipleConnectAsync;
				if (dnsEndPoint.AddressFamily == AddressFamily.Unspecified)
				{
					multipleConnectAsync = new DualSocketMultipleConnectAsync(socketType, protocolType);
				}
				else
				{
					socket = new Socket(dnsEndPoint.AddressFamily, socketType, protocolType);
					multipleConnectAsync = new SingleSocketMultipleConnectAsync(socket, false);
				}
				e.StartOperationCommon(socket);
				e.StartOperationWrapperConnect(multipleConnectAsync);
				try
				{
					flag2 = multipleConnectAsync.StartConnectAsync(e, dnsEndPoint);
					goto IL_00B7;
				}
				catch
				{
					Interlocked.Exchange(ref e.in_progress, 0);
					throw;
				}
			}
			flag2 = new Socket(remoteEndPoint.AddressFamily, socketType, protocolType).ConnectAsync(e);
			IL_00B7:
			bool flag3 = Socket.s_LoggingEnabled;
			return flag2;
		}

		// Token: 0x06003D83 RID: 15747 RVA: 0x000D196C File Offset: 0x000CFB6C
		internal void InternalShutdown(SocketShutdown how)
		{
			if (!this.is_connected || this.CleanedUp)
			{
				return;
			}
			int num;
			Socket.Shutdown_internal(this.m_Handle, how, out num);
		}

		// Token: 0x06003D84 RID: 15748 RVA: 0x000D1998 File Offset: 0x000CFB98
		internal IAsyncResult UnsafeBeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
		{
			return this.BeginConnect(remoteEP, callback, state);
		}

		// Token: 0x06003D85 RID: 15749 RVA: 0x000D19A3 File Offset: 0x000CFBA3
		internal IAsyncResult UnsafeBeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			return this.BeginSend(buffer, offset, size, socketFlags, callback, state);
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x000D19B4 File Offset: 0x000CFBB4
		internal IAsyncResult UnsafeBeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			return this.BeginReceive(buffer, offset, size, socketFlags, callback, state);
		}

		// Token: 0x06003D87 RID: 15751 RVA: 0x000D19C8 File Offset: 0x000CFBC8
		internal IAsyncResult BeginMultipleSend(BufferOffsetSize[] buffers, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			ArraySegment<byte>[] array = new ArraySegment<byte>[buffers.Length];
			for (int i = 0; i < buffers.Length; i++)
			{
				array[i] = new ArraySegment<byte>(buffers[i].Buffer, buffers[i].Offset, buffers[i].Size);
			}
			return this.BeginSend(array, socketFlags, callback, state);
		}

		// Token: 0x06003D88 RID: 15752 RVA: 0x000D1A1B File Offset: 0x000CFC1B
		internal IAsyncResult UnsafeBeginMultipleSend(BufferOffsetSize[] buffers, SocketFlags socketFlags, AsyncCallback callback, object state)
		{
			return this.BeginMultipleSend(buffers, socketFlags, callback, state);
		}

		// Token: 0x06003D89 RID: 15753 RVA: 0x000D1A28 File Offset: 0x000CFC28
		internal int EndMultipleSend(IAsyncResult asyncResult)
		{
			return this.EndSend(asyncResult);
		}

		// Token: 0x06003D8A RID: 15754 RVA: 0x000D1A34 File Offset: 0x000CFC34
		internal void MultipleSend(BufferOffsetSize[] buffers, SocketFlags socketFlags)
		{
			ArraySegment<byte>[] array = new ArraySegment<byte>[buffers.Length];
			for (int i = 0; i < buffers.Length; i++)
			{
				array[i] = new ArraySegment<byte>(buffers[i].Buffer, buffers[i].Offset, buffers[i].Size);
			}
			this.Send(array, socketFlags);
		}

		// Token: 0x06003D8B RID: 15755 RVA: 0x000D1A88 File Offset: 0x000CFC88
		internal void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue, bool silent)
		{
			if (this.CleanedUp && this.is_closed)
			{
				if (silent)
				{
					return;
				}
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			else
			{
				int num;
				Socket.SetSocketOption_internal(this.m_Handle, optionLevel, optionName, null, null, optionValue, out num);
				if (!silent && num != 0)
				{
					throw new SocketException(num);
				}
				return;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Sockets.Socket" /> class using the specified value returned from <see cref="M:System.Net.Sockets.Socket.DuplicateAndClose(System.Int32)" />.</summary>
		/// <param name="socketInformation">The socket information returned by <see cref="M:System.Net.Sockets.Socket.DuplicateAndClose(System.Int32)" />.</param>
		// Token: 0x06003D8C RID: 15756 RVA: 0x000D1ADC File Offset: 0x000CFCDC
		public Socket(SocketInformation socketInformation)
		{
			this.ReadSem = new SemaphoreSlim(1, 1);
			this.WriteSem = new SemaphoreSlim(1, 1);
			this.is_blocking = true;
			base..ctor();
			this.is_listening = (socketInformation.Options & SocketInformationOptions.Listening) > (SocketInformationOptions)0;
			this.is_connected = (socketInformation.Options & SocketInformationOptions.Connected) > (SocketInformationOptions)0;
			this.is_blocking = (socketInformation.Options & SocketInformationOptions.NonBlocking) == (SocketInformationOptions)0;
			this.useOverlappedIO = (socketInformation.Options & SocketInformationOptions.UseOnlyOverlappedIO) > (SocketInformationOptions)0;
			IList list = DataConverter.Unpack("iiiil", socketInformation.ProtocolInformation, 0);
			this.addressFamily = (AddressFamily)((int)list[0]);
			this.socketType = (SocketType)((int)list[1]);
			this.protocolType = (ProtocolType)((int)list[2]);
			this.is_bound = (int)list[3] != 0;
			this.m_Handle = new SafeSocketHandle((IntPtr)((long)list[4]), true);
			Socket.InitializeSockets();
			this.SocketDefaults();
		}

		// Token: 0x06003D8D RID: 15757 RVA: 0x000D1BE0 File Offset: 0x000CFDE0
		internal Socket(AddressFamily family, SocketType type, ProtocolType proto, SafeSocketHandle safe_handle)
		{
			this.ReadSem = new SemaphoreSlim(1, 1);
			this.WriteSem = new SemaphoreSlim(1, 1);
			this.is_blocking = true;
			base..ctor();
			this.addressFamily = family;
			this.socketType = type;
			this.protocolType = proto;
			this.m_Handle = safe_handle;
			this.is_connected = true;
			Socket.InitializeSockets();
		}

		// Token: 0x06003D8E RID: 15758 RVA: 0x000D1C40 File Offset: 0x000CFE40
		private void SocketDefaults()
		{
			try
			{
				if (this.addressFamily == AddressFamily.InterNetwork)
				{
					this.DontFragment = false;
					if (this.protocolType == ProtocolType.Tcp)
					{
						this.NoDelay = false;
					}
				}
				else if (this.addressFamily == AddressFamily.InterNetworkV6 && this.socketType != SocketType.Raw)
				{
					this.DualMode = true;
				}
			}
			catch (SocketException)
			{
			}
		}

		// Token: 0x06003D8F RID: 15759
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Socket_icall(AddressFamily family, SocketType type, ProtocolType proto, out int error);

		/// <summary>Gets the amount of data that has been received from the network and is available to be read.</summary>
		/// <returns>The number of bytes of data received from the network and available to be read.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x06003D90 RID: 15760 RVA: 0x000D1CA0 File Offset: 0x000CFEA0
		public int Available
		{
			get
			{
				this.ThrowIfDisposedAndClosed();
				int num2;
				int num = Socket.Available_internal(this.m_Handle, out num2);
				if (num2 != 0)
				{
					throw new SocketException(num2);
				}
				return num;
			}
		}

		// Token: 0x06003D91 RID: 15761 RVA: 0x000D1CCC File Offset: 0x000CFECC
		private static int Available_internal(SafeSocketHandle safeHandle, out int error)
		{
			bool flag = false;
			int num;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				num = Socket.Available_icall(safeHandle.DangerousGetHandle(), out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
			return num;
		}

		// Token: 0x06003D92 RID: 15762
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Available_icall(IntPtr socket, out int error);

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that specifies whether the <see cref="T:System.Net.Sockets.Socket" /> can send or receive broadcast packets.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Net.Sockets.Socket" /> allows broadcast packets; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">This option is valid for a datagram socket only.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x06003D93 RID: 15763 RVA: 0x000D1D10 File Offset: 0x000CFF10
		// (set) Token: 0x06003D94 RID: 15764 RVA: 0x000D1D42 File Offset: 0x000CFF42
		public bool EnableBroadcast
		{
			get
			{
				this.ThrowIfDisposedAndClosed();
				if (this.protocolType != ProtocolType.Udp)
				{
					throw new SocketException(10042);
				}
				return (int)this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast) != 0;
			}
			set
			{
				this.ThrowIfDisposedAndClosed();
				if (this.protocolType != ProtocolType.Udp)
				{
					throw new SocketException(10042);
				}
				this.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, value ? 1 : 0);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Net.Sockets.Socket" /> is bound to a specific local port.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Net.Sockets.Socket" /> is bound to a local port; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x06003D95 RID: 15765 RVA: 0x000D1D73 File Offset: 0x000CFF73
		public bool IsBound
		{
			get
			{
				return this.is_bound;
			}
		}

		/// <summary>Gets or sets a value that specifies whether outgoing multicast packets are delivered to the sending application.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Net.Sockets.Socket" /> receives outgoing multicast packets; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x06003D96 RID: 15766 RVA: 0x000D1D7C File Offset: 0x000CFF7C
		// (set) Token: 0x06003D97 RID: 15767 RVA: 0x000D1DE4 File Offset: 0x000CFFE4
		public bool MulticastLoopback
		{
			get
			{
				this.ThrowIfDisposedAndClosed();
				if (this.protocolType == ProtocolType.Tcp)
				{
					throw new SocketException(10042);
				}
				AddressFamily addressFamily = this.addressFamily;
				if (addressFamily == AddressFamily.InterNetwork)
				{
					return (int)this.GetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback) != 0;
				}
				if (addressFamily != AddressFamily.InterNetworkV6)
				{
					throw new NotSupportedException("This property is only valid for InterNetwork and InterNetworkV6 sockets");
				}
				return (int)this.GetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastLoopback) != 0;
			}
			set
			{
				this.ThrowIfDisposedAndClosed();
				if (this.protocolType == ProtocolType.Tcp)
				{
					throw new SocketException(10042);
				}
				AddressFamily addressFamily = this.addressFamily;
				if (addressFamily == AddressFamily.InterNetwork)
				{
					this.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback, value ? 1 : 0);
					return;
				}
				if (addressFamily != AddressFamily.InterNetworkV6)
				{
					throw new NotSupportedException("This property is only valid for InterNetwork and InterNetworkV6 sockets");
				}
				this.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.MulticastLoopback, value ? 1 : 0);
			}
		}

		/// <summary>Gets the local endpoint.</summary>
		/// <returns>The <see cref="T:System.Net.EndPoint" /> that the <see cref="T:System.Net.Sockets.Socket" /> is using for communications.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x06003D98 RID: 15768 RVA: 0x000D1E4C File Offset: 0x000D004C
		public EndPoint LocalEndPoint
		{
			get
			{
				this.ThrowIfDisposedAndClosed();
				if (this.seed_endpoint == null)
				{
					return null;
				}
				int num;
				SocketAddress socketAddress = Socket.LocalEndPoint_internal(this.m_Handle, (int)this.addressFamily, out num);
				if (num != 0)
				{
					throw new SocketException(num);
				}
				return this.seed_endpoint.Create(socketAddress);
			}
		}

		// Token: 0x06003D99 RID: 15769 RVA: 0x000D1E94 File Offset: 0x000D0094
		private static SocketAddress LocalEndPoint_internal(SafeSocketHandle safeHandle, int family, out int error)
		{
			bool flag = false;
			SocketAddress socketAddress;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				socketAddress = Socket.LocalEndPoint_icall(safeHandle.DangerousGetHandle(), family, out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
			return socketAddress;
		}

		// Token: 0x06003D9A RID: 15770
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SocketAddress LocalEndPoint_icall(IntPtr socket, int family, out int error);

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Net.Sockets.Socket" /> is in blocking mode.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Net.Sockets.Socket" /> will block; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x06003D9B RID: 15771 RVA: 0x000D1ED8 File Offset: 0x000D00D8
		// (set) Token: 0x06003D9C RID: 15772 RVA: 0x000D1EE0 File Offset: 0x000D00E0
		public bool Blocking
		{
			get
			{
				return this.is_blocking;
			}
			set
			{
				this.ThrowIfDisposedAndClosed();
				int num;
				Socket.Blocking_internal(this.m_Handle, value, out num);
				if (num != 0)
				{
					throw new SocketException(num);
				}
				this.is_blocking = value;
			}
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x000D1F14 File Offset: 0x000D0114
		private static void Blocking_internal(SafeSocketHandle safeHandle, bool block, out int error)
		{
			bool flag = false;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				Socket.Blocking_icall(safeHandle.DangerousGetHandle(), block, out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
		}

		// Token: 0x06003D9E RID: 15774
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Blocking_icall(IntPtr socket, bool block, out int error);

		/// <summary>Gets a value that indicates whether a <see cref="T:System.Net.Sockets.Socket" /> is connected to a remote host as of the last <see cref="Overload:System.Net.Sockets.Socket.Send" /> or <see cref="Overload:System.Net.Sockets.Socket.Receive" /> operation.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Net.Sockets.Socket" /> was connected to a remote resource as of the most recent operation; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x06003D9F RID: 15775 RVA: 0x000D1F54 File Offset: 0x000D0154
		// (set) Token: 0x06003DA0 RID: 15776 RVA: 0x000D1F5C File Offset: 0x000D015C
		public bool Connected
		{
			get
			{
				return this.is_connected;
			}
			internal set
			{
				this.is_connected = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that specifies whether the stream <see cref="T:System.Net.Sockets.Socket" /> is using the Nagle algorithm.</summary>
		/// <returns>
		///   <see langword="false" /> if the <see cref="T:System.Net.Sockets.Socket" /> uses the Nagle algorithm; otherwise, <see langword="true" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the <see cref="T:System.Net.Sockets.Socket" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x06003DA1 RID: 15777 RVA: 0x000D1F65 File Offset: 0x000D0165
		// (set) Token: 0x06003DA2 RID: 15778 RVA: 0x000D1F83 File Offset: 0x000D0183
		public bool NoDelay
		{
			get
			{
				this.ThrowIfDisposedAndClosed();
				this.ThrowIfUdp();
				return (int)this.GetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug) != 0;
			}
			set
			{
				this.ThrowIfDisposedAndClosed();
				this.ThrowIfUdp();
				this.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, value ? 1 : 0);
			}
		}

		/// <summary>Gets the remote endpoint.</summary>
		/// <returns>The <see cref="T:System.Net.EndPoint" /> with which the <see cref="T:System.Net.Sockets.Socket" /> is communicating.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x06003DA3 RID: 15779 RVA: 0x000D1FA0 File Offset: 0x000D01A0
		public EndPoint RemoteEndPoint
		{
			get
			{
				this.ThrowIfDisposedAndClosed();
				if (!this.is_connected || this.seed_endpoint == null)
				{
					return null;
				}
				int num;
				SocketAddress socketAddress = Socket.RemoteEndPoint_internal(this.m_Handle, (int)this.addressFamily, out num);
				if (num != 0)
				{
					throw new SocketException(num);
				}
				return this.seed_endpoint.Create(socketAddress);
			}
		}

		// Token: 0x06003DA4 RID: 15780 RVA: 0x000D1FF0 File Offset: 0x000D01F0
		private static SocketAddress RemoteEndPoint_internal(SafeSocketHandle safeHandle, int family, out int error)
		{
			bool flag = false;
			SocketAddress socketAddress;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				socketAddress = Socket.RemoteEndPoint_icall(safeHandle.DangerousGetHandle(), family, out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
			return socketAddress;
		}

		// Token: 0x06003DA5 RID: 15781
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SocketAddress RemoteEndPoint_icall(IntPtr socket, int family, out int error);

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x06003DA6 RID: 15782 RVA: 0x000D2034 File Offset: 0x000D0234
		internal SafeHandle SafeHandle
		{
			get
			{
				return this.m_Handle;
			}
		}

		/// <summary>Determines the status of one or more sockets.</summary>
		/// <param name="checkRead">An <see cref="T:System.Collections.IList" /> of <see cref="T:System.Net.Sockets.Socket" /> instances to check for readability.</param>
		/// <param name="checkWrite">An <see cref="T:System.Collections.IList" /> of <see cref="T:System.Net.Sockets.Socket" /> instances to check for writability.</param>
		/// <param name="checkError">An <see cref="T:System.Collections.IList" /> of <see cref="T:System.Net.Sockets.Socket" /> instances to check for errors.</param>
		/// <param name="microSeconds">The time-out value, in microseconds. A -1 value indicates an infinite time-out.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="checkRead" /> parameter is <see langword="null" /> or empty.  
		///  -and-  
		///  The <paramref name="checkWrite" /> parameter is <see langword="null" /> or empty  
		///  -and-  
		///  The <paramref name="checkError" /> parameter is <see langword="null" /> or empty.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		// Token: 0x06003DA7 RID: 15783 RVA: 0x000D203C File Offset: 0x000D023C
		public static void Select(IList checkRead, IList checkWrite, IList checkError, int microSeconds)
		{
			List<Socket> list = new List<Socket>();
			Socket.AddSockets(list, checkRead, "checkRead");
			Socket.AddSockets(list, checkWrite, "checkWrite");
			Socket.AddSockets(list, checkError, "checkError");
			if (list.Count == 3)
			{
				throw new ArgumentNullException("checkRead, checkWrite, checkError", "All the lists are null or empty.");
			}
			Socket[] array = list.ToArray();
			int num;
			Socket.Select_icall(ref array, microSeconds, out num);
			if (num != 0)
			{
				throw new SocketException(num);
			}
			if (array == null)
			{
				if (checkRead != null)
				{
					checkRead.Clear();
				}
				if (checkWrite != null)
				{
					checkWrite.Clear();
				}
				if (checkError != null)
				{
					checkError.Clear();
				}
				return;
			}
			int num2 = 0;
			int num3 = array.Length;
			IList list2 = checkRead;
			int num4 = 0;
			for (int i = 0; i < num3; i++)
			{
				Socket socket = array[i];
				if (socket == null)
				{
					if (list2 != null)
					{
						int num5 = list2.Count - num4;
						for (int j = 0; j < num5; j++)
						{
							list2.RemoveAt(num4);
						}
					}
					list2 = ((num2 == 0) ? checkWrite : checkError);
					num4 = 0;
					num2++;
				}
				else
				{
					if (num2 == 1 && list2 == checkWrite && !socket.is_connected && (int)socket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Error) == 0)
					{
						socket.is_connected = true;
					}
					while ((Socket)list2[num4] != socket)
					{
						list2.RemoveAt(num4);
					}
					num4++;
				}
			}
		}

		// Token: 0x06003DA8 RID: 15784 RVA: 0x000D2184 File Offset: 0x000D0384
		private static void AddSockets(List<Socket> sockets, IList list, string name)
		{
			if (list != null)
			{
				foreach (object obj in list)
				{
					Socket socket = (Socket)obj;
					if (socket == null)
					{
						throw new ArgumentNullException(name, "Contains a null element");
					}
					sockets.Add(socket);
				}
			}
			sockets.Add(null);
		}

		// Token: 0x06003DA9 RID: 15785
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Select_icall(ref Socket[] sockets, int microSeconds, out int error);

		/// <summary>Determines the status of the <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <param name="microSeconds">The time to wait for a response, in microseconds.</param>
		/// <param name="mode">One of the <see cref="T:System.Net.Sockets.SelectMode" /> values.</param>
		/// <returns>The status of the <see cref="T:System.Net.Sockets.Socket" /> based on the polling mode value passed in the <paramref name="mode" /> parameter.  
		///   Mode  
		///
		///   Return Value  
		///
		///  <see cref="F:System.Net.Sockets.SelectMode.SelectRead" /><see langword="true" /> if <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" /> has been called and a connection is pending;  
		///
		///  -or-  
		///
		///  <see langword="true" /> if data is available for reading;  
		///
		///  -or-  
		///
		///  <see langword="true" /> if the connection has been closed, reset, or terminated;  
		///
		///  otherwise, returns <see langword="false" />.  
		///
		///  <see cref="F:System.Net.Sockets.SelectMode.SelectWrite" /><see langword="true" />, if processing a <see cref="M:System.Net.Sockets.Socket.Connect(System.Net.EndPoint)" />, and the connection has succeeded;  
		///
		///  -or-  
		///
		///  <see langword="true" /> if data can be sent;  
		///
		///  otherwise, returns <see langword="false" />.  
		///
		///  <see cref="F:System.Net.Sockets.SelectMode.SelectError" /><see langword="true" /> if processing a <see cref="M:System.Net.Sockets.Socket.Connect(System.Net.EndPoint)" /> that does not block, and the connection has failed;  
		///
		///  -or-  
		///
		///  <see langword="true" /> if <see cref="F:System.Net.Sockets.SocketOptionName.OutOfBandInline" /> is not set and out-of-band data is available;  
		///
		///  otherwise, returns <see langword="false" />.</returns>
		/// <exception cref="T:System.NotSupportedException">The <paramref name="mode" /> parameter is not one of the <see cref="T:System.Net.Sockets.SelectMode" /> values.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See remarks below.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003DAA RID: 15786 RVA: 0x000D21F4 File Offset: 0x000D03F4
		public bool Poll(int microSeconds, SelectMode mode)
		{
			this.ThrowIfDisposedAndClosed();
			if (mode != SelectMode.SelectRead && mode != SelectMode.SelectWrite && mode != SelectMode.SelectError)
			{
				throw new NotSupportedException("'mode' parameter is not valid.");
			}
			int num;
			bool flag = Socket.Poll_internal(this.m_Handle, mode, microSeconds, out num);
			if (num != 0)
			{
				throw new SocketException(num);
			}
			if (mode == SelectMode.SelectWrite && flag && !this.is_connected && (int)this.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Error) == 0)
			{
				this.is_connected = true;
			}
			return flag;
		}

		// Token: 0x06003DAB RID: 15787 RVA: 0x000D2268 File Offset: 0x000D0468
		private static bool Poll_internal(SafeSocketHandle safeHandle, SelectMode mode, int timeout, out int error)
		{
			bool flag = false;
			bool flag2;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				flag2 = Socket.Poll_icall(safeHandle.DangerousGetHandle(), mode, timeout, out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
			return flag2;
		}

		// Token: 0x06003DAC RID: 15788
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Poll_icall(IntPtr socket, SelectMode mode, int timeout, out int error);

		/// <summary>Creates a new <see cref="T:System.Net.Sockets.Socket" /> for a newly created connection.</summary>
		/// <returns>A <see cref="T:System.Net.Sockets.Socket" /> for a newly created connection.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The accepting socket is not listening for connections. You must call <see cref="M:System.Net.Sockets.Socket.Bind(System.Net.EndPoint)" /> and <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" /> before calling <see cref="M:System.Net.Sockets.Socket.Accept" />.</exception>
		// Token: 0x06003DAD RID: 15789 RVA: 0x000D22AC File Offset: 0x000D04AC
		public Socket Accept()
		{
			this.ThrowIfDisposedAndClosed();
			int num = 0;
			SafeSocketHandle safeSocketHandle = Socket.Accept_internal(this.m_Handle, out num, this.is_blocking);
			if (num != 0)
			{
				if (this.is_closed)
				{
					num = 10004;
				}
				throw new SocketException(num);
			}
			return new Socket(this.AddressFamily, this.SocketType, this.ProtocolType, safeSocketHandle)
			{
				seed_endpoint = this.seed_endpoint,
				Blocking = this.Blocking
			};
		}

		// Token: 0x06003DAE RID: 15790 RVA: 0x000D2320 File Offset: 0x000D0520
		internal void Accept(Socket acceptSocket)
		{
			this.ThrowIfDisposedAndClosed();
			int num = 0;
			SafeSocketHandle safeSocketHandle = Socket.Accept_internal(this.m_Handle, out num, this.is_blocking);
			if (num != 0)
			{
				if (this.is_closed)
				{
					num = 10004;
				}
				throw new SocketException(num);
			}
			acceptSocket.addressFamily = this.AddressFamily;
			acceptSocket.socketType = this.SocketType;
			acceptSocket.protocolType = this.ProtocolType;
			acceptSocket.m_Handle = safeSocketHandle;
			acceptSocket.is_connected = true;
			acceptSocket.seed_endpoint = this.seed_endpoint;
			acceptSocket.Blocking = this.Blocking;
		}

		/// <summary>Begins an asynchronous operation to accept an incoming connection attempt.</summary>
		/// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
		/// <returns>
		///   <see langword="true" /> if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation.  
		/// <see langword="false" /> if the I/O operation completed synchronously. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
		/// <exception cref="T:System.ArgumentException">An argument is not valid. This exception occurs if the buffer provided is not large enough. The buffer must be at least 2 * (sizeof(SOCKADDR_STORAGE + 16) bytes.  
		///  This exception also occurs if multiple buffers are specified, the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.BufferList" /> property is not null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">An argument is out of range. The exception occurs if the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Count" /> is less than 0.</exception>
		/// <exception cref="T:System.InvalidOperationException">An invalid operation was requested. This exception occurs if the accepting <see cref="T:System.Net.Sockets.Socket" /> is not listening for connections or the accepted socket is bound.  
		///  You must call the <see cref="M:System.Net.Sockets.Socket.Bind(System.Net.EndPoint)" /> and <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" /> method before calling the <see cref="M:System.Net.Sockets.Socket.AcceptAsync(System.Net.Sockets.SocketAsyncEventArgs)" /> method.  
		///  This exception also occurs if the socket is already connected or a socket operation was already in progress using the specified <paramref name="e" /> parameter.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows XP or later is required for this method.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003DAF RID: 15791 RVA: 0x000D23AC File Offset: 0x000D05AC
		public bool AcceptAsync(SocketAsyncEventArgs e)
		{
			this.ThrowIfDisposedAndClosed();
			if (!this.is_bound)
			{
				throw new InvalidOperationException("You must call the Bind method before performing this operation.");
			}
			if (!this.is_listening)
			{
				throw new InvalidOperationException("You must call the Listen method before performing this operation.");
			}
			if (e.BufferList != null)
			{
				throw new ArgumentException("Multiple buffers cannot be used with this method.");
			}
			if (e.Count < 0)
			{
				throw new ArgumentOutOfRangeException("e.Count");
			}
			Socket acceptSocket = e.AcceptSocket;
			if (acceptSocket != null && (acceptSocket.is_bound || acceptSocket.is_connected))
			{
				throw new InvalidOperationException("AcceptSocket: The socket must not be bound or connected.");
			}
			this.InitSocketAsyncEventArgs(e, Socket.AcceptAsyncCallback, e, SocketOperation.Accept);
			this.QueueIOSelectorJob(this.ReadSem, e.socket_async_result.Handle, new IOSelectorJob(IOOperation.Read, Socket.BeginAcceptCallback, e.socket_async_result));
			return true;
		}

		/// <summary>Begins an asynchronous operation to accept an incoming connection attempt.</summary>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
		/// <param name="state">An object that contains state information for this request.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous <see cref="T:System.Net.Sockets.Socket" /> creation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows NT is required for this method.</exception>
		/// <exception cref="T:System.InvalidOperationException">The accepting socket is not listening for connections. You must call <see cref="M:System.Net.Sockets.Socket.Bind(System.Net.EndPoint)" /> and <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" /> before calling <see cref="M:System.Net.Sockets.Socket.BeginAccept(System.AsyncCallback,System.Object)" />.  
		///  -or-  
		///  The accepted socket is bound.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="receiveSize" /> is less than 0.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		// Token: 0x06003DB0 RID: 15792 RVA: 0x000D2468 File Offset: 0x000D0668
		public IAsyncResult BeginAccept(AsyncCallback callback, object state)
		{
			this.ThrowIfDisposedAndClosed();
			if (!this.is_bound || !this.is_listening)
			{
				throw new InvalidOperationException();
			}
			SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, callback, state, SocketOperation.Accept);
			this.QueueIOSelectorJob(this.ReadSem, socketAsyncResult.Handle, new IOSelectorJob(IOOperation.Read, Socket.BeginAcceptCallback, socketAsyncResult));
			return socketAsyncResult;
		}

		/// <summary>Begins an asynchronous operation to accept an incoming connection attempt from a specified socket and receives the first block of data sent by the client application.</summary>
		/// <param name="acceptSocket">The accepted <see cref="T:System.Net.Sockets.Socket" /> object. This value may be <see langword="null" />.</param>
		/// <param name="receiveSize">The maximum number of bytes to receive.</param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
		/// <param name="state">An object that contains state information for this request.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that references the asynchronous <see cref="T:System.Net.Sockets.Socket" /> object creation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows NT is required for this method.</exception>
		/// <exception cref="T:System.InvalidOperationException">The accepting socket is not listening for connections. You must call <see cref="M:System.Net.Sockets.Socket.Bind(System.Net.EndPoint)" /> and <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" /> before calling <see cref="M:System.Net.Sockets.Socket.BeginAccept(System.AsyncCallback,System.Object)" />.  
		///  -or-  
		///  The accepted socket is bound.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="receiveSize" /> is less than 0.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		// Token: 0x06003DB1 RID: 15793 RVA: 0x000D24BC File Offset: 0x000D06BC
		public IAsyncResult BeginAccept(Socket acceptSocket, int receiveSize, AsyncCallback callback, object state)
		{
			this.ThrowIfDisposedAndClosed();
			if (receiveSize < 0)
			{
				throw new ArgumentOutOfRangeException("receiveSize", "receiveSize is less than zero");
			}
			if (acceptSocket != null)
			{
				this.ThrowIfDisposedAndClosed(acceptSocket);
				if (acceptSocket.IsBound)
				{
					throw new InvalidOperationException();
				}
				if (acceptSocket.ProtocolType != ProtocolType.Tcp)
				{
					throw new SocketException(10022);
				}
			}
			SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, callback, state, SocketOperation.AcceptReceive)
			{
				Buffer = new byte[receiveSize],
				Offset = 0,
				Size = receiveSize,
				SockFlags = SocketFlags.None,
				AcceptSocket = acceptSocket
			};
			this.QueueIOSelectorJob(this.ReadSem, socketAsyncResult.Handle, new IOSelectorJob(IOOperation.Read, Socket.BeginAcceptReceiveCallback, socketAsyncResult));
			return socketAsyncResult;
		}

		/// <summary>Asynchronously accepts an incoming connection attempt and creates a new <see cref="T:System.Net.Sockets.Socket" /> to handle remote host communication.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information for this asynchronous operation as well as any user defined data.</param>
		/// <returns>A <see cref="T:System.Net.Sockets.Socket" /> to handle communication with the remote host.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not created by a call to <see cref="M:System.Net.Sockets.Socket.BeginAccept(System.AsyncCallback,System.Object)" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndAccept(System.IAsyncResult)" /> method was previously called.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows NT is required for this method.</exception>
		// Token: 0x06003DB2 RID: 15794 RVA: 0x000D2568 File Offset: 0x000D0768
		public Socket EndAccept(IAsyncResult asyncResult)
		{
			byte[] array;
			int num;
			return this.EndAccept(out array, out num, asyncResult);
		}

		/// <summary>Asynchronously accepts an incoming connection attempt and creates a new <see cref="T:System.Net.Sockets.Socket" /> object to handle remote host communication. This method returns a buffer that contains the initial data and the number of bytes transferred.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the bytes transferred.</param>
		/// <param name="bytesTransferred">The number of bytes transferred.</param>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> object that stores state information for this asynchronous operation as well as any user defined data.</param>
		/// <returns>A <see cref="T:System.Net.Sockets.Socket" /> object to handle communication with the remote host.</returns>
		/// <exception cref="T:System.NotSupportedException">Windows NT is required for this method.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is empty.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not created by a call to <see cref="M:System.Net.Sockets.Socket.BeginAccept(System.AsyncCallback,System.Object)" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndAccept(System.IAsyncResult)" /> method was previously called.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the <see cref="T:System.Net.Sockets.Socket" />.</exception>
		// Token: 0x06003DB3 RID: 15795 RVA: 0x000D2580 File Offset: 0x000D0780
		public Socket EndAccept(out byte[] buffer, out int bytesTransferred, IAsyncResult asyncResult)
		{
			this.ThrowIfDisposedAndClosed();
			SocketAsyncResult socketAsyncResult = this.ValidateEndIAsyncResult(asyncResult, "EndAccept", "asyncResult");
			if (!socketAsyncResult.IsCompleted)
			{
				socketAsyncResult.AsyncWaitHandle.WaitOne();
			}
			socketAsyncResult.CheckIfThrowDelayedException();
			buffer = socketAsyncResult.Buffer.ToArray();
			bytesTransferred = socketAsyncResult.Total;
			return socketAsyncResult.AcceptedSocket;
		}

		// Token: 0x06003DB4 RID: 15796 RVA: 0x000D25DC File Offset: 0x000D07DC
		private static SafeSocketHandle Accept_internal(SafeSocketHandle safeHandle, out int error, bool blocking)
		{
			SafeSocketHandle safeSocketHandle;
			try
			{
				safeHandle.RegisterForBlockingSyscall();
				safeSocketHandle = new SafeSocketHandle(Socket.Accept_icall(safeHandle.DangerousGetHandle(), out error, blocking), true);
			}
			finally
			{
				safeHandle.UnRegisterForBlockingSyscall();
			}
			return safeSocketHandle;
		}

		// Token: 0x06003DB5 RID: 15797
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Accept_icall(IntPtr sock, out int error, bool blocking);

		/// <summary>Associates a <see cref="T:System.Net.Sockets.Socket" /> with a local endpoint.</summary>
		/// <param name="localEP">The local <see cref="T:System.Net.EndPoint" /> to associate with the <see cref="T:System.Net.Sockets.Socket" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="localEP" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Security.SecurityException">A caller higher in the call stack does not have permission for the requested operation.</exception>
		// Token: 0x06003DB6 RID: 15798 RVA: 0x000D2620 File Offset: 0x000D0820
		public void Bind(EndPoint localEP)
		{
			this.ThrowIfDisposedAndClosed();
			if (localEP == null)
			{
				throw new ArgumentNullException("localEP");
			}
			IPEndPoint ipendPoint = localEP as IPEndPoint;
			if (ipendPoint != null)
			{
				localEP = this.RemapIPEndPoint(ipendPoint);
			}
			int num;
			Socket.Bind_internal(this.m_Handle, localEP.Serialize(), out num);
			if (num != 0)
			{
				throw new SocketException(num);
			}
			if (num == 0)
			{
				this.is_bound = true;
			}
			this.seed_endpoint = localEP;
		}

		// Token: 0x06003DB7 RID: 15799 RVA: 0x000D2684 File Offset: 0x000D0884
		private static void Bind_internal(SafeSocketHandle safeHandle, SocketAddress sa, out int error)
		{
			bool flag = false;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				Socket.Bind_icall(safeHandle.DangerousGetHandle(), sa, out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
		}

		// Token: 0x06003DB8 RID: 15800
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Bind_icall(IntPtr sock, SocketAddress sa, out int error);

		/// <summary>Places a <see cref="T:System.Net.Sockets.Socket" /> in a listening state.</summary>
		/// <param name="backlog">The maximum length of the pending connections queue.</param>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003DB9 RID: 15801 RVA: 0x000D26C4 File Offset: 0x000D08C4
		public void Listen(int backlog)
		{
			this.ThrowIfDisposedAndClosed();
			if (!this.is_bound)
			{
				throw new SocketException(10022);
			}
			int num;
			Socket.Listen_internal(this.m_Handle, backlog, out num);
			if (num != 0)
			{
				throw new SocketException(num);
			}
			this.is_listening = true;
		}

		// Token: 0x06003DBA RID: 15802 RVA: 0x000D270C File Offset: 0x000D090C
		private static void Listen_internal(SafeSocketHandle safeHandle, int backlog, out int error)
		{
			bool flag = false;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				Socket.Listen_icall(safeHandle.DangerousGetHandle(), backlog, out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
		}

		// Token: 0x06003DBB RID: 15803
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Listen_icall(IntPtr sock, int backlog, out int error);

		/// <summary>Establishes a connection to a remote host. The host is specified by an IP address and a port number.</summary>
		/// <param name="address">The IP address of the remote host.</param>
		/// <param name="port">The port number of the remote host.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="address" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The port number is not valid.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.NotSupportedException">This method is valid for sockets in the <see cref="F:System.Net.Sockets.AddressFamily.InterNetwork" /> or <see cref="F:System.Net.Sockets.AddressFamily.InterNetworkV6" /> families.</exception>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="address" /> is zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> has been placed in a listening state by calling <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" />.</exception>
		// Token: 0x06003DBC RID: 15804 RVA: 0x000D274C File Offset: 0x000D094C
		public void Connect(IPAddress address, int port)
		{
			this.Connect(new IPEndPoint(address, port));
		}

		/// <summary>Establishes a connection to a remote host. The host is specified by a host name and a port number.</summary>
		/// <param name="host">The name of the remote host.</param>
		/// <param name="port">The port number of the remote host.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="host" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The port number is not valid.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.NotSupportedException">This method is valid for sockets in the <see cref="F:System.Net.Sockets.AddressFamily.InterNetwork" /> or <see cref="F:System.Net.Sockets.AddressFamily.InterNetworkV6" /> families.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> has been placed in a listening state by calling <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" />.</exception>
		// Token: 0x06003DBD RID: 15805 RVA: 0x000D275B File Offset: 0x000D095B
		public void Connect(string host, int port)
		{
			this.Connect(Dns.GetHostAddresses(host), port);
		}

		/// <summary>Establishes a connection to a remote host.</summary>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" /> that represents the remote device.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="remoteEP" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Security.SecurityException">A caller higher in the call stack does not have permission for the requested operation.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> has been placed in a listening state by calling <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" />.</exception>
		// Token: 0x06003DBE RID: 15806 RVA: 0x000D276C File Offset: 0x000D096C
		public void Connect(EndPoint remoteEP)
		{
			this.ThrowIfDisposedAndClosed();
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			IPEndPoint ipendPoint = remoteEP as IPEndPoint;
			if (ipendPoint != null && this.socketType != SocketType.Dgram && (ipendPoint.Address.Equals(IPAddress.Any) || ipendPoint.Address.Equals(IPAddress.IPv6Any)))
			{
				throw new SocketException(10049);
			}
			if (this.is_listening)
			{
				throw new InvalidOperationException();
			}
			if (ipendPoint != null)
			{
				remoteEP = this.RemapIPEndPoint(ipendPoint);
			}
			SocketAddress socketAddress = remoteEP.Serialize();
			int num = 0;
			Socket.Connect_internal(this.m_Handle, socketAddress, out num, this.is_blocking);
			if (num == 0 || num == 10035)
			{
				this.seed_endpoint = remoteEP;
			}
			if (num != 0)
			{
				if (this.is_closed)
				{
					num = 10004;
				}
				throw new SocketException(num);
			}
			this.is_connected = this.socketType != SocketType.Dgram || ipendPoint == null || (!ipendPoint.Address.Equals(IPAddress.Any) && !ipendPoint.Address.Equals(IPAddress.IPv6Any));
			this.is_bound = true;
		}

		/// <summary>Begins an asynchronous request for a connection to a remote host.</summary>
		/// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
		/// <returns>
		///   <see langword="true" /> if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation.  
		/// <see langword="false" /> if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
		/// <exception cref="T:System.ArgumentException">An argument is not valid. This exception occurs if multiple buffers are specified, the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.BufferList" /> property is not null.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="e" /> parameter cannot be null and the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.RemoteEndPoint" /> cannot be null.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> is listening or a socket operation was already in progress using the <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object specified in the <paramref name="e" /> parameter.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows XP or later is required for this method. This exception also occurs if the local endpoint and the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.RemoteEndPoint" /> are not the same address family.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Security.SecurityException">A caller higher in the call stack does not have permission for the requested operation.</exception>
		// Token: 0x06003DBF RID: 15807 RVA: 0x000D2874 File Offset: 0x000D0A74
		public bool ConnectAsync(SocketAsyncEventArgs e)
		{
			this.ThrowIfDisposedAndClosed();
			if (this.is_listening)
			{
				throw new InvalidOperationException("You may not perform this operation after calling the Listen method.");
			}
			if (e.RemoteEndPoint == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			this.InitSocketAsyncEventArgs(e, null, e, SocketOperation.Connect);
			bool flag2;
			try
			{
				IPAddress[] array;
				SocketAsyncResult socketAsyncResult;
				bool flag;
				if (!this.GetCheckedIPs(e, out array))
				{
					socketAsyncResult = new SocketAsyncResult(this, Socket.ConnectAsyncCallback, e, SocketOperation.Connect)
					{
						EndPoint = e.RemoteEndPoint
					};
					flag = Socket.BeginSConnect(socketAsyncResult);
				}
				else
				{
					DnsEndPoint dnsEndPoint = (DnsEndPoint)e.RemoteEndPoint;
					if (array == null)
					{
						throw new ArgumentNullException("addresses");
					}
					if (array.Length == 0)
					{
						throw new ArgumentException("Empty addresses list");
					}
					if (this.AddressFamily != AddressFamily.InterNetwork && this.AddressFamily != AddressFamily.InterNetworkV6)
					{
						throw new NotSupportedException("This method is only valid for addresses in the InterNetwork or InterNetworkV6 families");
					}
					if (dnsEndPoint.Port <= 0 || dnsEndPoint.Port > 65535)
					{
						throw new ArgumentOutOfRangeException("port", "Must be > 0 and < 65536");
					}
					socketAsyncResult = new SocketAsyncResult(this, Socket.ConnectAsyncCallback, e, SocketOperation.Connect)
					{
						Addresses = array,
						Port = dnsEndPoint.Port
					};
					this.is_connected = false;
					flag = Socket.BeginMConnect(socketAsyncResult);
				}
				if (!flag)
				{
					e.CurrentSocket.EndConnect(socketAsyncResult);
				}
				flag2 = flag;
			}
			catch (SocketException ex)
			{
				e.SocketError = ex.SocketErrorCode;
				e.socket_async_result.Complete(ex, true);
				flag2 = false;
			}
			catch (Exception ex2)
			{
				e.socket_async_result.Complete(ex2, true);
				flag2 = false;
			}
			return flag2;
		}

		/// <summary>Cancels an asynchronous request for a remote host connection.</summary>
		/// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object used to request the connection to the remote host by calling one of the <see cref="M:System.Net.Sockets.Socket.ConnectAsync(System.Net.Sockets.SocketType,System.Net.Sockets.ProtocolType,System.Net.Sockets.SocketAsyncEventArgs)" /> methods.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="e" /> parameter cannot be null and the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.RemoteEndPoint" /> cannot be null.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Security.SecurityException">A caller higher in the call stack does not have permission for the requested operation.</exception>
		// Token: 0x06003DC0 RID: 15808 RVA: 0x000D29F0 File Offset: 0x000D0BF0
		public static void CancelConnectAsync(SocketAsyncEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (e.in_progress != 0 && e.LastOperation == SocketAsyncOperation.Connect)
			{
				Socket currentSocket = e.CurrentSocket;
				if (currentSocket == null)
				{
					return;
				}
				currentSocket.Close();
			}
		}

		/// <summary>Begins an asynchronous request for a remote host connection. The host is specified by a host name and a port number.</summary>
		/// <param name="host">The name of the remote host.</param>
		/// <param name="port">The port number of the remote host.</param>
		/// <param name="requestCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the connect operation is complete.</param>
		/// <param name="state">A user-defined object that contains information about the connect operation. This object is passed to the <paramref name="requestCallback" /> delegate when the operation is complete.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous connection.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="host" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.NotSupportedException">This method is valid for sockets in the <see cref="F:System.Net.Sockets.AddressFamily.InterNetwork" /> or <see cref="F:System.Net.Sockets.AddressFamily.InterNetworkV6" /> families.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The port number is not valid.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> has been placed in a listening state by calling <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" />, or an asynchronous operation is already in progress.</exception>
		// Token: 0x06003DC1 RID: 15809 RVA: 0x000D2A24 File Offset: 0x000D0C24
		public IAsyncResult BeginConnect(string host, int port, AsyncCallback callback, object state)
		{
			this.ThrowIfDisposedAndClosed();
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (this.addressFamily != AddressFamily.InterNetwork && this.addressFamily != AddressFamily.InterNetworkV6)
			{
				throw new NotSupportedException("This method is valid only for sockets in the InterNetwork and InterNetworkV6 families");
			}
			if (port <= 0 || port > 65535)
			{
				throw new ArgumentOutOfRangeException("port", "Must be > 0 and < 65536");
			}
			if (this.is_listening)
			{
				throw new InvalidOperationException();
			}
			SocketAsyncResult sockares = new SocketAsyncResult(this, callback, state, SocketOperation.Connect)
			{
				Port = port
			};
			Dns.GetHostAddressesAsync(host).ContinueWith(delegate(Task<IPAddress[]> t)
			{
				if (t.IsFaulted)
				{
					sockares.Complete(t.Exception.InnerException);
					return;
				}
				if (t.IsCanceled)
				{
					sockares.Complete(new OperationCanceledException());
					return;
				}
				sockares.Addresses = t.Result;
				Socket.BeginMConnect(sockares);
			}, TaskScheduler.Default);
			return sockares;
		}

		/// <summary>Begins an asynchronous request for a remote host connection.</summary>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" /> that represents the remote host.</param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
		/// <param name="state">An object that contains state information for this request.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous connection.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="remoteEP" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Security.SecurityException">A caller higher in the call stack does not have permission for the requested operation.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> has been placed in a listening state by calling <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" />, or an asynchronous operation is already in progress.</exception>
		// Token: 0x06003DC2 RID: 15810 RVA: 0x000D2ACD File Offset: 0x000D0CCD
		public IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
		{
			this.ThrowIfDisposedAndClosed();
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			if (this.is_listening)
			{
				throw new InvalidOperationException();
			}
			SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, callback, state, SocketOperation.Connect);
			socketAsyncResult.EndPoint = remoteEP;
			Socket.BeginSConnect(socketAsyncResult);
			return socketAsyncResult;
		}

		/// <summary>Begins an asynchronous request for a remote host connection. The host is specified by an <see cref="T:System.Net.IPAddress" /> array and a port number.</summary>
		/// <param name="addresses">At least one <see cref="T:System.Net.IPAddress" />, designating the remote host.</param>
		/// <param name="port">The port number of the remote host.</param>
		/// <param name="requestCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the connect operation is complete.</param>
		/// <param name="state">A user-defined object that contains information about the connect operation. This object is passed to the <paramref name="requestCallback" /> delegate when the operation is complete.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous connections.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="addresses" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.NotSupportedException">This method is valid for sockets that use <see cref="F:System.Net.Sockets.AddressFamily.InterNetwork" /> or <see cref="F:System.Net.Sockets.AddressFamily.InterNetworkV6" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The port number is not valid.</exception>
		/// <exception cref="T:System.ArgumentException">The length of <paramref name="address" /> is zero.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> has been placed in a listening state by calling <see cref="M:System.Net.Sockets.Socket.Listen(System.Int32)" />, or an asynchronous operation is already in progress.</exception>
		// Token: 0x06003DC3 RID: 15811 RVA: 0x000D2B08 File Offset: 0x000D0D08
		public IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback requestCallback, object state)
		{
			this.ThrowIfDisposedAndClosed();
			if (addresses == null)
			{
				throw new ArgumentNullException("addresses");
			}
			if (addresses.Length == 0)
			{
				throw new ArgumentException("Empty addresses list");
			}
			if (this.AddressFamily != AddressFamily.InterNetwork && this.AddressFamily != AddressFamily.InterNetworkV6)
			{
				throw new NotSupportedException("This method is only valid for addresses in the InterNetwork or InterNetworkV6 families");
			}
			if (port <= 0 || port > 65535)
			{
				throw new ArgumentOutOfRangeException("port", "Must be > 0 and < 65536");
			}
			if (this.is_listening)
			{
				throw new InvalidOperationException();
			}
			SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, requestCallback, state, SocketOperation.Connect);
			socketAsyncResult.Addresses = addresses;
			socketAsyncResult.Port = port;
			this.is_connected = false;
			Socket.BeginMConnect(socketAsyncResult);
			return socketAsyncResult;
		}

		// Token: 0x06003DC4 RID: 15812 RVA: 0x000D2BA8 File Offset: 0x000D0DA8
		private static bool BeginMConnect(SocketAsyncResult sockares)
		{
			Exception ex = null;
			for (int i = sockares.CurrentAddress; i < sockares.Addresses.Length; i++)
			{
				try
				{
					sockares.CurrentAddress++;
					sockares.EndPoint = new IPEndPoint(sockares.Addresses[i], sockares.Port);
					if (sockares.socket.CanTryAddressFamily(sockares.EndPoint.AddressFamily))
					{
						return Socket.BeginSConnect(sockares);
					}
				}
				catch (Exception ex)
				{
				}
			}
			sockares.Complete(ex, true);
			return false;
		}

		// Token: 0x06003DC5 RID: 15813 RVA: 0x000D2C38 File Offset: 0x000D0E38
		private static bool BeginSConnect(SocketAsyncResult sockares)
		{
			EndPoint endPoint = sockares.EndPoint;
			if (endPoint is IPEndPoint)
			{
				IPEndPoint ipendPoint = (IPEndPoint)endPoint;
				if (ipendPoint.Address.Equals(IPAddress.Any) || ipendPoint.Address.Equals(IPAddress.IPv6Any))
				{
					sockares.Complete(new SocketException(10049), true);
					return false;
				}
				endPoint = (sockares.EndPoint = sockares.socket.RemapIPEndPoint(ipendPoint));
			}
			if (!sockares.socket.CanTryAddressFamily(sockares.EndPoint.AddressFamily))
			{
				sockares.Complete(new ArgumentException("None of the discovered or specified addresses match the socket address family."), true);
				return false;
			}
			int num = 0;
			if (sockares.socket.connect_in_progress)
			{
				sockares.socket.connect_in_progress = false;
				sockares.socket.m_Handle.Dispose();
				sockares.socket.m_Handle = new SafeSocketHandle(Socket.Socket_icall(sockares.socket.addressFamily, sockares.socket.socketType, sockares.socket.protocolType, out num), true);
				if (num != 0)
				{
					sockares.Complete(new SocketException(num), true);
					return false;
				}
			}
			bool flag = sockares.socket.is_blocking;
			if (flag)
			{
				sockares.socket.Blocking = false;
			}
			Socket.Connect_internal(sockares.socket.m_Handle, endPoint.Serialize(), out num, false);
			if (flag)
			{
				sockares.socket.Blocking = true;
			}
			if (num == 0)
			{
				sockares.socket.is_connected = true;
				sockares.socket.is_bound = true;
				sockares.Complete(true);
				return false;
			}
			if (num != 10036 && num != 10035)
			{
				sockares.socket.is_connected = false;
				sockares.socket.is_bound = false;
				sockares.Complete(new SocketException(num), true);
				return false;
			}
			sockares.socket.is_connected = false;
			sockares.socket.is_bound = false;
			sockares.socket.connect_in_progress = true;
			IOSelector.Add(sockares.Handle, new IOSelectorJob(IOOperation.Write, Socket.BeginConnectCallback, sockares));
			return true;
		}

		/// <summary>Ends a pending asynchronous connection request.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information and any user defined data for this asynchronous operation.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by a call to the <see cref="M:System.Net.Sockets.Socket.BeginConnect(System.Net.EndPoint,System.AsyncCallback,System.Object)" /> method.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndConnect(System.IAsyncResult)" /> was previously called for the asynchronous connection.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003DC6 RID: 15814 RVA: 0x000D2E20 File Offset: 0x000D1020
		public void EndConnect(IAsyncResult asyncResult)
		{
			this.ThrowIfDisposedAndClosed();
			SocketAsyncResult socketAsyncResult = this.ValidateEndIAsyncResult(asyncResult, "EndConnect", "asyncResult");
			if (!socketAsyncResult.IsCompleted)
			{
				socketAsyncResult.AsyncWaitHandle.WaitOne();
			}
			socketAsyncResult.CheckIfThrowDelayedException();
		}

		// Token: 0x06003DC7 RID: 15815 RVA: 0x000D2E60 File Offset: 0x000D1060
		private static void Connect_internal(SafeSocketHandle safeHandle, SocketAddress sa, out int error, bool blocking)
		{
			try
			{
				safeHandle.RegisterForBlockingSyscall();
				Socket.Connect_icall(safeHandle.DangerousGetHandle(), sa, out error, blocking);
			}
			finally
			{
				safeHandle.UnRegisterForBlockingSyscall();
			}
		}

		// Token: 0x06003DC8 RID: 15816
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Connect_icall(IntPtr sock, SocketAddress sa, out int error, bool blocking);

		// Token: 0x06003DC9 RID: 15817 RVA: 0x000D2E9C File Offset: 0x000D109C
		private bool GetCheckedIPs(SocketAsyncEventArgs e, out IPAddress[] addresses)
		{
			addresses = null;
			DnsEndPoint dnsEndPoint = e.RemoteEndPoint as DnsEndPoint;
			if (dnsEndPoint == null)
			{
				e.SetConnectByNameError(null);
				return false;
			}
			addresses = Dns.GetHostAddresses(dnsEndPoint.Host);
			if (dnsEndPoint.AddressFamily == AddressFamily.Unspecified)
			{
				return true;
			}
			int num = 0;
			for (int i = 0; i < addresses.Length; i++)
			{
				if (addresses[i].AddressFamily == dnsEndPoint.AddressFamily)
				{
					addresses[num++] = addresses[i];
				}
			}
			if (num != addresses.Length)
			{
				Array.Resize<IPAddress>(ref addresses, num);
			}
			return true;
		}

		/// <summary>Closes the socket connection and allows reuse of the socket.</summary>
		/// <param name="reuseSocket">
		///   <see langword="true" /> if this socket can be reused after the current connection is closed; otherwise, <see langword="false" />.</param>
		/// <exception cref="T:System.PlatformNotSupportedException">This method requires Windows 2000 or earlier, or the exception will be thrown.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		// Token: 0x06003DCA RID: 15818 RVA: 0x000D2F18 File Offset: 0x000D1118
		public void Disconnect(bool reuseSocket)
		{
			this.ThrowIfDisposedAndClosed();
			int num = 0;
			Socket.Disconnect_internal(this.m_Handle, reuseSocket, out num);
			if (num == 0)
			{
				this.is_connected = false;
				return;
			}
			if (num == 50)
			{
				throw new PlatformNotSupportedException();
			}
			throw new SocketException(num);
		}

		/// <summary>Begins an asynchronous request to disconnect from a remote endpoint.</summary>
		/// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
		/// <returns>
		///   <see langword="true" /> if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation.  
		/// <see langword="false" /> if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="e" /> parameter cannot be null.</exception>
		/// <exception cref="T:System.InvalidOperationException">A socket operation was already in progress using the <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object specified in the <paramref name="e" /> parameter.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows XP or later is required for this method.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		// Token: 0x06003DCB RID: 15819 RVA: 0x000D2F59 File Offset: 0x000D1159
		public bool DisconnectAsync(SocketAsyncEventArgs e)
		{
			this.ThrowIfDisposedAndClosed();
			this.InitSocketAsyncEventArgs(e, Socket.DisconnectAsyncCallback, e, SocketOperation.Disconnect);
			IOSelector.Add(e.socket_async_result.Handle, new IOSelectorJob(IOOperation.Write, Socket.BeginDisconnectCallback, e.socket_async_result));
			return true;
		}

		/// <summary>Begins an asynchronous request to disconnect from a remote endpoint.</summary>
		/// <param name="reuseSocket">
		///   <see langword="true" /> if this socket can be reused after the connection is closed; otherwise, <see langword="false" />.</param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
		/// <param name="state">An object that contains state information for this request.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that references the asynchronous operation.</returns>
		/// <exception cref="T:System.NotSupportedException">The operating system is Windows 2000 or earlier, and this method requires Windows XP.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		// Token: 0x06003DCC RID: 15820 RVA: 0x000D2F94 File Offset: 0x000D1194
		public IAsyncResult BeginDisconnect(bool reuseSocket, AsyncCallback callback, object state)
		{
			this.ThrowIfDisposedAndClosed();
			SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, callback, state, SocketOperation.Disconnect)
			{
				ReuseSocket = reuseSocket
			};
			IOSelector.Add(socketAsyncResult.Handle, new IOSelectorJob(IOOperation.Write, Socket.BeginDisconnectCallback, socketAsyncResult));
			return socketAsyncResult;
		}

		/// <summary>Ends a pending asynchronous disconnect request.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> object that stores state information and any user-defined data for this asynchronous operation.</param>
		/// <exception cref="T:System.NotSupportedException">The operating system is Windows 2000 or earlier, and this method requires Windows XP.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by a call to the <see cref="M:System.Net.Sockets.Socket.BeginDisconnect(System.Boolean,System.AsyncCallback,System.Object)" /> method.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndDisconnect(System.IAsyncResult)" /> was previously called for the asynchronous connection.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.Net.WebException">The disconnect request has timed out.</exception>
		// Token: 0x06003DCD RID: 15821 RVA: 0x000D2FD0 File Offset: 0x000D11D0
		public void EndDisconnect(IAsyncResult asyncResult)
		{
			this.ThrowIfDisposedAndClosed();
			SocketAsyncResult socketAsyncResult = this.ValidateEndIAsyncResult(asyncResult, "EndDisconnect", "asyncResult");
			if (!socketAsyncResult.IsCompleted)
			{
				socketAsyncResult.AsyncWaitHandle.WaitOne();
			}
			socketAsyncResult.CheckIfThrowDelayedException();
		}

		// Token: 0x06003DCE RID: 15822 RVA: 0x000D3010 File Offset: 0x000D1210
		private static void Disconnect_internal(SafeSocketHandle safeHandle, bool reuse, out int error)
		{
			bool flag = false;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				Socket.Disconnect_icall(safeHandle.DangerousGetHandle(), reuse, out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
		}

		// Token: 0x06003DCF RID: 15823
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Disconnect_icall(IntPtr sock, bool reuse, out int error);

		/// <summary>Receives data from a bound <see cref="T:System.Net.Sockets.Socket" /> into a receive buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data.</param>
		/// <param name="offset">The position in the <paramref name="buffer" /> parameter to store the received data.</param>
		/// <param name="size">The number of bytes to receive.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
		/// <returns>The number of bytes received.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.  
		/// -or-  
		/// <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.  
		/// -or-  
		/// <paramref name="size" /> is less than 0.  
		/// -or-  
		/// <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="socketFlags" /> is not a valid combination of values.  
		/// -or-  
		/// The <see cref="P:System.Net.Sockets.Socket.LocalEndPoint" /> property is not set.  
		/// -or-  
		/// An operating system error occurs while accessing the <see cref="T:System.Net.Sockets.Socket" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call stack does not have the required permissions.</exception>
		// Token: 0x06003DD0 RID: 15824 RVA: 0x000D3050 File Offset: 0x000D1250
		public unsafe int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
		{
			this.ThrowIfDisposedAndClosed();
			this.ThrowIfBufferNull(buffer);
			this.ThrowIfBufferOutOfRange(buffer, offset, size);
			int num2;
			int num;
			fixed (byte[] array = buffer)
			{
				byte* ptr;
				if (buffer == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				num = Socket.Receive_internal(this.m_Handle, ptr + offset, size, socketFlags, out num2, this.is_blocking);
			}
			errorCode = (SocketError)num2;
			if (errorCode != SocketError.Success && errorCode != SocketError.WouldBlock && errorCode != SocketError.InProgress)
			{
				this.is_connected = false;
				this.is_bound = false;
				return num;
			}
			this.is_connected = true;
			return num;
		}

		// Token: 0x06003DD1 RID: 15825 RVA: 0x000D30DC File Offset: 0x000D12DC
		private unsafe int Receive(Memory<byte> buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
		{
			this.ThrowIfDisposedAndClosed();
			int num2;
			int num;
			using (MemoryHandle memoryHandle = buffer.Slice(offset, size).Pin())
			{
				num = Socket.Receive_internal(this.m_Handle, (byte*)memoryHandle.Pointer, size, socketFlags, out num2, this.is_blocking);
			}
			errorCode = (SocketError)num2;
			if (errorCode != SocketError.Success && errorCode != SocketError.WouldBlock && errorCode != SocketError.InProgress)
			{
				this.is_connected = false;
				this.is_bound = false;
			}
			else
			{
				this.is_connected = true;
			}
			return num;
		}

		/// <summary>Receives data from a bound <see cref="T:System.Net.Sockets.Socket" /> into the list of receive buffers, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <param name="buffers">A list of <see cref="T:System.ArraySegment`1" />s of type <see cref="T:System.Byte" /> that contains the received data.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
		/// <returns>The number of bytes received.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffers" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="buffers" />.Count is zero.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred while attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003DD2 RID: 15826 RVA: 0x000D3174 File Offset: 0x000D1374
		[CLSCompliant(false)]
		public unsafe int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
		{
			this.ThrowIfDisposedAndClosed();
			if (buffers == null || buffers.Count == 0)
			{
				throw new ArgumentNullException("buffers");
			}
			int count = buffers.Count;
			GCHandle[] array = new GCHandle[count];
			int num2;
			int num;
			try
			{
				try
				{
					Socket.WSABUF[] array2;
					Socket.WSABUF* ptr;
					if ((array2 = new Socket.WSABUF[count]) == null || array2.Length == 0)
					{
						ptr = null;
					}
					else
					{
						ptr = &array2[0];
					}
					for (int i = 0; i < count; i++)
					{
						ArraySegment<byte> arraySegment = buffers[i];
						if (arraySegment.Offset < 0 || arraySegment.Count < 0 || arraySegment.Count > arraySegment.Array.Length - arraySegment.Offset)
						{
							throw new ArgumentOutOfRangeException("segment");
						}
						try
						{
						}
						finally
						{
							array[i] = GCHandle.Alloc(arraySegment.Array, GCHandleType.Pinned);
						}
						ptr[i].len = arraySegment.Count;
						ptr[i].buf = Marshal.UnsafeAddrOfPinnedArrayElement<byte>(arraySegment.Array, arraySegment.Offset);
					}
					num = Socket.Receive_internal(this.m_Handle, ptr, count, socketFlags, out num2, this.is_blocking);
				}
				finally
				{
					Socket.WSABUF[] array2 = null;
				}
			}
			finally
			{
				for (int j = 0; j < count; j++)
				{
					if (array[j].IsAllocated)
					{
						array[j].Free();
					}
				}
			}
			errorCode = (SocketError)num2;
			return num;
		}

		// Token: 0x06003DD3 RID: 15827 RVA: 0x000D32F4 File Offset: 0x000D14F4
		public int Receive(Span<byte> buffer, SocketFlags socketFlags, out SocketError errorCode)
		{
			byte[] array = new byte[buffer.Length];
			int num = this.Receive(array, 0, array.Length, socketFlags, out errorCode);
			array.CopyTo<byte>(buffer);
			return num;
		}

		// Token: 0x06003DD4 RID: 15828 RVA: 0x000D3324 File Offset: 0x000D1524
		public int Send(ReadOnlySpan<byte> buffer, SocketFlags socketFlags, out SocketError errorCode)
		{
			byte[] array = buffer.ToArray();
			return this.Send(array, 0, array.Length, socketFlags, out errorCode);
		}

		// Token: 0x06003DD5 RID: 15829 RVA: 0x000D3348 File Offset: 0x000D1548
		public int Receive(Span<byte> buffer, SocketFlags socketFlags)
		{
			byte[] array = new byte[buffer.Length];
			int num = this.Receive(array, SocketFlags.None);
			array.CopyTo<byte>(buffer);
			return num;
		}

		// Token: 0x06003DD6 RID: 15830 RVA: 0x000D3371 File Offset: 0x000D1571
		public int Receive(Span<byte> buffer)
		{
			return this.Receive(buffer, SocketFlags.None);
		}

		/// <summary>Begins an asynchronous request to receive data from a connected <see cref="T:System.Net.Sockets.Socket" /> object.</summary>
		/// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
		/// <returns>
		///   <see langword="true" /> if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation.  
		/// <see langword="false" /> if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
		/// <exception cref="T:System.ArgumentException">An argument was invalid. The <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> or <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.BufferList" /> properties on the <paramref name="e" /> parameter must reference valid buffers. One or the other of these properties may be set, but not both at the same time.</exception>
		/// <exception cref="T:System.InvalidOperationException">A socket operation was already in progress using the <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object specified in the <paramref name="e" /> parameter.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows XP or later is required for this method.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		// Token: 0x06003DD7 RID: 15831 RVA: 0x000D337C File Offset: 0x000D157C
		public bool ReceiveAsync(SocketAsyncEventArgs e)
		{
			this.ThrowIfDisposedAndClosed();
			if (e.MemoryBuffer.Equals(default(Memory<byte>)) && e.BufferList == null)
			{
				throw new NullReferenceException("Either e.Buffer or e.BufferList must be valid buffers.");
			}
			if (e.BufferList != null)
			{
				this.InitSocketAsyncEventArgs(e, Socket.ReceiveAsyncCallback, e, SocketOperation.ReceiveGeneric);
				e.socket_async_result.Buffers = e.BufferList;
				this.QueueIOSelectorJob(this.ReadSem, e.socket_async_result.Handle, new IOSelectorJob(IOOperation.Read, Socket.BeginReceiveGenericCallback, e.socket_async_result));
			}
			else
			{
				this.InitSocketAsyncEventArgs(e, Socket.ReceiveAsyncCallback, e, SocketOperation.Receive);
				e.socket_async_result.Buffer = e.MemoryBuffer;
				e.socket_async_result.Offset = e.Offset;
				e.socket_async_result.Size = e.Count;
				this.QueueIOSelectorJob(this.ReadSem, e.socket_async_result.Handle, new IOSelectorJob(IOOperation.Read, Socket.BeginReceiveCallback, e.socket_async_result));
			}
			return true;
		}

		/// <summary>Begins to asynchronously receive data from a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data.</param>
		/// <param name="offset">The location in <paramref name="buffer" /> to store the received data.</param>
		/// <param name="size">The number of bytes to receive.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
		/// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete.</param>
		/// <param name="state">A user-defined object that contains information about the receive operation. This object is passed to the <see cref="M:System.Net.Sockets.Socket.EndReceive(System.IAsyncResult)" /> delegate when the operation is complete.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous read.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///   <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.  
		/// -or-  
		/// <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.  
		/// -or-  
		/// <paramref name="size" /> is less than 0.  
		/// -or-  
		/// <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter.</exception>
		// Token: 0x06003DD8 RID: 15832 RVA: 0x000D3478 File Offset: 0x000D1678
		public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
		{
			this.ThrowIfDisposedAndClosed();
			this.ThrowIfBufferNull(buffer);
			this.ThrowIfBufferOutOfRange(buffer, offset, size);
			errorCode = SocketError.Success;
			SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, callback, state, SocketOperation.Receive)
			{
				Buffer = buffer,
				Offset = offset,
				Size = size,
				SockFlags = socketFlags
			};
			this.QueueIOSelectorJob(this.ReadSem, socketAsyncResult.Handle, new IOSelectorJob(IOOperation.Read, Socket.BeginReceiveCallback, socketAsyncResult));
			return socketAsyncResult;
		}

		/// <summary>Begins to asynchronously receive data from a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <param name="buffers">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
		/// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete.</param>
		/// <param name="state">A user-defined object that contains information about the receive operation. This object is passed to the <see cref="M:System.Net.Sockets.Socket.EndReceive(System.IAsyncResult)" /> delegate when the operation is complete.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous read.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///   <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003DD9 RID: 15833 RVA: 0x000D34EC File Offset: 0x000D16EC
		[CLSCompliant(false)]
		public IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
		{
			this.ThrowIfDisposedAndClosed();
			if (buffers == null)
			{
				throw new ArgumentNullException("buffers");
			}
			errorCode = SocketError.Success;
			SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, callback, state, SocketOperation.ReceiveGeneric)
			{
				Buffers = buffers,
				SockFlags = socketFlags
			};
			this.QueueIOSelectorJob(this.ReadSem, socketAsyncResult.Handle, new IOSelectorJob(IOOperation.Read, Socket.BeginReceiveGenericCallback, socketAsyncResult));
			return socketAsyncResult;
		}

		/// <summary>Ends a pending asynchronous read.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information and any user defined data for this asynchronous operation.</param>
		/// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
		/// <returns>The number of bytes received.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by a call to the <see cref="M:System.Net.Sockets.Socket.BeginReceive(System.Byte[],System.Int32,System.Int32,System.Net.Sockets.SocketFlags,System.AsyncCallback,System.Object)" /> method.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndReceive(System.IAsyncResult)" /> was previously called for the asynchronous read.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003DDA RID: 15834 RVA: 0x000D354C File Offset: 0x000D174C
		public int EndReceive(IAsyncResult asyncResult, out SocketError errorCode)
		{
			this.ThrowIfDisposedAndClosed();
			SocketAsyncResult socketAsyncResult = this.ValidateEndIAsyncResult(asyncResult, "EndReceive", "asyncResult");
			if (!socketAsyncResult.IsCompleted)
			{
				socketAsyncResult.AsyncWaitHandle.WaitOne();
			}
			errorCode = socketAsyncResult.ErrorCode;
			if (errorCode != SocketError.Success && errorCode != SocketError.WouldBlock && errorCode != SocketError.InProgress)
			{
				this.is_connected = false;
			}
			if (errorCode == SocketError.Success)
			{
				socketAsyncResult.CheckIfThrowDelayedException();
			}
			return socketAsyncResult.Total;
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x000D35BC File Offset: 0x000D17BC
		private unsafe static int Receive_internal(SafeSocketHandle safeHandle, Socket.WSABUF* bufarray, int count, SocketFlags flags, out int error, bool blocking)
		{
			int num;
			try
			{
				safeHandle.RegisterForBlockingSyscall();
				num = Socket.Receive_array_icall(safeHandle.DangerousGetHandle(), bufarray, count, flags, out error, blocking);
			}
			finally
			{
				safeHandle.UnRegisterForBlockingSyscall();
			}
			return num;
		}

		// Token: 0x06003DDC RID: 15836
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int Receive_array_icall(IntPtr sock, Socket.WSABUF* bufarray, int count, SocketFlags flags, out int error, bool blocking);

		// Token: 0x06003DDD RID: 15837 RVA: 0x000D35FC File Offset: 0x000D17FC
		private unsafe static int Receive_internal(SafeSocketHandle safeHandle, byte* buffer, int count, SocketFlags flags, out int error, bool blocking)
		{
			int num;
			try
			{
				safeHandle.RegisterForBlockingSyscall();
				num = Socket.Receive_icall(safeHandle.DangerousGetHandle(), buffer, count, flags, out error, blocking);
			}
			finally
			{
				safeHandle.UnRegisterForBlockingSyscall();
			}
			return num;
		}

		// Token: 0x06003DDE RID: 15838
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int Receive_icall(IntPtr sock, byte* buffer, int count, SocketFlags flags, out int error, bool blocking);

		/// <summary>Receives the specified number of bytes of data into the specified location of the data buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />, and stores the endpoint.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for received data.</param>
		/// <param name="offset">The position in the <paramref name="buffer" /> parameter to store the received data.</param>
		/// <param name="size">The number of bytes to receive.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" />, passed by reference, that represents the remote server.</param>
		/// <returns>The number of bytes received.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="remoteEP" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.  
		/// -or-  
		/// <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.  
		/// -or-  
		/// <paramref name="size" /> is less than 0.  
		/// -or-  
		/// <paramref name="size" /> is greater than the length of the <paramref name="buffer" /> minus the value of the offset parameter.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="socketFlags" /> is not a valid combination of values.  
		/// -or-  
		/// The <see cref="P:System.Net.Sockets.Socket.LocalEndPoint" /> property was not set.  
		/// -or-  
		/// An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003DDF RID: 15839 RVA: 0x000D363C File Offset: 0x000D183C
		public int ReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP)
		{
			this.ThrowIfDisposedAndClosed();
			this.ThrowIfBufferNull(buffer);
			this.ThrowIfBufferOutOfRange(buffer, offset, size);
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			SocketError socketError;
			int num = this.ReceiveFrom(buffer, offset, size, socketFlags, ref remoteEP, out socketError);
			if (socketError != SocketError.Success)
			{
				throw new SocketException(socketError);
			}
			return num;
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x000D3688 File Offset: 0x000D1888
		internal unsafe int ReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, out SocketError errorCode)
		{
			SocketAddress socketAddress = remoteEP.Serialize();
			int num2;
			int num;
			fixed (byte[] array = buffer)
			{
				byte* ptr;
				if (buffer == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				num = Socket.ReceiveFrom_internal(this.m_Handle, ptr + offset, size, socketFlags, ref socketAddress, out num2, this.is_blocking);
			}
			errorCode = (SocketError)num2;
			if (errorCode != SocketError.Success)
			{
				if (errorCode != SocketError.WouldBlock && errorCode != SocketError.InProgress)
				{
					this.is_connected = false;
				}
				else if (errorCode == SocketError.WouldBlock && this.is_blocking)
				{
					errorCode = SocketError.TimedOut;
				}
				return 0;
			}
			this.is_connected = true;
			this.is_bound = true;
			if (socketAddress != null)
			{
				remoteEP = remoteEP.Create(socketAddress);
			}
			this.seed_endpoint = remoteEP;
			return num;
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x000D3744 File Offset: 0x000D1944
		private unsafe int ReceiveFrom(Memory<byte> buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, out SocketError errorCode)
		{
			SocketAddress socketAddress = remoteEP.Serialize();
			int num2;
			int num;
			using (MemoryHandle memoryHandle = buffer.Slice(offset, size).Pin())
			{
				num = Socket.ReceiveFrom_internal(this.m_Handle, (byte*)memoryHandle.Pointer, size, socketFlags, ref socketAddress, out num2, this.is_blocking);
			}
			errorCode = (SocketError)num2;
			if (errorCode != SocketError.Success)
			{
				if (errorCode != SocketError.WouldBlock && errorCode != SocketError.InProgress)
				{
					this.is_connected = false;
				}
				else if (errorCode == SocketError.WouldBlock && this.is_blocking)
				{
					errorCode = SocketError.TimedOut;
				}
				return 0;
			}
			this.is_connected = true;
			this.is_bound = true;
			if (socketAddress != null)
			{
				remoteEP = remoteEP.Create(socketAddress);
			}
			this.seed_endpoint = remoteEP;
			return num;
		}

		/// <summary>Begins to asynchronously receive data from a specified network device.</summary>
		/// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
		/// <returns>
		///   <see langword="true" /> if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation.  
		/// <see langword="false" /> if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.RemoteEndPoint" /> cannot be null.</exception>
		/// <exception cref="T:System.InvalidOperationException">A socket operation was already in progress using the <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object specified in the <paramref name="e" /> parameter.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows XP or later is required for this method.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		// Token: 0x06003DE2 RID: 15842 RVA: 0x000D3818 File Offset: 0x000D1A18
		public bool ReceiveFromAsync(SocketAsyncEventArgs e)
		{
			this.ThrowIfDisposedAndClosed();
			if (e.BufferList != null)
			{
				throw new NotSupportedException("Mono doesn't support using BufferList at this point.");
			}
			if (e.RemoteEndPoint == null)
			{
				throw new ArgumentNullException("remoteEP", "Value cannot be null.");
			}
			this.InitSocketAsyncEventArgs(e, Socket.ReceiveFromAsyncCallback, e, SocketOperation.ReceiveFrom);
			e.socket_async_result.Buffer = e.Buffer;
			e.socket_async_result.Offset = e.Offset;
			e.socket_async_result.Size = e.Count;
			e.socket_async_result.EndPoint = e.RemoteEndPoint;
			e.socket_async_result.SockFlags = e.SocketFlags;
			this.QueueIOSelectorJob(this.ReadSem, e.socket_async_result.Handle, new IOSelectorJob(IOOperation.Read, Socket.BeginReceiveFromCallback, e.socket_async_result));
			return true;
		}

		/// <summary>Begins to asynchronously receive data from a specified network device.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data.</param>
		/// <param name="offset">The zero-based position in the <paramref name="buffer" /> parameter at which to store the data.</param>
		/// <param name="size">The number of bytes to receive.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" /> that represents the source of the data.</param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
		/// <param name="state">An object that contains state information for this request.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous read.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="remoteEP" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.  
		/// -or-  
		/// <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.  
		/// -or-  
		/// <paramref name="size" /> is less than 0.  
		/// -or-  
		/// <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Security.SecurityException">A caller higher in the call stack does not have permission for the requested operation.</exception>
		// Token: 0x06003DE3 RID: 15843 RVA: 0x000D38E8 File Offset: 0x000D1AE8
		public IAsyncResult BeginReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, AsyncCallback callback, object state)
		{
			this.ThrowIfDisposedAndClosed();
			this.ThrowIfBufferNull(buffer);
			this.ThrowIfBufferOutOfRange(buffer, offset, size);
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, callback, state, SocketOperation.ReceiveFrom)
			{
				Buffer = buffer,
				Offset = offset,
				Size = size,
				SockFlags = socketFlags,
				EndPoint = remoteEP
			};
			this.QueueIOSelectorJob(this.ReadSem, socketAsyncResult.Handle, new IOSelectorJob(IOOperation.Read, Socket.BeginReceiveFromCallback, socketAsyncResult));
			return socketAsyncResult;
		}

		/// <summary>Ends a pending asynchronous read from a specific endpoint.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information and any user defined data for this asynchronous operation.</param>
		/// <param name="endPoint">The source <see cref="T:System.Net.EndPoint" />.</param>
		/// <returns>If successful, the number of bytes received. If unsuccessful, returns 0.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by a call to the <see cref="M:System.Net.Sockets.Socket.BeginReceiveFrom(System.Byte[],System.Int32,System.Int32,System.Net.Sockets.SocketFlags,System.Net.EndPoint@,System.AsyncCallback,System.Object)" /> method.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndReceiveFrom(System.IAsyncResult,System.Net.EndPoint@)" /> was previously called for the asynchronous read.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003DE4 RID: 15844 RVA: 0x000D3974 File Offset: 0x000D1B74
		public int EndReceiveFrom(IAsyncResult asyncResult, ref EndPoint endPoint)
		{
			this.ThrowIfDisposedAndClosed();
			if (endPoint == null)
			{
				throw new ArgumentNullException("endPoint");
			}
			SocketAsyncResult socketAsyncResult = this.ValidateEndIAsyncResult(asyncResult, "EndReceiveFrom", "asyncResult");
			if (!socketAsyncResult.IsCompleted)
			{
				socketAsyncResult.AsyncWaitHandle.WaitOne();
			}
			socketAsyncResult.CheckIfThrowDelayedException();
			endPoint = socketAsyncResult.EndPoint;
			return socketAsyncResult.Total;
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x000D39D0 File Offset: 0x000D1BD0
		private int EndReceiveFrom_internal(SocketAsyncResult sockares, SocketAsyncEventArgs ares)
		{
			this.ThrowIfDisposedAndClosed();
			if (Interlocked.CompareExchange(ref sockares.EndCalled, 1, 0) == 1)
			{
				throw new InvalidOperationException("EndReceiveFrom can only be called once per asynchronous operation");
			}
			if (!sockares.IsCompleted)
			{
				sockares.AsyncWaitHandle.WaitOne();
			}
			sockares.CheckIfThrowDelayedException();
			ares.RemoteEndPoint = sockares.EndPoint;
			return sockares.Total;
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x000D3A2C File Offset: 0x000D1C2C
		private unsafe static int ReceiveFrom_internal(SafeSocketHandle safeHandle, byte* buffer, int count, SocketFlags flags, ref SocketAddress sockaddr, out int error, bool blocking)
		{
			int num;
			try
			{
				safeHandle.RegisterForBlockingSyscall();
				num = Socket.ReceiveFrom_icall(safeHandle.DangerousGetHandle(), buffer, count, flags, ref sockaddr, out error, blocking);
			}
			finally
			{
				safeHandle.UnRegisterForBlockingSyscall();
			}
			return num;
		}

		// Token: 0x06003DE7 RID: 15847
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int ReceiveFrom_icall(IntPtr sock, byte* buffer, int count, SocketFlags flags, ref SocketAddress sockaddr, out int error, bool blocking);

		/// <summary>Receives the specified number of bytes of data into the specified location of the data buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />, and stores the endpoint and packet information.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for received data.</param>
		/// <param name="offset">The position in the <paramref name="buffer" /> parameter to store the received data.</param>
		/// <param name="size">The number of bytes to receive.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" />, passed by reference, that represents the remote server.</param>
		/// <param name="ipPacketInformation">An <see cref="T:System.Net.Sockets.IPPacketInformation" /> holding address and interface information.</param>
		/// <returns>The number of bytes received.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.  
		/// - or-  
		/// <paramref name="remoteEP" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.  
		/// -or-  
		/// <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.  
		/// -or-  
		/// <paramref name="size" /> is less than 0.  
		/// -or-  
		/// <paramref name="size" /> is greater than the length of the <paramref name="buffer" /> minus the value of the offset parameter.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="socketFlags" /> is not a valid combination of values.  
		/// -or-  
		/// The <see cref="P:System.Net.Sockets.Socket.LocalEndPoint" /> property was not set.  
		/// -or-  
		/// The .NET Framework is running on an AMD 64-bit processor.  
		/// -or-  
		/// An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.NotSupportedException">The operating system is Windows 2000 or earlier, and this method requires Windows XP.</exception>
		// Token: 0x06003DE8 RID: 15848 RVA: 0x000D3A70 File Offset: 0x000D1C70
		[MonoTODO("Not implemented")]
		public int ReceiveMessageFrom(byte[] buffer, int offset, int size, ref SocketFlags socketFlags, ref EndPoint remoteEP, out IPPacketInformation ipPacketInformation)
		{
			this.ThrowIfDisposedAndClosed();
			this.ThrowIfBufferNull(buffer);
			this.ThrowIfBufferOutOfRange(buffer, offset, size);
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			throw new NotImplementedException();
		}

		/// <summary>Begins to asynchronously receive the specified number of bytes of data into the specified location in the data buffer, using the specified <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.SocketFlags" />, and stores the endpoint and packet information.</summary>
		/// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
		/// <returns>
		///   <see langword="true" /> if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation.  
		/// <see langword="false" /> if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.RemoteEndPoint" /> cannot be null.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows XP or later is required for this method.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		// Token: 0x06003DE9 RID: 15849 RVA: 0x000D3A9D File Offset: 0x000D1C9D
		[MonoTODO("Not implemented")]
		public bool ReceiveMessageFromAsync(SocketAsyncEventArgs e)
		{
			this.ThrowIfDisposedAndClosed();
			throw new NotImplementedException();
		}

		/// <summary>Begins to asynchronously receive the specified number of bytes of data into the specified location of the data buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />, and stores the endpoint and packet information.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that is the storage location for the received data.</param>
		/// <param name="offset">The zero-based position in the <paramref name="buffer" /> parameter at which to store the data.</param>
		/// <param name="size">The number of bytes to receive.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" /> that represents the source of the data.</param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
		/// <param name="state">An object that contains state information for this request.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous read.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="remoteEP" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.  
		/// -or-  
		/// <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.  
		/// -or-  
		/// <paramref name="size" /> is less than 0.  
		/// -or-  
		/// <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.NotSupportedException">The operating system is Windows 2000 or earlier, and this method requires Windows XP.</exception>
		// Token: 0x06003DEA RID: 15850 RVA: 0x000D3A70 File Offset: 0x000D1C70
		[MonoTODO]
		public IAsyncResult BeginReceiveMessageFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, AsyncCallback callback, object state)
		{
			this.ThrowIfDisposedAndClosed();
			this.ThrowIfBufferNull(buffer);
			this.ThrowIfBufferOutOfRange(buffer, offset, size);
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			throw new NotImplementedException();
		}

		/// <summary>Ends a pending asynchronous read from a specific endpoint. This method also reveals more information about the packet than <see cref="M:System.Net.Sockets.Socket.EndReceiveFrom(System.IAsyncResult,System.Net.EndPoint@)" />.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information and any user defined data for this asynchronous operation.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values for the received packet.</param>
		/// <param name="endPoint">The source <see cref="T:System.Net.EndPoint" />.</param>
		/// <param name="ipPacketInformation">The <see cref="T:System.Net.IPAddress" /> and interface of the received packet.</param>
		/// <returns>If successful, the number of bytes received. If unsuccessful, returns 0.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is <see langword="null" />  
		/// -or-  
		/// <paramref name="endPoint" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by a call to the <see cref="M:System.Net.Sockets.Socket.BeginReceiveMessageFrom(System.Byte[],System.Int32,System.Int32,System.Net.Sockets.SocketFlags,System.Net.EndPoint@,System.AsyncCallback,System.Object)" /> method.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndReceiveMessageFrom(System.IAsyncResult,System.Net.Sockets.SocketFlags@,System.Net.EndPoint@,System.Net.Sockets.IPPacketInformation@)" /> was previously called for the asynchronous read.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003DEB RID: 15851 RVA: 0x000D3AAA File Offset: 0x000D1CAA
		[MonoTODO]
		public int EndReceiveMessageFrom(IAsyncResult asyncResult, ref SocketFlags socketFlags, ref EndPoint endPoint, out IPPacketInformation ipPacketInformation)
		{
			this.ThrowIfDisposedAndClosed();
			if (endPoint == null)
			{
				throw new ArgumentNullException("endPoint");
			}
			this.ValidateEndIAsyncResult(asyncResult, "EndReceiveMessageFrom", "asyncResult");
			throw new NotImplementedException();
		}

		/// <summary>Sends the specified number of bytes of data to a connected <see cref="T:System.Net.Sockets.Socket" />, starting at the specified offset, and using the specified <see cref="T:System.Net.Sockets.SocketFlags" /></summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent.</param>
		/// <param name="offset">The position in the data buffer at which to begin sending data.</param>
		/// <param name="size">The number of bytes to send.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
		/// <returns>The number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.  
		/// -or-  
		/// <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.  
		/// -or-  
		/// <paramref name="size" /> is less than 0.  
		/// -or-  
		/// <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="socketFlags" /> is not a valid combination of values.  
		/// -or-  
		/// An operating system error occurs while accessing the <see cref="T:System.Net.Sockets.Socket" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003DEC RID: 15852 RVA: 0x000D3AD8 File Offset: 0x000D1CD8
		public unsafe int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
		{
			this.ThrowIfDisposedAndClosed();
			this.ThrowIfBufferNull(buffer);
			this.ThrowIfBufferOutOfRange(buffer, offset, size);
			if (size == 0)
			{
				errorCode = SocketError.Success;
				return 0;
			}
			int num = 0;
			for (;;)
			{
				int num2;
				fixed (byte[] array = buffer)
				{
					byte* ptr;
					if (buffer == null || array.Length == 0)
					{
						ptr = null;
					}
					else
					{
						ptr = &array[0];
					}
					num += Socket.Send_internal(this.m_Handle, ptr + (offset + num), size - num, socketFlags, out num2, this.is_blocking);
				}
				errorCode = (SocketError)num2;
				if (errorCode != SocketError.Success && errorCode != SocketError.WouldBlock && errorCode != SocketError.InProgress)
				{
					break;
				}
				this.is_connected = true;
				if (num >= size)
				{
					return num;
				}
			}
			this.is_connected = false;
			this.is_bound = false;
			return num;
		}

		/// <summary>Sends the set of buffers in the list to a connected <see cref="T:System.Net.Sockets.Socket" />, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <param name="buffers">A list of <see cref="T:System.ArraySegment`1" />s of type <see cref="T:System.Byte" /> that contains the data to be sent.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
		/// <returns>The number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffers" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="buffers" /> is empty.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003DED RID: 15853 RVA: 0x000D3B7C File Offset: 0x000D1D7C
		[CLSCompliant(false)]
		public unsafe int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
		{
			this.ThrowIfDisposedAndClosed();
			if (buffers == null)
			{
				throw new ArgumentNullException("buffers");
			}
			if (buffers.Count == 0)
			{
				throw new ArgumentException("Buffer is empty", "buffers");
			}
			int count = buffers.Count;
			GCHandle[] array = new GCHandle[count];
			int num2;
			int num;
			try
			{
				try
				{
					Socket.WSABUF[] array2;
					Socket.WSABUF* ptr;
					if ((array2 = new Socket.WSABUF[count]) == null || array2.Length == 0)
					{
						ptr = null;
					}
					else
					{
						ptr = &array2[0];
					}
					for (int i = 0; i < count; i++)
					{
						ArraySegment<byte> arraySegment = buffers[i];
						if (arraySegment.Offset < 0 || arraySegment.Count < 0 || arraySegment.Count > arraySegment.Array.Length - arraySegment.Offset)
						{
							throw new ArgumentOutOfRangeException("segment");
						}
						try
						{
						}
						finally
						{
							array[i] = GCHandle.Alloc(arraySegment.Array, GCHandleType.Pinned);
						}
						ptr[i].len = arraySegment.Count;
						ptr[i].buf = Marshal.UnsafeAddrOfPinnedArrayElement<byte>(arraySegment.Array, arraySegment.Offset);
					}
					num = Socket.Send_internal(this.m_Handle, ptr, count, socketFlags, out num2, this.is_blocking);
				}
				finally
				{
					Socket.WSABUF[] array2 = null;
				}
			}
			finally
			{
				for (int j = 0; j < count; j++)
				{
					if (array[j].IsAllocated)
					{
						array[j].Free();
					}
				}
			}
			errorCode = (SocketError)num2;
			return num;
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x000D3D0C File Offset: 0x000D1F0C
		public int Send(ReadOnlySpan<byte> buffer, SocketFlags socketFlags)
		{
			return this.Send(buffer.ToArray(), socketFlags);
		}

		// Token: 0x06003DEF RID: 15855 RVA: 0x000D3D1C File Offset: 0x000D1F1C
		public int Send(ReadOnlySpan<byte> buffer)
		{
			return this.Send(buffer, SocketFlags.None);
		}

		/// <summary>Sends data asynchronously to a connected <see cref="T:System.Net.Sockets.Socket" /> object.</summary>
		/// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
		/// <returns>
		///   <see langword="true" /> if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation.  
		/// <see langword="false" /> if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> or <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.BufferList" /> properties on the <paramref name="e" /> parameter must reference valid buffers. One or the other of these properties may be set, but not both at the same time.</exception>
		/// <exception cref="T:System.InvalidOperationException">A socket operation was already in progress using the <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object specified in the <paramref name="e" /> parameter.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows XP or later is required for this method.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">The <see cref="T:System.Net.Sockets.Socket" /> is not yet connected or was not obtained via an <see cref="M:System.Net.Sockets.Socket.Accept" />, <see cref="M:System.Net.Sockets.Socket.AcceptAsync(System.Net.Sockets.SocketAsyncEventArgs)" />,or <see cref="Overload:System.Net.Sockets.Socket.BeginAccept" />, method.</exception>
		// Token: 0x06003DF0 RID: 15856 RVA: 0x000D3D28 File Offset: 0x000D1F28
		public bool SendAsync(SocketAsyncEventArgs e)
		{
			this.ThrowIfDisposedAndClosed();
			if (e.MemoryBuffer.Equals(default(Memory<byte>)) && e.BufferList == null)
			{
				throw new NullReferenceException("Either e.Buffer or e.BufferList must be valid buffers.");
			}
			if (e.BufferList != null)
			{
				this.InitSocketAsyncEventArgs(e, Socket.SendAsyncCallback, e, SocketOperation.SendGeneric);
				e.socket_async_result.Buffers = e.BufferList;
				this.QueueIOSelectorJob(this.WriteSem, e.socket_async_result.Handle, new IOSelectorJob(IOOperation.Write, Socket.BeginSendGenericCallback, e.socket_async_result));
			}
			else
			{
				this.InitSocketAsyncEventArgs(e, Socket.SendAsyncCallback, e, SocketOperation.Send);
				e.socket_async_result.Buffer = e.MemoryBuffer;
				e.socket_async_result.Offset = e.Offset;
				e.socket_async_result.Size = e.Count;
				this.QueueIOSelectorJob(this.WriteSem, e.socket_async_result.Handle, new IOSelectorJob(IOOperation.Write, delegate(IOAsyncResult s)
				{
					Socket.BeginSendCallback((SocketAsyncResult)s, 0);
				}, e.socket_async_result));
			}
			return true;
		}

		/// <summary>Sends data asynchronously to a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to send.</param>
		/// <param name="offset">The zero-based position in the <paramref name="buffer" /> parameter at which to begin sending data.</param>
		/// <param name="size">The number of bytes to send.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
		/// <param name="state">An object that contains state information for this request.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous send.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See remarks section below.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.  
		/// -or-  
		/// <paramref name="offset" /> is less than the length of <paramref name="buffer" />.  
		/// -or-  
		/// <paramref name="size" /> is less than 0.  
		/// -or-  
		/// <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003DF1 RID: 15857 RVA: 0x000D3E40 File Offset: 0x000D2040
		public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
		{
			this.ThrowIfDisposedAndClosed();
			this.ThrowIfBufferNull(buffer);
			this.ThrowIfBufferOutOfRange(buffer, offset, size);
			if (!this.is_connected)
			{
				errorCode = SocketError.NotConnected;
				return null;
			}
			errorCode = SocketError.Success;
			SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, callback, state, SocketOperation.Send)
			{
				Buffer = buffer,
				Offset = offset,
				Size = size,
				SockFlags = socketFlags
			};
			this.QueueIOSelectorJob(this.WriteSem, socketAsyncResult.Handle, new IOSelectorJob(IOOperation.Write, delegate(IOAsyncResult s)
			{
				Socket.BeginSendCallback((SocketAsyncResult)s, 0);
			}, socketAsyncResult));
			return socketAsyncResult;
		}

		// Token: 0x06003DF2 RID: 15858 RVA: 0x000D3EE0 File Offset: 0x000D20E0
		private unsafe static void BeginSendCallback(SocketAsyncResult sockares, int sent_so_far)
		{
			int num = 0;
			try
			{
				using (MemoryHandle memoryHandle = sockares.Buffer.Slice(sockares.Offset, sockares.Size).Pin())
				{
					num = Socket.Send_internal(sockares.socket.m_Handle, (byte*)memoryHandle.Pointer, sockares.Size, sockares.SockFlags, out sockares.error, false);
				}
			}
			catch (Exception ex)
			{
				sockares.Complete(ex);
				return;
			}
			if (sockares.error == 0)
			{
				sent_so_far += num;
				sockares.Offset += num;
				sockares.Size -= num;
				if (sockares.socket.CleanedUp)
				{
					sockares.Complete(sent_so_far);
					return;
				}
				if (sockares.Size > 0)
				{
					IOSelector.Add(sockares.Handle, new IOSelectorJob(IOOperation.Write, delegate(IOAsyncResult s)
					{
						Socket.BeginSendCallback((SocketAsyncResult)s, sent_so_far);
					}, sockares));
					return;
				}
				sockares.Total = sent_so_far;
			}
			sockares.Complete(sent_so_far);
		}

		/// <summary>Sends data asynchronously to a connected <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <param name="buffers">An array of type <see cref="T:System.Byte" /> that contains the data to send.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
		/// <param name="state">An object that contains state information for this request.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous send.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffers" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="buffers" /> is empty.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See remarks section below.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003DF3 RID: 15859 RVA: 0x000D4010 File Offset: 0x000D2210
		[CLSCompliant(false)]
		public IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
		{
			this.ThrowIfDisposedAndClosed();
			if (buffers == null)
			{
				throw new ArgumentNullException("buffers");
			}
			if (!this.is_connected)
			{
				errorCode = SocketError.NotConnected;
				return null;
			}
			errorCode = SocketError.Success;
			SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, callback, state, SocketOperation.SendGeneric)
			{
				Buffers = buffers,
				SockFlags = socketFlags
			};
			this.QueueIOSelectorJob(this.WriteSem, socketAsyncResult.Handle, new IOSelectorJob(IOOperation.Write, Socket.BeginSendGenericCallback, socketAsyncResult));
			return socketAsyncResult;
		}

		/// <summary>Ends a pending asynchronous send.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information for this asynchronous operation.</param>
		/// <param name="errorCode">A <see cref="T:System.Net.Sockets.SocketError" /> object that stores the socket error.</param>
		/// <returns>If successful, the number of bytes sent to the <see cref="T:System.Net.Sockets.Socket" />; otherwise, an invalid <see cref="T:System.Net.Sockets.Socket" /> error.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by a call to the <see cref="M:System.Net.Sockets.Socket.BeginSend(System.Byte[],System.Int32,System.Int32,System.Net.Sockets.SocketFlags,System.AsyncCallback,System.Object)" /> method.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndSend(System.IAsyncResult)" /> was previously called for the asynchronous send.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003DF4 RID: 15860 RVA: 0x000D4080 File Offset: 0x000D2280
		public int EndSend(IAsyncResult asyncResult, out SocketError errorCode)
		{
			this.ThrowIfDisposedAndClosed();
			SocketAsyncResult socketAsyncResult = this.ValidateEndIAsyncResult(asyncResult, "EndSend", "asyncResult");
			if (!socketAsyncResult.IsCompleted)
			{
				socketAsyncResult.AsyncWaitHandle.WaitOne();
			}
			errorCode = socketAsyncResult.ErrorCode;
			if (errorCode != SocketError.Success && errorCode != SocketError.WouldBlock && errorCode != SocketError.InProgress)
			{
				this.is_connected = false;
			}
			if (errorCode == SocketError.Success)
			{
				socketAsyncResult.CheckIfThrowDelayedException();
			}
			return socketAsyncResult.Total;
		}

		// Token: 0x06003DF5 RID: 15861 RVA: 0x000D40F0 File Offset: 0x000D22F0
		private unsafe static int Send_internal(SafeSocketHandle safeHandle, Socket.WSABUF* bufarray, int count, SocketFlags flags, out int error, bool blocking)
		{
			int num;
			try
			{
				safeHandle.RegisterForBlockingSyscall();
				num = Socket.Send_array_icall(safeHandle.DangerousGetHandle(), bufarray, count, flags, out error, blocking);
			}
			finally
			{
				safeHandle.UnRegisterForBlockingSyscall();
			}
			return num;
		}

		// Token: 0x06003DF6 RID: 15862
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int Send_array_icall(IntPtr sock, Socket.WSABUF* bufarray, int count, SocketFlags flags, out int error, bool blocking);

		// Token: 0x06003DF7 RID: 15863 RVA: 0x000D4130 File Offset: 0x000D2330
		private unsafe static int Send_internal(SafeSocketHandle safeHandle, byte* buffer, int count, SocketFlags flags, out int error, bool blocking)
		{
			int num;
			try
			{
				safeHandle.RegisterForBlockingSyscall();
				num = Socket.Send_icall(safeHandle.DangerousGetHandle(), buffer, count, flags, out error, blocking);
			}
			finally
			{
				safeHandle.UnRegisterForBlockingSyscall();
			}
			return num;
		}

		// Token: 0x06003DF8 RID: 15864
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int Send_icall(IntPtr sock, byte* buffer, int count, SocketFlags flags, out int error, bool blocking);

		/// <summary>Sends the specified number of bytes of data to the specified endpoint, starting at the specified location in the buffer, and using the specified <see cref="T:System.Net.Sockets.SocketFlags" />.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to be sent.</param>
		/// <param name="offset">The position in the data buffer at which to begin sending data.</param>
		/// <param name="size">The number of bytes to send.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="remoteEP">The <see cref="T:System.Net.EndPoint" /> that represents the destination location for the data.</param>
		/// <returns>The number of bytes sent.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="remoteEP" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.  
		/// -or-  
		/// <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.  
		/// -or-  
		/// <paramref name="size" /> is less than 0.  
		/// -or-  
		/// <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="socketFlags" /> is not a valid combination of values.  
		/// -or-  
		/// An operating system error occurs while accessing the <see cref="T:System.Net.Sockets.Socket" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call stack does not have the required permissions.</exception>
		// Token: 0x06003DF9 RID: 15865 RVA: 0x000D4170 File Offset: 0x000D2370
		public unsafe int SendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP)
		{
			this.ThrowIfDisposedAndClosed();
			this.ThrowIfBufferNull(buffer);
			this.ThrowIfBufferOutOfRange(buffer, offset, size);
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			int num2;
			int num;
			fixed (byte[] array = buffer)
			{
				byte* ptr;
				if (buffer == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				num = Socket.SendTo_internal(this.m_Handle, ptr + offset, size, socketFlags, remoteEP.Serialize(), out num2, this.is_blocking);
			}
			SocketError socketError = (SocketError)num2;
			if (socketError != SocketError.Success)
			{
				if (socketError != SocketError.WouldBlock && socketError != SocketError.InProgress)
				{
					this.is_connected = false;
				}
				throw new SocketException(num2);
			}
			this.is_connected = true;
			this.is_bound = true;
			this.seed_endpoint = remoteEP;
			return num;
		}

		// Token: 0x06003DFA RID: 15866 RVA: 0x000D4218 File Offset: 0x000D2418
		private unsafe int SendTo(Memory<byte> buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP)
		{
			this.ThrowIfDisposedAndClosed();
			if (remoteEP == null)
			{
				throw new ArgumentNullException("remoteEP");
			}
			int num2;
			int num;
			using (MemoryHandle memoryHandle = buffer.Slice(offset, size).Pin())
			{
				num = Socket.SendTo_internal(this.m_Handle, (byte*)memoryHandle.Pointer, size, socketFlags, remoteEP.Serialize(), out num2, this.is_blocking);
			}
			SocketError socketError = (SocketError)num2;
			if (socketError != SocketError.Success)
			{
				if (socketError != SocketError.WouldBlock && socketError != SocketError.InProgress)
				{
					this.is_connected = false;
				}
				throw new SocketException(num2);
			}
			this.is_connected = true;
			this.is_bound = true;
			this.seed_endpoint = remoteEP;
			return num;
		}

		/// <summary>Sends data asynchronously to a specific remote host.</summary>
		/// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
		/// <returns>
		///   <see langword="true" /> if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation.  
		/// <see langword="false" /> if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.RemoteEndPoint" /> cannot be null.</exception>
		/// <exception cref="T:System.InvalidOperationException">A socket operation was already in progress using the <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object specified in the <paramref name="e" /> parameter.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows XP or later is required for this method.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">The protocol specified is connection-oriented, but the <see cref="T:System.Net.Sockets.Socket" /> is not yet connected.</exception>
		// Token: 0x06003DFB RID: 15867 RVA: 0x000D42CC File Offset: 0x000D24CC
		public bool SendToAsync(SocketAsyncEventArgs e)
		{
			this.ThrowIfDisposedAndClosed();
			if (e.BufferList != null)
			{
				throw new NotSupportedException("Mono doesn't support using BufferList at this point.");
			}
			if (e.RemoteEndPoint == null)
			{
				throw new ArgumentNullException("remoteEP", "Value cannot be null.");
			}
			this.InitSocketAsyncEventArgs(e, Socket.SendToAsyncCallback, e, SocketOperation.SendTo);
			e.socket_async_result.Buffer = e.Buffer;
			e.socket_async_result.Offset = e.Offset;
			e.socket_async_result.Size = e.Count;
			e.socket_async_result.SockFlags = e.SocketFlags;
			e.socket_async_result.EndPoint = e.RemoteEndPoint;
			this.QueueIOSelectorJob(this.WriteSem, e.socket_async_result.Handle, new IOSelectorJob(IOOperation.Write, delegate(IOAsyncResult s)
			{
				Socket.BeginSendToCallback((SocketAsyncResult)s, 0);
			}, e.socket_async_result));
			return true;
		}

		/// <summary>Sends data asynchronously to a specific remote host.</summary>
		/// <param name="buffer">An array of type <see cref="T:System.Byte" /> that contains the data to send.</param>
		/// <param name="offset">The zero-based position in <paramref name="buffer" /> at which to begin sending data.</param>
		/// <param name="size">The number of bytes to send.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" /> that represents the remote device.</param>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
		/// <param name="state">An object that contains state information for this request.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous send.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="remoteEP" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than 0.  
		/// -or-  
		/// <paramref name="offset" /> is greater than the length of <paramref name="buffer" />.  
		/// -or-  
		/// <paramref name="size" /> is less than 0.  
		/// -or-  
		/// <paramref name="size" /> is greater than the length of <paramref name="buffer" /> minus the value of the <paramref name="offset" /> parameter.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Security.SecurityException">A caller higher in the call stack does not have permission for the requested operation.</exception>
		// Token: 0x06003DFC RID: 15868 RVA: 0x000D43B8 File Offset: 0x000D25B8
		public IAsyncResult BeginSendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP, AsyncCallback callback, object state)
		{
			this.ThrowIfDisposedAndClosed();
			this.ThrowIfBufferNull(buffer);
			this.ThrowIfBufferOutOfRange(buffer, offset, size);
			SocketAsyncResult socketAsyncResult = new SocketAsyncResult(this, callback, state, SocketOperation.SendTo)
			{
				Buffer = buffer,
				Offset = offset,
				Size = size,
				SockFlags = socketFlags,
				EndPoint = remoteEP
			};
			this.QueueIOSelectorJob(this.WriteSem, socketAsyncResult.Handle, new IOSelectorJob(IOOperation.Write, delegate(IOAsyncResult s)
			{
				Socket.BeginSendToCallback((SocketAsyncResult)s, 0);
			}, socketAsyncResult));
			return socketAsyncResult;
		}

		// Token: 0x06003DFD RID: 15869 RVA: 0x000D444C File Offset: 0x000D264C
		private static void BeginSendToCallback(SocketAsyncResult sockares, int sent_so_far)
		{
			try
			{
				int num = sockares.socket.SendTo(sockares.Buffer, sockares.Offset, sockares.Size, sockares.SockFlags, sockares.EndPoint);
				if (sockares.error == 0)
				{
					sent_so_far += num;
					sockares.Offset += num;
					sockares.Size -= num;
				}
				if (sockares.Size > 0)
				{
					IOSelector.Add(sockares.Handle, new IOSelectorJob(IOOperation.Write, delegate(IOAsyncResult s)
					{
						Socket.BeginSendToCallback((SocketAsyncResult)s, sent_so_far);
					}, sockares));
					return;
				}
				sockares.Total = sent_so_far;
			}
			catch (Exception ex)
			{
				sockares.Complete(ex);
				return;
			}
			sockares.Complete();
		}

		/// <summary>Ends a pending asynchronous send to a specific location.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that stores state information and any user defined data for this asynchronous operation.</param>
		/// <returns>If successful, the number of bytes sent; otherwise, an invalid <see cref="T:System.Net.Sockets.Socket" /> error.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by a call to the <see cref="M:System.Net.Sockets.Socket.BeginSendTo(System.Byte[],System.Int32,System.Int32,System.Net.Sockets.SocketFlags,System.Net.EndPoint,System.AsyncCallback,System.Object)" /> method.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndSendTo(System.IAsyncResult)" /> was previously called for the asynchronous send.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003DFE RID: 15870 RVA: 0x000D451C File Offset: 0x000D271C
		public int EndSendTo(IAsyncResult asyncResult)
		{
			this.ThrowIfDisposedAndClosed();
			SocketAsyncResult socketAsyncResult = this.ValidateEndIAsyncResult(asyncResult, "EndSendTo", "result");
			if (!socketAsyncResult.IsCompleted)
			{
				socketAsyncResult.AsyncWaitHandle.WaitOne();
			}
			socketAsyncResult.CheckIfThrowDelayedException();
			return socketAsyncResult.Total;
		}

		// Token: 0x06003DFF RID: 15871 RVA: 0x000D4564 File Offset: 0x000D2764
		private unsafe static int SendTo_internal(SafeSocketHandle safeHandle, byte* buffer, int count, SocketFlags flags, SocketAddress sa, out int error, bool blocking)
		{
			int num;
			try
			{
				safeHandle.RegisterForBlockingSyscall();
				num = Socket.SendTo_icall(safeHandle.DangerousGetHandle(), buffer, count, flags, sa, out error, blocking);
			}
			finally
			{
				safeHandle.UnRegisterForBlockingSyscall();
			}
			return num;
		}

		// Token: 0x06003E00 RID: 15872
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern int SendTo_icall(IntPtr sock, byte* buffer, int count, SocketFlags flags, SocketAddress sa, out int error, bool blocking);

		/// <summary>Sends the file <paramref name="fileName" /> and buffers of data to a connected <see cref="T:System.Net.Sockets.Socket" /> object using the specified <see cref="T:System.Net.Sockets.TransmitFileOptions" /> value.</summary>
		/// <param name="fileName">A <see cref="T:System.String" /> that contains the path and name of the file to be sent. This parameter can be <see langword="null" />.</param>
		/// <param name="preBuffer">A <see cref="T:System.Byte" /> array that contains data to be sent before the file is sent. This parameter can be <see langword="null" />.</param>
		/// <param name="postBuffer">A <see cref="T:System.Byte" /> array that contains data to be sent after the file is sent. This parameter can be <see langword="null" />.</param>
		/// <param name="flags">One or more of <see cref="T:System.Net.Sockets.TransmitFileOptions" /> values.</param>
		/// <exception cref="T:System.NotSupportedException">The operating system is not Windows NT or later.  
		/// -or-
		///  The socket is not connected to a remote host.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.Socket" /> object is not in blocking mode and cannot accept this synchronous call.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file <paramref name="fileName" /> was not found.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		// Token: 0x06003E01 RID: 15873 RVA: 0x000D45A8 File Offset: 0x000D27A8
		public void SendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags)
		{
			this.ThrowIfDisposedAndClosed();
			if (!this.is_connected)
			{
				throw new NotSupportedException();
			}
			if (!this.is_blocking)
			{
				throw new InvalidOperationException();
			}
			int num = 0;
			if (Socket.SendFile_internal(this.m_Handle, fileName, preBuffer, postBuffer, flags, out num, this.is_blocking) && num == 0)
			{
				return;
			}
			SocketException ex = new SocketException(num);
			if (ex.ErrorCode == 2 || ex.ErrorCode == 3)
			{
				throw new FileNotFoundException();
			}
			throw ex;
		}

		/// <summary>Sends a file and buffers of data asynchronously to a connected <see cref="T:System.Net.Sockets.Socket" /> object.</summary>
		/// <param name="fileName">A string that contains the path and name of the file to be sent. This parameter can be <see langword="null" />.</param>
		/// <param name="preBuffer">A <see cref="T:System.Byte" /> array that contains data to be sent before the file is sent. This parameter can be <see langword="null" />.</param>
		/// <param name="postBuffer">A <see cref="T:System.Byte" /> array that contains data to be sent after the file is sent. This parameter can be <see langword="null" />.</param>
		/// <param name="flags">A bitwise combination of <see cref="T:System.Net.Sockets.TransmitFileOptions" /> values.</param>
		/// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate to be invoked when this operation completes. This parameter can be <see langword="null" />.</param>
		/// <param name="state">A user-defined object that contains state information for this request. This parameter can be <see langword="null" />.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that represents the asynchronous operation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See remarks section below.</exception>
		/// <exception cref="T:System.NotSupportedException">The operating system is not Windows NT or later.  
		/// -or-
		///  The socket is not connected to a remote host.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file <paramref name="fileName" /> was not found.</exception>
		// Token: 0x06003E02 RID: 15874 RVA: 0x000D4618 File Offset: 0x000D2818
		public IAsyncResult BeginSendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags, AsyncCallback callback, object state)
		{
			this.ThrowIfDisposedAndClosed();
			if (!this.is_connected)
			{
				throw new NotSupportedException();
			}
			if (!File.Exists(fileName))
			{
				throw new FileNotFoundException();
			}
			Socket.SendFileHandler handler = new Socket.SendFileHandler(this.SendFile);
			return new Socket.SendFileAsyncResult(handler, handler.BeginInvoke(fileName, preBuffer, postBuffer, flags, delegate(IAsyncResult ar)
			{
				callback(new Socket.SendFileAsyncResult(handler, ar));
			}, state));
		}

		/// <summary>Ends a pending asynchronous send of a file.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> object that stores state information for this asynchronous operation.</param>
		/// <exception cref="T:System.NotSupportedException">Windows NT is required for this method.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is empty.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by a call to the <see cref="M:System.Net.Sockets.Socket.BeginSendFile(System.String,System.AsyncCallback,System.Object)" /> method.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.Sockets.Socket.EndSendFile(System.IAsyncResult)" /> was previously called for the asynchronous <see cref="M:System.Net.Sockets.Socket.BeginSendFile(System.String,System.AsyncCallback,System.Object)" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See remarks section below.</exception>
		// Token: 0x06003E03 RID: 15875 RVA: 0x000D4690 File Offset: 0x000D2890
		public void EndSendFile(IAsyncResult asyncResult)
		{
			this.ThrowIfDisposedAndClosed();
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			Socket.SendFileAsyncResult sendFileAsyncResult = asyncResult as Socket.SendFileAsyncResult;
			if (sendFileAsyncResult == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "asyncResult");
			}
			sendFileAsyncResult.Delegate.EndInvoke(sendFileAsyncResult.Original);
		}

		// Token: 0x06003E04 RID: 15876 RVA: 0x000D46DC File Offset: 0x000D28DC
		private static bool SendFile_internal(SafeSocketHandle safeHandle, string filename, byte[] pre_buffer, byte[] post_buffer, TransmitFileOptions flags, out int error, bool blocking)
		{
			bool flag;
			try
			{
				safeHandle.RegisterForBlockingSyscall();
				flag = Socket.SendFile_icall(safeHandle.DangerousGetHandle(), filename, pre_buffer, post_buffer, flags, out error, blocking);
			}
			finally
			{
				safeHandle.UnRegisterForBlockingSyscall();
			}
			return flag;
		}

		// Token: 0x06003E05 RID: 15877
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SendFile_icall(IntPtr sock, string filename, byte[] pre_buffer, byte[] post_buffer, TransmitFileOptions flags, out int error, bool blocking);

		/// <summary>Sends a collection of files or in memory data buffers asynchronously to a connected <see cref="T:System.Net.Sockets.Socket" /> object.</summary>
		/// <param name="e">The <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object to use for this asynchronous socket operation.</param>
		/// <returns>
		///   <see langword="true" /> if the I/O operation is pending. The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will be raised upon completion of the operation.  
		/// <see langword="false" /> if the I/O operation completed synchronously. In this case, The <see cref="E:System.Net.Sockets.SocketAsyncEventArgs.Completed" /> event on the <paramref name="e" /> parameter will not be raised and the <paramref name="e" /> object passed as a parameter may be examined immediately after the method call returns to retrieve the result of the operation.</returns>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified in the <see cref="P:System.Net.Sockets.SendPacketsElement.FilePath" /> property was not found.</exception>
		/// <exception cref="T:System.InvalidOperationException">A socket operation was already in progress using the <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> object specified in the <paramref name="e" /> parameter.</exception>
		/// <exception cref="T:System.NotSupportedException">Windows XP or later is required for this method. This exception also occurs if the <see cref="T:System.Net.Sockets.Socket" /> is not connected to a remote host.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">A connectionless <see cref="T:System.Net.Sockets.Socket" /> is being used and the file being sent exceeds the maximum packet size of the underlying transport.</exception>
		// Token: 0x06003E06 RID: 15878 RVA: 0x000D3A9D File Offset: 0x000D1C9D
		[MonoTODO("Not implemented")]
		public bool SendPacketsAsync(SocketAsyncEventArgs e)
		{
			this.ThrowIfDisposedAndClosed();
			throw new NotImplementedException();
		}

		// Token: 0x06003E07 RID: 15879
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Duplicate_icall(IntPtr handle, int targetProcessId, out IntPtr duplicateHandle, out MonoIOError error);

		/// <summary>Duplicates the socket reference for the target process, and closes the socket for this process.</summary>
		/// <param name="targetProcessId">The ID of the target process where a duplicate of the socket reference is created.</param>
		/// <returns>The socket reference to be passed to the target process.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">
		///   <paramref name="targetProcessID" /> is not a valid process id.  
		/// -or-  
		/// Duplication of the socket reference failed.</exception>
		// Token: 0x06003E08 RID: 15880 RVA: 0x000D4720 File Offset: 0x000D2920
		[MonoLimitation("We do not support passing sockets across processes, we merely allow this API to pass the socket across AppDomains")]
		public SocketInformation DuplicateAndClose(int targetProcessId)
		{
			SocketInformation socketInformation = default(SocketInformation);
			socketInformation.Options = (this.is_listening ? SocketInformationOptions.Listening : ((SocketInformationOptions)0)) | (this.is_connected ? SocketInformationOptions.Connected : ((SocketInformationOptions)0)) | (this.is_blocking ? ((SocketInformationOptions)0) : SocketInformationOptions.NonBlocking) | (this.useOverlappedIO ? SocketInformationOptions.UseOnlyOverlappedIO : ((SocketInformationOptions)0));
			IntPtr intPtr;
			MonoIOError monoIOError;
			if (!Socket.Duplicate_icall(this.Handle, targetProcessId, out intPtr, out monoIOError))
			{
				throw MonoIO.GetException(monoIOError);
			}
			socketInformation.ProtocolInformation = DataConverter.Pack("iiiil", new object[]
			{
				(int)this.addressFamily,
				(int)this.socketType,
				(int)this.protocolType,
				this.is_bound ? 1 : 0,
				(long)intPtr
			});
			this.m_Handle = null;
			return socketInformation;
		}

		/// <summary>Returns the specified <see cref="T:System.Net.Sockets.Socket" /> option setting, represented as a byte array.</summary>
		/// <param name="optionLevel">One of the <see cref="T:System.Net.Sockets.SocketOptionLevel" /> values.</param>
		/// <param name="optionName">One of the <see cref="T:System.Net.Sockets.SocketOptionName" /> values.</param>
		/// <param name="optionValue">An array of type <see cref="T:System.Byte" /> that is to receive the option setting.</param>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.  
		/// -or-
		///  In .NET Compact Framework applications, the Windows CE default buffer space is set to 32768 bytes. You can change the per socket buffer space by calling <see cref="Overload:System.Net.Sockets.Socket.SetSocketOption" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003E09 RID: 15881 RVA: 0x000D47F4 File Offset: 0x000D29F4
		public void GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
		{
			this.ThrowIfDisposedAndClosed();
			if (optionValue == null)
			{
				throw new SocketException(10014, "Error trying to dereference an invalid pointer");
			}
			int num;
			Socket.GetSocketOption_arr_internal(this.m_Handle, optionLevel, optionName, ref optionValue, out num);
			if (num != 0)
			{
				throw new SocketException(num);
			}
		}

		/// <summary>Returns the value of the specified <see cref="T:System.Net.Sockets.Socket" /> option in an array.</summary>
		/// <param name="optionLevel">One of the <see cref="T:System.Net.Sockets.SocketOptionLevel" /> values.</param>
		/// <param name="optionName">One of the <see cref="T:System.Net.Sockets.SocketOptionName" /> values.</param>
		/// <param name="optionLength">The length, in bytes, of the expected return value.</param>
		/// <returns>An array of type <see cref="T:System.Byte" /> that contains the value of the socket option.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.  
		/// -or-
		///  In .NET Compact Framework applications, the Windows CE default buffer space is set to 32768 bytes. You can change the per socket buffer space by calling <see cref="Overload:System.Net.Sockets.Socket.SetSocketOption" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003E0A RID: 15882 RVA: 0x000D4838 File Offset: 0x000D2A38
		public byte[] GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionLength)
		{
			this.ThrowIfDisposedAndClosed();
			byte[] array = new byte[optionLength];
			int num;
			Socket.GetSocketOption_arr_internal(this.m_Handle, optionLevel, optionName, ref array, out num);
			if (num != 0)
			{
				throw new SocketException(num);
			}
			return array;
		}

		/// <summary>Returns the value of a specified <see cref="T:System.Net.Sockets.Socket" /> option, represented as an object.</summary>
		/// <param name="optionLevel">One of the <see cref="T:System.Net.Sockets.SocketOptionLevel" /> values.</param>
		/// <param name="optionName">One of the <see cref="T:System.Net.Sockets.SocketOptionName" /> values.</param>
		/// <returns>An object that represents the value of the option. When the <paramref name="optionName" /> parameter is set to <see cref="F:System.Net.Sockets.SocketOptionName.Linger" /> the return value is an instance of the <see cref="T:System.Net.Sockets.LingerOption" /> class. When <paramref name="optionName" /> is set to <see cref="F:System.Net.Sockets.SocketOptionName.AddMembership" /> or <see cref="F:System.Net.Sockets.SocketOptionName.DropMembership" />, the return value is an instance of the <see cref="T:System.Net.Sockets.MulticastOption" /> class. When <paramref name="optionName" /> is any other value, the return value is an integer.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.  
		///  -or-  
		///  <paramref name="optionName" /> was set to the unsupported value <see cref="F:System.Net.Sockets.SocketOptionName.MaxConnections" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003E0B RID: 15883 RVA: 0x000D4870 File Offset: 0x000D2A70
		public object GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName)
		{
			this.ThrowIfDisposedAndClosed();
			object obj;
			int num;
			Socket.GetSocketOption_obj_internal(this.m_Handle, optionLevel, optionName, out obj, out num);
			if (num != 0)
			{
				throw new SocketException(num);
			}
			if (optionName == SocketOptionName.Linger)
			{
				return (LingerOption)obj;
			}
			if (optionName == SocketOptionName.AddMembership || optionName == SocketOptionName.DropMembership)
			{
				return (MulticastOption)obj;
			}
			if (obj is int)
			{
				return (int)obj;
			}
			return obj;
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x000D48D4 File Offset: 0x000D2AD4
		private static void GetSocketOption_arr_internal(SafeSocketHandle safeHandle, SocketOptionLevel level, SocketOptionName name, ref byte[] byte_val, out int error)
		{
			bool flag = false;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				Socket.GetSocketOption_arr_icall(safeHandle.DangerousGetHandle(), level, name, ref byte_val, out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
		}

		// Token: 0x06003E0D RID: 15885
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetSocketOption_arr_icall(IntPtr socket, SocketOptionLevel level, SocketOptionName name, ref byte[] byte_val, out int error);

		// Token: 0x06003E0E RID: 15886 RVA: 0x000D4918 File Offset: 0x000D2B18
		private static void GetSocketOption_obj_internal(SafeSocketHandle safeHandle, SocketOptionLevel level, SocketOptionName name, out object obj_val, out int error)
		{
			bool flag = false;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				Socket.GetSocketOption_obj_icall(safeHandle.DangerousGetHandle(), level, name, out obj_val, out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
		}

		// Token: 0x06003E0F RID: 15887
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetSocketOption_obj_icall(IntPtr socket, SocketOptionLevel level, SocketOptionName name, out object obj_val, out int error);

		/// <summary>Sets the specified <see cref="T:System.Net.Sockets.Socket" /> option to the specified value, represented as a byte array.</summary>
		/// <param name="optionLevel">One of the <see cref="T:System.Net.Sockets.SocketOptionLevel" /> values.</param>
		/// <param name="optionName">One of the <see cref="T:System.Net.Sockets.SocketOptionName" /> values.</param>
		/// <param name="optionValue">An array of type <see cref="T:System.Byte" /> that represents the value of the option.</param>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003E10 RID: 15888 RVA: 0x000D495C File Offset: 0x000D2B5C
		public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
		{
			this.ThrowIfDisposedAndClosed();
			if (optionValue == null)
			{
				throw new SocketException(10014, "Error trying to dereference an invalid pointer");
			}
			int num;
			Socket.SetSocketOption_internal(this.m_Handle, optionLevel, optionName, null, optionValue, 0, out num);
			if (num == 0)
			{
				return;
			}
			if (num == 10022)
			{
				throw new ArgumentException();
			}
			throw new SocketException(num);
		}

		/// <summary>Sets the specified <see cref="T:System.Net.Sockets.Socket" /> option to the specified value, represented as an object.</summary>
		/// <param name="optionLevel">One of the <see cref="T:System.Net.Sockets.SocketOptionLevel" /> values.</param>
		/// <param name="optionName">One of the <see cref="T:System.Net.Sockets.SocketOptionName" /> values.</param>
		/// <param name="optionValue">A <see cref="T:System.Net.Sockets.LingerOption" /> or <see cref="T:System.Net.Sockets.MulticastOption" /> that contains the value of the option.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="optionValue" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003E11 RID: 15889 RVA: 0x000D49AC File Offset: 0x000D2BAC
		public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue)
		{
			this.ThrowIfDisposedAndClosed();
			if (optionValue == null)
			{
				throw new ArgumentNullException("optionValue");
			}
			int num;
			if (optionLevel == SocketOptionLevel.Socket && optionName == SocketOptionName.Linger)
			{
				LingerOption lingerOption = optionValue as LingerOption;
				if (lingerOption == null)
				{
					throw new ArgumentException("A 'LingerOption' value must be specified.", "optionValue");
				}
				Socket.SetSocketOption_internal(this.m_Handle, optionLevel, optionName, lingerOption, null, 0, out num);
			}
			else if (optionLevel == SocketOptionLevel.IP && (optionName == SocketOptionName.AddMembership || optionName == SocketOptionName.DropMembership))
			{
				MulticastOption multicastOption = optionValue as MulticastOption;
				if (multicastOption == null)
				{
					throw new ArgumentException("A 'MulticastOption' value must be specified.", "optionValue");
				}
				Socket.SetSocketOption_internal(this.m_Handle, optionLevel, optionName, multicastOption, null, 0, out num);
			}
			else
			{
				if (optionLevel != SocketOptionLevel.IPv6 || (optionName != SocketOptionName.AddMembership && optionName != SocketOptionName.DropMembership))
				{
					throw new ArgumentException("Invalid value specified.", "optionValue");
				}
				IPv6MulticastOption pv6MulticastOption = optionValue as IPv6MulticastOption;
				if (pv6MulticastOption == null)
				{
					throw new ArgumentException("A 'IPv6MulticastOption' value must be specified.", "optionValue");
				}
				Socket.SetSocketOption_internal(this.m_Handle, optionLevel, optionName, pv6MulticastOption, null, 0, out num);
			}
			if (num == 0)
			{
				return;
			}
			if (num == 10022)
			{
				throw new ArgumentException();
			}
			throw new SocketException(num);
		}

		/// <summary>Sets the specified <see cref="T:System.Net.Sockets.Socket" /> option to the specified <see cref="T:System.Boolean" /> value.</summary>
		/// <param name="optionLevel">One of the <see cref="T:System.Net.Sockets.SocketOptionLevel" /> values.</param>
		/// <param name="optionName">One of the <see cref="T:System.Net.Sockets.SocketOptionName" /> values.</param>
		/// <param name="optionValue">The value of the option, represented as a <see cref="T:System.Boolean" />.</param>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> object has been closed.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		// Token: 0x06003E12 RID: 15890 RVA: 0x000D4AB0 File Offset: 0x000D2CB0
		public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue)
		{
			int num = (optionValue ? 1 : 0);
			this.SetSocketOption(optionLevel, optionName, num);
		}

		/// <summary>Sets the specified <see cref="T:System.Net.Sockets.Socket" /> option to the specified integer value.</summary>
		/// <param name="optionLevel">One of the <see cref="T:System.Net.Sockets.SocketOptionLevel" /> values.</param>
		/// <param name="optionName">One of the <see cref="T:System.Net.Sockets.SocketOptionName" /> values.</param>
		/// <param name="optionValue">A value of the option.</param>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003E13 RID: 15891 RVA: 0x000D4AD0 File Offset: 0x000D2CD0
		public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue)
		{
			this.ThrowIfDisposedAndClosed();
			int num;
			Socket.SetSocketOption_internal(this.m_Handle, optionLevel, optionName, null, null, optionValue, out num);
			if (num == 0)
			{
				return;
			}
			if (num == 10022)
			{
				throw new ArgumentException();
			}
			throw new SocketException(num);
		}

		// Token: 0x06003E14 RID: 15892 RVA: 0x000D4B10 File Offset: 0x000D2D10
		private static void SetSocketOption_internal(SafeSocketHandle safeHandle, SocketOptionLevel level, SocketOptionName name, object obj_val, byte[] byte_val, int int_val, out int error)
		{
			bool flag = false;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				Socket.SetSocketOption_icall(safeHandle.DangerousGetHandle(), level, name, obj_val, byte_val, int_val, out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
		}

		// Token: 0x06003E15 RID: 15893
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSocketOption_icall(IntPtr socket, SocketOptionLevel level, SocketOptionName name, object obj_val, byte[] byte_val, int int_val, out int error);

		/// <summary>Sets low-level operating modes for the <see cref="T:System.Net.Sockets.Socket" /> using numerical control codes.</summary>
		/// <param name="ioControlCode">An <see cref="T:System.Int32" /> value that specifies the control code of the operation to perform.</param>
		/// <param name="optionInValue">A <see cref="T:System.Byte" /> array that contains the input data required by the operation.</param>
		/// <param name="optionOutValue">A <see cref="T:System.Byte" /> array that contains the output data returned by the operation.</param>
		/// <returns>The number of bytes in the <paramref name="optionOutValue" /> parameter.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to change the blocking mode without using the <see cref="P:System.Net.Sockets.Socket.Blocking" /> property.</exception>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call stack does not have the required permissions.</exception>
		// Token: 0x06003E16 RID: 15894 RVA: 0x000D4B58 File Offset: 0x000D2D58
		public int IOControl(int ioControlCode, byte[] optionInValue, byte[] optionOutValue)
		{
			if (this.CleanedUp)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			int num2;
			int num = Socket.IOControl_internal(this.m_Handle, ioControlCode, optionInValue, optionOutValue, out num2);
			if (num2 != 0)
			{
				throw new SocketException(num2);
			}
			if (num == -1)
			{
				throw new InvalidOperationException("Must use Blocking property instead.");
			}
			return num;
		}

		// Token: 0x06003E17 RID: 15895 RVA: 0x000D4BA8 File Offset: 0x000D2DA8
		private static int IOControl_internal(SafeSocketHandle safeHandle, int ioctl_code, byte[] input, byte[] output, out int error)
		{
			bool flag = false;
			int num;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				num = Socket.IOControl_icall(safeHandle.DangerousGetHandle(), ioctl_code, input, output, out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
			return num;
		}

		// Token: 0x06003E18 RID: 15896
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int IOControl_icall(IntPtr sock, int ioctl_code, byte[] input, byte[] output, out int error);

		/// <summary>Closes the <see cref="T:System.Net.Sockets.Socket" /> connection and releases all associated resources.</summary>
		// Token: 0x06003E19 RID: 15897 RVA: 0x000D4BF0 File Offset: 0x000D2DF0
		public void Close()
		{
			this.linger_timeout = 0;
			this.Dispose();
		}

		/// <summary>Closes the <see cref="T:System.Net.Sockets.Socket" /> connection and releases all associated resources with a specified timeout to allow queued data to be sent.</summary>
		/// <param name="timeout">Wait up to <paramref name="timeout" /> seconds to send any remaining data, then close the socket.</param>
		// Token: 0x06003E1A RID: 15898 RVA: 0x000D4BFF File Offset: 0x000D2DFF
		public void Close(int timeout)
		{
			this.linger_timeout = timeout;
			this.Dispose();
		}

		// Token: 0x06003E1B RID: 15899
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Close_icall(IntPtr socket, out int error);

		/// <summary>Disables sends and receives on a <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <param name="how">One of the <see cref="T:System.Net.Sockets.SocketShutdown" /> values that specifies the operation that will no longer be allowed.</param>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed.</exception>
		// Token: 0x06003E1C RID: 15900 RVA: 0x000D4C10 File Offset: 0x000D2E10
		public void Shutdown(SocketShutdown how)
		{
			this.ThrowIfDisposedAndClosed();
			if (!this.is_connected)
			{
				throw new SocketException(10057);
			}
			int num;
			Socket.Shutdown_internal(this.m_Handle, how, out num);
			if (num == 10057)
			{
				return;
			}
			if (num != 0)
			{
				throw new SocketException(num);
			}
		}

		// Token: 0x06003E1D RID: 15901 RVA: 0x000D4C58 File Offset: 0x000D2E58
		private static void Shutdown_internal(SafeSocketHandle safeHandle, SocketShutdown how, out int error)
		{
			bool flag = false;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				Socket.Shutdown_icall(safeHandle.DangerousGetHandle(), how, out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
		}

		// Token: 0x06003E1E RID: 15902
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Shutdown_icall(IntPtr socket, SocketShutdown how, out int error);

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Sockets.Socket" />, and optionally disposes of the managed resources.</summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to releases only unmanaged resources.</param>
		// Token: 0x06003E1F RID: 15903 RVA: 0x000D4C98 File Offset: 0x000D2E98
		protected virtual void Dispose(bool disposing)
		{
			if (this.CleanedUp)
			{
				return;
			}
			this.m_IntCleanedUp = 1;
			bool flag = this.is_connected;
			this.is_connected = false;
			if (this.m_Handle != null)
			{
				this.is_closed = true;
				IntPtr handle = this.Handle;
				if (flag)
				{
					this.Linger(handle);
				}
				this.m_Handle.Dispose();
			}
		}

		// Token: 0x06003E20 RID: 15904 RVA: 0x000D4CF0 File Offset: 0x000D2EF0
		private void Linger(IntPtr handle)
		{
			if (!this.is_connected || this.linger_timeout <= 0)
			{
				return;
			}
			int num;
			Socket.Shutdown_icall(handle, SocketShutdown.Receive, out num);
			if (num != 0)
			{
				return;
			}
			int num2 = this.linger_timeout / 1000;
			int num3 = this.linger_timeout % 1000;
			if (num3 > 0)
			{
				Socket.Poll_icall(handle, SelectMode.SelectRead, num3 * 1000, out num);
				if (num != 0)
				{
					return;
				}
			}
			if (num2 > 0)
			{
				LingerOption lingerOption = new LingerOption(true, num2);
				Socket.SetSocketOption_icall(handle, SocketOptionLevel.Socket, SocketOptionName.Linger, lingerOption, null, 0, out num);
			}
		}

		// Token: 0x06003E21 RID: 15905 RVA: 0x000D4D70 File Offset: 0x000D2F70
		private void ThrowIfDisposedAndClosed(Socket socket)
		{
			if (socket.CleanedUp && socket.is_closed)
			{
				throw new ObjectDisposedException(socket.GetType().ToString());
			}
		}

		// Token: 0x06003E22 RID: 15906 RVA: 0x000D4D93 File Offset: 0x000D2F93
		private void ThrowIfDisposedAndClosed()
		{
			if (this.CleanedUp && this.is_closed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
		}

		// Token: 0x06003E23 RID: 15907 RVA: 0x000D4DB6 File Offset: 0x000D2FB6
		private void ThrowIfBufferNull(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
		}

		// Token: 0x06003E24 RID: 15908 RVA: 0x000D4DC8 File Offset: 0x000D2FC8
		private void ThrowIfBufferOutOfRange(byte[] buffer, int offset, int size)
		{
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "offset must be >= 0");
			}
			if (offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset", "offset must be <= buffer.Length");
			}
			if (size < 0)
			{
				throw new ArgumentOutOfRangeException("size", "size must be >= 0");
			}
			if (size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size", "size must be <= buffer.Length - offset");
			}
		}

		// Token: 0x06003E25 RID: 15909 RVA: 0x000D4E2B File Offset: 0x000D302B
		private void ThrowIfUdp()
		{
			if (this.protocolType == ProtocolType.Udp)
			{
				throw new SocketException(10042);
			}
		}

		// Token: 0x06003E26 RID: 15910 RVA: 0x000D4E44 File Offset: 0x000D3044
		private SocketAsyncResult ValidateEndIAsyncResult(IAsyncResult ares, string methodName, string argName)
		{
			if (ares == null)
			{
				throw new ArgumentNullException(argName);
			}
			SocketAsyncResult socketAsyncResult = ares as SocketAsyncResult;
			if (socketAsyncResult == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", argName);
			}
			if (Interlocked.CompareExchange(ref socketAsyncResult.EndCalled, 1, 0) == 1)
			{
				throw new InvalidOperationException(methodName + " can only be called once per asynchronous operation");
			}
			return socketAsyncResult;
		}

		// Token: 0x06003E27 RID: 15911 RVA: 0x000D4E94 File Offset: 0x000D3094
		private void QueueIOSelectorJob(SemaphoreSlim sem, IntPtr handle, IOSelectorJob job)
		{
			Task task = sem.WaitAsync();
			if (!task.IsCompleted)
			{
				task.ContinueWith(delegate(Task t)
				{
					if (this.CleanedUp)
					{
						job.MarkDisposed();
						return;
					}
					IOSelector.Add(handle, job);
				});
				return;
			}
			if (this.CleanedUp)
			{
				job.MarkDisposed();
				return;
			}
			IOSelector.Add(handle, job);
		}

		// Token: 0x06003E28 RID: 15912 RVA: 0x000D4F04 File Offset: 0x000D3104
		private void InitSocketAsyncEventArgs(SocketAsyncEventArgs e, AsyncCallback callback, object state, SocketOperation operation)
		{
			e.socket_async_result.Init(this, callback, state, operation);
			if (e.AcceptSocket != null)
			{
				e.socket_async_result.AcceptSocket = e.AcceptSocket;
			}
			e.SetCurrentSocket(this);
			e.SetLastOperation(this.SocketOperationToSocketAsyncOperation(operation));
			e.SocketError = SocketError.Success;
			e.SetBytesTransferred(0);
		}

		// Token: 0x06003E29 RID: 15913 RVA: 0x000D4F60 File Offset: 0x000D3160
		private SocketAsyncOperation SocketOperationToSocketAsyncOperation(SocketOperation op)
		{
			switch (op)
			{
			case SocketOperation.Accept:
				return SocketAsyncOperation.Accept;
			case SocketOperation.Connect:
				return SocketAsyncOperation.Connect;
			case SocketOperation.Receive:
			case SocketOperation.ReceiveGeneric:
				return SocketAsyncOperation.Receive;
			case SocketOperation.ReceiveFrom:
				return SocketAsyncOperation.ReceiveFrom;
			case SocketOperation.Send:
			case SocketOperation.SendGeneric:
				return SocketAsyncOperation.Send;
			case SocketOperation.SendTo:
				return SocketAsyncOperation.SendTo;
			case SocketOperation.Disconnect:
				return SocketAsyncOperation.Disconnect;
			}
			throw new NotImplementedException(string.Format("Operation {0} is not implemented", op));
		}

		// Token: 0x06003E2A RID: 15914 RVA: 0x000D4FC9 File Offset: 0x000D31C9
		private IPEndPoint RemapIPEndPoint(IPEndPoint input)
		{
			if (this.IsDualMode && input.AddressFamily == AddressFamily.InterNetwork)
			{
				return new IPEndPoint(input.Address.MapToIPv6(), input.Port);
			}
			return input;
		}

		// Token: 0x06003E2B RID: 15915
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void cancel_blocking_socket_operation(Thread thread);

		// Token: 0x06003E2C RID: 15916
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool SupportsPortReuse(ProtocolType proto);

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x06003E2D RID: 15917 RVA: 0x000D4FF4 File Offset: 0x000D31F4
		internal static int FamilyHint
		{
			get
			{
				int num = 0;
				if (Socket.OSSupportsIPv4)
				{
					num = 1;
				}
				if (Socket.OSSupportsIPv6)
				{
					num = ((num == 0) ? 2 : 0);
				}
				return num;
			}
		}

		// Token: 0x06003E2E RID: 15918
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsProtocolSupported_internal(NetworkInterfaceComponent networkInterface);

		// Token: 0x06003E2F RID: 15919 RVA: 0x000D501C File Offset: 0x000D321C
		private static bool IsProtocolSupported(NetworkInterfaceComponent networkInterface)
		{
			return Socket.IsProtocolSupported_internal(networkInterface);
		}

		// Token: 0x06003E30 RID: 15920 RVA: 0x00003917 File Offset: 0x00001B17
		internal void ReplaceHandleIfNecessaryAfterFailedConnect()
		{
		}

		// Token: 0x04002423 RID: 9251
		private static readonly EventHandler<SocketAsyncEventArgs> AcceptCompletedHandler = delegate(object s, SocketAsyncEventArgs e)
		{
			Socket.CompleteAccept((Socket)s, (Socket.TaskSocketAsyncEventArgs<Socket>)e);
		};

		// Token: 0x04002424 RID: 9252
		private static readonly EventHandler<SocketAsyncEventArgs> ReceiveCompletedHandler = delegate(object s, SocketAsyncEventArgs e)
		{
			Socket.CompleteSendReceive((Socket)s, (Socket.Int32TaskSocketAsyncEventArgs)e, true);
		};

		// Token: 0x04002425 RID: 9253
		private static readonly EventHandler<SocketAsyncEventArgs> SendCompletedHandler = delegate(object s, SocketAsyncEventArgs e)
		{
			Socket.CompleteSendReceive((Socket)s, (Socket.Int32TaskSocketAsyncEventArgs)e, false);
		};

		// Token: 0x04002426 RID: 9254
		private static readonly Socket.TaskSocketAsyncEventArgs<Socket> s_rentedSocketSentinel = new Socket.TaskSocketAsyncEventArgs<Socket>();

		// Token: 0x04002427 RID: 9255
		private static readonly Socket.Int32TaskSocketAsyncEventArgs s_rentedInt32Sentinel = new Socket.Int32TaskSocketAsyncEventArgs();

		// Token: 0x04002428 RID: 9256
		private static readonly Task<int> s_zeroTask = Task.FromResult<int>(0);

		// Token: 0x04002429 RID: 9257
		private Socket.CachedEventArgs _cachedTaskEventArgs;

		// Token: 0x0400242A RID: 9258
		private static object s_InternalSyncObject;

		// Token: 0x0400242B RID: 9259
		internal static volatile bool s_SupportsIPv4;

		// Token: 0x0400242C RID: 9260
		internal static volatile bool s_SupportsIPv6;

		// Token: 0x0400242D RID: 9261
		internal static volatile bool s_OSSupportsIPv6;

		// Token: 0x0400242E RID: 9262
		internal static volatile bool s_Initialized;

		// Token: 0x0400242F RID: 9263
		private static volatile bool s_LoggingEnabled;

		// Token: 0x04002430 RID: 9264
		internal static volatile bool s_PerfCountersEnabled;

		// Token: 0x04002431 RID: 9265
		internal const int DefaultCloseTimeout = -1;

		// Token: 0x04002432 RID: 9266
		private const int SOCKET_CLOSED_CODE = 10004;

		// Token: 0x04002433 RID: 9267
		private const string TIMEOUT_EXCEPTION_MSG = "A connection attempt failed because the connected party did not properly respondafter a period of time, or established connection failed because connected host has failed to respond";

		// Token: 0x04002434 RID: 9268
		private bool is_closed;

		// Token: 0x04002435 RID: 9269
		private bool is_listening;

		// Token: 0x04002436 RID: 9270
		private bool useOverlappedIO;

		// Token: 0x04002437 RID: 9271
		private int linger_timeout;

		// Token: 0x04002438 RID: 9272
		private AddressFamily addressFamily;

		// Token: 0x04002439 RID: 9273
		private SocketType socketType;

		// Token: 0x0400243A RID: 9274
		private ProtocolType protocolType;

		// Token: 0x0400243B RID: 9275
		internal SafeSocketHandle m_Handle;

		// Token: 0x0400243C RID: 9276
		internal EndPoint seed_endpoint;

		// Token: 0x0400243D RID: 9277
		internal SemaphoreSlim ReadSem;

		// Token: 0x0400243E RID: 9278
		internal SemaphoreSlim WriteSem;

		// Token: 0x0400243F RID: 9279
		internal bool is_blocking;

		// Token: 0x04002440 RID: 9280
		internal bool is_bound;

		// Token: 0x04002441 RID: 9281
		internal bool is_connected;

		// Token: 0x04002442 RID: 9282
		private int m_IntCleanedUp;

		// Token: 0x04002443 RID: 9283
		internal bool connect_in_progress;

		// Token: 0x04002444 RID: 9284
		internal readonly int ID;

		// Token: 0x04002445 RID: 9285
		private static AsyncCallback AcceptAsyncCallback = delegate(IAsyncResult ares)
		{
			SocketAsyncEventArgs e = (SocketAsyncEventArgs)((SocketAsyncResult)ares).AsyncState;
			if (Interlocked.Exchange(ref e.in_progress, 0) != 1)
			{
				throw new InvalidOperationException("No operation in progress");
			}
			try
			{
				e.AcceptSocket = e.CurrentSocket.EndAccept(ares);
			}
			catch (SocketException ex)
			{
				e.SocketError = ex.SocketErrorCode;
			}
			catch (ObjectDisposedException)
			{
				e.SocketError = SocketError.OperationAborted;
			}
			finally
			{
				if (e.AcceptSocket == null)
				{
					e.AcceptSocket = new Socket(e.CurrentSocket.AddressFamily, e.CurrentSocket.SocketType, e.CurrentSocket.ProtocolType, null);
				}
				e.Complete_internal();
			}
		};

		// Token: 0x04002446 RID: 9286
		private static IOAsyncCallback BeginAcceptCallback = delegate(IOAsyncResult ares)
		{
			SocketAsyncResult socketAsyncResult = (SocketAsyncResult)ares;
			Socket socket = null;
			try
			{
				if (socketAsyncResult.AcceptSocket == null)
				{
					socket = socketAsyncResult.socket.Accept();
				}
				else
				{
					socket = socketAsyncResult.AcceptSocket;
					socketAsyncResult.socket.Accept(socket);
				}
			}
			catch (Exception ex2)
			{
				socketAsyncResult.Complete(ex2);
				return;
			}
			socketAsyncResult.Complete(socket);
		};

		// Token: 0x04002447 RID: 9287
		private static IOAsyncCallback BeginAcceptReceiveCallback = delegate(IOAsyncResult ares)
		{
			SocketAsyncResult socketAsyncResult2 = (SocketAsyncResult)ares;
			Socket socket2 = null;
			try
			{
				if (socketAsyncResult2.AcceptSocket == null)
				{
					socket2 = socketAsyncResult2.socket.Accept();
				}
				else
				{
					socket2 = socketAsyncResult2.AcceptSocket;
					socketAsyncResult2.socket.Accept(socket2);
				}
			}
			catch (Exception ex3)
			{
				socketAsyncResult2.Complete(ex3);
				return;
			}
			int num = 0;
			if (socketAsyncResult2.Size > 0)
			{
				try
				{
					SocketError socketError;
					num = socket2.Receive(socketAsyncResult2.Buffer, socketAsyncResult2.Offset, socketAsyncResult2.Size, socketAsyncResult2.SockFlags, out socketError);
					if (socketError != SocketError.Success)
					{
						socketAsyncResult2.Complete(new SocketException((int)socketError));
						return;
					}
				}
				catch (Exception ex4)
				{
					socketAsyncResult2.Complete(ex4);
					return;
				}
			}
			socketAsyncResult2.Complete(socket2, num);
		};

		// Token: 0x04002448 RID: 9288
		private static AsyncCallback ConnectAsyncCallback = delegate(IAsyncResult ares)
		{
			SocketAsyncEventArgs e2 = (SocketAsyncEventArgs)((SocketAsyncResult)ares).AsyncState;
			if (Interlocked.Exchange(ref e2.in_progress, 0) != 1)
			{
				throw new InvalidOperationException("No operation in progress");
			}
			try
			{
				e2.CurrentSocket.EndConnect(ares);
			}
			catch (SocketException ex5)
			{
				e2.SocketError = ex5.SocketErrorCode;
			}
			catch (ObjectDisposedException)
			{
				e2.SocketError = SocketError.OperationAborted;
			}
			finally
			{
				e2.Complete_internal();
			}
		};

		// Token: 0x04002449 RID: 9289
		private static IOAsyncCallback BeginConnectCallback = delegate(IOAsyncResult ares)
		{
			SocketAsyncResult socketAsyncResult3 = (SocketAsyncResult)ares;
			if (socketAsyncResult3.EndPoint == null)
			{
				socketAsyncResult3.Complete(new SocketException(10049));
				return;
			}
			try
			{
				int num2 = (int)socketAsyncResult3.socket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Error);
				if (num2 == 0)
				{
					socketAsyncResult3.socket.seed_endpoint = socketAsyncResult3.EndPoint;
					socketAsyncResult3.socket.is_connected = true;
					socketAsyncResult3.socket.is_bound = true;
					socketAsyncResult3.socket.connect_in_progress = false;
					socketAsyncResult3.error = 0;
					socketAsyncResult3.Complete();
				}
				else if (socketAsyncResult3.Addresses == null)
				{
					socketAsyncResult3.socket.connect_in_progress = false;
					socketAsyncResult3.Complete(new SocketException(num2));
				}
				else if (socketAsyncResult3.CurrentAddress >= socketAsyncResult3.Addresses.Length)
				{
					socketAsyncResult3.Complete(new SocketException(num2));
				}
				else
				{
					Socket.BeginMConnect(socketAsyncResult3);
				}
			}
			catch (Exception ex6)
			{
				socketAsyncResult3.socket.connect_in_progress = false;
				socketAsyncResult3.Complete(ex6);
			}
		};

		// Token: 0x0400244A RID: 9290
		private static AsyncCallback DisconnectAsyncCallback = delegate(IAsyncResult ares)
		{
			SocketAsyncEventArgs e3 = (SocketAsyncEventArgs)((SocketAsyncResult)ares).AsyncState;
			if (Interlocked.Exchange(ref e3.in_progress, 0) != 1)
			{
				throw new InvalidOperationException("No operation in progress");
			}
			try
			{
				e3.CurrentSocket.EndDisconnect(ares);
			}
			catch (SocketException ex7)
			{
				e3.SocketError = ex7.SocketErrorCode;
			}
			catch (ObjectDisposedException)
			{
				e3.SocketError = SocketError.OperationAborted;
			}
			finally
			{
				e3.Complete_internal();
			}
		};

		// Token: 0x0400244B RID: 9291
		private static IOAsyncCallback BeginDisconnectCallback = delegate(IOAsyncResult ares)
		{
			SocketAsyncResult socketAsyncResult4 = (SocketAsyncResult)ares;
			try
			{
				socketAsyncResult4.socket.Disconnect(socketAsyncResult4.ReuseSocket);
			}
			catch (Exception ex8)
			{
				socketAsyncResult4.Complete(ex8);
				return;
			}
			socketAsyncResult4.Complete();
		};

		// Token: 0x0400244C RID: 9292
		private static AsyncCallback ReceiveAsyncCallback = delegate(IAsyncResult ares)
		{
			SocketAsyncEventArgs e4 = (SocketAsyncEventArgs)((SocketAsyncResult)ares).AsyncState;
			if (Interlocked.Exchange(ref e4.in_progress, 0) != 1)
			{
				throw new InvalidOperationException("No operation in progress");
			}
			try
			{
				e4.SetBytesTransferred(e4.CurrentSocket.EndReceive(ares));
			}
			catch (SocketException ex9)
			{
				e4.SocketError = ex9.SocketErrorCode;
			}
			catch (ObjectDisposedException)
			{
				e4.SocketError = SocketError.OperationAborted;
			}
			finally
			{
				e4.Complete_internal();
			}
		};

		// Token: 0x0400244D RID: 9293
		private static IOAsyncCallback BeginReceiveCallback = delegate(IOAsyncResult ares)
		{
			SocketAsyncResult socketAsyncResult5 = (SocketAsyncResult)ares;
			int num3 = 0;
			try
			{
				using (MemoryHandle memoryHandle = socketAsyncResult5.Buffer.Slice(socketAsyncResult5.Offset, socketAsyncResult5.Size).Pin())
				{
					num3 = Socket.Receive_internal(socketAsyncResult5.socket.m_Handle, (byte*)memoryHandle.Pointer, socketAsyncResult5.Size, socketAsyncResult5.SockFlags, out socketAsyncResult5.error, socketAsyncResult5.socket.is_blocking);
				}
			}
			catch (Exception ex10)
			{
				socketAsyncResult5.Complete(ex10);
				return;
			}
			socketAsyncResult5.Complete(num3);
		};

		// Token: 0x0400244E RID: 9294
		private static IOAsyncCallback BeginReceiveGenericCallback = delegate(IOAsyncResult ares)
		{
			SocketAsyncResult socketAsyncResult6 = (SocketAsyncResult)ares;
			int num4 = 0;
			try
			{
				num4 = socketAsyncResult6.socket.Receive(socketAsyncResult6.Buffers, socketAsyncResult6.SockFlags);
			}
			catch (Exception ex11)
			{
				socketAsyncResult6.Complete(ex11);
				return;
			}
			socketAsyncResult6.Complete(num4);
		};

		// Token: 0x0400244F RID: 9295
		private static AsyncCallback ReceiveFromAsyncCallback = delegate(IAsyncResult ares)
		{
			SocketAsyncEventArgs e5 = (SocketAsyncEventArgs)((SocketAsyncResult)ares).AsyncState;
			if (Interlocked.Exchange(ref e5.in_progress, 0) != 1)
			{
				throw new InvalidOperationException("No operation in progress");
			}
			try
			{
				e5.SetBytesTransferred(e5.CurrentSocket.EndReceiveFrom_internal((SocketAsyncResult)ares, e5));
			}
			catch (SocketException ex12)
			{
				e5.SocketError = ex12.SocketErrorCode;
			}
			catch (ObjectDisposedException)
			{
				e5.SocketError = SocketError.OperationAborted;
			}
			finally
			{
				e5.Complete_internal();
			}
		};

		// Token: 0x04002450 RID: 9296
		private static IOAsyncCallback BeginReceiveFromCallback = delegate(IOAsyncResult ares)
		{
			SocketAsyncResult socketAsyncResult7 = (SocketAsyncResult)ares;
			int num5 = 0;
			try
			{
				SocketError socketError2;
				num5 = socketAsyncResult7.socket.ReceiveFrom(socketAsyncResult7.Buffer, socketAsyncResult7.Offset, socketAsyncResult7.Size, socketAsyncResult7.SockFlags, ref socketAsyncResult7.EndPoint, out socketError2);
				if (socketError2 != SocketError.Success)
				{
					socketAsyncResult7.Complete(new SocketException(socketError2));
					return;
				}
			}
			catch (Exception ex13)
			{
				socketAsyncResult7.Complete(ex13);
				return;
			}
			socketAsyncResult7.Complete(num5);
		};

		// Token: 0x04002451 RID: 9297
		private static AsyncCallback SendAsyncCallback = delegate(IAsyncResult ares)
		{
			SocketAsyncEventArgs e6 = (SocketAsyncEventArgs)((SocketAsyncResult)ares).AsyncState;
			if (Interlocked.Exchange(ref e6.in_progress, 0) != 1)
			{
				throw new InvalidOperationException("No operation in progress");
			}
			try
			{
				e6.SetBytesTransferred(e6.CurrentSocket.EndSend(ares));
			}
			catch (SocketException ex14)
			{
				e6.SocketError = ex14.SocketErrorCode;
			}
			catch (ObjectDisposedException)
			{
				e6.SocketError = SocketError.OperationAborted;
			}
			finally
			{
				e6.Complete_internal();
			}
		};

		// Token: 0x04002452 RID: 9298
		private static IOAsyncCallback BeginSendGenericCallback = delegate(IOAsyncResult ares)
		{
			SocketAsyncResult socketAsyncResult8 = (SocketAsyncResult)ares;
			int num6 = 0;
			try
			{
				num6 = socketAsyncResult8.socket.Send(socketAsyncResult8.Buffers, socketAsyncResult8.SockFlags);
			}
			catch (Exception ex15)
			{
				socketAsyncResult8.Complete(ex15);
				return;
			}
			socketAsyncResult8.Complete(num6);
		};

		// Token: 0x04002453 RID: 9299
		private static AsyncCallback SendToAsyncCallback = delegate(IAsyncResult ares)
		{
			SocketAsyncEventArgs e7 = (SocketAsyncEventArgs)((SocketAsyncResult)ares).AsyncState;
			if (Interlocked.Exchange(ref e7.in_progress, 0) != 1)
			{
				throw new InvalidOperationException("No operation in progress");
			}
			try
			{
				e7.SetBytesTransferred(e7.CurrentSocket.EndSendTo(ares));
			}
			catch (SocketException ex16)
			{
				e7.SocketError = ex16.SocketErrorCode;
			}
			catch (ObjectDisposedException)
			{
				e7.SocketError = SocketError.OperationAborted;
			}
			finally
			{
				e7.Complete_internal();
			}
		};

		// Token: 0x02000794 RID: 1940
		private class StateTaskCompletionSource<TField1, TResult> : TaskCompletionSource<TResult>
		{
			// Token: 0x06003E32 RID: 15922 RVA: 0x000D51CA File Offset: 0x000D33CA
			public StateTaskCompletionSource(object baseState)
				: base(baseState)
			{
			}

			// Token: 0x04002454 RID: 9300
			internal TField1 _field1;
		}

		// Token: 0x02000795 RID: 1941
		private class StateTaskCompletionSource<TField1, TField2, TResult> : Socket.StateTaskCompletionSource<TField1, TResult>
		{
			// Token: 0x06003E33 RID: 15923 RVA: 0x000D51D3 File Offset: 0x000D33D3
			public StateTaskCompletionSource(object baseState)
				: base(baseState)
			{
			}

			// Token: 0x04002455 RID: 9301
			internal TField2 _field2;
		}

		// Token: 0x02000796 RID: 1942
		private sealed class CachedEventArgs
		{
			// Token: 0x04002456 RID: 9302
			public Socket.TaskSocketAsyncEventArgs<Socket> TaskAccept;

			// Token: 0x04002457 RID: 9303
			public Socket.Int32TaskSocketAsyncEventArgs TaskReceive;

			// Token: 0x04002458 RID: 9304
			public Socket.Int32TaskSocketAsyncEventArgs TaskSend;

			// Token: 0x04002459 RID: 9305
			public Socket.AwaitableSocketAsyncEventArgs ValueTaskReceive;

			// Token: 0x0400245A RID: 9306
			public Socket.AwaitableSocketAsyncEventArgs ValueTaskSend;
		}

		// Token: 0x02000797 RID: 1943
		private class TaskSocketAsyncEventArgs<TResult> : SocketAsyncEventArgs
		{
			// Token: 0x06003E35 RID: 15925 RVA: 0x000D51DC File Offset: 0x000D33DC
			internal TaskSocketAsyncEventArgs()
				: base(false)
			{
			}

			// Token: 0x06003E36 RID: 15926 RVA: 0x000D51E8 File Offset: 0x000D33E8
			internal AsyncTaskMethodBuilder<TResult> GetCompletionResponsibility(out bool responsibleForReturningToPool)
			{
				AsyncTaskMethodBuilder<TResult> builder;
				lock (this)
				{
					responsibleForReturningToPool = this._accessed;
					this._accessed = true;
					Task<TResult> task = this._builder.Task;
					builder = this._builder;
				}
				return builder;
			}

			// Token: 0x0400245B RID: 9307
			internal AsyncTaskMethodBuilder<TResult> _builder;

			// Token: 0x0400245C RID: 9308
			internal bool _accessed;
		}

		// Token: 0x02000798 RID: 1944
		private sealed class Int32TaskSocketAsyncEventArgs : Socket.TaskSocketAsyncEventArgs<int>
		{
			// Token: 0x0400245D RID: 9309
			internal bool _wrapExceptionsInIOExceptions;
		}

		// Token: 0x02000799 RID: 1945
		internal sealed class AwaitableSocketAsyncEventArgs : SocketAsyncEventArgs, IValueTaskSource, IValueTaskSource<int>
		{
			// Token: 0x06003E38 RID: 15928 RVA: 0x000D5248 File Offset: 0x000D3448
			public AwaitableSocketAsyncEventArgs()
				: base(false)
			{
			}

			// Token: 0x17000E25 RID: 3621
			// (get) Token: 0x06003E39 RID: 15929 RVA: 0x000D525C File Offset: 0x000D345C
			// (set) Token: 0x06003E3A RID: 15930 RVA: 0x000D5264 File Offset: 0x000D3464
			public bool WrapExceptionsInIOExceptions { get; set; }

			// Token: 0x06003E3B RID: 15931 RVA: 0x000D526D File Offset: 0x000D346D
			public bool Reserve()
			{
				return Interlocked.CompareExchange<Action<object>>(ref this._continuation, null, Socket.AwaitableSocketAsyncEventArgs.s_availableSentinel) == Socket.AwaitableSocketAsyncEventArgs.s_availableSentinel;
			}

			// Token: 0x06003E3C RID: 15932 RVA: 0x000D5287 File Offset: 0x000D3487
			private void Release()
			{
				this._token += 1;
				Volatile.Write<Action<object>>(ref this._continuation, Socket.AwaitableSocketAsyncEventArgs.s_availableSentinel);
			}

			// Token: 0x06003E3D RID: 15933 RVA: 0x000D52A8 File Offset: 0x000D34A8
			protected override void OnCompleted(SocketAsyncEventArgs _)
			{
				Action<object> action = this._continuation;
				if (action != null || (action = Interlocked.CompareExchange<Action<object>>(ref this._continuation, Socket.AwaitableSocketAsyncEventArgs.s_completedSentinel, null)) != null)
				{
					object userToken = base.UserToken;
					base.UserToken = null;
					this._continuation = Socket.AwaitableSocketAsyncEventArgs.s_completedSentinel;
					ExecutionContext executionContext = this._executionContext;
					if (executionContext == null)
					{
						this.InvokeContinuation(action, userToken, false);
						return;
					}
					this._executionContext = null;
					ExecutionContext.Run(executionContext, delegate(object runState)
					{
						Tuple<Socket.AwaitableSocketAsyncEventArgs, Action<object>, object> tuple = (Tuple<Socket.AwaitableSocketAsyncEventArgs, Action<object>, object>)runState;
						tuple.Item1.InvokeContinuation(tuple.Item2, tuple.Item3, false);
					}, Tuple.Create<Socket.AwaitableSocketAsyncEventArgs, Action<object>, object>(this, action, userToken));
				}
			}

			// Token: 0x06003E3E RID: 15934 RVA: 0x000D5338 File Offset: 0x000D3538
			public ValueTask<int> ReceiveAsync(Socket socket)
			{
				if (socket.ReceiveAsync(this))
				{
					return new ValueTask<int>(this, this._token);
				}
				int bytesTransferred = base.BytesTransferred;
				SocketError socketError = base.SocketError;
				this.Release();
				if (socketError != SocketError.Success)
				{
					return new ValueTask<int>(Task.FromException<int>(this.CreateException(socketError)));
				}
				return new ValueTask<int>(bytesTransferred);
			}

			// Token: 0x06003E3F RID: 15935 RVA: 0x000D538C File Offset: 0x000D358C
			public ValueTask<int> SendAsync(Socket socket)
			{
				if (socket.SendAsync(this))
				{
					return new ValueTask<int>(this, this._token);
				}
				int bytesTransferred = base.BytesTransferred;
				SocketError socketError = base.SocketError;
				this.Release();
				if (socketError != SocketError.Success)
				{
					return new ValueTask<int>(Task.FromException<int>(this.CreateException(socketError)));
				}
				return new ValueTask<int>(bytesTransferred);
			}

			// Token: 0x06003E40 RID: 15936 RVA: 0x000D53E0 File Offset: 0x000D35E0
			public ValueTask SendAsyncForNetworkStream(Socket socket)
			{
				if (socket.SendAsync(this))
				{
					return new ValueTask(this, this._token);
				}
				SocketError socketError = base.SocketError;
				this.Release();
				if (socketError != SocketError.Success)
				{
					return new ValueTask(Task.FromException(this.CreateException(socketError)));
				}
				return default(ValueTask);
			}

			// Token: 0x06003E41 RID: 15937 RVA: 0x000D542E File Offset: 0x000D362E
			public ValueTaskSourceStatus GetStatus(short token)
			{
				if (token != this._token)
				{
					this.ThrowIncorrectTokenException();
				}
				if (this._continuation != Socket.AwaitableSocketAsyncEventArgs.s_completedSentinel)
				{
					return ValueTaskSourceStatus.Pending;
				}
				if (base.SocketError != SocketError.Success)
				{
					return ValueTaskSourceStatus.Faulted;
				}
				return ValueTaskSourceStatus.Succeeded;
			}

			// Token: 0x06003E42 RID: 15938 RVA: 0x000D545C File Offset: 0x000D365C
			public void OnCompleted(Action<object> continuation, object state, short token, ValueTaskSourceOnCompletedFlags flags)
			{
				if (token != this._token)
				{
					this.ThrowIncorrectTokenException();
				}
				if ((flags & ValueTaskSourceOnCompletedFlags.FlowExecutionContext) != ValueTaskSourceOnCompletedFlags.None)
				{
					this._executionContext = ExecutionContext.Capture();
				}
				if ((flags & ValueTaskSourceOnCompletedFlags.UseSchedulingContext) != ValueTaskSourceOnCompletedFlags.None)
				{
					SynchronizationContext synchronizationContext = SynchronizationContext.Current;
					if (synchronizationContext != null && synchronizationContext.GetType() != typeof(SynchronizationContext))
					{
						this._scheduler = synchronizationContext;
					}
					else
					{
						TaskScheduler taskScheduler = TaskScheduler.Current;
						if (taskScheduler != TaskScheduler.Default)
						{
							this._scheduler = taskScheduler;
						}
					}
				}
				base.UserToken = state;
				Action<object> action = Interlocked.CompareExchange<Action<object>>(ref this._continuation, continuation, null);
				if (action == Socket.AwaitableSocketAsyncEventArgs.s_completedSentinel)
				{
					this._executionContext = null;
					base.UserToken = null;
					this.InvokeContinuation(continuation, state, true);
					return;
				}
				if (action != null)
				{
					this.ThrowMultipleContinuationsException();
				}
			}

			// Token: 0x06003E43 RID: 15939 RVA: 0x000D550C File Offset: 0x000D370C
			private void InvokeContinuation(Action<object> continuation, object state, bool forceAsync)
			{
				object scheduler = this._scheduler;
				this._scheduler = null;
				if (scheduler != null)
				{
					SynchronizationContext synchronizationContext = scheduler as SynchronizationContext;
					if (synchronizationContext != null)
					{
						synchronizationContext.Post(delegate(object s)
						{
							Tuple<Action<object>, object> tuple = (Tuple<Action<object>, object>)s;
							tuple.Item1(tuple.Item2);
						}, Tuple.Create<Action<object>, object>(continuation, state));
						return;
					}
					Task.Factory.StartNew(continuation, state, CancellationToken.None, TaskCreationOptions.DenyChildAttach, (TaskScheduler)scheduler);
					return;
				}
				else
				{
					if (forceAsync)
					{
						ThreadPool.QueueUserWorkItem<object>(continuation, state, true);
						return;
					}
					continuation(state);
					return;
				}
			}

			// Token: 0x06003E44 RID: 15940 RVA: 0x000D5590 File Offset: 0x000D3790
			public int GetResult(short token)
			{
				if (token != this._token)
				{
					this.ThrowIncorrectTokenException();
				}
				SocketError socketError = base.SocketError;
				int bytesTransferred = base.BytesTransferred;
				this.Release();
				if (socketError != SocketError.Success)
				{
					this.ThrowException(socketError);
				}
				return bytesTransferred;
			}

			// Token: 0x06003E45 RID: 15941 RVA: 0x000D55CC File Offset: 0x000D37CC
			void IValueTaskSource.GetResult(short token)
			{
				if (token != this._token)
				{
					this.ThrowIncorrectTokenException();
				}
				SocketError socketError = base.SocketError;
				this.Release();
				if (socketError != SocketError.Success)
				{
					this.ThrowException(socketError);
				}
			}

			// Token: 0x06003E46 RID: 15942 RVA: 0x000D55FF File Offset: 0x000D37FF
			private void ThrowIncorrectTokenException()
			{
				throw new InvalidOperationException("The result of the operation was already consumed and may not be used again.");
			}

			// Token: 0x06003E47 RID: 15943 RVA: 0x000D560B File Offset: 0x000D380B
			private void ThrowMultipleContinuationsException()
			{
				throw new InvalidOperationException("Another continuation was already registered.");
			}

			// Token: 0x06003E48 RID: 15944 RVA: 0x000D5617 File Offset: 0x000D3817
			private void ThrowException(SocketError error)
			{
				throw this.CreateException(error);
			}

			// Token: 0x06003E49 RID: 15945 RVA: 0x000D5620 File Offset: 0x000D3820
			private Exception CreateException(SocketError error)
			{
				SocketException ex = new SocketException((int)error);
				if (!this.WrapExceptionsInIOExceptions)
				{
					return ex;
				}
				return new IOException(SR.Format("Unable to read data from the transport connection: {0}.", ex.Message), ex);
			}

			// Token: 0x0400245E RID: 9310
			internal static readonly Socket.AwaitableSocketAsyncEventArgs Reserved = new Socket.AwaitableSocketAsyncEventArgs
			{
				_continuation = null
			};

			// Token: 0x0400245F RID: 9311
			private static readonly Action<object> s_completedSentinel = delegate(object state)
			{
				throw new Exception("s_completedSentinel");
			};

			// Token: 0x04002460 RID: 9312
			private static readonly Action<object> s_availableSentinel = delegate(object state)
			{
				throw new Exception("s_availableSentinel");
			};

			// Token: 0x04002461 RID: 9313
			private Action<object> _continuation = Socket.AwaitableSocketAsyncEventArgs.s_availableSentinel;

			// Token: 0x04002462 RID: 9314
			private ExecutionContext _executionContext;

			// Token: 0x04002463 RID: 9315
			private object _scheduler;

			// Token: 0x04002464 RID: 9316
			private short _token;
		}

		// Token: 0x0200079B RID: 1947
		// (Invoke) Token: 0x06003E52 RID: 15954
		private delegate void SendFileHandler(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags);

		// Token: 0x0200079C RID: 1948
		private sealed class SendFileAsyncResult : IAsyncResult
		{
			// Token: 0x06003E55 RID: 15957 RVA: 0x000D5709 File Offset: 0x000D3909
			public SendFileAsyncResult(Socket.SendFileHandler d, IAsyncResult ares)
			{
				this.d = d;
				this.ares = ares;
			}

			// Token: 0x17000E26 RID: 3622
			// (get) Token: 0x06003E56 RID: 15958 RVA: 0x000D571F File Offset: 0x000D391F
			public object AsyncState
			{
				get
				{
					return this.ares.AsyncState;
				}
			}

			// Token: 0x17000E27 RID: 3623
			// (get) Token: 0x06003E57 RID: 15959 RVA: 0x000D572C File Offset: 0x000D392C
			public WaitHandle AsyncWaitHandle
			{
				get
				{
					return this.ares.AsyncWaitHandle;
				}
			}

			// Token: 0x17000E28 RID: 3624
			// (get) Token: 0x06003E58 RID: 15960 RVA: 0x000D5739 File Offset: 0x000D3939
			public bool CompletedSynchronously
			{
				get
				{
					return this.ares.CompletedSynchronously;
				}
			}

			// Token: 0x17000E29 RID: 3625
			// (get) Token: 0x06003E59 RID: 15961 RVA: 0x000D5746 File Offset: 0x000D3946
			public bool IsCompleted
			{
				get
				{
					return this.ares.IsCompleted;
				}
			}

			// Token: 0x17000E2A RID: 3626
			// (get) Token: 0x06003E5A RID: 15962 RVA: 0x000D5753 File Offset: 0x000D3953
			public Socket.SendFileHandler Delegate
			{
				get
				{
					return this.d;
				}
			}

			// Token: 0x17000E2B RID: 3627
			// (get) Token: 0x06003E5B RID: 15963 RVA: 0x000D575B File Offset: 0x000D395B
			public IAsyncResult Original
			{
				get
				{
					return this.ares;
				}
			}

			// Token: 0x04002469 RID: 9321
			private IAsyncResult ares;

			// Token: 0x0400246A RID: 9322
			private Socket.SendFileHandler d;
		}

		// Token: 0x0200079D RID: 1949
		private struct WSABUF
		{
			// Token: 0x0400246B RID: 9323
			public int len;

			// Token: 0x0400246C RID: 9324
			public IntPtr buf;
		}
	}
}
