using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002CB RID: 715
	public struct CustomStyleProperty<T> : IEquatable<CustomStyleProperty<T>>
	{
		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06001554 RID: 5460 RVA: 0x00054ED3 File Offset: 0x000530D3
		// (set) Token: 0x06001555 RID: 5461 RVA: 0x00054EDB File Offset: 0x000530DB
		public string name { readonly get; private set; }

		// Token: 0x06001556 RID: 5462 RVA: 0x00054EE4 File Offset: 0x000530E4
		public CustomStyleProperty(string propertyName)
		{
			bool flag = !string.IsNullOrEmpty(propertyName) && !propertyName.StartsWith("--");
			if (flag)
			{
				throw new ArgumentException("Custom style property \"" + propertyName + "\" must start with \"--\" prefix.");
			}
			this.name = propertyName;
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x00054F30 File Offset: 0x00053130
		public override bool Equals(object obj)
		{
			bool flag = !(obj is CustomStyleProperty<T>);
			return !flag && this.Equals((CustomStyleProperty<T>)obj);
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x00054F64 File Offset: 0x00053164
		public bool Equals(CustomStyleProperty<T> other)
		{
			return this.name == other.name;
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x00054F88 File Offset: 0x00053188
		public override int GetHashCode()
		{
			return this.name.GetHashCode();
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x00054FA8 File Offset: 0x000531A8
		public static bool operator ==(CustomStyleProperty<T> a, CustomStyleProperty<T> b)
		{
			return a.Equals(b);
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x00054FC4 File Offset: 0x000531C4
		public static bool operator !=(CustomStyleProperty<T> a, CustomStyleProperty<T> b)
		{
			return !(a == b);
		}
	}
}
