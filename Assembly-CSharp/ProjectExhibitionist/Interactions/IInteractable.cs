using System;
using UnityEngine;

namespace ProjectExhibitionist.Interactions
{
	// Token: 0x02000909 RID: 2313
	public interface IInteractable
	{
		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06003E86 RID: 16006
		InteractionConfigSO Config { get; }

		// Token: 0x06003E87 RID: 16007
		bool CanInteract(GameObject player);

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06003E88 RID: 16008
		bool IsBeingUsed { get; }

		// Token: 0x06003E89 RID: 16009
		void OnInteract(GameObject player);

		// Token: 0x06003E8A RID: 16010
		void OnActionExecuted(string actionName, GameObject player);
	}
}
