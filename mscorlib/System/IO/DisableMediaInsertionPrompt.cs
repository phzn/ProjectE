using System;

namespace System.IO
{
	// Token: 0x02000B00 RID: 2816
	internal struct DisableMediaInsertionPrompt : IDisposable
	{
		// Token: 0x06006492 RID: 25746 RVA: 0x00155C18 File Offset: 0x00153E18
		public static DisableMediaInsertionPrompt Create()
		{
			DisableMediaInsertionPrompt disableMediaInsertionPrompt = default(DisableMediaInsertionPrompt);
			disableMediaInsertionPrompt._disableSuccess = Interop.Kernel32.SetThreadErrorMode(1U, out disableMediaInsertionPrompt._oldMode);
			return disableMediaInsertionPrompt;
		}

		// Token: 0x06006493 RID: 25747 RVA: 0x00155C44 File Offset: 0x00153E44
		public void Dispose()
		{
			if (this._disableSuccess)
			{
				uint num;
				Interop.Kernel32.SetThreadErrorMode(this._oldMode, out num);
			}
		}

		// Token: 0x04003B2A RID: 15146
		private bool _disableSuccess;

		// Token: 0x04003B2B RID: 15147
		private uint _oldMode;
	}
}
