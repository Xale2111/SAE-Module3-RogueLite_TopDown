using UnityEngine;
using UnityEngine.UIElements;

public class UITimer : MonoBehaviour
{
    [SerializeField] UIDocument UIDocument;
    VisualElement timer;

    float totalTime = 0;

    int milliseconds = 0;
    int seconds = 0;
    int minutes = 0;

    public string timerText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = UIDocument.rootVisualElement.Query("Timer").First();
        timer.dataSource = this;
    }

    // Update is called once per frame
    void Update()
    {
        totalTime += Time.deltaTime;

        minutes = (int)(totalTime / 60);
        seconds = (int)(totalTime % 60);
        milliseconds = (int)((totalTime * 100) % 100);

        timerText = minutes + " : " + seconds + " : " + milliseconds;
    }
}
