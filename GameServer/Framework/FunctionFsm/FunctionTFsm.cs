using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.FunctionFsm
{
    public class FunctionTFsm<T> where T : Enum
    {
        private readonly List<FunctionTState<T>> _states = new List<FunctionTState<T>>();
        private FunctionTState<T> _currentState = null;

        public bool AddState(FunctionTState<T> state)
        {
            if (FindState(state.Key) != null)
            {
                //실패
                return false;
            }

            _states.Add(state);

            return true;
        }

        public void RemoteState(T key)
        {
            var state = FindState(key);
            if (state == null)
                return;

            _states.Remove(state);
        }

        public FunctionTState<T> FindState(T key) 
        {
            return _states.FirstOrDefault(state => state.Key.Equals(key));
        }
    
        public bool ChangeState(T key)
        {
            _currentState?.OnExit?.Invoke();
            _currentState = null;

            var state = FindState(key);
            if (state == null)
                return false;

            _currentState = state;
            _currentState.OnEnter?.Invoke();
        
            return true;
        }

        public void Update()
        {
            _currentState?.OnExecute?.Invoke();
        }
    }
}
