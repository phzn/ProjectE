using System;
using UnityEngine.XR;

namespace UnityEngine.Rendering
{
	// Token: 0x0200005A RID: 90
	[Serializable]
	public class XRGraphics
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000CAEC File Offset: 0x0000ACEC
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x0000CB00 File Offset: 0x0000AD00
		public static float eyeTextureResolutionScale
		{
			get
			{
				if (XRGraphics.enabled)
				{
					return XRSettings.eyeTextureResolutionScale;
				}
				return 1f;
			}
			set
			{
				XRSettings.eyeTextureResolutionScale = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0000CB08 File Offset: 0x0000AD08
		public static float renderViewportScale
		{
			get
			{
				if (XRGraphics.enabled)
				{
					return XRSettings.renderViewportScale;
				}
				return 1f;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000CB1C File Offset: 0x0000AD1C
		public static bool enabled
		{
			get
			{
				return XRSettings.enabled;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0000CB23 File Offset: 0x0000AD23
		public static bool isDeviceActive
		{
			get
			{
				return XRGraphics.enabled && XRSettings.isDeviceActive;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060002DD RID: 733 RVA: 0x0000CB33 File Offset: 0x0000AD33
		public static string loadedDeviceName
		{
			get
			{
				if (XRGraphics.enabled)
				{
					return XRSettings.loadedDeviceName;
				}
				return "No XR device loaded";
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000CB47 File Offset: 0x0000AD47
		public static string[] supportedDevices
		{
			get
			{
				if (XRGraphics.enabled)
				{
					return XRSettings.supportedDevices;
				}
				return new string[1];
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000CB5C File Offset: 0x0000AD5C
		public static XRGraphics.StereoRenderingMode stereoRenderingMode
		{
			get
			{
				if (XRGraphics.enabled)
				{
					return (XRGraphics.StereoRenderingMode)XRSettings.stereoRenderingMode;
				}
				return XRGraphics.StereoRenderingMode.SinglePass;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000CB6C File Offset: 0x0000AD6C
		public static RenderTextureDescriptor eyeTextureDesc
		{
			get
			{
				if (XRGraphics.enabled)
				{
					return XRSettings.eyeTextureDesc;
				}
				return new RenderTextureDescriptor(0, 0);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000CB82 File Offset: 0x0000AD82
		public static int eyeTextureWidth
		{
			get
			{
				if (XRGraphics.enabled)
				{
					return XRSettings.eyeTextureWidth;
				}
				return 0;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000CB92 File Offset: 0x0000AD92
		public static int eyeTextureHeight
		{
			get
			{
				if (XRGraphics.enabled)
				{
					return XRSettings.eyeTextureHeight;
				}
				return 0;
			}
		}

		// Token: 0x02000161 RID: 353
		public enum StereoRenderingMode
		{
			// Token: 0x040005EE RID: 1518
			MultiPass,
			// Token: 0x040005EF RID: 1519
			SinglePass,
			// Token: 0x040005F0 RID: 1520
			SinglePassInstanced,
			// Token: 0x040005F1 RID: 1521
			SinglePassMultiView
		}
	}
}
