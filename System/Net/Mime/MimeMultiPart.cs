using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace System.Net.Mime
{
	// Token: 0x020007DB RID: 2011
	internal class MimeMultiPart : MimeBasePart
	{
		// Token: 0x06004063 RID: 16483 RVA: 0x000DC21B File Offset: 0x000DA41B
		internal MimeMultiPart(MimeMultiPartType type)
		{
			this.MimeMultiPartType = type;
		}

		// Token: 0x17000E8B RID: 3723
		// (set) Token: 0x06004064 RID: 16484 RVA: 0x000DC22A File Offset: 0x000DA42A
		internal MimeMultiPartType MimeMultiPartType
		{
			set
			{
				if (value > MimeMultiPartType.Related || value < MimeMultiPartType.Mixed)
				{
					throw new NotSupportedException(value.ToString());
				}
				this.SetType(value);
			}
		}

		// Token: 0x06004065 RID: 16485 RVA: 0x000DC24E File Offset: 0x000DA44E
		private void SetType(MimeMultiPartType type)
		{
			base.ContentType.MediaType = "multipart/" + type.ToString().ToLower(CultureInfo.InvariantCulture);
			base.ContentType.Boundary = this.GetNextBoundary();
		}

		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x06004066 RID: 16486 RVA: 0x000DC28D File Offset: 0x000DA48D
		internal Collection<MimeBasePart> Parts
		{
			get
			{
				if (this._parts == null)
				{
					this._parts = new Collection<MimeBasePart>();
				}
				return this._parts;
			}
		}

		// Token: 0x06004067 RID: 16487 RVA: 0x000DC2A8 File Offset: 0x000DA4A8
		internal void Complete(IAsyncResult result, Exception e)
		{
			MimeMultiPart.MimePartContext mimePartContext = (MimeMultiPart.MimePartContext)result.AsyncState;
			if (mimePartContext._completed)
			{
				ExceptionDispatchInfo.Throw(e);
			}
			try
			{
				mimePartContext._outputStream.Close();
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

		// Token: 0x06004068 RID: 16488 RVA: 0x000DC30C File Offset: 0x000DA50C
		internal void MimeWriterCloseCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimeMultiPart.MimePartContext)result.AsyncState)._completedSynchronously = false;
			try
			{
				this.MimeWriterCloseCallbackHandler(result);
			}
			catch (Exception ex)
			{
				this.Complete(result, ex);
			}
		}

		// Token: 0x06004069 RID: 16489 RVA: 0x000DC358 File Offset: 0x000DA558
		private void MimeWriterCloseCallbackHandler(IAsyncResult result)
		{
			((MimeWriter)((MimeMultiPart.MimePartContext)result.AsyncState)._writer).EndClose(result);
			this.Complete(result, null);
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x000DC380 File Offset: 0x000DA580
		internal void MimePartSentCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimeMultiPart.MimePartContext)result.AsyncState)._completedSynchronously = false;
			try
			{
				this.MimePartSentCallbackHandler(result);
			}
			catch (Exception ex)
			{
				this.Complete(result, ex);
			}
		}

		// Token: 0x0600406B RID: 16491 RVA: 0x000DC3CC File Offset: 0x000DA5CC
		private void MimePartSentCallbackHandler(IAsyncResult result)
		{
			MimeMultiPart.MimePartContext mimePartContext = (MimeMultiPart.MimePartContext)result.AsyncState;
			mimePartContext._partsEnumerator.Current.EndSend(result);
			if (mimePartContext._partsEnumerator.MoveNext())
			{
				IAsyncResult asyncResult = mimePartContext._partsEnumerator.Current.BeginSend(mimePartContext._writer, this._mimePartSentCallback, this._allowUnicode, mimePartContext);
				if (asyncResult.CompletedSynchronously)
				{
					this.MimePartSentCallbackHandler(asyncResult);
				}
				return;
			}
			IAsyncResult asyncResult2 = ((MimeWriter)mimePartContext._writer).BeginClose(new AsyncCallback(this.MimeWriterCloseCallback), mimePartContext);
			if (asyncResult2.CompletedSynchronously)
			{
				this.MimeWriterCloseCallbackHandler(asyncResult2);
			}
		}

		// Token: 0x0600406C RID: 16492 RVA: 0x000DC464 File Offset: 0x000DA664
		internal void ContentStreamCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimeMultiPart.MimePartContext)result.AsyncState)._completedSynchronously = false;
			try
			{
				this.ContentStreamCallbackHandler(result);
			}
			catch (Exception ex)
			{
				this.Complete(result, ex);
			}
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x000DC4B0 File Offset: 0x000DA6B0
		private void ContentStreamCallbackHandler(IAsyncResult result)
		{
			MimeMultiPart.MimePartContext mimePartContext = (MimeMultiPart.MimePartContext)result.AsyncState;
			mimePartContext._outputStream = mimePartContext._writer.EndGetContentStream(result);
			mimePartContext._writer = new MimeWriter(mimePartContext._outputStream, base.ContentType.Boundary);
			if (mimePartContext._partsEnumerator.MoveNext())
			{
				MimeBasePart mimeBasePart = mimePartContext._partsEnumerator.Current;
				this._mimePartSentCallback = new AsyncCallback(this.MimePartSentCallback);
				IAsyncResult asyncResult = mimeBasePart.BeginSend(mimePartContext._writer, this._mimePartSentCallback, this._allowUnicode, mimePartContext);
				if (asyncResult.CompletedSynchronously)
				{
					this.MimePartSentCallbackHandler(asyncResult);
				}
				return;
			}
			IAsyncResult asyncResult2 = ((MimeWriter)mimePartContext._writer).BeginClose(new AsyncCallback(this.MimeWriterCloseCallback), mimePartContext);
			if (asyncResult2.CompletedSynchronously)
			{
				this.MimeWriterCloseCallbackHandler(asyncResult2);
			}
		}

		// Token: 0x0600406E RID: 16494 RVA: 0x000DC578 File Offset: 0x000DA778
		internal override IAsyncResult BeginSend(BaseWriter writer, AsyncCallback callback, bool allowUnicode, object state)
		{
			this._allowUnicode = allowUnicode;
			base.PrepareHeaders(allowUnicode);
			writer.WriteHeaders(base.Headers, allowUnicode);
			MimeBasePart.MimePartAsyncResult mimePartAsyncResult = new MimeBasePart.MimePartAsyncResult(this, state, callback);
			MimeMultiPart.MimePartContext mimePartContext = new MimeMultiPart.MimePartContext(writer, mimePartAsyncResult, this.Parts.GetEnumerator());
			IAsyncResult asyncResult = writer.BeginGetContentStream(new AsyncCallback(this.ContentStreamCallback), mimePartContext);
			if (asyncResult.CompletedSynchronously)
			{
				this.ContentStreamCallbackHandler(asyncResult);
			}
			return mimePartAsyncResult;
		}

		// Token: 0x0600406F RID: 16495 RVA: 0x000DC5E4 File Offset: 0x000DA7E4
		internal override void Send(BaseWriter writer, bool allowUnicode)
		{
			base.PrepareHeaders(allowUnicode);
			writer.WriteHeaders(base.Headers, allowUnicode);
			Stream contentStream = writer.GetContentStream();
			MimeWriter mimeWriter = new MimeWriter(contentStream, base.ContentType.Boundary);
			foreach (MimeBasePart mimeBasePart in this.Parts)
			{
				mimeBasePart.Send(mimeWriter, allowUnicode);
			}
			mimeWriter.Close();
			contentStream.Close();
		}

		// Token: 0x06004070 RID: 16496 RVA: 0x000DC66C File Offset: 0x000DA86C
		internal string GetNextBoundary()
		{
			return "--boundary_" + (Interlocked.Increment(ref MimeMultiPart.s_boundary) - 1).ToString(CultureInfo.InvariantCulture) + "_" + Guid.NewGuid().ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x0400268C RID: 9868
		private Collection<MimeBasePart> _parts;

		// Token: 0x0400268D RID: 9869
		private static int s_boundary;

		// Token: 0x0400268E RID: 9870
		private AsyncCallback _mimePartSentCallback;

		// Token: 0x0400268F RID: 9871
		private bool _allowUnicode;

		// Token: 0x020007DC RID: 2012
		internal class MimePartContext
		{
			// Token: 0x06004071 RID: 16497 RVA: 0x000DC6B4 File Offset: 0x000DA8B4
			internal MimePartContext(BaseWriter writer, LazyAsyncResult result, IEnumerator<MimeBasePart> partsEnumerator)
			{
				this._writer = writer;
				this._result = result;
				this._partsEnumerator = partsEnumerator;
			}

			// Token: 0x04002690 RID: 9872
			internal IEnumerator<MimeBasePart> _partsEnumerator;

			// Token: 0x04002691 RID: 9873
			internal Stream _outputStream;

			// Token: 0x04002692 RID: 9874
			internal LazyAsyncResult _result;

			// Token: 0x04002693 RID: 9875
			internal BaseWriter _writer;

			// Token: 0x04002694 RID: 9876
			internal bool _completed;

			// Token: 0x04002695 RID: 9877
			internal bool _completedSynchronously = true;
		}
	}
}
