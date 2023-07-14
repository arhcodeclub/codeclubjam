using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{

    public float timeseconds;
    public float timeminutes = 12;
    public TextMeshProUGUI clocktimer;
    public float animationspeed;
    float animationtimer = 1;
    bool anim = true;
    public ParticleSystem explosionparticles;

    bool exploded = false;

    public GameObject diedpanel;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("kaboom") == 1)
        {
            diedpanel.SetActive(true);
        }
    }

    void Update()
    {
        animationtimer -= Time.deltaTime;
        timeseconds += Time.deltaTime;
        if (timeseconds >= 60)
        {
            timeminutes++;
            timeseconds = 0;
        }

        if (animationtimer < 0)
        {
            animationtimer = animationspeed;
            anim = !anim;
        }

        if (anim)
        {
            clocktimer.text = timeminutes.ToString("00") + ":" + timeseconds.ToString("00");
        }else
        {
            clocktimer.text = timeminutes.ToString("00") + " " + timeseconds.ToString("00");
        }



        if (timeminutes >= 15)
        {
            if (!exploded)
            {
                exploded = true;
                StartCoroutine(Reactorexplode());
            }
        }
    }

    public void Die()
    {
        if (!exploded)
        {
            exploded = true;
            StartCoroutine(Reactorexplode());
        }
    }
    public GameObject victoryscreen;
    public void Win()
    {
        victoryscreen.SetActive(true);
    }

    public IEnumerator Reactorexplode()
    {
        explosionparticles.Play();
        yield return new WaitForSeconds(5);
        PlayerPrefs.SetInt("kaboom", 1);
        SceneManager.LoadScene("MainLevel");
    }
}
