using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200018B RID: 395
	[Serializable]
	internal sealed class OrdinalIgnoreCaseComparer : OrdinalComparer, ISerializable
	{
		// Token: 0x06000FC3 RID: 4035 RVA: 0x000419B2 File Offset: 0x0003FBB2
		public OrdinalIgnoreCaseComparer()
			: base(true)
		{
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x000419BB File Offset: 0x0003FBBB
		public override int Compare(string x, string y)
		{
			return string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x000419C5 File Offset: 0x0003FBC5
		public override bool Equals(string x, string y)
		{
			return string.Equals(x, y, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x000419CF File Offset: 0x0003FBCF
		public override int GetHashCode(string obj)
		{
			if (obj == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.obj);
			}
			return CompareInfo.GetIgnoreCaseHash(obj);
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x000419E0 File Offset: 0x0003FBE0
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.SetType(typeof(OrdinalComparer));
			info.AddValue("_ignoreCase", true);
		}
	}
}
