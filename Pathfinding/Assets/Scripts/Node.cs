using System;
using System.Collections;
using UnityEngine;


public class Node : MonoBehaviour 
{
    //Posicion del Nodo
    public int[] posicion { get; set; }

    //Coste y Heurisitca
    public float heuristica { get; set; }
    public float coste = Calculator.distance;

    //Coste del nodo segun coste+heurisitca+costetotal del parent
    public float costeTotal { get; set; }

    //Nodo "anterior"
    public Node parent { get; set; }

    public Node(int[] _posicion, int[] objetivo = null, Node parent = null)
    {
        posicion = _posicion;
        if (objetivo != null)
        {
            heuristica = Vector3.Distance(new Vector3(_posicion[0], _posicion[1], 0), new Vector3(objetivo[0], objetivo[1], 0));
        }
        else heuristica = 0;


        if (parent != null)
        {
            this.parent = parent;
            costeTotal = heuristica + coste + (parent.costeTotal - parent.heuristica);
        }
        else
        {
            this.parent = null;
            costeTotal = heuristica + coste * 2;
        }
    }
}