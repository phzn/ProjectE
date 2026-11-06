using System;
using System.IO;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x020007E2 RID: 2018
	internal class QEncodedStream : DelegatedStream, IEncodableStream
	{
		// Token: 0x0600409A RID: 16538 RVA: 0x000DCF9B File Offset: 0x000DB19B
		internal QEncodedStream(WriteStateInfoBase wsi)
			: base(new MemoryStream())
		{
			this._writeState = wsi;
		}

		// Token: 0x17000E91 RID: 3729
		// (get) Token: 0x0600409B RID: 16539 RVA: 0x000DCFB0 File Offset: 0x000DB1B0
		private QEncodedStream.ReadStateInfo ReadState
		{
			get
			{
				QEncodedStream.ReadStateInfo readStateInfo;
				if ((readStateInfo = this._readState) == null)
				{
					readStateInfo = (this._readState = new QEncodedStream.ReadStateInfo());
				}
				return readStateInfo;
			}
		}

		// Token: 0x17000E92 RID: 3730
		// (get) Token: 0x0600409C RID: 16540 RVA: 0x000DCFD5 File Offset: 0x000DB1D5
		internal WriteStateInfoBase WriteState
		{
			get
			{
				return this._writeState;
			}
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x000DCFE0 File Offset: 0x000DB1E0
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			QEncodedStream.WriteAsyncResult writeAsyncResult = new QEncodedStream.WriteAsyncResult(this, buffer, offset, count, callback, state);
			writeAsyncResult.Write();
			return writeAsyncResult;
		}

		// Token: 0x0600409E RID: 16542 RVA: 0x000DD036 File Offset: 0x000DB236
		public override void Close()
		{
			this.FlushInternal();
			base.Close();
		}

		// Token: 0x0600409F RID: 16543 RVA: 0x000DD044 File Offset: 0x000DB244
		public unsafe int DecodeBytes(byte[] buffer, int offset, int count)
		{
			byte* ptr;
			if (buffer == null || buffer.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &buffer[0];
			}
			byte* ptr2 = ptr + offset;
			byte* ptr3 = ptr2;
			byte* ptr4 = ptr2;
			byte* ptr5 = ptr2 + count;
			if (this.ReadState.IsEscaped)
			{
				if (this.ReadState.Byte == -1)
				{
					if (count == 1)
					{
						this.ReadState.Byte = (short)(*ptr3);
						return 0;
					}
					if (*ptr3 != 13 || ptr3[1] != 10)
					{
						byte b = QEncodedStream.s_hexDecodeMap[(int)(*ptr3)];
						byte b2 = QEncodedStream.s_hexDecodeMap[(int)ptr3[1]];
						if (b == 255)
						{
							throw new FormatException(SR.Format("Invalid hex digit '{0}'.", b));
						}
						if (b2 == 255)
						{
							throw new FormatException(SR.Format("Invalid hex digit '{0}'.", b2));
						}
						*(ptr4++) = (byte)(((int)b << 4) + (int)b2);
					}
					ptr3 += 2;
				}
				else
				{
					if (this.ReadState.Byte != 13 || *ptr3 != 10)
					{
						byte b3 = QEncodedStream.s_hexDecodeMap[(int)this.ReadState.Byte];
						byte b4 = QEncodedStream.s_hexDecodeMap[(int)(*ptr3)];
						if (b3 == 255)
						{
							throw new FormatException(SR.Format("Invalid hex digit '{0}'.", b3));
						}
						if (b4 == 255)
						{
							throw new FormatException(SR.Format("Invalid hex digit '{0}'.", b4));
						}
						*(ptr4++) = (byte)(((int)b3 << 4) + (int)b4);
					}
					ptr3++;
				}
				this.ReadState.IsEscaped = false;
				this.ReadState.Byte = -1;
			}
			while (ptr3 < ptr5)
			{
				if (*ptr3 == 61)
				{
					long num = (long)(ptr5 - ptr3);
					if (num != 1L)
					{
						if (num != 2L)
						{
							if (ptr3[1] != 13 || ptr3[2] != 10)
							{
								byte b5 = QEncodedStream.s_hexDecodeMap[(int)ptr3[1]];
								byte b6 = QEncodedStream.s_hexDecodeMap[(int)ptr3[2]];
								if (b5 == 255)
								{
									throw new FormatException(SR.Format("Invalid hex digit '{0}'.", b5));
								}
								if (b6 == 255)
								{
									throw new FormatException(SR.Format("Invalid hex digit '{0}'.", b6));
								}
								*(ptr4++) = (byte)(((int)b5 << 4) + (int)b6);
							}
							ptr3 += 3;
							continue;
						}
						this.ReadState.Byte = (short)ptr3[1];
					}
					this.ReadState.IsEscaped = true;
					break;
				}
				if (*ptr3 == 95)
				{
					*(ptr4++) = 32;
					ptr3++;
				}
				else
				{
					*(ptr4++) = *(ptr3++);
				}
			}
			return (int)((long)(ptr4 - ptr2));
		}

		// Token: 0x060040A0 RID: 16544 RVA: 0x000DD2C0 File Offset: 0x000DB4C0
		public int EncodeBytes(byte[] buffer, int offset, int count)
		{
			this._writeState.AppendHeader();
			int i;
			for (i = offset; i < count + offset; i++)
			{
				if ((this.WriteState.CurrentLineLength + 3 + this.WriteState.FooterLength >= this.WriteState.MaxLineLength && (buffer[i] == 32 || buffer[i] == 9 || buffer[i] == 13 || buffer[i] == 10)) || this.WriteState.CurrentLineLength + this._writeState.FooterLength >= this.WriteState.MaxLineLength)
				{
					this.WriteState.AppendCRLF(true);
				}
				if (buffer[i] == 13 && i + 1 < count + offset && buffer[i + 1] == 10)
				{
					i++;
					this.WriteState.Append(new byte[] { 61, 48, 68, 61, 48, 65 });
				}
				else if (buffer[i] == 32)
				{
					this.WriteState.Append(95);
				}
				else if (QEncodedStream.IsAsciiLetterOrDigit((char)buffer[i]))
				{
					this.WriteState.Append(buffer[i]);
				}
				else
				{
					this.WriteState.Append(61);
					this.WriteState.Append(QEncodedStream.s_hexEncodeMap[buffer[i] >> 4]);
					this.WriteState.Append(QEncodedStream.s_hexEncodeMap[(int)(buffer[i] & 15)]);
				}
			}
			this.WriteState.AppendFooter();
			return i - offset;
		}

		// Token: 0x060040A1 RID: 16545 RVA: 0x000DD410 File Offset: 0x000DB610
		private static bool IsAsciiLetterOrDigit(char character)
		{
			return QEncodedStream.IsAsciiLetter(character) || (character >= '0' && character <= '9');
		}

		// Token: 0x060040A2 RID: 16546 RVA: 0x00027FB4 File Offset: 0x000261B4
		private static bool IsAsciiLetter(char character)
		{
			return (character >= 'a' && character <= 'z') || (character >= 'A' && character <= 'Z');
		}

		// Token: 0x060040A3 RID: 16547 RVA: 0x000075E1 File Offset: 0x000057E1
		public Stream GetStream()
		{
			return this;
		}

		// Token: 0x060040A4 RID: 16548 RVA: 0x000DD42B File Offset: 0x000DB62B
		public string GetEncodedString()
		{
			return Encoding.ASCII.GetString(this.WriteState.Buffer, 0, this.WriteState.Length);
		}

		// Token: 0x060040A5 RID: 16549 RVA: 0x000DD44E File Offset: 0x000DB64E
		public override void EndWrite(IAsyncResult asyncResult)
		{
			QEncodedStream.WriteAsyncResult.End(asyncResult);
		}

		// Token: 0x060040A6 RID: 16550 RVA: 0x000DD456 File Offset: 0x000DB656
		public override void Flush()
		{
			this.FlushInternal();
			base.Flush();
		}

		// Token: 0x060040A7 RID: 16551 RVA: 0x000DD464 File Offset: 0x000DB664
		private void FlushInternal()
		{
			if (this._writeState != null && this._writeState.Length > 0)
			{
				base.Write(this.WriteState.Buffer, 0, this.WriteState.Length);
				this.WriteState.Reset();
			}
		}

		// Token: 0x060040A8 RID: 16552 RVA: 0x000DD4A4 File Offset: 0x000DB6A4
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			int num = 0;
			for (;;)
			{
				num += this.EncodeBytes(buffer, offset + num, count - num);
				if (num >= count)
				{
					break;
				}
				this.FlushInternal();
			}
		}

		// Token: 0x040026AE RID: 9902
		private const int SizeOfFoldingCRLF = 3;

		// Token: 0x040026AF RID: 9903
		private static readonly byte[] s_hexDecodeMap = new byte[]
		{
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 0, 1,
			2, 3, 4, 5, 6, 7, 8, 9, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 10, 11, 12, 13, 14,
			15, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 10, 11, 12,
			13, 14, 15, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue
		};

		// Token: 0x040026B0 RID: 9904
		private static readonly byte[] s_hexEncodeMap = new byte[]
		{
			48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
			65, 66, 67, 68, 69, 70
		};

		// Token: 0x040026B1 RID: 9905
		private QEncodedStream.ReadStateInfo _readState;

		// Token: 0x040026B2 RID: 9906
		private WriteStateInfoBase _writeState;

		// Token: 0x020007E3 RID: 2019
		private sealed class ReadStateInfo
		{
			// Token: 0x17000E93 RID: 3731
			// (get) Token: 0x060040AA RID: 16554 RVA: 0x000DD538 File Offset: 0x000DB738
			// (set) Token: 0x060040AB RID: 16555 RVA: 0x000DD540 File Offset: 0x000DB740
			internal bool IsEscaped { get; set; }

			// Token: 0x17000E94 RID: 3732
			// (get) Token: 0x060040AC RID: 16556 RVA: 0x000DD549 File Offset: 0x000DB749
			// (set) Token: 0x060040AD RID: 16557 RVA: 0x000DD551 File Offset: 0x000DB751
			internal short Byte { get; set; } = -1;
		}

		// Token: 0x020007E4 RID: 2020
		private class WriteAsyncResult : LazyAsyncResult
		{
			// Token: 0x060040AF RID: 16559 RVA: 0x000DD569 File Offset: 0x000DB769
			internal WriteAsyncResult(QEncodedStream parent, byte[] buffer, int offset, int count, AsyncCallback callback, object state)
				: base(null, state, callback)
			{
				this._parent = parent;
				this._buffer = buffer;
				this._offset = offset;
				this._count = count;
			}

			// Token: 0x060040B0 RID: 16560 RVA: 0x000DD593 File Offset: 0x000DB793
			private void CompleteWrite(IAsyncResult result)
			{
				this._parent.BaseStream.EndWrite(result);
				this._parent.WriteState.Reset();
			}

			// Token: 0x060040B1 RID: 16561 RVA: 0x000DD5B6 File Offset: 0x000DB7B6
			internal static void End(IAsyncResult result)
			{
				((QEncodedStream.WriteAsyncResult)result).InternalWaitForCompletion();
			}

			// Token: 0x060040B2 RID: 16562 RVA: 0x000DD5C4 File Offset: 0x000DB7C4
			private static void OnWrite(IAsyncResult result)
			{
				if (!result.CompletedSynchronously)
				{
					QEncodedStream.WriteAsyncResult writeAsyncResult = (QEncodedStream.WriteAsyncResult)result.AsyncState;
					try
					{
						writeAsyncResult.CompleteWrite(result);
						writeAsyncResult.Write();
					}
					catch (Exception ex)
					{
						writeAsyncResult.InvokeCallback(ex);
					}
				}
			}

			// Token: 0x060040B3 RID: 16563 RVA: 0x000DD610 File Offset: 0x000DB810
			internal void Write()
			{
				for (;;)
				{
					this._written += this._parent.EncodeBytes(this._buffer, this._offset + this._written, this._count - this._written);
					if (this._written >= this._count)
					{
						break;
					}
					IAsyncResult asyncResult = this._parent.BaseStream.BeginWrite(this._parent.WriteState.Buffer, 0, this._parent.WriteState.Length, QEncodedStream.WriteAsyncResult.s_onWrite, this);
					if (!asyncResult.CompletedSynchronously)
					{
						return;
					}
					this.CompleteWrite(asyncResult);
				}
				base.InvokeCallback();
			}

			// Token: 0x040026B5 RID: 9909
			private static readonly AsyncCallback s_onWrite = new AsyncCallback(QEncodedStream.WriteAsyncResult.OnWrite);

			// Token: 0x040026B6 RID: 9910
			private readonly QEncodedStream _parent;

			// Token: 0x040026B7 RID: 9911
			private readonly byte[] _buffer;

			// Token: 0x040026B8 RID: 9912
			private readonly int _offset;

			// Token: 0x040026B9 RID: 9913
			private readonly int _count;

			// Token: 0x040026BA RID: 9914
			private int _written;
		}
	}
}
