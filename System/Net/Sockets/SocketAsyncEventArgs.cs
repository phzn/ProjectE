using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Unity;

namespace System.Net.Sockets
{
	/// <summary>Represents an asynchronous socket operation.</summary>
	// Token: 0x020007C1 RID: 1985
	public class SocketAsyncEventArgs : EventArgs, IDisposable
	{
		/// <summary>Gets the exception in the case of a connection failure when a <see cref="T:System.Net.DnsEndPoint" /> was used.</summary>
		/// <returns>An <see cref="T:System.Exception" /> that indicates the cause of the connection error when a <see cref="T:System.Net.DnsEndPoint" /> was specified for the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.RemoteEndPoint" /> property.</returns>
		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x06003F5C RID: 16220 RVA: 0x000D8AB3 File Offset: 0x000D6CB3
		// (set) Token: 0x06003F5D RID: 16221 RVA: 0x000D8ABB File Offset: 0x000D6CBB
		public Exception ConnectByNameError { get; private set; }

		/// <summary>Gets or sets the socket to use or the socket created for accepting a connection with an asynchronous socket method.</summary>
		/// <returns>The <see cref="T:System.Net.Sockets.Socket" /> to use or the socket created for accepting a connection with an asynchronous socket method.</returns>
		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x06003F5E RID: 16222 RVA: 0x000D8AC4 File Offset: 0x000D6CC4
		// (set) Token: 0x06003F5F RID: 16223 RVA: 0x000D8ACC File Offset: 0x000D6CCC
		public Socket AcceptSocket { get; set; }

		/// <summary>Gets the number of bytes transferred in the socket operation.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that contains the number of bytes transferred in the socket operation.</returns>
		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x06003F60 RID: 16224 RVA: 0x000D8AD5 File Offset: 0x000D6CD5
		// (set) Token: 0x06003F61 RID: 16225 RVA: 0x000D8ADD File Offset: 0x000D6CDD
		public int BytesTransferred { get; private set; }

		/// <summary>Gets or sets a value that specifies if socket can be reused after a disconnect operation.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> that specifies if socket can be reused after a disconnect operation.</returns>
		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x06003F62 RID: 16226 RVA: 0x000D8AE6 File Offset: 0x000D6CE6
		// (set) Token: 0x06003F63 RID: 16227 RVA: 0x000D8AEE File Offset: 0x000D6CEE
		public bool DisconnectReuseSocket { get; set; }

		/// <summary>Gets the type of socket operation most recently performed with this context object.</summary>
		/// <returns>A <see cref="T:System.Net.Sockets.SocketAsyncOperation" /> instance that indicates the type of socket operation most recently performed with this context object.</returns>
		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x06003F64 RID: 16228 RVA: 0x000D8AF7 File Offset: 0x000D6CF7
		// (set) Token: 0x06003F65 RID: 16229 RVA: 0x000D8AFF File Offset: 0x000D6CFF
		public SocketAsyncOperation LastOperation { get; private set; }

		/// <summary>Gets or sets the remote IP endpoint for an asynchronous operation.</summary>
		/// <returns>An <see cref="T:System.Net.EndPoint" /> that represents the remote IP endpoint for an asynchronous operation.</returns>
		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x06003F66 RID: 16230 RVA: 0x000D8B08 File Offset: 0x000D6D08
		// (set) Token: 0x06003F67 RID: 16231 RVA: 0x000D8B10 File Offset: 0x000D6D10
		public EndPoint RemoteEndPoint
		{
			get
			{
				return this.remote_ep;
			}
			set
			{
				this.remote_ep = value;
			}
		}

		/// <summary>Gets the IP address and interface of a received packet.</summary>
		/// <returns>An <see cref="T:System.Net.Sockets.IPPacketInformation" /> instance that contains the destination IP address and interface of a received packet.</returns>
		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x06003F68 RID: 16232 RVA: 0x000D8B19 File Offset: 0x000D6D19
		// (set) Token: 0x06003F69 RID: 16233 RVA: 0x000D8B21 File Offset: 0x000D6D21
		public IPPacketInformation ReceiveMessageFromPacketInfo { get; private set; }

		/// <summary>Gets or sets an array of buffers to be sent for an asynchronous operation used by the <see cref="M:System.Net.Sockets.Socket.SendPacketsAsync(System.Net.Sockets.SocketAsyncEventArgs)" /> method.</summary>
		/// <returns>An array of <see cref="T:System.Net.Sockets.SendPacketsElement" /> objects that represent an array of buffers to be sent.</returns>
		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x06003F6A RID: 16234 RVA: 0x000D8B2A File Offset: 0x000D6D2A
		// (set) Token: 0x06003F6B RID: 16235 RVA: 0x000D8B32 File Offset: 0x000D6D32
		public SendPacketsElement[] SendPacketsElements { get; set; }

		/// <summary>Gets or sets a bitwise combination of <see cref="T:System.Net.Sockets.TransmitFileOptions" /> values for an asynchronous operation used by the <see cref="M:System.Net.Sockets.Socket.SendPacketsAsync(System.Net.Sockets.SocketAsyncEventArgs)" /> method.</summary>
		/// <returns>A <see cref="T:System.Net.Sockets.TransmitFileOptions" /> that contains a bitwise combination of values that are used with an asynchronous operation.</returns>
		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06003F6C RID: 16236 RVA: 0x000D8B3B File Offset: 0x000D6D3B
		// (set) Token: 0x06003F6D RID: 16237 RVA: 0x000D8B43 File Offset: 0x000D6D43
		public TransmitFileOptions SendPacketsFlags { get; set; }

		/// <summary>Gets or sets the size, in bytes, of the data block used in the send operation.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that contains the size, in bytes, of the data block used in the send operation.</returns>
		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x06003F6E RID: 16238 RVA: 0x000D8B4C File Offset: 0x000D6D4C
		// (set) Token: 0x06003F6F RID: 16239 RVA: 0x000D8B54 File Offset: 0x000D6D54
		[MonoTODO("unused property")]
		public int SendPacketsSendSize { get; set; }

		/// <summary>Gets or sets the result of the asynchronous socket operation.</summary>
		/// <returns>A <see cref="T:System.Net.Sockets.SocketError" /> that represents the result of the asynchronous socket operation.</returns>
		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x06003F70 RID: 16240 RVA: 0x000D8B5D File Offset: 0x000D6D5D
		// (set) Token: 0x06003F71 RID: 16241 RVA: 0x000D8B65 File Offset: 0x000D6D65
		public SocketError SocketError { get; set; }

		/// <summary>Gets the results of an asynchronous socket operation or sets the behavior of an asynchronous operation.</summary>
		/// <returns>A <see cref="T:System.Net.Sockets.SocketFlags" /> that represents the results of an asynchronous socket operation.</returns>
		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x06003F72 RID: 16242 RVA: 0x000D8B6E File Offset: 0x000D6D6E
		// (set) Token: 0x06003F73 RID: 16243 RVA: 0x000D8B76 File Offset: 0x000D6D76
		public SocketFlags SocketFlags { get; set; }

		/// <summary>Gets or sets a user or application object associated with this asynchronous socket operation.</summary>
		/// <returns>An object that represents the user or application object associated with this asynchronous socket operation.</returns>
		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x06003F74 RID: 16244 RVA: 0x000D8B7F File Offset: 0x000D6D7F
		// (set) Token: 0x06003F75 RID: 16245 RVA: 0x000D8B87 File Offset: 0x000D6D87
		public object UserToken { get; set; }

		/// <summary>The created and connected <see cref="T:System.Net.Sockets.Socket" /> object after successful completion of the <see cref="Overload:System.Net.Sockets.Socket.ConnectAsync" /> method.</summary>
		/// <returns>The connected <see cref="T:System.Net.Sockets.Socket" /> object.</returns>
		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x06003F76 RID: 16246 RVA: 0x000D8B90 File Offset: 0x000D6D90
		public Socket ConnectSocket
		{
			get
			{
				if (this.SocketError == SocketError.AccessDenied)
				{
					return null;
				}
				return this.current_socket;
			}
		}

		/// <summary>The event used to complete an asynchronous operation.</summary>
		// Token: 0x14000076 RID: 118
		// (add) Token: 0x06003F77 RID: 16247 RVA: 0x000D8BA8 File Offset: 0x000D6DA8
		// (remove) Token: 0x06003F78 RID: 16248 RVA: 0x000D8BE0 File Offset: 0x000D6DE0
		public event EventHandler<SocketAsyncEventArgs> Completed;

		/// <summary>Creates an empty <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> instance.</summary>
		/// <exception cref="T:System.NotSupportedException">The platform is not supported.</exception>
		// Token: 0x06003F79 RID: 16249 RVA: 0x000D8C15 File Offset: 0x000D6E15
		public SocketAsyncEventArgs()
		{
			this.SendPacketsSendSize = -1;
		}

		// Token: 0x06003F7A RID: 16250 RVA: 0x000D8C2F File Offset: 0x000D6E2F
		internal SocketAsyncEventArgs(bool flowExecutionContext)
		{
		}

		// Token: 0x06003F7B RID: 16251 RVA: 0x000D8C44 File Offset: 0x000D6E44
		~SocketAsyncEventArgs()
		{
			this.Dispose(false);
		}

		// Token: 0x06003F7C RID: 16252 RVA: 0x000D8C74 File Offset: 0x000D6E74
		private void Dispose(bool disposing)
		{
			this.disposed = true;
			if (disposing)
			{
				int num = this.in_progress;
				return;
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Sockets.SocketAsyncEventArgs" /> instance and optionally disposes of the managed resources.</summary>
		// Token: 0x06003F7D RID: 16253 RVA: 0x000D8C8A File Offset: 0x000D6E8A
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003F7E RID: 16254 RVA: 0x000D8C99 File Offset: 0x000D6E99
		internal void SetConnectByNameError(Exception error)
		{
			this.ConnectByNameError = error;
		}

		// Token: 0x06003F7F RID: 16255 RVA: 0x000D8CA2 File Offset: 0x000D6EA2
		internal void SetBytesTransferred(int value)
		{
			this.BytesTransferred = value;
		}

		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x06003F80 RID: 16256 RVA: 0x000D8CAB File Offset: 0x000D6EAB
		internal Socket CurrentSocket
		{
			get
			{
				return this.current_socket;
			}
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x000D8CB3 File Offset: 0x000D6EB3
		internal void SetCurrentSocket(Socket socket)
		{
			this.current_socket = socket;
		}

		// Token: 0x06003F82 RID: 16258 RVA: 0x000D8CBC File Offset: 0x000D6EBC
		internal void SetLastOperation(SocketAsyncOperation op)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("System.Net.Sockets.SocketAsyncEventArgs");
			}
			if (Interlocked.Exchange(ref this.in_progress, 1) != 0)
			{
				throw new InvalidOperationException("Operation already in progress");
			}
			this.LastOperation = op;
		}

		// Token: 0x06003F83 RID: 16259 RVA: 0x000D8CF1 File Offset: 0x000D6EF1
		internal void Complete_internal()
		{
			this.in_progress = 0;
			this.OnCompleted(this);
		}

		/// <summary>Represents a method that is called when an asynchronous operation completes.</summary>
		/// <param name="e">The event that is signaled.</param>
		// Token: 0x06003F84 RID: 16260 RVA: 0x000D8D04 File Offset: 0x000D6F04
		protected virtual void OnCompleted(SocketAsyncEventArgs e)
		{
			if (e == null)
			{
				return;
			}
			EventHandler<SocketAsyncEventArgs> completed = e.Completed;
			if (completed != null)
			{
				completed(e.current_socket, e);
			}
		}

		// Token: 0x06003F85 RID: 16261 RVA: 0x000D8CB3 File Offset: 0x000D6EB3
		internal void StartOperationCommon(Socket socket)
		{
			this.current_socket = socket;
		}

		// Token: 0x06003F86 RID: 16262 RVA: 0x000D8D2C File Offset: 0x000D6F2C
		internal void StartOperationWrapperConnect(MultipleConnectAsync args)
		{
			this.SetLastOperation(SocketAsyncOperation.Connect);
		}

		// Token: 0x06003F87 RID: 16263 RVA: 0x000D8D35 File Offset: 0x000D6F35
		internal void FinishConnectByNameSyncFailure(Exception exception, int bytesTransferred, SocketFlags flags)
		{
			this.SetResults(exception, bytesTransferred, flags);
			if (this.current_socket != null)
			{
				this.current_socket.is_connected = false;
			}
			this.Complete_internal();
		}

		// Token: 0x06003F88 RID: 16264 RVA: 0x000D8D35 File Offset: 0x000D6F35
		internal void FinishOperationAsyncFailure(Exception exception, int bytesTransferred, SocketFlags flags)
		{
			this.SetResults(exception, bytesTransferred, flags);
			if (this.current_socket != null)
			{
				this.current_socket.is_connected = false;
			}
			this.Complete_internal();
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x000D8D5A File Offset: 0x000D6F5A
		internal void FinishWrapperConnectSuccess(Socket connectSocket, int bytesTransferred, SocketFlags flags)
		{
			this.SetResults(SocketError.Success, bytesTransferred, flags);
			this.current_socket = connectSocket;
			this.Complete_internal();
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x000D8D72 File Offset: 0x000D6F72
		internal void SetResults(SocketError socketError, int bytesTransferred, SocketFlags flags)
		{
			this.SocketError = socketError;
			this.ConnectByNameError = null;
			this.BytesTransferred = bytesTransferred;
			this.SocketFlags = flags;
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x000D8D90 File Offset: 0x000D6F90
		internal void SetResults(Exception exception, int bytesTransferred, SocketFlags flags)
		{
			this.ConnectByNameError = exception;
			this.BytesTransferred = bytesTransferred;
			this.SocketFlags = flags;
			if (exception == null)
			{
				this.SocketError = SocketError.Success;
				return;
			}
			SocketException ex = exception as SocketException;
			if (ex != null)
			{
				this.SocketError = ex.SocketErrorCode;
				return;
			}
			this.SocketError = SocketError.SocketError;
		}

		/// <summary>Gets the data buffer to use with an asynchronous socket method.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array that represents the data buffer to use with an asynchronous socket method.</returns>
		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x06003F8C RID: 16268 RVA: 0x000D8DDC File Offset: 0x000D6FDC
		public byte[] Buffer
		{
			get
			{
				if (this._bufferIsExplicitArray)
				{
					ArraySegment<byte> arraySegment;
					MemoryMarshal.TryGetArray<byte>(this._buffer, out arraySegment);
					return arraySegment.Array;
				}
				return null;
			}
		}

		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x06003F8D RID: 16269 RVA: 0x000D8E0D File Offset: 0x000D700D
		public Memory<byte> MemoryBuffer
		{
			get
			{
				return this._buffer;
			}
		}

		/// <summary>Gets the offset, in bytes, into the data buffer referenced by the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that contains the offset, in bytes, into the data buffer referenced by the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property.</returns>
		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x06003F8E RID: 16270 RVA: 0x000D8E15 File Offset: 0x000D7015
		public int Offset
		{
			get
			{
				return this._offset;
			}
		}

		/// <summary>Gets the maximum amount of data, in bytes, to send or receive in an asynchronous operation.</summary>
		/// <returns>An <see cref="T:System.Int32" /> that contains the maximum amount of data, in bytes, to send or receive.</returns>
		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x06003F8F RID: 16271 RVA: 0x000D8E1D File Offset: 0x000D701D
		public int Count
		{
			get
			{
				return this._count;
			}
		}

		/// <summary>Gets or sets an array of data buffers to use with an asynchronous socket method.</summary>
		/// <returns>An <see cref="T:System.Collections.IList" /> that represents an array of data buffers to use with an asynchronous socket method.</returns>
		/// <exception cref="T:System.ArgumentException">There are ambiguous buffers specified on a set operation. This exception occurs if the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property has been set to a non-null value and an attempt was made to set the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.BufferList" /> property to a non-null value.</exception>
		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x06003F90 RID: 16272 RVA: 0x000D8E25 File Offset: 0x000D7025
		// (set) Token: 0x06003F91 RID: 16273 RVA: 0x000D8E30 File Offset: 0x000D7030
		public IList<ArraySegment<byte>> BufferList
		{
			get
			{
				return this._bufferList;
			}
			set
			{
				if (value != null)
				{
					if (!this._buffer.Equals(default(Memory<byte>)))
					{
						throw new ArgumentException(SR.Format("Buffer and BufferList properties cannot both be non-null.", "Buffer"));
					}
					int count = value.Count;
					if (this._bufferListInternal == null)
					{
						this._bufferListInternal = new List<ArraySegment<byte>>(count);
					}
					else
					{
						this._bufferListInternal.Clear();
					}
					for (int i = 0; i < count; i++)
					{
						ArraySegment<byte> arraySegment = value[i];
						RangeValidationHelpers.ValidateSegment(arraySegment);
						this._bufferListInternal.Add(arraySegment);
					}
				}
				else
				{
					List<ArraySegment<byte>> bufferListInternal = this._bufferListInternal;
					if (bufferListInternal != null)
					{
						bufferListInternal.Clear();
					}
				}
				this._bufferList = value;
			}
		}

		/// <summary>Sets the data buffer to use with an asynchronous socket method.</summary>
		/// <param name="offset">The offset, in bytes, in the data buffer where the operation starts.</param>
		/// <param name="count">The maximum amount of data, in bytes, to send or receive in the buffer.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">An argument was out of range. This exception occurs if the <paramref name="offset" /> parameter is less than zero or greater than the length of the array in the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property. This exception also occurs if the <paramref name="count" /> parameter is less than zero or greater than the length of the array in the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property minus the <paramref name="offset" /> parameter.</exception>
		// Token: 0x06003F92 RID: 16274 RVA: 0x000D8ED4 File Offset: 0x000D70D4
		public void SetBuffer(int offset, int count)
		{
			if (!this._buffer.Equals(default(Memory<byte>)))
			{
				if ((ulong)offset > (ulong)((long)this._buffer.Length))
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				if ((ulong)count > (ulong)((long)(this._buffer.Length - offset)))
				{
					throw new ArgumentOutOfRangeException("count");
				}
				if (!this._bufferIsExplicitArray)
				{
					throw new InvalidOperationException("This operation may only be performed when the buffer was set using the SetBuffer overload that accepts an array.");
				}
				this._offset = offset;
				this._count = count;
			}
		}

		// Token: 0x06003F93 RID: 16275 RVA: 0x000D8F50 File Offset: 0x000D7150
		internal void CopyBufferFrom(SocketAsyncEventArgs source)
		{
			this._buffer = source._buffer;
			this._offset = source._offset;
			this._count = source._count;
			this._bufferIsExplicitArray = source._bufferIsExplicitArray;
		}

		/// <summary>Sets the data buffer to use with an asynchronous socket method.</summary>
		/// <param name="buffer">The data buffer to use with an asynchronous socket method.</param>
		/// <param name="offset">The offset, in bytes, in the data buffer where the operation starts.</param>
		/// <param name="count">The maximum amount of data, in bytes, to send or receive in the buffer.</param>
		/// <exception cref="T:System.ArgumentException">There are ambiguous buffers specified. This exception occurs if the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property is also not null and the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.BufferList" /> property is also not null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">An argument was out of range. This exception occurs if the <paramref name="offset" /> parameter is less than zero or greater than the length of the array in the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property. This exception also occurs if the <paramref name="count" /> parameter is less than zero or greater than the length of the array in the <see cref="P:System.Net.Sockets.SocketAsyncEventArgs.Buffer" /> property minus the <paramref name="offset" /> parameter.</exception>
		// Token: 0x06003F94 RID: 16276 RVA: 0x000D8F84 File Offset: 0x000D7184
		public void SetBuffer(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				this._buffer = default(Memory<byte>);
				this._offset = 0;
				this._count = 0;
				this._bufferIsExplicitArray = false;
				return;
			}
			if (this._bufferList != null)
			{
				throw new ArgumentException(SR.Format("Buffer and BufferList properties cannot both be non-null.", "BufferList"));
			}
			if ((ulong)offset > (ulong)((long)buffer.Length))
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if ((ulong)count > (ulong)((long)(buffer.Length - offset)))
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this._buffer = buffer;
			this._offset = offset;
			this._count = count;
			this._bufferIsExplicitArray = true;
		}

		// Token: 0x06003F95 RID: 16277 RVA: 0x000D901C File Offset: 0x000D721C
		public void SetBuffer(Memory<byte> buffer)
		{
			if (buffer.Length != 0 && this._bufferList != null)
			{
				throw new ArgumentException(SR.Format("Buffer and BufferList properties cannot both be non-null.", "BufferList"));
			}
			this._buffer = buffer;
			this._offset = 0;
			this._count = buffer.Length;
			this._bufferIsExplicitArray = false;
		}

		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x06003F96 RID: 16278 RVA: 0x000D9071 File Offset: 0x000D7271
		internal bool HasMultipleBuffers
		{
			get
			{
				return this._bufferList != null;
			}
		}

		/// <summary>Gets or sets the protocol to use to download the socket client access policy file.</summary>
		/// <returns>The protocol to use to download the socket client access policy file.</returns>
		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x06003F97 RID: 16279 RVA: 0x000D907C File Offset: 0x000D727C
		// (set) Token: 0x06003F98 RID: 16280 RVA: 0x00013BCA File Offset: 0x00011DCA
		public SocketClientAccessPolicyProtocol SocketClientAccessPolicyProtocol
		{
			[CompilerGenerated]
			get
			{
				global::Unity.ThrowStub.ThrowNotSupportedException();
				return SocketClientAccessPolicyProtocol.Tcp;
			}
			[CompilerGenerated]
			set
			{
				global::Unity.ThrowStub.ThrowNotSupportedException();
			}
		}

		// Token: 0x040025DA RID: 9690
		private bool disposed;

		// Token: 0x040025DB RID: 9691
		internal volatile int in_progress;

		// Token: 0x040025DC RID: 9692
		private EndPoint remote_ep;

		// Token: 0x040025DD RID: 9693
		private Socket current_socket;

		// Token: 0x040025DE RID: 9694
		internal SocketAsyncResult socket_async_result = new SocketAsyncResult();

		// Token: 0x040025EC RID: 9708
		private Memory<byte> _buffer;

		// Token: 0x040025ED RID: 9709
		private int _offset;

		// Token: 0x040025EE RID: 9710
		private int _count;

		// Token: 0x040025EF RID: 9711
		private bool _bufferIsExplicitArray;

		// Token: 0x040025F0 RID: 9712
		private IList<ArraySegment<byte>> _bufferList;

		// Token: 0x040025F1 RID: 9713
		private List<ArraySegment<byte>> _bufferListInternal;
	}
}
