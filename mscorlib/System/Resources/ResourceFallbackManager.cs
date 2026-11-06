using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace System.Resources
{
	// Token: 0x0200085D RID: 2141
	internal class ResourceFallbackManager : IEnumerable<CultureInfo>, IEnumerable
	{
		// Token: 0x06004744 RID: 18244 RVA: 0x000E818C File Offset: 0x000E638C
		internal ResourceFallbackManager(CultureInfo startingCulture, CultureInfo neutralResourcesCulture, bool useParents)
		{
			if (startingCulture != null)
			{
				this.m_startingCulture = startingCulture;
			}
			else
			{
				this.m_startingCulture = CultureInfo.CurrentUICulture;
			}
			this.m_neutralResourcesCulture = neutralResourcesCulture;
			this.m_useParents = useParents;
		}

		// Token: 0x06004745 RID: 18245 RVA: 0x000E81B9 File Offset: 0x000E63B9
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06004746 RID: 18246 RVA: 0x000E81C1 File Offset: 0x000E63C1
		public IEnumerator<CultureInfo> GetEnumerator()
		{
			bool reachedNeutralResourcesCulture = false;
			CultureInfo currentCulture = this.m_startingCulture;
			while (this.m_neutralResourcesCulture == null || !(currentCulture.Name == this.m_neutralResourcesCulture.Name))
			{
				yield return currentCulture;
				currentCulture = currentCulture.Parent;
				if (!this.m_useParents || currentCulture.HasInvariantCultureName)
				{
					IL_00CE:
					if (!this.m_useParents || this.m_startingCulture.HasInvariantCultureName)
					{
						yield break;
					}
					if (reachedNeutralResourcesCulture)
					{
						yield break;
					}
					yield return CultureInfo.InvariantCulture;
					yield break;
				}
			}
			yield return CultureInfo.InvariantCulture;
			reachedNeutralResourcesCulture = true;
			goto IL_00CE;
		}

		// Token: 0x04002DA4 RID: 11684
		private CultureInfo m_startingCulture;

		// Token: 0x04002DA5 RID: 11685
		private CultureInfo m_neutralResourcesCulture;

		// Token: 0x04002DA6 RID: 11686
		private bool m_useParents;
	}
}
