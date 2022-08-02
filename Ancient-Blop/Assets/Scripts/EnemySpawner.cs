using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] MonsterTypes;

    private GameObject spawnedMonster;

    [SerializeField]
    private Transform leftSpawn, rightSpawn;

    private int randomSide;
    private int randomIndex;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnMonsters());
    }

    IEnumerator SpawnMonsters()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1, 5));

            randomIndex = Random.Range(0, MonsterTypes.Length);
            randomSide = Random.Range(0, 2);

            spawnedMonster = Instantiate(MonsterTypes[randomIndex]);

            if (randomSide == 0)
            {
                spawnedMonster.transform.position = leftSpawn.position;
                spawnedMonster.GetComponent<Blop>().speed = Random.Range(4, 10);
            }
            else
            {
                spawnedMonster.transform.position = rightSpawn.position;
                spawnedMonster.GetComponent<Blop>().speed = -Random.Range(4, 8);
                spawnedMonster.transform.localScale = new Vector3(-1f, 1f, 1f);
                // another way to flip the monster not using sprite editor
            }
        }
    }

}
