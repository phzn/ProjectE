using System;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000EF5 RID: 3829
	[DisallowMultipleComponent]
	internal sealed class NPCSimulationPauseParticipantHook : MonoBehaviour
	{
		// Token: 0x060072B2 RID: 29362 RVA: 0x002EAB43 File Offset: 0x002E8D43
		private void Awake()
		{
			base.hideFlags = HideFlags.HideInInspector;
		}

		// Token: 0x060072B3 RID: 29363 RVA: 0x000320C6 File Offset: 0x000302C6
		private void OnEnable()
		{
			NPCSimulationPause.RegisterParticipant(this);
		}

		// Token: 0x060072B4 RID: 29364 RVA: 0x002EAB4C File Offset: 0x002E8D4C
		private void OnDisable()
		{
			NPCSimulationPause.UnregisterParticipant(base.transform);
		}
	}
}
