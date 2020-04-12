using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
    [SerializeField] private bool m_isOpen = false;
    [SerializeField] private Collider2D m_collider;
    private Vector3 m_endMarker;

    public void Start()
    {
        m_collider.enabled = !m_isOpen;
    }

    public void OpenFence()
    {
        //m_collider.enabled = false;
        m_endMarker = transform.position + Vector3.up * 3;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        transform.position = Vector3.Lerp(transform.position, m_endMarker, 0.05f);
        //if (transform.position != m_endMarker)
        if (Vector3.Distance(transform.position, m_endMarker) > 0.01f)
        {
            yield return new WaitForFixedUpdate(); // C'est ici qu'il faut le mettre, sinon tu rappelles la couroutine dans la même frame et ça devient
                               // un while infini... Infini car tu as fais un lerp "smooth", le transform.position va ce rapprocher
                               // de plus en plus lentement de m_endMarker mais (en théorie) tu l'atteindra jamais. x)
                               // Après, j'ai mis WaitForFixedUpdate pour le peut importe le nombre de FPS du jeu, le lerp aille à la 
                               // même vitesse. (yield return null; c'est bien mais à 60FPS tu appelles 60x Move()/s, à 300FPS tu l'appelles
                               // 300x/s... Quand c'est visible pour le joueur, ou quand c'est de la physique c'est fixedUpdate. :p

            StartCoroutine(Move()); // 2ème chose, mettre le yield avant le StartCoroutine, sinon c'est comme faire du récursif 
                                    // où la condition d'arrêt est après l'appel récursif x)
        }
        //else
        //yield return null;
        else
            transform.position = m_endMarker; // Pour avoir 0 de distance
    }

    public void CloseFence()
    {
        m_collider.enabled = true;
    }
}
