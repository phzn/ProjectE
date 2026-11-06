using System;
using Invector.vCharacterController;

namespace Invector.vItemManager
{
	// Token: 0x0200055C RID: 1372
	[Serializable]
	public class ChangeEquipmentControl
	{
		// Token: 0x0400217D RID: 8573
		public GenericInput useItemInput = new GenericInput("U", "Start", "Start");

		// Token: 0x0400217E RID: 8574
		public GenericInput previousItemInput = new GenericInput("LeftArrow", "D - Pad Horizontal", "D-Pad Horizontal");

		// Token: 0x0400217F RID: 8575
		public GenericInput nextItemInput = new GenericInput("RightArrow", "D - Pad Horizontal", "D-Pad Horizontal");

		// Token: 0x04002180 RID: 8576
		public vEquipArea equipArea;

		// Token: 0x04002181 RID: 8577
		public vEquipmentDisplay display;
	}
}
