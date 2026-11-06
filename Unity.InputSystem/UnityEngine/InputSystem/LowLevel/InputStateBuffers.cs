using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000FB RID: 251
	internal struct InputStateBuffers
	{
		// Token: 0x06000EC9 RID: 3785 RVA: 0x00048E1C File Offset: 0x0004701C
		public InputStateBuffers.DoubleBuffers GetDoubleBuffersFor(InputUpdateType updateType)
		{
			if (updateType - InputUpdateType.Dynamic <= 1 || updateType == InputUpdateType.BeforeRender || updateType == InputUpdateType.Manual)
			{
				return this.m_PlayerStateBuffers;
			}
			throw new ArgumentException("Unrecognized InputUpdateType: " + updateType.ToString(), "updateType");
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x00048E55 File Offset: 0x00047055
		public unsafe static void* GetFrontBufferForDevice(int deviceIndex)
		{
			return InputStateBuffers.s_CurrentBuffers.GetFrontBuffer(deviceIndex);
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x00048E62 File Offset: 0x00047062
		public unsafe static void* GetBackBufferForDevice(int deviceIndex)
		{
			return InputStateBuffers.s_CurrentBuffers.GetBackBuffer(deviceIndex);
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x00048E6F File Offset: 0x0004706F
		public static void SwitchTo(InputStateBuffers buffers, InputUpdateType update)
		{
			InputStateBuffers.s_CurrentBuffers = buffers.GetDoubleBuffersFor(update);
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x00048E80 File Offset: 0x00047080
		public unsafe void AllocateAll(InputDevice[] devices, int deviceCount)
		{
			this.sizePerBuffer = InputStateBuffers.ComputeSizeOfSingleStateBuffer(devices, deviceCount);
			if (this.sizePerBuffer == 0U)
			{
				return;
			}
			this.sizePerBuffer = this.sizePerBuffer.AlignToMultipleOf(4U);
			uint num = (uint)(deviceCount * sizeof(void*) * 2);
			this.totalSize = 0U;
			this.totalSize += this.sizePerBuffer * 2U;
			this.totalSize += num;
			this.totalSize += this.sizePerBuffer * 3U;
			this.m_AllBuffers = UnsafeUtility.Malloc((long)((ulong)this.totalSize), 4, Allocator.Persistent);
			UnsafeUtility.MemClear(this.m_AllBuffers, (long)((ulong)this.totalSize));
			byte* allBuffers = (byte*)this.m_AllBuffers;
			this.m_PlayerStateBuffers = InputStateBuffers.SetUpDeviceToBufferMappings(deviceCount, ref allBuffers, this.sizePerBuffer, num);
			this.defaultStateBuffer = (void*)allBuffers;
			this.noiseMaskBuffer = (void*)(allBuffers + this.sizePerBuffer);
			this.resetMaskBuffer = (void*)(allBuffers + this.sizePerBuffer * 2U);
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x00048F68 File Offset: 0x00047168
		private unsafe static InputStateBuffers.DoubleBuffers SetUpDeviceToBufferMappings(int deviceCount, ref byte* bufferPtr, uint sizePerBuffer, uint mappingTableSizePerBuffer)
		{
			byte* ptr = bufferPtr;
			byte* ptr2 = bufferPtr + sizePerBuffer;
			void** ptr3 = bufferPtr / (IntPtr)sizeof(void*) + sizePerBuffer * 2U;
			bufferPtr += (IntPtr)((UIntPtr)(sizePerBuffer * 2U + mappingTableSizePerBuffer));
			InputStateBuffers.DoubleBuffers doubleBuffers = new InputStateBuffers.DoubleBuffers
			{
				deviceToBufferMapping = ptr3,
				deviceCount = deviceCount
			};
			for (int i = 0; i < deviceCount; i++)
			{
				int num = i;
				doubleBuffers.SetFrontBuffer(num, (void*)ptr);
				doubleBuffers.SetBackBuffer(num, (void*)ptr2);
			}
			return doubleBuffers;
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x00048FD8 File Offset: 0x000471D8
		public void FreeAll()
		{
			if (this.m_AllBuffers != null)
			{
				UnsafeUtility.Free(this.m_AllBuffers, Allocator.Persistent);
				this.m_AllBuffers = null;
			}
			this.m_PlayerStateBuffers = default(InputStateBuffers.DoubleBuffers);
			InputStateBuffers.s_CurrentBuffers = default(InputStateBuffers.DoubleBuffers);
			if (InputStateBuffers.s_DefaultStateBuffer == this.defaultStateBuffer)
			{
				InputStateBuffers.s_DefaultStateBuffer = null;
			}
			this.defaultStateBuffer = null;
			if (InputStateBuffers.s_NoiseMaskBuffer == this.noiseMaskBuffer)
			{
				InputStateBuffers.s_NoiseMaskBuffer = null;
			}
			if (InputStateBuffers.s_ResetMaskBuffer == this.resetMaskBuffer)
			{
				InputStateBuffers.s_ResetMaskBuffer = null;
			}
			this.noiseMaskBuffer = null;
			this.resetMaskBuffer = null;
			this.totalSize = 0U;
			this.sizePerBuffer = 0U;
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x0004907C File Offset: 0x0004727C
		public void MigrateAll(InputDevice[] devices, int deviceCount, InputStateBuffers oldBuffers)
		{
			if (oldBuffers.totalSize > 0U)
			{
				InputStateBuffers.MigrateDoubleBuffer(this.m_PlayerStateBuffers, devices, deviceCount, oldBuffers.m_PlayerStateBuffers);
				InputStateBuffers.MigrateSingleBuffer(this.defaultStateBuffer, devices, deviceCount, oldBuffers.defaultStateBuffer);
				InputStateBuffers.MigrateSingleBuffer(this.noiseMaskBuffer, devices, deviceCount, oldBuffers.noiseMaskBuffer);
				InputStateBuffers.MigrateSingleBuffer(this.resetMaskBuffer, devices, deviceCount, oldBuffers.resetMaskBuffer);
			}
			uint num = 0U;
			for (int i = 0; i < deviceCount; i++)
			{
				InputDevice inputDevice = devices[i];
				uint byteOffset = inputDevice.m_StateBlock.byteOffset;
				if (byteOffset == 4294967295U)
				{
					inputDevice.m_StateBlock.byteOffset = 0U;
					if (num != 0U)
					{
						inputDevice.BakeOffsetIntoStateBlockRecursive(num);
					}
				}
				else
				{
					uint num2 = num - byteOffset;
					if (num2 != 0U)
					{
						inputDevice.BakeOffsetIntoStateBlockRecursive(num2);
					}
				}
				num = InputStateBuffers.NextDeviceOffset(num, inputDevice);
			}
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x00049134 File Offset: 0x00047334
		private unsafe static void MigrateDoubleBuffer(InputStateBuffers.DoubleBuffers newBuffer, InputDevice[] devices, int deviceCount, InputStateBuffers.DoubleBuffers oldBuffer)
		{
			if (!newBuffer.valid)
			{
				return;
			}
			if (!oldBuffer.valid)
			{
				return;
			}
			uint num = 0U;
			for (int i = 0; i < deviceCount; i++)
			{
				InputDevice inputDevice = devices[i];
				if (inputDevice.m_StateBlock.byteOffset == 4294967295U)
				{
					break;
				}
				int deviceIndex = inputDevice.m_DeviceIndex;
				int num2 = i;
				uint alignedSizeInBytes = inputDevice.m_StateBlock.alignedSizeInBytes;
				byte* ptr = (byte*)oldBuffer.GetFrontBuffer(deviceIndex) + inputDevice.m_StateBlock.byteOffset;
				byte* ptr2 = (byte*)oldBuffer.GetBackBuffer(deviceIndex) + inputDevice.m_StateBlock.byteOffset;
				byte* ptr3 = (byte*)newBuffer.GetFrontBuffer(num2) + num;
				void* ptr4 = (void*)((byte*)newBuffer.GetBackBuffer(num2) + num);
				UnsafeUtility.MemCpy((void*)ptr3, (void*)ptr, (long)((ulong)alignedSizeInBytes));
				UnsafeUtility.MemCpy(ptr4, (void*)ptr2, (long)((ulong)alignedSizeInBytes));
				num = InputStateBuffers.NextDeviceOffset(num, inputDevice);
			}
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x000491F8 File Offset: 0x000473F8
		private unsafe static void MigrateSingleBuffer(void* newBuffer, InputDevice[] devices, int deviceCount, void* oldBuffer)
		{
			uint num = 0U;
			for (int i = 0; i < deviceCount; i++)
			{
				InputDevice inputDevice = devices[i];
				if (inputDevice.m_StateBlock.byteOffset == 4294967295U)
				{
					break;
				}
				uint alignedSizeInBytes = inputDevice.m_StateBlock.alignedSizeInBytes;
				byte* ptr = (byte*)oldBuffer + inputDevice.m_StateBlock.byteOffset;
				UnsafeUtility.MemCpy((void*)((byte*)newBuffer + num), (void*)ptr, (long)((ulong)alignedSizeInBytes));
				num = InputStateBuffers.NextDeviceOffset(num, inputDevice);
			}
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x00049258 File Offset: 0x00047458
		private static uint ComputeSizeOfSingleStateBuffer(InputDevice[] devices, int deviceCount)
		{
			uint num = 0U;
			for (int i = 0; i < deviceCount; i++)
			{
				num = InputStateBuffers.NextDeviceOffset(num, devices[i]);
			}
			return num;
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x00049280 File Offset: 0x00047480
		private static uint NextDeviceOffset(uint currentOffset, InputDevice device)
		{
			uint alignedSizeInBytes = device.m_StateBlock.alignedSizeInBytes;
			if (alignedSizeInBytes == 0U)
			{
				throw new ArgumentException(string.Format("Device '{0}' has a zero-size state buffer", device), "device");
			}
			return currentOffset + alignedSizeInBytes.AlignToMultipleOf(4U);
		}

		// Token: 0x0400062D RID: 1581
		public uint sizePerBuffer;

		// Token: 0x0400062E RID: 1582
		public uint totalSize;

		// Token: 0x0400062F RID: 1583
		public unsafe void* defaultStateBuffer;

		// Token: 0x04000630 RID: 1584
		public unsafe void* noiseMaskBuffer;

		// Token: 0x04000631 RID: 1585
		public unsafe void* resetMaskBuffer;

		// Token: 0x04000632 RID: 1586
		private unsafe void* m_AllBuffers;

		// Token: 0x04000633 RID: 1587
		internal InputStateBuffers.DoubleBuffers m_PlayerStateBuffers;

		// Token: 0x04000634 RID: 1588
		internal unsafe static void* s_DefaultStateBuffer;

		// Token: 0x04000635 RID: 1589
		internal unsafe static void* s_NoiseMaskBuffer;

		// Token: 0x04000636 RID: 1590
		internal unsafe static void* s_ResetMaskBuffer;

		// Token: 0x04000637 RID: 1591
		internal static InputStateBuffers.DoubleBuffers s_CurrentBuffers;

		// Token: 0x02000224 RID: 548
		[Serializable]
		internal struct DoubleBuffers
		{
			// Token: 0x170005B4 RID: 1460
			// (get) Token: 0x0600153E RID: 5438 RVA: 0x00062DE1 File Offset: 0x00060FE1
			public bool valid
			{
				get
				{
					return this.deviceToBufferMapping != null;
				}
			}

			// Token: 0x0600153F RID: 5439 RVA: 0x00062DF0 File Offset: 0x00060FF0
			public unsafe void SetFrontBuffer(int deviceIndex, void* ptr)
			{
				if (deviceIndex < this.deviceCount)
				{
					*(IntPtr*)(this.deviceToBufferMapping + (IntPtr)(deviceIndex * 2) * (IntPtr)sizeof(void*) / (IntPtr)sizeof(void*)) = ptr;
				}
			}

			// Token: 0x06001540 RID: 5440 RVA: 0x00062E0F File Offset: 0x0006100F
			public unsafe void SetBackBuffer(int deviceIndex, void* ptr)
			{
				if (deviceIndex < this.deviceCount)
				{
					*(IntPtr*)(this.deviceToBufferMapping + (IntPtr)(deviceIndex * 2 + 1) * (IntPtr)sizeof(void*) / (IntPtr)sizeof(void*)) = ptr;
				}
			}

			// Token: 0x06001541 RID: 5441 RVA: 0x00062E30 File Offset: 0x00061030
			public unsafe void* GetFrontBuffer(int deviceIndex)
			{
				if (deviceIndex < this.deviceCount)
				{
					return *(IntPtr*)(this.deviceToBufferMapping + (IntPtr)(deviceIndex * 2) * (IntPtr)sizeof(void*) / (IntPtr)sizeof(void*));
				}
				return null;
			}

			// Token: 0x06001542 RID: 5442 RVA: 0x00062E51 File Offset: 0x00061051
			public unsafe void* GetBackBuffer(int deviceIndex)
			{
				if (deviceIndex < this.deviceCount)
				{
					return *(IntPtr*)(this.deviceToBufferMapping + (IntPtr)(deviceIndex * 2 + 1) * (IntPtr)sizeof(void*) / (IntPtr)sizeof(void*));
				}
				return null;
			}

			// Token: 0x06001543 RID: 5443 RVA: 0x00062E74 File Offset: 0x00061074
			public unsafe void SwapBuffers(int deviceIndex)
			{
				if (!this.valid)
				{
					return;
				}
				void* frontBuffer = this.GetFrontBuffer(deviceIndex);
				void* backBuffer = this.GetBackBuffer(deviceIndex);
				this.SetFrontBuffer(deviceIndex, backBuffer);
				this.SetBackBuffer(deviceIndex, frontBuffer);
			}

			// Token: 0x04000BC4 RID: 3012
			public unsafe void** deviceToBufferMapping;

			// Token: 0x04000BC5 RID: 3013
			public int deviceCount;
		}
	}
}
