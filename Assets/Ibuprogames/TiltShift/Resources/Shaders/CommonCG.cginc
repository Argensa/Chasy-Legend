///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Ibuprogames <hello@ibuprogames.com>. All rights reserved.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#ifndef COMMON_CGINC
#define COMMON_CGINC

#define _PI  3.141592653589

UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);
float4 _MainTex_ST;
float4 _MainTex_TexelSize;

#if SHADER_API_PS4
inline float2 lerp(float2 a, float2 b, float t)
{
  return lerp(a, b, (float2)t);
}

inline float3 lerp(float3 a, float3 b, float t)
{
  return lerp(a, b, (float3)t);
}

inline float4 lerp(float4 a, float4 b, float t)
{
  return lerp(a, b, (float4)t);
}
#endif

inline float Rand(float c)
{
  return frac(sin(dot(float2(c, 1.0 - c), float2(12.9898, 78.233))) * 43758.5453);
}

inline float Rand(float2 n)
{
  return frac(sin(dot(n, float2(12.9898, 78.233))) * 43758.5453);
}

inline float SRand(float2 n)
{
  return Rand(n) * 2.0 - 1.0;
}

inline float Trunc(float x, float num_levels)
{
  return floor(x * num_levels) / num_levels;
}

inline float2 Trunc(float2 x, float2 num_levels)
{
  return floor(x * num_levels) / num_levels;
}

inline float mod(float x, float y)
{
  return x - y * floor(x / y);
}

inline float3 GammaToLinear(float3 pixel)
{
  return pixel * (pixel * (pixel * 0.305306011 + 0.682171111) + 0.012522878);
}

inline float3 LinearToGamma(float3 pixel)
{
  return max(1.055 * pow(pixel, 0.4545) - 0.055, 0.0);
}

inline float3 LDRGamma(float3 pixel)
{
#if QUALITY_MOBILE
  pixel = min((0.999).xxx, pixel);
#endif

  return pixel;
}

inline float3 HDRGamma(float3 pixel)
{
#if QUALITY_MOBILE
  pixel = min((0.999).xxx, pixel);
#else
  pixel = saturate(pixel);
#endif

  return pixel;
}

inline float3 LDRLinear(float3 pixel)
{
  pixel = LinearToGamma(pixel);

#if QUALITY_MOBILE
  pixel = min((0.999).xxx, pixel);
#endif

  return pixel;
}

inline float3 HDRLinear(float3 pixel)
{
  pixel = LinearToGamma(pixel);

#if QUALITY_MOBILE
  pixel = min((0.999).xxx, pixel);
#else
  pixel = saturate(pixel);
#endif

  return pixel;
}

inline float4 SampleMainTexture(float2 uv)
{
  float4 pixel = 
#if defined(UNITY_SINGLE_PASS_STEREO)
  tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(uv, _MainTex_ST));
#else
  UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, uv);
#endif

#ifdef LDR_GAMMA
  pixel.rgb = LDRGamma(pixel.rgb);
#elif HDR_GAMMA
  pixel.rgb = HDRGamma(pixel.rgb);
#elif LDR_LINEAR
  pixel.rgb = LDRLinear(pixel.rgb);
#elif HDR_GAMMA
  pixel.rgb = HDRGamma(pixel.rgb);
#endif

  return pixel;
}

inline float4 SampleTexture(sampler2D tex, float2 uv)
{
  float4 pixel = UNITY_SAMPLE_SCREENSPACE_TEXTURE(tex, uv);

#ifdef LDR_GAMMA
  pixel.rgb = LDRGamma(pixel.rgb);
#elif HDR_GAMMA
  pixel.rgb = HDRGamma(pixel.rgb);
#elif LDR_LINEAR
  pixel.rgb = LDRLinear(pixel.rgb);
#elif HDR_GAMMA
  pixel.rgb = HDRGamma(pixel.rgb);
#endif

  return pixel;
}

#endif
