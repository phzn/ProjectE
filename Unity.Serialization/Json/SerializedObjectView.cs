using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Properties;
using Unity.Serialization.Json.Unsafe;

namespace Unity.Serialization.Json
{
	// Token: 0x02000089 RID: 137
	public readonly struct SerializedObjectView : ISerializedView, IEnumerable<SerializedMemberView>, IEnumerable
	{
		// Token: 0x06000322 RID: 802 RVA: 0x0000E122 File Offset: 0x0000C322
		static SerializedObjectView()
		{
			PropertyBag.Register<SerializedObjectView>(new SerializedObjectViewPropertyBag());
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000E12E File Offset: 0x0000C32E
		internal unsafe SerializedObjectView(UnsafePackedBinaryStream* stream, Handle handle)
		{
			this.m_Stream = stream;
			this.m_Handle = handle;
		}

		// Token: 0x1700005D RID: 93
		public SerializedValueView this[string name]
		{
			get
			{
				return this.GetValue(name);
			}
		}

		// Token: 0x1700005E RID: 94
		public SerializedValueView this[in FixedString32Bytes name]
		{
			get
			{
				return this.GetValue<FixedString32Bytes>(in name);
			}
		}

		// Token: 0x1700005F RID: 95
		public SerializedValueView this[in FixedString64Bytes name]
		{
			get
			{
				return this.GetValue<FixedString64Bytes>(in name);
			}
		}

		// Token: 0x17000060 RID: 96
		public SerializedValueView this[in FixedString128Bytes name]
		{
			get
			{
				return this.GetValue<FixedString128Bytes>(in name);
			}
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000E164 File Offset: 0x0000C364
		public SerializedMemberView GetMember(string name)
		{
			SerializedMemberView serializedMemberView;
			if (!this.TryGetMember(name, out serializedMemberView))
			{
				throw new KeyNotFoundException(name);
			}
			return serializedMemberView;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000E184 File Offset: 0x0000C384
		public bool TryGetMember(string name, out SerializedMemberView member)
		{
			foreach (SerializedMemberView serializedMemberView in this)
			{
				if (serializedMemberView.Name().Equals(name))
				{
					member = serializedMemberView;
					return true;
				}
			}
			member = default(SerializedMemberView);
			return false;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000E1F4 File Offset: 0x0000C3F4
		public SerializedValueView GetValue(string name)
		{
			SerializedValueView serializedValueView;
			if (!this.TryGetValue(name, out serializedValueView))
			{
				throw new KeyNotFoundException(name);
			}
			return serializedValueView;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000E214 File Offset: 0x0000C414
		public bool TryGetValue(string name, out SerializedValueView value)
		{
			foreach (SerializedMemberView serializedMemberView in this)
			{
				if (serializedMemberView.Name().Equals(name))
				{
					value = serializedMemberView.Value();
					return true;
				}
			}
			value = default(SerializedValueView);
			return false;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000E288 File Offset: 0x0000C488
		public bool TryGetValueAsString(string name, out string value)
		{
			value = null;
			SerializedValueView serializedValueView;
			if (!this.TryGetValue(name, out serializedValueView))
			{
				return false;
			}
			if (serializedValueView.Type != TokenType.String)
			{
				return false;
			}
			value = serializedValueView.ToString();
			return true;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000E2C4 File Offset: 0x0000C4C4
		public bool TryGetValueAsInt64(string name, out long value)
		{
			value = 0L;
			SerializedValueView serializedValueView;
			if (!this.TryGetValue(name, out serializedValueView))
			{
				return false;
			}
			if (serializedValueView.Type != TokenType.Primitive)
			{
				return false;
			}
			value = serializedValueView.AsPrimitiveView().AsInt64();
			return true;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000E300 File Offset: 0x0000C500
		public bool TryGetValueAsUInt64(string name, out ulong value)
		{
			value = 0UL;
			SerializedValueView serializedValueView;
			if (!this.TryGetValue(name, out serializedValueView))
			{
				return false;
			}
			if (serializedValueView.Type != TokenType.Primitive)
			{
				return false;
			}
			value = serializedValueView.AsPrimitiveView().AsUInt64();
			return true;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000E33C File Offset: 0x0000C53C
		public bool TryGetValueAsFloat(string name, out float value)
		{
			value = 0f;
			SerializedValueView serializedValueView;
			if (!this.TryGetValue(name, out serializedValueView))
			{
				return false;
			}
			if (serializedValueView.Type != TokenType.Primitive)
			{
				return false;
			}
			value = serializedValueView.AsPrimitiveView().AsFloat();
			return true;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000E37C File Offset: 0x0000C57C
		public bool TryGetValueAsDouble(string name, out double value)
		{
			value = 0.0;
			SerializedValueView serializedValueView;
			if (!this.TryGetValue(name, out serializedValueView))
			{
				return false;
			}
			if (serializedValueView.Type != TokenType.Primitive)
			{
				return false;
			}
			value = serializedValueView.AsPrimitiveView().AsDouble();
			return true;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000E3C0 File Offset: 0x0000C5C0
		public bool TryGetValueAsBoolean(string name, out bool value)
		{
			value = false;
			SerializedValueView serializedValueView;
			if (!this.TryGetValue(name, out serializedValueView))
			{
				return false;
			}
			if (serializedValueView.Type != TokenType.Primitive)
			{
				return false;
			}
			value = serializedValueView.AsPrimitiveView().AsBoolean();
			return true;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000E3FC File Offset: 0x0000C5FC
		public SerializedValueView GetValue<[IsUnmanaged] T>(in T name) where T : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			SerializedValueView serializedValueView;
			if (!this.TryGetValue<T>(in name, out serializedValueView))
			{
				throw new KeyNotFoundException();
			}
			return serializedValueView;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000E41C File Offset: 0x0000C61C
		public bool TryGetValue<[IsUnmanaged] T>(in T name, out SerializedValueView value) where T : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			foreach (SerializedMemberView serializedMemberView in this)
			{
				if (serializedMemberView.Name().Equals<T>(name))
				{
					value = serializedMemberView.Value();
					return true;
				}
			}
			value = default(SerializedValueView);
			return false;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000E498 File Offset: 0x0000C698
		public SerializedMemberView GetMember<[IsUnmanaged] T>(in T name) where T : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			SerializedMemberView serializedMemberView;
			if (!this.TryGetMember<T>(in name, out serializedMemberView))
			{
				throw new KeyNotFoundException();
			}
			return serializedMemberView;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000E4B8 File Offset: 0x0000C6B8
		public bool TryGetMember<[IsUnmanaged] T>(in T name, out SerializedMemberView member) where T : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			foreach (SerializedMemberView serializedMemberView in this)
			{
				if (serializedMemberView.Name().Equals<T>(name))
				{
					member = serializedMemberView;
					return true;
				}
			}
			member = default(SerializedMemberView);
			return false;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000E52C File Offset: 0x0000C72C
		public SerializedObjectView.Enumerator GetEnumerator()
		{
			return new SerializedObjectView.Enumerator(this.m_Stream, this.m_Handle);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000E53F File Offset: 0x0000C73F
		IEnumerator<SerializedMemberView> IEnumerable<SerializedMemberView>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000E53F File Offset: 0x0000C73F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000E54C File Offset: 0x0000C74C
		public static implicit operator SerializedValueView(SerializedObjectView view)
		{
			return new SerializedValueView(view.m_Stream, view.m_Handle);
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000E55F File Offset: 0x0000C75F
		internal unsafe UnsafeObjectView AsUnsafe()
		{
			return new UnsafeObjectView(this.m_Stream, this.m_Stream->GetTokenIndex(this.m_Handle));
		}

		// Token: 0x040001B9 RID: 441
		[NativeDisableUnsafePtrRestriction]
		private unsafe readonly UnsafePackedBinaryStream* m_Stream;

		// Token: 0x040001BA RID: 442
		private readonly Handle m_Handle;

		// Token: 0x0200008A RID: 138
		public struct Enumerator : IEnumerator<SerializedMemberView>, IEnumerator, IDisposable
		{
			// Token: 0x0600033B RID: 827 RVA: 0x0000E580 File Offset: 0x0000C780
			internal unsafe Enumerator(UnsafePackedBinaryStream* stream, Handle start)
			{
				this.m_Stream = stream;
				this.m_Start = start;
				this.m_Current = new Handle
				{
					Index = -1,
					Version = -1
				};
			}

			// Token: 0x0600033C RID: 828 RVA: 0x0000E5BC File Offset: 0x0000C7BC
			public unsafe bool MoveNext()
			{
				int tokenIndex = this.m_Stream->GetTokenIndex(this.m_Start);
				BinaryToken token = this.m_Stream->GetToken(tokenIndex);
				if (token.Length == 1)
				{
					return false;
				}
				if (this.m_Current.Index == -1)
				{
					this.m_Current = this.m_Stream->GetFirstChild(this.m_Start);
					return true;
				}
				if (!this.m_Stream->IsValid(this.m_Current))
				{
					return false;
				}
				int tokenIndex2 = this.m_Stream->GetTokenIndex(this.m_Current);
				BinaryToken token2 = this.m_Stream->GetToken(tokenIndex2);
				if (tokenIndex2 + token2.Length >= tokenIndex + token.Length)
				{
					return false;
				}
				this.m_Current = this.m_Stream->GetHandle(tokenIndex2 + token2.Length);
				return true;
			}

			// Token: 0x0600033D RID: 829 RVA: 0x0000E67C File Offset: 0x0000C87C
			public void Reset()
			{
				this.m_Current = new Handle
				{
					Index = -1,
					Version = -1
				};
			}

			// Token: 0x17000061 RID: 97
			// (get) Token: 0x0600033E RID: 830 RVA: 0x0000E6A8 File Offset: 0x0000C8A8
			public SerializedMemberView Current
			{
				get
				{
					if (this.m_Current.Index < 0)
					{
						throw new InvalidOperationException();
					}
					return new SerializedMemberView(this.m_Stream, this.m_Current);
				}
			}

			// Token: 0x17000062 RID: 98
			// (get) Token: 0x0600033F RID: 831 RVA: 0x0000E6CF File Offset: 0x0000C8CF
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06000340 RID: 832 RVA: 0x0000578E File Offset: 0x0000398E
			public void Dispose()
			{
			}

			// Token: 0x040001BB RID: 443
			private unsafe readonly UnsafePackedBinaryStream* m_Stream;

			// Token: 0x040001BC RID: 444
			private readonly Handle m_Start;

			// Token: 0x040001BD RID: 445
			private Handle m_Current;
		}
	}
}
