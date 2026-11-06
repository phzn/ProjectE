using System;
using System.Collections.Generic;

namespace Features.NeckSigns
{
	// Token: 0x02000F75 RID: 3957
	public sealed class NeckSignDesignStore
	{
		// Token: 0x0600771E RID: 30494 RVA: 0x00306CEC File Offset: 0x00304EEC
		public NeckSignDesignData Get(string partId)
		{
			NeckSignDesignData neckSignDesignData;
			if (string.IsNullOrEmpty(partId) || !this._designs.TryGetValue(partId, out neckSignDesignData))
			{
				return new NeckSignDesignData
				{
					partId = partId
				};
			}
			return neckSignDesignData.Clone();
		}

		// Token: 0x0600771F RID: 30495 RVA: 0x00306D24 File Offset: 0x00304F24
		public void Set(NeckSignDesignData design)
		{
			if (design == null || string.IsNullOrEmpty(design.partId))
			{
				return;
			}
			this._designs[design.partId] = design.Clone();
		}

		// Token: 0x06007720 RID: 30496 RVA: 0x00306D50 File Offset: 0x00304F50
		public List<NeckSignDesignData> Capture()
		{
			List<NeckSignDesignData> list = new List<NeckSignDesignData>(this._designs.Count);
			foreach (NeckSignDesignData neckSignDesignData in this._designs.Values)
			{
				list.Add(neckSignDesignData.Clone());
			}
			return list;
		}

		// Token: 0x06007721 RID: 30497 RVA: 0x00306DC0 File Offset: 0x00304FC0
		public void Apply(IEnumerable<NeckSignDesignData> designs)
		{
			this._designs.Clear();
			if (designs == null)
			{
				return;
			}
			foreach (NeckSignDesignData neckSignDesignData in designs)
			{
				this.Set(neckSignDesignData);
			}
		}

		// Token: 0x06007722 RID: 30498 RVA: 0x00306E18 File Offset: 0x00305018
		public void Clear()
		{
			this._designs.Clear();
		}

		// Token: 0x04007A60 RID: 31328
		private readonly Dictionary<string, NeckSignDesignData> _designs = new Dictionary<string, NeckSignDesignData>(StringComparer.OrdinalIgnoreCase);
	}
}
