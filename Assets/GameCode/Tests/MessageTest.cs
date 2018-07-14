using UnityEngine;

public class MessageTest : MonoBehaviour
{
    public const string MessageType = "MessageTest.Test";
    public int listenerCount = 250;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)) Test();
    }

    private void Test()
    {
        var listeners = new TestListener[listenerCount];
        for (int i = 0; i < listenerCount; ++i)
        {
            listeners[i] = new TestListener();
            listeners[i].AddObservers();
        }

        this.PostMessage(MessageType);

        for (int i = 0; i < listeners.Length; ++i) listeners[i].RemoveObservers();
        MessageCenter.Instance.Clean();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 40), "Press Space to Test Listeneres");
    }

    private class TestListener : IObserver
    {
        public const string Clear = "TestListener.Clear";

        public void AddObservers()
        {
            ((object)this).AddObserver(OnTest, MessageType);
            this.AddObserver(OnClear, Clear);
        }

        public void RemoveObservers()
        {
            this.RemoveObserver(OnTest, MessageType);
            this.RemoveObserver(OnClear, Clear);
        }

        private void OnTest(object sender, object e) => this.PostMessage(Clear);
        private void OnClear(object sender, object e) => this.RemoveObserver(OnTest, MessageType);
    }
}
