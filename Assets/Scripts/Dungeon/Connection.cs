using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour
{
    Node[] nodes = new Node[2];
    public Node[] Nodes
    {
        get { return nodes; }
        set { nodes = value; }
    }

    public Connection(Node node1, Node node2)
    {
        nodes[0] = node1;
        nodes[1] = node2;
    }
}
