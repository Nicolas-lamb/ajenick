using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject prefab; // O prefab que ser� instanciado

    public Tilemap wallTilemap; // Refer�ncia ao Tilemap que cont�m as paredes
    public float checkRadius = 0.5f; // Raio da esfera de verifica��o
    public float minDistanceBetweenPrefabs = 1.0f; // Dist�ncia m�nima entre prefabs
    public Vector3 spawnAreaMin; // Coordenadas m�nimas da �rea de spawn
    public Vector3 spawnAreaMax; // Coordenadas m�ximas da �rea de spawn

    private List<Vector3> spawnedPositions = new List<Vector3>();

    

    void Start()
    {
       

        Player_input player_Input = Player.GetComponent<Player_input>();
        int spawnCount = Player_input.contPerguntas; // N�mero de prefabs a serem instanciados

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            if (spawnPosition != Vector3.zero)
            {
                Instantiate(prefab, spawnPosition, Quaternion.identity);
                spawnedPositions.Add(spawnPosition);
            }
        }
    }

    void Update()
    {
    }

    public void GeneratePrefab()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        if (spawnPosition != Vector3.zero)
        {
            Instantiate(prefab, spawnPosition, Quaternion.identity);
            spawnedPositions.Add(spawnPosition);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {

        const int maxAttempts = 20;
        for (int attempts = 0; attempts < maxAttempts; attempts++)
        {
            float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector3 randomPosition = new Vector3(randomX, randomY, 0);

            if (!IsPositionOnWall(randomPosition) && !Physics2D.OverlapCircle(randomPosition, checkRadius) && IsPositionFarEnough(randomPosition))
            {
                return randomPosition;
            }
        }
        return Vector3.zero; // Falhou em encontrar uma posi��o v�lida
    }

    bool IsPositionOnWall(Vector3 position)
    {
        Vector3Int cellPosition = wallTilemap.WorldToCell(position);
        TileBase tile = wallTilemap.GetTile(cellPosition);
        return tile != null && wallTilemap.HasTile(cellPosition);
    }

    bool IsPositionFarEnough(Vector3 position)
    {
        foreach (Vector3 spawnedPosition in spawnedPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < minDistanceBetweenPrefabs)
            {
                return false;
            }
        }
        return true;
    }
}
