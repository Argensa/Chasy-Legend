/*! 
 * \file
 * \author Stig Olavsen <stig.olavsen@freakshowstudio.com>
 * \author http://www.freakshowstudio.com
 * \date © 2011-2015
 */
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using FreakLib.SelectParent;

namespace FreakLibEditor.SelectParent
{
	/// <summary>
	/// Editor class for enabling the Select Parent function
	/// </summary>
	[InitializeOnLoad]
	public class SelectParent : Editor
	{
		/// <summary>
		/// Key used to store settings in EditorPrefs.
		/// </summary>
		private const string PREFS_KEY = "AlwaysSelectParent";

		/// <summary>
		/// Property that determines if we should be selecting parent, 
		/// uses editor prefs to keep value when entering/exiting playmode.
		/// </summary>
		/// <value><c>true</c> if always select parent; 
		/// otherwise, <c>false</c>.</value>
		private static bool AlwaysSelectParent
		{
			get 
			{ 
				bool selectParent = EditorPrefs.GetBool(PREFS_KEY, false);
				return selectParent; 
			}
			set 
			{ 
				EditorPrefs.SetBool(PREFS_KEY, value);
			}
		}

		/// <summary>
		/// Static constructor (in combination with InitializeOnLoad) 
		/// lets us set up the callback every time the script is 
		/// recompiled.
		/// </summary>
		static SelectParent()
		{
			EditorApplication.update += SelectParents;
		}

		/// <summary>
		/// Function for selecting the parent transform
		/// </summary>
		[MenuItem("Edit/Select Parent &p", false, 200001)]
		private static void SelectParentMenuitem()
		{
			if (Selection.activeTransform != null && 
				Selection.activeTransform.parent != null)
			{
				Selection.activeTransform = Selection.activeTransform.parent;
			}
		}
		
		/// <summary>
		/// Function for selecting the root transform
		/// </summary>
		[MenuItem("Edit/Select Root &#p", false, 200002)]
		private static void SelectRootMenuItem()
		{
			if (Selection.activeTransform != null &&
				Selection.activeTransform.root != null)
			{
				Selection.activeTransform = Selection.activeTransform.root;
			}
		}	
		
		/// <summary>
		/// Function for turning Always Select Parent On
		/// </summary>
		[MenuItem("Edit/Always Select Parent/On", false, 200003)]
		private static void EnableSelectParent()
		{
			AlwaysSelectParent = true;
		}
	
		/// <summary>
		/// Validation Function for turning Always Select Parent On
		/// </summary>
		[MenuItem("Edit/Always Select Parent/On", true, 200003)]
		private static bool EnableSelectParentValidate()
		{
			return (!AlwaysSelectParent);
		}
		
		/// <summary>
		/// Function for turning Always Select Parent Off
		/// </summary>
		[MenuItem("Edit/Always Select Parent/Off", false, 200004)]
		private static void DisableSelectParent()
		{
			AlwaysSelectParent = false;
		}
	
		/// <summary>
		/// Validation Function for turning Always Select Parent Off
		/// </summary>
		[MenuItem("Edit/Always Select Parent/Off", true, 200004)]
		private static bool DisableSelectParentValidate()
		{
			return AlwaysSelectParent;
		}
		
		/// <summary>
		/// Function to toggle the state of Always Select Parent (On/Off)
		/// </summary>
		[MenuItem("Edit/Always Select Parent/Toggle %&#p", false, 200005)]
		private static void ToggleSelectParent()
		{
			if (!AlwaysSelectParent)
			{
				EnableSelectParent();
			} else
			{
				DisableSelectParent();
			}
		}
		
		/// <summary>
		/// Function that is added to the editors update callback.
		/// This will select the parent of the current transform if
		/// it has the AlwaysSelectMyParent component added.
		/// </summary>
		private static void SelectParents()
		{
			if (!AlwaysSelectParent)
			{
				return;
			}

			Transform t = Selection.activeTransform;
			if (t != null)
			{
				if (t.GetComponent<AlwaysSelectMyParent>() != null)
				{
					if (t.parent != null)
					{
						Selection.activeTransform = t.parent;
					}
				}
			}
		}
	}
}
#endif // UNITY_EDITOR
