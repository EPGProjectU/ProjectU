using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Sprite emptyHeart;
    public Sprite fullHeart;
    private Image[] hearts;
    public PlayerController pc;
    public Text gameOver;

    // Start is called before the first frame update
    void Start()
    {
        int hp = pc.health;
        string imgName = "Image0";
        hearts = new Image[hp];
        GameObject img = new GameObject(imgName);
        RectTransform rectTransform = img.AddComponent<RectTransform>();
        rectTransform.localScale = new Vector3(1, 1, 1);
        rectTransform.sizeDelta = new Vector2(30, 30);
        Image imageHeart = img.AddComponent<Image>();
        UnityEngine.Debug.Log("test");
        Texture2D texture2D = Resources.Load<Texture2D>("Sprites/Health_graphic/heart_full");
        Sprite spriteheart = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        for (int i = 0; i < hearts.Length; i++)
        {
            imgName = "Image" + (i + 1).ToString();
            img = new GameObject(imgName);
            img.transform.SetParent(GameObject.Find("Container").transform);
            img.AddComponent<RectTransform>();
            img.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            img.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
            imageHeart = img.AddComponent<Image>();
            imageHeart.sprite = spriteheart;
            hearts[i] = GameObject.Find(imgName).GetComponent<Image>();
        }
        Destroy(GameObject.Find("Image0"));

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i >= pc.health)
            {
                hearts[i].sprite = emptyHeart;
            }
            else
            {
                hearts[i].sprite = fullHeart;
            }
        }
        if (pc.health == 0)
        {
            GameOver();
            pc.health = -1;
        }
    }

    void GameOver()
    {
        gameOver.enabled = true;
    }
}
