using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.SqlClient.SNI
{
	// Token: 0x020002A2 RID: 674
	internal sealed class SslOverTdsStream : Stream
	{
		// Token: 0x06001EFB RID: 7931 RVA: 0x0009284C File Offset: 0x00090A4C
		public SslOverTdsStream(Stream stream)
		{
			this._stream = stream;
			this._encapsulate = true;
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x00092862 File Offset: 0x00090A62
		public void FinishHandshake()
		{
			this._encapsulate = false;
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x0009286C File Offset: 0x00090A6C
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.ReadInternal(buffer, offset, count, CancellationToken.None, false).GetAwaiter().GetResult();
		}

		// Token: 0x06001EFE RID: 7934 RVA: 0x00092895 File Offset: 0x00090A95
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.WriteInternal(buffer, offset, count, CancellationToken.None, false).Wait();
		}

		// Token: 0x06001EFF RID: 7935 RVA: 0x000928AB File Offset: 0x00090AAB
		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken token)
		{
			return this.WriteInternal(buffer, offset, count, token, true);
		}

		// Token: 0x06001F00 RID: 7936 RVA: 0x000928B9 File Offset: 0x00090AB9
		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken token)
		{
			return this.ReadInternal(buffer, offset, count, token, true);
		}

		// Token: 0x06001F01 RID: 7937 RVA: 0x000928C8 File Offset: 0x00090AC8
		private async Task<int> ReadInternal(byte[] buffer, int offset, int count, CancellationToken token, bool async)
		{
			int i = 0;
			byte[] packetData = new byte[(count < 8) ? 8 : count];
			int num2;
			if (this._encapsulate)
			{
				if (this._packetBytes == 0)
				{
					while (i < 8)
					{
						int num = i;
						if (async)
						{
							num2 = await this._stream.ReadAsync(packetData, i, 8 - i, token).ConfigureAwait(false);
						}
						else
						{
							num2 = this._stream.Read(packetData, i, 8 - i);
						}
						i = num + num2;
					}
					this._packetBytes = ((int)packetData[2] << 8) | (int)packetData[3];
					this._packetBytes -= 8;
				}
				if (count > this._packetBytes)
				{
					count = this._packetBytes;
				}
			}
			if (async)
			{
				num2 = await this._stream.ReadAsync(packetData, 0, count, token).ConfigureAwait(false);
			}
			else
			{
				num2 = this._stream.Read(packetData, 0, count);
			}
			i = num2;
			if (this._encapsulate)
			{
				this._packetBytes -= i;
			}
			Buffer.BlockCopy(packetData, 0, buffer, offset, i);
			return i;
		}

		// Token: 0x06001F02 RID: 7938 RVA: 0x00092938 File Offset: 0x00090B38
		private async Task WriteInternal(byte[] buffer, int offset, int count, CancellationToken token, bool async)
		{
			int currentCount = 0;
			int currentOffset = offset;
			while (count > 0)
			{
				if (this._encapsulate)
				{
					if (count > 4088)
					{
						currentCount = 4088;
					}
					else
					{
						currentCount = count;
					}
					count -= currentCount;
					byte[] array = new byte[8 + currentCount];
					array[0] = 18;
					array[1] = ((count > 0) ? 0 : 1);
					array[2] = (byte)((currentCount + 8) / 256);
					array[3] = (byte)((currentCount + 8) % 256);
					array[4] = 0;
					array[5] = 0;
					array[6] = 0;
					array[7] = 0;
					for (int i = 8; i < array.Length; i++)
					{
						array[i] = buffer[currentOffset + (i - 8)];
					}
					if (async)
					{
						await this._stream.WriteAsync(array, 0, array.Length, token).ConfigureAwait(false);
					}
					else
					{
						this._stream.Write(array, 0, array.Length);
					}
				}
				else
				{
					currentCount = count;
					count = 0;
					if (async)
					{
						await this._stream.WriteAsync(buffer, currentOffset, currentCount, token).ConfigureAwait(false);
					}
					else
					{
						this._stream.Write(buffer, currentOffset, currentCount);
					}
				}
				if (async)
				{
					await this._stream.FlushAsync().ConfigureAwait(false);
				}
				else
				{
					this._stream.Flush();
				}
				currentOffset += currentCount;
			}
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x00087F51 File Offset: 0x00086151
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001F04 RID: 7940 RVA: 0x000929A5 File Offset: 0x00090BA5
		public override void Flush()
		{
			if (!(this._stream is PipeStream))
			{
				this._stream.Flush();
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001F05 RID: 7941 RVA: 0x00087F51 File Offset: 0x00086151
		// (set) Token: 0x06001F06 RID: 7942 RVA: 0x00087F51 File Offset: 0x00086151
		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06001F07 RID: 7943 RVA: 0x00087F51 File Offset: 0x00086151
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001F08 RID: 7944 RVA: 0x000929BF File Offset: 0x00090BBF
		public override bool CanRead
		{
			get
			{
				return this._stream.CanRead;
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001F09 RID: 7945 RVA: 0x000929CC File Offset: 0x00090BCC
		public override bool CanWrite
		{
			get
			{
				return this._stream.CanWrite;
			}
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001F0A RID: 7946 RVA: 0x00006D64 File Offset: 0x00004F64
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001F0B RID: 7947 RVA: 0x00087F51 File Offset: 0x00086151
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x04001580 RID: 5504
		private readonly Stream _stream;

		// Token: 0x04001581 RID: 5505
		private int _packetBytes;

		// Token: 0x04001582 RID: 5506
		private bool _encapsulate;

		// Token: 0x04001583 RID: 5507
		private const int PACKET_SIZE_WITHOUT_HEADER = 4088;

		// Token: 0x04001584 RID: 5508
		private const int PRELOGIN_PACKET_TYPE = 18;
	}
}
