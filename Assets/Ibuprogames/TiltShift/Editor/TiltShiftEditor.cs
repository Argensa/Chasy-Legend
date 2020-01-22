///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Ibuprogames <hello@ibuprogames.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;

namespace Ibuprogames
{
  namespace TiltShiftAsset
  {
    /// <summary>
    /// Tilt Shift inspector.
    /// </summary>
    [CustomEditor(typeof(TiltShift))]
    public class TiltShiftEditor : Inspector
    {
      /// <summary>
      /// OnInspectorGUI.
      /// </summary>
      protected override void InspectorGUI()
      {
        TiltShift baseTarget = this.target as TiltShift;

        BeginVertical();
        {
          /////////////////////////////////////////////////
          // Common.
          /////////////////////////////////////////////////

          Separator();

          baseTarget.Strength = Slider("Strength", "The strength of the effect.\nFrom 0.0 (no effect) to 1.0 (full effect).", baseTarget.Strength, 0.0f, 1.0f, 1.0f);

          baseTarget.Mode = (TiltShiftModes)EnumPopup("Mode", "Blur zone mode, proportional or fixed (default).", baseTarget.Mode, TiltShiftModes.Fixed);

          /////////////////////////////////////////////////
          // Mask.
          /////////////////////////////////////////////////
          
          Separator();
          
          Header("Mask");

          IndentLevel++;

          if (baseTarget.Mode == TiltShiftModes.Proportional)
          {
            baseTarget.Angle = Slider("Angle", "Mask angle [-89, 90].", baseTarget.Angle, -90.0f, 90.0f, 0.0f);

            baseTarget.Aperture = Slider("Aperture", "Mask aperture [0.1, 5].", baseTarget.Aperture, 0.1f, 5.0f, 0.5f);
          
            baseTarget.Offset = Slider("Offset", "Mask vertical offset [-1, 1].", baseTarget.Offset, -1.0f, 1.0f, 0.0f);
          }
          else
          {
            baseTarget.TopFixedZone = Slider("Top", "Width of the upper zone, in pixels [0, ...].", baseTarget.TopFixedZone, 0.0f, 2048.0f, 100.0f);
            
            baseTarget.BottomFixedZone = Slider("Bottom", "Width of the lower zone, in pixels [0, ...].", baseTarget.BottomFixedZone, 0.0f, 2048.0f, 100.0f);
            
            baseTarget.FocusFixedFallOff = Slider("Falloff", "Width of the falloff zone, in pixels [0, ...].", baseTarget.FocusFixedFallOff, 0.0f, 2048.0f, 100.0f);
          }

          IndentLevel--;

          /////////////////////////////////////////////////
          // Blur.
          /////////////////////////////////////////////////

          Separator();

          Header("Blur");
          
          IndentLevel++;
          
          baseTarget.BlurCurve = Slider("Blur curve", "Blur curve [1, 10].", baseTarget.BlurCurve, 1.0f, 10.0f, 3.0f);

          baseTarget.BlurMultiplier = Slider("Blur multiplier", "Blur multiplier [0, 10].", baseTarget.BlurMultiplier, 0.0f, 10.0f, 5.0f);

          IndentLevel--;

          /////////////////////////////////////////////////
          // Distortion.
          /////////////////////////////////////////////////

          Separator();

          bool showDistortion = true;
          baseTarget.EnableDistortion = ToogleFoldout("Distortion", baseTarget.EnableDistortion, ref showDistortion);
          if (showDistortion == true)
          {
            IndentLevel++;

            EnableGUI = baseTarget.EnableDistortion;

            baseTarget.Distortion = Slider("Force", "Distortion force [0, 20].", baseTarget.Distortion, 0.0f, 20.0f, 5.0f);
            baseTarget.DistortionScale = Slider("Scale", "Distortion scale [0.01, 2].", baseTarget.DistortionScale, 0.01f, 20.0f, 1.0f);

            EnableGUI = true;
            
            IndentLevel--;
          }
          
          /////////////////////////////////////////////////
          // Color.
          /////////////////////////////////////////////////

          Separator();

          bool showColor = true;
          baseTarget.EnableColor = ToogleFoldout("Color", baseTarget.EnableColor, ref showColor);
          if (showColor == true)
          {
            IndentLevel++;

            EnableGUI = baseTarget.EnableColor;

            baseTarget.Tint = Color("Tint", "Tint color", baseTarget.Tint, UnityEngine.Color.white);

            baseTarget.Saturation = Slider("Saturation", "Color saturation [0, 1].", baseTarget.Saturation, 0.0f, 1.0f, 1.0f);
            baseTarget.Brightness = Slider("Brightness", "Color brightness.", baseTarget.Brightness, -1.0f, 1.0f, 0.0f);
            baseTarget.Contrast = Slider("Contrast", "The difference in color and brightness.", baseTarget.Contrast, -1.0f, 1.0f, 0.0f);
            baseTarget.Gamma = Slider("Gamma", "Optimizes the contrast and brightness in the midtones.", baseTarget.Gamma, 0.01f, 5.0f, 1.0f);

            EnableGUI = true;
            
            IndentLevel--;
          }

          /////////////////////////////////////////////////
          // Debug.
          /////////////////////////////////////////////////

          Separator();

          if (Foldout("Debug") == true)
          {
            IndentLevel++;

            if (baseTarget.Mode == TiltShiftModes.Proportional)
              baseTarget.ShowLine = Toggle("Focus line", "Show focus line.", baseTarget.ShowLine, false);

            baseTarget.ShowMask = Toggle("Show mask", "Show blur mask.", baseTarget.ShowMask, false);

            IndentLevel--;
          }

          /////////////////////////////////////////////////
          // Description.
          /////////////////////////////////////////////////

          Separator();

          EditorGUILayout.HelpBox("'Tilt Shift' makes the scene seem much smaller than it actually is, simulating the shallow depth of field normally encountered in close-up photography.", MessageType.Info);

          /////////////////////////////////////////////////
          // Misc.
          /////////////////////////////////////////////////

          Separator();

          BeginHorizontal();
          {
            if (GUILayout.Button(new GUIContent("[doc]", "Online documentation"), GUI.skin.label) == true)
              Application.OpenURL("http://www.ibuprogames.com/2019/10/02/tilt-shift/");

            FlexibleSpace();

            if (Button("Reset") == true)
              baseTarget.ResetDefaultValues();
          }
          EndHorizontal();
        }
        EndVertical();

        Separator();
      }
    }
  }
}
