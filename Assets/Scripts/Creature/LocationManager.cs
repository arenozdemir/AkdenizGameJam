using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
public class LocationManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Grid grid;
    public Vector3 GetNodeToPatrol()
    {
        List<Vector3> walkableNodes = new List<Vector3>();
        try
        {
            Vector3 playerPosition = playerController.transform.position;
            Node playerNode = grid.NodeFromWorldPoint(playerPosition);

            float searchArea = playerController.searchArea;

            for (int x = -Mathf.RoundToInt(searchArea); x <= Mathf.RoundToInt(searchArea); x++)
            {
                for (int y = -Mathf.RoundToInt(searchArea); y <= searchArea; y++)
                {
                    Vector3 offset = new Vector3(x, 0, y);
                    Vector3 worldPoint = playerPosition + offset;
                    Node node = grid.NodeFromWorldPoint(worldPoint);

                    if (Vector3.Distance(playerPosition, worldPoint) <= searchArea && node.walkable && node != playerNode)
                    {
                        walkableNodes.Add(node.worldPosition);
                    }
                }
            }
        }
        catch
        {
            throw new System.Exception("PlayerController or Grid not found");
        }

        if (walkableNodes.Count > 0)
        {
            return walkableNodes[Random.Range(0, walkableNodes.Count)];
        }
        else
        {
            return Vector3.zero;
        }
    }
    public Vector3 GetPlayerPosition()
    {
        return playerController.transform.position;
    }

    public Vector3 GetSearchNode()
    {
        List<Vector3> walkableNodes = new List<Vector3>();
        try
        {
            Vector3 playerPosition = playerController.transform.position;
            Node playerNode = grid.NodeFromWorldPoint(playerPosition);

            float searchArea = playerController.noiseRadius;

            for (int x = -Mathf.RoundToInt(searchArea); x <= Mathf.RoundToInt(searchArea); x++)
            {
                for (int y = -Mathf.RoundToInt(searchArea); y <= searchArea; y++)
                {
                    Vector3 offset = new Vector3(x, 0, y);
                    Vector3 worldPoint = playerPosition + offset;
                    Node node = grid.NodeFromWorldPoint(worldPoint);

                    if (Vector3.Distance(playerPosition, worldPoint) <= searchArea && node.walkable && node != playerNode)
                    {
                        walkableNodes.Add(node.worldPosition);
                    }
                }
            }
        }
        catch
        {
            throw new System.Exception("PlayerController or Grid not found");
        }

        if (walkableNodes.Count > 0)
        {
            return walkableNodes[Random.Range(0, walkableNodes.Count)];
        }
        else
        {
            return Vector3.zero;
        }
    }
}