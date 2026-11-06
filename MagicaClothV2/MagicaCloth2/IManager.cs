using System;
using System.Text;

namespace MagicaCloth2
{
	// Token: 0x0200007B RID: 123
	public interface IManager : IDisposable
	{
		// Token: 0x060001E2 RID: 482
		void Initialize();

		// Token: 0x060001E3 RID: 483
		void EnterdEditMode();

		// Token: 0x060001E4 RID: 484
		void InformationLog(StringBuilder allsb);
	}
}
