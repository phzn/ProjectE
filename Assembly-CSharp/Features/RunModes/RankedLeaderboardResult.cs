using System;
using System.Collections.Generic;

namespace Features.RunModes
{
	// Token: 0x02000D54 RID: 3412
	public sealed class RankedLeaderboardResult
	{
		// Token: 0x1700104B RID: 4171
		// (get) Token: 0x06006446 RID: 25670 RVA: 0x002829C7 File Offset: 0x00280BC7
		public RankedLeaderboardScope Scope { get; }

		// Token: 0x1700104C RID: 4172
		// (get) Token: 0x06006447 RID: 25671 RVA: 0x002829CF File Offset: 0x00280BCF
		public RankedLeaderboardState State { get; }

		// Token: 0x1700104D RID: 4173
		// (get) Token: 0x06006448 RID: 25672 RVA: 0x002829D7 File Offset: 0x00280BD7
		public IReadOnlyList<RankedLeaderboardEntry> Entries { get; }

		// Token: 0x1700104E RID: 4174
		// (get) Token: 0x06006449 RID: 25673 RVA: 0x002829DF File Offset: 0x00280BDF
		public RankedLeaderboardEntry LocalPlayerEntry { get; }

		// Token: 0x1700104F RID: 4175
		// (get) Token: 0x0600644A RID: 25674 RVA: 0x002829E7 File Offset: 0x00280BE7
		public int Page { get; }

		// Token: 0x17001050 RID: 4176
		// (get) Token: 0x0600644B RID: 25675 RVA: 0x002829EF File Offset: 0x00280BEF
		public int PageSize { get; }

		// Token: 0x17001051 RID: 4177
		// (get) Token: 0x0600644C RID: 25676 RVA: 0x002829F7 File Offset: 0x00280BF7
		public int TotalEntries { get; }

		// Token: 0x17001052 RID: 4178
		// (get) Token: 0x0600644D RID: 25677 RVA: 0x002829FF File Offset: 0x00280BFF
		public string ErrorMessage { get; }

		// Token: 0x17001053 RID: 4179
		// (get) Token: 0x0600644E RID: 25678 RVA: 0x00282A07 File Offset: 0x00280C07
		public bool HasPreviousPage
		{
			get
			{
				return this.Scope != RankedLeaderboardScope.AroundPlayer && this.Page > 0;
			}
		}

		// Token: 0x17001054 RID: 4180
		// (get) Token: 0x0600644F RID: 25679 RVA: 0x00282A20 File Offset: 0x00280C20
		public bool HasNextPage
		{
			get
			{
				if (this.Scope == RankedLeaderboardScope.AroundPlayer || this.State != RankedLeaderboardState.Ready)
				{
					return false;
				}
				bool? hasNextPage = this._hasNextPage;
				if (hasNextPage == null)
				{
					return ((long)this.Page + 1L) * (long)this.PageSize < (long)this.TotalEntries;
				}
				return hasNextPage.GetValueOrDefault();
			}
		}

		// Token: 0x06006450 RID: 25680 RVA: 0x00282A74 File Offset: 0x00280C74
		public RankedLeaderboardResult(RankedLeaderboardScope scope, RankedLeaderboardState state, int page, int pageSize, IReadOnlyList<RankedLeaderboardEntry> entries = null, RankedLeaderboardEntry localPlayerEntry = null, int totalEntries = 0, string errorMessage = null, bool? hasNextPage = null)
		{
			this.Scope = scope;
			this.State = state;
			this.Page = ((scope == RankedLeaderboardScope.AroundPlayer) ? 0 : Math.Max(0, page));
			this.PageSize = Math.Max(1, Math.Min(100, pageSize));
			this.Entries = entries ?? Array.Empty<RankedLeaderboardEntry>();
			this.LocalPlayerEntry = localPlayerEntry;
			this.TotalEntries = Math.Max(-1, totalEntries);
			this.ErrorMessage = errorMessage ?? string.Empty;
			this._hasNextPage = hasNextPage;
		}

		// Token: 0x04006747 RID: 26439
		private readonly bool? _hasNextPage;
	}
}
