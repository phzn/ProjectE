using System;

namespace UnityEngine
{
	// Token: 0x02000032 RID: 50
	internal class GUILayoutEntry
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x0000CE5C File Offset: 0x0000B05C
		// (set) Token: 0x060003E8 RID: 1000 RVA: 0x0000CE74 File Offset: 0x0000B074
		public GUIStyle style
		{
			get
			{
				return this.m_Style;
			}
			set
			{
				this.m_Style = value;
				this.ApplyStyleSettings(value);
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x0000CE86 File Offset: 0x0000B086
		public virtual int marginLeft
		{
			get
			{
				return this.style.margin.left;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x0000CE98 File Offset: 0x0000B098
		public virtual int marginRight
		{
			get
			{
				return this.style.margin.right;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x0000CEAA File Offset: 0x0000B0AA
		public virtual int marginTop
		{
			get
			{
				return this.style.margin.top;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x0000CEBC File Offset: 0x0000B0BC
		public virtual int marginBottom
		{
			get
			{
				return this.style.margin.bottom;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x0000CECE File Offset: 0x0000B0CE
		public int marginHorizontal
		{
			get
			{
				return this.marginLeft + this.marginRight;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x0000CEDD File Offset: 0x0000B0DD
		public int marginVertical
		{
			get
			{
				return this.marginBottom + this.marginTop;
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000CEEC File Offset: 0x0000B0EC
		public GUILayoutEntry(float _minWidth, float _maxWidth, float _minHeight, float _maxHeight, GUIStyle _style)
		{
			this.minWidth = _minWidth;
			this.maxWidth = _maxWidth;
			this.minHeight = _minHeight;
			this.maxHeight = _maxHeight;
			bool flag = _style == null;
			if (flag)
			{
				_style = GUIStyle.none;
			}
			this.style = _style;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000CF68 File Offset: 0x0000B168
		public GUILayoutEntry(float _minWidth, float _maxWidth, float _minHeight, float _maxHeight, GUIStyle _style, GUILayoutOption[] options)
		{
			this.minWidth = _minWidth;
			this.maxWidth = _maxWidth;
			this.minHeight = _minHeight;
			this.maxHeight = _maxHeight;
			this.style = _style;
			this.ApplyOptions(options);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00002221 File Offset: 0x00000421
		public virtual void CalcWidth()
		{
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00002221 File Offset: 0x00000421
		public virtual void CalcHeight()
		{
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000CFDD File Offset: 0x0000B1DD
		public virtual void SetHorizontal(float x, float width)
		{
			this.rect.x = x;
			this.rect.width = width;
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000CFFA File Offset: 0x0000B1FA
		public virtual void SetVertical(float y, float height)
		{
			this.rect.y = y;
			this.rect.height = height;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000D018 File Offset: 0x0000B218
		protected virtual void ApplyStyleSettings(GUIStyle style)
		{
			this.stretchWidth = ((style.fixedWidth == 0f && style.stretchWidth) ? 1 : 0);
			this.stretchHeight = ((style.fixedHeight == 0f && style.stretchHeight) ? 1 : 0);
			this.m_Style = style;
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000D06C File Offset: 0x0000B26C
		public virtual void ApplyOptions(GUILayoutOption[] options)
		{
			bool flag = options == null;
			if (!flag)
			{
				foreach (GUILayoutOption guilayoutOption in options)
				{
					switch (guilayoutOption.type)
					{
					case GUILayoutOption.Type.fixedWidth:
						this.minWidth = (this.maxWidth = (float)guilayoutOption.value);
						this.stretchWidth = 0;
						break;
					case GUILayoutOption.Type.fixedHeight:
						this.minHeight = (this.maxHeight = (float)guilayoutOption.value);
						this.stretchHeight = 0;
						break;
					case GUILayoutOption.Type.minWidth:
					{
						this.minWidth = (float)guilayoutOption.value;
						bool flag2 = this.maxWidth < this.minWidth;
						if (flag2)
						{
							this.maxWidth = this.minWidth;
						}
						break;
					}
					case GUILayoutOption.Type.maxWidth:
					{
						this.maxWidth = (float)guilayoutOption.value;
						bool flag3 = this.minWidth > this.maxWidth;
						if (flag3)
						{
							this.minWidth = this.maxWidth;
						}
						this.stretchWidth = 0;
						break;
					}
					case GUILayoutOption.Type.minHeight:
					{
						this.minHeight = (float)guilayoutOption.value;
						bool flag4 = this.maxHeight < this.minHeight;
						if (flag4)
						{
							this.maxHeight = this.minHeight;
						}
						break;
					}
					case GUILayoutOption.Type.maxHeight:
					{
						this.maxHeight = (float)guilayoutOption.value;
						bool flag5 = this.minHeight > this.maxHeight;
						if (flag5)
						{
							this.minHeight = this.maxHeight;
						}
						this.stretchHeight = 0;
						break;
					}
					case GUILayoutOption.Type.stretchWidth:
						this.stretchWidth = (int)guilayoutOption.value;
						break;
					case GUILayoutOption.Type.stretchHeight:
						this.stretchHeight = (int)guilayoutOption.value;
						break;
					}
				}
				bool flag6 = this.maxWidth != 0f && this.maxWidth < this.minWidth;
				if (flag6)
				{
					this.maxWidth = this.minWidth;
				}
				bool flag7 = this.maxHeight != 0f && this.maxHeight < this.minHeight;
				if (flag7)
				{
					this.maxHeight = this.minHeight;
				}
			}
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000D294 File Offset: 0x0000B494
		public override string ToString()
		{
			string text = "";
			for (int i = 0; i < GUILayoutEntry.indent; i++)
			{
				text += " ";
			}
			return string.Concat(new string[]
			{
				text,
				UnityString.Format("{1}-{0} (x:{2}-{3}, y:{4}-{5})", new object[]
				{
					(this.style != null) ? this.style.name : "NULL",
					base.GetType(),
					this.rect.x,
					this.rect.xMax,
					this.rect.y,
					this.rect.yMax
				}),
				"   -   W: ",
				this.minWidth.ToString(),
				"-",
				this.maxWidth.ToString(),
				(this.stretchWidth != 0) ? "+" : "",
				", H: ",
				this.minHeight.ToString(),
				"-",
				this.maxHeight.ToString(),
				(this.stretchHeight != 0) ? "+" : ""
			});
		}

		// Token: 0x040000F2 RID: 242
		public float minWidth;

		// Token: 0x040000F3 RID: 243
		public float maxWidth;

		// Token: 0x040000F4 RID: 244
		public float minHeight;

		// Token: 0x040000F5 RID: 245
		public float maxHeight;

		// Token: 0x040000F6 RID: 246
		public Rect rect = new Rect(0f, 0f, 0f, 0f);

		// Token: 0x040000F7 RID: 247
		public int stretchWidth;

		// Token: 0x040000F8 RID: 248
		public int stretchHeight;

		// Token: 0x040000F9 RID: 249
		public bool consideredForMargin = true;

		// Token: 0x040000FA RID: 250
		private GUIStyle m_Style = GUIStyle.none;

		// Token: 0x040000FB RID: 251
		internal static Rect kDummyRect = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x040000FC RID: 252
		protected static int indent = 0;
	}
}
