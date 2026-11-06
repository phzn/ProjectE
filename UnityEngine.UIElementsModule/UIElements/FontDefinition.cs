using System;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;

namespace UnityEngine.UIElements
{
	// Token: 0x020002CF RID: 719
	public struct FontDefinition : IEquatable<FontDefinition>
	{
		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x0600156F RID: 5487 RVA: 0x000550D8 File Offset: 0x000532D8
		// (set) Token: 0x06001570 RID: 5488 RVA: 0x000550F0 File Offset: 0x000532F0
		public Font font
		{
			get
			{
				return this.m_Font;
			}
			set
			{
				bool flag = value != null && this.fontAsset != null;
				if (flag)
				{
					throw new InvalidOperationException("Cannot set both Font and FontAsset on FontDefinition");
				}
				this.m_Font = value;
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06001571 RID: 5489 RVA: 0x0005512C File Offset: 0x0005332C
		// (set) Token: 0x06001572 RID: 5490 RVA: 0x00055144 File Offset: 0x00053344
		public FontAsset fontAsset
		{
			get
			{
				return this.m_FontAsset;
			}
			set
			{
				bool flag = value != null && this.font != null;
				if (flag)
				{
					throw new InvalidOperationException("Cannot set both Font and FontAsset on FontDefinition");
				}
				this.m_FontAsset = value;
			}
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x00055180 File Offset: 0x00053380
		public static FontDefinition FromFont(Font f)
		{
			return new FontDefinition
			{
				m_Font = f
			};
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x000551A4 File Offset: 0x000533A4
		public static FontDefinition FromSDFFont(FontAsset f)
		{
			return new FontDefinition
			{
				m_FontAsset = f
			};
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x000551C8 File Offset: 0x000533C8
		internal static FontDefinition FromObject(object obj)
		{
			Font font = obj as Font;
			bool flag = font != null;
			FontDefinition fontDefinition;
			if (flag)
			{
				fontDefinition = FontDefinition.FromFont(font);
			}
			else
			{
				FontAsset fontAsset = obj as FontAsset;
				bool flag2 = fontAsset != null;
				if (flag2)
				{
					fontDefinition = FontDefinition.FromSDFFont(fontAsset);
				}
				else
				{
					fontDefinition = default(FontDefinition);
				}
			}
			return fontDefinition;
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06001576 RID: 5494 RVA: 0x0005521C File Offset: 0x0005341C
		internal static IEnumerable<Type> allowedAssetTypes
		{
			get
			{
				yield return typeof(Font);
				yield return typeof(FontAsset);
				yield break;
			}
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x00055234 File Offset: 0x00053434
		internal bool IsEmpty()
		{
			return this.m_Font == null && this.m_FontAsset == null;
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x00055264 File Offset: 0x00053464
		public override string ToString()
		{
			bool flag = this.font != null;
			string text;
			if (flag)
			{
				text = string.Format("{0}", this.font);
			}
			else
			{
				text = string.Format("{0}", this.fontAsset);
			}
			return text;
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x000552AC File Offset: 0x000534AC
		public bool Equals(FontDefinition other)
		{
			return object.Equals(this.m_Font, other.m_Font) && object.Equals(this.m_FontAsset, other.m_FontAsset);
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x000552E8 File Offset: 0x000534E8
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is FontDefinition)
			{
				FontDefinition fontDefinition = (FontDefinition)obj;
				flag = this.Equals(fontDefinition);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x00055314 File Offset: 0x00053514
		public override int GetHashCode()
		{
			return (((this.m_Font != null) ? this.m_Font.GetHashCode() : 0) * 397) ^ ((this.m_FontAsset != null) ? this.m_FontAsset.GetHashCode() : 0);
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x00055368 File Offset: 0x00053568
		public static bool operator ==(FontDefinition left, FontDefinition right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x00055384 File Offset: 0x00053584
		public static bool operator !=(FontDefinition left, FontDefinition right)
		{
			return !left.Equals(right);
		}

		// Token: 0x040009D6 RID: 2518
		private Font m_Font;

		// Token: 0x040009D7 RID: 2519
		private FontAsset m_FontAsset;
	}
}
