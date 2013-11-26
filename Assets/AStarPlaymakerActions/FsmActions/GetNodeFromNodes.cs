using HutongGames.PlayMaker.Pathfinding;
using UnityEngine;

namespace HutongGames.PlayMaker.Pathfinding
{
	[ActionCategory("A Star")]
	[Tooltip("Currently supports both FsmNodes and FsmGridNodes. More to come. Gets a node at a certain index from an array/list of nodes")]
	public class GetNodeFromNodes : FsmStateAction
	{
		[ActionSection("Input")]
		[RequiredField]
		[ObjectType(typeof(FsmNodes))]
		[Tooltip("The nodes ")	]
		public FsmObject nodes;
		
		[RequiredField]
		[Tooltip("Index of the node")	]
		public FsmInt index ;
		
		[ActionSection("Output")]
		[ObjectType(typeof(FsmNode))]
		[Tooltip("Any type of node")	]
		public FsmObject node;

		public override void Reset()
		{
			nodes = new FsmObject();
			node = new FsmObject();
            index = 0;
		}
	  
		public override void OnEnter()  
	  	{
			var underlyingFsmNodes = nodes.Value as FsmNodes;
			if(underlyingFsmNodes == null || (underlyingFsmNodes.Value == null) || !node.UseVariable) 
			{
				Debug.Log("No Input");
				Finish(); 
				return;
			} 
			
			if (underlyingFsmNodes.Value.Count <= index.Value)
			{
				Debug.Log("index is higher than the number of nodes in the nodes list/variable");
				Finish();
				return;
			}

            var currentNode = (nodes.Value as FsmNodes).Value[index.Value];
            node.Value = new FsmNode { Value = currentNode };
			Finish();			
		}  
   	}
}