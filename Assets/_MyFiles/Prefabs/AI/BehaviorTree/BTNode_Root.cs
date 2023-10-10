using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNode_Root : BTNode
{
    BTNode child;

    public void AddChild(BTNode childToAdd)
    {
        child = childToAdd;
    }

    public List<BTNode> GetChildren()
    {
        return new List<BTNode> { child };
    }

    public void RemoveChild(List<BTNode> childToRemove)
    {
        
    }

    protected override BTNodeResult Execute()
    {
        return BTNodeResult.InProgress;
    }

    protected override BTNodeResult Update()
    {
        return child.UpdateNode();
    }
}
