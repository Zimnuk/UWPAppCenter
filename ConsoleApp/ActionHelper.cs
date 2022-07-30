

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppCenterService;

namespace AppCenterConsole
{
    
public class ActionHelper
{
    private readonly AppCenterApi _api;
    private Dictionary<int, BranchInfo> _currentBranches;
    public ActionHelper()
    {
        _api = new AppCenterApi();
    }
    public async Task GetBranches()
    {
        _currentBranches = new Dictionary<int, BranchInfo>();
        Console.WriteLine("Uploading list of branches...");
        var branches = await _api.GetBranches();
        Console.WriteLine("Success");
        Console.WriteLine("Choose an action");
        Console.WriteLine("-1. Update list");
        Console.WriteLine($"0. Build all branches");
        var i = 1;
        foreach (var branch in branches)
        {
            _currentBranches.Add(i, branch.Info);
            Console.WriteLine($"{i++}.  build only {branch.Info.Name} branch");
        }
    }

    public async Task ActionChoose(string action)
    {
        switch (action)
        {
            case "-1":
                GetBranches();
                return;
            case "0":
                BuildAllBranches();
                return;
            default:
                BuildConcreteBranch(action);
                return;
        }
    }

    public async Task BuildConcreteBranch(string action)
    {
        var result = 0;
        var correctNumber = int.TryParse(action, out result);
        if (correctNumber && _currentBranches.ContainsKey(result))
        {
            _api.BuildConcreteBranch(_currentBranches[result]);
        }

    }
    private async Task BuildAllBranches()
    {
        Console.WriteLine("Building of all branches");
        await _api.BuildAllBranches(_currentBranches.Select(q=>q.Value).ToList());
        
    }
}
}
