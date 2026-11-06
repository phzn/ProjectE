using System;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x0200070E RID: 1806
	public interface IActionReceiver : IActionController
	{
		// Token: 0x06002EB0 RID: 11952
		void OnReceiveAction(vTriggerGenericAction actionInfo);
	}
}
