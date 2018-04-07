using System;
using System.Collections.Generic;
using System.Data;
using TheTennisProject.Services;

namespace TheTennisProject
{
    /// <summary>
    /// Délégué pour l'évènement <see cref="SqlMapping.DataLoadingProgressEvent"/>.
    /// </summary>
    /// <param name="evt">L'évènement associé.</param>
    public delegate void DataLoadingProgressEventHandler(SqlMapping.DataLoadingProgressEvent evt);

    /// <summary>
    /// Classe de mappage de la base de données vers les structures objets.
    /// </summary>
    /// <remarks>Singleton.</remarks>
    public class SqlMapping
    {
        #region Champs et propriétés

        // Instance singleton.
        private static SqlMapping _instance = null;

        /// <summary>
        /// Instance singleton.
        /// </summary>
        public static SqlMapping Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SqlMapping();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Evènement se produisant à chaque chargement d'une donnée depuis la base.
        /// </summary>
        public event DataLoadingProgressEventHandler _dataLoadingProgressEventHandler;

        // Nombre total de données unitaires.
        private int _totalDataCount = 0;
        // Nomre de données déjà chargées.
        private int _currentDataCount = 0;

        #endregion

        /// <summary>
        /// Constructeur privé.
        /// </summary>
        private SqlMapping() { }

        /// <summary>
        /// Procède à l'importation de toutes les données.
        /// </summary>
        /// <param name="worker">Le job ayant appelée de manière asynchrone cette méthode (si applicable).</param>
        public void Import(System.ComponentModel.BackgroundWorker worker)
        {
            if (worker != null)
            {
                _dataLoadingProgressEventHandler += delegate (DataLoadingProgressEvent loadingEvt)
                {
                    worker.ReportProgress(loadingEvt.ProgressionPercentage);
                };
            }

            ComputeDataCount();
            CreatePlayers();
            CreatePointsAtpScale();
            CreateTournaments();
            CreateCountries();
            CreateEditions();
            if (Properties.Settings.Default.ComputeMatchesWhileLoading)
            {
                CreateMatches(null, null);
            }

            // SetPlayerStatsForYearEditions(2017);
        }

        // Calcule le nombre de données à charger.
        private void ComputeDataCount()
        {
            List<string> tables = new List<string> { "points", "editions", "tournaments", "players", "players_nat_history", "countries" };
            if (Properties.Settings.Default.ComputeStatisticsWhileLoading)
            {
                tables.Add("edition_player_stats");
            }
            if (Properties.Settings.Default.ComputeMatchesWhileLoading)
            {
                tables.Add("matches");
            }
            foreach (string table in tables)
            {
                _totalDataCount += SqlTools.ExecuteScalar(string.Format("select count(*) from {0}", table), 0);
            }
        }

        // Procède à l'importation des joueurs.
        private void CreatePlayers()
        {
            string query = "select * from players";
            using (DataTableReader reader = SqlTools.ExecuteReader(query))
            {
                while (reader.Read())
                {
                    // Calcul de la latéralité.
                    string hand = reader.GetString("hand");
                    bool? isLeftHanded = null;
                    if (!string.IsNullOrWhiteSpace(hand))
                    {
                        isLeftHanded = hand.Trim().ToUpper() == "L";
                    }

                    new Player(reader.GetUint64("ID"),
                        reader.GetString("name"),
                        reader.GetString("nationality"),
                        isLeftHanded,
                        reader.GetUint32Null("height"),
                        reader.GetDateTimeNull("date_of_birth"));

                    _dataLoadingProgressEventHandler?.Invoke(new DataLoadingProgressEvent(100 * ++_currentDataCount / _totalDataCount));
                }
            }

            // Importation de l'historique des nationalités.
            query = "select * from players_nat_history order by date_end";
            using (DataTableReader reader = SqlTools.ExecuteReader(query))
            {
                while (reader.Read())
                {
                    Player.AddNationalitiesHistoryEntry(reader.GetUint64("ID"), reader.GetString("nationality"), reader.GetDateTime("date_end"));

                    _dataLoadingProgressEventHandler?.Invoke(new DataLoadingProgressEvent(100 * ++_currentDataCount / _totalDataCount));
                }
            }
        }

        // Procède à l'importation des tournois
        private void CreateTournaments()
        {
            // Importation des tournois.
            string query = "select * from tournaments";
            using (DataTableReader reader = SqlTools.ExecuteReader(query))
            {
                while (reader.Read())
                {
                    new Tournament(reader.GetUint32("ID"),
                        reader.GetString("name"),
                        reader.GetString("city"),
                        (Level)reader.GetByte("level_ID"),
                        (Surface)reader.GetByte("surface_ID"),
                        reader.GetBoolean("is_indoor"),
                        reader["slot_order"] == DBNull.Value ? (byte)0 : reader.GetByte("slot_order"),
                        reader["last_year"] == DBNull.Value ? 0 : reader.GetUint32("last_year"),
                        reader["substitute_ID"] == DBNull.Value ? 0 : reader.GetUint32("substitute_ID"));

                    _dataLoadingProgressEventHandler?.Invoke(new DataLoadingProgressEvent(100 * ++_currentDataCount / _totalDataCount));
                }
            }
        }

        // Procède à l'importation des éditions de tournoi.
        private void CreateEditions()
        {
            string query = "select * from editions order by tournament_ID, year";
            using (DataTableReader reader = SqlTools.ExecuteReader(query))
            {
                while (reader.Read())
                {
                    Edition edition = new Edition(reader.GetUint32("ID"),
                        reader.GetUint32("tournament_ID"),
                        reader.GetUint32("year"),
                        reader.GetUint16("draw_size"),
                        reader.GetDateTime("date_begin"),
                        reader.GetBoolean("is_indoor"),
                        (Level)reader.GetByte("level_ID"),
                        reader.GetString("name"),
                        reader.GetString("city"),
                        reader["slot_order"] == DBNull.Value ? (byte)0 : reader.GetByte("slot_order"),
                        (Surface)reader.GetByte("surface_ID"));

                    if (Properties.Settings.Default.ComputeStatisticsWhileLoading)
                    {
                        CreateEditionsStatistics(edition);
                    }

                    _dataLoadingProgressEventHandler?.Invoke(new DataLoadingProgressEvent(100 * ++_currentDataCount / _totalDataCount));
                }
            }
        }

        /// <summary>
        /// Charge les statistiques associées à une édition de tournoi.
        /// </summary>
        /// <param name="edition">Edition de tournoi.</param>
        /// <exception cref="ArgumentNullException">L'argument <paramref name="edition"/> est <c>Null</c>.</exception>
        public void CreateEditionsStatistics(Edition edition)
        {
            if (edition == null)
            {
                throw new ArgumentNullException(nameof(edition));
            }

            if (edition.StatisticsAreCompute)
            {
                return;
            }

            string query = "select * from edition_player_stats where edition_ID = @edition";
            using (DataTableReader subReader = SqlTools.ExecuteReader(query, new SqlParam("@edition", DbType.UInt32, edition.ID)))
            {
                while (subReader.Read())
                {
                    ulong playerId = subReader.GetUint64("player_ID");
                    for (int i = 0; i < subReader.FieldCount; i++)
                    {
                        string columnName = subReader.GetName(i);
                        if (columnName == "edition_ID" || columnName == "player_ID")
                        {
                            continue;
                        }

                        edition.AddPlayerStatistics(playerId, Tools.GetEnumValueFromSqlMapping<StatType>(columnName), Convert.ToUInt32(subReader[columnName]));
                    }

                    if (Properties.Settings.Default.ComputeStatisticsWhileLoading)
                    {
                        _dataLoadingProgressEventHandler?.Invoke(new DataLoadingProgressEvent(100 * ++_currentDataCount / _totalDataCount));
                    }
                }
            }
        }

        /// <summary>
        /// Importe des matchs depuis la base de données selon des paramètres optionnels.
        /// </summary>
        /// <param name="editionId">Identifiant d'édition de tournoi.</param>
        /// <param name="playerId">Identifiant de joueur.</param>
        /// <returns>Les matchs importés.</returns>
        public IEnumerable<Match> CreateMatches(uint? editionId, ulong? playerId)
        {
            List<Match> matchs = new List<Match>();

            Match.SetBatchMode(true);

            string query = "select * from matches where 1 = 1";
            List<SqlParam> sqlParams = new List<SqlParam>();
            if (editionId.HasValue)
            {
                query += " and edition_ID = @edition";
                sqlParams.Add(new SqlParam("@edition", DbType.UInt32, editionId.Value));
            }
            if (playerId.HasValue)
            {
                query += " and (winner_ID = @player or loser_ID = @player)";
                sqlParams.Add(new SqlParam("@player", DbType.UInt32, playerId.Value));
            }

            using (DataTableReader reader = SqlTools.ExecuteReader(query, sqlParams.ToArray()))
            {
                while (reader.Read())
                {
                    Match match = new Match(reader.GetUint64("ID"),
                        reader.GetUint32("edition_ID"),
                        reader.GetUint16("match_num"),
                        (Round)reader.GetByte("round_ID"),
                        reader.GetByte("best_of"),
                        reader.GetUint32Null("minutes"),
                        reader.GetBoolean("unfinished"),
                        reader.GetBoolean("retirement"),
                        reader.GetBoolean("walkover"),
                        reader.GetUint32("winner_ID"),
                        reader.GetUint32Null("winner_seed"),
                        reader.GetString("winner_entry"),
                        reader.GetUint32Null("winner_rank"),
                        reader.GetUint32Null("winner_rank_points"),
                        reader.GetUint32("loser_ID"),
                        reader.GetUint32Null("loser_seed"),
                        reader.GetString("loser_entry"),
                        reader.GetUint32Null("loser_rank"),
                        reader.GetUint32Null("loser_rank_points"));
                    match.DefineStatistics(reader.ToDynamicDictionnary<uint?>(true), reader.ToDynamicDictionnary<uint?>(true));
                    for (byte i = 1; i <= 5; i++)
                    {
                        match.AddSetByNumber(i, reader.GetByteNull("w_set_" + i.ToString()), reader.GetByteNull("l_set_" + i.ToString()), reader.GetUint16Null("tb_set_" + i.ToString()));
                    }
                    matchs.Add(match);

                    if (Properties.Settings.Default.ComputeMatchesWhileLoading)
                    {
                        _dataLoadingProgressEventHandler?.Invoke(new DataLoadingProgressEvent(100 * ++_currentDataCount / _totalDataCount));
                    }
                }
            }

            Match.SetBatchMode(false);

            return matchs;
        }

        // Procède à l'importation du barème des points ATP.
        private void CreatePointsAtpScale()
        {
            foreach (object level in Enum.GetValues(typeof(Level)))
            {
                string query = string.Format("select * from points where level_ID = {0}", (int)level);
                using (DataTableReader reader = SqlTools.ExecuteReader(query))
                {
                    while (reader.Read())
                    {
                        new PointsAtpScale((Level)level,
                             (Round)reader.GetByte("round_ID"),
                             reader.GetUint32("points_w"),
                             reader.GetUint32("points_l"),
                             reader.GetUint32("points_l_ex"),
                             reader.GetBoolean("is_cumuled"));

                        _dataLoadingProgressEventHandler?.Invoke(new DataLoadingProgressEvent(100 * ++_currentDataCount / _totalDataCount));
                    }
                }
            }
        }

        // procède à l'importation des pays.
        private void CreateCountries()
        {
            string sqlQuery = "select * from countries";
            using (DataTableReader reader = SqlTools.ExecuteReader(sqlQuery))
            {
                while (reader.Read())
                {
                    new Country(reader.GetString("code_ISO2"), reader.GetString("code_ISO3"), reader.GetString("name_EN"), reader.GetString("name_FR"));

                    _dataLoadingProgressEventHandler?.Invoke(new DataLoadingProgressEvent(100 * ++_currentDataCount / _totalDataCount));
                }
            }
        }

        // TODO : retirer ce code mort si confirmée
        /*public List<ulong> GetActivePlayersId(DateTime begin, DateTime end)
        {
            if (end < begin)
            {
                var temp = end;
                end = begin;
                begin = temp;
            }

            var idList = new List<ulong>();

            var sbSql = new System.Text.StringBuilder();
            sbSql.AppendLine("select m.winner_id as ID from matches as m ");
            sbSql.AppendLine("join editions as e on m.edition_ID = e.ID ");
            sbSql.AppendLine("where e.date_begin >= @begin and e.date_begin <= @end");
            sbSql.AppendLine("union all ");
            sbSql.AppendLine("select m.loser_id as ID from matches as m ");
            sbSql.AppendLine("join editions as e on m.edition_ID = e.ID ");
            sbSql.AppendLine("where e.date_begin >= @begin and e.date_begin <= @end");

            var reader = SqlTools.ExecuteReader(sbSql.ToString(), new SqlParam("@begin", DbType.DateTime, begin), new SqlParam("@end", DbType.DateTime, end));
            while (reader.Read())
            {
                idList.Add(reader.GetUint64("ID"));
            }

            return idList.Distinct().ToList();
        }*/

        /// <summary>
        /// Pour une année donnée, calcule les points ATP de chaque joueur pour chaque tournoi.
        /// </summary>
        /// <param name="year">L'année à traiter.</param>
        public void SetPlayerStatsForYearEditions(int year)
        {
            SqlTools.ExecuteNonQuery("delete from edition_player_stats where edition_ID in (select ID from editions where year = @year)",
                new SqlParam("@year", DbType.UInt32, year));

            Dictionary<string, string> sqlFields = new Dictionary<string, string>
            {
                { "edition_ID", "@edition" },
                { "player_ID", "@player" }
            };
            List<SqlParam> sqlParams = new List<SqlParam>
            {
                new SqlParam("@edition", DbType.UInt32),
                new SqlParam("@player", DbType.UInt64)
            };
            Dictionary<string, object> sqlParamValues = new Dictionary<string, object>
            {
                { "@edition", null },
                { "@player", null },
            };
            foreach (object statTypeRaw in Enum.GetValues(typeof(StatType)))
            {
                StatType statType = (StatType)statTypeRaw;
                DbType dbType = DbType.UInt16;
                switch (statType)
                {
                    case StatType.round:
                        dbType = DbType.Byte;
                        break;
                    case StatType.is_winner:
                        dbType = DbType.Boolean;
                        break;
                    case StatType.points:
                        dbType = DbType.UInt32;
                        break;
                }
                sqlFields.Add(Tools.GetEnumSqlMapping<StatType>(statType), string.Concat("@", statType));
                sqlParams.Add(new SqlParam(string.Concat("@", statType), dbType));
                sqlParamValues.Add(string.Concat("@", statType), null);
            }

            using (SqlTools.SqlPrepared sqlPrepared = new SqlTools.SqlPrepared(SqlTools.BuildInsertQuery("edition_player_stats", sqlFields), sqlParams.ToArray()))
            {
                System.Text.StringBuilder sbSql = new System.Text.StringBuilder();
                sbSql.AppendLine("select distinct tmp.ID, tmp.pid ");
                sbSql.AppendLine("from( ");
                sbSql.AppendLine("  SELECT e.ID, m.winner_id as pid ");
                sbSql.AppendLine("  FROM matches as m ");
                sbSql.AppendLine("  join editions as e on m.edition_ID = e.ID ");
                sbSql.AppendLine("  WHERE e.year = @year ");
                sbSql.AppendLine("  union ALL ");
                sbSql.AppendLine("  SELECT e.ID, m.loser_id as pid ");
                sbSql.AppendLine("  FROM matches as m ");
                sbSql.AppendLine("  join editions as e on m.edition_ID = e.ID ");
                sbSql.AppendLine("  WHERE e.year = @year ");
                sbSql.AppendLine(") as tmp");

                using (DataTableReader reader = SqlTools.ExecuteReader(sbSql.ToString(), new SqlParam("@year", DbType.UInt32, year)))
                {
                    while (reader.Read())
                    {
                        uint editionId = reader.GetUint32("ID");
                        ulong playerId = reader.GetUint64("pid");

                        sqlParamValues["@edition"] = editionId;
                        sqlParamValues["@player"] = playerId;

                        foreach (object statTypeRaw in Enum.GetValues(typeof(StatType)))
                        {
                            sqlParamValues[string.Concat("@", statTypeRaw)] = Player.ComputePlayerStatsForEdition(playerId, editionId, (StatType)statTypeRaw);
                        }

                        sqlPrepared.Execute(sqlParamValues);
                    }
                }
            }
        }

        /// <summary>
        /// Evènement indiquant la progression du chargement des données depuis la base.
        /// </summary>
        public class DataLoadingProgressEvent : EventArgs
        {
            /// <summary>
            /// Pourcentage de progression.
            /// </summary>
            public int ProgressionPercentage { get; private set; }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="progressionPercentage">Pourcentage de progression.</param>
            /// <exception cref="ArgumentException">Le pourcentage de progression spécifié est invalide.</exception>
            public DataLoadingProgressEvent(int progressionPercentage)
            {
                if (progressionPercentage < 0 || progressionPercentage > 100)
                {
                    throw new ArgumentException("Le pourcentage de progression spécifié est invalide.", nameof(progressionPercentage));
                }

                ProgressionPercentage = progressionPercentage;
            }
        }
    }
}
