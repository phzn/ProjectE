using System;
using System.IO;

namespace System.Net.Mime
{
	// Token: 0x020007D1 RID: 2001
	internal class EightBitStream : DelegatedStream, IEncodableStream
	{
		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x06004032 RID: 16434 RVA: 0x000DB6E0 File Offset: 0x000D98E0
		private WriteStateInfoBase WriteState
		{
			get
			{
				WriteStateInfoBase writeStateInfoBase;
				if ((writeStateInfoBase = this._writeState) == null)
				{
					writeStateInfoBase = (this._writeState = new WriteStateInfoBase());
				}
				return writeStateInfoBase;
			}
		}

		// Token: 0x06004033 RID: 16435 RVA: 0x000DB705 File Offset: 0x000D9905
		internal EightBitStream(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x06004034 RID: 16436 RVA: 0x000DB70E File Offset: 0x000D990E
		internal EightBitStream(Stream stream, bool shouldEncodeLeadingDots)
			: this(stream)
		{
			this._shouldEncodeLeadingDots = shouldEncodeLeadingDots;
		}

		// Token: 0x06004035 RID: 16437 RVA: 0x000DB720 File Offset: 0x000D9920
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset >= buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			IAsyncResult asyncResult;
			if (this._shouldEncodeLeadingDots)
			{
				this.EncodeLines(buffer, offset, count);
				asyncResult = base.BeginWrite(this.WriteState.Buffer, 0, this.WriteState.Length, callback, state);
			}
			else
			{
				asyncResult = base.BeginWrite(buffer, offset, count, callback, state);
			}
			return asyncResult;
		}

		// Token: 0x06004036 RID: 16438 RVA: 0x000DB7A7 File Offset: 0x000D99A7
		public override void EndWrite(IAsyncResult asyncResult)
		{
			base.EndWrite(asyncResult);
			this.WriteState.BufferFlushed();
		}

		// Token: 0x06004037 RID: 16439 RVA: 0x000DB7BC File Offset: 0x000D99BC
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset >= buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this._shouldEncodeLeadingDots)
			{
				this.EncodeLines(buffer, offset, count);
				base.Write(this.WriteState.Buffer, 0, this.WriteState.Length);
				this.WriteState.BufferFlushed();
				return;
			}
			base.Write(buffer, offset, count);
		}

		// Token: 0x06004038 RID: 16440 RVA: 0x000DB844 File Offset: 0x000D9A44
		private void EncodeLines(byte[] buffer, int offset, int count)
		{
			int num = offset;
			while (num < offset + count && num < buffer.Length)
			{
				if (buffer[num] == 13 && num + 1 < offset + count && buffer[num + 1] == 10)
				{
					this.WriteState.AppendCRLF(false);
					num++;
				}
				else if (this.WriteState.CurrentLineLength == 0 && buffer[num] == 46)
				{
					this.WriteState.Append(46);
					this.WriteState.Append(buffer[num]);
				}
				else
				{
					this.WriteState.Append(buffer[num]);
				}
				num++;
			}
		}

		// Token: 0x06004039 RID: 16441 RVA: 0x000075E1 File Offset: 0x000057E1
		public Stream GetStream()
		{
			return this;
		}

		// Token: 0x0600403A RID: 16442 RVA: 0x0000829A File Offset: 0x0000649A
		public int DecodeBytes(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600403B RID: 16443 RVA: 0x0000829A File Offset: 0x0000649A
		public int EncodeBytes(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x0000829A File Offset: 0x0000649A
		public string GetEncodedString()
		{
			throw new NotImplementedException();
		}

		// Token: 0x04002672 RID: 9842
		private WriteStateInfoBase _writeState;

		// Token: 0x04002673 RID: 9843
		private bool _shouldEncodeLeadingDots;
	}
}
