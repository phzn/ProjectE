using System;
using System.Diagnostics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000120 RID: 288
	[Serializable]
	public struct ResultCode
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x0002FE6C File Offset: 0x0002E06C
		public Define.Result Result
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x0002FE76 File Offset: 0x0002E076
		public static ResultCode None
		{
			get
			{
				return new ResultCode(Define.Result.None);
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x0002FE7E File Offset: 0x0002E07E
		public static ResultCode Empty
		{
			get
			{
				return new ResultCode(Define.Result.Empty);
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0002FE86 File Offset: 0x0002E086
		public static ResultCode Success
		{
			get
			{
				return new ResultCode(Define.Result.Success);
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x0002FE8E File Offset: 0x0002E08E
		public static ResultCode Error
		{
			get
			{
				return new ResultCode(Define.Result.Error);
			}
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0002FE9A File Offset: 0x0002E09A
		public ResultCode(Define.Result initResult)
		{
			this.result = initResult;
			this.warning = Define.Result.None;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0002FEAE File Offset: 0x0002E0AE
		public void Clear()
		{
			this.result = Define.Result.None;
			this.warning = Define.Result.None;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0002FEC2 File Offset: 0x0002E0C2
		public void SetResult(Define.Result code)
		{
			this.result = code;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x0002FECD File Offset: 0x0002E0CD
		public void SetSuccess()
		{
			this.SetResult(Define.Result.Success);
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0002FED6 File Offset: 0x0002E0D6
		public void SetCancel()
		{
			this.SetResult(Define.Result.Cancel);
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0002FEC2 File Offset: 0x0002E0C2
		public void SetError(Define.Result code = Define.Result.Error)
		{
			this.result = code;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x0002FEDF File Offset: 0x0002E0DF
		public void SetWarning(Define.Result code = Define.Result.Warning)
		{
			if (code == Define.Result.None)
			{
				return;
			}
			this.warning = code;
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0002FEEE File Offset: 0x0002E0EE
		public void Merge(ResultCode src)
		{
			if (src.IsError())
			{
				this.result = src.result;
			}
			if (src.IsWarning())
			{
				this.warning = src.warning;
			}
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0002FF22 File Offset: 0x0002E122
		public void SetProcess()
		{
			this.SetResult(Define.Result.Process);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0002FF2B File Offset: 0x0002E12B
		public bool IsResult(Define.Result code)
		{
			return this.result == code;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0002FF38 File Offset: 0x0002E138
		public bool IsNone()
		{
			return this.result == Define.Result.None;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0002FF45 File Offset: 0x0002E145
		public bool IsSuccess()
		{
			return this.result == Define.Result.Success;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0002FF52 File Offset: 0x0002E152
		public bool IsFaild()
		{
			return !this.IsSuccess();
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0002FF5D File Offset: 0x0002E15D
		public bool IsCancel()
		{
			return this.result == Define.Result.Cancel;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0002FF6A File Offset: 0x0002E16A
		public bool IsNormal()
		{
			return this.result < Define.Result.Warning;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0002FF7B File Offset: 0x0002E17B
		public bool IsError()
		{
			return this.result >= Define.Result.Error;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0002FF8F File Offset: 0x0002E18F
		public bool IsProcess()
		{
			return this.result == Define.Result.Process;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0002FF9C File Offset: 0x0002E19C
		public bool IsWarning()
		{
			return this.warning > Define.Result.None;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0002FFA9 File Offset: 0x0002E1A9
		public string GetResultString()
		{
			if (this.IsNormal())
			{
				return this.result.ToString();
			}
			return string.Format("({0}) {1}", (int)this.result, this.result);
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0002FFE9 File Offset: 0x0002E1E9
		public string GetWarningString()
		{
			return string.Format("({0}) {1}", (int)this.warning, this.warning);
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00030010 File Offset: 0x0002E210
		public string GetResultInformation()
		{
			Define.Result result = this.result;
			if (result <= Define.Result.Init_ScaleIsZero)
			{
				if (result == Define.Result.SerializeData_Over31Renderers)
				{
					return string.Format("There are {0} renderers that can be set.", 31);
				}
				if (result == Define.Result.Init_ScaleIsZero)
				{
					return "Component scale values is 0.";
				}
			}
			else
			{
				if (result == Define.Result.Init_NegativeScale)
				{
					return "Component has negative scale.";
				}
				if (result == Define.Result.RenderSetup_Unreadable)
				{
					return "It is necessary to turn on [Read/Write] in the model import settings.";
				}
				if (result == Define.Result.RenderSetup_Over65535vertices)
				{
					return "Original mesh must have no more than 65,535 vertices.";
				}
			}
			return null;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00030088 File Offset: 0x0002E288
		public string GetWarningInformation()
		{
			Define.Result result = this.warning;
			if (result == Define.Result.RenderMesh_VertexWeightIs5BonesOrMore)
			{
				return "The source renderer mesh contains vertex weights that utilize more than 5 bones.\nA weight of 5 or more is invalid.";
			}
			if (result != Define.Result.Init_NonUniformScale)
			{
				return null;
			}
			return "Component scale values \u200b\u200bshould be uniform.\nIf the scale is not uniform, there is a risk that it will not work properly.";
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x000300BD File Offset: 0x0002E2BD
		[Conditional("MC2_DEBUG")]
		public void DebugLog(bool error = true, bool warning = true, bool normal = true)
		{
			if (!this.IsError() || !error)
			{
			}
			bool flag = this.IsWarning() && warning;
		}

		// Token: 0x040008B7 RID: 2231
		[SerializeField]
		private volatile Define.Result result;

		// Token: 0x040008B8 RID: 2232
		[SerializeField]
		private volatile Define.Result warning;
	}
}
