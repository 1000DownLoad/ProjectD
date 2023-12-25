using System.Collections.Generic;
using System.Linq;

namespace Framework.FunctionFsm
{
    public class FunctionFsm
    {
        private readonly List<FunctionState> _states = new List<FunctionState>();
        private FunctionState _currentState = null;

        public bool AddState(FunctionState state)
        {
            if (FindState(state.Key) != null)
            {
                //실패
                return false;
            }

            _states.Add(state);

            return true;
        }

        public void RemoteState(int key)
        {
            var state = FindState(key);
            if (state == null)
                return;

            _states.Remove(state);
        }

        public FunctionState FindState(int key)
        {
            return _states.FirstOrDefault(state => state.Key == key);
        }

        public void ClearState()
        {
            _states.Clear();
        }

        public FunctionState CurrentState()
        {
            return _currentState;
        }



        public bool ChangeState(int key)
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
