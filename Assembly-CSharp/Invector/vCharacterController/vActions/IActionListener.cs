using System;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x02000712 RID: 1810
	public interface IActionListener : IActionEnterListener, IActionController, IActionExitListener, IActionStayListener
	{
		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06002EB4 RID: 11956
		// (set) Token: 0x06002EB5 RID: 11957
		bool actionEnter { get; set; }

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06002EB6 RID: 11958
		// (set) Token: 0x06002EB7 RID: 11959
		bool actionExit { get; set; }

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06002EB8 RID: 11960
		// (set) Token: 0x06002EB9 RID: 11961
		bool actionStay { get; set; }
	}
}
