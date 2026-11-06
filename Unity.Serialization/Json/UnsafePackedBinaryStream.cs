using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Json
{
	// Token: 0x0200004F RID: 79
	internal struct UnsafePackedBinaryStream : IDisposable
	{
		// Token: 0x060001AE RID: 430 RVA: 0x000077FC File Offset: 0x000059FC
		public unsafe UnsafePackedBinaryStream(int initialTokenCapacity, int initialBufferCapacity, Allocator label)
		{
			this.m_Label = label;
			this.Tokens = (BinaryToken*)UnsafeUtility.Malloc((long)(sizeof(BinaryToken) * initialTokenCapacity), UnsafeUtility.AlignOf<BinaryToken>(), this.m_Label);
			this.Handles = (HandleData*)UnsafeUtility.Malloc((long)(sizeof(HandleData) * initialTokenCapacity), UnsafeUtility.AlignOf<HandleData>(), this.m_Label);
			this.TokenCapacity = initialTokenCapacity;
			this.TokenNextIndex = 0;
			this.TokenParentIndex = -1;
			this.Buffer = (byte*)UnsafeUtility.Malloc((long)initialBufferCapacity, UnsafeUtility.AlignOf<byte>(), this.m_Label);
			this.BufferCapacity = initialBufferCapacity;
			this.BufferPosition = 0;
			for (int i = 0; i < initialTokenCapacity; i++)
			{
				this.Tokens[i] = new BinaryToken
				{
					HandleIndex = i
				};
				this.Handles[i] = new HandleData
				{
					DataIndex = i,
					DataVersion = 1
				};
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x000078EC File Offset: 0x00005AEC
		public unsafe void Dispose()
		{
			UnsafeUtility.Free((void*)this.Tokens, this.m_Label);
			UnsafeUtility.Free((void*)this.Handles, this.m_Label);
			UnsafeUtility.Free((void*)this.Buffer, this.m_Label);
			this.Tokens = null;
			this.Handles = null;
			this.Buffer = null;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00007944 File Offset: 0x00005B44
		[BurstDiscard]
		private void CheckTokenRangeAndThrow(int index)
		{
			if (index >= this.TokenCapacity)
			{
				throw new IndexOutOfRangeException();
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00007955 File Offset: 0x00005B55
		[BurstDiscard]
		private void CheckVersionAndThrow(Handle handle, HandleData data)
		{
			if (handle.Version != data.DataVersion)
			{
				throw new InvalidOperationException("View is invalid. The underlying data has been released.");
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00007970 File Offset: 0x00005B70
		[BurstDiscard]
		private void CheckBufferRangeAndThrow(int length)
		{
			if (length > this.BufferPosition)
			{
				throw new IndexOutOfRangeException();
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00007981 File Offset: 0x00005B81
		internal unsafe bool IsValid(Handle handle)
		{
			return handle.Index < this.TokenCapacity && this.Handles[handle.Index].DataVersion == handle.Version;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x000079B5 File Offset: 0x00005BB5
		internal unsafe BinaryToken GetToken(int tokenIndex)
		{
			this.CheckTokenRangeAndThrow(tokenIndex);
			return this.Tokens[tokenIndex];
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x000079D4 File Offset: 0x00005BD4
		internal unsafe int GetTokenIndex(Handle handle)
		{
			this.CheckTokenRangeAndThrow(handle.Index);
			HandleData handleData = this.Handles[handle.Index];
			this.CheckVersionAndThrow(handle, handleData);
			return handleData.DataIndex;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00007A18 File Offset: 0x00005C18
		internal unsafe BinaryToken GetToken(Handle handle)
		{
			this.CheckTokenRangeAndThrow(handle.Index);
			HandleData handleData = this.Handles[handle.Index];
			this.CheckVersionAndThrow(handle, handleData);
			return this.GetToken(handleData.DataIndex);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00007A60 File Offset: 0x00005C60
		internal unsafe Handle GetHandle(int tokenIndex)
		{
			this.CheckTokenRangeAndThrow(tokenIndex);
			HandleData handleData = this.Handles[this.Tokens[tokenIndex].HandleIndex];
			return new Handle
			{
				Index = this.Tokens[tokenIndex].HandleIndex,
				Version = handleData.DataVersion
			};
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00007AD0 File Offset: 0x00005CD0
		internal unsafe Handle GetHandle(BinaryToken token)
		{
			return new Handle
			{
				Index = token.HandleIndex,
				Version = this.Handles[token.HandleIndex].DataVersion
			};
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00007B14 File Offset: 0x00005D14
		internal Handle GetFirstChild(Handle handle)
		{
			int tokenIndex = this.GetTokenIndex(handle);
			for (int i = tokenIndex + 1; i < this.TokenNextIndex; i++)
			{
				BinaryToken token = this.GetToken(i);
				if (token.Length != -1 && token.Parent == tokenIndex && token.Type != TokenType.Comment)
				{
					return this.GetHandle(token);
				}
			}
			return Handle.Null;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00007B6C File Offset: 0x00005D6C
		internal unsafe int GetFirstChildIndex(int start)
		{
			if (this.Tokens[start].Length <= 1)
			{
				return start + 1;
			}
			int num = start + 1;
			for (;;)
			{
				BinaryToken binaryToken = this.Tokens[num];
				if (binaryToken.Length != -1 && binaryToken.Parent == start && binaryToken.Type != TokenType.Comment)
				{
					break;
				}
				num++;
			}
			return num;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00007BD1 File Offset: 0x00005DD1
		internal unsafe T* GetBufferPtr<[IsUnmanaged] T>(int tokenIndex) where T : struct, ValueType
		{
			this.CheckBufferRangeAndThrow(this.Tokens[tokenIndex].Position + sizeof(T));
			return (T*)(this.Buffer + this.Tokens[tokenIndex].Position);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00007C14 File Offset: 0x00005E14
		internal unsafe T* GetBufferPtr<[IsUnmanaged] T>(Handle handle) where T : struct, ValueType
		{
			int position = this.GetToken(handle).Position;
			this.CheckBufferRangeAndThrow(position + sizeof(T));
			return (T*)(this.Buffer + position);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00007C44 File Offset: 0x00005E44
		internal unsafe void EnsureTokenCapacity(int newLength)
		{
			if (newLength <= this.TokenCapacity)
			{
				return;
			}
			int tokenCapacity = this.TokenCapacity;
			this.Tokens = UnsafePackedBinaryStream.Resize<BinaryToken>(this.Tokens, tokenCapacity, newLength, this.m_Label);
			this.Handles = UnsafePackedBinaryStream.Resize<HandleData>(this.Handles, tokenCapacity, newLength, this.m_Label);
			for (int i = tokenCapacity; i < newLength; i++)
			{
				this.Tokens[i] = new BinaryToken
				{
					HandleIndex = i
				};
				this.Handles[i] = new HandleData
				{
					DataIndex = i,
					DataVersion = 1
				};
			}
			this.TokenCapacity = newLength;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00007CFB File Offset: 0x00005EFB
		internal void EnsureBufferCapacity(int length)
		{
			if (length <= this.BufferCapacity)
			{
				return;
			}
			this.Buffer = UnsafePackedBinaryStream.Resize<byte>(this.Buffer, this.BufferPosition, length, this.m_Label);
			this.BufferCapacity = length;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00007D2C File Offset: 0x00005F2C
		public unsafe void Clear()
		{
			for (int i = 0; i < this.TokenNextIndex; i++)
			{
				this.Handles[i].DataVersion++;
			}
			this.TokenNextIndex = 0;
			this.TokenParentIndex = -1;
			this.BufferPosition = 0;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00007D7C File Offset: 0x00005F7C
		internal unsafe int DiscardCompleted()
		{
			int* ptr = stackalloc int[(UIntPtr)512];
			int i = -1;
			int num;
			for (num = this.TokenNextIndex - 1; num != -1; num = this.Tokens[num].Parent)
			{
				if (this.Tokens[num].Length != -1)
				{
					break;
				}
			}
			while (num != -1)
			{
				BinaryToken binaryToken = this.Tokens[num];
				int num2 = num + 1;
				int num3 = 1;
				while (num2 < this.TokenNextIndex && this.Tokens[num2].Length == -1)
				{
					num3++;
					num2++;
				}
				if (i + num3 >= 128)
				{
					return -4;
				}
				for (int j = num3 - 1; j >= 0; j--)
				{
					ptr[(IntPtr)(++i) * 4] = num + j;
				}
				num = binaryToken.Parent;
			}
			int num4 = i + 1;
			int num5 = 0;
			int num6 = 0;
			while (i >= 0)
			{
				num = ptr[i];
				if (this.TokenParentIndex == num)
				{
					this.TokenParentIndex = num6;
				}
				ref BinaryToken ptr2 = ref this.Tokens[num6];
				ref BinaryToken ptr3 = ref this.Tokens[num];
				BinaryToken binaryToken2 = this.Tokens[num];
				BinaryToken binaryToken3 = this.Tokens[num6];
				ptr2 = binaryToken2;
				ptr3 = binaryToken3;
				this.Handles[this.Tokens[num6].HandleIndex].DataIndex = num6;
				this.Handles[this.Tokens[num].HandleIndex].DataIndex = num;
				BinaryToken binaryToken4 = this.Tokens[num6];
				int num7;
				if (num + 1 >= this.TokenNextIndex)
				{
					num7 = this.BufferPosition - binaryToken4.Position;
				}
				else
				{
					num7 = this.Tokens[num + 1].Position - binaryToken4.Position;
				}
				UnsafeUtility.MemCpy((void*)(this.Buffer + num5), (void*)(this.Buffer + binaryToken4.Position), (long)num7);
				binaryToken4.Position = num5;
				int num8 = num6 - 1;
				if (binaryToken4.Length != -1)
				{
					while (num8 != -1 && this.Tokens[num8].Length == -1)
					{
						num8--;
					}
				}
				binaryToken4.Parent = num8;
				this.Tokens[num6] = binaryToken4;
				num5 += num7;
				num6++;
				i--;
			}
			int k = 0;
			int num9 = num4;
			while (k < num4)
			{
				BinaryToken binaryToken5 = this.Tokens[k];
				if (binaryToken5.Length != -1)
				{
					binaryToken5.Length = num9;
				}
				num9--;
				this.Tokens[k] = binaryToken5;
				k++;
			}
			for (int l = num4; l < this.TokenNextIndex; l++)
			{
				this.Handles[this.Tokens[l].HandleIndex].DataVersion++;
			}
			this.BufferPosition = num5;
			this.TokenNextIndex = num4;
			return 0;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x000080E8 File Offset: 0x000062E8
		private unsafe static T* Resize<[IsUnmanaged] T>(T* buffer, int fromLength, int toLength, Allocator label) where T : struct, ValueType
		{
			T* ptr = (T*)UnsafeUtility.Malloc((long)(toLength * sizeof(T)), UnsafeUtility.AlignOf<T>(), label);
			UnsafeUtility.MemCpy((void*)ptr, (void*)buffer, (long)(fromLength * sizeof(T)));
			UnsafeUtility.Free((void*)buffer, label);
			return ptr;
		}

		// Token: 0x040000DC RID: 220
		public const int ResultSuccess = 0;

		// Token: 0x040000DD RID: 221
		public const int ResultStackOverflow = -4;

		// Token: 0x040000DE RID: 222
		private readonly Allocator m_Label;

		// Token: 0x040000DF RID: 223
		public unsafe BinaryToken* Tokens;

		// Token: 0x040000E0 RID: 224
		public unsafe HandleData* Handles;

		// Token: 0x040000E1 RID: 225
		public int TokenCapacity;

		// Token: 0x040000E2 RID: 226
		public int TokenNextIndex;

		// Token: 0x040000E3 RID: 227
		public int TokenParentIndex;

		// Token: 0x040000E4 RID: 228
		public unsafe byte* Buffer;

		// Token: 0x040000E5 RID: 229
		public int BufferCapacity;

		// Token: 0x040000E6 RID: 230
		public int BufferPosition;
	}
}
