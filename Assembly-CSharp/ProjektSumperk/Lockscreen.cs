using System;
using TMPro;
using UnityEngine;

namespace ProjektSumperk
{
	// Token: 0x020003F3 RID: 1011
	public class Lockscreen : MonoBehaviour
	{
		// Token: 0x060019AE RID: 6574 RVA: 0x000A2D68 File Offset: 0x000A0F68
		private void Update()
		{
			this.digitalTime.text = DateTime.Now.ToString("hh:mm");
			this.DateText.text = DateTime.Now.DayOfWeek.ToString() + " " + DateTime.Now.ToString("dd MMMM");
		}

		// Token: 0x04001ADD RID: 6877
		public TMP_Text digitalTime;

		// Token: 0x04001ADE RID: 6878
		public TMP_Text DateText;
	}
}
