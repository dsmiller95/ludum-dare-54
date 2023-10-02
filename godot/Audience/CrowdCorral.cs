using System.Collections.Generic;
using Godot;
using LudumDare54.Settings;

namespace LudumDare54.Audience;

public partial class CrowdCorral: Node2D
{
    private Dictionary<int, Dictionary<int, List<CrowdActor>>> crowdHash;

    private const int _segmentSize = 100;

    public override void _Ready()
    {
        ProcessPriority = 10;
    }

    public override void _PhysicsProcess(double delta)
    {
        var shouldUseCalculation = SettingsSingleton.Settings?.UseNeighborCalculation ?? true;
        if (!shouldUseCalculation)
        {
            crowdHash = null;
            return;
        }
        crowdHash = new();
        var children = GetChildren();

        foreach (var child in children)
        {
            if (child is not CrowdActor crowdChild)
            {
                continue;
            }

            var hashedPosition = crowdChild.GlobalPosition / _segmentSize;

            if (!crowdHash.ContainsKey((int)hashedPosition.X))
            {
                crowdHash[(int)hashedPosition.X] = new();
            }

            if (!crowdHash[(int)hashedPosition.X].ContainsKey((int)hashedPosition.Y))
            {
                crowdHash[(int)hashedPosition.X][(int)hashedPosition.Y] = new();
            }
            
            crowdHash[(int)hashedPosition.X][(int)hashedPosition.Y].Add(crowdChild);
        }
    }

    public List<CrowdActor> GetNeighbors(CrowdActor crowdActor)
    {
        if (crowdHash == null) return null;
        var hashedPosition = crowdActor.GlobalPosition / _segmentSize;
        var neighbors = new List<CrowdActor>();

        for (var x = (int)hashedPosition.X - 1; x <= (int)hashedPosition.X + 1; x++)
        {
            if (!crowdHash.ContainsKey(x))
            {
                continue;
            }

            for (var y = (int)hashedPosition.Y - 1; y <= (int)hashedPosition.Y + 1; y++)
            {
                if (!crowdHash[x].ContainsKey(y))
                {
                    continue;
                }
                
                neighbors.AddRange(crowdHash[x][y]);
            }
        }

        return neighbors;
    }
}