using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.Rendering
{
	// Token: 0x0200007B RID: 123
	public sealed class ShaderDebugPrintManager
	{
		// Token: 0x060003CE RID: 974 RVA: 0x000107F4 File Offset: 0x0000E9F4
		private int DebugValueTypeToElemSize(ShaderDebugPrintManager.DebugValueType type)
		{
			switch (type)
			{
			case ShaderDebugPrintManager.DebugValueType.TypeUint:
			case ShaderDebugPrintManager.DebugValueType.TypeInt:
			case ShaderDebugPrintManager.DebugValueType.TypeFloat:
			case ShaderDebugPrintManager.DebugValueType.TypeBool:
				return 1;
			case ShaderDebugPrintManager.DebugValueType.TypeUint2:
			case ShaderDebugPrintManager.DebugValueType.TypeInt2:
			case ShaderDebugPrintManager.DebugValueType.TypeFloat2:
				return 2;
			case ShaderDebugPrintManager.DebugValueType.TypeUint3:
			case ShaderDebugPrintManager.DebugValueType.TypeInt3:
			case ShaderDebugPrintManager.DebugValueType.TypeFloat3:
				return 3;
			case ShaderDebugPrintManager.DebugValueType.TypeUint4:
			case ShaderDebugPrintManager.DebugValueType.TypeInt4:
			case ShaderDebugPrintManager.DebugValueType.TypeFloat4:
				return 4;
			default:
				return 0;
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00010848 File Offset: 0x0000EA48
		private ShaderDebugPrintManager()
		{
			for (int i = 0; i < 4; i++)
			{
				this.m_OutputBuffers.Add(new GraphicsBuffer(GraphicsBuffer.Target.Structured, 16384, 4));
				this.m_ReadbackRequests.Add(default(AsyncGPUReadbackRequest));
			}
			this.m_BufferReadCompleteAction = new Action<AsyncGPUReadbackRequest>(this.BufferReadComplete);
			this.m_OutputAction = new Action<string>(this.DefaultOutput);
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x000108D8 File Offset: 0x0000EAD8
		public static ShaderDebugPrintManager instance
		{
			get
			{
				return ShaderDebugPrintManager.s_Instance;
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x000108E0 File Offset: 0x0000EAE0
		public void SetShaderDebugPrintInputConstants(CommandBuffer cmd, ShaderDebugPrintInput input)
		{
			Vector4 vector = new Vector4(input.pos.x, input.pos.y, (float)(input.leftDown ? 1 : 0), (float)(input.rightDown ? 1 : 0));
			cmd.SetGlobalVector(ShaderDebugPrintManager.m_ShaderPropertyIDInputMouse, vector);
			cmd.SetGlobalInt(ShaderDebugPrintManager.m_ShaderPropertyIDInputFrame, this.m_FrameCounter);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00010948 File Offset: 0x0000EB48
		public void SetShaderDebugPrintBindings(CommandBuffer cmd)
		{
			int num = this.m_FrameCounter % 4;
			if (!this.m_ReadbackRequests[num].done)
			{
				this.m_ReadbackRequests[num].WaitForCompletion();
			}
			cmd.SetRandomWriteTarget(7, this.m_OutputBuffers[num]);
			this.ClearShaderDebugPrintBuffer();
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x000109A4 File Offset: 0x0000EBA4
		private void ClearShaderDebugPrintBuffer()
		{
			if (!this.m_FrameCleared)
			{
				int num = this.m_FrameCounter % 4;
				NativeArray<uint> nativeArray = new NativeArray<uint>(1, Allocator.Temp, NativeArrayOptions.ClearMemory);
				nativeArray[0] = 0U;
				this.m_OutputBuffers[num].SetData<uint>(nativeArray, 0, 0, 1);
				this.m_FrameCleared = true;
			}
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x000109F4 File Offset: 0x0000EBF4
		private unsafe void BufferReadComplete(AsyncGPUReadbackRequest request)
		{
			using (new ProfilingScope(null, ShaderDebugPrintManager.Profiling.BufferReadComplete))
			{
				if (!request.hasError)
				{
					NativeArray<uint> data = request.GetData<uint>(0);
					uint num = data[0];
					if (num >= 16384U)
					{
						num = 16384U;
						Debug.LogWarning("Debug Shader Print Buffer Full!");
					}
					string text = "";
					if (num > 0U)
					{
						text = text + "Frame #" + this.m_FrameCounter.ToString() + ": ";
					}
					uint* unsafePtr = (uint*)data.GetUnsafePtr<uint>();
					int num2 = 1;
					while ((long)num2 < (long)((ulong)num))
					{
						ShaderDebugPrintManager.DebugValueType debugValueType = (ShaderDebugPrintManager.DebugValueType)(data[num2] & 15U);
						if ((data[num2] & 128U) == 128U && (long)(num2 + 1) < (long)((ulong)num))
						{
							uint num3 = data[num2 + 1];
							num2++;
							for (int i = 0; i < 4; i++)
							{
								char c = (char)(num3 & 255U);
								if (c != '\0')
								{
									text += c.ToString();
									num3 >>= 8;
								}
							}
							text += " ";
						}
						int num4 = this.DebugValueTypeToElemSize(debugValueType);
						if ((long)(num2 + num4) > (long)((ulong)num))
						{
							break;
						}
						num2++;
						switch (debugValueType)
						{
						case ShaderDebugPrintManager.DebugValueType.TypeUint:
							text += string.Format("{0}u", data[num2]);
							break;
						case ShaderDebugPrintManager.DebugValueType.TypeInt:
						{
							int num5 = (int)unsafePtr[num2];
							text += num5.ToString();
							break;
						}
						case ShaderDebugPrintManager.DebugValueType.TypeFloat:
						{
							float num6 = *(float*)(unsafePtr + num2);
							text += string.Format("{0}f", num6);
							break;
						}
						case ShaderDebugPrintManager.DebugValueType.TypeUint2:
						{
							uint* ptr = unsafePtr + num2;
							text += string.Format("uint2({0}, {1})", *ptr, ptr[1]);
							break;
						}
						case ShaderDebugPrintManager.DebugValueType.TypeInt2:
						{
							int* ptr2 = (int*)(unsafePtr + num2);
							text += string.Format("int2({0}, {1})", *ptr2, ptr2[1]);
							break;
						}
						case ShaderDebugPrintManager.DebugValueType.TypeFloat2:
						{
							float* ptr3 = (float*)(unsafePtr + num2);
							text += string.Format("float2({0}, {1})", *ptr3, ptr3[1]);
							break;
						}
						case ShaderDebugPrintManager.DebugValueType.TypeUint3:
						{
							uint* ptr4 = unsafePtr + num2;
							text += string.Format("uint3({0}, {1}, {2})", *ptr4, ptr4[1], ptr4[2]);
							break;
						}
						case ShaderDebugPrintManager.DebugValueType.TypeInt3:
						{
							int* ptr5 = (int*)(unsafePtr + num2);
							text += string.Format("int3({0}, {1}, {2})", *ptr5, ptr5[1], ptr5[2]);
							break;
						}
						case ShaderDebugPrintManager.DebugValueType.TypeFloat3:
						{
							float* ptr6 = (float*)(unsafePtr + num2);
							text += string.Format("float3({0}, {1}, {2})", *ptr6, ptr6[1], ptr6[2]);
							break;
						}
						case ShaderDebugPrintManager.DebugValueType.TypeUint4:
						{
							uint* ptr7 = unsafePtr + num2;
							text += string.Format("uint4({0}, {1}, {2}, {3})", new object[]
							{
								*ptr7,
								ptr7[1],
								ptr7[2],
								ptr7[3]
							});
							break;
						}
						case ShaderDebugPrintManager.DebugValueType.TypeInt4:
						{
							int* ptr8 = (int*)(unsafePtr + num2);
							text += string.Format("int4({0}, {1}, {2}, {3})", new object[]
							{
								*ptr8,
								ptr8[1],
								ptr8[2],
								ptr8[3]
							});
							break;
						}
						case ShaderDebugPrintManager.DebugValueType.TypeFloat4:
						{
							float* ptr9 = (float*)(unsafePtr + num2);
							text += string.Format("float4({0}, {1}, {2}, {3})", new object[]
							{
								*ptr9,
								ptr9[1],
								ptr9[2],
								ptr9[3]
							});
							break;
						}
						case ShaderDebugPrintManager.DebugValueType.TypeBool:
							text += ((data[num2] == 0U) ? "False" : "True");
							break;
						default:
							num2 = (int)num;
							break;
						}
						num2 += num4;
						text += " ";
					}
					if (num > 0U)
					{
						this.m_OutputLine = text;
						this.m_OutputAction(text);
					}
				}
				else
				{
					this.m_OutputLine = "Error at read back!";
					this.m_OutputAction("Error at read back!");
				}
			}
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00010ED8 File Offset: 0x0000F0D8
		public void EndFrame()
		{
			int num = this.m_FrameCounter % 4;
			this.m_ReadbackRequests[num] = AsyncGPUReadback.Request(this.m_OutputBuffers[num], this.m_BufferReadCompleteAction);
			this.m_FrameCounter++;
			this.m_FrameCleared = false;
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x00010F26 File Offset: 0x0000F126
		public string outputLine
		{
			get
			{
				return this.m_OutputLine;
			}
		}

		// Token: 0x17000094 RID: 148
		// (set) Token: 0x060003D7 RID: 983 RVA: 0x00010F2E File Offset: 0x0000F12E
		public Action<string> outputAction
		{
			set
			{
				this.m_OutputAction = value;
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00010F37 File Offset: 0x0000F137
		public void DefaultOutput(string line)
		{
			Debug.Log(line);
		}

		// Token: 0x04000219 RID: 537
		private static readonly ShaderDebugPrintManager s_Instance = new ShaderDebugPrintManager();

		// Token: 0x0400021A RID: 538
		private const int k_DebugUAVSlot = 7;

		// Token: 0x0400021B RID: 539
		private const int k_FramesInFlight = 4;

		// Token: 0x0400021C RID: 540
		private const int k_MaxBufferElements = 16384;

		// Token: 0x0400021D RID: 541
		private List<GraphicsBuffer> m_OutputBuffers = new List<GraphicsBuffer>();

		// Token: 0x0400021E RID: 542
		private List<AsyncGPUReadbackRequest> m_ReadbackRequests = new List<AsyncGPUReadbackRequest>();

		// Token: 0x0400021F RID: 543
		private Action<AsyncGPUReadbackRequest> m_BufferReadCompleteAction;

		// Token: 0x04000220 RID: 544
		private int m_FrameCounter;

		// Token: 0x04000221 RID: 545
		private bool m_FrameCleared;

		// Token: 0x04000222 RID: 546
		private string m_OutputLine = "";

		// Token: 0x04000223 RID: 547
		private Action<string> m_OutputAction;

		// Token: 0x04000224 RID: 548
		private static readonly int m_ShaderPropertyIDInputMouse = Shader.PropertyToID("_ShaderDebugPrintInputMouse");

		// Token: 0x04000225 RID: 549
		private static readonly int m_ShaderPropertyIDInputFrame = Shader.PropertyToID("_ShaderDebugPrintInputFrame");

		// Token: 0x04000226 RID: 550
		private const uint k_TypeHasTag = 128U;

		// Token: 0x02000190 RID: 400
		private static class Profiling
		{
			// Token: 0x0400066E RID: 1646
			public static readonly ProfilingSampler BufferReadComplete = new ProfilingSampler("ShaderDebugPrintManager.BufferReadComplete");
		}

		// Token: 0x02000191 RID: 401
		private enum DebugValueType
		{
			// Token: 0x04000670 RID: 1648
			TypeUint = 1,
			// Token: 0x04000671 RID: 1649
			TypeInt,
			// Token: 0x04000672 RID: 1650
			TypeFloat,
			// Token: 0x04000673 RID: 1651
			TypeUint2,
			// Token: 0x04000674 RID: 1652
			TypeInt2,
			// Token: 0x04000675 RID: 1653
			TypeFloat2,
			// Token: 0x04000676 RID: 1654
			TypeUint3,
			// Token: 0x04000677 RID: 1655
			TypeInt3,
			// Token: 0x04000678 RID: 1656
			TypeFloat3,
			// Token: 0x04000679 RID: 1657
			TypeUint4,
			// Token: 0x0400067A RID: 1658
			TypeInt4,
			// Token: 0x0400067B RID: 1659
			TypeFloat4,
			// Token: 0x0400067C RID: 1660
			TypeBool
		}
	}
}
