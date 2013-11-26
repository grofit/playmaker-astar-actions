using HutongGames.PlayMaker.Pathfinding;
using Pathfinding;
using Pathfinding.Nodes;

namespace HutongGames.PlayMaker.Extensions
{
    public static class FsmObjectExtensions
    {
        public static object GetAnythingShallow(this FsmObject fsmObject)
        {
            if (fsmObject == null || fsmObject.Value == null)
            { return null; }

            if (fsmObject.Value is FsmNode)
            { return GetNode(fsmObject); }
            if (fsmObject.Value is FsmGridNode)
            { return GetGridNode(fsmObject); }
            if (fsmObject.Value is FsmGridNodes)
            { return GetGridNodes(fsmObject); }
            if (fsmObject.Value is FsmPath || fsmObject.Value is FsmABPath)
            { return GetPath(fsmObject); }
            if (fsmObject.Value is FsmPointGraph)
            { return GetPointGraph(fsmObject); }
            if (fsmObject.Value is FsmPointGraphs)
            { return GetPointGraphs(fsmObject); }

            return null;
        }

        public static AstarPath GetAstarPath(this FsmObject gameObject)
        { return (gameObject.Value as FsmAstarPath).Value; }

        public static NavGraph[] GetNavGraphs(this FsmObject gameObject)
        { return (gameObject.Value as FsmNavGraphs).Value; }

        public static NavGraph GetNavGraph(this FsmObject gameObject)
        { return (gameObject.Value as FsmNavGraph).Value; }

        public static PointGraph[] GetPointGraphs(this FsmObject gameObject)
        { return (gameObject.Value as FsmPointGraphs).Value; }

        public static PointGraph GetPointGraph(this FsmObject gameObject)
        { return (gameObject.Value as FsmPointGraph).Value; }

        public static Node GetNode(this FsmObject gameObject)
        { return (gameObject.Value as FsmNode).Value; }

        public static GridNode GetGridNode(this FsmObject gameObject)
        { return (gameObject.Value as FsmGridNode).Value; }

        public static GridNode[] GetGridNodes(this FsmObject gameObject)
        { return (gameObject.Value as FsmGridNodes).Value; }

        public static Path GetPath(this FsmObject gameObject)
        { return (gameObject.Value as FsmPath).Value; }
    }
}