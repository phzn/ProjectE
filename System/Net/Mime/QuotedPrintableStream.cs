using System;
using System.IO;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x020007E5 RID: 2021
	internal class QuotedPrintableStream : DelegatedStream, IEncodableStream
	{
		// Token: 0x060040B5 RID: 16565 RVA: 0x000DD6C8 File Offset: 0x000DB8C8
		internal QuotedPrintableStream(Stream stream, int lineLength)
			: base(stream)
		{
			if (lineLength < 0)
			{
				throw new ArgumentOutOfRangeException("lineLength");
			}
			this._lineLength = lineLength;
		}

		// Token: 0x060040B6 RID: 16566 RVA: 0x000DD6E7 File Offset: 0x000DB8E7
		internal QuotedPrintableStream(Stream stream, bool encodeCRLF)
			: this(stream, 70)
		{
			this._encodeCRLF = encodeCRLF;
		}

		// Token: 0x17000E95 RID: 3733
		// (get) Token: 0x060040B7 RID: 16567 RVA: 0x000DD6FC File Offset: 0x000DB8FC
		private QuotedPrintableStream.ReadStateInfo ReadState
		{
			get
			{
				QuotedPrintableStream.ReadStateInfo readStateInfo;
				if ((readStateInfo = this._readState) == null)
				{
					readStateInfo = (this._readState = new QuotedPrintableStream.ReadStateInfo());
				}
				return readStateInfo;
			}
		}

		// Token: 0x17000E96 RID: 3734
		// (get) Token: 0x060040B8 RID: 16568 RVA: 0x000DD724 File Offset: 0x000DB924
		internal WriteStateInfoBase WriteState
		{
			get
			{
				WriteStateInfoBase writeStateInfoBase;
				if ((writeStateInfoBase = this._writeState) == null)
				{
					writeStateInfoBase = (this._writeState = new WriteStateInfoBase(1024, null, null, this._lineLength));
				}
				return writeStateInfoBase;
			}
		}

		// Token: 0x060040B9 RID: 16569 RVA: 0x000DD758 File Offset: 0x000DB958
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
			QuotedPrintableStream.WriteAsyncResult writeAsyncResult = new QuotedPrintableStream.WriteAsyncResult(this, buffer, offset, count, callback, state);
			writeAsyncResult.Write();
			return writeAsyncResult;
		}

		// Token: 0x060040BA RID: 16570 RVA: 0x000DD7AE File Offset: 0x000DB9AE
		public override void Close()
		{
			this.FlushInternal();
			base.Close();
		}

		// Token: 0x060040BB RID: 16571 RVA: 0x000DD7BC File Offset: 0x000DB9BC
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
						byte b = QuotedPrintableStream.s_hexDecodeMap[(int)(*ptr3)];
						byte b2 = QuotedPrintableStream.s_hexDecodeMap[(int)ptr3[1]];
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
						byte b3 = QuotedPrintableStream.s_hexDecodeMap[(int)this.ReadState.Byte];
						byte b4 = QuotedPrintableStream.s_hexDecodeMap[(int)(*ptr3)];
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
								byte b5 = QuotedPrintableStream.s_hexDecodeMap[(int)ptr3[1]];
								byte b6 = QuotedPrintableStream.s_hexDecodeMap[(int)ptr3[2]];
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
				*(ptr4++) = *(ptr3++);
			}
			return (int)((long)(ptr4 - ptr2));
		}

		// Token: 0x060040BC RID: 16572 RVA: 0x000DDA20 File Offset: 0x000DBC20
		public int EncodeBytes(byte[] buffer, int offset, int count)
		{
			int i;
			for (i = offset; i < count + offset; i++)
			{
				if ((this._lineLength != -1 && this.WriteState.CurrentLineLength + 3 + 2 >= this._lineLength && (buffer[i] == 32 || buffer[i] == 9 || buffer[i] == 13 || buffer[i] == 10)) || this._writeState.CurrentLineLength + 3 + 2 >= 70)
				{
					if (this.WriteState.Buffer.Length - this.WriteState.Length < 3)
					{
						return i - offset;
					}
					this.WriteState.Append(61);
					this.WriteState.AppendCRLF(false);
				}
				if (buffer[i] == 13 && i + 1 < count + offset && buffer[i + 1] == 10)
				{
					if (this.WriteState.Buffer.Length - this.WriteState.Length < (this._encodeCRLF ? 6 : 2))
					{
						return i - offset;
					}
					i++;
					if (this._encodeCRLF)
					{
						this.WriteState.Append(new byte[] { 61, 48, 68, 61, 48, 65 });
					}
					else
					{
						this.WriteState.AppendCRLF(false);
					}
				}
				else if ((buffer[i] < 32 && buffer[i] != 9) || buffer[i] == 61 || buffer[i] > 126)
				{
					if (this.WriteState.Buffer.Length - this.WriteState.Length < 3)
					{
						return i - offset;
					}
					this.WriteState.Append(61);
					this.WriteState.Append(QuotedPrintableStream.s_hexEncodeMap[buffer[i] >> 4]);
					this.WriteState.Append(QuotedPrintableStream.s_hexEncodeMap[(int)(buffer[i] & 15)]);
				}
				else
				{
					if (this.WriteState.Buffer.Length - this.WriteState.Length < 1)
					{
						return i - offset;
					}
					if ((buffer[i] == 9 || buffer[i] == 32) && i + 1 >= count + offset)
					{
						if (this.WriteState.Buffer.Length - this.WriteState.Length < 3)
						{
							return i - offset;
						}
						this.WriteState.Append(61);
						this.WriteState.Append(QuotedPrintableStream.s_hexEncodeMap[buffer[i] >> 4]);
						this.WriteState.Append(QuotedPrintableStream.s_hexEncodeMap[(int)(buffer[i] & 15)]);
					}
					else
					{
						this.WriteState.Append(buffer[i]);
					}
				}
			}
			return i - offset;
		}

		// Token: 0x060040BD RID: 16573 RVA: 0x000075E1 File Offset: 0x000057E1
		public Stream GetStream()
		{
			return this;
		}

		// Token: 0x060040BE RID: 16574 RVA: 0x000DDC68 File Offset: 0x000DBE68
		public string GetEncodedString()
		{
			return Encoding.ASCII.GetString(this.WriteState.Buffer, 0, this.WriteState.Length);
		}

		// Token: 0x060040BF RID: 16575 RVA: 0x000DDC8B File Offset: 0x000DBE8B
		public override void EndWrite(IAsyncResult asyncResult)
		{
			QuotedPrintableStream.WriteAsyncResult.End(asyncResult);
		}

		// Token: 0x060040C0 RID: 16576 RVA: 0x000DDC93 File Offset: 0x000DBE93
		public override void Flush()
		{
			this.FlushInternal();
			base.Flush();
		}

		// Token: 0x060040C1 RID: 16577 RVA: 0x000DDCA1 File Offset: 0x000DBEA1
		private void FlushInternal()
		{
			if (this._writeState != null && this._writeState.Length > 0)
			{
				base.Write(this.WriteState.Buffer, 0, this.WriteState.Length);
				this.WriteState.BufferFlushed();
			}
		}

		// Token: 0x060040C2 RID: 16578 RVA: 0x000DDCE4 File Offset: 0x000DBEE4
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

		// Token: 0x040026BB RID: 9915
		private bool _encodeCRLF;

		// Token: 0x040026BC RID: 9916
		private const int SizeOfSoftCRLF = 3;

		// Token: 0x040026BD RID: 9917
		private const int SizeOfEncodedChar = 3;

		// Token: 0x040026BE RID: 9918
		private const int SizeOfEncodedCRLF = 6;

		// Token: 0x040026BF RID: 9919
		private const int SizeOfNonEncodedCRLF = 2;

		// Token: 0x040026C0 RID: 9920
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

		// Token: 0x040026C1 RID: 9921
		private static readonly byte[] s_hexEncodeMap = new byte[]
		{
			48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
			65, 66, 67, 68, 69, 70
		};

		// Token: 0x040026C2 RID: 9922
		private int _lineLength;

		// Token: 0x040026C3 RID: 9923
		private QuotedPrintableStream.ReadStateInfo _readState;

		// Token: 0x040026C4 RID: 9924
		private WriteStateInfoBase _writeState;

		// Token: 0x020007E6 RID: 2022
		private sealed class ReadStateInfo
		{
			// Token: 0x17000E97 RID: 3735
			// (get) Token: 0x060040C4 RID: 16580 RVA: 0x000DDD78 File Offset: 0x000DBF78
			// (set) Token: 0x060040C5 RID: 16581 RVA: 0x000DDD80 File Offset: 0x000DBF80
			internal bool IsEscaped { get; set; }

			// Token: 0x17000E98 RID: 3736
			// (get) Token: 0x060040C6 RID: 16582 RVA: 0x000DDD89 File Offset: 0x000DBF89
			// (set) Token: 0x060040C7 RID: 16583 RVA: 0x000DDD91 File Offset: 0x000DBF91
			internal short Byte { get; set; } = -1;
		}

		// Token: 0x020007E7 RID: 2023
		private sealed class WriteAsyncResult : LazyAsyncResult
		{
			// Token: 0x060040C9 RID: 16585 RVA: 0x000DDDA9 File Offset: 0x000DBFA9
			internal WriteAsyncResult(QuotedPrintableStream parent, byte[] buffer, int offset, int count, AsyncCallback callback, object state)
				: base(null, state, callback)
			{
				this._parent = parent;
				this._buffer = buffer;
				this._offset = offset;
				this._count = count;
			}

			// Token: 0x060040CA RID: 16586 RVA: 0x000DDDD3 File Offset: 0x000DBFD3
			private void CompleteWrite(IAsyncResult result)
			{
				this._parent.BaseStream.EndWrite(result);
				this._parent.WriteState.BufferFlushed();
			}

			// Token: 0x060040CB RID: 16587 RVA: 0x000DDDF6 File Offset: 0x000DBFF6
			internal static void End(IAsyncResult result)
			{
				((QuotedPrintableStream.WriteAsyncResult)result).InternalWaitForCompletion();
			}

			// Token: 0x060040CC RID: 16588 RVA: 0x000DDE04 File Offset: 0x000DC004
			private static void OnWrite(IAsyncResult result)
			{
				if (!result.CompletedSynchronously)
				{
					QuotedPrintableStream.WriteAsyncResult writeAsyncResult = (QuotedPrintableStream.WriteAsyncResult)result.AsyncState;
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

			// Token: 0x060040CD RID: 16589 RVA: 0x000DDE50 File Offset: 0x000DC050
			internal void Write()
			{
				for (;;)
				{
					this._written += this._parent.EncodeBytes(this._buffer, this._offset + this._written, this._count - this._written);
					if (this._written >= this._count)
					{
						break;
					}
					IAsyncResult asyncResult = this._parent.BaseStream.BeginWrite(this._parent.WriteState.Buffer, 0, this._parent.WriteState.Length, QuotedPrintableStream.WriteAsyncResult.s_onWrite, this);
					if (!asyncResult.CompletedSynchronously)
					{
						return;
					}
					this.CompleteWrite(asyncResult);
				}
				base.InvokeCallback();
			}

			// Token: 0x040026C7 RID: 9927
			private readonly QuotedPrintableStream _parent;

			// Token: 0x040026C8 RID: 9928
			private readonly byte[] _buffer;

			// Token: 0x040026C9 RID: 9929
			private readonly int _offset;

			// Token: 0x040026CA RID: 9930
			private readonly int _count;

			// Token: 0x040026CB RID: 9931
			private static readonly AsyncCallback s_onWrite = new AsyncCallback(QuotedPrintableStream.WriteAsyncResult.OnWrite);

			// Token: 0x040026CC RID: 9932
			private int _written;
		}
	}
}
