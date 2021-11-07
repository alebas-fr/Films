using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCam : MonoBehaviour
{
    // Objet unity que l'on manipule
    public GameObject Earth;
    public GameObject MScene;
    public Canvas Intro;
    public Image imgIntro;
    public Text txtIntro;
    public AudioSource frein;
    public Canvas outro;
    public Image fondOutro;
    public Text auteurOutro;
    public Text AnneeOutro;
    public Text musiqueOutro;

    // Variables qui nous servent dans le script
    private bool freinIsPlayed;
    private bool playScene;
    private Vector3 direction;
    private float alpha;
    private float alphaOutro;
    private int toursTerre;
    private float valeurAnglePrec;

    // Start is called before the first frame update

    void Start()
    {
        // Initialiser les valeurs n�cessaires pour faire l'animation
        Application.targetFrameRate = 24;
        alpha = 1.0f; 
        alphaOutro = 0.0f;
        freinIsPlayed = false;
        toursTerre = 0;
        valeurAnglePrec = -3.0f;
        playScene = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > 5)
        { 
            // Lancer la transition entre l'intro et la sc�ne principale
            if (MScene.activeSelf==false)
            {
                MScene.SetActive(true);
            } 
            // Changer la valeur de transparence pour faire une transition
            alpha -= 0.01f;
            if (alpha < 0.0f)
            {
                alpha = 0f;
                playScene = true;
            }
            imgIntro.color = new Color(imgIntro.color.r, imgIntro.color.g, imgIntro.color.b, alpha);
            txtIntro.color = new Color(txtIntro.color.r, txtIntro.color.g, txtIntro.color.b, alpha);
        }
        if (playScene) // Jouer la sc�ne principale
        {
            if (transform.position.z < 7.5) // Rapprocher la cam�ra de la terre
            {
                direction = new Vector3(0f, 0f, Mathf.Exp(-0.005f * transform.position.z)); // Approcher de la Terre avec une exponentielle pour faire un effet de frein
                transform.Translate(direction * Time.deltaTime); // Faire avancer la cam�ra
                if (transform.position.z>5.0 && freinIsPlayed==false) // Jouer un son de crissement de pneu quand on arrive sur la terre pour plus de r�alisme
                {
                    freinIsPlayed = true;
                    frein.PlayOneShot(frein.clip);
                }
            }
            else // Une fois assez proche de la terre on arr�te de deplacer la cam�ra 
            {
                if ((Earth.transform.rotation.y>=0 && valeurAnglePrec<0) || (Earth.transform.rotation.y <= 0 && valeurAnglePrec > 0)) // Quand la terre � fait un tour sur elle m�me
                {
                    toursTerre += 1;
                    print(toursTerre);
                }
                valeurAnglePrec = Earth.transform.rotation.y;
                if (toursTerre == 2)
                {
                    playScene = false;
                }
            }
            //transform.LookAt(Earth.transform);
        }
        if (toursTerre == 2) // Quand la terre � fait deux tour sur elle m�me apr�s que la cam�ra ait arr�t� de bouger
        {
            // On ne d�sactive pas la sc�ne principale pour continuer � avoir la musique de fond
            // Lancer la transition vers l'outro de la m�me mani�re qu'avec l'intro c'est-�-dire avec une transparence
            alphaOutro += 0.01f;
            if (alphaOutro > 1.0f)
            {
                alphaOutro = 1.0f;
            }
            fondOutro.color = new Color(fondOutro.color.r, fondOutro.color.g, fondOutro.color.b, alphaOutro);
            auteurOutro.color = new Color(auteurOutro.color.r, auteurOutro.color.g, auteurOutro.color.b, alphaOutro);
            AnneeOutro.color = new Color(AnneeOutro.color.r, AnneeOutro.color.g, AnneeOutro.color.b, alphaOutro);
            musiqueOutro.color = new Color(musiqueOutro.color.r, musiqueOutro.color.g, musiqueOutro.color.b, alphaOutro);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            // Pour pouvoir quitter si jamais on build l'application
            Application.Quit();
        }
    }
}
