using UnityEngine;
using UnityEngine.UI;

public class PlayrAnim : MonoBehaviour
{
    public Image player;
    public Sprite img1, img2;

    float timer = 0;

    private void Update()
    {
        if (timer <= 0)
        {
            timer = 0.28f;
            if (player.sprite == img1)
            {
                player.sprite = img2;
            }
            else
            {
                player.sprite = img1;
            }
        }
        else
            timer -= Time.deltaTime;
    }
}
