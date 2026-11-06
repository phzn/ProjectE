using System;
using System.Numerics.Hashing;

namespace System
{
	// Token: 0x020001C5 RID: 453
	public readonly struct SequencePosition : IEquatable<SequencePosition>
	{
		// Token: 0x0600134A RID: 4938 RVA: 0x0004DB31 File Offset: 0x0004BD31
		public SequencePosition(object @object, int integer)
		{
			this._object = @object;
			this._integer = integer;
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x0004DB41 File Offset: 0x0004BD41
		public object GetObject()
		{
			return this._object;
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x0004DB49 File Offset: 0x0004BD49
		public int GetInteger()
		{
			return this._integer;
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x0004DB51 File Offset: 0x0004BD51
		public bool Equals(SequencePosition other)
		{
			return this._integer == other._integer && object.Equals(this._object, other._object);
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x0004DB74 File Offset: 0x0004BD74
		public override bool Equals(object obj)
		{
			if (obj is SequencePosition)
			{
				SequencePosition sequencePosition = (SequencePosition)obj;
				return this.Equals(sequencePosition);
			}
			return false;
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x0004DB99 File Offset: 0x0004BD99
		public override int GetHashCode()
		{
			object @object = this._object;
			return HashHelpers.Combine((@object != null) ? @object.GetHashCode() : 0, this._integer);
		}

		// Token: 0x04001444 RID: 5188
		private readonly object _object;

		// Token: 0x04001445 RID: 5189
		private readonly int _integer;
	}
}
