namespace Ui
{
    public static class UiManager
    {
        public delegate void UiEvent(bool open);
        public static UiEvent HudOpened;
    }
}