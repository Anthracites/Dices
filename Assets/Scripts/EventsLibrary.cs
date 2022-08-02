namespace Dices
{
    public static class EventsLibrary // Class for keep events
    {
        //Game
        public static string Spawn = "Spawn"; // Вызов спавна кубиков
        public static string StopRotate = "StopRotate"; // Вызов остановки вращения кубиков
        public static string ScoreChanged = "ScoreChanged"; // Оповещение об изменении счета
        public static string ScoreHistoryWritten = "ScoreHistoryWritten"; //Оповещени о внесении результата в историю бросков

        //Settings
        public static string DisecSpawned = "DisecSpawned"; // Оповещение о том, что кубики заспавнены и находятся на сцене
        public static string SoundON = "SoundON";
        public static string SoundOff = "SoundOff";
        public static string CubeFalled = "CubeFalled";
        public static string SettingsChanged = "SettingsChanged";
        public static string OrientationChanged = "OrientationChanged";

        //Монетизация
        public static string OnPurchaseConsumable = "OnPurchaseConsumable";//Отправка оплаты
        public static string OnSuccessConsumable = "OnSuccessConsumable";//Успешная оплата
        public static string DonationShowStoped = "DonationShowStoped";//Успешная оплата

    }
}
