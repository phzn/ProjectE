using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Security.Cryptography
{
	/// <summary>Defines a stream that links data streams to cryptographic transformations.</summary>
	// Token: 0x02000469 RID: 1129
	public class CryptoStream : Stream, IDisposable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.CryptoStream" /> class with a target data stream, the transformation to use, and the mode of the stream.</summary>
		/// <param name="stream">The stream on which to perform the cryptographic transformation.</param>
		/// <param name="transform">The cryptographic transformation that is to be performed on the stream.</param>
		/// <param name="mode">One of the <see cref="T:System.Security.Cryptography.CryptoStreamMode" /> values.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="stream" /> is invalid.</exception>
		// Token: 0x06002DD2 RID: 11730 RVA: 0x000A4180 File Offset: 0x000A2380
		public CryptoStream(Stream stream, ICryptoTransform transform, CryptoStreamMode mode)
			: this(stream, transform, mode, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.CryptoStream" /> class.</summary>
		/// <param name="stream">The stream on which to perform the cryptographic transformation.</param>
		/// <param name="transform">The cryptographic transformation that is to be performed on the stream.</param>
		/// <param name="mode">The mode of the stream.</param>
		/// <param name="leaveOpen">
		///   <see langword="true" /> to not close the underlying stream when the <see cref="T:System.Security.Cryptography.CryptoStream" /> object is disposed; otherwise, <see langword="false" />.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="mode" /> is invalid.</exception>
		// Token: 0x06002DD3 RID: 11731 RVA: 0x000A418C File Offset: 0x000A238C
		public CryptoStream(Stream stream, ICryptoTransform transform, CryptoStreamMode mode, bool leaveOpen)
		{
			this._stream = stream;
			this._transformMode = mode;
			this._transform = transform;
			this._leaveOpen = leaveOpen;
			CryptoStreamMode transformMode = this._transformMode;
			if (transformMode != CryptoStreamMode.Read)
			{
				if (transformMode != CryptoStreamMode.Write)
				{
					throw new ArgumentException("Argument {0} should be larger than {1}.");
				}
				if (!this._stream.CanWrite)
				{
					throw new ArgumentException(SR.Format("Stream was not writable.", "stream"));
				}
				this._canWrite = true;
			}
			else
			{
				if (!this._stream.CanRead)
				{
					throw new ArgumentException(SR.Format("Stream was not readable.", "stream"));
				}
				this._canRead = true;
			}
			this.InitializeBuffer();
		}

		/// <summary>Gets a value indicating whether the current <see cref="T:System.Security.Cryptography.CryptoStream" /> is readable.</summary>
		/// <returns>
		///   <see langword="true" /> if the current stream is readable; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06002DD4 RID: 11732 RVA: 0x000A4233 File Offset: 0x000A2433
		public override bool CanRead
		{
			get
			{
				return this._canRead;
			}
		}

		/// <summary>Gets a value indicating whether you can seek within the current <see cref="T:System.Security.Cryptography.CryptoStream" />.</summary>
		/// <returns>Always <see langword="false" />.</returns>
		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06002DD5 RID: 11733 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the current <see cref="T:System.Security.Cryptography.CryptoStream" /> is writable.</summary>
		/// <returns>
		///   <see langword="true" /> if the current stream is writable; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06002DD6 RID: 11734 RVA: 0x000A423B File Offset: 0x000A243B
		public override bool CanWrite
		{
			get
			{
				return this._canWrite;
			}
		}

		/// <summary>Gets the length in bytes of the stream.</summary>
		/// <returns>This property is not supported.</returns>
		/// <exception cref="T:System.NotSupportedException">This property is not supported.</exception>
		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06002DD7 RID: 11735 RVA: 0x000A4243 File Offset: 0x000A2443
		public override long Length
		{
			get
			{
				throw new NotSupportedException("Stream does not support seeking.");
			}
		}

		/// <summary>Gets or sets the position within the current stream.</summary>
		/// <returns>This property is not supported.</returns>
		/// <exception cref="T:System.NotSupportedException">This property is not supported.</exception>
		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06002DD8 RID: 11736 RVA: 0x000A4243 File Offset: 0x000A2443
		// (set) Token: 0x06002DD9 RID: 11737 RVA: 0x000A4243 File Offset: 0x000A2443
		public override long Position
		{
			get
			{
				throw new NotSupportedException("Stream does not support seeking.");
			}
			set
			{
				throw new NotSupportedException("Stream does not support seeking.");
			}
		}

		/// <summary>Gets a value indicating whether the final buffer block has been written to the underlying stream.</summary>
		/// <returns>
		///   <see langword="true" /> if the final block has been flushed; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06002DDA RID: 11738 RVA: 0x000A424F File Offset: 0x000A244F
		public bool HasFlushedFinalBlock
		{
			get
			{
				return this._finalBlockTransformed;
			}
		}

		/// <summary>Updates the underlying data source or repository with the current state of the buffer, then clears the buffer.</summary>
		/// <exception cref="T:System.Security.Cryptography.CryptographicException">The key is corrupt which can cause invalid padding to the stream.</exception>
		/// <exception cref="T:System.NotSupportedException">The current stream is not writable.  
		///  -or-  
		///  The final block has already been transformed.</exception>
		// Token: 0x06002DDB RID: 11739 RVA: 0x000A4258 File Offset: 0x000A2458
		public void FlushFinalBlock()
		{
			if (this._finalBlockTransformed)
			{
				throw new NotSupportedException("FlushFinalBlock() method was called twice on a CryptoStream. It can only be called once.");
			}
			byte[] array = this._transform.TransformFinalBlock(this._inputBuffer, 0, this._inputBufferIndex);
			this._finalBlockTransformed = true;
			if (this._canWrite && this._outputBufferIndex > 0)
			{
				this._stream.Write(this._outputBuffer, 0, this._outputBufferIndex);
				this._outputBufferIndex = 0;
			}
			if (this._canWrite)
			{
				this._stream.Write(array, 0, array.Length);
			}
			CryptoStream cryptoStream = this._stream as CryptoStream;
			if (cryptoStream != null)
			{
				if (!cryptoStream.HasFlushedFinalBlock)
				{
					cryptoStream.FlushFinalBlock();
				}
			}
			else
			{
				this._stream.Flush();
			}
			if (this._inputBuffer != null)
			{
				Array.Clear(this._inputBuffer, 0, this._inputBuffer.Length);
			}
			if (this._outputBuffer != null)
			{
				Array.Clear(this._outputBuffer, 0, this._outputBuffer.Length);
			}
		}

		/// <summary>Clears all buffers for the current stream and causes any buffered data to be written to the underlying device.</summary>
		// Token: 0x06002DDC RID: 11740 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public override void Flush()
		{
		}

		/// <summary>Clears all buffers for the current stream asynchronously, causes any buffered data to be written to the underlying device, and monitors cancellation requests.</summary>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
		/// <returns>A task that represents the asynchronous flush operation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
		// Token: 0x06002DDD RID: 11741 RVA: 0x000A4342 File Offset: 0x000A2542
		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			if (base.GetType() != typeof(CryptoStream))
			{
				return base.FlushAsync(cancellationToken);
			}
			if (!cancellationToken.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}
			return Task.FromCanceled(cancellationToken);
		}

		/// <summary>Sets the position within the current stream.</summary>
		/// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
		/// <param name="origin">A <see cref="T:System.IO.SeekOrigin" /> object indicating the reference point used to obtain the new position.</param>
		/// <returns>This method is not supported.</returns>
		/// <exception cref="T:System.NotSupportedException">This method is not supported.</exception>
		// Token: 0x06002DDE RID: 11742 RVA: 0x000A4243 File Offset: 0x000A2443
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("Stream does not support seeking.");
		}

		/// <summary>Sets the length of the current stream.</summary>
		/// <param name="value">The desired length of the current stream in bytes.</param>
		/// <exception cref="T:System.NotSupportedException">This property exists only to support inheritance from <see cref="T:System.IO.Stream" />, and cannot be used.</exception>
		// Token: 0x06002DDF RID: 11743 RVA: 0x000A4243 File Offset: 0x000A2443
		public override void SetLength(long value)
		{
			throw new NotSupportedException("Stream does not support seeking.");
		}

		/// <summary>Reads a sequence of bytes from the current stream asynchronously, advances the position within the stream by the number of bytes read, and monitors cancellation requests.</summary>
		/// <param name="buffer">The buffer to write the data into.</param>
		/// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin writing data from the stream.</param>
		/// <param name="count">The maximum number of bytes to read.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
		/// <returns>A task that represents the asynchronous read operation. The value of the task object's <paramref name="TResult" /> parameter contains the total number of bytes read into the buffer. The result can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the stream has been reached.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset" /> and <paramref name="count" /> is larger than the buffer length.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The stream is currently in use by a previous read operation.</exception>
		// Token: 0x06002DE0 RID: 11744 RVA: 0x000A4378 File Offset: 0x000A2578
		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			this.CheckReadArguments(buffer, offset, count);
			return this.ReadAsyncInternal(buffer, offset, count, cancellationToken);
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x000A438E File Offset: 0x000A258E
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return TaskToApm.Begin(this.ReadAsync(buffer, offset, count, CancellationToken.None), callback, state);
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x000A43A7 File Offset: 0x000A25A7
		public override int EndRead(IAsyncResult asyncResult)
		{
			return TaskToApm.End<int>(asyncResult);
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x000A43B0 File Offset: 0x000A25B0
		private async Task<int> ReadAsyncInternal(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			SemaphoreSlim semaphore = this.AsyncActiveSemaphore;
			await semaphore.WaitAsync().ForceAsync();
			int num;
			try
			{
				num = await this.ReadAsyncCore(buffer, offset, count, cancellationToken, true);
			}
			finally
			{
				semaphore.Release();
			}
			return num;
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x000A4414 File Offset: 0x000A2614
		public override int ReadByte()
		{
			if (this._outputBufferIndex > 1)
			{
				int num = (int)this._outputBuffer[0];
				Buffer.BlockCopy(this._outputBuffer, 1, this._outputBuffer, 0, this._outputBufferIndex - 1);
				this._outputBufferIndex--;
				return num;
			}
			return base.ReadByte();
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x000A4464 File Offset: 0x000A2664
		public override void WriteByte(byte value)
		{
			if (this._inputBufferIndex + 1 < this._inputBlockSize)
			{
				byte[] inputBuffer = this._inputBuffer;
				int inputBufferIndex = this._inputBufferIndex;
				this._inputBufferIndex = inputBufferIndex + 1;
				inputBuffer[inputBufferIndex] = value;
				return;
			}
			base.WriteByte(value);
		}

		/// <summary>Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.</summary>
		/// <param name="buffer">An array of bytes. A maximum of <paramref name="count" /> bytes are read from the current stream and stored in <paramref name="buffer" />.</param>
		/// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
		/// <param name="count">The maximum number of bytes to be read from the current stream.</param>
		/// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero if the end of the stream has been reached.</returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Security.Cryptography.CryptoStreamMode" /> associated with current <see cref="T:System.Security.Cryptography.CryptoStream" /> object does not match the underlying stream.  For example, this exception is thrown when using <see cref="F:System.Security.Cryptography.CryptoStreamMode.Read" /> with an underlying stream that is write only.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="offset" /> parameter is less than zero.  
		///  -or-  
		///  The <paramref name="count" /> parameter is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">Thesum of the <paramref name="count" /> and <paramref name="offset" /> parameters is longer than the length of the buffer.</exception>
		// Token: 0x06002DE6 RID: 11750 RVA: 0x000A44A4 File Offset: 0x000A26A4
		public override int Read(byte[] buffer, int offset, int count)
		{
			this.CheckReadArguments(buffer, offset, count);
			return this.ReadAsyncCore(buffer, offset, count, default(CancellationToken), false).GetAwaiter().GetResult();
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x000A44DC File Offset: 0x000A26DC
		private void CheckReadArguments(byte[] buffer, int offset, int count)
		{
			if (!this.CanRead)
			{
				throw new NotSupportedException("Stream does not support reading.");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x000A4538 File Offset: 0x000A2738
		private async Task<int> ReadAsyncCore(byte[] buffer, int offset, int count, CancellationToken cancellationToken, bool useAsync)
		{
			int bytesToDeliver = count;
			int currentOutputIndex = offset;
			if (this._outputBufferIndex != 0)
			{
				if (this._outputBufferIndex > count)
				{
					Buffer.BlockCopy(this._outputBuffer, 0, buffer, offset, count);
					Buffer.BlockCopy(this._outputBuffer, count, this._outputBuffer, 0, this._outputBufferIndex - count);
					this._outputBufferIndex -= count;
					int num = this._outputBuffer.Length - this._outputBufferIndex;
					CryptographicOperations.ZeroMemory(new Span<byte>(this._outputBuffer, this._outputBufferIndex, num));
					return count;
				}
				Buffer.BlockCopy(this._outputBuffer, 0, buffer, offset, this._outputBufferIndex);
				bytesToDeliver -= this._outputBufferIndex;
				currentOutputIndex += this._outputBufferIndex;
				int num2 = this._outputBuffer.Length - this._outputBufferIndex;
				CryptographicOperations.ZeroMemory(new Span<byte>(this._outputBuffer, this._outputBufferIndex, num2));
				this._outputBufferIndex = 0;
			}
			int num3;
			if (this._finalBlockTransformed)
			{
				num3 = count - bytesToDeliver;
			}
			else
			{
				int num4 = bytesToDeliver / this._outputBlockSize;
				if (num4 > 1 && this._transform.CanTransformMultipleBlocks)
				{
					int numWholeBlocksInBytes = num4 * this._inputBlockSize;
					byte[] tempInputBuffer = ArrayPool<byte>.Shared.Rent(numWholeBlocksInBytes);
					byte[] tempOutputBuffer = null;
					try
					{
						int num5;
						if (useAsync)
						{
							num5 = await this._stream.ReadAsync(new Memory<byte>(tempInputBuffer, this._inputBufferIndex, numWholeBlocksInBytes - this._inputBufferIndex), cancellationToken);
						}
						else
						{
							num5 = this._stream.Read(tempInputBuffer, this._inputBufferIndex, numWholeBlocksInBytes - this._inputBufferIndex);
						}
						int num6 = num5;
						int num7 = this._inputBufferIndex + num6;
						if (num7 < this._inputBlockSize)
						{
							Buffer.BlockCopy(tempInputBuffer, this._inputBufferIndex, this._inputBuffer, this._inputBufferIndex, num6);
							this._inputBufferIndex = num7;
						}
						else
						{
							Buffer.BlockCopy(this._inputBuffer, 0, tempInputBuffer, 0, this._inputBufferIndex);
							CryptographicOperations.ZeroMemory(new Span<byte>(this._inputBuffer, 0, this._inputBufferIndex));
							num6 += this._inputBufferIndex;
							this._inputBufferIndex = 0;
							int num8 = num6 / this._inputBlockSize;
							int num9 = num8 * this._inputBlockSize;
							int num10 = num6 - num9;
							if (num10 != 0)
							{
								this._inputBufferIndex = num10;
								Buffer.BlockCopy(tempInputBuffer, num9, this._inputBuffer, 0, num10);
							}
							tempOutputBuffer = ArrayPool<byte>.Shared.Rent(num8 * this._outputBlockSize);
							int num11 = this._transform.TransformBlock(tempInputBuffer, 0, num9, tempOutputBuffer, 0);
							Buffer.BlockCopy(tempOutputBuffer, 0, buffer, currentOutputIndex, num11);
							CryptographicOperations.ZeroMemory(new Span<byte>(tempOutputBuffer, 0, num11));
							ArrayPool<byte>.Shared.Return(tempOutputBuffer, false);
							tempOutputBuffer = null;
							bytesToDeliver -= num11;
							currentOutputIndex += num11;
						}
					}
					finally
					{
						if (tempOutputBuffer != null)
						{
							CryptographicOperations.ZeroMemory(tempOutputBuffer);
							ArrayPool<byte>.Shared.Return(tempOutputBuffer, false);
							tempOutputBuffer = null;
						}
						CryptographicOperations.ZeroMemory(new Span<byte>(tempInputBuffer, 0, numWholeBlocksInBytes));
						ArrayPool<byte>.Shared.Return(tempInputBuffer, false);
						tempInputBuffer = null;
					}
					tempInputBuffer = null;
					tempOutputBuffer = null;
				}
				while (bytesToDeliver > 0)
				{
					while (this._inputBufferIndex < this._inputBlockSize)
					{
						int num5;
						if (useAsync)
						{
							num5 = await this._stream.ReadAsync(new Memory<byte>(this._inputBuffer, this._inputBufferIndex, this._inputBlockSize - this._inputBufferIndex), cancellationToken);
						}
						else
						{
							num5 = this._stream.Read(this._inputBuffer, this._inputBufferIndex, this._inputBlockSize - this._inputBufferIndex);
						}
						int num6 = num5;
						if (num6 != 0)
						{
							this._inputBufferIndex += num6;
						}
						else
						{
							byte[] array = this._transform.TransformFinalBlock(this._inputBuffer, 0, this._inputBufferIndex);
							this._outputBuffer = array;
							this._outputBufferIndex = array.Length;
							this._finalBlockTransformed = true;
							if (bytesToDeliver < this._outputBufferIndex)
							{
								Buffer.BlockCopy(this._outputBuffer, 0, buffer, currentOutputIndex, bytesToDeliver);
								this._outputBufferIndex -= bytesToDeliver;
								Buffer.BlockCopy(this._outputBuffer, bytesToDeliver, this._outputBuffer, 0, this._outputBufferIndex);
								int num12 = this._outputBuffer.Length - this._outputBufferIndex;
								CryptographicOperations.ZeroMemory(new Span<byte>(this._outputBuffer, this._outputBufferIndex, num12));
								return count;
							}
							Buffer.BlockCopy(this._outputBuffer, 0, buffer, currentOutputIndex, this._outputBufferIndex);
							bytesToDeliver -= this._outputBufferIndex;
							this._outputBufferIndex = 0;
							CryptographicOperations.ZeroMemory(this._outputBuffer);
							return count - bytesToDeliver;
						}
					}
					int num11 = this._transform.TransformBlock(this._inputBuffer, 0, this._inputBlockSize, this._outputBuffer, 0);
					this._inputBufferIndex = 0;
					if (bytesToDeliver < num11)
					{
						Buffer.BlockCopy(this._outputBuffer, 0, buffer, currentOutputIndex, bytesToDeliver);
						this._outputBufferIndex = num11 - bytesToDeliver;
						Buffer.BlockCopy(this._outputBuffer, bytesToDeliver, this._outputBuffer, 0, this._outputBufferIndex);
						int num13 = this._outputBuffer.Length - this._outputBufferIndex;
						CryptographicOperations.ZeroMemory(new Span<byte>(this._outputBuffer, this._outputBufferIndex, num13));
						return count;
					}
					Buffer.BlockCopy(this._outputBuffer, 0, buffer, currentOutputIndex, num11);
					CryptographicOperations.ZeroMemory(new Span<byte>(this._outputBuffer, 0, num11));
					currentOutputIndex += num11;
					bytesToDeliver -= num11;
				}
				num3 = count;
			}
			return num3;
		}

		/// <summary>Writes a sequence of bytes to the current stream asynchronously, advances the current position within the stream by the number of bytes written, and monitors cancellation requests.</summary>
		/// <param name="buffer">The buffer to write data from.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> from which to begin writing bytes to the stream.</param>
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
		// Token: 0x06002DE9 RID: 11753 RVA: 0x000A45A5 File Offset: 0x000A27A5
		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			this.CheckWriteArguments(buffer, offset, count);
			return this.WriteAsyncInternal(buffer, offset, count, cancellationToken);
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x000A45BB File Offset: 0x000A27BB
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return TaskToApm.Begin(this.WriteAsync(buffer, offset, count, CancellationToken.None), callback, state);
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x000A45D4 File Offset: 0x000A27D4
		public override void EndWrite(IAsyncResult asyncResult)
		{
			TaskToApm.End(asyncResult);
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x000A45DC File Offset: 0x000A27DC
		private async Task WriteAsyncInternal(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			SemaphoreSlim semaphore = this.AsyncActiveSemaphore;
			await semaphore.WaitAsync().ForceAsync();
			try
			{
				await this.WriteAsyncCore(buffer, offset, count, cancellationToken, true);
			}
			finally
			{
				semaphore.Release();
			}
		}

		/// <summary>Writes a sequence of bytes to the current <see cref="T:System.Security.Cryptography.CryptoStream" /> and advances the current position within the stream by the number of bytes written.</summary>
		/// <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from <paramref name="buffer" /> to the current stream.</param>
		/// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
		/// <param name="count">The number of bytes to be written to the current stream.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Security.Cryptography.CryptoStreamMode" /> associated with current <see cref="T:System.Security.Cryptography.CryptoStream" /> object does not match the underlying stream.  For example, this exception is thrown when using <see cref="F:System.Security.Cryptography.CryptoStreamMode.Write" /> with an underlying stream that is read only.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="offset" /> parameter is less than zero.  
		///  -or-  
		///  The <paramref name="count" /> parameter is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">The sum of the <paramref name="count" /> and <paramref name="offset" /> parameters is longer than the length of the buffer.</exception>
		// Token: 0x06002DED RID: 11757 RVA: 0x000A4640 File Offset: 0x000A2840
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.CheckWriteArguments(buffer, offset, count);
			this.WriteAsyncCore(buffer, offset, count, default(CancellationToken), false).GetAwaiter().GetResult();
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x000A4678 File Offset: 0x000A2878
		private void CheckWriteArguments(byte[] buffer, int offset, int count)
		{
			if (!this.CanWrite)
			{
				throw new NotSupportedException("Stream does not support writing.");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x000A46D4 File Offset: 0x000A28D4
		private async Task WriteAsyncCore(byte[] buffer, int offset, int count, CancellationToken cancellationToken, bool useAsync)
		{
			int bytesToWrite = count;
			int currentInputIndex = offset;
			if (this._inputBufferIndex > 0)
			{
				if (count < this._inputBlockSize - this._inputBufferIndex)
				{
					Buffer.BlockCopy(buffer, offset, this._inputBuffer, this._inputBufferIndex, count);
					this._inputBufferIndex += count;
					return;
				}
				Buffer.BlockCopy(buffer, offset, this._inputBuffer, this._inputBufferIndex, this._inputBlockSize - this._inputBufferIndex);
				currentInputIndex += this._inputBlockSize - this._inputBufferIndex;
				bytesToWrite -= this._inputBlockSize - this._inputBufferIndex;
				this._inputBufferIndex = this._inputBlockSize;
			}
			if (this._outputBufferIndex > 0)
			{
				if (useAsync)
				{
					await this._stream.WriteAsync(new ReadOnlyMemory<byte>(this._outputBuffer, 0, this._outputBufferIndex), cancellationToken);
				}
				else
				{
					this._stream.Write(this._outputBuffer, 0, this._outputBufferIndex);
				}
				this._outputBufferIndex = 0;
			}
			if (this._inputBufferIndex == this._inputBlockSize)
			{
				int numOutputBytes = this._transform.TransformBlock(this._inputBuffer, 0, this._inputBlockSize, this._outputBuffer, 0);
				if (useAsync)
				{
					await this._stream.WriteAsync(new ReadOnlyMemory<byte>(this._outputBuffer, 0, numOutputBytes), cancellationToken);
				}
				else
				{
					this._stream.Write(this._outputBuffer, 0, numOutputBytes);
				}
				this._inputBufferIndex = 0;
			}
			while (bytesToWrite > 0)
			{
				if (bytesToWrite < this._inputBlockSize)
				{
					Buffer.BlockCopy(buffer, currentInputIndex, this._inputBuffer, 0, bytesToWrite);
					this._inputBufferIndex += bytesToWrite;
					break;
				}
				int num = bytesToWrite / this._inputBlockSize;
				if (this._transform.CanTransformMultipleBlocks && num > 1)
				{
					int numWholeBlocksInBytes = num * this._inputBlockSize;
					byte[] tempOutputBuffer = ArrayPool<byte>.Shared.Rent(num * this._outputBlockSize);
					int numOutputBytes = 0;
					try
					{
						numOutputBytes = this._transform.TransformBlock(buffer, currentInputIndex, numWholeBlocksInBytes, tempOutputBuffer, 0);
						if (useAsync)
						{
							await this._stream.WriteAsync(new ReadOnlyMemory<byte>(tempOutputBuffer, 0, numOutputBytes), cancellationToken);
						}
						else
						{
							this._stream.Write(tempOutputBuffer, 0, numOutputBytes);
						}
						currentInputIndex += numWholeBlocksInBytes;
						bytesToWrite -= numWholeBlocksInBytes;
					}
					finally
					{
						CryptographicOperations.ZeroMemory(new Span<byte>(tempOutputBuffer, 0, numOutputBytes));
						ArrayPool<byte>.Shared.Return(tempOutputBuffer, false);
						tempOutputBuffer = null;
					}
					tempOutputBuffer = null;
				}
				else
				{
					int numOutputBytes = this._transform.TransformBlock(buffer, currentInputIndex, this._inputBlockSize, this._outputBuffer, 0);
					if (useAsync)
					{
						await this._stream.WriteAsync(new ReadOnlyMemory<byte>(this._outputBuffer, 0, numOutputBytes), cancellationToken);
					}
					else
					{
						this._stream.Write(this._outputBuffer, 0, numOutputBytes);
					}
					currentInputIndex += this._inputBlockSize;
					bytesToWrite -= this._inputBlockSize;
				}
			}
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Security.Cryptography.CryptoStream" />.</summary>
		// Token: 0x06002DF0 RID: 11760 RVA: 0x000A4741 File Offset: 0x000A2941
		public void Clear()
		{
			this.Close();
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Security.Cryptography.CryptoStream" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x06002DF1 RID: 11761 RVA: 0x000A474C File Offset: 0x000A294C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					if (!this._finalBlockTransformed)
					{
						this.FlushFinalBlock();
					}
					if (!this._leaveOpen)
					{
						this._stream.Dispose();
					}
				}
			}
			finally
			{
				try
				{
					this._finalBlockTransformed = true;
					if (this._inputBuffer != null)
					{
						Array.Clear(this._inputBuffer, 0, this._inputBuffer.Length);
					}
					if (this._outputBuffer != null)
					{
						Array.Clear(this._outputBuffer, 0, this._outputBuffer.Length);
					}
					this._inputBuffer = null;
					this._outputBuffer = null;
					this._canRead = false;
					this._canWrite = false;
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x000A4804 File Offset: 0x000A2A04
		private void InitializeBuffer()
		{
			if (this._transform != null)
			{
				this._inputBlockSize = this._transform.InputBlockSize;
				this._inputBuffer = new byte[this._inputBlockSize];
				this._outputBlockSize = this._transform.OutputBlockSize;
				this._outputBuffer = new byte[this._outputBlockSize];
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06002DF3 RID: 11763 RVA: 0x000A485D File Offset: 0x000A2A5D
		private SemaphoreSlim AsyncActiveSemaphore
		{
			get
			{
				return LazyInitializer.EnsureInitialized<SemaphoreSlim>(ref this._lazyAsyncActiveSemaphore, () => new SemaphoreSlim(1, 1));
			}
		}

		// Token: 0x040020D1 RID: 8401
		private readonly Stream _stream;

		// Token: 0x040020D2 RID: 8402
		private readonly ICryptoTransform _transform;

		// Token: 0x040020D3 RID: 8403
		private readonly CryptoStreamMode _transformMode;

		// Token: 0x040020D4 RID: 8404
		private byte[] _inputBuffer;

		// Token: 0x040020D5 RID: 8405
		private int _inputBufferIndex;

		// Token: 0x040020D6 RID: 8406
		private int _inputBlockSize;

		// Token: 0x040020D7 RID: 8407
		private byte[] _outputBuffer;

		// Token: 0x040020D8 RID: 8408
		private int _outputBufferIndex;

		// Token: 0x040020D9 RID: 8409
		private int _outputBlockSize;

		// Token: 0x040020DA RID: 8410
		private bool _canRead;

		// Token: 0x040020DB RID: 8411
		private bool _canWrite;

		// Token: 0x040020DC RID: 8412
		private bool _finalBlockTransformed;

		// Token: 0x040020DD RID: 8413
		private SemaphoreSlim _lazyAsyncActiveSemaphore;

		// Token: 0x040020DE RID: 8414
		private readonly bool _leaveOpen;
	}
}
