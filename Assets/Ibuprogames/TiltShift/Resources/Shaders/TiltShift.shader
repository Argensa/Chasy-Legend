///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Ibuprogames <hello@ibuprogames.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

Shader "Hidden/TiltShift"
{
  Properties
  {
    _MainTex("Base (RGB)", 2D) = "white" {}
  }

  CGINCLUDE
  #include "UnityCG.cginc"
  #include "CommonCG.cginc"

  float _Strength;

#if MODE_PROPORTIONAL
  float _Angle;
  float _Aperture;
  float _Offset;
#else
  float _TopFixedZone;
  float _BottomFixedZone;
  float _FocusFixedFallOff;
#endif
  
#if ENABLE_DISTORTION
  float _CubicDistortion;
  float _DistortionScale;
#endif
  
  float _BlurCurve;
  float _BlurMultiplier;

#if ENABLE_COLOR
  float3 _Tint;
  float _Saturation;
  float _Brightness;
  float _Contrast;
  float _Gamma;
#endif  

  struct appdata_t
  {
    float4 vertex : POSITION;
    half2 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
  };
    
  struct v2f
  {
    float4 vertex : SV_POSITION;
    half2 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO    
  };
    
  v2f vert(appdata_t v)
  {
    v2f o;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_TRANSFER_INSTANCE_ID(v, o);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
    o.vertex = UnityObjectToClipPos(v.vertex);
    
#if UNITY_UV_STARTS_AT_TOP
    if (_MainTex_TexelSize.y < 0)
      o.texcoord.y = 1.0 - o.texcoord.y;
#endif
    
    o.texcoord = v.texcoord;
    
    return o;
  }

  inline float4 fragPass0(v2f i) : SV_Target
  {
    const float Weight[11] =
    {
      0.082607,
      0.080977,
      0.076276,
      0.069041,
      0.060049,
      0.050187,
      0.040306,
      0.031105,
      0.023066,
      0.016436,
      0.011254
    };
  
    UNITY_SETUP_INSTANCE_ID(i);

    float4 pixel = SampleMainTexture(i.texcoord);
    float4 final = pixel;

    float aspectRatio = _ScreenParams.y / _ScreenParams.x;
    
    float2 uv = i.texcoord;
    float blurMask = 0.0;

#if ENABLE_DISTORTION
    float2 h = uv - float2(0.5, 0.5);
    float r2 = dot(h, h);
    uv = (1.0 + r2 * (_CubicDistortion * sqrt(r2))) * _DistortionScale * h + 0.5;
#endif

#if MODE_PROPORTIONAL    
    float2 uvAspect = uv;

    uvAspect.y += aspectRatio * 0.5 - 0.5;
    uvAspect.y /= aspectRatio;
    uvAspect = uvAspect * 2.0 - 1.0;
    uvAspect *= _Aperture;
    
    float2 tiltVector = float2(sin(_Angle), cos(_Angle));
    
    blurMask = abs(dot(tiltVector, uvAspect) + _Offset);
    blurMask = max(0.0, min(1.0, blurMask));
#else
    blurMask = 1.0 - smoothstep(_TopFixedZone - _FocusFixedFallOff, _TopFixedZone, uv.y);
    blurMask += smoothstep(_BottomFixedZone, _BottomFixedZone + _FocusFixedFallOff, uv.y);
#endif

    final.a = blurMask;
    blurMask = pow(blurMask, _BlurCurve);
    uv = i.texcoord;

    if (blurMask > 0.0)
    {
      float uvOffset = _MainTex_TexelSize.x * blurMask * _BlurMultiplier;
      final.rgb *= Weight[0];

      [unroll]
      for (int i = 1; i < 11; i++)
      {
        float sampleOffset = i * uvOffset;

        final.rgb += (tex2Dlod(_MainTex, float4(uv + float2(sampleOffset, 0.0), 0.0, 0.0)).rgb +
                      tex2Dlod(_MainTex, float4(uv - float2(sampleOffset, 0.0), 0.0, 0.0)).rgb) * Weight[i];
      }
    }
    
    final.rgb = lerp(pixel.rgb, final.rgb, _Strength);

    return final;
  }

  inline float4 fragPass1(v2f i) : SV_Target
  {
	const float Weight[11] =
	{
		0.082607,
		0.080977,
		0.076276,
		0.069041,
		0.060049,
		0.050187,
		0.040306,
		0.031105,
		0.023066,
		0.016436,
		0.011254
	};

    UNITY_SETUP_INSTANCE_ID(i);

    float2 uv = i.texcoord;
    float4 pixel = SampleMainTexture(uv);
    float4 final = pixel;

    float blurMask = pow(abs(pixel.a), _BlurCurve);

    if (blurMask > 0.0)
    {
      float uvOffset = _MainTex_TexelSize.y * blurMask * _BlurMultiplier;
      final.rgb *= Weight[0];
    
      [unroll]
      for (int i = 1; i < 11; i++)
      {
        float sampleOffset = i * uvOffset;

        final.rgb += (tex2Dlod(_MainTex, float4(uv + float2(0.0, sampleOffset), 0.0, 0.0)).rgb +
                      tex2Dlod(_MainTex, float4(uv - float2(0.0, sampleOffset), 0.0, 0.0)).rgb) * Weight[i];
      }
    }

#if ENABLE_COLOR
    final.rgb = lerp(final.rgb, final.rgb * _Tint, blurMask);
    
    float3 color = lerp(dot(final.rgb, float3(0.299, 0.587, 0.114)), final.rgb, _Saturation);
    final.rgb = lerp(final.rgb, color, blurMask);
    
    color = (final.rgb - 0.5) * _Contrast + 0.5 + _Brightness;
    color = clamp(color, 0.0, 1.0);
    final.rgb = lerp(final.rgb, color, blurMask);

    color = pow(final.rgb, _Gamma);    
    final.rgb = lerp(final.rgb, color, blurMask);
#endif

    final.rgb = lerp(pixel.rgb, final.rgb, _Strength);

#if LDR_LINEAR || HDR_LINEAR
    final.rgb = GammaToLinear(final.rgb);
#endif

#if SHOW_MASK
    final = blurMask; 
#endif

#if SHOW_LINE
    if (pixel.a < 0.005)
      final = float4(1.0, 0.0, 0.0, 1.0);
#endif

    return final;
  }
  ENDCG

  SubShader
  {
    Cull Off
    ZWrite Off
    ZTest Always

    // Pass 0: Alpha and horizontal Gaussian blur.
    Pass
    {
      CGPROGRAM
      #pragma target 3.0
      #pragma fragmentoption ARB_precision_hint_fastest
      #pragma exclude_renderers d3d9 d3d11_9x ps3 flash

      #pragma multi_compile ___ LDR_GAMMA HDR_GAMMA LDR_LINEAR HDR_LINEAR
      #pragma multi_compile ___ MODE_PROPORTIONAL
      #pragma multi_compile ___ ENABLE_DISTORTION

      #pragma vertex vert
      #pragma fragment fragPass0
      ENDCG
    }
    
    // Pass 1: Vertical Gaussian blur and RedLine.
    Pass
    {
      CGPROGRAM
      #pragma target 3.0
      #pragma fragmentoption ARB_precision_hint_fastest
      #pragma exclude_renderers d3d9 d3d11_9x ps3 flash

      #pragma multi_compile ___ LDR_GAMMA HDR_GAMMA LDR_LINEAR HDR_LINEAR
      #pragma multi_compile ___ ENABLE_COLOR
      #pragma multi_compile ___ SHOW_LINE
      #pragma multi_compile ___ SHOW_MASK

      #pragma vertex vert
      #pragma fragment fragPass1
      ENDCG
    }
  }

  Fallback off
}
