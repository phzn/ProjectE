using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200030C RID: 780
	public struct StyleTransformOrigin : IStyleValue<TransformOrigin>, IEquatable<StyleTransformOrigin>
	{
		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06001AFC RID: 6908 RVA: 0x0006A5E4 File Offset: 0x000687E4
		// (set) Token: 0x06001AFD RID: 6909 RVA: 0x0006A649 File Offset: 0x00068849
		public TransformOrigin value
		{
			get
			{
				StyleKeyword keyword = this.m_Keyword;
				if (!true)
				{
				}
				TransformOrigin transformOrigin;
				switch (keyword)
				{
				case StyleKeyword.Undefined:
					transformOrigin = this.m_Value;
					goto IL_004F;
				case StyleKeyword.Null:
					transformOrigin = TransformOrigin.Initial();
					goto IL_004F;
				case StyleKeyword.None:
					transformOrigin = TransformOrigin.Initial();
					goto IL_004F;
				case StyleKeyword.Initial:
					transformOrigin = TransformOrigin.Initial();
					goto IL_004F;
				}
				throw new NotImplementedException();
				IL_004F:
				if (!true)
				{
				}
				return transformOrigin;
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06001AFE RID: 6910 RVA: 0x0006A65C File Offset: 0x0006885C
		// (set) Token: 0x06001AFF RID: 6911 RVA: 0x0006A674 File Offset: 0x00068874
		public StyleKeyword keyword
		{
			get
			{
				return this.m_Keyword;
			}
			set
			{
				this.m_Keyword = value;
			}
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x0006A67E File Offset: 0x0006887E
		public StyleTransformOrigin(TransformOrigin v)
		{
			this = new StyleTransformOrigin(v, StyleKeyword.Undefined);
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x0006A68C File Offset: 0x0006888C
		public StyleTransformOrigin(StyleKeyword keyword)
		{
			this = new StyleTransformOrigin(default(TransformOrigin), keyword);
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x0006A6AB File Offset: 0x000688AB
		internal StyleTransformOrigin(TransformOrigin v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x0006A6BC File Offset: 0x000688BC
		public static bool operator ==(StyleTransformOrigin lhs, StyleTransformOrigin rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x0006A6F0 File Offset: 0x000688F0
		public static bool operator !=(StyleTransformOrigin lhs, StyleTransformOrigin rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x0006A70C File Offset: 0x0006890C
		public static implicit operator StyleTransformOrigin(StyleKeyword keyword)
		{
			return new StyleTransformOrigin(keyword);
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x0006A724 File Offset: 0x00068924
		public static implicit operator StyleTransformOrigin(TransformOrigin v)
		{
			return new StyleTransformOrigin(v);
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x0006A73C File Offset: 0x0006893C
		public bool Equals(StyleTransformOrigin other)
		{
			return other == this;
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x0006A75C File Offset: 0x0006895C
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleTransformOrigin)
			{
				StyleTransformOrigin styleTransformOrigin = (StyleTransformOrigin)obj;
				flag = this.Equals(styleTransformOrigin);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x0006A788 File Offset: 0x00068988
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x0006A7BC File Offset: 0x000689BC
		public override string ToString()
		{
			return this.DebugString<TransformOrigin>();
		}

		// Token: 0x04000AFE RID: 2814
		private TransformOrigin m_Value;

		// Token: 0x04000AFF RID: 2815
		private StyleKeyword m_Keyword;
	}
}
