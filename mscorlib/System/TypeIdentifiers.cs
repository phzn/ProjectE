using System;

namespace System
{
	// Token: 0x0200025B RID: 603
	internal class TypeIdentifiers
	{
		// Token: 0x06001BB9 RID: 7097 RVA: 0x00067AF5 File Offset: 0x00065CF5
		internal static TypeIdentifier FromDisplay(string displayName)
		{
			return new TypeIdentifiers.Display(displayName);
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x00067AFD File Offset: 0x00065CFD
		internal static TypeIdentifier FromInternal(string internalName)
		{
			return new TypeIdentifiers.Internal(internalName);
		}

		// Token: 0x06001BBB RID: 7099 RVA: 0x00067B05 File Offset: 0x00065D05
		internal static TypeIdentifier FromInternal(string internalNameSpace, TypeIdentifier typeName)
		{
			return new TypeIdentifiers.Internal(internalNameSpace, typeName);
		}

		// Token: 0x06001BBC RID: 7100 RVA: 0x00067B0E File Offset: 0x00065D0E
		internal static TypeIdentifier WithoutEscape(string simpleName)
		{
			return new TypeIdentifiers.NoEscape(simpleName);
		}

		// Token: 0x0200025C RID: 604
		private class Display : TypeNames.ATypeName, TypeIdentifier, TypeName, IEquatable<TypeName>
		{
			// Token: 0x06001BBE RID: 7102 RVA: 0x00067B16 File Offset: 0x00065D16
			internal Display(string displayName)
			{
				this.displayName = displayName;
				this.internal_name = null;
			}

			// Token: 0x1700032F RID: 815
			// (get) Token: 0x06001BBF RID: 7103 RVA: 0x00067B2C File Offset: 0x00065D2C
			public override string DisplayName
			{
				get
				{
					return this.displayName;
				}
			}

			// Token: 0x17000330 RID: 816
			// (get) Token: 0x06001BC0 RID: 7104 RVA: 0x00067B34 File Offset: 0x00065D34
			public string InternalName
			{
				get
				{
					if (this.internal_name == null)
					{
						this.internal_name = this.GetInternalName();
					}
					return this.internal_name;
				}
			}

			// Token: 0x06001BC1 RID: 7105 RVA: 0x00067B50 File Offset: 0x00065D50
			private string GetInternalName()
			{
				return TypeSpec.UnescapeInternalName(this.displayName);
			}

			// Token: 0x06001BC2 RID: 7106 RVA: 0x00067B5D File Offset: 0x00065D5D
			public override TypeName NestedName(TypeIdentifier innerName)
			{
				return TypeNames.FromDisplay(this.DisplayName + "+" + innerName.DisplayName);
			}

			// Token: 0x0400198E RID: 6542
			private string displayName;

			// Token: 0x0400198F RID: 6543
			private string internal_name;
		}

		// Token: 0x0200025D RID: 605
		private class Internal : TypeNames.ATypeName, TypeIdentifier, TypeName, IEquatable<TypeName>
		{
			// Token: 0x06001BC3 RID: 7107 RVA: 0x00067B7A File Offset: 0x00065D7A
			internal Internal(string internalName)
			{
				this.internalName = internalName;
				this.display_name = null;
			}

			// Token: 0x06001BC4 RID: 7108 RVA: 0x00067B90 File Offset: 0x00065D90
			internal Internal(string nameSpaceInternal, TypeIdentifier typeName)
			{
				this.internalName = nameSpaceInternal + "." + typeName.InternalName;
				this.display_name = null;
			}

			// Token: 0x17000331 RID: 817
			// (get) Token: 0x06001BC5 RID: 7109 RVA: 0x00067BB6 File Offset: 0x00065DB6
			public override string DisplayName
			{
				get
				{
					if (this.display_name == null)
					{
						this.display_name = this.GetDisplayName();
					}
					return this.display_name;
				}
			}

			// Token: 0x17000332 RID: 818
			// (get) Token: 0x06001BC6 RID: 7110 RVA: 0x00067BD2 File Offset: 0x00065DD2
			public string InternalName
			{
				get
				{
					return this.internalName;
				}
			}

			// Token: 0x06001BC7 RID: 7111 RVA: 0x00067BDA File Offset: 0x00065DDA
			private string GetDisplayName()
			{
				return TypeSpec.EscapeDisplayName(this.internalName);
			}

			// Token: 0x06001BC8 RID: 7112 RVA: 0x00067B5D File Offset: 0x00065D5D
			public override TypeName NestedName(TypeIdentifier innerName)
			{
				return TypeNames.FromDisplay(this.DisplayName + "+" + innerName.DisplayName);
			}

			// Token: 0x04001990 RID: 6544
			private string internalName;

			// Token: 0x04001991 RID: 6545
			private string display_name;
		}

		// Token: 0x0200025E RID: 606
		private class NoEscape : TypeNames.ATypeName, TypeIdentifier, TypeName, IEquatable<TypeName>
		{
			// Token: 0x06001BC9 RID: 7113 RVA: 0x00067BE7 File Offset: 0x00065DE7
			internal NoEscape(string simpleName)
			{
				this.simpleName = simpleName;
			}

			// Token: 0x17000333 RID: 819
			// (get) Token: 0x06001BCA RID: 7114 RVA: 0x00067BF6 File Offset: 0x00065DF6
			public override string DisplayName
			{
				get
				{
					return this.simpleName;
				}
			}

			// Token: 0x17000334 RID: 820
			// (get) Token: 0x06001BCB RID: 7115 RVA: 0x00067BF6 File Offset: 0x00065DF6
			public string InternalName
			{
				get
				{
					return this.simpleName;
				}
			}

			// Token: 0x06001BCC RID: 7116 RVA: 0x00067B5D File Offset: 0x00065D5D
			public override TypeName NestedName(TypeIdentifier innerName)
			{
				return TypeNames.FromDisplay(this.DisplayName + "+" + innerName.DisplayName);
			}

			// Token: 0x04001992 RID: 6546
			private string simpleName;
		}
	}
}
