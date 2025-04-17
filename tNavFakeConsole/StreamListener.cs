using System;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace tNav.FakeConsole;

public class StreamListener
{
    Stream reader;
    public StreamListener(Stream reader)
    {
        this.reader = reader;
        LastAccess=DateTime.Now;
        Thread inputThread = new Thread(() =>
        {
            Listen();
        })
        {
            IsBackground = true
        };
        inputThread.Start();

    }

    public string GetString()
    {
        var value = sb.ToString();
        sb.Clear();
        return value;
    }
    private StringBuilder sb = new();
    private DateTime LastAccess;
    public double Age => (DateTime.Now - LastAccess).TotalMilliseconds;
    public void Listen()
    {
        byte[] buffer = new byte[4096];
        while (true)
        {
            var count = reader.Read(buffer);
            if (count > 0)
            {
                string result = System.Text.Encoding.UTF8.GetString(buffer, 0, count);
                LastAccess = DateTime.Now;
                sb.Append(result);
            }
            else
            {
                if (sb.Length == 0)
                {
                    LastAccess = DateTime.Now;
                }
            }
        }

    }
}