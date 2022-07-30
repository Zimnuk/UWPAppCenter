using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AppCenterService;
using Flurl;

namespace AppCenterConsole
{
    
public class AppCenterApi
{
    private string _baseApi = "https://api.appcenter.ms/v0.1/apps";
    private readonly string Author;
    private readonly string AppName;
    private readonly string Token;
    public AppCenterApi()
    {
        Author = Environment.GetEnvironmentVariable("Author");
        AppName = Environment.GetEnvironmentVariable("AppName");
        Token = Environment.GetEnvironmentVariable("Token");
    }
    public async Task<List<FullBranch>> GetBranches()
    {
        var url = new Url(_baseApi).AppendPathSegment(Author).AppendPathSegment(AppName)
            .AppendPathSegment("branches");
        var json = await GetRequest(url);
        var result = JsonSerializer.Deserialize<List<FullBranch>>(json);
        return result;
    }

    public async Task BuildAllBranches(IEnumerable<BranchInfo> branches)
    {
        var tasks = branches.Select(BuildConcreteBranch).ToList();

        Task.WhenAll(tasks);
    }
    

    public async Task BuildConcreteBranch(BranchInfo branch)
    {
        var lastCommit = branch.Commit.Sha;
        var url = new Url(_baseApi).AppendPathSegment(Author).AppendPathSegment(AppName).AppendPathSegment("branches")
            .AppendPathSegment(branch.Name).AppendPathSegment("builds");
        var body = new BuildParameters(lastCommit, true);
        try
        {
            Console.WriteLine($"{branch.Name} has been sent for building");
            var branchBuild = await PostRequest(url, body);
            var build = JsonSerializer.Deserialize<Build>(branchBuild);
            var time = DateTime.Parse(build.StartTime) - DateTime.Parse(build.FinishTime);
            Console.WriteLine($"{branch.Name} build completed in {time.Seconds}");
            
        }
        catch (Exception e)
        {
            Console.WriteLine($"Branch {branch.Name} didn't build");
        }
    }
    private async Task<string> GetRequest(string url)
    {
        var request = WebRequest.Create(url) as HttpWebRequest;
        request.Accept = "application/json";
        request.Method = "GET";
        request.Headers.Add($"X-API-Token: {Token}");
        var response = request.GetResponse();
        string result;
        using (var reader = new StreamReader(response.GetResponseStream()))
        {
            result = reader.ReadToEnd();
        }
        response.Close();
        return result;
    }

    private async Task<string> PostRequest(string apiRequest, object body)
    {
        var request = WebRequest.Create(apiRequest) as HttpWebRequest;
        request.ContentType = "application/json";
        request.Timeout = 600000;
        request.Method = "Post";
        request.Headers.Add($"X-API-Token: {Token}");
        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            var bodyJson = JsonSerializer.Serialize(body);
            streamWriter.Write(bodyJson);
        }
        var response = await request.GetResponseAsync();
        string result;
        using (var reader = new StreamReader(response.GetResponseStream()))
        {
            result = reader.ReadToEnd();
        }

        response.Close();
        return result;
    }
}    
}
