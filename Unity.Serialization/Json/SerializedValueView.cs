using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Serialization.Json.Unsafe;

namespace Unity.Serialization.Json
{
	// Token: 0x0200008D RID: 141
	public readonly struct SerializedValueView : ISerializedView
	{
		// Token: 0x0600035F RID: 863 RVA: 0x0000ECEB File Offset: 0x0000CEEB
		internal unsafe SerializedValueView(UnsafePackedBinaryStream* stream, Handle handle)
		{
			this.m_Stream = stream;
			this.m_Handle = handle;
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000360 RID: 864 RVA: 0x0000ECFB File Offset: 0x0000CEFB
		public unsafe TokenType Type
		{
			get
			{
				return this.m_Stream->GetToken(this.m_Handle).Type;
			}
		}

		// Token: 0x17000065 RID: 101
		[BurstDiscard]
		public SerializedValueView this[string name]
		{
			get
			{
				return this.GetValue(in name);
			}
		}

		// Token: 0x17000066 RID: 102
		public SerializedValueView this[in FixedString32Bytes name]
		{
			get
			{
				return this.GetValue<FixedString32Bytes>(in name);
			}
		}

		// Token: 0x17000067 RID: 103
		public SerializedValueView this[in FixedString64Bytes name]
		{
			get
			{
				return this.GetValue<FixedString64Bytes>(in name);
			}
		}

		// Token: 0x17000068 RID: 104
		public SerializedValueView this[in FixedString128Bytes name]
		{
			get
			{
				return this.GetValue<FixedString128Bytes>(in name);
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000ED38 File Offset: 0x0000CF38
		public SerializedValueView GetValue(in string name)
		{
			SerializedValueView serializedValueView;
			if (!this.TryGetValue(in name, out serializedValueView))
			{
				throw new KeyNotFoundException(name);
			}
			return serializedValueView;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000ED5C File Offset: 0x0000CF5C
		public bool TryGetValue(in string name, out SerializedValueView value)
		{
			if (this.Type == TokenType.Object)
			{
				return this.AsObjectView().TryGetValue(name, out value);
			}
			value = default(SerializedValueView);
			return false;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000ED8C File Offset: 0x0000CF8C
		public SerializedValueView GetValue<[IsUnmanaged] T>(in T name) where T : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			SerializedValueView serializedValueView;
			if (!this.TryGetValue<T>(in name, out serializedValueView))
			{
				throw new KeyNotFoundException();
			}
			return serializedValueView;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000EDAC File Offset: 0x0000CFAC
		public bool TryGetValue<[IsUnmanaged] T>(in T name, out SerializedValueView value) where T : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			if (this.Type == TokenType.Object)
			{
				return this.AsObjectView().TryGetValue<T>(in name, out value);
			}
			value = default(SerializedValueView);
			return false;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000EDDC File Offset: 0x0000CFDC
		public unsafe bool IsMember()
		{
			BinaryToken token = this.m_Stream->GetToken(this.m_Handle);
			return (token.Parent == -1 || this.m_Stream->GetToken(token.Parent).Type == TokenType.Object) && (token.Type == TokenType.String || token.Type == TokenType.Primitive);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000EE34 File Offset: 0x0000D034
		public unsafe bool IsNull()
		{
			return this.m_Stream->GetToken(this.m_Handle).Type == TokenType.Primitive && this.AsPrimitiveView().IsNull();
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000EE6A File Offset: 0x0000D06A
		public SerializedArrayView AsArrayView()
		{
			this.CheckValueType(TokenType.Array);
			return new SerializedArrayView(this.m_Stream, this.m_Handle);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000EE84 File Offset: 0x0000D084
		public SerializedObjectView AsObjectView()
		{
			this.CheckValueType(TokenType.Object);
			return new SerializedObjectView(this.m_Stream, this.m_Handle);
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000EEA0 File Offset: 0x0000D0A0
		public unsafe SerializedStringView AsStringView()
		{
			BinaryToken token = this.m_Stream->GetToken(this.m_Handle);
			if (token.Type != TokenType.String && token.Type != TokenType.Primitive)
			{
				throw new InvalidOperationException(string.Format("Failed to read value RequestedType=[{0}|{1}] ActualType=[{2}]", TokenType.String, TokenType.Primitive, token.Type));
			}
			return new SerializedStringView(this.m_Stream, this.m_Handle);
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000EF09 File Offset: 0x0000D109
		public SerializedMemberView AsMemberView()
		{
			if (!this.IsMember())
			{
				throw new InvalidOperationException("Failed to read value as member");
			}
			return new SerializedMemberView(this.m_Stream, this.m_Handle);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000EF2F File Offset: 0x0000D12F
		public SerializedPrimitiveView AsPrimitiveView()
		{
			this.CheckValueType(TokenType.Primitive);
			return new SerializedPrimitiveView(this.m_Stream, this.m_Handle);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000EF4C File Offset: 0x0000D14C
		public long AsInt64()
		{
			return this.AsPrimitiveView().AsInt64();
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000EF68 File Offset: 0x0000D168
		public int AsInt32()
		{
			return (int)this.AsPrimitiveView().AsInt64();
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000EF84 File Offset: 0x0000D184
		public ulong AsUInt64()
		{
			return this.AsPrimitiveView().AsUInt64();
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000EFA0 File Offset: 0x0000D1A0
		public float AsFloat()
		{
			return this.AsPrimitiveView().AsFloat();
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000EFBC File Offset: 0x0000D1BC
		public double AsDouble()
		{
			return this.AsPrimitiveView().AsDouble();
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000EFD8 File Offset: 0x0000D1D8
		public bool AsBoolean()
		{
			return this.AsPrimitiveView().AsBoolean();
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000EFF4 File Offset: 0x0000D1F4
		private unsafe void CheckValueType(TokenType type)
		{
			BinaryToken token = this.m_Stream->GetToken(this.m_Handle);
			if (token.Type != type)
			{
				throw new InvalidOperationException(string.Format("Failed to read value RequestedType=[{0}] ActualType=[{1}]", type, token.Type));
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000F040 File Offset: 0x0000D240
		public override string ToString()
		{
			return this.AsStringView().ToString();
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000F064 File Offset: 0x0000D264
		public T AsFixedString<[IsUnmanaged] T>() where T : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			return this.AsStringView().AsFixedString<T>();
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000F080 File Offset: 0x0000D280
		public NativeText AsNativeText(Allocator allocator)
		{
			return this.AsStringView().AsNativeText(allocator);
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000F09C File Offset: 0x0000D29C
		public UnsafeText AsUnsafeText(Allocator allocator)
		{
			return this.AsStringView().AsUnsafeText(allocator);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000F0B8 File Offset: 0x0000D2B8
		internal unsafe UnsafeValueView AsUnsafe()
		{
			return new UnsafeValueView(this.m_Stream, this.m_Stream->GetTokenIndex(this.m_Handle));
		}

		// Token: 0x040001C2 RID: 450
		[NativeDisableUnsafePtrRestriction]
		internal unsafe readonly UnsafePackedBinaryStream* m_Stream;

		// Token: 0x040001C3 RID: 451
		internal readonly Handle m_Handle;
	}
}
