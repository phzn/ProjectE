using System;
using System.Reflection;

namespace Microsoft.VisualBasic
{
	// Token: 0x02000139 RID: 313
	internal sealed class VBTypeAttributeConverter : VBModifierAttributeConverter
	{
		// Token: 0x060007C2 RID: 1986 RVA: 0x00018558 File Offset: 0x00016758
		private VBTypeAttributeConverter()
		{
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x000185A5 File Offset: 0x000167A5
		public static VBTypeAttributeConverter Default { get; } = new VBTypeAttributeConverter();

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x000185AC File Offset: 0x000167AC
		protected override string[] Names { get; } = new string[] { "Public", "Friend" };

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x000185B4 File Offset: 0x000167B4
		protected override object[] Values { get; } = new object[]
		{
			TypeAttributes.Public,
			TypeAttributes.NotPublic
		};

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x000185BC File Offset: 0x000167BC
		protected override object DefaultValue
		{
			get
			{
				return TypeAttributes.Public;
			}
		}
	}
}
