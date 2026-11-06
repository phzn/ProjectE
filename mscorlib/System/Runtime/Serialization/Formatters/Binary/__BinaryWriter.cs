using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020006AA RID: 1706
	internal sealed class __BinaryWriter
	{
		// Token: 0x06003EB6 RID: 16054 RVA: 0x000D8B38 File Offset: 0x000D6D38
		internal __BinaryWriter(Stream sout, ObjectWriter objectWriter, FormatterTypeStyle formatterTypeStyle)
		{
			this.sout = sout;
			this.formatterTypeStyle = formatterTypeStyle;
			this.objectWriter = objectWriter;
			this.m_nestedObjectCount = 0;
			this.dataWriter = new BinaryWriter(sout, Encoding.UTF8);
		}

		// Token: 0x06003EB7 RID: 16055 RVA: 0x00004BF9 File Offset: 0x00002DF9
		internal void WriteBegin()
		{
		}

		// Token: 0x06003EB8 RID: 16056 RVA: 0x000D8B78 File Offset: 0x000D6D78
		internal void WriteEnd()
		{
			this.dataWriter.Flush();
		}

		// Token: 0x06003EB9 RID: 16057 RVA: 0x000D8B85 File Offset: 0x000D6D85
		internal void WriteBoolean(bool value)
		{
			this.dataWriter.Write(value);
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x000D8B93 File Offset: 0x000D6D93
		internal void WriteByte(byte value)
		{
			this.dataWriter.Write(value);
		}

		// Token: 0x06003EBB RID: 16059 RVA: 0x000D8BA1 File Offset: 0x000D6DA1
		private void WriteBytes(byte[] value)
		{
			this.dataWriter.Write(value);
		}

		// Token: 0x06003EBC RID: 16060 RVA: 0x000D8BAF File Offset: 0x000D6DAF
		private void WriteBytes(byte[] byteA, int offset, int size)
		{
			this.dataWriter.Write(byteA, offset, size);
		}

		// Token: 0x06003EBD RID: 16061 RVA: 0x000D8BBF File Offset: 0x000D6DBF
		internal void WriteChar(char value)
		{
			this.dataWriter.Write(value);
		}

		// Token: 0x06003EBE RID: 16062 RVA: 0x000D8BCD File Offset: 0x000D6DCD
		internal void WriteChars(char[] value)
		{
			this.dataWriter.Write(value);
		}

		// Token: 0x06003EBF RID: 16063 RVA: 0x000D8BDB File Offset: 0x000D6DDB
		internal void WriteDecimal(decimal value)
		{
			this.WriteString(value.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x06003EC0 RID: 16064 RVA: 0x000D8BEF File Offset: 0x000D6DEF
		internal void WriteSingle(float value)
		{
			this.dataWriter.Write(value);
		}

		// Token: 0x06003EC1 RID: 16065 RVA: 0x000D8BFD File Offset: 0x000D6DFD
		internal void WriteDouble(double value)
		{
			this.dataWriter.Write(value);
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x000D8C0B File Offset: 0x000D6E0B
		internal void WriteInt16(short value)
		{
			this.dataWriter.Write(value);
		}

		// Token: 0x06003EC3 RID: 16067 RVA: 0x000D8C19 File Offset: 0x000D6E19
		internal void WriteInt32(int value)
		{
			this.dataWriter.Write(value);
		}

		// Token: 0x06003EC4 RID: 16068 RVA: 0x000D8C27 File Offset: 0x000D6E27
		internal void WriteInt64(long value)
		{
			this.dataWriter.Write(value);
		}

		// Token: 0x06003EC5 RID: 16069 RVA: 0x000D8C35 File Offset: 0x000D6E35
		internal void WriteSByte(sbyte value)
		{
			this.WriteByte((byte)value);
		}

		// Token: 0x06003EC6 RID: 16070 RVA: 0x000D8C3F File Offset: 0x000D6E3F
		internal void WriteString(string value)
		{
			this.dataWriter.Write(value);
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x000D8C4D File Offset: 0x000D6E4D
		internal void WriteTimeSpan(TimeSpan value)
		{
			this.WriteInt64(value.Ticks);
		}

		// Token: 0x06003EC8 RID: 16072 RVA: 0x000D8C5C File Offset: 0x000D6E5C
		internal void WriteDateTime(DateTime value)
		{
			this.WriteInt64(value.ToBinaryRaw());
		}

		// Token: 0x06003EC9 RID: 16073 RVA: 0x000D8C6B File Offset: 0x000D6E6B
		internal void WriteUInt16(ushort value)
		{
			this.dataWriter.Write(value);
		}

		// Token: 0x06003ECA RID: 16074 RVA: 0x000D8C79 File Offset: 0x000D6E79
		internal void WriteUInt32(uint value)
		{
			this.dataWriter.Write(value);
		}

		// Token: 0x06003ECB RID: 16075 RVA: 0x000D8C87 File Offset: 0x000D6E87
		internal void WriteUInt64(ulong value)
		{
			this.dataWriter.Write(value);
		}

		// Token: 0x06003ECC RID: 16076 RVA: 0x00004BF9 File Offset: 0x00002DF9
		internal void WriteObjectEnd(NameInfo memberNameInfo, NameInfo typeNameInfo)
		{
		}

		// Token: 0x06003ECD RID: 16077 RVA: 0x000D8C95 File Offset: 0x000D6E95
		internal void WriteSerializationHeaderEnd()
		{
			MessageEnd messageEnd = new MessageEnd();
			messageEnd.Dump(this.sout);
			messageEnd.Write(this);
		}

		// Token: 0x06003ECE RID: 16078 RVA: 0x000D8CAE File Offset: 0x000D6EAE
		internal void WriteSerializationHeader(int topId, int headerId, int minorVersion, int majorVersion)
		{
			SerializationHeaderRecord serializationHeaderRecord = new SerializationHeaderRecord(BinaryHeaderEnum.SerializedStreamHeader, topId, headerId, minorVersion, majorVersion);
			serializationHeaderRecord.Dump();
			serializationHeaderRecord.Write(this);
		}

		// Token: 0x06003ECF RID: 16079 RVA: 0x000D8CC7 File Offset: 0x000D6EC7
		internal void WriteMethodCall()
		{
			if (this.binaryMethodCall == null)
			{
				this.binaryMethodCall = new BinaryMethodCall();
			}
			this.binaryMethodCall.Dump();
			this.binaryMethodCall.Write(this);
		}

		// Token: 0x06003ED0 RID: 16080 RVA: 0x000D8CF4 File Offset: 0x000D6EF4
		internal object[] WriteCallArray(string uri, string methodName, string typeName, Type[] instArgs, object[] args, object methodSignature, object callContext, object[] properties)
		{
			if (this.binaryMethodCall == null)
			{
				this.binaryMethodCall = new BinaryMethodCall();
			}
			return this.binaryMethodCall.WriteArray(uri, methodName, typeName, instArgs, args, methodSignature, callContext, properties);
		}

		// Token: 0x06003ED1 RID: 16081 RVA: 0x000D8D2C File Offset: 0x000D6F2C
		internal void WriteMethodReturn()
		{
			if (this.binaryMethodReturn == null)
			{
				this.binaryMethodReturn = new BinaryMethodReturn();
			}
			this.binaryMethodReturn.Dump();
			this.binaryMethodReturn.Write(this);
		}

		// Token: 0x06003ED2 RID: 16082 RVA: 0x000D8D58 File Offset: 0x000D6F58
		internal object[] WriteReturnArray(object returnValue, object[] args, Exception exception, object callContext, object[] properties)
		{
			if (this.binaryMethodReturn == null)
			{
				this.binaryMethodReturn = new BinaryMethodReturn();
			}
			return this.binaryMethodReturn.WriteArray(returnValue, args, exception, callContext, properties);
		}

		// Token: 0x06003ED3 RID: 16083 RVA: 0x000D8D80 File Offset: 0x000D6F80
		internal void WriteObject(NameInfo nameInfo, NameInfo typeNameInfo, int numMembers, string[] memberNames, Type[] memberTypes, WriteObjectInfo[] memberObjectInfos)
		{
			this.InternalWriteItemNull();
			int num = (int)nameInfo.NIobjectId;
			string text;
			if (num < 0)
			{
				text = typeNameInfo.NIname;
			}
			else
			{
				text = nameInfo.NIname;
			}
			if (this.objectMapTable == null)
			{
				this.objectMapTable = new Hashtable();
			}
			ObjectMapInfo objectMapInfo = (ObjectMapInfo)this.objectMapTable[text];
			if (objectMapInfo != null && objectMapInfo.isCompatible(numMembers, memberNames, memberTypes))
			{
				if (this.binaryObject == null)
				{
					this.binaryObject = new BinaryObject();
				}
				this.binaryObject.Set(num, objectMapInfo.objectId);
				this.binaryObject.Write(this);
				return;
			}
			if (!typeNameInfo.NItransmitTypeOnObject)
			{
				if (this.binaryObjectWithMap == null)
				{
					this.binaryObjectWithMap = new BinaryObjectWithMap();
				}
				int num2 = (int)typeNameInfo.NIassemId;
				this.binaryObjectWithMap.Set(num, text, numMembers, memberNames, num2);
				this.binaryObjectWithMap.Dump();
				this.binaryObjectWithMap.Write(this);
				if (objectMapInfo == null)
				{
					this.objectMapTable.Add(text, new ObjectMapInfo(num, numMembers, memberNames, memberTypes));
					return;
				}
			}
			else
			{
				BinaryTypeEnum[] array = new BinaryTypeEnum[numMembers];
				object[] array2 = new object[numMembers];
				int[] array3 = new int[numMembers];
				int num2;
				for (int i = 0; i < numMembers; i++)
				{
					object obj = null;
					array[i] = BinaryConverter.GetBinaryTypeInfo(memberTypes[i], memberObjectInfos[i], null, this.objectWriter, out obj, out num2);
					array2[i] = obj;
					array3[i] = num2;
				}
				if (this.binaryObjectWithMapTyped == null)
				{
					this.binaryObjectWithMapTyped = new BinaryObjectWithMapTyped();
				}
				num2 = (int)typeNameInfo.NIassemId;
				this.binaryObjectWithMapTyped.Set(num, text, numMembers, memberNames, array, array2, array3, num2);
				this.binaryObjectWithMapTyped.Write(this);
				if (objectMapInfo == null)
				{
					this.objectMapTable.Add(text, new ObjectMapInfo(num, numMembers, memberNames, memberTypes));
				}
			}
		}

		// Token: 0x06003ED4 RID: 16084 RVA: 0x000D8F34 File Offset: 0x000D7134
		internal void WriteObjectString(int objectId, string value)
		{
			this.InternalWriteItemNull();
			if (this.binaryObjectString == null)
			{
				this.binaryObjectString = new BinaryObjectString();
			}
			this.binaryObjectString.Set(objectId, value);
			this.binaryObjectString.Write(this);
		}

		// Token: 0x06003ED5 RID: 16085 RVA: 0x000D8F68 File Offset: 0x000D7168
		[SecurityCritical]
		internal void WriteSingleArray(NameInfo memberNameInfo, NameInfo arrayNameInfo, WriteObjectInfo objectInfo, NameInfo arrayElemTypeNameInfo, int length, int lowerBound, Array array)
		{
			this.InternalWriteItemNull();
			int[] array2 = new int[] { length };
			int[] array3 = null;
			object obj = null;
			BinaryArrayTypeEnum binaryArrayTypeEnum;
			if (lowerBound == 0)
			{
				binaryArrayTypeEnum = BinaryArrayTypeEnum.Single;
			}
			else
			{
				binaryArrayTypeEnum = BinaryArrayTypeEnum.SingleOffset;
				array3 = new int[] { lowerBound };
			}
			int num;
			BinaryTypeEnum binaryTypeInfo = BinaryConverter.GetBinaryTypeInfo(arrayElemTypeNameInfo.NItype, objectInfo, arrayElemTypeNameInfo.NIname, this.objectWriter, out obj, out num);
			if (this.binaryArray == null)
			{
				this.binaryArray = new BinaryArray();
			}
			this.binaryArray.Set((int)arrayNameInfo.NIobjectId, 1, array2, array3, binaryTypeInfo, obj, binaryArrayTypeEnum, num);
			long niobjectId = arrayNameInfo.NIobjectId;
			this.binaryArray.Write(this);
			if (Converter.IsWriteAsByteArray(arrayElemTypeNameInfo.NIprimitiveTypeEnum) && lowerBound == 0)
			{
				if (arrayElemTypeNameInfo.NIprimitiveTypeEnum == InternalPrimitiveTypeE.Byte)
				{
					this.WriteBytes((byte[])array);
					return;
				}
				if (arrayElemTypeNameInfo.NIprimitiveTypeEnum == InternalPrimitiveTypeE.Char)
				{
					this.WriteChars((char[])array);
					return;
				}
				this.WriteArrayAsBytes(array, Converter.TypeLength(arrayElemTypeNameInfo.NIprimitiveTypeEnum));
			}
		}

		// Token: 0x06003ED6 RID: 16086 RVA: 0x000D905C File Offset: 0x000D725C
		[SecurityCritical]
		private void WriteArrayAsBytes(Array array, int typeLength)
		{
			this.InternalWriteItemNull();
			int i = 0;
			if (this.byteBuffer == null)
			{
				this.byteBuffer = new byte[this.chunkSize];
			}
			while (i < array.Length)
			{
				int num = Math.Min(this.chunkSize / typeLength, array.Length - i);
				int num2 = num * typeLength;
				Buffer.InternalBlockCopy(array, i * typeLength, this.byteBuffer, 0, num2);
				if (!BitConverter.IsLittleEndian)
				{
					for (int j = 0; j < num2; j += typeLength)
					{
						for (int k = 0; k < typeLength / 2; k++)
						{
							byte b = this.byteBuffer[j + k];
							this.byteBuffer[j + k] = this.byteBuffer[j + typeLength - 1 - k];
							this.byteBuffer[j + typeLength - 1 - k] = b;
						}
					}
				}
				this.WriteBytes(this.byteBuffer, 0, num2);
				i += num;
			}
		}

		// Token: 0x06003ED7 RID: 16087 RVA: 0x000D913C File Offset: 0x000D733C
		internal void WriteJaggedArray(NameInfo memberNameInfo, NameInfo arrayNameInfo, WriteObjectInfo objectInfo, NameInfo arrayElemTypeNameInfo, int length, int lowerBound)
		{
			this.InternalWriteItemNull();
			int[] array = new int[] { length };
			int[] array2 = null;
			object obj = null;
			int num = 0;
			BinaryArrayTypeEnum binaryArrayTypeEnum;
			if (lowerBound == 0)
			{
				binaryArrayTypeEnum = BinaryArrayTypeEnum.Jagged;
			}
			else
			{
				binaryArrayTypeEnum = BinaryArrayTypeEnum.JaggedOffset;
				array2 = new int[] { lowerBound };
			}
			BinaryTypeEnum binaryTypeInfo = BinaryConverter.GetBinaryTypeInfo(arrayElemTypeNameInfo.NItype, objectInfo, arrayElemTypeNameInfo.NIname, this.objectWriter, out obj, out num);
			if (this.binaryArray == null)
			{
				this.binaryArray = new BinaryArray();
			}
			this.binaryArray.Set((int)arrayNameInfo.NIobjectId, 1, array, array2, binaryTypeInfo, obj, binaryArrayTypeEnum, num);
			long niobjectId = arrayNameInfo.NIobjectId;
			this.binaryArray.Write(this);
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x000D91DC File Offset: 0x000D73DC
		internal void WriteRectangleArray(NameInfo memberNameInfo, NameInfo arrayNameInfo, WriteObjectInfo objectInfo, NameInfo arrayElemTypeNameInfo, int rank, int[] lengthA, int[] lowerBoundA)
		{
			this.InternalWriteItemNull();
			BinaryArrayTypeEnum binaryArrayTypeEnum = BinaryArrayTypeEnum.Rectangular;
			object obj = null;
			int num = 0;
			BinaryTypeEnum binaryTypeInfo = BinaryConverter.GetBinaryTypeInfo(arrayElemTypeNameInfo.NItype, objectInfo, arrayElemTypeNameInfo.NIname, this.objectWriter, out obj, out num);
			if (this.binaryArray == null)
			{
				this.binaryArray = new BinaryArray();
			}
			for (int i = 0; i < rank; i++)
			{
				if (lowerBoundA[i] != 0)
				{
					binaryArrayTypeEnum = BinaryArrayTypeEnum.RectangularOffset;
					break;
				}
			}
			this.binaryArray.Set((int)arrayNameInfo.NIobjectId, rank, lengthA, lowerBoundA, binaryTypeInfo, obj, binaryArrayTypeEnum, num);
			long niobjectId = arrayNameInfo.NIobjectId;
			this.binaryArray.Write(this);
		}

		// Token: 0x06003ED9 RID: 16089 RVA: 0x000D9275 File Offset: 0x000D7475
		[SecurityCritical]
		internal void WriteObjectByteArray(NameInfo memberNameInfo, NameInfo arrayNameInfo, WriteObjectInfo objectInfo, NameInfo arrayElemTypeNameInfo, int length, int lowerBound, byte[] byteA)
		{
			this.InternalWriteItemNull();
			this.WriteSingleArray(memberNameInfo, arrayNameInfo, objectInfo, arrayElemTypeNameInfo, length, lowerBound, byteA);
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x000D9290 File Offset: 0x000D7490
		internal void WriteMember(NameInfo memberNameInfo, NameInfo typeNameInfo, object value)
		{
			this.InternalWriteItemNull();
			InternalPrimitiveTypeE niprimitiveTypeEnum = typeNameInfo.NIprimitiveTypeEnum;
			if (memberNameInfo.NItransmitTypeOnMember)
			{
				if (this.memberPrimitiveTyped == null)
				{
					this.memberPrimitiveTyped = new MemberPrimitiveTyped();
				}
				this.memberPrimitiveTyped.Set(niprimitiveTypeEnum, value);
				bool niisArrayItem = memberNameInfo.NIisArrayItem;
				this.memberPrimitiveTyped.Dump();
				this.memberPrimitiveTyped.Write(this);
				return;
			}
			if (this.memberPrimitiveUnTyped == null)
			{
				this.memberPrimitiveUnTyped = new MemberPrimitiveUnTyped();
			}
			this.memberPrimitiveUnTyped.Set(niprimitiveTypeEnum, value);
			bool niisArrayItem2 = memberNameInfo.NIisArrayItem;
			this.memberPrimitiveUnTyped.Dump();
			this.memberPrimitiveUnTyped.Write(this);
		}

		// Token: 0x06003EDB RID: 16091 RVA: 0x000D9330 File Offset: 0x000D7530
		internal void WriteNullMember(NameInfo memberNameInfo, NameInfo typeNameInfo)
		{
			this.InternalWriteItemNull();
			if (this.objectNull == null)
			{
				this.objectNull = new ObjectNull();
			}
			if (!memberNameInfo.NIisArrayItem)
			{
				this.objectNull.SetNullCount(1);
				this.objectNull.Dump();
				this.objectNull.Write(this);
				this.nullCount = 0;
			}
		}

		// Token: 0x06003EDC RID: 16092 RVA: 0x000D9388 File Offset: 0x000D7588
		internal void WriteMemberObjectRef(NameInfo memberNameInfo, int idRef)
		{
			this.InternalWriteItemNull();
			if (this.memberReference == null)
			{
				this.memberReference = new MemberReference();
			}
			this.memberReference.Set(idRef);
			bool niisArrayItem = memberNameInfo.NIisArrayItem;
			this.memberReference.Dump();
			this.memberReference.Write(this);
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x000D93D8 File Offset: 0x000D75D8
		internal void WriteMemberNested(NameInfo memberNameInfo)
		{
			this.InternalWriteItemNull();
			bool niisArrayItem = memberNameInfo.NIisArrayItem;
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x000D93E7 File Offset: 0x000D75E7
		internal void WriteMemberString(NameInfo memberNameInfo, NameInfo typeNameInfo, string value)
		{
			this.InternalWriteItemNull();
			bool niisArrayItem = memberNameInfo.NIisArrayItem;
			this.WriteObjectString((int)typeNameInfo.NIobjectId, value);
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x000D9404 File Offset: 0x000D7604
		internal void WriteItem(NameInfo itemNameInfo, NameInfo typeNameInfo, object value)
		{
			this.InternalWriteItemNull();
			this.WriteMember(itemNameInfo, typeNameInfo, value);
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x000D9415 File Offset: 0x000D7615
		internal void WriteNullItem(NameInfo itemNameInfo, NameInfo typeNameInfo)
		{
			this.nullCount++;
			this.InternalWriteItemNull();
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x000D942B File Offset: 0x000D762B
		internal void WriteDelayedNullItem()
		{
			this.nullCount++;
		}

		// Token: 0x06003EE2 RID: 16098 RVA: 0x000D943B File Offset: 0x000D763B
		internal void WriteItemEnd()
		{
			this.InternalWriteItemNull();
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x000D9444 File Offset: 0x000D7644
		private void InternalWriteItemNull()
		{
			if (this.nullCount > 0)
			{
				if (this.objectNull == null)
				{
					this.objectNull = new ObjectNull();
				}
				this.objectNull.SetNullCount(this.nullCount);
				this.objectNull.Dump();
				this.objectNull.Write(this);
				this.nullCount = 0;
			}
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x000D949C File Offset: 0x000D769C
		internal void WriteItemObjectRef(NameInfo nameInfo, int idRef)
		{
			this.InternalWriteItemNull();
			this.WriteMemberObjectRef(nameInfo, idRef);
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x000D94AC File Offset: 0x000D76AC
		internal void WriteAssembly(Type type, string assemblyString, int assemId, bool isNew)
		{
			this.InternalWriteItemNull();
			if (assemblyString == null)
			{
				assemblyString = string.Empty;
			}
			if (isNew)
			{
				if (this.binaryAssembly == null)
				{
					this.binaryAssembly = new BinaryAssembly();
				}
				this.binaryAssembly.Set(assemId, assemblyString);
				this.binaryAssembly.Dump();
				this.binaryAssembly.Write(this);
			}
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x000D9504 File Offset: 0x000D7704
		internal void WriteValue(InternalPrimitiveTypeE code, object value)
		{
			switch (code)
			{
			case InternalPrimitiveTypeE.Boolean:
				this.WriteBoolean(Convert.ToBoolean(value, CultureInfo.InvariantCulture));
				return;
			case InternalPrimitiveTypeE.Byte:
				this.WriteByte(Convert.ToByte(value, CultureInfo.InvariantCulture));
				return;
			case InternalPrimitiveTypeE.Char:
				this.WriteChar(Convert.ToChar(value, CultureInfo.InvariantCulture));
				return;
			case InternalPrimitiveTypeE.Decimal:
				this.WriteDecimal(Convert.ToDecimal(value, CultureInfo.InvariantCulture));
				return;
			case InternalPrimitiveTypeE.Double:
				this.WriteDouble(Convert.ToDouble(value, CultureInfo.InvariantCulture));
				return;
			case InternalPrimitiveTypeE.Int16:
				this.WriteInt16(Convert.ToInt16(value, CultureInfo.InvariantCulture));
				return;
			case InternalPrimitiveTypeE.Int32:
				this.WriteInt32(Convert.ToInt32(value, CultureInfo.InvariantCulture));
				return;
			case InternalPrimitiveTypeE.Int64:
				this.WriteInt64(Convert.ToInt64(value, CultureInfo.InvariantCulture));
				return;
			case InternalPrimitiveTypeE.SByte:
				this.WriteSByte(Convert.ToSByte(value, CultureInfo.InvariantCulture));
				return;
			case InternalPrimitiveTypeE.Single:
				this.WriteSingle(Convert.ToSingle(value, CultureInfo.InvariantCulture));
				return;
			case InternalPrimitiveTypeE.TimeSpan:
				this.WriteTimeSpan((TimeSpan)value);
				return;
			case InternalPrimitiveTypeE.DateTime:
				this.WriteDateTime((DateTime)value);
				return;
			case InternalPrimitiveTypeE.UInt16:
				this.WriteUInt16(Convert.ToUInt16(value, CultureInfo.InvariantCulture));
				return;
			case InternalPrimitiveTypeE.UInt32:
				this.WriteUInt32(Convert.ToUInt32(value, CultureInfo.InvariantCulture));
				return;
			case InternalPrimitiveTypeE.UInt64:
				this.WriteUInt64(Convert.ToUInt64(value, CultureInfo.InvariantCulture));
				return;
			}
			throw new SerializationException(Environment.GetResourceString("Invalid type code in stream '{0}'.", new object[] { code.ToString() }));
		}

		// Token: 0x040028D6 RID: 10454
		internal Stream sout;

		// Token: 0x040028D7 RID: 10455
		internal FormatterTypeStyle formatterTypeStyle;

		// Token: 0x040028D8 RID: 10456
		internal Hashtable objectMapTable;

		// Token: 0x040028D9 RID: 10457
		internal ObjectWriter objectWriter;

		// Token: 0x040028DA RID: 10458
		internal BinaryWriter dataWriter;

		// Token: 0x040028DB RID: 10459
		internal int m_nestedObjectCount;

		// Token: 0x040028DC RID: 10460
		private int nullCount;

		// Token: 0x040028DD RID: 10461
		internal BinaryMethodCall binaryMethodCall;

		// Token: 0x040028DE RID: 10462
		internal BinaryMethodReturn binaryMethodReturn;

		// Token: 0x040028DF RID: 10463
		internal BinaryObject binaryObject;

		// Token: 0x040028E0 RID: 10464
		internal BinaryObjectWithMap binaryObjectWithMap;

		// Token: 0x040028E1 RID: 10465
		internal BinaryObjectWithMapTyped binaryObjectWithMapTyped;

		// Token: 0x040028E2 RID: 10466
		internal BinaryObjectString binaryObjectString;

		// Token: 0x040028E3 RID: 10467
		internal BinaryArray binaryArray;

		// Token: 0x040028E4 RID: 10468
		private byte[] byteBuffer;

		// Token: 0x040028E5 RID: 10469
		private int chunkSize = 4096;

		// Token: 0x040028E6 RID: 10470
		internal MemberPrimitiveUnTyped memberPrimitiveUnTyped;

		// Token: 0x040028E7 RID: 10471
		internal MemberPrimitiveTyped memberPrimitiveTyped;

		// Token: 0x040028E8 RID: 10472
		internal ObjectNull objectNull;

		// Token: 0x040028E9 RID: 10473
		internal MemberReference memberReference;

		// Token: 0x040028EA RID: 10474
		internal BinaryAssembly binaryAssembly;
	}
}
