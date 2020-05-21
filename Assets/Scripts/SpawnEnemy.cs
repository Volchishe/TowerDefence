using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
  public GameObject enemyPrefab1;
	public GameObject enemyPrefab2;
  public float spawnInterval = 2;
}

public class SpawnEnemy : MonoBehaviour
{
    public GameObject EnemyPrefab;
		public GameObject[] waypoints;
		public Wave[] waves;
    public int timeBetweenWaves = 5;
    private GameManagerBehavior gameManager;
    private float lastSpawnTime;
    private int enemiesSpawned = 0;
		private int stepRnd = 0;
		private int maxEnemies = 0;
		private int incdmg = 0;
		private int increw = 0;
		private int incspd = 0;
    private int inchp = 0;			
		// Start is called before the first frame update
    void Start()
    {
      lastSpawnTime = Time.time;
      gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehavior>(); 
    }

    // Update is called once per frame
    void Update()
    {
      int currentWave = gameManager.Wave;     			
			if (currentWave < waves.Length)
      {
				if (stepRnd < currentWave+1) 
				{
					stepRnd = currentWave+1;
					maxEnemies = UnityEngine.Random.Range((gameManager.Wave+1),(gameManager.Wave + 5));
					if (currentWave > 0)
					{
					  incdmg += 1;
				  	increw += 10;
				  	incspd += 1;
						inchp += 20;
					}

				}
				float timeInterval = Time.time - lastSpawnTime;
        float spawnInterval = waves[currentWave].spawnInterval;
        if (((enemiesSpawned == 0 && timeInterval > timeBetweenWaves) || timeInterval > spawnInterval) && enemiesSpawned < maxEnemies)
        { 
          lastSpawnTime = Time.time;
					int pref = Random.Range(0,10);
					GameObject newEnemy = (GameObject)
					Instantiate(pref < 5 ? waves[currentWave].enemyPrefab1 : waves[currentWave].enemyPrefab2);
          newEnemy.GetComponent<EnemyMove>().waypoints = waypoints; 
					newEnemy.GetComponent<EnemyMove>().dmg +=incdmg;
					newEnemy.GetComponent<EnemyMove>().reward +=increw;
					newEnemy.GetComponent<EnemyMove>().speed +=incspd;
					Transform healthBarTransform = newEnemy.transform.Find("HealthBar");
					HealthBar healthBar = healthBarTransform.gameObject.GetComponent<HealthBar>();
					healthBar.maxHealth += inchp;
					healthBar.currentHealth += inchp;
					enemiesSpawned++;
        }
        if (enemiesSpawned == maxEnemies && GameObject.FindGameObjectWithTag("Enemy") == null)
        {
          if (currentWave+1 == waves.Length)
					{
						gameManager.gameOver = true;
					}
					gameManager.Wave++;
          gameManager.Gold = Mathf.RoundToInt(gameManager.Gold * 1.2f);
          enemiesSpawned = 0;
          lastSpawnTime = Time.time;
        }
      }
      else
      {
        GameObject gameOverText = GameObject.FindGameObjectWithTag ("GameWon");
        gameOverText.GetComponent<Animator>().SetBool("gameOver", true);
      }  
    }
}
