using System;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UIElements;

namespace UnityEngine.InputSystem.UI
{
	// Token: 0x02000087 RID: 135
	public class ExtendedPointerEventData : PointerEventData
	{
		// Token: 0x06000B00 RID: 2816 RVA: 0x0003B232 File Offset: 0x00039432
		public ExtendedPointerEventData(EventSystem eventSystem)
			: base(eventSystem)
		{
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x0003B23B File Offset: 0x0003943B
		// (set) Token: 0x06000B02 RID: 2818 RVA: 0x0003B243 File Offset: 0x00039443
		public InputControl control { get; set; }

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000B03 RID: 2819 RVA: 0x0003B24C File Offset: 0x0003944C
		// (set) Token: 0x06000B04 RID: 2820 RVA: 0x0003B254 File Offset: 0x00039454
		public InputDevice device { get; set; }

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x0003B25D File Offset: 0x0003945D
		// (set) Token: 0x06000B06 RID: 2822 RVA: 0x0003B265 File Offset: 0x00039465
		public int touchId { get; set; }

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000B07 RID: 2823 RVA: 0x0003B26E File Offset: 0x0003946E
		// (set) Token: 0x06000B08 RID: 2824 RVA: 0x0003B276 File Offset: 0x00039476
		public UIPointerType pointerType { get; set; }

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x0003B27F File Offset: 0x0003947F
		// (set) Token: 0x06000B0A RID: 2826 RVA: 0x0003B287 File Offset: 0x00039487
		public int uiToolkitPointerId { get; set; }

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000B0B RID: 2827 RVA: 0x0003B290 File Offset: 0x00039490
		// (set) Token: 0x06000B0C RID: 2828 RVA: 0x0003B298 File Offset: 0x00039498
		public Vector3 trackedDevicePosition { get; set; }

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000B0D RID: 2829 RVA: 0x0003B2A1 File Offset: 0x000394A1
		// (set) Token: 0x06000B0E RID: 2830 RVA: 0x0003B2A9 File Offset: 0x000394A9
		public Quaternion trackedDeviceOrientation { get; set; }

		// Token: 0x06000B0F RID: 2831 RVA: 0x0003B2B4 File Offset: 0x000394B4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.ToString());
			stringBuilder.AppendLine("button: " + base.button.ToString());
			stringBuilder.AppendLine("clickTime: " + base.clickTime.ToString());
			stringBuilder.AppendLine("clickCount: " + base.clickCount.ToString());
			string text = "device: ";
			InputDevice device = this.device;
			stringBuilder.AppendLine(text + ((device != null) ? device.ToString() : null));
			stringBuilder.AppendLine("pointerType: " + this.pointerType.ToString());
			stringBuilder.AppendLine("touchId: " + this.touchId.ToString());
			stringBuilder.AppendLine("pressPosition: " + base.pressPosition.ToString());
			stringBuilder.AppendLine("trackedDevicePosition: " + this.trackedDevicePosition.ToString());
			stringBuilder.AppendLine("trackedDeviceOrientation: " + this.trackedDeviceOrientation.ToString());
			stringBuilder.AppendLine("pressure" + base.pressure.ToString());
			stringBuilder.AppendLine("radius: " + base.radius.ToString());
			stringBuilder.AppendLine("azimuthAngle: " + base.azimuthAngle.ToString());
			stringBuilder.AppendLine("altitudeAngle: " + base.altitudeAngle.ToString());
			stringBuilder.AppendLine("twist: " + base.twist.ToString());
			stringBuilder.AppendLine("displayIndex: " + base.displayIndex.ToString());
			return stringBuilder.ToString();
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0003B4D5 File Offset: 0x000396D5
		internal static int MakePointerIdForTouch(int deviceId, int touchId)
		{
			return (deviceId << 24) + touchId;
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x0003B4DD File Offset: 0x000396DD
		internal static int TouchIdFromPointerId(int pointerId)
		{
			return pointerId & 255;
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x0003B4E8 File Offset: 0x000396E8
		internal unsafe void ReadDeviceState()
		{
			Pen pen = this.control.parent as Pen;
			if (pen != null)
			{
				this.uiToolkitPointerId = ExtendedPointerEventData.GetPenPointerId(pen);
				base.pressure = pen.pressure.magnitude;
				base.azimuthAngle = (pen.tilt.value.x + 1f) * 3.1415927f / 2f;
				base.altitudeAngle = (pen.tilt.value.y + 1f) * 3.1415927f / 2f;
				base.twist = *pen.twist.value * 3.1415927f * 2f;
				base.displayIndex = pen.displayIndex.ReadValue();
				return;
			}
			TouchControl touchControl = this.control.parent as TouchControl;
			if (touchControl != null)
			{
				this.uiToolkitPointerId = ExtendedPointerEventData.GetTouchPointerId(touchControl);
				base.pressure = touchControl.pressure.magnitude;
				base.radius = *touchControl.radius.value;
				base.displayIndex = touchControl.displayIndex.ReadValue();
				return;
			}
			Touchscreen touchscreen = this.control.parent as Touchscreen;
			if (touchscreen != null)
			{
				this.uiToolkitPointerId = ExtendedPointerEventData.GetTouchPointerId(touchscreen.primaryTouch);
				base.pressure = touchscreen.pressure.magnitude;
				base.radius = *touchscreen.radius.value;
				base.displayIndex = touchscreen.displayIndex.ReadValue();
				return;
			}
			this.uiToolkitPointerId = PointerId.mousePointerId;
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x0003B66C File Offset: 0x0003986C
		private static int GetPenPointerId(Pen pen)
		{
			int num = 0;
			foreach (InputDevice inputDevice in InputSystem.devices)
			{
				Pen pen2 = inputDevice as Pen;
				if (pen2 != null)
				{
					if (pen == pen2)
					{
						return PointerId.penPointerIdBase + Mathf.Min(num, PointerId.penPointerCount - 1);
					}
					num++;
				}
			}
			return PointerId.penPointerIdBase;
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x0003B6EC File Offset: 0x000398EC
		private static int GetTouchPointerId(TouchControl touchControl)
		{
			int num = ((Touchscreen)touchControl.device).touches.IndexOfReference(touchControl);
			return PointerId.touchPointerIdBase + Mathf.Clamp(num, 0, PointerId.touchPointerCount - 1);
		}
	}
}
