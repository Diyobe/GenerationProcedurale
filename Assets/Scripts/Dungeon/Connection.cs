using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour
{
    Node[] nodes = new Node[2];

    public void SetNodes(Node node1, Node node2)
    {
        nodes[0] = node1;
        nodes[1] = node2;
    }
}
