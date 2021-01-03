using System;
using System.IO;

namespace ProductManager.ViewModel.Controller
{
    public class FilesController
    {
        private string _folderTarget;
        private string _fileOriginFullPath;
        private string _fileName;

        public string FolderTarget => _folderTarget;
        public string FileOriginFullPath => _fileOriginFullPath;
        public string FileName => _fileName;
        public enum FileType
        {
            Image,
            ImageArchived,
            Settings,
            Products
        }

        /// <summary>
        /// Konstruktor für spezifische Speicherorte.
        /// </summary>
        /// <param name="origin">Originaldatei mit vollständigem Pfad</param>
        /// <param name="type">Vordefinierte Speicherorte.
        /// <see cref="FileType.Image"/>, <see cref="FileType.Settings"/>, <see cref="FileType.Products"/></param>
        /// <param name="filename">Dateiname ohne erweiterung. Z.B *.jpg</param>
        public FilesController(FileType type, string filename, string origin = null)
        {
            _fileOriginFullPath = origin;
            _fileName = filename;
            _folderTarget = type switch
            {
                FileType.Image => Properties.IMAGE_PATH,
                FileType.ImageArchived => Properties.IMAGEARCHIVED_PATH,
                FileType.Settings => Properties.SETTINGS_PATH,
                FileType.Products => Properties.PRODUCTS_PATH,
                _ => throw new NotImplementedException("Not supportet Filetype format."),
            };
        }

        /// <summary>
        /// Speichert die angegebene Datei. Dazu muss ein <seealso cref="FilesController"/> übergeben werden.
        /// Benötigt den Ursprungspfad, Zielpfad und Dateinamen
        /// </summary>
        /// <param name="fc"></param>
        /// <returns>Den vollständigen Pfad der Kopierten Datei als <see cref="string"/></returns>
        public static string Save(FilesController fc)
        {
            if (File.Exists(fc._fileOriginFullPath))
            {
                fc._fileName += Path.GetExtension(fc._fileOriginFullPath);

                if (fc._fileOriginFullPath != Path.Combine(fc._folderTarget, fc._fileName))
                {
                    if (!Directory.Exists(fc._folderTarget))
                    {
                        Directory.CreateDirectory(fc._folderTarget);
                    }

                    if (!File.Exists(Path.Combine(fc._folderTarget, fc._fileName)))
                    {
                        File.Copy(fc._fileOriginFullPath, Path.Combine(fc._folderTarget, fc._fileName));
                    }
                }
            }

            return fc._fileName;
        }

        /// <summary>
        /// Löscht eine angegebene Datei, entsprechend dem Dateityp
        /// </summary>
        /// <param name="fc">Ein <see cref="FilesController"/> Objekt, mit dem Pfad und Dateiname.</param>
        public static void Delete(FilesController fc)
        {
            if (File.Exists(fc.FolderTarget + fc.FileName))
            {
                File.Delete(fc.FolderTarget + fc.FileName);
            }
        }

        /// <summary>
        /// Verschiebt eine Datei
        /// </summary>
        /// <param name="fc"></param>
        public static void Move(FilesController fc)
        {
            if (File.Exists(fc.FileOriginFullPath + fc.FileName))
            {
                if (!Directory.Exists(fc.FolderTarget))
                {
                    Directory.CreateDirectory(fc.FolderTarget);
                }

                File.Move(fc.FileOriginFullPath + fc.FileName, fc.FolderTarget + fc.FileName, true);
            }
        }
    }
}