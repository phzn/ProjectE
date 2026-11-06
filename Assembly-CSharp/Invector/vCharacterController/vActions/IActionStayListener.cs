using System;
using UnityEngine;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x02000711 RID: 1809
	public interface IActionStayListener : IActionController
	{
		// Token: 0x06002EB3 RID: 11955
		void OnActionStay(Collider actionCollider);
	}
}
