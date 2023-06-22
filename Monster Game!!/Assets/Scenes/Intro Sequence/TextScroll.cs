using System.Collections;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Joeri.Tools;
using Joeri.Tools.Structure;
using UnityEngine.InputSystem;

[System.Serializable]
public class TextScroll
{
    [SerializeField] private float m_scrollSpeed = 60f;
    [SerializeField] private float m_waitTime = 0f;
    [Space]
    [SerializeField] private TextMeshProUGUI m_text = null;

    //  Run-time properties;
    private bool m_busy = false;
    private float m_interval = 0f;
    private Stack<string> m_dialogueSentences = null;

    //  Events:
    private System.Action<string> m_onAdvance = null;
    private System.Action m_onFinish = null;

    //  State machine:
    private FSM m_fsm = null;

    public void Activate(string[] dialogues, System.Action onFinish, System.Action<string> onAdvance = null)
    {
        m_busy = true;
        m_interval = 1f / m_scrollSpeed;
        m_dialogueSentences = new Stack<string>(); for (int i = dialogues.Length - 1; i >= 0; i--) m_dialogueSentences.Push(dialogues[i]);

        m_onFinish = onFinish;
        m_onAdvance = onAdvance;

        m_fsm = new FSM(typeof(Idle), new Idle(), new Scrolling(this), new Waiting(this));

        Advance();
    }

    public void Tick(float deltaTime)
    {
        if (!m_busy) return;
        m_fsm.Tick(deltaTime);
    }

    private void Advance()
    {
        if (m_dialogueSentences.Count == 0)
        {
            m_onFinish.Invoke();
            Deactivate();
            return;
        }

        var dialogue = m_dialogueSentences.Pop();

        m_fsm.SwitchToState<Scrolling>().Setup(dialogue);
        m_onAdvance?.Invoke(dialogue);
    }

    public void Deactivate()
    {
        m_text.text = "";

        m_busy = false;
        m_dialogueSentences = null;
        m_interval = 0f;

        m_onFinish = null;
        m_onAdvance = null;

        m_fsm = null;
    }

    public class Idle : State { }

    public class Scrolling : FlexState<TextScroll>
    {
        private string m_string = "";

        private StringBuilder m_builder = null;
        private Timer m_timer = null;

        private int m_numberIndex = 0;

        public Scrolling(TextScroll root) : base(root) { }

        public void Setup(string dialogue)
        {
            m_string = dialogue;

            m_builder = new StringBuilder();
            m_timer = new Timer(root.m_interval);

            root.m_text.text = "";
        }

        public override void OnTick(float deltaTime)
        {
            //  Go to input state if all is done.
            if (m_numberIndex >= m_string.Length)
            {
                SwitchToState(typeof(Waiting));
                return;
            }

            //  Skip to end of sequence if button is pressed.
            if (Gamepad.current.buttonSouth.wasReleasedThisFrame)
            {
                root.m_text.text = m_string;
                SwitchToState(typeof(Waiting));
                return;
            }

            //  If the timer is not done yet, come back later.
            if (!m_timer.HasReached(deltaTime)) return;

            //  Append string.
            m_builder.Append(m_string[m_numberIndex]);
            root.m_text.text = m_builder.ToString();

            //  Add timer value to the passed time value, and reset the timer.
            m_timer.timer = 0f;

            //  Iterate on the number index.
            m_numberIndex++;
        }


        public override void OnExit()
        {
            m_string = "";

            m_builder = null;
            m_timer = null;

            m_numberIndex = 0;
        }
    }

    public class Waiting : FlexState<TextScroll>
    {
        private Timer m_timer = null;

        public Waiting(TextScroll root) : base(root) { }

        public override void OnEnter()
        {
            m_timer = new Timer(root.m_waitTime);
        }

        public override void OnTick(float deltaTime)
        {
            if (m_timer.HasReached(deltaTime)) root.Advance();

            //  Skip mechanic
            if (!Gamepad.current.buttonSouth.wasReleasedThisFrame) return;
            root.Advance();
        }

        public override void OnExit()
        {
            m_timer = null;
        }
    }
}
