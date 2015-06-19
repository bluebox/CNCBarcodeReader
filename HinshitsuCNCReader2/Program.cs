using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace HinshitsuCNCReader2
{
	class Program
	{
		static void Main(string[] args)
		{

			Console.WriteLine("***************************************\n");
			Console.WriteLine("HINSHISTU Manufacturing Private Limited\n");
			Console.WriteLine("***************************************\n");

			Console.WriteLine("\n");
			Console.WriteLine("\n");


			List<String> matches0 = Find("C:\\Users\\mithun\\Downloads\\Programs\\2015.06.18 Joanna", "SINK", "003p", false);
			
			foreach (String file in matches0)
			{
				Console.WriteLine("{0}", file);
			}

			while (true)
			{
				Console.WriteLine("Scan or type code: ");
				String line = Console.ReadLine().Trim();

				if ("exit".Equals(line, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}

				if ("dir".Equals(line, StringComparison.OrdinalIgnoreCase))
				{
					Process.Start(line);
					continue;
				}

				if (line.Length != 8)
				{
					Console.WriteLine("Input must be 8 charaters.");
					continue;
				}

				if (line[3] != '_')
				{
					Console.WriteLine("Module name prefix must be 001_ format.");
					continue;
				}

				if (line[7] != 'p')
				{
					Console.WriteLine("File name prefix must be in 001p format.");
					continue;
				}

				List<String> matches = Find(Path.GetFullPath("."), line.Substring(0, 4), line.Substring(4, 4), false);

				if (matches.Count == 0)
				{
					Console.WriteLine("No match found in directory tree.");
					continue;
				}

				if (matches.Count > 1)
				{
					Console.WriteLine("Multiple matches found.");

					foreach (String file in matches)
					{
						Console.WriteLine("{0}", file);
					}

					continue;
				}

				String match = matches[0];

				Console.WriteLine("Match found:");
				Console.WriteLine("{0}", match);

				Process.Start(match);
			}
		}

		/// <summary>
		/// Finds a cix file where 
		/// 
		/// cix.directory[0:4] == folderPrefix
		/// cix.filename[0:4] == filePrefix
		/// 
		/// </summary>
		/// <param name="folder">
		///		The current search folder.
		///		Assumes full folder path is given.	
		/// </param>
		/// <param name="folderPrefix"></param>
		/// <param name="filePrefix"></param>
		/// <param name="folderMatched">
		///		True, if the containing folder has matched folderPrefix.
		///	</param>
		/// <returns>Full path of all matching cix file.</returns>
		public static List<String> Find(String folderPath, String folderPrefix, String filePrefix, Boolean folderMatched)
		{
			List<String> matches = new List<String>();
			String folderName = Path.GetFileName(folderPath);

			if (folderName.StartsWith(folderPrefix, StringComparison.OrdinalIgnoreCase))
			{
				folderMatched = true;
			}

			if (folderMatched)
			{
				// Search for matching file.
				foreach (String filePath in Directory.GetFiles(folderPath, filePrefix + "*.cix"))
				{
					String fileName = Path.GetFileName(filePath);

					if (fileName.StartsWith(filePrefix, StringComparison.OrdinalIgnoreCase))
					{
						matches.Add(filePath);
					}
				}
			}

			foreach (String sub in Directory.GetDirectories(folderPath))
			{
				matches.AddRange(Find(sub, folderPrefix, filePrefix, folderMatched));
			}

			return matches;
		}
	}
}
