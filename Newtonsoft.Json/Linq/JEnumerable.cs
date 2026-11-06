using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000B9 RID: 185
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct JEnumerable<[Nullable(0)] T> : IJEnumerable<T>, IEnumerable<T>, IEnumerable, IEquatable<JEnumerable<T>> where T : JToken
	{
		// Token: 0x06000A0A RID: 2570 RVA: 0x000289F0 File Offset: 0x00026BF0
		public JEnumerable(IEnumerable<T> enumerable)
		{
			ValidationUtils.ArgumentNotNull(enumerable, "enumerable");
			this._enumerable = enumerable;
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x00028A04 File Offset: 0x00026C04
		public IEnumerator<T> GetEnumerator()
		{
			return (this._enumerable ?? JEnumerable<T>.Empty).GetEnumerator();
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x00028A1F File Offset: 0x00026C1F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x170001D7 RID: 471
		public IJEnumerable<JToken> this[object key]
		{
			get
			{
				if (this._enumerable == null)
				{
					return JEnumerable<JToken>.Empty;
				}
				return new JEnumerable<JToken>(this._enumerable.Values<T, JToken>(key));
			}
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x00028A52 File Offset: 0x00026C52
		public bool Equals([Nullable(new byte[] { 0, 1 })] JEnumerable<T> other)
		{
			return object.Equals(this._enumerable, other._enumerable);
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x00028A68 File Offset: 0x00026C68
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			if (obj is JEnumerable<T>)
			{
				JEnumerable<T> jenumerable = (JEnumerable<T>)obj;
				return this.Equals(jenumerable);
			}
			return false;
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x00028A8D File Offset: 0x00026C8D
		public override int GetHashCode()
		{
			if (this._enumerable == null)
			{
				return 0;
			}
			return this._enumerable.GetHashCode();
		}

		// Token: 0x0400037A RID: 890
		[Nullable(new byte[] { 0, 1 })]
		public static readonly JEnumerable<T> Empty = new JEnumerable<T>(Enumerable.Empty<T>());

		// Token: 0x0400037B RID: 891
		private readonly IEnumerable<T> _enumerable;
	}
}
