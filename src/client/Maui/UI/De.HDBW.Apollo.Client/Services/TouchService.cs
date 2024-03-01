// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;

namespace De.HDBW.Apollo.Client.Services
{
    public class TouchService : ITouchService
    {
        private readonly List<WeakReference> _registeredListeners = new List<WeakReference>();

        public void RegisterView(ITouchAwareView view)
        {
            CleanupListeners();
            AddListener(view);
        }

        public void UnregisterView(ITouchAwareView view)
        {
            RemoveListener(view);
            CleanupListeners();
        }

        public void TouchDownReceived(float x, float y)
        {
            CleanupListeners();
            foreach (var view in _registeredListeners)
            {
                var listeningView = view.Target as ITouchAwareView;
                listeningView?.OnTouchDown(x, y);
            }
        }

        public void TouchUpReceived(float x, float y)
        {
            CleanupListeners();
            foreach (var view in _registeredListeners)
            {
                var listeningView = view.Target as ITouchAwareView;
                listeningView?.OnTouchUp(x, y);
            }
        }

        private void AddListener(IView view)
        {
            if (_registeredListeners.Any(x => x.Target == view))
            {
                return;
            }

            _registeredListeners.Add(new WeakReference(view));
        }

        private void RemoveListener(IView view)
        {
            var registration = _registeredListeners.FirstOrDefault(x => x.Target == view);
            if (registration == null)
            {
                return;
            }

            _registeredListeners.Remove(registration);
        }

        private void CleanupListeners()
        {
            _registeredListeners.Where(x => !x.IsAlive).ToList().ForEach(x => _registeredListeners.Remove(x));
        }
    }
}
