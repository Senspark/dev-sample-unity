using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Examples.ServiceLocator.Editor
{
    public class StartupScene : EditorWindow
    {
        private void OnGUI()
        {
            EditorSceneManager.playModeStartScene = (SceneAsset)EditorGUILayout.ObjectField(new GUIContent("Start Scene"), EditorSceneManager.playModeStartScene, typeof(SceneAsset), false);
        }
    }
}