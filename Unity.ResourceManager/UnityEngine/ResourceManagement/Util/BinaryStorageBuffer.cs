using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x02000013 RID: 19
	internal class BinaryStorageBuffer
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x000041B9 File Offset: 0x000023B9
		private unsafe static void ComputeHash(void* pData, ulong size, Hash128* hash)
		{
			if (pData == null || size == 0UL)
			{
				*hash = default(Hash128);
				return;
			}
			HashUnsafeUtilities.ComputeHash128(pData, size, hash);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000041D4 File Offset: 0x000023D4
		private static void AddSerializationAdapter(Dictionary<Type, BinaryStorageBuffer.ISerializationAdapter> serializationAdapters, BinaryStorageBuffer.ISerializationAdapter adapter, bool forceOverride = false)
		{
			bool flag = false;
			foreach (Type type in adapter.GetType().GetInterfaces())
			{
				if (type.IsGenericType && typeof(BinaryStorageBuffer.ISerializationAdapter).IsAssignableFrom(type))
				{
					Type type2 = type.GenericTypeArguments[0];
					if (serializationAdapters.ContainsKey(type2))
					{
						if (forceOverride)
						{
							BinaryStorageBuffer.ISerializationAdapter serializationAdapter = serializationAdapters[type2];
							serializationAdapters.Remove(type2);
							serializationAdapters[type2] = adapter;
							flag = true;
							Debug.Log(string.Format("Replacing adapter for type {0}: {1} -> {2}", type2, serializationAdapter, adapter));
						}
						else
						{
							Debug.Log(string.Format("Failed to register adapter for type {0}: {1}, {2} is already registered.", type2, adapter, serializationAdapters[type2]));
						}
					}
					else
					{
						serializationAdapters[type2] = adapter;
						flag = true;
					}
				}
			}
			if (flag)
			{
				IEnumerable<BinaryStorageBuffer.ISerializationAdapter> dependencies = adapter.Dependencies;
				if (dependencies != null)
				{
					foreach (BinaryStorageBuffer.ISerializationAdapter serializationAdapter2 in dependencies)
					{
						BinaryStorageBuffer.AddSerializationAdapter(serializationAdapters, serializationAdapter2, false);
					}
				}
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000042EC File Offset: 0x000024EC
		private static bool GetSerializationAdapter(Dictionary<Type, BinaryStorageBuffer.ISerializationAdapter> serializationAdapters, Type t, out BinaryStorageBuffer.ISerializationAdapter adapter)
		{
			if (!serializationAdapters.TryGetValue(t, out adapter))
			{
				foreach (KeyValuePair<Type, BinaryStorageBuffer.ISerializationAdapter> keyValuePair in serializationAdapters)
				{
					if (keyValuePair.Key.IsAssignableFrom(t))
					{
						BinaryStorageBuffer.ISerializationAdapter value;
						adapter = (value = keyValuePair.Value);
						return value != null;
					}
				}
				Debug.LogError(string.Format("Unable to find serialization adapter for type {0}.", t));
			}
			return adapter != null;
		}

		// Token: 0x0400003E RID: 62
		private const uint kUnicodeStringFlag = 2147483648U;

		// Token: 0x0400003F RID: 63
		private const uint kDynamicStringFlag = 1073741824U;

		// Token: 0x04000040 RID: 64
		private const uint kClearFlagsMask = 1073741823U;

		// Token: 0x02000069 RID: 105
		private class BuiltinTypesSerializer : BinaryStorageBuffer.ISerializationAdapter<int>, BinaryStorageBuffer.ISerializationAdapter, BinaryStorageBuffer.ISerializationAdapter<bool>, BinaryStorageBuffer.ISerializationAdapter<long>, BinaryStorageBuffer.ISerializationAdapter<string>, BinaryStorageBuffer.ISerializationAdapter<Hash128>
		{
			// Token: 0x170000D1 RID: 209
			// (get) Token: 0x06000312 RID: 786 RVA: 0x0000A400 File Offset: 0x00008600
			public IEnumerable<BinaryStorageBuffer.ISerializationAdapter> Dependencies
			{
				get
				{
					return null;
				}
			}

			// Token: 0x06000313 RID: 787 RVA: 0x0000A404 File Offset: 0x00008604
			public object Deserialize(BinaryStorageBuffer.Reader reader, Type t, uint offset)
			{
				if (offset == 4294967295U)
				{
					return null;
				}
				if (t == typeof(int))
				{
					return reader.ReadValue<int>(offset);
				}
				if (t == typeof(bool))
				{
					return reader.ReadValue<bool>(offset);
				}
				if (t == typeof(long))
				{
					return reader.ReadValue<long>(offset);
				}
				if (t == typeof(Hash128))
				{
					return reader.ReadValue<Hash128>(offset);
				}
				if (t == typeof(string))
				{
					BinaryStorageBuffer.BuiltinTypesSerializer.ObjectToStringRemap objectToStringRemap = reader.ReadValue<BinaryStorageBuffer.BuiltinTypesSerializer.ObjectToStringRemap>(offset);
					return reader.ReadString(objectToStringRemap.stringId, objectToStringRemap.separator, false);
				}
				return null;
			}

			// Token: 0x06000314 RID: 788 RVA: 0x0000A4C4 File Offset: 0x000086C4
			private char FindBestSeparator(string str, params char[] seps)
			{
				int num = 0;
				char c2 = '\0';
				for (int i = 0; i < seps.Length; i++)
				{
					char s = seps[i];
					int num2 = str.Count<char>((char c) => c == s);
					if (num2 > num)
					{
						num = num2;
						c2 = s;
					}
				}
				string[] array = str.Split(c2, StringSplitOptions.None);
				int num3 = 0;
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					if (array2[i].Length > 8)
					{
						num3++;
					}
				}
				if (num3 < 2)
				{
					c2 = '\0';
				}
				return c2;
			}

			// Token: 0x06000315 RID: 789 RVA: 0x0000A558 File Offset: 0x00008758
			public uint Serialize(BinaryStorageBuffer.Writer writer, object val)
			{
				if (val == null)
				{
					return uint.MaxValue;
				}
				Type type = val.GetType();
				if (type == typeof(int))
				{
					return writer.Write<int>((int)val);
				}
				if (type == typeof(bool))
				{
					return writer.Write<bool>((bool)val);
				}
				if (type == typeof(long))
				{
					return writer.Write<long>((long)val);
				}
				if (type == typeof(Hash128))
				{
					return writer.Write<Hash128>((Hash128)val);
				}
				if (type == typeof(string))
				{
					string text = val as string;
					char c = this.FindBestSeparator(text, new char[] { '/', '.', '-', '_', '\\', ',' });
					return writer.Write<BinaryStorageBuffer.BuiltinTypesSerializer.ObjectToStringRemap>(new BinaryStorageBuffer.BuiltinTypesSerializer.ObjectToStringRemap
					{
						stringId = writer.WriteString((string)val, '\0'),
						separator = c
					});
				}
				return uint.MaxValue;
			}

			// Token: 0x02000087 RID: 135
			private struct ObjectToStringRemap
			{
				// Token: 0x04000192 RID: 402
				public uint stringId;

				// Token: 0x04000193 RID: 403
				public char separator;
			}
		}

		// Token: 0x0200006A RID: 106
		private class TypeSerializer : BinaryStorageBuffer.ISerializationAdapter<Type>, BinaryStorageBuffer.ISerializationAdapter
		{
			// Token: 0x170000D2 RID: 210
			// (get) Token: 0x06000317 RID: 791 RVA: 0x0000A654 File Offset: 0x00008854
			public IEnumerable<BinaryStorageBuffer.ISerializationAdapter> Dependencies
			{
				get
				{
					return null;
				}
			}

			// Token: 0x06000318 RID: 792 RVA: 0x0000A658 File Offset: 0x00008858
			public object Deserialize(BinaryStorageBuffer.Reader reader, Type type, uint offset)
			{
				object obj;
				try
				{
					BinaryStorageBuffer.TypeSerializer.Data data = reader.ReadValue<BinaryStorageBuffer.TypeSerializer.Data>(offset);
					string text = reader.ReadString(data.assemblyId, '.', true);
					string text2 = reader.ReadString(data.classId, '.', true);
					Assembly assembly = Assembly.Load(text);
					obj = ((assembly == null) ? null : assembly.GetType(text2));
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
					obj = null;
				}
				return obj;
			}

			// Token: 0x06000319 RID: 793 RVA: 0x0000A6C4 File Offset: 0x000088C4
			public uint Serialize(BinaryStorageBuffer.Writer writer, object val)
			{
				if (val == null)
				{
					return uint.MaxValue;
				}
				Type type = val as Type;
				return writer.Write<BinaryStorageBuffer.TypeSerializer.Data>(new BinaryStorageBuffer.TypeSerializer.Data
				{
					assemblyId = writer.WriteString(type.Assembly.FullName, '.'),
					classId = writer.WriteString(type.FullName, '.')
				});
			}

			// Token: 0x02000089 RID: 137
			private struct Data
			{
				// Token: 0x04000195 RID: 405
				public uint assemblyId;

				// Token: 0x04000196 RID: 406
				public uint classId;
			}
		}

		// Token: 0x0200006B RID: 107
		private struct DynamicString
		{
			// Token: 0x04000142 RID: 322
			public uint stringId;

			// Token: 0x04000143 RID: 323
			public uint nextId;
		}

		// Token: 0x0200006C RID: 108
		private struct ObjectTypeData
		{
			// Token: 0x04000144 RID: 324
			public uint typeId;

			// Token: 0x04000145 RID: 325
			public uint objectId;
		}

		// Token: 0x0200006D RID: 109
		public interface ISerializationAdapter
		{
			// Token: 0x170000D3 RID: 211
			// (get) Token: 0x0600031B RID: 795
			IEnumerable<BinaryStorageBuffer.ISerializationAdapter> Dependencies { get; }

			// Token: 0x0600031C RID: 796
			uint Serialize(BinaryStorageBuffer.Writer writer, object val);

			// Token: 0x0600031D RID: 797
			object Deserialize(BinaryStorageBuffer.Reader reader, Type t, uint offset);
		}

		// Token: 0x0200006E RID: 110
		public interface ISerializationAdapter<T> : BinaryStorageBuffer.ISerializationAdapter
		{
		}

		// Token: 0x0200006F RID: 111
		public class Reader
		{
			// Token: 0x0600031E RID: 798 RVA: 0x0000A724 File Offset: 0x00008924
			private void Init(byte[] data, int maxCachedObjects, params BinaryStorageBuffer.ISerializationAdapter[] adapters)
			{
				this.m_Buffer = data;
				this.stringBuilder = new StringBuilder(1024);
				this.m_Cache = new LRUCache<uint, object>(maxCachedObjects);
				this.m_Adapters = new Dictionary<Type, BinaryStorageBuffer.ISerializationAdapter>();
				foreach (BinaryStorageBuffer.ISerializationAdapter serializationAdapter in adapters)
				{
					BinaryStorageBuffer.AddSerializationAdapter(this.m_Adapters, serializationAdapter, false);
				}
				BinaryStorageBuffer.AddSerializationAdapter(this.m_Adapters, new BinaryStorageBuffer.TypeSerializer(), false);
				BinaryStorageBuffer.AddSerializationAdapter(this.m_Adapters, new BinaryStorageBuffer.BuiltinTypesSerializer(), false);
			}

			// Token: 0x0600031F RID: 799 RVA: 0x0000A7A2 File Offset: 0x000089A2
			public void AddSerializationAdapter(BinaryStorageBuffer.ISerializationAdapter a)
			{
				BinaryStorageBuffer.AddSerializationAdapter(this.m_Adapters, a, false);
			}

			// Token: 0x06000320 RID: 800 RVA: 0x0000A7B1 File Offset: 0x000089B1
			public Reader(byte[] data, int maxCachedObjects = 1024, params BinaryStorageBuffer.ISerializationAdapter[] adapters)
			{
				this.Init(data, maxCachedObjects, adapters);
			}

			// Token: 0x06000321 RID: 801 RVA: 0x0000A7C2 File Offset: 0x000089C2
			internal byte[] GetBuffer()
			{
				return this.m_Buffer;
			}

			// Token: 0x06000322 RID: 802 RVA: 0x0000A7CC File Offset: 0x000089CC
			public Reader(Stream inputStream, uint bufferSize, int maxCachedObjects, params BinaryStorageBuffer.ISerializationAdapter[] adapters)
			{
				byte[] array = new byte[(bufferSize == 0U) ? inputStream.Length : ((long)((ulong)bufferSize))];
				inputStream.Read(array, 0, array.Length);
				this.Init(array, maxCachedObjects, adapters);
			}

			// Token: 0x06000323 RID: 803 RVA: 0x0000A80C File Offset: 0x00008A0C
			private bool TryGetCachedValue<T>(uint offset, out T val)
			{
				object obj;
				if (this.m_Cache.TryGet(offset, out obj))
				{
					val = (T)((object)obj);
					return true;
				}
				val = default(T);
				return false;
			}

			// Token: 0x06000324 RID: 804 RVA: 0x0000A840 File Offset: 0x00008A40
			public object[] ReadObjectArray(uint id, bool cacheValues = true)
			{
				if (id == 4294967295U)
				{
					return null;
				}
				uint[] array = this.ReadValueArray<uint>(id, cacheValues);
				object[] array2 = new object[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = this.ReadObject(array[i], cacheValues);
				}
				return array2;
			}

			// Token: 0x06000325 RID: 805 RVA: 0x0000A884 File Offset: 0x00008A84
			public object[] ReadObjectArray(Type t, uint id, bool cacheValues = true)
			{
				if (id == 4294967295U)
				{
					return null;
				}
				uint[] array = this.ReadValueArray<uint>(id, cacheValues);
				object[] array2 = new object[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = this.ReadObject(t, array[i], cacheValues);
				}
				return array2;
			}

			// Token: 0x06000326 RID: 806 RVA: 0x0000A8C8 File Offset: 0x00008AC8
			public T[] ReadObjectArray<T>(uint id, bool cacheValues = true)
			{
				if (id == 4294967295U)
				{
					return null;
				}
				uint[] array = this.ReadValueArray<uint>(id, cacheValues);
				T[] array2 = new T[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = this.ReadObject<T>(array[i], cacheValues);
				}
				return array2;
			}

			// Token: 0x06000327 RID: 807 RVA: 0x0000A910 File Offset: 0x00008B10
			public object ReadObject(uint id, bool cacheValue = true)
			{
				if (id == 4294967295U)
				{
					return null;
				}
				BinaryStorageBuffer.ObjectTypeData objectTypeData = this.ReadValue<BinaryStorageBuffer.ObjectTypeData>(id);
				Type type = this.ReadObject<Type>(objectTypeData.typeId, true);
				return this.ReadObject(type, objectTypeData.objectId, cacheValue);
			}

			// Token: 0x06000328 RID: 808 RVA: 0x0000A947 File Offset: 0x00008B47
			public T ReadObject<T>(uint offset, bool cacheValue = true)
			{
				return (T)((object)this.ReadObject(typeof(T), offset, cacheValue));
			}

			// Token: 0x06000329 RID: 809 RVA: 0x0000A960 File Offset: 0x00008B60
			public object ReadObject(Type t, uint id, bool cacheValue = true)
			{
				if (id == 4294967295U)
				{
					return null;
				}
				object obj;
				if (this.TryGetCachedValue<object>(id, out obj))
				{
					return obj;
				}
				BinaryStorageBuffer.ISerializationAdapter serializationAdapter;
				if (!BinaryStorageBuffer.GetSerializationAdapter(this.m_Adapters, t, out serializationAdapter))
				{
					return null;
				}
				object obj2 = null;
				try
				{
					obj2 = serializationAdapter.Deserialize(this, t, id);
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
					return null;
				}
				if (cacheValue && obj2 != null)
				{
					this.m_Cache.TryAdd(id, obj2);
				}
				return obj2;
			}

			// Token: 0x0600032A RID: 810 RVA: 0x0000A9D0 File Offset: 0x00008BD0
			public unsafe T[] ReadValueArray<[IsUnmanaged] T>(uint id, bool cacheValue = true) where T : struct, ValueType
			{
				if (id == 4294967295U)
				{
					return null;
				}
				if ((ulong)(id - 4U) >= (ulong)((long)this.m_Buffer.Length))
				{
					throw new Exception(string.Format("Data offset {0} is out of bounds of buffer with length of {1}.", id, this.m_Buffer.Length));
				}
				fixed (byte* ptr = &this.m_Buffer[(int)(id - 4U)])
				{
					byte* ptr2 = ptr;
					T[] array;
					if (this.TryGetCachedValue<T[]>(id, out array))
					{
						return array;
					}
					uint num = 0U;
					UnsafeUtility.MemCpy((void*)(&num), (void*)ptr2, 4L);
					if ((ulong)(id + num) > (ulong)((long)this.m_Buffer.Length))
					{
						throw new Exception(string.Format("Data size {0} is out of bounds of buffer with length of {1}.", num, this.m_Buffer.Length));
					}
					T[] array2 = new T[(ulong)num / (ulong)((long)sizeof(T))];
					T[] array3;
					T* ptr3;
					if ((array3 = array2) == null || array3.Length == 0)
					{
						ptr3 = null;
					}
					else
					{
						ptr3 = &array3[0];
					}
					UnsafeUtility.MemCpy((void*)ptr3, (void*)(ptr2 + 4), (long)((ulong)num));
					array3 = null;
					if (cacheValue)
					{
						this.m_Cache.TryAdd(id, array2);
					}
					return array2;
				}
			}

			// Token: 0x0600032B RID: 811 RVA: 0x0000AAC8 File Offset: 0x00008CC8
			public unsafe T ReadValue<[IsUnmanaged] T>(uint id) where T : struct, ValueType
			{
				if (id == 4294967295U)
				{
					return default(T);
				}
				if ((ulong)id >= (ulong)((long)this.m_Buffer.Length))
				{
					throw new Exception(string.Format("Data offset {0} is out of bounds of buffer with length of {1}.", id, this.m_Buffer.Length));
				}
				byte[] buffer;
				byte* ptr;
				if ((buffer = this.m_Buffer) == null || buffer.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &buffer[0];
				}
				T t;
				UnsafeUtility.MemCpy((void*)(&t), (void*)(ptr + id), (long)sizeof(T));
				return t;
			}

			// Token: 0x0600032C RID: 812 RVA: 0x0000AB44 File Offset: 0x00008D44
			public string ReadString(uint id, char sep = '\0', bool cacheValue = true)
			{
				if (id == 4294967295U)
				{
					return null;
				}
				if (sep == '\0')
				{
					return this.ReadAutoEncodedString(id, cacheValue);
				}
				return this.ReadDynamicString(id, sep, cacheValue);
			}

			// Token: 0x0600032D RID: 813 RVA: 0x0000AB64 File Offset: 0x00008D64
			private unsafe string ReadStringInternal(uint offset, Encoding enc, bool cacheValue = true)
			{
				if ((ulong)(offset - 4U) >= (ulong)((long)this.m_Buffer.Length))
				{
					throw new Exception(string.Format("Data offset {0} is out of bounds of buffer with length of {1}.", offset, this.m_Buffer.Length));
				}
				string text;
				if (this.TryGetCachedValue<string>(offset, out text))
				{
					return text;
				}
				byte[] buffer;
				byte* ptr;
				if ((buffer = this.m_Buffer) == null || buffer.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &buffer[0];
				}
				uint num = *(uint*)(ptr + (offset - 4U));
				if ((ulong)(offset + num) > (ulong)((long)this.m_Buffer.Length))
				{
					throw new Exception(string.Format("Data offset {0}, len {1} is out of bounds of buffer with length of {2}.", offset, num, this.m_Buffer.Length));
				}
				string @string = enc.GetString(ptr + offset, (int)num);
				if (cacheValue)
				{
					this.m_Cache.TryAdd(offset, @string);
				}
				return @string;
			}

			// Token: 0x0600032E RID: 814 RVA: 0x0000AC30 File Offset: 0x00008E30
			private string ReadAutoEncodedString(uint id, bool cacheValue)
			{
				if ((id & 2147483648U) == 2147483648U)
				{
					return this.ReadStringInternal(id & 1073741823U, Encoding.Unicode, cacheValue);
				}
				return this.ReadStringInternal(id, Encoding.ASCII, cacheValue);
			}

			// Token: 0x0600032F RID: 815 RVA: 0x0000AC64 File Offset: 0x00008E64
			private string ReadDynamicString(uint id, char sep, bool cacheValue)
			{
				if ((id & 1073741824U) == 1073741824U)
				{
					string text;
					if (!this.TryGetCachedValue<string>(id, out text))
					{
						BinaryStorageBuffer.DynamicString dynamicString = this.ReadValue<BinaryStorageBuffer.DynamicString>(id & 1073741823U);
						this.stringBuilder.Append(this.ReadAutoEncodedString(dynamicString.stringId, cacheValue));
						while (dynamicString.nextId != 4294967295U)
						{
							dynamicString = this.ReadValue<BinaryStorageBuffer.DynamicString>(dynamicString.nextId);
							this.stringBuilder.Append(sep);
							this.stringBuilder.Append(this.ReadAutoEncodedString(dynamicString.stringId, cacheValue));
						}
						text = this.stringBuilder.ToString();
						this.stringBuilder.Clear();
						if (cacheValue)
						{
							this.m_Cache.TryAdd(id, text);
						}
					}
					return text;
				}
				return this.ReadAutoEncodedString(id, cacheValue);
			}

			// Token: 0x04000146 RID: 326
			private byte[] m_Buffer;

			// Token: 0x04000147 RID: 327
			private Dictionary<Type, BinaryStorageBuffer.ISerializationAdapter> m_Adapters;

			// Token: 0x04000148 RID: 328
			private LRUCache<uint, object> m_Cache;

			// Token: 0x04000149 RID: 329
			private StringBuilder stringBuilder;
		}

		// Token: 0x02000070 RID: 112
		public class Writer
		{
			// Token: 0x170000D4 RID: 212
			// (get) Token: 0x06000330 RID: 816 RVA: 0x0000AD28 File Offset: 0x00008F28
			public uint Length
			{
				get
				{
					return this.totalBytes;
				}
			}

			// Token: 0x06000331 RID: 817 RVA: 0x0000AD30 File Offset: 0x00008F30
			public Writer(int chunkSize = 1048576, params BinaryStorageBuffer.ISerializationAdapter[] adapters)
			{
				this.defaulChunkSize = (uint)((chunkSize > 0) ? chunkSize : 1048576);
				this.existingValues = new Dictionary<Hash128, uint>();
				this.chunks = new List<BinaryStorageBuffer.Writer.Chunk>(10);
				this.chunks.Add(new BinaryStorageBuffer.Writer.Chunk
				{
					position = 0U
				});
				this.serializationAdapters = new Dictionary<Type, BinaryStorageBuffer.ISerializationAdapter>();
				BinaryStorageBuffer.AddSerializationAdapter(this.serializationAdapters, new BinaryStorageBuffer.TypeSerializer(), false);
				BinaryStorageBuffer.AddSerializationAdapter(this.serializationAdapters, new BinaryStorageBuffer.BuiltinTypesSerializer(), false);
				foreach (BinaryStorageBuffer.ISerializationAdapter serializationAdapter in adapters)
				{
					BinaryStorageBuffer.AddSerializationAdapter(this.serializationAdapters, serializationAdapter, true);
				}
			}

			// Token: 0x06000332 RID: 818 RVA: 0x0000ADD4 File Offset: 0x00008FD4
			private BinaryStorageBuffer.Writer.Chunk FindChunkWithSpace(uint length)
			{
				BinaryStorageBuffer.Writer.Chunk chunk = this.chunks[this.chunks.Count - 1];
				if (chunk.data == null)
				{
					chunk.data = new byte[(length > this.defaulChunkSize) ? length : this.defaulChunkSize];
				}
				if ((ulong)length > (ulong)((long)chunk.data.Length - (long)((ulong)chunk.position)))
				{
					chunk = new BinaryStorageBuffer.Writer.Chunk
					{
						position = 0U,
						data = new byte[(length > this.defaulChunkSize) ? length : this.defaulChunkSize]
					};
					this.chunks.Add(chunk);
				}
				return chunk;
			}

			// Token: 0x06000333 RID: 819 RVA: 0x0000AE6C File Offset: 0x0000906C
			private unsafe uint WriteInternal(void* pData, uint dataSize, bool prefixSize)
			{
				Hash128 hash;
				BinaryStorageBuffer.ComputeHash(pData, (ulong)dataSize, &hash);
				uint num;
				if (this.existingValues.TryGetValue(hash, out num))
				{
					return num;
				}
				uint num2 = (prefixSize ? (dataSize + 4U) : dataSize);
				BinaryStorageBuffer.Writer.Chunk chunk = this.FindChunkWithSpace(num2);
				fixed (byte* ptr = &chunk.data[(int)chunk.position])
				{
					byte* ptr2 = ptr;
					uint num3 = this.totalBytes;
					if (prefixSize)
					{
						UnsafeUtility.MemCpy((void*)ptr2, (void*)(&dataSize), 4L);
						if (dataSize > 0U)
						{
							UnsafeUtility.MemCpy((void*)(ptr2 + 4), pData, (long)((ulong)dataSize));
						}
						num3 += 4U;
					}
					else
					{
						if (dataSize == 0U)
						{
							return uint.MaxValue;
						}
						UnsafeUtility.MemCpy((void*)ptr2, pData, (long)((ulong)dataSize));
					}
					this.totalBytes += num2;
					chunk.position += num2;
					this.existingValues[hash] = num3;
					return num3;
				}
			}

			// Token: 0x06000334 RID: 820 RVA: 0x0000AF2C File Offset: 0x0000912C
			private uint ReserveInternal(uint dataSize, bool prefixSize)
			{
				uint num = (prefixSize ? (dataSize + 4U) : dataSize);
				BinaryStorageBuffer.Writer.Chunk chunk = this.FindChunkWithSpace(num);
				this.totalBytes += num;
				chunk.position += num;
				return this.totalBytes - dataSize;
			}

			// Token: 0x06000335 RID: 821 RVA: 0x0000AF70 File Offset: 0x00009170
			private unsafe void WriteInternal(uint id, void* pData, uint dataSize, bool prefixSize)
			{
				Hash128 hash;
				BinaryStorageBuffer.ComputeHash(pData, (ulong)dataSize, &hash);
				this.existingValues[hash] = id;
				uint num = id;
				foreach (BinaryStorageBuffer.Writer.Chunk chunk in this.chunks)
				{
					if (num < chunk.position)
					{
						try
						{
							byte[] array;
							byte* ptr;
							if ((array = chunk.data) == null || array.Length == 0)
							{
								ptr = null;
							}
							else
							{
								ptr = &array[0];
							}
							if (prefixSize)
							{
								UnsafeUtility.MemCpy((void*)(ptr + (num - 4U)), (void*)(&dataSize), 4L);
							}
							UnsafeUtility.MemCpy((void*)(ptr + num), pData, (long)((ulong)dataSize));
							break;
						}
						finally
						{
							byte[] array = null;
						}
					}
					num -= chunk.position;
				}
			}

			// Token: 0x06000336 RID: 822 RVA: 0x0000B044 File Offset: 0x00009244
			public uint Reserve<[IsUnmanaged] T>() where T : struct, ValueType
			{
				return this.ReserveInternal((uint)sizeof(T), false);
			}

			// Token: 0x06000337 RID: 823 RVA: 0x0000B054 File Offset: 0x00009254
			public unsafe uint Write<[IsUnmanaged] T>(in T val) where T : struct, ValueType
			{
				fixed (T* ptr = &val)
				{
					T* ptr2 = ptr;
					return this.WriteInternal((void*)ptr2, (uint)sizeof(T), false);
				}
			}

			// Token: 0x06000338 RID: 824 RVA: 0x0000B074 File Offset: 0x00009274
			public unsafe uint Write<[IsUnmanaged] T>(T val) where T : struct, ValueType
			{
				return this.WriteInternal((void*)(&val), (uint)sizeof(T), false);
			}

			// Token: 0x06000339 RID: 825 RVA: 0x0000B088 File Offset: 0x00009288
			public unsafe uint Write<[IsUnmanaged] T>(uint offset, in T val) where T : struct, ValueType
			{
				fixed (T* ptr = &val)
				{
					T* ptr2 = ptr;
					this.WriteInternal(offset, (void*)ptr2, (uint)sizeof(T), false);
				}
				return offset;
			}

			// Token: 0x0600033A RID: 826 RVA: 0x0000B0AD File Offset: 0x000092AD
			public unsafe uint Write<[IsUnmanaged] T>(uint offset, T val) where T : struct, ValueType
			{
				this.WriteInternal(offset, (void*)(&val), (uint)sizeof(T), false);
				return offset;
			}

			// Token: 0x0600033B RID: 827 RVA: 0x0000B0C1 File Offset: 0x000092C1
			public uint Reserve<[IsUnmanaged] T>(uint count) where T : struct, ValueType
			{
				return this.ReserveInternal((uint)(sizeof(T) * (int)count), true);
			}

			// Token: 0x0600033C RID: 828 RVA: 0x0000B0D4 File Offset: 0x000092D4
			public unsafe uint Write<[IsUnmanaged] T>(T[] values, bool hashElements = true) where T : struct, ValueType
			{
				T* ptr;
				if (values == null || values.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &values[0];
				}
				uint num = (uint)(values.Length * sizeof(T));
				Hash128 hash;
				BinaryStorageBuffer.ComputeHash((void*)ptr, (ulong)num, &hash);
				uint num2;
				if (this.existingValues.TryGetValue(hash, out num2))
				{
					return num2;
				}
				BinaryStorageBuffer.Writer.Chunk chunk = this.FindChunkWithSpace(num + 4U);
				fixed (byte* ptr2 = &chunk.data[(int)chunk.position])
				{
					byte* ptr3 = ptr2;
					uint num3 = this.totalBytes + 4U;
					UnsafeUtility.MemCpy((void*)ptr3, (void*)(&num), 4L);
					UnsafeUtility.MemCpy((void*)(ptr3 + 4), (void*)ptr, (long)((ulong)num));
					uint num4 = num + 4U;
					this.totalBytes += num4;
					chunk.position += num4;
					this.existingValues[hash] = num3;
					if (hashElements && sizeof(T) > 4)
					{
						for (int i = 0; i < values.Length; i++)
						{
							hash = default(Hash128);
							BinaryStorageBuffer.ComputeHash((void*)(ptr + (IntPtr)i * (IntPtr)sizeof(T) / (IntPtr)sizeof(T)), (ulong)((long)sizeof(T)), &hash);
							this.existingValues[hash] = num3 + (uint)(i * sizeof(T));
						}
					}
					return num3;
				}
			}

			// Token: 0x0600033D RID: 829 RVA: 0x0000B1F4 File Offset: 0x000093F4
			public unsafe uint Write<[IsUnmanaged] T>(uint offset, T[] values, bool hashElements = true) where T : struct, ValueType
			{
				uint num = (uint)(values.Length * sizeof(T));
				uint num2 = offset;
				fixed (T[] array = values)
				{
					T* ptr;
					if (values == null || array.Length == 0)
					{
						ptr = null;
					}
					else
					{
						ptr = &array[0];
					}
					foreach (BinaryStorageBuffer.Writer.Chunk chunk in this.chunks)
					{
						if (num2 < chunk.position)
						{
							try
							{
								byte[] array2;
								byte* ptr2;
								if ((array2 = chunk.data) == null || array2.Length == 0)
								{
									ptr2 = null;
								}
								else
								{
									ptr2 = &array2[0];
								}
								UnsafeUtility.MemCpy((void*)(ptr2 + (num2 - 4U)), (void*)(&num), 4L);
								UnsafeUtility.MemCpy((void*)(ptr2 + num2), (void*)ptr, (long)((ulong)num));
								if (hashElements && sizeof(T) > 4)
								{
									for (int i = 0; i < values.Length; i++)
									{
										T t = values[i];
										Hash128 hash;
										BinaryStorageBuffer.ComputeHash((void*)(&t), (ulong)((long)sizeof(T)), &hash);
										this.existingValues[hash] = offset + (uint)(i * sizeof(T));
									}
								}
								return offset;
							}
							finally
							{
								byte[] array2 = null;
							}
						}
						num2 -= chunk.position;
					}
				}
				return uint.MaxValue;
			}

			// Token: 0x0600033E RID: 830 RVA: 0x0000B334 File Offset: 0x00009534
			public uint WriteObjects<T>(IEnumerable<T> objs, bool serizalizeTypeData)
			{
				if (objs == null)
				{
					return uint.MaxValue;
				}
				uint[] array = new uint[objs.Count<T>()];
				int num = 0;
				foreach (T t in objs)
				{
					array[num++] = this.WriteObject(t, serizalizeTypeData);
				}
				return this.Write<uint>(array, true);
			}

			// Token: 0x0600033F RID: 831 RVA: 0x0000B3A4 File Offset: 0x000095A4
			public uint WriteObject(object obj, bool serializeTypeData)
			{
				if (obj == null)
				{
					return uint.MaxValue;
				}
				Type type = obj.GetType();
				BinaryStorageBuffer.ISerializationAdapter serializationAdapter;
				if (!BinaryStorageBuffer.GetSerializationAdapter(this.serializationAdapters, type, out serializationAdapter))
				{
					return uint.MaxValue;
				}
				uint num = serializationAdapter.Serialize(this, obj);
				if (serializeTypeData)
				{
					num = this.Write<BinaryStorageBuffer.ObjectTypeData>(new BinaryStorageBuffer.ObjectTypeData
					{
						typeId = this.WriteObject(type, false),
						objectId = num
					});
				}
				return num;
			}

			// Token: 0x06000340 RID: 832 RVA: 0x0000B403 File Offset: 0x00009603
			public uint WriteString(string str, char sep = '\0')
			{
				if (str == null)
				{
					return uint.MaxValue;
				}
				if (sep != '\0')
				{
					return this.WriteDynamicString(str, sep);
				}
				return this.WriteAutoEncodedString(str);
			}

			// Token: 0x06000341 RID: 833 RVA: 0x0000B420 File Offset: 0x00009620
			private unsafe uint WriteStringInternal(string val, Encoding enc)
			{
				if (val == null)
				{
					return uint.MaxValue;
				}
				byte[] bytes = enc.GetBytes(val);
				byte[] array;
				byte* ptr;
				if ((array = bytes) == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				return this.WriteInternal((void*)ptr, (uint)bytes.Length, true);
			}

			// Token: 0x06000342 RID: 834 RVA: 0x0000B460 File Offset: 0x00009660
			public unsafe byte[] SerializeToByteArray()
			{
				byte[] array = new byte[this.totalBytes];
				byte[] array2;
				byte* ptr;
				if ((array2 = array) == null || array2.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array2[0];
				}
				uint num = 0U;
				foreach (BinaryStorageBuffer.Writer.Chunk chunk in this.chunks)
				{
					try
					{
						byte[] array3;
						byte* ptr2;
						if ((array3 = chunk.data) == null || array3.Length == 0)
						{
							ptr2 = null;
						}
						else
						{
							ptr2 = &array3[0];
						}
						UnsafeUtility.MemCpy((void*)(ptr + num), (void*)ptr2, (long)((ulong)chunk.position));
					}
					finally
					{
						byte[] array3 = null;
					}
					num += chunk.position;
				}
				array2 = null;
				return array;
			}

			// Token: 0x06000343 RID: 835 RVA: 0x0000B52C File Offset: 0x0000972C
			public uint SerializeToStream(Stream str)
			{
				foreach (BinaryStorageBuffer.Writer.Chunk chunk in this.chunks)
				{
					str.Write(chunk.data, 0, (int)chunk.position);
				}
				return this.totalBytes;
			}

			// Token: 0x06000344 RID: 836 RVA: 0x0000B594 File Offset: 0x00009794
			private static bool IsUnicode(string str)
			{
				for (int i = 0; i < str.Length; i++)
				{
					if (str[i] > 'ÿ')
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06000345 RID: 837 RVA: 0x0000B5C3 File Offset: 0x000097C3
			private uint WriteAutoEncodedString(string str)
			{
				if (str == null)
				{
					return uint.MaxValue;
				}
				if (BinaryStorageBuffer.Writer.IsUnicode(str))
				{
					return this.WriteUnicodeString(str);
				}
				return this.WriteStringInternal(str, Encoding.ASCII);
			}

			// Token: 0x06000346 RID: 838 RVA: 0x0000B5E8 File Offset: 0x000097E8
			private uint WriteUnicodeString(string str)
			{
				uint num = this.WriteStringInternal(str, Encoding.Unicode);
				return 2147483648U | num;
			}

			// Token: 0x06000347 RID: 839 RVA: 0x0000B60C File Offset: 0x0000980C
			private static uint ComputeStringSize(string str, out bool isUnicode)
			{
				if (isUnicode = BinaryStorageBuffer.Writer.IsUnicode(str))
				{
					return (uint)Encoding.Unicode.GetByteCount(str);
				}
				return (uint)Encoding.ASCII.GetByteCount(str);
			}

			// Token: 0x06000348 RID: 840 RVA: 0x0000B640 File Offset: 0x00009840
			private uint RecurseDynamicStringParts(BinaryStorageBuffer.Writer.StringParts[] parts, int index, char sep, uint minSize)
			{
				while (index < parts.Length - 1)
				{
					uint dataSize = parts[index].dataSize;
					uint dataSize2 = parts[index + 1].dataSize;
					if (dataSize >= minSize && dataSize2 >= minSize)
					{
						break;
					}
					parts[index + 1].str = string.Format("{0}{1}{2}", parts[index].str, sep, parts[index + 1].str);
					index++;
				}
				uint num = (parts[index].isUnicode ? this.WriteUnicodeString(parts[index].str) : this.WriteStringInternal(parts[index].str, Encoding.ASCII));
				uint num2 = ((index < parts.Length - 1) ? this.RecurseDynamicStringParts(parts, index + 1, sep, minSize) : uint.MaxValue);
				return this.Write<BinaryStorageBuffer.DynamicString>(new BinaryStorageBuffer.DynamicString
				{
					stringId = num,
					nextId = num2
				});
			}

			// Token: 0x06000349 RID: 841 RVA: 0x0000B72C File Offset: 0x0000992C
			private unsafe uint WriteDynamicString(string str, char sep)
			{
				if (str == null)
				{
					return uint.MaxValue;
				}
				uint num = (uint)sizeof(BinaryStorageBuffer.DynamicString);
				string[] array = str.Split(sep, StringSplitOptions.None);
				BinaryStorageBuffer.Writer.StringParts[] array2 = new BinaryStorageBuffer.Writer.StringParts[array.Length];
				for (int i = 0; i < array2.Length; i++)
				{
					bool flag;
					uint num2 = BinaryStorageBuffer.Writer.ComputeStringSize(array[i], out flag);
					array2[i] = new BinaryStorageBuffer.Writer.StringParts
					{
						str = array[i],
						dataSize = num2,
						isUnicode = flag
					};
				}
				if (array2.Length < 2 || (array2.Length == 2 && array2[0].dataSize + array2[1].dataSize < num))
				{
					return this.WriteAutoEncodedString(str);
				}
				return 1073741824U | this.RecurseDynamicStringParts(array2, 0, sep, num);
			}

			// Token: 0x0400014A RID: 330
			private uint totalBytes;

			// Token: 0x0400014B RID: 331
			private uint defaulChunkSize;

			// Token: 0x0400014C RID: 332
			private List<BinaryStorageBuffer.Writer.Chunk> chunks;

			// Token: 0x0400014D RID: 333
			private Dictionary<Hash128, uint> existingValues;

			// Token: 0x0400014E RID: 334
			private Dictionary<Type, BinaryStorageBuffer.ISerializationAdapter> serializationAdapters;

			// Token: 0x0200008A RID: 138
			private class Chunk
			{
				// Token: 0x04000197 RID: 407
				public uint position;

				// Token: 0x04000198 RID: 408
				public byte[] data;
			}

			// Token: 0x0200008B RID: 139
			private struct StringParts
			{
				// Token: 0x04000199 RID: 409
				public string str;

				// Token: 0x0400019A RID: 410
				public uint dataSize;

				// Token: 0x0400019B RID: 411
				public bool isUnicode;
			}
		}
	}
}
