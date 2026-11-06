using System;
using UnityEngine;

namespace Invector.Utils
{
	// Token: 0x02000615 RID: 1557
	[vClassHeader("Set Transform", true, "icon_v2", false, "", openClose = false)]
	public class vSetTransform : vMonoBehaviour
	{
		// Token: 0x060023D7 RID: 9175 RVA: 0x000CA89C File Offset: 0x000C8A9C
		public void SetPosition(Transform _target)
		{
			_target.position = this.SelfTransform.position;
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x000CA8AF File Offset: 0x000C8AAF
		public void SetRotation(Transform _target)
		{
			_target.rotation = this.SelfTransform.rotation;
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x000CA8C2 File Offset: 0x000C8AC2
		public void SetPositionAndRotation(Transform _target)
		{
			this.SetPosition(_target);
			this.SetRotation(_target);
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x060023DA RID: 9178 RVA: 0x000CA8D2 File Offset: 0x000C8AD2
		public Transform SelfTransform
		{
			get
			{
				if (!this.targetPosition)
				{
					return base.transform;
				}
				return this.targetPosition;
			}
		}

		// Token: 0x060023DB RID: 9179 RVA: 0x000CA8EE File Offset: 0x000C8AEE
		public void SetPosition(Collider _target)
		{
			_target.transform.position = this.SelfTransform.position;
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x000CA906 File Offset: 0x000C8B06
		public void SetRotation(Collider _target)
		{
			_target.transform.rotation = this.SelfTransform.rotation;
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x000CA91E File Offset: 0x000C8B1E
		public void SetPositionAndRotation(Collider _target)
		{
			this.SetPosition(_target);
			this.SetRotation(_target);
		}

		// Token: 0x040023D2 RID: 9170
		public Transform targetPosition;
	}
}
