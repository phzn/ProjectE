using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Sockets
{
	/// <summary>This class contains extension methods to the <see cref="T:System.Net.Sockets.Socket" /> class.</summary>
	// Token: 0x020007C7 RID: 1991
	public static class SocketTaskExtensions
	{
		/// <summary>Performs an asynchronous operation on to accept an incoming connection attempt on the socket.</summary>
		/// <param name="socket">The socket that is listening for connections.</param>
		/// <returns>An asynchronous task that completes with a <see cref="T:System.Net.Sockets.Socket" /> to handle communication with the remote host.</returns>
		// Token: 0x06003FAA RID: 16298 RVA: 0x000D9350 File Offset: 0x000D7550
		public static Task<Socket> AcceptAsync(this Socket socket)
		{
			return Task<Socket>.Factory.FromAsync((AsyncCallback callback, object state) => ((Socket)state).BeginAccept(callback, state), (IAsyncResult asyncResult) => ((Socket)asyncResult.AsyncState).EndAccept(asyncResult), socket);
		}

		/// <summary>Performs an asynchronous operation on to accept an incoming connection attempt on the socket.</summary>
		/// <param name="socket">The socket that is listening for incoming connections.</param>
		/// <param name="acceptSocket">The accepted <see cref="T:System.Net.Sockets.Socket" /> object. This value may be <see langword="null" />.</param>
		/// <returns>An asynchronous task that completes with a <see cref="T:System.Net.Sockets.Socket" /> to handle communication with the remote host.</returns>
		// Token: 0x06003FAB RID: 16299 RVA: 0x000D93A8 File Offset: 0x000D75A8
		public static Task<Socket> AcceptAsync(this Socket socket, Socket acceptSocket)
		{
			return Task<Socket>.Factory.FromAsync<Socket, int>((Socket socketForAccept, int receiveSize, AsyncCallback callback, object state) => ((Socket)state).BeginAccept(socketForAccept, receiveSize, callback, state), (IAsyncResult asyncResult) => ((Socket)asyncResult.AsyncState).EndAccept(asyncResult), acceptSocket, 0, socket);
		}

		/// <summary>Establishes a connection to a remote host.</summary>
		/// <param name="socket">The socket that is used for establishing a connection.</param>
		/// <param name="remoteEP">An EndPoint that represents the remote device.</param>
		/// <returns>An asynchronous Task.</returns>
		// Token: 0x06003FAC RID: 16300 RVA: 0x000D9400 File Offset: 0x000D7600
		public static Task ConnectAsync(this Socket socket, EndPoint remoteEP)
		{
			return Task.Factory.FromAsync<EndPoint>((EndPoint targetEndPoint, AsyncCallback callback, object state) => ((Socket)state).BeginConnect(targetEndPoint, callback, state), delegate(IAsyncResult asyncResult)
			{
				((Socket)asyncResult.AsyncState).EndConnect(asyncResult);
			}, remoteEP, socket);
		}

		/// <summary>Establishes a connection to a remote host. The host is specified by an IP address and a port number.</summary>
		/// <param name="socket">The socket to perform the connect operation on.</param>
		/// <param name="address">The IP address of the remote host.</param>
		/// <param name="port">The port number of the remote host.</param>
		// Token: 0x06003FAD RID: 16301 RVA: 0x000D9458 File Offset: 0x000D7658
		public static Task ConnectAsync(this Socket socket, IPAddress address, int port)
		{
			return Task.Factory.FromAsync<IPAddress, int>((IPAddress targetAddress, int targetPort, AsyncCallback callback, object state) => ((Socket)state).BeginConnect(targetAddress, targetPort, callback, state), delegate(IAsyncResult asyncResult)
			{
				((Socket)asyncResult.AsyncState).EndConnect(asyncResult);
			}, address, port, socket);
		}

		/// <summary>Establishes a connection to a remote host. The host is specified by an array of IP addresses and a port number.</summary>
		/// <param name="socket">The socket that the connect operation is performed on.</param>
		/// <param name="addresses">The IP addresses of the remote host.</param>
		/// <param name="port">The port number of the remote host.</param>
		/// <returns>A task that represents the asynchronous connect operation.</returns>
		// Token: 0x06003FAE RID: 16302 RVA: 0x000D94B0 File Offset: 0x000D76B0
		public static Task ConnectAsync(this Socket socket, IPAddress[] addresses, int port)
		{
			return Task.Factory.FromAsync<IPAddress[], int>((IPAddress[] targetAddresses, int targetPort, AsyncCallback callback, object state) => ((Socket)state).BeginConnect(targetAddresses, targetPort, callback, state), delegate(IAsyncResult asyncResult)
			{
				((Socket)asyncResult.AsyncState).EndConnect(asyncResult);
			}, addresses, port, socket);
		}

		/// <summary>Establishes a connection to a remote host. The host is specified by a host name and a port number.</summary>
		/// <param name="socket">The socket to perform the connect operation on.</param>
		/// <param name="host">The name of the remote host.</param>
		/// <param name="port">The port number of the remote host.</param>
		/// <returns>An asynchronous task.</returns>
		// Token: 0x06003FAF RID: 16303 RVA: 0x000D9508 File Offset: 0x000D7708
		public static Task ConnectAsync(this Socket socket, string host, int port)
		{
			return Task.Factory.FromAsync<string, int>((string targetHost, int targetPort, AsyncCallback callback, object state) => ((Socket)state).BeginConnect(targetHost, targetPort, callback, state), delegate(IAsyncResult asyncResult)
			{
				((Socket)asyncResult.AsyncState).EndConnect(asyncResult);
			}, host, port, socket);
		}

		/// <summary>Receives data from a connected socket.</summary>
		/// <param name="socket">The socket to perform the receive operation on.</param>
		/// <param name="buffer">An array that is the storage location for the received data.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <returns>A task that represents the asynchronous receive operation. The value of the <paramref name="TResult" /> parameter contains the number of bytes received.</returns>
		// Token: 0x06003FB0 RID: 16304 RVA: 0x000D9560 File Offset: 0x000D7760
		public static Task<int> ReceiveAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags)
		{
			return Task<int>.Factory.FromAsync<ArraySegment<byte>, SocketFlags>((ArraySegment<byte> targetBuffer, SocketFlags flags, AsyncCallback callback, object state) => ((Socket)state).BeginReceive(targetBuffer.Array, targetBuffer.Offset, targetBuffer.Count, flags, callback, state), (IAsyncResult asyncResult) => ((Socket)asyncResult.AsyncState).EndReceive(asyncResult), buffer, socketFlags, socket);
		}

		/// <summary>Receives data from a connected socket.</summary>
		/// <param name="socket">The socket to perform the receive operation on.</param>
		/// <param name="buffers">An array that is the storage location for the received data.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <returns>A task that represents the asynchronous receive operation. The value of the <paramref name="TResult" /> parameter contains the number of bytes received.</returns>
		// Token: 0x06003FB1 RID: 16305 RVA: 0x000D95B8 File Offset: 0x000D77B8
		public static Task<int> ReceiveAsync(this Socket socket, IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
		{
			return Task<int>.Factory.FromAsync<IList<ArraySegment<byte>>, SocketFlags>((IList<ArraySegment<byte>> targetBuffers, SocketFlags flags, AsyncCallback callback, object state) => ((Socket)state).BeginReceive(targetBuffers, flags, callback, state), (IAsyncResult asyncResult) => ((Socket)asyncResult.AsyncState).EndReceive(asyncResult), buffers, socketFlags, socket);
		}

		/// <summary>Receives data from a specified network device.</summary>
		/// <param name="socket">The socket to perform the ReceiveFrom operation on.</param>
		/// <param name="buffer">An array of type Byte that is the storage location for the received data.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="remoteEndPoint">An EndPoint that represents the source of the data.</param>
		/// <returns>An asynchronous Task that completes with a SocketReceiveFromResult struct.</returns>
		// Token: 0x06003FB2 RID: 16306 RVA: 0x000D9610 File Offset: 0x000D7810
		public static Task<SocketReceiveFromResult> ReceiveFromAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags, EndPoint remoteEndPoint)
		{
			object[] array = new object[] { socket, remoteEndPoint };
			return Task<SocketReceiveFromResult>.Factory.FromAsync<ArraySegment<byte>, SocketFlags>(delegate(ArraySegment<byte> targetBuffer, SocketFlags flags, AsyncCallback callback, object state)
			{
				object[] array2 = (object[])state;
				Socket socket2 = (Socket)array2[0];
				EndPoint endPoint = (EndPoint)array2[1];
				IAsyncResult asyncResult2 = socket2.BeginReceiveFrom(targetBuffer.Array, targetBuffer.Offset, targetBuffer.Count, flags, ref endPoint, callback, state);
				array2[1] = endPoint;
				return asyncResult2;
			}, delegate(IAsyncResult asyncResult)
			{
				object[] array3 = (object[])asyncResult.AsyncState;
				Socket socket3 = (Socket)array3[0];
				EndPoint endPoint2 = (EndPoint)array3[1];
				int num = socket3.EndReceiveFrom(asyncResult, ref endPoint2);
				return new SocketReceiveFromResult
				{
					ReceivedBytes = num,
					RemoteEndPoint = endPoint2
				};
			}, buffer, socketFlags, array);
		}

		/// <summary>Receives the specified number of bytes of data into the specified location of the data buffer, using the specified <see cref="T:System.Net.Sockets.SocketFlags" />, and stores the endpoint and packet information.</summary>
		/// <param name="socket">The socket to perform the operation on.</param>
		/// <param name="buffer">An array that is the storage location for received data.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="remoteEndPoint">An <see cref="T:System.Net.EndPoint" />, that represents the remote server.</param>
		/// <returns>An asynchronous Task that completes with a <see cref="T:System.Net.Sockets.SocketReceiveMessageFromResult" /> struct.</returns>
		// Token: 0x06003FB3 RID: 16307 RVA: 0x000D9678 File Offset: 0x000D7878
		public static Task<SocketReceiveMessageFromResult> ReceiveMessageFromAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags, EndPoint remoteEndPoint)
		{
			object[] array = new object[] { socket, socketFlags, remoteEndPoint };
			return Task<SocketReceiveMessageFromResult>.Factory.FromAsync<ArraySegment<byte>>(delegate(ArraySegment<byte> targetBuffer, AsyncCallback callback, object state)
			{
				object[] array2 = (object[])state;
				Socket socket2 = (Socket)array2[0];
				SocketFlags socketFlags2 = (SocketFlags)array2[1];
				EndPoint endPoint = (EndPoint)array2[2];
				IAsyncResult asyncResult2 = socket2.BeginReceiveMessageFrom(targetBuffer.Array, targetBuffer.Offset, targetBuffer.Count, socketFlags2, ref endPoint, callback, state);
				array2[2] = endPoint;
				return asyncResult2;
			}, delegate(IAsyncResult asyncResult)
			{
				object[] array3 = (object[])asyncResult.AsyncState;
				Socket socket3 = (Socket)array3[0];
				SocketFlags socketFlags3 = (SocketFlags)array3[1];
				EndPoint endPoint2 = (EndPoint)array3[2];
				IPPacketInformation ippacketInformation;
				int num = socket3.EndReceiveMessageFrom(asyncResult, ref socketFlags3, ref endPoint2, out ippacketInformation);
				return new SocketReceiveMessageFromResult
				{
					PacketInformation = ippacketInformation,
					ReceivedBytes = num,
					RemoteEndPoint = endPoint2,
					SocketFlags = socketFlags3
				};
			}, buffer, array);
		}

		/// <summary>Sends data to a connected socket.</summary>
		/// <param name="socket">The socket to perform the operation on.</param>
		/// <param name="buffer">An array of type Byte that contains the data to send.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <returns>An asynchronous task that completes with number of bytes sent to the socket if the operation was successful. Otherwise, the task will complete with an invalid socket error.</returns>
		// Token: 0x06003FB4 RID: 16308 RVA: 0x000D96E8 File Offset: 0x000D78E8
		public static Task<int> SendAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags)
		{
			return Task<int>.Factory.FromAsync<ArraySegment<byte>, SocketFlags>((ArraySegment<byte> targetBuffer, SocketFlags flags, AsyncCallback callback, object state) => ((Socket)state).BeginSend(targetBuffer.Array, targetBuffer.Offset, targetBuffer.Count, flags, callback, state), (IAsyncResult asyncResult) => ((Socket)asyncResult.AsyncState).EndSend(asyncResult), buffer, socketFlags, socket);
		}

		/// <summary>Sends data to a connected socket.</summary>
		/// <param name="socket">The socket to perform the operation on.</param>
		/// <param name="buffers">An array that contains the data to send.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <returns>An asynchronous task that completes with number of bytes sent to the socket if the operation was successful. Otherwise, the task will complete with an invalid socket error.</returns>
		// Token: 0x06003FB5 RID: 16309 RVA: 0x000D9740 File Offset: 0x000D7940
		public static Task<int> SendAsync(this Socket socket, IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
		{
			return Task<int>.Factory.FromAsync<IList<ArraySegment<byte>>, SocketFlags>((IList<ArraySegment<byte>> targetBuffers, SocketFlags flags, AsyncCallback callback, object state) => ((Socket)state).BeginSend(targetBuffers, flags, callback, state), (IAsyncResult asyncResult) => ((Socket)asyncResult.AsyncState).EndSend(asyncResult), buffers, socketFlags, socket);
		}

		/// <summary>Sends data asynchronously to a specific remote host.</summary>
		/// <param name="socket">The socket to perform the operation on.</param>
		/// <param name="buffer">An array that contains the data to send.</param>
		/// <param name="socketFlags">A bitwise combination of the <see cref="T:System.Net.Sockets.SocketFlags" /> values.</param>
		/// <param name="remoteEP">An <see cref="T:System.Net.EndPoint" /> that represents the remote device.</param>
		/// <returns>An asynchronous task that completes with number of bytes sent if the operation was successful. Otherwise, the task will complete with an invalid socket error.</returns>
		// Token: 0x06003FB6 RID: 16310 RVA: 0x000D9798 File Offset: 0x000D7998
		public static Task<int> SendToAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags, EndPoint remoteEP)
		{
			return Task<int>.Factory.FromAsync<ArraySegment<byte>, SocketFlags, EndPoint>((ArraySegment<byte> targetBuffer, SocketFlags flags, EndPoint endPoint, AsyncCallback callback, object state) => ((Socket)state).BeginSendTo(targetBuffer.Array, targetBuffer.Offset, targetBuffer.Count, flags, endPoint, callback, state), (IAsyncResult asyncResult) => ((Socket)asyncResult.AsyncState).EndSendTo(asyncResult), buffer, socketFlags, remoteEP, socket);
		}

		// Token: 0x06003FB7 RID: 16311 RVA: 0x000D97F1 File Offset: 0x000D79F1
		public static ValueTask<int> SendAsync(this Socket socket, ReadOnlyMemory<byte> buffer, SocketFlags socketFlags, CancellationToken cancellationToken = default(CancellationToken))
		{
			return socket.SendAsync(buffer, socketFlags, cancellationToken);
		}

		// Token: 0x06003FB8 RID: 16312 RVA: 0x000D97FC File Offset: 0x000D79FC
		public static ValueTask<int> ReceiveAsync(this Socket socket, Memory<byte> memory, SocketFlags socketFlags, CancellationToken cancellationToken = default(CancellationToken))
		{
			TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>(socket);
			byte[] buffer = memory.ToArray();
			socket.BeginReceive(buffer, 0, memory.Length, socketFlags, delegate(IAsyncResult iar)
			{
				cancellationToken.ThrowIfCancellationRequested();
				Memory<byte> memory2 = new Memory<byte>(buffer);
				memory2.CopyTo(memory);
				TaskCompletionSource<int> taskCompletionSource2 = (TaskCompletionSource<int>)iar.AsyncState;
				Socket socket2 = (Socket)taskCompletionSource2.Task.AsyncState;
				try
				{
					taskCompletionSource2.TrySetResult(socket2.EndReceive(iar));
				}
				catch (Exception ex)
				{
					taskCompletionSource2.TrySetException(ex);
				}
			}, taskCompletionSource);
			cancellationToken.ThrowIfCancellationRequested();
			return new ValueTask<int>(taskCompletionSource.Task);
		}
	}
}
