using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class provide screen fading and screen appearance
/// </summary>
public class FaderController : MonoBehaviour
{
    /// <summary>
    /// Default color for panel
    /// </summary>
    public Color defaultColor = Color.black;

    /// <summary>
    /// Panel's Image that will be fading
    /// </summary>
    private Image fadePanel;

    /// <summary>
    /// Contain actual coroutine function
    /// </summary>
    private IEnumerator FadeCoroutine;

    //[SerializeField]
    private bool isFadeOut;

    //[SerializeField]
    private bool isFadeIn;

    //[SerializeField]
    private bool isFadeInOut;

    // Start is called before the first frame update
    void Start()
    {
        fadePanel = GetComponent<Image> ();
    }

    /// <summary>
    /// Check if the process of showing or fading is producing
    /// </summary>
    /// <returns></returns>
    public bool IsFading()
    {
        if (isFadeIn || isFadeOut || isFadeInOut)
            return true;

        return false;
    }

    /// <summary>
    /// Function is fading the screen, waiting some time and showing the screen back
    /// </summary>
    /// <param name="durationIn">Screen fading duration in seconds. Default value is 3 seconds.</param>
    /// <param name="colorIn">Panel's Image fading animation color. Default value is black.</param>
    /// <param name="durationOut">Screen showing duration in seconds. Default value is 3 seconds.</param>
    /// <param name="colorOut">Panel's Image showing animation color. Default value is black.</param>
    /// <param name="fadeDelay">Delay between fading and showing back.</param>
    public void FadeInOut( float durationIn = 3, Color? colorIn = null,
                          float durationOut = 3, Color? colorOut = null, float fadeDelay = 5 )
    {
        if (colorIn == null)
            colorIn = defaultColor;

        if (colorOut == null)
            colorOut = defaultColor;

        Color animColorIn = (Color)colorIn;
        Color animColorOut = (Color)colorIn;

        // Save original alpha of fadePanel
        animColorIn.a = fadePanel.color.a;
        fadePanel.color = animColorIn;
        isFadeInOut = true;

        FadeCoroutine = FadeInOutCoroutine (durationIn, animColorIn, durationOut, animColorOut, fadeDelay);
        StartCoroutine (FadeCoroutine);
    }

    /// <summary>
    /// Supporting function for FadeInOut, which produces the fading and the showing of the screen.
    /// </summary>
    private IEnumerator FadeInOutCoroutine( float durationIn, Color colorIn,
                                           float durationOut, Color colorOut, float fadeDelay )
    {
        FadeIn (durationIn, colorIn);
        yield return new WaitForSeconds (fadeDelay + durationIn);
        FadeOut (durationOut, colorOut);
        yield return new WaitForSeconds (durationOut);
        FadeCoroutine = null;
        isFadeInOut = false;
    }

    /// <summary>
    /// Function is showing the screen in time
    /// </summary>
    /// <param name="duration">Screen showing duration in seconds. Default value is 3 seconds.</param>
    /// <param name="color">Panel's Image animation color. Default value is black.</param>
    public void FadeOut( float duration = 3, Color? color = null )
    {
        if (color == null)
            color = defaultColor;

        Color animColor = (Color)color;
        // Save original alpha of fadePanel
        animColor.a = fadePanel.color.a;
        fadePanel.color = animColor;

        isFadeOut = true;

        // Stop all coroutines if scene is fading or wating between fading
        // and showing in function FadiInOut
        if (isFadeIn || ( isFadeInOut && FadeCoroutine == null ))
        {
            StopAllCoroutines ();
            isFadeInOut = false;
            isFadeIn = false;
        }

        FadeCoroutine = FadeOutCoroutine (duration, animColor);
        StartCoroutine (FadeCoroutine);

    }

    /// <summary>
    /// Function is fading the screen in time
    /// </summary>
    /// <param name="duration">Screen fading duration in seconds. Default value is 3 seconds.</param>
    /// <param name="color">Panel's Image animation color. Default value is black.</param>
    public void FadeIn( float duration = 3, Color? color = null )
    {
        if (color == null)
            color = defaultColor;

        Color animColor = (Color)color;
        // Save original alpha of fadePanel
        animColor.a = fadePanel.color.a;
        fadePanel.color = animColor;

        // Stop all coroutines if scene is showing or wating between fading
        // and showing in function FadiInOut
        if (isFadeOut || ( isFadeInOut && FadeCoroutine == null ))
        {
            StopAllCoroutines ();
            isFadeInOut = false;
            isFadeOut = false;
        }

        isFadeIn = true;

        FadeCoroutine = FadeInCoroutine (duration, animColor);
        StartCoroutine (FadeCoroutine);
    }

    /// <summary>
    /// Supporting function for FadeIn, which produces the fading of screen.
    /// </summary>
    private IEnumerator FadeInCoroutine( float duration, Color color )
    {
        // Time, when the script was run
        float startTime = Time.time;
        // Starting image alpha
        float alpha = color.a;
        // Fading step (depends on fading duration)
        float step = 1;

        // Chages alpha channel every frame by step
        while (startTime + duration > Time.time)
        {
            step -= ( 1 / duration ) * Time.deltaTime;
            color.a = Mathf.Lerp (1, alpha, step);
            fadePanel.color = color;

            yield return null;
        }
        FadeCoroutine = null;
        isFadeIn = false;
    }

    /// <summary>
    /// Supporting function for FadeOut, which produces the appearance of screen.
    /// </summary>
    private IEnumerator FadeOutCoroutine( float duration, Color color )
    {
        // Time, when the script was run
        float startTime = Time.time;
        // Starting image alpha
        float alpha = color.a;
        // Fading step (depends on fading duration)
        float step = 0;

        // Chages alpha channel every frame by step
        while (startTime + duration > Time.time)
        {
            step += ( 1 / duration ) * Time.deltaTime;
            color.a = Mathf.Lerp (alpha, 0, step);
            fadePanel.color = color;

            yield return null;
        }
        FadeCoroutine = null;
        isFadeOut = false;
    }
}