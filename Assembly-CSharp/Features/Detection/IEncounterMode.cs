using System;
using System.Collections;
using Invector.vCharacterController;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012B9 RID: 4793
	public interface IEncounterMode
	{
		// Token: 0x170017AC RID: 6060
		// (get) Token: 0x060094A0 RID: 38048
		GenericInput EnterInput { get; }

		// Token: 0x170017AD RID: 6061
		// (get) Token: 0x060094A1 RID: 38049
		string UIHintText { get; }

		// Token: 0x170017AE RID: 6062
		// (get) Token: 0x060094A2 RID: 38050
		GameObject HintUI { get; }

		// Token: 0x170017AF RID: 6063
		// (get) Token: 0x060094A3 RID: 38051
		bool IsActive { get; }

		// Token: 0x060094A4 RID: 38052
		IEnumerator Execute(ExhibitionEventDirector director);
	}
}
