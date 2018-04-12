using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TheTennisProject.Properties;
using TheTennisProject.Services;

namespace TheTennisProject.Graphics
{
    /// <summary>
    /// Logique d'interactions pour la fenêtre principale.
    /// </summary>
    public partial class wdwMain : Window
    {
        #region Champs et propriétés

        // Instance unique de la fenêtre.
        private static wdwMain _instance = null;

        /// <summary>
        /// Instance unique de la fenêtre.
        /// </summary>
        public static wdwMain Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new wdwMain();
                }
                return _instance;
            }
        }

        // historique des colonnes de tri du tablean de classement des joueurs (onglet "Classement ATP")
        private Dictionary<string, bool> _rankingStatsSortParameter = new Dictionary<string, bool>();

        #region Propriétés de l'animation ATP

        // Indique que l'animation "ATP Animation" est démarrée
        private volatile bool _animationIsRunning = false;

        // Timer pour l'animation "ATP Animation"
        private System.Timers.Timer _animationTimer;

        // Délai entre les rafraichissements de l'animation "ATP Animation"
        private double _animationRefreshTickTime = 250;

        // Date actuelle de l'animation (fin)
        private DateTime? _animationCurrentDate;

        // Indique qu'un "tick" de l'animation est en cours de calcul
        private volatile bool _animationisComputing;

        // Date de début de l'animation
        private DateTime? _animationBeginDate = null;

        // Liste TOP "ATP_LIVE_SIZE" du classement ATP Live
        private List<Bindings.LiveRanking> _liveAtpCurrentTop20List = new List<Bindings.LiveRanking>();

        // Liste sous-jacente à "_liveAtpCurrentTop20List"
        private List<AtpRanking> _innerLiveAtpCurrentTop20List = new List<AtpRanking>();

        // Nombre de joueurs classés dans l'animation ATP Live
        private const int ATP_LIVE_SIZE = 20;

        #endregion

        #endregion

        // Constructeur privé.
        private wdwMain()
        {
            InitializeComponent();
            InitializeAtpAnimationTab();
            LoadBackgroundDatas(
                delegate (object sender, DoWorkEventArgs evt)
                {
                    SqlMapping.Instance.Import(sender as BackgroundWorker);
                }, true, true, null, null
            );
        }

        /// <summary>
        /// Procède à un chargement de données en tâche de fond. La majorité des composants graphiques sont désactivés pendant ce chargement.
        /// </summary>
        /// <param name="handler">Méthode à appeler.</param>
        /// <param name="reportsProgress">Rapport de progression oui/non.</param>
        /// <param name="isMainLoading">Est chargement principal oui/non.</param>
        /// <param name="todoWithResult">Action éventuelle (sinon <c>Null</c>) à exécuter post-traitement, utilisant comme argument le résultat de <paramref name="handler"/>.</param>
        /// <param name="doWorkArgs">Argument évetuel à fournir à la méthode <paramref name="handler"/>.</param>
        /// <param name="noBlinking">Si vrai, la fenêtre reste active pendant le chargement. Ecrase la valeur de <paramref name="reportsProgress"/>.</param>
        private void LoadBackgroundDatas(DoWorkEventHandler handler, bool reportsProgress, bool isMainLoading, Action<object> todoWithResult, object doWorkArgs, bool noBlinking = false)
        {
            if (!noBlinking)
            {
                lblLoadSqlMapping.Content = "Chargement...";
                stpLoadSqlMapping.Visibility = Visibility.Visible;
                tctMain.IsEnabled = false;
            }
            else
            {
                reportsProgress = false;
            }

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = reportsProgress;
            worker.WorkerSupportsCancellation = false;
            worker.DoWork += handler;
            if (reportsProgress)
            {
                pgbLoadSqlMapping.IsIndeterminate = false;
                worker.ProgressChanged += delegate (object sender, ProgressChangedEventArgs evt)
                {
                    if (evt.UserState != null)
                    {
                        lblLoadSqlMapping.Content = evt.UserState.ToString();
                    }
                    pgbLoadSqlMapping.Value = evt.ProgressPercentage;
                };
            }
            else
            {
                pgbLoadSqlMapping.IsIndeterminate = true;
            }
            worker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs evt)
            {
                if (todoWithResult != null)
                {
                    todoWithResult.Invoke(evt.Result);
                }
                if (!noBlinking)
                {
                    stpLoadSqlMapping.Visibility = Visibility.Collapsed;
                    tctMain.IsEnabled = true;
                }
                else
                {
                    lblLoadSqlMapping.Content = string.Empty;
                }
                if (isMainLoading)
                {
                    BuildMainTabControl();
                }
                worker.Dispose();
            };
            worker.RunWorkerAsync(doWorkArgs);
        }

        // Construit le contenu du TabControl après le chargement initial.
        private void BuildMainTabControl()
        {
            cbbAllPlayers.ItemsSource = Player.GetList.OrderBy(item => item.ID == Player.UNKNOWN_PLAYER_ID).ThenBy(item => item.Name);
            cbbAllPlayers.DisplayMemberPath = "Name";

            cbbPalmaresTournament.ItemsSource = Tournament.GetList.OrderBy(_ => _.Name);
            cbbPalmaresTournament.DisplayMemberPath = "Name";
        }

        // Méthode interne du construction du classement ATP pour les méthodes "RecomputeAtpRanking" et "RecomputeAtpLiveRanking"
        private void RecomputeAtpRankingInner(object sender, DoWorkEventArgs evt)
        {
            Dictionary<StatType, uint> statsDictionnary = Enum.GetValues(typeof(StatType)).Cast<StatType>().ToDictionary(st => st, st => (uint)0);
            statsDictionnary.Remove(StatType.round);

            List<Bindings.PlayerAtpRanking> playersRankList = new List<Bindings.PlayerAtpRanking>();

            object[] arguments = evt.Argument as object[];
            List<Edition> editions = Edition.GetByPeriod(
                (DateTime)arguments[0],
                (DateTime)arguments[1],
                arguments[3] as IEnumerable<Level>,
                arguments[4] as IEnumerable<Surface>,
                (bool)arguments[5]
            );

            foreach (Edition edition in editions)
            {
                // TODO : peut être pas la meilleure solution.
                if (!edition.StatisticsAreCompute)
                {
                    SqlMapping.Instance.CreateEditionsStatistics(edition);
                }
                foreach (Edition.Stats stat in edition.Statistics)
                {
                    Bindings.PlayerAtpRanking currentBinding = playersRankList.FirstOrDefault(prl => prl.InnerPlayer.ID == stat.Player.ID);
                    if (currentBinding == null)
                    {
                        currentBinding = new Bindings.PlayerAtpRanking(stat.Player, statsDictionnary);
                        playersRankList.Add(currentBinding);
                    }
                    if (currentBinding.Statistics.ContainsKey(stat.StatType))
                    {
                        if (stat.StatType == StatType.is_winner)
                        {
                            Edition.Stats roundStateValue = edition.Statistics.First(s => s.Player.ID == stat.Player.ID && s.StatType == StatType.round);
                            if (roundStateValue.Value == (uint)Round.F)
                            {
                                currentBinding.AggregateStatistic(stat.StatType, stat.Value);
                            }
                        }
                        else
                        {
                            currentBinding.AggregateStatistic(stat.StatType, stat.Value);
                        }
                    }
                }
            }

            // filtrage par nationalité
            if (arguments[6] != null)
            {
                playersRankList.RemoveAll(p => !p.InnerPlayer.Nationality.Equals(((Country)arguments[6]).CodeIso3, StringComparison.InvariantCultureIgnoreCase));
            }

            // Note : à chaque refiltrage, le tri est perdu
            _rankingStatsSortParameter.Clear();
            _rankingStatsSortParameter.Add(StatType.points.ToString(), true);

            Bindings.PlayerAtpRanking.SortAndSetRanking(ref playersRankList, _rankingStatsSortParameter);

            // filtrage par nom
            if (arguments[2] != null && !string.IsNullOrWhiteSpace(arguments[2].ToString()))
            {
                playersRankList.RemoveAll(p => !p.InnerPlayer.Name.ToLowerInvariant().Contains(arguments[2].ToString().ToLowerInvariant()));
            }

            evt.Result = playersRankList;
        }

        #region Onglet "fiche joueur"

        // Se produit quand la touche entrée est saisie sur le texte. Une recherche de joueur est effectuée sur le texte fourni.
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSearchPlayer_Click(sender, null);
            }
        }

        // Se produit quand le bouton de recherche de joueur à partir de la zone de texte est activé.
        private void btnSearchPlayer_Click(object sender, RoutedEventArgs e)
        {
            Player currentPlayerSelected = cbbAllPlayers.ItemsSource.Cast<Player>().FirstOrDefault(item => item.Name.Trim().ToLower().Contains(txtSearchPlayer.Text.Trim().ToLower()));
            if (currentPlayerSelected != null)
            {
                cbbAllPlayers.SelectedItem = currentPlayerSelected;
            }
            else
            {
                cbbAllPlayers.SelectedIndex = -1;
            }
        }

        // Se produit quand le contrôle est donné (au clic) à la boite de texte permettant de rechercher un joueur.
        private void txtSearchPlayer_MouseEnter(object sender, MouseEventArgs e)
        {
            if (txtSearchPlayer.Text == "Recherche...")
            {
                txtSearchPlayer.Text = string.Empty;
            }
        }

        // Se produit quand le sélecteur de joueur reçoit une nouvelle valeur
        private void cbbAllPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Player selectedPlayer = cbbAllPlayers.SelectedItem as Player;
            if (selectedPlayer == null)
            {
                return;
            }

            IOrderedEnumerable<Match> allMatchPlayedOrdered =
                Match.GetPlayerMatches(selectedPlayer.ID).OrderByDescending(item => item.Edition.DateBegin).ThenByDescending(item => item.MatchNum);

            #region Onglet général

            lblPlayerAct.Content = allMatchPlayedOrdered.Any() ? allMatchPlayedOrdered.ElementAt(0).Edition.Year.ToString() : "Inconnue";
            lblPlayerDob.Content = !selectedPlayer.DateOfBirth.HasValue ? "Inconnue" : string.Concat(selectedPlayer.DateOfBirth.Value.ToString("dd/MM/yyyy"), " (", Tools.GetEventAge(selectedPlayer.DateOfBirth.Value), " ans)");
            lblPlayerHei.Content = selectedPlayer.HeightInMeters == 0 ? "Inconnue" : string.Format("{0}m{1}", Math.Floor(selectedPlayer.HeightInMeters), Convert.ToInt32((selectedPlayer.HeightInMeters - Math.Floor(selectedPlayer.HeightInMeters)) * 100).ToString().PadLeft(2, '0'));
            lblPlayerLat.Content = selectedPlayer.IsLeftHanded.HasValue ? (selectedPlayer.IsLeftHanded.Value ? "Gaucher" : "Droitier") : "Inconnue";
            lblPlayerNam.Content = selectedPlayer.Name;
            lblPlayerNat.Content = selectedPlayer.Nationality;
            if (selectedPlayer.NationalitiesHistory.Any())
            {
                StringBuilder stringBuildNatHistory = new StringBuilder();
                stringBuildNatHistory.AppendLine("Changements de nationalité sportive :");
                foreach (KeyValuePair<string, DateTime> rowHisto in selectedPlayer.NationalitiesHistory)
                {
                    stringBuildNatHistory.AppendLine(string.Format("{0} jusqu'au {1}", rowHisto.Key, rowHisto.Value.ToString("dd/MM/yyyy")));
                }
                imgToolTipNatHistory.ToolTip = stringBuildNatHistory.ToString();
            }
            else
            {
                imgToolTipNatHistory.Visibility = Visibility.Collapsed;
            }

            #region Recherche d'une image (désactivé)

            /*LoadBackgroundDatas(
                delegate (object insideSender, DoWorkEventArgs insideE)
                {
                    insideE.Result = Tools.GetImageFromGoogle(selectedPlayer.Name);
                }, false, false, delegate (object result)
                {
                    if (result != null && !string.IsNullOrWhiteSpace(result.ToString()))
                    {
                        imgPlayer.Source = new BitmapImage(new Uri(result.ToString()));
                    }
                    else
                    {
                        imgPlayer.Source = new BitmapImage(new Uri("../Resources/unknown_player.png", UriKind.Relative));
                    }
                }, null);*/

            #endregion

            #endregion

            #region Onglet historique

            List<string> years = new List<string>();
            years.Add("Tout");
            for (uint i = Settings.Default.OpenEraYearBegin; i <= DateTime.Now.Year; i++)
            {
                if (allMatchPlayedOrdered.Any(item => (item.Round == Round.F || item.Round == Round.SF) && item.Edition.Year == i))
                {
                    years.Add(i.ToString());
                }
            }
            cbbHistoryYear.ItemsSource = years;

            Dictionary<int, string> surfaces = new Dictionary<int, string>();
            surfaces.Add(0, "Tout");
            foreach (object value in Enum.GetValues(typeof(Surface)))
            {
                Surface typedValue = (Surface)value;
                surfaces.Add((int)typedValue, typedValue.GetTranslation());
            }
            cbbHistorySurface.ItemsSource = surfaces;
            cbbHistorySurface.DisplayMemberPath = "Value";

            Dictionary<int, string> levels = new Dictionary<int, string>();
            levels.Add(0, "Tout");
            foreach (object value in Enum.GetValues(typeof(Level)))
            {
                Level typedValue = (Level)value;
                levels.Add((int)typedValue, typedValue.GetTranslation());
            }
            cbbHistoryLevel.ItemsSource = levels.OrderBy(item => item.Key);
            cbbHistoryLevel.DisplayMemberPath = "Value";

            Dictionary<int, string> rounds = new Dictionary<int, string>();
            rounds.Add(0, "Tout");
            foreach (object value in Enum.GetValues(typeof(Round)))
            {
                Round typedValue = (Round)value;
                rounds.Add((int)typedValue, typedValue.GetTranslation());
            }
            cbbHistoryRound.ItemsSource = rounds;
            cbbHistoryRound.DisplayMemberPath = "Value";

            #endregion

            #region Onglet "Stats"

            cbbStatsLevel.ItemsSource = levels.OrderBy(item => item.Key);
            cbbStatsLevel.DisplayMemberPath = "Value";
            cbbStatsRound.ItemsSource = rounds;
            cbbStatsRound.DisplayMemberPath = "Value";
            cbbStatsSurface.ItemsSource = surfaces;
            cbbStatsSurface.DisplayMemberPath = "Value";
            cbbStatsYear.ItemsSource = years;

            // Déclenche un changement (factice) dans un sélecteur, afin de forcer l'affichage des statistiques globales
            cbbStatsAny_SelectionChanged(null, null);

            #endregion
        }
        
        // Filtre la liste des matchs du joueur courant avec les valeurs sélectionnées pour chaque filtre.
        private void cbbHistoryAny_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Player selectedPlayer = cbbAllPlayers.SelectedItem as Player;
            if (selectedPlayer == null)
            {
                return;
            }

            // le joueur sélectionné est taggé
            lstHistoryResult.Tag = selectedPlayer;

            IEnumerable<Match> baseMatchesList = Match.GetPlayerMatches(selectedPlayer.ID);

            // tansforme la liste de base (read-only) en colection éditable
            List<Match> filteredMatchesList = new List<Match>();
            foreach (Match match in baseMatchesList)
            {
                filteredMatchesList.Add(match);
            }

            if (cbbHistoryYear.SelectedIndex >= 0)
            {
                if (cbbHistoryYear.SelectedItem.ToString() != "Tout")
                {
                    filteredMatchesList = filteredMatchesList.Where(item => item.Edition.Year == Convert.ToUInt32(cbbHistoryYear.SelectedItem)).ToList();
                }
            }
            if (cbbHistoryLevel.SelectedIndex >= 0)
            {
                KeyValuePair<int, string> levelValue = (KeyValuePair<int, string>)cbbHistoryLevel.SelectedItem;
                if (levelValue.Key > 0)
                {
                    filteredMatchesList = filteredMatchesList.Where(item => item.Edition.TournamentLevel == (Level)levelValue.Key).ToList();
                }
            }
            if (cbbHistorySurface.SelectedIndex >= 0)
            {
                KeyValuePair<int, string> surfaceValue = (KeyValuePair<int, string>)cbbHistorySurface.SelectedItem;
                if (surfaceValue.Key > 0)
                {
                    filteredMatchesList = filteredMatchesList.Where(item => item.Edition.TournamentSurface == (Surface)surfaceValue.Key).ToList();
                }
            }
            if (cbbHistoryRound.SelectedIndex >= 0)
            {
                KeyValuePair<int, string> roundValue = (KeyValuePair<int, string>)cbbHistoryRound.SelectedItem;
                if (roundValue.Key > 0)
                {
                    filteredMatchesList = filteredMatchesList.Where(item => item.Round == (Round)roundValue.Key).ToList();
                }
            }

            lstHistoryResult.ItemsSource = filteredMatchesList;
        }

        #region Onglet "Stats"

        // Filtre les statistiques du joueur courant avec les valeurs sélectionnées pour chaque filtre.
        private void cbbStatsAny_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Player selectedPlayer = cbbAllPlayers.SelectedItem as Player;
            if (selectedPlayer == null)
            {
                return;
            }

            IEnumerable<Match> baseMatchesList = Match.GetPlayerMatches(selectedPlayer.ID);

            // tansforme la liste de base (read-only) en colection éditable
            List<Match> filteredMatchesList = new List<Match>(baseMatchesList);

            if (cbbStatsYear.SelectedIndex >= 0 && cbbStatsYear.SelectedItem.ToString() != "Tout")
            {
                filteredMatchesList = filteredMatchesList.Where(item => item.Edition.Year == Convert.ToUInt32(cbbStatsYear.SelectedItem)).ToList();
            }
            if (cbbStatsLevel.SelectedIndex >= 0)
            {
                KeyValuePair<int, string> levelValue = (KeyValuePair<int, string>)cbbStatsLevel.SelectedItem;
                if (levelValue.Key > 0)
                {
                    filteredMatchesList = filteredMatchesList.Where(item => item.Edition.TournamentLevel == (Level)levelValue.Key).ToList();
                }
            }
            if (cbbStatsSurface.SelectedIndex >= 0)
            {
                KeyValuePair<int, string> surfaceValue = (KeyValuePair<int, string>)cbbStatsSurface.SelectedItem;
                if (surfaceValue.Key > 0)
                {
                    filteredMatchesList = filteredMatchesList.Where(item => item.Edition.TournamentSurface == (Surface)surfaceValue.Key).ToList();
                }
            }
            if (cbbStatsRound.SelectedIndex >= 0)
            {
                KeyValuePair<int, string> roundValue = (KeyValuePair<int, string>)cbbStatsRound.SelectedItem;
                if (roundValue.Key > 0)
                {
                    filteredMatchesList = filteredMatchesList.Where(item => item.Round == (Round)roundValue.Key).ToList();
                }
            }

            int countMatch = filteredMatchesList.Count;
            if (countMatch == 0)
            {
                lblStatsCountAce.Content = "N/A";
                lblStatsCountDF.Content = "N/A";
                lblStatsCountEdition.Content = "N/A";
                lblStatsCountMatch.Content = "N/A";
                lblStatsCountTitle.Content = "N/A";
                lblStatsCountWin.Content = "N/A";
                lblStatsFirstMatch.Content = "N/A";
                lblStatsLastMatch.Content = "N/A";
                lblStatsTimeSpent.Content = "N/A";
                lblStatsCountGame.Content = "N/A";
                lblStatsLoseRun.Content = "N/A";
                lblStatsWinRun.Content = "N/A";
                lblStatsShortMatch.Content = "N/A";
                lblStatsLongMatch.Content = "N/A";
            }
            else
            {
                long totalMinutes = filteredMatchesList.Sum(_ => _.Minutes);
                int totalGames = filteredMatchesList.Sum(_ => _.CountGames);
                long totalAces = filteredMatchesList.Sum(_ => _.Loser.ID == selectedPlayer.ID ? _.LoserCountAce ?? 0 : _.WinnerCountAce ?? 0);
                long totalDF = filteredMatchesList.Sum(_ => _.Loser.ID == selectedPlayer.ID ? _.LoserCountDbFault ?? 0 : _.WinnerCountDbFault ?? 0);
                int countEdition = filteredMatchesList.Select(_ => _.Edition.ID).Distinct().Count();
                int countTitle = filteredMatchesList.Count(_ => _.Round == Round.F && _.Winner.ID == selectedPlayer.ID);
                int countWin = filteredMatchesList.Count(_ => _.Winner.ID == selectedPlayer.ID);
                uint minutesShortWin = filteredMatchesList.Where(_ => !_.Unfinished && !_.Walkover).OrderBy(_ => _.Minutes).First().Minutes;
                uint minutesLongWin = filteredMatchesList.OrderByDescending(_ => _.Minutes).First().Minutes;

                lblStatsCountAce.Content = string.Format("{0} ({1} / m.)", totalAces, Math.Round(totalAces / (decimal)countMatch, 1));
                lblStatsCountDF.Content = string.Format("{0} ({1} / m.)", totalDF, Math.Round(totalDF / (decimal)countMatch, 1));
                lblStatsCountEdition.Content = countEdition;
                lblStatsCountMatch.Content = countMatch;
                lblStatsCountTitle.Content = string.Format("{0} ({1} %)", countTitle, Math.Round(countTitle / (decimal)countEdition * 100, 1));
                lblStatsCountWin.Content = string.Format("{0} ({1} %)", countWin, Math.Round(countWin / (decimal)countMatch * 100, 1));
                lblStatsFirstMatch.Content = filteredMatchesList.OrderBy(_ => _.Edition.DateBegin).First().Edition.DateBegin.ToString("dd/MM/yyyy");
                lblStatsLastMatch.Content = filteredMatchesList.OrderByDescending(_ => _.Edition.DateBegin).First().Edition.DateBegin.ToString("dd/MM/yyyy");
                lblStatsTimeSpent.Content = string.Format("{0} h. ({1} min. / m.)", Math.Round(totalMinutes / (decimal)60, 0), Math.Round(totalMinutes / (decimal)countMatch, 0));
                lblStatsCountGame.Content = string.Format("{0} ({1} / m.)", totalGames, Math.Round(totalGames / (decimal)countMatch, 1));
                lblStatsShortMatch.Content = string.Format("{0} m.", minutesShortWin);
                lblStatsLongMatch.Content = string.Format("{0} m.", minutesLongWin);
                lblStatsLoseRun.Content = filteredMatchesList.GetWinLoseRun(selectedPlayer.ID, true);
                lblStatsWinRun.Content = filteredMatchesList.GetWinLoseRun(selectedPlayer.ID, false);
            }
        }

        #endregion

        #endregion

        #region Onglet "Classement ATP"

        // calcule le classement ATP aux dates sélectionnées et crée la liaison avec le composant ListView
        private void RecomputeAtpRanking()
        {
            LoadBackgroundDatas(
                RecomputeAtpRankingInner, false, false,
                delegate(object result)
                {
                    #region Reconstruction des colonnes (désactivé)
                    /*
                    var columnsCount = gdvAtpRanking.Columns.Count;
                    for (int i = 2; i < columnsCount; i++)
                    {
                        gdvAtpRanking.Columns.RemoveAt(i);
                    }

                    foreach (var statType in statsDictionnary.Keys)
                    {
                        var xamlBuilder = new StringBuilder();
                        xamlBuilder.AppendLine("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">");
                        xamlBuilder.AppendLine("    <Label Content=\"{Binding Path=Statistics[(localSvc:StatType)" + statType + "]}\" />");
                        xamlBuilder.AppendLine("</DataTemplate>");

                        DataTemplate template;
                        using (var stringReader = new System.IO.StringReader(xamlBuilder.ToString()))
                        {
                            var xmlReader = System.Xml.XmlReader.Create(stringReader);
                            template = System.Windows.Markup.XamlReader.Load(xmlReader) as DataTemplate;
                            xmlReader.Close();
                        }

                        var column = new GridViewColumn
                        {
                            Header = Tools.GetEnumDescription<StatType>(statType),
                            CellTemplate = template
                        };
                        gdvAtpRanking.Columns.Add(column);
                    }
                    */
                    #endregion
                    
                    lstAtpRanking.ItemsSource = result as List<Bindings.PlayerAtpRanking>;
                },
                new object[]
                {
                    dtpAtpRankingDateBegin.DisplayDate,
                    dtpAtpRankingDateEnd.DisplayDate,
                    txtPlayerName.Text,
                    lsbSelectLevel.SelectedItems.Cast<Level>(),
                    lsbelectSurface.SelectedItems.Cast<Surface>(),
                    chkIsIndoor.IsChecked == true,
                    cbbNationalityFilter.SelectedItem
                },
                false
            );
        }

        // se produit quand le bouton de filtrage est cliqué
        private void btnFilterAction_Click(object sender, RoutedEventArgs e)
        {
            RecomputeAtpRanking();
        }

        // se produit quand une entête de colonne est cliquée pour tri
        private void gdvAtpRanking_header_Click(object sender, RoutedEventArgs e)
        {
            List<Bindings.PlayerAtpRanking> fullList = lstAtpRanking.ItemsSource as List<Bindings.PlayerAtpRanking>;

            string columnName = (sender as GridViewColumnHeader).Name.Replace("StatType_", string.Empty);
            // ce hack permet de renommer proprement la colonne de tri
            // qui est nommée ainsi afin d'éviter un warning sur le fait que le nom de la colonne substitue la propriété "Name" de la fenêtre
            if (columnName == "PName")
            {
                columnName = "Name";
            }

            bool isDesc = false;
            if (_rankingStatsSortParameter.ContainsKey(columnName))
            {
                isDesc = !_rankingStatsSortParameter[columnName];
                _rankingStatsSortParameter.Remove(columnName);
            }

            Dictionary<string, bool> temp = new Dictionary<string, bool>();
            temp.Add(columnName, isDesc);
            foreach (string key in _rankingStatsSortParameter.Keys)
            {
                temp.Add(key, _rankingStatsSortParameter[key]);
            }
            _rankingStatsSortParameter = temp;

            Bindings.PlayerAtpRanking.SortAndSetRanking(ref fullList, _rankingStatsSortParameter);

            lstAtpRanking.ItemsSource = fullList;
        }

        #endregion

        #region Onglet "Palmarès tournoi"

        // Se produit quand la sélection du sélecteur de tournoi est modifiée
        private void cbbPalmaresTournament_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Bindings.TournamentPalmares> palmares = new List<Bindings.TournamentPalmares>();

            Tournament selectedTournament = cbbPalmaresTournament.SelectedItem as Tournament;
            if (selectedTournament != null)
            {
                List<Edition> editions = Edition.GetByTournament(selectedTournament.ID);
                foreach (Edition edition in editions)
                {
                    palmares.Add(new Bindings.TournamentPalmares(edition.Year, Match.GetByEdition((uint)edition.ID)));
                }
            }

            lstPalmarestDetails.ItemsSource = palmares.OrderByDescending(_ => _.Year);
        }

        #endregion

        #region Onglet "ATP Animation"

        // Se produit au clic sur le bouton d'action
        private void btnAtpLiveStart_Click(object sender, RoutedEventArgs e)
        {
            if (!_animationIsRunning)
            {
                _animationIsRunning = true;
                btnAtpLiveStart.Content =
                    new Image()
                    {
                        Source = Tools.ImageSourceForBitmap(Properties.Resources.player_pause)
                    };
                _animationTimer.Start();
            }
            else
            {
                _animationTimer.Stop();
                _animationIsRunning = false;
                _animationisComputing = false;
                btnAtpLiveStart.Content =
                    new Image()
                    {
                        Source = Tools.ImageSourceForBitmap(Properties.Resources.player_play)
                    };
            }
        }

        // Se produit au "tick" du timer qui gère l'animation ATP
        private void _animationTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(RecomputeAtpLiveRanking));
        }

        // Se produit au clic sur le bouton d'annulation d'animation ATP
        private void btnEraseAtpLive_Click(object sender, RoutedEventArgs e)
        {
            _animationTimer.Stop();
            _animationIsRunning = false;
            _animationisComputing = false;
            btnAtpLiveStart.Content =
                new Image()
                {
                    Source = Tools.ImageSourceForBitmap(Properties.Resources.player_play)
                };
            _animationCurrentDate = null;
            _animationBeginDate = null;
            cavAtpLiveList.Children.Clear();
        }

        // Construction du classement ATP "live" pour l'animation
        private void RecomputeAtpLiveRanking()
        {
            if (_animationisComputing)
            {
                return;
            }
            _animationisComputing = true;

            if (!_animationBeginDate.HasValue)
            {
                _animationBeginDate = dtpAtpLiveDateBegin.SelectedDate;
            }

            bool yearStep = cbbAtpLiveStep.SelectedIndex == 1;
            if (!_animationCurrentDate.HasValue)
            {
                _animationCurrentDate = dtpAtpLiveDateBegin.SelectedDate.Value;
            }
            else
            {
                _animationCurrentDate = yearStep ? _animationCurrentDate.Value.AddDays(7 * 52) : _animationCurrentDate.Value.AddDays(7);
            }

            DateTime dateBegin;
            DateTime dateEnd = _animationCurrentDate.Value;
            if (yearStep)
            {
                dateBegin = _animationCurrentDate.Value.Year == Settings.Default.OpenEraYearBegin ?
                   Tools.ATP_RANKING_DEBUT : _animationCurrentDate.Value.AddDays(7 * 52 * -1);
            }
            else
            {
                dateBegin = _animationCurrentDate.Value.Year == Settings.Default.OpenEraYearBegin ?
                    _animationBeginDate.Value : _animationCurrentDate.Value.AddDays(7 * 52 * -1);
            }

            LoadBackgroundDatas(
                delegate(object sender, DoWorkEventArgs evt)
                {
                    List<AtpRanking> baseList = AtpRanking.GetRankingAtDate((DateTime)evt.Argument, true, ATP_LIVE_SIZE).ToList();
                    if (baseList.Count == 0)
                    {
                        // Cas des années de 53 semaines sans classement la dernière semaine (pourquoi ?)
                        baseList = _innerLiveAtpCurrentTop20List;
                    }
                    evt.Result = baseList
                                .Select(_ =>
                                    new Bindings.LiveRanking(
                                        _.Player.Name,
                                        _.YearRollingPoints,
                                        (uint)(baseList.IndexOf(_) + 1),
                                        // TODO : faire plus simple
                                        _innerLiveAtpCurrentTop20List.Any(__ => __.Player == _.Player) ?
                                            (uint)(_innerLiveAtpCurrentTop20List.IndexOf(_innerLiveAtpCurrentTop20List.First(__ => __.Player == _.Player))) : ATP_LIVE_SIZE + 1
                                    ))
                                .ToList();
                    _innerLiveAtpCurrentTop20List = baseList;
                }, false, false,
                delegate (object result)
                {
                    FillAtpLiveCanvas(result as List<Bindings.LiveRanking>);
                    lblAtpLivePeriod.Content = string.Format("{0} - {1}", dateBegin.ToString("dd/MM/yyyy"), dateEnd.ToString("dd/MM/yyyy"));
                    _animationisComputing = false;
                },
                dateEnd,
                true
            );
        }

        // Remplit le canvas du classement ATP Live avec la liste d'éléments spécifiée
        private void FillAtpLiveCanvas(List<Bindings.LiveRanking> bindingList)
        {
            cavAtpLiveList.Children.Clear();
            int i = 0;
            foreach (Bindings.LiveRanking binding in bindingList)
            {
                PlayernRanking prControl = new PlayernRanking
                {
                    DataContext = binding
                };

                Canvas.SetTop(prControl, i == 0 ? 2.5 : (i * 35));
                Canvas.SetLeft(prControl, 5);
                cavAtpLiveList.Children.Add(prControl);
                i++;
            }
        }

        // Initialise les composants graphiques de l'onglet "ATP animation"
        private void InitializeAtpAnimationTab()
        {
            _animationTimer = new System.Timers.Timer(_animationRefreshTickTime);
            _animationTimer.Elapsed += _animationTimer_Elapsed;
            cavAtpLiveList.Children.Clear();
            btnAtpLiveStart.Content = new Image() { Source = Tools.ImageSourceForBitmap(Properties.Resources.player_play) };
            btnEraseAtpLive.Content = new Image() { Source = Tools.ImageSourceForBitmap(Properties.Resources.button_cancel) };
            dtpAtpLiveDateBegin.SelectedDate = Tools.ATP_RANKING_DEBUT;
            dtpAtpLiveDateBegin.DisplayDateStart = Tools.ATP_RANKING_DEBUT;
        }

        #endregion
    }
}
