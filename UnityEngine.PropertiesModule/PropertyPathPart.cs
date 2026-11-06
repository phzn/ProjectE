using System;
using System.Runtime.CompilerServices;

namespace Unity.Properties
{
	// Token: 0x0200002A RID: 42
	public readonly struct PropertyPathPart : IEquatable<PropertyPathPart>
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x000035BB File Offset: 0x000017BB
		public bool IsName
		{
			get
			{
				return this.Kind == PropertyPathPartKind.Name;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x000035C6 File Offset: 0x000017C6
		public bool IsIndex
		{
			get
			{
				return this.Kind == PropertyPathPartKind.Index;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x000035D1 File Offset: 0x000017D1
		public bool IsKey
		{
			get
			{
				return this.Kind == PropertyPathPartKind.Key;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x000035DC File Offset: 0x000017DC
		public PropertyPathPartKind Kind
		{
			get
			{
				return this.m_Kind;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x000035E4 File Offset: 0x000017E4
		public string Name
		{
			get
			{
				this.CheckKind(PropertyPathPartKind.Name);
				return this.m_Name;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00003604 File Offset: 0x00001804
		public int Index
		{
			get
			{
				this.CheckKind(PropertyPathPartKind.Index);
				return this.m_Index;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00003624 File Offset: 0x00001824
		public object Key
		{
			get
			{
				this.CheckKind(PropertyPathPartKind.Key);
				return this.m_Key;
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003644 File Offset: 0x00001844
		public PropertyPathPart(string name)
		{
			this.m_Kind = PropertyPathPartKind.Name;
			this.m_Name = name;
			this.m_Index = -1;
			this.m_Key = null;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00003663 File Offset: 0x00001863
		public PropertyPathPart(int index)
		{
			this.m_Kind = PropertyPathPartKind.Index;
			this.m_Name = string.Empty;
			this.m_Index = index;
			this.m_Key = null;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003686 File Offset: 0x00001886
		public PropertyPathPart(object key)
		{
			this.m_Kind = PropertyPathPartKind.Key;
			this.m_Name = string.Empty;
			this.m_Index = -1;
			this.m_Key = key;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000036AC File Offset: 0x000018AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CheckKind(PropertyPathPartKind type)
		{
			bool flag = type != this.Kind;
			if (flag)
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000036D0 File Offset: 0x000018D0
		public override string ToString()
		{
			PropertyPathPartKind kind = this.Kind;
			if (!true)
			{
			}
			string text;
			switch (kind)
			{
			case PropertyPathPartKind.Name:
				text = this.m_Name;
				break;
			case PropertyPathPartKind.Index:
				text = "[" + this.m_Index.ToString() + "]";
				break;
			case PropertyPathPartKind.Key:
			{
				string text2 = "[\"";
				object key = this.m_Key;
				text = text2 + ((key != null) ? key.ToString() : null) + "\"]";
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
			if (!true)
			{
			}
			return text;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003758 File Offset: 0x00001958
		public bool Equals(PropertyPathPart other)
		{
			return this.m_Kind == other.m_Kind && this.m_Name == other.m_Name && this.m_Index == other.m_Index && object.Equals(this.m_Key, other.m_Key);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000037B0 File Offset: 0x000019B0
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is PropertyPathPart)
			{
				PropertyPathPart propertyPathPart = (PropertyPathPart)obj;
				flag = this.Equals(propertyPathPart);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000037DC File Offset: 0x000019DC
		public override int GetHashCode()
		{
			int kind = (int)this.m_Kind;
			PropertyPathPartKind kind2 = this.m_Kind;
			if (!true)
			{
			}
			int num;
			switch (kind2)
			{
			case PropertyPathPartKind.Name:
				num = (kind * 397) ^ ((this.m_Name != null) ? this.m_Name.GetHashCode() : 0);
				break;
			case PropertyPathPartKind.Index:
				num = (kind * 397) ^ this.m_Index;
				break;
			case PropertyPathPartKind.Key:
				num = (kind * 397) ^ ((this.m_Key != null) ? this.m_Key.GetHashCode() : 0);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			if (!true)
			{
			}
			return num;
		}

		// Token: 0x0400003D RID: 61
		private readonly PropertyPathPartKind m_Kind;

		// Token: 0x0400003E RID: 62
		private readonly string m_Name;

		// Token: 0x0400003F RID: 63
		private readonly int m_Index;

		// Token: 0x04000040 RID: 64
		private readonly object m_Key;
	}
}
