using System;
using UnityEngine;

namespace Pegasus
{
	// Token: 0x020001C5 RID: 453
	public class TriggerControlPegasus : TriggerBase
	{
		// Token: 0x06000D69 RID: 3433 RVA: 0x0005C93C File Offset: 0x0005AB3C
		public override void OnStart(PegasusPoi poi)
		{
			if (poi == null)
			{
				Debug.LogWarning(string.Format("Poi was not supplied on {0} - exiting", base.name));
				return;
			}
			if (this.m_pegasus == null)
			{
				Debug.LogWarning(string.Format("Pegasus was not supplied on {0} - exiting", base.name));
				return;
			}
			if (this.m_disabled)
			{
				return;
			}
			if (this.m_triggerAtStart)
			{
				switch (this.m_actionOnStart)
				{
				case PegasusConstants.PoiPegasusTriggerAction.PlayPegasus:
					if (poi.m_manager.m_displayDebug)
					{
						Debug.Log(string.Format("Started flythrough on {0} from {1}", poi.m_manager.name, poi.name));
					}
					if (poi.m_manager.m_target != null && this.m_pegasus.m_target != null && poi.m_manager.m_target.GetInstanceID() == this.m_pegasus.m_target.GetInstanceID())
					{
						poi.m_manager.StopFlythrough();
					}
					if (this.m_pegasus.m_currentState == PegasusConstants.FlythroughState.Paused)
					{
						this.m_pegasus.ResumeFlythrough();
					}
					else
					{
						this.m_pegasus.StartFlythrough(false);
					}
					if (this.m_disableAfterActioned)
					{
						this.m_disabled = true;
						return;
					}
					break;
				case PegasusConstants.PoiPegasusTriggerAction.PausePegasus:
					if (poi.m_manager.m_displayDebug)
					{
						Debug.Log(string.Format("Pausing flythrough on {0} from {1}", poi.m_manager.name, poi.name));
					}
					this.m_pegasus.PauseFlythrough();
					if (this.m_disableAfterActioned)
					{
						this.m_disabled = true;
						return;
					}
					break;
				case PegasusConstants.PoiPegasusTriggerAction.ResumePegasus:
					if (poi.m_manager.m_displayDebug)
					{
						Debug.Log(string.Format("Resuming flythrough on {0} from {1}", poi.m_manager.name, poi.name));
					}
					this.m_pegasus.ResumeFlythrough();
					if (this.m_disableAfterActioned)
					{
						this.m_disabled = true;
						return;
					}
					break;
				case PegasusConstants.PoiPegasusTriggerAction.StopPegasus:
					if (poi.m_manager.m_displayDebug)
					{
						Debug.Log(string.Format("Stopping flythrough on {0} from {1}", poi.m_manager.name, poi.name));
					}
					this.m_pegasus.StopFlythrough();
					if (this.m_disableAfterActioned)
					{
						this.m_disabled = true;
						return;
					}
					break;
				case PegasusConstants.PoiPegasusTriggerAction.DoNothing:
					if (poi.m_manager.m_displayDebug)
					{
						Debug.Log(string.Format("Doing nothing on {0} from {1}", poi.m_manager.name, poi.name));
					}
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0005CB84 File Offset: 0x0005AD84
		public override void OnEnd(PegasusPoi poi)
		{
			if (poi == null)
			{
				Debug.LogWarning(string.Format("Poi was not supplied on {0} - exiting", base.name));
				return;
			}
			if (this.m_pegasus == null)
			{
				Debug.LogWarning(string.Format("Pegasus was not supplied on {0} - exiting", base.name));
				return;
			}
			if (this.m_disabled)
			{
				return;
			}
			if (this.m_triggerAtEnd)
			{
				switch (this.m_actionOnEnd)
				{
				case PegasusConstants.PoiPegasusTriggerAction.PlayPegasus:
					if (poi.m_manager.m_displayDebug)
					{
						Debug.Log(string.Format("Started flythrough on {0} from {1}", poi.m_manager.name, poi.name));
					}
					poi.m_manager.StopFlythrough();
					if (this.m_pegasus.m_currentState == PegasusConstants.FlythroughState.Paused)
					{
						this.m_pegasus.ResumeFlythrough();
					}
					else
					{
						this.m_pegasus.StartFlythrough(false);
					}
					if (this.m_disableAfterActioned)
					{
						this.m_disabled = true;
						return;
					}
					break;
				case PegasusConstants.PoiPegasusTriggerAction.PausePegasus:
					if (poi.m_manager.m_displayDebug)
					{
						Debug.Log(string.Format("Pausing flythrough on {0} from {1}", poi.m_manager.name, poi.name));
					}
					this.m_pegasus.PauseFlythrough();
					if (this.m_disableAfterActioned)
					{
						this.m_disabled = true;
						return;
					}
					break;
				case PegasusConstants.PoiPegasusTriggerAction.ResumePegasus:
					if (poi.m_manager.m_displayDebug)
					{
						Debug.Log(string.Format("Resuming flythrough on {0} from {1}", poi.m_manager.name, poi.name));
					}
					this.m_pegasus.ResumeFlythrough();
					if (this.m_disableAfterActioned)
					{
						this.m_disabled = true;
						return;
					}
					break;
				case PegasusConstants.PoiPegasusTriggerAction.StopPegasus:
					if (poi.m_manager.m_displayDebug)
					{
						Debug.Log(string.Format("Stopping flythrough on {0} from {1}", poi.m_manager.name, poi.name));
					}
					this.m_pegasus.StopFlythrough();
					if (this.m_disableAfterActioned)
					{
						this.m_disabled = true;
						return;
					}
					break;
				case PegasusConstants.PoiPegasusTriggerAction.DoNothing:
					if (poi.m_manager.m_displayDebug)
					{
						Debug.Log(string.Format("Doing nothing on {0} from {1}", poi.m_manager.name, poi.name));
					}
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x04000DB3 RID: 3507
		public PegasusConstants.PoiPegasusTriggerAction m_actionOnStart;

		// Token: 0x04000DB4 RID: 3508
		public PegasusConstants.PoiPegasusTriggerAction m_actionOnEnd = PegasusConstants.PoiPegasusTriggerAction.StopPegasus;

		// Token: 0x04000DB5 RID: 3509
		public PegasusManager m_pegasus;

		// Token: 0x04000DB6 RID: 3510
		public bool m_disabled;

		// Token: 0x04000DB7 RID: 3511
		public bool m_disableAfterActioned = true;
	}
}
