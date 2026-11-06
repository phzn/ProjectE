using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting
{
	/// <summary>Implements a base class that holds the configuration information used to activate an instance of a remote type.</summary>
	// Token: 0x02000576 RID: 1398
	[ComVisible(true)]
	public class TypeEntry
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.TypeEntry" /> class.</summary>
		// Token: 0x060036EF RID: 14063 RVA: 0x0000259F File Offset: 0x0000079F
		protected TypeEntry()
		{
		}

		/// <summary>Gets the assembly name of the object type configured to be a remote-activated type.</summary>
		/// <returns>The assembly name of the object type configured to be a remote-activated type.</returns>
		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x060036F0 RID: 14064 RVA: 0x000C6630 File Offset: 0x000C4830
		// (set) Token: 0x060036F1 RID: 14065 RVA: 0x000C6638 File Offset: 0x000C4838
		public string AssemblyName
		{
			get
			{
				return this.assembly_name;
			}
			set
			{
				this.assembly_name = value;
			}
		}

		/// <summary>Gets the full type name of the object type configured to be a remote-activated type.</summary>
		/// <returns>The full type name of the object type configured to be a remote-activated type.</returns>
		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x060036F2 RID: 14066 RVA: 0x000C6641 File Offset: 0x000C4841
		// (set) Token: 0x060036F3 RID: 14067 RVA: 0x000C6649 File Offset: 0x000C4849
		public string TypeName
		{
			get
			{
				return this.type_name;
			}
			set
			{
				this.type_name = value;
			}
		}

		// Token: 0x04002568 RID: 9576
		private string assembly_name;

		// Token: 0x04002569 RID: 9577
		private string type_name;
	}
}
