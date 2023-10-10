using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BTNodeGraph : GraphView
{
    public new class UxmlFactory : UxmlFactory<BTNodeGraph, UxmlTraits> { }

    public BTNodeGraph()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviorEditor/BehaviorTreeEditor.uss");
        styleSheets.Add(styleSheet);
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);
        var allNodeTypes = TypeCache.GetTypesDerivedFrom<BTNode>();
        foreach (System.Type type in allNodeTypes)
        {
            if (type.IsAbstract) continue;
            if (type == typeof(BTNode_Root)) continue;
            evt.menu.AppendAction(type.Name, (arg) => CreateGraphNode(type));
        }
    }


    private void CreateNode(Type nodeType)
    {
        BTNode newNode = tree.CreateNode(nodeType);
        CreateGraphNode(newNode);
    }

    private void CreateGraphNode(BTNode newNode)
    {
        BTGraphNode newGraphNode = new BTGraphNode(newNode);
        AddElement(newGraphNode);
    }

    internal void PopulateTree(BehaviorTree selectedAsTree)
    {
        tree = selectedAsTree;
        tree.PreConstruct();
        foreach(BTNode node in tree.GetNodes())
        {
            CreateGraphNode(node);
        }
    }
}
