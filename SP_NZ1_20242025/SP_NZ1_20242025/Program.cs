using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

class Program
{
    private const string DataFileName = "data.txt";
    private static readonly Random RandomGenerator = new Random();

    static void Main()
    {
        int pid = Process.GetCurrentProcess().Id;
        string logFileName = $"{pid}.txt";

        // Osigurajte da je data.txt veličine 1MB
        EnsureDataFile();

        using (var logWriter = new StreamWriter(logFileName, true))
        {
            while (true)
            {
                try
                {
                    // Generiranje slučajnih brojeva za zapis
                    int offset = RandomGenerator.Next(0, 1024) * 1024; // Pozicija u višekratnicima od 1024
                    int dataSize = RandomGenerator.Next(1, 65) * 1024; // Veliko od 1KB do 64KB

                    // Generiraj podatke za zapisivanje (PID na širini od 8 bajtova)
                    byte[] data = GenerateData(pid, dataSize);

                    // Zaključaj raspon i piši
                    if (TryWriteData(offset, data))
                    {
                        Log(logWriter, $"Uspješno zapisano {dataSize / 1024}KB na poziciji {offset}");
                    }
                    else
                    {
                        Log(logWriter, $"Zapisivanje nije uspjelo na poziciji {offset}");
                    }

                    // Pauza od 100ms nakon svakih 128 zapisa (1KB)
                    if (dataSize >= 1024)
                    {
                        Thread.Sleep(100 * (dataSize / 1024));
                    }
                }
                catch (Exception ex)
                {
                    Log(logWriter, $"Greška: {ex.Message}");
                }

                // Pauza između zapisa (1-5 sekundi)
                Thread.Sleep(RandomGenerator.Next(1000, 5001));
            }
        }
    }

    private static void EnsureDataFile()
    {
        if (!File.Exists(DataFileName))
        {
            using (var fs = new FileStream(DataFileName, FileMode.Create, FileAccess.Write))
            {
                fs.SetLength(1024 * 1024); // Postavi veličinu datoteke na 1MB
            }
        }
    }

    private static byte[] GenerateData(int pid, int size)
    {
        byte[] data = new byte[size];
        string pidStr = pid.ToString("D8"); // PID na širini od 8 znakova

        for (int i = 0; i < data.Length; i += 8)
        {
            byte[] pidBytes = Encoding.ASCII.GetBytes(pidStr);
            Array.Copy(pidBytes, 0, data, i, Math.Min(8, data.Length - i));
        }

        return data;
    }

    private static bool TryWriteData(int offset, byte[] data)
    {
        try
        {
            using (var fs = new FileStream(DataFileName, FileMode.Open, FileAccess.Write, FileShare.None))
            {
                fs.Lock(offset, data.Length);
                fs.Seek(offset, SeekOrigin.Begin);
                fs.Write(data, 0, data.Length);
                fs.Unlock(offset, data.Length);
                return true;
            }
        }
        catch (IOException)
        {
            return false; // Ako je dio datoteke već zaključan
        }
    }

    private static void Log(StreamWriter logWriter, string message)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        logWriter.WriteLine($"{timestamp} {message}");
        logWriter.Flush();
    }
}
