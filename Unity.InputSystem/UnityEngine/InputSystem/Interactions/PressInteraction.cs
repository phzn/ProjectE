using System;
using System.ComponentModel;
using UnityEngine.InputSystem.Controls;

namespace UnityEngine.InputSystem.Interactions
{
	// Token: 0x02000126 RID: 294
	[DisplayName("Press")]
	public class PressInteraction : IInputInteraction
	{
		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x0600109F RID: 4255 RVA: 0x0004FCC9 File Offset: 0x0004DEC9
		private float pressPointOrDefault
		{
			get
			{
				if (this.pressPoint <= 0f)
				{
					return ButtonControl.s_GlobalDefaultButtonPressPoint;
				}
				return this.pressPoint;
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x060010A0 RID: 4256 RVA: 0x0004FCE4 File Offset: 0x0004DEE4
		private float releasePointOrDefault
		{
			get
			{
				return this.pressPointOrDefault * ButtonControl.s_GlobalDefaultButtonReleaseThreshold;
			}
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x0004FCF4 File Offset: 0x0004DEF4
		public void Process(ref InputInteractionContext context)
		{
			float num = context.ComputeMagnitude();
			switch (this.behavior)
			{
			case PressBehavior.PressOnly:
				if (this.m_WaitingForRelease)
				{
					if (num <= this.releasePointOrDefault)
					{
						this.m_WaitingForRelease = false;
						if (Mathf.Approximately(0f, num))
						{
							context.Canceled();
							return;
						}
						context.Started();
						return;
					}
				}
				else
				{
					if (num >= this.pressPointOrDefault)
					{
						this.m_WaitingForRelease = true;
						context.PerformedAndStayPerformed();
						return;
					}
					if (num > 0f && !context.isStarted)
					{
						context.Started();
						return;
					}
					if (Mathf.Approximately(0f, num) && context.isStarted)
					{
						context.Canceled();
						return;
					}
				}
				break;
			case PressBehavior.ReleaseOnly:
				if (this.m_WaitingForRelease)
				{
					if (num <= this.releasePointOrDefault)
					{
						this.m_WaitingForRelease = false;
						context.Performed();
						context.Canceled();
						return;
					}
				}
				else if (num >= this.pressPointOrDefault)
				{
					this.m_WaitingForRelease = true;
					if (!context.isStarted)
					{
						context.Started();
						return;
					}
				}
				else
				{
					bool isStarted = context.isStarted;
					if (num > 0f && !isStarted)
					{
						context.Started();
						return;
					}
					if (Mathf.Approximately(0f, num) && isStarted)
					{
						context.Canceled();
						return;
					}
				}
				break;
			case PressBehavior.PressAndRelease:
				if (this.m_WaitingForRelease)
				{
					if (num <= this.releasePointOrDefault)
					{
						this.m_WaitingForRelease = false;
						context.Performed();
						if (Mathf.Approximately(0f, num))
						{
							context.Canceled();
							return;
						}
					}
				}
				else
				{
					if (num >= this.pressPointOrDefault)
					{
						this.m_WaitingForRelease = true;
						context.PerformedAndStayPerformed();
						return;
					}
					bool isStarted2 = context.isStarted;
					if (num > 0f && !isStarted2)
					{
						context.Started();
						return;
					}
					if (Mathf.Approximately(0f, num) && isStarted2)
					{
						context.Canceled();
					}
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x0004FE98 File Offset: 0x0004E098
		public void Reset()
		{
			this.m_WaitingForRelease = false;
		}

		// Token: 0x040006EE RID: 1774
		[Tooltip("The amount of actuation a control requires before being considered pressed. If not set, default to 'Default Press Point' in the global input settings.")]
		public float pressPoint;

		// Token: 0x040006EF RID: 1775
		[Tooltip("Determines how button presses trigger the action. By default (PressOnly), the action is performed on press. With ReleaseOnly, the action is performed on release. With PressAndRelease, the action is performed on press and release.")]
		public PressBehavior behavior;

		// Token: 0x040006F0 RID: 1776
		private bool m_WaitingForRelease;
	}
}
