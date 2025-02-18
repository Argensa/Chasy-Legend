﻿///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
    /// About window.
    /// </summary>
    public sealed class TiltShiftAbout : EditorWindow
    {
      public static readonly string assetID = "Ibuprogames.TiltShift";
      private static readonly string description = "'Tilt Shift' makes the scene seem much smaller than it actually is, simulating the shallow depth of field normally encountered in close-up photography.";

      private static readonly string documentationURL = "http://www.ibuprogames.com/2019/10/02/tilt-shift/";
      private static readonly string publisherURL = "https://assetstore.unity.com/publishers/8484";
      private static readonly string forumURL = "https://forum.unity.com/threads/tilt-shift.754493/";
      private static readonly string storeURL = "https://assetstore.unity.com/packages/slug/155099";
      private static readonly string twitterURL = "https://twitter.com/Ibuprogames";
      private static readonly string facebookURL = "https://www.facebook.com/ibuprogames";
      private static readonly string githubURL = "https://github.com/Ibuprogames";

      private Vector2 scroll = Vector2.zero;

      private GUIStyle descriptionStyle;
      private GUIStyle buttonStyle;
      private GUIStyle iconStyle;
      private GUIStyle scrollStyle;

      private string changelog = "Changelog not available :(";
      private string version = "1.0";

      private Texture2D headerTexture;
      private Texture2D forumTexture;
      private Texture2D twitterTexture;
      private Texture2D facebookTexture;
      private Texture2D githubTexture;

      [MenuItem("Help/Ibuprogames/Tilt Shift/About", false, 0)]
      public static void MenuAbout()
      {
        Open();
      }

      [MenuItem("Help/Ibuprogames/Tilt Shift/Documentation", false, 0)]
      public static void MenuDocumentation()
      {
        Application.OpenURL(documentationURL);
      }

      public static string GetAssetPath()
      {
        string[] results = AssetDatabase.FindAssets("TiltShiftAbout t:Script", null);
        if (results.Length > 0)
        {
          string assetPath = AssetDatabase.GUIDToAssetPath(results[0]);
          assetPath = assetPath.Replace("Editor/About/TiltShiftAbout.cs", string.Empty);
          if (string.IsNullOrEmpty(assetPath) == false)
            return assetPath;
        }

        return string.Empty;
      }

      public static void Open()
      {
        TiltShiftAbout window = EditorWindow.GetWindow<TiltShiftAbout>(true, "About 'Tilt Shift'", true);
        window.minSize = window.maxSize = new Vector2(700, 500);
        window.ShowUtility();
      }

      private void OnEnable()
      {
        string assetPath = GetAssetPath();
        if (string.IsNullOrEmpty(assetPath) == false)
        {
          TextAsset changeLogAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath + "Changelog.txt");
          if (changeLogAsset != null && string.IsNullOrEmpty(changeLogAsset.text) == false)
          {
            changelog = changeLogAsset.text;

            version = changelog.Split(new char[] { '\n', '\r' })[0];
          }

          headerTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath + "Editor/About/Header.png");
          forumTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath + "Editor/About/Forum.png");
          twitterTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath + "Editor/About/Twitter.png");
          facebookTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath + "Editor/About/Facebook.png");
          githubTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath + "Editor/About/Github.png");
        }

        EditorPrefs.SetString(assetID, version);
      }

      private void OnGUI()
      {
        if (descriptionStyle == null)
        {
          descriptionStyle = new GUIStyle(GUI.skin.label);
          descriptionStyle.alignment = TextAnchor.MiddleLeft;
          descriptionStyle.fontSize = 17;
          descriptionStyle.richText = true;
          descriptionStyle.wordWrap = true;
        }

        if (buttonStyle == null)
        {
          buttonStyle = new GUIStyle(GUI.skin.button);
          buttonStyle.alignment = TextAnchor.MiddleLeft;
          buttonStyle.fontSize = 17;
          buttonStyle.richText = true;
          buttonStyle.wordWrap = true;
        }

        if (iconStyle == null)
        {
          iconStyle = new GUIStyle(GUI.skin.label);
          iconStyle.alignment = TextAnchor.MiddleCenter;
        }

        if (scrollStyle == null)
        {
          scrollStyle = new GUIStyle(GUI.skin.box);
          scrollStyle.richText = true;
          scrollStyle.wordWrap = true;
        }

        GUILayout.BeginVertical("box");
        {
          const float space = 5.0f;

          if (headerTexture != null)
          {
            Rect headerRect = new Rect(8.0f, 8.0f, 684.0f, 190.0f);
            GUI.DrawTexture(headerRect, headerTexture, ScaleMode.ScaleAndCrop, false);

            GUILayout.Space(200.0f);
          }

          GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
          {
            GUILayout.Space(space);

            GUILayout.TextArea(description, descriptionStyle, GUILayout.ExpandWidth(true), GUILayout.Height(60.0f));

            GUILayout.Space(space);
          }
          GUILayout.EndHorizontal();

          GUILayout.Space(space);

          GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
          {
            GUILayout.BeginVertical();
            {
              if (GUILayout.Button(" Online <b>documentation</b>.", buttonStyle, GUILayout.ExpandHeight(true)) == true)
                Application.OpenURL(documentationURL);

              if (GUILayout.Button(" Check out <b>our assets</b>.", buttonStyle, GUILayout.ExpandHeight(true)) == true)
                Application.OpenURL(publisherURL);

              if (GUILayout.Button(" <b>Rate</b> 'Tilt Shift', thanks!", buttonStyle, GUILayout.ExpandHeight(true)) == true)
                Application.OpenURL(storeURL);
            }
            GUILayout.EndVertical();

            scroll = GUILayout.BeginScrollView(scroll, scrollStyle, GUILayout.Width(this.minSize.x * 0.5f));
            {
              GUILayout.Label(changelog);
            }
            GUILayout.EndScrollView();
          }
          GUILayout.EndHorizontal();

          GUILayout.Space(space);

          GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
          {
            GUILayout.TextArea("<size=12>Any question or suggestion? Do not hesitate to send us an email to '<b>hello@ibuprogames.com</b>'.</size>", descriptionStyle, GUILayout.ExpandHeight(true));

            GUILayout.BeginHorizontal(GUILayout.Width(this.minSize.x * 0.5f));
            {
              if (GUILayout.Button(forumTexture, iconStyle) == true)
                Application.OpenURL(forumURL);

              if (GUILayout.Button(twitterTexture, iconStyle) == true)
                Application.OpenURL(twitterURL);

              if (GUILayout.Button(facebookTexture, iconStyle) == true)
                Application.OpenURL(facebookURL);

              if (GUILayout.Button(githubTexture, iconStyle) == true)
                Application.OpenURL(githubURL);
            }
            GUILayout.EndHorizontal();
          }
          GUILayout.EndHorizontal();

          GUILayout.Space(space);
        }
        GUILayout.EndVertical();
      }
    }

    /// <summary>
    /// About window post-processor.
    /// </summary>
    public sealed class TiltShiftAboutProcessor : AssetPostprocessor
    {
      static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
      {
        string version = "1.0";

        for (int i = 0; i < importedAssets.Length; ++i)
        {
          if (importedAssets[i].Contains("TiltShift/Changelog.txt") == true && importedAssets[i].EndsWith(".meta") == false)
          {
            string assetPath = TiltShiftAbout.GetAssetPath();
            if (string.IsNullOrEmpty(assetPath) == false)
            {
              TextAsset changeLogAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath + "Changelog.txt");
              if (changeLogAsset != null && string.IsNullOrEmpty(changeLogAsset.text) == false)
              {
                version = changeLogAsset.text.Split(new char[] { '\n', '\r' })[0];

                if (EditorPrefs.GetString(TiltShiftAbout.assetID, "0.0") != version)
                  TiltShiftAbout.Open();

                break;
              }
            }
          }
        }
      }
    }
  }
}
