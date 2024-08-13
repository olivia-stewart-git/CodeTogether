using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeTogether.Deployment
{
	internal class ScriptRunner
	{
		/// <returns>If it was successful</returns>
		public static bool RunPowershellScript()
		{
			Console.WriteLine("Executing auto deployment");
			var assembly = Assembly.GetExecutingAssembly();

			using var stream = assembly.GetManifestResourceStream("CodeTogether.Deployment.Deployment.ps1");

			if (stream == null)
			{
				Console.WriteLine("PowerShell script not found.");
				return false;
			}

			using var reader = new StreamReader(stream);
			var scriptContent = reader.ReadToEnd();

			// Write the script content to a temporary file
			var assemblyDirectory = Directory.GetParent(assembly.Location)!.FullName;
			var tempScriptPath = Path.Combine(assemblyDirectory, "tempDeployment.ps1");
			File.WriteAllText(tempScriptPath, scriptContent);

			try
			{
				var startInfo = new ProcessStartInfo
				{
					FileName = "powershell.exe",
					Arguments = $"-ExecutionPolicy Bypass -File \"{tempScriptPath}\" \"{Directory.GetParent(assemblyDirectory)?.FullName}\"",
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				};

				bool hasErrors = false;
				using (var process = Process.Start(startInfo))
				{
					if (process != null)
					{
						// Capture output and errors in real-time
						process.OutputDataReceived += (sender, e) =>
						{
							if (!string.IsNullOrEmpty(e.Data))
							{
								Console.WriteLine(e.Data);
							}
						};
						process.ErrorDataReceived += (sender, e) =>
						{
							if (!string.IsNullOrEmpty(e.Data))
							{
								Console.WriteLine(e.Data);
								hasErrors = true;
							}
						};

						// Begin reading output and errors
						process.BeginOutputReadLine();
						process.BeginErrorReadLine();

						process.WaitForExit();
					}
					else
					{
						Console.WriteLine("Failed to start PowerShell process.");
						hasErrors = true;
					}
				}

				return !hasErrors;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
				return false;
			}
			finally
			{
				// Clean up the temporary file
				if (File.Exists(tempScriptPath))
				{
					File.Delete(tempScriptPath);
				}
			}
		}
	}
}
