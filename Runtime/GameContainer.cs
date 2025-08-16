using System.Collections.Concurrent;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Yang.Data
{
    public class InjectAttribute : Attribute
    {
        public InjectAttribute() { }
    }

    public interface INeedInjection
    {
        void OnInjectionCompleted();

        void OnInjectionCleared();
    }

    public interface IData
    {
        int CountDownTime { get; }
        int GameNumber { get; }

        event Action<int> CountDownTimeChanged;
        event Action<int> GameNumberChanged;
    }

    public interface IBaccaratData
    {
        public enum Color{ Red, Blue, Green,}

        Color PlayerColor { get; }
        Color BankerColor { get; }

        event Action<Color, Color> ColorsChanged;
    }

    public class TimerUI : MonoBehaviour, INeedInjection
    {
        [SerializeField] private Text countDownText;

        [Inject] private IData tableData;

        private void Awake()
        {
            countDownText.text = "-";
        }

        public void OnInjectionCompleted()
        {
            tableData.CountDownTimeChanged += OnCountDownTimeChanged;

            countDownText.text = tableData.CountDownTime.ToString();
        }

        public void OnInjectionCleared()
        {
            countDownText.text = "-";
        }

        private void OnCountDownTimeChanged(int countDown)
        {
            countDownText.text = countDown.ToString();
        }
    }

    public class GameNumberTextUI : MonoBehaviour, INeedInjection
    {
        [SerializeField] private Text gameNumberText;
        [Inject] private IData tableData;

        private void Awake()
        {
            gameNumberText.text = "-";
        }

        public void OnInjectionCompleted()
        {
            tableData.GameNumberChanged += OnGameNumberChanged;
            gameNumberText.text = tableData.GameNumber.ToString();
        }

        public void OnInjectionCleared()
        {
            gameNumberText.text = "-";
        }

        private void OnGameNumberChanged(int gameNumber)
        {
            gameNumberText.text = gameNumber.ToString();
        }
    }

    public class BaccaratFelt : MonoBehaviour, INeedInjection
    {
        [SerializeField] private Image playerColorImage;
        [SerializeField] private Image bankerColorImage;

        [Inject] private IData tableData;
        [Inject] private IBaccaratData baccaratData;

        private void Awake()
        {
            UpdateColors(Color.white, Color.white);
        }

        public void OnInjectionCompleted()
        {
            baccaratData.ColorsChanged += OnColorsChanged;

            UpdateColors(GetUnityColor(baccaratData.PlayerColor), GetUnityColor(baccaratData.BankerColor));
        }

        public void OnInjectionCleared()
        {
            UpdateColors(Color.white, Color.white);
        }

        private void OnColorsChanged(IBaccaratData.Color playerColor, IBaccaratData.Color bankerColor)
        {
            UpdateColors(GetUnityColor(playerColor), GetUnityColor(bankerColor));
        }

        private void UpdateColors(Color playerColor, Color bankerColor)
        {
            playerColorImage.color = playerColor;
            bankerColorImage.color = bankerColor;
        }

        private Color GetUnityColor(IBaccaratData.Color color)
        {
            return color switch
            {
                IBaccaratData.Color.Red => Color.red,
                IBaccaratData.Color.Blue => Color.blue,
                IBaccaratData.Color.Green => Color.green,
                _ => Color.white
            };
        }
    }
}
