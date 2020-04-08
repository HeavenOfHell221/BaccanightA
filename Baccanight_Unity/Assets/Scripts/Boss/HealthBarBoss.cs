using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBoss : MonoBehaviour
{
    [SerializeField]
    private Image redBar;
    [SerializeField]
    private Image yellowBar;


    // Start is called before the first frame update
    void Awake()
    {
        GetComponentInParent<HealthBoss>().OnHealthPctChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(float pct)
    {
        StopAllCoroutines();

        StartCoroutine(ChangeToPct(pct));
    }

    private IEnumerator ChangeToPct(float pct)
    {
        redBar.fillAmount = pct;

        yield return new WaitForSeconds(0.5f);
        yellowBar.fillAmount = pct;

        yield return null;
    }

    /*private void LateUpdate()
    {
        transform.LookAt(new Vector3(transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z));
        transform.Rotate(0, 180, 0);
    }*/
}
