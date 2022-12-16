using System;
using System.IO;

namespace AokanaEx2_Extract
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: ext.exe <*.dat file path> [output_path]");
                Console.WriteLine("If output path not specified, a list of filenames will be shown.");
                return;
            }
            var dat_filename = args[0];
            PRead pread = null;
            if (File.Exists(dat_filename))
            {
                try
                {
                    pread = new PRead(dat_filename);
                }
                catch
                {
                }
                if (pread == null)
                {
                    Console.WriteLine(string.Format("Uncaught exception. {0} maybe an invalid .dat file.", dat_filename));
                    return;
                }
            }
            var filenames = pread.ti.Keys;
            if (args.Length == 1)
            {
                foreach(var filename in filenames)
                {
                    Console.WriteLine(filename); 
                }
            }
            if (args.Length == 2)
            {
                var output_path = args[1];
                if (Directory.Exists(output_path))
                {
                    foreach(var filename in filenames)
                    {
                        var all_bytes = pread.Data(filename);
                        var splited_filename = filename.Split("\\");
                        var save_path = Path.Combine(output_path, splited_filename[splited_filename.Length - 1]);
                        Console.WriteLine(String.Format("Saving content for {0}...", filename));
                        Directory.CreateDirectory(Path.GetDirectoryName(save_path));
                        File.WriteAllBytes(Path.Combine(output_path, save_path), all_bytes);
                    }
                    Console.WriteLine("Done");
                    return;
                }
            }
        }
    }
}
