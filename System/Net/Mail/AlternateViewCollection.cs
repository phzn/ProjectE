using System;
using System.Collections.ObjectModel;

namespace System.Net.Mail
{
	/// <summary>Represents a collection of <see cref="T:System.Net.Mail.AlternateView" /> objects.</summary>
	// Token: 0x020007F8 RID: 2040
	public sealed class AlternateViewCollection : Collection<AlternateView>, IDisposable
	{
		// Token: 0x0600413A RID: 16698 RVA: 0x000DFB40 File Offset: 0x000DDD40
		internal AlternateViewCollection()
		{
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Net.Mail.AlternateViewCollection" />.</summary>
		// Token: 0x0600413B RID: 16699 RVA: 0x00003917 File Offset: 0x00001B17
		public void Dispose()
		{
		}

		// Token: 0x0600413C RID: 16700 RVA: 0x000DFB48 File Offset: 0x000DDD48
		protected override void ClearItems()
		{
			base.ClearItems();
		}

		// Token: 0x0600413D RID: 16701 RVA: 0x000DFB50 File Offset: 0x000DDD50
		protected override void InsertItem(int index, AlternateView item)
		{
			base.InsertItem(index, item);
		}

		// Token: 0x0600413E RID: 16702 RVA: 0x000DFB5A File Offset: 0x000DDD5A
		protected override void RemoveItem(int index)
		{
			base.RemoveItem(index);
		}

		// Token: 0x0600413F RID: 16703 RVA: 0x000DFB63 File Offset: 0x000DDD63
		protected override void SetItem(int index, AlternateView item)
		{
			base.SetItem(index, item);
		}
	}
}
