using System.Net;
using System.Text.Json;
using AppCenterService;
using Flurl;

namespace AppCenterConsole;

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
    public List<FullBranch> GetBranches()
    {
        var url = new Url(_baseApi).AppendPathSegment(Author).AppendPathSegment(AppName)
            .AppendPathSegment("branches");
        var json = GetRequest(url);
        var result = JsonSerializer.Deserialize<List<FullBranch>>(json);
        return result;
    }

    public bool BuildBranch(string branch)
    {
        var branches = GetBranches();
        if (!branches.Select(q=>q.Branch.Name).Contains(branch))
        {
            throw new Exception("incorrect branch name");
        }

        var lastCommit = branches.First(q => q.Branch.Name == branch).Branch.Commit.Sha;
        var url = new Url().AppendPathSegment(Author).AppendPathSegment(AppName).AppendPathSegment("branches")
            .AppendPathSegment(branch).AppendPathSegment("builds");
        var body = new BuildParameters(lastCommit, false);
        PostRequest(url, body);
        return true;
    }
    private string GetRequest(string url)
    {
        var request = WebRequest.Create(url) as HttpWebRequest;
        request.Accept = "application/json";
        request.Method = "GET";
        request.Headers.Add($"X-API-Token: {Token}");
        var response = (HttpWebResponse)request.GetResponse();
        string result;
        using (var reader = new StreamReader(response.GetResponseStream()))
        {
            result = reader.ReadToEnd();
        }
        response.Close();
        return result;
    }

    private string PostRequest(string apiRequest, object body)
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

        var response = (HttpWebResponse)request.GetResponse();
        string result;
        using (var reader = new StreamReader(response.GetResponseStream()))
        {
            result = reader.ReadToEnd();
        }

        response.Close();
        return result;
    }
}