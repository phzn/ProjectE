using System;
using System.Threading.Tasks;

namespace System.Xml
{
	// Token: 0x02000015 RID: 21
	internal abstract class Base64Encoder
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00002DC7 File Offset: 0x00000FC7
		internal Base64Encoder()
		{
			this.charsLine = new char[76];
		}

		// Token: 0x0600003E RID: 62
		internal abstract void WriteChars(char[] chars, int index, int count);

		// Token: 0x0600003F RID: 63 RVA: 0x00002DDC File Offset: 0x00000FDC
		internal void Encode(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > buffer.Length - index)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.leftOverBytesCount > 0)
			{
				int num = this.leftOverBytesCount;
				while (num < 3 && count > 0)
				{
					this.leftOverBytes[num++] = buffer[index++];
					count--;
				}
				if (count == 0 && num < 3)
				{
					this.leftOverBytesCount = num;
					return;
				}
				int num2 = Convert.ToBase64CharArray(this.leftOverBytes, 0, 3, this.charsLine, 0);
				this.WriteChars(this.charsLine, 0, num2);
			}
			this.leftOverBytesCount = count % 3;
			if (this.leftOverBytesCount > 0)
			{
				count -= this.leftOverBytesCount;
				if (this.leftOverBytes == null)
				{
					this.leftOverBytes = new byte[3];
				}
				for (int i = 0; i < this.leftOverBytesCount; i++)
				{
					this.leftOverBytes[i] = buffer[index + count + i];
				}
			}
			int num3 = index + count;
			int num4 = 57;
			while (index < num3)
			{
				if (index + num4 > num3)
				{
					num4 = num3 - index;
				}
				int num5 = Convert.ToBase64CharArray(buffer, index, num4, this.charsLine, 0);
				this.WriteChars(this.charsLine, 0, num5);
				index += num4;
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002F20 File Offset: 0x00001120
		internal void Flush()
		{
			if (this.leftOverBytesCount > 0)
			{
				int num = Convert.ToBase64CharArray(this.leftOverBytes, 0, this.leftOverBytesCount, this.charsLine, 0);
				this.WriteChars(this.charsLine, 0, num);
				this.leftOverBytesCount = 0;
			}
		}

		// Token: 0x06000041 RID: 65
		internal abstract Task WriteCharsAsync(char[] chars, int index, int count);

		// Token: 0x06000042 RID: 66 RVA: 0x00002F68 File Offset: 0x00001168
		internal async Task EncodeAsync(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > buffer.Length - index)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.leftOverBytesCount > 0)
			{
				int num = this.leftOverBytesCount;
				while (num < 3 && count > 0)
				{
					byte[] array = this.leftOverBytes;
					int num2 = num++;
					int num3 = index;
					index = num3 + 1;
					array[num2] = buffer[num3];
					num3 = count;
					count = num3 - 1;
				}
				if (count == 0 && num < 3)
				{
					this.leftOverBytesCount = num;
					return;
				}
				int num4 = Convert.ToBase64CharArray(this.leftOverBytes, 0, 3, this.charsLine, 0);
				await this.WriteCharsAsync(this.charsLine, 0, num4).ConfigureAwait(false);
			}
			this.leftOverBytesCount = count % 3;
			if (this.leftOverBytesCount > 0)
			{
				count -= this.leftOverBytesCount;
				if (this.leftOverBytes == null)
				{
					this.leftOverBytes = new byte[3];
				}
				for (int i = 0; i < this.leftOverBytesCount; i++)
				{
					this.leftOverBytes[i] = buffer[index + count + i];
				}
			}
			int endIndex = index + count;
			int chunkSize = 57;
			while (index < endIndex)
			{
				if (index + chunkSize > endIndex)
				{
					chunkSize = endIndex - index;
				}
				int num5 = Convert.ToBase64CharArray(buffer, index, chunkSize, this.charsLine, 0);
				await this.WriteCharsAsync(this.charsLine, 0, num5).ConfigureAwait(false);
				index += chunkSize;
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002FC4 File Offset: 0x000011C4
		internal async Task FlushAsync()
		{
			if (this.leftOverBytesCount > 0)
			{
				int num = Convert.ToBase64CharArray(this.leftOverBytes, 0, this.leftOverBytesCount, this.charsLine, 0);
				await this.WriteCharsAsync(this.charsLine, 0, num).ConfigureAwait(false);
				this.leftOverBytesCount = 0;
			}
		}

		// Token: 0x040004EB RID: 1259
		private byte[] leftOverBytes;

		// Token: 0x040004EC RID: 1260
		private int leftOverBytesCount;

		// Token: 0x040004ED RID: 1261
		private char[] charsLine;

		// Token: 0x040004EE RID: 1262
		internal const int Base64LineSize = 76;

		// Token: 0x040004EF RID: 1263
		internal const int LineSizeInBytes = 57;
	}
}
