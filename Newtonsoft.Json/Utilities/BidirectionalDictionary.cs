using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000043 RID: 67
	[NullableContext(1)]
	[Nullable(0)]
	internal class BidirectionalDictionary<TFirst, TSecond>
	{
		// Token: 0x06000439 RID: 1081 RVA: 0x0001077C File Offset: 0x0000E97C
		public BidirectionalDictionary()
			: this(EqualityComparer<TFirst>.Default, EqualityComparer<TSecond>.Default)
		{
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0001078E File Offset: 0x0000E98E
		public BidirectionalDictionary(IEqualityComparer<TFirst> firstEqualityComparer, IEqualityComparer<TSecond> secondEqualityComparer)
			: this(firstEqualityComparer, secondEqualityComparer, "Duplicate item already exists for '{0}'.", "Duplicate item already exists for '{0}'.")
		{
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x000107A2 File Offset: 0x0000E9A2
		public BidirectionalDictionary(IEqualityComparer<TFirst> firstEqualityComparer, IEqualityComparer<TSecond> secondEqualityComparer, string duplicateFirstErrorMessage, string duplicateSecondErrorMessage)
		{
			this._firstToSecond = new Dictionary<TFirst, TSecond>(firstEqualityComparer);
			this._secondToFirst = new Dictionary<TSecond, TFirst>(secondEqualityComparer);
			this._duplicateFirstErrorMessage = duplicateFirstErrorMessage;
			this._duplicateSecondErrorMessage = duplicateSecondErrorMessage;
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x000107D4 File Offset: 0x0000E9D4
		public void Set(TFirst first, TSecond second)
		{
			TSecond tsecond;
			if (this._firstToSecond.TryGetValue(first, out tsecond) && !tsecond.Equals(second))
			{
				throw new ArgumentException(this._duplicateFirstErrorMessage.FormatWith(CultureInfo.InvariantCulture, first));
			}
			TFirst tfirst;
			if (this._secondToFirst.TryGetValue(second, out tfirst) && !tfirst.Equals(first))
			{
				throw new ArgumentException(this._duplicateSecondErrorMessage.FormatWith(CultureInfo.InvariantCulture, second));
			}
			this._firstToSecond.Add(first, second);
			this._secondToFirst.Add(second, first);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0001087D File Offset: 0x0000EA7D
		public bool TryGetByFirst(TFirst first, [Nullable(2)] [NotNullWhen(true)] out TSecond second)
		{
			return this._firstToSecond.TryGetValue(first, out second);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0001088C File Offset: 0x0000EA8C
		public bool TryGetBySecond(TSecond second, [Nullable(2)] [NotNullWhen(true)] out TFirst first)
		{
			return this._secondToFirst.TryGetValue(second, out first);
		}

		// Token: 0x04000155 RID: 341
		private readonly IDictionary<TFirst, TSecond> _firstToSecond;

		// Token: 0x04000156 RID: 342
		private readonly IDictionary<TSecond, TFirst> _secondToFirst;

		// Token: 0x04000157 RID: 343
		private readonly string _duplicateFirstErrorMessage;

		// Token: 0x04000158 RID: 344
		private readonly string _duplicateSecondErrorMessage;
	}
}
