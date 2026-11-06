using System;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000047 RID: 71
	[InputControlLayout(isGenericTypeOfDevice = true)]
	public class Sensor : InputDevice
	{
		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x0002DCA4 File Offset: 0x0002BEA4
		// (set) Token: 0x060007DB RID: 2011 RVA: 0x0002DCDC File Offset: 0x0002BEDC
		public float samplingFrequency
		{
			get
			{
				QuerySamplingFrequencyCommand querySamplingFrequencyCommand = QuerySamplingFrequencyCommand.Create();
				if (base.ExecuteCommand<QuerySamplingFrequencyCommand>(ref querySamplingFrequencyCommand) >= 0L)
				{
					return querySamplingFrequencyCommand.frequency;
				}
				throw new NotSupportedException(string.Format("Device '{0}' does not support querying sampling frequency", this));
			}
			set
			{
				SetSamplingFrequencyCommand setSamplingFrequencyCommand = SetSamplingFrequencyCommand.Create(value);
				base.ExecuteCommand<SetSamplingFrequencyCommand>(ref setSamplingFrequencyCommand);
			}
		}
	}
}
