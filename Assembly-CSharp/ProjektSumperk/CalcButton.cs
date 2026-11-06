using System;
using TMPro;
using UnityEngine;

namespace ProjektSumperk
{
	// Token: 0x020003CE RID: 974
	public class CalcButton : MonoBehaviour
	{
		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x060018F0 RID: 6384 RVA: 0x0009E88B File Offset: 0x0009CA8B
		public CalculatorManager calcManager
		{
			get
			{
				if (CalcButton._calcManager == null)
				{
					CalcButton._calcManager = base.GetComponentInParent<CalculatorManager>();
				}
				return CalcButton._calcManager;
			}
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x0009E8AA File Offset: 0x0009CAAA
		public void onTapped()
		{
			this.calcManager.buttonTapped(this.label.text[0]);
		}

		// Token: 0x040019EE RID: 6638
		public TMP_Text label;

		// Token: 0x040019EF RID: 6639
		private static CalculatorManager _calcManager;
	}
}
