using System;
using System.Collections.Generic;

using Mono.Cecil;

namespace getclrinterface
{
	class MainClass
	{
		public static void PrintInterface (InterfaceInfo info)
		{
			foreach (KeyValuePair<AssemblyNameReference, ModuleInterfaceInfo> kvp in info)
			{
				System.Console.WriteLine(kvp.Key.ToString());
			}
		}
		
		public static void Main (string[] args)
		{
			bool exports;
			string filename;
			AssemblyDefinition assembly;
			InterfaceInfo info = new InterfaceInfo();

			if (args[0] == "-i")
			{
				exports = false;
				filename = args[1];
			}
			else if (args[0] == "-e")
			{
				exports = true;
				filename = args[1];
			}
			else
			{
				exports = true;
				filename = args[0];
			}

			assembly = AssemblyDefinition.ReadAssembly(filename);

			if (!exports)
				info.ReadAssemblyImports(assembly);
			else
				info.ReadAssemblyExports(assembly);
			
			PrintInterface(info);
		}
	}
}

