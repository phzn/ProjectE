using System;
using System.Collections.ObjectModel;

namespace System.Net.Mail
{
	/// <summary>Stores attachments to be sent as part of an email message.</summary>
	// Token: 0x020007FC RID: 2044
	public sealed class AttachmentCollection : Collection<Attachment>, IDisposable
	{
		// Token: 0x06004161 RID: 16737 RVA: 0x000E214A File Offset: 0x000E034A
		internal AttachmentCollection()
		{
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Net.Mail.AttachmentCollection" />.</summary>
		// Token: 0x06004162 RID: 16738 RVA: 0x000E2154 File Offset: 0x000E0354
		public void Dispose()
		{
			for (int i = 0; i < base.Count; i++)
			{
				base[i].Dispose();
			}
		}

		// Token: 0x06004163 RID: 16739 RVA: 0x000E217E File Offset: 0x000E037E
		protected override void ClearItems()
		{
			base.ClearItems();
		}

		// Token: 0x06004164 RID: 16740 RVA: 0x000E2186 File Offset: 0x000E0386
		protected override void InsertItem(int index, Attachment item)
		{
			base.InsertItem(index, item);
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x000E2190 File Offset: 0x000E0390
		protected override void RemoveItem(int index)
		{
			base.RemoveItem(index);
		}

		// Token: 0x06004166 RID: 16742 RVA: 0x000E2199 File Offset: 0x000E0399
		protected override void SetItem(int index, Attachment item)
		{
			base.SetItem(index, item);
		}
	}
}
