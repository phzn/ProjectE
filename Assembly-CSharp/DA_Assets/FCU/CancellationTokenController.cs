using System;
using System.Collections.Generic;
using System.Threading;
using DA_Assets.DAI;

namespace DA_Assets.FCU
{
	// Token: 0x020002BA RID: 698
	[Serializable]
	public class CancellationTokenController : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x060013B1 RID: 5041 RVA: 0x0007F466 File Offset: 0x0007D666
		internal CancellationTokenSource GetToken(TokenType type)
		{
			if (this._tokens.ContainsKey(type))
			{
				return this._tokens[type];
			}
			return null;
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x0007F484 File Offset: 0x0007D684
		public CancellationTokenSource CreateNew(TokenType type)
		{
			this.Cancel(type);
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			this._tokens[type] = cancellationTokenSource;
			return cancellationTokenSource;
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x0007F4AC File Offset: 0x0007D6AC
		public void Cancel(TokenType type)
		{
			if (this._tokens.ContainsKey(type))
			{
				CancellationTokenSource cancellationTokenSource = this._tokens[type];
				if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
				{
					cancellationTokenSource.Cancel();
				}
			}
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x0007F4E8 File Offset: 0x0007D6E8
		public void CancelAll()
		{
			foreach (CancellationTokenSource cancellationTokenSource in this._tokens.Values)
			{
				if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
				{
					cancellationTokenSource.Cancel();
				}
			}
		}

		// Token: 0x0400157C RID: 5500
		private Dictionary<TokenType, CancellationTokenSource> _tokens = new Dictionary<TokenType, CancellationTokenSource>();
	}
}
