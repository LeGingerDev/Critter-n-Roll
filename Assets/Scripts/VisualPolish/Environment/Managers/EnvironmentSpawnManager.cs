using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace YourNamespace
{
    /// <summary>
    /// Collects all EnvironmentElementIdentifier instances under this GameObject,
    /// provides public methods to move them up or down either instantly (no tween)
    /// or via DOTween coroutines. You can pass in an Action onComplete to be called
    /// when the entire up/down sequence is finished.
    /// </summary>
    public class EnvironmentSpawnManager : SerializedMonoBehaviour
    {
        [SerializeField, FoldoutGroup("Spawn Settings")]
        private float _heightOffset = 30f;

        [SerializeField, FoldoutGroup("Spawn Settings")]
        private float _tweenDuration = 1f;

        [SerializeField, FoldoutGroup("Spawn Settings")]
        private float _tweenTotalTime = 0.1f;

        [SerializeField]
        private readonly List<EnvironmentElementIdentifier> _elements = new List<EnvironmentElementIdentifier>();

        [SerializeField]
        private readonly Dictionary<EnvironmentElementType, List<EnvironmentElementIdentifier>> _groupedElements
            = new Dictionary<EnvironmentElementType, List<EnvironmentElementIdentifier>>();

        private void Start()
        {
            // On Start, automatically move up via coroutine (non?instant).
            CollectElements();
            float defaultDelay = CalculatePerElementDelay();
            StartCoroutine(MoveAllElementsUpCoroutine(defaultDelay));
        }

        /// <summary>
        /// Finds all EnvironmentElementIdentifier components under this GameObject and groups them by type.
        /// </summary>
        private void CollectElements()
        {
            _elements.Clear();
            _elements.AddRange(GetComponentsInChildren<EnvironmentElementIdentifier>());

            _groupedElements.Clear();
            foreach (EnvironmentElementType type in Enum.GetValues(typeof(EnvironmentElementType)))
            {
                _groupedElements[type] = new List<EnvironmentElementIdentifier>();
            }

            foreach (var element in _elements)
            {
                _groupedElements[element.GetElementType()].Add(element);
            }
        }

        /// <summary>
        /// Calculates a default per?element delay based on total time and total count.
        /// </summary>
        public float CalculatePerElementDelay()
        {
            return (_elements.Count > 0) ? (_tweenTotalTime / _elements.Count) : 0f;
        }

        //––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––

        /// <summary>
        /// Public: Move everything up. 
        /// If isInstant==true, sets positions immediately and calls onComplete right away.
        /// Otherwise, starts a DOTween coroutine and invokes onComplete after all tweens finish.
        /// </summary>
        /// <param name="isInstant">If true, skip tween and snap them up.</param>
        /// <param name="onComplete">Optional callback to run once the entire sequence is done.</param>
        public void MoveAllElementsUp(bool isInstant, Action onComplete = null)
        {
            CollectElements();

            if (isInstant)
            {
                // Instant—no tween:
                foreach (var kvp in _groupedElements)
                {
                    foreach (var element in kvp.Value)
                    {
                        element.transform.position = element.GetInitialPosition() + Vector3.up * _heightOffset;
                    }
                }

                // Immediately invoke callback:
                onComplete?.Invoke();
            }
            else
            {
                // Tween via coroutine, passing the onComplete:
                float delay = CalculatePerElementDelay();
                StartCoroutine(MoveAllElementsUpCoroutine(delay, onComplete));
            }
        }

        /// <summary>
        /// (Odin button) wraps the public method with isInstant=false and no callback.
        /// </summary>
        [Button, FoldoutGroup("Spawn Settings")]
        private void MoveAllElementsUp()
        {
            MoveAllElementsUp(isInstant: false, onComplete: null);
        }

        /// <summary>
        /// Coroutine that tweens each element up by _heightOffset, in enum order.
        /// “perElementDelay” is the wait?time between finishing one tween and starting the next.
        /// When all are done, invokes onComplete (if not null).
        /// </summary>
        private IEnumerator MoveAllElementsUpCoroutine(float perElementDelay, Action onComplete = null)
        {
            foreach (EnvironmentElementType type in Enum.GetValues(typeof(EnvironmentElementType)))
            {
                foreach (var element in _groupedElements[type])
                {
                    float targetY = element.GetInitialPosition().y + _heightOffset;
                    element.transform.DOMoveY(targetY, _tweenDuration);
                    yield return null;
                    
                }
            }

            // All “up” tweens are finished—invoke callback:
            onComplete?.Invoke();
        }

        //––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––

        /// <summary>
        /// Public: Move everything down (spawn). 
        /// If isInstant==true, sets positions immediately and calls onComplete right away.
        /// Otherwise, starts a DOTween coroutine and invokes onComplete after all tweens finish.
        /// </summary>
        /// <param name="isInstant">If true, snap them back down immediately.</param>
        /// <param name="onComplete">Optional callback to run once the entire sequence is done.</param>
        public void MoveAllElementsDown(bool isInstant, Action onComplete = null)
        {
            CollectElements();

            if (isInstant)
            {
                // Instant—no tween:
                foreach (var kvp in _groupedElements)
                {
                    foreach (var element in kvp.Value)
                    {
                        float origY = element.GetInitialPosition().y;
                        Vector3 pos = element.transform.position;
                        element.transform.position = new Vector3(pos.x, origY, pos.z);
                    }
                }

                // Immediately invoke callback:
                onComplete?.Invoke();
            }
            else
            {
                // Tween via coroutine, passing the onComplete:
                float delay = CalculatePerElementDelay();
                StartCoroutine(SpawnCoroutine(delay, onComplete));
            }
        }

        /// <summary>
        /// (Odin button) wraps the public method with isInstant=false and no callback.
        /// </summary>
        [Button(ButtonSizes.Large), FoldoutGroup("Spawn Settings")]
        private void PlaySpawnEffect()
        {
            MoveAllElementsDown(isInstant: false, onComplete: null);
        }

        /// <summary>
        /// Coroutine that animates each element back down to its original Y?position, in enum order.
        /// When all are done, invokes onComplete (if not null).
        /// </summary>
        private IEnumerator SpawnCoroutine(float perElementDelay, Action onComplete = null)
        {
            foreach (EnvironmentElementType type in Enum.GetValues(typeof(EnvironmentElementType)))
            {
                foreach (var element in _groupedElements[type])
                {
                    float originalY = element.GetInitialPosition().y;
                    yield return element.transform.DOMoveY(originalY, _tweenDuration);

                    //if (perElementDelay > 0f)
                    //   yield return new WaitForSeconds(perElementDelay);
                }
            }

            // All “down” tweens are finished—invoke callback:
            onComplete?.Invoke();
        }
    }
}
