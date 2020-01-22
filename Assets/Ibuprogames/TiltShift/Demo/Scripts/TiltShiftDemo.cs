///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Ibuprogames <hello@ibuprogames.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;

using Ibuprogames.TiltShiftAsset;

/// <summary>
/// Tilt Shift demo.
/// </summary>
public sealed class TiltShiftDemo : MonoBehaviour
{
  [SerializeField]
  private bool guiShow = true;

  private TiltShift tiltShift;

  private bool menuOpen = false;

  private const float guiMargen = 10.0f;
  private const float guiWidth = 200.0f;

  private float updateInterval = 0.5f;
  private float accum = 0.0f;
  private int frames = 0;
  private float timeleft;
  private float fps = 0.0f;

  private GUIStyle effectNameStyle;
  private GUIStyle menuStyle;
  private GUIStyle boxStyle;

  private void OnEnable()
  {
    Camera selectedCamera = null;
    Camera[] cameras = GameObject.FindObjectsOfType<Camera>();

    for (int i = 0; i < cameras.Length; ++i)
    {
      if (cameras[i].enabled == true)
      {
        selectedCamera = cameras[i];

        break;
      }
    }

    if (selectedCamera != null)
    {
      tiltShift = selectedCamera.gameObject.GetComponent<TiltShift>();
      if (tiltShift == null)
        tiltShift = selectedCamera.gameObject.AddComponent<TiltShift>();

      ResetDemo();
    }
    else
      Debug.LogWarning("No camera found.");
  }

  private void Update()
  {
    timeleft -= Time.deltaTime;
    accum += Time.timeScale / Time.deltaTime;
    frames++;

    if (timeleft <= 0.0f)
    {
      fps = accum / frames;
      timeleft = updateInterval;
      accum = 0.0f;
      frames = 0;
    }

    if (Input.GetKeyUp(KeyCode.F1) == true)
      guiShow = !guiShow;

#if !UNITY_WEBPLAYER
    if (Input.GetKeyDown(KeyCode.Escape))
      Application.Quit();
#endif
  }

  private void OnGUI()
  {
    if (tiltShift == null)
      return;

    if (effectNameStyle == null)
    {
      effectNameStyle = new GUIStyle(GUI.skin.textArea);
      effectNameStyle.alignment = TextAnchor.MiddleCenter;
      effectNameStyle.fontSize = 22;
    }

    if (menuStyle == null)
    {
      menuStyle = new GUIStyle(GUI.skin.textArea);
      menuStyle.alignment = TextAnchor.MiddleCenter;
      menuStyle.fontSize = 14;
    }

    if (boxStyle == null)
    {
      boxStyle = new GUIStyle(GUI.skin.box);
      boxStyle.normal.background = MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 0.75f));
      boxStyle.focused.textColor = Color.red;
    }

    if (guiShow == false)
      return;

    GUILayout.BeginHorizontal(boxStyle, GUILayout.Width(Screen.width));
    {
      GUILayout.Space(guiMargen);

      if (GUILayout.Button("MENU", menuStyle, GUILayout.Width(80.0f)) == true)
        menuOpen = !menuOpen;

      GUILayout.FlexibleSpace();

      GUILayout.Label("Tilt Shift", menuStyle, GUILayout.Width(200.0f));

      GUILayout.FlexibleSpace();

      if (fps < 30.0f)
        GUI.contentColor = Color.yellow;
      else if (fps < 15.0f)
        GUI.contentColor = Color.red;
      else
        GUI.contentColor = Color.green;

      GUILayout.Label(fps.ToString("000"), menuStyle, GUILayout.Width(40.0f));

      GUI.contentColor = Color.white;

      GUILayout.Space(guiMargen);
    }
    GUILayout.EndHorizontal();

    if (menuOpen == true)
    {
      GUILayout.BeginVertical(boxStyle, GUILayout.Width(guiWidth));
      {
        GUILayout.Space(guiMargen);

        tiltShift.enabled = GUILayout.SelectionGrid(tiltShift.enabled == true ? 1 : 0, new[] { "OFF", "ON" }, 2) == 1;

        GUI.enabled = tiltShift.enabled;

        // Parameters.
        GUILayout.BeginVertical(boxStyle);
        {
          GUILayout.BeginHorizontal();
          {
            GUILayout.Label("Strength", GUILayout.Width(70));
            tiltShift.Strength = GUILayout.HorizontalSlider(tiltShift.Strength, 0.0f, 1.0f);
          }
          GUILayout.EndHorizontal();

          GUILayout.Space(guiMargen);
          
          GUILayout.BeginHorizontal();
          {
            GUILayout.Label("Angle", GUILayout.Width(70));
            tiltShift.Angle = GUILayout.HorizontalSlider(tiltShift.Angle, -90.0f, 90.0f);
          }
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
          {
            GUILayout.Label("Aperture", GUILayout.Width(70));
            tiltShift.Aperture = GUILayout.HorizontalSlider(tiltShift.Aperture, 0.1f, 5.0f);
          }
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
          {
            GUILayout.Label("Offset", GUILayout.Width(70));
            tiltShift.Offset = GUILayout.HorizontalSlider(tiltShift.Offset, -1.0f, 1.0f);
          }
          GUILayout.EndHorizontal();

          GUILayout.Space(guiMargen);
          
          GUILayout.BeginHorizontal();
          {
            GUILayout.Label("Curve", GUILayout.Width(70));
            tiltShift.BlurCurve = GUILayout.HorizontalSlider(tiltShift.BlurCurve, 1.0f, 10.0f);
          }
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
          {
            GUILayout.Label("Multiplier", GUILayout.Width(70));
            tiltShift.BlurMultiplier = GUILayout.HorizontalSlider(tiltShift.BlurMultiplier, 0.0f, 10.0f);
          }
          GUILayout.EndHorizontal();

          GUILayout.Space(guiMargen);
          
          GUILayout.BeginHorizontal();
          {
            GUILayout.Label("Saturation", GUILayout.Width(70));
            tiltShift.Saturation = GUILayout.HorizontalSlider(tiltShift.Saturation, 0.0f, 1.0f);
          }
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
          {
            GUILayout.Label("Brightness", GUILayout.Width(70));
            tiltShift.Brightness = GUILayout.HorizontalSlider(tiltShift.Brightness, -1.0f, 1.0f);
          }
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
          {
            GUILayout.Label("Contrast", GUILayout.Width(70));
            tiltShift.Contrast = GUILayout.HorizontalSlider(tiltShift.Contrast, -1.0f, 1.0f);
          }
          GUILayout.EndHorizontal();

          GUILayout.BeginHorizontal();
          {
            GUILayout.Label("Gamma", GUILayout.Width(70));
            tiltShift.Gamma = GUILayout.HorizontalSlider(tiltShift.Gamma, 0.1f, 5.0f);
          }
          GUILayout.EndHorizontal();
          
          GUILayout.Space(guiMargen);
          
          tiltShift.ShowLine = GUILayout.Toggle(tiltShift.ShowLine, "  Line");

          tiltShift.ShowMask = GUILayout.Toggle(tiltShift.ShowMask, "  Mask");
          
          GUILayout.Space(guiMargen);
          
          if (GUILayout.Button("Reset") == true)
            ResetDemo();
        }
        GUILayout.EndVertical();

        GUI.enabled = true;

        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical(boxStyle);
        {
          GUILayout.Label("F1 - Hide/Show gui.");
        }
        GUILayout.EndVertical();

        GUILayout.Space(guiMargen);

        if (GUILayout.Button("Open documentation") == true)
          Application.OpenURL("http://www.ibuprogames.com/2019/10/02/tilt-shift/");

#if !UNITY_WEBPLAYER
        if (GUILayout.Button("Quit") == true)
          Application.Quit();
#endif
      }
      GUILayout.EndVertical();
    }
  }

  private void ResetDemo()
  {
    tiltShift.ResetDefaultValues();

    tiltShift.Strength = 1.0f;
  }

  private Texture2D MakeTex(int width, int height, Color col)
  {
    Color[] pix = new Color[width * height];
    for (int i = 0; i < pix.Length; ++i)
      pix[i] = col;

    Texture2D result = new Texture2D(width, height);
    result.SetPixels(pix);
    result.Apply();

    return result;
  }
}
