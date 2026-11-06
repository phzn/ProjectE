using System;

namespace UnityEngine.InputSystem.Users
{
	// Token: 0x02000084 RID: 132
	[Serializable]
	internal class InputUserSettings
	{
		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x0003B110 File Offset: 0x00039310
		// (set) Token: 0x06000AE1 RID: 2785 RVA: 0x0003B118 File Offset: 0x00039318
		public string customBindings { get; set; }

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x0003B121 File Offset: 0x00039321
		// (set) Token: 0x06000AE3 RID: 2787 RVA: 0x0003B129 File Offset: 0x00039329
		public bool invertMouseX { get; set; }

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x0003B132 File Offset: 0x00039332
		// (set) Token: 0x06000AE5 RID: 2789 RVA: 0x0003B13A File Offset: 0x0003933A
		public bool invertMouseY { get; set; }

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x0003B143 File Offset: 0x00039343
		// (set) Token: 0x06000AE7 RID: 2791 RVA: 0x0003B14B File Offset: 0x0003934B
		public float? mouseSmoothing { get; set; }

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x0003B154 File Offset: 0x00039354
		// (set) Token: 0x06000AE9 RID: 2793 RVA: 0x0003B15C File Offset: 0x0003935C
		public float? mouseSensitivity { get; set; }

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000AEA RID: 2794 RVA: 0x0003B165 File Offset: 0x00039365
		// (set) Token: 0x06000AEB RID: 2795 RVA: 0x0003B16D File Offset: 0x0003936D
		public bool invertStickX { get; set; }

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x0003B176 File Offset: 0x00039376
		// (set) Token: 0x06000AED RID: 2797 RVA: 0x0003B17E File Offset: 0x0003937E
		public bool invertStickY { get; set; }

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x0003B187 File Offset: 0x00039387
		// (set) Token: 0x06000AEF RID: 2799 RVA: 0x0003B18F File Offset: 0x0003938F
		public bool swapSticks { get; set; }

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x0003B198 File Offset: 0x00039398
		// (set) Token: 0x06000AF1 RID: 2801 RVA: 0x0003B1A0 File Offset: 0x000393A0
		public bool swapBumpers { get; set; }

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x0003B1A9 File Offset: 0x000393A9
		// (set) Token: 0x06000AF3 RID: 2803 RVA: 0x0003B1B1 File Offset: 0x000393B1
		public bool swapTriggers { get; set; }

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x0003B1BA File Offset: 0x000393BA
		// (set) Token: 0x06000AF5 RID: 2805 RVA: 0x0003B1C2 File Offset: 0x000393C2
		public bool swapDpadAndLeftStick { get; set; }

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x0003B1CB File Offset: 0x000393CB
		// (set) Token: 0x06000AF7 RID: 2807 RVA: 0x0003B1D3 File Offset: 0x000393D3
		public float vibrationStrength { get; set; }

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0003B1DC File Offset: 0x000393DC
		public virtual void Apply(IInputActionCollection actions)
		{
		}

		// Token: 0x040003E1 RID: 993
		[SerializeField]
		private string m_CustomBindings;
	}
}
