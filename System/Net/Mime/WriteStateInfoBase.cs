using System;

namespace System.Net.Mime
{
	// Token: 0x020007EA RID: 2026
	internal class WriteStateInfoBase
	{
		// Token: 0x060040DC RID: 16604 RVA: 0x000DE440 File Offset: 0x000DC640
		internal WriteStateInfoBase()
		{
			this._header = Array.Empty<byte>();
			this._footer = Array.Empty<byte>();
			this._maxLineLength = 70;
			this._buffer = new byte[1024];
			this._currentLineLength = 0;
			this._currentBufferUsed = 0;
		}

		// Token: 0x060040DD RID: 16605 RVA: 0x000DE48F File Offset: 0x000DC68F
		internal WriteStateInfoBase(int bufferSize, byte[] header, byte[] footer, int maxLineLength)
			: this(bufferSize, header, footer, maxLineLength, 0)
		{
		}

		// Token: 0x060040DE RID: 16606 RVA: 0x000DE49D File Offset: 0x000DC69D
		internal WriteStateInfoBase(int bufferSize, byte[] header, byte[] footer, int maxLineLength, int mimeHeaderLength)
		{
			this._buffer = new byte[bufferSize];
			this._header = header;
			this._footer = footer;
			this._maxLineLength = maxLineLength;
			this._currentLineLength = mimeHeaderLength;
			this._currentBufferUsed = 0;
		}

		// Token: 0x17000E9A RID: 3738
		// (get) Token: 0x060040DF RID: 16607 RVA: 0x000DE4D6 File Offset: 0x000DC6D6
		internal int FooterLength
		{
			get
			{
				return this._footer.Length;
			}
		}

		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x060040E0 RID: 16608 RVA: 0x000DE4E0 File Offset: 0x000DC6E0
		internal byte[] Footer
		{
			get
			{
				return this._footer;
			}
		}

		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x060040E1 RID: 16609 RVA: 0x000DE4E8 File Offset: 0x000DC6E8
		internal byte[] Header
		{
			get
			{
				return this._header;
			}
		}

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x060040E2 RID: 16610 RVA: 0x000DE4F0 File Offset: 0x000DC6F0
		internal byte[] Buffer
		{
			get
			{
				return this._buffer;
			}
		}

		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x060040E3 RID: 16611 RVA: 0x000DE4F8 File Offset: 0x000DC6F8
		internal int Length
		{
			get
			{
				return this._currentBufferUsed;
			}
		}

		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x060040E4 RID: 16612 RVA: 0x000DE500 File Offset: 0x000DC700
		internal int CurrentLineLength
		{
			get
			{
				return this._currentLineLength;
			}
		}

		// Token: 0x060040E5 RID: 16613 RVA: 0x000DE508 File Offset: 0x000DC708
		private void EnsureSpaceInBuffer(int moreBytes)
		{
			int num = this.Buffer.Length;
			while (this._currentBufferUsed + moreBytes >= num)
			{
				num *= 2;
			}
			if (num > this.Buffer.Length)
			{
				byte[] array = new byte[num];
				this._buffer.CopyTo(array, 0);
				this._buffer = array;
			}
		}

		// Token: 0x060040E6 RID: 16614 RVA: 0x000DE558 File Offset: 0x000DC758
		internal void Append(byte aByte)
		{
			this.EnsureSpaceInBuffer(1);
			byte[] buffer = this.Buffer;
			int currentBufferUsed = this._currentBufferUsed;
			this._currentBufferUsed = currentBufferUsed + 1;
			buffer[currentBufferUsed] = aByte;
			this._currentLineLength++;
		}

		// Token: 0x060040E7 RID: 16615 RVA: 0x000DE593 File Offset: 0x000DC793
		internal void Append(params byte[] bytes)
		{
			this.EnsureSpaceInBuffer(bytes.Length);
			bytes.CopyTo(this._buffer, this.Length);
			this._currentLineLength += bytes.Length;
			this._currentBufferUsed += bytes.Length;
		}

		// Token: 0x060040E8 RID: 16616 RVA: 0x000DE5D0 File Offset: 0x000DC7D0
		internal void AppendCRLF(bool includeSpace)
		{
			this.AppendFooter();
			this.Append(new byte[] { 13, 10 });
			this._currentLineLength = 0;
			if (includeSpace)
			{
				this.Append(32);
			}
			this.AppendHeader();
		}

		// Token: 0x060040E9 RID: 16617 RVA: 0x000DE606 File Offset: 0x000DC806
		internal void AppendHeader()
		{
			if (this.Header != null && this.Header.Length != 0)
			{
				this.Append(this.Header);
			}
		}

		// Token: 0x060040EA RID: 16618 RVA: 0x000DE625 File Offset: 0x000DC825
		internal void AppendFooter()
		{
			if (this.Footer != null && this.Footer.Length != 0)
			{
				this.Append(this.Footer);
			}
		}

		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x060040EB RID: 16619 RVA: 0x000DE644 File Offset: 0x000DC844
		internal int MaxLineLength
		{
			get
			{
				return this._maxLineLength;
			}
		}

		// Token: 0x060040EC RID: 16620 RVA: 0x000DE64C File Offset: 0x000DC84C
		internal void Reset()
		{
			this._currentBufferUsed = 0;
			this._currentLineLength = 0;
		}

		// Token: 0x060040ED RID: 16621 RVA: 0x000DE65C File Offset: 0x000DC85C
		internal void BufferFlushed()
		{
			this._currentBufferUsed = 0;
		}

		// Token: 0x040026E3 RID: 9955
		protected readonly byte[] _header;

		// Token: 0x040026E4 RID: 9956
		protected readonly byte[] _footer;

		// Token: 0x040026E5 RID: 9957
		protected readonly int _maxLineLength;

		// Token: 0x040026E6 RID: 9958
		protected byte[] _buffer;

		// Token: 0x040026E7 RID: 9959
		protected int _currentLineLength;

		// Token: 0x040026E8 RID: 9960
		protected int _currentBufferUsed;

		// Token: 0x040026E9 RID: 9961
		protected const int DefaultBufferSize = 1024;
	}
}
