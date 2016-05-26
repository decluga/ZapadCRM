using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace zapad.Public.WebInterface.Models.Authorization
{
    /// <summary>
    /// Базовый класс для обработки транзаций и механизма сессий
    /// </summary>
    /// <typeparam name="TKey">Тип ключа транзакции</typeparam>
    public abstract class TransactionListBase<TKey> : IDisposable  //where TKey : struct
    {
        /// <summary>
        /// Инициализация класса
        /// </summary>
        /// <param name="TimerMilliseconds">Время в милисекундах между срабатываниями таймера очистки устаревших транзакций</param>
        protected TransactionListBase(long TimerMilliseconds)
        {
            ClearTimer = new Timer(ClearTimer_Elapsed, null, TimerMilliseconds, TimerMilliseconds);
        }

        /// <summary>
        /// Внутреннее хранилище
        /// </summary>
        private Dictionary<TKey, TransactionBase> m_List = new Dictionary<TKey, TransactionBase>();

        /// <summary>
        /// Объект для получения блокировки через lock
        /// </summary>
        protected object m_lock = new object();

        /// <summary>
        /// Провека наличия ключа транзакции в хранилище
        /// </summary>
        /// <param name="key">Ключ поиска</param>
        /// <returns>true - транзация существует, false - транзакция отсутствует</returns>
        protected bool Contains(TKey key)
        {
            return this.m_List.Keys.Contains(key);
        }

        /// <summary>
        /// Получение экземпляра транзакта по ключу (если ключ не найден - возвращается null
        /// </summary>
        /// <typeparam name="TTransaction">Тип транзакции</typeparam>
        /// <param name="key">Ключ поиска</param>
        /// <returns>Ссылка на экземпляр элемента или null, если такового не найдено</returns>
        protected TTransaction Find<TTransaction>(TKey key) where TTransaction : TransactionBase
        {
            try
            {
                TTransaction finded;
                lock (m_lock)
                {
                    finded = (TTransaction)this.m_List[key];
                    finded.LastAccess = DateTime.Now;
                }
                return finded;
            }
            catch (KeyNotFoundException) { return null; }
        }

        /// <summary>
        /// Удаляет экземпляр транзакта по ключу. 
        /// </summary>
        /// <param name="key">Ключ удаляемого транзакта</param>
        /// <returns>true, если элемент найдеи и удален, false - в других случаях (в т.ч. если удалить не удалось или элемент не найден)</returns>
        protected bool Remove(TKey key)
        {
            bool res = false;
            try
            {
                lock (m_lock)
                {
                    res = this.m_List.Remove(key);
                }
                return res;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Поиск экземпляра транзакта по предикату
        /// </summary>
        /// <typeparam name="TTransaction">Тип транзакции</typeparam>
        /// <param name="fn">Условие поиска</param>
        /// <returns>Ссылка на экземпляр элемента или null, если такового не найдено</returns>
        protected TTransaction Find<TTransaction>(Func<TTransaction, bool> fn) where TTransaction : TransactionBase
        {
            // FirstOrDefault возвращает KeyValuePair (struct), соответственно Value всегда будет существовать
            TTransaction finded = (TTransaction)this.m_List.FirstOrDefault(x => fn((TTransaction)x.Value)).Value;
            finded.LastAccess = DateTime.Now;
            return finded;
        }

        /// <summary>
        /// Добавление транзакции в хранилище
        /// </summary>
        /// <typeparam name="TTransaction">Тип транзакции</typeparam>
        /// <param name="nkey">Ключ</param>
        /// <param name="trans">Транзакция</param>
        /// <returns>Вновь созданный экземпляр</returns>
        public virtual TTransaction Add<TTransaction>(TKey nkey, TTransaction trans) where TTransaction : TransactionBase
        {
            lock (this.m_lock)
            {                        
                trans.Key = nkey;
                this.m_List.Add(nkey, trans);
                return trans;
            }
        }

        /// <summary>
        /// Таймер для очистки активных транзакций
        /// </summary>
        Timer ClearTimer;

        /// <summary>
        /// Событие таймера очистки транзакций
        /// </summary>
        protected virtual void ClearTimer_Elapsed(object sender)
        {
            lock (this.m_lock)
            {
                TKey[] KeysOfObsoleteTrans = this.KeysOfObsoleteTrans.ToArray();
                foreach (TKey key in KeysOfObsoleteTrans)
                    m_List.Remove(key);
            }
        }
        
        /// <summary>
        /// Перечисление всех кодов транзакций, являющихся устаревшими
        /// </summary>
        public IEnumerable<TKey> KeysOfObsoleteTrans
        {
            get
            {
                return this.m_List.Where(x => (DateTime.Now - x.Value.LastAccess) > x.Value.LifeTime).Select(x => x.Key);
            }
        }
        
        /// <summary>
        /// Общее кол-во транзакций
        /// </summary>
        public int KeysTotal { get { return this.m_List.Count; } }

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_List = null;
                ClearTimer.Dispose();
            }

        }
        #endregion

        /// <summary>
        /// Базовый класс транзакций
        /// </summary>
        public abstract class TransactionBase
        {
            /// <summary>
            /// Инициализация
            /// </summary>
            /// <param name="vLifeTime">Время жизни транзакции, задается настройками пользователя</param>
            /// <param name="vParent">Контейнер транзакции</param>
            public TransactionBase(TimeSpan vLifeTime, TransactionListBase<TKey> vParent)
            {
                this.LastAccess = this.Created = DateTime.Now;
                this.LifeTime = vLifeTime;
                this.Parent = vParent;
            }

            /// <summary>
            /// Ссылка на контейнер
            /// </summary>
            public virtual TransactionListBase<TKey> Parent { get; private set; }
            
            /// <summary>
            /// Дата и время создания транзакции
            /// </summary>
            public DateTime Created { get; protected set; }
            
            /// <summary>
            /// Дата и время последнего доступа к транзакции
            /// </summary>
            public DateTime LastAccess { get; internal set; }
            
            /// <summary>
            /// Время жизни транзакции, задается при инициализации настройками пользователя
            /// </summary>
            public TimeSpan LifeTime { get; protected set; }
            
            /// <summary>
            /// Ключ транзации
            /// </summary>
            public TKey Key { get; internal set; }
            
            /// <summary>
            /// Удаление транзакцией самой себя из родительского хранилища
            /// </summary>
            public virtual void Remove()
            {
                lock (this.Parent.m_lock)
                {
                    this.Parent.m_List.Remove(this.Key);
                }
            }
        }
        
    }
}