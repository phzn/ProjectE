using System;
using DA_Assets.Tools;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000035 RID: 53
	[Serializable]
	public class FNames
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00003A09 File Offset: 0x00001C09
		public SerializedDictionary<FcuNameType, string> Names
		{
			get
			{
				return this.names;
			}
		}

		// Token: 0x1700007A RID: 122
		public string this[FcuNameType key]
		{
			get
			{
				string text;
				if (!this.names.TryGetValue(key, out text))
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				this.names[key] = value;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00003A47 File Offset: 0x00001C47
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00003A51 File Offset: 0x00001C51
		public string UITK_SpritePath
		{
			get
			{
				return this[FcuNameType.UITK_SpritePath];
			}
			set
			{
				this[FcuNameType.UITK_SpritePath] = value;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00003A5C File Offset: 0x00001C5C
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00003A66 File Offset: 0x00001C66
		public string UITK_FontPath
		{
			get
			{
				return this[FcuNameType.UITK_FontPath];
			}
			set
			{
				this[FcuNameType.UITK_FontPath] = value;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00003A71 File Offset: 0x00001C71
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00003A7B File Offset: 0x00001C7B
		public string FigmaName
		{
			get
			{
				return this[FcuNameType.Figma];
			}
			set
			{
				this[FcuNameType.Figma] = value;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00003A86 File Offset: 0x00001C86
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00003A8F File Offset: 0x00001C8F
		public string HumanizedTextPrefabName
		{
			get
			{
				return this[FcuNameType.HumanizedTextPrefabName];
			}
			set
			{
				this[FcuNameType.HumanizedTextPrefabName] = value;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00003A99 File Offset: 0x00001C99
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00003AA2 File Offset: 0x00001CA2
		public string FileName
		{
			get
			{
				return this[FcuNameType.File];
			}
			set
			{
				this[FcuNameType.File] = value;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00003AAC File Offset: 0x00001CAC
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00003AB5 File Offset: 0x00001CB5
		public string ObjectName
		{
			get
			{
				return this[FcuNameType.Object];
			}
			set
			{
				this[FcuNameType.Object] = value;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00003ABF File Offset: 0x00001CBF
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00003AC8 File Offset: 0x00001CC8
		public string UitkGuid
		{
			get
			{
				return this[FcuNameType.UitkGuid];
			}
			set
			{
				this[FcuNameType.UitkGuid] = value;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00003AD2 File Offset: 0x00001CD2
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00003ADB File Offset: 0x00001CDB
		public string FieldName
		{
			get
			{
				return this[FcuNameType.Field];
			}
			set
			{
				this[FcuNameType.Field] = value;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00003AE5 File Offset: 0x00001CE5
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00003AEE File Offset: 0x00001CEE
		public string MethodName
		{
			get
			{
				return this[FcuNameType.Method];
			}
			set
			{
				this[FcuNameType.Method] = value;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00003AF8 File Offset: 0x00001CF8
		// (set) Token: 0x0600011C RID: 284 RVA: 0x00003B01 File Offset: 0x00001D01
		public string ClassName
		{
			get
			{
				return this[FcuNameType.Class];
			}
			set
			{
				this[FcuNameType.Class] = value;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00003B0B File Offset: 0x00001D0B
		// (set) Token: 0x0600011E RID: 286 RVA: 0x00003B14 File Offset: 0x00001D14
		public string UssClassName
		{
			get
			{
				return this[FcuNameType.UssClass];
			}
			set
			{
				this[FcuNameType.UssClass] = value;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00003B1E File Offset: 0x00001D1E
		// (set) Token: 0x06000120 RID: 288 RVA: 0x00003B28 File Offset: 0x00001D28
		public string UxmlPath
		{
			get
			{
				return this[FcuNameType.UxmlPath];
			}
			set
			{
				this[FcuNameType.UxmlPath] = value;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00003B33 File Offset: 0x00001D33
		// (set) Token: 0x06000122 RID: 290 RVA: 0x00003B3C File Offset: 0x00001D3C
		public string LocKey
		{
			get
			{
				return this[FcuNameType.LocKey];
			}
			set
			{
				this[FcuNameType.LocKey] = value;
			}
		}

		// Token: 0x0400032D RID: 813
		[SerializeField]
		private SerializedDictionary<FcuNameType, string> names = new SerializedDictionary<FcuNameType, string>();
	}
}
