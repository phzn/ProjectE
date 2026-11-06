using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Json
{
	// Token: 0x02000034 RID: 52
	public struct JsonWriter : IDisposable
	{
		// Token: 0x060000FD RID: 253 RVA: 0x000042D3 File Offset: 0x000024D3
		public JsonWriter(Allocator label, JsonWriterOptions options = default(JsonWriterOptions))
		{
			this.m_Impl = new JsonWriter.Unsafe(32, label, options);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000042E4 File Offset: 0x000024E4
		public JsonWriter(int initialCapacity, Allocator label, JsonWriterOptions options = default(JsonWriterOptions))
		{
			this.m_Impl = new JsonWriter.Unsafe(initialCapacity, label, options);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000042F4 File Offset: 0x000024F4
		public void Dispose()
		{
			this.m_Impl.Dispose();
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00004301 File Offset: 0x00002501
		public override string ToString()
		{
			return this.m_Impl.ToString();
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00004314 File Offset: 0x00002514
		public JsonWriter.Unsafe AsUnsafe()
		{
			return this.m_Impl;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000431C File Offset: 0x0000251C
		public void Clear()
		{
			this.m_Impl.Clear();
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00004329 File Offset: 0x00002529
		public JsonWriter.ObjectScope WriteObjectScope(string key = null)
		{
			return new JsonWriter.ObjectScope(this, key);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00004337 File Offset: 0x00002537
		public JsonWriter.ArrayScope WriteArrayScope(string key = null)
		{
			return new JsonWriter.ArrayScope(this, key);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00004348 File Offset: 0x00002548
		public unsafe void WriteBeginObject(string key = null)
		{
			if (string.IsNullOrEmpty(key))
			{
				this.m_Impl.WriteBeginObject();
				return;
			}
			fixed (string text = key)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				this.m_Impl.WriteBeginObject(ptr, key.Length);
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000438D File Offset: 0x0000258D
		public void WriteEndObject()
		{
			this.m_Impl.WriteEndObject();
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000439C File Offset: 0x0000259C
		public unsafe void WriteBeginArray(string key = null)
		{
			if (string.IsNullOrEmpty(key))
			{
				this.m_Impl.WriteBeginArray();
				return;
			}
			fixed (string text = key)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				this.m_Impl.WriteBeginArray(ptr, key.Length);
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000043E1 File Offset: 0x000025E1
		public void WriteEndArray()
		{
			this.m_Impl.WriteEndArray();
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000043F0 File Offset: 0x000025F0
		public unsafe void WriteKey(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			fixed (string text = key)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				this.m_Impl.WriteKey(ptr, key.Length);
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00004434 File Offset: 0x00002634
		public void WriteValue(int value)
		{
			this.m_Impl.WriteValue(value);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00004442 File Offset: 0x00002642
		public void WriteValue(long value)
		{
			this.m_Impl.WriteValue(value);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00004450 File Offset: 0x00002650
		public void WriteValue(float value)
		{
			this.m_Impl.WriteValue(value);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000445E File Offset: 0x0000265E
		public void WriteValue(bool value)
		{
			this.m_Impl.WriteValue(value);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000446C File Offset: 0x0000266C
		public unsafe void WriteValue(uint value)
		{
			string text = value.ToString();
			fixed (string text2 = text)
			{
				char* ptr = text2;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				this.m_Impl.WriteValueLiteral(ptr, text.Length);
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000044A8 File Offset: 0x000026A8
		public unsafe void WriteValue(ulong value)
		{
			string text = value.ToString();
			fixed (string text2 = text)
			{
				char* ptr = text2;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				this.m_Impl.WriteValueLiteral(ptr, text.Length);
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000044E4 File Offset: 0x000026E4
		public unsafe void WriteValue(double value)
		{
			string text = value.ToString(CultureInfo.InvariantCulture);
			fixed (string text2 = text)
			{
				char* ptr = text2;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				this.m_Impl.WriteValueLiteral(ptr, text.Length);
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00004522 File Offset: 0x00002722
		public void WriteNull()
		{
			this.m_Impl.WriteNull();
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000452F File Offset: 0x0000272F
		public void WriteValue(char value)
		{
			this.m_Impl.WriteValue(value);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00004540 File Offset: 0x00002740
		public unsafe void WriteValue(string value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			fixed (string text = value)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				this.m_Impl.WriteValue(ptr, value.Length);
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000457B File Offset: 0x0000277B
		public void WriteValueLiteral(char value)
		{
			this.m_Impl.WriteValueLiteral(value);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000458C File Offset: 0x0000278C
		public unsafe void WriteValueLiteral(string value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			fixed (string text = value)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				this.m_Impl.WriteValueLiteral(ptr, value.Length);
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000045C7 File Offset: 0x000027C7
		public void WriteKeyValue(string key, int value)
		{
			this.WriteKey(key);
			this.WriteValue(value);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000045D7 File Offset: 0x000027D7
		public void WriteKeyValue(string key, long value)
		{
			this.WriteKey(key);
			this.WriteValue(value);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000045E7 File Offset: 0x000027E7
		public void WriteKeyValue(string key, float value)
		{
			this.WriteKey(key);
			this.WriteValue(value);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000045F7 File Offset: 0x000027F7
		public void WriteKeyValue(string key, bool value)
		{
			this.WriteKey(key);
			this.WriteValue(value);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00004607 File Offset: 0x00002807
		public void WriteKeyValue(string key, uint value)
		{
			this.WriteKey(key);
			this.WriteValue(value);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00004617 File Offset: 0x00002817
		public void WriteKeyValue(string key, ulong value)
		{
			this.WriteKey(key);
			this.WriteValue(value);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00004627 File Offset: 0x00002827
		public void WriteKeyValue(string key, double value)
		{
			this.WriteKey(key);
			this.WriteValue(value);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00004637 File Offset: 0x00002837
		public void WriteKeyValue(string key, string value)
		{
			this.WriteKey(key);
			this.WriteValue(value);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00004647 File Offset: 0x00002847
		public void WriteKeyValueLiteral(string key, string value)
		{
			this.WriteKey(key);
			this.WriteValueLiteral(value);
		}

		// Token: 0x0400006D RID: 109
		private JsonWriter.Unsafe m_Impl;

		// Token: 0x02000035 RID: 53
		public struct Unsafe : IDisposable
		{
			// Token: 0x17000025 RID: 37
			// (get) Token: 0x0600011F RID: 287 RVA: 0x00004657 File Offset: 0x00002857
			public int Length
			{
				get
				{
					return this.m_Buffer.Length;
				}
			}

			// Token: 0x06000120 RID: 288 RVA: 0x00004664 File Offset: 0x00002864
			public unsafe char* GetUnsafeReadOnlyPtr()
			{
				return this.m_Buffer.GetUnsafeReadOnlyPtr<char>();
			}

			// Token: 0x06000121 RID: 289 RVA: 0x00004674 File Offset: 0x00002874
			public unsafe Unsafe(int initialCapacity, Allocator label, JsonWriterOptions options = default(JsonWriterOptions))
			{
				this.m_Label = label;
				this.m_Data = (JsonWriter.Unsafe.Data*)UnsafeUtility.Malloc((long)sizeof(JsonWriter.Unsafe.Data), UnsafeUtility.AlignOf<JsonWriter.Unsafe.Data>(), label);
				UnsafeUtility.MemClear((void*)this.m_Data, (long)sizeof(JsonWriter.Unsafe.Data));
				this.m_Buffer = new NativeList<char>(initialCapacity, label);
				this.m_Stack = new NativeList<JsonWriter.Unsafe.State>(label);
				this.m_Options = options;
			}

			// Token: 0x06000122 RID: 290 RVA: 0x000046DC File Offset: 0x000028DC
			public unsafe void Dispose()
			{
				this.m_Buffer.Dispose();
				this.m_Stack.Dispose();
				UnsafeUtility.Free((void*)this.m_Data, this.m_Label);
				this.m_Data = null;
			}

			// Token: 0x06000123 RID: 291 RVA: 0x0000470D File Offset: 0x0000290D
			public override string ToString()
			{
				return new string(this.m_Buffer.GetUnsafeReadOnlyPtr<char>(), 0, this.m_Buffer.Length);
			}

			// Token: 0x06000124 RID: 292 RVA: 0x0000472B File Offset: 0x0000292B
			public unsafe void Clear()
			{
				UnsafeUtility.MemClear((void*)this.m_Data, (long)sizeof(JsonWriter.Unsafe.Data));
				this.m_Stack.Clear();
				this.m_Buffer.Clear();
			}

			// Token: 0x06000125 RID: 293 RVA: 0x00004755 File Offset: 0x00002955
			public unsafe void WriteBeginObject(char* ptr, int length)
			{
				this.WriteKey(ptr, length);
				this.WriteBeginObject();
			}

			// Token: 0x06000126 RID: 294 RVA: 0x00004768 File Offset: 0x00002968
			public unsafe void WriteBeginObject()
			{
				if (this.m_Stack.Length != 0 && this.PeekState().Type == JsonWriter.Unsafe.StateType.Array)
				{
					this.WriteMemberSeparator();
				}
				this.m_Data->Key = false;
				this.Write('{');
				this.PushState(JsonWriter.Unsafe.StateType.Object);
				this.m_Data->Indent++;
			}

			// Token: 0x06000127 RID: 295 RVA: 0x000047C4 File Offset: 0x000029C4
			public unsafe void WriteEndObject()
			{
				this.m_Data->Indent--;
				if (!this.m_Options.Minified && this.PeekState().Count > 0)
				{
					this.WriteIndent();
				}
				this.Write('}');
				this.PopState();
				if (this.m_Stack.Length == 0)
				{
					this.m_Data->End = true;
				}
			}

			// Token: 0x06000128 RID: 296 RVA: 0x00004829 File Offset: 0x00002A29
			public unsafe void WriteBeginArray(char* ptr, int length)
			{
				this.WriteKey(ptr, length);
				this.WriteBeginArray();
			}

			// Token: 0x06000129 RID: 297 RVA: 0x0000483C File Offset: 0x00002A3C
			public unsafe void WriteBeginArray()
			{
				if (this.m_Stack.Length != 0 && this.PeekState().Type == JsonWriter.Unsafe.StateType.Array)
				{
					this.WriteMemberSeparator();
				}
				this.m_Data->Key = false;
				this.Write('[');
				this.PushState(JsonWriter.Unsafe.StateType.Array);
				this.m_Data->Indent++;
			}

			// Token: 0x0600012A RID: 298 RVA: 0x00004898 File Offset: 0x00002A98
			public unsafe void WriteEndArray()
			{
				this.m_Data->Indent--;
				if (!this.m_Options.Minified && this.PeekState().Count > 0)
				{
					this.WriteIndent();
				}
				this.Write(']');
				this.PopState();
				if (this.m_Stack.Length == 0)
				{
					this.m_Data->End = true;
				}
			}

			// Token: 0x0600012B RID: 299 RVA: 0x00004900 File Offset: 0x00002B00
			public unsafe void WriteKey(char* ptr, int length)
			{
				this.WriteMemberSeparator();
				bool flag = !this.m_Options.Simplified || JsonWriter.Unsafe.ContainsAnySpecialCharacters(ptr, length);
				if (flag)
				{
					this.Write('"');
				}
				this.Write(ptr, length);
				if (flag)
				{
					this.Write('"');
				}
				if (!this.m_Options.Minified && this.m_Options.Simplified)
				{
					this.Write(' ');
				}
				this.Write(this.m_Options.Simplified ? '=' : ':');
				if (!this.m_Options.Minified)
				{
					this.Write(' ');
				}
				this.m_Data->Key = true;
			}

			// Token: 0x0600012C RID: 300 RVA: 0x000049A4 File Offset: 0x00002BA4
			public void WriteValue(int value)
			{
				FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
				(ref fixedString128Bytes).Append<FixedString128Bytes>(value);
				this.WriteValue(fixedString128Bytes);
			}

			// Token: 0x0600012D RID: 301 RVA: 0x000049CC File Offset: 0x00002BCC
			public void WriteValue(long value)
			{
				FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
				(ref fixedString128Bytes).Append<FixedString128Bytes>(value);
				this.WriteValue(fixedString128Bytes);
			}

			// Token: 0x0600012E RID: 302 RVA: 0x000049F4 File Offset: 0x00002BF4
			public void WriteValue(float value)
			{
				if (float.IsInfinity(value))
				{
					if (float.IsNegativeInfinity(value))
					{
						this.WriteNegativeInfinity();
					}
					else
					{
						this.WritePositiveInfinity();
					}
				}
				FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
				(ref fixedString128Bytes).Append<FixedString128Bytes>(value, '.');
				this.WriteValue(fixedString128Bytes);
			}

			// Token: 0x0600012F RID: 303 RVA: 0x00004A3C File Offset: 0x00002C3C
			public unsafe void WriteValue(bool value)
			{
				if (value)
				{
					IntPtr intPtr = stackalloc byte[(UIntPtr)8];
					*intPtr = 116;
					*(intPtr + 2) = 114;
					*(intPtr + (IntPtr)2 * 2) = 117;
					*(intPtr + (IntPtr)3 * 2) = 101;
					char* ptr = intPtr;
					this.WriteValueLiteral(ptr, 4);
					return;
				}
				IntPtr intPtr2 = stackalloc byte[(UIntPtr)10];
				*intPtr2 = 102;
				*(intPtr2 + 2) = 97;
				*(intPtr2 + (IntPtr)2 * 2) = 108;
				*(intPtr2 + (IntPtr)3 * 2) = 115;
				*(intPtr2 + (IntPtr)4 * 2) = 101;
				char* ptr2 = intPtr2;
				this.WriteValueLiteral(ptr2, 5);
			}

			// Token: 0x06000130 RID: 304 RVA: 0x00004AAC File Offset: 0x00002CAC
			public unsafe void WriteNull()
			{
				IntPtr intPtr = stackalloc byte[(UIntPtr)8];
				*intPtr = 110;
				*(intPtr + 2) = 117;
				*(intPtr + (IntPtr)2 * 2) = 108;
				*(intPtr + (IntPtr)3 * 2) = 108;
				char* ptr = intPtr;
				this.WriteValueLiteral(ptr, 4);
			}

			// Token: 0x06000131 RID: 305 RVA: 0x00004AE4 File Offset: 0x00002CE4
			private unsafe void WritePositiveInfinity()
			{
				IntPtr intPtr = stackalloc byte[(UIntPtr)16];
				*intPtr = 105;
				*(intPtr + 2) = 110;
				*(intPtr + (IntPtr)2 * 2) = 102;
				*(intPtr + (IntPtr)3 * 2) = 105;
				*(intPtr + (IntPtr)4 * 2) = 110;
				*(intPtr + (IntPtr)5 * 2) = 105;
				*(intPtr + (IntPtr)6 * 2) = 116;
				*(intPtr + (IntPtr)7 * 2) = 121;
				char* ptr = intPtr;
				this.WriteValueLiteral(ptr, 8);
			}

			// Token: 0x06000132 RID: 306 RVA: 0x00004B40 File Offset: 0x00002D40
			private unsafe void WriteNegativeInfinity()
			{
				IntPtr intPtr = stackalloc byte[(UIntPtr)18];
				*intPtr = 45;
				*(intPtr + 2) = 105;
				*(intPtr + (IntPtr)2 * 2) = 110;
				*(intPtr + (IntPtr)3 * 2) = 102;
				*(intPtr + (IntPtr)4 * 2) = 105;
				*(intPtr + (IntPtr)5 * 2) = 110;
				*(intPtr + (IntPtr)6 * 2) = 105;
				*(intPtr + (IntPtr)7 * 2) = 116;
				*(intPtr + (IntPtr)8 * 2) = 121;
				char* ptr = intPtr;
				this.WriteValueLiteral(ptr, 9);
			}

			// Token: 0x06000133 RID: 307 RVA: 0x00004BA5 File Offset: 0x00002DA5
			public unsafe void WriteValue(char value)
			{
				this.WriteValue(&value, 1);
			}

			// Token: 0x06000134 RID: 308 RVA: 0x00004BB4 File Offset: 0x00002DB4
			public unsafe void WriteValue(char* ptr, int length)
			{
				if (this.m_Stack.Length != 0)
				{
					if (this.PeekState().Type == JsonWriter.Unsafe.StateType.Array)
					{
						this.WriteMemberSeparator();
					}
					this.m_Data->Key = false;
				}
				else
				{
					this.m_Data->End = true;
				}
				if (null == ptr)
				{
					this.WriteNull();
					return;
				}
				this.Write('"');
				if (this.m_Options.StringEscapeHandling)
				{
					int i = 0;
					while (i < length)
					{
						char c = ptr[i];
						if (c <= '\r')
						{
							if (c != '\0')
							{
								switch (c)
								{
								case '\b':
									this.Write('\\');
									this.Write('b');
									break;
								case '\t':
									this.Write('\\');
									this.Write('t');
									break;
								case '\n':
									this.Write('\\');
									this.Write('n');
									break;
								case '\v':
								case '\f':
									goto IL_0126;
								case '\r':
									this.Write('\\');
									this.Write('r');
									break;
								default:
									goto IL_0126;
								}
							}
							else
							{
								this.Write('\\');
								this.Write('0');
							}
						}
						else if (c != '"')
						{
							if (c != '\\')
							{
								goto IL_0126;
							}
							this.Write('\\');
							this.Write('\\');
						}
						else
						{
							this.Write('\\');
							this.Write('"');
						}
						IL_012D:
						i++;
						continue;
						IL_0126:
						this.Write(c);
						goto IL_012D;
					}
				}
				else
				{
					for (int j = 0; j < length; j++)
					{
						this.Write(ptr[j]);
					}
				}
				this.Write('"');
			}

			// Token: 0x06000135 RID: 309 RVA: 0x00004D1C File Offset: 0x00002F1C
			private unsafe void WriteValue(FixedString128Bytes value)
			{
				void* ptr = UnsafeUtility.AddressOf<FixedString128Bytes>(ref value);
				ushort num = *(ushort*)ptr;
				byte* ptr2 = (byte*)ptr + 2;
				checked
				{
					char* ptr3 = stackalloc char[unchecked((UIntPtr)num) * 2];
					int num2;
					Unicode.Utf8ToUtf16(ptr2, (int)num, ptr3, out num2, (int)num);
					this.WriteValueLiteral(ptr3, num2);
				}
			}

			// Token: 0x06000136 RID: 310 RVA: 0x00004D51 File Offset: 0x00002F51
			public unsafe void WriteValueLiteral(char value)
			{
				this.WriteValueLiteral(&value, 1);
			}

			// Token: 0x06000137 RID: 311 RVA: 0x00004D60 File Offset: 0x00002F60
			public unsafe void WriteValueLiteral(char* ptr, int length)
			{
				if (this.m_Stack.Length != 0)
				{
					if (this.PeekState().Type == JsonWriter.Unsafe.StateType.Array)
					{
						this.WriteMemberSeparator();
					}
					this.m_Data->Key = false;
				}
				else
				{
					this.m_Data->End = true;
				}
				if (null == ptr)
				{
					this.WriteNull();
					return;
				}
				this.Write(ptr, length);
			}

			// Token: 0x06000138 RID: 312 RVA: 0x00004DBC File Offset: 0x00002FBC
			public unsafe void WriteKeyValue(char* ptr, int length, int value)
			{
				this.WriteKey(ptr, length);
				this.WriteValue(value);
			}

			// Token: 0x06000139 RID: 313 RVA: 0x00004DCD File Offset: 0x00002FCD
			public unsafe void WriteKeyValue(char* ptr, int length, uint value)
			{
				this.WriteKey(ptr, length);
				this.WriteValue((long)((ulong)value));
			}

			// Token: 0x0600013A RID: 314 RVA: 0x00004DDF File Offset: 0x00002FDF
			public unsafe void WriteKeyValue(char* ptr, int length, long value)
			{
				this.WriteKey(ptr, length);
				this.WriteValue(value);
			}

			// Token: 0x0600013B RID: 315 RVA: 0x00004DF0 File Offset: 0x00002FF0
			public unsafe void WriteKeyValue(char* ptr, int length, ulong value)
			{
				this.WriteKey(ptr, length);
				this.WriteValue(value);
			}

			// Token: 0x0600013C RID: 316 RVA: 0x00004E03 File Offset: 0x00003003
			public unsafe void WriteKeyValue(char* ptr, int length, float value)
			{
				this.WriteKey(ptr, length);
				this.WriteValue(value);
			}

			// Token: 0x0600013D RID: 317 RVA: 0x00004E14 File Offset: 0x00003014
			public unsafe void WriteKeyValue(char* keyPtr, int keyLength, char* valuePtr, int valueLength)
			{
				this.WriteKey(keyPtr, keyLength);
				this.WriteValue(valuePtr, valueLength);
			}

			// Token: 0x0600013E RID: 318 RVA: 0x00004E27 File Offset: 0x00003027
			public unsafe void WriteKeyValueLiteral(char* keyPtr, int keyLength, char* valuePtr, int valueLength)
			{
				this.WriteKey(keyPtr, keyLength);
				this.WriteValueLiteral(valuePtr, valueLength);
			}

			// Token: 0x0600013F RID: 319 RVA: 0x00004E3C File Offset: 0x0000303C
			private void WriteMemberSeparator()
			{
				int count = this.PeekState().Count;
				if (!this.m_Options.Simplified)
				{
					if (count > 0)
					{
						this.Write(',');
					}
					if (!this.m_Options.Minified)
					{
						this.WriteIndent();
					}
				}
				else if (!this.m_Options.Minified)
				{
					this.WriteIndent();
				}
				else if (count > 0)
				{
					this.Write(' ');
				}
				this.IncrementElementCount();
			}

			// Token: 0x06000140 RID: 320 RVA: 0x00004EAC File Offset: 0x000030AC
			private unsafe void WriteIndent()
			{
				if (this.m_Options.Minified)
				{
					return;
				}
				this.Write('\n');
				for (int i = 0; i < this.m_Options.Indent * this.m_Data->Indent; i++)
				{
					this.Write(' ');
				}
			}

			// Token: 0x06000141 RID: 321 RVA: 0x00004EF9 File Offset: 0x000030F9
			private unsafe void Write(char* ptr, int length)
			{
				this.m_Buffer.AddRange((void*)ptr, length);
			}

			// Token: 0x06000142 RID: 322 RVA: 0x00004F08 File Offset: 0x00003108
			private void Write(char value)
			{
				this.m_Buffer.Add(in value);
			}

			// Token: 0x06000143 RID: 323 RVA: 0x00004F17 File Offset: 0x00003117
			private JsonWriter.Unsafe.State PeekState()
			{
				return this.m_Stack[this.m_Stack.Length - 1];
			}

			// Token: 0x06000144 RID: 324 RVA: 0x00004F31 File Offset: 0x00003131
			private void IncrementElementCount()
			{
				this.m_Stack.ElementAt(this.m_Stack.Length - 1).Count++;
			}

			// Token: 0x06000145 RID: 325 RVA: 0x00004F58 File Offset: 0x00003158
			private void PushState(JsonWriter.Unsafe.StateType type)
			{
				JsonWriter.Unsafe.State state = default(JsonWriter.Unsafe.State);
				state.Type = type;
				state.Count = 0;
				this.m_Stack.Add(in state);
			}

			// Token: 0x06000146 RID: 326 RVA: 0x00004F8A File Offset: 0x0000318A
			private void PopState()
			{
				this.m_Stack.RemoveAt(this.m_Stack.Length - 1);
			}

			// Token: 0x06000147 RID: 327 RVA: 0x00004FA4 File Offset: 0x000031A4
			private unsafe static bool ContainsAnySpecialCharacters(char* ptr, int length)
			{
				for (int i = 0; i < length; i++)
				{
					char c = ptr[i];
					if (c == ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\0' || c == ',' || c == '[' || c == ']' || c == '{' || c == '}' || c == ':' || c == '=')
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06000148 RID: 328 RVA: 0x00005004 File Offset: 0x00003204
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			private unsafe void ValidateWriteBeginObject()
			{
				if (this.m_Data->End || (this.m_Stack.Length != 0 && !this.m_Data->Key && this.PeekState().Type != JsonWriter.Unsafe.StateType.Array))
				{
					throw new InvalidOperationException("WriteBeginObject can only called as a root element, array element or after WriteKey.");
				}
			}

			// Token: 0x06000149 RID: 329 RVA: 0x00005054 File Offset: 0x00003254
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			private unsafe void ValidateWriteEndObject()
			{
				if (this.m_Data->End || this.m_Stack.Length == 0 || this.m_Data->Key || this.PeekState().Type != JsonWriter.Unsafe.StateType.Object)
				{
					throw new InvalidOperationException("WriteEndObject can only called after WriteBeginObject or WriteValue.");
				}
			}

			// Token: 0x0600014A RID: 330 RVA: 0x000050A0 File Offset: 0x000032A0
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			private unsafe void ValidateWriteBeginArray()
			{
				if (this.m_Data->End || (this.m_Stack.Length != 0 && !this.m_Data->Key && this.PeekState().Type != JsonWriter.Unsafe.StateType.Array))
				{
					throw new InvalidOperationException("WriteBeginArray can only called as a root element, array element, or after WriteKey.");
				}
			}

			// Token: 0x0600014B RID: 331 RVA: 0x000050F0 File Offset: 0x000032F0
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			private unsafe void ValidateWriteEndArray()
			{
				if (this.m_Data->End || this.m_Stack.Length == 0 || this.m_Data->Key || this.PeekState().Type != JsonWriter.Unsafe.StateType.Array)
				{
					throw new InvalidOperationException("WriteEndArray can only called after WriteBeginArray or WriteValue.");
				}
			}

			// Token: 0x0600014C RID: 332 RVA: 0x00005140 File Offset: 0x00003340
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			private unsafe void ValidateWriteKey()
			{
				if (this.m_Data->End || this.m_Stack.Length == 0 || this.m_Data->Key || this.PeekState().Type != JsonWriter.Unsafe.StateType.Object)
				{
					throw new InvalidOperationException("WriteKey can only be called in an object scope.");
				}
			}

			// Token: 0x0600014D RID: 333 RVA: 0x0000518C File Offset: 0x0000338C
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			private unsafe void ValidateWriteValue()
			{
				if (this.m_Data->End || (this.m_Stack.Length != 0 && !this.m_Data->Key && this.PeekState().Type != JsonWriter.Unsafe.StateType.Array))
				{
					throw new InvalidOperationException("WriteValue can only be called as a root element, array element, or after WriteKey.");
				}
			}

			// Token: 0x0400006E RID: 110
			private const char k_BeginObjectToken = '{';

			// Token: 0x0400006F RID: 111
			private const char k_EndObjectToken = '}';

			// Token: 0x04000070 RID: 112
			private const char k_BeginArrayToken = '[';

			// Token: 0x04000071 RID: 113
			private const char k_EndArrayToken = ']';

			// Token: 0x04000072 RID: 114
			private const char k_NewlineToken = '\n';

			// Token: 0x04000073 RID: 115
			private const char k_QuoteToken = '"';

			// Token: 0x04000074 RID: 116
			private const char k_SpaceToken = ' ';

			// Token: 0x04000075 RID: 117
			private readonly Allocator m_Label;

			// Token: 0x04000076 RID: 118
			[NativeDisableUnsafePtrRestriction]
			private unsafe JsonWriter.Unsafe.Data* m_Data;

			// Token: 0x04000077 RID: 119
			private NativeList<char> m_Buffer;

			// Token: 0x04000078 RID: 120
			private NativeList<JsonWriter.Unsafe.State> m_Stack;

			// Token: 0x04000079 RID: 121
			private readonly JsonWriterOptions m_Options;

			// Token: 0x02000036 RID: 54
			private enum StateType
			{
				// Token: 0x0400007B RID: 123
				Object,
				// Token: 0x0400007C RID: 124
				Array
			}

			// Token: 0x02000037 RID: 55
			private struct State
			{
				// Token: 0x0400007D RID: 125
				public JsonWriter.Unsafe.StateType Type;

				// Token: 0x0400007E RID: 126
				public int Count;
			}

			// Token: 0x02000038 RID: 56
			private struct Data
			{
				// Token: 0x0400007F RID: 127
				public bool Key;

				// Token: 0x04000080 RID: 128
				public bool End;

				// Token: 0x04000081 RID: 129
				public int Indent;
			}
		}

		// Token: 0x02000039 RID: 57
		public struct ObjectScope : IDisposable
		{
			// Token: 0x0600014E RID: 334 RVA: 0x000051D9 File Offset: 0x000033D9
			public ObjectScope(JsonWriter writer, string key)
			{
				this.Writer = writer;
				this.Writer.WriteBeginObject(key);
			}

			// Token: 0x0600014F RID: 335 RVA: 0x000051EE File Offset: 0x000033EE
			public void Dispose()
			{
				this.Writer.WriteEndObject();
			}

			// Token: 0x04000082 RID: 130
			private JsonWriter Writer;
		}

		// Token: 0x0200003A RID: 58
		public struct ArrayScope : IDisposable
		{
			// Token: 0x06000150 RID: 336 RVA: 0x000051FB File Offset: 0x000033FB
			public ArrayScope(JsonWriter writer, string key = null)
			{
				this.Writer = writer;
				this.Writer.WriteBeginArray(key);
			}

			// Token: 0x06000151 RID: 337 RVA: 0x00005210 File Offset: 0x00003410
			public void Dispose()
			{
				this.Writer.WriteEndArray();
			}

			// Token: 0x04000083 RID: 131
			private JsonWriter Writer;
		}
	}
}
