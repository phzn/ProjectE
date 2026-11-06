using System;
using System.Collections.Generic;
using Invector;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000117 RID: 279
[vClassHeader("Mult-Toggle Event", true, "icon_v2", false, "", helpBoxText = "Use the method SetToggleOn/Off via Events", openClose = false)]
public class vMultToggleEvent : vMonoBehaviour
{
	// Token: 0x060009B3 RID: 2483 RVA: 0x00044D22 File Offset: 0x00042F22
	public void Start()
	{
		this.CheckValidation();
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x00044D2A File Offset: 0x00042F2A
	public void ToggleOn(int index)
	{
		if (this.toggles.Count > 0 && index < this.toggles.Count)
		{
			this.toggles[index].ToggleOn();
			this.CheckValidation();
		}
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x00044D5F File Offset: 0x00042F5F
	public void ToggleOff(int index)
	{
		if (this.toggles.Count > 0 && index < this.toggles.Count)
		{
			this.toggles[index].ToggleOff();
			this.CheckValidation();
		}
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x00044D94 File Offset: 0x00042F94
	public void ToggleOn(string name)
	{
		vMultToggleEvent.Toggle toggle = this.toggles.Find((vMultToggleEvent.Toggle t) => t.name.Equals(name));
		if (toggle != null)
		{
			toggle.ToggleOn();
			this.CheckValidation();
		}
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x00044DD8 File Offset: 0x00042FD8
	public void ToggleOff(string name)
	{
		vMultToggleEvent.Toggle toggle = this.toggles.Find((vMultToggleEvent.Toggle t) => t.name.Equals(name));
		if (toggle != null)
		{
			toggle.ToggleOff();
			this.CheckValidation();
		}
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x00044E1C File Offset: 0x0004301C
	private void CheckValidation()
	{
		bool flag = this.isValid;
		flag = this.toggles.FindAll((vMultToggleEvent.Toggle t) => t.isValid).Count == this.toggles.Count;
		if (flag != this.isValid)
		{
			this.isValid = flag;
			if (this.isValid)
			{
				this.onValidate.Invoke();
				return;
			}
			this.onInvalidate.Invoke();
		}
	}

	// Token: 0x0400090D RID: 2317
	public List<vMultToggleEvent.Toggle> toggles;

	// Token: 0x0400090E RID: 2318
	public bool isValid;

	// Token: 0x0400090F RID: 2319
	public UnityEvent onValidate;

	// Token: 0x04000910 RID: 2320
	public UnityEvent onInvalidate;

	// Token: 0x02000118 RID: 280
	[Serializable]
	public class Toggle
	{
		// Token: 0x060009BA RID: 2490 RVA: 0x00044EA1 File Offset: 0x000430A1
		public void ToggleOn()
		{
			this.value = true;
			this.onTurnOn.Invoke();
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x00044EB5 File Offset: 0x000430B5
		public void ToggleOff()
		{
			this.value = false;
			this.onTurnOff.Invoke();
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x00044EC9 File Offset: 0x000430C9
		public bool isValid
		{
			get
			{
				return this.value.Equals(this.validation);
			}
		}

		// Token: 0x04000911 RID: 2321
		public string name;

		// Token: 0x04000912 RID: 2322
		[Header("Current Value of the toogle")]
		public bool value;

		// Token: 0x04000913 RID: 2323
		[Header("Validation to compare with value")]
		public bool validation;

		// Token: 0x04000914 RID: 2324
		public UnityEvent onTurnOn;

		// Token: 0x04000915 RID: 2325
		public UnityEvent onTurnOff;
	}
}
