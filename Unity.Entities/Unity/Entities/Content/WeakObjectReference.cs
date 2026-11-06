using System;
using Unity.Entities.Serialization;
using UnityEngine;

namespace Unity.Entities.Content
{
	// Token: 0x020004BE RID: 1214
	[Serializable]
	public struct WeakObjectReference<TObject> : IEquatable<WeakObjectReference<TObject>> where TObject : Object
	{
		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06001D37 RID: 7479 RVA: 0x00066252 File Offset: 0x00064452
		public bool IsReferenceValid
		{
			get
			{
				return this.Id.IsValid;
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06001D38 RID: 7480 RVA: 0x00066264 File Offset: 0x00064464
		public ObjectLoadingStatus LoadingStatus
		{
			get
			{
				return RuntimeContentManager.GetObjectLoadingStatus(in this.Id);
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06001D39 RID: 7481 RVA: 0x00066271 File Offset: 0x00064471
		public TObject Result
		{
			get
			{
				return RuntimeContentManager.GetObjectValue<TObject>(this.Id);
			}
		}

		// Token: 0x06001D3A RID: 7482 RVA: 0x0006627E File Offset: 0x0006447E
		public WeakObjectReference(UntypedWeakReferenceId id)
		{
			this.Id = id;
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x00066287 File Offset: 0x00064487
		public void LoadAsync()
		{
			RuntimeContentManager.LoadObjectAsync(in this.Id);
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x00066294 File Offset: 0x00064494
		public void Release()
		{
			RuntimeContentManager.ReleaseObjectAsync(in this.Id);
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x000662A1 File Offset: 0x000644A1
		public bool WaitForCompletion(int timeoutMs = 0)
		{
			return RuntimeContentManager.WaitForObjectCompletion(this.Id, timeoutMs);
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x000662AF File Offset: 0x000644AF
		public override string ToString()
		{
			return string.Format("WeakObjectReference<{0}> -> {1}", typeof(TObject), this.Id);
		}

		// Token: 0x06001D3F RID: 7487 RVA: 0x000662D0 File Offset: 0x000644D0
		public bool Equals(WeakObjectReference<TObject> other)
		{
			return this.Id.Equals(other.Id);
		}

		// Token: 0x06001D40 RID: 7488 RVA: 0x000662E3 File Offset: 0x000644E3
		public override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}

		// Token: 0x04000A83 RID: 2691
		[SerializeField]
		internal UntypedWeakReferenceId Id;
	}
}
