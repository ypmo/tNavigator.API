using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace tNav.FakeConsole.Tests;

public class LogParserTests
{
    static string TestData =
  "2025-03-31 23:40:37.050414\n" +
  "***INPUT***\n" +
  "create_project(path = \"SNP/API_BuildND.snp\", case = \"model_designer\", type = \"md\")\n" +
  "***END***\n" +
  "OK\n" +
  "0\n" +
  "***INPUT***\n" +
  "run_py_code(code = \"save_project ()\", id = \"0\")\n" +
  "***END***\n" +
  "OK\n" +
  "04000000000000004e6f6e65***INPUT***\n" +
  "close_project(id = \"0\")\n" +
  "***END***\n" +
  "OK\n";

    static List<Seans> ActualData = [
        new Seans(){
            Query="create_project(path = \"SNP/API_BuildND.snp\", case = \"model_designer\", type = \"md\")\n",
            Responses=[
                new Response{Data="OK\n"},
                new Response{Data="0\n"},
                ]},
                new Seans(){
            Query="run_py_code(code = \"save_project ()\", id = \"0\")\n",
            Responses=[
                new Response{Data="OK\n"},
                new Response{Data="04000000000000004e6f6e65"},
                ]},
                new Seans(){
            Query="close_project(id = \"0\")\n",
            Responses=[
                new Response{Data="OK\n"}
                ]},
        ];

    [Fact]
    public void ParserCanRead()
    {
        var parser = new LogParser();
        var log = parser.Parse(TestData);
        Assert.NotNull(log);
        Assert.True(log.Count() == 3);
        var test = JsonConvert.SerializeObject(log);
        var actual = JsonConvert.SerializeObject(ActualData);
        Assert.Equal(test, actual);
    }
}
