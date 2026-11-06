using System;
using System.Globalization;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000D5F RID: 3423
	public static class RankedPendingScoreRecord
	{
		// Token: 0x0600648E RID: 25742 RVA: 0x00283900 File Offset: 0x00281B00
		public static bool TryCreate(uint appId, ulong accountId, string board, RankedRunResult result, out string json)
		{
			return RankedPendingScoreRecord.TryCreate(appId, accountId, board, result, "PE_STEAM_BUILD.release.v1", out json);
		}

		// Token: 0x0600648F RID: 25743 RVA: 0x00283914 File Offset: 0x00281B14
		public static bool TryCreate(uint appId, ulong accountId, string board, RankedRunResult result, string distribution, out string json)
		{
			json = null;
			if (!RankedPendingScoreRecord.ValidContext(appId, accountId, board, distribution) || !result.IsValid)
			{
				return false;
			}
			RankedPendingScoreRecord.Record record = new RankedPendingScoreRecord.Record
			{
				schema = 3,
				appId = appId,
				accountId = accountId,
				board = board,
				distribution = distribution,
				distanceMeters = result.DistanceMeters,
				score = result.Score
			};
			if (!RankedLocalIntegrity.TrySign("ranked-pending-score-v3", RankedPendingScoreRecord.Payload(record), out record.seal))
			{
				return false;
			}
			json = JsonUtility.ToJson(record);
			return true;
		}

		// Token: 0x06006490 RID: 25744 RVA: 0x002839A4 File Offset: 0x00281BA4
		public static bool TryRead(string json, uint appId, ulong accountId, string board, out RankedRunResult result)
		{
			return RankedPendingScoreRecord.TryRead(json, appId, accountId, board, "PE_STEAM_BUILD.release.v1", out result);
		}

		// Token: 0x06006491 RID: 25745 RVA: 0x002839B8 File Offset: 0x00281BB8
		public static bool TryRead(string json, uint appId, ulong accountId, string board, string distribution, out RankedRunResult result)
		{
			result = default(RankedRunResult);
			if (!RankedPendingScoreRecord.ValidContext(appId, accountId, board, distribution) || string.IsNullOrEmpty(json) || json.Length > 4096)
			{
				return false;
			}
			RankedPendingScoreRecord.Record record;
			try
			{
				record = JsonUtility.FromJson<RankedPendingScoreRecord.Record>(json);
			}
			catch (ArgumentException)
			{
				return false;
			}
			if (record == null || record.schema != 3 || record.appId != appId || record.accountId != accountId || record.board != board || record.distribution != distribution || !new RankedRunResult(record.distanceMeters, record.score).IsValid || !RankedLocalIntegrity.Verify("ranked-pending-score-v3", RankedPendingScoreRecord.Payload(record), record.seal))
			{
				return false;
			}
			result = new RankedRunResult(record.distanceMeters, record.score);
			return true;
		}

		// Token: 0x06006492 RID: 25746 RVA: 0x00283AA0 File Offset: 0x00281CA0
		private static bool ValidContext(uint appId, ulong accountId, string board, string distribution)
		{
			return appId == 4688700U && accountId != 0UL && !string.IsNullOrWhiteSpace(board) && board.Length <= 128 && board.IndexOf('\n') < 0 && board.IndexOf('\r') < 0 && distribution == "PE_STEAM_BUILD.release.v1";
		}

		// Token: 0x06006493 RID: 25747 RVA: 0x00283AF0 File Offset: 0x00281CF0
		private static string Payload(RankedPendingScoreRecord.Record record)
		{
			return string.Join("\n", new string[]
			{
				record.schema.ToString(CultureInfo.InvariantCulture),
				record.appId.ToString(CultureInfo.InvariantCulture),
				record.accountId.ToString(CultureInfo.InvariantCulture),
				record.board,
				record.distribution,
				record.distanceMeters.ToString(CultureInfo.InvariantCulture),
				record.score.ToString(CultureInfo.InvariantCulture)
			});
		}

		// Token: 0x0400675F RID: 26463
		private const string Purpose = "ranked-pending-score-v3";

		// Token: 0x04006760 RID: 26464
		private const int Schema = 3;

		// Token: 0x02000D60 RID: 3424
		[Serializable]
		private sealed class Record
		{
			// Token: 0x04006761 RID: 26465
			public int schema;

			// Token: 0x04006762 RID: 26466
			public uint appId;

			// Token: 0x04006763 RID: 26467
			public ulong accountId;

			// Token: 0x04006764 RID: 26468
			public string board;

			// Token: 0x04006765 RID: 26469
			public string distribution;

			// Token: 0x04006766 RID: 26470
			public int distanceMeters;

			// Token: 0x04006767 RID: 26471
			public int score;

			// Token: 0x04006768 RID: 26472
			public string seal;
		}
	}
}
