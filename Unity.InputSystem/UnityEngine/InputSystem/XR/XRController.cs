using System;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.XR;

namespace UnityEngine.InputSystem.XR
{
	// Token: 0x02000066 RID: 102
	[InputControlLayout(commonUsages = new string[] { "LeftHand", "RightHand" }, isGenericTypeOfDevice = true, displayName = "XR Controller")]
	public class XRController : TrackedDevice
	{
		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060009ED RID: 2541 RVA: 0x00037129 File Offset: 0x00035329
		public static XRController leftHand
		{
			get
			{
				return InputSystem.GetDevice<XRController>(CommonUsages.LeftHand);
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060009EE RID: 2542 RVA: 0x00037135 File Offset: 0x00035335
		public static XRController rightHand
		{
			get
			{
				return InputSystem.GetDevice<XRController>(CommonUsages.RightHand);
			}
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00037144 File Offset: 0x00035344
		protected override void FinishSetup()
		{
			base.FinishSetup();
			XRDeviceDescriptor xrdeviceDescriptor = XRDeviceDescriptor.FromJson(base.description.capabilities);
			if (xrdeviceDescriptor != null)
			{
				if ((xrdeviceDescriptor.characteristics & InputDeviceCharacteristics.Left) != InputDeviceCharacteristics.None)
				{
					InputSystem.SetDeviceUsage(this, CommonUsages.LeftHand);
					return;
				}
				if ((xrdeviceDescriptor.characteristics & InputDeviceCharacteristics.Right) != InputDeviceCharacteristics.None)
				{
					InputSystem.SetDeviceUsage(this, CommonUsages.RightHand);
				}
			}
		}
	}
}
