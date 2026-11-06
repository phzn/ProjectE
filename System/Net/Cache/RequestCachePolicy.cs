using System;

namespace System.Net.Cache
{
	/// <summary>Defines an application's caching requirements for resources obtained by using <see cref="T:System.Net.WebRequest" /> objects.</summary>
	// Token: 0x02000789 RID: 1929
	public class RequestCachePolicy
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Cache.RequestCachePolicy" /> class.</summary>
		// Token: 0x06003CC4 RID: 15556 RVA: 0x000CEC20 File Offset: 0x000CCE20
		public RequestCachePolicy()
			: this(RequestCacheLevel.Default)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Cache.RequestCachePolicy" /> class. using the specified cache policy.</summary>
		/// <param name="level">A <see cref="T:System.Net.Cache.RequestCacheLevel" /> that specifies the cache behavior for resources obtained using <see cref="T:System.Net.WebRequest" /> objects.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">level is not a valid <see cref="T:System.Net.Cache.RequestCacheLevel" />.value.</exception>
		// Token: 0x06003CC5 RID: 15557 RVA: 0x000CEC29 File Offset: 0x000CCE29
		public RequestCachePolicy(RequestCacheLevel level)
		{
			if (level < RequestCacheLevel.Default || level > RequestCacheLevel.NoCacheNoStore)
			{
				throw new ArgumentOutOfRangeException("level");
			}
			this.m_Level = level;
		}

		/// <summary>Gets the <see cref="T:System.Net.Cache.RequestCacheLevel" /> value specified when this instance was constructed.</summary>
		/// <returns>A <see cref="T:System.Net.Cache.RequestCacheLevel" /> value that specifies the cache behavior for resources obtained using <see cref="T:System.Net.WebRequest" /> objects.</returns>
		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x06003CC6 RID: 15558 RVA: 0x000CEC4B File Offset: 0x000CCE4B
		public RequestCacheLevel Level
		{
			get
			{
				return this.m_Level;
			}
		}

		/// <summary>Returns a string representation of this instance.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the <see cref="P:System.Net.Cache.RequestCachePolicy.Level" /> for this instance.</returns>
		// Token: 0x06003CC7 RID: 15559 RVA: 0x000CEC53 File Offset: 0x000CCE53
		public override string ToString()
		{
			return "Level:" + this.m_Level.ToString();
		}

		// Token: 0x040023F2 RID: 9202
		private RequestCacheLevel m_Level;
	}
}
