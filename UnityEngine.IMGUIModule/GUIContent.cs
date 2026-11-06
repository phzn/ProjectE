using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000014 RID: 20
	[RequiredByNativeCode(Optional = true, GenerateProxy = true)]
	[NativeHeader("Modules/IMGUI/GUIContent.h")]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class GUIContent
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600018C RID: 396 RVA: 0x00007C88 File Offset: 0x00005E88
		// (remove) Token: 0x0600018D RID: 397 RVA: 0x00007CC0 File Offset: 0x00005EC0
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action OnTextChanged;

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00007CF8 File Offset: 0x00005EF8
		// (set) Token: 0x0600018F RID: 399 RVA: 0x00007D10 File Offset: 0x00005F10
		public string text
		{
			get
			{
				return this.m_Text;
			}
			set
			{
				bool flag = value == this.m_Text;
				if (!flag)
				{
					this.m_Text = value;
					Action onTextChanged = this.OnTextChanged;
					if (onTextChanged != null)
					{
						onTextChanged();
					}
				}
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00007D4C File Offset: 0x00005F4C
		// (set) Token: 0x06000191 RID: 401 RVA: 0x00007D64 File Offset: 0x00005F64
		public Texture image
		{
			get
			{
				return this.m_Image;
			}
			set
			{
				this.m_Image = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00007D70 File Offset: 0x00005F70
		// (set) Token: 0x06000193 RID: 403 RVA: 0x00007D88 File Offset: 0x00005F88
		public string tooltip
		{
			get
			{
				return this.m_Tooltip;
			}
			set
			{
				this.m_Tooltip = value;
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00007D92 File Offset: 0x00005F92
		public GUIContent()
		{
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00007DB2 File Offset: 0x00005FB2
		public GUIContent(string text)
			: this(text, null, string.Empty)
		{
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00007DC3 File Offset: 0x00005FC3
		public GUIContent(Texture image)
			: this(string.Empty, image, string.Empty)
		{
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00007DD8 File Offset: 0x00005FD8
		public GUIContent(string text, Texture image)
			: this(text, image, string.Empty)
		{
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00007DE9 File Offset: 0x00005FE9
		public GUIContent(string text, string tooltip)
			: this(text, null, tooltip)
		{
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00007DF6 File Offset: 0x00005FF6
		public GUIContent(Texture image, string tooltip)
			: this(string.Empty, image, tooltip)
		{
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00007E07 File Offset: 0x00006007
		public GUIContent(string text, Texture image, string tooltip)
		{
			this.text = text;
			this.image = image;
			this.tooltip = tooltip;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00007E40 File Offset: 0x00006040
		public GUIContent(GUIContent src)
		{
			this.text = src.m_Text;
			this.image = src.m_Image;
			this.tooltip = src.m_Tooltip;
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00007E94 File Offset: 0x00006094
		internal int hash
		{
			get
			{
				int num = 0;
				bool flag = !string.IsNullOrEmpty(this.m_Text);
				if (flag)
				{
					num = this.m_Text.GetHashCode() * 37;
				}
				return num;
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00007ECC File Offset: 0x000060CC
		internal static GUIContent Temp(string t)
		{
			GUIContent.s_Text.m_Text = t;
			GUIContent.s_Text.m_Tooltip = string.Empty;
			return GUIContent.s_Text;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00007F00 File Offset: 0x00006100
		internal static GUIContent Temp(string t, string tooltip)
		{
			GUIContent.s_Text.m_Text = t;
			GUIContent.s_Text.m_Tooltip = tooltip;
			return GUIContent.s_Text;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00007F30 File Offset: 0x00006130
		internal static GUIContent Temp(Texture i)
		{
			GUIContent.s_Image.m_Image = i;
			GUIContent.s_Image.m_Tooltip = string.Empty;
			return GUIContent.s_Image;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00007F64 File Offset: 0x00006164
		internal static GUIContent Temp(Texture i, string tooltip)
		{
			GUIContent.s_Image.m_Image = i;
			GUIContent.s_Image.m_Tooltip = tooltip;
			return GUIContent.s_Image;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00007F94 File Offset: 0x00006194
		internal static GUIContent Temp(string t, Texture i)
		{
			GUIContent.s_TextImage.m_Text = t;
			GUIContent.s_TextImage.m_Image = i;
			return GUIContent.s_TextImage;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00007FC4 File Offset: 0x000061C4
		internal static void ClearStaticCache()
		{
			GUIContent.s_Text.m_Text = null;
			GUIContent.s_Text.m_Tooltip = string.Empty;
			GUIContent.s_Image.m_Image = null;
			GUIContent.s_Image.m_Tooltip = string.Empty;
			GUIContent.s_TextImage.m_Text = null;
			GUIContent.s_TextImage.m_Image = null;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000801C File Offset: 0x0000621C
		internal static GUIContent[] Temp(string[] texts)
		{
			GUIContent[] array = new GUIContent[texts.Length];
			for (int i = 0; i < texts.Length; i++)
			{
				array[i] = new GUIContent(texts[i]);
			}
			return array;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00008058 File Offset: 0x00006258
		internal static GUIContent[] Temp(Texture[] images)
		{
			GUIContent[] array = new GUIContent[images.Length];
			for (int i = 0; i < images.Length; i++)
			{
				array[i] = new GUIContent(images[i]);
			}
			return array;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00008094 File Offset: 0x00006294
		public override string ToString()
		{
			string text;
			if ((text = this.text) == null)
			{
				text = this.tooltip ?? base.ToString();
			}
			return text;
		}

		// Token: 0x0400006D RID: 109
		[SerializeField]
		private string m_Text = string.Empty;

		// Token: 0x0400006E RID: 110
		[SerializeField]
		private Texture m_Image;

		// Token: 0x0400006F RID: 111
		[SerializeField]
		private string m_Tooltip = string.Empty;

		// Token: 0x04000071 RID: 113
		private static readonly GUIContent s_Text = new GUIContent();

		// Token: 0x04000072 RID: 114
		private static readonly GUIContent s_Image = new GUIContent();

		// Token: 0x04000073 RID: 115
		private static readonly GUIContent s_TextImage = new GUIContent();

		// Token: 0x04000074 RID: 116
		public static GUIContent none = new GUIContent("");
	}
}
