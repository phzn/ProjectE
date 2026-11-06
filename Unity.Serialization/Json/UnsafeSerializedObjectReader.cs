using System;
using System.IO;
using System.Runtime.InteropServices;
using AOT;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;

namespace Unity.Serialization.Json
{
	// Token: 0x02000056 RID: 86
	[BurstCompile]
	internal struct UnsafeSerializedObjectReader : IDisposable
	{
		// Token: 0x060001FB RID: 507 RVA: 0x00008684 File Offset: 0x00006884
		internal unsafe static void CreateBurstDelegates()
		{
			if (UnsafeSerializedObjectReader.s_GetNextBlockDelegateReference == null)
			{
				UnsafeSerializedObjectReader.GetNextBlockDelegate getNextBlockDelegate = new UnsafeSerializedObjectReader.GetNextBlockDelegate(UnsafeSerializedObjectReader.GetNextBlockDelegateInMono);
				UnsafeSerializedObjectReader.s_GetNextBlockDelegateReference = getNextBlockDelegate;
				*UnsafeSerializedObjectReader.s_GetNextBlockTrampoline.Data = Marshal.GetFunctionPointerForDelegate<UnsafeSerializedObjectReader.GetNextBlockDelegate>(getNextBlockDelegate);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001FC RID: 508 RVA: 0x000086BC File Offset: 0x000068BC
		// (set) Token: 0x060001FD RID: 509 RVA: 0x000086C4 File Offset: 0x000068C4
		public bool RequiresExplicitExceptionHandling
		{
			get
			{
				return this.m_RequiresExplicitExceptionHandling;
			}
			set
			{
				this.m_RequiresExplicitExceptionHandling = value;
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000839A File Offset: 0x0000659A
		private static PackedBinaryStream OpenBinaryStreamWithConfiguration(SerializedObjectReaderConfiguration configuration, Allocator label)
		{
			if (configuration.TokenBufferSize < 16)
			{
				throw new ArgumentException("TokenBufferSize < 16");
			}
			if (configuration.OutputBufferSize < 16)
			{
				throw new ArgumentException("OutputBufferSize < 16");
			}
			return new PackedBinaryStream(configuration.TokenBufferSize, configuration.OutputBufferSize, label);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x000086CD File Offset: 0x000068CD
		public UnsafeSerializedObjectReader(Stream input, SerializedObjectReaderConfiguration configuration, Allocator label = Allocator.TempJob, bool leaveInputOpen = true)
		{
			this = new UnsafeSerializedObjectReader(input, UnsafeSerializedObjectReader.OpenBinaryStreamWithConfiguration(configuration, label), configuration, label, leaveInputOpen, false);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x000086E2 File Offset: 0x000068E2
		public UnsafeSerializedObjectReader(Stream input, PackedBinaryStream output, SerializedObjectReaderConfiguration configuration, Allocator label = Allocator.TempJob, bool leaveInputOpen = true)
		{
			this = new UnsafeSerializedObjectReader(input, output, configuration, label, leaveInputOpen, true);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x000086F4 File Offset: 0x000068F4
		internal UnsafeSerializedObjectReader(Stream input, PackedBinaryStream output, SerializedObjectReaderConfiguration configuration, Allocator label = Allocator.TempJob, bool leaveInputOpen = true, bool leaveOutputOpen = true)
		{
			if (configuration.BlockBufferSize < 16)
			{
				throw new ArgumentException("BlockBufferSize < 16");
			}
			if (configuration.TokenBufferSize < 16)
			{
				throw new ArgumentException("TokenBufferSize < 16");
			}
			this.m_LeaveOutputOpen = leaveOutputOpen;
			this.m_JsonTokenStream = new JsonTokenStream(configuration.TokenBufferSize, label);
			this.m_JsonTokenizer = new JsonTokenizer(label);
			this.m_JsonValidator = default(JsonValidator);
			JsonValidationType validationType = configuration.ValidationType;
			if (validationType - JsonValidationType.Standard <= 1)
			{
				this.m_JsonValidator = new JsonValidator(configuration.ValidationType, label);
			}
			this.m_NodeParser = new NodeParser(this.m_JsonTokenStream, configuration.NodeBufferSize, label);
			this.m_PackedBinaryStream = output;
			this.m_PackedBinaryWriter = new PackedBinaryWriter(this.m_JsonTokenStream, this.m_PackedBinaryStream, label)
			{
				StripStringEscapeCharacters = configuration.StripStringEscapeCharacters
			};
			IUnsafeStreamBlockReader unsafeStreamBlockReader2;
			if (!configuration.UseReadAsync)
			{
				IUnsafeStreamBlockReader unsafeStreamBlockReader = new SyncBlockReader(input, configuration.BlockBufferSize, leaveInputOpen);
				unsafeStreamBlockReader2 = unsafeStreamBlockReader;
			}
			else
			{
				IUnsafeStreamBlockReader unsafeStreamBlockReader = new AsyncBlockReader(input, configuration.BlockBufferSize, leaveInputOpen);
				unsafeStreamBlockReader2 = unsafeStreamBlockReader;
			}
			IUnsafeStreamBlockReader unsafeStreamBlockReader3 = unsafeStreamBlockReader2;
			this.m_StreamBlockReader = GCHandle.Alloc(unsafeStreamBlockReader3);
			this.m_InitialBlock = default(UnsafeBuffer<char>);
			this.m_CurrentBlock = default(UnsafeBuffer<char>);
			this.m_CurrentBlockIsFinal = false;
			this.m_IsInvalid = false;
			this.m_ValidationResult = default(JsonValidationResult);
			this.m_RequiresExplicitExceptionHandling = false;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00008838 File Offset: 0x00006A38
		public unsafe UnsafeSerializedObjectReader(char* buffer, int length, SerializedObjectReaderConfiguration configuration, Allocator label = Allocator.TempJob)
		{
			this = new UnsafeSerializedObjectReader(buffer, length, UnsafeSerializedObjectReader.OpenBinaryStreamWithConfiguration(configuration, label), configuration, label);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000884D File Offset: 0x00006A4D
		public unsafe UnsafeSerializedObjectReader(char* buffer, int length, PackedBinaryStream output, SerializedObjectReaderConfiguration configuration, Allocator label = Allocator.TempJob)
		{
			this = new UnsafeSerializedObjectReader(buffer, length, output, configuration, label, true);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00008860 File Offset: 0x00006A60
		internal unsafe UnsafeSerializedObjectReader(char* buffer, int length, PackedBinaryStream output, SerializedObjectReaderConfiguration configuration, Allocator label = Allocator.TempJob, bool leaveOutputOpen = true)
		{
			if (configuration.BlockBufferSize < 16)
			{
				throw new ArgumentException("BlockBufferSize < 16");
			}
			if (configuration.TokenBufferSize < 16)
			{
				throw new ArgumentException("TokenBufferSize < 16");
			}
			this.m_LeaveOutputOpen = leaveOutputOpen;
			this.m_JsonTokenStream = new JsonTokenStream(configuration.TokenBufferSize, label);
			this.m_JsonTokenizer = new JsonTokenizer(label);
			this.m_JsonValidator = default(JsonValidator);
			JsonValidationType validationType = configuration.ValidationType;
			if (validationType - JsonValidationType.Standard <= 1)
			{
				this.m_JsonValidator = new JsonValidator(configuration.ValidationType, label);
			}
			this.m_NodeParser = new NodeParser(this.m_JsonTokenStream, configuration.NodeBufferSize, label);
			this.m_PackedBinaryStream = output;
			this.m_PackedBinaryWriter = new PackedBinaryWriter(this.m_JsonTokenStream, this.m_PackedBinaryStream, label)
			{
				StripStringEscapeCharacters = configuration.StripStringEscapeCharacters
			};
			this.m_StreamBlockReader = default(GCHandle);
			this.m_InitialBlock = new UnsafeBuffer<char>(buffer, length);
			this.m_CurrentBlock = default(UnsafeBuffer<char>);
			this.m_CurrentBlockIsFinal = false;
			this.m_IsInvalid = false;
			this.m_ValidationResult = default(JsonValidationResult);
			this.m_RequiresExplicitExceptionHandling = false;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00008984 File Offset: 0x00006B84
		public void Dispose()
		{
			this.m_JsonTokenStream.Dispose();
			this.m_JsonTokenizer.Dispose();
			this.m_JsonValidator.Dispose();
			this.m_NodeParser.Dispose();
			if (!this.m_LeaveOutputOpen)
			{
				this.m_PackedBinaryStream.Dispose();
			}
			this.m_PackedBinaryWriter.Dispose();
			this.DisposeManagedResources();
		}

		// Token: 0x06000206 RID: 518 RVA: 0x000089E1 File Offset: 0x00006BE1
		[BurstDiscard]
		private void DisposeManagedResources()
		{
			if (this.m_StreamBlockReader.IsAllocated)
			{
				IDisposable disposable = this.m_StreamBlockReader.Target as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				this.m_StreamBlockReader.Free();
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00008A18 File Offset: 0x00006C18
		public void Reset()
		{
			this.m_JsonTokenStream.Reset();
			this.m_JsonTokenizer.Reset();
			this.m_JsonValidator.Reset();
			this.m_NodeParser.Reset();
			this.m_PackedBinaryStream.Clear();
			this.m_PackedBinaryWriter.Seek(0, -1);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00008A69 File Offset: 0x00006C69
		public unsafe void SetSource(char* ptr, int length)
		{
			this.Reset();
			this.m_InitialBlock = new UnsafeBuffer<char>(ptr, length);
			this.m_CurrentBlock = default(UnsafeBuffer<char>);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00008A8A File Offset: 0x00006C8A
		public void DiscardCompleted()
		{
			this.m_PackedBinaryStream.DiscardCompleted();
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00008A97 File Offset: 0x00006C97
		public NodeType Step(NodeType node = NodeType.Any)
		{
			this.ReadInternal(node, -2);
			return this.m_NodeParser.NodeType;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00008AAE File Offset: 0x00006CAE
		public NodeType Step(out SerializedValueView view, NodeType node = NodeType.Any)
		{
			view = this.ReadInternal(node, -2);
			return this.m_NodeParser.NodeType;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00008ACC File Offset: 0x00006CCC
		public NodeType Read()
		{
			SerializedValueView serializedValueView;
			return this.Read(out serializedValueView, NodeType.Any);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00008AE2 File Offset: 0x00006CE2
		public NodeType Read(NodeType node = NodeType.Any)
		{
			this.ReadInternal(node, this.m_NodeParser.TokenParentIndex);
			return this.m_NodeParser.NodeType;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00008B02 File Offset: 0x00006D02
		public NodeType Read(out SerializedValueView view, NodeType node = NodeType.Any)
		{
			view = this.ReadInternal(node, this.m_NodeParser.TokenParentIndex);
			return this.m_NodeParser.NodeType;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00008B28 File Offset: 0x00006D28
		public SerializedObjectView ReadObject()
		{
			this.FillBuffers();
			if (!this.CheckNextTokenType(TokenType.Object))
			{
				throw new InvalidOperationException(string.Format("Invalid token read Expected=[{0}] but Received=[{1}]", TokenType.Object, this.GetNextTokenType()));
			}
			SerializedValueView serializedValueView;
			this.Read(out serializedValueView, NodeType.Any);
			return serializedValueView.AsObjectView();
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00008B78 File Offset: 0x00006D78
		public SerializedMemberView ReadMember()
		{
			this.FillBuffers();
			TokenType nextTokenType = this.GetNextTokenType();
			if (nextTokenType != TokenType.String && nextTokenType != TokenType.Primitive)
			{
				throw new InvalidOperationException(string.Format("Invalid token read Expected=[{0}|{1}] but Received=[{2}]", TokenType.String, TokenType.Primitive, this.GetNextTokenType()));
			}
			SerializedValueView serializedValueView;
			this.Read(out serializedValueView, NodeType.Any);
			return serializedValueView.AsMemberView();
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00008BD3 File Offset: 0x00006DD3
		public void ReadMember(SerializedMemberViewCollection collection)
		{
			collection.Add(this.ReadMember());
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00008BE4 File Offset: 0x00006DE4
		public bool ReadArrayElement(out SerializedValueView view)
		{
			if (!this.CheckArrayElement())
			{
				view = default(SerializedValueView);
				return false;
			}
			view = this.ReadInternal(NodeType.Any, this.m_NodeParser.TokenParentIndex);
			if (this.m_NodeParser.NodeType != NodeType.Any)
			{
				return true;
			}
			view = default(SerializedValueView);
			return false;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00008C32 File Offset: 0x00006E32
		public unsafe int ReadArrayElementBatch(SerializedValueView* views, int count)
		{
			if (!this.CheckArrayElement())
			{
				return 0;
			}
			count = this.ReadInternalBatch(views, count, NodeType.Any, this.m_NodeParser.TokenParentIndex);
			return count;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00008C58 File Offset: 0x00006E58
		private unsafe SerializedValueView ReadInternal(NodeType type, int parent)
		{
			int num;
			int num2;
			do
			{
				if (this.FillBuffers() && parent >= 0)
				{
					parent = this.m_JsonTokenStream.Discard[parent];
				}
				num = this.m_NodeParser.TokenNextIndex - 1;
				num2 = this.m_PackedBinaryStream.TokenNextIndex - 1;
				this.m_NodeParser.Step(type, parent);
				this.m_PackedBinaryWriter.Write(this.m_CurrentBlock, this.m_NodeParser.TokenNextIndex - this.m_PackedBinaryWriter.TokenNextIndex);
			}
			while (this.m_NodeParser.NodeType == NodeType.None && !UnsafeSerializedObjectReader.IsEmpty(this.m_CurrentBlock));
			int node = this.m_NodeParser.Node;
			if (node != -1)
			{
				return this.m_PackedBinaryStream.GetView(this.GetViewIndex(node, num, num2));
			}
			return default(SerializedValueView);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00008D24 File Offset: 0x00006F24
		private unsafe int ReadInternalBatch(SerializedValueView* views, int count, NodeType type, int parent)
		{
			int num = 0;
			do
			{
				if (this.FillBuffers() && parent >= 0)
				{
					parent = this.m_JsonTokenStream.Discard[parent];
				}
				int num2 = this.m_NodeParser.TokenNextIndex - 1;
				int num3 = this.m_PackedBinaryStream.TokenNextIndex - 1;
				int num4 = this.m_NodeParser.StepBatch(count - num, type, parent);
				this.m_PackedBinaryWriter.Write(this.m_CurrentBlock, this.m_NodeParser.TokenNextIndex - this.m_PackedBinaryWriter.TokenNextIndex);
				for (int i = 0; i < num4; i++)
				{
					views[num + i] = this.m_PackedBinaryStream.GetView(this.GetViewIndex(this.m_NodeParser.Nodes[i], num2, num3));
				}
				num += num4;
			}
			while (this.m_NodeParser.NodeType == NodeType.None && !UnsafeSerializedObjectReader.IsEmpty(this.m_CurrentBlock));
			return num;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00008E15 File Offset: 0x00007015
		private static bool IsEmpty(UnsafeBuffer<char> buffer)
		{
			return buffer.Buffer == null || buffer.Length == 0;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00008E2C File Offset: 0x0000702C
		private unsafe bool FillBuffers()
		{
			if (this.m_NodeParser.TokenNextIndex < this.m_JsonTokenStream.TokenNextIndex || this.m_NodeParser.NodeType != NodeType.None)
			{
				return false;
			}
			if (this.m_StreamBlockReader.IsAllocated)
			{
				bool flag = false;
				this.TryGetNextBlock(ref flag);
				if (!flag)
				{
					fixed (UnsafeSerializedObjectReader* ptr = &this)
					{
						void* ptr2 = (void*)ptr;
						new FunctionPointer<UnsafeSerializedObjectReader.GetNextBlockDelegate>(*UnsafeSerializedObjectReader.s_GetNextBlockTrampoline.Data).Invoke((IntPtr)ptr2);
					}
				}
			}
			else
			{
				this.m_CurrentBlock = this.m_InitialBlock;
				this.m_InitialBlock = default(UnsafeBuffer<char>);
				this.m_CurrentBlockIsFinal = true;
			}
			if (UnsafeSerializedObjectReader.IsEmpty(this.m_CurrentBlock))
			{
				this.m_CurrentBlock = default(UnsafeBuffer<char>);
				this.m_JsonTokenizer.Write(this.m_JsonTokenStream, this.m_CurrentBlock, 0, 0, true);
				this.Validate();
				return false;
			}
			this.m_JsonTokenStream.DiscardCompleted(128);
			this.m_NodeParser.Seek(this.m_JsonTokenStream.TokenNextIndex, this.m_JsonTokenStream.TokenParentIndex);
			this.m_PackedBinaryWriter.Seek(this.m_JsonTokenStream.TokenNextIndex, (this.m_PackedBinaryWriter.TokenParentIndex != -1) ? this.m_JsonTokenStream.Discard[this.m_PackedBinaryWriter.TokenParentIndex] : (-1));
			this.m_JsonTokenizer.Write(this.m_JsonTokenStream, this.m_CurrentBlock, 0, this.m_CurrentBlock.Length, this.m_CurrentBlockIsFinal);
			this.Validate();
			return true;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00008FA8 File Offset: 0x000071A8
		[BurstDiscard]
		private void TryGetNextBlock(ref bool result)
		{
			result = true;
			BlockInfo nextBlock = (this.m_StreamBlockReader.Target as IUnsafeStreamBlockReader).GetNextBlock();
			this.m_CurrentBlock = nextBlock.Block;
			this.m_CurrentBlockIsFinal = nextBlock.IsFinal;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00008FE8 File Offset: 0x000071E8
		[BurstDiscard]
		[MonoPInvokeCallback(typeof(UnsafeSerializedObjectReader.GetNextBlockDelegate))]
		private unsafe static void GetNextBlockDelegateInMono(IntPtr target)
		{
			try
			{
				UnsafeSerializedObjectReader* ptr = (UnsafeSerializedObjectReader*)target.ToPointer();
				BlockInfo nextBlock = (ptr->m_StreamBlockReader.Target as IUnsafeStreamBlockReader).GetNextBlock();
				ptr->m_CurrentBlock = nextBlock.Block;
				ptr->m_CurrentBlockIsFinal = nextBlock.IsFinal;
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00009044 File Offset: 0x00007244
		private unsafe void Validate()
		{
			if (!this.m_JsonValidator.IsCreated)
			{
				if (this.m_JsonTokenizer.ResultCode == -1)
				{
					this.m_IsInvalid = true;
					if (!this.m_RequiresExplicitExceptionHandling)
					{
						this.CheckAndThrowInvalidJsonException();
					}
				}
				return;
			}
			this.m_ValidationResult = this.m_JsonValidator.Validate(this.m_CurrentBlock, 0, this.m_CurrentBlock.Length);
			if (!this.m_ValidationResult.IsValid() && (this.m_ValidationResult.ActualType != JsonType.EOF || this.m_CurrentBlockIsFinal) && (this.m_JsonTokenStream.TokenNextIndex != 1 || this.m_JsonTokenStream.Tokens->Type != TokenType.Primitive))
			{
				this.m_IsInvalid = true;
				if (!this.m_RequiresExplicitExceptionHandling)
				{
					this.CheckAndThrowInvalidJsonException();
				}
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00009104 File Offset: 0x00007304
		[BurstDiscard]
		public void CheckAndThrowInvalidJsonException()
		{
			if (!this.m_IsInvalid)
			{
				return;
			}
			if (this.m_JsonValidator.IsCreated)
			{
				throw new InvalidJsonException(this.m_ValidationResult)
				{
					Line = this.m_ValidationResult.LineCount,
					Character = this.m_ValidationResult.CharCount
				};
			}
			throw new InvalidJsonException("Input json was structurally invalid. Try with JsonValidationType=[Standard or Simple]")
			{
				Line = -1,
				Character = -1
			};
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00009170 File Offset: 0x00007370
		private unsafe int GetViewIndex(int node, int inputTokenStart, int binaryTokenStart)
		{
			if (node == -1)
			{
				return -1;
			}
			UnsafePackedBinaryStream* unsafePtr = this.m_PackedBinaryStream.GetUnsafePtr();
			if (node >= inputTokenStart)
			{
				int num = this.m_NodeParser.TokenNextIndex - node;
				return unsafePtr->TokenNextIndex - num;
			}
			int num2 = binaryTokenStart;
			BinaryToken* tokens = this.m_PackedBinaryStream.GetUnsafePtr()->Tokens;
			while (inputTokenStart != node)
			{
				if (inputTokenStart == -1)
				{
					throw new IndexOutOfRangeException();
				}
				inputTokenStart = this.m_JsonTokenStream.Tokens[inputTokenStart].Parent;
				num2 = tokens[num2].Parent;
			}
			return num2;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x000091FB File Offset: 0x000073FB
		private unsafe TokenType GetNextTokenType()
		{
			if (this.m_NodeParser.TokenNextIndex < this.m_JsonTokenStream.TokenNextIndex)
			{
				return this.m_JsonTokenStream.Tokens[this.m_NodeParser.TokenNextIndex].Type;
			}
			return TokenType.Undefined;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000923C File Offset: 0x0000743C
		private unsafe bool CheckNextTokenType(TokenType type)
		{
			return this.m_NodeParser.TokenNextIndex < this.m_JsonTokenStream.TokenNextIndex && this.m_JsonTokenStream.Tokens[this.m_NodeParser.TokenNextIndex].Type == type;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000928C File Offset: 0x0000748C
		private unsafe bool CheckArrayElement()
		{
			return this.m_NodeParser.Node != -1 && this.CheckNextTokenParent((this.m_NodeParser.NodeType == NodeType.BeginArray) ? this.m_NodeParser.Node : this.m_JsonTokenStream.Tokens[this.m_NodeParser.Node].Parent);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000092F0 File Offset: 0x000074F0
		private unsafe bool CheckNextTokenParent(int parent)
		{
			return this.m_NodeParser.TokenNextIndex < this.m_JsonTokenStream.TokenNextIndex && this.m_JsonTokenStream.Tokens[this.m_NodeParser.TokenNextIndex].Parent == parent;
		}

		// Token: 0x040000FE RID: 254
		private static readonly SharedStatic<IntPtr> s_GetNextBlockTrampoline = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -4300240374171355993L, 0L);

		// Token: 0x040000FF RID: 255
		private static object s_GetNextBlockDelegateReference;

		// Token: 0x04000100 RID: 256
		private readonly bool m_LeaveOutputOpen;

		// Token: 0x04000101 RID: 257
		private JsonTokenStream m_JsonTokenStream;

		// Token: 0x04000102 RID: 258
		private JsonTokenizer m_JsonTokenizer;

		// Token: 0x04000103 RID: 259
		private JsonValidator m_JsonValidator;

		// Token: 0x04000104 RID: 260
		private NodeParser m_NodeParser;

		// Token: 0x04000105 RID: 261
		private PackedBinaryStream m_PackedBinaryStream;

		// Token: 0x04000106 RID: 262
		private PackedBinaryWriter m_PackedBinaryWriter;

		// Token: 0x04000107 RID: 263
		private GCHandle m_StreamBlockReader;

		// Token: 0x04000108 RID: 264
		private UnsafeBuffer<char> m_InitialBlock;

		// Token: 0x04000109 RID: 265
		private UnsafeBuffer<char> m_CurrentBlock;

		// Token: 0x0400010A RID: 266
		private bool m_CurrentBlockIsFinal;

		// Token: 0x0400010B RID: 267
		private bool m_RequiresExplicitExceptionHandling;

		// Token: 0x0400010C RID: 268
		private bool m_IsInvalid;

		// Token: 0x0400010D RID: 269
		private JsonValidationResult m_ValidationResult;

		// Token: 0x02000057 RID: 87
		// (Invoke) Token: 0x06000223 RID: 547
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void GetNextBlockDelegate(IntPtr self);
	}
}
