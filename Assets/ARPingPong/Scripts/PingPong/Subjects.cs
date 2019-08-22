namespace Ricoh.TeamHaptics.AR.PingPong
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public enum Subject
    {
        Ball,
        Stage
    }

    public interface IObserver
    {
        void OnUpdate();        
    }

    /// <summary>
    /// オブジェクトの監視を管理します。
    /// Observer は、このクラスのインスタンスに監視したい Subject と通知先を登録します。
    /// Observable(Subject) は、通知すべき変更があれば、このクラスのインスタンスに通知します。
    /// 変更があったことを通知するだけなので、監視側は自分で対象を取得してください
    /// このクラス用の GameObject を用意してアタッチすることで実質 Singleton とします。
    /// </summary>
    public class Subjects : MonoBehaviour
    {
        private Dictionary<Subject, List<IObserver>> observers = new Dictionary<Subject, List<IObserver>>();
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
         
        }

        public void Add(Subject target, IObserver observer)
        {
            if (observers.ContainsKey(target))
            {
                if (!observers[target].Contains(observer))
                {
                    observers[target].Add(observer);    
                }
            }
            else
            {
                var newList = new List<IObserver>();
                newList.Add(observer);
                observers[target] = newList;
            }
        }

        public void Remove(Subject target, IObserver observer)
        {
            if (observers.ContainsKey(target))
            {
                observers[target].Remove(observer);
            }
        }

        public void NotifyUpdate(Subject subject)
        {
            if (!observers.ContainsKey(subject))
            {
                Debug.Log("Observers Not Found");
            }
            foreach (var observer in observers[subject])
            {
                observer.OnUpdate();
            }
        }
    }
}
