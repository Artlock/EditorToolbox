﻿using ToolboxEngine;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace ToolboxEditor
{
    [CustomEditor(typeof(ActionsSequencer))]
    public class ActionsSequencerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate"))
            {
                GenerateActions();
            }
        }

        private void GenerateActions()
        {
            ActionsSequencer sequencer = target as ActionsSequencer;
            AnimatorController controller = sequencer.controller as AnimatorController;

            while (sequencer.transform.childCount > 0)
            {
                DestroyImmediate(sequencer.transform.GetChild(0).gameObject);
            }

            AnimatorControllerLayer mainLayer = controller.layers[0];

            AnimatorState state = mainLayer.stateMachine.defaultState;
            GameObject stateParentGameObject = new GameObject(state.name);
            stateParentGameObject.transform.parent = sequencer.transform;

            while (state.transitions.Length > 0)
            {
                AnimatorState subState = state.transitions[0].destinationState;
                GameObject subStateGameObject = new GameObject(subState.name);
                subStateGameObject.transform.parent = stateParentGameObject.transform;
                state = subState;
                stateParentGameObject = subStateGameObject;
            }

            /*
            foreach (ChildAnimatorState childState in mainLayer.stateMachine.states)
            {
                GameObject go = new GameObject(childState.state.name);
                go.transform.parent = sequencer.transform;
            }
            */
        }
    }
}
