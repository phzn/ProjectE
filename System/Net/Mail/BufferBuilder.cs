using System;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x020007F1 RID: 2033
	internal sealed class BufferBuilder
	{
		// Token: 0x060040FF RID: 16639 RVA: 0x000DEE37 File Offset: 0x000DD037
		internal BufferBuilder()
			: this(256)
		{
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x000DEE44 File Offset: 0x000DD044
		internal BufferBuilder(int initialSize)
		{
			this._buffer = new byte[initialSize];
		}

		// Token: 0x06004101 RID: 16641 RVA: 0x000DEE58 File Offset: 0x000DD058
		private void EnsureBuffer(int count)
		{
			if (count > this._buffer.Length - this._offset)
			{
				byte[] array = new byte[(this._buffer.Length * 2 > this._buffer.Length + count) ? (this._buffer.Length * 2) : (this._buffer.Length + count)];
				Buffer.BlockCopy(this._buffer, 0, array, 0, this._offset);
				this._buffer = array;
			}
		}

		// Token: 0x06004102 RID: 16642 RVA: 0x000DEEC4 File Offset: 0x000DD0C4
		internal void Append(byte value)
		{
			this.EnsureBuffer(1);
			byte[] buffer = this._buffer;
			int offset = this._offset;
			this._offset = offset + 1;
			buffer[offset] = value;
		}

		// Token: 0x06004103 RID: 16643 RVA: 0x000DEEF1 File Offset: 0x000DD0F1
		internal void Append(byte[] value)
		{
			this.Append(value, 0, value.Length);
		}

		// Token: 0x06004104 RID: 16644 RVA: 0x000DEEFE File Offset: 0x000DD0FE
		internal void Append(byte[] value, int offset, int count)
		{
			this.EnsureBuffer(count);
			Buffer.BlockCopy(value, offset, this._buffer, this._offset, count);
			this._offset += count;
		}

		// Token: 0x06004105 RID: 16645 RVA: 0x000DEF29 File Offset: 0x000DD129
		internal void Append(string value)
		{
			this.Append(value, false);
		}

		// Token: 0x06004106 RID: 16646 RVA: 0x000DEF33 File Offset: 0x000DD133
		internal void Append(string value, bool allowUnicode)
		{
			if (string.IsNullOrEmpty(value))
			{
				return;
			}
			this.Append(value, 0, value.Length, allowUnicode);
		}

		// Token: 0x06004107 RID: 16647 RVA: 0x000DEF50 File Offset: 0x000DD150
		internal void Append(string value, int offset, int count, bool allowUnicode)
		{
			if (allowUnicode)
			{
				int byteCount = Encoding.UTF8.GetByteCount(value, offset, count);
				this.EnsureBuffer(byteCount);
				Encoding.UTF8.GetBytes(value, offset, count, this._buffer, this._offset);
				this._offset += byteCount;
				return;
			}
			this.Append(value, offset, count);
		}

		// Token: 0x06004108 RID: 16648 RVA: 0x000DEFA8 File Offset: 0x000DD1A8
		internal void Append(string value, int offset, int count)
		{
			this.EnsureBuffer(count);
			for (int i = 0; i < count; i++)
			{
				char c = value[offset + i];
				if (c > 'ÿ')
				{
					throw new FormatException(SR.Format("An invalid character was found in the mail header: '{0}'.", c));
				}
				this._buffer[this._offset + i] = (byte)c;
			}
			this._offset += count;
		}

		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x06004109 RID: 16649 RVA: 0x000DF00F File Offset: 0x000DD20F
		internal int Length
		{
			get
			{
				return this._offset;
			}
		}

		// Token: 0x0600410A RID: 16650 RVA: 0x000DF017 File Offset: 0x000DD217
		internal byte[] GetBuffer()
		{
			return this._buffer;
		}

		// Token: 0x0600410B RID: 16651 RVA: 0x000DF01F File Offset: 0x000DD21F
		internal void Reset()
		{
			this._offset = 0;
		}

		// Token: 0x040026EA RID: 9962
		private byte[] _buffer;

		// Token: 0x040026EB RID: 9963
		private int _offset;
	}
}
