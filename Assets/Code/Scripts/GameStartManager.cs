using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    void Start()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.ResetTime();
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore(); // nếu bạn có viết thêm hàm này
        }

        InfiniteModeManager infinite = FindObjectOfType<InfiniteModeManager>();
        if (infinite != null)
        {
            infinite.ResetLogic(); // nếu bạn có viết thêm hàm này
        }
    }

}
