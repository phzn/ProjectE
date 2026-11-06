using System;

namespace System.Data.Common
{
	/// <summary>Provides a mechanism for enumerating all available instances of database servers within the local network.</summary>
	// Token: 0x02000395 RID: 917
	public abstract class DbDataSourceEnumerator
	{
		/// <summary>Retrieves a <see cref="T:System.Data.DataTable" /> containing information about all visible instances of the server represented by the strongly typed instance of this class.</summary>
		/// <returns>A <see cref="T:System.Data.DataTable" /> containing information about the visible instances of the associated data source.</returns>
		// Token: 0x06002CA1 RID: 11425
		public abstract DataTable GetDataSources();
	}
}
