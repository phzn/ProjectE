using System;
using UnityEngine;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x02000713 RID: 1811
	public abstract class vActionListener : vMonoBehaviour, IActionListener, IActionEnterListener, IActionController, IActionExitListener, IActionStayListener
	{
		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06002EBA RID: 11962 RVA: 0x00107D19 File Offset: 0x00105F19
		// (set) Token: 0x06002EBB RID: 11963 RVA: 0x00107D21 File Offset: 0x00105F21
		public bool actionEnter { get; set; }

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06002EBC RID: 11964 RVA: 0x00107D2A File Offset: 0x00105F2A
		// (set) Token: 0x06002EBD RID: 11965 RVA: 0x00107D32 File Offset: 0x00105F32
		public bool actionExit { get; set; }

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06002EBE RID: 11966 RVA: 0x00107D3B File Offset: 0x00105F3B
		// (set) Token: 0x06002EBF RID: 11967 RVA: 0x00107D43 File Offset: 0x00105F43
		public bool actionStay { get; set; }

		// Token: 0x06002EC0 RID: 11968 RVA: 0x00107D4C File Offset: 0x00105F4C
		protected virtual void Awake()
		{
			this.SetUpListener();
		}

		// Token: 0x06002EC1 RID: 11969 RVA: 0x00106B70 File Offset: 0x00104D70
		protected virtual void SetUpListener()
		{
			this.actionEnter = true;
			this.actionExit = true;
			this.actionStay = true;
		}

		// Token: 0x06002EC2 RID: 11970 RVA: 0x00004F31 File Offset: 0x00003131
		protected virtual void Start()
		{
		}

		// Token: 0x06002EC3 RID: 11971 RVA: 0x00004F31 File Offset: 0x00003131
		public virtual void OnActionEnter(Collider other)
		{
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x00004F31 File Offset: 0x00003131
		public virtual void OnActionStay(Collider other)
		{
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x00004F31 File Offset: 0x00003131
		public virtual void OnActionExit(Collider other)
		{
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x000C43E2 File Offset: 0x000C25E2
		bool IActionController.get_enabled()
		{
			return base.enabled;
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x000C43EA File Offset: 0x000C25EA
		void IActionController.set_enabled(bool value)
		{
			base.enabled = value;
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x000B0592 File Offset: 0x000AE792
		GameObject IActionController.get_gameObject()
		{
			return base.gameObject;
		}

		// Token: 0x06002ECA RID: 11978 RVA: 0x000AD00A File Offset: 0x000AB20A
		Transform IActionController.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06002ECB RID: 11979 RVA: 0x000C43F3 File Offset: 0x000C25F3
		string IActionController.get_name()
		{
			return base.name;
		}

		// Token: 0x06002ECC RID: 11980 RVA: 0x000C43FB File Offset: 0x000C25FB
		Type IActionController.GetType()
		{
			return base.GetType();
		}
	}
}
