using System.Collections.Generic;
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

            // Loop through the grid to find walkable nodes within search area
            for (int x = -Mathf.RoundToInt(playerController.searchArea); x <= Mathf.RoundToInt(playerController.searchArea); x++)
            {
                for (int y = -Mathf.RoundToInt(playerController.searchArea); y <= playerController.searchArea; y++)
                {
                    Vector3 offset = new Vector3(x, 0, y);
                    Vector3 worldPoint = playerPosition + offset;
                    Node node = grid.NodeFromWorldPoint(worldPoint);

                    // Check if the node is within the search area, walkable, and not the player's current node
                    if (Vector3.Distance(playerPosition, worldPoint) <= playerController.searchArea && node.walkable && node != playerNode)
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

        // Return a random walkable node from the list, or Vector3.zero if no valid nodes found
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