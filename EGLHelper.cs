using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace EGLProjectPathManager
{
    class EGLHelper
    {
        string GUS_DEFAULT_PATH = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\EpicGamesLauncher\\Saved\\Config\\Windows\\GameUserSettings.ini";

        /// <summary>
        /// Checks if the GameUserSettings.ini file exists
        /// </summary>
        /// <returns></returns>
        internal bool DoesEGLGUSConfigExist()
        {
            if (File.Exists(GUS_DEFAULT_PATH))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the section exists in the .ini file
        /// </summary>
        /// <param name="section"></param>
        /// <returns>True or False based on if the Section exists</returns>
        public bool DoesSectionExist(string section = "")
        {
            string[] lines = File.ReadAllLines(GUS_DEFAULT_PATH);

            foreach (string l in lines)
            {
                if (l == "[" + section + "]")
                {
                    return true;
                }
            }

            return false;
        }
        public void AppendDirectoryToIni(string directoryPath)
        {
            // Unreal will handle the formatting of the file the next time EGL is launched

            File.AppendAllText(GUS_DEFAULT_PATH, "\n[Launcher]\nCreatedProjectPaths=" + directoryPath);
            return;
        }

        /// <summary>
        /// Creates a backup of GameUserSettings.ini
        /// </summary>
        public void CreateGSUBackup()
        {
            // Check if backup exists.
            if (!File.Exists(GUS_DEFAULT_PATH + ".backup"))
            {
                // Create a backup file with .backup as the extension
                File.Copy(GUS_DEFAULT_PATH, GUS_DEFAULT_PATH + ".backup");
            }
        }

        public List<string> GetProjectPaths()
        {
            List<string> paths = new List<string>();
            string[] lines = File.ReadAllLines(GUS_DEFAULT_PATH);

            foreach (string l in lines)
            {
                if (l.StartsWith("CreatedProjectPaths"))
                {
                    string[] split = l.Split("=");
                    paths.Add(split[1]);
                }
            }

            return paths;
        }


        public void RemoveProjectPath(string pathToRemove)
        {
            var temp = Path.GetTempFileName();

            var linesToKeep = File.ReadAllLines(GUS_DEFAULT_PATH).Where((l) => {
                var newL = l.Substring(l.IndexOf("=") + 1);
                newL = newL.ToLower();
                pathToRemove = pathToRemove.ToLower();

                pathToRemove = pathToRemove.Replace("/", "\\");

                newL = newL.Replace("/", "\\");

                newL = newL.Trim();
                pathToRemove = pathToRemove.Trim();

                if (newL == pathToRemove)
                {
                    return false;
                }
                return true;
            });

            File.WriteAllLines(temp, linesToKeep);
            File.Delete(GUS_DEFAULT_PATH);
            File.Move(temp, GUS_DEFAULT_PATH);
        }

        public bool DoesUProjectExistInFolder(string path)
        {
            if (Directory.GetFiles(path).Where(f => f.Contains(".uproject")).ToList().Count > 0)
            {
                return true;
            }
            return false;
        }

        public bool DoesDirectoryAlreadyExistInIni(string path)
        {
            string[] lines = File.ReadAllLines(GUS_DEFAULT_PATH);

            foreach (string l in lines)
            {
                if (l.StartsWith("CreatedProjectPaths="))
                {
                    var newL = l.Substring(l.IndexOf("=") + 1);
                    newL = newL.ToLower();
                    path = path.ToLower();

                    path = path.Replace("/", "\\");

                    newL = newL.Replace("/", "\\");

                    newL = newL.Trim();
                    path = path.Trim();

                    if (newL == path)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
