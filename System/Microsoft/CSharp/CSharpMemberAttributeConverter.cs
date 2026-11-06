using System;
using System.CodeDom;

namespace Microsoft.CSharp
{
	// Token: 0x0200013D RID: 317
	internal sealed class CSharpMemberAttributeConverter : CSharpModifierAttributeConverter
	{
		// Token: 0x06000873 RID: 2163 RVA: 0x0001E60C File Offset: 0x0001C80C
		private CSharpMemberAttributeConverter()
		{
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x0001E6A0 File Offset: 0x0001C8A0
		public static CSharpMemberAttributeConverter Default { get; } = new CSharpMemberAttributeConverter();

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x0001E6A7 File Offset: 0x0001C8A7
		protected override string[] Names { get; } = new string[] { "Public", "Protected", "Protected Internal", "Internal", "Private" };

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000876 RID: 2166 RVA: 0x0001E6AF File Offset: 0x0001C8AF
		protected override object[] Values { get; } = new object[]
		{
			MemberAttributes.Public,
			MemberAttributes.Family,
			MemberAttributes.FamilyOrAssembly,
			MemberAttributes.Assembly,
			MemberAttributes.Private
		};

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x00018453 File Offset: 0x00016653
		protected override object DefaultValue
		{
			get
			{
				return MemberAttributes.Private;
			}
		}
	}
}
