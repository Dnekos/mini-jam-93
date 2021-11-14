using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BlackoutScript : MonoBehaviour
{
    Image img;
    bool falling;
    public int index;
    [SerializeField] float speedmultiplier = 11;
    static public bool FadeOut = false;
    
    void Awake()
    {
        FadeOut = true; // prevent clicking when active

        falling = false;
        img = gameObject.GetComponent<Image>();
        Color col = img.color;
        col.a = 0.0f;
        img.color = col;
		DontDestroyOnLoad(transform.parent.gameObject);
    }
    
    void Update()
    { 
        Color col = img.color;
        if(falling)
        {
			Debug.Log("alpha is " + col.a);
            if (col.a < 0.01f)
            {
                FadeOut = false;
                Destroy(transform.parent.gameObject);
            }
            col.a = Mathf.Lerp(col.a, 0, Time.deltaTime);
        }
        else
        { 
            if(col.a > 0.99f)
            {
                SceneManager.LoadScene(index);
                falling = true;
            }
            col.a = Mathf.Lerp(col.a, 1, speedmultiplier * Time.deltaTime);
        }
        img.color = col;
    }
}