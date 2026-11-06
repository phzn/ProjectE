using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020007E5 RID: 2021
	[StructLayout(LayoutKind.Auto)]
	public readonly struct ConfiguredCancelableAsyncEnumerable<T>
	{
		// Token: 0x060045DB RID: 17883 RVA: 0x000E53A2 File Offset: 0x000E35A2
		internal ConfiguredCancelableAsyncEnumerable(IAsyncEnumerable<T> enumerable, bool continueOnCapturedContext, CancellationToken cancellationToken)
		{
			this._enumerable = enumerable;
			this._continueOnCapturedContext = continueOnCapturedContext;
			this._cancellationToken = cancellationToken;
		}

		// Token: 0x060045DC RID: 17884 RVA: 0x000E53B9 File Offset: 0x000E35B9
		public ConfiguredCancelableAsyncEnumerable<T> ConfigureAwait(bool continueOnCapturedContext)
		{
			return new ConfiguredCancelableAsyncEnumerable<T>(this._enumerable, continueOnCapturedContext, this._cancellationToken);
		}

		// Token: 0x060045DD RID: 17885 RVA: 0x000E53CD File Offset: 0x000E35CD
		public ConfiguredCancelableAsyncEnumerable<T> WithCancellation(CancellationToken cancellationToken)
		{
			return new ConfiguredCancelableAsyncEnumerable<T>(this._enumerable, this._continueOnCapturedContext, cancellationToken);
		}

		// Token: 0x060045DE RID: 17886 RVA: 0x000E53E1 File Offset: 0x000E35E1
		public ConfiguredCancelableAsyncEnumerable<T>.Enumerator GetAsyncEnumerator()
		{
			return new ConfiguredCancelableAsyncEnumerable<T>.Enumerator(this._enumerable.GetAsyncEnumerator(this._cancellationToken), this._continueOnCapturedContext);
		}

		// Token: 0x04002D2D RID: 11565
		private readonly IAsyncEnumerable<T> _enumerable;

		// Token: 0x04002D2E RID: 11566
		private readonly CancellationToken _cancellationToken;

		// Token: 0x04002D2F RID: 11567
		private readonly bool _continueOnCapturedContext;

		// Token: 0x020007E6 RID: 2022
		[StructLayout(LayoutKind.Auto)]
		public readonly struct Enumerator
		{
			// Token: 0x060045DF RID: 17887 RVA: 0x000E53FF File Offset: 0x000E35FF
			internal Enumerator(IAsyncEnumerator<T> enumerator, bool continueOnCapturedContext)
			{
				this._enumerator = enumerator;
				this._continueOnCapturedContext = continueOnCapturedContext;
			}

			// Token: 0x060045E0 RID: 17888 RVA: 0x000E5410 File Offset: 0x000E3610
			public ConfiguredValueTaskAwaitable<bool> MoveNextAsync()
			{
				return this._enumerator.MoveNextAsync().ConfigureAwait(this._continueOnCapturedContext);
			}

			// Token: 0x17000AB9 RID: 2745
			// (get) Token: 0x060045E1 RID: 17889 RVA: 0x000E5436 File Offset: 0x000E3636
			public T Current
			{
				get
				{
					return this._enumerator.Current;
				}
			}

			// Token: 0x060045E2 RID: 17890 RVA: 0x000E5444 File Offset: 0x000E3644
			public ConfiguredValueTaskAwaitable DisposeAsync()
			{
				return this._enumerator.DisposeAsync().ConfigureAwait(this._continueOnCapturedContext);
			}

			// Token: 0x04002D30 RID: 11568
			private readonly IAsyncEnumerator<T> _enumerator;

			// Token: 0x04002D31 RID: 11569
			private readonly bool _continueOnCapturedContext;
		}
	}
}
