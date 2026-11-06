using System;
using System.Threading;

namespace System
{
	// Token: 0x02000152 RID: 338
	internal sealed class LazyDebugView<T>
	{
		// Token: 0x06000CBC RID: 3260 RVA: 0x00032DB1 File Offset: 0x00030FB1
		public LazyDebugView(Lazy<T> lazy)
		{
			this._lazy = lazy;
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x00032DC0 File Offset: 0x00030FC0
		public bool IsValueCreated
		{
			get
			{
				return this._lazy.IsValueCreated;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x00032DCD File Offset: 0x00030FCD
		public T Value
		{
			get
			{
				return this._lazy.ValueForDebugDisplay;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x00032DDA File Offset: 0x00030FDA
		public LazyThreadSafetyMode? Mode
		{
			get
			{
				return this._lazy.Mode;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x00032DE7 File Offset: 0x00030FE7
		public bool IsValueFaulted
		{
			get
			{
				return this._lazy.IsValueFaulted;
			}
		}

		// Token: 0x04001270 RID: 4720
		private readonly Lazy<T> _lazy;
	}
}
