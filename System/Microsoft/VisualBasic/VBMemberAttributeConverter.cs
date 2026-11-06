using System;
using System.CodeDom;

namespace Microsoft.VisualBasic
{
	// Token: 0x02000137 RID: 311
	internal sealed class VBMemberAttributeConverter : VBModifierAttributeConverter
	{
		// Token: 0x060007B2 RID: 1970 RVA: 0x000183A8 File Offset: 0x000165A8
		private VBMemberAttributeConverter()
		{
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x0001843C File Offset: 0x0001663C
		public static VBMemberAttributeConverter Default { get; } = new VBMemberAttributeConverter();

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x00018443 File Offset: 0x00016643
		protected override string[] Names { get; } = new string[] { "Public", "Protected", "Protected Friend", "Friend", "Private" };

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x0001844B File Offset: 0x0001664B
		protected override object[] Values { get; } = new object[]
		{
			MemberAttributes.Public,
			MemberAttributes.Family,
			MemberAttributes.FamilyOrAssembly,
			MemberAttributes.Assembly,
			MemberAttributes.Private
		};

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x00018453 File Offset: 0x00016653
		protected override object DefaultValue
		{
			get
			{
				return MemberAttributes.Private;
			}
		}
	}
}
