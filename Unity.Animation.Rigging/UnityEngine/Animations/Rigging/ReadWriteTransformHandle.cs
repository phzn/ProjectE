using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000034 RID: 52
	public struct ReadWriteTransformHandle
	{
		// Token: 0x060000E1 RID: 225 RVA: 0x00005833 File Offset: 0x00003A33
		public Vector3 GetLocalPosition(AnimationStream stream)
		{
			return this.m_Handle.GetLocalPosition(stream);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00005841 File Offset: 0x00003A41
		public Quaternion GetLocalRotation(AnimationStream stream)
		{
			return this.m_Handle.GetLocalRotation(stream);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000584F File Offset: 0x00003A4F
		public Vector3 GetLocalScale(AnimationStream stream)
		{
			return this.m_Handle.GetLocalScale(stream);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000585D File Offset: 0x00003A5D
		public void GetLocalTRS(AnimationStream stream, out Vector3 position, out Quaternion rotation, out Vector3 scale)
		{
			this.m_Handle.GetLocalTRS(stream, out position, out rotation, out scale);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000586F File Offset: 0x00003A6F
		public void SetLocalPosition(AnimationStream stream, Vector3 position)
		{
			this.m_Handle.SetLocalPosition(stream, position);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000587E File Offset: 0x00003A7E
		public void SetLocalRotation(AnimationStream stream, Quaternion rotation)
		{
			this.m_Handle.SetLocalRotation(stream, rotation);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000588D File Offset: 0x00003A8D
		public void SetLocalScale(AnimationStream stream, Vector3 scale)
		{
			this.m_Handle.SetLocalScale(stream, scale);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000589C File Offset: 0x00003A9C
		public void SetLocalTRS(AnimationStream stream, Vector3 position, Quaternion rotation, Vector3 scale, bool useMask = false)
		{
			this.m_Handle.SetLocalTRS(stream, position, rotation, scale, useMask);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000058B0 File Offset: 0x00003AB0
		public Vector3 GetPosition(AnimationStream stream)
		{
			return this.m_Handle.GetPosition(stream);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000058BE File Offset: 0x00003ABE
		public Quaternion GetRotation(AnimationStream stream)
		{
			return this.m_Handle.GetRotation(stream);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000058CC File Offset: 0x00003ACC
		public void GetGlobalTR(AnimationStream stream, out Vector3 position, out Quaternion rotation)
		{
			this.m_Handle.GetGlobalTR(stream, out position, out rotation);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000058DC File Offset: 0x00003ADC
		public void SetPosition(AnimationStream stream, Vector3 position)
		{
			this.m_Handle.SetPosition(stream, position);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000058EB File Offset: 0x00003AEB
		public void SetRotation(AnimationStream stream, Quaternion rotation)
		{
			this.m_Handle.SetRotation(stream, rotation);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000058FA File Offset: 0x00003AFA
		public void SetGlobalTR(AnimationStream stream, Vector3 position, Quaternion rotation, bool useMask = false)
		{
			this.m_Handle.SetGlobalTR(stream, position, rotation, useMask);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000590C File Offset: 0x00003B0C
		public bool IsResolved(AnimationStream stream)
		{
			return this.m_Handle.IsResolved(stream);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000591A File Offset: 0x00003B1A
		public bool IsValid(AnimationStream stream)
		{
			return this.m_Handle.IsValid(stream);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00005928 File Offset: 0x00003B28
		public void Resolve(AnimationStream stream)
		{
			this.m_Handle.Resolve(stream);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005938 File Offset: 0x00003B38
		public static ReadWriteTransformHandle Bind(Animator animator, Transform transform)
		{
			ReadWriteTransformHandle readWriteTransformHandle = default(ReadWriteTransformHandle);
			if (transform == null)
			{
				throw new ArgumentNullException("transform");
			}
			if (!transform.IsChildOf(animator.avatarRoot))
			{
				throw new InvalidOperationException("Transform '" + transform.name + "' is not a child of the Animator hierarchy, and cannot be written to.");
			}
			readWriteTransformHandle.m_Handle = animator.BindStreamTransform(transform);
			return readWriteTransformHandle;
		}

		// Token: 0x04000085 RID: 133
		private TransformStreamHandle m_Handle;
	}
}
