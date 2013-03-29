#region Imports (5)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#endregion Imports (5)

namespace Minova.ClientApi
{


    public class MinovaClient
    {
        #region Enums of MinovaClient (2)

        private List<string> _repositories = new List<string>(new[] {
            "http://repo.minova.tk/minecraft/",
            "http://repo.minova.tk/libraries/",
            "http://repo.minova.tk/mods/"
        });
        private string _targetDirectory = Environment.CurrentDirectory;

        #endregion Enums of MinovaClient (2)

        #region Properties of MinovaClient (7)

        internal DirectoryInfo MinovaDirectoryInfo { get; private set; }

        internal string MinovaRepositoriesListFilePath { get { return Path.Combine(MinovaDirectoryInfo.FullName, "repositories.list"); } }

        public List<string> Repositories { get { return _repositories; } }

        internal Uri RepositoryEnhancementsUrl { get { return new Uri(RepositoryUrl, "enhanced/"); } }

        public Uri RepositoryUrl { get; private set; }

        public string TargetDirectory { get { return _targetDirectory; } set { _targetDirectory = value; } }

        internal DirectoryInfo TargetDirectoryInfo { get { return new DirectoryInfo(_targetDirectory); } }

        #endregion Properties of MinovaClient (7)

        #region Methods of MinovaClient (4)

        private void _internalTargetInit()
        {
            MinovaDirectoryInfo = TargetDirectoryInfo.CreateSubdirectory("minova");
            if (!File.Exists(MinovaRepositoriesListFilePath))
                _saveRepositoriesList();
            else
                _loadRepositoriesList();
        }

        private void _loadRepositoriesList()
        {
            Repositories.Clear();
            Repositories.AddRange(File.ReadAllLines(MinovaRepositoriesListFilePath));
        }

        private void _saveRepositoriesList()
        {
            File.WriteAllLines(MinovaRepositoriesListFilePath, Repositories);
        }

        public void Save()
        {
            _saveRepositoriesList();
        }

        #endregion Methods of MinovaClient (4)
    }
}
