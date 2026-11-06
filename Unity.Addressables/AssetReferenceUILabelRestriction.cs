using System;
using System.Text;

namespace UnityEngine
{
	// Token: 0x02000005 RID: 5
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public sealed class AssetReferenceUILabelRestriction : AssetReferenceUIRestriction
	{
		// Token: 0x06000009 RID: 9 RVA: 0x000020F2 File Offset: 0x000002F2
		public AssetReferenceUILabelRestriction(params string[] allowedLabels)
		{
			this.m_AllowedLabels = allowedLabels;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002101 File Offset: 0x00000301
		public override bool ValidateAsset(Object obj)
		{
			return true;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002104 File Offset: 0x00000304
		public override bool ValidateAsset(string path)
		{
			return true;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002108 File Offset: 0x00000308
		public override string ToString()
		{
			if (this.m_CachedToString == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = true;
				foreach (string text in this.m_AllowedLabels)
				{
					if (!flag)
					{
						stringBuilder.Append(',');
					}
					flag = false;
					stringBuilder.Append(text);
				}
				this.m_CachedToString = stringBuilder.ToString();
			}
			return this.m_CachedToString;
		}

		// Token: 0x04000002 RID: 2
		public string[] m_AllowedLabels;

		// Token: 0x04000003 RID: 3
		public string m_CachedToString;
	}
}
