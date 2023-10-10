
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
public class BTGraphNode : Node
{
    public BTNode Node { get; private set; }
    public BTGraphNode(BTNode node)
    {
        Node = node;
        title = node.GetType().Name;
    }
}
