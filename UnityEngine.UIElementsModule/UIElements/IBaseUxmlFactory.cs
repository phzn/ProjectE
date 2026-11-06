using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020003CC RID: 972
	public interface IBaseUxmlFactory
	{
		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06002003 RID: 8195
		string uxmlName { get; }

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06002004 RID: 8196
		string uxmlNamespace { get; }

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06002005 RID: 8197
		string uxmlQualifiedName { get; }

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06002006 RID: 8198
		Type uxmlType { get; }

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06002007 RID: 8199
		bool canHaveAnyAttribute { get; }

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06002008 RID: 8200
		IEnumerable<UxmlAttributeDescription> uxmlAttributesDescription { get; }

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06002009 RID: 8201
		IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription { get; }

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x0600200A RID: 8202
		string substituteForTypeName { get; }

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x0600200B RID: 8203
		string substituteForTypeNamespace { get; }

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x0600200C RID: 8204
		string substituteForTypeQualifiedName { get; }

		// Token: 0x0600200D RID: 8205
		bool AcceptsAttributeBag(IUxmlAttributes bag, CreationContext cc);
	}
}
