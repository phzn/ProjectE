using System;
using UnityEngine;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x0200070F RID: 1807
	public interface IActionEnterListener : IActionController
	{
		// Token: 0x06002EB1 RID: 11953
		void OnActionEnter(Collider actionCollider);
	}
}
