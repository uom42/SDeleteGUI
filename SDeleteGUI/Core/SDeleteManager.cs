using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDeleteGUI.Core
{
	internal class SDeleteManager
	{
		private const string C_SDBIN_DIR = @"C:\ProgramData\chocolatey\lib\sysinternals\tools";
		private const string C_SDBIN_FILE64 = @"sdelete64.exe";
		private const string C_SDBIN_FILE = @"sdelete.exe";

		public readonly FileInfo Binary;


		/*
		usage: sdelete [-p passes] [-r] [-s] [-q] <file or directory> [...]
		sdelete [-p passes] [-z|-c [percent free]] <drive letter [...]>
		sdelete [-p passes] [-z|-c] <physical disk number>

		-c         Clean free space. Specify an option amount of space
			to leave free for use by a running system.
		-f         Force arguments containing only letters to be treated as a file/directory rather than a disk.
			Not required if the argument contains other characters (path separators or file extensions for example)
		-p         Specifies number of overwrite passes (default is 1)
		-r         Remove Read-Only attribute
		-s         Recurse subdirectories
		-z         Zero free space (good for virtual disk optimization)
		-nobanner  Do not display the startup banner and copyright message.
		 */

		private const string C_ARG_CLEAN_FREE_SPACE = @"-c";
		private const string C_ARG_FORCE_PATH = @"-f";
		private const string C_ARG_PASSES = @"-p";
		private const string C_ARG_REMOVE_RO = @"-r";
		private const string C_ARG_RECURSE = @"-s";
		private const string C_ARG_ZERO_FREE_SPACE = @"-z";
		private const string C_ARG_NO_BANNER = @"-nobanner";

		public SDeleteManager() {
			DirectoryInfo diBin = new(C_SDBIN_DIR);
			if (!diBin.Exists)
			{
				throw new Exception ( $"Not found dir '{C_SDBIN_DIR}'!");				
			}

			var found64 = diBin.EnumerateFiles(C_SDBIN_FILE64).Any();
			var found32 = diBin.EnumerateFiles(C_SDBIN_FILE).Any();

			if (!found64 && !found32)
			{
				throw new Exception($"Not found SDelete binary file '{C_SDBIN_FILE}'!");				
			}

			//public readonly FileInfo? fiBin = null;


			Binary = new(Path.Combine(diBin.FullName, 
				found64
				? C_SDBIN_FILE64
				: C_SDBIN_FILE));
		}

		/// <summary>CleanDirectory
		/// usage: sdelete [-p passes] [-r] [-s] [-q] <file or directory> [...]
		/// </summary>
		public void Run(DirectoryInfo dirToClean) { 

		}
	}
}
