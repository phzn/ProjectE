using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x02000009 RID: 9
	public class ContactProvider : MonoBehaviour
	{
		// Token: 0x0600004B RID: 75 RVA: 0x00003ABA File Offset: 0x00001CBA
		public bool GetContact()
		{
			return this.contact;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003AC2 File Offset: 0x00001CC2
		private void OnTriggerStay(Collider other)
		{
			this.contact = true;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003ACB File Offset: 0x00001CCB
		private void OnTriggerExit(Collider other)
		{
			this.contact = false;
		}

		// Token: 0x0400004B RID: 75
		private bool contact;
	}
}
