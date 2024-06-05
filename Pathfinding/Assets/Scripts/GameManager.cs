using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject token1, token2, token3, token4, token5;
    private int[,] GameMatrix; //0 not chosen, 1 player, 2 enemy
    private int[] startPos = new int[2];
    private int[] objectivePos = new int[2];

    private bool win = false;

    private bool pathAcabado = false;
    private Node nodoActual;
    private Node objetivo;

    //Guardamos los nodos por recorrer
    List<Node> openNodeList = new List<Node>();

    //Guardamos los nodos recorridos
    List<Node> closedNodeList = new List<Node>();



    private void Awake()
    {
        GameMatrix = new int[Calculator.length, Calculator.length];

        //Recorrer matriz
        for (int i = 0; i < Calculator.length; i++) //Horizontal
        {
            
            for (int j = 0; j < Calculator.length; j++) //Vertical
            {
                GameMatrix[i, j] = 0;
            }
        } 
        //Generamos variables random para crear la posicion inicial dentro del rango de la matriz
        var rand1 = Random.Range(0, Calculator.length);
        var rand2 = Random.Range(0, Calculator.length);

        startPos[0] = rand1;
        startPos[1] = rand2;

        //LLamamos a la funcion para colocar el punto inicial
        SetObjectivePoint(startPos);
        nodoActual = new Node(startPos, objetivo.posicion);

        //Almacenamos el nodo actual en las listas
        openNodeList.Add(nodoActual);
        closedNodeList.Add(nodoActual);

        GameMatrix[startPos[0], startPos[1]] = 1;
        GameMatrix[objectivePos[0], objectivePos[1]] = 2;

        InstantiateToken(token1, startPos);
        InstantiateToken(token2, objectivePos);
        ShowMatrix();
    }

    //Instanciamos el token en la posicion indicada
    private void InstantiateToken(GameObject token, int[] position)
    {
        Instantiate(token, Calculator.GetPositionFromMatrix(position),
            Quaternion.identity);
    }

    private void SetObjectivePoint(int[] startPos)
    {
        var rand1 = Random.Range(0, Calculator.length);
        var rand2 = Random.Range(0, Calculator.length);
        if (rand1 != startPos[0] || rand2 != startPos[1])
        {
            objectivePos[0] = rand1;
            objectivePos[1] = rand2;
            objetivo = new Node(objectivePos);
        }
    }

    private void ShowMatrix() //fa un debug log de la matriu
    {
        string matrix = "";
        for (int i = 0; i < Calculator.length; i++)
        {
            for (int j = 0; j < Calculator.length; j++)
            {
                matrix += GameMatrix[i, j] + " ";
            }
            matrix += "\n";
        }
        Debug.Log(matrix);
    }

    //EL VOSTRE EXERCICI COMENÇA AQUI
    private void Update()
    {
        if (!win)
        {
            CheckAndAddNeighbor(nodoActual.posicion[0], nodoActual.posicion[1] - 1); //Izquierda
            CheckAndAddNeighbor(nodoActual.posicion[0], nodoActual.posicion[1] + 1); // Derecha
            CheckAndAddNeighbor(nodoActual.posicion[0] - 1, nodoActual.posicion[1]); // Arriba
            CheckAndAddNeighbor(nodoActual.posicion[0] + 1, nodoActual.posicion[1]); // Abajo

            nodoActual = GetBestNode(openNodeList);
            closedNodeList.Add(nodoActual);
        }


        if (GameMatrix[nodoActual.posicion[0], nodoActual.posicion[1]] == 2)
        {

            if (!pathAcabado)
            {
                List<Node> currentPath = new List<Node> { nodoActual };
                Node currentNode = nodoActual;

                foreach (Node node in closedNodeList)
                {
                    if (currentNode.costeTotal > node.costeTotal)
                    {
                        currentPath.Add(node);
                        currentNode = node;
                    }
                }

                pathAcabado = true;
            }
            win = true;

        }
    }


    private void CheckAndAddNeighbor(int x, int y)
    {
        if (IsValidPosition(x, y))
        {
            int[] position = { x, y };
            openNodeList.Add(new Node(position, objetivo.posicion));
            InstantiateToken(token3, position);
        }
    }


    private bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < Calculator.length && y >= 0 && y < Calculator.length;
    }

    //Funcion que nos ayudará a encontrar el mejor nodo segun coste y heurística
    private Node GetBestNode(List<Node> nodeList)
    {
        Node bestNode = nodeList[0];
        foreach (Node node in nodeList)
        {
            if (node.costeTotal < bestNode.costeTotal)
            {
                bestNode = node;
            }
        }
        nodeList.Remove(bestNode);
        
        //Devolvemos el nodo con el coste mas bajo
        return bestNode;
    }

}
