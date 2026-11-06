using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000022 RID: 34
	[UsedByNativeCode]
	public struct WebCamDevice
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00002DC4 File Offset: 0x00000FC4
		public string name
		{
			get
			{
				return this.m_Name;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00002DDC File Offset: 0x00000FDC
		public bool isFrontFacing
		{
			get
			{
				return (this.m_Flags & 1) != 0;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00002DFC File Offset: 0x00000FFC
		public WebCamKind kind
		{
			get
			{
				return this.m_Kind;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00002E14 File Offset: 0x00001014
		public string depthCameraName
		{
			get
			{
				return (this.m_DepthCameraName == "") ? null : this.m_DepthCameraName;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00002E44 File Offset: 0x00001044
		public bool isAutoFocusPointSupported
		{
			get
			{
				return (this.m_Flags & 2) != 0;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00002E64 File Offset: 0x00001064
		public Resolution[] availableResolutions
		{
			get
			{
				return this.m_Resolutions;
			}
		}

		// Token: 0x04000066 RID: 102
		[NativeName("name")]
		internal string m_Name;

		// Token: 0x04000067 RID: 103
		[NativeName("depthCameraName")]
		internal string m_DepthCameraName;

		// Token: 0x04000068 RID: 104
		[NativeName("flags")]
		internal int m_Flags;

		// Token: 0x04000069 RID: 105
		[NativeName("kind")]
		internal WebCamKind m_Kind;

		// Token: 0x0400006A RID: 106
		[NativeName("resolutions")]
		internal Resolution[] m_Resolutions;
	}
}
