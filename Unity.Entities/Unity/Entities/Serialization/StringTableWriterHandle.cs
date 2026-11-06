using System;
using System.Text;

namespace Unity.Entities.Serialization
{
	// Token: 0x0200037C RID: 892
	internal struct StringTableWriterHandle : IDisposable
	{
		// Token: 0x060017BE RID: 6078 RVA: 0x000591FE File Offset: 0x000573FE
		public StringTableWriterHandle(DotsSerializationWriter.NodeHandle<DotsSerialization.StringTableNode> handle)
		{
			this._nodeHandle = handle;
			this._nodeHandle.NodeHeader.StringCount = 0;
			this._nodeHandle.Dispose();
			this._writerHandle = this._nodeHandle.GetDeferredWriteHandle();
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x00059234 File Offset: 0x00057434
		public unsafe int WriteString(string str)
		{
			this._nodeHandle.NodeHeader.StringCount++;
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			byte[] array;
			void* ptr;
			if ((array = bytes) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = (void*)(&array[0]);
			}
			int num = (int)this._writerHandle.Writer.Position;
			this._writerHandle.Writer.Write(bytes.Length);
			this._writerHandle.Writer.WriteBytes(ptr, bytes.Length);
			return num;
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x000592B2 File Offset: 0x000574B2
		public void Dispose()
		{
			this._writerHandle.Dispose();
		}

		// Token: 0x04000910 RID: 2320
		private DotsSerializationWriter.NodeHandle<DotsSerialization.StringTableNode> _nodeHandle;

		// Token: 0x04000911 RID: 2321
		private DotsSerializationWriter.DeferredWriterHandle<DotsSerialization.StringTableNode> _writerHandle;
	}
}
