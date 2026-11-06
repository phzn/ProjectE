using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000D67 RID: 3431
	public static class RankedRunIntegrity
	{
		// Token: 0x0600650F RID: 25871 RVA: 0x0028554B File Offset: 0x0028374B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetSessionRecords()
		{
			RankedRunIntegrity.Records.Clear();
		}

		// Token: 0x06006510 RID: 25872 RVA: 0x00285558 File Offset: 0x00283758
		private static string RecordPayload(RankedRunIntegrity.Record value)
		{
			return string.Concat(new string[]
			{
				value.runId,
				"\n",
				value.accountId,
				"\n",
				value.state.ToString(),
				"\n",
				value.reason
			});
		}

		// Token: 0x06006511 RID: 25873 RVA: 0x002855B4 File Offset: 0x002837B4
		public static bool Begin(RunModeSession session)
		{
			session.rankedRunId = Guid.NewGuid().ToString("N");
			return RankedRunIntegrity.Write(new RankedRunIntegrity.Record
			{
				runId = session.rankedRunId,
				accountId = session.rankedAccountId
			});
		}

		// Token: 0x06006512 RID: 25874 RVA: 0x002855FC File Offset: 0x002837FC
		public static string GetExclusion(RunModeSession session)
		{
			RankedRunIntegrity.Record record = RankedRunIntegrity.Read(session);
			if (record == null)
			{
				return "This ranked session could not be verified. You can keep playing without submitting a score.";
			}
			if (record.state == 1)
			{
				if (!string.IsNullOrEmpty(record.reason))
				{
					return record.reason;
				}
				return "This run was modified and cannot submit a score.";
			}
			else
			{
				if (record.state == 2)
				{
					return "This run has already submitted its result.";
				}
				if (record.state == 3)
				{
					return "This ranked run has ended and cannot submit another result.";
				}
				if (record.state != 0)
				{
					return "This ranked session could not be verified. You can keep playing without submitting a score.";
				}
				return string.Empty;
			}
		}

		// Token: 0x06006513 RID: 25875 RVA: 0x00285670 File Offset: 0x00283870
		public static void Disqualify(RunModeSession session, string reason)
		{
			RankedRunIntegrity.Record record = RankedRunIntegrity.Read(session);
			if (record == null || record.state != 0)
			{
				return;
			}
			record.state = 1;
			record.reason = reason;
			RankedRunIntegrity.Write(record);
		}

		// Token: 0x06006514 RID: 25876 RVA: 0x002856A8 File Offset: 0x002838A8
		public static bool MarkSubmitted(RunModeSession session)
		{
			RankedRunIntegrity.Record record = RankedRunIntegrity.Read(session);
			if (record == null || record.state != 0)
			{
				return false;
			}
			record.state = 2;
			return RankedRunIntegrity.Write(record);
		}

		// Token: 0x06006515 RID: 25877 RVA: 0x002856D8 File Offset: 0x002838D8
		public static void Close(RunModeSession session)
		{
			if (!RankedRunIntegrity.IsRanked(session))
			{
				return;
			}
			RankedRunIntegrity.Record record = RankedRunIntegrity.Read(session);
			if (record == null || record.state != 0)
			{
				return;
			}
			record.state = 3;
			RankedRunIntegrity.Write(record);
		}

		// Token: 0x06006516 RID: 25878 RVA: 0x0028570F File Offset: 0x0028390F
		public static bool SealSession(RunModeSession session)
		{
			session.rankedIntegritySeal = null;
			return !RankedRunIntegrity.IsRanked(session) || RankedLocalIntegrity.TrySign("ranked-session-v1", JsonUtility.ToJson(session), out session.rankedIntegritySeal);
		}

		// Token: 0x06006517 RID: 25879 RVA: 0x00285738 File Offset: 0x00283938
		public static bool VerifySession(RunModeSession session)
		{
			if (!RankedRunIntegrity.IsRanked(session))
			{
				return true;
			}
			string rankedIntegritySeal = session.rankedIntegritySeal;
			session.rankedIntegritySeal = null;
			bool flag;
			try
			{
				string text = JsonUtility.ToJson(session);
				if (RankedLocalIntegrity.Verify("ranked-session-v1", text, rankedIntegritySeal))
				{
					flag = true;
				}
				else if (session.settings == null || session.settings.npcDifficulty != FreeNpcDifficulty.Default)
				{
					flag = false;
				}
				else
				{
					int num = text.IndexOf("\"npcDifficulty\":0,", StringComparison.Ordinal);
					if (num < 0 || num != text.LastIndexOf("\"npcDifficulty\":0,", StringComparison.Ordinal))
					{
						flag = false;
					}
					else
					{
						flag = RankedLocalIntegrity.Verify("ranked-session-v1", text.Remove(num, "\"npcDifficulty\":0,".Length), rankedIntegritySeal);
					}
				}
			}
			finally
			{
				session.rankedIntegritySeal = rankedIntegritySeal;
			}
			return flag;
		}

		// Token: 0x06006518 RID: 25880 RVA: 0x002857EC File Offset: 0x002839EC
		public static bool IsRanked(RunModeSession session)
		{
			return session != null && session.prepared && session.mode == RunModeKind.Classic && session.classicRunKind == ClassicRunKind.Ranked;
		}

		// Token: 0x06006519 RID: 25881 RVA: 0x00285810 File Offset: 0x00283A10
		private static RankedRunIntegrity.Record Read(RunModeSession session)
		{
			if (session == null || string.IsNullOrEmpty(session.rankedRunId) || session.rankedRunId.Length != 32 || string.IsNullOrEmpty(session.rankedAccountId))
			{
				return null;
			}
			RankedRunIntegrity.Record record;
			if (!RankedRunIntegrity.Records.TryGetValue(session.rankedRunId, out record) || record.runId != session.rankedRunId || record.accountId != session.rankedAccountId || !RankedLocalIntegrity.Verify("ranked-live-record-v1", RankedRunIntegrity.RecordPayload(record), record.seal))
			{
				return null;
			}
			return new RankedRunIntegrity.Record
			{
				runId = record.runId,
				accountId = record.accountId,
				state = record.state,
				reason = record.reason,
				seal = record.seal
			};
		}

		// Token: 0x0600651A RID: 25882 RVA: 0x002858E0 File Offset: 0x00283AE0
		private static bool Write(RankedRunIntegrity.Record record)
		{
			if (!RankedLocalIntegrity.TrySign("ranked-live-record-v1", RankedRunIntegrity.RecordPayload(record), out record.seal))
			{
				return false;
			}
			RankedRunIntegrity.Records[record.runId] = record;
			return true;
		}

		// Token: 0x04006782 RID: 26498
		public const string InvalidSessionReason = "This ranked session could not be verified. You can keep playing without submitting a score.";

		// Token: 0x04006783 RID: 26499
		private static readonly Dictionary<string, RankedRunIntegrity.Record> Records = new Dictionary<string, RankedRunIntegrity.Record>();

		// Token: 0x02000D68 RID: 3432
		[Serializable]
		private sealed class Record
		{
			// Token: 0x04006784 RID: 26500
			public string runId;

			// Token: 0x04006785 RID: 26501
			public string accountId;

			// Token: 0x04006786 RID: 26502
			public string reason;

			// Token: 0x04006787 RID: 26503
			public string seal;

			// Token: 0x04006788 RID: 26504
			public int state;
		}
	}
}
