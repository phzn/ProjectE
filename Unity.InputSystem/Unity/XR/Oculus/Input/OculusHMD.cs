using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XR;

namespace Unity.XR.Oculus.Input
{
	// Token: 0x0200000B RID: 11
	[InputControlLayout(displayName = "Oculus Headset", hideInUI = true)]
	public class OculusHMD : XRHMD
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600006F RID: 111 RVA: 0x000027CE File Offset: 0x000009CE
		// (set) Token: 0x06000070 RID: 112 RVA: 0x000027D6 File Offset: 0x000009D6
		[InputControl]
		[InputControl(name = "trackingState", layout = "Integer", aliases = new string[] { "devicetrackingstate" })]
		[InputControl(name = "isTracked", layout = "Button", aliases = new string[] { "deviceistracked" })]
		public ButtonControl userPresence { get; protected set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000027DF File Offset: 0x000009DF
		// (set) Token: 0x06000072 RID: 114 RVA: 0x000027E7 File Offset: 0x000009E7
		[InputControl(noisy = true)]
		public Vector3Control deviceAngularVelocity { get; protected set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000073 RID: 115 RVA: 0x000027F0 File Offset: 0x000009F0
		// (set) Token: 0x06000074 RID: 116 RVA: 0x000027F8 File Offset: 0x000009F8
		[InputControl(noisy = true)]
		public Vector3Control deviceAcceleration { get; protected set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00002801 File Offset: 0x00000A01
		// (set) Token: 0x06000076 RID: 118 RVA: 0x00002809 File Offset: 0x00000A09
		[InputControl(noisy = true)]
		public Vector3Control deviceAngularAcceleration { get; protected set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00002812 File Offset: 0x00000A12
		// (set) Token: 0x06000078 RID: 120 RVA: 0x0000281A File Offset: 0x00000A1A
		[InputControl(noisy = true)]
		public Vector3Control leftEyeAngularVelocity { get; protected set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00002823 File Offset: 0x00000A23
		// (set) Token: 0x0600007A RID: 122 RVA: 0x0000282B File Offset: 0x00000A2B
		[InputControl(noisy = true)]
		public Vector3Control leftEyeAcceleration { get; protected set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00002834 File Offset: 0x00000A34
		// (set) Token: 0x0600007C RID: 124 RVA: 0x0000283C File Offset: 0x00000A3C
		[InputControl(noisy = true)]
		public Vector3Control leftEyeAngularAcceleration { get; protected set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00002845 File Offset: 0x00000A45
		// (set) Token: 0x0600007E RID: 126 RVA: 0x0000284D File Offset: 0x00000A4D
		[InputControl(noisy = true)]
		public Vector3Control rightEyeAngularVelocity { get; protected set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00002856 File Offset: 0x00000A56
		// (set) Token: 0x06000080 RID: 128 RVA: 0x0000285E File Offset: 0x00000A5E
		[InputControl(noisy = true)]
		public Vector3Control rightEyeAcceleration { get; protected set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00002867 File Offset: 0x00000A67
		// (set) Token: 0x06000082 RID: 130 RVA: 0x0000286F File Offset: 0x00000A6F
		[InputControl(noisy = true)]
		public Vector3Control rightEyeAngularAcceleration { get; protected set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00002878 File Offset: 0x00000A78
		// (set) Token: 0x06000084 RID: 132 RVA: 0x00002880 File Offset: 0x00000A80
		[InputControl(noisy = true)]
		public Vector3Control centerEyeAngularVelocity { get; protected set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00002889 File Offset: 0x00000A89
		// (set) Token: 0x06000086 RID: 134 RVA: 0x00002891 File Offset: 0x00000A91
		[InputControl(noisy = true)]
		public Vector3Control centerEyeAcceleration { get; protected set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000087 RID: 135 RVA: 0x0000289A File Offset: 0x00000A9A
		// (set) Token: 0x06000088 RID: 136 RVA: 0x000028A2 File Offset: 0x00000AA2
		[InputControl(noisy = true)]
		public Vector3Control centerEyeAngularAcceleration { get; protected set; }

		// Token: 0x06000089 RID: 137 RVA: 0x000028AC File Offset: 0x00000AAC
		protected override void FinishSetup()
		{
			base.FinishSetup();
			this.userPresence = base.GetChildControl<ButtonControl>("userPresence");
			this.deviceAngularVelocity = base.GetChildControl<Vector3Control>("deviceAngularVelocity");
			this.deviceAcceleration = base.GetChildControl<Vector3Control>("deviceAcceleration");
			this.deviceAngularAcceleration = base.GetChildControl<Vector3Control>("deviceAngularAcceleration");
			this.leftEyeAngularVelocity = base.GetChildControl<Vector3Control>("leftEyeAngularVelocity");
			this.leftEyeAcceleration = base.GetChildControl<Vector3Control>("leftEyeAcceleration");
			this.leftEyeAngularAcceleration = base.GetChildControl<Vector3Control>("leftEyeAngularAcceleration");
			this.rightEyeAngularVelocity = base.GetChildControl<Vector3Control>("rightEyeAngularVelocity");
			this.rightEyeAcceleration = base.GetChildControl<Vector3Control>("rightEyeAcceleration");
			this.rightEyeAngularAcceleration = base.GetChildControl<Vector3Control>("rightEyeAngularAcceleration");
			this.centerEyeAngularVelocity = base.GetChildControl<Vector3Control>("centerEyeAngularVelocity");
			this.centerEyeAcceleration = base.GetChildControl<Vector3Control>("centerEyeAcceleration");
			this.centerEyeAngularAcceleration = base.GetChildControl<Vector3Control>("centerEyeAngularAcceleration");
		}
	}
}
