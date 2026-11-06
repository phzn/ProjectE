using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Unity.Entities.Content
{
	// Token: 0x0200047C RID: 1148
	internal struct ManagedStringTable : IDisposable
	{
		// Token: 0x06001C45 RID: 7237 RVA: 0x0006324C File Offset: 0x0006144C
		public ManagedStringTable(int capacity)
		{
			this.ValuesHandle = GCHandle.Alloc(new List<string>(capacity));
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06001C46 RID: 7238 RVA: 0x0006325F File Offset: 0x0006145F
		public bool IsCreated
		{
			get
			{
				return this.ValuesHandle.IsAllocated;
			}
		}

		// Token: 0x06001C47 RID: 7239 RVA: 0x0006326C File Offset: 0x0006146C
		public int Add(string s)
		{
			List<string> list = this.ValuesHandle.Target as List<string>;
			list.Add(s);
			return list.Count - 1;
		}

		// Token: 0x06001C48 RID: 7240 RVA: 0x0006328C File Offset: 0x0006148C
		public void Dispose()
		{
			this.ValuesHandle.Free();
		}

		// Token: 0x17000245 RID: 581
		public string this[int i]
		{
			get
			{
				return (this.ValuesHandle.Target as List<string>)[i];
			}
		}

		// Token: 0x04000A04 RID: 2564
		private GCHandle ValuesHandle;
	}
}
