using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Json
{
	// Token: 0x02000076 RID: 118
	internal struct JsonValidator : IDisposable
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000BD01 File Offset: 0x00009F01
		public bool IsCreated
		{
			get
			{
				return null != this.m_Data;
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000BD10 File Offset: 0x00009F10
		public unsafe JsonValidator(JsonValidationType validationType, Allocator label = Allocator.TempJob)
		{
			this.m_ValidationType = validationType;
			this.m_Label = label;
			this.m_Data = (UnsafeJsonValidator*)UnsafeUtility.Malloc((long)sizeof(UnsafeJsonValidator), UnsafeUtility.AlignOf<UnsafeJsonValidator>(), label);
			UnsafeUtility.MemClear((void*)this.m_Data, (long)sizeof(UnsafeJsonValidator));
			this.m_Data->Stack = new JsonTypeStack(128, label);
			this.Reset();
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000BD71 File Offset: 0x00009F71
		public unsafe void Dispose()
		{
			if (null == this.m_Data)
			{
				return;
			}
			this.m_Data->Stack.Dispose();
			UnsafeUtility.Free((void*)this.m_Data, this.m_Label);
			this.m_Data = null;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000BDA8 File Offset: 0x00009FA8
		public unsafe void Reset()
		{
			if (null == this.m_Data)
			{
				return;
			}
			this.m_Data->Stack.Clear();
			this.m_Data->CharBufferPosition = 0;
			this.m_Data->PrevChar = 0;
			this.m_Data->Expected = JsonType.Value;
			this.m_Data->Actual = JsonType.Undefined;
			this.m_Data->LineCount = 1;
			this.m_Data->LineStart = -1;
			this.m_Data->CharCount = 1;
			this.m_Data->Char = 0;
			this.m_Data->PartialTokenType = JsonType.Undefined;
			this.m_Data->PartialTokenState = 0;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000BE4C File Offset: 0x0000A04C
		public unsafe JsonValidationResult Validate(UnsafeBuffer<char> buffer, int start, int count)
		{
			this.m_Data->CharBufferPosition = start;
			switch (this.m_ValidationType)
			{
			case JsonValidationType.None:
				return default(JsonValidationResult);
			case JsonValidationType.Standard:
			{
				JsonValidator.StandardJsonValidation standardJsonValidation = default(JsonValidator.StandardJsonValidation);
				standardJsonValidation.Data = this.m_Data;
				standardJsonValidation.CharBuffer = (ushort*)buffer.Buffer;
				standardJsonValidation.CharBufferLength = start + count;
				standardJsonValidation.Validate();
				break;
			}
			case JsonValidationType.Simple:
			{
				JsonValidator.SimpleJsonValidation simpleJsonValidation = default(JsonValidator.SimpleJsonValidation);
				simpleJsonValidation.Data = this.m_Data;
				simpleJsonValidation.CharBuffer = (ushort*)buffer.Buffer;
				simpleJsonValidation.CharBufferLength = start + count;
				simpleJsonValidation.Validate();
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
			return this.GetResult();
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000BF00 File Offset: 0x0000A100
		public unsafe JsonValidationResult GetResult()
		{
			return new JsonValidationResult
			{
				ValidationType = this.m_ValidationType,
				ExpectedType = this.m_Data->Expected,
				ActualType = this.m_Data->Actual,
				Char = (char)this.m_Data->Char,
				LineCount = this.m_Data->LineCount,
				CharCount = this.m_Data->CharCount
			};
		}

		// Token: 0x04000177 RID: 375
		private const int k_ResultSuccess = 0;

		// Token: 0x04000178 RID: 376
		private const int k_ResultEndOfStream = -1;

		// Token: 0x04000179 RID: 377
		private const int k_ResultInvalidJson = -2;

		// Token: 0x0400017A RID: 378
		private const int k_DefaultDepthLimit = 128;

		// Token: 0x0400017B RID: 379
		private readonly JsonValidationType m_ValidationType;

		// Token: 0x0400017C RID: 380
		private readonly Allocator m_Label;

		// Token: 0x0400017D RID: 381
		[NativeDisableUnsafePtrRestriction]
		private unsafe UnsafeJsonValidator* m_Data;

		// Token: 0x02000077 RID: 119
		private struct SimpleJsonValidation
		{
			// Token: 0x060002B9 RID: 697 RVA: 0x0000BF80 File Offset: 0x0000A180
			private unsafe void Break(JsonType actual)
			{
				int num = this.m_CharBufferPosition - this.m_LineStart;
				this.Data->CharBufferPosition = this.m_CharBufferPosition;
				this.Data->Stack = this.m_Stack;
				this.Data->PrevChar = this.m_PrevChar;
				this.Data->Expected = this.m_Expected;
				this.Data->Actual = actual;
				this.Data->LineCount = this.m_LineCount;
				this.Data->LineStart = -num;
				this.Data->CharCount = num;
				this.Data->Char = ((this.m_CharBufferPosition < this.CharBufferLength) ? this.CharBuffer[this.m_CharBufferPosition] : 0);
				this.Data->PartialTokenType = this.m_PartialTokenType;
				this.Data->PartialTokenState = this.m_PartialTokenState;
			}

			// Token: 0x060002BA RID: 698 RVA: 0x0000C064 File Offset: 0x0000A264
			public unsafe void Validate()
			{
				this.m_CharBufferPosition = this.Data->CharBufferPosition;
				this.m_Stack = this.Data->Stack;
				this.m_PrevChar = this.Data->PrevChar;
				this.m_Expected = this.Data->Expected;
				this.m_LineCount = this.Data->LineCount;
				this.m_LineStart = this.Data->LineStart;
				this.m_PartialTokenType = this.Data->PartialTokenType;
				this.m_PartialTokenState = this.Data->PartialTokenState;
				if (this.m_PartialTokenType == JsonType.String)
				{
					int num = this.ReadString();
					if (num != 0)
					{
						this.m_PartialTokenType = JsonType.String;
						this.m_PartialTokenState += this.m_CharBufferPosition;
						this.Break((num == -1) ? JsonType.EOF : JsonType.Undefined);
						return;
					}
					this.m_PartialTokenType = JsonType.Undefined;
					this.m_PartialTokenState = 0;
					this.m_CharBufferPosition++;
				}
				while (this.m_CharBufferPosition < this.CharBufferLength)
				{
					ushort num2 = this.CharBuffer[this.m_CharBufferPosition];
					if (num2 <= 58)
					{
						if (num2 <= 32)
						{
							switch (num2)
							{
							case 9:
							case 13:
								break;
							case 10:
								this.m_LineCount++;
								this.m_LineStart = this.m_CharBufferPosition;
								break;
							case 11:
							case 12:
								goto IL_03E0;
							default:
								if (num2 != 32)
								{
									goto IL_03E0;
								}
								break;
							}
						}
						else if (num2 != 34)
						{
							if (num2 != 44)
							{
								if (num2 != 58)
								{
									goto IL_03E0;
								}
								goto IL_02FE;
							}
							else
							{
								if (!this.IsExpected(JsonType.ValueSeparator))
								{
									this.Break(JsonType.ValueSeparator);
									return;
								}
								JsonType jsonType = this.m_Stack.Peek();
								if (jsonType != JsonType.BeginObject)
								{
									if (jsonType != JsonType.BeginArray)
									{
										this.m_Expected = JsonType.Undefined;
									}
									else
									{
										this.m_Expected = JsonType.Value;
									}
								}
								else
								{
									this.m_Expected = JsonType.String;
								}
							}
						}
						else
						{
							if (!this.IsExpected(JsonType.String))
							{
								this.Break(JsonType.String);
								return;
							}
							int charBufferPosition = this.m_CharBufferPosition;
							this.m_CharBufferPosition++;
							int num3 = this.ReadString();
							if (num3 != 0)
							{
								this.m_PartialTokenType = JsonType.String;
								this.m_PartialTokenState = this.m_CharBufferPosition - charBufferPosition;
								this.Break((num3 == -1) ? JsonType.EOF : JsonType.Undefined);
								return;
							}
						}
					}
					else if (num2 <= 91)
					{
						if (num2 == 61)
						{
							goto IL_02FE;
						}
						if (num2 != 91)
						{
							goto IL_03E0;
						}
						if (!this.IsExpected(JsonType.BeginArray))
						{
							this.Break(JsonType.BeginArray);
							return;
						}
						this.m_Stack.Push(JsonType.BeginArray);
						this.m_Expected = JsonType.BeginObject | JsonType.BeginArray | JsonType.EndArray | JsonType.String | JsonType.Number | JsonType.Negative | JsonType.NaN | JsonType.Infinity | JsonType.True | JsonType.False | JsonType.Null;
					}
					else if (num2 != 93)
					{
						if (num2 != 123)
						{
							if (num2 != 125)
							{
								goto IL_03E0;
							}
							if (!this.IsExpected(JsonType.EndObject))
							{
								this.Break(JsonType.EndObject);
								return;
							}
							this.m_Stack.Pop();
							if (this.m_Stack.Peek() == JsonType.String)
							{
								this.m_Stack.Pop();
							}
							JsonType jsonType = this.m_Stack.Peek();
							if (jsonType != JsonType.BeginObject)
							{
								if (jsonType != JsonType.BeginArray)
								{
									this.m_Expected = JsonType.EOF;
								}
								else
								{
									this.m_Expected = JsonType.BeginObject | JsonType.BeginArray | JsonType.EndArray | JsonType.ValueSeparator | JsonType.String | JsonType.Number | JsonType.Negative | JsonType.NaN | JsonType.Infinity | JsonType.True | JsonType.False | JsonType.Null;
								}
							}
							else
							{
								this.m_Expected = JsonType.BeginObject | JsonType.EndObject | JsonType.BeginArray | JsonType.ValueSeparator | JsonType.String | JsonType.Number | JsonType.Negative | JsonType.NaN | JsonType.Infinity | JsonType.True | JsonType.False | JsonType.Null;
							}
						}
						else
						{
							if (!this.IsExpected(JsonType.BeginObject))
							{
								this.Break(JsonType.BeginObject);
								return;
							}
							this.m_Stack.Push(JsonType.BeginObject);
							this.m_Expected = JsonType.EndObject | JsonType.String;
						}
					}
					else
					{
						if (!this.IsExpected(JsonType.EndArray))
						{
							this.Break(JsonType.EndArray);
							return;
						}
						this.m_Stack.Pop();
						if (this.m_Stack.Peek() == JsonType.String)
						{
							this.m_Stack.Pop();
						}
						JsonType jsonType = this.m_Stack.Peek();
						if (jsonType != JsonType.BeginObject)
						{
							if (jsonType != JsonType.BeginArray)
							{
								this.m_Expected = JsonType.EOF;
							}
							else
							{
								this.m_Expected = JsonType.BeginObject | JsonType.BeginArray | JsonType.EndArray | JsonType.ValueSeparator | JsonType.String | JsonType.Number | JsonType.Negative | JsonType.NaN | JsonType.Infinity | JsonType.True | JsonType.False | JsonType.Null;
							}
						}
						else
						{
							this.m_Expected = JsonType.BeginObject | JsonType.EndObject | JsonType.BeginArray | JsonType.ValueSeparator | JsonType.String | JsonType.Number | JsonType.Negative | JsonType.NaN | JsonType.Infinity | JsonType.True | JsonType.False | JsonType.Null;
						}
					}
					IL_0448:
					this.m_CharBufferPosition++;
					continue;
					IL_02FE:
					if (!this.IsExpected(JsonType.MemberSeparator))
					{
						this.Break(JsonType.MemberSeparator);
						return;
					}
					this.m_Expected = JsonType.Value;
					goto IL_0448;
					IL_03E0:
					if (!this.IsExpected(JsonType.Value) && !this.IsExpected(JsonType.String))
					{
						this.Break(JsonType.Value);
						return;
					}
					int charBufferPosition2 = this.m_CharBufferPosition;
					int num4 = this.ReadPrimitive();
					if (num4 != 0)
					{
						this.m_PartialTokenType = JsonType.String;
						this.m_PartialTokenState = this.m_CharBufferPosition - charBufferPosition2;
						this.Break((num4 == -1) ? JsonType.EOF : JsonType.Undefined);
						return;
					}
					goto IL_0448;
				}
				this.m_PartialTokenType = JsonType.Undefined;
				this.Break(JsonType.EOF);
			}

			// Token: 0x060002BB RID: 699 RVA: 0x0000C4EC File Offset: 0x0000A6EC
			private unsafe int ReadPrimitive()
			{
				while (this.m_CharBufferPosition < this.CharBufferLength)
				{
					ushort num = this.CharBuffer[this.m_CharBufferPosition];
					if (num == 9 || num == 13 || num == 10 || num == 44 || num == 32 || num == 58 || num == 61 || num == 93 || num == 125 || num == 123 || num == 91)
					{
						JsonType jsonType = this.m_Stack.Peek();
						if (jsonType - JsonType.Undefined > 1)
						{
							if (jsonType != JsonType.BeginArray)
							{
								if (jsonType == JsonType.String)
								{
									this.m_Stack.Pop();
									JsonType jsonType2 = this.m_Stack.Peek();
									if (jsonType2 != JsonType.BeginObject)
									{
										if (jsonType2 != JsonType.BeginArray)
										{
											this.m_Expected = JsonType.Undefined;
										}
										else
										{
											this.m_Expected = JsonType.BeginObject | JsonType.BeginArray | JsonType.EndArray | JsonType.ValueSeparator | JsonType.String | JsonType.Number | JsonType.Negative | JsonType.NaN | JsonType.Infinity | JsonType.True | JsonType.False | JsonType.Null;
										}
									}
									else
									{
										this.m_Expected = JsonType.BeginObject | JsonType.EndObject | JsonType.BeginArray | JsonType.ValueSeparator | JsonType.String | JsonType.Number | JsonType.Negative | JsonType.NaN | JsonType.Infinity | JsonType.True | JsonType.False | JsonType.Null;
									}
								}
							}
							else
							{
								this.m_Expected = JsonType.BeginObject | JsonType.BeginArray | JsonType.EndArray | JsonType.ValueSeparator | JsonType.String | JsonType.Number | JsonType.Negative | JsonType.NaN | JsonType.Infinity | JsonType.True | JsonType.False | JsonType.Null;
							}
						}
						else
						{
							this.m_Stack.Push(JsonType.String);
							this.m_Expected = JsonType.MemberSeparator;
						}
						this.m_CharBufferPosition--;
						return 0;
					}
					this.m_PrevChar = num;
					this.m_CharBufferPosition++;
				}
				if (this.m_Stack.Peek() == JsonType.String && this.m_Stack.Peek(1) == JsonType.Undefined)
				{
					this.m_Expected |= JsonType.EOF;
				}
				this.m_PartialTokenType = JsonType.Value;
				return -1;
			}

			// Token: 0x060002BC RID: 700 RVA: 0x0000C648 File Offset: 0x0000A848
			private unsafe int ReadString()
			{
				this.m_PrevChar = 0;
				while (this.m_CharBufferPosition < this.CharBufferLength)
				{
					ushort num = this.CharBuffer[this.m_CharBufferPosition];
					if (num == 92)
					{
						this.m_NextStringCharIsEscaped = !this.m_NextStringCharIsEscaped;
					}
					else
					{
						if (num == 34 && !this.m_NextStringCharIsEscaped)
						{
							this.m_NextStringCharIsEscaped = false;
							JsonType jsonType = this.m_Stack.Peek();
							if (jsonType - JsonType.Undefined > 1)
							{
								if (jsonType != JsonType.BeginArray)
								{
									if (jsonType == JsonType.String)
									{
										this.m_Stack.Pop();
										JsonType jsonType2 = this.m_Stack.Peek();
										if (jsonType2 != JsonType.Undefined)
										{
											if (jsonType2 != JsonType.BeginObject)
											{
												if (jsonType2 == JsonType.BeginArray)
												{
													this.m_Expected = JsonType.BeginObject | JsonType.BeginArray | JsonType.EndArray | JsonType.ValueSeparator | JsonType.String | JsonType.Number | JsonType.Negative | JsonType.NaN | JsonType.Infinity | JsonType.True | JsonType.False | JsonType.Null;
												}
											}
											else
											{
												this.m_Expected = JsonType.BeginObject | JsonType.EndObject | JsonType.BeginArray | JsonType.ValueSeparator | JsonType.String | JsonType.Number | JsonType.Negative | JsonType.NaN | JsonType.Infinity | JsonType.True | JsonType.False | JsonType.Null | JsonType.EOF;
											}
										}
										else
										{
											this.m_Expected = JsonType.BeginObject | JsonType.BeginArray | JsonType.ValueSeparator | JsonType.String | JsonType.Number | JsonType.Negative | JsonType.NaN | JsonType.Infinity | JsonType.True | JsonType.False | JsonType.Null | JsonType.EOF;
										}
									}
								}
								else
								{
									this.m_Expected = JsonType.BeginObject | JsonType.BeginArray | JsonType.EndArray | JsonType.ValueSeparator | JsonType.String | JsonType.Number | JsonType.Negative | JsonType.NaN | JsonType.Infinity | JsonType.True | JsonType.False | JsonType.Null;
								}
							}
							else
							{
								this.m_Stack.Push(JsonType.String);
								this.m_Expected = JsonType.MemberSeparator | JsonType.EOF;
							}
							return 0;
						}
						this.m_NextStringCharIsEscaped = false;
					}
					this.m_PrevChar = num;
					this.m_CharBufferPosition++;
				}
				return -1;
			}

			// Token: 0x060002BD RID: 701 RVA: 0x0000C76A File Offset: 0x0000A96A
			private bool IsExpected(JsonType type)
			{
				return (type & this.m_Expected) == type;
			}

			// Token: 0x0400017E RID: 382
			[NativeDisableUnsafePtrRestriction]
			public unsafe UnsafeJsonValidator* Data;

			// Token: 0x0400017F RID: 383
			[NativeDisableUnsafePtrRestriction]
			public unsafe ushort* CharBuffer;

			// Token: 0x04000180 RID: 384
			public int CharBufferLength;

			// Token: 0x04000181 RID: 385
			private int m_CharBufferPosition;

			// Token: 0x04000182 RID: 386
			private JsonTypeStack m_Stack;

			// Token: 0x04000183 RID: 387
			private ushort m_PrevChar;

			// Token: 0x04000184 RID: 388
			private JsonType m_Expected;

			// Token: 0x04000185 RID: 389
			private int m_LineCount;

			// Token: 0x04000186 RID: 390
			private int m_LineStart;

			// Token: 0x04000187 RID: 391
			private JsonType m_PartialTokenType;

			// Token: 0x04000188 RID: 392
			private int m_PartialTokenState;

			// Token: 0x04000189 RID: 393
			private bool m_NextStringCharIsEscaped;
		}

		// Token: 0x02000078 RID: 120
		private struct StandardJsonValidation
		{
			// Token: 0x060002BE RID: 702 RVA: 0x0000C778 File Offset: 0x0000A978
			private unsafe void Break(JsonType actual)
			{
				int num = this.m_CharBufferPosition - this.m_LineStart;
				this.Data->CharBufferPosition = this.m_CharBufferPosition;
				this.Data->Stack = this.m_Stack;
				this.Data->PrevChar = this.m_PrevChar;
				this.Data->Expected = this.m_Expected;
				this.Data->Actual = actual;
				this.Data->LineCount = this.m_LineCount;
				this.Data->LineStart = -num;
				this.Data->CharCount = num;
				this.Data->Char = ((this.m_CharBufferPosition < this.CharBufferLength) ? this.CharBuffer[this.m_CharBufferPosition] : 0);
				this.Data->PartialTokenType = this.m_PartialTokenType;
				this.Data->PartialTokenState = this.m_PartialTokenState;
			}

			// Token: 0x060002BF RID: 703 RVA: 0x0000C85C File Offset: 0x0000AA5C
			public unsafe void Validate()
			{
				this.m_CharBufferPosition = this.Data->CharBufferPosition;
				this.m_Stack = this.Data->Stack;
				this.m_PrevChar = this.Data->PrevChar;
				this.m_Expected = this.Data->Expected;
				this.m_LineCount = this.Data->LineCount;
				this.m_LineStart = this.Data->LineStart;
				this.m_PartialTokenType = this.Data->PartialTokenType;
				this.m_PartialTokenState = this.Data->PartialTokenState;
				JsonType jsonType = this.m_PartialTokenType;
				if (jsonType <= JsonType.Infinity)
				{
					if (jsonType <= JsonType.Number)
					{
						if (jsonType != JsonType.String)
						{
							if (jsonType == JsonType.Number)
							{
								int partialTokenState = this.m_PartialTokenState;
								int num = this.ReadNumber(ref partialTokenState);
								if (num != 0)
								{
									this.m_PartialTokenType = JsonType.Number;
									this.m_PartialTokenState = partialTokenState;
									this.Break((num == -1) ? JsonType.EOF : JsonType.Undefined);
									return;
								}
								this.m_PartialTokenType = JsonType.Undefined;
								this.m_PartialTokenState = 0;
							}
						}
						else
						{
							int num2 = this.ReadString();
							if (num2 != 0)
							{
								this.m_PartialTokenType = JsonType.String;
								this.m_PartialTokenState += this.m_CharBufferPosition;
								this.Break((num2 == -1) ? JsonType.EOF : JsonType.Undefined);
								return;
							}
							this.m_PartialTokenType = JsonType.Undefined;
							this.m_PartialTokenState = 0;
							this.m_CharBufferPosition++;
						}
					}
					else if (jsonType != JsonType.NaN)
					{
						if (jsonType == JsonType.Infinity)
						{
							int num3 = this.ReadInfinity(this.m_PartialTokenState);
							if (num3 != 0)
							{
								this.m_PartialTokenType = JsonType.Infinity;
								this.m_PartialTokenState += this.m_CharBufferPosition;
								this.Break((num3 == -1) ? JsonType.EOF : JsonType.Undefined);
								return;
							}
							this.m_PartialTokenType = JsonType.Undefined;
							this.m_PartialTokenState = 0;
						}
					}
					else
					{
						int num4 = this.ReadNaN(this.m_PartialTokenState);
						if (num4 != 0)
						{
							this.m_PartialTokenType = JsonType.NaN;
							this.m_PartialTokenState += this.m_CharBufferPosition;
							this.Break((num4 == -1) ? JsonType.EOF : JsonType.Undefined);
							return;
						}
						this.m_PartialTokenType = JsonType.Undefined;
						this.m_PartialTokenState = 0;
					}
				}
				else if (jsonType <= JsonType.False)
				{
					if (jsonType != JsonType.True)
					{
						if (jsonType == JsonType.False)
						{
							int num5 = this.ReadFalse(this.m_PartialTokenState);
							if (num5 != 0)
							{
								this.m_PartialTokenType = JsonType.False;
								this.m_PartialTokenState += this.m_CharBufferPosition;
								this.Break((num5 == -1) ? JsonType.EOF : JsonType.Undefined);
								return;
							}
							this.m_PartialTokenType = JsonType.Undefined;
							this.m_PartialTokenState = 0;
						}
					}
					else
					{
						int num6 = this.ReadTrue(this.m_PartialTokenState);
						if (num6 != 0)
						{
							this.m_PartialTokenType = JsonType.True;
							this.m_PartialTokenState += this.m_CharBufferPosition;
							this.Break((num6 == -1) ? JsonType.EOF : JsonType.Undefined);
							return;
						}
						this.m_PartialTokenType = JsonType.Undefined;
						this.m_PartialTokenState = 0;
					}
				}
				else if (jsonType != JsonType.Null)
				{
					if (jsonType == (JsonType.NaN | JsonType.Null))
					{
						int charBufferPosition = this.m_CharBufferPosition;
						int num7 = this.ReadNull(this.m_PartialTokenState);
						if (num7 == 0)
						{
							this.m_PartialTokenType = JsonType.Undefined;
							this.m_PartialTokenState = 0;
						}
						else
						{
							if (num7 == -1)
							{
								this.m_PartialTokenState += this.m_CharBufferPosition;
								this.m_PartialTokenType = JsonType.Null;
								this.Break(JsonType.EOF);
								return;
							}
							this.m_CharBufferPosition = charBufferPosition;
							num7 = this.ReadNaN(this.m_PartialTokenState);
							if (num7 == 0)
							{
								this.m_PartialTokenType = JsonType.Undefined;
								this.m_PartialTokenState = 0;
							}
							else
							{
								if (num7 == -1)
								{
									this.m_PartialTokenState += this.m_CharBufferPosition;
									this.m_PartialTokenType = JsonType.NaN;
									this.Break(JsonType.EOF);
									return;
								}
								this.Break(JsonType.Undefined);
							}
						}
					}
				}
				else
				{
					int num8 = this.ReadNull(this.m_PartialTokenState);
					if (num8 != 0)
					{
						this.m_PartialTokenType = JsonType.Null;
						this.m_PartialTokenState += this.m_CharBufferPosition;
						this.Break((num8 == -1) ? JsonType.EOF : JsonType.Undefined);
						return;
					}
					this.m_PartialTokenType = JsonType.Undefined;
					this.m_PartialTokenState = 0;
				}
				while (this.m_CharBufferPosition < this.CharBufferLength)
				{
					ushort num9 = this.CharBuffer[this.m_CharBufferPosition];
					if (num9 <= 91)
					{
						if (num9 <= 70)
						{
							switch (num9)
							{
							case 9:
							case 13:
								break;
							case 10:
								this.m_LineCount++;
								this.m_LineStart = this.m_CharBufferPosition;
								break;
							case 11:
							case 12:
								goto IL_0AAC;
							default:
								switch (num9)
								{
								case 32:
									break;
								case 33:
								case 35:
								case 36:
								case 37:
								case 38:
								case 39:
								case 40:
								case 41:
								case 42:
								case 43:
								case 46:
								case 47:
									goto IL_0AAC;
								case 34:
								{
									if (!this.IsExpected(JsonType.String))
									{
										this.Break(JsonType.String);
										return;
									}
									int charBufferPosition2 = this.m_CharBufferPosition;
									this.m_CharBufferPosition++;
									int num10 = this.ReadString();
									if (num10 != 0)
									{
										this.m_PartialTokenType = JsonType.String;
										this.m_PartialTokenState = this.m_CharBufferPosition - charBufferPosition2;
										this.Break((num10 == -1) ? JsonType.EOF : JsonType.Undefined);
										return;
									}
									break;
								}
								case 44:
									if (!this.IsExpected(JsonType.ValueSeparator))
									{
										this.Break(JsonType.ValueSeparator);
										return;
									}
									jsonType = this.m_Stack.Peek();
									if (jsonType != JsonType.BeginObject)
									{
										if (jsonType != JsonType.BeginArray)
										{
											this.m_Expected = JsonType.Undefined;
										}
										else
										{
											this.m_Expected = JsonType.Value;
										}
									}
									else
									{
										this.m_Expected = JsonType.String;
									}
									break;
								case 45:
									if (!this.IsExpected(JsonType.Negative))
									{
										this.Break(JsonType.Negative);
										return;
									}
									this.m_Expected = JsonType.Number | JsonType.Infinity;
									break;
								case 48:
								case 49:
								case 50:
								case 51:
								case 52:
								case 53:
								case 54:
								case 55:
								case 56:
								case 57:
								{
									if (!this.IsExpected(JsonType.Number))
									{
										this.Break(JsonType.Number);
										return;
									}
									int num11 = 0;
									int num12 = this.ReadNumber(ref num11);
									if (num12 != 0)
									{
										this.m_PartialTokenType = JsonType.Number;
										this.m_PartialTokenState = num11;
										this.Break((num12 == -1) ? JsonType.EOF : JsonType.Undefined);
										return;
									}
									this.m_CharBufferPosition--;
									break;
								}
								case 58:
									if (!this.IsExpected(JsonType.MemberSeparator))
									{
										this.Break(JsonType.MemberSeparator);
										return;
									}
									this.m_Expected = JsonType.Value;
									break;
								default:
									if (num9 != 70)
									{
										goto IL_0AAC;
									}
									goto IL_08CA;
								}
								break;
							}
						}
						else if (num9 <= 78)
						{
							if (num9 == 73)
							{
								goto IL_0A40;
							}
							if (num9 != 78)
							{
								goto IL_0AAC;
							}
							goto IL_0939;
						}
						else
						{
							if (num9 == 84)
							{
								goto IL_085B;
							}
							if (num9 != 91)
							{
								goto IL_0AAC;
							}
							if (!this.IsExpected(JsonType.BeginArray))
							{
								this.Break(JsonType.BeginArray);
								return;
							}
							this.m_Stack.Push(JsonType.BeginArray);
							this.m_Expected = JsonType.BeginObject | JsonType.BeginArray | JsonType.EndArray | JsonType.String | JsonType.Number | JsonType.Negative | JsonType.NaN | JsonType.Infinity | JsonType.True | JsonType.False | JsonType.Null;
						}
					}
					else if (num9 <= 105)
					{
						if (num9 != 93)
						{
							if (num9 == 102)
							{
								goto IL_08CA;
							}
							if (num9 != 105)
							{
								goto IL_0AAC;
							}
							goto IL_0A40;
						}
						else
						{
							if (!this.IsExpected(JsonType.EndArray))
							{
								this.Break(JsonType.EndArray);
								return;
							}
							this.m_Stack.Pop();
							if (this.m_Stack.Peek() == JsonType.String)
							{
								this.m_Stack.Pop();
							}
							jsonType = this.m_Stack.Peek();
							if (jsonType != JsonType.BeginObject)
							{
								if (jsonType != JsonType.BeginArray)
								{
									this.m_Expected = JsonType.EOF;
								}
								else
								{
									this.m_Expected = JsonType.EndArray | JsonType.ValueSeparator;
								}
							}
							else
							{
								this.m_Expected = JsonType.EndObject | JsonType.ValueSeparator;
							}
						}
					}
					else if (num9 <= 116)
					{
						if (num9 == 110)
						{
							goto IL_0939;
						}
						if (num9 != 116)
						{
							goto IL_0AAC;
						}
						goto IL_085B;
					}
					else if (num9 != 123)
					{
						if (num9 != 125)
						{
							goto IL_0AAC;
						}
						if (!this.IsExpected(JsonType.EndObject))
						{
							this.Break(JsonType.EndObject);
							return;
						}
						this.m_Stack.Pop();
						if (this.m_Stack.Peek() == JsonType.String)
						{
							this.m_Stack.Pop();
						}
						jsonType = this.m_Stack.Peek();
						if (jsonType != JsonType.BeginObject)
						{
							if (jsonType != JsonType.BeginArray)
							{
								this.m_Expected = JsonType.EOF;
							}
							else
							{
								this.m_Expected = JsonType.EndArray | JsonType.ValueSeparator;
							}
						}
						else
						{
							this.m_Expected = JsonType.EndObject | JsonType.ValueSeparator;
						}
					}
					else
					{
						if (!this.IsExpected(JsonType.BeginObject))
						{
							this.Break(JsonType.BeginObject);
							return;
						}
						this.m_Stack.Push(JsonType.BeginObject);
						this.m_Expected = JsonType.EndObject | JsonType.String;
					}
					IL_0AB4:
					this.m_CharBufferPosition++;
					continue;
					IL_085B:
					if (!this.IsExpected(JsonType.True))
					{
						this.Break(JsonType.True);
						return;
					}
					int charBufferPosition3 = this.m_CharBufferPosition;
					int num13 = this.ReadTrue(0);
					if (num13 != 0)
					{
						this.m_PartialTokenType = JsonType.True;
						this.m_PartialTokenState = this.m_CharBufferPosition - charBufferPosition3;
						this.Break((num13 == -1) ? JsonType.EOF : JsonType.Undefined);
						return;
					}
					this.m_CharBufferPosition--;
					goto IL_0AB4;
					IL_08CA:
					if (!this.IsExpected(JsonType.False))
					{
						this.Break(JsonType.False);
						return;
					}
					int charBufferPosition4 = this.m_CharBufferPosition;
					int num14 = this.ReadFalse(0);
					if (num14 != 0)
					{
						this.m_PartialTokenType = JsonType.False;
						this.m_PartialTokenState = this.m_CharBufferPosition - charBufferPosition4;
						this.Break((num14 == -1) ? JsonType.EOF : JsonType.Undefined);
						return;
					}
					this.m_CharBufferPosition--;
					goto IL_0AB4;
					IL_0939:
					if (!this.IsExpected(JsonType.Null) && !this.IsExpected(JsonType.NaN))
					{
						this.Break(JsonType.NaN | JsonType.Null);
						return;
					}
					int charBufferPosition5 = this.m_CharBufferPosition;
					int num15 = this.ReadNull(0);
					if (num15 == 0)
					{
						this.m_CharBufferPosition--;
						goto IL_0AB4;
					}
					if (num15 == -1)
					{
						if (this.m_CharBufferPosition - charBufferPosition5 == 1)
						{
							this.m_PartialTokenState = this.m_CharBufferPosition - charBufferPosition5;
							this.m_PartialTokenType = JsonType.NaN | JsonType.Null;
							this.Break(JsonType.EOF);
							return;
						}
						this.m_PartialTokenState = this.m_CharBufferPosition - charBufferPosition5;
						this.m_PartialTokenType = JsonType.Null;
						this.Break(JsonType.EOF);
						return;
					}
					else
					{
						this.m_CharBufferPosition = charBufferPosition5;
						num15 = this.ReadNaN(0);
						if (num15 == 0)
						{
							this.m_CharBufferPosition--;
							goto IL_0AB4;
						}
						if (num15 == -1)
						{
							this.m_PartialTokenState = this.m_CharBufferPosition - charBufferPosition5;
							this.m_PartialTokenType = JsonType.NaN;
							this.Break(JsonType.EOF);
							return;
						}
						this.Break(JsonType.Undefined);
						goto IL_0AB4;
					}
					IL_0A40:
					if (!this.IsExpected(JsonType.Infinity))
					{
						this.Break(JsonType.Infinity);
						return;
					}
					int charBufferPosition6 = this.m_CharBufferPosition;
					int num16 = this.ReadInfinity(0);
					if (num16 != 0)
					{
						this.m_PartialTokenType = JsonType.Infinity;
						this.m_PartialTokenState = this.m_CharBufferPosition - charBufferPosition6;
						this.Break((num16 == -1) ? JsonType.EOF : JsonType.Undefined);
						return;
					}
					this.m_CharBufferPosition--;
					goto IL_0AB4;
					IL_0AAC:
					this.Break(JsonType.Undefined);
					return;
				}
				this.m_PartialTokenType = JsonType.Undefined;
				this.Break(JsonType.EOF);
			}

			// Token: 0x060002C0 RID: 704 RVA: 0x0000D350 File Offset: 0x0000B550
			private unsafe int ReadString()
			{
				this.m_PrevChar = 0;
				while (this.m_CharBufferPosition < this.CharBufferLength)
				{
					ushort num = this.CharBuffer[this.m_CharBufferPosition];
					if (num == 92)
					{
						this.m_NextStringCharIsEscaped = !this.m_NextStringCharIsEscaped;
					}
					else
					{
						if (num == 34 && !this.m_NextStringCharIsEscaped)
						{
							this.m_NextStringCharIsEscaped = false;
							JsonType jsonType = this.m_Stack.Peek();
							if (jsonType <= JsonType.BeginObject)
							{
								if (jsonType != JsonType.Undefined)
								{
									if (jsonType == JsonType.BeginObject)
									{
										this.m_Stack.Push(JsonType.String);
										this.m_Expected = JsonType.MemberSeparator;
									}
								}
								else
								{
									this.m_Expected = JsonType.EOF;
								}
							}
							else if (jsonType != JsonType.BeginArray)
							{
								if (jsonType == JsonType.String)
								{
									this.m_Stack.Pop();
									JsonType jsonType2 = this.m_Stack.Peek();
									if (jsonType2 != JsonType.BeginObject)
									{
										if (jsonType2 != JsonType.BeginArray)
										{
											this.m_Expected = JsonType.Undefined;
										}
										else
										{
											this.m_Expected = JsonType.EndArray | JsonType.ValueSeparator;
										}
									}
									else
									{
										this.m_Expected = JsonType.EndObject | JsonType.ValueSeparator;
									}
								}
							}
							else
							{
								this.m_Expected = JsonType.EndArray | JsonType.ValueSeparator;
							}
							return 0;
						}
						this.m_NextStringCharIsEscaped = false;
					}
					this.m_PrevChar = num;
					this.m_CharBufferPosition++;
				}
				return -1;
			}

			// Token: 0x060002C1 RID: 705 RVA: 0x0000D474 File Offset: 0x0000B674
			private unsafe int ReadNumber(ref int state)
			{
				this.m_PrevChar = 0;
				while (this.m_CharBufferPosition < this.CharBufferLength)
				{
					ushort num = this.CharBuffer[this.m_CharBufferPosition];
					if (num == 9 || num == 13 || num == 10 || num == 32 || num == 44 || num == 93 || num == 125)
					{
						break;
					}
					switch (num)
					{
					case 43:
					case 45:
						if (state != 3)
						{
							if (state != 0)
							{
								return -2;
							}
							state = 1;
						}
						break;
					case 44:
						goto IL_00BF;
					case 46:
						if (state != 1)
						{
							return -2;
						}
						state = 2;
						break;
					default:
						if (num != 69 && num != 101)
						{
							goto IL_00BF;
						}
						if (this.m_PrevChar == 45 || this.m_PrevChar == 46 || (state != 2 && state != 1))
						{
							return -2;
						}
						state = 3;
						break;
					}
					IL_00D3:
					this.m_PrevChar = num;
					this.m_CharBufferPosition++;
					continue;
					IL_00BF:
					if (num < 48 || num > 57)
					{
						return -2;
					}
					if (state == 0)
					{
						state = 1;
						goto IL_00D3;
					}
					goto IL_00D3;
				}
				if (this.m_CharBufferPosition >= this.CharBufferLength)
				{
					return -1;
				}
				if (this.m_PrevChar == 101 || this.m_PrevChar == 69 || this.m_PrevChar == 45 || this.m_PrevChar == 46)
				{
					return -2;
				}
				if (this.m_Stack.Peek() == JsonType.String)
				{
					this.m_Stack.Pop();
				}
				JsonType jsonType = this.m_Stack.Peek();
				if (jsonType != JsonType.BeginObject)
				{
					if (jsonType != JsonType.BeginArray)
					{
						this.m_Expected = JsonType.Undefined;
					}
					else
					{
						this.m_Expected = JsonType.EndArray | JsonType.ValueSeparator;
					}
				}
				else
				{
					this.m_Expected = JsonType.EndObject | JsonType.ValueSeparator;
				}
				return 0;
			}

			// Token: 0x060002C2 RID: 706 RVA: 0x0000D604 File Offset: 0x0000B804
			private unsafe int ReadTrue(int start)
			{
				IntPtr intPtr = stackalloc byte[(UIntPtr)8];
				*intPtr = 116;
				*(intPtr + 2) = 114;
				*(intPtr + (IntPtr)2 * 2) = 117;
				*(intPtr + (IntPtr)3 * 2) = 101;
				ushort* ptr = intPtr;
				return this.ReadPrimitive(ptr, start, 4);
			}

			// Token: 0x060002C3 RID: 707 RVA: 0x0000D63C File Offset: 0x0000B83C
			private unsafe int ReadFalse(int start)
			{
				IntPtr intPtr = stackalloc byte[(UIntPtr)10];
				*intPtr = 102;
				*(intPtr + 2) = 97;
				*(intPtr + (IntPtr)2 * 2) = 108;
				*(intPtr + (IntPtr)3 * 2) = 115;
				*(intPtr + (IntPtr)4 * 2) = 101;
				ushort* ptr = intPtr;
				return this.ReadPrimitive(ptr, start, 5);
			}

			// Token: 0x060002C4 RID: 708 RVA: 0x0000D680 File Offset: 0x0000B880
			private unsafe int ReadNull(int start)
			{
				IntPtr intPtr = stackalloc byte[(UIntPtr)8];
				*intPtr = 110;
				*(intPtr + 2) = 117;
				*(intPtr + (IntPtr)2 * 2) = 108;
				*(intPtr + (IntPtr)3 * 2) = 108;
				ushort* ptr = intPtr;
				return this.ReadPrimitive(ptr, start, 4);
			}

			// Token: 0x060002C5 RID: 709 RVA: 0x0000D6B8 File Offset: 0x0000B8B8
			private unsafe int ReadNaN(int start)
			{
				IntPtr intPtr = stackalloc byte[(UIntPtr)6];
				*intPtr = 110;
				*(intPtr + 2) = 97;
				*(intPtr + (IntPtr)2 * 2) = 110;
				ushort* ptr = intPtr;
				return this.ReadPrimitive(ptr, start, 3);
			}

			// Token: 0x060002C6 RID: 710 RVA: 0x0000D6E8 File Offset: 0x0000B8E8
			private unsafe int ReadInfinity(int start)
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
				ushort* ptr = intPtr;
				return this.ReadPrimitive(ptr, start, 8);
			}

			// Token: 0x060002C7 RID: 711 RVA: 0x0000D744 File Offset: 0x0000B944
			private unsafe int ReadPrimitive(ushort* expected, int start, int length)
			{
				int num = start;
				while (num < length && this.m_CharBufferPosition < this.CharBufferLength)
				{
					if ((this.CharBuffer[this.m_CharBufferPosition] | 32) != expected[num])
					{
						return -2;
					}
					this.m_CharBufferPosition++;
					num++;
				}
				if (this.m_CharBufferPosition >= this.CharBufferLength)
				{
					return -1;
				}
				if (this.m_Stack.Peek() == JsonType.String)
				{
					this.m_Stack.Pop();
				}
				JsonType jsonType = this.m_Stack.Peek();
				if (jsonType != JsonType.BeginObject)
				{
					if (jsonType != JsonType.BeginArray)
					{
						this.m_Expected = JsonType.Undefined;
					}
					else
					{
						this.m_Expected = JsonType.EndArray | JsonType.ValueSeparator;
					}
				}
				else
				{
					this.m_Expected = JsonType.EndObject | JsonType.ValueSeparator;
				}
				return 0;
			}

			// Token: 0x060002C8 RID: 712 RVA: 0x0000D7F8 File Offset: 0x0000B9F8
			private bool IsExpected(JsonType type)
			{
				return (type & this.m_Expected) == type;
			}

			// Token: 0x0400018A RID: 394
			[NativeDisableUnsafePtrRestriction]
			public unsafe UnsafeJsonValidator* Data;

			// Token: 0x0400018B RID: 395
			[NativeDisableUnsafePtrRestriction]
			public unsafe ushort* CharBuffer;

			// Token: 0x0400018C RID: 396
			public int CharBufferLength;

			// Token: 0x0400018D RID: 397
			private int m_CharBufferPosition;

			// Token: 0x0400018E RID: 398
			private JsonTypeStack m_Stack;

			// Token: 0x0400018F RID: 399
			private ushort m_PrevChar;

			// Token: 0x04000190 RID: 400
			private JsonType m_Expected;

			// Token: 0x04000191 RID: 401
			private int m_LineCount;

			// Token: 0x04000192 RID: 402
			private int m_LineStart;

			// Token: 0x04000193 RID: 403
			private JsonType m_PartialTokenType;

			// Token: 0x04000194 RID: 404
			private int m_PartialTokenState;

			// Token: 0x04000195 RID: 405
			private bool m_NextStringCharIsEscaped;
		}
	}
}
