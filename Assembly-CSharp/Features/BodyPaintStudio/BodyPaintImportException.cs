using System;

namespace Features.BodyPaintStudio
{
	// Token: 0x02001515 RID: 5397
	public sealed class BodyPaintImportException : Exception
	{
		// Token: 0x170019BB RID: 6587
		// (get) Token: 0x0600A6C5 RID: 42693 RVA: 0x0045B942 File Offset: 0x00459B42
		public BodyPaintImportError Code { get; }

		// Token: 0x0600A6C6 RID: 42694 RVA: 0x0045B94A File Offset: 0x00459B4A
		public BodyPaintImportException(BodyPaintImportError code, string message, Exception innerException = null)
			: base(message, innerException)
		{
			this.Code = code;
		}
	}
}
