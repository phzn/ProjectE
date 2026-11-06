using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections.LowLevel.Unsafe.NotBurstCompatible;
using Unity.Properties;

namespace Unity.Serialization.Binary
{
	// Token: 0x020000A2 RID: 162
	public static class BinarySerialization
	{
		// Token: 0x06000403 RID: 1027 RVA: 0x00010124 File Offset: 0x0000E324
		public unsafe static T FromBinary<T>(UnsafeAppendBuffer.Reader* stream, BinarySerializationParameters parameters = default(BinarySerializationParameters))
		{
			BinarySerializationState binarySerializationState = parameters.State ?? (parameters.RequiresThreadSafety ? new BinarySerializationState() : BinarySerialization.GetSharedState());
			BinaryPropertyReader binaryPropertyReader = binarySerializationState.GetBinaryPropertyReader();
			binaryPropertyReader.SetStream(stream);
			binaryPropertyReader.SetSerializedType(parameters.SerializedType);
			binaryPropertyReader.SetDisableRootAdapters(parameters.DisableRootAdapters);
			binaryPropertyReader.SetGlobalAdapters(BinarySerialization.GetGlobalAdapters());
			binaryPropertyReader.SetUserDefinedAdapters(parameters.UserDefinedAdapters);
			binaryPropertyReader.SetSerializedReferences(parameters.DisableSerializedReferences ? null : binarySerializationState.GetSerializedReferences());
			PropertyWrapper<T> propertyWrapper = new PropertyWrapper<T>(default(T));
			using (binaryPropertyReader.Lock())
			{
				PropertyContainer.Accept<PropertyWrapper<T>>(binaryPropertyReader, ref propertyWrapper, default(VisitParameters));
			}
			if (parameters.State == null && !parameters.DisableSerializedReferences)
			{
				binarySerializationState.GetSerializedReferences().Clear();
			}
			return propertyWrapper.Value;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00010218 File Offset: 0x0000E418
		public unsafe static void ToBinary<T>(UnsafeAppendBuffer* stream, T value, BinarySerializationParameters parameters = default(BinarySerializationParameters))
		{
			PropertyWrapper<T> propertyWrapper = new PropertyWrapper<T>(value);
			BinarySerializationState binarySerializationState = parameters.State ?? (parameters.RequiresThreadSafety ? new BinarySerializationState() : BinarySerialization.GetSharedState());
			BinaryPropertyWriter binaryPropertyWriter = binarySerializationState.GetBinaryPropertyWriter();
			binaryPropertyWriter.SetStream(stream);
			binaryPropertyWriter.SetSerializedType(parameters.SerializedType);
			binaryPropertyWriter.SetDisableRootAdapters(parameters.DisableRootAdapters);
			binaryPropertyWriter.SetGlobalAdapters(BinarySerialization.GetGlobalAdapters());
			binaryPropertyWriter.SetUserDefinedAdapters(parameters.UserDefinedAdapters);
			binaryPropertyWriter.SetSerializedReferences(parameters.DisableSerializedReferences ? null : binarySerializationState.GetSerializedReferences());
			using (binaryPropertyWriter.Lock())
			{
				PropertyContainer.Accept<PropertyWrapper<T>>(binaryPropertyWriter, ref propertyWrapper, default(VisitParameters));
			}
			if (parameters.State == null && !parameters.DisableSerializedReferences)
			{
				binarySerializationState.GetSerializedReferences().Clear();
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x000102FC File Offset: 0x0000E4FC
		private static BinarySerializationState GetSharedState()
		{
			if (BinarySerialization.k_SharedState.IsLocked)
			{
				return new BinarySerializationState();
			}
			BinarySerialization.k_SharedState.ClearSerializedReferences();
			return BinarySerialization.k_SharedState;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0001031F File Offset: 0x0000E51F
		public static void AddGlobalAdapter(IBinaryAdapter adapter)
		{
			if (BinarySerialization.k_Adapters.Contains(adapter))
			{
				throw new ArgumentException("IBinaryAdapter has already been registered.");
			}
			BinarySerialization.k_Adapters.Add(adapter);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00010344 File Offset: 0x0000E544
		private static List<IBinaryAdapter> GetGlobalAdapters()
		{
			return BinarySerialization.k_Adapters;
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0001034C File Offset: 0x0000E54C
		internal unsafe static void WritePrimitiveUnsafe<TValue>(UnsafeAppendBuffer* stream, ref TValue value, Type type)
		{
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Boolean:
				stream->Add<byte>((*UnsafeUtility.As<TValue, bool>(ref value)) ? 1 : 0);
				return;
			case TypeCode.Char:
				stream->Add<char>(*UnsafeUtility.As<TValue, char>(ref value));
				return;
			case TypeCode.SByte:
				stream->Add<sbyte>(*UnsafeUtility.As<TValue, sbyte>(ref value));
				return;
			case TypeCode.Byte:
				stream->Add<byte>(*UnsafeUtility.As<TValue, byte>(ref value));
				return;
			case TypeCode.Int16:
				stream->Add<short>(*UnsafeUtility.As<TValue, short>(ref value));
				return;
			case TypeCode.UInt16:
				stream->Add<ushort>(*UnsafeUtility.As<TValue, ushort>(ref value));
				return;
			case TypeCode.Int32:
				stream->Add<int>(*UnsafeUtility.As<TValue, int>(ref value));
				return;
			case TypeCode.UInt32:
				stream->Add<uint>(*UnsafeUtility.As<TValue, uint>(ref value));
				return;
			case TypeCode.Int64:
				stream->Add<long>(*UnsafeUtility.As<TValue, long>(ref value));
				return;
			case TypeCode.UInt64:
				stream->Add<ulong>(*UnsafeUtility.As<TValue, ulong>(ref value));
				return;
			case TypeCode.Single:
				stream->Add<float>(*UnsafeUtility.As<TValue, float>(ref value));
				return;
			case TypeCode.Double:
				stream->Add<double>(*UnsafeUtility.As<TValue, double>(ref value));
				return;
			case TypeCode.String:
				(ref *stream).AddNBC(value as string);
				return;
			}
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00010478 File Offset: 0x0000E678
		internal unsafe static void WritePrimitiveBoxed(UnsafeAppendBuffer* stream, object value, Type type)
		{
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Boolean:
				stream->Add<byte>(((bool)value) ? 1 : 0);
				return;
			case TypeCode.Char:
				stream->Add<char>((char)value);
				return;
			case TypeCode.SByte:
				stream->Add<sbyte>((sbyte)value);
				return;
			case TypeCode.Byte:
				stream->Add<byte>((byte)value);
				return;
			case TypeCode.Int16:
				stream->Add<short>((short)value);
				return;
			case TypeCode.UInt16:
				stream->Add<ushort>((ushort)value);
				return;
			case TypeCode.Int32:
				stream->Add<int>((int)value);
				return;
			case TypeCode.UInt32:
				stream->Add<uint>((uint)value);
				return;
			case TypeCode.Int64:
				stream->Add<long>((long)value);
				return;
			case TypeCode.UInt64:
				stream->Add<ulong>((ulong)value);
				return;
			case TypeCode.Single:
				stream->Add<float>((float)value);
				return;
			case TypeCode.Double:
				stream->Add<double>((double)value);
				return;
			case TypeCode.String:
				(ref *stream).AddNBC(value as string);
				return;
			}
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00010590 File Offset: 0x0000E790
		internal unsafe static void ReadPrimitiveUnsafe<TValue>(UnsafeAppendBuffer.Reader* stream, ref TValue value, Type type)
		{
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Boolean:
			{
				byte b;
				stream->ReadNext<byte>(out b);
				bool flag = b == 1;
				value = *UnsafeUtility.As<bool, TValue>(ref flag);
				return;
			}
			case TypeCode.Char:
			{
				char c;
				stream->ReadNext<char>(out c);
				value = *UnsafeUtility.As<char, TValue>(ref c);
				return;
			}
			case TypeCode.SByte:
			{
				sbyte b2;
				stream->ReadNext<sbyte>(out b2);
				value = *UnsafeUtility.As<sbyte, TValue>(ref b2);
				return;
			}
			case TypeCode.Byte:
			{
				byte b3;
				stream->ReadNext<byte>(out b3);
				value = *UnsafeUtility.As<byte, TValue>(ref b3);
				return;
			}
			case TypeCode.Int16:
			{
				short num;
				stream->ReadNext<short>(out num);
				value = *UnsafeUtility.As<short, TValue>(ref num);
				return;
			}
			case TypeCode.UInt16:
			{
				ushort num2;
				stream->ReadNext<ushort>(out num2);
				value = *UnsafeUtility.As<ushort, TValue>(ref num2);
				return;
			}
			case TypeCode.Int32:
			{
				int num3;
				stream->ReadNext<int>(out num3);
				value = *UnsafeUtility.As<int, TValue>(ref num3);
				return;
			}
			case TypeCode.UInt32:
			{
				uint num4;
				stream->ReadNext<uint>(out num4);
				value = *UnsafeUtility.As<uint, TValue>(ref num4);
				return;
			}
			case TypeCode.Int64:
			{
				long num5;
				stream->ReadNext<long>(out num5);
				value = *UnsafeUtility.As<long, TValue>(ref num5);
				return;
			}
			case TypeCode.UInt64:
			{
				ulong num6;
				stream->ReadNext<ulong>(out num6);
				value = *UnsafeUtility.As<ulong, TValue>(ref num6);
				return;
			}
			case TypeCode.Single:
			{
				float num7;
				stream->ReadNext<float>(out num7);
				value = *UnsafeUtility.As<float, TValue>(ref num7);
				return;
			}
			case TypeCode.Double:
			{
				double num8;
				stream->ReadNext<double>(out num8);
				value = *UnsafeUtility.As<double, TValue>(ref num8);
				return;
			}
			case TypeCode.String:
			{
				string text;
				(ref *stream).ReadNextNBC(out text);
				value = (TValue)((object)text);
				return;
			}
			}
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0001075C File Offset: 0x0000E95C
		internal unsafe static void ReadPrimitiveBoxed<TValue>(UnsafeAppendBuffer.Reader* stream, ref TValue value, Type type)
		{
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Boolean:
			{
				bool flag = stream->ReadNext<byte>() == 1;
				TypeConversion.TryConvert<bool, TValue>(ref flag, out value);
				return;
			}
			case TypeCode.Char:
			{
				char c = stream->ReadNext<char>();
				TypeConversion.TryConvert<char, TValue>(ref c, out value);
				return;
			}
			case TypeCode.SByte:
			{
				sbyte b = stream->ReadNext<sbyte>();
				TypeConversion.TryConvert<sbyte, TValue>(ref b, out value);
				return;
			}
			case TypeCode.Byte:
			{
				byte b2 = stream->ReadNext<byte>();
				TypeConversion.TryConvert<byte, TValue>(ref b2, out value);
				return;
			}
			case TypeCode.Int16:
			{
				short num = stream->ReadNext<short>();
				TypeConversion.TryConvert<short, TValue>(ref num, out value);
				return;
			}
			case TypeCode.UInt16:
			{
				ushort num2 = stream->ReadNext<ushort>();
				TypeConversion.TryConvert<ushort, TValue>(ref num2, out value);
				return;
			}
			case TypeCode.Int32:
			{
				int num3 = stream->ReadNext<int>();
				TypeConversion.TryConvert<int, TValue>(ref num3, out value);
				return;
			}
			case TypeCode.UInt32:
			{
				uint num4 = stream->ReadNext<uint>();
				TypeConversion.TryConvert<uint, TValue>(ref num4, out value);
				return;
			}
			case TypeCode.Int64:
			{
				long num5 = stream->ReadNext<long>();
				TypeConversion.TryConvert<long, TValue>(ref num5, out value);
				return;
			}
			case TypeCode.UInt64:
			{
				ulong num6 = stream->ReadNext<ulong>();
				TypeConversion.TryConvert<ulong, TValue>(ref num6, out value);
				return;
			}
			case TypeCode.Single:
			{
				float num7 = stream->ReadNext<float>();
				TypeConversion.TryConvert<float, TValue>(ref num7, out value);
				return;
			}
			case TypeCode.Double:
			{
				double num8 = stream->ReadNext<double>();
				TypeConversion.TryConvert<double, TValue>(ref num8, out value);
				return;
			}
			case TypeCode.String:
			{
				string text;
				(ref *stream).ReadNextNBC(out text);
				TypeConversion.TryConvert<string, TValue>(ref text, out value);
				return;
			}
			}
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x040001EF RID: 495
		private static readonly List<IBinaryAdapter> k_Adapters = new List<IBinaryAdapter>();

		// Token: 0x040001F0 RID: 496
		private static readonly BinarySerializationState k_SharedState = new BinarySerializationState();
	}
}
