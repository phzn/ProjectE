using System;

namespace System.ComponentModel.Design.Serialization
{
	/// <summary>Provides data for the <see cref="E:System.ComponentModel.Design.Serialization.IDesignerSerializationManager.ResolveName" /> event.</summary>
	// Token: 0x02000490 RID: 1168
	public class ResolveNameEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Design.Serialization.ResolveNameEventArgs" /> class.</summary>
		/// <param name="name">The name to resolve.</param>
		// Token: 0x0600254F RID: 9551 RVA: 0x00083278 File Offset: 0x00081478
		public ResolveNameEventArgs(string name)
		{
			this.Name = name;
			this.Value = null;
		}

		/// <summary>Gets the name of the object to resolve.</summary>
		/// <returns>The name of the object to resolve.</returns>
		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06002550 RID: 9552 RVA: 0x0008328E File Offset: 0x0008148E
		public string Name { get; }

		/// <summary>Gets or sets the object that matches the name.</summary>
		/// <returns>The object that the name is associated with.</returns>
		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06002551 RID: 9553 RVA: 0x00083296 File Offset: 0x00081496
		// (set) Token: 0x06002552 RID: 9554 RVA: 0x0008329E File Offset: 0x0008149E
		public object Value { get; set; }
	}
}
