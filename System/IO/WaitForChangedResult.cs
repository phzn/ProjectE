using System;

namespace System.IO
{
	/// <summary>Contains information on the change that occurred.</summary>
	// Token: 0x020004FA RID: 1274
	public struct WaitForChangedResult
	{
		// Token: 0x060029AA RID: 10666 RVA: 0x0008F1F1 File Offset: 0x0008D3F1
		internal WaitForChangedResult(WatcherChangeTypes changeType, string name, string oldName, bool timedOut)
		{
			this.ChangeType = changeType;
			this.Name = name;
			this.OldName = oldName;
			this.TimedOut = timedOut;
		}

		/// <summary>Gets or sets the type of change that occurred.</summary>
		/// <returns>One of the <see cref="T:System.IO.WatcherChangeTypes" /> values.</returns>
		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x060029AB RID: 10667 RVA: 0x0008F210 File Offset: 0x0008D410
		// (set) Token: 0x060029AC RID: 10668 RVA: 0x0008F218 File Offset: 0x0008D418
		public WatcherChangeTypes ChangeType { readonly get; set; }

		/// <summary>Gets or sets the name of the file or directory that changed.</summary>
		/// <returns>The name of the file or directory that changed.</returns>
		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x060029AD RID: 10669 RVA: 0x0008F221 File Offset: 0x0008D421
		// (set) Token: 0x060029AE RID: 10670 RVA: 0x0008F229 File Offset: 0x0008D429
		public string Name { readonly get; set; }

		/// <summary>Gets or sets the original name of the file or directory that was renamed.</summary>
		/// <returns>The original name of the file or directory that was renamed.</returns>
		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x060029AF RID: 10671 RVA: 0x0008F232 File Offset: 0x0008D432
		// (set) Token: 0x060029B0 RID: 10672 RVA: 0x0008F23A File Offset: 0x0008D43A
		public string OldName { readonly get; set; }

		/// <summary>Gets or sets a value indicating whether the wait operation timed out.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="M:System.IO.FileSystemWatcher.WaitForChanged(System.IO.WatcherChangeTypes)" /> method timed out; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x060029B1 RID: 10673 RVA: 0x0008F243 File Offset: 0x0008D443
		// (set) Token: 0x060029B2 RID: 10674 RVA: 0x0008F24B File Offset: 0x0008D44B
		public bool TimedOut { readonly get; set; }

		// Token: 0x040015FA RID: 5626
		internal static readonly WaitForChangedResult TimedOutResult = new WaitForChangedResult((WatcherChangeTypes)0, null, null, true);
	}
}
