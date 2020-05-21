using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceMonster : MonoBehaviour
{
    public GameObject monsterPrefab;
    private GameObject monster;
		private GameManagerBehavior gameManager;
		
		// Start is called before the first frame update
    void Start()
    {
			//получаем стартовое количество золота
			gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehavior>();
			//на старте располагаем башни на заданных точках 
      monster = (GameObject) 
      Instantiate(monsterPrefab, transform.position, Quaternion.identity); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
		
		//проверяем, можно ли улучшить башню
		private bool CanUpgradeMonster()
    {
      if (monster != null)
      {
        MonsterData monsterData = monster.GetComponent<MonsterData>();
        MonsterLevel nextLevel = monsterData.GetNextLevel();
        if (nextLevel != null)
        {
          return gameManager.Gold >= nextLevel.cost;
        }
      }
      return false;
    }
		
    void OnMouseUp()
    {
      if (CanUpgradeMonster())
      {
        //улучшаем башню
				monster.GetComponent<MonsterData>().IncreaseLevel();
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);
        //вычитаем золото
				gameManager.Gold -= monster.GetComponent<MonsterData>().CurrentLevel.cost;
      }
    }	
}
