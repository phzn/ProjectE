using System;
using UnityEngine;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x0200070D RID: 1805
	public interface IActionController
	{
		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06002EAA RID: 11946
		// (set) Token: 0x06002EAB RID: 11947
		bool enabled { get; set; }

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06002EAC RID: 11948
		GameObject gameObject { get; }

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06002EAD RID: 11949
		Transform transform { get; }

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06002EAE RID: 11950
		string name { get; }

		// Token: 0x06002EAF RID: 11951
		Type GetType();
	}
}
