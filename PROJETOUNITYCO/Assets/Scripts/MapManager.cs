using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] maps; // Assign your 3 maps in inspector

    void Start()
    {
        RandomizeMap();
    }

    void RandomizeMap()
    {
        if (maps.Length == 0) return;

        // Deactivate all maps first
        foreach (GameObject map in maps)
        {
            map.SetActive(false);
        }

        // Activate one randomly
        int index = Random.Range(0, maps.Length);
        maps[index].SetActive(true);

        Debug.Log("Map " + index + " is active.");
    }
}