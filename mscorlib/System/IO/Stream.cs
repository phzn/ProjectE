using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
	/// <summary>Provides a generic view of a sequence of bytes. This is an abstract class.</summary>
	// Token: 0x02000B4A RID: 2890
	[Serializable]
	public abstract class Stream : MarshalByRefObject, IDisposable, IAsyncDisposable
	{
		// Token: 0x06006850 RID: 26704 RVA: 0x00164C62 File Offset: 0x00162E62
		internal SemaphoreSlim EnsureAsyncActiveSemaphoreInitialized()
		{
			return LazyInitializer.EnsureInitialized<SemaphoreSlim>(ref this._asyncActiveSemaphore, () => new SemaphoreSlim(1, 1));
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the current stream supports reading.</summary>
		/// <returns>
		///   <see langword="true" /> if the stream supports reading; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700120B RID: 4619
		// (get) Token: 0x06006851 RID: 26705
		public abstract bool CanRead { get; }

		/// <summary>When overridden in a derived class, gets a value indicating whether the current stream supports seeking.</summary>
		/// <returns>
		///   <see langword="true" /> if the stream supports seeking; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700120C RID: 4620
		// (get) Token: 0x06006852 RID: 26706
		public abstract bool CanSeek { get; }

		/// <summary>Gets a value that determines whether the current stream can time out.</summary>
		/// <returns>A value that determines whether the current stream can time out.</returns>
		// Token: 0x1700120D RID: 4621
		// (get) Token: 0x06006853 RID: 26707 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public virtual bool CanTimeout
		{
			get
			{
				return false;
			}
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the current stream supports writing.</summary>
		/// <returns>
		///   <see langword="true" /> if the stream supports writing; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700120E RID: 4622
		// (get) Token: 0x06006854 RID: 26708
		public abstract bool CanWrite { get; }

		/// <summary>When overridden in a derived class, gets the length in bytes of the stream.</summary>
		/// <returns>A long value representing the length of the stream in bytes.</returns>
		/// <exception cref="T:System.NotSupportedException">A class derived from <see langword="Stream" /> does not support seeking.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
		// Token: 0x1700120F RID: 4623
		// (get) Token: 0x06006855 RID: 26709
		public abstract long Length { get; }

		/// <summary>When overridden in a derived class, gets or sets the position within the current stream.</summary>
		/// <returns>The current position within the stream.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support seeking.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
		// Token: 0x17001210 RID: 4624
		// (get) Token: 0x06006856 RID: 26710
		// (set) Token: 0x06006857 RID: 26711
		public abstract long Position { get; set; }

		/// <summary>Gets or sets a value, in miliseconds, that determines how long the stream will attempt to read before timing out.</summary>
		/// <returns>A value, in miliseconds, that determines how long the stream will attempt to read before timing out.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.IO.Stream.ReadTimeout" /> method always throws an <see cref="T:System.InvalidOperationException" />.</exception>
		// Token: 0x17001211 RID: 4625
		// (get) Token: 0x06006858 RID: 26712 RVA: 0x00164C8E File Offset: 0x00162E8E
		// (set) Token: 0x06006859 RID: 26713 RVA: 0x00164C8E File Offset: 0x00162E8E
		public virtual int ReadTimeout
		{
			get
			{
				throw new InvalidOperationException("Timeouts are not supported on this stream.");
			}
			set
			{
				throw new InvalidOperationException("Timeouts are not supported on this stream.");
			}
		}

		/// <summary>Gets or sets a value, in miliseconds, that determines how long the stream will attempt to write before timing out.</summary>
		/// <returns>A value, in miliseconds, that determines how long the stream will attempt to write before timing out.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.IO.Stream.WriteTimeout" /> method always throws an <see cref="T:System.InvalidOperationException" />.</exception>
		// Token: 0x17001212 RID: 4626
		// (get) Token: 0x0600685A RID: 26714 RVA: 0x00164C8E File Offset: 0x00162E8E
		// (set) Token: 0x0600685B RID: 26715 RVA: 0x00164C8E File Offset: 0x00162E8E
		public virtual int WriteTimeout
		{
			get
			{
				throw new InvalidOperationException("Timeouts are not supported on this stream.");
			}
			set
			{
				throw new InvalidOperationException("Timeouts are not supported on this stream.");
			}
		}

		/// <summary>Asynchronously reads the bytes from the current stream and writes them to another stream.</summary>
		/// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
		/// <returns>A task that represents the asynchronous copy operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="destination" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Either the current stream or the destination stream is disposed.</exception>
		/// <exception cref="T:System.NotSupportedException">The current stream does not support reading, or the destination stream does not support writing.</exception>
		// Token: 0x0600685C RID: 26716 RVA: 0x00164C9C File Offset: 0x00162E9C
		public Task CopyToAsync(Stream destination)
		{
			int copyBufferSize = this.GetCopyBufferSize();
			return this.CopyToAsync(destination, copyBufferSize);
		}

		/// <summary>Asynchronously reads the bytes from the current stream and writes them to another stream, using a specified buffer size.</summary>
		/// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
		/// <param name="bufferSize">The size, in bytes, of the buffer. This value must be greater than zero. The default size is 81920.</param>
		/// <returns>A task that represents the asynchronous copy operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="destination" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="buffersize" /> is negative or zero.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Either the current stream or the destination stream is disposed.</exception>
		/// <exception cref="T:System.NotSupportedException">The current stream does not support reading, or the destination stream does not support writing.</exception>
		// Token: 0x0600685D RID: 26717 RVA: 0x00164CB8 File Offset: 0x00162EB8
		public Task CopyToAsync(Stream destination, int bufferSize)
		{
			return this.CopyToAsync(destination, bufferSize, CancellationToken.None);
		}

		// Token: 0x0600685E RID: 26718 RVA: 0x00164CC8 File Offset: 0x00162EC8
		public Task CopyToAsync(Stream destination, CancellationToken cancellationToken)
		{
			int copyBufferSize = this.GetCopyBufferSize();
			return this.CopyToAsync(destination, copyBufferSize, cancellationToken);
		}

		/// <summary>Asynchronously reads the bytes from the current stream and writes them to another stream, using a specified buffer size and cancellation token.</summary>
		/// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
		/// <param name="bufferSize">The size, in bytes, of the buffer. This value must be greater than zero. The default size is 81920.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
		/// <returns>A task that represents the asynchronous copy operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="destination" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="buffersize" /> is negative or zero.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Either the current stream or the destination stream is disposed.</exception>
		/// <exception cref="T:System.NotSupportedException">The current stream does not support reading, or the destination stream does not support writing.</exception>
		// Token: 0x0600685F RID: 26719 RVA: 0x00164CE5 File Offset: 0x00162EE5
		public virtual Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			StreamHelpers.ValidateCopyToArgs(this, destination, bufferSize);
			return this.CopyToAsyncInternal(destination, bufferSize, cancellationToken);
		}

		// Token: 0x06006860 RID: 26720 RVA: 0x00164CF8 File Offset: 0x00162EF8
		private async Task CopyToAsyncInternal(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
			try
			{
				for (;;)
				{
					int num = await this.ReadAsync(new Memory<byte>(buffer), cancellationToken).ConfigureAwait(false);
					if (num == 0)
					{
						break;
					}
					await destination.WriteAsync(new ReadOnlyMemory<byte>(buffer, 0, num), cancellationToken).ConfigureAwait(false);
				}
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(buffer, false);
			}
		}

		/// <summary>Reads the bytes from the current stream and writes them to another stream.</summary>
		/// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="destination" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.NotSupportedException">The current stream does not support reading.  
		///  -or-  
		///  <paramref name="destination" /> does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Either the current stream or <paramref name="destination" /> were closed before the <see cref="M:System.IO.Stream.CopyTo(System.IO.Stream)" /> method was called.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x06006861 RID: 26721 RVA: 0x00164D54 File Offset: 0x00162F54
		public void CopyTo(Stream destination)
		{
			int copyBufferSize = this.GetCopyBufferSize();
			this.CopyTo(destination, copyBufferSize);
		}

		/// <summary>Reads the bytes from the current stream and writes them to another stream, using a specified buffer size.</summary>
		/// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
		/// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="destination" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="bufferSize" /> is negative or zero.</exception>
		/// <exception cref="T:System.NotSupportedException">The current stream does not support reading.  
		///  -or-  
		///  <paramref name="destination" /> does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Either the current stream or <paramref name="destination" /> were closed before the <see cref="M:System.IO.Stream.CopyTo(System.IO.Stream)" /> method was called.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
		// Token: 0x06006862 RID: 26722 RVA: 0x00164D70 File Offset: 0x00162F70
		public virtual void CopyTo(Stream destination, int bufferSize)
		{
			StreamHelpers.ValidateCopyToArgs(this, destination, bufferSize);
			byte[] array = ArrayPool<byte>.Shared.Rent(bufferSize);
			try
			{
				int num;
				while ((num = this.Read(array, 0, array.Length)) != 0)
				{
					destination.Write(array, 0, num);
				}
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(array, false);
			}
		}

		// Token: 0x06006863 RID: 26723 RVA: 0x00164DCC File Offset: 0x00162FCC
		private int GetCopyBufferSize()
		{
			int num = 81920;
			if (this.CanSeek)
			{
				long length = this.Length;
				long position = this.Position;
				if (length <= position)
				{
					num = 1;
				}
				else
				{
					long num2 = length - position;
					if (num2 > 0L)
					{
						num = (int)Math.Min((long)num, num2);
					}
				}
			}
			return num;
		}

		/// <summary>Closes the current stream and releases any resources (such as sockets and file handles) associated with the current stream. Instead of calling this method, ensure that the stream is properly disposed.</summary>
		// Token: 0x06006864 RID: 26724 RVA: 0x00164E11 File Offset: 0x00163011
		public virtual void Close()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases all resources used by the <see cref="T:System.IO.Stream" />.</summary>
		// Token: 0x06006865 RID: 26725 RVA: 0x000A4741 File Offset: 0x000A2941
		public void Dispose()
		{
			this.Close();
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.Stream" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x06006866 RID: 26726 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void Dispose(bool disposing)
		{
		}

		/// <summary>When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.</summary>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x06006867 RID: 26727
		public abstract void Flush();

		/// <summary>Asynchronously clears all buffers for this stream and causes any buffered data to be written to the underlying device.</summary>
		/// <returns>A task that represents the asynchronous flush operation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
		// Token: 0x06006868 RID: 26728 RVA: 0x00164E20 File Offset: 0x00163020
		public Task FlushAsync()
		{
			return this.FlushAsync(CancellationToken.None);
		}

		/// <summary>Asynchronously clears all buffers for this stream, causes any buffered data to be written to the underlying device, and monitors cancellation requests.</summary>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
		/// <returns>A task that represents the asynchronous flush operation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
		// Token: 0x06006869 RID: 26729 RVA: 0x00164E2D File Offset: 0x0016302D
		public virtual Task FlushAsync(CancellationToken cancellationToken)
		{
			return Task.Factory.StartNew(delegate(object state)
			{
				((Stream)state).Flush();
			}, this, cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
		}

		/// <summary>Allocates a <see cref="T:System.Threading.WaitHandle" /> object.</summary>
		/// <returns>A reference to the allocated <see langword="WaitHandle" />.</returns>
		// Token: 0x0600686A RID: 26730 RVA: 0x00164E60 File Offset: 0x00163060
		[Obsolete("CreateWaitHandle will be removed eventually.  Please use \"new ManualResetEvent(false)\" instead.")]
		protected virtual WaitHandle CreateWaitHandle()
		{
			return new ManualResetEvent(false);
		}

		/// <summary>Begins an asynchronous read operation. (Consider using <see cref="M:System.IO.Stream.ReadAsync(System.Byte[],System.Int32,System.Int32)" /> instead.)</summary>
		/// <param name="buffer">The buffer to read the data into.</param>
		/// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin writing data read from the stream.</param>
		/// <param name="count">The maximum number of bytes to read.</param>
		/// <param name="callback">An optional asynchronous callback, to be called when the read is complete.</param>
		/// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that represents the asynchronous read, which could still be pending.</returns>
		/// <exception cref="T:System.IO.IOException">Attempted an asynchronous read past the end of the stream, or a disk error occurs.</exception>
		/// <exception cref="T:System.ArgumentException">One or more of the arguments is invalid.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
		/// <exception cref="T:System.NotSupportedException">The current <see langword="Stream" /> implementation does not support the read operation.</exception>
		// Token: 0x0600686B RID: 26731 RVA: 0x00164E68 File Offset: 0x00163068
		public virtual IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.BeginReadInternal(buffer, offset, count, callback, state, false, true);
		}

		// Token: 0x0600686C RID: 26732 RVA: 0x00164E7C File Offset: 0x0016307C
		internal IAsyncResult BeginReadInternal(byte[] buffer, int offset, int count, AsyncCallback callback, object state, bool serializeAsynchronously, bool apm)
		{
			if (!this.CanRead)
			{
				throw Error.GetReadNotSupported();
			}
			SemaphoreSlim semaphoreSlim = this.EnsureAsyncActiveSemaphoreInitialized();
			Task task = null;
			if (serializeAsynchronously)
			{
				task = semaphoreSlim.WaitAsync();
			}
			else
			{
				semaphoreSlim.Wait();
			}
			Stream.ReadWriteTask readWriteTask = new Stream.ReadWriteTask(true, apm, delegate
			{
				Stream.ReadWriteTask readWriteTask2 = Task.InternalCurrent as Stream.ReadWriteTask;
				int num;
				try
				{
					num = readWriteTask2._stream.Read(readWriteTask2._buffer, readWriteTask2._offset, readWriteTask2._count);
				}
				finally
				{
					if (!readWriteTask2._apm)
					{
						readWriteTask2._stream.FinishTrackingAsyncOperation();
					}
					readWriteTask2.ClearBeginState();
				}
				return num;
			}, state, this, buffer, offset, count, callback);
			if (task != null)
			{
				this.RunReadWriteTaskWhenReady(task, readWriteTask);
			}
			else
			{
				this.RunReadWriteTask(readWriteTask);
			}
			return readWriteTask;
		}

		/// <summary>Waits for the pending asynchronous read to complete. (Consider using <see cref="M:System.IO.Stream.ReadAsync(System.Byte[],System.Int32,System.Int32)" /> instead.)</summary>
		/// <param name="asyncResult">The reference to the pending asynchronous request to finish.</param>
		/// <returns>The number of bytes read from the stream, between zero (0) and the number of bytes you requested. Streams return zero (0) only at the end of the stream, otherwise, they should block until at least one byte is available.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">A handle to the pending read operation is not available.  
		///  -or-  
		///  The pending operation does not support reading.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="asyncResult" /> did not originate from a <see cref="M:System.IO.Stream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> method on the current stream.</exception>
		/// <exception cref="T:System.IO.IOException">The stream is closed or an internal error has occurred.</exception>
		// Token: 0x0600686D RID: 26733 RVA: 0x00164EF8 File Offset: 0x001630F8
		public virtual int EndRead(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			Stream.ReadWriteTask activeReadWriteTask = this._activeReadWriteTask;
			if (activeReadWriteTask == null)
			{
				throw new ArgumentException("Either the IAsyncResult object did not come from the corresponding async method on this type, or EndRead was called multiple times with the same IAsyncResult.");
			}
			if (activeReadWriteTask != asyncResult)
			{
				throw new InvalidOperationException("Either the IAsyncResult object did not come from the corresponding async method on this type, or EndRead was called multiple times with the same IAsyncResult.");
			}
			if (!activeReadWriteTask._isRead)
			{
				throw new ArgumentException("Either the IAsyncResult object did not come from the corresponding async method on this type, or EndRead was called multiple times with the same IAsyncResult.");
			}
			int result;
			try
			{
				result = activeReadWriteTask.GetAwaiter().GetResult();
			}
			finally
			{
				this.FinishTrackingAsyncOperation();
			}
			return result;
		}

		/// <summary>Asynchronously reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.</summary>
		/// <param name="buffer">The buffer to write the data into.</param>
		/// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin writing data from the stream.</param>
		/// <param name="count">The maximum number of bytes to read.</param>
		/// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains the total number of bytes read into the buffer. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the stream has been reached.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset" /> and <paramref name="count" /> is larger than the buffer length.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The stream is currently in use by a previous read operation.</exception>
		// Token: 0x0600686E RID: 26734 RVA: 0x00164F74 File Offset: 0x00163174
		public Task<int> ReadAsync(byte[] buffer, int offset, int count)
		{
			return this.ReadAsync(buffer, offset, count, CancellationToken.None);
		}

		/// <summary>Asynchronously reads a sequence of bytes from the current stream, advances the position within the stream by the number of bytes read, and monitors cancellation requests.</summary>
		/// <param name="buffer">The buffer to write the data into.</param>
		/// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin writing data from the stream.</param>
		/// <param name="count">The maximum number of bytes to read.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
		/// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains the total number of bytes read into the buffer. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the stream has been reached.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset" /> and <paramref name="count" /> is larger than the buffer length.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The stream is currently in use by a previous read operation.</exception>
		// Token: 0x0600686F RID: 26735 RVA: 0x00164F84 File Offset: 0x00163184
		public virtual Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				return this.BeginEndReadAsync(buffer, offset, count);
			}
			return Task.FromCanceled<int>(cancellationToken);
		}

		// Token: 0x06006870 RID: 26736 RVA: 0x00164FA0 File Offset: 0x001631A0
		public virtual ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			ArraySegment<byte> arraySegment;
			if (MemoryMarshal.TryGetArray<byte>(buffer, out arraySegment))
			{
				return new ValueTask<int>(this.ReadAsync(arraySegment.Array, arraySegment.Offset, arraySegment.Count, cancellationToken));
			}
			byte[] array = ArrayPool<byte>.Shared.Rent(buffer.Length);
			return Stream.<ReadAsync>g__FinishReadAsync|44_0(this.ReadAsync(array, 0, buffer.Length, cancellationToken), array, buffer);
		}

		// Token: 0x06006871 RID: 26737 RVA: 0x00165008 File Offset: 0x00163208
		private Task<int> BeginEndReadAsync(byte[] buffer, int offset, int count)
		{
			if (!this.HasOverriddenBeginEndRead())
			{
				return (Task<int>)this.BeginReadInternal(buffer, offset, count, null, null, true, false);
			}
			return TaskFactory<int>.FromAsyncTrim<Stream, Stream.ReadWriteParameters>(this, new Stream.ReadWriteParameters
			{
				Buffer = buffer,
				Offset = offset,
				Count = count
			}, (Stream stream, Stream.ReadWriteParameters args, AsyncCallback callback, object state) => stream.BeginRead(args.Buffer, args.Offset, args.Count, callback, state), (Stream stream, IAsyncResult asyncResult) => stream.EndRead(asyncResult));
		}

		/// <summary>Begins an asynchronous write operation. (Consider using <see cref="M:System.IO.Stream.WriteAsync(System.Byte[],System.Int32,System.Int32)" /> instead.)</summary>
		/// <param name="buffer">The buffer to write data from.</param>
		/// <param name="offset">The byte offset in <paramref name="buffer" /> from which to begin writing.</param>
		/// <param name="count">The maximum number of bytes to write.</param>
		/// <param name="callback">An optional asynchronous callback, to be called when the write is complete.</param>
		/// <param name="state">A user-provided object that distinguishes this particular asynchronous write request from other requests.</param>
		/// <returns>An <see langword="IAsyncResult" /> that represents the asynchronous write, which could still be pending.</returns>
		/// <exception cref="T:System.IO.IOException">Attempted an asynchronous write past the end of the stream, or a disk error occurs.</exception>
		/// <exception cref="T:System.ArgumentException">One or more of the arguments is invalid.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
		/// <exception cref="T:System.NotSupportedException">The current <see langword="Stream" /> implementation does not support the write operation.</exception>
		// Token: 0x06006872 RID: 26738 RVA: 0x00165095 File Offset: 0x00163295
		public virtual IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.BeginWriteInternal(buffer, offset, count, callback, state, false, true);
		}

		// Token: 0x06006873 RID: 26739 RVA: 0x001650A8 File Offset: 0x001632A8
		internal IAsyncResult BeginWriteInternal(byte[] buffer, int offset, int count, AsyncCallback callback, object state, bool serializeAsynchronously, bool apm)
		{
			if (!this.CanWrite)
			{
				throw Error.GetWriteNotSupported();
			}
			SemaphoreSlim semaphoreSlim = this.EnsureAsyncActiveSemaphoreInitialized();
			Task task = null;
			if (serializeAsynchronously)
			{
				task = semaphoreSlim.WaitAsync();
			}
			else
			{
				semaphoreSlim.Wait();
			}
			Stream.ReadWriteTask readWriteTask = new Stream.ReadWriteTask(false, apm, delegate
			{
				Stream.ReadWriteTask readWriteTask2 = Task.InternalCurrent as Stream.ReadWriteTask;
				int num;
				try
				{
					readWriteTask2._stream.Write(readWriteTask2._buffer, readWriteTask2._offset, readWriteTask2._count);
					num = 0;
				}
				finally
				{
					if (!readWriteTask2._apm)
					{
						readWriteTask2._stream.FinishTrackingAsyncOperation();
					}
					readWriteTask2.ClearBeginState();
				}
				return num;
			}, state, this, buffer, offset, count, callback);
			if (task != null)
			{
				this.RunReadWriteTaskWhenReady(task, readWriteTask);
			}
			else
			{
				this.RunReadWriteTask(readWriteTask);
			}
			return readWriteTask;
		}

		// Token: 0x06006874 RID: 26740 RVA: 0x00165124 File Offset: 0x00163324
		private void RunReadWriteTaskWhenReady(Task asyncWaiter, Stream.ReadWriteTask readWriteTask)
		{
			if (asyncWaiter.IsCompleted)
			{
				this.RunReadWriteTask(readWriteTask);
				return;
			}
			asyncWaiter.ContinueWith(delegate(Task t, object state)
			{
				Stream.ReadWriteTask readWriteTask2 = (Stream.ReadWriteTask)state;
				readWriteTask2._stream.RunReadWriteTask(readWriteTask2);
			}, readWriteTask, default(CancellationToken), TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
		}

		// Token: 0x06006875 RID: 26741 RVA: 0x0016517B File Offset: 0x0016337B
		private void RunReadWriteTask(Stream.ReadWriteTask readWriteTask)
		{
			this._activeReadWriteTask = readWriteTask;
			readWriteTask.m_taskScheduler = TaskScheduler.Default;
			readWriteTask.ScheduleAndStart(false);
		}

		// Token: 0x06006876 RID: 26742 RVA: 0x00165196 File Offset: 0x00163396
		private void FinishTrackingAsyncOperation()
		{
			this._activeReadWriteTask = null;
			this._asyncActiveSemaphore.Release();
		}

		/// <summary>Ends an asynchronous write operation. (Consider using <see cref="M:System.IO.Stream.WriteAsync(System.Byte[],System.Int32,System.Int32)" /> instead.)</summary>
		/// <param name="asyncResult">A reference to the outstanding asynchronous I/O request.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">A handle to the pending write operation is not available.  
		///  -or-  
		///  The pending operation does not support writing.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="asyncResult" /> did not originate from a <see cref="M:System.IO.Stream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> method on the current stream.</exception>
		/// <exception cref="T:System.IO.IOException">The stream is closed or an internal error has occurred.</exception>
		// Token: 0x06006877 RID: 26743 RVA: 0x001651AC File Offset: 0x001633AC
		public virtual void EndWrite(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			Stream.ReadWriteTask activeReadWriteTask = this._activeReadWriteTask;
			if (activeReadWriteTask == null)
			{
				throw new ArgumentException("Either the IAsyncResult object did not come from the corresponding async method on this type, or EndWrite was called multiple times with the same IAsyncResult.");
			}
			if (activeReadWriteTask != asyncResult)
			{
				throw new InvalidOperationException("Either the IAsyncResult object did not come from the corresponding async method on this type, or EndWrite was called multiple times with the same IAsyncResult.");
			}
			if (activeReadWriteTask._isRead)
			{
				throw new ArgumentException("Either the IAsyncResult object did not come from the corresponding async method on this type, or EndWrite was called multiple times with the same IAsyncResult.");
			}
			try
			{
				activeReadWriteTask.GetAwaiter().GetResult();
			}
			finally
			{
				this.FinishTrackingAsyncOperation();
			}
		}

		/// <summary>Asynchronously writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.</summary>
		/// <param name="buffer">The buffer to write data from.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> from which to begin copying bytes to the stream.</param>
		/// <param name="count">The maximum number of bytes to write.</param>
		/// <returns>A task that represents the asynchronous write operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset" /> and <paramref name="count" /> is larger than the buffer length.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The stream is currently in use by a previous write operation.</exception>
		// Token: 0x06006878 RID: 26744 RVA: 0x00165228 File Offset: 0x00163428
		public Task WriteAsync(byte[] buffer, int offset, int count)
		{
			return this.WriteAsync(buffer, offset, count, CancellationToken.None);
		}

		/// <summary>Asynchronously writes a sequence of bytes to the current stream, advances the current position within this stream by the number of bytes written, and monitors cancellation requests.</summary>
		/// <param name="buffer">The buffer to write data from.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> from which to begin copying bytes to the stream.</param>
		/// <param name="count">The maximum number of bytes to write.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
		/// <returns>A task that represents the asynchronous write operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset" /> and <paramref name="count" /> is larger than the buffer length.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The stream is currently in use by a previous write operation.</exception>
		// Token: 0x06006879 RID: 26745 RVA: 0x00165238 File Offset: 0x00163438
		public virtual Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				return this.BeginEndWriteAsync(buffer, offset, count);
			}
			return Task.FromCanceled(cancellationToken);
		}

		// Token: 0x0600687A RID: 26746 RVA: 0x00165254 File Offset: 0x00163454
		public virtual ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			ArraySegment<byte> arraySegment;
			if (MemoryMarshal.TryGetArray<byte>(buffer, out arraySegment))
			{
				return new ValueTask(this.WriteAsync(arraySegment.Array, arraySegment.Offset, arraySegment.Count, cancellationToken));
			}
			byte[] array = ArrayPool<byte>.Shared.Rent(buffer.Length);
			buffer.Span.CopyTo(array);
			return new ValueTask(this.FinishWriteAsync(this.WriteAsync(array, 0, buffer.Length, cancellationToken), array));
		}

		// Token: 0x0600687B RID: 26747 RVA: 0x001652D0 File Offset: 0x001634D0
		private async Task FinishWriteAsync(Task writeTask, byte[] localBuffer)
		{
			try
			{
				await writeTask.ConfigureAwait(false);
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(localBuffer, false);
			}
		}

		// Token: 0x0600687C RID: 26748 RVA: 0x0016531C File Offset: 0x0016351C
		private Task BeginEndWriteAsync(byte[] buffer, int offset, int count)
		{
			if (!this.HasOverriddenBeginEndWrite())
			{
				return (Task)this.BeginWriteInternal(buffer, offset, count, null, null, true, false);
			}
			return TaskFactory<VoidTaskResult>.FromAsyncTrim<Stream, Stream.ReadWriteParameters>(this, new Stream.ReadWriteParameters
			{
				Buffer = buffer,
				Offset = offset,
				Count = count
			}, (Stream stream, Stream.ReadWriteParameters args, AsyncCallback callback, object state) => stream.BeginWrite(args.Buffer, args.Offset, args.Count, callback, state), delegate(Stream stream, IAsyncResult asyncResult)
			{
				stream.EndWrite(asyncResult);
				return default(VoidTaskResult);
			});
		}

		/// <summary>When overridden in a derived class, sets the position within the current stream.</summary>
		/// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
		/// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to obtain the new position.</param>
		/// <returns>The new position within the current stream.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support seeking, such as if the stream is constructed from a pipe or console output.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
		// Token: 0x0600687D RID: 26749
		public abstract long Seek(long offset, SeekOrigin origin);

		/// <summary>When overridden in a derived class, sets the length of the current stream.</summary>
		/// <param name="value">The desired length of the current stream in bytes.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
		// Token: 0x0600687E RID: 26750
		public abstract void SetLength(long value);

		/// <summary>When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.</summary>
		/// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the bytes read from the current source.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
		/// <param name="count">The maximum number of bytes to be read from the current stream.</param>
		/// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
		/// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset" /> and <paramref name="count" /> is larger than the buffer length.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
		// Token: 0x0600687F RID: 26751
		public abstract int Read(byte[] buffer, int offset, int count);

		// Token: 0x06006880 RID: 26752 RVA: 0x001653AC File Offset: 0x001635AC
		public virtual int Read(Span<byte> buffer)
		{
			byte[] array = ArrayPool<byte>.Shared.Rent(buffer.Length);
			int num2;
			try
			{
				int num = this.Read(array, 0, buffer.Length);
				if ((ulong)num > (ulong)((long)buffer.Length))
				{
					throw new IOException("Stream was too long.");
				}
				new Span<byte>(array, 0, num).CopyTo(buffer);
				num2 = num;
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(array, false);
			}
			return num2;
		}

		/// <summary>Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream.</summary>
		/// <returns>The unsigned byte cast to an <see langword="Int32" />, or -1 if at the end of the stream.</returns>
		/// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
		// Token: 0x06006881 RID: 26753 RVA: 0x00165428 File Offset: 0x00163628
		public virtual int ReadByte()
		{
			byte[] array = new byte[1];
			if (this.Read(array, 0, 1) == 0)
			{
				return -1;
			}
			return (int)array[0];
		}

		/// <summary>When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.</summary>
		/// <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from <paramref name="buffer" /> to the current stream.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
		/// <param name="count">The number of bytes to be written to the current stream.</param>
		/// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset" /> and <paramref name="count" /> is greater than the buffer length.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occured, such as the specified file cannot be found.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///   <see cref="M:System.IO.Stream.Write(System.Byte[],System.Int32,System.Int32)" /> was called after the stream was closed.</exception>
		// Token: 0x06006882 RID: 26754
		public abstract void Write(byte[] buffer, int offset, int count);

		// Token: 0x06006883 RID: 26755 RVA: 0x0016544C File Offset: 0x0016364C
		public virtual void Write(ReadOnlySpan<byte> buffer)
		{
			byte[] array = ArrayPool<byte>.Shared.Rent(buffer.Length);
			try
			{
				buffer.CopyTo(array);
				this.Write(array, 0, buffer.Length);
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(array, false);
			}
		}

		/// <summary>Writes a byte to the current position in the stream and advances the position within the stream by one byte.</summary>
		/// <param name="value">The byte to write to the stream.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support writing, or the stream is already closed.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
		// Token: 0x06006884 RID: 26756 RVA: 0x001654A8 File Offset: 0x001636A8
		public virtual void WriteByte(byte value)
		{
			this.Write(new byte[] { value }, 0, 1);
		}

		/// <summary>Creates a thread-safe (synchronized) wrapper around the specified <see cref="T:System.IO.Stream" /> object.</summary>
		/// <param name="stream">The <see cref="T:System.IO.Stream" /> object to synchronize.</param>
		/// <returns>A thread-safe <see cref="T:System.IO.Stream" /> object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> is <see langword="null" />.</exception>
		// Token: 0x06006885 RID: 26757 RVA: 0x001654C9 File Offset: 0x001636C9
		public static Stream Synchronized(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (stream is Stream.SyncStream)
			{
				return stream;
			}
			return new Stream.SyncStream(stream);
		}

		/// <summary>Provides support for a <see cref="T:System.Diagnostics.Contracts.Contract" />.</summary>
		// Token: 0x06006886 RID: 26758 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[Obsolete("Do not call or override this method.")]
		protected virtual void ObjectInvariant()
		{
		}

		// Token: 0x06006887 RID: 26759 RVA: 0x001654EC File Offset: 0x001636EC
		internal IAsyncResult BlockingBeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			Stream.SynchronousAsyncResult synchronousAsyncResult;
			try
			{
				synchronousAsyncResult = new Stream.SynchronousAsyncResult(this.Read(buffer, offset, count), state);
			}
			catch (IOException ex)
			{
				synchronousAsyncResult = new Stream.SynchronousAsyncResult(ex, state, false);
			}
			if (callback != null)
			{
				callback(synchronousAsyncResult);
			}
			return synchronousAsyncResult;
		}

		// Token: 0x06006888 RID: 26760 RVA: 0x00165534 File Offset: 0x00163734
		internal static int BlockingEndRead(IAsyncResult asyncResult)
		{
			return Stream.SynchronousAsyncResult.EndRead(asyncResult);
		}

		// Token: 0x06006889 RID: 26761 RVA: 0x0016553C File Offset: 0x0016373C
		internal IAsyncResult BlockingBeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			Stream.SynchronousAsyncResult synchronousAsyncResult;
			try
			{
				this.Write(buffer, offset, count);
				synchronousAsyncResult = new Stream.SynchronousAsyncResult(state);
			}
			catch (IOException ex)
			{
				synchronousAsyncResult = new Stream.SynchronousAsyncResult(ex, state, true);
			}
			if (callback != null)
			{
				callback(synchronousAsyncResult);
			}
			return synchronousAsyncResult;
		}

		// Token: 0x0600688A RID: 26762 RVA: 0x00165584 File Offset: 0x00163784
		internal static void BlockingEndWrite(IAsyncResult asyncResult)
		{
			Stream.SynchronousAsyncResult.EndWrite(asyncResult);
		}

		// Token: 0x0600688B RID: 26763 RVA: 0x000040F7 File Offset: 0x000022F7
		private bool HasOverriddenBeginEndRead()
		{
			return true;
		}

		// Token: 0x0600688C RID: 26764 RVA: 0x000040F7 File Offset: 0x000022F7
		private bool HasOverriddenBeginEndWrite()
		{
			return true;
		}

		// Token: 0x0600688D RID: 26765 RVA: 0x0016558C File Offset: 0x0016378C
		public virtual ValueTask DisposeAsync()
		{
			ValueTask valueTask;
			try
			{
				this.Dispose();
				valueTask = default(ValueTask);
				valueTask = valueTask;
			}
			catch (Exception ex)
			{
				valueTask = new ValueTask(Task.FromException(ex));
			}
			return valueTask;
		}

		// Token: 0x06006890 RID: 26768 RVA: 0x001655D8 File Offset: 0x001637D8
		[CompilerGenerated]
		internal static async ValueTask<int> <ReadAsync>g__FinishReadAsync|44_0(Task<int> readTask, byte[] localBuffer, Memory<byte> localDestination)
		{
			int num2;
			try
			{
				int num = await readTask.ConfigureAwait(false);
				new Span<byte>(localBuffer, 0, num).CopyTo(localDestination.Span);
				num2 = num;
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(localBuffer, false);
			}
			return num2;
		}

		/// <summary>A <see langword="Stream" /> with no backing store.</summary>
		// Token: 0x04003CD1 RID: 15569
		public static readonly Stream Null = new Stream.NullStream();

		// Token: 0x04003CD2 RID: 15570
		private const int DefaultCopyBufferSize = 81920;

		// Token: 0x04003CD3 RID: 15571
		[NonSerialized]
		private Stream.ReadWriteTask _activeReadWriteTask;

		// Token: 0x04003CD4 RID: 15572
		[NonSerialized]
		private SemaphoreSlim _asyncActiveSemaphore;

		// Token: 0x02000B4B RID: 2891
		private struct ReadWriteParameters
		{
			// Token: 0x04003CD5 RID: 15573
			internal byte[] Buffer;

			// Token: 0x04003CD6 RID: 15574
			internal int Offset;

			// Token: 0x04003CD7 RID: 15575
			internal int Count;
		}

		// Token: 0x02000B4C RID: 2892
		private sealed class ReadWriteTask : Task<int>, ITaskCompletionAction
		{
			// Token: 0x06006891 RID: 26769 RVA: 0x0016562B File Offset: 0x0016382B
			internal void ClearBeginState()
			{
				this._stream = null;
				this._buffer = null;
			}

			// Token: 0x06006892 RID: 26770 RVA: 0x0016563C File Offset: 0x0016383C
			public ReadWriteTask(bool isRead, bool apm, Func<object, int> function, object state, Stream stream, byte[] buffer, int offset, int count, AsyncCallback callback)
				: base(function, state, CancellationToken.None, TaskCreationOptions.DenyChildAttach)
			{
				this._isRead = isRead;
				this._apm = apm;
				this._stream = stream;
				this._buffer = buffer;
				this._offset = offset;
				this._count = count;
				if (callback != null)
				{
					this._callback = callback;
					this._context = ExecutionContext.Capture();
					base.AddCompletionAction(this);
				}
			}

			// Token: 0x06006893 RID: 26771 RVA: 0x001656A4 File Offset: 0x001638A4
			private static void InvokeAsyncCallback(object completedTask)
			{
				Stream.ReadWriteTask readWriteTask = (Stream.ReadWriteTask)completedTask;
				AsyncCallback callback = readWriteTask._callback;
				readWriteTask._callback = null;
				callback(readWriteTask);
			}

			// Token: 0x06006894 RID: 26772 RVA: 0x001656CC File Offset: 0x001638CC
			void ITaskCompletionAction.Invoke(Task completingTask)
			{
				ExecutionContext context = this._context;
				if (context == null)
				{
					AsyncCallback callback = this._callback;
					this._callback = null;
					callback(completingTask);
					return;
				}
				this._context = null;
				ContextCallback contextCallback = Stream.ReadWriteTask.s_invokeAsyncCallback;
				if (contextCallback == null)
				{
					contextCallback = (Stream.ReadWriteTask.s_invokeAsyncCallback = new ContextCallback(Stream.ReadWriteTask.InvokeAsyncCallback));
				}
				ExecutionContext.RunInternal(context, contextCallback, this);
			}

			// Token: 0x17001213 RID: 4627
			// (get) Token: 0x06006895 RID: 26773 RVA: 0x000040F7 File Offset: 0x000022F7
			bool ITaskCompletionAction.InvokeMayRunArbitraryCode
			{
				get
				{
					return true;
				}
			}

			// Token: 0x04003CD8 RID: 15576
			internal readonly bool _isRead;

			// Token: 0x04003CD9 RID: 15577
			internal readonly bool _apm;

			// Token: 0x04003CDA RID: 15578
			internal Stream _stream;

			// Token: 0x04003CDB RID: 15579
			internal byte[] _buffer;

			// Token: 0x04003CDC RID: 15580
			internal readonly int _offset;

			// Token: 0x04003CDD RID: 15581
			internal readonly int _count;

			// Token: 0x04003CDE RID: 15582
			private AsyncCallback _callback;

			// Token: 0x04003CDF RID: 15583
			private ExecutionContext _context;

			// Token: 0x04003CE0 RID: 15584
			private static ContextCallback s_invokeAsyncCallback;
		}

		// Token: 0x02000B4D RID: 2893
		private sealed class NullStream : Stream
		{
			// Token: 0x06006896 RID: 26774 RVA: 0x00165722 File Offset: 0x00163922
			internal NullStream()
			{
			}

			// Token: 0x17001214 RID: 4628
			// (get) Token: 0x06006897 RID: 26775 RVA: 0x000040F7 File Offset: 0x000022F7
			public override bool CanRead
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17001215 RID: 4629
			// (get) Token: 0x06006898 RID: 26776 RVA: 0x000040F7 File Offset: 0x000022F7
			public override bool CanWrite
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17001216 RID: 4630
			// (get) Token: 0x06006899 RID: 26777 RVA: 0x000040F7 File Offset: 0x000022F7
			public override bool CanSeek
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17001217 RID: 4631
			// (get) Token: 0x0600689A RID: 26778 RVA: 0x0005CD52 File Offset: 0x0005AF52
			public override long Length
			{
				get
				{
					return 0L;
				}
			}

			// Token: 0x17001218 RID: 4632
			// (get) Token: 0x0600689B RID: 26779 RVA: 0x0005CD52 File Offset: 0x0005AF52
			// (set) Token: 0x0600689C RID: 26780 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public override long Position
			{
				get
				{
					return 0L;
				}
				set
				{
				}
			}

			// Token: 0x0600689D RID: 26781 RVA: 0x0016572A File Offset: 0x0016392A
			public override void CopyTo(Stream destination, int bufferSize)
			{
				StreamHelpers.ValidateCopyToArgs(this, destination, bufferSize);
			}

			// Token: 0x0600689E RID: 26782 RVA: 0x00165734 File Offset: 0x00163934
			public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
			{
				StreamHelpers.ValidateCopyToArgs(this, destination, bufferSize);
				if (!cancellationToken.IsCancellationRequested)
				{
					return Task.CompletedTask;
				}
				return Task.FromCanceled(cancellationToken);
			}

			// Token: 0x0600689F RID: 26783 RVA: 0x00004BF9 File Offset: 0x00002DF9
			protected override void Dispose(bool disposing)
			{
			}

			// Token: 0x060068A0 RID: 26784 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public override void Flush()
			{
			}

			// Token: 0x060068A1 RID: 26785 RVA: 0x00165753 File Offset: 0x00163953
			public override Task FlushAsync(CancellationToken cancellationToken)
			{
				if (!cancellationToken.IsCancellationRequested)
				{
					return Task.CompletedTask;
				}
				return Task.FromCanceled(cancellationToken);
			}

			// Token: 0x060068A2 RID: 26786 RVA: 0x0016576A File Offset: 0x0016396A
			public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				if (!this.CanRead)
				{
					throw Error.GetReadNotSupported();
				}
				return base.BlockingBeginRead(buffer, offset, count, callback, state);
			}

			// Token: 0x060068A3 RID: 26787 RVA: 0x00165787 File Offset: 0x00163987
			public override int EndRead(IAsyncResult asyncResult)
			{
				if (asyncResult == null)
				{
					throw new ArgumentNullException("asyncResult");
				}
				return Stream.BlockingEndRead(asyncResult);
			}

			// Token: 0x060068A4 RID: 26788 RVA: 0x0016579D File Offset: 0x0016399D
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				if (!this.CanWrite)
				{
					throw Error.GetWriteNotSupported();
				}
				return base.BlockingBeginWrite(buffer, offset, count, callback, state);
			}

			// Token: 0x060068A5 RID: 26789 RVA: 0x001657BA File Offset: 0x001639BA
			public override void EndWrite(IAsyncResult asyncResult)
			{
				if (asyncResult == null)
				{
					throw new ArgumentNullException("asyncResult");
				}
				Stream.BlockingEndWrite(asyncResult);
			}

			// Token: 0x060068A6 RID: 26790 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
			public override int Read(byte[] buffer, int offset, int count)
			{
				return 0;
			}

			// Token: 0x060068A7 RID: 26791 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
			public override int Read(Span<byte> buffer)
			{
				return 0;
			}

			// Token: 0x060068A8 RID: 26792 RVA: 0x001657D0 File Offset: 0x001639D0
			public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
			{
				return Stream.NullStream.s_zeroTask;
			}

			// Token: 0x060068A9 RID: 26793 RVA: 0x001657D7 File Offset: 0x001639D7
			public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
			{
				return new ValueTask<int>(0);
			}

			// Token: 0x060068AA RID: 26794 RVA: 0x0012276A File Offset: 0x0012096A
			public override int ReadByte()
			{
				return -1;
			}

			// Token: 0x060068AB RID: 26795 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public override void Write(byte[] buffer, int offset, int count)
			{
			}

			// Token: 0x060068AC RID: 26796 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public override void Write(ReadOnlySpan<byte> buffer)
			{
			}

			// Token: 0x060068AD RID: 26797 RVA: 0x001657DF File Offset: 0x001639DF
			public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
			{
				if (!cancellationToken.IsCancellationRequested)
				{
					return Task.CompletedTask;
				}
				return Task.FromCanceled(cancellationToken);
			}

			// Token: 0x060068AE RID: 26798 RVA: 0x001657F8 File Offset: 0x001639F8
			public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
			{
				if (!cancellationToken.IsCancellationRequested)
				{
					return default(ValueTask);
				}
				return new ValueTask(Task.FromCanceled(cancellationToken));
			}

			// Token: 0x060068AF RID: 26799 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public override void WriteByte(byte value)
			{
			}

			// Token: 0x060068B0 RID: 26800 RVA: 0x0005CD52 File Offset: 0x0005AF52
			public override long Seek(long offset, SeekOrigin origin)
			{
				return 0L;
			}

			// Token: 0x060068B1 RID: 26801 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public override void SetLength(long length)
			{
			}

			// Token: 0x04003CE1 RID: 15585
			private static readonly Task<int> s_zeroTask = Task.FromResult<int>(0);
		}

		// Token: 0x02000B4E RID: 2894
		private sealed class SynchronousAsyncResult : IAsyncResult
		{
			// Token: 0x060068B3 RID: 26803 RVA: 0x00165830 File Offset: 0x00163A30
			internal SynchronousAsyncResult(int bytesRead, object asyncStateObject)
			{
				this._bytesRead = bytesRead;
				this._stateObject = asyncStateObject;
			}

			// Token: 0x060068B4 RID: 26804 RVA: 0x00165846 File Offset: 0x00163A46
			internal SynchronousAsyncResult(object asyncStateObject)
			{
				this._stateObject = asyncStateObject;
				this._isWrite = true;
			}

			// Token: 0x060068B5 RID: 26805 RVA: 0x0016585C File Offset: 0x00163A5C
			internal SynchronousAsyncResult(Exception ex, object asyncStateObject, bool isWrite)
			{
				this._exceptionInfo = ExceptionDispatchInfo.Capture(ex);
				this._stateObject = asyncStateObject;
				this._isWrite = isWrite;
			}

			// Token: 0x17001219 RID: 4633
			// (get) Token: 0x060068B6 RID: 26806 RVA: 0x000040F7 File Offset: 0x000022F7
			public bool IsCompleted
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700121A RID: 4634
			// (get) Token: 0x060068B7 RID: 26807 RVA: 0x0016587E File Offset: 0x00163A7E
			public WaitHandle AsyncWaitHandle
			{
				get
				{
					return LazyInitializer.EnsureInitialized<ManualResetEvent>(ref this._waitHandle, () => new ManualResetEvent(true));
				}
			}

			// Token: 0x1700121B RID: 4635
			// (get) Token: 0x060068B8 RID: 26808 RVA: 0x001658AA File Offset: 0x00163AAA
			public object AsyncState
			{
				get
				{
					return this._stateObject;
				}
			}

			// Token: 0x1700121C RID: 4636
			// (get) Token: 0x060068B9 RID: 26809 RVA: 0x000040F7 File Offset: 0x000022F7
			public bool CompletedSynchronously
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060068BA RID: 26810 RVA: 0x001658B2 File Offset: 0x00163AB2
			internal void ThrowIfError()
			{
				if (this._exceptionInfo != null)
				{
					this._exceptionInfo.Throw();
				}
			}

			// Token: 0x060068BB RID: 26811 RVA: 0x001658C8 File Offset: 0x00163AC8
			internal static int EndRead(IAsyncResult asyncResult)
			{
				Stream.SynchronousAsyncResult synchronousAsyncResult = asyncResult as Stream.SynchronousAsyncResult;
				if (synchronousAsyncResult == null || synchronousAsyncResult._isWrite)
				{
					throw new ArgumentException("IAsyncResult object did not come from the corresponding async method on this type.");
				}
				if (synchronousAsyncResult._endXxxCalled)
				{
					throw new ArgumentException("EndRead can only be called once for each asynchronous operation.");
				}
				synchronousAsyncResult._endXxxCalled = true;
				synchronousAsyncResult.ThrowIfError();
				return synchronousAsyncResult._bytesRead;
			}

			// Token: 0x060068BC RID: 26812 RVA: 0x00165918 File Offset: 0x00163B18
			internal static void EndWrite(IAsyncResult asyncResult)
			{
				Stream.SynchronousAsyncResult synchronousAsyncResult = asyncResult as Stream.SynchronousAsyncResult;
				if (synchronousAsyncResult == null || !synchronousAsyncResult._isWrite)
				{
					throw new ArgumentException("IAsyncResult object did not come from the corresponding async method on this type.");
				}
				if (synchronousAsyncResult._endXxxCalled)
				{
					throw new ArgumentException("EndWrite can only be called once for each asynchronous operation.");
				}
				synchronousAsyncResult._endXxxCalled = true;
				synchronousAsyncResult.ThrowIfError();
			}

			// Token: 0x04003CE2 RID: 15586
			private readonly object _stateObject;

			// Token: 0x04003CE3 RID: 15587
			private readonly bool _isWrite;

			// Token: 0x04003CE4 RID: 15588
			private ManualResetEvent _waitHandle;

			// Token: 0x04003CE5 RID: 15589
			private ExceptionDispatchInfo _exceptionInfo;

			// Token: 0x04003CE6 RID: 15590
			private bool _endXxxCalled;

			// Token: 0x04003CE7 RID: 15591
			private int _bytesRead;
		}

		// Token: 0x02000B50 RID: 2896
		private sealed class SyncStream : Stream, IDisposable
		{
			// Token: 0x060068C0 RID: 26816 RVA: 0x00165976 File Offset: 0x00163B76
			internal SyncStream(Stream stream)
			{
				if (stream == null)
				{
					throw new ArgumentNullException("stream");
				}
				this._stream = stream;
			}

			// Token: 0x1700121D RID: 4637
			// (get) Token: 0x060068C1 RID: 26817 RVA: 0x00165993 File Offset: 0x00163B93
			public override bool CanRead
			{
				get
				{
					return this._stream.CanRead;
				}
			}

			// Token: 0x1700121E RID: 4638
			// (get) Token: 0x060068C2 RID: 26818 RVA: 0x001659A0 File Offset: 0x00163BA0
			public override bool CanWrite
			{
				get
				{
					return this._stream.CanWrite;
				}
			}

			// Token: 0x1700121F RID: 4639
			// (get) Token: 0x060068C3 RID: 26819 RVA: 0x001659AD File Offset: 0x00163BAD
			public override bool CanSeek
			{
				get
				{
					return this._stream.CanSeek;
				}
			}

			// Token: 0x17001220 RID: 4640
			// (get) Token: 0x060068C4 RID: 26820 RVA: 0x001659BA File Offset: 0x00163BBA
			public override bool CanTimeout
			{
				get
				{
					return this._stream.CanTimeout;
				}
			}

			// Token: 0x17001221 RID: 4641
			// (get) Token: 0x060068C5 RID: 26821 RVA: 0x001659C8 File Offset: 0x00163BC8
			public override long Length
			{
				get
				{
					Stream stream = this._stream;
					long length;
					lock (stream)
					{
						length = this._stream.Length;
					}
					return length;
				}
			}

			// Token: 0x17001222 RID: 4642
			// (get) Token: 0x060068C6 RID: 26822 RVA: 0x00165A10 File Offset: 0x00163C10
			// (set) Token: 0x060068C7 RID: 26823 RVA: 0x00165A58 File Offset: 0x00163C58
			public override long Position
			{
				get
				{
					Stream stream = this._stream;
					long position;
					lock (stream)
					{
						position = this._stream.Position;
					}
					return position;
				}
				set
				{
					Stream stream = this._stream;
					lock (stream)
					{
						this._stream.Position = value;
					}
				}
			}

			// Token: 0x17001223 RID: 4643
			// (get) Token: 0x060068C8 RID: 26824 RVA: 0x00165AA0 File Offset: 0x00163CA0
			// (set) Token: 0x060068C9 RID: 26825 RVA: 0x00165AAD File Offset: 0x00163CAD
			public override int ReadTimeout
			{
				get
				{
					return this._stream.ReadTimeout;
				}
				set
				{
					this._stream.ReadTimeout = value;
				}
			}

			// Token: 0x17001224 RID: 4644
			// (get) Token: 0x060068CA RID: 26826 RVA: 0x00165ABB File Offset: 0x00163CBB
			// (set) Token: 0x060068CB RID: 26827 RVA: 0x00165AC8 File Offset: 0x00163CC8
			public override int WriteTimeout
			{
				get
				{
					return this._stream.WriteTimeout;
				}
				set
				{
					this._stream.WriteTimeout = value;
				}
			}

			// Token: 0x060068CC RID: 26828 RVA: 0x00165AD8 File Offset: 0x00163CD8
			public override void Close()
			{
				Stream stream = this._stream;
				lock (stream)
				{
					try
					{
						this._stream.Close();
					}
					finally
					{
						base.Dispose(true);
					}
				}
			}

			// Token: 0x060068CD RID: 26829 RVA: 0x00165B34 File Offset: 0x00163D34
			protected override void Dispose(bool disposing)
			{
				Stream stream = this._stream;
				lock (stream)
				{
					try
					{
						if (disposing)
						{
							((IDisposable)this._stream).Dispose();
						}
					}
					finally
					{
						base.Dispose(disposing);
					}
				}
			}

			// Token: 0x060068CE RID: 26830 RVA: 0x00165B90 File Offset: 0x00163D90
			public override void Flush()
			{
				Stream stream = this._stream;
				lock (stream)
				{
					this._stream.Flush();
				}
			}

			// Token: 0x060068CF RID: 26831 RVA: 0x00165BD8 File Offset: 0x00163DD8
			public override int Read(byte[] bytes, int offset, int count)
			{
				Stream stream = this._stream;
				int num;
				lock (stream)
				{
					num = this._stream.Read(bytes, offset, count);
				}
				return num;
			}

			// Token: 0x060068D0 RID: 26832 RVA: 0x00165C24 File Offset: 0x00163E24
			public override int Read(Span<byte> buffer)
			{
				Stream stream = this._stream;
				int num;
				lock (stream)
				{
					num = this._stream.Read(buffer);
				}
				return num;
			}

			// Token: 0x060068D1 RID: 26833 RVA: 0x00165C6C File Offset: 0x00163E6C
			public override int ReadByte()
			{
				Stream stream = this._stream;
				int num;
				lock (stream)
				{
					num = this._stream.ReadByte();
				}
				return num;
			}

			// Token: 0x060068D2 RID: 26834 RVA: 0x00165CB4 File Offset: 0x00163EB4
			public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				bool flag = this._stream.HasOverriddenBeginEndRead();
				Stream stream = this._stream;
				IAsyncResult asyncResult;
				lock (stream)
				{
					asyncResult = (flag ? this._stream.BeginRead(buffer, offset, count, callback, state) : this._stream.BeginReadInternal(buffer, offset, count, callback, state, true, true));
				}
				return asyncResult;
			}

			// Token: 0x060068D3 RID: 26835 RVA: 0x00165D28 File Offset: 0x00163F28
			public override int EndRead(IAsyncResult asyncResult)
			{
				if (asyncResult == null)
				{
					throw new ArgumentNullException("asyncResult");
				}
				Stream stream = this._stream;
				int num;
				lock (stream)
				{
					num = this._stream.EndRead(asyncResult);
				}
				return num;
			}

			// Token: 0x060068D4 RID: 26836 RVA: 0x00165D80 File Offset: 0x00163F80
			public override long Seek(long offset, SeekOrigin origin)
			{
				Stream stream = this._stream;
				long num;
				lock (stream)
				{
					num = this._stream.Seek(offset, origin);
				}
				return num;
			}

			// Token: 0x060068D5 RID: 26837 RVA: 0x00165DCC File Offset: 0x00163FCC
			public override void SetLength(long length)
			{
				Stream stream = this._stream;
				lock (stream)
				{
					this._stream.SetLength(length);
				}
			}

			// Token: 0x060068D6 RID: 26838 RVA: 0x00165E14 File Offset: 0x00164014
			public override void Write(byte[] bytes, int offset, int count)
			{
				Stream stream = this._stream;
				lock (stream)
				{
					this._stream.Write(bytes, offset, count);
				}
			}

			// Token: 0x060068D7 RID: 26839 RVA: 0x00165E5C File Offset: 0x0016405C
			public override void Write(ReadOnlySpan<byte> buffer)
			{
				Stream stream = this._stream;
				lock (stream)
				{
					this._stream.Write(buffer);
				}
			}

			// Token: 0x060068D8 RID: 26840 RVA: 0x00165EA4 File Offset: 0x001640A4
			public override void WriteByte(byte b)
			{
				Stream stream = this._stream;
				lock (stream)
				{
					this._stream.WriteByte(b);
				}
			}

			// Token: 0x060068D9 RID: 26841 RVA: 0x00165EEC File Offset: 0x001640EC
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				bool flag = this._stream.HasOverriddenBeginEndWrite();
				Stream stream = this._stream;
				IAsyncResult asyncResult;
				lock (stream)
				{
					asyncResult = (flag ? this._stream.BeginWrite(buffer, offset, count, callback, state) : this._stream.BeginWriteInternal(buffer, offset, count, callback, state, true, true));
				}
				return asyncResult;
			}

			// Token: 0x060068DA RID: 26842 RVA: 0x00165F60 File Offset: 0x00164160
			public override void EndWrite(IAsyncResult asyncResult)
			{
				if (asyncResult == null)
				{
					throw new ArgumentNullException("asyncResult");
				}
				Stream stream = this._stream;
				lock (stream)
				{
					this._stream.EndWrite(asyncResult);
				}
			}

			// Token: 0x04003CEA RID: 15594
			private Stream _stream;
		}
	}
}
