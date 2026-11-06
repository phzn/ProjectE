using System;
using System.Collections.Generic;

namespace Unity.Properties.Internal
{
	// Token: 0x02000098 RID: 152
	internal interface IAttributes
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000336 RID: 822
		// (set) Token: 0x06000337 RID: 823
		List<Attribute> Attributes { get; set; }

		// Token: 0x06000338 RID: 824
		void AddAttribute(Attribute attribute);

		// Token: 0x06000339 RID: 825
		void AddAttributes(IEnumerable<Attribute> attributes);

		// Token: 0x0600033A RID: 826
		AttributesScope CreateAttributesScope(IAttributes attributes);
	}
}
