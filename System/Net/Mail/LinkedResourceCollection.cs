using System;
using System.Collections.ObjectModel;

namespace System.Net.Mail
{
	/// <summary>Stores linked resources to be sent as part of an email message.</summary>
	// Token: 0x020007FF RID: 2047
	public sealed class LinkedResourceCollection : Collection<LinkedResource>, IDisposable
	{
		// Token: 0x06004172 RID: 16754 RVA: 0x000E2297 File Offset: 0x000E0497
		internal LinkedResourceCollection()
		{
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Net.Mail.LinkedResourceCollection" />.</summary>
		// Token: 0x06004173 RID: 16755 RVA: 0x000E229F File Offset: 0x000E049F
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004174 RID: 16756 RVA: 0x00003917 File Offset: 0x00001B17
		private void Dispose(bool disposing)
		{
		}

		// Token: 0x06004175 RID: 16757 RVA: 0x000E22AE File Offset: 0x000E04AE
		protected override void ClearItems()
		{
			base.ClearItems();
		}

		// Token: 0x06004176 RID: 16758 RVA: 0x000E22B6 File Offset: 0x000E04B6
		protected override void InsertItem(int index, LinkedResource item)
		{
			base.InsertItem(index, item);
		}

		// Token: 0x06004177 RID: 16759 RVA: 0x000E22C0 File Offset: 0x000E04C0
		protected override void RemoveItem(int index)
		{
			base.RemoveItem(index);
		}

		// Token: 0x06004178 RID: 16760 RVA: 0x000E22C9 File Offset: 0x000E04C9
		protected override void SetItem(int index, LinkedResource item)
		{
			base.SetItem(index, item);
		}
	}
}
