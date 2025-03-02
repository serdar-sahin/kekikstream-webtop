using Python.Included;
using Python.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.DependencyInjection;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;
using Microsoft.AspNetCore.Http;

namespace KekikStream.PythonInterop
{
    public class PythonService: IPythonService, IDisposable
    {
        public PythonService() 
        {
            //var result = CheckLocalPython();

            // PS C:\Users\serdar_dell\AppData\Roaming\Python\Python312\site-packages\KekikStreamAPI> python __init__.py
        }

        /// <summary>
        /// Check python environment for all operating systems and set Python.Runtime
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckGlobalPython()
        {
            try
            {
                string[]? pythonPaths;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    pythonPaths = await GetWindowsPythonPath();
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    pythonPaths = await GetUnixPythonPath();
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    pythonPaths = await GetUnixPythonPath();
                }
                else
                {
                    return false;
                }

                
                if (pythonPaths != null && pythonPaths.Length > 0)
                {
                    string? pythonPath = pythonPaths.Where(x => x.Contains("python3")).FirstOrDefault();

                    if (pythonPath is null)
                    {
                        foreach (var path in pythonPaths)
                        {
                            pythonPath = path;
                            break;
                        }
                    }

                    if (pythonPath != null)
                    {
                        Log("Global Python path: " + pythonPath);

                        string? version = await GetPythonVersion();

                        if (!string.IsNullOrEmpty(version))
                        {
                            Log("Global Python version: " + version);

                            string? pythonLibName = GetPythonLibraryName(version);

                            Log("Global Python lib: " + pythonLibName);

                            if (!string.IsNullOrEmpty(pythonLibName))
                            {
                                Installer.InstallPath = pythonPath;

                                string pythonLibPath = Path.Combine(pythonPath, pythonLibName);

                                Log("Global Python lib path: " + pythonLibPath);


                                Runtime.PythonDLL = pythonLibPath;
                                PythonEngine.Initialize();

                                dynamic sys = Py.Import("sys");
                                string? pyVersion = (string)sys.version;

                                Log("Python.Runtime checked version: " + pyVersion);
                                if (!string.IsNullOrEmpty(pyVersion))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

            return false;
        }

        /// <summary>
        /// Check local embedded python environment for windows only
        /// </summary>
        /// <returns></returns>
        public bool CheckLocalPython()
        {
            try
            {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string appFolder = Path.Combine(appDataPath, "kekik");
                if (!Directory.Exists(appFolder))
                {
                    Log("appFolder is not exist");
                    return false;
                }

                Installer.InstallPath = appFolder;
                Log("appFolder: " + appFolder);
                Log("EmbeddedPythonHome: " + Installer.EmbeddedPythonHome);

                string pythonDllPath = Path.Combine(Installer.EmbeddedPythonHome, "python311.dll");

                Runtime.PythonDLL = pythonDllPath;

                PythonEngine.Initialize();
                string apiPath = @"\Lib\site-packages\KekikStreamAPI";

                string fullPath = Installer.EmbeddedPythonHome + apiPath;

                dynamic sys = Py.Import("sys");
                sys.path.append(fullPath);
                Log("Python.Engine.Initialize");

                string version = (string)sys.version;
                Log("Python version: " + version);

                if (!string.IsNullOrEmpty(version))
                {
                    //var apiResult = StartKekikStreamApi();
                    var apiResult = StartKekikStreamApiFromTerminal(Installer.EmbeddedPythonHome);
                    Log("apiResult: " + apiResult);
                    return apiResult;
                }
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

            return false;
        }


        /// <summary>
        /// Windows spesific
        /// </summary>
        /// <returns></returns>
        public async Task<bool> InstallLocalPythonAsync()
        {
           string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(appDataPath, "kekik");
            if (!Directory.Exists(appFolder))
            {
                Directory.CreateDirectory(appFolder);
            }

            try
            {
                // app data paths
                // windows: C: \Users\< UserName >\AppData\Roaming
                // macOS: / Users /< UserName >/ Library/Application Support
                // linux: / home /< UserName >/.config

                // set the download source
                //Python.Deployment.Installer.Source = new Python.Deployment.Installer.DownloadInstallationSource()
                //{
                //    DownloadUrl = @"https://www.python.org/ftp/python/3.14.0/python-3.14.0-embed-amd64.zip",
                //};

                // install in local app data of user account
                Installer.InstallPath = appFolder;

                // install in local directory
                //Installer.InstallPath = Path.GetFullPath(".");

                // see what the installer is doing
                Installer.LogMessage += Log;

                // install from the given source
                await Installer.SetupPython();
                Log("Python Ok");

                // install pip3 for package installation
                //Debug.WriteLine("check pip");
                //if (!Installer.IsPipInstalled())
                //{
                Log("installing Pip");
                await Installer.InstallPip();
                Log("Pip Ok");
                //}

                // download and install kekikstream from the internet
                //Debug.WriteLine("check Kekik");
                //if (!Installer.IsModuleInstalled("KekikStream"))
                //{
                Log("installing KekikStream");
                await Installer.PipInstallModule("KekikStream");
                Log("KekikStream Ok");
                //}

                return true;
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

            return false;
        }

        /// <summary>
        /// Windows spesific
        /// </summary>
        /// <returns></returns>
        public async Task<bool> InstallLocalKekikStream()
        {
            try
            {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string appFolder = Path.Combine(appDataPath, "kekik");
                Installer.InstallPath = appFolder;
                Installer.LogMessage += Log;

                // install pip3 for package installation
                if (!Installer.IsPipInstalled())
                {
                    await Installer.TryInstallPip();
                    Debug.WriteLine("Pip Ok");
                }

                // download and install kekikstream from the pip
                if (!Installer.IsModuleInstalled("KekikStream"))
                {
                    await Installer.PipInstallModule("KekikStream");
                    Debug.WriteLine("KekikStream Ok");
                }

                return true;

            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

            return false;
        }

        /// <summary>
        /// Windows spesific
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UpdatelocalKekikStream()
        {
            bool result = false;

            try
            {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string appFolder = Path.Combine(appDataPath, "kekik");
                Installer.InstallPath = appFolder;
                Installer.LogMessage += Log;

                // install pip3 for package installation
                if (!Installer.IsPipInstalled())
                {
                    await Installer.TryInstallPip();
                    Log("Pip Ok");
                }

                // delete old one and download and install kekikstream from the internet
                //await Installer.PipInstallModule("KekikStream");
                //await Installer.PipInstallModule("KekikStream", "1.1.3", true);

                // update kekikstream from the pip
                string pipPath = Path.Combine(Installer.EmbeddedPythonHome, "Scripts", "pip");
                string moduleName = "KekikStream";

                Python.Deployment.Installer.PythonDirectoryName = Installer.EmbeddedPythonHome;
                Python.Deployment.Installer.LogMessage += (log) =>
                {
                    if (log.Contains("Successfully installed KekikStream"))
                    {
                        //Debug.WriteLine(log);
                        result = true;
                    }
                };

                Python.Deployment.Installer.RunCommand($"\"{pipPath}\" install -U \"{moduleName}\" ");

                Log("KekikStream Ok");
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

            return result;
        }


        /// <summary>
        /// Get python path for windows
        /// </summary>
        /// <returns></returns>
        private async Task<string[]?> GetWindowsPythonPath()
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "where",
                    Arguments = "python",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processStartInfo))
                {
                    using (var reader = process.StandardOutput)
                    {
                        string output = await reader.ReadToEndAsync();
                        return output.Replace(@"\python.exe", "").Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        //return output.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex.ToString() );
            }

            return null;
        }

        /// <summary>
        /// Get python path for unix based linux and macos
        /// </summary>
        /// <returns></returns>
        private async Task<string[]?> GetUnixPythonPath()
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "which",
                    Arguments = "-a python",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processStartInfo))
                {
                    using (var reader = process.StandardOutput)
                    {
                        string output = await reader.ReadToEndAsync();
                        return output.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        //return output.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

            return null;

            // /usr/local/bin/python
            // /usr/bin/python3

        }

        private async Task<string?> GetPythonVersion()
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = "--version",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processStartInfo))
                {
                    using (var reader = process.StandardOutput)
                    {
                        string output = await reader.ReadToEndAsync();
                        return output.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

            return null;

            // Python 3.11.0
        }

        
        public bool StartKekikStreamApi()
        {
            bool result = false;

            try
            {
                string pythonPath = Path.Combine(Installer.EmbeddedPythonHome, "python");
                string apiPath = Path.Combine(Installer.EmbeddedPythonHome, "Lib\\site-packages\\KekikStreamAPI\\basla.py");
                
                Python.Deployment.Installer.PythonDirectoryName = Installer.EmbeddedPythonHome;
                Python.Deployment.Installer.LogMessage += (log) =>
                {
                    Log(log.ToString());   
                    if (log.Contains("başlatılmıştır"))
                    {
                        result = true;
                    }
                };

                Python.Deployment.Installer.RunCommand($"\"{pythonPath}\" \"{apiPath}\" ");
                //Python.Deployment.Installer.RunCommand($" \"{apiPath}\" ");

                Log("KekikStreamAPI Ok");
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

            return result;
        }

        private async Task<bool> StartKekikStreamApiFromTerminalAsync(string path)
        {
            try
            {
                // C:\Users\user\AppData\Roaming\kekik\python-3.11.0-embed-amd64> ./python.exe .\Lib\site-packages\KekikStreamAPI\basla.py
                Log(path + @"\python.exe");

                string command = path + @"\python.exe" + " " + path + @"\Lib\site-packages\KekikStreamAPI\basla.py";

                string fileName = "cmd.exe";
                string args = "/C \"" + command + "\"";

                var processStartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = false
                };

                using (var process = Process.Start(processStartInfo))
                {
                    using (var reader = process.StandardOutput)
                    {
                        string output = await reader.ReadToEndAsync();
                        Log("StartKekikStreamApi output: " + output);

                        if (output.Trim().Contains("başlatılmıştır"))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

            return false;

            // Python 3.11.0
        }

        private bool StartKekikStreamApiFromTerminal(string path)
        {
            try
            {
                // C:\Users\user\AppData\Roaming\kekik\python-3.11.0-embed-amd64> ./python.exe .\Lib\site-packages\KekikStreamAPI\basla.py
                Log(path + @"\python.exe");

                string command = path + @"\python.exe" + " " + path + @"\Lib\site-packages\KekikStreamAPI\start.py";

                string fileName = "cmd.exe";
                string args = "/C \"" + command + "\"";

                var processStartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = false
                };

                using (var process = Process.Start(processStartInfo))
                {
                    using (var reader = process.StandardOutput)
                    {
                        string output = reader.ReadToEnd();
                        Log("StartKekikStreamApi output: " + output);

                        if (output.Trim().Contains("başlatılmıştır"))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

            return false;

            // Python 3.11.0
        }

        public void StartKekikStreamApiFromEngine()
        {
            if (!PythonEngine.IsInitialized)
            {
                Initialize();
            }

            using (Py.GIL())
            {
                //dynamic cli = Py.Import("KekikStreamAPI.CLI");
                //dynamic core = Py.Import("KekikStreamAPI.Core.__init__");
                //dynamic api = Py.Import("KekikStreamAPI.basla");
                dynamic api = Py.Import("KekikStreamAPI.Core.Motor");
                api.basla();
                Log("KekikStreamAPI started");
            }
        }


        /// <summary>
        /// Get platform specific python executable file name
        /// </summary>
        /// <param name="pythonVersion"></param>
        /// <returns></returns>
        public string? GetPythonLibraryName(string pythonVersion)
        {
            // ex: Python 3.11.0
            string version = pythonVersion.Split(' ')[1];  // ex: 3.11.0
            string unixMajorMinorVersion = version.Substring(0, 4);  // 3.11
            string windowsMajorMinorVersion = unixMajorMinorVersion.Replace(".", ""); // 311

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return $"python{windowsMajorMinorVersion}.dll";  // ex "python311.dll"
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return $"libpython{unixMajorMinorVersion}.so";  // ex "libpython3.11.so"
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return $"libpython{unixMajorMinorVersion}.dylib";  // ex "libpython3.11.dylib"
            }

            return null;
        }

        private void Initialize()
        {
            PythonEngine.Initialize();
            string rootPath = Installer.InstallPath;

            string apiPath = @"\site-packages\KekikStreamAPI";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                apiPath = @"\Lib\site-packages\KekikStreamAPI";
            }

            string sysPath = Path.Combine(rootPath, apiPath);
            Log("sysPath: " + sysPath);

            dynamic sys = Py.Import("sys");
            sys.path.append(sysPath);
            Log("Python.Engine.Initialize");
        }


        public async Task<string> HttpGet(string url)
        {
            try
            {
                Uri uri = new Uri(url);

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Referer", url);
                    client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");
                    client.DefaultRequestHeaders.Add("authority", uri.Authority);
                    client.DefaultRequestHeaders.Add("origin", url);
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.81 Safari/537.36");

                    using (HttpResponseMessage response = await client.GetAsync(url))
                    {
                        using (HttpContent content = response.Content)
                        {

                            string json = await content.ReadAsStringAsync();
                            return json;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

            return string.Empty;
        }


        // todo: add logger
        private void Log(string message)
        {
            Debug.WriteLine(message);

            //try
            //{

            //}
            //catch (Exception ex)
            //{
            //    Log(ex.ToString());
            //}
        }

        public void Dispose()
        {
            PythonEngine.Shutdown();
            //AppContext.SetSwitch("System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", false);
        }
    }
}
