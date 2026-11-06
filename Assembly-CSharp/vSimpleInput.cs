using System;
using Invector;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000132 RID: 306
[vClassHeader("Simple Input", true, "icon_v2", false, "", openClose = false)]
public class vSimpleInput : vMonoBehaviour
{
	// Token: 0x06000A24 RID: 2596 RVA: 0x000461B4 File Offset: 0x000443B4
	private void Update()
	{
		if (this.input.GetButtonDown() && base.gameObject.activeSelf)
		{
			if (this.disableThisObjectAfterInput)
			{
				base.gameObject.SetActive(false);
			}
			this.OnPressInput.Invoke();
		}
	}

	// Token: 0x04000956 RID: 2390
	[Tooltip("Input to press")]
	public GenericInput input = new GenericInput("Escape", "B", "B");

	// Token: 0x04000957 RID: 2391
	[Tooltip("This Gameobject will turn off after the input is pressed")]
	public bool disableThisObjectAfterInput;

	// Token: 0x04000958 RID: 2392
	public UnityEvent OnPressInput;
}
