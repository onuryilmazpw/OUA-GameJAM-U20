using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MiniSystem : MonoBehaviour
{
    TextMeshProUGUI puanYazi;

    public List<GameObject> spawnSweetPrefab;
    public GameObject spawnBadPrefab;
    public int targetPuan;
    public AudioClip artiPuan;
    public AudioClip eksiPuan;



    private int puan = 0;
    private int sweetIndex = 0;
    private AudioSource aSource;

    private void Start()
    {
        puanYazi = GameObject.Find("Puan").GetComponent<TextMeshProUGUI>();
        aSource = GetComponent<AudioSource>();
        AudioManager.instance.StopSound("Education");
        aSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("+1puan"))
        {
            Destroy(other.gameObject);
            puan++;
            aSource.PlayOneShot(artiPuan);
            SpawnSweet();
            puanYazi.text = "Puan : " + puan + "/" + targetPuan.ToString();
            if(puan >= targetPuan)
            {
                WinScene();
            }
        }
        else if (other.CompareTag("-2puan"))
        {
            Destroy(other.gameObject);
            puan -= 2;
            aSource.PlayOneShot(eksiPuan);
            if (puan < 0)
            {
                puan = 0;
            }
            SpawnBad();
            puanYazi.text = "Puan: " + puan + "/" + targetPuan.ToString();
        }
    }

    private void SpawnSweet()
    {
        Vector2 spawnPosition = new Vector2(Random.Range(-8.00f, 7.80f), Random.Range(-4.25f, 3.75f));
        sweetIndex = Random.Range(0, spawnSweetPrefab.Count);
        Instantiate(spawnSweetPrefab[sweetIndex], spawnPosition, Quaternion.identity);
    }
    private void SpawnBad()
    {
        Vector2 spawnPosition = new Vector2(Random.Range(-8.00f, 7.80f), Random.Range(-4.25f, 3.75f));
        Instantiate(spawnBadPrefab, spawnPosition, Quaternion.identity);
    }

    private void WinScene()
    {
        GameManager.SetInt(SceneManager.GetActiveScene().name, GameManager.GetInt(SceneManager.GetActiveScene().name, 1) + 1);
        SceneManager.LoadScene("WarScene");
    }

}
