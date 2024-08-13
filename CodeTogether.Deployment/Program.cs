using CodeTogether.Data;
using CodeTogether.Data.Seeding;
using CodeTogether.Services.Seeding;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;

namespace CodeTogether.Deployment;

class Program
{
	static void Main(string[] args)
	{
		if (!args.Contains("SeedOnly"))
		{
			var ranSuccessfully = RunPowershellScript();
			if (!ranSuccessfully)
			{
				return;
			}
		}
		else
		{
			Console.WriteLine("Skipping powershell script");
		}

		SetupDb();
	}

	static void SetupDb()
	{
		Console.WriteLine("Connecting to database");
		using var dbContext = new ApplicationDbContext();
		dbContext.Database.BeginTransaction();

		Console.WriteLine("Ensure Database Exists");
		dbContext.Database.EnsureCreated();
		Console.WriteLine("Running migrations");
		dbContext.Database.Migrate();

		dbContext.Database.CommitTransaction();

		var seeder = new Seeder(dbContext, Console.WriteLine);
		seeder.ExplicitSeed(
			typeof(UserSeeder),
			typeof(QuestionSeeder),
			typeof(SchemaVersionSeeder)
		);
	}

	static bool RunPowershellScript()
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