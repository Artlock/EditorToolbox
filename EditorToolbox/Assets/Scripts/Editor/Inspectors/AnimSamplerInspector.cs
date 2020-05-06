using ToolboxEngine;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.Animations;

namespace ToolboxEditor
{
    [CustomEditor(typeof(AnimSampler))]
    public class AnimSamplerInspector : Editor
    {
        // All of what's below was made to play animations in scene mode, which is not normally possible

        private Animator animator;
        private AnimationClip[] animClipsArr = null;
        private string[] animNamesArr = null;

        private bool isInitialized = false;

        private bool isPlaying = false;
        private int currentAnimIndex = 0;

        private float editorLastTime;

        private void OnDisable()
        {
            StopAnim();
        }

        public override void OnInspectorGUI()
        {
            // as keyword can only be used for references and sets pointer to null if cast fails instead of running an exception
            AnimSampler sampler = target as AnimSampler;
            if (sampler == null) return;

            if (!isInitialized)
            {
                animator = sampler.GetComponent<Animator>();
                animClipsArr = FindAnimClips(animator);
                animNamesArr = FindAnimNames(animClipsArr);
                isInitialized = true;
            }

            currentAnimIndex = EditorGUILayout.Popup("Current Anim", currentAnimIndex, animNamesArr);

            if (isPlaying)
            {
                if (GUILayout.Button("Stop"))
                {
                    StopAnim();
                    isPlaying = false;
                }
            }
            else
            {
                if (GUILayout.Button("Play"))
                {
                    PlayAnim();
                    isPlaying = true;
                }
            }
        }

        private void PlayAnim()
        {
            if (isPlaying) return;

            editorLastTime = Time.realtimeSinceStartup;
            EditorApplication.update += OnEditorUpdate;
            AnimationMode.StartAnimationMode();

            isPlaying = true;
        }

        private void StopAnim()
        {
            if (!isPlaying) return;

            EditorApplication.update -= OnEditorUpdate;
            AnimationMode.StopAnimationMode();

            isPlaying = false;
        }

        private void OnEditorUpdate()
        {
            AnimationClip animClip = animClipsArr[currentAnimIndex];

            float animTime = Time.realtimeSinceStartup - editorLastTime;

            // Looping anim
            animTime %= animClip.length;

            AnimationMode.SampleAnimationClip(animator.gameObject, animClip, animTime);
        }

        private string[] FindAnimNames(AnimationClip[] animClipsArr)
        {
            List<string> resultList = new List<string>();

            foreach (AnimationClip clip in animClipsArr)
            {
                resultList.Add(clip.name);
            }

            return resultList.ToArray();
        }

        private AnimationClip[] FindAnimClips(Animator animator)
        {
            List<AnimationClip> resultList = new List<AnimationClip>();
            AnimatorController editorController = (AnimatorController)animator.runtimeAnimatorController;
            AnimatorControllerLayer controllerLayer = editorController.layers[0];

            foreach (ChildAnimatorState childState in controllerLayer.stateMachine.states)
            {
                // as keyword can only be used for references and sets pointer to null if cast fails instead of running an exception
                AnimationClip animClip = childState.state.motion as AnimationClip;

                if (animClip != null)
                {
                    resultList.Add(animClip);
                }
            }

            return resultList.ToArray();
        }
    }
}
