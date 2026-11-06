using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000035 RID: 53
	public struct ReadOnlyTransformHandle
	{
		// Token: 0x060000F3 RID: 243 RVA: 0x00005999 File Offset: 0x00003B99
		public Vector3 GetLocalPosition(AnimationStream stream)
		{
			if (this.m_InStream != 1)
			{
				return this.m_SceneHandle.GetLocalPosition(stream);
			}
			return this.m_StreamHandle.GetLocalPosition(stream);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000059BD File Offset: 0x00003BBD
		public Quaternion GetLocalRotation(AnimationStream stream)
		{
			if (this.m_InStream != 1)
			{
				return this.m_SceneHandle.GetLocalRotation(stream);
			}
			return this.m_StreamHandle.GetLocalRotation(stream);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000059E1 File Offset: 0x00003BE1
		public Vector3 GetLocalScale(AnimationStream stream)
		{
			if (this.m_InStream != 1)
			{
				return this.m_SceneHandle.GetLocalScale(stream);
			}
			return this.m_StreamHandle.GetLocalScale(stream);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00005A05 File Offset: 0x00003C05
		public void GetLocalTRS(AnimationStream stream, out Vector3 position, out Quaternion rotation, out Vector3 scale)
		{
			if (this.m_InStream == 1)
			{
				this.m_StreamHandle.GetLocalTRS(stream, out position, out rotation, out scale);
				return;
			}
			this.m_SceneHandle.GetLocalTRS(stream, out position, out rotation, out scale);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00005A31 File Offset: 0x00003C31
		public Matrix4x4 GetLocalToParentMatrix(AnimationStream stream)
		{
			if (this.m_InStream != 1)
			{
				return this.m_SceneHandle.GetLocalToParentMatrix(stream);
			}
			return this.m_StreamHandle.GetLocalToParentMatrix(stream);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00005A55 File Offset: 0x00003C55
		public Vector3 GetPosition(AnimationStream stream)
		{
			if (this.m_InStream != 1)
			{
				return this.m_SceneHandle.GetPosition(stream);
			}
			return this.m_StreamHandle.GetPosition(stream);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00005A79 File Offset: 0x00003C79
		public Quaternion GetRotation(AnimationStream stream)
		{
			if (this.m_InStream != 1)
			{
				return this.m_SceneHandle.GetRotation(stream);
			}
			return this.m_StreamHandle.GetRotation(stream);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00005A9D File Offset: 0x00003C9D
		public void GetGlobalTR(AnimationStream stream, out Vector3 position, out Quaternion rotation)
		{
			if (this.m_InStream == 1)
			{
				this.m_StreamHandle.GetGlobalTR(stream, out position, out rotation);
				return;
			}
			this.m_SceneHandle.GetGlobalTR(stream, out position, out rotation);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00005AC5 File Offset: 0x00003CC5
		public Matrix4x4 GetLocalToWorldMatrix(AnimationStream stream)
		{
			if (this.m_InStream != 1)
			{
				return this.m_SceneHandle.GetLocalToWorldMatrix(stream);
			}
			return this.m_StreamHandle.GetLocalToWorldMatrix(stream);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005AE9 File Offset: 0x00003CE9
		public bool IsResolved(AnimationStream stream)
		{
			return this.m_InStream != 1 || this.m_StreamHandle.IsResolved(stream);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005B02 File Offset: 0x00003D02
		public bool IsValid(AnimationStream stream)
		{
			if (this.m_InStream != 1)
			{
				return this.m_SceneHandle.IsValid(stream);
			}
			return this.m_StreamHandle.IsValid(stream);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005B26 File Offset: 0x00003D26
		public void Resolve(AnimationStream stream)
		{
			if (this.m_InStream == 1)
			{
				this.m_StreamHandle.Resolve(stream);
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005B40 File Offset: 0x00003D40
		public static ReadOnlyTransformHandle Bind(Animator animator, Transform transform)
		{
			ReadOnlyTransformHandle readOnlyTransformHandle = default(ReadOnlyTransformHandle);
			if (transform == null)
			{
				throw new ArgumentNullException("transform");
			}
			readOnlyTransformHandle.m_InStream = (transform.IsChildOf(animator.avatarRoot) ? 1 : 0);
			if (readOnlyTransformHandle.m_InStream == 1)
			{
				readOnlyTransformHandle.m_StreamHandle = animator.BindStreamTransform(transform);
			}
			else
			{
				readOnlyTransformHandle.m_SceneHandle = animator.BindSceneTransform(transform);
			}
			return readOnlyTransformHandle;
		}

		// Token: 0x04000086 RID: 134
		private TransformStreamHandle m_StreamHandle;

		// Token: 0x04000087 RID: 135
		private TransformSceneHandle m_SceneHandle;

		// Token: 0x04000088 RID: 136
		private byte m_InStream;
	}
}
