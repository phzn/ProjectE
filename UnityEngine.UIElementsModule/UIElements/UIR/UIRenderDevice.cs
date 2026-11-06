using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Profiling;
using UnityEngine.Rendering;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x0200042D RID: 1069
	internal class UIRenderDevice : IDisposable
	{
		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x060021F3 RID: 8691 RVA: 0x00080105 File Offset: 0x0007E305
		internal uint maxVerticesPerPage { get; } = 65535U;

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x060021F4 RID: 8692 RVA: 0x0008010D File Offset: 0x0007E30D
		// (set) Token: 0x060021F5 RID: 8693 RVA: 0x00080115 File Offset: 0x0007E315
		internal bool breakBatches { get; set; }

		// Token: 0x060021F6 RID: 8694 RVA: 0x00080120 File Offset: 0x0007E320
		static UIRenderDevice()
		{
			Utility.EngineUpdate += UIRenderDevice.OnEngineUpdateGlobal;
			Utility.FlushPendingResources += UIRenderDevice.OnFlushPendingResources;
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x000801E9 File Offset: 0x0007E3E9
		public UIRenderDevice(uint initialVertexCapacity = 0U, uint initialIndexCapacity = 0U)
			: this(initialVertexCapacity, initialIndexCapacity, false)
		{
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x000801F8 File Offset: 0x0007E3F8
		protected UIRenderDevice(uint initialVertexCapacity, uint initialIndexCapacity, bool mockDevice)
		{
			this.m_MockDevice = mockDevice;
			Debug.Assert(!UIRenderDevice.m_SynchronousFree);
			Debug.Assert(true);
			bool flag = UIRenderDevice.m_ActiveDeviceCount++ == 0;
			if (flag)
			{
				bool flag2 = !UIRenderDevice.m_SubscribedToNotifications && !this.m_MockDevice;
				if (flag2)
				{
					Utility.NotifyOfUIREvents(true);
					UIRenderDevice.m_SubscribedToNotifications = true;
				}
			}
			this.m_NextPageVertexCount = Math.Max(initialVertexCapacity, 2048U);
			this.m_LargeMeshVertexCount = this.m_NextPageVertexCount;
			this.m_IndexToVertexCountRatio = initialIndexCapacity / initialVertexCapacity;
			this.m_IndexToVertexCountRatio = Mathf.Max(this.m_IndexToVertexCountRatio, 2f);
			this.m_DeferredFrees = new List<List<UIRenderDevice.AllocToFree>>(4);
			this.m_Updates = new List<List<UIRenderDevice.AllocToUpdate>>(4);
			int num = 0;
			while ((long)num < 4L)
			{
				this.m_DeferredFrees.Add(new List<UIRenderDevice.AllocToFree>());
				this.m_Updates.Add(new List<UIRenderDevice.AllocToUpdate>());
				num++;
			}
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x060021F9 RID: 8697 RVA: 0x0008036C File Offset: 0x0007E56C
		internal static Texture2D defaultShaderInfoTexFloat
		{
			get
			{
				bool flag = UIRenderDevice.s_DefaultShaderInfoTexFloat == null;
				if (flag)
				{
					UIRenderDevice.s_DefaultShaderInfoTexFloat = new Texture2D(64, 64, TextureFormat.RGBAFloat, false);
					UIRenderDevice.s_DefaultShaderInfoTexFloat.name = "DefaultShaderInfoTexFloat";
					UIRenderDevice.s_DefaultShaderInfoTexFloat.hideFlags = HideFlags.HideAndDontSave;
					UIRenderDevice.s_DefaultShaderInfoTexFloat.filterMode = FilterMode.Point;
					UIRenderDevice.s_DefaultShaderInfoTexFloat.SetPixel(UIRVEShaderInfoAllocator.identityTransformTexel.x, UIRVEShaderInfoAllocator.identityTransformTexel.y, UIRVEShaderInfoAllocator.identityTransformRow0Value);
					UIRenderDevice.s_DefaultShaderInfoTexFloat.SetPixel(UIRVEShaderInfoAllocator.identityTransformTexel.x, UIRVEShaderInfoAllocator.identityTransformTexel.y + 1, UIRVEShaderInfoAllocator.identityTransformRow1Value);
					UIRenderDevice.s_DefaultShaderInfoTexFloat.SetPixel(UIRVEShaderInfoAllocator.identityTransformTexel.x, UIRVEShaderInfoAllocator.identityTransformTexel.y + 2, UIRVEShaderInfoAllocator.identityTransformRow2Value);
					UIRenderDevice.s_DefaultShaderInfoTexFloat.SetPixel(UIRVEShaderInfoAllocator.infiniteClipRectTexel.x, UIRVEShaderInfoAllocator.infiniteClipRectTexel.y, UIRVEShaderInfoAllocator.infiniteClipRectValue);
					UIRenderDevice.s_DefaultShaderInfoTexFloat.SetPixel(UIRVEShaderInfoAllocator.fullOpacityTexel.x, UIRVEShaderInfoAllocator.fullOpacityTexel.y, UIRVEShaderInfoAllocator.fullOpacityValue);
					UIRenderDevice.s_DefaultShaderInfoTexFloat.SetPixel(UIRVEShaderInfoAllocator.defaultTextCoreSettingsTexel.x, UIRVEShaderInfoAllocator.defaultTextCoreSettingsTexel.y, Color.white);
					UIRenderDevice.s_DefaultShaderInfoTexFloat.SetPixel(UIRVEShaderInfoAllocator.defaultTextCoreSettingsTexel.x, UIRVEShaderInfoAllocator.defaultTextCoreSettingsTexel.y + 1, Color.clear);
					UIRenderDevice.s_DefaultShaderInfoTexFloat.SetPixel(UIRVEShaderInfoAllocator.defaultTextCoreSettingsTexel.x, UIRVEShaderInfoAllocator.defaultTextCoreSettingsTexel.y + 2, Color.clear);
					UIRenderDevice.s_DefaultShaderInfoTexFloat.SetPixel(UIRVEShaderInfoAllocator.defaultTextCoreSettingsTexel.x, UIRVEShaderInfoAllocator.defaultTextCoreSettingsTexel.y + 3, Color.clear);
					UIRenderDevice.s_DefaultShaderInfoTexFloat.Apply(false, true);
				}
				return UIRenderDevice.s_DefaultShaderInfoTexFloat;
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x060021FA RID: 8698 RVA: 0x0008057C File Offset: 0x0007E77C
		internal static Texture2D defaultShaderInfoTexARGB8
		{
			get
			{
				bool flag = UIRenderDevice.s_DefaultShaderInfoTexARGB8 == null;
				if (flag)
				{
					UIRenderDevice.s_DefaultShaderInfoTexARGB8 = new Texture2D(64, 64, TextureFormat.RGBA32, false);
					UIRenderDevice.s_DefaultShaderInfoTexARGB8.name = "DefaultShaderInfoTexARGB8";
					UIRenderDevice.s_DefaultShaderInfoTexARGB8.hideFlags = HideFlags.HideAndDontSave;
					UIRenderDevice.s_DefaultShaderInfoTexARGB8.filterMode = FilterMode.Point;
					UIRenderDevice.s_DefaultShaderInfoTexARGB8.SetPixel(UIRVEShaderInfoAllocator.fullOpacityTexel.x, UIRVEShaderInfoAllocator.fullOpacityTexel.y, UIRVEShaderInfoAllocator.fullOpacityValue);
					UIRenderDevice.s_DefaultShaderInfoTexARGB8.SetPixel(UIRVEShaderInfoAllocator.defaultTextCoreSettingsTexel.x, UIRVEShaderInfoAllocator.defaultTextCoreSettingsTexel.y, Color.white);
					UIRenderDevice.s_DefaultShaderInfoTexARGB8.SetPixel(UIRVEShaderInfoAllocator.defaultTextCoreSettingsTexel.x, UIRVEShaderInfoAllocator.defaultTextCoreSettingsTexel.y + 1, Color.clear);
					UIRenderDevice.s_DefaultShaderInfoTexARGB8.SetPixel(UIRVEShaderInfoAllocator.defaultTextCoreSettingsTexel.x, UIRVEShaderInfoAllocator.defaultTextCoreSettingsTexel.y + 2, Color.clear);
					UIRenderDevice.s_DefaultShaderInfoTexARGB8.SetPixel(UIRVEShaderInfoAllocator.defaultTextCoreSettingsTexel.x, UIRVEShaderInfoAllocator.defaultTextCoreSettingsTexel.y + 3, Color.clear);
					UIRenderDevice.s_DefaultShaderInfoTexARGB8.Apply(false, true);
				}
				return UIRenderDevice.s_DefaultShaderInfoTexARGB8;
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x060021FB RID: 8699 RVA: 0x000806CC File Offset: 0x0007E8CC
		internal static bool vertexTexturingIsAvailable
		{
			get
			{
				bool flag = UIRenderDevice.s_VertexTexturingIsAvailable == null;
				if (flag)
				{
					Shader shader = Shader.Find(UIRUtility.k_DefaultShaderName);
					Material material = new Material(shader);
					material.hideFlags |= HideFlags.DontSaveInEditor;
					string tag = material.GetTag("UIE_VertexTexturingIsAvailable", false);
					UIRUtility.Destroy(material);
					UIRenderDevice.s_VertexTexturingIsAvailable = new bool?(tag == "1");
				}
				return UIRenderDevice.s_VertexTexturingIsAvailable.Value;
			}
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x060021FC RID: 8700 RVA: 0x00080748 File Offset: 0x0007E948
		internal static bool shaderModelIs35
		{
			get
			{
				bool flag = UIRenderDevice.s_ShaderModelIs35 == null;
				if (flag)
				{
					Shader shader = Shader.Find(UIRUtility.k_DefaultShaderName);
					Material material = new Material(shader);
					material.hideFlags |= HideFlags.DontSaveInEditor;
					string tag = material.GetTag("UIE_ShaderModelIs35", false);
					UIRUtility.Destroy(material);
					UIRenderDevice.s_ShaderModelIs35 = new bool?(tag == "1");
				}
				return UIRenderDevice.s_ShaderModelIs35.Value;
			}
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x000807C4 File Offset: 0x0007E9C4
		private void InitVertexDeclaration()
		{
			VertexAttributeDescriptor[] array = new VertexAttributeDescriptor[]
			{
				new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3, 0),
				new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.UNorm8, 4, 0),
				new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2, 0),
				new VertexAttributeDescriptor(VertexAttribute.TexCoord1, VertexAttributeFormat.UNorm8, 4, 0),
				new VertexAttributeDescriptor(VertexAttribute.TexCoord2, VertexAttributeFormat.UNorm8, 4, 0),
				new VertexAttributeDescriptor(VertexAttribute.TexCoord3, VertexAttributeFormat.UNorm8, 4, 0),
				new VertexAttributeDescriptor(VertexAttribute.TexCoord4, VertexAttributeFormat.UNorm8, 4, 0),
				new VertexAttributeDescriptor(VertexAttribute.TexCoord5, VertexAttributeFormat.UNorm8, 4, 0),
				new VertexAttributeDescriptor(VertexAttribute.TexCoord6, VertexAttributeFormat.Float32, 4, 0),
				new VertexAttributeDescriptor(VertexAttribute.TexCoord7, VertexAttributeFormat.Float32, 1, 0)
			};
			this.m_VertexDecl = Utility.GetVertexDeclaration(array);
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x0008088C File Offset: 0x0007EA8C
		private void CompleteCreation()
		{
			bool flag = this.m_MockDevice || this.fullyCreated;
			if (!flag)
			{
				this.InitVertexDeclaration();
				this.m_Fences = new uint[4];
				this.m_StandardMatProps = new MaterialPropertyBlock();
				this.m_DefaultStencilState = Utility.CreateStencilState(new StencilState
				{
					enabled = true,
					readMask = byte.MaxValue,
					writeMask = byte.MaxValue,
					compareFunctionFront = CompareFunction.Equal,
					passOperationFront = StencilOp.Keep,
					failOperationFront = StencilOp.Keep,
					zFailOperationFront = StencilOp.IncrementSaturate,
					compareFunctionBack = CompareFunction.Less,
					passOperationBack = StencilOp.Keep,
					failOperationBack = StencilOp.Keep,
					zFailOperationBack = StencilOp.DecrementSaturate
				});
			}
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x060021FF RID: 8703 RVA: 0x00080954 File Offset: 0x0007EB54
		private bool fullyCreated
		{
			get
			{
				return this.m_Fences != null;
			}
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06002200 RID: 8704 RVA: 0x0008096F File Offset: 0x0007EB6F
		// (set) Token: 0x06002201 RID: 8705 RVA: 0x00080977 File Offset: 0x0007EB77
		private protected bool disposed { protected get; private set; }

		// Token: 0x06002202 RID: 8706 RVA: 0x00080980 File Offset: 0x0007EB80
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x00080992 File Offset: 0x0007EB92
		internal void DisposeImmediate()
		{
			Debug.Assert(!UIRenderDevice.m_SynchronousFree);
			UIRenderDevice.m_SynchronousFree = true;
			this.Dispose();
			UIRenderDevice.m_SynchronousFree = false;
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x000809B8 File Offset: 0x0007EBB8
		protected virtual void Dispose(bool disposing)
		{
			bool disposed = this.disposed;
			if (!disposed)
			{
				UIRenderDevice.m_ActiveDeviceCount--;
				if (disposing)
				{
					UIRenderDevice.DeviceToFree deviceToFree = new UIRenderDevice.DeviceToFree
					{
						handle = (this.m_MockDevice ? 0U : Utility.InsertCPUFence()),
						page = this.m_FirstPage
					};
					bool flag = deviceToFree.handle == 0U;
					if (flag)
					{
						deviceToFree.Dispose();
					}
					else
					{
						UIRenderDevice.m_DeviceFreeQueue.AddLast(deviceToFree);
						bool synchronousFree = UIRenderDevice.m_SynchronousFree;
						if (synchronousFree)
						{
							UIRenderDevice.ProcessDeviceFreeQueue();
						}
					}
				}
				this.disposed = true;
			}
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x00080A58 File Offset: 0x0007EC58
		public MeshHandle Allocate(uint vertexCount, uint indexCount, out NativeSlice<Vertex> vertexData, out NativeSlice<ushort> indexData, out ushort indexOffset)
		{
			MeshHandle meshHandle = this.m_MeshHandles.Get();
			meshHandle.triangleCount = indexCount / 3U;
			this.Allocate(meshHandle, vertexCount, indexCount, out vertexData, out indexData, false);
			indexOffset = (ushort)meshHandle.allocVerts.start;
			return meshHandle;
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x00080AA0 File Offset: 0x0007ECA0
		public void Update(MeshHandle mesh, uint vertexCount, out NativeSlice<Vertex> vertexData)
		{
			Debug.Assert(mesh.allocVerts.size >= vertexCount);
			bool flag = mesh.allocTime == this.m_FrameIndex;
			if (flag)
			{
				vertexData = mesh.allocPage.vertices.cpuData.Slice<Vertex>((int)mesh.allocVerts.start, (int)vertexCount);
			}
			else
			{
				uint start = mesh.allocVerts.start;
				NativeSlice<ushort> nativeSlice = new NativeSlice<ushort>(mesh.allocPage.indices.cpuData, (int)mesh.allocIndices.start, (int)mesh.allocIndices.size);
				NativeSlice<ushort> nativeSlice2;
				ushort num;
				UIRenderDevice.AllocToUpdate allocToUpdate;
				this.UpdateAfterGPUUsedData(mesh, vertexCount, mesh.allocIndices.size, out vertexData, out nativeSlice2, out num, out allocToUpdate, false);
				int size = (int)mesh.allocIndices.size;
				int num2 = (int)((uint)num - start);
				for (int i = 0; i < size; i++)
				{
					nativeSlice2[i] = (ushort)((int)nativeSlice[i] + num2);
				}
			}
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x00080B9C File Offset: 0x0007ED9C
		public void Update(MeshHandle mesh, uint vertexCount, uint indexCount, out NativeSlice<Vertex> vertexData, out NativeSlice<ushort> indexData, out ushort indexOffset)
		{
			Debug.Assert(mesh.allocVerts.size >= vertexCount);
			Debug.Assert(mesh.allocIndices.size >= indexCount);
			bool flag = mesh.allocTime == this.m_FrameIndex;
			if (flag)
			{
				vertexData = mesh.allocPage.vertices.cpuData.Slice<Vertex>((int)mesh.allocVerts.start, (int)vertexCount);
				indexData = mesh.allocPage.indices.cpuData.Slice<ushort>((int)mesh.allocIndices.start, (int)indexCount);
				indexOffset = (ushort)mesh.allocVerts.start;
				this.UpdateCopyBackIndices(mesh, true);
			}
			else
			{
				UIRenderDevice.AllocToUpdate allocToUpdate;
				this.UpdateAfterGPUUsedData(mesh, vertexCount, indexCount, out vertexData, out indexData, out indexOffset, out allocToUpdate, true);
			}
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x00080C68 File Offset: 0x0007EE68
		private void UpdateCopyBackIndices(MeshHandle mesh, bool copyBackIndices)
		{
			bool flag = mesh.updateAllocID == 0U;
			if (!flag)
			{
				int num = (int)(mesh.updateAllocID - 1U);
				List<UIRenderDevice.AllocToUpdate> list = this.ActiveUpdatesForMeshHandle(mesh);
				UIRenderDevice.AllocToUpdate allocToUpdate = list[num];
				allocToUpdate.copyBackIndices = true;
				list[num] = allocToUpdate;
			}
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x00080CB0 File Offset: 0x0007EEB0
		internal List<UIRenderDevice.AllocToUpdate> ActiveUpdatesForMeshHandle(MeshHandle mesh)
		{
			return this.m_Updates[(int)(mesh.allocTime % (uint)this.m_Updates.Count)];
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x00080CE0 File Offset: 0x0007EEE0
		private bool TryAllocFromPage(Page page, uint vertexCount, uint indexCount, ref Alloc va, ref Alloc ia, bool shortLived)
		{
			va = page.vertices.allocator.Allocate(vertexCount, shortLived);
			bool flag = va.size > 0U;
			if (flag)
			{
				ia = page.indices.allocator.Allocate(indexCount, shortLived);
				bool flag2 = ia.size > 0U;
				if (flag2)
				{
					return true;
				}
				page.vertices.allocator.Free(va);
				va.size = 0U;
			}
			return false;
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x00080D6C File Offset: 0x0007EF6C
		private void Allocate(MeshHandle meshHandle, uint vertexCount, uint indexCount, out NativeSlice<Vertex> vertexData, out NativeSlice<ushort> indexData, bool shortLived)
		{
			Page page = null;
			Alloc alloc = default(Alloc);
			Alloc alloc2 = default(Alloc);
			bool flag = vertexCount <= this.m_LargeMeshVertexCount;
			if (flag)
			{
				bool flag2 = this.m_FirstPage != null;
				if (flag2)
				{
					page = this.m_FirstPage;
					for (;;)
					{
						bool flag3 = this.TryAllocFromPage(page, vertexCount, indexCount, ref alloc, ref alloc2, shortLived) || page.next == null;
						if (flag3)
						{
							break;
						}
						page = page.next;
					}
				}
				else
				{
					this.CompleteCreation();
				}
				bool flag4 = alloc2.size == 0U;
				if (flag4)
				{
					this.m_NextPageVertexCount <<= 1;
					this.m_NextPageVertexCount = Math.Max(this.m_NextPageVertexCount, vertexCount * 2U);
					this.m_NextPageVertexCount = Math.Min(this.m_NextPageVertexCount, this.maxVerticesPerPage);
					uint num = (uint)(this.m_NextPageVertexCount * this.m_IndexToVertexCountRatio + 0.5f);
					num = Math.Max(num, indexCount * 2U);
					Debug.Assert(((page != null) ? page.next : null) == null);
					page = new Page(this.m_NextPageVertexCount, num, 4U, this.m_MockDevice);
					page.next = this.m_FirstPage;
					this.m_FirstPage = page;
					alloc = page.vertices.allocator.Allocate(vertexCount, shortLived);
					alloc2 = page.indices.allocator.Allocate(indexCount, shortLived);
					Debug.Assert(alloc.size > 0U);
					Debug.Assert(alloc2.size > 0U);
				}
			}
			else
			{
				this.CompleteCreation();
				Page page2 = this.m_FirstPage;
				Page page3 = this.m_FirstPage;
				int num2 = int.MaxValue;
				while (page2 != null)
				{
					int num3 = page2.vertices.cpuData.Length - (int)vertexCount;
					int num4 = page2.indices.cpuData.Length - (int)indexCount;
					bool flag5 = page2.isEmpty && num3 >= 0 && num4 >= 0 && num3 < num2;
					if (flag5)
					{
						page = page2;
						num2 = num3;
					}
					page3 = page2;
					page2 = page2.next;
				}
				bool flag6 = page == null;
				if (flag6)
				{
					uint num5 = ((vertexCount > this.maxVerticesPerPage) ? 2U : vertexCount);
					Debug.Assert(vertexCount <= this.maxVerticesPerPage, "Requested Vertex count is above the limit. Alloc will fail.");
					page = new Page(num5, indexCount, 4U, this.m_MockDevice);
					bool flag7 = page3 != null;
					if (flag7)
					{
						page3.next = page;
					}
					else
					{
						this.m_FirstPage = page;
					}
				}
				alloc = page.vertices.allocator.Allocate(vertexCount, shortLived);
				alloc2 = page.indices.allocator.Allocate(indexCount, shortLived);
			}
			Debug.Assert(alloc.size == vertexCount, "Vertices allocated != Vertices requested");
			Debug.Assert(alloc2.size == indexCount, "Indices allocated != Indices requested");
			bool flag8 = alloc.size != vertexCount || alloc2.size != indexCount;
			if (flag8)
			{
				bool flag9 = alloc.handle != null;
				if (flag9)
				{
					page.vertices.allocator.Free(alloc);
				}
				bool flag10 = alloc2.handle != null;
				if (flag10)
				{
					page.vertices.allocator.Free(alloc2);
				}
				alloc2 = default(Alloc);
				alloc = default(Alloc);
			}
			page.vertices.RegisterUpdate(alloc.start, alloc.size);
			page.indices.RegisterUpdate(alloc2.start, alloc2.size);
			vertexData = new NativeSlice<Vertex>(page.vertices.cpuData, (int)alloc.start, (int)alloc.size);
			indexData = new NativeSlice<ushort>(page.indices.cpuData, (int)alloc2.start, (int)alloc2.size);
			meshHandle.allocPage = page;
			meshHandle.allocVerts = alloc;
			meshHandle.allocIndices = alloc2;
			meshHandle.allocTime = this.m_FrameIndex;
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x00081138 File Offset: 0x0007F338
		private void UpdateAfterGPUUsedData(MeshHandle mesh, uint vertexCount, uint indexCount, out NativeSlice<Vertex> vertexData, out NativeSlice<ushort> indexData, out ushort indexOffset, out UIRenderDevice.AllocToUpdate allocToUpdate, bool copyBackIndices)
		{
			UIRenderDevice.AllocToUpdate allocToUpdate2 = default(UIRenderDevice.AllocToUpdate);
			uint nextUpdateID = this.m_NextUpdateID;
			this.m_NextUpdateID = nextUpdateID + 1U;
			allocToUpdate2.id = nextUpdateID;
			allocToUpdate2.allocTime = this.m_FrameIndex;
			allocToUpdate2.meshHandle = mesh;
			allocToUpdate2.copyBackIndices = copyBackIndices;
			allocToUpdate = allocToUpdate2;
			Debug.Assert(this.m_NextUpdateID > 0U);
			bool flag = mesh.updateAllocID == 0U;
			if (flag)
			{
				allocToUpdate.permAllocVerts = mesh.allocVerts;
				allocToUpdate.permAllocIndices = mesh.allocIndices;
				allocToUpdate.permPage = mesh.allocPage;
			}
			else
			{
				int num = (int)(mesh.updateAllocID - 1U);
				List<UIRenderDevice.AllocToUpdate> list = this.m_Updates[(int)(mesh.allocTime % (uint)this.m_Updates.Count)];
				UIRenderDevice.AllocToUpdate allocToUpdate3 = list[num];
				Debug.Assert(allocToUpdate3.id == mesh.updateAllocID);
				allocToUpdate.copyBackIndices |= allocToUpdate3.copyBackIndices;
				allocToUpdate.permAllocVerts = allocToUpdate3.permAllocVerts;
				allocToUpdate.permAllocIndices = allocToUpdate3.permAllocIndices;
				allocToUpdate.permPage = allocToUpdate3.permPage;
				allocToUpdate3.allocTime = uint.MaxValue;
				list[num] = allocToUpdate3;
				List<UIRenderDevice.AllocToFree> list2 = this.m_DeferredFrees[(int)(this.m_FrameIndex % (uint)this.m_DeferredFrees.Count)];
				list2.Add(new UIRenderDevice.AllocToFree
				{
					alloc = mesh.allocVerts,
					page = mesh.allocPage,
					vertices = true
				});
				list2.Add(new UIRenderDevice.AllocToFree
				{
					alloc = mesh.allocIndices,
					page = mesh.allocPage,
					vertices = false
				});
			}
			bool flag2 = this.TryAllocFromPage(mesh.allocPage, vertexCount, indexCount, ref mesh.allocVerts, ref mesh.allocIndices, true);
			if (flag2)
			{
				mesh.allocPage.vertices.RegisterUpdate(mesh.allocVerts.start, mesh.allocVerts.size);
				mesh.allocPage.indices.RegisterUpdate(mesh.allocIndices.start, mesh.allocIndices.size);
			}
			else
			{
				this.Allocate(mesh, vertexCount, indexCount, out vertexData, out indexData, true);
			}
			mesh.triangleCount = indexCount / 3U;
			mesh.updateAllocID = allocToUpdate.id;
			mesh.allocTime = allocToUpdate.allocTime;
			this.m_Updates[(int)((ulong)this.m_FrameIndex % (ulong)((long)this.m_Updates.Count))].Add(allocToUpdate);
			vertexData = new NativeSlice<Vertex>(mesh.allocPage.vertices.cpuData, (int)mesh.allocVerts.start, (int)vertexCount);
			indexData = new NativeSlice<ushort>(mesh.allocPage.indices.cpuData, (int)mesh.allocIndices.start, (int)indexCount);
			indexOffset = (ushort)mesh.allocVerts.start;
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x00081428 File Offset: 0x0007F628
		public void Free(MeshHandle mesh)
		{
			bool flag = mesh.updateAllocID > 0U;
			if (flag)
			{
				int num = (int)(mesh.updateAllocID - 1U);
				List<UIRenderDevice.AllocToUpdate> list = this.m_Updates[(int)(mesh.allocTime % (uint)this.m_Updates.Count)];
				UIRenderDevice.AllocToUpdate allocToUpdate = list[num];
				Debug.Assert(allocToUpdate.id == mesh.updateAllocID);
				List<UIRenderDevice.AllocToFree> list2 = this.m_DeferredFrees[(int)(this.m_FrameIndex % (uint)this.m_DeferredFrees.Count)];
				list2.Add(new UIRenderDevice.AllocToFree
				{
					alloc = allocToUpdate.permAllocVerts,
					page = allocToUpdate.permPage,
					vertices = true
				});
				list2.Add(new UIRenderDevice.AllocToFree
				{
					alloc = allocToUpdate.permAllocIndices,
					page = allocToUpdate.permPage,
					vertices = false
				});
				list2.Add(new UIRenderDevice.AllocToFree
				{
					alloc = mesh.allocVerts,
					page = mesh.allocPage,
					vertices = true
				});
				list2.Add(new UIRenderDevice.AllocToFree
				{
					alloc = mesh.allocIndices,
					page = mesh.allocPage,
					vertices = false
				});
				allocToUpdate.allocTime = uint.MaxValue;
				list[num] = allocToUpdate;
			}
			else
			{
				bool flag2 = mesh.allocTime != this.m_FrameIndex;
				if (flag2)
				{
					int num2 = (int)(this.m_FrameIndex % (uint)this.m_DeferredFrees.Count);
					this.m_DeferredFrees[num2].Add(new UIRenderDevice.AllocToFree
					{
						alloc = mesh.allocVerts,
						page = mesh.allocPage,
						vertices = true
					});
					this.m_DeferredFrees[num2].Add(new UIRenderDevice.AllocToFree
					{
						alloc = mesh.allocIndices,
						page = mesh.allocPage,
						vertices = false
					});
				}
				else
				{
					mesh.allocPage.vertices.allocator.Free(mesh.allocVerts);
					mesh.allocPage.indices.allocator.Free(mesh.allocIndices);
				}
			}
			mesh.allocVerts = default(Alloc);
			mesh.allocIndices = default(Alloc);
			mesh.allocPage = null;
			mesh.updateAllocID = 0U;
			this.m_MeshHandles.Return(mesh);
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x000816B0 File Offset: 0x0007F8B0
		public void OnFrameRenderingBegin()
		{
			this.AdvanceFrame();
			this.m_DrawStats = default(UIRenderDevice.DrawStatistics);
			this.m_DrawStats.currentFrameIndex = (int)this.m_FrameIndex;
			for (Page page = this.m_FirstPage; page != null; page = page.next)
			{
				page.vertices.SendUpdates();
				page.indices.SendUpdates();
			}
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x00081714 File Offset: 0x0007F914
		internal unsafe static NativeSlice<T> PtrToSlice<T>(void* p, int count) where T : struct
		{
			return NativeSliceUnsafeUtility.ConvertExistingDataToNativeSlice<T>(p, UnsafeUtility.SizeOf<T>(), count);
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x00081734 File Offset: 0x0007F934
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ApplyDrawCommandState(RenderChainCommand cmd, int textureSlot, Material newMat, bool newMatDiffers, ref UIRenderDevice.EvaluationState st)
		{
			if (newMatDiffers)
			{
				st.curState.material = newMat;
				st.mustApplyMaterial = true;
			}
			st.curPage = cmd.mesh.allocPage;
			bool flag = cmd.state.texture != TextureId.invalid;
			if (flag)
			{
				bool flag2 = textureSlot < 0;
				if (flag2)
				{
					textureSlot = this.m_TextureSlotManager.FindOldestSlot();
					this.m_TextureSlotManager.Bind(cmd.state.texture, cmd.state.sdfScale, textureSlot, st.stateMatProps);
					st.mustApplyStateBlock = true;
				}
				else
				{
					this.m_TextureSlotManager.MarkUsed(textureSlot);
				}
			}
			bool flag3 = cmd.state.stencilRef != st.curState.stencilRef;
			if (flag3)
			{
				st.curState.stencilRef = cmd.state.stencilRef;
				st.mustApplyStencil = true;
			}
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x00081828 File Offset: 0x0007FA28
		private void ApplyBatchState(ref UIRenderDevice.EvaluationState st, bool allowMaterialChange)
		{
			bool flag = !this.m_MockDevice;
			if (flag)
			{
				bool mustApplyMaterial = st.mustApplyMaterial;
				if (mustApplyMaterial)
				{
					bool flag2 = !allowMaterialChange;
					if (flag2)
					{
						Debug.LogError("Attempted to change material when it is not allowed to do so.");
						return;
					}
					this.m_DrawStats.materialSetCount = this.m_DrawStats.materialSetCount + 1U;
					st.curState.material.SetPass(0);
					bool flag3 = this.m_StandardMatProps != null;
					if (flag3)
					{
						Utility.SetPropertyBlock(this.m_StandardMatProps);
					}
					st.mustApplyCommonBlock = true;
					st.mustApplyStateBlock = true;
					st.mustApplyStencil = true;
				}
				bool mustApplyStateBlock = st.mustApplyStateBlock;
				if (mustApplyStateBlock)
				{
					Utility.SetPropertyBlock(st.stateMatProps);
				}
				bool mustApplyStencil = st.mustApplyStencil;
				if (mustApplyStencil)
				{
					this.m_DrawStats.stencilRefChanges = this.m_DrawStats.stencilRefChanges + 1U;
					Utility.SetStencilState(this.m_DefaultStencilState, st.curState.stencilRef);
				}
			}
			st.mustApplyMaterial = false;
			st.mustApplyCommonBlock = false;
			st.mustApplyStateBlock = false;
			st.mustApplyStencil = false;
			this.m_TextureSlotManager.StartNewBatch();
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x00081934 File Offset: 0x0007FB34
		public unsafe void EvaluateChain(RenderChainCommand head, Material initialMat, Material defaultMat, Texture gradientSettings, Texture shaderInfo, float pixelsPerPoint, NativeSlice<Transform3x4> transforms, NativeSlice<Vector4> clipRects, MaterialPropertyBlock stateMatProps, bool allowMaterialChange, ref Exception immediateException)
		{
			Utility.ProfileDrawChainBegin();
			bool breakBatches = this.breakBatches;
			DrawParams drawParams = this.m_DrawParams;
			drawParams.Reset();
			drawParams.renderTexture.Add(RenderTexture.active);
			stateMatProps.Clear();
			this.m_TextureSlotManager.Reset();
			bool fullyCreated = this.fullyCreated;
			if (fullyCreated)
			{
				bool flag = gradientSettings != null;
				if (flag)
				{
					this.m_StandardMatProps.SetTexture(UIRenderDevice.s_GradientSettingsTexID, gradientSettings);
				}
				bool flag2 = shaderInfo != null;
				if (flag2)
				{
					this.m_StandardMatProps.SetTexture(UIRenderDevice.s_ShaderInfoTexID, shaderInfo);
				}
				bool flag3 = transforms.Length > 0;
				if (flag3)
				{
					Utility.SetVectorArray<Transform3x4>(this.m_StandardMatProps, UIRenderDevice.s_TransformsPropID, transforms);
				}
				bool flag4 = clipRects.Length > 0;
				if (flag4)
				{
					Utility.SetVectorArray<Vector4>(this.m_StandardMatProps, UIRenderDevice.s_ClipRectsPropID, clipRects);
				}
				Utility.SetPropertyBlock(this.m_StandardMatProps);
			}
			int num = 1024;
			DrawBufferRange* ptr;
			checked
			{
				ptr = stackalloc DrawBufferRange[unchecked((UIntPtr)num) * (UIntPtr)sizeof(DrawBufferRange)];
			}
			int num2 = num - 1;
			int num3 = 0;
			int num4 = 0;
			DrawBufferRange drawBufferRange = default(DrawBufferRange);
			int num5 = -1;
			UIRenderDevice.EvaluationState evaluationState = new UIRenderDevice.EvaluationState
			{
				stateMatProps = stateMatProps,
				defaultMat = defaultMat,
				curState = new State
				{
					material = initialMat
				},
				mustApplyCommonBlock = true,
				mustApplyStateBlock = true,
				mustApplyStencil = true
			};
			while (head != null)
			{
				this.m_DrawStats.commandCount = this.m_DrawStats.commandCount + 1U;
				this.m_DrawStats.drawCommandCount = this.m_DrawStats.drawCommandCount + ((head.type == CommandType.Draw) ? 1U : 0U);
				bool flag5 = drawBufferRange.indexCount > 0 && num4 == num - 1;
				bool flag6 = false;
				bool flag7 = false;
				bool flag8 = false;
				int num6 = -1;
				Material material = null;
				bool flag9 = false;
				bool flag10 = head.type == CommandType.Draw;
				if (flag10)
				{
					material = ((head.state.material != null) ? head.state.material : defaultMat);
					bool flag11 = material != evaluationState.curState.material;
					if (flag11)
					{
						flag8 = true;
						flag9 = true;
						flag6 = true;
						flag7 = true;
					}
					bool flag12 = head.mesh.allocPage != evaluationState.curPage;
					if (flag12)
					{
						flag8 = true;
						flag6 = true;
						flag7 = true;
					}
					else
					{
						bool flag13 = (long)num5 != (long)((ulong)head.mesh.allocIndices.start + (ulong)((long)head.indexOffset));
						if (flag13)
						{
							flag6 = true;
						}
					}
					bool flag14 = head.state.texture != TextureId.invalid;
					if (flag14)
					{
						flag8 = true;
						num6 = this.m_TextureSlotManager.IndexOf(head.state.texture);
						bool flag15 = num6 < 0 && this.m_TextureSlotManager.FreeSlots < 1;
						if (flag15)
						{
							flag6 = true;
							flag7 = true;
						}
					}
					bool flag16 = head.state.stencilRef != evaluationState.curState.stencilRef;
					if (flag16)
					{
						flag8 = true;
						flag6 = true;
						flag7 = true;
					}
					bool flag17 = flag6 && flag5;
					if (flag17)
					{
						flag7 = true;
					}
				}
				else
				{
					flag6 = true;
					flag7 = true;
				}
				bool flag18 = breakBatches;
				if (flag18)
				{
					flag6 = true;
					flag7 = true;
				}
				bool flag19 = flag6;
				if (flag19)
				{
					bool flag20 = drawBufferRange.indexCount > 0;
					if (flag20)
					{
						int num7 = (num3 + num4++) & num2;
						ptr[num7] = drawBufferRange;
						Debug.Assert(num4 < num || flag7);
						drawBufferRange = default(DrawBufferRange);
						this.m_DrawStats.drawRangeCount = this.m_DrawStats.drawRangeCount + 1U;
					}
					bool flag21 = head.type == CommandType.Draw;
					if (flag21)
					{
						drawBufferRange.firstIndex = (int)(head.mesh.allocIndices.start + (uint)head.indexOffset);
						drawBufferRange.indexCount = head.indexCount;
						drawBufferRange.vertsReferenced = (int)head.mesh.allocVerts.size;
						drawBufferRange.minIndexVal = (int)head.mesh.allocVerts.start;
						num5 = drawBufferRange.firstIndex + head.indexCount;
						this.m_DrawStats.totalIndices = this.m_DrawStats.totalIndices + (uint)head.indexCount;
					}
					bool flag22 = flag7;
					if (flag22)
					{
						bool flag23 = num4 > 0;
						if (flag23)
						{
							this.ApplyBatchState(ref evaluationState, allowMaterialChange);
							this.KickRanges(ptr, ref num4, ref num3, num, evaluationState.curPage);
						}
						bool flag24 = head.type > CommandType.Draw;
						if (flag24)
						{
							bool flag25 = !this.m_MockDevice;
							if (flag25)
							{
								head.ExecuteNonDrawMesh(drawParams, pixelsPerPoint, ref immediateException);
							}
							bool flag26 = head.type == CommandType.Immediate || head.type == CommandType.ImmediateCull || head.type == CommandType.BlitToPreviousRT || head.type == CommandType.PushRenderTexture || head.type == CommandType.PopDefaultMaterial || head.type == CommandType.PushDefaultMaterial;
							if (flag26)
							{
								evaluationState.curState.material = null;
								evaluationState.mustApplyMaterial = false;
								this.m_DrawStats.immediateDraws = this.m_DrawStats.immediateDraws + 1U;
								bool flag27 = head.type == CommandType.PopDefaultMaterial;
								if (flag27)
								{
									int num8 = drawParams.defaultMaterial.Count - 1;
									defaultMat = drawParams.defaultMaterial[num8];
									drawParams.defaultMaterial.RemoveAt(num8);
								}
								bool flag28 = head.type == CommandType.PushDefaultMaterial;
								if (flag28)
								{
									drawParams.defaultMaterial.Add(defaultMat);
									defaultMat = head.state.material;
								}
							}
						}
					}
					bool flag29 = head.type == CommandType.Draw && flag8;
					if (flag29)
					{
						this.ApplyDrawCommandState(head, num6, material, flag9, ref evaluationState);
					}
					head = head.next;
				}
				else
				{
					bool flag30 = drawBufferRange.indexCount == 0;
					if (flag30)
					{
						num5 = (drawBufferRange.firstIndex = (int)(head.mesh.allocIndices.start + (uint)head.indexOffset));
					}
					drawBufferRange.indexCount += head.indexCount;
					int minIndexVal = drawBufferRange.minIndexVal;
					int start = (int)head.mesh.allocVerts.start;
					int num9 = drawBufferRange.minIndexVal + drawBufferRange.vertsReferenced;
					int num10 = (int)(head.mesh.allocVerts.start + head.mesh.allocVerts.size);
					drawBufferRange.minIndexVal = Mathf.Min(minIndexVal, start);
					drawBufferRange.vertsReferenced = Mathf.Max(num9, num10) - drawBufferRange.minIndexVal;
					num5 += head.indexCount;
					this.m_DrawStats.totalIndices = this.m_DrawStats.totalIndices + (uint)head.indexCount;
					bool flag31 = flag8;
					if (flag31)
					{
						this.ApplyDrawCommandState(head, num6, material, flag9, ref evaluationState);
					}
					head = head.next;
				}
			}
			bool flag32 = drawBufferRange.indexCount > 0;
			if (flag32)
			{
				int num11 = (num3 + num4++) & num2;
				ptr[num11] = drawBufferRange;
			}
			bool flag33 = num4 > 0;
			if (flag33)
			{
				this.ApplyBatchState(ref evaluationState, allowMaterialChange);
				this.KickRanges(ptr, ref num4, ref num3, num, evaluationState.curPage);
			}
			this.UpdateFenceValue();
			Utility.ProfileDrawChainEnd();
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x00082040 File Offset: 0x00080240
		private unsafe void UpdateFenceValue()
		{
			bool flag = this.m_Fences != null;
			if (flag)
			{
				uint num = Utility.InsertCPUFence();
				fixed (uint* ptr = &this.m_Fences[(int)((ulong)this.m_FrameIndex % (ulong)((long)this.m_Fences.Length))])
				{
					uint* ptr2 = ptr;
					bool flag3;
					do
					{
						uint num2 = *ptr2;
						bool flag2 = num - num2 <= 0U;
						if (flag2)
						{
							break;
						}
						int num3 = Interlocked.CompareExchange(ref *(int*)ptr2, (int)num, (int)num2);
						flag3 = (long)num3 == (long)((ulong)num2);
					}
					while (!flag3);
				}
			}
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x000820C0 File Offset: 0x000802C0
		private unsafe void KickRanges(DrawBufferRange* ranges, ref int rangesReady, ref int rangesStart, int rangesCount, Page curPage)
		{
			Debug.Assert(rangesReady > 0);
			bool flag = rangesStart + rangesReady <= rangesCount;
			if (flag)
			{
				bool flag2 = !this.m_MockDevice;
				if (flag2)
				{
					this.DrawRanges<ushort, Vertex>(curPage.indices.gpuData, curPage.vertices.gpuData, UIRenderDevice.PtrToSlice<DrawBufferRange>((void*)(ranges + rangesStart), rangesReady));
				}
				this.m_DrawStats.drawRangeCallCount = this.m_DrawStats.drawRangeCallCount + 1U;
			}
			else
			{
				int num = rangesCount - rangesStart;
				int num2 = rangesReady - num;
				bool flag3 = !this.m_MockDevice;
				if (flag3)
				{
					this.DrawRanges<ushort, Vertex>(curPage.indices.gpuData, curPage.vertices.gpuData, UIRenderDevice.PtrToSlice<DrawBufferRange>((void*)(ranges + rangesStart), num));
					this.DrawRanges<ushort, Vertex>(curPage.indices.gpuData, curPage.vertices.gpuData, UIRenderDevice.PtrToSlice<DrawBufferRange>((void*)ranges, num2));
				}
				this.m_DrawStats.drawRangeCallCount = this.m_DrawStats.drawRangeCallCount + 2U;
			}
			rangesStart = (rangesStart + rangesReady) & (rangesCount - 1);
			rangesReady = 0;
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x000821D4 File Offset: 0x000803D4
		private unsafe void DrawRanges<I, T>(Utility.GPUBuffer<I> ib, Utility.GPUBuffer<T> vb, NativeSlice<DrawBufferRange> ranges) where I : struct where T : struct
		{
			checked
			{
				IntPtr* ptr = stackalloc IntPtr[unchecked((UIntPtr)1) * (UIntPtr)sizeof(IntPtr)];
				*ptr = vb.BufferPointer;
				Utility.DrawRanges(ib.BufferPointer, ptr, 1, new IntPtr(ranges.GetUnsafePtr<DrawBufferRange>()), ranges.Length, this.m_VertexDecl);
			}
		}

		// Token: 0x06002216 RID: 8726 RVA: 0x0008221C File Offset: 0x0008041C
		internal void WaitOnAllCpuFences()
		{
			for (int i = 0; i < this.m_Fences.Length; i++)
			{
				this.WaitOnCpuFence(this.m_Fences[i]);
			}
		}

		// Token: 0x06002217 RID: 8727 RVA: 0x00082250 File Offset: 0x00080450
		private void WaitOnCpuFence(uint fence)
		{
			bool flag = fence != 0U && !Utility.CPUFencePassed(fence);
			if (flag)
			{
				Utility.WaitForCPUFencePassed(fence);
			}
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x0008227C File Offset: 0x0008047C
		public void AdvanceFrame()
		{
			this.m_FrameIndex += 1U;
			this.m_DrawStats.currentFrameIndex = (int)this.m_FrameIndex;
			bool flag = this.m_Fences != null;
			if (flag)
			{
				int num = (int)((ulong)this.m_FrameIndex % (ulong)((long)this.m_Fences.Length));
				uint num2 = this.m_Fences[num];
				this.WaitOnCpuFence(num2);
				this.m_Fences[num] = 0U;
			}
			this.m_NextUpdateID = 1U;
			List<UIRenderDevice.AllocToFree> list = this.m_DeferredFrees[(int)(this.m_FrameIndex % (uint)this.m_DeferredFrees.Count)];
			foreach (UIRenderDevice.AllocToFree allocToFree in list)
			{
				bool vertices = allocToFree.vertices;
				if (vertices)
				{
					allocToFree.page.vertices.allocator.Free(allocToFree.alloc);
				}
				else
				{
					allocToFree.page.indices.allocator.Free(allocToFree.alloc);
				}
			}
			list.Clear();
			List<UIRenderDevice.AllocToUpdate> list2 = this.m_Updates[(int)(this.m_FrameIndex % (uint)this.m_DeferredFrees.Count)];
			foreach (UIRenderDevice.AllocToUpdate allocToUpdate in list2)
			{
				bool flag2 = allocToUpdate.meshHandle.updateAllocID == allocToUpdate.id && allocToUpdate.meshHandle.allocTime == allocToUpdate.allocTime;
				if (flag2)
				{
					NativeSlice<Vertex> nativeSlice = new NativeSlice<Vertex>(allocToUpdate.meshHandle.allocPage.vertices.cpuData, (int)allocToUpdate.meshHandle.allocVerts.start, (int)allocToUpdate.meshHandle.allocVerts.size);
					NativeSlice<Vertex> nativeSlice2 = new NativeSlice<Vertex>(allocToUpdate.permPage.vertices.cpuData, (int)allocToUpdate.permAllocVerts.start, (int)allocToUpdate.meshHandle.allocVerts.size);
					nativeSlice2.CopyFrom(nativeSlice);
					allocToUpdate.permPage.vertices.RegisterUpdate(allocToUpdate.permAllocVerts.start, allocToUpdate.meshHandle.allocVerts.size);
					bool copyBackIndices = allocToUpdate.copyBackIndices;
					if (copyBackIndices)
					{
						NativeSlice<ushort> nativeSlice3 = new NativeSlice<ushort>(allocToUpdate.meshHandle.allocPage.indices.cpuData, (int)allocToUpdate.meshHandle.allocIndices.start, (int)allocToUpdate.meshHandle.allocIndices.size);
						NativeSlice<ushort> nativeSlice4 = new NativeSlice<ushort>(allocToUpdate.permPage.indices.cpuData, (int)allocToUpdate.permAllocIndices.start, (int)allocToUpdate.meshHandle.allocIndices.size);
						int length = nativeSlice4.Length;
						int num3 = (int)(allocToUpdate.permAllocVerts.start - allocToUpdate.meshHandle.allocVerts.start);
						for (int i = 0; i < length; i++)
						{
							nativeSlice4[i] = (ushort)((int)nativeSlice3[i] + num3);
						}
						allocToUpdate.permPage.indices.RegisterUpdate(allocToUpdate.permAllocIndices.start, allocToUpdate.meshHandle.allocIndices.size);
					}
					list.Add(new UIRenderDevice.AllocToFree
					{
						alloc = allocToUpdate.meshHandle.allocVerts,
						page = allocToUpdate.meshHandle.allocPage,
						vertices = true
					});
					list.Add(new UIRenderDevice.AllocToFree
					{
						alloc = allocToUpdate.meshHandle.allocIndices,
						page = allocToUpdate.meshHandle.allocPage,
						vertices = false
					});
					allocToUpdate.meshHandle.allocVerts = allocToUpdate.permAllocVerts;
					allocToUpdate.meshHandle.allocIndices = allocToUpdate.permAllocIndices;
					allocToUpdate.meshHandle.allocPage = allocToUpdate.permPage;
					allocToUpdate.meshHandle.updateAllocID = 0U;
				}
			}
			list2.Clear();
			this.PruneUnusedPages();
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x000826DC File Offset: 0x000808DC
		private void PruneUnusedPages()
		{
			Page page4;
			Page page3;
			Page page2;
			Page page = (page2 = (page3 = (page4 = null)));
			Page next;
			for (Page page5 = this.m_FirstPage; page5 != null; page5 = next)
			{
				bool flag = !page5.isEmpty;
				if (flag)
				{
					page5.framesEmpty = 0;
				}
				else
				{
					page5.framesEmpty++;
				}
				bool flag2 = page5.framesEmpty < 60;
				if (flag2)
				{
					bool flag3 = page2 != null;
					if (flag3)
					{
						page.next = page5;
					}
					else
					{
						page2 = page5;
					}
					page = page5;
				}
				else
				{
					bool flag4 = page3 != null;
					if (flag4)
					{
						page4.next = page5;
					}
					else
					{
						page3 = page5;
					}
					page4 = page5;
				}
				next = page5.next;
				page5.next = null;
			}
			this.m_FirstPage = page2;
			Page next2;
			for (Page page5 = page3; page5 != null; page5 = next2)
			{
				next2 = page5.next;
				page5.next = null;
				page5.Dispose();
			}
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x000827BC File Offset: 0x000809BC
		internal static void PrepareForGfxDeviceRecreate()
		{
			UIRenderDevice.m_ActiveDeviceCount++;
			bool flag = UIRenderDevice.s_DefaultShaderInfoTexFloat != null;
			if (flag)
			{
				UIRUtility.Destroy(UIRenderDevice.s_DefaultShaderInfoTexFloat);
				UIRenderDevice.s_DefaultShaderInfoTexFloat = null;
			}
			bool flag2 = UIRenderDevice.s_DefaultShaderInfoTexARGB8 != null;
			if (flag2)
			{
				UIRUtility.Destroy(UIRenderDevice.s_DefaultShaderInfoTexARGB8);
				UIRenderDevice.s_DefaultShaderInfoTexARGB8 = null;
			}
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x0008281A File Offset: 0x00080A1A
		internal static void WrapUpGfxDeviceRecreate()
		{
			UIRenderDevice.m_ActiveDeviceCount--;
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x00082829 File Offset: 0x00080A29
		internal static void FlushAllPendingDeviceDisposes()
		{
			Utility.SyncRenderThread();
			UIRenderDevice.ProcessDeviceFreeQueue();
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x00082838 File Offset: 0x00080A38
		internal UIRenderDevice.AllocationStatistics GatherAllocationStatistics()
		{
			UIRenderDevice.AllocationStatistics allocationStatistics = default(UIRenderDevice.AllocationStatistics);
			allocationStatistics.completeInit = this.fullyCreated;
			allocationStatistics.freesDeferred = new int[this.m_DeferredFrees.Count];
			for (int i = 0; i < this.m_DeferredFrees.Count; i++)
			{
				allocationStatistics.freesDeferred[i] = this.m_DeferredFrees[i].Count;
			}
			int num = 0;
			for (Page page = this.m_FirstPage; page != null; page = page.next)
			{
				num++;
			}
			allocationStatistics.pages = new UIRenderDevice.AllocationStatistics.PageStatistics[num];
			num = 0;
			for (Page page = this.m_FirstPage; page != null; page = page.next)
			{
				allocationStatistics.pages[num].vertices = page.vertices.allocator.GatherStatistics();
				allocationStatistics.pages[num].indices = page.indices.allocator.GatherStatistics();
				num++;
			}
			return allocationStatistics;
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x00082944 File Offset: 0x00080B44
		internal UIRenderDevice.DrawStatistics GatherDrawStatistics()
		{
			return this.m_DrawStats;
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x0008295C File Offset: 0x00080B5C
		public static void ProcessDeviceFreeQueue()
		{
			bool synchronousFree = UIRenderDevice.m_SynchronousFree;
			if (synchronousFree)
			{
				Utility.SyncRenderThread();
			}
			for (LinkedListNode<UIRenderDevice.DeviceToFree> linkedListNode = UIRenderDevice.m_DeviceFreeQueue.First; linkedListNode != null; linkedListNode = UIRenderDevice.m_DeviceFreeQueue.First)
			{
				bool flag = !Utility.CPUFencePassed(linkedListNode.Value.handle);
				if (flag)
				{
					break;
				}
				linkedListNode.Value.Dispose();
				UIRenderDevice.m_DeviceFreeQueue.RemoveFirst();
			}
			Debug.Assert(!UIRenderDevice.m_SynchronousFree || UIRenderDevice.m_DeviceFreeQueue.Count == 0);
			bool flag2 = UIRenderDevice.m_ActiveDeviceCount == 0 && UIRenderDevice.m_SubscribedToNotifications;
			if (flag2)
			{
				bool flag3 = UIRenderDevice.s_DefaultShaderInfoTexFloat != null;
				if (flag3)
				{
					UIRUtility.Destroy(UIRenderDevice.s_DefaultShaderInfoTexFloat);
					UIRenderDevice.s_DefaultShaderInfoTexFloat = null;
				}
				bool flag4 = UIRenderDevice.s_DefaultShaderInfoTexARGB8 != null;
				if (flag4)
				{
					UIRUtility.Destroy(UIRenderDevice.s_DefaultShaderInfoTexARGB8);
					UIRenderDevice.s_DefaultShaderInfoTexARGB8 = null;
				}
				Utility.NotifyOfUIREvents(false);
				UIRenderDevice.m_SubscribedToNotifications = false;
			}
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x00082A59 File Offset: 0x00080C59
		private static void OnEngineUpdateGlobal()
		{
			UIRenderDevice.ProcessDeviceFreeQueue();
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x00082A62 File Offset: 0x00080C62
		private static void OnFlushPendingResources()
		{
			UIRenderDevice.m_SynchronousFree = true;
			UIRenderDevice.ProcessDeviceFreeQueue();
		}

		// Token: 0x04000E86 RID: 3718
		internal const uint k_MaxQueuedFrameCount = 4U;

		// Token: 0x04000E87 RID: 3719
		internal const int k_PruneEmptyPageFrameCount = 60;

		// Token: 0x04000E88 RID: 3720
		private readonly bool m_MockDevice;

		// Token: 0x04000E89 RID: 3721
		private IntPtr m_DefaultStencilState;

		// Token: 0x04000E8A RID: 3722
		private IntPtr m_VertexDecl;

		// Token: 0x04000E8B RID: 3723
		private Page m_FirstPage;

		// Token: 0x04000E8C RID: 3724
		private uint m_NextPageVertexCount;

		// Token: 0x04000E8D RID: 3725
		private uint m_LargeMeshVertexCount;

		// Token: 0x04000E8E RID: 3726
		private float m_IndexToVertexCountRatio;

		// Token: 0x04000E8F RID: 3727
		private List<List<UIRenderDevice.AllocToFree>> m_DeferredFrees;

		// Token: 0x04000E90 RID: 3728
		private List<List<UIRenderDevice.AllocToUpdate>> m_Updates;

		// Token: 0x04000E91 RID: 3729
		private uint[] m_Fences;

		// Token: 0x04000E92 RID: 3730
		private MaterialPropertyBlock m_StandardMatProps;

		// Token: 0x04000E93 RID: 3731
		private uint m_FrameIndex;

		// Token: 0x04000E94 RID: 3732
		private uint m_NextUpdateID = 1U;

		// Token: 0x04000E95 RID: 3733
		private UIRenderDevice.DrawStatistics m_DrawStats;

		// Token: 0x04000E96 RID: 3734
		private readonly LinkedPool<MeshHandle> m_MeshHandles = new LinkedPool<MeshHandle>(() => new MeshHandle(), delegate(MeshHandle mh)
		{
		}, 10000);

		// Token: 0x04000E97 RID: 3735
		private readonly DrawParams m_DrawParams = new DrawParams();

		// Token: 0x04000E98 RID: 3736
		private readonly TextureSlotManager m_TextureSlotManager = new TextureSlotManager();

		// Token: 0x04000E99 RID: 3737
		private static LinkedList<UIRenderDevice.DeviceToFree> m_DeviceFreeQueue = new LinkedList<UIRenderDevice.DeviceToFree>();

		// Token: 0x04000E9A RID: 3738
		private static int m_ActiveDeviceCount = 0;

		// Token: 0x04000E9B RID: 3739
		private static bool m_SubscribedToNotifications;

		// Token: 0x04000E9C RID: 3740
		private static bool m_SynchronousFree;

		// Token: 0x04000E9D RID: 3741
		private static readonly int s_GradientSettingsTexID = Shader.PropertyToID("_GradientSettingsTex");

		// Token: 0x04000E9E RID: 3742
		private static readonly int s_ShaderInfoTexID = Shader.PropertyToID("_ShaderInfoTex");

		// Token: 0x04000E9F RID: 3743
		private static readonly int s_TransformsPropID = Shader.PropertyToID("_Transforms");

		// Token: 0x04000EA0 RID: 3744
		private static readonly int s_ClipRectsPropID = Shader.PropertyToID("_ClipRects");

		// Token: 0x04000EA1 RID: 3745
		private static ProfilerMarker s_MarkerAllocate = new ProfilerMarker("UIR.Allocate");

		// Token: 0x04000EA2 RID: 3746
		private static ProfilerMarker s_MarkerFree = new ProfilerMarker("UIR.Free");

		// Token: 0x04000EA3 RID: 3747
		private static ProfilerMarker s_MarkerAdvanceFrame = new ProfilerMarker("UIR.AdvanceFrame");

		// Token: 0x04000EA4 RID: 3748
		private static ProfilerMarker s_MarkerFence = new ProfilerMarker("UIR.WaitOnFence");

		// Token: 0x04000EA5 RID: 3749
		private static ProfilerMarker s_MarkerBeforeDraw = new ProfilerMarker("UIR.BeforeDraw");

		// Token: 0x04000EA6 RID: 3750
		private static bool? s_VertexTexturingIsAvailable;

		// Token: 0x04000EA7 RID: 3751
		private const string k_VertexTexturingIsAvailableTag = "UIE_VertexTexturingIsAvailable";

		// Token: 0x04000EA8 RID: 3752
		private const string k_VertexTexturingIsAvailableTrue = "1";

		// Token: 0x04000EA9 RID: 3753
		private static bool? s_ShaderModelIs35;

		// Token: 0x04000EAA RID: 3754
		private const string k_ShaderModelIs35Tag = "UIE_ShaderModelIs35";

		// Token: 0x04000EAB RID: 3755
		private const string k_ShaderModelIs35True = "1";

		// Token: 0x04000EAE RID: 3758
		private static Texture2D s_DefaultShaderInfoTexFloat;

		// Token: 0x04000EAF RID: 3759
		private static Texture2D s_DefaultShaderInfoTexARGB8;

		// Token: 0x0200042E RID: 1070
		internal struct AllocToUpdate
		{
			// Token: 0x04000EB1 RID: 3761
			public uint id;

			// Token: 0x04000EB2 RID: 3762
			public uint allocTime;

			// Token: 0x04000EB3 RID: 3763
			public MeshHandle meshHandle;

			// Token: 0x04000EB4 RID: 3764
			public Alloc permAllocVerts;

			// Token: 0x04000EB5 RID: 3765
			public Alloc permAllocIndices;

			// Token: 0x04000EB6 RID: 3766
			public Page permPage;

			// Token: 0x04000EB7 RID: 3767
			public bool copyBackIndices;
		}

		// Token: 0x0200042F RID: 1071
		private struct AllocToFree
		{
			// Token: 0x04000EB8 RID: 3768
			public Alloc alloc;

			// Token: 0x04000EB9 RID: 3769
			public Page page;

			// Token: 0x04000EBA RID: 3770
			public bool vertices;
		}

		// Token: 0x02000430 RID: 1072
		private struct DeviceToFree
		{
			// Token: 0x06002222 RID: 8738 RVA: 0x00082A74 File Offset: 0x00080C74
			public void Dispose()
			{
				while (this.page != null)
				{
					Page page = this.page;
					this.page = this.page.next;
					page.Dispose();
				}
			}

			// Token: 0x04000EBB RID: 3771
			public uint handle;

			// Token: 0x04000EBC RID: 3772
			public Page page;
		}

		// Token: 0x02000431 RID: 1073
		private struct EvaluationState
		{
			// Token: 0x04000EBD RID: 3773
			public MaterialPropertyBlock stateMatProps;

			// Token: 0x04000EBE RID: 3774
			public Material defaultMat;

			// Token: 0x04000EBF RID: 3775
			public State curState;

			// Token: 0x04000EC0 RID: 3776
			public Page curPage;

			// Token: 0x04000EC1 RID: 3777
			public bool mustApplyMaterial;

			// Token: 0x04000EC2 RID: 3778
			public bool mustApplyCommonBlock;

			// Token: 0x04000EC3 RID: 3779
			public bool mustApplyStateBlock;

			// Token: 0x04000EC4 RID: 3780
			public bool mustApplyStencil;
		}

		// Token: 0x02000432 RID: 1074
		internal struct AllocationStatistics
		{
			// Token: 0x04000EC5 RID: 3781
			public UIRenderDevice.AllocationStatistics.PageStatistics[] pages;

			// Token: 0x04000EC6 RID: 3782
			public int[] freesDeferred;

			// Token: 0x04000EC7 RID: 3783
			public bool completeInit;

			// Token: 0x02000433 RID: 1075
			public struct PageStatistics
			{
				// Token: 0x04000EC8 RID: 3784
				internal HeapStatistics vertices;

				// Token: 0x04000EC9 RID: 3785
				internal HeapStatistics indices;
			}
		}

		// Token: 0x02000434 RID: 1076
		internal struct DrawStatistics
		{
			// Token: 0x04000ECA RID: 3786
			public int currentFrameIndex;

			// Token: 0x04000ECB RID: 3787
			public uint totalIndices;

			// Token: 0x04000ECC RID: 3788
			public uint commandCount;

			// Token: 0x04000ECD RID: 3789
			public uint drawCommandCount;

			// Token: 0x04000ECE RID: 3790
			public uint materialSetCount;

			// Token: 0x04000ECF RID: 3791
			public uint drawRangeCount;

			// Token: 0x04000ED0 RID: 3792
			public uint drawRangeCallCount;

			// Token: 0x04000ED1 RID: 3793
			public uint immediateDraws;

			// Token: 0x04000ED2 RID: 3794
			public uint stencilRefChanges;
		}
	}
}
