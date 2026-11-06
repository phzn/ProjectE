using System;
using Invector.vCamera;
using UnityEngine;

namespace ProjectExhibitionist.BenchSeating
{
	// Token: 0x02000B2B RID: 2859
	public sealed class BenchCameraFraming : IDisposable
	{
		// Token: 0x0600510F RID: 20751 RVA: 0x001F6777 File Offset: 0x001F4977
		public BenchCameraFraming(Object owner, Transform actor, Transform head, vThirdPersonCamera camera)
		{
			this._owner = owner;
			this._actor = actor;
			this._head = head;
			this._camera = camera;
			this._standingHeadHeight = this.HeadHeight;
		}

		// Token: 0x17000D81 RID: 3457
		// (get) Token: 0x06005110 RID: 20752 RVA: 0x001F67A8 File Offset: 0x001F49A8
		private float HeadHeight
		{
			get
			{
				if (!(this._actor != null) || !(this._head != null))
				{
					return 0f;
				}
				return Vector3.Dot(this._head.position - this._actor.position, this._actor.up);
			}
		}

		// Token: 0x06005111 RID: 20753 RVA: 0x001F6804 File Offset: 0x001F4A04
		public void ApplyPose()
		{
			if (this._actor == null || this._head == null || this._camera == null)
			{
				return;
			}
			this._camera.SetFollowHeightOffset(this._owner, this._actor, this.HeadHeight - this._standingHeadHeight);
		}

		// Token: 0x06005112 RID: 20754 RVA: 0x001F6860 File Offset: 0x001F4A60
		public void Dispose()
		{
			if (this._camera != null)
			{
				this._camera.ClearFollowHeightOffset(this._owner);
			}
		}

		// Token: 0x040051BF RID: 20927
		private readonly Object _owner;

		// Token: 0x040051C0 RID: 20928
		private readonly Transform _actor;

		// Token: 0x040051C1 RID: 20929
		private readonly Transform _head;

		// Token: 0x040051C2 RID: 20930
		private readonly vThirdPersonCamera _camera;

		// Token: 0x040051C3 RID: 20931
		private readonly float _standingHeadHeight;
	}
}
