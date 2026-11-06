using System;
using DA_Assets.DAI;
using TMPro;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000389 RID: 905
	[Serializable]
	public class TextMeshSettings : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x060017A2 RID: 6050 RVA: 0x000975F1 File Offset: 0x000957F1
		// (set) Token: 0x060017A3 RID: 6051 RVA: 0x000975F9 File Offset: 0x000957F9
		public bool AutoSize
		{
			get
			{
				return this.autoSize;
			}
			set
			{
				base.SetValue<bool>(ref this.autoSize, value);
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x060017A4 RID: 6052 RVA: 0x00097608 File Offset: 0x00095808
		// (set) Token: 0x060017A5 RID: 6053 RVA: 0x00097610 File Offset: 0x00095810
		public bool OverrideTags
		{
			get
			{
				return this.overrideTags;
			}
			set
			{
				base.SetValue<bool>(ref this.overrideTags, value);
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060017A6 RID: 6054 RVA: 0x0009761F File Offset: 0x0009581F
		// (set) Token: 0x060017A7 RID: 6055 RVA: 0x00097627 File Offset: 0x00095827
		public bool Wrapping
		{
			get
			{
				return this.wrapping;
			}
			set
			{
				base.SetValue<bool>(ref this.wrapping, value);
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060017A8 RID: 6056 RVA: 0x00097636 File Offset: 0x00095836
		// (set) Token: 0x060017A9 RID: 6057 RVA: 0x0009763E File Offset: 0x0009583E
		public bool OrthographicMode
		{
			get
			{
				return this.orthographicMode;
			}
			set
			{
				base.SetValue<bool>(ref this.orthographicMode, value);
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x060017AA RID: 6058 RVA: 0x0009764D File Offset: 0x0009584D
		// (set) Token: 0x060017AB RID: 6059 RVA: 0x00097655 File Offset: 0x00095855
		public bool RichText
		{
			get
			{
				return this.richText;
			}
			set
			{
				base.SetValue<bool>(ref this.richText, value);
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060017AC RID: 6060 RVA: 0x00097664 File Offset: 0x00095864
		// (set) Token: 0x060017AD RID: 6061 RVA: 0x0009766C File Offset: 0x0009586C
		public bool RaycastTarget
		{
			get
			{
				return this.raycastTarget;
			}
			set
			{
				base.SetValue<bool>(ref this.raycastTarget, value);
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060017AE RID: 6062 RVA: 0x0009767B File Offset: 0x0009587B
		// (set) Token: 0x060017AF RID: 6063 RVA: 0x00097683 File Offset: 0x00095883
		public bool ParseEscapeCharacters
		{
			get
			{
				return this.parseEscapeCharacters;
			}
			set
			{
				base.SetValue<bool>(ref this.parseEscapeCharacters, value);
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060017B0 RID: 6064 RVA: 0x00097692 File Offset: 0x00095892
		// (set) Token: 0x060017B1 RID: 6065 RVA: 0x0009769A File Offset: 0x0009589A
		public bool VisibleDescender
		{
			get
			{
				return this.visibleDescender;
			}
			set
			{
				base.SetValue<bool>(ref this.visibleDescender, value);
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060017B2 RID: 6066 RVA: 0x000976A9 File Offset: 0x000958A9
		// (set) Token: 0x060017B3 RID: 6067 RVA: 0x000976B1 File Offset: 0x000958B1
		public bool Kerning
		{
			get
			{
				return this.kerning;
			}
			set
			{
				base.SetValue<bool>(ref this.kerning, value);
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x060017B4 RID: 6068 RVA: 0x000976C0 File Offset: 0x000958C0
		// (set) Token: 0x060017B5 RID: 6069 RVA: 0x000976C8 File Offset: 0x000958C8
		public bool ExtraPadding
		{
			get
			{
				return this.extraPadding;
			}
			set
			{
				base.SetValue<bool>(ref this.extraPadding, value);
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x060017B6 RID: 6070 RVA: 0x000976D7 File Offset: 0x000958D7
		// (set) Token: 0x060017B7 RID: 6071 RVA: 0x000976DF File Offset: 0x000958DF
		public TextOverflowModes Overflow
		{
			get
			{
				return this.overflow;
			}
			set
			{
				base.SetValue<TextOverflowModes>(ref this.overflow, value);
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x060017B8 RID: 6072 RVA: 0x000976EE File Offset: 0x000958EE
		// (set) Token: 0x060017B9 RID: 6073 RVA: 0x000976F6 File Offset: 0x000958F6
		public TextureMappingOptions HorizontalMapping
		{
			get
			{
				return this.horizontalMapping;
			}
			set
			{
				base.SetValue<TextureMappingOptions>(ref this.horizontalMapping, value);
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x060017BA RID: 6074 RVA: 0x00097705 File Offset: 0x00095905
		// (set) Token: 0x060017BB RID: 6075 RVA: 0x0009770D File Offset: 0x0009590D
		public TextureMappingOptions VerticalMapping
		{
			get
			{
				return this.verticalMapping;
			}
			set
			{
				base.SetValue<TextureMappingOptions>(ref this.verticalMapping, value);
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x060017BC RID: 6076 RVA: 0x0009771C File Offset: 0x0009591C
		// (set) Token: 0x060017BD RID: 6077 RVA: 0x00097724 File Offset: 0x00095924
		public VertexSortingOrder GeometrySorting
		{
			get
			{
				return this.geometrySorting;
			}
			set
			{
				base.SetValue<VertexSortingOrder>(ref this.geometrySorting, value);
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x060017BE RID: 6078 RVA: 0x00097733 File Offset: 0x00095933
		// (set) Token: 0x060017BF RID: 6079 RVA: 0x0009773B File Offset: 0x0009593B
		public Shader Shader
		{
			get
			{
				return this.shader;
			}
			set
			{
				base.SetValue<Shader>(ref this.shader, value);
			}
		}

		// Token: 0x0400191B RID: 6427
		[SerializeField]
		private bool autoSize = true;

		// Token: 0x0400191C RID: 6428
		[SerializeField]
		private bool overrideTags;

		// Token: 0x0400191D RID: 6429
		[SerializeField]
		private bool wrapping = true;

		// Token: 0x0400191E RID: 6430
		[SerializeField]
		private bool orthographicMode = true;

		// Token: 0x0400191F RID: 6431
		[SerializeField]
		private bool richText = true;

		// Token: 0x04001920 RID: 6432
		[SerializeField]
		private bool raycastTarget = true;

		// Token: 0x04001921 RID: 6433
		[SerializeField]
		private bool parseEscapeCharacters = true;

		// Token: 0x04001922 RID: 6434
		[SerializeField]
		private bool visibleDescender = true;

		// Token: 0x04001923 RID: 6435
		[SerializeField]
		private bool kerning = true;

		// Token: 0x04001924 RID: 6436
		[SerializeField]
		private bool extraPadding;

		// Token: 0x04001925 RID: 6437
		[SerializeField]
		private TextOverflowModes overflow;

		// Token: 0x04001926 RID: 6438
		[SerializeField]
		private TextureMappingOptions horizontalMapping;

		// Token: 0x04001927 RID: 6439
		[SerializeField]
		private TextureMappingOptions verticalMapping;

		// Token: 0x04001928 RID: 6440
		[SerializeField]
		private VertexSortingOrder geometrySorting;

		// Token: 0x04001929 RID: 6441
		[SerializeField]
		private Shader shader;
	}
}
