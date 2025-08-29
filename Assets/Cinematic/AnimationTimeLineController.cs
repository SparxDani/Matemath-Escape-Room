using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class AnimationTimeLineController : MonoBehaviour
{
    public GameObject camera1;
    public PlayableDirector timeline1;

    public GameObject camera2;
    public PlayableDirector timeline2_1;
    public PlayableDirector timeline2_2;
    public PlayableDirector timeline2_3;

    public GameObject camera3;

    void Start()
    {
        StartCoroutine(ControlSequence());
    }

    IEnumerator ControlSequence()
    {
        while (true)
        {
            // Solo cámara 1 activa
            camera1.SetActive(true);
            camera2.SetActive(false);
            camera3.SetActive(false);

            timeline1.gameObject.SetActive(true);
            timeline2_1.gameObject.SetActive(false);
            timeline2_2.gameObject.SetActive(false);
            timeline2_3.gameObject.SetActive(false);

            // Inicia timeline1
            timeline1.Play();
            yield return new WaitForSeconds((float)timeline1.duration);

            // Cámara 2 y primer timeline
            camera1.SetActive(false);
            camera2.SetActive(true);

            timeline1.gameObject.SetActive(false);
            timeline2_1.gameObject.SetActive(true);
            timeline2_1.Play();
            yield return new WaitForSeconds((float)timeline2_1.duration);

            // Segundo timeline de cámara 2
            timeline2_1.gameObject.SetActive(false);
            timeline2_2.gameObject.SetActive(true);
            timeline2_2.Play();
            yield return new WaitForSeconds((float)timeline2_2.duration);

            // Tercer timeline de cámara 2
            timeline2_2.gameObject.SetActive(false);
            timeline2_3.gameObject.SetActive(true);
            timeline2_3.Play();
            yield return new WaitForSeconds((float)timeline2_3.duration);

            // Cámara 3 durante 10 segundos
            camera2.SetActive(false);
            timeline2_3.gameObject.SetActive(false);
            camera3.SetActive(true);

            yield return new WaitForSeconds(10f);

            camera3.SetActive(false);
        }
    }
}