using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioSourceIngredient))]
public class AudioSourceInspectorIngredient : Editor
{
	private string[] actions = new string[] { "Play", "Stop", "Cross Fade", "Fade Out" };
	private string[] types = new string[] { "Play Random", "Play Sequence", "Play Parallel" };

	private int listCount = 0;

	private SerializedProperty typeId;
	private SerializedProperty audioType;
	private SerializedProperty actionId;
	private SerializedProperty secondAudioEndVolume;
	private SerializedProperty audioDuration;
	private SerializedProperty audioSourcesProperty;

	void OnEnable()
	{
		typeId = serializedObject.FindProperty("typeId");
		audioType = serializedObject.FindProperty("audioType");
		actionId = serializedObject.FindProperty("actionId");
		secondAudioEndVolume = serializedObject.FindProperty("secondAudioEndVolume");
		audioDuration = serializedObject.FindProperty("audioDuration");
		audioSourcesProperty = serializedObject.FindProperty("audioSources");
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		serializedObject.Update();

		AudioSourceIngredient audioIngredient = (AudioSourceIngredient)target;
		bool allowSceneObjects = !EditorUtility.IsPersistent(target);

		AudioSourceIngredient.EAudioType audioTypeEnum = (AudioSourceIngredient.EAudioType)audioType.enumValueIndex;
		audioTypeEnum = (AudioSourceIngredient.EAudioType)(EditorGUILayout.EnumPopup("Audio Type", audioTypeEnum, EditorStyles.popup));
		audioType.enumValueIndex = (int)audioTypeEnum;

		actionId.intValue = EditorGUILayout.Popup("Action", actionId.intValue, actions, EditorStyles.popup);

		switch (audioIngredient.actionId)
		{
			case 0:
			case 1:
			case 3:
				if (audioIngredient.actionId == 0)
				{
					EditorGUILayout.BeginHorizontal();
					audioIngredient.typeId = EditorGUILayout.Popup("Type", audioIngredient.typeId, types, EditorStyles.popup);
					EditorGUILayout.EndHorizontal();
				}

				if (audioIngredient.actionId == 3)
				{
					EditorGUILayout.BeginHorizontal();
					audioDuration.floatValue = EditorGUILayout.FloatField("Fade Time:", audioDuration.floatValue);
					EditorGUILayout.EndHorizontal();
				}

				EditorGUILayout.BeginHorizontal();
				listCount = EditorGUILayout.IntField("Count:", audioSourcesProperty.arraySize);
				listCount = Mathf.Max(listCount, 1);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.PropertyField(audioSourcesProperty, true);

				EditorGUI.indentLevel += 1;
				for (int i = 0; i < audioSourcesProperty.arraySize; i++)
				{
					EditorGUILayout.PropertyField(audioSourcesProperty.GetArrayElementAtIndex(i));
				}

				while (listCount > audioSourcesProperty.arraySize)
				{
					if (audioSourcesProperty.arraySize > 0)
					{
						audioSourcesProperty.InsertArrayElementAtIndex(audioSourcesProperty.arraySize - 1);
						audioSourcesProperty.GetArrayElementAtIndex(audioSourcesProperty.arraySize - 1).objectReferenceValue = null;
					}
					else
					{
						audioSourcesProperty.InsertArrayElementAtIndex(0);
						audioSourcesProperty.GetArrayElementAtIndex(0).objectReferenceValue = null;
					}
				}

				while (listCount < audioSourcesProperty.arraySize)
				{
					audioSourcesProperty.GetArrayElementAtIndex(audioSourcesProperty.arraySize - 1).objectReferenceValue = null;
					audioSourcesProperty.DeleteArrayElementAtIndex(audioSourcesProperty.arraySize - 1);
				}
				EditorGUI.indentLevel -= 1;

				break;

			case 2:
				EditorGUILayout.PropertyField(audioSourcesProperty, true);

				EditorGUI.indentLevel += 1;
				while (audioSourcesProperty.arraySize < 2)
				{
					audioSourcesProperty.InsertArrayElementAtIndex(audioSourcesProperty.arraySize - 1);
					audioSourcesProperty.GetArrayElementAtIndex(audioSourcesProperty.arraySize - 1).objectReferenceValue = null;
				}

				while (audioSourcesProperty.arraySize > 2)
				{
					audioSourcesProperty.GetArrayElementAtIndex(audioSourcesProperty.arraySize - 1).objectReferenceValue = null;
					audioSourcesProperty.DeleteArrayElementAtIndex(audioSourcesProperty.arraySize - 1);
				}

				for (int i = 0; i < audioSourcesProperty.arraySize; i++)
				{
					EditorGUILayout.PropertyField(audioSourcesProperty.GetArrayElementAtIndex(i));
				}

				EditorGUI.indentLevel -= 1;

				EditorGUILayout.BeginHorizontal();
				secondAudioEndVolume.floatValue = EditorGUILayout.FloatField("Audio 2 End Volume:", secondAudioEndVolume.floatValue);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				audioDuration.floatValue = EditorGUILayout.FloatField("Cross Fade Time:", audioDuration.floatValue);
				EditorGUILayout.EndHorizontal();
				break;
		}

		if (GUI.changed)
		{
			EditorUtility.SetDirty(target);
			serializedObject.ApplyModifiedProperties();
		}
	}
}