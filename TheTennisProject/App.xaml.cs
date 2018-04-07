using System.Windows;

namespace TheTennisProject
{
    /// <summary>
    /// Logique d'interfaction pour <see cref="App"/>.
    /// </summary>
    public partial class App : Application
    {
        // Affiche la fenêtre principale au démarrage.
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            #region Intégration des données d'une année (à décommenter)
            /*
            // Vérifier dans les paramètres le chemin d'accès vers le fichier de log.
            // Lire les commentaires de chaque méthode de la classe "CsvFileIntegration".
            CsvFileIntegration.InitializeDefault(TODO, TODO, TODO);
            CsvFileIntegration.Default.IntegrateEditionOfTournaments();
            CsvFileIntegration.Default.IntegrateNewPlayers();
            CsvFileIntegration.Default.IntegrateMatchs();
            CsvFileIntegration.Default.SetUnfinishedMatchsDatas();
            // Dernière étape : décommenter, dans SqlMapping.Import(), la dernière ligne, et spécifier l'année
            */
            #endregion

            Graphics.wdwMain.Instance.ShowDialog();
        }
    }
}
