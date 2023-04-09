using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MiniSystem : MonoBehaviour
{
    public TextMeshProUGUI puanYazi;

    public GameObject spawnSweetPrefab;
    public GameObject spawnBadPrefab;

    private int puan = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("+1puan"))
        {
            Destroy(other.gameObject);
            puan++;
            SpawnSweet();
            puanYazi.text = "Puan : " + puan + "/20";
            if(puan == 20)
            {
                WinScene();
            }
        }
        else if (other.CompareTag("-2puan"))
        {
            Destroy(other.gameObject);
            puan = puan - 2;
            if(puan < 0)
            {
                puan = 0;
            }
            SpawnBad();
            puanYazi.text = "Puan: " + puan + "/20";
        }
    }

    private void SpawnSweet()
    {
        Vector2 spawnPosition = new Vector2(Random.Range(-8.00f, 7.80f), Random.Range(-4.25f, 3.75f));
        Instantiate(spawnSweetPrefab, spawnPosition, Quaternion.identity);
    }
    private void SpawnBad()
    {
        Vector2 spawnPosition = new Vector2(Random.Range(-8.00f, 7.80f), Random.Range(-4.25f, 3.75f));
        Instantiate(spawnBadPrefab, spawnPosition, Quaternion.identity);
    }

    private void WinScene()
    {
        SceneManager.LoadScene("StateScene");
    }

}
