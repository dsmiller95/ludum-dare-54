using System.Collections.Generic;
using System.Runtime.InteropServices;
using DotnetLibrary.Audience;
using DotnetLibrary.Audience.Factors;
using Godot;
using Godot.Collections;
using LudumDare54.Settings;

namespace LudumDare54.Audience;

public partial class CrowdCorral: Node2D
{
    private System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, List<CrowdActor>>> crowdHash;

    private const int _segmentSize = 100;

    private List<CrowdActor> AllActors = new();
    
    public override void _Ready()
    {
        ProcessPriority = 10;
    }

    public void UpdateInternalActorList()
    {
        AllActors.Clear();
        var children = GetChildren();
        foreach (var child in children)
        {
            if (child is not CrowdActor crowdChild)
            {
                continue;
            }
            AllActors.Add(crowdChild);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        var children = AllActors;
        var shouldUseCalculation = SettingsSingleton.Settings?.UseNeighborCalculation ?? true;
        if (shouldUseCalculation)
        {
            UpdateNeighborCache(children);
        }
        else 
        {
            crowdHash = null;
        }

        var workingNeighborList = new List<AiNeighbor?>();
        
        
        
        foreach (var child in children)
        {
            if (child is null)
            {
                GD.PrintErr("null child in crowd corral internal tracker");
                continue;
            }
            
            if (shouldUseCalculation)
            {
                workingNeighborList.Clear();
                GetNeighbors(child, workingNeighborList);
            }
            child.OwnedPhysicsProcess(delta, CollectionsMarshal.AsSpan(workingNeighborList));
        }

    }

    private void UpdateNeighborCache(List<CrowdActor> children)
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
            if (child is null)
            {
                GD.PrintErr("null child in crowd corral internal tracker");
                continue;
            }

            var hashedPosition = child.GlobalPosition / _segmentSize;

            if (!crowdHash.ContainsKey((int)hashedPosition.X))
            {
                crowdHash[(int)hashedPosition.X] = new();
            }

            if (!crowdHash[(int)hashedPosition.X].ContainsKey((int)hashedPosition.Y))
            {
                crowdHash[(int)hashedPosition.X][(int)hashedPosition.Y] = new();
            }

            crowdHash[(int)hashedPosition.X][(int)hashedPosition.Y].Add(child);
        }
    }

    private void GetNeighbors(CrowdActor crowdActor, List<AiNeighbor?> neighbors)
    {
        if (crowdHash == null) return;
        var hashedPosition = crowdActor.GlobalPosition / _segmentSize;
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

                var itemsInHash = crowdHash[x][y];
                for (int i = 0; i < itemsInHash.Count; i++)
                {
                    var neighbor = itemsInHash[i];
                    var relativePosition = neighbor.GlobalPosition - crowdActor.GlobalPosition;
                    var actor = neighbor.CrowdActorImpl;
                    if(actor is null) continue;
                    neighbors.Add(new AiNeighbor
                    {
                        Position = relativePosition,
                        Factors = actor.factors
                    });
                }
            }
        }
    }
}