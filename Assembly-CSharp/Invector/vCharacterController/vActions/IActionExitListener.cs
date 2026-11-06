using System;
using UnityEngine;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x02000710 RID: 1808
	public interface IActionExitListener : IActionController
	{
		// Token: 0x06002EB2 RID: 11954
		void OnActionExit(Collider actionCollider);
	}
}
