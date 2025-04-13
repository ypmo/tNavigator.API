using System;

namespace tNav.FakeConsole;

public class LogParser
{
    const string startWord = "***INPUT***";
    const string endWord = "***END***";
    public static List<Seans> Parse(string log)
    {
        List<Seans> result = [];
        TextReader reader = new StringReader(log);
        reader.ReadLine();
        reader.ReadLine();

        while (true)
        {
            Seans S = new();
            var query = GetQuery(reader);
            if (query == null) return result;
            S.Query = query;

            S.Responses = GetResponses(reader);
            result.Add(S);
        }
        ;
    }

    static List<Response> GetResponses(TextReader reader)
    {
        List<Response> result = [];
        while (true)
        {
            Response response = new Response();
            var line = reader.ReadLine();
            if (line == null) { return result; }
            if (line.Contains(startWord))
            {
                int index = line.IndexOf(startWord);
                if (index < 0)
                {
                    throw new InvalidOperationException("������ �� �������");
                }
                else if (index > 0)
                {
                    var subresult = line.Substring(0, index);
                    response.Data = subresult;
                    result.Add(response);
                }
                return result;
            }
            response.Data = line + "\n";
            result.Add(response);
        }
           ;
    }
    static string? GetQuery(TextReader reader)
    {
        string? result = null;
        while (true)
        {
            var line = reader.ReadLine();
            if (line == null) { return result; }
            if (line.Contains(endWord))
            {
                int index = line.IndexOf(endWord);
                if (index < 0)
                {
                    throw new InvalidOperationException("������ �� �������");
                }
                else if (index > 0)
                {
                    var subresult = line.Substring(0, index);
                    result += subresult;
                }
                return result;
            }
            result += line + "\n";
        }
          ;
    }
}
