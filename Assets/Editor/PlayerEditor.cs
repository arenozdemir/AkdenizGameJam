using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerController))]
public class PlayerEditor : Editor
{
    private void OnSceneGUI()
    {
        PlayerController player = (PlayerController)target;
        
        if (player.noiseRadius > 0)
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(player.transform.position + new Vector3(0, 1, 0), Vector3.up, player.noiseRadius);
            Handles.Label(player.transform.position + Vector3.up * player.noiseRadius, "Noise Radius");
        }

        if (player.searchArea > 0)
        {
            Handles.color = Color.blue;
            Handles.DrawWireDisc(player.transform.position + new Vector3(0, 1, 0), Vector3.up, player.searchArea);
            Handles.Label(player.transform.position + Vector3.up * player.searchArea, "Search Area");
        }
    }
}