using System;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D3 RID: 211
	internal enum QueryOperator
	{
		// Token: 0x040003D0 RID: 976
		None,
		// Token: 0x040003D1 RID: 977
		Equals,
		// Token: 0x040003D2 RID: 978
		NotEquals,
		// Token: 0x040003D3 RID: 979
		Exists,
		// Token: 0x040003D4 RID: 980
		LessThan,
		// Token: 0x040003D5 RID: 981
		LessThanOrEquals,
		// Token: 0x040003D6 RID: 982
		GreaterThan,
		// Token: 0x040003D7 RID: 983
		GreaterThanOrEquals,
		// Token: 0x040003D8 RID: 984
		And,
		// Token: 0x040003D9 RID: 985
		Or,
		// Token: 0x040003DA RID: 986
		RegexEquals,
		// Token: 0x040003DB RID: 987
		StrictEquals,
		// Token: 0x040003DC RID: 988
		StrictNotEquals
	}
}
