namespace SmartTechTest.Main.Pool
{
    using System;

    /// <summary>
    /// Обертка для работы с пулом
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGamePool<T> where T : class
    {
        /// <summary>
        /// Запрос обьекта
        /// </summary>
        /// <param name="baseObject">Базовый обьект = ключ</param>
        /// <param name="returnedObjectFromPool">Возвращаемый обьект</param>
        /// <returns>Коллбек на возвращение обьекта</returns>
        public Action<T> RequestObject(T baseObject, out T returnedObjectFromPool);
    }
}
