using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection
{
    Node[] nodes = new Node[2];

    bool isLocked;
    public bool IsLocked
    {
        get { return isLocked; }
        set { isLocked = value; }
    }

    public Node[] Nodes
    {
        get { return nodes; }
        set { nodes = value; }
    }

    public Connection(Node node1, Node node2, bool isLocked)
    {
        nodes[0] = node1;
        nodes[1] = node2;
        this.isLocked = isLocked;
    }
}
