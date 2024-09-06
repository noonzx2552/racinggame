using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // ฟังก์ชันสำหรับเปลี่ยนฉาก
    public void ChangeSample()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void ChangeSetting()
    {
        SceneManager.LoadScene("Setting");
    }
}