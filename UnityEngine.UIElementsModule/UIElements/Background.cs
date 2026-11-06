using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020002C5 RID: 709
	public struct Background : IEquatable<Background>
	{
		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06001487 RID: 5255 RVA: 0x00049348 File Offset: 0x00047548
		// (set) Token: 0x06001488 RID: 5256 RVA: 0x00049360 File Offset: 0x00047560
		public Texture2D texture
		{
			get
			{
				return this.m_Texture;
			}
			set
			{
				bool flag = this.m_Texture == value;
				if (!flag)
				{
					this.m_Texture = value;
					this.m_Sprite = null;
					this.m_RenderTexture = null;
					this.m_VectorImage = null;
				}
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001489 RID: 5257 RVA: 0x0004939C File Offset: 0x0004759C
		// (set) Token: 0x0600148A RID: 5258 RVA: 0x000493B4 File Offset: 0x000475B4
		public Sprite sprite
		{
			get
			{
				return this.m_Sprite;
			}
			set
			{
				bool flag = this.m_Sprite == value;
				if (!flag)
				{
					this.m_Texture = null;
					this.m_Sprite = value;
					this.m_RenderTexture = null;
					this.m_VectorImage = null;
				}
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x0600148B RID: 5259 RVA: 0x000493F0 File Offset: 0x000475F0
		// (set) Token: 0x0600148C RID: 5260 RVA: 0x00049408 File Offset: 0x00047608
		public RenderTexture renderTexture
		{
			get
			{
				return this.m_RenderTexture;
			}
			set
			{
				bool flag = this.m_RenderTexture == value;
				if (!flag)
				{
					this.m_Texture = null;
					this.m_Sprite = null;
					this.m_RenderTexture = value;
					this.m_VectorImage = null;
				}
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x0600148D RID: 5261 RVA: 0x00049444 File Offset: 0x00047644
		// (set) Token: 0x0600148E RID: 5262 RVA: 0x0004945C File Offset: 0x0004765C
		public VectorImage vectorImage
		{
			get
			{
				return this.m_VectorImage;
			}
			set
			{
				bool flag = this.vectorImage == value;
				if (!flag)
				{
					this.m_Texture = null;
					this.m_Sprite = null;
					this.m_RenderTexture = null;
					this.m_VectorImage = value;
				}
			}
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x00049498 File Offset: 0x00047698
		[Obsolete("Use Background.FromTexture2D instead")]
		public Background(Texture2D t)
		{
			this.m_Texture = t;
			this.m_Sprite = null;
			this.m_RenderTexture = null;
			this.m_VectorImage = null;
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x000494B8 File Offset: 0x000476B8
		public static Background FromTexture2D(Texture2D t)
		{
			return new Background
			{
				texture = t
			};
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x000494DC File Offset: 0x000476DC
		public static Background FromRenderTexture(RenderTexture rt)
		{
			return new Background
			{
				renderTexture = rt
			};
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x00049500 File Offset: 0x00047700
		public static Background FromSprite(Sprite s)
		{
			return new Background
			{
				sprite = s
			};
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x00049524 File Offset: 0x00047724
		public static Background FromVectorImage(VectorImage vi)
		{
			return new Background
			{
				vectorImage = vi
			};
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x00049548 File Offset: 0x00047748
		internal static Background FromObject(object obj)
		{
			Texture2D texture2D = obj as Texture2D;
			bool flag = texture2D != null;
			Background background;
			if (flag)
			{
				background = Background.FromTexture2D(texture2D);
			}
			else
			{
				RenderTexture renderTexture = obj as RenderTexture;
				bool flag2 = renderTexture != null;
				if (flag2)
				{
					background = Background.FromRenderTexture(renderTexture);
				}
				else
				{
					Sprite sprite = obj as Sprite;
					bool flag3 = sprite != null;
					if (flag3)
					{
						background = Background.FromSprite(sprite);
					}
					else
					{
						VectorImage vectorImage = obj as VectorImage;
						bool flag4 = vectorImage != null;
						if (flag4)
						{
							background = Background.FromVectorImage(vectorImage);
						}
						else
						{
							background = default(Background);
						}
					}
				}
			}
			return background;
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001495 RID: 5269 RVA: 0x000495E0 File Offset: 0x000477E0
		internal static IEnumerable<Type> allowedAssetTypes
		{
			get
			{
				yield return typeof(Texture2D);
				yield return typeof(RenderTexture);
				yield return typeof(Sprite);
				yield return typeof(VectorImage);
				yield break;
			}
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x000495F8 File Offset: 0x000477F8
		public static bool operator ==(Background lhs, Background rhs)
		{
			return lhs.texture == rhs.texture && lhs.sprite == rhs.sprite && lhs.renderTexture == rhs.renderTexture && lhs.vectorImage == rhs.vectorImage;
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x00049660 File Offset: 0x00047860
		public static bool operator !=(Background lhs, Background rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x0004967C File Offset: 0x0004787C
		public static implicit operator Background(Texture2D v)
		{
			return Background.FromTexture2D(v);
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x00049694 File Offset: 0x00047894
		public bool Equals(Background other)
		{
			return other == this;
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x000496B4 File Offset: 0x000478B4
		public override bool Equals(object obj)
		{
			bool flag = !(obj is Background);
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				Background background = (Background)obj;
				flag2 = background == this;
			}
			return flag2;
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x000496F0 File Offset: 0x000478F0
		public override int GetHashCode()
		{
			int num = 851985039;
			bool flag = this.texture != null;
			if (flag)
			{
				num = num * -1521134295 + this.texture.GetHashCode();
			}
			bool flag2 = this.sprite != null;
			if (flag2)
			{
				num = num * -1521134295 + this.sprite.GetHashCode();
			}
			bool flag3 = this.renderTexture != null;
			if (flag3)
			{
				num = num * -1521134295 + this.renderTexture.GetHashCode();
			}
			bool flag4 = this.vectorImage != null;
			if (flag4)
			{
				num = num * -1521134295 + this.vectorImage.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x00049794 File Offset: 0x00047994
		public override string ToString()
		{
			bool flag = this.texture != null;
			string text;
			if (flag)
			{
				text = this.texture.ToString();
			}
			else
			{
				bool flag2 = this.sprite != null;
				if (flag2)
				{
					text = this.sprite.ToString();
				}
				else
				{
					bool flag3 = this.renderTexture != null;
					if (flag3)
					{
						text = this.renderTexture.ToString();
					}
					else
					{
						bool flag4 = this.vectorImage != null;
						if (flag4)
						{
							text = this.vectorImage.ToString();
						}
						else
						{
							text = "";
						}
					}
				}
			}
			return text;
		}

		// Token: 0x0400098D RID: 2445
		private Texture2D m_Texture;

		// Token: 0x0400098E RID: 2446
		private Sprite m_Sprite;

		// Token: 0x0400098F RID: 2447
		private RenderTexture m_RenderTexture;

		// Token: 0x04000990 RID: 2448
		private VectorImage m_VectorImage;
	}
}
