using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200004A RID: 74
	[InputControlLayout(stateType = typeof(GravityState), displayName = "Gravity")]
	public class GravitySensor : Sensor
	{
		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x0002DDCB File Offset: 0x0002BFCB
		// (set) Token: 0x060007EE RID: 2030 RVA: 0x0002DDD3 File Offset: 0x0002BFD3
		public Vector3Control gravity { get; protected set; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x0002DDDC File Offset: 0x0002BFDC
		// (set) Token: 0x060007F0 RID: 2032 RVA: 0x0002DDE3 File Offset: 0x0002BFE3
		public static GravitySensor current { get; private set; }

		// Token: 0x060007F1 RID: 2033 RVA: 0x0002DDEB File Offset: 0x0002BFEB
		protected override void FinishSetup()
		{
			this.gravity = base.GetChildControl<Vector3Control>("gravity");
			base.FinishSetup();
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0002DE04 File Offset: 0x0002C004
		public override void MakeCurrent()
		{
			base.MakeCurrent();
			GravitySensor.current = this;
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x0002DE12 File Offset: 0x0002C012
		protected override void OnRemoved()
		{
			base.OnRemoved();
			if (GravitySensor.current == this)
			{
				GravitySensor.current = null;
			}
		}
	}
}
