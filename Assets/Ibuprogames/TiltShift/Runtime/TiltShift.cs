///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Ibuprogames <hello@ibuprogames.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;

namespace Ibuprogames
{
  namespace TiltShiftAsset
  {
    /// <summary>
    /// Tilt Shift effect.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [HelpURL("http://www.ibuprogames.com/2019/10/02/tilt-shift/")]
    [AddComponentMenu("Image Effects/Ibuprogames/Tilt Shift")]
    public sealed class TiltShift : MonoBehaviour
    {
      /// <summary>
      /// Strength of the effect [0, 1]. Default 1.
      /// </summary>
      public float Strength
      {
        get { return strength; }
        set { strength = Mathf.Clamp01(value); }
      }

      /// <summary>
      /// Blur zone mode: proportional or fixed (default).
      /// </summary>
      public TiltShiftModes Mode
      {
        get { return mode; }
        set
        {
          mode = value;
          
          if (showLine == true && mode == TiltShiftModes.Fixed)
            showLine = false;
        }
      }
      
      #region Mask.
      /// <summary>
      /// Angle [-90, 90]. Default 0.
      /// Only available if Mode == TiltShiftModes.Proportional.
      /// </summary>
      public float Angle
      {
        get { return angle; }
        set { angle = Mathf.Clamp(value, -90.0f, 90.0f); }
      }

      /// <summary>
      /// Effect aperture [0.1, 5]. Default 0.5.
      /// Only available if Mode == TiltShiftModes.Proportional.
      /// </summary>
      public float Aperture
      {
        get { return aperture; }
        set { aperture = value; }
      }
      
      /// <summary>
      /// Vertical offset [-1.0, 1.0]. Default 0.
      /// Only available if Mode == TiltShiftModes.Proportional.
      /// </summary>
      public float Offset
      {
        get { return offset; }
        set { offset = Mathf.Clamp(value, -1.0f, 1.0f); }
      }
      
      /// <summary>
      /// Width of the upper zone, in pixels [0, ...]. Default 100.
      /// Only available if Mode == TiltShiftModes.Fixed.
      /// </summary>
      public float TopFixedZone
      {
        get { return topFixedZone; }
        set { topFixedZone = Mathf.Max(value, 0.0f); }
      }
      
      /// <summary>
      /// Width of the upper zone, in pixels [0, ...]. Default 100.
      /// Only available if Mode == TiltShiftModes.Fixed.
      /// </summary>
      public float BottomFixedZone
      {
        get { return bottomFixedZone; }
        set { bottomFixedZone = Mathf.Max(value, 0.0f); }
      }
      
      /// <summary>
      /// Width of the falloff zone, in pixels [0, ...]. Default 100.
      /// Only available if Mode == TiltShiftModes.Fixed.
      /// </summary>
      public float FocusFixedFallOff
      {
        get { return focusFixedFallOff; }
        set { focusFixedFallOff = Mathf.Max(value, 0.0f); }
      }
      #endregion
      
      #region Blur.
      /// <summary>
      /// Blur curve [1.0, 10.0]. Default 3.0.
      /// </summary>
      public float BlurCurve
      {
        get { return blurCurve; }
        set { blurCurve = Mathf.Clamp(value, 1.0f, 10.0f); }
      }

      /// <summary>
      /// Blur multiplier [0.0, 10.0]. Default 5.
      /// </summary>
      public float BlurMultiplier
      {
        get { return blurMultiplier; }
        set { blurMultiplier = Mathf.Clamp(value, 0.0f, 10.0f); }
      }
      #endregion
      
      #region Distortion
      /// <summary>
      /// Enable distortion effect.
      /// </summary>
      public bool EnableDistortion
      {
        get { return enableDistortion; }
        set { enableDistortion = value; }
      }
      
      /// <summary>
      /// Distortion force [0, 20]. Default 5.
      /// </summary>
      public float Distortion
      {
        get { return cubicDistortion; }
        set { cubicDistortion = Mathf.Clamp(value, 0.0f, 20.0f); }
      }
      
      /// <summary>
      /// Distortion scale [0.01, 2]. Default 1.
      /// </summary>
      public float DistortionScale
      {
        get { return distortionScale; }
        set { distortionScale = Mathf.Clamp(value, 0.01f, 2.0f); }
      }
      #endregion
      
      #region Color.
      /// <summary>
      /// Enable color effects.
      /// </summary>
      public bool EnableColor
      {
        get { return enableColor; }
        set { enableColor = value; }
      }
      
      /// <summary>
      /// Tint color.
      /// </summary>
      public Color Tint
      {
        get { return tint; }
        set { tint = value; }
      }
      
      /// <summary>
      /// Color saturation [0.0, 1.0]. Default 1.
      /// </summary>
      public float Saturation
      {
        get { return saturation; }
        set { saturation = Mathf.Clamp01(value); }
      }

      /// <summary>
      /// Brightness [-1.0, 1.0]. Default 0.
      /// </summary>
      public float Brightness
      {
        get { return brightness; }
        set { brightness = Mathf.Clamp(value, -1.0f, 1.0f); }
      }

      /// <summary>
      /// Contrast [-1.0, 1.0). Default 0.
      /// </summary>
      public float Contrast
      {
        get { return contrast; }
        set { contrast = Mathf.Clamp(value, -1.0f, 1.0f); }
      }

      /// <summary>
      /// Gamma [0.1, 5.0]. Default 1.
      /// </summary>
      public float Gamma
      {
        get { return gamma; }
        set { gamma = Mathf.Clamp(value, 0.1f, 5.0f); }
      }      
      #endregion

      #region Debug.
      /// <summary>
      /// Show angle line. Default false.
      /// </summary>
      public bool ShowLine
      {
        get { return showLine; }
        set { showLine = value; }
      }

      /// <summary>
      /// Show blur mask. Default false.
      /// </summary>
      public bool ShowMask
      {
        get { return showMask; }
        set { showMask = value; }
      }
      #endregion
      
      [SerializeField]
      private float strength = 1.0f;

      [SerializeField]
      private TiltShiftModes mode = TiltShiftModes.Proportional;
      
      [SerializeField]
      private float angle = 0.0f;

      [SerializeField]
      private float aperture = 0.5f;
      
      [SerializeField]
      private float offset = 0.0f;

      [SerializeField]
      private float topFixedZone = 100.0f;

      [SerializeField]
      private float bottomFixedZone = 100.0f;

      [SerializeField]
      private float focusFixedFallOff = 100.0f;

      [SerializeField]
      private float blurCurve = 3.0f;

      [SerializeField]
      private float blurMultiplier = 5.0f;

      [SerializeField]
      private bool enableDistortion = false;
      
      [SerializeField]
      private float cubicDistortion = 5.0f;
      
      [SerializeField]
      private float distortionScale = 1.0f;
      
      [SerializeField]
      private bool enableColor = true;

      [SerializeField]
      private Color tint = Color.white;

      [SerializeField]
      private float saturation = 1.0f;
      
      [SerializeField]
      private float brightness = 0.0f;

      [SerializeField]
      private float contrast = 0.0f;

      [SerializeField]
      private float gamma = 1.0f;

      [SerializeField]
      private bool showLine = false;

      [SerializeField]
      private bool showMask = false;
      
      private Shader shader;

      private Material material;

      private readonly int variableStrength = Shader.PropertyToID("_Strength");
      private readonly int variableAngle = Shader.PropertyToID("_Angle");
      private readonly int variableAperture = Shader.PropertyToID("_Aperture");
      private readonly int variableOffset = Shader.PropertyToID("_Offset");
      private readonly int variableTopFixedZone = Shader.PropertyToID("_TopFixedZone");
      private readonly int variableBottomFixedZone = Shader.PropertyToID("_BottomFixedZone");
      private readonly int variableFocusFixedFallOff = Shader.PropertyToID("_FocusFixedFallOff");
      private readonly int variableBlurCurve = Shader.PropertyToID("_BlurCurve");
      private readonly int variableBlurMultiplier = Shader.PropertyToID("_BlurMultiplier");
      private readonly int variableCubicDistortion = Shader.PropertyToID("_CubicDistortion");
      private readonly int variableDistortionScale = Shader.PropertyToID("_DistortionScale");
      private readonly int variableTint = Shader.PropertyToID("_Tint");
      private readonly int variableSaturation = Shader.PropertyToID("_Saturation");
      private readonly int variableBrightness = Shader.PropertyToID("_Brightness");
      private readonly int variableContrast = Shader.PropertyToID("_Contrast");
      private readonly int variableGamma = Shader.PropertyToID("_Gamma");

      private readonly string keywordModeProportional = "MODE_PROPORTIONAL";
      private readonly string keywordEnableDistortion = "ENABLE_DISTORTION";
      private readonly string keywordEnableColor = "ENABLE_COLOR";
      private readonly string keywordShowLine = "SHOW_LINE";
      private readonly string keywordShowMask = "SHOW_MASK";

      private readonly string shaderPath = "Shaders/TiltShift";

      /// <summary>
      /// Effect supported by the current hardware?
      /// </summary>
      public bool IsSupported()
      {
        bool supported = false;

        Shader test = Resources.Load<Shader>(shaderPath);
        if (test != null)
        {
          supported = test.isSupported;

          Resources.UnloadAsset(test);
        }

        return supported;
      }

      /// <summary>
      /// Reset to default values.
      /// </summary>
      public void ResetDefaultValues()
      {
        strength = 1.0f;
        
        angle = 0.0f;
        aperture = 0.5f;
        offset = 0.0f;
        
        topFixedZone = 100.0f;
        bottomFixedZone = 100.0f;
        focusFixedFallOff = 100.0f;

        blurCurve = 3.0f;
        blurMultiplier = 5.0f;
        
        cubicDistortion = 5.0f;
        distortionScale = 1.0f;

        tint = Color.white;
        saturation = 1.0f;
        brightness = 0.0f;
        contrast = 0.0f;
        gamma = 1.0f;
      }

      private Material Material
      {
        get
        {
          if (material == null && shader != null)
          {
            string materialName = this.GetType().Name;

            material = new Material(shader);
            if (material != null)
            {
              material.name = materialName;
              material.hideFlags = HideFlags.HideAndDontSave;
            }
            else
            {
              Debug.LogErrorFormat("[Ibuprogames.TiltShift] '{0}' material null. Please contact with 'hello@ibuprogames.com' and send the log file.", materialName);

              this.enabled = false;
            }
          }

          return material;
        }
      }

      /// <summary>
      /// Called on the frame when a script is enabled just before any of the Update methods is called the first time.
      /// </summary>
      private void Start()
      {
        shader = Resources.Load<Shader>(shaderPath);
        if (shader != null)
        {
          if (shader.isSupported == false)
          {
            Debug.LogErrorFormat("[Ibuprogames.TiltShift] '{0}' shader not supported. Please contact with 'hello@ibuprogames.com' and send the log file.", shaderPath);

            this.enabled = false;
          }
        }
        else
        {
          Debug.LogErrorFormat("[Ibuprogames.TiltShift] Shader 'Ibuprogames/TiltShift/Resources/{0}.shader' not found. '{1}' disabled.", shaderPath, this.GetType().Name);

          this.enabled = false;
        }
      }

      /// <summary>
      /// When the MonoBehaviour will be destroyed.
      /// </summary>
      private void OnDestroy()
      {
        if (material != null)
#if UNITY_EDITOR
          DestroyImmediate(material);
#else
				  Destroy(material);
#endif
      }

      /// <summary>
      /// Called after all rendering is complete to render image.
      /// </summary>
      private void OnRenderImage(RenderTexture source, RenderTexture destination)
      {
        if (Material != null)
        {
          material.shaderKeywords = null;

          // Common.
          material.SetFloat(variableStrength, strength);

          // Mask.
          if (mode == TiltShiftModes.Proportional)
          {
            material.EnableKeyword(keywordModeProportional);
            
            material.SetFloat(variableAngle, Mathf.Deg2Rad * angle);
            material.SetFloat(variableAperture, aperture);
            material.SetFloat(variableOffset, offset);
          }
          else
          {
            material.SetFloat(variableBottomFixedZone, (Screen.height - topFixedZone) / Screen.height);
            material.SetFloat(variableTopFixedZone, bottomFixedZone / Screen.height);
            material.SetFloat(variableFocusFixedFallOff, focusFixedFallOff / Screen.height);
          }

          // Blur.
          material.SetFloat(variableBlurCurve, blurCurve);
          material.SetFloat(variableBlurMultiplier, blurMultiplier);
          
          // Distortion.
          if (enableDistortion == true)
          {
            material.EnableKeyword(keywordEnableDistortion);
            
            material.SetFloat(variableCubicDistortion, cubicDistortion);
            material.SetFloat(variableDistortionScale, distortionScale);
          }

          // Color.
          if (enableColor == true)
          {
            material.EnableKeyword(keywordEnableColor);
            
            material.SetColor(variableTint, tint);
            material.SetFloat(variableSaturation, saturation);
            material.SetFloat(variableBrightness, brightness);
            material.SetFloat(variableContrast, contrast + 1.0f);
            material.SetFloat(variableGamma, 1.0f / gamma);
          }

          // Debug.
          if (showLine == true)
            material.EnableKeyword(keywordShowLine);

          if (showMask == true)
            material.EnableKeyword(keywordShowMask);
          
          RenderTexture renderTexture0 = RenderTexture.GetTemporary(source.width, source.height, 0,
            Application.isMobilePlatform == true ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR);

          Graphics.Blit(source, renderTexture0, material, 0);
          
          Graphics.Blit(renderTexture0, destination, material, 1);
          
          RenderTexture.ReleaseTemporary(renderTexture0);
        }
        else
          Graphics.Blit(source, destination);
      }
    }
  }
}
