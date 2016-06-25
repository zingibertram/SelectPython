using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows;

namespace SelectPython.Core
{
    public class PythonInfo
    {
        public static List<String> Get(String pyDirPath)
        {
            List<String> res = null;
            try
            {
                var pyPath = Path.Combine(pyDirPath, "python.exe");
                if (File.Exists(pyPath))
                {
                    var verScriptPath = Path.Combine(new String[] { AppDomain.CurrentDomain.BaseDirectory, "Core", "Python", "PythonVersion.py" });
                    verScriptPath = String.Format("\"{0}\"", verScriptPath);

                    var process = new Process();
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.FileName = pyPath;
                    process.StartInfo.Arguments = verScriptPath;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.WaitForExit();
                    var verInfo = process.StandardOutput.ReadToEnd();
                    var error = process.StandardError.ReadToEnd();
                    if (!String.IsNullOrEmpty(error))
                    {
                        MessageBox.Show("PROCESS Error\"" + error + "\"");
                        return null;
                    }
                    else
                    {
                        verInfo = verInfo.Replace("\r\n", "");
                        verInfo = verInfo.Replace("(", "");
                        verInfo = verInfo.Replace(")", "");
                        var infos = verInfo.Split(new[] { "//" }, StringSplitOptions.RemoveEmptyEntries);
                        res = new List<String> { pyDirPath, infos[0], infos[1], infos[3] };
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return res;
        }
    }
}
