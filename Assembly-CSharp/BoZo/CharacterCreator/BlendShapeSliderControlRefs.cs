using System;
using UnityEngine;
using UnityEngine.UI;

namespace BoZo.CharacterCreator
{
	// Token: 0x020007B1 RID: 1969
	public class BlendShapeSliderControlRefs : MonoBehaviour
	{
		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x06003407 RID: 13319 RVA: 0x00128095 File Offset: 0x00126295
		public Button ResetButton
		{
			get
			{
				return this.resetButton;
			}
		}

		// Token: 0x0400348C RID: 13452
		[SerializeField]
		private Button resetButton;
	}
}
