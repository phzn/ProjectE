using System;
using System.IO;
using System.Net.Mail;
using System.Runtime.ExceptionServices;

namespace System.Net.Mime
{
	// Token: 0x020007DE RID: 2014
	internal class MimePart : MimeBasePart, IDisposable
	{
		// Token: 0x06004072 RID: 16498 RVA: 0x000DC6D8 File Offset: 0x000DA8D8
		internal MimePart()
		{
		}

		// Token: 0x06004073 RID: 16499 RVA: 0x000DC6E0 File Offset: 0x000DA8E0
		public void Dispose()
		{
			if (this._stream != null)
			{
				this._stream.Close();
			}
		}

		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x06004074 RID: 16500 RVA: 0x000DC6F5 File Offset: 0x000DA8F5
		internal Stream Stream
		{
			get
			{
				return this._stream;
			}
		}

		// Token: 0x17000E8E RID: 3726
		// (get) Token: 0x06004075 RID: 16501 RVA: 0x000DC6FD File Offset: 0x000DA8FD
		// (set) Token: 0x06004076 RID: 16502 RVA: 0x000DC705 File Offset: 0x000DA905
		internal ContentDisposition ContentDisposition
		{
			get
			{
				return this._contentDisposition;
			}
			set
			{
				this._contentDisposition = value;
				if (value == null)
				{
					((HeaderCollection)base.Headers).InternalRemove(MailHeaderInfo.GetString(MailHeaderID.ContentDisposition));
					return;
				}
				this._contentDisposition.PersistIfNeeded((HeaderCollection)base.Headers, true);
			}
		}

		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x06004077 RID: 16503 RVA: 0x000DC740 File Offset: 0x000DA940
		// (set) Token: 0x06004078 RID: 16504 RVA: 0x000DC7A0 File Offset: 0x000DA9A0
		internal TransferEncoding TransferEncoding
		{
			get
			{
				string text = base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)];
				if (text.Equals("base64", StringComparison.OrdinalIgnoreCase))
				{
					return TransferEncoding.Base64;
				}
				if (text.Equals("quoted-printable", StringComparison.OrdinalIgnoreCase))
				{
					return TransferEncoding.QuotedPrintable;
				}
				if (text.Equals("7bit", StringComparison.OrdinalIgnoreCase))
				{
					return TransferEncoding.SevenBit;
				}
				if (text.Equals("8bit", StringComparison.OrdinalIgnoreCase))
				{
					return TransferEncoding.EightBit;
				}
				return TransferEncoding.Unknown;
			}
			set
			{
				if (value == TransferEncoding.Base64)
				{
					base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)] = "base64";
					return;
				}
				if (value == TransferEncoding.QuotedPrintable)
				{
					base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)] = "quoted-printable";
					return;
				}
				if (value == TransferEncoding.SevenBit)
				{
					base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)] = "7bit";
					return;
				}
				if (value == TransferEncoding.EightBit)
				{
					base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)] = "8bit";
					return;
				}
				throw new NotSupportedException(SR.Format("The MIME transfer encoding '{0}' is not supported.", value));
			}
		}

		// Token: 0x06004079 RID: 16505 RVA: 0x000DC830 File Offset: 0x000DAA30
		internal void SetContent(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (this._streamSet)
			{
				this._stream.Close();
				this._stream = null;
				this._streamSet = false;
			}
			this._stream = stream;
			this._streamSet = true;
			this._streamUsedOnce = false;
			this.TransferEncoding = TransferEncoding.Base64;
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x000DC888 File Offset: 0x000DAA88
		internal void SetContent(Stream stream, string name, string mimeType)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (mimeType != null && mimeType != string.Empty)
			{
				this._contentType = new ContentType(mimeType);
			}
			if (name != null && name != string.Empty)
			{
				base.ContentType.Name = name;
			}
			this.SetContent(stream);
		}

		// Token: 0x0600407B RID: 16507 RVA: 0x000DC8E2 File Offset: 0x000DAAE2
		internal void SetContent(Stream stream, ContentType contentType)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this._contentType = contentType;
			this.SetContent(stream);
		}

		// Token: 0x0600407C RID: 16508 RVA: 0x000DC900 File Offset: 0x000DAB00
		internal void Complete(IAsyncResult result, Exception e)
		{
			MimePart.MimePartContext mimePartContext = (MimePart.MimePartContext)result.AsyncState;
			if (mimePartContext._completed)
			{
				ExceptionDispatchInfo.Throw(e);
			}
			try
			{
				if (mimePartContext._outputStream != null)
				{
					mimePartContext._outputStream.Close();
				}
			}
			catch (Exception ex)
			{
				if (e == null)
				{
					e = ex;
				}
			}
			mimePartContext._completed = true;
			mimePartContext._result.InvokeCallback(e);
		}

		// Token: 0x0600407D RID: 16509 RVA: 0x000DC96C File Offset: 0x000DAB6C
		internal void ReadCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimePart.MimePartContext)result.AsyncState)._completedSynchronously = false;
			try
			{
				this.ReadCallbackHandler(result);
			}
			catch (Exception ex)
			{
				this.Complete(result, ex);
			}
		}

		// Token: 0x0600407E RID: 16510 RVA: 0x000DC9B8 File Offset: 0x000DABB8
		internal void ReadCallbackHandler(IAsyncResult result)
		{
			MimePart.MimePartContext mimePartContext = (MimePart.MimePartContext)result.AsyncState;
			mimePartContext._bytesLeft = this.Stream.EndRead(result);
			if (mimePartContext._bytesLeft > 0)
			{
				IAsyncResult asyncResult = mimePartContext._outputStream.BeginWrite(mimePartContext._buffer, 0, mimePartContext._bytesLeft, this._writeCallback, mimePartContext);
				if (asyncResult.CompletedSynchronously)
				{
					this.WriteCallbackHandler(asyncResult);
					return;
				}
			}
			else
			{
				this.Complete(result, null);
			}
		}

		// Token: 0x0600407F RID: 16511 RVA: 0x000DCA24 File Offset: 0x000DAC24
		internal void WriteCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimePart.MimePartContext)result.AsyncState)._completedSynchronously = false;
			try
			{
				this.WriteCallbackHandler(result);
			}
			catch (Exception ex)
			{
				this.Complete(result, ex);
			}
		}

		// Token: 0x06004080 RID: 16512 RVA: 0x000DCA70 File Offset: 0x000DAC70
		internal void WriteCallbackHandler(IAsyncResult result)
		{
			MimePart.MimePartContext mimePartContext = (MimePart.MimePartContext)result.AsyncState;
			mimePartContext._outputStream.EndWrite(result);
			IAsyncResult asyncResult = this.Stream.BeginRead(mimePartContext._buffer, 0, mimePartContext._buffer.Length, this._readCallback, mimePartContext);
			if (asyncResult.CompletedSynchronously)
			{
				this.ReadCallbackHandler(asyncResult);
			}
		}

		// Token: 0x06004081 RID: 16513 RVA: 0x000DCAC8 File Offset: 0x000DACC8
		internal Stream GetEncodedStream(Stream stream)
		{
			Stream stream2 = stream;
			if (this.TransferEncoding == TransferEncoding.Base64)
			{
				stream2 = new Base64Stream(stream2, new Base64WriteStateInfo());
			}
			else if (this.TransferEncoding == TransferEncoding.QuotedPrintable)
			{
				stream2 = new QuotedPrintableStream(stream2, true);
			}
			else if (this.TransferEncoding == TransferEncoding.SevenBit || this.TransferEncoding == TransferEncoding.EightBit)
			{
				stream2 = new EightBitStream(stream2);
			}
			return stream2;
		}

		// Token: 0x06004082 RID: 16514 RVA: 0x000DCB1C File Offset: 0x000DAD1C
		internal void ContentStreamCallbackHandler(IAsyncResult result)
		{
			MimePart.MimePartContext mimePartContext = (MimePart.MimePartContext)result.AsyncState;
			Stream stream = mimePartContext._writer.EndGetContentStream(result);
			mimePartContext._outputStream = this.GetEncodedStream(stream);
			this._readCallback = new AsyncCallback(this.ReadCallback);
			this._writeCallback = new AsyncCallback(this.WriteCallback);
			IAsyncResult asyncResult = this.Stream.BeginRead(mimePartContext._buffer, 0, mimePartContext._buffer.Length, this._readCallback, mimePartContext);
			if (asyncResult.CompletedSynchronously)
			{
				this.ReadCallbackHandler(asyncResult);
			}
		}

		// Token: 0x06004083 RID: 16515 RVA: 0x000DCBA4 File Offset: 0x000DADA4
		internal void ContentStreamCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimePart.MimePartContext)result.AsyncState)._completedSynchronously = false;
			try
			{
				this.ContentStreamCallbackHandler(result);
			}
			catch (Exception ex)
			{
				this.Complete(result, ex);
			}
		}

		// Token: 0x06004084 RID: 16516 RVA: 0x000DCBF0 File Offset: 0x000DADF0
		internal override IAsyncResult BeginSend(BaseWriter writer, AsyncCallback callback, bool allowUnicode, object state)
		{
			base.PrepareHeaders(allowUnicode);
			writer.WriteHeaders(base.Headers, allowUnicode);
			MimeBasePart.MimePartAsyncResult mimePartAsyncResult = new MimeBasePart.MimePartAsyncResult(this, state, callback);
			MimePart.MimePartContext mimePartContext = new MimePart.MimePartContext(writer, mimePartAsyncResult);
			this.ResetStream();
			this._streamUsedOnce = true;
			IAsyncResult asyncResult = writer.BeginGetContentStream(new AsyncCallback(this.ContentStreamCallback), mimePartContext);
			if (asyncResult.CompletedSynchronously)
			{
				this.ContentStreamCallbackHandler(asyncResult);
			}
			return mimePartAsyncResult;
		}

		// Token: 0x06004085 RID: 16517 RVA: 0x000DCC54 File Offset: 0x000DAE54
		internal override void Send(BaseWriter writer, bool allowUnicode)
		{
			if (this.Stream != null)
			{
				byte[] array = new byte[17408];
				base.PrepareHeaders(allowUnicode);
				writer.WriteHeaders(base.Headers, allowUnicode);
				Stream stream = writer.GetContentStream();
				stream = this.GetEncodedStream(stream);
				this.ResetStream();
				this._streamUsedOnce = true;
				int num;
				while ((num = this.Stream.Read(array, 0, 17408)) > 0)
				{
					stream.Write(array, 0, num);
				}
				stream.Close();
			}
		}

		// Token: 0x06004086 RID: 16518 RVA: 0x000DCCCC File Offset: 0x000DAECC
		internal void ResetStream()
		{
			if (!this._streamUsedOnce)
			{
				return;
			}
			if (this.Stream.CanSeek)
			{
				this.Stream.Seek(0L, SeekOrigin.Begin);
				this._streamUsedOnce = false;
				return;
			}
			throw new InvalidOperationException("One of the streams has already been used and can't be reset to the origin.");
		}

		// Token: 0x0400269C RID: 9884
		private Stream _stream;

		// Token: 0x0400269D RID: 9885
		private bool _streamSet;

		// Token: 0x0400269E RID: 9886
		private bool _streamUsedOnce;

		// Token: 0x0400269F RID: 9887
		private AsyncCallback _readCallback;

		// Token: 0x040026A0 RID: 9888
		private AsyncCallback _writeCallback;

		// Token: 0x040026A1 RID: 9889
		private const int maxBufferSize = 17408;

		// Token: 0x020007DF RID: 2015
		internal class MimePartContext
		{
			// Token: 0x06004087 RID: 16519 RVA: 0x000DCD05 File Offset: 0x000DAF05
			internal MimePartContext(BaseWriter writer, LazyAsyncResult result)
			{
				this._writer = writer;
				this._result = result;
				this._buffer = new byte[17408];
			}

			// Token: 0x040026A2 RID: 9890
			internal Stream _outputStream;

			// Token: 0x040026A3 RID: 9891
			internal LazyAsyncResult _result;

			// Token: 0x040026A4 RID: 9892
			internal int _bytesLeft;

			// Token: 0x040026A5 RID: 9893
			internal BaseWriter _writer;

			// Token: 0x040026A6 RID: 9894
			internal byte[] _buffer;

			// Token: 0x040026A7 RID: 9895
			internal bool _completed;

			// Token: 0x040026A8 RID: 9896
			internal bool _completedSynchronously = true;
		}
	}
}
