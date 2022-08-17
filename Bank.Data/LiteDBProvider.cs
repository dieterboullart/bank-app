using System;
using System.IO;
using LiteDB;

namespace Bank.Data
{
    public static class LiteDBProvider
    {
        private static string _dbPath;

        private static string DBPath
        {
            get
            {
                if (_dbPath == null)
                {
                    // var rootFolder = CreateRootFolder();
                    _dbPath = Path.Combine("bank.db");
                }

                return _dbPath;
            }
        }
        
        public static LiteDatabase CreateDatabase()
        {
            return new(DBPath);
        }
        
        private static string CreateRootFolder()
        {
            var developmentFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Development");
            var rootFolder = Path.Combine(developmentFolder, "LiteDBExercises");

            CreateDirectoryIfNotExists(developmentFolder);
            CreateDirectoryIfNotExists(rootFolder);

            return rootFolder;
        }

        private static void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}