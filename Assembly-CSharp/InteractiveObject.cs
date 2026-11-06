using System;
using UnityEngine;

// Token: 0x0200014D RID: 333
public class InteractiveObject : MonoBehaviour
{
	// Token: 0x06000A84 RID: 2692 RVA: 0x00047C42 File Offset: 0x00045E42
	private void OnGUI()
	{
		GUI.Label(new Rect(10f, 10f, 100f, 20f), "");
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x00047C67 File Offset: 0x00045E67
	private void Start()
	{
		this.m_state = InteractiveObject.eInteractiveState.Inactive;
		this.sonido = base.GetComponent<AudioSource>();
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x00047C7C File Offset: 0x00045E7C
	public void TrigegrInteraction()
	{
		if (!this.DoorWindow.GetComponent<Animation>().isPlaying)
		{
			Debug.Log("Interactive object");
			InteractiveObject.eInteractiveState state = this.m_state;
			if (state == InteractiveObject.eInteractiveState.Active)
			{
				this.DoorWindow.GetComponent<Animation>().Play("Close");
				this.m_state = InteractiveObject.eInteractiveState.Inactive;
				this.sonido.clip = this.CloseSound;
				this.sonido.Play();
				return;
			}
			if (state != InteractiveObject.eInteractiveState.Inactive)
			{
				return;
			}
			this.DoorWindow.GetComponent<Animation>().Play("Open");
			this.m_state = InteractiveObject.eInteractiveState.Active;
			this.sonido.clip = this.OpenSound;
			this.sonido.Play();
		}
	}

	// Token: 0x040009CA RID: 2506
	private AudioSource sonido;

	// Token: 0x040009CB RID: 2507
	public AudioClip OpenSound;

	// Token: 0x040009CC RID: 2508
	public AudioClip CloseSound;

	// Token: 0x040009CD RID: 2509
	public GameObject DoorWindow;

	// Token: 0x040009CE RID: 2510
	private InteractiveObject.eInteractiveState m_state;

	// Token: 0x0200014E RID: 334
	public enum eInteractiveState
	{
		// Token: 0x040009D0 RID: 2512
		Active,
		// Token: 0x040009D1 RID: 2513
		Inactive
	}
}
