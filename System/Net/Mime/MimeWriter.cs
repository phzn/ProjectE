using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x020007E0 RID: 2016
	internal class MimeWriter : BaseWriter
	{
		// Token: 0x06004088 RID: 16520 RVA: 0x000DCD32 File Offset: 0x000DAF32
		internal MimeWriter(Stream stream, string boundary)
			: base(stream, false)
		{
			if (boundary == null)
			{
				throw new ArgumentNullException("boundary");
			}
			this._boundaryBytes = Encoding.ASCII.GetBytes(boundary);
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x000DCD64 File Offset: 0x000DAF64
		internal override void WriteHeaders(NameValueCollection headers, bool allowUnicode)
		{
			if (headers == null)
			{
				throw new ArgumentNullException("headers");
			}
			foreach (object obj in headers)
			{
				string text = (string)obj;
				base.WriteHeader(text, headers[text], allowUnicode);
			}
		}

		// Token: 0x0600408A RID: 16522 RVA: 0x000DCDD0 File Offset: 0x000DAFD0
		internal IAsyncResult BeginClose(AsyncCallback callback, object state)
		{
			MultiAsyncResult multiAsyncResult = new MultiAsyncResult(this, callback, state);
			this.Close(multiAsyncResult);
			multiAsyncResult.CompleteSequence();
			return multiAsyncResult;
		}

		// Token: 0x0600408B RID: 16523 RVA: 0x000DCDF4 File Offset: 0x000DAFF4
		internal void EndClose(IAsyncResult result)
		{
			MultiAsyncResult.End(result);
			this._stream.Close();
		}

		// Token: 0x0600408C RID: 16524 RVA: 0x000DCE08 File Offset: 0x000DB008
		internal override void Close()
		{
			this.Close(null);
			this._stream.Close();
		}

		// Token: 0x0600408D RID: 16525 RVA: 0x000DCE1C File Offset: 0x000DB01C
		private void Close(MultiAsyncResult multiResult)
		{
			this._bufferBuilder.Append(BaseWriter.s_crlf);
			this._bufferBuilder.Append(MimeWriter.s_DASHDASH);
			this._bufferBuilder.Append(this._boundaryBytes);
			this._bufferBuilder.Append(MimeWriter.s_DASHDASH);
			this._bufferBuilder.Append(BaseWriter.s_crlf);
			base.Flush(multiResult);
		}

		// Token: 0x0600408E RID: 16526 RVA: 0x000DCE81 File Offset: 0x000DB081
		protected override void OnClose(object sender, EventArgs args)
		{
			if (this._contentStream != sender)
			{
				return;
			}
			this._contentStream.Flush();
			this._contentStream = null;
			this._writeBoundary = true;
			this._isInContent = false;
		}

		// Token: 0x0600408F RID: 16527 RVA: 0x000DCEB0 File Offset: 0x000DB0B0
		protected override void CheckBoundary()
		{
			if (this._writeBoundary)
			{
				this._bufferBuilder.Append(BaseWriter.s_crlf);
				this._bufferBuilder.Append(MimeWriter.s_DASHDASH);
				this._bufferBuilder.Append(this._boundaryBytes);
				this._bufferBuilder.Append(BaseWriter.s_crlf);
				this._writeBoundary = false;
			}
		}

		// Token: 0x040026A9 RID: 9897
		private static byte[] s_DASHDASH = new byte[] { 45, 45 };

		// Token: 0x040026AA RID: 9898
		private byte[] _boundaryBytes;

		// Token: 0x040026AB RID: 9899
		private bool _writeBoundary = true;
	}
}
