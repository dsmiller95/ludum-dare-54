using System.Collections.Generic;
using System.Runtime.InteropServices;
using DotnetLibrary.Audience;
using Godot;
using Godot.Collections;
using LudumDare54.Settings;

namespace LudumDare54.Audience;

public partial class CrowdCorral: Node2D
{
    private System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, List<CrowdActor>>> crowdHash;

    private const int _segmentSize = 100;

    public override void _Ready()
    {
        ProcessPriority = 10;
    }

    public override void _PhysicsProcess(double delta)
    {
        var children = GetChildren();
        var shouldUseCalculation = SettingsSingleton.Settings?.UseNeighborCalculation ?? true;
        if (shouldUseCalculation)
        {
            UpdateNeighborCache(children);
        }
        else 
        {
            crowdHash = null;
        }

        var workingNeighborList = new List<CrowdActor>();
        
        foreach (var child in children)
        {
            if (child is not CrowdActor crowdChild)
            {
                continue;
            }
            
            if (shouldUseCalculation)
            {
                workingNeighborList.Clear();
                GetNeighbors(crowdChild, workingNeighborList);
            }
            crowdChild.OwnedPhysicsProcess(delta, CollectionsMarshal.AsSpan(workingNeighborList));
        }

    }

    private void UpdateNeighborCache(Array<Node> children)
    {
        if (crowdHash == null) crowdHash = new();

        foreach (var xMap in crowdHash.Values)
        {
            foreach (var yList in xMap.Values)
            {
                yList.Clear();
            }
        }


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

    public List<CrowdActor> GetNeighbors(CrowdActor crowdActor, List<CrowdActor> neighbors = null)
    {
        if (crowdHash == null) return null;
        var hashedPosition = crowdActor.GlobalPosition / _segmentSize;
        neighbors ??= new List<CrowdActor>();
        neighbors.Clear();

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