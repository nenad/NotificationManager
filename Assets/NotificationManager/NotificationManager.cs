using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace NM
{
    public class NotificationManager : MonoBehaviour
    {

        public struct Message
        {
            public string Content;
            public float Duration;
            public Symbol Symbol;
            public Animation Animation;
            public UnityEngine.Events.UnityAction Action;
        }


        [Header("UI Elements")]
        public Text textUI;
        public Image symbolUI;
        public Button closeButtonUI;
        public Button actionButtonUI;

        [SerializeField]
        [HideInInspector]
        public List<Sprite> sprites;

        private static Queue messageQueue;
        private static bool isShowingMessage = false;
        private float _currentTime = 0;
        private float _reverseTime = 0;

        private static float msgDuration = 0;
        private static Animator animator;
        private static NotificationManager instance;
        private static Animation lastAnimation;
        private static float lastAnimationLength;
        private static UnityEngine.Events.UnityAction lastAction;
        private bool isReversing;

        void Awake()
        {
            if (instance == null)
            {
                animator = GetComponent<Animator>();
                messageQueue = new Queue();
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            if (GameObject.FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                eventSystem.AddComponent<UnityEngine.EventSystems.TouchInputModule>();
                Debug.LogWarning("Instantiated default EventSystem because there wasn't one.");
            }
        }

        public static void ShowMessage(string message, Symbol symbol, float duration, Animation animation = Animation.SlideFromTop, UnityEngine.Events.UnityAction action = null)
        {
            if (instance == null)
            {
                instance.Awake();
            }

            if (isShowingMessage)
            {
                messageQueue.Enqueue(new Message()
                {
                    Symbol = symbol,
                    Content = message,
                    Duration = duration,
                    Animation = animation,
                    Action = action
                });
                return;
            }
            else
            {
                //Set flag
                isShowingMessage = true;

                //Set UI Stuff
                instance.textUI.text = message;
                instance.symbolUI.sprite = instance.GetSprite(symbol);

                if (action != null)
                {
                    instance.actionButtonUI.onClick.AddListener(action);
                    instance.actionButtonUI.onClick.AddListener(instance.CloseNotification);
                }

                //Animation settings
                var animHash = Animator.StringToHash(animation.ToString());
                if (animator.HasState(0, animHash))
                {
                    animator.Play(animation.ToString());
                    lastAction = action;
                    var animLength = GetCurrentAnimationClip(animation).length;

                    msgDuration = duration + animLength;
                    lastAnimation = animation;
                    lastAnimationLength = animLength;
                }
                else
                {
                    Debug.LogError(string.Format("Animation state {0} not found! Make sure you have \"{0}\" animation state and \"{0}_Reversed\" with animation clip with the same name.", animation.ToString()));
                }
            }
        }

        public void CloseNotification()
        {
            HideAnimation();
        }

        void Update()
        {
            //Pop the next one when the last finishes.
            if (messageQueue.Count > 0 && !isShowingMessage)
            {
                var msg = (Message)messageQueue.Dequeue();
                ShowMessage(msg.Content, msg.Symbol, msg.Duration, msg.Animation, msg.Action);
            }

            //Check if we have a message to show, and see the duration.
            if (isShowingMessage && !isReversing)
            {
                _currentTime += Time.unscaledDeltaTime;
                if (_currentTime >= msgDuration)
                {
                    HideAnimation();
                    _currentTime = 0;
                }
            }

            if (isReversing)
            {
                _reverseTime += Time.unscaledDeltaTime;
                if (_reverseTime >= lastAnimationLength)
                {
                    isShowingMessage = false;
                    isReversing = false;
                    _reverseTime = 0;
                }
            }
        }

        private void HideAnimation()
        {
            if (lastAction != null)
            {
                actionButtonUI.onClick.RemoveAllListeners();
                lastAction = null;
            }
            animator.Play(lastAnimation.ToString() + "_Reverse");
            isReversing = true;
        }

        private Sprite GetSprite(Symbol symbol)
        {
            var allSymbols = System.Enum.GetValues(typeof(Symbol));

            for (int i = 0; i < allSymbols.Length; i++)
            {
                if (allSymbols.GetValue(i).ToString() == symbol.ToString())
                {
                    return sprites[i];
                }
            }
            return null;
        }

        private static AnimationClip GetCurrentAnimationClip(Animation animation)
        {
            RuntimeAnimatorController controller = instance.GetComponent<Animator>().runtimeAnimatorController;
            foreach (var anim in controller.animationClips)
            {
                if (anim.name == animation.ToString())
                {
                    return anim;
                }
            }
            Debug.LogError(string.Format("Animation clip {0} not found! Make sure you have \"{0}\" animation state and \"{0}_Reversed\" with animation clip with the same name.", animation.ToString()));
            return null;
        }

    }
}