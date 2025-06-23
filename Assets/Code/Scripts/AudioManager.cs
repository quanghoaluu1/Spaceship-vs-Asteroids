using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    
    [Header("UI Components")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Image muteButtonImage;

    [Header("Sprites")]
    [SerializeField] private Sprite unmutedSprite;
    [SerializeField] private Sprite mutedSprite;

    [Header("Audio")]
    [SerializeField] private AudioMixer mainMixer;
    
    // Tên của tham số Volume đã expose trong Audio Mixer
    private const string MIXER_VOLUME_PARAM = "MasterVolume"; 
    private float lastVolumeBeforeMute;

    void Start()
    {
        // Khởi tạo slider với giá trị volume hiện tại của mixer
        // Chuyển đổi từ decibel (logarithmic) sang giá trị linear (0-1) cho slider
        if (mainMixer.GetFloat(MIXER_VOLUME_PARAM, out float currentDb))
        {
            volumeSlider.value = DecibelToLinear(currentDb);
        }
        else
        {
            volumeSlider.value = 1f; // Mặc định nếu không lấy được
        }

        // Gán hàm cho sự kiện của slider và nút bấm
        volumeSlider.onValueChanged.AddListener(SetVolume);
        muteButtonImage.GetComponent<Button>().onClick.AddListener(ToggleMute);

        // Cập nhật hình ảnh ban đầu
        UpdateMuteIcon();
    }

    public void SetVolume(float linearValue)
    {
        // Chuyển giá trị linear (0-1) của slider thành decibel cho mixer
        // Giá trị 0 của slider tương ứng với -80dB (gần như tắt tiếng)
        float dbValue = LinearToDecibel(linearValue);
        mainMixer.SetFloat(MIXER_VOLUME_PARAM, dbValue);

        UpdateMuteIcon();
    }

    public void ToggleMute()
    {
        // Lấy giá trị decibel hiện tại
        mainMixer.GetFloat(MIXER_VOLUME_PARAM, out float currentDb);
        float currentLinear = DecibelToLinear(currentDb);

        // Nếu đang không mute (hoặc volume rất nhỏ)
        if (currentLinear > 0.001f)
        {
            // Lưu lại volume hiện tại và mute
            lastVolumeBeforeMute = currentLinear;
            volumeSlider.value = 0;
            SetVolume(0);
        }
        else // Nếu đang mute
        {
            // Nếu trước đó chưa có lưu volume nào, đặt về 100%
            if (lastVolumeBeforeMute <= 0.001f)
            {
                lastVolumeBeforeMute = 1f;
            }
            // Khôi phục lại volume
            volumeSlider.value = lastVolumeBeforeMute;
            SetVolume(lastVolumeBeforeMute);
        }
    }

    private void UpdateMuteIcon()
    {
        if (volumeSlider.value <= 0.001f)
        {
            muteButtonImage.sprite = mutedSprite;
        }
        else
        {
            muteButtonImage.sprite = unmutedSprite;
        }
    }

    // Công thức chuyển đổi từ Linear (0 -> 1) sang Decibel (-80dB -> 0dB)
    private float LinearToDecibel(float linear)
    {
        // Mathf.Log10(0) là không xác định, nên ta chặn giá trị thấp nhất
        if (linear <= 0.0001f)
        {
            return -80.0f;
        }
        return Mathf.Log10(linear) * 20.0f;
    }

    // Công thức chuyển đổi từ Decibel (-80dB -> 0dB) sang Linear (0 -> 1)
    private float DecibelToLinear(float db)
    {
        return Mathf.Pow(10.0f, db / 20.0f);
    }
}
