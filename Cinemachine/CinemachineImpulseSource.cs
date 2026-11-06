using System;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x0200005A RID: 90
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[SaveDuringPlay]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineImpulseSourceOverview.html")]
	public class CinemachineImpulseSource : MonoBehaviour
	{
		// Token: 0x060003A0 RID: 928 RVA: 0x000167B3 File Offset: 0x000149B3
		private void OnValidate()
		{
			this.m_ImpulseDefinition.OnValidate();
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x000167C0 File Offset: 0x000149C0
		private void Reset()
		{
			this.m_ImpulseDefinition = new CinemachineImpulseDefinition
			{
				m_ImpulseChannel = 1,
				m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Bump,
				m_CustomImpulseShape = new AnimationCurve(),
				m_ImpulseDuration = 0.2f,
				m_ImpulseType = CinemachineImpulseDefinition.ImpulseTypes.Uniform,
				m_DissipationDistance = 100f,
				m_DissipationRate = 0.25f,
				m_PropagationSpeed = 343f
			};
			this.m_DefaultVelocity = Vector3.down;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0001682F File Offset: 0x00014A2F
		public void GenerateImpulseAtPositionWithVelocity(Vector3 position, Vector3 velocity)
		{
			if (this.m_ImpulseDefinition != null)
			{
				this.m_ImpulseDefinition.CreateEvent(position, velocity);
			}
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00016846 File Offset: 0x00014A46
		public void GenerateImpulseWithVelocity(Vector3 velocity)
		{
			this.GenerateImpulseAtPositionWithVelocity(base.transform.position, velocity);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0001685A File Offset: 0x00014A5A
		public void GenerateImpulseWithForce(float force)
		{
			this.GenerateImpulseAtPositionWithVelocity(base.transform.position, this.m_DefaultVelocity * force);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00016879 File Offset: 0x00014A79
		public void GenerateImpulse()
		{
			this.GenerateImpulseWithVelocity(this.m_DefaultVelocity);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00016887 File Offset: 0x00014A87
		public void GenerateImpulseAt(Vector3 position, Vector3 velocity)
		{
			this.GenerateImpulseAtPositionWithVelocity(position, velocity);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00016891 File Offset: 0x00014A91
		public void GenerateImpulse(Vector3 velocity)
		{
			this.GenerateImpulseWithVelocity(velocity);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0001689A File Offset: 0x00014A9A
		public void GenerateImpulse(float force)
		{
			this.GenerateImpulseWithForce(force);
		}

		// Token: 0x0400027A RID: 634
		public CinemachineImpulseDefinition m_ImpulseDefinition = new CinemachineImpulseDefinition();

		// Token: 0x0400027B RID: 635
		[Header("Default Invocation")]
		[Tooltip("The default direction and force of the Impulse Signal in the absense of any specified overrides.  Overrides can be specified by calling the appropriate GenerateImpulse method in the API.")]
		public Vector3 m_DefaultVelocity = Vector3.down;
	}
}
