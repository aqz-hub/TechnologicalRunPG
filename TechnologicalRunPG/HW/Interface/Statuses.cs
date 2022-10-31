using System;

namespace TechnologicalRunPG.HW
{
    class Statuses
    {
        /// <summary>
        /// Ожидание
        /// </summary>
        public string waiting = "Ожидание";
        /// <summary>
        /// Подключение
        /// </summary>
        public string loading = "Подключение";
        /// <summary>
        /// Идёт прогон
        /// </summary>
        public string runing = "Идёт прогон";
        /// <summary>
        /// Завершен
        /// </summary>
        public string complete = "Завершён";
        /// <summary>
        /// Не завершён
        /// </summary>
        public string notComplete = "Не завершён";
        /// <summary>
        /// Обрыв связи
        /// </summary>
        public string disconnected = "Обрыв связи";
        /// <summary>
        /// Системная ошибка
        /// </summary>
        public string warning = "Сис. ошибка";
        /// <summary>
        /// Вне диапазона
        /// </summary>
        public string outrange = "Вне диапазона";
        /// <summary>
        /// Неверная версия
        /// </summary>
        public string wrongversion = "Неверная вер.";
    }
}
