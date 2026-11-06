using System;
using System.IO;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Json
{
	// Token: 0x02000054 RID: 84
	public struct SerializedObjectReader : IDisposable
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0000837F File Offset: 0x0000657F
		// (set) Token: 0x060001DB RID: 475 RVA: 0x0000838C File Offset: 0x0000658C
		public unsafe bool RequiresExplicitExceptionHandling
		{
			get
			{
				return this.m_Data->RequiresExplicitExceptionHandling;
			}
			set
			{
				this.m_Data->RequiresExplicitExceptionHandling = value;
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000839A File Offset: 0x0000659A
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

		// Token: 0x060001DD RID: 477 RVA: 0x000083D8 File Offset: 0x000065D8
		private static Stream OpenFileStreamWithConfiguration(string path, SerializedObjectReaderConfiguration configuration)
		{
			if (configuration.BlockBufferSize < 16)
			{
				throw new ArgumentException("SerializedObjectReaderConfiguration.BlockBufferSize < 16");
			}
			return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, configuration.BlockBufferSize, configuration.UseReadAsync ? FileOptions.Asynchronous : FileOptions.None);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000840E File Offset: 0x0000660E
		public SerializedObjectReader(SerializedObjectReaderConfiguration configuration, Allocator label = Allocator.TempJob)
		{
			this = new SerializedObjectReader(null, 0, SerializedObjectReader.OpenBinaryStreamWithConfiguration(configuration, label), configuration, label, false);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00008423 File Offset: 0x00006623
		public SerializedObjectReader(PackedBinaryStream output, SerializedObjectReaderConfiguration configuration, Allocator label = Allocator.TempJob)
		{
			this = new SerializedObjectReader(null, 0, output, configuration, label, false);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00008432 File Offset: 0x00006632
		public SerializedObjectReader(string path, Allocator label = Allocator.TempJob)
		{
			this = new SerializedObjectReader(path, SerializedObjectReaderConfiguration.Default, label);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00008441 File Offset: 0x00006641
		public SerializedObjectReader(string path, SerializedObjectReaderConfiguration configuration, Allocator label = Allocator.TempJob)
		{
			this = new SerializedObjectReader(path, SerializedObjectReader.OpenBinaryStreamWithConfiguration(configuration, label), configuration, label, false);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00008454 File Offset: 0x00006654
		public SerializedObjectReader(string path, PackedBinaryStream output, Allocator label = Allocator.TempJob, bool leaveOutputOpen = true)
		{
			this = new SerializedObjectReader(path, output, SerializedObjectReaderConfiguration.Default, label, leaveOutputOpen);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00008466 File Offset: 0x00006666
		public SerializedObjectReader(string path, PackedBinaryStream output, SerializedObjectReaderConfiguration configuration, Allocator label = Allocator.TempJob, bool leaveOutputOpen = true)
		{
			this = new SerializedObjectReader(SerializedObjectReader.OpenFileStreamWithConfiguration(path, configuration), output, configuration, label, false, leaveOutputOpen);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000847C File Offset: 0x0000667C
		public SerializedObjectReader(Stream input, Allocator label = Allocator.TempJob, bool leaveInputOpen = true)
		{
			this = new SerializedObjectReader(input, SerializedObjectReaderConfiguration.Default, label, leaveInputOpen);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000848C File Offset: 0x0000668C
		public SerializedObjectReader(Stream input, PackedBinaryStream output, Allocator label = Allocator.TempJob, bool leaveInputOpen = true, bool leaveOutputOpen = true)
		{
			this = new SerializedObjectReader(input, output, SerializedObjectReaderConfiguration.Default, label, leaveInputOpen, leaveOutputOpen);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000084A0 File Offset: 0x000066A0
		public SerializedObjectReader(Stream input, SerializedObjectReaderConfiguration configuration, Allocator label = Allocator.TempJob, bool leaveInputOpen = true)
		{
			this = new SerializedObjectReader(input, SerializedObjectReader.OpenBinaryStreamWithConfiguration(configuration, label), configuration, label, leaveInputOpen, false);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x000084B5 File Offset: 0x000066B5
		public unsafe SerializedObjectReader(Stream input, PackedBinaryStream output, SerializedObjectReaderConfiguration configuration, Allocator label = Allocator.TempJob, bool leaveInputOpen = true, bool leaveOutputOpen = true)
		{
			this.m_Label = label;
			this.m_Data = (UnsafeSerializedObjectReader*)UnsafeUtility.Malloc((long)sizeof(UnsafeSerializedObjectReader), UnsafeUtility.AlignOf<UnsafeSerializedObjectReader>(), this.m_Label);
			*this.m_Data = new UnsafeSerializedObjectReader(input, output, configuration, label, leaveInputOpen, leaveOutputOpen);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x000084F5 File Offset: 0x000066F5
		public unsafe SerializedObjectReader(char* buffer, int length, SerializedObjectReaderConfiguration configuration, Allocator label = Allocator.TempJob)
		{
			this = new SerializedObjectReader(buffer, length, SerializedObjectReader.OpenBinaryStreamWithConfiguration(configuration, label), configuration, label, false);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000850B File Offset: 0x0000670B
		public unsafe SerializedObjectReader(char* buffer, int length, PackedBinaryStream output, SerializedObjectReaderConfiguration configuration, Allocator label = Allocator.TempJob)
		{
			this = new SerializedObjectReader(buffer, length, output, configuration, label, true);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000851B File Offset: 0x0000671B
		internal unsafe SerializedObjectReader(char* buffer, int length, PackedBinaryStream output, SerializedObjectReaderConfiguration configuration, Allocator label = Allocator.TempJob, bool leaveOutputOpen = true)
		{
			this.m_Label = label;
			this.m_Data = (UnsafeSerializedObjectReader*)UnsafeUtility.Malloc((long)sizeof(UnsafeSerializedObjectReader), UnsafeUtility.AlignOf<UnsafeSerializedObjectReader>(), this.m_Label);
			*this.m_Data = new UnsafeSerializedObjectReader(buffer, length, output, configuration, label, leaveOutputOpen);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000855B File Offset: 0x0000675B
		public unsafe void Dispose()
		{
			this.m_Data->Dispose();
			UnsafeUtility.Free((void*)this.m_Data, this.m_Label);
			this.m_Data = null;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00008581 File Offset: 0x00006781
		public unsafe void Reset()
		{
			this.m_Data->Reset();
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000858E File Offset: 0x0000678E
		public unsafe void SetSource(char* ptr, int length)
		{
			this.m_Data->SetSource(ptr, length);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000859D File Offset: 0x0000679D
		public unsafe NodeType Step(NodeType node = NodeType.Any)
		{
			return this.m_Data->Step(node);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x000085AB File Offset: 0x000067AB
		public unsafe NodeType Step(out SerializedValueView view, NodeType node = NodeType.Any)
		{
			return this.m_Data->Step(out view, node);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x000085BC File Offset: 0x000067BC
		public unsafe SerializedValueView Read(char* ptr, int length)
		{
			this.SetSource(ptr, length);
			SerializedValueView serializedValueView;
			this.m_Data->Read(out serializedValueView, NodeType.Any);
			return serializedValueView;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x000085E1 File Offset: 0x000067E1
		public unsafe NodeType Read(NodeType node = NodeType.Any)
		{
			return this.m_Data->Read(node);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x000085EF File Offset: 0x000067EF
		public unsafe NodeType Read(out SerializedValueView view, NodeType node = NodeType.Any)
		{
			return this.m_Data->Read(out view, node);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x000085FE File Offset: 0x000067FE
		public unsafe SerializedObjectView ReadObject()
		{
			return this.m_Data->ReadObject();
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000860B File Offset: 0x0000680B
		public unsafe SerializedMemberView ReadMember()
		{
			return this.m_Data->ReadMember();
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00008618 File Offset: 0x00006818
		public unsafe void ReadMember(SerializedMemberViewCollection collection)
		{
			this.m_Data->ReadMember(collection);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00008626 File Offset: 0x00006826
		public unsafe bool ReadArrayElement(out SerializedValueView view)
		{
			return this.m_Data->ReadArrayElement(out view);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00008634 File Offset: 0x00006834
		public unsafe int ReadArrayElementBatch(NativeArray<SerializedValueView> views, int count)
		{
			if (count > views.Length)
			{
				throw new IndexOutOfRangeException();
			}
			return this.m_Data->ReadArrayElementBatch((SerializedValueView*)views.GetUnsafePtr<SerializedValueView>(), count);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00008658 File Offset: 0x00006858
		public unsafe int ReadArrayElementBatch(SerializedValueView* views, int count)
		{
			return this.m_Data->ReadArrayElementBatch(views, count);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00008667 File Offset: 0x00006867
		public unsafe void DiscardCompleted()
		{
			this.m_Data->DiscardCompleted();
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00008674 File Offset: 0x00006874
		public unsafe void CheckAndThrowInvalidJsonException()
		{
			this.m_Data->CheckAndThrowInvalidJsonException();
		}

		// Token: 0x040000F5 RID: 245
		private readonly Allocator m_Label;

		// Token: 0x040000F6 RID: 246
		[NativeDisableUnsafePtrRestriction]
		private unsafe UnsafeSerializedObjectReader* m_Data;
	}
}
