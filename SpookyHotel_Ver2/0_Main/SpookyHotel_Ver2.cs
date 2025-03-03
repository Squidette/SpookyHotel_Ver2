class SpookyHotel_Ver2
{
    public static bool gameRunning = true;
    public static RenderMode renderMode = RenderMode.CONSOLERENDERER;

    // 렌더 모드
    public enum RenderMode
    {
        CONSOLERENDERER,    // 영문자 + 특수문자로 콘솔에 그림 그리기
        KRTEXTPRINTER       // 텍스트 출력하기 (한글가능)
    }

    static void Main()
    {
        FixedUpdate();

        while (gameRunning)
        {
            InputManager.Instance.Update();
        }
    }

    public static async void FixedUpdate()
    {
        while (gameRunning)
        {
            // 타이머 업데이트
            FCTimer.FixedUpdate();

            // 매니저 업데이트
            SoundManager.Instance.FixedUpdate();
            SceneManager.Instance.FixedUpdate();
            InputManager.Instance.FixedUpdate(); // 키 초기화코드가 있기때문에 씬매니저보다 뒤에서 업데이트되어야 한다

            // 최종 렌더
            switch (renderMode)
            {
                case RenderMode.CONSOLERENDERER:
                    ConsoleRenderer.Instance.Render();
                    break;
                case RenderMode.KRTEXTPRINTER:
                    KrTextPrinter.Instance.Print();
                    break;
            }

            await Task.Delay(16);
        }
    }
}