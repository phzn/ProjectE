using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
	// Token: 0x02000814 RID: 2068
	internal abstract class DelegatingStream : Stream
	{
		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x06004230 RID: 16944 RVA: 0x000E4C9B File Offset: 0x000E2E9B
		public override bool CanRead
		{
			get
			{
				return this._innerStream.CanRead;
			}
		}

		// Token: 0x17000ED9 RID: 3801
		// (get) Token: 0x06004231 RID: 16945 RVA: 0x000E4CA8 File Offset: 0x000E2EA8
		public override bool CanSeek
		{
			get
			{
				return this._innerStream.CanSeek;
			}
		}

		// Token: 0x17000EDA RID: 3802
		// (get) Token: 0x06004232 RID: 16946 RVA: 0x000E4CB5 File Offset: 0x000E2EB5
		public override bool CanWrite
		{
			get
			{
				return this._innerStream.CanWrite;
			}
		}

		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x06004233 RID: 16947 RVA: 0x000E4CC2 File Offset: 0x000E2EC2
		public override long Length
		{
			get
			{
				return this._innerStream.Length;
			}
		}

		// Token: 0x17000EDC RID: 3804
		// (get) Token: 0x06004234 RID: 16948 RVA: 0x000E4CCF File Offset: 0x000E2ECF
		// (set) Token: 0x06004235 RID: 16949 RVA: 0x000E4CDC File Offset: 0x000E2EDC
		public override long Position
		{
			get
			{
				return this._innerStream.Position;
			}
			set
			{
				this._innerStream.Position = value;
			}
		}

		// Token: 0x17000EDD RID: 3805
		// (get) Token: 0x06004236 RID: 16950 RVA: 0x000E4CEA File Offset: 0x000E2EEA
		// (set) Token: 0x06004237 RID: 16951 RVA: 0x000E4CF7 File Offset: 0x000E2EF7
		public override int ReadTimeout
		{
			get
			{
				return this._innerStream.ReadTimeout;
			}
			set
			{
				this._innerStream.ReadTimeout = value;
			}
		}

		// Token: 0x17000EDE RID: 3806
		// (get) Token: 0x06004238 RID: 16952 RVA: 0x000E4D05 File Offset: 0x000E2F05
		public override bool CanTimeout
		{
			get
			{
				return this._innerStream.CanTimeout;
			}
		}

		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x06004239 RID: 16953 RVA: 0x000E4D12 File Offset: 0x000E2F12
		// (set) Token: 0x0600423A RID: 16954 RVA: 0x000E4D1F File Offset: 0x000E2F1F
		public override int WriteTimeout
		{
			get
			{
				return this._innerStream.WriteTimeout;
			}
			set
			{
				this._innerStream.WriteTimeout = value;
			}
		}

		// Token: 0x0600423B RID: 16955 RVA: 0x000E4D2D File Offset: 0x000E2F2D
		protected DelegatingStream(Stream innerStream)
		{
			this._innerStream = innerStream;
		}

		// Token: 0x0600423C RID: 16956 RVA: 0x000E4D3C File Offset: 0x000E2F3C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._innerStream.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600423D RID: 16957 RVA: 0x000E4D53 File Offset: 0x000E2F53
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._innerStream.Seek(offset, origin);
		}

		// Token: 0x0600423E RID: 16958 RVA: 0x000E4D62 File Offset: 0x000E2F62
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this._innerStream.Read(buffer, offset, count);
		}

		// Token: 0x0600423F RID: 16959 RVA: 0x000E4D72 File Offset: 0x000E2F72
		public override int Read(Span<byte> buffer)
		{
			return this._innerStream.Read(buffer);
		}

		// Token: 0x06004240 RID: 16960 RVA: 0x000E4D80 File Offset: 0x000E2F80
		public override int ReadByte()
		{
			return this._innerStream.ReadByte();
		}

		// Token: 0x06004241 RID: 16961 RVA: 0x000E4D8D File Offset: 0x000E2F8D
		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return this._innerStream.ReadAsync(buffer, offset, count, cancellationToken);
		}

		// Token: 0x06004242 RID: 16962 RVA: 0x000E4D9F File Offset: 0x000E2F9F
		public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			return this._innerStream.ReadAsync(buffer, cancellationToken);
		}

		// Token: 0x06004243 RID: 16963 RVA: 0x000E4DAE File Offset: 0x000E2FAE
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this._innerStream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06004244 RID: 16964 RVA: 0x000E4DC2 File Offset: 0x000E2FC2
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this._innerStream.EndRead(asyncResult);
		}

		// Token: 0x06004245 RID: 16965 RVA: 0x000E4DD0 File Offset: 0x000E2FD0
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x06004246 RID: 16966 RVA: 0x000E4DDD File Offset: 0x000E2FDD
		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			return this._innerStream.FlushAsync(cancellationToken);
		}

		// Token: 0x06004247 RID: 16967 RVA: 0x000E4DEB File Offset: 0x000E2FEB
		public override void SetLength(long value)
		{
			this._innerStream.SetLength(value);
		}

		// Token: 0x06004248 RID: 16968 RVA: 0x000E4DF9 File Offset: 0x000E2FF9
		public override void Write(byte[] buffer, int offset, int count)
		{
			this._innerStream.Write(buffer, offset, count);
		}

		// Token: 0x06004249 RID: 16969 RVA: 0x000E4E09 File Offset: 0x000E3009
		public override void Write(ReadOnlySpan<byte> buffer)
		{
			this._innerStream.Write(buffer);
		}

		// Token: 0x0600424A RID: 16970 RVA: 0x000E4E17 File Offset: 0x000E3017
		public override void WriteByte(byte value)
		{
			this._innerStream.WriteByte(value);
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x000E4E25 File Offset: 0x000E3025
		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return this._innerStream.WriteAsync(buffer, offset, count, cancellationToken);
		}

		// Token: 0x0600424C RID: 16972 RVA: 0x000E4E37 File Offset: 0x000E3037
		public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			return this._innerStream.WriteAsync(buffer, cancellationToken);
		}

		// Token: 0x0600424D RID: 16973 RVA: 0x000E4E46 File Offset: 0x000E3046
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this._innerStream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x0600424E RID: 16974 RVA: 0x000E4E5A File Offset: 0x000E305A
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this._innerStream.EndWrite(asyncResult);
		}

		// Token: 0x0600424F RID: 16975 RVA: 0x000E4E68 File Offset: 0x000E3068
		public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			return this._innerStream.CopyToAsync(destination, bufferSize, cancellationToken);
		}

		// Token: 0x0400279F RID: 10143
		private readonly Stream _innerStream;
	}
}
