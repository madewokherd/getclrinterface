using System;
using System.Collections.Generic;

using Mono.Cecil;

namespace getclrinterface
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			bool exports = true;
			AssemblyDefinition assembly;
			InterfaceInfo info = new InterfaceInfo();
			int arg;

			if (args[0] == "-i")
			{
				exports = false;
				arg = 1;
			}
			else if (args[0] == "-e")
			{
				exports = true;
				arg = 1;
			}
			else
			{
				exports = true;
				arg = 0;
			}

			while (arg < args.Length)
			{
				if (args[arg] == "-i")
				{
					exports = false;
					continue;
				}
				else if (args[arg] == "-e")
				{
					exports = true;
					continue;
				}

				try
				{
					assembly = AssemblyDefinition.ReadAssembly(args[arg]);

					if (!exports)
						info.ReadAssemblyImports(assembly);
					else
						info.ReadAssemblyExports(assembly);
				}
				catch (System.BadImageFormatException)
				{
					Console.WriteLine ("{0}: not a CLR image.", args[arg]);
				}

				arg++;
			}
			
			info.PrintInterface();
		}
	}
}

