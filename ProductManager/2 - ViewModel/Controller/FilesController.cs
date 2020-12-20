using System.IO;

namespace ProductManager.ViewModel.Controller
{
    public class FilesController
    {
        private readonly string IMAGE_PATH = Directory.GetCurrentDirectory() + @"\Images\";
        private readonly string SETTINGS_PATH = Directory.GetCurrentDirectory() + @"\Settings\";
        private readonly string PRODUCTS_PATH = Directory.GetCurrentDirectory() + @"\Products\";

        private string _folderTarget;
        private string _fileOriginFullPath;
        private string _newFileName;

        public string FolderTarget => _folderTarget;
        public string FileOriginFullPath => _fileOriginFullPath;
        public string NewFileName => _newFileName;
        public enum Type
        {
            Image,
            Settings,
            Products
        }

        /// <summary>
        /// Allgemeiner Konstruktor
        /// </summary>
        /// <param name="origin">Originaldatei mit vollständigem Pfad</param>
        /// <param name="target">Zielordner, ohne Dateiangabe</param>
        /// <param name="filename">Dateiname ohne erweiterung. Z.B *.jpg</param>
        public FilesController(string origin, string target, string filename)
        {
            _fileOriginFullPath = origin;
            _folderTarget = target;
            _newFileName = filename;
        }

        /// <summary>
        /// Systeminterner Konstruktor für spezifische Speicherorte.
        /// </summary>
        /// <param name="origin">Originaldatei mit vollständigem Pfad</param>
        /// <param name="type">Für Systeminterne Speicherorte.</param>
        /// <param name="filename">Dateiname ohne erweiterung. Z.B *.jpg</param>
        public FilesController(string origin, Type type, string filename)
        {
            _folderTarget = type switch
            {
                Type.Image => IMAGE_PATH,
                Type.Settings => SETTINGS_PATH,
                Type.Products => PRODUCTS_PATH,
                _ => throw new System.NotImplementedException(),
            };

            _fileOriginFullPath = origin;
            _newFileName = filename;
        }

        /// <summary>
        /// Speichert die angegebene Datei. Dazu muss ein <seealso cref="FilesController"/> übergeben werden.
        /// </summary>
        /// <param name="fc"></param>
        /// <returns>Den vollständigen Pfad der Kopierten Datei als <see cref="string"/></returns>
        public static string Save(FilesController fc)
        {
            if (File.Exists(fc._fileOriginFullPath))
            {
                fc._newFileName += Path.GetExtension(fc._fileOriginFullPath);

                if (fc._fileOriginFullPath != Path.Combine(fc._folderTarget, fc._newFileName))
                {
                    if (!Directory.Exists(fc._folderTarget))
                    {
                        Directory.CreateDirectory(fc._folderTarget);
                    }

                    if (!File.Exists(Path.Combine(fc._folderTarget, fc._newFileName)))
                    {
                        File.Copy(fc._fileOriginFullPath, Path.Combine(fc._folderTarget, fc._newFileName));
                    }
                }
            }

            return Path.Combine(fc._folderTarget, fc._newFileName);
        }

        /// <summary>
        /// Löscht eine angegebene Datei
        /// </summary>
        /// <param name="path">Der vollständige Pfad zur löschenden Datei</param>
        public static void Delete(string path)
        {
            if (File.Exists(path))
            {
                // Muss noch implementiert werden.
                // Probleme mit "Datei wird bereits ausgeführt"
                throw new System.NotImplementedException();
            }
        }
    }
}